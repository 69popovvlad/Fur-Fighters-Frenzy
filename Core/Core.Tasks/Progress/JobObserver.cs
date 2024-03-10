using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Ui;

namespace Core.Tasks.Progress
{
    public abstract class JobObserver<T>: IJobObserver where T: IJobObserver
    {
        public event Action<IJobObserver> Updated;

        private readonly Task _task;

        public Task Task => _task;
        
        public abstract string Description { get; }

        protected JobObserver(Task task)
        {
            _task = task;
        }

        public void Dispose()
        {
            _task?.Dispose();
        }

        public async void Run(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception exception)
            {
                exception.ShowException();
                throw;
            }
            
            if (!_task.IsCompleted)
            {
                await Task.Run(() => _task, token);
            }
            
            OnUpdated();

            if (_task.IsFaulted)
            {
                OnFailed();
            }
            else
            {
                OnCompleted();
            }
        }

        protected virtual void OnUpdated()
        {
            Updated?.Invoke(this);
        }
        
        protected virtual void OnCompleted()
        {
            /* Nothing to do */
        }
        
        protected virtual void OnFailed()
        {
            /* Nothing to do */
        }
    }
}