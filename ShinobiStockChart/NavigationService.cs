using System;
using MonoTouch.UIKit;
using ShinobiStockChart.Presenter;
using ShinobiStockChart.Presenter.Service;

namespace ShinobiStockChart
{
    public class NavigationService : INavigationService
    {

        private UINavigationController _navigationController;

        public NavigationService (UINavigationController navigationController)
        {
            _navigationController = navigationController;
        }

        #region INavigationService implementation

        public void PushPresenter (object presenter)
        {
            if (presenter is StockPriceListPresenter)
            {
                var viewController = new StocksListViewController(presenter as StockPriceListPresenter);
                _navigationController.PushViewController(viewController, true);
            }

            if (presenter is StockChartPresenter)
            {
                var viewController = new StockChartViewController(presenter as StockChartPresenter);
                _navigationController.PushViewController(viewController, true);
            }
        }

        #endregion

    }
}

