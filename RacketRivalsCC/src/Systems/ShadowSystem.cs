using FavobeanGames.MGFramework.ECS;
using Microsoft.Xna.Framework;

namespace RacketRivalsCC.Systems;

public class ShadowSystem : EntityUpdateSystem
{
    private ComponentMapper<Shadow> shadowMapper;
    private ComponentMapper<GameObject> gameObjectMapper;

    public ShadowSystem()
        : base(Aspect.All(typeof(Shadow)))
    {
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entityId in ActiveEntities)
        {
            var shadow = shadowMapper.Get(entityId);

            shadow?.Update();
        }
    }

    protected override void Initialize(IComponentService componentService)
    {
        shadowMapper = componentService.GetMapper<Shadow>();
        gameObjectMapper = componentService.GetMapper<GameObject>();
    }
}