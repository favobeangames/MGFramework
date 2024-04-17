using System.Drawing;
using FavobeanGames.MGFramework.Graphics;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.UI;

public class Backdrop : Graphic
{
    public RectangleF Bounds { get; private set; }

    public Backdrop()
        : base(GraphicType.Sprite)
    {

    }

    public override MonoGame.Extended.RectangleF GetAABB()
    {
        throw new System.NotImplementedException();
    }

    public override Vector2[] GetTransformedVertices()
    {
        throw new System.NotImplementedException();
    }
}