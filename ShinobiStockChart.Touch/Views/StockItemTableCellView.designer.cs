// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace ShinobiStockChart.Touch.Views
{
	[Register ("StockItemTableCellView")]
	partial class StockItemTableCellView
	{
		[Outlet]
		MonoTouch.UIKit.UITableViewCell cell { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel tickerLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel stockPrice { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (cell != null) {
				cell.Dispose ();
				cell = null;
			}

			if (tickerLabel != null) {
				tickerLabel.Dispose ();
				tickerLabel = null;
			}

			if (stockPrice != null) {
				stockPrice.Dispose ();
				stockPrice = null;
			}
		}
	}
}
