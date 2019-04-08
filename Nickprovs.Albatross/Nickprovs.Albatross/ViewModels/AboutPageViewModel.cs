using Nickprovs.Albatross;
using Prism.Commands;
using Prism.Navigation;
using System;
using Xamarin.Forms;

namespace Nickprovs.Albatross.ViewModels
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

        private void OnLearnMore()
        {
            Device.OpenUri(new Uri("https://www.soundsofspacetime.org/"));
        }

        #endregion
    }
}
