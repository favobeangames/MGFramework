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
    {
    }

    public Shape(Transform2 transform)
        : base(GraphicType.Primitive)
    {
        Transform2 = transform;
    }

    public Shape(Transform2 transform, Color fillColor)
        : base(GraphicType.Primitive)
    {
        Transform2 = transform;
        FillColor = fillColor;
    }
    public Shape(Transform2 transform, Color outlineColor, int outlineThickness, Color fillColor)
        : base(GraphicType.Primitive)
    {
        Transform2 = transform;
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

    /// <summary>
    /// Returns a list of vertices transformed into world space
    /// </summary>
    /// <returns>Array of Vector2</returns>
    public virtual Vector2[] GetTransformedVertices()
    {
        return null;
    }

    public override RectangleF GetAABB()
    {
        throw new NotImplementedException();
    }
}