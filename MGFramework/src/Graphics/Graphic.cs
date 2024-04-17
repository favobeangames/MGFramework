using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace FavobeanGames.MGFramework.Graphics;

/// <summary>
/// Flag to determine the type of graphic it is.
///
/// </summary>
public enum GraphicType
{
    None,
    Sprite,
    Primitive,
    Text,
    UI,
}

public abstract class Graphic
{
    /// <summary>
    /// GraphicType for the entity
    /// </summary>
    public GraphicType GraphicType { get; }

    /// <summary>
    /// Alpha channel for the color of the graphic
    /// </summary>
    public float Alpha = 1f;

    /// <summary>
    /// Base color of the graphic.
    /// Defaults to Color.White
    /// </summary>
    protected Color color;
    public Color Color
    {
        get => color * Alpha;
        private set => color = value;
    }

    /// <summary>
    /// Width of the sprite in pixels
    /// </summary>
    public int Width;

    /// <summary>
    /// Height of the sprite in pixels
    /// </summary>
    public int Height;

    /// <summary>
    /// Depth that graphic is drawn by the spritebatch.
    /// Defaults to 0.5
    /// </summary>
    protected float Depth = 0.5f;

    /// <summary>
    /// Basic sprite effect.
    /// Default to SpriteEffects.FlipVertically as GraphicsBatch will flip graphics
    /// due to swapping from MonoGames base coordinate system
    /// </summary>
    protected SpriteEffects SpriteEffects = SpriteEffects.FlipVertically;

    /// <summary>
    /// Stores transform properties for the graphic
    /// </summary>
    public Transform2 Transform2 { get; protected set; }

    /// <summary>
    /// Position of the graphic
    /// </summary>
    public Vector2 Position => Transform2.Position;

    /// <summary>
    /// Center Position of the graphic
    /// </summary>
    public Vector2 CenterPosition => new(Position.X - Width / 2f, Position.Y - Height / 2f);

    private RenderTarget2D renderTarget;
    /// <summary>
    /// Render target to draw to the screen
    /// </summary>
    public RenderTarget2D RenderTarget
    {
        get => renderTarget;
        set
        {
            if (value == null)
            {
                renderTarget?.Dispose();
            }

            renderTarget = value;
        }
    }

    /// <summary>
    /// Axis aligned bounding box for the graphic
    /// </summary>
    public RectangleF AABB;

    private int layerDepth = 0;
    /// <summary>
    /// Determines where to draw the entity on the screen
    /// Lower = Rendered below other graphics
    /// Higher = Rendered above other graphics
    /// </summary>
    public int LayerDepth
    {
        get => layerDepth;
        set => layerDepth = value;
    }

    protected Effect activeEffect;

    /// <summary>
    /// Active effect for the graphic
    /// </summary>
    public Effect ActiveEffect => activeEffect;

    public Graphic()
    {
        Color = Color.White;
        GraphicType = GraphicType.None;
    }

    public Graphic(GraphicType graphicType)
    {
        Color = Color.White;
        GraphicType = graphicType;
        Transform2 = Transform2.Empty;
    }

    public Graphic(GraphicType graphicType, Transform2 transform2)
    {
        Color = Color.White;
        GraphicType = graphicType;
        Transform2 = transform2;
    }

    public virtual void Draw(SpriteBatch spriteBatch) { }
    public virtual void Draw(SpriteBatch spriteBatch, Transform2 transform2) { }
    public virtual void Draw(ShapeBatch shapeBatch) { }
    public virtual void Draw(ShapeBatch shapeBatch, Transform2 transform2) { }

    public abstract RectangleF GetAABB();

    public abstract Vector2[] GetTransformedVertices();
}