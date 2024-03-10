using System.Threading;
using JetBrains.Annotations;

namespace Core.Tasks.ThreadTrampolines
{
    public static class SynchronizationContextExtensions
    {
        public static void PostWithNotification(this SynchronizationContext context,
            [NotNull] SendOrPostCallback callback, object state)
        {
            context.OperationStarted();
            context.Post(s =>
            {
                try
                {
                    callback(state);
                }
                finally
                {
                    context.OperationCompleted();
                }
            }, state);
        }
    }
}