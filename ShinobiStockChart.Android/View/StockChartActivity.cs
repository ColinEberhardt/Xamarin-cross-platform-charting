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
using ShinobiStockChart.Presenter;

namespace ShinobiStockChart.Android
{
	[Activity (Label = "StockChartActivity")]			
	public class StockChartActivity : Activity, StockChartPresenter.View
	{
		#region View implementation

		public void UpdateChartWithData (List<ChartDataPoint> data)
		{
			FindViewById<TextView> (Resource.Id.textView1).Text = "Data Received";
		}

		public string ChartTitle {
			set {

			}
		}

		#endregion

		private StockChartPresenter _presenter;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Prepare the view
			SetContentView (Resource.Layout.StockChartActivityLayout);

			// Manage the updates of the presenter and application
			var app = ShinobiStockChartApplication.GetApplication (this);
			app.CurrentActivity = this;
			_presenter = app.Presenter as StockChartPresenter;
			_presenter.SetView (this);

		}
	}
}

