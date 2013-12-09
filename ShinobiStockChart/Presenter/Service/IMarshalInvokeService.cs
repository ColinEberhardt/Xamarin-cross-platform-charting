using System;

namespace ShinobiStockChart
{
    /// <summary>
    /// A service which marshals incocations onto the UI thread
    /// </summary>
    public interface IMarshalInvokeService
    {
        void Invoke(Action action);
    }
}

