using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace ShinobiStockChart
{
  /// <summary>
  /// A view controller that renders a list of stocks frm the FTSE 100 index.
  /// </summary>
  public partial class StocksListViewController : UIViewController
  {
    private string FTSE100 = "AAL.L,ABF.L,ADM.L,AGK.L,AMEC.L,ANTO.L,ARM.L,AU.L,AV.L,AZN.L,BA.L,BARC.L,BATS.L,BG.L,BLND.L,BLT.L,BP.L,BRBY.L,BSY.L,BT-A.L,CCL.L,CNA.L,CNE.L,CPG.L,CPI.L,CSCG.L,DGE.L,EMG.L,ENRC.L,ESSR.L,EXPN.L,FRES.L,GFS.L,GKN.L,GLEN.L,GSK.L,HL.L,HMSO.L,HSBA.L,IAG.L,IAP.L,IHG.L,III.L,IMI.L,IMT.L,INVP.L,IPR.L,ISAT.L,ITRK.L,ITV.L,JMAT.L,KAZ.L,KGF.L,LAND.L,LGEN.L,LLOY.L,LMI.L,MKS.L,MRW.L,NG.L,NXT.L,OML.L,PFC.L,PRU.L,PSON.L,RB.L,RBS.L,RDSA.L,RDSB.L,REX.L,RIO.L,RR.L,RRS.L,RSA.L,RSL.L,SAB.L,SBRY.L,SDR.L,SDRC.L,SGE.L,SHP.L,SL.L,SMIN.L,SN.L,SRP.L,SSE.L,STAN.L,SVT.L,TATE.L,TLW.L,TSCO.L,ULVR.L,UU.L,VED.L,VOD.L,WEIR.L,WG.L,WOS.L,WPP.L,WTB.L,XTA.L";
    
    // a data item that represents a single stock
    private class StockItem
    {
      public StockItem(string symbol)
      {
        Price = double.NaN;
        Symbol = symbol;
      }
      
      public string Symbol { get; private set; }

      public double Price { get; set; }
      
      public double Change { get; set; }
    }    
    
    // the list of socks
    private List<StockItem> _stocks = new List<StockItem> ();
    
    public StocksListViewController () : base ("StocksListViewController", null)
    {
      Title = "FTSE 100";
      
      // generate the stock data items from a CSV list
      var symbols = FTSE100.Split (',');
      foreach (var symbol in symbols) {
        _stocks.Add (new StockItem (symbol));
      }
      
      UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
      
      FetchQuotes ();
    }
    
    
    public override void ViewDidLoad ()
    {
      base.ViewDidLoad ();      
      
      // set the UITableView source / delegate
      stockListTable.Source = new TableSource (this.NavigationController, _stocks);
    }
    
    public override void ViewDidUnload ()
    {
      base.ViewDidUnload ();
            
      ReleaseDesignerOutlets ();
    }
    
    
    /// <summary>
    /// Fetches the current stock quote from Yahoo
    /// </summary>
    private void FetchQuotes ()
    {
      string url = "http://finance.yahoo.com/d/quotes.csv?f=sac1k&s=";      
      url += string.Join ("+", _stocks.Select (s => s.Symbol));
      
      WebClient client = new WebClient ();
      client.DownloadStringCompleted += (s,e) => ParseStockQuotes(e.Result);     
      client.DownloadStringAsync (new Uri (url));
    }
    
    
    private void ParseStockQuotes (string quotesCSV)
    {
      // split each line
      var lines = quotesCSV.Split ('\n');
      foreach (var line in lines) {
        // split each item within the line
        var components = line.Split (',');
        if (components.Length > 1) {
          try {
            // extract the symbol, price and change
            string symbol = components [0].Replace ("\"", "");
            double quote = double.Parse (components [1]);
            double change = double.Parse (components [2]);
            
            // locate the respective data item and update its state
            var stockItem = _stocks.SingleOrDefault (s => s.Symbol == symbol);
            if (stockItem != null) {
              stockItem.Price = quote;
              stockItem.Change = change;
            }
          } catch {
          }
        }
      }
      
      // re-render the list
      stockListTable.ReloadData ();
      UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
    }
    
    // a table source that renders our list of stocks
    private class TableSource : UITableViewSource
    {
      private static int _sequence;
      private static readonly string _cellIdentifier = "TableCell";
      
      private Dictionary<int, StockItemTableCellView> _cellControllers;      
      private List<StockItem> _tableItems;
      private UINavigationController _navigationController;
 
      public TableSource (UINavigationController navigationController, List<StockItem> items)
      {
        _tableItems = items;
        _navigationController = navigationController;
        _cellControllers = new Dictionary<int, StockItemTableCellView>();
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
        var chartViewController = new StockChartViewController (stockDataItem.Symbol);
        _navigationController.PushViewController (chartViewController, true);
      }
    }
  }
}



