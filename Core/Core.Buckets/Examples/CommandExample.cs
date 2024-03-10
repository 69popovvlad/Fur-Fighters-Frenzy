namespace Core.Buckets.Examples
{
    public struct CommandExample: IBroadcast
    {
        public object SomeData;

        public CommandExample(object someData)
        {
            SomeData = someData;
        }
    }
}