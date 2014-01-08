// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace ShinobiStockChart.Touch.Views
{
	[Register ("StocksListViewController")]
	partial class StocksListViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView stockListTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (stockListTable != null) {
				stockListTable.Dispose ();
				stockListTable = null;
			}
		}
	}
}
