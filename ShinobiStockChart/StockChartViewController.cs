using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ShinobiCharts;
using System.Net;
using System.Linq;
using ShinobiStockChart.Utilities;

namespace ShinobiStockChart
{
    public partial class StockChartViewController : UIViewController, StockChartPresenter.View
    {
        #region View implementation

        public void UpdateChartWithData (List<SChartData> data)
        {
            _chartDataSource.DataPoints = data; 
            _chart.ReloadData ();
            _chart.RedrawChart ();

            progressIndicatorView.Hidden = true;
            chartHostView.Hidden = false;
        }

        #endregion

        private ShinobiChart _chart;
        private StockChartDataSource _chartDataSource;

        public StockChartViewController (StockChartPresenter presenter) : base ("StockChartViewController", null)
        {
            presenter.SetView (this);
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
      
            // symbolLabel.Text = _symbol;
      
            // create the chart and add to the view      
            _chart = new ShinobiChart (chartHostView.Bounds);
            _chart.LicenseKey = ShinobiLicenseKeyProviderJson.Instance.ChartsLicenseKey;
      
            // set the datasource
            _chartDataSource = new StockChartDataSource ();
            _chart.DataSource = _chartDataSource;
      
      
            _chart.Theme = new SChartMidnightTheme ();
            View.BackgroundColor = _chart.Theme.ChartStyle.BackgroundColor;
 
            // add a couple of axes
            _chart.XAxis = new SChartDateTimeAxis ();
            _chart.YAxis = new SChartNumberAxis ();
            ConfigureAxis (_chart.XAxis);
            ConfigureAxis (_chart.YAxis);
      
            // add a fancy border to the loading indicato
            progressIndicatorView.Layer.MasksToBounds = false;
            progressIndicatorView.Layer.CornerRadius = 10;
            progressIndicatorView.Layer.ShadowColor = UIColor.DarkGray.CGColor;
            progressIndicatorView.Layer.ShadowOpacity = 1.0f;
            progressIndicatorView.Layer.ShadowRadius = 6.0f;
            progressIndicatorView.Layer.ShadowOffset = new SizeF (0f, 3f);
      
            chartHostView.Hidden = true;
            chartHostView.InsertSubview (_chart, 0);
        }

        private void ConfigureAxis (SChartAxis axis)
        {
            axis.EnableGesturePanning = true;
            axis.EnableGestureZooming = true;
            axis.EnableMomentumPanning = true;
            axis.EnableMomentumZooming = true;
        }


        private class StockChartDataSource : SChartDataSource
        {
            private List<SChartData> _dataPoints;

            public StockChartDataSource ()
            {
                _dataPoints = new List<SChartData> ();      
            }

            public List<SChartData> DataPoints {
                set {
                    _dataPoints = value;
                }
            }

            public override SChartData GetDataPoint (ShinobiChart chart, int dataIndex, int seriesIndex)
            {
                // no-op
                return null;
            }

            protected override SChartData[] GetDataPoints (ShinobiChart chart, int seriesIndex)
            {
                return _dataPoints.ToArray ();
            }

            public override int GetNumberOfSeries (ShinobiChart chart)
            {
                return 1;
            }

            public override int GetNumberOfDataPoints (ShinobiChart chart, int seriesIndex)
            {
                return _dataPoints.Count;
            }

            public override SChartSeries GetSeries (ShinobiChart chart, int index)
            {
                var lineSeries = new SChartLineSeries ();
         
                lineSeries.Style.LineColor = UIColor.FromRGB (166, 166, 166);
                lineSeries.Style.AreaColor = UIColor.FromRGB (16, 99, 123);
                lineSeries.Style.AreaColorLowGradient = UIColor.FromRGB (0, 0, 41);
                lineSeries.Style.ShowFill = true;
        
                lineSeries.CrosshairEnabled = true;
        
                return lineSeries;
            }
        }
    }
}


