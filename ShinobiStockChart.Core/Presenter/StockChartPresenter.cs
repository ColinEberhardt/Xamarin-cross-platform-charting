using System;
using ShinobiStockChart.Core.Model;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using ShinobiStockChart.Core.Presenter.Service;
using System.IO;
using ShinobiStockChart.Core.Utilities;

namespace ShinobiStockChart.Core.Presenter
{
    public class StockChartPresenter : BasePresenter
    {
        public interface View 
        {
            void UpdateChartWithData (List<ChartDataPoint> data);

            void UpdateChartWithMovingAverage (List<ChartDataPoint> data);

            event EventHandler<MovingAverageRequestedEventArgs> MovingAverageRequested;

            string ChartTitle { set; }
        }

        private IAppStatusService _statusService;

        private IMarshalInvokeService _marshalInvoke;

        private IDataSource _dataSource = new WebDataSource();

        private View _view;

        private List<ChartDataPoint> _chartData;

        private StockItem _stockItem;

        public StockChartPresenter (IAppStatusService statusService, IMarshalInvokeService marshalInvoke, StockItem stockItem)
            : base(stockItem.Symbol)
        {
            _statusService = statusService;
            _marshalInvoke = marshalInvoke;
            _stockItem = stockItem;

            FetchPriceData (stockItem.Symbol);
        }

        public void SetView(View view)
        {
            _view = view;
            _view.ChartTitle = _stockItem.Symbol;

            // Listen to moving average request events
            _view.MovingAverageRequested += HandleMovingAverageRequested;

            // if we already have data - supply it to the chart
            if (_chartData != null) {
                _view.UpdateChartWithData (_chartData);
            }
        }

        void HandleMovingAverageRequested (object sender, MovingAverageRequestedEventArgs e)
        {
            // Create the moving average values
            var movingAverage = _chartData
                .Window (e.NumberOfDays, window => {
                    return new ChartDataPoint () {
                        XValue = window.Last ().XValue,
                        YValue = window.Select (dp => dp.YValue).Average ()
                    };
                })
                .ToList ();
          
            // Send the result back to the the view
            _view.UpdateChartWithMovingAverage (movingAverage);
        }

        private void FetchPriceData (string symbol)
        {
            _dataSource.FetchStockData (symbol, data => {
                ReceivePriceData(data);
            });
        }

        private void ReceivePriceData(string csvData)
        {
            _chartData = ParseCSVStockPrices (csvData); 

            _marshalInvoke.Invoke (() => {
                _statusService.NetworkActivityIndicatorVisible = false;
                _view.UpdateChartWithData(_chartData);
            });
        }

        private List<ChartDataPoint> ParseCSVStockPrices (string csvData)
        {
            var seriesData = new List<ChartDataPoint> ();

            var lines = csvData.Split (new char[] { '\n', '\r' });
            foreach (var line in lines.Skip(1)) {
                var components = line.Split (',');
                if (components.Length > 1) {
                    DateTime date = DateTime.Parse (components [0]);
                    double value = double.Parse (components [1]);
                    seriesData.Add (new ChartDataPoint () {
                        XValue = date,
                        YValue = value
                    });
                }
            }

            seriesData.Sort ((dp1, dp2) => dp1.XValue.CompareTo (dp2.XValue));

            return seriesData;
        }
    }
}

