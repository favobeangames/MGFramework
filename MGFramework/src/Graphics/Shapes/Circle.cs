using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

public class Circle : Primitive
{
    private int points;

    public Circle()
    {
        ShapeType = ShapeType.Circle;
    }

    public Circle(Transform2 transform2, int points, Color fillColor)
        :base(transform2)
    {
        this.points = points;
        FillColor = fillColor;
        ShapeType = ShapeType.Circle;
    }

    public Circle(Transform2 transform2, int points, Color fillColor, int outlineThickness, Color outlineColor)
        :base(transform2, outlineColor, outlineThickness, fillColor)
    {
        this.points = points;
        ShapeType = ShapeType.Circle;
        Geometry = new Geometry2D.Shapes.Circle(0, 0, 0);
    }

    public Circle(Transform2 transform2, float radius, int points, Color fillColor, int outlineThickness, Color outlineColor)
        :base(transform2, outlineColor, outlineThickness, fillColor)
    {
        this.points = points;
        ShapeType = ShapeType.Circle;
        Geometry = new Geometry2D.Shapes.Circle(0, 0, 0);
    }
    public Circle(Vector2 center, float radius, int points, Color fillColor)
    {
        this.points = points;
        FillColor = fillColor;
        ShapeType = ShapeType.Circle;
        Geometry = new Geometry2D.Shapes.Circle(center, radius);
    }

    public override void Draw(PrimitiveBatch primitiveBatch)
    {
        // X and Y are the origin of the circle when rendering. The TransformMatrix
        // supplies the position in which it will be translated to before rendering.
        primitiveBatch.DrawCircleFill(0, 0, Geometry.Radius, points, FillColor, Transform2.TransformMatrix);

        // Vector2 va = Vector2.Transform(Vector2.Zero, Transform2.VertexTransformMatrix);
        // Vector2 vb = Vector2.Transform(new Vector2(Transform2.Radius, 0f), Transform2.VertexTransformMatrix);
        //
        // primitiveBatch.DrawLine(va, vb, 1f, Color.White);

        if (HasOutline)
        {
            primitiveBatch.DrawCircle(0, 0, Geometry.Radius, points, OutlineThickness, OutlineColor, Transform2?.TransformMatrix ?? Matrix.Identity);
        }

    }
}