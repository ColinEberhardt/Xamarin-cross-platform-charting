using System;
using ShinobiStockChart.Model;
using System.Net;
using System.Collections.Generic;
using ShinobiCharts;
using System.Linq;
using MonoTouch.Foundation;
using ShinobiStockChart.Utilities;

namespace ShinobiStockChart
{
    public class StockChartPresenter
    {
        public interface View 
        {
            void UpdateChartWithData (List<SChartData> data);
        }

        private IAppStatusService _statusService;

        private IMarshalInvokeService _marshalInvoke;

        private View _view;

        private List<SChartData> _chartData;

        public StockChartPresenter (IAppStatusService statusService, IMarshalInvokeService marshalInvoke, StockItem stockItem)
        {
            _statusService = statusService;
            _marshalInvoke = marshalInvoke;

            FetchPriceData (stockItem.Symbol);
        }

        public void SetView(View view)
        {
            _view = view;

            // if we already have data - supply it to the chart
            if (_chartData != null) {
                _view.UpdateChartWithData (_chartData);
            }
        }

        private void FetchPriceData (string symbol)
        {
            _statusService.NetworkActivityIndicatorVisible = true;

            string url = "http://ichart.finance.yahoo.com/table.csv?d=0&e=28&f=2013&g=d&a=3&b=12&c=1996&ignore=.csv&s="
                         + symbol;

            WebClient client = new WebClient ();
            client.DownloadStringCompleted += (s, e) => {
                _chartData = ParseCSVStockPrices (e.Result); 

                _marshalInvoke.Invoke (() => {
                    _statusService.NetworkActivityIndicatorVisible = false;
                    _view.UpdateChartWithData(_chartData);
                });
            };
            client.DownloadStringAsync (new Uri (url));
        }

        private List<SChartData> ParseCSVStockPrices (string csvData)
        {
            var seriesData = new List<SChartData> ();

            var lines = csvData.Split ('\n');
            foreach (var line in lines.Skip(1)) {
                var components = line.Split (',');
                if (components.Length > 1) {
                    DateTime date = DateTime.Parse (components [0]);
                    double value = double.Parse (components [1]);
                    seriesData.Add (new SChartDataPoint () {
                        XValue = date.ToNSDate (),
                        YValue = new NSNumber (value)
                    });
                }
            }

            return seriesData;
        }
    }
}

