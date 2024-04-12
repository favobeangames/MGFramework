using FavobeanGames.MGFramework.ECS;
using Microsoft.Xna.Framework;
using RacketRivalsCC.Match;

namespace RacketRivalsCC.Systems;

public class BallSystem : EntityUpdateSystem
{
    private ComponentMapper<Ball> ballMapper;
    private ComponentMapper<GameObject> gameObjectMapper;

    public BallSystem()
        : base(Aspect.All(typeof(Ball), typeof(GameObject)))
    {
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entityId in ActiveEntities)
        {
            var ball = ballMapper.Get(entityId);
            var gameObject = gameObjectMapper.Get(entityId);


        }
    }

    protected override void Initialize(IComponentService componentService)
    {
        ballMapper = componentService.GetMapper<Ball>();
        gameObjectMapper = componentService.GetMapper<GameObject>();
    }
}