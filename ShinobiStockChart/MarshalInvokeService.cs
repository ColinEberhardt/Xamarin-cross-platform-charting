using System;
using MonoTouch.Foundation;
using ShinobiStockChart.Core.Presenter.Service;

namespace ShinobiStockChart
{
    public class MarshalInvokeService : IMarshalInvokeService
    {
        private NSObject _obj = new NSObject();

        public MarshalInvokeService ()
        {
        }

        public void Invoke (Action action)
        {
            _obj.InvokeOnMainThread(() => action());
        }
    }
}

