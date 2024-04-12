using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Physics;
using Microsoft.Xna.Framework;

namespace RacketRivalsCC.Systems;

public class GameObjectSystem : EntityUpdateSystem
{
    private ComponentMapper<GameObject> gameObjectMapper;
    private ComponentMapper<RigidBody> rigidBodyMapper;

    public GameObjectSystem()
        : base(Aspect.All(typeof(GameObject)).Any(typeof(Shadow), typeof(RigidBody)))
    {
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entityId in ActiveEntities)
        {
            var obj = gameObjectMapper.Get(entityId);
        }
    }

    protected override void Initialize(IComponentService componentService)
    {
        gameObjectMapper = componentService.GetMapper<GameObject>();
        rigidBodyMapper = componentService.GetMapper<RigidBody>();
    }
}