using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.MGFramework.Graphics
{
    /// <summary>
    /// 2D Graphic
    /// </summary>
    public class Sprite : Graphic
    {
        /// <summary>
        /// Rectangle that represents the area of the texture that this sprite
        /// should render
        /// </summary>
        protected Rectangle? SourceRect;
        
        /// <summary>
        /// Basic sprite effect. Defaults to SpriteEffects.None
        /// Default to SpriteEffects.FlipVertically as GraphicsBatch
        /// </summary>
        protected SpriteEffects SpriteEffects = SpriteEffects.FlipVertically;
        
        /// <summary>
        /// Texture for the sprite.
        /// </summary>
        protected Texture2D Texture;

        public Sprite()
            : base(GraphicType.Sprite)
        {
            
        }

        public Sprite(Texture2D texture, Vector2 position, Rectangle? sourceRect, Vector2 scale, Vector2 origin)
            : base(GraphicType.Sprite)
        {
            Texture = texture;
            Position = position;
            SourceRect = sourceRect;
            Scale = scale;
            Origin = origin;
        }
        
        /// <summary>
        /// Loads content into the Game instance of the Content Manager
        /// </summary>
        /// <param name="content">ContentManager instance <see cref="ContentManager"/></param>
        protected virtual void LoadContent(ContentManager content)
        {
        }

        /// <summary>
        /// Base update logic for the Sprite
        /// </summary>
        /// <param name="gameTime">GameTime instance <see cref="GameTime"/></param>
        public override void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draws the sprite to the window
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance <see cref="SpriteBatch"/></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRect, Color, Rotation, Origin, Scale, SpriteEffects, Depth);
        }
    }

}