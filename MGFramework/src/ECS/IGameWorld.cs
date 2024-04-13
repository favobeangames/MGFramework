using FavobeanGames.MGFramework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FavobeanGames.MGFramework.ECS;

public interface IGameWorld
{
    public void LoadContent(ContentManager content);
    public void Update(GameTime gameTime);
    public void Draw(GraphicsBatch graphicsBatch);
}