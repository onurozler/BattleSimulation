using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Util
{
    public static class CollectionHelper
    {
        public static T GetRandom<T>(this IList<T> list)
        {
            if (list.Count <= 0) return default;
            return list[Random.Range(0, list.Count)];
        }
        
        /// <summary>
        /// It returns different (and random) collection from passed key
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> GetOtherListRandomly<T>(this Dictionary<int,IList<T>> dictionary, int key)
        {
            var keyList = dictionary.Keys.ToList();
            keyList.Remove(key);
            
            var randomKey = keyList.GetRandom();
            return dictionary[randomKey];
        }
    }
}