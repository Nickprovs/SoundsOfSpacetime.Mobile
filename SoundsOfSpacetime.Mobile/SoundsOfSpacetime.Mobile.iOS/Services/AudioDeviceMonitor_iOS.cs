using SoundsOfSpacetime.Mobile.Events.Args;
using SoundsOfSpacetime.Mobile.Interfaces;
using System;
using System.ComponentModel;

namespace SoundsOfSpacetime.Mobile.iOS.Services
{
    public class AudioDeviceMonitor_iOS : IAudioDeviceMonitor
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
        }

        #endregion

        #region Non Public Methods

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
