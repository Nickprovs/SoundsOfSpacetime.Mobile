using System;
using System.Collections.Generic;
using System.Text;

namespace Nickprovs.Albatross.Controls
{
    public sealed class StatusChangedEventArgs : EventArgs
    {
        public StatusChangedEventArgs(ExpandStatus status)
        {
            Status = status;
        }

        public ExpandStatus Status { get; }
    }
}
