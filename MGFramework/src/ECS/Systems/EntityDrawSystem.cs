using FavobeanGames.MGFramework.Components;
using FavobeanGames.MGFramework.DataStructures.Collections;
using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.MGFramework;

public class EntityDrawSystem: EntitySystem, IDrawSystem
{
    protected GraphicsDevice graphicsDevice;
    protected GraphicsBatch graphicsBatch;

    public EntityDrawSystem(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
        graphicsBatch = new GraphicsBatch(graphicsDevice);
    }

    public void Draw(params Entity[] entities)
    {
        throw new System.NotImplementedException();
    }
}