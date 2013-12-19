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
		
		}
			
	}
}

