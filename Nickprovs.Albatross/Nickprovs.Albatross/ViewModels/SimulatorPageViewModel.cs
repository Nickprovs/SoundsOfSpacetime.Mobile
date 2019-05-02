using Nickprovs.Albatross.Controls;
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
        /// The path to the more options icon we want to be displayed on the view.
        /// </summary>
        private string _moreOptionsIconPath;

        #endregion

        #region Properties

        /// <summary>
        /// General information about the device we're running on.
        /// </summary>
        public IBindableDeviceInfo BindableDeviceInfo
        {
            get { return this._bindableDeviceInfo; }
            set { this.SetProperty(ref _bindableDeviceInfo, value); }
        }

        /// <summary>
        /// The path to the icon we want to be displayed on the view.
        /// </summary>
        public string MoreOptionsIconPath
        {
            get { return this._moreOptionsIconPath; }
            set { this.SetProperty(ref _moreOptionsIconPath, value); }
        }

        /// <summary>
        /// The command that should execute when the user affects the expansion status of the more simulator options.
        /// </summary>
        public DelegateCommand<object> MoreOptionsExpansionStatusChangedCommand { get; }

        #endregion

        #region Constructors and Destructors

        public SimulatorPageViewModel(INavigationService navigationService, IBindableDeviceInfo bindableDeviceInfo) : base(navigationService)
        {
            //Set the title for the page.
            this.Title = "Simulator";
            this.BindableDeviceInfo = bindableDeviceInfo;

            //Get the default icon resource from the resource dictionary. Note... only TryGetValue works in Xamarin Forms
            object iconPath;
            Application.Current.Resources.TryGetValue("chevron_right", out iconPath);
            this.MoreOptionsIconPath = iconPath as string;

            //Load the command with a handler
            this.MoreOptionsExpansionStatusChangedCommand = new DelegateCommand<object>(this.OnMoreOptionsExpansionStatusChanged);
        }

        #endregion

        #region Private Methods

        private void OnMoreOptionsExpansionStatusChanged(object newStatus)
        {
            var newStatusEnum = (ExpandStatus)newStatus;
            //If more options are not currently visible
            if(newStatusEnum == ExpandStatus.Expanding)
            {
                //Get and Set the chevron_down icon to show that we're going to have the more options view expanded.
                object iconPath;
                Application.Current.Resources.TryGetValue("chevron_down", out iconPath);
                this.MoreOptionsIconPath = iconPath as string;
                return;
            }

            //If more options are currently visible.
            else if(newStatusEnum == ExpandStatus.Collapsing)
            {
                //Get and Set the chevron_right icon to show that we're going to have the more options view hidden.
                object iconPath;
                Application.Current.Resources.TryGetValue("chevron_right", out iconPath);
                this.MoreOptionsIconPath = iconPath as string;
                return;
            }
        }

        #endregion
    }

}
