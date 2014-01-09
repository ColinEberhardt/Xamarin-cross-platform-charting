using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace ShinobiStockChart.Droid.Views
{
    public class ChangeIndicatorTextView : TextView
    {
        public ChangeIndicatorTextView (Context context) :
            base (context)
        {
            Initialize ();
        }

        public ChangeIndicatorTextView (Context context, IAttributeSet attrs) :
            base (context, attrs)
        {
            Initialize ();
        }

        public ChangeIndicatorTextView (Context context, IAttributeSet attrs, int defStyle) :
            base (context, attrs, defStyle)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        public double Value {
            set { 
                this.Text = (double.IsNaN (value) ? "-" : string.Format ("{0:0.00}", value));
            }
        }

        public ChangeDirection Direction {
            set {
                if (value == ChangeDirection.Increasing) {
                    SetTextColor (Resources.GetColor (Resource.Color.change_indicator_increasing));
                } else if (value == ChangeDirection.Decreasing) {
                    SetTextColor (Resources.GetColor (Resource.Color.change_indicator_decreasing));
                } else {
                    SetTextColor (Color.DarkGray);
                }
            }
        }

        public enum ChangeDirection
        {
            Increasing,
            Decreasing,
            NoChange
        }
    }
}

