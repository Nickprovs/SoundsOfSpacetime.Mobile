using System;

namespace SoundsOfSpacetime.Mobile.Events.Args
{
    public class HeadphoneStatusChangedEventArgs : EventArgs
    {
        public bool Connected { get; }

        public HeadphoneStatusChangedEventArgs(bool connected)
        {
            this.Connected = connected;
        }
    }
}