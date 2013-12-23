// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace ShinobiStockChart
{
	[Register ("StockChartViewController")]
	partial class StockChartViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnCreateMovingAverage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView chartHostView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField movingAveragePeriod { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView progressIndicatorView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel symbolLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (chartHostView != null) {
				chartHostView.Dispose ();
				chartHostView = null;
			}

			if (movingAveragePeriod != null) {
				movingAveragePeriod.Dispose ();
				movingAveragePeriod = null;
			}

			if (progressIndicatorView != null) {
				progressIndicatorView.Dispose ();
				progressIndicatorView = null;
			}

			if (symbolLabel != null) {
				symbolLabel.Dispose ();
				symbolLabel = null;
			}

			if (btnCreateMovingAverage != null) {
				btnCreateMovingAverage.Dispose ();
				btnCreateMovingAverage = null;
			}
		}
	}
}
