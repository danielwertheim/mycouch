using System;
using System.Collections.Generic;
using EnsureThat;

namespace MyCouch
{
    internal class MyObservable<T> : IObservable<T>
    {
        private readonly List<IObserver<T>> _observers;

        public MyObservable()
        {
            _observers = new List<IObserver<T>>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            EnsureArg.IsNotNull(observer, nameof(observer));

            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new Unsubscriber(_observers, observer);
        }

        public virtual void Notify(T value)
        {
            foreach (var observer in _observers)
                observer.OnNext(value);
        }

        public virtual void Complete()
        {
            var obs = _observers.ToArray();
            _observers.Clear();

            foreach (var observer in obs)
                observer.OnCompleted();
        }

        public virtual void Error(Exception error)
        {
            var obs = _observers.ToArray();
            _observers.Clear();

            foreach (var observer in obs)
                observer.OnError(error);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly IList<IObserver<T>> _observers;
            private readonly IObserver<T> _observer;

            public Unsubscriber(IList<IObserver<T>> observers, IObserver<T> observer)
            {
                EnsureArg.HasItems(observers, nameof(observers));
                EnsureArg.IsNotNull(observer, nameof(observer));

                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}