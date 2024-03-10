using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Tasks.ThreadTrampolines
{
    public static partial class UnityTask
    {
        /// <summary>
        /// Launches the program execution flow onto off-thread.
        /// </summary>
        public static readonly OffThreadTrampoline OffThread = new OffThreadTrampoline();

        public static Task Run(Action action)
        {
            return Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach,
                _taskScheduler);
        }

        public sealed class OffThreadTrampoline : INotifyCompletion
        {
            public bool IsCompleted => false;

            public OffThreadTrampoline GetAwaiter() => this;

            public void GetResult()
            {
                /* Nothing to do */
            }

            public void OnCompleted(Action continuation)
            {
                /*
                 * continuation delegate represents the rest of an async method, right below the line this instance
                 * is being awaited at; it is a courtesy of C# compiler.
                 */

                if (!IsMainThread)
                {
                    continuation();

                    return;
                }

                Run(continuation);
            }
        }
    }
}