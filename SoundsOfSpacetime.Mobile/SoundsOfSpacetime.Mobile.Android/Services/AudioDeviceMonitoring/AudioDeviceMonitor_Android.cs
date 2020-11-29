using System;
using System.ComponentModel;
using Android.Bluetooth;
using Android.Content;
using Android.Media;
using Android.OS;
using AndroidX.Media;
using SoundsOfSpacetime.Mobile.Events.Args;
using SoundsOfSpacetime.Mobile.Interfaces;
using Xamarin.Forms;

namespace SoundsOfSpacetime.Mobile.Droid.Services.AudioDeviceMonitoring
{
    [BroadcastReceiver(Enabled = true, Exported = false)]

    public class AudioDeviceMonitor_Android :  IAudioDeviceMonitor
    {
        #region Fields

        private bool _headphonesInUse;

        #endregion

        #region Properties

        public bool HeadphonesInUse 
        { 
            private set 
            { 
                if(this._headphonesInUse != value)
                {
                    this._headphonesInUse = value;
                    this.OnPropertyChanged(nameof(this.HeadphonesInUse));
                }
            }
            get 
            { 
                return this._headphonesInUse; 
            } 
        }


        #endregion

        #region Events

        public event EventHandler<HeadphoneStatusChangedEventArgs> HeadphonesInUseChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors and Destructors

        public AudioDeviceMonitor_Android()
        {
            MessagingCenter.Subscribe<SoundsOfSpacetime.Mobile.App, Boolean>((SoundsOfSpacetime.Mobile.App) Application.Current, "Headset", OnHeadphoneConnectedStatusChanged);
        }

        #endregion

        #region Non Public Methods

        private void OnHeadphoneConnectedStatusChanged(App app, bool connected)
        { 
            //The or is necessary because we often receive headset changed messages on startup that are not accurate. Although further messages are.
            //IsHeadsetOn appears to always be accurate.
            this.HeadphonesInUse = connected || this.IsHeadsetOn();
            this.HeadphonesInUseChanged?.Invoke(this, new HeadphoneStatusChangedEventArgs(this.HeadphonesInUse));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsHeadsetOn()
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

        #endregion
    }
}