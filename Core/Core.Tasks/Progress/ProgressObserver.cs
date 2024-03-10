using System;
using System.Collections.Generic;
using System.Threading;
using Core.Tasks.ThreadTrampolines;

namespace Core.Tasks.Progress
{
    public class ProgressObserver: IDisposable
    {
        public event Action Updated;
        public event Action Finished;
        
        private readonly HashSet<IJobObserver> _uniqueJobs = new HashSet<IJobObserver>();
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly object _lock = new object();
        
        private long _started;
        private long _disposed;
        private long _tasksCount;
        private long _finishedTasksCount;
        private IJobObserver _currentJob;

        public float Progress => _finishedTasksCount / (float)_tasksCount;

        public long TasksCount => Interlocked.Read(ref _tasksCount);
        
        public long FinishedTasksCount => Interlocked.Read(ref _finishedTasksCount);

        public string JobDescription => _currentJob.Description;

        public bool IsStarted => Interlocked.Read(ref _started) > 0;
        
        private bool IsCanceled => _cancellationTokenSource is { IsCancellationRequested: true };
        
        private CancellationToken Token => _cancellationTokenSource?.Token ?? CancellationToken.None;

        public ProgressObserver(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
        }

        ~ProgressObserver()
        {
            _cancellationTokenSource?.Cancel();
        }
        
        public void Run()
        {
            if (Interlocked.Exchange(ref _started, 1) > 0 || IsCanceled)
            {
                return;
            }
            
            foreach (var job in _uniqueJobs)
            {
                lock (_lock)
                {
                    _currentJob = job;
                }
                
                job.Run(Token);
            }
        }

        public void StopAll()
        {
            _cancellationTokenSource?.Cancel();
        }

        public void AddTaskRange(IEnumerable<IJobObserver> jobs)
        {
            foreach (var job in jobs)
            {
                AddTask(job);
            }
        }
        
        public void AddTask(IJobObserver job)
        {
            if (!_uniqueJobs.Add(job) || IsCanceled)
            {
                return;
            }
            
            Interlocked.Increment(ref _tasksCount);

            UnityTask.RunOnMainThread(() =>
            {
                job.Updated += OnProgressJobUpdated;
                Updated?.Invoke();

                if (Interlocked.Read(ref _started) == 1)
                {
                    job.Run(Token);
                }
            });
        }

        private void OnProgressJobUpdated(IJobObserver job)
        {
            if (job.Task.IsCompleted)
            {
                job.Updated -= OnProgressJobUpdated;
                Interlocked.Increment(ref _finishedTasksCount);

                if (job.Task.IsFaulted)
                {
                    Interlocked.Decrement(ref _tasksCount);
                }
                
                if (FinishedTasksCount >= TasksCount)
                {
                    Finished?.Invoke();
                    return;
                }
            }
            
            Updated?.Invoke();
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) > 0)
            {
                return;
            }

            GC.SuppressFinalize(this);
        }
    }
}