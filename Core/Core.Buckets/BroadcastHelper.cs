using Core.Utilities;

namespace Core.Buckets
{
    internal static class BroadcastHelper
    {
        internal static ushort GetKey<T>()
        {
            return typeof(T).FullName.GetStableHashU16();
        }
        
        private static ushort GetStableHashU16(this string txt)
        {
            var hash32 = txt.GetStableHashU32();
            return (ushort)((hash32 >> 16) ^ hash32);
        }
    }
}