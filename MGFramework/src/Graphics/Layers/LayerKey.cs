using System;

namespace FavobeanGames.MGFramework.Graphics;

/// <summary>
/// LayerKey is the string key value for the graphics layers level
/// </summary>
public class LayerKey : IEquatable<LayerKey>
{
    /// <summary>
    /// String value for the Layer Key
    /// <example>
    /// Foreground, Base, Background, UI, etc..
    /// </example>
    /// </summary>
    public readonly string Key;

    private static LayerKey baseMapKey;
    public static LayerKey BaseMapKey
    {
        get
        {
            if (baseMapKey == null)
            {
                baseMapKey = new LayerKey("BaseMap");
            }

            return baseMapKey;
        }
    }
    
    public LayerKey(string key)
    {
        Key = key ?? throw new ArgumentNullException("key", "Key is a required property");
    }

    /// <summary>
    /// Compares LayerKey Key to the param
    /// </summary>
    private bool IsEqual(string key)
    {
        return Key.Equals(key, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Override to support IEquatable Equals function
    /// </summary>
    public bool Equals(LayerKey other)
    {
        if (other is null) return false;

        return IsEqual(other.Key);
    }
}
