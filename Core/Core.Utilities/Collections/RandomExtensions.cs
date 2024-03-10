using UnityEngine;

namespace Core.Utilities.Collections
{
    public static class RandomExtensions
    {
        public static void Shuffle<T> (this T[] array)
        {
            var n = array.Length;
            while (n > 1) 
            {
                var k = Random.Range(0, n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
    }
}