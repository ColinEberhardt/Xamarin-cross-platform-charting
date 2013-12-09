using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ShinobiStockChart.Presenter;

namespace ShinobiStockChart
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow window;

        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            window = new UIWindow (UIScreen.MainScreen.Bounds);

            var marshalInvokeService = new MarshalInvokeService ();
            var appStatusService = new AppStatusService ();
            var stockPriceListPresenter = new StockPriceListPresenter (appStatusService, marshalInvokeService);
      
            var navController = new UINavigationController ();      
            var mainView = new StocksListViewController (stockPriceListPresenter);
            navController.PushViewController (mainView, false);
            navController.NavigationBar.TintColor = UIColor.DarkGray;
      
            window.RootViewController = navController;
            window.MakeKeyAndVisible ();
      
            return true;
        }
    }
}

