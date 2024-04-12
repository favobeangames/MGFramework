using System;
using System.Diagnostics;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using FavobeanGames.MGFramework.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.MGFramework.Components;

/**
 * Entity is the base class for all game components.
 */
public class Entity
{
    /// <summary>
    /// Id value for the Entity
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Graphic object for the entity
    /// </summary>
    public Graphic Graphic { get; protected set; }

    /// <summary>
    /// Reference to the body in the physics world
    /// </summary>
    public RigidBody Body { get; set; }

    /// <summary>
    /// Position of the entity
    /// </summary>
    public virtual Vector2 Position { get; set; }

    private Vector2 velocity = Vector2.Zero;
    public Vector2 Velocity => Body?.LinearVelocity ?? velocity;

    private int layerDepth = 0;
    /// <summary>
    /// Determines where to draw the entity on the screen
    /// Lower = Rendered below other graphics
    /// Higher = Rendered above other graphics
    /// </summary>
    public int LayerDepth
    {
        get => layerDepth;
        set
        {
            layerDepth = value;
            LayerDepthUpdated = true;
        }
    }

    public bool LayerDepthUpdated;

    /// <summary>
    /// Flag to determine if a re-draw for the entity is needed. Used to determine if the layers
    /// RenderTarget needs to re-render all entities.
    /// </summary>
    protected bool needsRedraw;

    public Entity()
    {
        Id = Guid.NewGuid();
    }

    public Entity(Graphic graphic, RigidBody body)
    {
        Id = Guid.NewGuid();
        Graphic = graphic;
        Body = body;
        if (Body != null)
        {
            Body.SetId(Id);
            Body.BodyCollided += OnCollidedWith;
        }
    }

    public Entity(Graphic graphic, int layerDepth)
    {
        Id = Guid.NewGuid();
        Graphic = graphic;
        LayerDepth = layerDepth;
    }

    public virtual void Update(GameTime gameTime)
    {
    }
    
    public virtual void Draw(SpriteBatch spriteBatch)
    {
    }

    public virtual void Draw(PrimitiveBatch primitiveBatch)
    {
    }

    public virtual void Draw(GraphicsBatch graphicsBatch)
    {
        graphicsBatch.Draw(Graphic);
    }

    private void OnCollidedWith(Manifold collision)
    {
        Debug.WriteLine($"Entity with Id: {Id} collided with Entity with Id: {collision.BodyB.Id}");
    }

    /// <summary>
    /// Updates graphic position
    /// </summary>
    /// <param name="newPosition">New position for the graphic</param>
    public virtual void UpdatePosition(Vector2 newPosition)
    {
    }
}