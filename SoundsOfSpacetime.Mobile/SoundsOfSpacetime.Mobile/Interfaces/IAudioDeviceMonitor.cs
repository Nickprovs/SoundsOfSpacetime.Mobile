using SoundsOfSpacetime.Mobile.Events.Args;
using System;
using System.ComponentModel;

namespace SoundsOfSpacetime.Mobile.Interfaces
{
    public interface IAudioDeviceMonitor : INotifyPropertyChanged
    {
        bool HeadphonesInUse { get; }

        event EventHandler<HeadphoneStatusChangedEventArgs> HeadphonesInUseChanged;
    }
}
