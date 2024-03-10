using System;
using System.Collections.Generic;

namespace Core.Buckets
{
    public class BroadcastHandler<T>: BroadcastHandlerBase where T: struct, IBroadcast
    {
        private readonly List<Action<T>> _subscribes = new List<Action<T>>();
        
        public override void Subscribe(object handler)
        {
            var castedHandler = (Action<T>)handler;
            _subscribes.Add(castedHandler);
        }

        public override void Unsubscribe(object handler)
        {
            var castedHandler = (Action<T>)handler;
            _subscribes.Remove(castedHandler);
        }

        public override void Invoke(in object packet)
        {
            var castedPacket = (T)packet;
            for (int i = 0, length = _subscribes.Count; i < length; ++i)
            {
                _subscribes[i].Invoke(castedPacket);
            }
        }
    }
}