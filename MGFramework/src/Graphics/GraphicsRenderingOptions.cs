using System;

namespace FavobeanGames.MGFramework.Graphics;

public enum RenderStyleOptions
{
    // Default will render the graphics based on their ordering of the collection.
    Default,
    // YCoordinateTopDown will render the graphics based on position from the top to the bottom of the screen,
    // or from max Y coordinate to min Y coordinate of the screen.
    YCoordinateTopDown,
    // YCoordinateTopDown will render the graphics based on position from the bottom to the top of the screen,
    // or from min Y coordinate to max Y coordinate of the screen.
    YCoordinateBottomUp,
}

/**
 * Options containing how the GraphicsManager should render graphics
 * to the render target.
 */
public class GraphicsRenderingOptions
{
    // Flag to determine if the coordinate based drawing option should use
    // the Origin position of the graphics. If set to false, it will use its position.
    public bool UseOriginPosition { get; }
    public RenderStyleOptions RenderStyleOption { get; }

    public GraphicsRenderingOptions()
    {
        
    }

    public GraphicsRenderingOptions(bool useOriginPosition, RenderStyleOptions renderStyle)
    {
        UseOriginPosition = useOriginPosition;
        RenderStyleOption = renderStyle;
    }

    // Default rendering options for layers.
    public static readonly GraphicsRenderingOptions DefaultRenderingOptions = new GraphicsRenderingOptions(true, RenderStyleOptions.Default);
}