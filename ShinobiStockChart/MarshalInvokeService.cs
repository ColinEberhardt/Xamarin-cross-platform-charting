using System;
using MonoTouch.Foundation;

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

