using Nickprovs.Albatross;
using Prism.Commands;
using Prism.Navigation;

namespace Nickprovs.Albatross.ViewModels
{
    public class MasterPageViewModel : BaseViewModel
    {
        #region Fields


        #endregion

        #region Properties

        public DelegateCommand<string> NavigateCommand { get; }

        #endregion

        #region Constructors and Destructors

        public MasterPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.Title = "Welcome";

            this.NavigateCommand = new DelegateCommand<string>(this.OnNavigateCommandExecuted);
        }

        #endregion

        #region Private Methods

        private async void OnNavigateCommandExecuted(string path)
        {
            await _navigationService.NavigateAsync(path);
        }

        #endregion
    }
}
