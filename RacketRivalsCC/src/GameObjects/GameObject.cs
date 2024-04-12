using FavobeanGames.MGFramework;
using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Graphics.Primitives;
using FavobeanGames.MGFramework.Physics;
using Microsoft.Xna.Framework;

namespace RacketRivalsCC;

/// <summary>
/// Component to store details of the
/// </summary>
public class GameObject
{
    public int HeightOffGround { get; set; } = 0;

    private readonly int width = 0;
    public int Width
    {
        get => width;
        private init
        {
            width = value;
            if (transform2 != null)
                width = (int)(width * transform2.Scale.X);
        }
    }

    private readonly int height = 0;
    public int Height
    {
        get => height;
        private init
        {
            height = value;
            if (transform2 != null)
                height = (int)(height * transform2.Scale.Y);
        }
    }

    /// <summary>
    /// Position offset of the sprite to the ground point of the game object.
    /// This is the center of the "feet" of the object, or the point of the object
    /// that would rest on the floor
    /// </summary>
    public Vector2 SpritePositionOffset;

    private Vector2 bodyPositionOffset = Vector2.Zero;
    /// <summary>
    /// Positional offset between the Sprite and its body
    /// </summary>
    public Vector2 BodyPositionOffset
    {
        get => bodyPositionOffset - new Vector2(0, HeightOffGround);
        private set => bodyPositionOffset = value;
    }

    public bool HasBody;
    public bool HasShadow;

    private Shadow shadow;
    private RigidBody body;

    private Transform2 transform2;
    private Transform2 bodyTransform2;

    public Vector2 Position => bodyTransform2?.Position ?? transform2.Position - BodyPositionOffset;

    public GameObject()
    {
    }

    public GameObject(int width, int height, Transform2 transform2, Vector2? spritePositionOffset)
    {
        this.transform2 = transform2;
        Width = width;
        Height = height;
        HeightOffGround = 0;

        if (spritePositionOffset != null)
            SpritePositionOffset = spritePositionOffset.Value;
    }

    public void SetPosition(Vector2 newPos)
    {
        if (bodyTransform2 is not null)
        {
            bodyTransform2.Position = newPos;
            transform2.Position = newPos - SpritePositionOffset - BodyPositionOffset;
        }
        else
        {
            transform2.Position = newPos;
        }
    }

    public void CreateGameObjectBody(int id, Transform2 bodyTransform2, Entity entity, bool isStatic = false)
    {
        HasBody = true;
        this.bodyTransform2 = bodyTransform2;
        BodyPositionOffset = transform2.Position - bodyTransform2.Position;
        //var shapeType = bodyTransform2.Radius != float.MinValue ? ShapeType.Circle : ShapeType.Polygon;
        //body = new RigidBody(id, isStatic, 1f, 1f, 0f, 1f, shapeType, bodyTransform2);
        //body.BodyMovedTo += SetPosition;
        //entity.AttachComponent(body);
    }

    public void CreateGameObjectShadow(Transform2 transform2, Entity entity)
    {
        HasShadow = true;
        shadow = new Shadow(transform2, this);
        entity.AttachComponent(shadow.Graphic);
        entity.AttachComponent(shadow);
    }

}