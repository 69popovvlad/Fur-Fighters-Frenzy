namespace Core.Buckets
{
    public abstract class BroadcastHandlerBase
    {
        public abstract void Subscribe(object handler);
        
        public abstract void Unsubscribe(object handler);
        
        public abstract void Invoke(in object packet);
    }
}