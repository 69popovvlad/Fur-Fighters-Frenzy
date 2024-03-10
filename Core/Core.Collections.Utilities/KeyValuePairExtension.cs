using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Core.Collections.Utilities
{
    public static class KeyValuePairExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Deconstruct<K, V>(this KeyValuePair<K, V> tuple, out K key, out V value)
        {
            key = tuple.Key;
            value = tuple.Value;
        }
    }
}
