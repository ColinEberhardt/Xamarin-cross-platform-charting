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

namespace ShinobiStockChart.Droid.Views
{
    public class MovingAveragePeriodDialogFragment : DialogFragment
    {
        private Action<int> _newPeriodHandler;

        public MovingAveragePeriodDialogFragment (Action<int> newPeriodHandler)
            :base()
        {
            _newPeriodHandler = newPeriodHandler;
        }

        public override Dialog OnCreateDialog (Bundle savedInstanceState)
        {
            var builder = new AlertDialog.Builder (Activity);
            var inflator = Activity.LayoutInflater;

            builder.SetView (inflator.Inflate (Resource.Layout.MovingAveragePeriodDialogLayout, null))
                .SetPositiveButton ("OK", (sender, e) => {
                    if(_newPeriodHandler != null) {
                        var periodTextView = Dialog.FindViewById<TextView> (Resource.Id.moving_average_period);
                        var period = int.Parse (periodTextView.Text);
                        // Call the callback we were supplied
                        _newPeriodHandler(period);
                    }

            })
                .SetNegativeButton ("Cancel", (sender, e) => {
                Dialog.Cancel ();
            })
                .SetTitle ("Moving Average")
                .SetMessage ("Set the period of the moving average");

            return builder.Create ();
        }
    }
}

