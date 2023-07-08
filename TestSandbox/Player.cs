using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using FavobeanGames.Components.Input;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace fb_framework_test_sandbox;

public class Player : Entity
{
    private readonly InputManager inputManager;

    private static readonly Vector2[] vertices =
    {
        new Vector2(10, 0),
        new Vector2(-10, -10),
        new Vector2(-5, -3),
        new Vector2(-5, 3),
        new Vector2(-10, 10)
    };

    private static readonly int thickness = 1;
    private static readonly Color outlineColor = Color.Black;
    private static readonly Color color = Color.White;

    private Polygon ship { get; }

    public override Vector2 Position
    {
        get => ship.Position;
        set => ship.Position = value;
    }

    public Player(InputManager inputManager)
    {
        this.inputManager = inputManager;
        ship = new Polygon(vertices, Color.Transparent, thickness, outlineColor);
        Graphic = ship;
    }

    public void RotateShip(float amount)
    {
        ship.Rotation += amount;

        if (ship.Rotation < 0f)
        {
            ship.Rotation += MathHelper.TwoPi;
        }

        if (ship.Rotation >= MathHelper.TwoPi)
        {
            ship.Rotation -= MathHelper.TwoPi;
        }
    }

    public void ApplyForceToShip(float amount)
    {
        Vector2 forceDir = new Vector2(MathF.Cos(ship.Rotation), -MathF.Sin(ship.Rotation));
        ship.Velocity += forceDir * amount;
    }

    public override void Update(GameTime gameTime)
    {
        float playerRotationAmount = MathHelper.Pi * (float) gameTime.ElapsedGameTime.TotalSeconds;
        if (inputManager.IsKeyHeld(Keys.A))
        {
            RotateShip(-playerRotationAmount);
        }
        if (inputManager.IsKeyHeld(Keys.D))
        {
            RotateShip(playerRotationAmount);
        }
        if (inputManager.IsKeyHeld(Keys.W))
        {
            ApplyForceToShip(50f * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        ship.Update(gameTime);
    }

    public override void Draw(PrimitiveBatch primitiveBatch)
    {
        ship.Draw(primitiveBatch);
    }
}