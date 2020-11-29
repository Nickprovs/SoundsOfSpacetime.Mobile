using AVFoundation;
using Foundation;
using SoundsOfSpacetime.Mobile.Events.Args;
using SoundsOfSpacetime.Mobile.Interfaces;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace SoundsOfSpacetime.Mobile.iOS.Services
{
    public class AudioDeviceMonitor_iOS :  IAudioDeviceMonitor
    {
        #region Fields

        private bool _headphonesInUse;

        #endregion

        #region Properties

        public bool HeadphonesInUse
        {
            private set
            {
                if (this._headphonesInUse != value)
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

        public AudioDeviceMonitor_iOS()
        {
            var outputs = AVAudioSession.SharedInstance().CurrentRoute.Outputs;

            //It would be better to hook into the audio session manager but I can't get this to work.
            //var test= AVAudioSession.SharedInstance().AddObserver("AVAudioSessionRouteChange", NSKeyValueObservingOptions.New, this.OnAudioSessionRouteChange);
            NSNotificationCenter.DefaultCenter.AddObserver(AVAudioSession.RouteChangeNotification, OnAudioSessionRouteChange);

            this.HeadphonesInUse = this.IsHeadphonesConnected();
            this.HeadphonesInUseChanged?.Invoke(this, new HeadphoneStatusChangedEventArgs(this.HeadphonesInUse));
        }

        #endregion

        #region Non Public Methods
        private bool IsHeadphonesConnected()
        {
            var outputs = AVAudioSession.SharedInstance().CurrentRoute.Outputs;
            foreach(var output in outputs)
            {
                if (output.PortType == AVAudioSession.PortBluetoothA2DP ||
                   output.PortType == AVAudioSession.PortBluetoothHfp ||
                   output.PortType == AVAudioSession.PortBluetoothLE ||
                   output.PortType == AVAudioSession.PortHeadphones)
                    return true;
            }
            return false;
        }

        private void OnAudioSessionRouteChange(NSNotification change)
        {
            this.HeadphonesInUse = this.IsHeadphonesConnected();
        }

        private void OnHeadphoneConnectedStatusChanged(App app, bool connected)
        {
            this.HeadphonesInUse = connected;
            this.HeadphonesInUseChanged?.Invoke(this, new HeadphoneStatusChangedEventArgs(connected));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
