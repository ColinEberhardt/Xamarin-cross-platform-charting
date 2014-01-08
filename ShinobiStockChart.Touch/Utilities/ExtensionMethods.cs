using System;
using MonoTouch.Foundation;

namespace ShinobiStockChart.Touch.Utilities
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts a DateTime to an NSDate
        /// </param>
        public static NSDate ToNSDate (this DateTime date)
        {
            return NSDate.FromTimeIntervalSinceReferenceDate ((date - (new DateTime (2001, 1, 1, 0, 0, 0))).TotalSeconds);
        }
    }
}

