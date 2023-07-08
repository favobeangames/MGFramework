using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

public class Line : Primitive
{
    private float thickness;
    private Vector2 startPosition;
    private Vector2 endPosition;

    public Line()
    {
            
    }

    public Line(float thickness, Vector2 startPosition, Vector2 endPosition, Color fillColor)
        : base(fillColor)
    {
        this.thickness = thickness;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }

    public override void Draw(PrimitiveBatch primitiveBatch)
    {
        primitiveBatch.DrawLine(startPosition, endPosition, thickness, FillColor);
    }
}