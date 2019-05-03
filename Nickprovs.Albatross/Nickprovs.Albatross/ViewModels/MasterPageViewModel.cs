using Nickprovs.Albatross;
using Nickprovs.Albatross.Interfaces;
using Nickprovs.Albatross.Types;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Timers;
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

        /// <summary>
        /// The timer
        /// </summary>
        Timer _directorAnimationTimer = new Timer {  Interval = 7000, };

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

        /// <summary>
        /// Exposes whether or not the menu is visible.
        /// </summary>
        public bool IsMenuVisible
        {
            get { return this._isMenuVisible; }
            set { SetProperty(ref _isMenuVisible, value); }
        }

        /// <summary>
        /// Exposes app version information
        /// </summary>
        public IBindableVersionInfo VersionInfo { get; private set; }

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

        public MasterPageViewModel(INavigationService navigationService, IBindableVersionInfo versionInfo) : base(navigationService)
        {
            //Dependency Injection
            this.VersionInfo = versionInfo;

            //Set the title for the view
            this.Title = "Welcome";

            //Get the default icon resource from the resource dictionary. Note... only TryGetValue works in Xamarin Forms
            object iconPath;
            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;

            //Animation Timer
            Random random = new Random();
            int maxMilli = 7000;
            double rDub = random.NextDouble() * maxMilli;
            this._directorAnimationTimer.Interval = rDub;
            this._directorAnimationTimer.Elapsed += this.DirectorAnimationTimerElapsed;
            this._directorAnimationTimer.Start();

            //Commmand Wiring
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

        private async void DirectorAnimationTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this._directorAnimationTimer.Stop();

            var animationValues = Enum.GetValues(typeof(DirectorAnimationType));
            var animation = (DirectorAnimationType) animationValues.GetValue(new Random().Next(animationValues.Length));
            await this.PerformAnimationFromEnum(animation);

            Random random = new Random();
            int maxMilli = 7000;
            double rDub = random.NextDouble() * maxMilli;
            this._directorAnimationTimer.Interval = rDub;

            this._directorAnimationTimer.Start();

        }

        private async Task PerformAnimationFromEnum(DirectorAnimationType animationType)
        {
            switch(animationType)
            {
                case DirectorAnimationType.Happy:
                    await this.Happy();
                    break;
                case DirectorAnimationType.Angry:
                    await this.Anger();
                    break;
                case DirectorAnimationType.Half:
                    await this.Half();
                    break;
                case DirectorAnimationType.Ugh:
                    await this.Ugh();
                    break;
                case DirectorAnimationType.Squint:
                    await this.Squint();
                    break;
                case DirectorAnimationType.BlinkOnce:
                    await this.BlinkSlow();
                    break;
                case DirectorAnimationType.BlinkTwice:
                    await this.BlinkTwiceFast();
                    break;
                case DirectorAnimationType.Wink:
                    await this.Wink();
                    break;
            }
        }

        private async void OnIconTapped()
        {
            //await this.BlinkSlow();
            //await this.Half();
            //await this.BlinkTwiceFast();
            //await this.Happy();
            //await this.Squint();
            //await this.Wink();
            //await this.Ugh();
            //await this.Anger();
        }

        #region Animations

        private async Task Wink()
        {
            object iconPath;

            Application.Current.Resources.TryGetValue("director_winking1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking4", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking5", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking6", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking7", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winked", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(300);

            Application.Current.Resources.TryGetValue("director_winking7", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking6", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking5", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking4", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_winking1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(15);

            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;
        }

        private async Task BlinkSlow()
        {
            object iconPath;

            Application.Current.Resources.TryGetValue("director_blinking1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking4", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking5", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking6", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking7", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinked", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(100);

            Application.Current.Resources.TryGetValue("director_blinking7", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking6", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking5", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking4", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_blinking1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(20);

            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;
        }

        private async Task BlinkFast()
        {
            object iconPath;

            Application.Current.Resources.TryGetValue("director_blinking1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking4", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking5", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking6", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking7", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinked", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(120);

            Application.Current.Resources.TryGetValue("director_blinking7", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking6", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking5", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking4", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_blinking1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(12);

            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;
        }

        private async Task BlinkTwiceFast()
        {
            await this.BlinkFast();
            await Task.Delay(50);
            await this.BlinkFast();
        }

        private async Task Squint()
        {
            object iconPath;

            Application.Current.Resources.TryGetValue("director_squinting1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(30);

            Application.Current.Resources.TryGetValue("director_squinting2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(40);

            Application.Current.Resources.TryGetValue("director_squinting3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_squinted", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(500);

            Application.Current.Resources.TryGetValue("director_squinting3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_squinting2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(40);

            Application.Current.Resources.TryGetValue("director_squinting1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(30);

            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;
        }

        private async Task Happy()
        {
            object iconPath;

            Application.Current.Resources.TryGetValue("director_happying1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(30);

            Application.Current.Resources.TryGetValue("director_happying2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(40);

            Application.Current.Resources.TryGetValue("director_happying3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_happying4", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_happy", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(500);

            Application.Current.Resources.TryGetValue("director_happying4", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_happying3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_happying2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(40);

            Application.Current.Resources.TryGetValue("director_happying1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(30);

            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;
        }

        private async Task Ugh()
        {
            object iconPath;

            Application.Current.Resources.TryGetValue("director_ughing1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(30);

            Application.Current.Resources.TryGetValue("director_ughing2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(40);

            Application.Current.Resources.TryGetValue("director_ughing3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_ughed", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(500);

            Application.Current.Resources.TryGetValue("director_ughing3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_ughing2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(40);

            Application.Current.Resources.TryGetValue("director_ughing1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(30);

            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;
        }

        private async Task Half()
        {
            object iconPath;

            Application.Current.Resources.TryGetValue("director_halfing1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(30);

            Application.Current.Resources.TryGetValue("director_halfing2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(40);

            Application.Current.Resources.TryGetValue("director_halfing3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_halfed", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(500);

            Application.Current.Resources.TryGetValue("director_halfing3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_halfing2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(40);

            Application.Current.Resources.TryGetValue("director_halfing1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(30);

            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;
        }

        private async Task Anger()
        {
            object iconPath;

            Application.Current.Resources.TryGetValue("director_angering1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(30);

            Application.Current.Resources.TryGetValue("director_angering2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(40);

            Application.Current.Resources.TryGetValue("director_angering3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_angered", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(500);

            Application.Current.Resources.TryGetValue("director_angering3", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(50);

            Application.Current.Resources.TryGetValue("director_angering2", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(40);

            Application.Current.Resources.TryGetValue("director_angering1", out iconPath);
            this.IconPath = iconPath as string;
            await Task.Delay(30);

            Application.Current.Resources.TryGetValue("director_main", out iconPath);
            this.IconPath = iconPath as string;
        }

        #endregion

        #endregion

        #endregion
    }
}
