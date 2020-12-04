using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SoundsOfSpacetime.Mobile.Droid.Utilities
{
    public static class AudioDeviceUtilities
    {
        public static bool IsHeadsetOn()
        {
            AudioManager am = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);

            if (am == null)
                return false;

            if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.M)
            {
                return am.WiredHeadsetOn || am.BluetoothScoOn || am.BluetoothA2dpOn;
            }
            else
            {
                AudioDeviceInfo[] devices = am.GetDevices(GetDevicesTargets.Outputs);

                foreach (var device in devices)
                {
                    if (device.Type == AudioDeviceType.WiredHeadset
                            || device.Type == AudioDeviceType.WiredHeadphones
                            || device.Type == AudioDeviceType.BluetoothSco
                            || device.Type == AudioDeviceType.BluetoothA2dp)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsWiredHeadsetOn()
        {
            AudioManager am = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);

            if (am == null)
                return false;

            if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.M)
            {
                return am.WiredHeadsetOn;
            }
            else
            {
                AudioDeviceInfo[] devices = am.GetDevices(GetDevicesTargets.Outputs);

                foreach (var device in devices)
                {
                    if (device.Type == AudioDeviceType.WiredHeadset)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}