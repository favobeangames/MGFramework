using System.Linq;
using FavobeanGames.MGFramework.Cameras;
using FavobeanGames.MGFramework.ECS;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Transform2 = FavobeanGames.MGFramework.Transform2;

namespace RacketRivalsCC.Systems;

public class CameraSystem : UpdateSystem
{
    private Camera camera;

    private Transform2 followTransform;
    private RectangleF? clampedBounds;

    public CameraSystem(Camera camera)
    {
        this.camera = camera;
    }
    public override void Update(GameTime gameTime)
    {
        FollowTransform();
        ClampCameraToBounds();
    }

    public void SetFollowTransform(Transform2 transform2)
    {
        followTransform = transform2;
    }

    public void SetViewingBounds(RectangleF bounds)
    {
        clampedBounds = bounds;
    }

    private void FollowTransform()
    {
        if (followTransform != null && camera.Position != followTransform.Position)
        {
            camera.Position = followTransform.Position;
        }
    }

    private void ClampCameraToBounds()
    {
        if (clampedBounds == null)
            return;

        var rect = clampedBounds.Value;

        // Check to see if the camera bounds are outside of
        // the set desired bounds
        camera.GetExtents(out RectangleF viewBounds);
        var outOfBounds = viewBounds
            .GetCorners()
            .Any(corner => !rect.Contains(corner));

        if (!outOfBounds)
            return;

        var cameraWidth = viewBounds.Width;
        var cameraHeight = viewBounds.Height;
        var newPosX = camera.Position.X;
        var newPosY = camera.Position.Y;
        var changedX = false;
        var changedY = false;

        if (viewBounds.Left < rect.Left)
        {
            newPosX = rect.Left + cameraWidth / 2;
            changedX = true;
        }

        if (viewBounds.Right > rect.Right)
        {
            newPosX = rect.Right - cameraWidth / 2;
            changedX = true;
        }

        if (viewBounds.Top < rect.Top)
        {
            newPosY = rect.Top + cameraHeight / 2;
            changedY = true;
        }

        if (viewBounds.Bottom > rect.Bottom)
        {
            newPosY = rect.Bottom - cameraHeight / 2;
            changedY = true;
        }

        if (changedX || changedY)
            camera.Position = new Vector2(newPosX, newPosY);
    }
}