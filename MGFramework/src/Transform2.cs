using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework;

/// <summary>
/// Stores transformation properties
/// </summary>
public class Transform2
{
    /// <summary>
    /// Event to subscribe to that will invoke any time a property of
    /// the transform is updated
    /// </summary>
    public event Action TransformUpdated;

    private float x;
    /// <summary>
    /// X coordinate of the position
    /// </summary>
    public float X
    {
        get => x;
        set
        {
            Position = new Vector2(value, Y);
            x = value;
        }
    }

    private float y;
    /// <summary>
    /// Y coordinate of the position
    /// </summary>
    public float Y
    {
        get => y;
        set
        {
            Position = new Vector2(X, value);
            y = value;
        }
    }

    private Vector2 position = Vector2.Zero;
    /// <summary>
    /// Pixel coordinates of the graphic. Defaults to the Top Left position of the
    /// graphic
    /// </summary>
    public Vector2 Position
    {
        get => position;
        set
        {
            position = value;
            TransformUpdated?.Invoke();
        }
    }

    private float rotation = 0f;
    /// <summary>
    /// Rotation angle in radians of graphic around the origin point.
    /// </summary>
    public float Rotation
    {
        get => rotation;
        set
        {
            rotation = value;
            TransformUpdated?.Invoke();
        }
    }

    private Vector2 scale = new Vector2(1, 1);
    /// <summary>
    /// X, Y scale factor for the graphic.
    /// Defaults to 100%, 100%.
    /// </summary>
    public Vector2 Scale
    {
        get => scale;
        set
        {
            scale = value;
            TransformUpdated?.Invoke();
        }
    }

    /// <summary>
    /// Transform matrix used to modify graphic on screen
    /// 3x2 Matrix as this is for 2D games.
    /// </summary>
    /// <returns>Matrix</returns>
    public Matrix TransformMatrix { get; private set; }

    /// <summary>
    /// Returns empty transform object
    /// </summary>
    public static readonly Transform2 Empty = new(Vector2.Zero, Vector2.Zero, 0f);

    public Transform2(Vector2 position, Vector2 scale, float rotation)
    {
        Position = position;
        Scale = scale;
        Rotation = rotation;

        UpdateTransformMatrix();
        TransformUpdated += UpdateTransformMatrix;
    }

    private void UpdateTransformMatrix()
    {
        TransformMatrix = Matrix2.CreateFrom(Position, Rotation, Scale);
    }
}