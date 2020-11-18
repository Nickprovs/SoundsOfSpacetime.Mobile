using Prism.Mvvm;
using SoundsOfSpacetime.Mobile.Interfaces;
using Xamarin.Essentials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SoundsOfSpacetime.Mobile.Utilities
{
    public class BindableDeviceInfo : BindableBase, IBindableDeviceInfo
    {

        #region Fields

        private double _width;

        private double _height;

        private DisplayOrientation _orientation;

        #endregion


        #region Properties

        public double Width
        {
            get { return this._width; }
            set { this.SetProperty(ref this._width, value); }
        }

        public double Height
        {
            get { return this._height; }
            set { this.SetProperty(ref this._height, value); }
        }

        public DisplayOrientation Orientation
        {
            get { return this._orientation; }
            set { this.SetProperty(ref this._orientation, value); }
        }

        #endregion

        #region Constructors and Destructors

        public BindableDeviceInfo()
        {
            DeviceDisplay.MainDisplayInfoChanged += MainDisplayInfoChanged;
            
        }

        private void MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            this.Height = DeviceDisplay.MainDisplayInfo.Height;
            this.Width = DeviceDisplay.MainDisplayInfo.Width;
            this.Orientation = DeviceDisplay.MainDisplayInfo.Orientation;
            Debug.WriteLine($"Width: {this.Width}, Height: {this.Height}, Orientation: {this.Orientation.ToString()}");
        }

        #endregion
    }
}
