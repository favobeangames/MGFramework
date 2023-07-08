using System;

namespace FavobeanGames.MGFramework.Graphics;

/// <summary>
/// Object storing the layer data for where the Entity is rendered
/// </summary>
public class LayerData
{
    public Guid LayersId { get; private set; }
    public LayerKey LayerKey { get; private set; }

    public LayerData()
    {
        Clear();
    }
    public LayerData(Guid layersId, LayerKey key)
    {
        LayersId = layersId;
        LayerKey = key;
    }

    /// <summary>
    /// Nullifies all data stored for the LayerData
    /// </summary>
    public void Clear()
    {
        LayersId = Guid.Empty;
        LayerKey = null;
    }
}