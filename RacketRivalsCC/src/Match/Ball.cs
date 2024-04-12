using System;
using Microsoft.Xna.Framework;

namespace RacketRivalsCC.Match;

public class Ball
{
    private int minHeight = 0;
    private int maxHeight = 250;

    private float maxSpin = 50;
    private float maxSpeed = 250;

    public int Height;
    public float Spin;
    public float Speed;
    public Vector2 Direction;
    public Vector2 Velocity => Direction * Speed;

    /// <summary>
    /// Flag to set when the ball is in active play
    /// </summary>
    public bool InPlay;
}