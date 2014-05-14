using System.Collections;
using System.Collections.Generic;
using EnsureThat;

namespace MyCouch.Net
{
    public class UrlParams : IEnumerable<UrlParam>
    {
        protected List<UrlParam> State { get; private set; }

        public UrlParams()
        {
            State = new List<UrlParam>();
        }

        public virtual void Add(UrlParam urlParam)
        {
            Ensure.That(urlParam, "urlParam").IsNotNull();

            State.Add(urlParam);
        }

        public virtual void AddRequired(string key, string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Add(new UrlParam(key, value));
        }

        public virtual void AddIfTrue(string key, bool? value)
        {
            AddIfTrue(key, value, "true");
        }

        public virtual void AddIfTrue(string key, bool? expression, string value)
        {
            if (expression.HasValue == false || expression.Value == false)
                return;

            Add(new UrlParam(key, value));
        }

        public virtual void AddIfNotNullOrWhiteSpace(string key, string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                return;

            Add(new UrlParam(key, value));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<UrlParam> GetEnumerator()
        {
            return State.GetEnumerator();
        }
    }
}