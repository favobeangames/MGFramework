using FavobeanGames.MGFramework.Geometry2D.Shapes;

namespace FavobeanGames.MGFramework.Geometry2D;

/// <summary>
/// Axis aligned bounding box
/// </summary>
public class AABB
{
    /// <summary>
    /// Bounds of the axis aligned bounding box
    /// </summary>
    public Rectangle Bounds;

    /// <summary>
    /// Reference of a Transform object for transforming
    /// of the AABB bounds
    /// </summary>
    public readonly Transform2 Transform2;

    public AABB(Transform2 transform2, float width, float height)
    {
        Bounds = new Rectangle(transform2, width, height);
        Transform2 = transform2;
    }
}