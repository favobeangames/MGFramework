using System.Collections.Generic;
using System.Linq;
using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameWindow = FavobeanGames.MGFramework.GameWindow;
using Transform2 = FavobeanGames.MGFramework.Transform2;

namespace RacketRivalsCC.Systems;

public class RenderItem
{
    public readonly int Id;
    public readonly Graphic Graphic;
    public readonly GameObject GameObject;

    public int LayerDepth => Graphic.LayerDepth;

    public Vector2 Position => GameObject?.Position ?? Graphic.Transform2.Position;

    public RenderItem(int id, Graphic graphic, GameObject gameObject = null)
    {
        Id = id;
        Graphic = graphic;
        GameObject = gameObject;
    }
}
public class RenderSystem : BaseRenderSystem
{
    private ComponentMapper<GameObject> gameObjectMapper;

    private List<RenderItem> graphics;
    private SortedSet<int> layerDepths;

    // Stores render targets for sprites/text that have effects applied to it
    private Dictionary<int, RenderTarget2D> spriteTargets;

    private readonly ScreenSystem screenSystem;

    public RenderSystem(Game game, GameWindow gameWindow, GraphicsRenderingOptions graphicsRenderingOptions, ScreenSystem screenSystem)
        : base(game, gameWindow, graphicsRenderingOptions, Aspect.Any(typeof(Sprite), typeof(Polygon), typeof(Circle), typeof(GameObject)))
    {
        graphics = new List<RenderItem>();
        layerDepths = new SortedSet<int>();
        spriteTargets = new Dictionary<int, RenderTarget2D>();
        this.screenSystem = screenSystem;
    }

    protected override void Initialize(IComponentService componentService)
    {
        base.Initialize(componentService);
        gameObjectMapper = componentService.GetMapper<GameObject>();
    }

    public override void Draw()
    {
        graphicsDevice.Clear(Color.Black);

        var sorted = graphics
            .GroupBy(g => g.LayerDepth)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(gr => gr.Position.Y).ToList());

        var spritesWithShadersActive = graphics
            .Where(r => r.Graphic.ActiveEffect != null)
            .ToArray();

        if (spritesWithShadersActive.Length > 0)
            DrawSpriteToTarget(spritesWithShadersActive);

        SetWindow();

        foreach (var grouping in sorted)
        {
            // Break up the graphics into two lists to batch them separately
            var sprites = grouping.Value
                .Where(r => r.Graphic.GetType() == typeof(Sprite))
                .Select(i => i.Graphic)
                .ToList();

            var primitives = grouping.Value
                .Where(r => r.Graphic.GetType() == typeof(Shape) || r.Graphic.GetType().IsSubclassOf(typeof(Shape)))
                .Select(i => i.Graphic)
                .ToList();

            // Draw sprites
            graphicsBatch.SpriteBatchBegin(camera);

            foreach (var sprite in sprites)
            {
                graphicsBatch.Draw(sprite);
            }

            graphicsBatch.SpriteBatchEnd();

            // Draw primitives
            graphicsBatch.PrimitiveBatchBegin(camera);

            foreach (var primitive in primitives)
            {
                graphicsBatch.Draw(primitive);
            }

            graphicsBatch.PrimitiveBatchEnd();
        }

        UnSetWindow();
        PresentWindow();
    }

    protected override void OnEntityAdded(int entityId)
    {
        base.OnEntityAdded(entityId);

        UpdateAddedGraphics(entityId);
    }

    protected override void OnEntityUpdated(int entityId)
    {
        base.OnEntityUpdated(entityId);

        UpdateAddedGraphics(entityId);
    }

    protected override void OnEntityDestroyed(int entityId)
    {
        base.OnEntityDestroyed(entityId);

        var sprite = spriteMapper.Get(entityId);
        if (sprite != null)
        {
            var index = graphics.FindIndex(r => r.Graphic == sprite);
            if (index >= 0)
            {
                graphics.RemoveAt(index);
            }
        }

        var polygon = polygonMapper.Get(entityId);
        if (polygon != null)
        {
            var index = graphics.FindIndex(r => r.Graphic == polygon);
            if (index >= 0)
            {
                graphics.RemoveAt(index);
            }
        }

        var circle = circleMapper.Get(entityId);
        if (circle != null)
        {
            var index = graphics.FindIndex(r => r.Graphic == circle);
            if (index >= 0)
            {
                graphics.RemoveAt(index);
            }
        }

    }

    private void UpdateAddedGraphics(int entityId)
    {
        if (ActiveEntities[entityId] == null) return;

        var gameObject = gameObjectMapper.Get(entityId);
        var sprite = spriteMapper.Get(entityId);
        if (sprite != null && !graphics.Exists(ri => ri.Graphic == sprite))
        {
            graphics.Add(new RenderItem(entityId, sprite, gameObject));
            layerDepths.Add(sprite.LayerDepth);
        }

        var polygon = polygonMapper.Get(entityId);
        if (polygon != null && !graphics.Exists(ri => ri.Graphic == polygon))
        {
            graphics.Add(new RenderItem(entityId, polygon, gameObject));
            layerDepths.Add(polygon.LayerDepth);
        }

        var circle = circleMapper.Get(entityId);
        if (circle != null && !graphics.Exists(ri => ri.Graphic == circle))
        {
            graphics.Add(new RenderItem(entityId, circle, gameObject));
            layerDepths.Add(circle.LayerDepth);
        }

        graphics.Sort((g1, g2) => g1.LayerDepth < g2.LayerDepth ? 0 : 1);
    }

    private void DrawSpriteToTarget(params RenderItem[] sprites)
    {
        foreach (var item in sprites)
        {
            if (!spriteTargets.TryGetValue(item.Id, out var target))
            {
                // Should we add a buffer around the target for outlines and other effects?
                spriteTargets.Add(item.Id, new RenderTarget2D(graphicsDevice, item.Graphic.Width, item.Graphic.Height));
            }

            graphicsDevice.SetRenderTarget(target);
            graphicsDevice.Clear(Color.Transparent);

            graphicsBatch.SpriteBatchBegin(eff: item.Graphic.ActiveEffect);
            graphicsBatch.Draw(item.Graphic, Transform2.Empty);
            graphicsBatch.SpriteBatchEnd();

            graphicsDevice.SetRenderTarget(null);
        }
    }
}

