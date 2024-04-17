using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace FavobeanGames.MGFramework.Graphics;

public class Text : Graphic
{
    /// <summary>
    /// Texture for the sprite.
    /// </summary>
    protected BitmapFont Font;

    protected string textValue;
    /// <summary>
    /// Text value to be rendered to the screen
    /// </summary>
    public string TextValue
    {
        get { return textValue; }
        set
        {
            textValue = value;
            Initialize();
        }
    }

    public Vector2 Origin;

    private readonly Vector2 scaleModifier = new Vector2(1, -1);

    public Text()
        : base(GraphicType.Text)
    {

    }

    public Text(BitmapFont font, string text)
        : base(GraphicType.Text)
    {
        Font = font;
        TextValue = text;

        Initialize();
    }

    public Text(Transform2 transform2, BitmapFont font, string text)
        : base(GraphicType.Sprite)
    {
        Font = font;
        TextValue = text;
        Transform2 = transform2;

        Initialize();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(Font, TextValue, Transform2.Position, Color, Transform2.Rotation, Origin, Transform2.Scale * scaleModifier, SpriteEffects.None, Depth);
    }

    public override RectangleF GetAABB()
    {
        throw new System.NotImplementedException();
    }

    public override Vector2[] GetTransformedVertices()
    {
        throw new System.NotImplementedException();
    }

    private void Initialize()
    {
        var size = Font.MeasureString(TextValue);
        Origin = new Vector2(size.Width / 2, size.Height / 2);
        Width = (int) System.Math.Ceiling(size.Width);
        Height = (int) System.Math.Ceiling(size.Height);
    }
}