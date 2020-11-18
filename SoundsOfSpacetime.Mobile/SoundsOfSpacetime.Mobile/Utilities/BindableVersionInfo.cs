using SoundsOfSpacetime.Mobile.Interfaces;
using Xamarin.Essentials;
using Prism.Mvvm;

namespace SoundsOfSpacetime.Mobile.Utilities
{
    public class BindableVersionInfo : BindableBase, IBindableVersionInfo
    {
        #region Fields

        private string _currentVersion;

        private string _currentBuild;

        #endregion

        #region Properties

        public string CurrentVersion
        {
            get { return this._currentVersion; }
            set { this.SetProperty(ref this._currentVersion, value); }
        }

        public string CurrentBuild
        {
            get { return this._currentBuild; }
            set { this.SetProperty(ref this._currentBuild, value); }
        }

        #endregion

        #region Constructors and Destructors

        public BindableVersionInfo()
        {
            //Initializes version tracking information.
            VersionTracking.Track();

            this.CurrentVersion = VersionTracking.CurrentVersion;
            this.CurrentBuild = VersionTracking.CurrentBuild;         
        }

        #endregion


    }
}
