using System;
using MonoTouch.UIKit;

namespace ShinobiStockChart
{
    public class AppStatusService : IAppStatusService
    {
        public bool NetworkActivityIndicatorVisible {
            set {
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = value;
            }
        }
    }
}

