using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Input;
using FavobeanGames.MGFramework.Physics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using RacketRivalsCC.Match;

namespace RacketRivalsCC.Systems;

public class PlayerSystem : EntityUpdateSystem
{
    private readonly Vector2 slowingVectorX = new(.8f, 1);
    private readonly Vector2 slowingVectorY = new(1, .8f);
    private readonly Vector2 maxRunningVel = new Vector2(180, 180);
    private bool movedX;
    private bool movedY;

    private ComponentMapper<Player> playerMapper;
    private ComponentMapper<RigidBody> bodyMapper;
    private ComponentMapper<Stats> statsMapper;
    private ComponentMapper<Sprite> spriteMapper;
    private ComponentMapper<MatchPlayer> matchPlayerMapper;

    public PlayerSystem()
        : base(
            Aspect
                .All(typeof(Player), typeof(RigidBody), typeof(Sprite), typeof(Stats))
                .Any(typeof(MatchPlayer), typeof(OverworldObject)))
    {
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entityId in ActiveEntities)
        {
            Update(entityId, gameTime);
        }
    }
    protected override void Initialize(IComponentService componentService)
    {
        playerMapper = componentService.GetMapper<Player>();
        statsMapper = componentService.GetMapper<Stats>();
        bodyMapper = componentService.GetMapper<RigidBody>();
        spriteMapper = componentService.GetMapper<Sprite>();
        matchPlayerMapper = componentService.GetMapper<MatchPlayer>();
    }

    private void Update(int entityId, GameTime gameTime)
    {
        var elapsed = gameTime.GetElapsedSeconds();
        var player = playerMapper.Get(entityId);
        var input = player?.InputManager;
        var body = bodyMapper.Get(entityId);
        var stats = statsMapper.Get(entityId);
        var sprite = spriteMapper.Get(entityId);
        var matchPlayer = matchPlayerMapper.Get(entityId);
        var animation = "";
        var shouldUpdateMovement = true;

        input?.Update(gameTime);

        if (matchPlayer is not null)
            UpdateMatchLogic(matchPlayer, input, body, ref animation, ref shouldUpdateMovement);

        if (shouldUpdateMovement)
            UpdateMovement(input, body, stats);

        if (animation != "")
            sprite.SetAnimation(animation);

        sprite.Update(elapsed);

    }

    private void UpdateMovement(InputManager input, RigidBody body, Stats stats)
    {
        movedX = false;
        movedY = false;

        if (input.IsControlHeld(input.ControlMapping.GetControl("moveLeft")))
        {
            if (body.LinearVelocity.X > 0)
                body.LinearVelocity *= slowingVectorX;
            body.LinearVelocity += new Vector2(-stats.Speed, 0);
            movedX = true;
        }

        if (input.IsControlHeld(input.ControlMapping.GetControl("moveRight")))
        {
            if (body.LinearVelocity.X < 0)
                body.LinearVelocity *= slowingVectorX;
            body.LinearVelocity += new Vector2(stats.Speed, 0);
            movedX = true;
        }

        if (input.IsControlHeld(input.ControlMapping.GetControl("moveUp")))
        {
            if (body.LinearVelocity.Y < 0)
                body.LinearVelocity *= slowingVectorY;
            body.LinearVelocity += new Vector2(0, stats.Speed/2);
            movedY = true;
        }

        if (input.IsControlHeld(input.ControlMapping.GetControl("moveDown")))
        {
            if (body.LinearVelocity.Y > 0)
                body.LinearVelocity *= slowingVectorY;
            body.LinearVelocity += new Vector2(0, -stats.Speed/2);
            movedY = true;
        }

        // Slow the player down
        if (!movedX && body.LinearVelocity.X != 0)
        {
            body.LinearVelocity *= slowingVectorX;
            if (body.LinearVelocity.X is <= 1 or >= -1) body.LinearVelocity.SetX(0);
        }

        if (!movedY && body.LinearVelocity.Y != 0)
        {
            body.LinearVelocity *= slowingVectorY;
            if (body.LinearVelocity.Y is <= 1 or >= -1) body.LinearVelocity.SetY(0);
        }

        body.LinearVelocity = Vector2.Clamp(body.LinearVelocity, -maxRunningVel, maxRunningVel);
    }

    private void UpdateMatchLogic(MatchPlayer matchPlayer, InputManager input, RigidBody body, ref string animation, ref bool shouldUpdateMovement)
    {
        if (matchPlayer is {Serving: true})
        {
            if (!matchPlayer.ThrownServe)
            {
                if (input.IsControlPressed(input.ControlMapping.GetControl("action")))
                {
                    matchPlayer.ThrownServe = true;
                    body.LinearVelocity = Vector2.Zero;
                    animation = "serveThrow";
                }
            }
            else
            {
                if (input.IsControlPressed(input.ControlMapping.GetControl("action")))
                {
                    matchPlayer.ThrownServe = false;
                    matchPlayer.HasServed = true;
                    animation = "serveHit";
                }
            }

            if (matchPlayer.ThrownServe)
                shouldUpdateMovement = false;

            if (!matchPlayer.ThrownServe && !matchPlayer.HasServed)
            {
                animation = "serveMovement";
            }
        }
    }
}