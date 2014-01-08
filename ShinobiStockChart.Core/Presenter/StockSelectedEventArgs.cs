using System;
using ShinobiStockChart.Core.Model;

namespace ShinobiStockChart.Core.Presenter
{
    public class StockSelectedEventArgs : EventArgs
    {
        public StockSelectedEventArgs (StockItem item)
        {
            SelectedItem = item;
        }

        public StockItem SelectedItem { get; private set;}
    }
}

