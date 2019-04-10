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
            this._navigationService.NavigateAsync("Navigation/Simulator");
        }

        #endregion

        #region Private Methods

        private async void OnNavigateCommandExecuted(string path)
        {
            var status = await _navigationService.NavigateAsync(path);
        }

        #endregion
    }
}
