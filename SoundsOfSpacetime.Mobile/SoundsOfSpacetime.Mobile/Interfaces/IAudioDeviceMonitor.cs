using System;
using System.Collections.Generic;
using System.Text;

namespace SoundsOfSpacetime.Mobile.Interfaces
{
    public interface IAudioDeviceMonitor
    {
        bool HeadphonesInUse { get; }

        event EventHandler HeadphonesInUseChanged;
    }
}
