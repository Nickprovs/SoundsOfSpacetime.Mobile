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
using Android.Support.V4.Content;

namespace SoundsOfSpacetime.Mobile.Droid.Services.AudioDeviceMonitoring
{
    [BroadcastReceiver(Enabled = true, Exported = false)]

    public class HeadsetStateBroadcastReceiver : BroadcastReceiver
    {
        public readonly static String[] HEADPHONE_ACTIONS = { Intent.ActionHeadsetPlug, "android.bluetooth.headset.action.STATE_CHANGED",
        "android.bluetooth.headset.profile.action.CONNECTION_STATE_CHANGED"};


        public override void OnReceive(Context context, Intent intent)
        {
            Boolean broadcast = false;
            int state = 0;
            Boolean connected = false;
            //If the intent corresponds to either a wired headset or bluetooth (legacy or modern)
            //Get the extra data from the intent. This will tell us if it's connected or disconnected.
            //Set the connected flag for the message. 1 = connected, 0 = disconnected.
            //If we hit one of these intents, broadcast there was an event and the status of the headphones.

            // Wired headset monitoring
            if (intent.Action.Equals(HEADPHONE_ACTIONS[0]))
            {
                state = intent.GetIntExtra("state", 0);
                //AudioPreferences.setWiredHeadphoneState(context, state > 0);
                broadcast = true;
            }

            //Bluetooth monitoring from Android 1-Honeycomb
            if (intent.Action.Equals(HEADPHONE_ACTIONS[1]))
            {
                state = intent.GetIntExtra("android.bluetooth.headset.extra.STATE", 0);
                //AudioPreferences.setBluetoothHeadsetState(context, state == 2);
                broadcast = true;
            }

            // Works for Ice Cream Sandwich-Beyond
            if (intent.Action.Equals(HEADPHONE_ACTIONS[2]))
            {
                state = intent.GetIntExtra("android.bluetooth.profile.extra.STATE", 0);
                //AudioPreferences.setBluetoothHeadsetState(context, state == 2);
                broadcast = true;
            }

            if (state > 0)
                connected = true;

            // Used to inform interested activities that the headset state has changed
            if (broadcast)
            {
                LocalBroadcastManager.GetInstance(context).SendBroadcast(new Intent("headsetStateChange"));
                Xamarin.Forms.MessagingCenter.Send<SoundsOfSpacetime.Mobile.App, Boolean>((SoundsOfSpacetime.Mobile.App)Xamarin.Forms.Application.Current, "Headset", connected);
            }
        }
    }
}