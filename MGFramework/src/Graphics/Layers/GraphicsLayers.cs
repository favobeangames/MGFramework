using System;
using System.Collections.Generic;
using System.Linq;
using FavobeanGames.MGFramework.Graphics.Primitives;
using MonoGame.Extended.Collections;

namespace FavobeanGames.MGFramework.Graphics;

/// <summary>
/// Manages the graphics layers.
/// </summary>
public class GraphicsLayers
{
    private Guid Id { get; }
    // Collection of graphics layers in rendering order.
    private KeyedCollection<LayerKey, GraphicsLayer> graphicsLayers;

    public GraphicsLayers()
    {
        Id = Guid.NewGuid();
        graphicsLayers = new KeyedCollection<LayerKey, GraphicsLayer>(e => e.Key);
    }

    /// <summary>
    /// Adds graphics layer to main collection of layers.
    /// </summary>
    /// <param name="layer">GraphicsLayer to be added to the collection</param>
    public void AddLayer(GraphicsLayer layer)
    {
        graphicsLayers.Add(layer);
    }

    /// <summary>
    /// Adds graphics layer to main collection of layers.
    /// </summary>
    /// <param name="layer">GraphicsLayer to be added to the collection</param>
    /// <param name="graphics">Graphics to be added to the layer</param>
    public void AddLayer(LayerKey key, params Graphic[] graphics)
    {
        graphicsLayers.Add(new GraphicsLayer(key));
        AddGraphicsToLayer(key, graphics);
    }

    /// <summary>
    /// Clears all GraphicsLayers from main collection
    /// </summary>
    public void ClearLayers()
    {
        graphicsLayers.Clear();
    }

    /// <summary>
    /// Returns all graphics layers in rendering order
    /// </summary>
    /// <returns>List of GraphicsLayers</returns>
    public List<GraphicsLayer> GetLayers()
    {
        return graphicsLayers.Values.ToList();
    }

    /// <summary>
    /// Adds graphics to the layer
    /// </summary>
    /// <param name="key">LayerKey associated to layer</param>
    /// <param name="graphics">Collection of graphics to add to the layer</param>
    /// <returns>Boolean indicating if graphics were added successfully</returns>
    public bool AddGraphicsToLayer(LayerKey key, params Graphic[] graphics)
    {
        GraphicsLayer layer = GetLayerByKey(key);
        if (layer != null)
        {
            foreach (var baseGraphic in graphics)
            {
                layer.Graphics.Add(baseGraphic);
                baseGraphic.LayerData = new LayerData(Id, key);
            }
        }

        return false;
    }

    public bool RemoveGraphicsFromLayer(LayerKey key, params Graphic[] graphics)
    {
        GraphicsLayer layer = GetLayerByKey(key);
        if (layer != null)
        {
            foreach (var baseGraphic in graphics)
            {
                layer.Graphics.Remove(baseGraphic);
                baseGraphic.LayerData = null;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns GraphicsLayer by Key associated to it
    /// </summary>
    /// <param name="key">LayerKey associated to layer</param>
    /// <returns>GraphicsLayer associated to LayerKey. Returns null if one doesn't exist for key.</returns>
    private GraphicsLayer GetLayerByKey(LayerKey key)
    {
        graphicsLayers.TryGetValue(key, out GraphicsLayer layer);
        return layer;
    }
}