using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.MGFramework
{
    public static class TextureCreator
    {
        public static Texture2D PixelTexture;

        private static readonly Color OUTLINE_COLOR = Color.White;
        public static GraphicsDevice Graphics { get; set; }

        public static void Initialize(GraphicsDevice graphics)
        {
            Graphics = graphics;

            // Initialize White pixel texture for general use
            PixelTexture = new Texture2D(graphics, 1, 1);
            var colorData = new Color[1];
            colorData[0] = Color.White;
            PixelTexture.SetData(colorData);
        }

        public static Texture2D CreateCircle(int radius, Color color, bool outline = false)
        {
            var diameter = radius * 2;
            var texture = new Texture2D(Graphics, diameter, diameter);
            var center = new Vector2(radius, radius);
            var colorData = new Color[diameter * diameter];
            var pixel = 0;
            for (var i = 0; i < diameter; i++)
            for (var j = 0; j < diameter; j++)
            {
                if (Vector2.Distance(new Vector2(j, i), center) < radius) colorData[pixel] = color;

                if (outline)
                    if (Vector2.Distance(new Vector2(j, i), center) == radius)
                        colorData[pixel] = OUTLINE_COLOR;
                pixel++;
            }

            texture.SetData(colorData);
            return texture;
        }

        public static Texture2D CreateRectangle(int width, int height, Color color)
        {
            var texture = new Texture2D(Graphics, width, height);
            var colorData = new Color[width * height];

            for (var i = 0; i < colorData.Length; i++) colorData[i] = color;

            texture.SetData(colorData);
            return texture;
        }
    }
}