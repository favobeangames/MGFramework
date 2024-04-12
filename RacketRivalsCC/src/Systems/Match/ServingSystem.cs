using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Util;
using Microsoft.Xna.Framework;
using RacketRivalsCC.Match;
using RectangleF = MonoGame.Extended.RectangleF;

namespace RacketRivalsCC.Systems;

public class ServingSystem : EntityUpdateSystem
{
    private ComponentMapper<GameObject> gameObjectMapper;
    private ComponentMapper<MatchPlayer> matchPlayerMapper;
    private ComponentMapper<ServingPlayer> servingPlayerMapper;

    public ServingSystem()
        : base(Aspect.All(typeof(ServingPlayer), typeof(GameObject), typeof(MatchPlayer)))
    {
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entityId in ActiveEntities)
        {
            var servingPlayer = servingPlayerMapper.Get(entityId);
            var gameObject = gameObjectMapper.Get(entityId);
            var matchPlayer = matchPlayerMapper.Get(entityId);

            // Check if the player has served, remove component if they have
            if (matchPlayer.Serving && matchPlayer.HasServed)
            {
                servingPlayerMapper.Delete(entityId);
                continue;
            }

            // Ensure player stays within the serving area
            ClampPlayerToServingBounds(gameObject, servingPlayer.ServingBounds);

            // AI player logic can be updated here to have them re-position and
            // serve to the opposing team
        }
    }

    protected override void Initialize(IComponentService componentService)
    {
        gameObjectMapper = componentService.GetMapper<GameObject>();
        matchPlayerMapper = componentService.GetMapper<MatchPlayer>();
        servingPlayerMapper = componentService.GetMapper<ServingPlayer>();
    }

    private void ClampPlayerToServingBounds(GameObject gameObject, RectangleF servingBounds)
    {
        var newPos = GeometryUtils.FindNearestPointInsideRect(servingBounds, gameObject.Position);
        if (newPos != gameObject.Position)
        {
            gameObject.SetPosition(newPos);
        }
    }
}