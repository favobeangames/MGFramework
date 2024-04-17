using FavobeanGames.MGFramework.Geometry2D.Shapes;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

public class Circle : Shape
{
    private int points;

    public Circle()
    {
        ShapeType = ShapeType.Circle;
    }

    public Circle(Geometry geometry, int points, Color fillColor)
        : base(fillColor)
    {
        Geometry = geometry;
        this.points = points;
        ShapeType = ShapeType.Circle;
    }

    public Circle(Geometry geometry, int points, Color fillColor, int outlineThickness, Color outlineColor)
        : base(outlineColor, outlineThickness, fillColor)
    {
        Geometry = geometry;
        this.points = points;
        ShapeType = ShapeType.Circle;
    }

    public Circle(Transform2 transform2, int points, Color fillColor)
    {
        this.points = points;
        FillColor = fillColor;
        ShapeType = ShapeType.Circle;
        Geometry = new Geometry2D.Shapes.Circle(transform2, 0);
    }

    public Circle(Transform2 transform2, int points, Color fillColor, int outlineThickness, Color outlineColor)
        :base(outlineColor, outlineThickness, fillColor)
    {
        this.points = points;
        ShapeType = ShapeType.Circle;
        Geometry = new Geometry2D.Shapes.Circle(transform2, 0);
    }

    public Circle(Transform2 transform2, float radius, int points, Color fillColor, int outlineThickness, Color outlineColor)
        :base(outlineColor, outlineThickness, fillColor)
    {
        this.points = points;
        ShapeType = ShapeType.Circle;
        Geometry = new Geometry2D.Shapes.Circle(transform2, radius);
    }
    public Circle(Vector2 center, float radius, int points, Color fillColor)
    {
        this.points = points;
        FillColor = fillColor;
        ShapeType = ShapeType.Circle;
        Geometry = new Geometry2D.Shapes.Circle(center, radius);
    }

    public override void Draw(ShapeBatch shapeBatch)
    {
        // X and Y are the origin of the circle when rendering. The TransformMatrix
        // supplies the position in which it will be translated to before rendering.
        shapeBatch.DrawCircleFill(0, 0, Geometry.Radius, points, FillColor, Geometry.Transform2.TransformMatrix);

        // Vector2 va = Vector2.Transform(Vector2.Zero, Transform2.VertexTransformMatrix);
        // Vector2 vb = Vector2.Transform(new Vector2(Transform2.Radius, 0f), Transform2.VertexTransformMatrix);
        //
        // primitiveBatch.DrawLine(va, vb, 1f, Color.White);

        if (HasOutline)
        {
            shapeBatch.DrawCircle(0, 0, Geometry.Radius, points, OutlineThickness, OutlineColor, Geometry.Transform2?.TransformMatrix ?? Matrix.Identity);
        }

    }
}