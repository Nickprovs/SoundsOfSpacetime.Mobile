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
    [Service]
    public class HeadsetMonitoringService : Service  //This service basically filters all intents related to headphones and passes them onto our receiver
    {
        HeadsetStateBroadcastReceiver headsetStateReceiver;

        public override void OnCreate()
        {

            headsetStateReceiver = new HeadsetStateBroadcastReceiver();
            IntentFilter filter = new IntentFilter();
            foreach (String action in HeadsetStateBroadcastReceiver.HEADPHONE_ACTIONS)
            {
                filter.AddAction(action);
            }

            RegisterReceiver(headsetStateReceiver, filter);

        }

        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            UnregisterReceiver(headsetStateReceiver);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}