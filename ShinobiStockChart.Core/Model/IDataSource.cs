using System;

namespace ShinobiStockChart.Core.Model
{
    public interface IDataSource
    {
        void FetchStockData (string symbol, Action<string> received);

        void FetchStockList (Action<string> received);
    }
}

