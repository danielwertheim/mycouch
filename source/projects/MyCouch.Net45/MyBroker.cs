using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyCouch
{
    internal class MyBroker<T> : IEnumerable<T>
    {
        private readonly ConcurrentQueue<T> _state = new ConcurrentQueue<T>();

        protected bool IsFinalized { get; set; }

        internal void Push(T item)
        {
            if (IsFinalized)
                throw new InvalidOperationException("Can not push when marked as finalized.");

            _state.Enqueue(item);
        }

        internal void PushMany(IEnumerable<T> src)
        {
            foreach (var i in src)
                Push(i);
        }

        internal async Task PushManyAsync(IEnumerable<T> src)
        {
            await Task.Factory.StartNew(() => PushMany(src));
        }

        internal void MarkAsFinalized()
        {
            IsFinalized = true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            Restart:
            SpinWait.SpinUntil(() => _state.Any() || IsFinalized);

            while (_state.Any())
            {
                T item;
                if (_state.TryDequeue(out item))
                    yield return item;
            }

            if (_state.Any() || !IsFinalized)
                goto Restart;
        }
    }
}