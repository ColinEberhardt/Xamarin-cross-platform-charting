using System;
using ShinobiStockChart.Model;

namespace ShinobiStockChart.Presenter
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

