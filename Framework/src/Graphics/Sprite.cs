using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.Graphics
{
    /// <summary>
    /// 2D Graphic
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// Base color of the sprite.
        /// Defaults to Color.White
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// Depth that sprite is drawn by the spritebatch.
        /// Defaults to 0.5
        /// </summary>
        public float Depth = 0.5f;

        /// <summary>
        /// Origin point of the sprite. Generally the center, but this is the pivot
        /// point were we will rotate the graphic
        /// </summary>
        public Vector2 Origin;
        
        /// <summary>
        /// Pixel coordinates of the sprite. Defaults to the Top Left position of the
        /// graphic
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Rotation angle in radians of sprite around the origin point.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// X, Y scale factor for the graphic.
        /// Defaults to 100%, 100%.
        /// </summary>
        public Vector2 Scale = new Vector2(1, 1);
        
        /// <summary>
        /// Rectangle that represents the area of the texture that this sprite
        /// should render
        /// </summary>
        public Rectangle SourceRect;
        
        /// <summary>
        /// Basic sprite effect. Defaults to SpriteEffects.None
        /// </summary>
        public SpriteEffects SpriteEffect = SpriteEffects.None;
        
        /// <summary>
        /// Texture for the sprite.
        /// </summary>
        public Texture2D Texture;


        /// <summary>
        /// Loads content into the Game instance of the Content Manager
        /// </summary>
        /// <param name="content">ContentManager instance <see cref="ContentManager"/></param>
        public virtual void LoadContent(ContentManager content)
        {
        }

        /// <summary>
        /// Base update logic for the Sprite
        /// </summary>
        /// <param name="gameTime">GameTime instance <see cref="GameTime"/></param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draws the sprite to the window
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance <see cref="SpriteBatch"/></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRect, Color, Rotation, Origin, Scale, SpriteEffect, Depth);
        }
    }

}