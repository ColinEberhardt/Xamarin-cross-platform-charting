using System;
using MonoTouch.UIKit;
using ShinobiStockChart.Core.Presenter.Service;

namespace ShinobiStockChart.Touch.Service
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

