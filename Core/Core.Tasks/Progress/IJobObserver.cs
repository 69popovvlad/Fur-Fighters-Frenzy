using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Tasks.Progress
{
    public interface IJobObserver: IDisposable
    {
        public event Action<IJobObserver> Updated;

        public Task Task { get; }
        
        public abstract string Description { get; }

        public void Run(CancellationToken token);
    }
}