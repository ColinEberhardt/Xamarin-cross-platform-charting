using System;
using Com.ShinobiControls.Charts;

namespace ShinobiStockChart.Android.ExtensionMethods
{
	public static class AxisExtensions
	{
		public static void EnableGestures(this Axis axis)
		{
			axis.GesturePanningEnabled = true;
			axis.GestureZoomingEnabled = true;
			axis.MomentumPanningEnabled = true;
			axis.MomentumZoomingEnabled = true;
		}
	}
}

