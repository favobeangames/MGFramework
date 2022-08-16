using System.Collections.Generic;
using FavobeanGames.DataStructures.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.Components
{
    /// <summary>
    ///     2D Sprite object for Monogame
    /// </summary>
    public class Sprite
    {
        /// <summary>
        ///     Base color of the sprite.
        ///     Defaults to Color.White
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        ///     Depth that sprite is drawn by the spritebatch.
        ///     Defaults to 0.5
        /// </summary>
        public float Depth = 0.5f;

        public Vector2 Origin;
        public Vector2 Position;

        /// <summary>
        ///     Rotation angle in radians of sprite around the origin point.
        /// </summary>
        public float Rotation;

        public Vector2 Scale = new Vector2(1, 1);
        public Rectangle SourceRect;
        public SpriteEffects SpriteEffect;
        public Texture2D Texture;


        public virtual void LoadContent(ContentManager content)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRect, Color, Rotation, Origin, Scale, SpriteEffect, Depth);
        }
    }

}