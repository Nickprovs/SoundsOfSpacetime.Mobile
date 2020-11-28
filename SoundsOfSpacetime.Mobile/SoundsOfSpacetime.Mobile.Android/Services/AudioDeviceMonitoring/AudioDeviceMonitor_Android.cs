using System;
using Android.Content;
using Android.Media;
using AndroidX.Media;
using SoundsOfSpacetime.Mobile.Interfaces;

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
                this._headphonesInUse = value; 
            }
            get 
            { 
                return this._headphonesInUse; 
            } 
        }


        #endregion

        #region Events

        public event EventHandler HeadphonesInUseChanged;

        #endregion

        #region Constructors and Destructors

        public AudioDeviceMonitor_Android()
        {
            Xamarin.Forms.MessagingCenter.Subscribe<SoundsOfSpacetime.Mobile.App, Boolean>((SoundsOfSpacetime.Mobile.App)Xamarin.Forms.Application.Current, "Headset", OnHeadphoneConnectedStatusChanged);
        }

        #endregion

        #region Non Public Methods

        private void OnHeadphoneConnectedStatusChanged(App app, bool connected)
        {
            this.HeadphonesInUse = connected;
            this.HeadphonesInUseChanged?.Invoke(this, new HeadphoneStatusChangedEventArgs(connected));
        }

        #endregion

    }
}