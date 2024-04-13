using FavobeanGames.MGFramework.CameraSystem;
using FavobeanGames.MGFramework.Components;

namespace FBFramework.Components.Player;

public class Player
{
    private Entity entity;

    /// <summary>
    /// Reference to the Camera for the player.
    /// This may be useful when it comes to multiplayer
    /// This may also be better managed in GameManager instead though...
    /// </summary>
    public readonly Camera Camera;

    public Player()
    {

    }

    public Player(Entity entity, Camera camera)
    {
        this.entity = entity;
        Camera = camera;
    }

}