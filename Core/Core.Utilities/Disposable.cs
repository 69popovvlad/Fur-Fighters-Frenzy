using System;
using System.Threading;

namespace Core.Utilities
{
    public sealed class Disposable: IDisposable
    {
        private long _disposed;
        private readonly DisposableDelegate _disposable;

        public delegate void DisposableDelegate(bool disposing);

        public Disposable(DisposableDelegate disposable)
        {
            _disposable = disposable ?? throw new ArgumentNullException(nameof(disposable));
        }

        ~Disposable()
        {
            if (Interlocked.Read(ref _disposed) == 1)
            {
                return;
            }

            _disposable(false);
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
            {
                return;
            }

            _disposable(true);
            GC.SuppressFinalize(this);
        }
    }
}
