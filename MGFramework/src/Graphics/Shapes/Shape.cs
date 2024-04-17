using System;
using FavobeanGames.MGFramework.Geometry2D.Shapes;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

public enum ShapeType
{
    Line,
    Circle,
    Polygon,
}
public class Shape : Graphic
{
    /// <summary>
    /// Type of shape (Line, Circle, Polygon, etc)
    /// </summary>
    public ShapeType ShapeType { get; protected set; }

    /// <summary>
    /// Stores the geometry information for the primitive graphic
    /// </summary>
    public Geometry Geometry { get; set; }

    public new Transform2 Transform2 => Geometry.Transform2;
    public override RectangleF GetAABB()
    {
        throw new NotImplementedException();
    }

    public override Vector2[] GetTransformedVertices()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Fill color for primitive shapes
    /// </summary>
    protected Color FillColor;

    /// <summary>
    /// Flag to determine if the primitive should have an outline
    /// </summary>
    protected bool HasOutline;

    /// <summary>
    /// Outline color
    /// </summary>
    public Color OutlineColor;

    /// <summary>
    /// Outline thickness
    /// </summary>
    public int OutlineThickness;

    public Shape()
        : base(GraphicType.Primitive)
    {
    }

    public Shape(Color fillColor)
        : base(GraphicType.Primitive)
    {
        FillColor = fillColor;
    }
    public Shape(Color outlineColor, int outlineThickness, Color fillColor)
        : base(GraphicType.Primitive)
    {
        HasOutline = true;
        OutlineColor = outlineColor;
        OutlineThickness = outlineThickness;
        FillColor = fillColor;
    }

    /// <summary>
    /// Returns a 1x1 white rectangle polygon
    /// </summary>
    public static readonly Polygon Rectangle = new Polygon(
        new Transform2(Vector2.Zero, Vector2.One, 0f),
        new []
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        },
        Color.White);
}