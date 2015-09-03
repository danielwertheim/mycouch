using System;
using System.Linq;
using MyCouch.Extensions;

namespace MyCouch
{
#if !PCL
    [Serializable]
#endif
    public class Row : Row<string, string>
    {
        public Row(string id, object key, string value, string includedDoc)
            : base(id, key, value, includedDoc)
        { }
    }

#if !PCL
    [Serializable]
#endif
    public class Row<TValue> : Row<TValue, string>
    {
        public Row(string id, object key, TValue value, string includedDoc)
            : base(id, key, value, includedDoc)
        { }
    }

#if !PCL
    [Serializable]
#endif
    public class Row<TValue, TIncludedDoc>
    {
        public string Id { get; private set; }
        public object Key { get; private set; }
        public TValue Value { get; private set; }
        public TIncludedDoc IncludedDoc { get; private set; }

        public Row(string id, object key, TValue value, TIncludedDoc includedDoc)
        {
            Id = id;
            Key = key;
            Value = value;
            IncludedDoc = includedDoc;
        }

        public object[] KeyAsArray()
        {
            return Key == null ? null : (object[])Key;
        }

        public T[] KeyAsArrayOf<T>()
        {
            return Key == null ? null : KeyAsArray().Select(e => (T)e).ToArray();
        }

        public string KeyAsString()
        {
            return Key == null ? null : Key.ToString();
        }

        public Guid? KeyAsGuid()
        {
            return Key == null
                ? null
                : (Guid?)Guid.Parse(Key.ToString());
        }

        public DateTime? KeyAsDateTime()
        {
            return Key == null
                ? null
                : (DateTime?)Key.ToString().AsDateTimeFromIso8601();
        }

        public int? KeyAsInt()
        {
            return Key == null
                ? null
                : (int?)int.Parse(Key.ToString(), MyCouchRuntime.FormatingCulture.NumberFormat);
        }

        public long? KeyAsLong()
        {
            return Key == null
                ? null
                : (long?)long.Parse(Key.ToString(), MyCouchRuntime.FormatingCulture.NumberFormat);
        }

        public double? KeyAsDouble()
        {
            return Key == null
                ? null
                : (double?)double.Parse(Key.ToString(), MyCouchRuntime.FormatingCulture.NumberFormat);
        }

        public decimal? KeyAsDecimal()
        {
            return Key == null
                ? null
                : (decimal?)decimal.Parse(Key.ToString(), MyCouchRuntime.FormatingCulture.NumberFormat);
        }
    }
}