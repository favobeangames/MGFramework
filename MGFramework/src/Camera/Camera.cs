using System;
using FavobeanGames.MGFramework.Components;
using FavobeanGames.MGFramework.Math;
using FavobeanGames.MGFramework.Screen;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tweening;

namespace FavobeanGames.MGFramework.CameraSystem
{
    /// <summary>
    /// Camera object
    /// </summary>
    public class Camera
    {
        public readonly static float MinZ = 1f;
        public readonly static float MaxZ = 2048f;

        public readonly static int MinZoom = 1;
        public readonly static int MaxZoom = 20;

        /// <summary>
        /// Camera options contain the set properties of the camera type desired
        /// </summary>
        public CameraOptions CameraOptions { get; }

        /// <summary>
        /// Reference to Entity for the camera to follow when set
        /// </summary>
        public Entity EntityToFollow { get; private set; }

        /// <summary>
        /// Current viewed game screen
        /// </summary>
        public GameScreen CurrentGameScreen { get; private set; }
        /// <summary>
        /// Flag to determine if we should clamp the camera extents to
        /// the GameScreen bounds.
        /// </summary>
        private bool clampCameraToGameScreenBounds { get; set; }


        /// <summary>
        /// Current screen that the camera is viewing
        /// </summary>
        public GameWindow GameWindow { get; private set; }

        /// <summary>
        /// How far the camera is from the game screen
        /// </summary>
        private float z;
        /// <summary>
        /// How far back the camera needs to be to see the entire scene
        /// </summary>
        private float baseZ;
        private int zoom;

        /// <summary>
        /// Aspect Ratio of the view
        /// </summary>
        private float aspectRatio;
        /// <summary>
        /// Angle of the camera to the screen
        /// </summary>
        private float fieldOfView;

        private Matrix view;
        private Matrix proj;

        /// <summary>
        /// Bounds of the view
        /// </summary>
        public RectangleF ViewBounds { get; private set; }
        public Vector2 Position { get; set; }
        public float Z => z;
        public Matrix View => view;
        public Matrix Projection => proj;

        public Camera(GameWindow gameWindow, CameraOptions options)
        {
            if (gameWindow is null)
            {
                throw new ArgumentNullException("gameWindow");
            }

            if (options is null)
            {
                CameraOptions = CameraOptions.PerspectiveCameraOptions;
            }
            else
            {
                CameraOptions = options;
            }

            GameWindow = gameWindow;

            aspectRatio = (float) gameWindow.Width / gameWindow.Height;
            fieldOfView = MathHelper.PiOver2;

            Position = new Vector2(0, 0);
            baseZ = GetZFromHeight(gameWindow.Height);
            z = baseZ;
            zoom = 1;

            UpdateMatrices();
        }

        public void Update(GameTime gameTime)
        {
            if (EntityToFollow is not null)
            {
                if (EntityToFollow.Position != Position)
                {
                    MoveTo(EntityToFollow.Position);
                }
            }
        }

        public void UpdateMatrices()
        {
            if (clampCameraToGameScreenBounds)
            {
                EnsureCameraIsWithinScreenBounds();
            }

            switch (CameraOptions.ProjectionType)
            {
                case CameraProjectionType.Perspective:
                    view = Matrix.CreateLookAt(new Vector3(Position, z), new Vector3(Position, 0), Vector3.Up);
                    proj = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, MinZ, MaxZ);
                    break;
                case CameraProjectionType.Orthographic:
                    view = Matrix.CreateLookAt(new Vector3(Position, z), new Vector3(Position, 0), Vector3.Up) * Matrix.CreateScale(zoom);
                    proj = Matrix.CreateOrthographic(GameWindow.Width, GameWindow.Height, MinZ, MaxZ);
                    break;
                default:
                    view = Matrix.Identity;
                    proj = Matrix.Identity;
                    break;
            }

            GetExtents(out RectangleF newBounds);
            ViewBounds = newBounds;
        }

        public float GetZFromHeight(float height)
        {
            return (0.5f * height) / MathF.Tan(0.5f * fieldOfView);
        }
        public float GetHeightFromZ()
        {
            return z * MathF.Tan(0.5f * fieldOfView) * 2f;
        }
        public void MoveZ(float amount)
        {
            z += amount;
            z = System.Math.Clamp(z, MinZ, MaxZ);
        }
        public void ResetZ()
        {
            z = baseZ;
        }

        public void Move(Vector2 amount)
        {
            MoveTo(Position + amount);
        }
        public void MoveTo(Vector2 newPostion)
        {
            Position = newPostion;
        }

        public void IncrementZoom()
        {
            zoom++;
            UpdatedZoom();
        }
        public void DecrementZoom()
        {
            zoom--;
            UpdatedZoom();
        }
        public void SetZoom(int amount)
        {
            zoom = amount;
            UpdatedZoom();
        }
        private void UpdatedZoom()
        {
            zoom = System.Math.Clamp(zoom, MinZoom, MaxZoom);
            z = baseZ / zoom;
        }

        /// <summary>
        /// Returns the dimensions of the viewport in pixels
        /// </summary>
        /// <param name="width">Width of the viewport in pixels</param>
        /// <param name="height">Height of the viewport in pixels</param>
        public void GetExtents(out float width, out float height)
        {
            height = GetHeightFromZ();
            width = height * aspectRatio;
        }

        /// <summary>
        /// Returns the viewport border positions in pixels
        /// </summary>
        /// <param name="left">Left bound of the viewport in pixels</param>
        /// <param name="right">Right bound of the viewport in pixels</param>
        /// <param name="bottom">Bottom bound of the viewport in pixels</param>
        /// <param name="top">Top bound of the viewport in pixels</param>
        public void GetExtents(out float left, out float right, out float bottom, out float top)
        {
            GetExtents(out float width, out float height);
            left = Position.X - width * 0.5f;
            right = left + width;
            bottom = Position.Y - height * 0.5f;
            top = bottom + height;
        }

        /// <summary>
        /// Returns the viewport border positions in pixels
        /// </summary>
        /// <param name="min">Vector2 of the bottom left corner of the viewport in pixels</param>
        /// <param name="max">Vector2 of the top right corner of the viewport in pixels</param>
        public void GetExtents(out Vector2 min, out Vector2 max)
        {
            GetExtents(out float left, out float right, out float bottom, out float top);
            min = new Vector2(left, bottom);
            max = new Vector2(right, top);
        }

        /// <summary>
        /// Returns the dimensions of the viewport in pixels
        /// </summary>
        /// <param name="tl">Top left position of the camera extents</param>
        /// <param name="tr">Top right position of the camera extents</param>
        /// <param name="bl">Bottom left position of the camera extents</param>
        /// <param name="br">Bottom right position of the camera extents</param>
        public void GetExtents(out Vector2 tl, out Vector2 tr, out Vector2 bl, out Vector2 br)
        {
            GetExtents(out float left, out float right, out float bottom, out float top);
            tl = new Vector2(left, top);
            tr = new Vector2(right, top);
            bl = new Vector2(left, bottom);
            br = new Vector2(right, bottom);
        }

        /// <summary>
        /// Returns the dimensions of the viewport in pixels
        /// </summary>
        /// <param name="viewBounds">Rectangle bounds of the extents</param>
        public void GetExtents(out RectangleF viewBounds)
        {
            GetExtents(out float left, out float right, out float bottom, out float top);
            viewBounds = new RectangleF(left, bottom, right - left, top - bottom);
        }

        /// <summary>
        /// Sets the camera to follow an entity
        /// </summary>
        /// <param name="entity">Entity for camera to follow</param>
        public void SetEntityToFollow(Entity entity)
        {
            EntityToFollow = entity;
        }

        public void ResetEntityToFollow()
        {
            EntityToFollow = null;
        }

        /// <summary>
        /// Gives the camera a reference to the current game screen rendered
        /// </summary>
        /// <param name="gameScreen">Current game screen being rendered</param>
        /// <param name="clampToBounds">Flag to make the camera stay within the bounds of the game screen</param>
        public void SetCurrentGameScreen(GameScreen gameScreen, bool clampToBounds)
        {
            CurrentGameScreen = gameScreen;
            clampCameraToGameScreenBounds = clampToBounds;

            if (CurrentGameScreen is not null && clampCameraToGameScreenBounds)
            {
                EnsureCameraIsWithinScreenBounds();
            }
        }

        /// <summary>
        /// Sets all GameScreen properties to null/false/empty
        /// </summary>
        public void ResetCurrentGameScreen()
        {
            CurrentGameScreen = null;
            clampCameraToGameScreenBounds = false;
        }

        public void EnsureCameraIsWithinScreenBounds()
        {
            bool outOfBounds = false;
            RectangleF gameScreenBounds = CurrentGameScreen.Bounds;
            foreach (var corner in ViewBounds.GetCorners())
            {
                if (!gameScreenBounds.Contains(corner))
                {
                    outOfBounds = true;
                    break;
                }
            }

            if (outOfBounds)
            {
                float newX = Position.X;
                float newY = Position.Y;
                if (gameScreenBounds.Left > ViewBounds.Left)
                {
                    newX = gameScreenBounds.Left + ViewBounds.Width / 2;
                }

                if (gameScreenBounds.Right < ViewBounds.Right)
                {
                    newX = gameScreenBounds.Right - ViewBounds.Width / 2;
                }

                if (gameScreenBounds.Top > ViewBounds.Top)
                {
                    newY = gameScreenBounds.Top + ViewBounds.Height / 2;
                }

                if (gameScreenBounds.Bottom < ViewBounds.Bottom)
                {
                    newY = gameScreenBounds.Bottom - ViewBounds.Height / 2;
                }

                Position = new Vector2(newX, newY);
            }
        }
    }
}