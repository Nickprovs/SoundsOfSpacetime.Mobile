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
using Android.Bluetooth;
using SoundsOfSpacetime.Mobile.Droid.Utilities;

namespace SoundsOfSpacetime.Mobile.Droid.Services.AudioDeviceMonitoring
{
    [BroadcastReceiver(Enabled = true, Exported = false)]

    public class HeadsetStateBroadcastReceiver : BroadcastReceiver
    {

        public readonly static String[] HEADPHONE_ACTIONS = { Intent.ActionHeadsetPlug, BluetoothAdapter.ActionStateChanged, "android.bluetooth.headset.action.STATE_CHANGED",
        "android.bluetooth.headset.profile.action.CONNECTION_STATE_CHANGED"};


        public override void OnReceive(Context context, Intent intent)
        {
            bool broadcast = false;
            bool connected = false;

            ////Bluetooth Ability turning on or off
            if (intent.Action.Equals(HEADPHONE_ACTIONS[1]))
            {
                int bluetoothStateInt = intent.GetIntExtra(BluetoothAdapter.ExtraState, BluetoothAdapter.Error);
                var blueToothStateParse = Enum.TryParse<State>(bluetoothStateInt.ToString(), out State bluetoothState);

                if (blueToothStateParse)
                {
                    switch (bluetoothState)
                    {
                        case Android.Bluetooth.State.Off:
                            connected = AudioDeviceUtilities.IsWiredHeadsetOn();
                            broadcast = true;
                            break;
                    }
                }
            }

            ////Headset (wired or bluetooth connecting / disconnecting). 1 means connected. 0 means not connected
            else
            {
                int state = 0;
                //Headset monitoring
                if (intent.Action.Equals(HEADPHONE_ACTIONS[0]))
                {
                    state = intent.GetIntExtra("state", 0);
                    //AudioPreferences.setWiredHeadphoneState(context, state > 0);
                    broadcast = true;
                }

                //Bluetooth monitoring from Android 1-Honeycomb
                if (intent.Action.Equals(HEADPHONE_ACTIONS[2]))
                {
                    state = intent.GetIntExtra("android.bluetooth.headset.extra.STATE", 0);
                    //AudioPreferences.setBluetoothHeadsetState(context, state == 2);
                    broadcast = true;
                }

                // Works for Ice Cream Sandwich-Beyond
                if (intent.Action.Equals(HEADPHONE_ACTIONS[3]))
                {
                    state = intent.GetIntExtra("android.bluetooth.profile.extra.STATE", 0);
                    //AudioPreferences.setBluetoothHeadsetState(context, state == 2);
                    broadcast = true;
                }
                if (state > 0)
                    connected = true;
            }


            // Used to inform interested activities that the headset state has changed
            if (broadcast)
            {
                LocalBroadcastManager.GetInstance(context).SendBroadcast(new Intent("headsetStateChange"));
                Xamarin.Forms.MessagingCenter.Send<SoundsOfSpacetime.Mobile.App, Boolean>((SoundsOfSpacetime.Mobile.App)Xamarin.Forms.Application.Current, "Headset", connected);
            }      
        }
    }
}