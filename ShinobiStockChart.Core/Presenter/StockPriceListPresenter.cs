using System;
using ShinobiStockChart.Core.Model;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using ShinobiStockChart.Core.Presenter.Service;
using System.IO;

namespace ShinobiStockChart.Core.Presenter
{
    public class StockPriceListPresenter : BasePresenter
    {
        public interface View
        {
            void SetStockPrices (List<StockItem> prices);

            event EventHandler<StockSelectedEventArgs> StockSelected;
        }

        private string FTSE100 = "AAL.L,ABF.L,ADM.L,AGK.L,AMEC.L,ANTO.L,ARM.L,AU.L,AV.L,AZN.L,BA.L,BARC.L,BATS.L,BG.L,BLND.L,BLT.L,BP.L,BRBY.L,BSY.L,BT-A.L,CCL.L,CNA.L,CNE.L,CPG.L,CPI.L,CSCG.L,DGE.L,EMG.L,ENRC.L,ESSR.L,EXPN.L,FRES.L,GFS.L,GKN.L,GLEN.L,GSK.L,HL.L,HMSO.L,HSBA.L,IAG.L,IAP.L,IHG.L,III.L,IMI.L,IMT.L,INVP.L,IPR.L,ISAT.L,ITRK.L,ITV.L,JMAT.L,KAZ.L,KGF.L,LAND.L,LGEN.L,LLOY.L,LMI.L,MKS.L,MRW.L,NG.L,NXT.L,OML.L,PFC.L,PRU.L,PSON.L,RB.L,RBS.L,RDSA.L,RDSB.L,REX.L,RIO.L,RR.L,RRS.L,RSA.L,RSL.L,SAB.L,SBRY.L,SDR.L,SDRC.L,SGE.L,SHP.L,SL.L,SMIN.L,SN.L,SRP.L,SSE.L,STAN.L,SVT.L,TATE.L,TLW.L,TSCO.L,ULVR.L,UU.L,VED.L,VOD.L,WEIR.L,WG.L,WOS.L,WPP.L,WTB.L,XTA.L";

        private View _view;

        private IDataSource _dataSource = new WebDataSource();

        private IAppStatusService _statusService;

        private IMarshalInvokeService _marshalInvoke;

        private INavigationService _navigationService;

        private List<StockItem> _stocks = new List<StockItem> ();

        public StockPriceListPresenter (IAppStatusService statusService, IMarshalInvokeService marshalInvoke, INavigationService navigationService)
            : base("FTSE 100")
        {           
            _statusService = statusService;
            _marshalInvoke = marshalInvoke;
            _navigationService = navigationService;

            // generate the stock data items from a CSV list
            var symbols = FTSE100.Split (',');
            foreach (var symbol in symbols) {
                _stocks.Add (new StockItem (symbol));
            }

            FetchQuotes ();
        }

        public void SetView(View view)
        {
            _view = view;
            _view.StockSelected += HandleStockSelected;
        }

        private void HandleStockSelected (object sender, StockSelectedEventArgs e)
        {
            _navigationService.PushPresenter (new StockChartPresenter (_statusService, _marshalInvoke, e.SelectedItem));
        }

        /// <summary>
        /// Fetches the current stock quote from Yahoo
        /// </summary>
        private void FetchQuotes ()
        {
            _statusService.NetworkActivityIndicatorVisible = true;

            _dataSource.FetchStockList (csv => ParseStockQuotes (csv));
        }


        private void ParseStockQuotes (string quotesCSV)
        {
            // split each line
            var lines = quotesCSV.Split (new char[] { '\n', '\r' });
            foreach (var line in lines) {
                // fail fast on any stocks that lack prices
                if (line.Contains ("N/A"))
                    continue;

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

            _marshalInvoke.Invoke (() => {
                _statusService.NetworkActivityIndicatorVisible = false;
                _view.SetStockPrices(_stocks);
            });
        }


    }
}

