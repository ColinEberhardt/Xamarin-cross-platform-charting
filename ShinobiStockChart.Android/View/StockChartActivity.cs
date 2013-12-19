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
using Com.ShinobiControls.Charts;
using ShinobiStockChart.Android.Utilities;
using Android.Support.V4.App;

namespace ShinobiStockChart.Android
{
	[Activity (Label = "StockChartActivity")]			
	public class StockChartActivity : Activity, StockChartPresenter.View
	{
		#region View implementation

		public void UpdateChartWithData (List<ChartDataPoint> data)
		{
			var adapter = new SimpleDataAdapter ();
			adapter.AddAll (data);
			if(_priceSeries == null) {
				_priceSeries = new LineSeries ();
				_chart.AddSeries (_priceSeries);
			}
			_priceSeries.DataAdapter = new SimpleDataAdapter ();
			_priceSeries.DataAdapter.AddAll (data.Select ( dp => 
				new DataPoint (DateUtils.ConvertToJavaDate (dp.XValue), dp.YValue)).ToList ());
			_progressDialog.Dismiss ();
		}

		public string ChartTitle {
			set {
				_chartTitle = value;
				var symbolTextView = FindViewById<TextView> (Resource.Id.symbolTextView);
				if(symbolTextView != null) {
					symbolTextView.Text = _chartTitle;
				}
			}
		}

		#endregion

		private StockChartPresenter _presenter;
		private IShinobiChart _chart;
		private LineSeries _priceSeries;
		private String _chartTitle;
		private ProgressDialog _progressDialog;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Prepare the view
			SetContentView (Resource.Layout.StockChartActivityLayout);

			// Create and show the progress dialog
			_progressDialog = new ProgressDialog (this);
			_progressDialog.SetTitle ("Retrieving Data...");
			_progressDialog.SetMessage ("Please wait");
			_progressDialog.SetCancelable (false);
			_progressDialog.Show ();

			// Manage the updates of the presenter and application
			var app = ShinobiStockChartApplication.GetApplication (this);
			app.CurrentActivity = this;
			_presenter = app.Presenter as StockChartPresenter;
			_presenter.SetView (this);

			var chartFrag = FragmentManager.FindFragmentById<ChartFragment> (Resource.Id.chart);
			_chart = chartFrag.ShinobiChart;
			_chart.SetLicenseKey ("<PUT YOUR LICENSE KEY HERE>");
		
			_chart.XAxis = new DateTimeAxis ();
			_chart.XAxis.EnableGestures ();
			_chart.YAxis = new NumberAxis ();
			_chart.YAxis.EnableGestures ();

			// Set the title
			if (_chartTitle != null) {
				FindViewById<TextView> (Resource.Id.symbolTextView).Text = _chartTitle;
			}

			// Enable the home button
			ActionBar.SetDisplayHomeAsUpEnabled (true);
			ActionBar.SetHomeButtonEnabled (true);
			ActionBar.Title = _presenter.Title;
		
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if(item.ItemId == global::Android.Resource.Id.Home) {
				NavUtils.NavigateUpTo (this, new Intent(this, typeof(StockPriceListActivity)));
				return true;
			}
			return base.OnOptionsItemSelected(item);
		}
			
	}
}

