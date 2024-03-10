using System.Collections.Generic;

namespace Core.Utilities.Collections
{
    public static class SortedListExtensions
    {
        // Ensure you dont call Min Linq extension method.
        public static KeyValuePair<K, V> Min<K, V>(this SortedList<K, V> dict)
        {
            return new KeyValuePair<K, V>(dict.Keys[0], dict.Values[0]); // is O(1)
        }

        // Ensure you dont call Max Linq extension method.
        public static KeyValuePair<K, V> Max<K, V>(this SortedList<K, V> dict)
        {
            var index = dict.Count - 1; // O(1) again
            return new KeyValuePair<K, V>(dict.Keys[index], dict.Values[index]);
        }
    }
}