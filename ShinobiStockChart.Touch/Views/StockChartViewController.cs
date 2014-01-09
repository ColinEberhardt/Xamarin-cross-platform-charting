using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ShinobiCharts;
using System.Net;
using System.Linq;
using ShinobiStockChart.Core.Utilities;
using ShinobiStockChart.Core.Presenter;
using ShinobiStockChart.Core.Model;
using ShinobiStockChart.Touch.Utilities;

namespace ShinobiStockChart.Touch.Views
{
    public partial class StockChartViewController : UIViewController, StockChartPresenter.View
    {
        #region View implementation

        public string ChartTitle {
            set {
                if(_chart != null) {
                    _chart.Title = value;
                } else {
                    _chartTitle = value;
                }
            }
        }

        public void UpdateChartWithData (List<ChartDataPoint> data)
        {
            _chartDataSource.DataPoints = data.Select(dp => new SChartDataPoint () {
                                                    XValue = dp.XValue.ToNSDate (),
                                                    YValue = new NSNumber (dp.YValue)
                                                })
                                                .Cast<SChartData>()
                                                .ToList(); 
            _chart.ReloadData ();
            _chart.RedrawChart ();

            progressIndicatorView.Hidden = true;
            chartHostView.Hidden = false;
        }

        public event EventHandler<MovingAverageRequestedEventArgs> MovingAverageRequested = delegate { };

        public void UpdateChartWithMovingAverage (List<ChartDataPoint> data)
        {
            _chartDataSource.MovingAverageDataPoints = data.Select (dp => new SChartDataPoint () {
                XValue = dp.XValue.ToNSDate (),
                YValue = new NSNumber (dp.YValue)
            })
                .Cast<SChartData> ()
                .ToList ();
            _chart.ReloadData ();
            _chart.RedrawChart ();
        }

        #endregion

        private ShinobiChart _chart;
        private string _chartTitle;
        private StockChartDataSource _chartDataSource;

        public StockChartViewController (StockChartPresenter presenter) : base ("StockChartViewController", null)
        {
            Title = presenter.Title;
            presenter.SetView (this);
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            // create the chart and add to the view      
            _chart = new ShinobiChart (chartHostView.Bounds);
            _chart.LicenseKey = "<PUT YOUR LICENSE KEY HERE";
            if(_chartTitle != null) {
                _chart.Title = _chartTitle;
            }

            // set the datasource
            _chartDataSource = new StockChartDataSource ();
            _chartDataSource.TintColor = View.TintColor;
            _chart.DataSource = _chartDataSource;
 
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

            // Add a nav bar button to add a trend line
            NavigationItem.SetRightBarButtonItem (
                new UIBarButtonItem (UIBarButtonSystemItem.Compose, (sender, e) => {
                    // Present an alert view
                    var alertView = new UIAlertView ("Moving Average",
                                        "Set the period of the moving average",
                                        null,
                                        "OK",
                                        new string[] { "Cancel" });
                    alertView.Clicked += (alertSender, button) => {
                        if(button.ButtonIndex == 0) {
                            MovingAverageRequested(this, new MovingAverageRequestedEventArgs (
                                int.Parse (alertView.GetTextField (0).Text))
                            );
                        }
                    };
                    alertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
                    alertView.GetTextField (0).Placeholder = "Moving Average Period";
                    alertView.GetTextField (0).KeyboardType = UIKeyboardType.NumberPad;
                    alertView.Show ();
                })
                , true);
        }

        private void ConfigureAxis (SChartAxis axis)
        {
            axis.EnableGesturePanning = true;
            axis.EnableGestureZooming = true;
            axis.EnableMomentumPanning = true;
            axis.EnableMomentumZooming = true;
            axis.Style.MajorGridLineStyle.ShowMajorGridLines = false;
        }


        private class StockChartDataSource : SChartDataSource
        {
            private List<SChartData> _dataPoints;
            private List<SChartData> _movingAverageDataPoints;

            public UIColor TintColor { get; set; }

            public StockChartDataSource ()
            {
                _dataPoints = new List<SChartData> ();      
            }

            public List<SChartData> DataPoints {
                set {
                    _dataPoints = value;
                }
            }

            public List<SChartData> MovingAverageDataPoints {
                set {
                    _movingAverageDataPoints = value;
                }
            }

            public override SChartData GetDataPoint (ShinobiChart chart, int dataIndex, int seriesIndex)
            {
                // no-op
                return null;
            }

            protected override SChartData[] GetDataPoints (ShinobiChart chart, int seriesIndex)
            {
                // Bit convoluted to get the z-index correct
                if(_movingAverageDataPoints == null || seriesIndex == 1) {
                    return _dataPoints.ToArray ();
                } else {
                    return _movingAverageDataPoints.ToArray ();
                }
            }

            public override int GetNumberOfSeries (ShinobiChart chart)
            {
                if(_movingAverageDataPoints != null) {
                    return 2;
                } else {
                    return 1;
                }
            }

            public override int GetNumberOfDataPoints (ShinobiChart chart, int seriesIndex)
            {
                // Bit convoluted to get the z-index correct
                if(_movingAverageDataPoints == null || seriesIndex == 1) {
                    return _dataPoints.Count;
                } else {
                    return _movingAverageDataPoints.Count;
                }
            }

            public override SChartSeries GetSeries (ShinobiChart chart, int index)
            {
                var lineSeries = new SChartLineSeries ();
         
                // Bit convoluted to get the z-index correct
                if (_movingAverageDataPoints == null || index == 1) {
                    lineSeries.Style.AreaLineColor = TintColor;
                    lineSeries.Style.AreaColor = TintColor.ColorWithAlpha (0.1f);
                    lineSeries.Style.AreaColorLowGradient = TintColor.ColorWithAlpha (0.8f);
                    lineSeries.Style.AreaLineWidth = 1.0;
                    lineSeries.Style.ShowFill = true;
                    lineSeries.CrosshairEnabled = true;
                } else {
                    lineSeries.Style.LineColor = UIColor.Red.ColorWithAlpha (0.8f);
                    lineSeries.Style.LineWidth = 1.0;
                    lineSeries.Style.ShowFill = false;
                    lineSeries.CrosshairEnabled = false;
                }
        
                return lineSeries;
            }
        }
    }
}


