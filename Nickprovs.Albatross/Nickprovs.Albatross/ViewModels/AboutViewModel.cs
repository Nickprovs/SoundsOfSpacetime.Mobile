using Nickprovs.Albatross;
using Prism.Commands;
using Prism.Navigation;
using System;
using Xamarin.Forms;

namespace Nickprovs.Albatross.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        #region Fields


        #endregion

        #region Properties

        public DelegateCommand LearnMoreCommand { get; }

        #endregion

        #region Constructors and Destructors

        public AboutViewModel(INavigationService navigationService) : base(navigationService)
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
