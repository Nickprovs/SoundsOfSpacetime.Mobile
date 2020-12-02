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
using Google.Android.Material.Snackbar;
using SoundsOfSpacetime.Mobile.Interfaces;
using Xamarin.Essentials;

namespace SoundsOfSpacetime.Mobile.Droid.Services
{
    public class AlertService_Android : IAlertService
    {
        public void ShowAlert(string message, TimeSpan timespan, bool gestureDismissable)
        {
            var snackBar = Snackbar.Make(Platform.CurrentActivity.FindViewById(Android.Resource.Id.Content), message, (int)timespan.TotalMilliseconds);
            
            //If consumer wants a gesture dismissable alert
            if (gestureDismissable)
            {
                snackBar.SetAction("DISMISS", (view) => {
                    snackBar.Dismiss();
                });
            }
           
            snackBar.Show();
        }
    }
}