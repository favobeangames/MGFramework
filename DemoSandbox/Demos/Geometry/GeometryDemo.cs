using System.Collections.Generic;
using System.Linq;
using FavobeanGames.MGFramework;
using FavobeanGames.MGFramework.Cameras;
using FavobeanGames.MGFramework.Geometry2D.Math;
using FavobeanGames.MGFramework.Geometry2D.Shapes;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using GameWindow = FavobeanGames.MGFramework.GameWindow;
using Transform2 = FavobeanGames.MGFramework.Transform2;
using PrimitiveLine = FavobeanGames.MGFramework.Graphics.Primitives.Line;

namespace DemoSandbox.Demos;


public class GeometryDemo : Demo
{
    private const int POINT_RADIUS = 5;

    private readonly Color lineColor = Color.White;
    private readonly Color nodeColor = Color.Red;
    private readonly Color nodeOutlineColor = Color.White;

    private Camera camera;
    private InputManager inputManager;

    private BitmapFont textFont;
    private List<Geometry> geometries;
    private List<GeomCollision> geomCollisions;
    private List<Graphic> graphics;

    private enum Geometries
    {
        Point,
        Line,
        Polygon
    }

    // State variables
    private bool drawingShape = false;
    private Geometries currentDrawingGeomType = Geometries.Line;
    private Geometry currentDrawingGeom;

    private bool isShapeActive = false;
    private int activeShapeIndex = -1;

    public override Demo Initialize(GameWindow gameWindow)
    {
        Title = "Geometry Demo";

        geometries = new List<Geometry>();
        geomCollisions = new List<GeomCollision>();
        graphics = new List<Graphic>();

        camera = new Camera(gameWindow, CameraOptions.OrthographicCameraOptions);
        inputManager = new InputManager(camera, gameWindow);

        camera.GetExtents(out var viewBounds);

        var titleTextObj = new Text(
            new Transform2(new Vector2(viewBounds.Left + 200, viewBounds.Bottom - 30), Vector2.One, 0),
            textFont,
            Title
        );
        var descriptionTextObj = new Text(
            new Transform2(new Vector2(viewBounds.Center.X, viewBounds.Bottom - 30), Vector2.One, 0),
            textFont,
            "Draw some lines!"
        );

        graphics.Add(titleTextObj);
        graphics.Add(descriptionTextObj);

        return this;
    }

    public override Demo LoadContent(ContentManager content)
    {
        textFont = content.Load<BitmapFont>("nirmalaui64");

        return this;
    }

    public override void Update(GameTime gameTime)
    {
        inputManager.Update(gameTime);
        var mousePosition = inputManager.GetMouseWorldPosition();
        mousePosition = mousePosition.Round(2);

        UpdateGeometryOnHover(mousePosition);

        if (inputManager.MouseLeftButtonPressed())
        {
            // If mouse is outside of the screen bounds when clicked we will just jgnore it
            camera.GetExtents(out var viewBounds);
            if (!viewBounds.Contains(mousePosition)) return;

            if (!drawingShape)
            {
                // TODO: Check if we clicked on an inactive shape and select it instead

                // Initialize new shape
                currentDrawingGeom = GetNewGeometry(mousePosition);
                drawingShape = true;
            }
            else
            {
                UpdateGeometry(mousePosition);
            }
        }

        CheckGeometryCollision();
    }

    private void UpdateGeometryOnHover(Vector2 position)
    {
        if (currentDrawingGeom is null) return;

        switch (currentDrawingGeomType)
        {
            case Geometries.Line:
                var vertices = currentDrawingGeom.Vertices;
                vertices[1] = position;
                currentDrawingGeom.UpdateVertices(vertices);
                break;
            case Geometries.Polygon:
                break;
            default:
                break;
        }
    }

    private void UpdateGeometry(Vector2 position)
    {
        switch (currentDrawingGeomType)
        {
            case Geometries.Line:
                drawingShape = false;
                currentDrawingGeom = null;
                graphics.Add(CreateNode(position));
                break;
            case Geometries.Polygon:
                break;
            default:
                break;
        }
    }

    private Geometry GetNewGeometry(Vector2 position)
    {
        var transform = new Transform2(position, Vector2.One, 0);
        switch (currentDrawingGeomType)
        {
            case Geometries.Line:
                var line = new Line(position, position);
                graphics.Add(new PrimitiveLine(line, 4f, lineColor));
                geometries.Add(line);
                graphics.Add(CreateNode(position));
                return line;
            case Geometries.Polygon:
                return new Polygon(position);
            default:
                return new Circle(transform, POINT_RADIUS);
        }
    }

    private void CheckGeometryCollision()
    {
        var collisions = new List<GeomCollision>();
        for (int i = 0; i < geometries.Count - 1; i++)
        {
            for (int j = i + 1; j < geometries.Count; j++)
            {
                if (i == j) continue;

                if (collisions.Find(c =>
                        c.Geometry1Index == i && c.Geometry2Index == j ||
                        c.Geometry2Index == i && c.Geometry1Index == j) != null)
                    continue;

                if (geometries[i].GeometryType == GeometryType.Line && geometries[j].GeometryType == GeometryType.Line)
                {
                    if (LineMath.LineSegmentIntersection(geometries[i] as Line, geometries[j] as Line, out var point))
                    {
                        if (!point.IsNaN())
                        {
                            var col = new GeomCollision(i, j);
                            col.CollisionPoints.Add(CreateNode(point));
                            collisions.Add(col);
                        }
                    }
                }
            }
        }

        geomCollisions = collisions;
    }

    private FavobeanGames.MGFramework.Graphics.Primitives.Circle CreateNode(Vector2 position)
    {
        var nodeTransform = new Transform2(position, Vector2.One, 0);
        var nodeGeom = new Circle(nodeTransform, 6f);
        return new FavobeanGames.MGFramework.Graphics.Primitives.Circle(nodeGeom, 24, nodeColor);
    }

    public override void Draw(GraphicsBatch graphicsBatch)
    {
        graphicsBatch.Begin(camera);

        graphics.ForEach(graphicsBatch.Draw);

        var collisionNodes = geomCollisions.SelectMany(c => c.CollisionPoints).ToList();
        collisionNodes.ForEach(graphicsBatch.Draw);

        graphicsBatch.End();
    }
}