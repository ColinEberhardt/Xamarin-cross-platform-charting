using System;

namespace ShinobiStockChart.Core.Presenter.Service
{
    /// <summary>
    /// A service which marshals incocations onto the UI thread
    /// </summary>
    public interface IMarshalInvokeService
    {
        void Invoke(Action action);
    }
}

