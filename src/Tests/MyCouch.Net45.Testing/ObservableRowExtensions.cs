using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace MyCouch.Testing
{
    public static class ObservableRowExtensions
    {
        public static IList<Row> ToRowList(this IObservable<Row> ob)
        {
            var result = new List<Row>();

            ob.ForEachAsync((row, i) => result.Add(row)).Wait();

            return result;
        }

        public static IList<Row<TValue>> ToRowList<TValue>(this IObservable<Row<TValue>> ob)
        {
            var result = new List<Row<TValue>>();

            ob.ForEachAsync((row, i) => result.Add(row)).Wait();

            return result;
        }

        public static IList<Row<TValue, TIncludedDoc>> ToRowList<TValue, TIncludedDoc>(this IObservable<Row<TValue, TIncludedDoc>> ob)
        {
            var result = new List<Row<TValue, TIncludedDoc>>();

            ob.ForEachAsync((row, i) => result.Add(row)).Wait();

            return result;
        }
    }
}