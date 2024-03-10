namespace Core.Buckets.Examples
{
    public class CommandInvoker
    {
        public void SendCommand()
        {
            var bucketExample = new BucketExample(); // Get it from Ioc instead, cause it's an IApplicationResource
            
            var commandExample = new CommandExample(null);
            bucketExample.Invoke(commandExample);
        }
    }
}