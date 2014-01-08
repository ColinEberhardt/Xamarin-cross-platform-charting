using System;
using System.Collections.Generic;

namespace ShinobiStockChart.Core.Utilities
{
    public static class ExtensionMethods
    {
        // Perform a given function in a windowed manner on the sequence
        public static IEnumerable<T> Window<T>(this IEnumerable<T> sequence,
            int period, Func<IEnumerable<T>, T> windowOperation) {
            var windowSamples = new Queue<T> ();
            foreach (var item in sequence)
            {
                windowSamples.Enqueue (item);
                if(windowSamples.Count > period) {
                    windowSamples.Dequeue ();
                    yield return windowOperation (windowSamples);
                }
            }
        }
    }
}

