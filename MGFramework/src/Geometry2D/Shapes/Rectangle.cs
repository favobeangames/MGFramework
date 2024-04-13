namespace FavobeanGames.MGFramework.Geometry2D.Shapes;

public class Rectangle : Geometry
{
    /// <summary>
    /// Width of the rectangle
    /// </summary>
    public float Width;

    /// <summary>
    /// Height of the rectangle
    /// </summary>
    public float Height;

    public float Top => Y - Height / 2f;
    public float Bottom => Y + Height / 2f;
    public float Left => X - Width / 2f;
    public float Right => X + Width / 2f;

    public Rectangle(Transform2 transform2, float x, float y, float width, float height)
        : base(transform2)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;

        GeometryType = GeometryType.Rectangle;

        UpdateTransformedVertices();
        UpdateAabb();
    }
    public Rectangle(Transform2 transform2, float width, float height)
        : base(transform2)
    {
        Width = width;
        Height = height;

        GeometryType = GeometryType.Rectangle;

        UpdateTransformedVertices();
        UpdateAabb();
    }
}