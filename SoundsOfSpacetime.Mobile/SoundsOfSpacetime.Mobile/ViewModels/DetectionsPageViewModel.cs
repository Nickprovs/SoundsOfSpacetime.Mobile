using SoundsOfSpacetime.Mobile;
using Prism.Commands;
using Prism.Navigation;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SoundsOfSpacetime.Mobile.ViewModels
{
    public class DetectionsPageViewModel : BaseViewModel
    {
        #region Fields


        #endregion

        #region Properties


        #endregion

        #region Constructors and Destructors

        public DetectionsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.Title = "Detections";
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
