using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

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
    UI,
}

public class Graphic
{

    /// <summary>
    /// Layer data that the entity is rendered on
    /// </summary>
    public LayerData LayerData { get; set; }

    /// <summary>
    /// GraphicType for the entity
    /// </summary>
    public GraphicType GraphicType { get; }

    /// <summary>
    /// Base color of the graphic.
    /// Defaults to Color.White
    /// </summary>
    protected Color Color;

    /// <summary>
    /// Depth that graphic is drawn by the spritebatch.
    /// Defaults to 0.5
    /// </summary>
    protected float Depth = 0.5f;

    /// <summary>
    /// Origin point of the graphic. Generally the center, but this is the pivot
    /// point were we will rotate the graphic
    /// </summary>
    protected Vector2 Origin = new Vector2(0, 0);

    /// <summary>
    /// Pixel coordinates of the graphic. Defaults to the Top Left position of the
    /// graphic
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// Rotation angle in radians of graphic around the origin point.
    /// </summary>
    public float Rotation = 0;

    /// <summary>
    /// X, Y scale factor for the graphic.
    /// Defaults to 100%, 100%.
    /// </summary>
    public Vector2 Scale = new Vector2(1, 1);

    /// <summary>
    /// Transform matrix used to modify graphic on screen
    /// 3x2 Matrix as this is for 2D games.
    /// </summary>
    /// <returns>Matrix</returns>
    protected Matrix TransformMatrix => Matrix2.CreateFrom(Position, Rotation, Scale, Origin);

    /// <summary>
    /// Vector containing the velocity
    /// </summary>
    public Vector2 Velocity { get; set; }

    public Graphic()
    {
        Color = Color.White;
        GraphicType = GraphicType.None;
    }

    public Graphic(GraphicType graphicType)
    {
        Color = Color.White;
        GraphicType = graphicType;
    }

    public virtual void Update(GameTime gameTime)
    {
        Position += Velocity * (float) gameTime.ElapsedGameTime.TotalSeconds;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
    }

    public virtual void Draw(PrimitiveBatch primitiveBatch)
    {
    }
}