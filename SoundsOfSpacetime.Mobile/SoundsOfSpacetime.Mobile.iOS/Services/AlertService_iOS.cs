using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SoundsOfSpacetime.Mobile.Interfaces;
using UIKit;

namespace SoundsOfSpacetime.Mobile.iOS.Services
{
    public class AlertService_iOS : IAlertService
    {
        public void ShowAlert(string message, TimeSpan timespan, bool gestureDismissable)
        {
            bool alertCleanupRan = false;

            //Create the alert and add a gesture recognizer and a timer (two ways of it being dismissed)
            var alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            NSTimer alertDismissTimer = null;
            UITapGestureRecognizer alertDismissGestureRecognizer = null;

            //If the consumer requests a gesture dismissable alert
            if (gestureDismissable)
            {
                alertDismissGestureRecognizer = new UITapGestureRecognizer((gestureRecognizer) =>
                {
                    if (!alertCleanupRan)
                    {
                        this.CleanupAlert(alert, alertDismissTimer, gestureRecognizer);
                        alertCleanupRan = true;
                    }
                });
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, () =>
                {
                    alert.View.Superview?.AddGestureRecognizer(alertDismissGestureRecognizer);
                });
            }

            alertDismissTimer = NSTimer.CreateScheduledTimer(timespan.TotalSeconds, (timer) =>
            {
                if (!alertCleanupRan)
                {
                    this.CleanupAlert(alert, timer, alertDismissGestureRecognizer);
                    alertCleanupRan = true;
                }
            });
        }

        private void CleanupAlert(UIAlertController alert, NSTimer timer, UITapGestureRecognizer gestureRecognizer)
        {
            if(gestureRecognizer != null)
                alert.View.Superview?.RemoveGestureRecognizer(gestureRecognizer);

            alert.DismissViewController(true, null);
            timer.Dispose();
        }
    }
}