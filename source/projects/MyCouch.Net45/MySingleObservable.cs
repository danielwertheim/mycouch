using System;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;

namespace MyCouch
{
    internal class MySingleObservable<T> : IObservable<T>
    {
        private IObserver<T> _observer;

        internal static MySingleObservable<T> Create(Action<Action<T>> c, CancellationToken cancellationToken, TaskFactory factory = null)
        {
            Ensure.That(c, "c").IsNotNull();

            var o = new MySingleObservable<T>();

            (factory ?? Task.Factory)
                .StartNew(() =>
                {
                    SpinWait.SpinUntil(() => o._observer != null);
                    c(o.Notify);
                }, cancellationToken)
                .ContinueWith(t => o.Complete(), cancellationToken);

            return o;
        }

        private MySingleObservable()
        {
            _observer = null;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if(_observer != null)
                throw new InvalidOperationException("This is a single observable, and there already is a subscribed observer.");

            _observer = observer;

            return new Unsubscriber(() => _observer = null);
        }

        protected virtual void Notify(T value)
        {
            if(_observer != null)
                _observer.OnNext(value);
        }

        protected virtual void Complete()
        {
            if (_observer != null)
                _observer.OnCompleted();
        }

        private class Unsubscriber : IDisposable
        {
            private readonly Action _unsub;

            public Unsubscriber(Action unsub)
            {
                _unsub = unsub;
            }

            public void Dispose()
            {
                if (_unsub != null)
                    _unsub();
            }
        }
    }
}