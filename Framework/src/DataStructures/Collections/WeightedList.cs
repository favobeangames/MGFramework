using System;
using System.Collections.Generic;

namespace FavobeanGames.Framework.DataStructures.Collections
{
    /// <summary>
    ///     List that allows for weights to be passed with objects added to collection.
    /// </summary>
    public class WeightedList<T> where T : IComparable<T>
    {
        private static Random random;

        private readonly List<WeightedObject> list;
        private int totalWeight;

        public WeightedList()
        {
            list = new List<WeightedObject>();
            random = new Random();
        }

        /// <summary>
        ///     Add item to list
        /// </summary>
        /// <param name="item"></param>
        /// <param name="weight"></param>
        public void Add(T item, int weight)
        {
            list.Add(new WeightedObject(item, weight));
            totalWeight += weight;
        }

        /// <summary>
        ///     Removes item from list.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            var foundIndex = list.FindIndex(obj => obj.Item.CompareTo(item) != -1);
            if (foundIndex != -1)
            {
                totalWeight -= list[foundIndex].Weight;
                list.RemoveAt(foundIndex);
            }
        }

        /// <summary>
        ///     TODO: Make this better. This solution is primitive and dumb. Be better!
        ///     Returns item randomly. Items with higher weighted value are more likely
        ///     to be picked.
        ///     Returns null if no items exist in list
        /// </summary>
        /// <returns>T generic item object</returns>
        public T GetRandomItem()
        {
            if (list.Count > 0)
            {
                var value = random.Next(0, totalWeight);
                var total = 0;
                foreach (var item in list)
                    if (value > total && value <= total + item.Weight)
                        return item.Item;
                    else
                        total += item.Weight;
            }

            return default;
        }

        /// <summary>
        ///     Private class to store the object passed into the collection.
        ///     Stores a weight to be used when randomly picking a stored item
        /// </summary>
        private class WeightedObject
        {
            public T Item;
            public readonly int Weight;

            public WeightedObject(T item, int weight)
            {
                Item = item;
                Weight = weight;
            }
        }
    }
}