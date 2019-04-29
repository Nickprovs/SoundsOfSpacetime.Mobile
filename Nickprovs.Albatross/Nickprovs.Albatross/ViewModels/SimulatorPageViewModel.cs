using Nickprovs.Albatross.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nickprovs.Albatross.ViewModels
{
    public class SimulatorPageViewModel : BaseViewModel
    {
        #region Fields

        /// <summary>
        /// General information about the device we're running on.
        /// </summary>
        private IBindableDeviceInfo _bindableDeviceInfo;

        /// <summary>
        /// The path to the icon we want to be displayed on the view.
        /// </summary>
        private string _iconPath;

        #endregion

        #region Properties

        /// <summary>
        /// General information about the device we're running on.
        /// </summary>
        public IBindableDeviceInfo BindableDeviceInfo
        {
            get { return this._bindableDeviceInfo; }
            set { SetProperty(ref _bindableDeviceInfo, value); }
        }

        /// <summary>
        /// The path to the icon we want to be displayed on the view.
        /// </summary>
        public string IconPath
        {
            get { return this._iconPath; }
            set { SetProperty(ref _iconPath, value); }
        }

        /// <summary>
        /// The command we fire when the icon is tapped.
        /// </summary>
        public DelegateCommand IconTappedCommand { get; }

        #endregion

        #region Constructors and Destructors

        public SimulatorPageViewModel(INavigationService navigationService, IBindableDeviceInfo bindableDeviceInfo) : base(navigationService)
        {
            //Set the title for the page.
            this.Title = "Simulator";
            this.BindableDeviceInfo = bindableDeviceInfo;

            //Get the default icon resource from the resource dictionary. Note... only TryGetValue works in Xamarin Forms
            object iconPath;
            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;

            //Load the command with a handler
            this.IconTappedCommand = new DelegateCommand(this.OnIconTapped);
        }

        #endregion

        #region Private Methods

        private async void OnIconTapped()
        {
            await this.BlinkTwice();
        }

        private async Task Wink()
        {
            object iconPath;

            Application.Current.Resources.TryGetValue("director_winking1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_winking2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_winking3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_winked", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(100);

            Application.Current.Resources.TryGetValue("director_winking3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_winking2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_winking1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;
        }

        private async Task Blink()
        {
            object iconPath;

            Application.Current.Resources.TryGetValue("director_closing1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_closing2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_closing3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_closed", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(100);

            Application.Current.Resources.TryGetValue("director_closing3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_closing2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_closing1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(25);

            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;
        }

        private async Task BlinkTwice()
        {
            await this.Blink();
            await Task.Delay(100);
            await this.Blink();
        }


        #endregion
    }

}
