using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Tasks.ThreadTrampolines
{
    public static partial class UnityTask
    {
        public static ScheduledTaskAwaitable ScheduleAwait(this Task task) => new ScheduledTaskAwaitable(task);

        public static ScheduledTaskAwaitable<TResult> ScheduleAwait<TResult>(this Task<TResult> task) =>
            new ScheduledTaskAwaitable<TResult>(task);

        private static void ScheduleTaskContinuation(this Task task, Action continuation)
        {
            if (continuation == null)
            {
                throw new ArgumentException(nameof(continuation));
            }
            
            task.ContinueWith(scheduled => continuation(), CancellationToken.None,
                TaskContinuationOptions.DenyChildAttach, _taskScheduler);
        }

        private static void ScheduleTaskContinuation<TResult>(this Task<TResult> task, Action continuation)
        {
            if (continuation == null)
            {
                throw new ArgumentException(nameof(continuation));
            }
            
            task.ContinueWith(scheduled => continuation(), CancellationToken.None,
                TaskContinuationOptions.DenyChildAttach, _taskScheduler);
        }
        
        public readonly struct ScheduledTaskAwaitable
        { 
            private readonly ScheduledTaskAwaiter _awaiter;
            
            internal ScheduledTaskAwaitable(Task task) => _awaiter = new ScheduledTaskAwaiter(task);

            public ScheduledTaskAwaiter GetAwaiter() => _awaiter;
            
            public readonly struct ScheduledTaskAwaiter : ICriticalNotifyCompletion
            {
                private readonly Task _task;
            
                public bool IsCompleted => _task.IsCompleted;
                
                internal ScheduledTaskAwaiter(Task task)
                {
                    _task = task;
                }
                
                public void GetResult()
                {
                    _task.GetAwaiter().GetResult();
                }

                public void OnCompleted(Action continuation) => _task.ScheduleTaskContinuation(continuation);

                public void UnsafeOnCompleted(Action continuation) => _task.ScheduleTaskContinuation(continuation);
            }
        }

        public readonly struct ScheduledTaskAwaitable<TResult>
        {
            private readonly ScheduledTaskAwaiter _awaiter;

            internal ScheduledTaskAwaitable(Task<TResult> task) => _awaiter = new ScheduledTaskAwaiter(task);

            public ScheduledTaskAwaiter GetAwaiter() => _awaiter;
            
            public readonly struct ScheduledTaskAwaiter : ICriticalNotifyCompletion
            {
                private readonly Task<TResult> _task;

                public bool IsCompleted => _task.IsCompleted;
            
                internal ScheduledTaskAwaiter(Task<TResult> task)
                {
                    _task = task;
                }

                public TResult GetResult() => _task.GetAwaiter().GetResult();

                public void OnCompleted(Action continuation) => _task.ScheduleTaskContinuation(continuation);

                public void UnsafeOnCompleted(Action continuation) => _task.ScheduleTaskContinuation(continuation);
            }
        }
    }
}