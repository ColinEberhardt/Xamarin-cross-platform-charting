using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ShinobiStockChart.Core.Presenter.Service;
using ShinobiStockChart.Core.Presenter;
using ShinobiStockChart.Droid.Views;

namespace ShinobiStockChart.Droid.Service
{
    class NavigationService : INavigationService
    {
        private ShinobiStockChartApplication _application;

        public NavigationService (ShinobiStockChartApplication application)
        {
            _application = application;
        }

        #region INavigationService implementation

        public void PushPresenter (object presenter)
        {
            object oldPresenter = _application.Presenter;
            if (presenter != oldPresenter) {
                _application.Presenter = presenter;
                Intent i = null;

                if (presenter is StockPriceListPresenter) {
                    i = new Intent (_application.CurrentActivity, typeof(StockPriceListActivity));
                } else if (presenter is StockChartPresenter) {
                    i = new Intent (_application.CurrentActivity, typeof(StockChartActivity));
                }

                if (i != null) {
                    _application.CurrentActivity.StartActivity (i);
                }
            }
        }

        #endregion
    }
}

