using System;
using System.Collections.Generic;
using MyCouch.EnsureThat;

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
            Ensure.That(observer, "observer").IsNotNull();

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

        private class Unsubscriber : IDisposable
        {
            private readonly IList<IObserver<T>> _observers;
            private readonly IObserver<T> _observer;

            public Unsubscriber(IList<IObserver<T>> observers, IObserver<T> observer)
            {
                Ensure.That(observers, "observers").HasItems();
                Ensure.That(observer, "observer").IsNotNull();

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