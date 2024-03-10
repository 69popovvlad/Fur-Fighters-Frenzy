using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Tasks.ThreadTrampolines
{
    public static partial class UnityTask
    {
        /// <summary>
        /// Launches the program execution flow onto main-thread.
        /// </summary>
        public static readonly MainThreadTrampoline MainThread = new MainThreadTrampoline();
        
        private static volatile int _mainThreadId;
        private static volatile int _initialised;
        
        private static SynchronizationContext _syncContext;
        private static TaskScheduler _taskScheduler;
        
        public static bool IsInitialised => _initialised == 1;

        public static bool IsMainThread
        {
            get
            {
                if (!IsInitialised)
                {
                    throw new InvalidOperationException("UnityTask is not initialized, call UnityTask.Init() first");
                }

                return Thread.CurrentThread.ManagedThreadId == _mainThreadId;
            }
        }

        public static void Initialize(SynchronizationContext context, TaskScheduler taskScheduler)
        {
            _syncContext = context ?? throw new ArgumentException(nameof(context));
            _taskScheduler = taskScheduler ?? throw new ArgumentException(nameof(context));
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
            _initialised = 1;
        }

        public static void RunOnMainThread(Action action)
        {
            if (!IsInitialised)
            {
                throw new InvalidOperationException("UnityTask is not initialized, call UnityTask.Init() first");
            }

            if (IsMainThread)
            {
                action.Invoke();
                return;
            }

            _syncContext.PostWithNotification(state =>
            {
                action.Invoke();
            }, null);
        }
        
        public sealed class MainThreadTrampoline : INotifyCompletion
        {
            public bool IsCompleted => false;

            public MainThreadTrampoline GetAwaiter() => this;

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
                RunOnMainThread(continuation);
            }
        }
    }
}