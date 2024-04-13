using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.Graphics
{
    /// <summary>
    /// 2D Graphic
    /// </summary>
    public class Sprite : Graphic
    {
        private Rectangle? sourceRect;

        /// <summary>
        /// Rectangle that represents the area of the texture that this sprite
        /// should render
        /// </summary>
        public Rectangle? SourceRect
        {
            get => SpriteSheet != null ? SpriteSheet.CurrentAnimation?.CurrentFrame : sourceRect;
            set
            {
                sourceRect = value;
                if (sourceRect != null)
                {
                    Width = sourceRect.Value.Width;
                    Height = sourceRect.Value.Height;
                }
            }
        }

        /// <summary>
        /// The point on the sprite that gets rendered at
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// Texture for the sprite
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Class containing the animations and logic
        /// for the sprite
        /// </summary>
        public readonly SpriteSheet SpriteSheet;

        public new Effect ActiveEffect
        {
            get => activeEffect;
            set => activeEffect = value;
        }

        public Sprite()
            : base(GraphicType.Sprite)
        {
            
        }

        public Sprite(Texture2D texture, Transform2 transform2, Rectangle? sourceRect, Vector2 origin)
            : base(GraphicType.Sprite)
        {
            Texture = texture;
            SourceRect = sourceRect;
            Transform2 = transform2;
            Origin = origin;
        }

        public Sprite(RenderTarget2D target, Transform2 transform2, Vector2 origin)
            : base(GraphicType.Sprite)
        {
            RenderTarget = target;
            Transform2 = transform2;
            Origin = origin;
        }

        public Sprite(Texture2D texture, SpriteSheet spriteSheet, Transform2 transform2, Vector2 origin)
            : base(GraphicType.Sprite)
        {
            Texture = texture;
            SpriteSheet = spriteSheet;
            Transform2 = transform2;
            Origin = origin;
        }

        /// <summary>
        /// Draws the sprite to the window
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance <see cref="SpriteBatch"/></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (RenderTarget != null)
                spriteBatch.Draw(RenderTarget,
                    Transform2.Position + Origin, RenderTarget.Bounds,
                    Color, Transform2.Rotation, Origin,
                    Transform2.Scale, SpriteEffects, Depth);
            else if (Texture != null)
                spriteBatch.Draw(Texture,
                    Transform2.Position + Origin, SourceRect, Color,
                    Transform2.Rotation, Origin,
                    Transform2.Scale, SpriteEffects, Depth);
        }

        /// <summary>
        /// Draws the sprite to the window
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance <see cref="SpriteBatch"/></param>
        public override void Draw(SpriteBatch spriteBatch, Transform2 transform2)
        {
            if (RenderTarget != null)
                spriteBatch.Draw(RenderTarget,
                    transform2.Position + Origin, RenderTarget.Bounds,
                    Color, transform2.Rotation, Origin,
                    transform2.Scale, SpriteEffects, Depth);
            else if (Texture != null)
                spriteBatch.Draw(Texture,
                    transform2.Position + Origin, SourceRect, Color,
                    transform2.Rotation, Origin,
                    transform2.Scale, SpriteEffects, Depth);
        }

        public override RectangleF GetAABB()
        {
            throw new NotImplementedException();
        }

        public void Update(float elapsed)
        {
            SpriteSheet?.Update(elapsed);
        }

        public void SetAnimation(string key)
        {
            SpriteSheet?.SetAnimation(key);
        }
    }

}