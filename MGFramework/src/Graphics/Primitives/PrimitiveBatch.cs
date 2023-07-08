using System;
using FavobeanGames.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace FavobeanGames.MGFramework.Graphics.Primitives;

/// <summary>
/// Graphic batcher class to render primitive graphics in batches
/// to the screen
/// </summary>
public class PrimitiveBatch : IDisposable
{
    private const float MinLineThickness = 2f;
    private const float MaxLineThickness = 10f;

    private const int RectangleVertexCount = 4;
    private const int RectangleIndexCount = 6;

    private const int TriangleVertexCount = 3;
    private const int TriangleIndexCount = 3;

    private const int MinCirclePoints = 3;
    private const int MaxCirclePoints = 256;

    private bool isDisposed;
    private Game game;
    private BasicEffect effect;

    private VertexPositionColor[] vertices;
    private int[] indices;
    
    private int shapeCount;
    private int vertexCount;
    private int indexCount;

    private bool isStarted;

    /// <summary>
    /// PrimitiveBatch handles batch drawing of primitive graphics to the screen
    /// </summary>
    /// <param name="game">MonoGame Game instance</param>
    /// <exception cref="ArgumentNullException">Throws exception if Game parameter is null</exception>
    public PrimitiveBatch(Game game)
    {
        isDisposed = false;
        this.game = game ?? throw new ArgumentNullException("game");
        
        // Base Effect for the Primitive Batcher
        effect = new BasicEffect(game.GraphicsDevice);
        effect.TextureEnabled = false;
        effect.Texture = null;
        effect.FogEnabled = false;
        effect.LightingEnabled = false;
        effect.VertexColorEnabled = true;
        effect.World = Matrix.Identity;
        effect.View = Matrix.Identity;
        effect.Projection = Matrix.Identity;

        const int maxVertexCount = 1024;
        const int maxIndexCount = maxVertexCount * 3;

        vertices = new VertexPositionColor[maxVertexCount];
        indices = new int[maxIndexCount];

        shapeCount = 0;
        vertexCount = 0;
        indexCount = 0;

        isStarted = false;
    }

    public void Dispose()
    {
        if (isDisposed) return;
        
        effect?.Dispose();
        isDisposed = true;
    }

    public void Begin(Camera camera)
    {
        if (isStarted)
        {
            throw new Exception("primitive batching is already started.");
        }

        if (camera is null)
        {
            Viewport vp = game.GraphicsDevice.Viewport;
            effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, 0, vp.Height, 0, 1f);
            effect.View = Matrix.Identity;
        }
        else
        {
            camera.UpdateMatrices();
            effect.Projection = camera.Projection;
            effect.View = camera.View;
        }

        isStarted = true;
    }

    public void End()
    {
        Flush();
        isStarted = false;
    }

    public void Flush()
    {
        if (shapeCount == 0) return;
        
        EnsureStarted();

        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            
            game.GraphicsDevice.DrawUserIndexedPrimitives(
                PrimitiveType.TriangleList,
                vertices,
                0,
                vertexCount,
                indices,
                0,
                indexCount / 3
                );
        }

        shapeCount = 0;
        vertexCount = 0;
        indexCount = 0;
    }

    private void EnsureStarted()
    {
        if (!isStarted)
        {
            throw new Exception("primitive batching was never started.");
        }
    }

    /// <summary>
    /// Check to see if there are enough available vertex and indices available
    /// to render to the screen
    /// </summary>
    /// <param name="shapeVertexCount"></param>
    /// <param name="shapeIndexCount"></param>
    /// <exception cref="Exception"></exception>
    private void EnsureSpace(int shapeVertexCount, int shapeIndexCount)
    {
        if (shapeVertexCount > vertices.Length)
        {
            throw new Exception("Maximum shape vertex count is: " + vertices.Length);
        }
        
        if (shapeIndexCount > indices.Length)
        {
            throw new Exception("Maximum shape index count is: " + indices.Length);
        }
        
        if (vertexCount + shapeVertexCount > vertices.Length ||
            indexCount + shapeIndexCount > indices.Length)
        {
            Flush();
        }
    }

    public void DrawRectangle(float x, float y, float width, float height, Color color)
    {
        EnsureStarted();
        EnsureSpace(RectangleVertexCount, RectangleIndexCount);

        float left = x;
        float right = x + width;
        float top = y + height;
        float bottom = y;

        indices[indexCount++] = 0 + vertexCount;
        indices[indexCount++] = 1 + vertexCount;
        indices[indexCount++] = 2 + vertexCount;
        indices[indexCount++] = 0 + vertexCount;
        indices[indexCount++] = 2 + vertexCount;
        indices[indexCount++] = 3 + vertexCount;
        
        vertices[vertexCount++] = new VertexPositionColor(new Vector3(left, top, 0f), color);
        vertices[vertexCount++] = new VertexPositionColor(new Vector3(right, top, 0f), color);
        vertices[vertexCount++] = new VertexPositionColor(new Vector3(right, bottom, 0f), color);
        vertices[vertexCount++] = new VertexPositionColor(new Vector3(left, bottom, 0f), color);

        shapeCount++;
    }

    public void DrawLine(Vector2 start, Vector2 end, float thickness, Color color)
    {
        DrawLine(start.X, start.Y, end.X, end.Y, thickness, color);
    }
    
    public void DrawLine(float startX, float startY, float endX, float endY, float thickness, Color color)
    {
        EnsureStarted();
        EnsureSpace(RectangleVertexCount, RectangleIndexCount);
        
        thickness = System.Math.Clamp(thickness, MinLineThickness, MaxLineThickness);
        
        float halfThickness = thickness / 2;

        float e1x = endX - startX;
        float e1y = endY - startY;
        
        // Normalize coordinates to create the thickness geometry for the line
        Utils.Normalize(ref e1x, ref e1y);

        e1x *= halfThickness;
        e1y *= halfThickness;

        float e2x = -e1x;
        float e2y = -e1y;

        float n1x = -e1y;
        float n1y = e1x;

        float n2x = -n1x;
        float n2y = -n1y;

        // Creating the "line" rectangles coordinates based on the thickness and the
        // start and end point of the line.
        float q1x = startX + n1x + e2x;
        float q1y = startY + n1y + e2y;

        float q2x = endX + n1x + e1x;
        float q2y = endY + n1y + e1y;

        float q3x = endX + n2x + e1x;
        float q3y = endY + n2y + e1y;

        float q4x = startX + n2x + e2x;
        float q4y = startY + n2y + e2y;

        indices[indexCount++] = 0 + vertexCount;
        indices[indexCount++] = 1 + vertexCount;
        indices[indexCount++] = 2 + vertexCount;
        indices[indexCount++] = 0 + vertexCount;
        indices[indexCount++] = 2 + vertexCount;
        indices[indexCount++] = 3 + vertexCount;
        
        vertices[vertexCount++] = new VertexPositionColor(new Vector3(q1x, q1y, 0f), color);
        vertices[vertexCount++] = new VertexPositionColor(new Vector3(q2x, q2y, 0f), color);
        vertices[vertexCount++] = new VertexPositionColor(new Vector3(q3x, q3y, 0f), color);
        vertices[vertexCount++] = new VertexPositionColor(new Vector3(q4x, q4y, 0f), color);

        shapeCount++;
    }

    public void DrawPolygon(Vector2[] polyVertices, float thickness, Color color, Matrix transform)
    {
        for (int i = 0; i < polyVertices.Length; i++)
        {
            Vector2 a = polyVertices[i];
            Vector2 b = polyVertices[(i + 1) % polyVertices.Length];

            a = Vector2.Transform(a, transform);
            b = Vector2.Transform(b, transform);

            DrawLine(a, b, thickness, color);
        }
    }

    public void DrawPolygonFill(Vector2[] polyVertices, int[] triangleIndices, Color color, Matrix transform)
    {
        #if DEBUG
        if (polyVertices is null)
        {
            throw new ArgumentNullException("polyVertices");
        }

        if (triangleIndices is null)
        {
            throw new ArgumentNullException("triangleIndices");
        }
        #endif

        EnsureStarted();
        EnsureSpace(polyVertices.Length, triangleIndices.Length);

        for (int i = 0; i < triangleIndices.Length; i++)
        {
            indices[indexCount++] = triangleIndices[i] + vertexCount;
        }

        for (int i = 0; i < polyVertices.Length; i++)
        {
            Vector2 vertexA = Vector2.Transform(polyVertices[i], transform);

            vertices[vertexCount++] = new VertexPositionColor(new Vector3(vertexA, 0), color);
        }

        shapeCount++;
    }

    public void DrawCircle(float x, float y, float radius, int points, float thickness, Color color, Matrix transform)
    {
        points = System.Math.Clamp(points, MinCirclePoints, MaxCirclePoints);

        float rotation = MathHelper.TwoPi / points;

        float sin = MathF.Sin(rotation);
        float cos = MathF.Cos(rotation);

        float ax = radius;
        float ay = 0f;

        float bx = 0f;
        float by = 0f;
        
        /*
            rotate vector (x1, y1) counterclockwise by the given angle
            (angle in radians)

            newX = oldX * cos(angle) - oldY * sin(angle)
            newY = oldX * sin(angle) + oldY * cos(angle)
        */
        for (int i = 0; i < points; i++)
        {
            bx = cos * ax - sin * ay;
            by = sin * ax + cos * ay;

            Vector2 start = Vector2.Transform(new Vector2(ax + x, ay + y), transform);
            Vector2 end = Vector2.Transform(new Vector2(bx + x, by + y), transform);

            DrawLine(start, end, thickness, color);

            ax = bx;
            ay = by;
        }
    }

    public void DrawCircleFill(float x, float y, float radius, int points,  Color color, Matrix transform)
    {
        EnsureStarted();

        points = System.Math.Clamp(points, MinCirclePoints, MaxCirclePoints);
        int shapeTriangleCount = points - 2;
        int shapeIndexCount = shapeTriangleCount * 3;

        EnsureSpace(points, shapeIndexCount);

        for (int i = 0; i < shapeTriangleCount; i++)
        {
            indices[indexCount++] = 0 + vertexCount;
            indices[indexCount++] = i + 1 + vertexCount;
            indices[indexCount++] = i + 2 + vertexCount;
        }
        
        float rotation = MathHelper.TwoPi / points;

        float sin = MathF.Sin(rotation);
        float cos = MathF.Cos(rotation);

        float ax = radius;
        float ay = 0f;

        for (int i = 0; i < points; i++)
        {
            float x1 = ax;
            float y1 = ay;

            Vector2 vertex = Vector2.Transform(new Vector2(x1 + x, y1 + y), transform);

            vertices[vertexCount++] = new VertexPositionColor(new Vector3(vertex, 0f), color);

            ax = cos * x1 - sin * y1;
            ay = sin * x1 + cos * y1;
        }

        shapeCount++;
    }
}