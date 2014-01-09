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
using ShinobiStockChart.Core.Presenter;
using Com.ShinobiControls.Charts;
using ShinobiStockChart.Droid.Utilities;
using Android.Support.V4.App;
using Android.Graphics;
using ShinobiStockChart.Core.Model;

namespace ShinobiStockChart.Droid.Views
{
    [Activity (Label = "StockChartActivity")]			
    public class StockChartActivity : Activity, StockChartPresenter.View
    {
        #region View implementation

        public void UpdateChartWithData (List<ChartDataPoint> data)
        {
            if (_priceSeries == null) {
                _priceSeries = new LineSeries ();
                // Set some styles
                _priceSeries.Style.AreaColor = Resources.GetColor (Resource.Color.chart_series1_area);
                _priceSeries.Style.AreaColorGradient = Resources.GetColor (Resource.Color.chart_series1_area_low);
                _priceSeries.Style.AreaLineColor = Resources.GetColor (Resource.Color.chart_series1_line);
                _priceSeries.Style.FillStyle = SeriesStyle.FillStyle.Gradient;
                _priceSeries.Style.AreaLineWidth = 1.5f;
                _chart.AddSeries (_priceSeries);
            }
            _priceSeries.DataAdapter = new SimpleDataAdapter ();
            _priceSeries.DataAdapter.AddAll (data.Select (dp => 
				new DataPoint (DateUtils.ConvertToJavaDate (dp.XValue), dp.YValue)).ToList ());
            _progressDialog.Dismiss ();
        }

        public string ChartTitle {
            set {
                _chartTitle = value;
                var symbolTextView = FindViewById<TextView> (Resource.Id.symbolTextView);
                if (symbolTextView != null) {
                    symbolTextView.Text = _chartTitle;
                }
            }
        }

        public event EventHandler<MovingAverageRequestedEventArgs> MovingAverageRequested = delegate { };

        public void UpdateChartWithMovingAverage (List<ChartDataPoint> data)
        {
            if(_movingAverageSeries == null) {
                _movingAverageSeries = new LineSeries ();
                _movingAverageSeries.Style.LineColor = Resources.GetColor (Resource.Color.chart_series2_line);
                _movingAverageSeries.Style.LineWidth = 1.5f;
                _chart.AddSeries (_movingAverageSeries);
            }
            _movingAverageSeries.DataAdapter = new SimpleDataAdapter ();
            _movingAverageSeries.DataAdapter.AddAll (data
                .Select (dp => 
                    new DataPoint (DateUtils.ConvertToJavaDate (dp.XValue), dp.YValue))
                .ToList ()
            );
        }

        #endregion

        private StockChartPresenter _presenter;
        private IShinobiChart _chart;
        private LineSeries _priceSeries;
        private LineSeries _movingAverageSeries;
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

            // Get the chart and configure it
            var chartFrag = FragmentManager.FindFragmentById<ChartFragment> (Resource.Id.chart);
            _chart = chartFrag.ShinobiChart;
            _chart.SetLicenseKey ("<PUT YOUR LICENSE KEY HERE>");

            _chart.XAxis = new DateTimeAxis ();
            _chart.XAxis.EnableGestures ();
            _chart.YAxis = new NumberAxis ();
            _chart.YAxis.EnableGestures ();

            // Set the style
            _chart.Style.BackgroundColor = Resources.GetColor (Resource.Color.chart_background);
            _chart.Style.CanvasBackgroundColor = Color.Transparent;
            _chart.Style.PlotAreaBackgroundColor = Color.Transparent;
            _chart.XAxis.Style.LineColor = Resources.GetColor (Resource.Color.chart_axis);
            _chart.YAxis.Style.LineColor = Resources.GetColor (Resource.Color.chart_axis);
            _chart.XAxis.Style.TickStyle.LabelColor = Resources.GetColor (Resource.Color.chart_axis);
            _chart.YAxis.Style.TickStyle.LabelColor = Resources.GetColor (Resource.Color.chart_axis);
            _chart.XAxis.Style.TickStyle.LineColor = Resources.GetColor (Resource.Color.chart_axis);
            _chart.YAxis.Style.TickStyle.LineColor = Resources.GetColor (Resource.Color.chart_axis);

            // Set the title
            if (_chartTitle != null) {
                FindViewById<TextView> (Resource.Id.symbolTextView).Text = _chartTitle;
            }

            // Enable the home button
            ActionBar.SetDisplayHomeAsUpEnabled (true);
            ActionBar.SetHomeButtonEnabled (true);
            ActionBar.Title = _presenter.Title;
        }

        public override bool OnOptionsItemSelected (IMenuItem item)
        {
            switch (item.ItemId) {
            case Android.Resource.Id.Home:
                NavUtils.NavigateUpTo (this, new Intent (this, typeof(StockPriceListActivity)));
                return true;
            case Resource.Id.action_add_moving_average:
                var dialog = new MovingAveragePeriodDialogFragment ((period) => {
                    MovingAverageRequested(this, new MovingAverageRequestedEventArgs (period));
                });
                dialog.Show(FragmentManager, "Moving Average Dialog");
                return true;
            default:
                return base.OnOptionsItemSelected (item);
            }
        }

        public override bool OnCreateOptionsMenu (IMenu menu)
        {
            MenuInflater.Inflate (Resource.Menu.chart_action_bar_menu, menu);
            return base.OnCreateOptionsMenu (menu);
        }
    }
}

