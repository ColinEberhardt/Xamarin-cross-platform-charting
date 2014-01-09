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

namespace ShinobiStockChart.Droid.Service
{
    class MarshalInvokeService : IMarshalInvokeService
    {
        private ShinobiStockChartApplication _application;

        public MarshalInvokeService (ShinobiStockChartApplication application)
        {
            _application = application;
        }

        #region IMarshalInvokeService implementation

        public void Invoke (Action action)
        {
            _application.CurrentActivity.RunOnUiThread (() => action ());
        }

        #endregion
    }
}

