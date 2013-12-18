using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ShinobiStockChart.Presenter;
using ShinobiStockChart.Android.Service;
using ShinobiStockChart.Model;

namespace ShinobiStockChart.Android
{

	[Activity (MainLauncher = true)]
	public class StockPriceListActivity : ListActivity, StockPriceListPresenter.View
	{

		#region View implementation
		public event EventHandler<StockSelectedEventArgs> StockSelected;
		public void SetStockPrices (List<StockItem> prices)
		{
			ListAdapter = new ArrayAdapter<StockItem> (this, global::Android.Resource.Layout.SimpleListItem1, prices);
		}
		#endregion

		private StockPriceListPresenter _presenter;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set up the correct properties on the application
			var app = ShinobiStockChartApplication.GetApplication (this);
			app.CurrentActivity = this;

			// Prepare the services
			var uiMarshal = new MarshalInvokeService (app);
			var appStatus = new AppStatusService ();
			var navigation = new NavigationService (app);

			// Create the presenter
			_presenter = new StockPriceListPresenter (appStatus, uiMarshal, navigation);
			_presenter.SetView (this);
			app.Presenter = _presenter;
		}

	}
}

