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
			}
		}

		#endregion

		private StockChartPresenter _presenter;
		private IShinobiChart _chart;
		private LineSeries _priceSeries;

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
			_chart.SetLicenseKey ("<INSERT LICENSE KEY HERE>");
		
			_chart.XAxis = new DateTimeAxis ();
			_chart.YAxis = new NumberAxis ();
		
		}
			
	}
}

