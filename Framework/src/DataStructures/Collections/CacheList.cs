using System.Collections.Generic;

namespace FavobeanGames.Framework.DataStructures.Collections
{
    /// <summary>
    ///     List that caches game objects stored for reuseability
    /// </summary>
    public class CacheList<T>
    {
        private readonly List<T> activeList;
        private readonly List<T> cache;

        public CacheList()
        {
            cache = new List<T>();
            activeList = new List<T>();
        }

        /// <summary>
        ///     Indexer declaration
        ///     Throws exception if index is out of range of activeList
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get => activeList[index];
            set => activeList[index] = value;
        }

        /// <summary>
        ///     Add item to the end of the list
        /// </summary>
        /// <param name="item">     </param>
        public void Add(T item)
        {
            activeList.Add(item);
        }

        /// <summary>
        ///     Removes item from list
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            activeList.Remove(item);
        }

        /// <summary>
        ///     Checks to see if any objects exist in cache
        /// </summary>
        /// <returns>bool</returns>
        public bool IsCacheEmpty()
        {
            return cache.Count == 0;
        }

        /// <summary>
        ///     Checks cache to see if any available objects exist.
        ///     Returns null if none exist
        /// </summary>
        /// <returns>T object</returns>
        public T GetAvailableCache()
        {
            return cache.Count > 0 ? cache[0] : default;
        }

        /// <summary>
        ///     Moves item from list to cache for reuse
        /// </summary>
        /// <param name="index"></param>
        public void MoveToCacheByIndex(int index)
        {
            if (activeList.Count - 1 >= index)
            {
                // Index exists in activeList
                cache.Add(activeList[index]);
                activeList.RemoveAt(index);
            }
        }

        /// <summary>
        ///     Removes all objects from cache
        /// </summary>
        public void ClearCache()
        {
            cache.RemoveAll(obj => true);
        }
    }
}