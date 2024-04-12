using FavobeanGames.MGFramework.Cameras;
using FavobeanGames.MGFramework.Input;
using Microsoft.Xna.Framework;
using RacketRivalsCC;

namespace FavobeanGames.MGFramework.ECS;

public class Player
{
    /// <summary>
    /// Reference to the camera for the player
    /// </summary>
    private readonly Camera camera;

    /// <summary>
    /// Input manager for the player
    /// </summary>
    internal readonly InputManager InputManager;

    /// <summary>
    /// Flag to determine if the player is in a match or
    /// the overworld
    /// </summary>
    public bool InAMatch;

    public Player(Camera camera, InputManager inputManager)
    {
        this.camera = camera;
        InputManager = inputManager;
    }

}