using System;
using System.Collections.Generic;
using Core.Application;

namespace Core.Buckets
{
    public abstract class BucketBase<T>: IApplicationResource
    {
        protected readonly Dictionary<ushort, BroadcastHandlerBase> Subscribes = new Dictionary<ushort, BroadcastHandlerBase>();

        public void Subscribe<T>(Action<T> readerDelegate) where T : struct, IBroadcast
        {
            var key = BroadcastHelper.GetKey<T>();
            if (!Subscribes.TryGetValue(key, out var broadcastHandler))
            {
                broadcastHandler = Subscribes[key] = new BroadcastHandler<T>();
            }
            
            broadcastHandler.Subscribe(readerDelegate);
        }

        public void Unsubscribe<T>(Action<T> readerDelegate) where T : struct, IBroadcast
        {
            var key = BroadcastHelper.GetKey<T>();
            if (Subscribes.TryGetValue(key, out var broadcastHandler))
            {
                broadcastHandler.Unsubscribe(readerDelegate);
            }
        }

        public void Invoke<K>(in K packet) where K: T
        {
            var key = BroadcastHelper.GetKey<K>();
            if (Subscribes.TryGetValue(key, out var broadcastHandler))
            {
                broadcastHandler.Invoke(packet);
            }
        }
    }
}