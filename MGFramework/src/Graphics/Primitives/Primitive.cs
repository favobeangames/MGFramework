using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.MGFramework.Graphics.Primitives;
public class Primitive : Graphic
{
    // Fill color for primitive shapes
    protected Color FillColor;
    // Flag to determine if the primitive should have an outline
    protected bool HasOutline;
    // Outline color
    protected Color OutlineColor;
    // Outline thickness
    protected int OutlineThickness;

    public Primitive()
    {

    }

    public Primitive(Color fillColor)
        : base(GraphicType.Primitive)
    {
        FillColor = fillColor;
    }
    public Primitive(Color outlineColor, int outlineThickness, Color fillColor)
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
    public static readonly Polygon Rectangle = new Polygon(new[]
    {
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0)
    }, Color.White);

    public bool Contains(Polygon polygon)
    {
#if DEBUG
        throw new NotImplementedException("Primitive base class does not implement Primitive.Contains()");
#endif
        return false;
    }

    public bool Intersects(Polygon polygon, out Polygon intersection)
    {
#if DEBUG
        throw new NotImplementedException("Primitive base class does not implement Primitive.Intersects()");
#endif
        intersection = null;
        return false;
    }
}