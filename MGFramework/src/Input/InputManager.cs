using FavobeanGames.MGFramework.CameraSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FavobeanGames.MGFramework.Input
{
    /// <summary>
    /// InputManager handles input for Keyboard, GamePad, and Mouse controls.
    /// </summary>
    public class InputManager
    {
        private GamePadState currentGamePadState;
        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;

        private GamePadState prevGamePadState;
        private KeyboardState prevKeyboardState;
        private MouseState prevMouseState;

        /// <summary>
        /// Reference to the camera for the player
        /// </summary>
        private Camera camera;

        /// <summary>
        /// Reference to the screen for the game
        /// </summary>
        private GameWindow gameWindow;

        // TODO: Need to figure out a component to manage known InputControls
        public InputManager()
        {
        }

        public InputManager(Camera camera, GameWindow gameWindow)
        {
            this.camera = camera;
            this.gameWindow = gameWindow;
        }
        public InputManager(Camera camera, GameWindow gameWindow, int playerIndex)
        {
            this.camera = camera;
            this.gameWindow = gameWindow;
            PlayerIndex = playerIndex;
        }

        // PlayerIndex is the port of the player. Defaults to 1
        public int PlayerIndex { get; set; }

        public void Update(GameTime gameTime)
        {
            prevKeyboardState = currentKeyboardState;
            prevGamePadState = currentGamePadState;
            prevMouseState = currentMouseState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex);
            currentMouseState = Mouse.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

        private void UpdateMouse()
        {
        }

        #region Control Functions

        /// <summary>
        /// Checks to see if the control is pressed.
        /// This only returns true if the button/key is not being held
        /// </summary>
        /// <param name="control">InputControl object</param>
        /// <returns></returns>
        public bool IsControlPressed(InputControl control)
        {
            if (IsGamepadButtonPressed(control.GamepadButton)) return true;

            if (IsKeyPressed(control.KeyboardKey)) return true;

            return false;
        }

        /// <summary>
        /// Checks to see keyboard key is pressed and released
        /// </summary>
        /// <param name="key">Keyboard key</param>
        /// <returns></returns>
        public bool IsKeyPressed(Keys key)
        {
            if (prevKeyboardState.IsKeyDown(key) &&
                !currentKeyboardState.IsKeyDown(key)) return true;

            return false;
        }

        /// <summary>
        /// Checks to see if the gamepad button is pressed and released
        /// </summary>
        /// <param name="button">Gamepad button</param>
        /// <returns></returns>
        public bool IsGamepadButtonPressed(Buttons button)
        {
            if (prevGamePadState.IsButtonDown(button) &&
                !currentGamePadState.IsButtonDown(button)) return true;

            return false;
        }

        /// <summary>
        /// Checks to see if the control is held down.
        /// </summary>
        /// <param name="control">InputControl object</param>
        /// <returns></returns>
        public bool IsControlHeld(InputControl control)
        {
            if (IsGamepadButtonHeld(control.GamepadButton)) return true;

            if (IsKeyHeld(control.KeyboardKey)) return true;

            return false;
        }

        /// <summary>
        /// Checks to see keyboard key is pressed and held
        /// </summary>
        /// <param name="key">Keyboard key</param>
        /// <returns></returns>
        public bool IsKeyHeld(Keys key)
        {
            if (prevKeyboardState.IsKeyDown(key) &&
                currentKeyboardState.IsKeyDown(key)) return true;

            return false;
        }

        /// <summary>
        /// Checks to see if the gamepad button is pressed and held
        /// </summary>
        /// <param name="button">Gamepad button</param>
        /// <returns></returns>
        public bool IsGamepadButtonHeld(Buttons button)
        {
            if (prevGamePadState.IsButtonDown(button) &&
                currentGamePadState.IsButtonDown(button)) return true;

            return false;
        }

        #endregion

        #region Mouse Functions

        public bool MouseLeftButtonPressed()
        {
            return prevMouseState.LeftButton == ButtonState.Pressed &&
                   currentMouseState.LeftButton == ButtonState.Released;
        }

        public bool MouseLeftButtonHeld()
        {
            return prevMouseState.LeftButton == ButtonState.Pressed &&
                   currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool MouseRightButtonPressed()
        {
            return prevMouseState.RightButton == ButtonState.Pressed &&
                   currentMouseState.RightButton == ButtonState.Released;
        }

        public bool MouseRightButtonHeld()
        {
            return prevMouseState.RightButton == ButtonState.Pressed &&
                   currentMouseState.RightButton == ButtonState.Pressed;
        }

        public Vector2 GetMousePosition()
        {
            return currentMouseState.Position.ToVector2();
        }

        public Vector2 GetMouseScreenPosition()
        {
            // Get the size and position of the screen when stretched to fit into the game window (keeping the correct aspect ratio).
            Rectangle screenDestinationRectangle = gameWindow.CalculateDestinationRectangle();

            // Get the position of the mouse in the game window backbuffer coordinates.
            Point mouseWindowPosition = currentMouseState.Position;

            // Get the position of the mouse relative to the screen destination rectangle position.
            float sx = mouseWindowPosition.X - screenDestinationRectangle.X;
            float sy = mouseWindowPosition.Y - screenDestinationRectangle.Y;

            // Convert the position to a normalized ratio inside the screen destination rectangle.
            sx /= screenDestinationRectangle.Width;
            sy /= screenDestinationRectangle.Height;

            // Multiply the normalized coordinates by the actual size of the screen to get the location in screen coordinates.
            float x = sx * gameWindow.Width;
            float y = sy * gameWindow.Height;

            return new Vector2(x, y);
        }

        public Vector2 GetMouseWorldPosition()
        {
            // Create a viewport based on the game screen.
            Viewport screenViewport = new Viewport(0, 0, gameWindow.Width, gameWindow.Height);

            // Get the mouse pixel coordinates in that screen.
            Vector2 mouseScreenPosition = GetMouseScreenPosition();

            // Create a ray that starts at the mouse screen position and points "into" the screen towards the game world plane.
            Ray mouseRay = CreateMouseRay(mouseScreenPosition, screenViewport);

            // Plane where the flat 2D game world takes place.
            Plane worldPlane = new Plane(new Vector3(0, 0, 1f), 0f);

            // Determine the point where the ray intersects the game world plane.
            float? dist = mouseRay.Intersects(worldPlane);
            if (dist != null)
            {
                Vector3 ip = mouseRay.Position + mouseRay.Direction * dist.Value;

                // Send the result as a 2D world position vector.
                Vector2 result = new Vector2(ip.X, ip.Y);
                return result;
            }

            return mouseScreenPosition;
        }

        private Ray CreateMouseRay(Vector2 mouseScreenPosition, Viewport viewport)
        {
            // Near and far points that will indicate the line segment used to define the ray.
            Vector3 nearPoint = new Vector3(mouseScreenPosition, 0);
            Vector3 farPoint = new Vector3(mouseScreenPosition, 1);

            // Convert the near and far points to world coordinates.
            nearPoint = viewport.Unproject(nearPoint, camera.Projection, camera.View, Matrix.Identity);
            farPoint = viewport.Unproject(farPoint, camera.Projection, camera.View, Matrix.Identity);

            // Determine the direction.
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // Resulting ray starts at the near mouse position and points "into" the screen.
            Ray result = new Ray(nearPoint, direction);
            return result;
        }

        public void SetMousePosition(Vector2 position)
        {
            Mouse.SetPosition((int) position.X, (int) position.Y);
        }

        #endregion
    }
}