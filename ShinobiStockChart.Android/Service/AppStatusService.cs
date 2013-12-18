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
using ShinobiStockChart.Presenter.Service;

namespace ShinobiStockChart.Android
{
	class AppStatusService : IAppStatusService
	{

		#region IAppStatusService implementation
		public bool NetworkActivityIndicatorVisible {
			set {

			}
		}
		#endregion
	}
}

