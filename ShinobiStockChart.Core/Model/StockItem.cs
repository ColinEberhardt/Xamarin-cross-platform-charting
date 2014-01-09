using System;

namespace ShinobiStockChart.Core.Model
{
    // a data item that represents a single stock
    public class StockItem
    {
        public StockItem (string symbol)
        {
            Price = double.NaN;
            Symbol = symbol;
        }

        public string Symbol { get; private set; }

        public double Price { get; set; }

        public double Change { get; set; }
    }
}

