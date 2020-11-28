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

namespace SoundsOfSpacetime.Mobile.Droid.Services.AudioDeviceMonitoring
{
    public class HeadphoneStatusChangedEventArgs : EventArgs
    {
        public bool Connected { get; }

        public HeadphoneStatusChangedEventArgs(bool connected)
        {
            this.Connected = connected;
        }
    }
}