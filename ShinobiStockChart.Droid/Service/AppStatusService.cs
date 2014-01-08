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
    class AppStatusService : IAppStatusService
    {

        private ProgressDialog _progressDialog;
        private ShinobiStockChartApplication _app;

        public AppStatusService (ShinobiStockChartApplication app)
        {
            _app = app;
        }
            

        #region IAppStatusService implementation

        public bool NetworkActivityIndicatorVisible {
            set {
                if(value && _progressDialog == null) {
                    _progressDialog = new ProgressDialog (_app.CurrentActivity);
                    _progressDialog.SetTitle ("Retrieving Data...");
                    _progressDialog.SetMessage ("Please wait");
                    _progressDialog.SetCancelable (false);
                    _progressDialog.Show ();
                } else {
                    if(_progressDialog != null) {
                        _progressDialog.Dismiss ();
                        _progressDialog = null;
                    }
                }
            }
        }

        #endregion
    }
}

