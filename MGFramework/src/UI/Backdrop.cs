using System.Drawing;
using FavobeanGames.MGFramework.Graphics;
using Microsoft.Xna.Framework;

namespace FavobeanGames.Components.UI;

public class Backdrop : Graphic
{
    public RectangleF Bounds { get; private set; }

    public Backdrop()
        : base(GraphicType.Sprite)
    {

    }
    
}