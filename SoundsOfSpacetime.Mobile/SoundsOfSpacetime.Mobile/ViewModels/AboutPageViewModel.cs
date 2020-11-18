using SoundsOfSpacetime.Mobile;
using Prism.Commands;
using Prism.Navigation;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SoundsOfSpacetime.Mobile.ViewModels
{
    public class AboutPageViewModel : BaseViewModel
    {
        #region Fields


        #endregion

        #region Properties

        public DelegateCommand LearnMoreCommand { get; }

        #endregion

        #region Constructors and Destructors

        public AboutPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.Title = "About";
            this.LearnMoreCommand = new DelegateCommand(this.OnLearnMore);
        }

        #endregion

        #region Private Methods

        private async void OnLearnMore()
        {
            await Launcher.OpenAsync(new Uri("https://www.soundsofspacetime.org/"));
        }

        #endregion
    }
}
