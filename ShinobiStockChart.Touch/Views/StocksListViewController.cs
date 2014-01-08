using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using ShinobiStockChart.Core.Model;
using ShinobiStockChart.Core.Presenter;

namespace ShinobiStockChart.Touch.Views
{
    /// <summary>
    /// A view controller that renders a list of stocks frm the FTSE 100 index.
    /// </summary>
    public partial class StocksListViewController : UIViewController, StockPriceListPresenter.View
    {
        #region View implementation

        public event EventHandler<StockSelectedEventArgs> StockSelected  = delegate { };

        public void SetStockPrices (List<StockItem> prices)
        {
            // set the UITableView source / delegate
            stockListTable.Source = new TableSource (this, prices);
            stockListTable.ReloadData ();
        }

        #endregion

        private StockPriceListPresenter _presenter;

        public StocksListViewController (StockPriceListPresenter presenter) : base ("StocksListViewController", null)
        {
            Title = presenter.Title;

            _presenter = presenter;
            _presenter.SetView (this);
        }

        protected void RaiseNavigationEvent(StockItem item)
        {
            StockSelected(this, new StockSelectedEventArgs(item));
        }

        public override void ViewWillAppear (bool animated)
        {
            stockListTable.DeselectRow (stockListTable.IndexPathForSelectedRow, animated);
            base.ViewWillAppear (animated);
        }

        // a table source that renders our list of stocks
        private class TableSource : UITableViewSource
        {
            private static int _sequence;
            private static readonly string _cellIdentifier = "TableCell";
            private Dictionary<int, StockItemTableCellView> _cellControllers;
            private List<StockItem> _tableItems;
            private StocksListViewController _viewController;

            public TableSource (StocksListViewController viewController, List<StockItem> items)
            {
                _tableItems = items;
                _viewController = viewController;
                _cellControllers = new Dictionary<int, StockItemTableCellView> ();
            }

            public override int RowsInSection (UITableView tableview, int section)
            {
                return _tableItems.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell (_cellIdentifier);
        
                // use the method described in this blog post to wrap a cell in a view controller
                // http://simon.nureality.ca/?p=91
                StockItemTableCellView cellController = null;        
                if (cell == null) {
                    cellController = new StockItemTableCellView ();
                    NSBundle.MainBundle.LoadNib ("StockItemTableCellView", cellController, null);
                    cell = cellController.Cell;
          
                    cell.Tag = _sequence++;
                    _cellControllers.Add (cell.Tag, cellController);
                } else {
                    cellController = _cellControllers [cell.Tag];
                }
        
                // set the state of this cell
                var stockDataItem = _tableItems [indexPath.Row];
                cellController.Symbol = stockDataItem.Symbol;
                cellController.Price = stockDataItem.Price; 
        
                if (stockDataItem.Change > 0.0) {
                    cellController.Direction = PriceDirection.Rising;
                } else if (stockDataItem.Change < 0.0) {
                    cellController.Direction = PriceDirection.Falling;
                } else {
                    cellController.Direction = PriceDirection.NonMover;
                }
                return cell;
            }

            public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {
                var stockDataItem = _tableItems [indexPath.Row];
                _viewController.RaiseNavigationEvent (stockDataItem);
            }
        }
    }
}



