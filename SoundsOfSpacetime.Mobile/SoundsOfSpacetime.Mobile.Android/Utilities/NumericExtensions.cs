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

namespace SoundsOfSpacetime.Mobile.Droid.Utilities
{
    public static class NumericExtensions
    {
        public static float ToDip(this float value, Context context)
        {
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, value, context.Resources.DisplayMetrics);
        }

        public static int ToDip(this int value, Context context)
        {
            var dipValue = TypedValue.ApplyDimension(ComplexUnitType.Dip, value, context.Resources.DisplayMetrics);
            return (int)Math.Round(dipValue);
        }
    }
}