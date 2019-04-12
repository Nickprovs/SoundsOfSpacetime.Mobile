using Nickprovs.Albatross;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nickprovs.Albatross.ViewModels
{
    public class MasterPageViewModel : BaseViewModel
    {
        #region Fields

        /// <summary>
        /// The path to the icon we want to be displayed on the view.
        /// </summary>
        private string _iconPath;

        /// <summary>
        /// Describes whether or not the menu is visible.
        /// </summary>
        private bool _isMenuVisible;

        #endregion

        #region Properties

        /// <summary>
        /// The path to the icon we want to be displayed on the view.
        /// </summary>
        public string IconPath
        {
            get { return this._iconPath; }
            set { SetProperty(ref _iconPath, value); }
        }

        public bool IsMenuVisible
        {
            get { return this._isMenuVisible; }
            set { SetProperty(ref _isMenuVisible, value); }
        }

        /// <summary>
        /// An event to command scenario so we know when the menu status changes.
        /// </summary>
        public DelegateCommand MenuPresentationChangedCommand { get; }

        /// <summary>
        /// The command we fire when the icon is tapped.
        /// </summary>
        public DelegateCommand IconTappedCommand { get; }

        /// <summary>
        /// The navigate command used to change the detail view.
        /// </summary>
        public DelegateCommand<string> NavigateCommand { get; }

        #endregion

        #region Constructors and Destructors

        public MasterPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.Title = "Welcome";

            //Get the default icon resource from the resource dictionary. Note... only TryGetValue works in Xamarin Forms
            object iconPath;
            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;

            this.MenuPresentationChangedCommand = new DelegateCommand(this.OnMenuPresentationStatusChanged);
            this.NavigateCommand = new DelegateCommand<string>(this.OnNavigateCommandExecuted);
            this.IconTappedCommand = new DelegateCommand(this.OnIconTapped);
        }

        #endregion

        #region Private Methods

        private async void OnNavigateCommandExecuted(string path)
        {
            var status = await _navigationService.NavigateAsync(path);
        }

        #region Menu Status

        private void OnMenuPresentationStatusChanged()
        {
            //TODO: If presented... start animating icon
            //TODO: Stop Animating icon
        }

        #endregion

        #region Menu Icon Animation

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

        #endregion
    }
}
