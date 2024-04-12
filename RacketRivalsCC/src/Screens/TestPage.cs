using System.Collections.Generic;
using System.Linq;
using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using RacketRivalsCC.Match;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Transform2 = FavobeanGames.MGFramework.Transform2;

namespace RacketRivalsCC.Screens;

public class TestPage : RRGameScreen
{
    public TestPage()
        :base(new RectangleF(0, 0, 300, 180), ScreenType.Match)
    {

    }

    public override void Initialize(GameWorld gameWorld)
    {

    }

    public override void UnloadScreen(GameWorld gameWorld)
    {
        base.UnloadScreen(gameWorld);
    }

    public override IEnumerable<int> LoadScreen(GameWorld gameWorld)
    {
        var entityCol = new Bag<Entity>();

        var courtTexture = Assets.Get("basicCourtV2");
        var textureWidth = courtTexture.Width;
        var textureHeight = courtTexture.Height;
        var frameHeight = textureHeight / 2;

        var court = gameWorld.CreateEntity();
        var origin = new Vector2(textureWidth / 2, frameHeight / 2);
        var scale = Vector2.One;
        // var courtTransform = new Transform2
        // {
        //     Position = Vector2.Zero,
        //     Scale = scale
        // };
        // var courtSprite = new Sprite(courtTexture, courtTransform, new Rectangle(0, 0, textureWidth, frameHeight), origin)
        // {
        //     LayerDepth = 0,
        //     Origin = origin
        // };
        // court.AttachComponent(courtTransform);
        //court.AttachComponent(courtSprite);
        entityCol.Add(court);

        var net = gameWorld.CreateEntity();
        var sourceRect = new Rectangle(74, frameHeight + 91, 152, 15);
        origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        // var netTransform = new Transform2
        // {
        //     Position = Bounds.Center - new Vector2(76, 15),
        //     Scale = scale,
        // };
        // var netCollisionTransform = new Transform2
        // {
        //     Position = Bounds.Center - new Vector2(0, 13),
        //     Scale = scale,
        //     Vertices = new []
        //     {
        //         new Vector2(-76, 2),
        //         new Vector2(76, 2),
        //         new Vector2(76, -2),
        //         new Vector2(-76, -2),
        //     }
        // };
        // var netObj = new GameObject(144, 12, netTransform, origin);
        // netObj.CreateGameObjectBody(net.Id, netCollisionTransform, net, true);
        // var courtNetSprite = new Sprite(courtTexture, netTransform, sourceRect, origin)
        // {
        //     LayerDepth = 1,
        //     Origin = origin
        // };
        // net.AttachComponent(netTransform);
        // net.AttachComponent(courtNetSprite);
        // net.AttachComponent(netObj);
        // net.AttachComponent(new Polygon(netCollisionTransform, Color.Red * .5f));
        entityCol.Add(net);

        return entityCol.Select(e => e.Id);
    }
}