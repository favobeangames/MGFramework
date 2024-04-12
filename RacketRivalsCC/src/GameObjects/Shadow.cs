using FavobeanGames.MGFramework;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;

namespace RacketRivalsCC;

public class Shadow
{
    private readonly Color shadowColor = Color.Black * .5f;
    private readonly float maxDistance = 64;

    private readonly Transform2 transform2;
    private readonly GameObject gameObject;

    public Circle Graphic { get; private set; }
    private Vector2 shadowPosDiff;

    public Shadow(Transform2 transform2, GameObject gameObject)
    {
        this.transform2 = transform2;
        this.gameObject = gameObject;

        Initialize();
    }

    private void Initialize()
    {
        shadowPosDiff = new Vector2(0, gameObject.Height/2f + gameObject.HeightOffGround);
        // var newTrans = new Transform2(transform2.Position - shadowPosDiff, new Vector2(1, .5f), transform2.Rotation)
        // {
        //     Radius = GetShadowRadius()
        // };
        //
        // Graphic = new Circle(newTrans, 24, shadowColor);
    }

    public void Update()
    {
        // shadowPosDiff = new Vector2(0, gameObject.Height/2 + gameObject.HeightOffGround);
        //Graphic.Transform2.Radius = GetShadowRadius();
        Graphic.Transform2.Position = transform2.Position + gameObject.SpritePositionOffset - shadowPosDiff;
    }

    private float GetShadowRadius()
    {
        var diff = gameObject.HeightOffGround / maxDistance;
        return diff > 1 ? 1 : (gameObject.Width/2) * (1 - diff);
    }
}