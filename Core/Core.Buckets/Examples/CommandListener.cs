namespace Core.Buckets.Examples
{
    public class CommandListener
    {
        public void Initialize()
        {
            var bucketExample = new BucketExample(); // Get it from Ioc instead, cause it's an IApplicationResource
            
            bucketExample.Subscribe<CommandExample>(OnCommandExample);
        }

        private void OnCommandExample(CommandExample command)
        {
            // do smth with command
        }
    }
}