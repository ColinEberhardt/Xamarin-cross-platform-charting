using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ShinobiStockChart.Droid
{
    [Application]
    class ShinobiStockChartApplication : Application
    {
        public ShinobiStockChartApplication ()
			: base ()
        {
        }

        public ShinobiStockChartApplication (IntPtr javaReference, JniHandleOwnership transfer)
			: base (javaReference, transfer)
        {
        }

        public Activity CurrentActivity {
            get;
            set;
        }

        public object Presenter {
            get;
            set;
        }

        public static ShinobiStockChartApplication GetApplication (Context context)
        {
            return (ShinobiStockChartApplication)context.ApplicationContext;
        }
    }
}

