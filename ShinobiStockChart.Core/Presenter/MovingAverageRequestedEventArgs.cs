using System;

namespace ShinobiStockChart.Core.Presenter
{
    public class MovingAverageRequestedEventArgs : EventArgs
    {
        public MovingAverageRequestedEventArgs (int numberOfDays)
        {
            NumberOfDays = numberOfDays;
        }

        public int NumberOfDays { get; private set; }
    }
}

