using System;
using MonoTouch.UIKit;
using ShinobiStockChart.Presenter.Service;

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

