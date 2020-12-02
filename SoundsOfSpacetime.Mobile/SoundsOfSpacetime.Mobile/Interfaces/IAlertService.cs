using System;
using System.Collections.Generic;
using System.Text;

namespace SoundsOfSpacetime.Mobile.Interfaces
{
    public interface IAlertService
    {
        void ShowAlert(string message, TimeSpan timespan, bool gestureDismissable = true);
    }
}
