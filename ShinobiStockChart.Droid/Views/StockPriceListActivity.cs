using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ShinobiStockChart.Core.Presenter;
using ShinobiStockChart.Droid.Service;
using ShinobiStockChart.Core.Model;
using ShinobiStockChart.Droid;
using Android.Graphics;

namespace ShinobiStockChart.Droid.Views
{
    [Activity (MainLauncher = true)]
    public class StockPriceListActivity : Activity, StockPriceListPresenter.View
    {
        private List<StockItem> _prices;

        #region View implementation

        public event EventHandler<StockSelectedEventArgs> StockSelected = delegate { };

        public void SetStockPrices (List<StockItem> prices)
        {
            _prices = prices;
            _listView.Adapter = new StockPriceListAdapter (this, _prices);
        }

        #endregion

        private StockPriceListPresenter _presenter;
        private ListView _listView;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Manage the views
            SetContentView (Resource.Layout.StockPriceListActivityLayout);
            _listView = FindViewById<ListView> (Resource.Id.stock_list);


            // Set up the correct properties on the application
            var app = ShinobiStockChartApplication.GetApplication (this);
            app.CurrentActivity = this;

            // Prepare the services
            var uiMarshal = new MarshalInvokeService (app);
            var appStatus = new AppStatusService (app);
            var navigation = new NavigationService (app);

            // Create the presenter
            _presenter = new StockPriceListPresenter (appStatus, uiMarshal, navigation);
            _presenter.SetView (this);
            app.Presenter = _presenter;

            // Set up click handling
            _listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                var stockItem = _prices [e.Position];
                StockSelected (this, new StockSelectedEventArgs (stockItem));
            };

            // Apply the title
            ActionBar.Title = _presenter.Title;
        }

        private class StockPriceListAdapter : BaseAdapter<StockItem>
        {
            private List<StockItem> _items;
            private Activity _context;

            public StockPriceListAdapter (Activity context, List<StockItem> items)
            {
                _context = context;
                _items = items;
            }

            #region implemented abstract members of BaseAdapter

            public override long GetItemId (int position)
            {
                return position;
            }

            public override View GetView (int position, View convertView, ViewGroup parent)
            {
                var stockItem = _items [position];
                View view = convertView;
                if (view == null) {
                    // Haven't been provided with a row to re-use, so let's create a new one
                    view = _context.LayoutInflater.Inflate (Resource.Layout.StockItemListViewRow, null);
                }
                // Set the stock name
                view.FindViewById<TextView> (Resource.Id.text_ticker).Text = stockItem.Symbol;
                // Set the stock price 
                ChangeIndicatorTextView priceView = view.FindViewById<ChangeIndicatorTextView> (Resource.Id.text_price);
                priceView.Value = stockItem.Price;
                // Set the colour of the price
                if (stockItem.Change < 0) {
                    priceView.Direction = ChangeIndicatorTextView.ChangeDirection.Decreasing;
                } else if (stockItem.Change > 0) {
                    priceView.Direction = ChangeIndicatorTextView.ChangeDirection.Increasing;
                } else {
                    priceView.Direction = ChangeIndicatorTextView.ChangeDirection.NoChange;
                }
                return view;
            }

            public override int Count {
                get {
                    return _items.Count;
                }
            }

            public override StockItem this [int index] {
                get {
                    return _items [index];
                }
            }

            #endregion
        }
    }
}

