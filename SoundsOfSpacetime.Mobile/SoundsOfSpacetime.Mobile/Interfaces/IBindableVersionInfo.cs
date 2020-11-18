using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SoundsOfSpacetime.Mobile.Interfaces
{
    public interface IBindableVersionInfo : INotifyPropertyChanged
    {
        string CurrentVersion { get; }
        string CurrentBuild { get; }
    }
}
