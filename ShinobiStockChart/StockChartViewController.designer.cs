// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace ShinobiStockChart
{
	[Register ("StockChartViewController")]
	partial class StockChartViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIView progressIndicatorView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView chartHostView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel symbolLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (progressIndicatorView != null) {
				progressIndicatorView.Dispose ();
				progressIndicatorView = null;
			}

			if (chartHostView != null) {
				chartHostView.Dispose ();
				chartHostView = null;
			}

			if (symbolLabel != null) {
				symbolLabel.Dispose ();
				symbolLabel = null;
			}
		}
	}
}
