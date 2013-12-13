using System;
using ShinobiStockChart.Model;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using ShinobiStockChart.Presenter.Service;
using System.IO;

namespace ShinobiStockChart.Presenter
{
    public class StockChartPresenter : BasePresenter
    {
        public interface View 
        {
            void UpdateChartWithData (List<ChartDataPoint> data);

            string ChartTitle { set; }
        }

        private IAppStatusService _statusService;

        private IMarshalInvokeService _marshalInvoke;

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

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create (url);
            webRequest.BeginGetResponse (new AsyncCallback (ReceivePriceData), webRequest);
        }

        private void ReceivePriceData(IAsyncResult result)
        {
            HttpWebResponse response = (HttpWebResponse)((HttpWebRequest)result.AsyncState).EndGetResponse(result);

            var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
            _chartData = ParseCSVStockPrices (body); 

            _marshalInvoke.Invoke (() => {
                _statusService.NetworkActivityIndicatorVisible = false;
                _view.UpdateChartWithData(_chartData);
            });
        }

        private List<ChartDataPoint> ParseCSVStockPrices (string csvData)
        {
            var seriesData = new List<ChartDataPoint> ();

            var lines = csvData.Split ('\n');
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

            return seriesData;
        }
    }
}

