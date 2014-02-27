using System;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace ShinobiStockChart.Core.Model
{
    public class WebDataSource : IDataSource
    {
        private string FTSE100 = "AAL.L,ABF.L,ADM.L,AGK.L,AMEC.L,ANTO.L,ARM.L,AU.L,AV.L,AZN.L,BA.L,BARC.L,BATS.L,BG.L,BLND.L,BLT.L,BP.L,BRBY.L,BSY.L,BT-A.L,CCL.L,CNA.L,CNE.L,CPG.L,CPI.L,CSCG.L,DGE.L,EMG.L,ENRC.L,ESSR.L,EXPN.L,FRES.L,GFS.L,GKN.L,GLEN.L,GSK.L,HL.L,HMSO.L,HSBA.L,IAG.L,IAP.L,IHG.L,III.L,IMI.L,IMT.L,INVP.L,IPR.L,ISAT.L,ITRK.L,ITV.L,JMAT.L,KAZ.L,KGF.L,LAND.L,LGEN.L,LLOY.L,LMI.L,MKS.L,MRW.L,NG.L,NXT.L,OML.L,PFC.L,PRU.L,PSON.L,RB.L,RBS.L,RDSA.L,RDSB.L,REX.L,RIO.L,RR.L,RRS.L,RSA.L,RSL.L,SAB.L,SBRY.L,SDR.L,SDRC.L,SGE.L,SHP.L,SL.L,SMIN.L,SN.L,SRP.L,SSE.L,STAN.L,SVT.L,TATE.L,TLW.L,TSCO.L,ULVR.L,UU.L,VED.L,VOD.L,WEIR.L,WG.L,WOS.L,WPP.L,WTB.L,XTA.L";

        public WebDataSource ()
        {

        }

        public void FetchStockData(string symbol, Action<string> received)
        {
            // generate the stock data items from a CSV list
            string url = "http://ichart.finance.yahoo.com/table.csv?d=0&e=28&f=2013&g=d&a=3&b=12&c=1996&ignore=.csv&s="
                         + symbol;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create (url);
            webRequest.BeginGetResponse (result => {
                using (HttpWebResponse response = (HttpWebResponse)webRequest.EndGetResponse(result))
                {
                    var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    received(body);
                }
            }, null);
        }

        public void FetchStockList(Action<string> received)
        {
            // generate the stock data items from a CSV list
            string url = "http://finance.yahoo.com/d/quotes.csv?f=sac1k&s=";      
            url += string.Join ("+", FTSE100.Split (','));

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create (url);
            webRequest.BeginGetResponse (result => {
                using (HttpWebResponse response = (HttpWebResponse)webRequest.EndGetResponse(result))
                {
                    var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    received(body);
                }
            }, null);
        }

       
    }
}

