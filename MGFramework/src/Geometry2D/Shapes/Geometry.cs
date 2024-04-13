using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.Geometry2D.Shapes;

public enum GeometryType
{
    Line,
    Circle,
    Rectangle,
    Polygon
}

public class Geometry
{
    /// <summary>
    /// Type of geometry (Line, circle, Rectangle, Polygon, etc.)
    /// </summary>
    public GeometryType GeometryType { get; protected set; }

    /// <summary>
    /// Transform object for the geometry.
    /// </summary>
    public Transform2 Transform2 { get; private set; }

    private float x;
    /// <summary>
    /// X coordinate of the position of the rectangle. Position will be
    /// the Center point of the rectangle
    /// </summary>
    public float X
    {
        get => Transform2 != null ? Transform2.Position.X : x;
        set
        {
            if (Transform2 != null)
            {
                Transform2.Position = new Vector2(value, Transform2.Position.Y);
            }

            x = value;
        }
    }

    private float y;
    /// <summary>
    /// Y coordinate of the position of the rectangle. Position will be
    /// the Center point of the rectangle
    /// </summary>
    public float Y
    {
        get => Transform2 != null ? Transform2.Position.Y : y;
        set
        {
            if (Transform2 != null)
            {
                Transform2.Position = new Vector2(Transform2.Position.X, value);
            }

            y = value;
        }
    }

    private float width;
    /// <summary>
    /// Width of the geometry
    /// </summary>
    public float Width
    {
        get => width * Transform2?.Scale.X ?? 1f;
        set => width = value;
    }

    private float height;
    /// <summary>
    /// Height of the geometry
    /// </summary>
    public float Height
    {
        get => height * Transform2?.Scale.Y ?? 1f;
        set => height = value;
    }

    private Vector2 position;
    /// <summary>
    /// Position of the geometry
    /// </summary>
    public Vector2 Position
    {
        get => Transform2?.Position ?? position;
        set
        {
            if (Transform2 != null)
            {
                Transform2.Position = Position;
            }
        }
    }

    private float radius = float.MinValue;
    /// <summary>
    /// Radius of the geometry
    /// Returns float.MinValue if left empty
    /// </summary>
    public float Radius
    {
        get => radius;
        set => radius = value;
    }

    /// <summary>
    /// Vertices for the geometry
    /// </summary>
    public Vector2[] Vertices { get; protected set; }

    /// <summary>
    /// Vertices of the geometry transformed into matrix space
    /// </summary>
    public Vector2[] TransformedVertices { get; protected set; }

    /// <summary>
    /// Axis aligned bounding box of the geometry
    /// </summary>
    public RectangleF Aabb { get; private set; }

    protected Geometry() { }
    protected Geometry(Transform2 transform2)
    {
        SetTransform(transform2);
    }

    /// <summary>
    /// Sets the transform object of the geometry
    /// </summary>
    /// <param name="transform2"></param>
    public void SetTransform(Transform2 transform2)
    {
        Transform2 = transform2;
        Transform2.TransformUpdated += UpdateTransformedVertices;
        Transform2.TransformUpdated += UpdateAabb;
    }

    /// <summary>
    /// Updates the TransformVertices property by the stored transform object
    /// </summary>
    protected void UpdateTransformedVertices()
    {
        if (Vertices is null || Transform2?.TransformMatrix is null)
            return;

        var transformedVerts = new Vector2[Vertices.Length];
        for (var i = 0; i < Vertices.Length; i++)
        {
            transformedVerts[i] = Vector2.Transform(Vertices[i], Transform2.TransformMatrix);
        }

        TransformedVertices = transformedVerts;
    }

    /// <summary>
    /// Updates the axis aligned bounding box of the geometry
    /// </summary>
    protected void UpdateAabb()
    {
        if (GeometryType == GeometryType.Circle)
        {
            Aabb = GetBoundingBoxForCircle();
        }
        else
        {
            Aabb = GetBoundingBoxForVertices();
        }
    }

    /// <summary>
    /// Calculates and updates the Axis aligned bounding box
    /// for a circle geometry
    /// </summary>
    /// <returns>Axis aligned bounding box</returns>
    private RectangleF GetBoundingBoxForCircle()
    {
        var pos = Transform2.Position;
        var minX = pos.X - Radius;
        var maxX = pos.X + Radius;
        var minY = pos.Y - Radius;
        var maxY = pos.Y + Radius;

        return new RectangleF(minX, minY, maxX - minX, maxY - minY);
    }

    /// <summary>
    /// Calculates and updates the Axis aligned bounding box
    /// for a geometry that contains vertices
    /// </summary>
    /// <returns>Axis aligned bounding box</returns>
    private RectangleF GetBoundingBoxForVertices()
    {
        var verts = TransformedVertices ?? Vertices;

        var minX = verts.Min(v => v.X);
        var minY = verts.Min(v => v.Y);
        var maxX = verts.Max(v => v.X);
        var maxY = verts.Max(v => v.Y);

        return new RectangleF(minX, minY, maxX - minX, maxY - minY);
    }
}