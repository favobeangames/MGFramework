using System;
using System.Data.Common;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FavobeanGames.MGFramework.Graphics;

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
    /// Position of the entity
    /// </summary>
    public virtual Vector2 Position { get; set; }

    /// <summary>
    /// Flag to determine if a re-draw for the entity is needed. Used to determine if the layers
    /// RenderTarget needs to re-render all entities.
    /// </summary>
    protected bool needsRedraw;

    public Entity()
    {
        Id = Guid.NewGuid();
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

    /// <summary>
    /// Updates graphic position
    /// </summary>
    /// <param name="newPosition">New position for the graphic</param>
    public virtual void UpdatePosition(Vector2 newPosition)
    {

    }
}