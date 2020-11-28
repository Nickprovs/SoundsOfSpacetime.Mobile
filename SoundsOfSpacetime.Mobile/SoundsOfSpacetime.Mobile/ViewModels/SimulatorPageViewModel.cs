using Expandable;
using MediaManager;
using SoundsOfSpacetime.Mobile.Interfaces;
using SoundsOfSpacetime.Mobile.Types;
using SoundsOfSpacetime.Mobile.Types.Audio.Wav;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SoundsOfSpacetime.Mobile.ViewModels
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

        /// <summary>
        /// The current simulator input
        /// </summary>
        private IGravitationalWaveInput _currentSimulatorInput;

        /// <summary>
        /// The previous simulator input
        /// </summary>
        private IGravitationalWaveInput _previousSimulatorInput;

        /// <summary>
        /// The native implementation of the plotting service.
        /// </summary>
        private IPlotService _plotService;

        /// <summary>
        /// The file system path service
        /// </summary>
        private IFileSystemPathService _fileSystemPathService;

        /// <summary>
        /// The gravitational wave calculator
        /// </summary>
        private IGravitationalWaveCalculator _gravitationalWaveCalculator;

        /// <summary>
        /// The audio device monitor
        /// </summary>
        private IAudioDeviceMonitor _audioDeviceMonitor;

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
        /// The current simulator input
        /// </summary>
        public IGravitationalWaveInput CurrentSimulatorInput
        {
            get { return this._currentSimulatorInput; }
            set { this.SetProperty(ref _currentSimulatorInput, value); }
        }

        /// <summary>
        /// The simulator input of the last generated event
        /// </summary>
        public IGravitationalWaveInput PreviousSimulatorInput
        {
            get { return this._previousSimulatorInput; }
            set { this.SetProperty(ref _previousSimulatorInput, value); }
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

        /// <summary>
        /// Tells us to simulate a binary's gravitational wave based on input.
        /// </summary>
        public DelegateCommand SimulateWaveCommand { get; }

        /// <summary>
        /// Tells us to simulate a binary's orbit based on input.
        /// </summary>
        public DelegateCommand SimulateOrbitCommand { get; }

        #endregion

        #region Constructors and Destructors

        public SimulatorPageViewModel(INavigationService navigationService, IBindableDeviceInfo bindableDeviceInfo, IGravitationalWaveCalculator gravitationalWaveCalculator, IAudioDeviceMonitor audioDeviceMonitor) : base(navigationService)
        {
            //Dependency Injection
            this.BindableDeviceInfo = bindableDeviceInfo;
            this._gravitationalWaveCalculator = gravitationalWaveCalculator;
            this._plotService = DependencyService.Resolve<IPlotService>();
            this._fileSystemPathService = DependencyService.Resolve<IFileSystemPathService>();
            this._audioDeviceMonitor = audioDeviceMonitor;

            //Set the title for the page.
            this.Title = "Simulator";

            //Configure the default simulator input values
            this.CurrentSimulatorInput = new GravitationalWaveInput(true, true, 3, 3, 0, 0, 0, 0, 0, 0);

            //Get the default icon resource from the resource dictionary. Note... only TryGetValue works in Xamarin Forms
            object iconPath;
            Application.Current.Resources.TryGetValue("chevron_right", out iconPath);
            this.MoreOptionsIconPath = iconPath as string;

            //Command Wiring
            this.SimulateWaveCommand = new DelegateCommand(this.OnSimulateWave);
            this.SimulateOrbitCommand = new DelegateCommand(this.OnSimulateOrbit);
            this.MoreOptionsExpansionStatusChangedCommand = new DelegateCommand<object>(this.OnMoreOptionsExpansionStatusChanged);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This will simulate a binary's gravitational wave based on current input
        /// </summary>
        private async void OnSimulateWave()
        {
            //Asynchronously calculate the event based on user data.
            var data = await Task.Run(() => this._gravitationalWaveCalculator.GenerateGravitationalWaveData(CurrentSimulatorInput));

            //Create a .wav file based on the wave
            //TODO: Enumerate somewhere before this to eliminate multiple enumeration
            var soundFile = new Wav(data.Wave.Select(point => point.Y).ToArray(), data.Wave.Select(point => point.X).LastOrDefault());

            //TODO: Save to local app private data instead.
            //if (CrossMediaManager.Current.IsPlaying())
            //    await CrossMediaManager.Current.Stop();

            await this.PlaySoundFile(soundFile);

            //Plot the wave
            this._plotService.SetXAxisTitle("t (sec)");
            this._plotService.SetYAxisTitle("h(t)");
            this._plotService.SetTitle("Wave");
            this._plotService.PlotAnimated(data.Wave, data.Wave.Select(point => point.X).LastOrDefault() * 1000);

            //Cache this last simulator input          
            this.PreviousSimulatorInput = this.CurrentSimulatorInput;

            //Create a copy of this last simulator input so previous / current simulator inputs are different objects.
            this.CurrentSimulatorInput = this.CurrentSimulatorInput.DeepCopy();
        }

        /// <summary>
        /// This will simulate a binary's orbit based on current input
        /// </summary>
        private async void OnSimulateOrbit()
        {            
            //Asynchronously calculate the event based on user data.
            var data = await Task.Run(()=> this._gravitationalWaveCalculator.GenerateGravitationalWaveData(CurrentSimulatorInput));

            var soundFile = new Wav(data.Wave.Select(point => point.Y).ToArray(), data.Wave.Select(point => point.X).LastOrDefault());
            await this.PlaySoundFile(soundFile);

            //Plot the orbit... Take the last x in the wave series with respect to the total orbit points (done in case we don't display full orbit for performance reasons)
            this._plotService.SetXAxisTitle("x");
            this._plotService.SetYAxisTitle("y");
            this._plotService.SetTitle("Orbit");
            this._plotService.PlotAnimated(data.Orbit, data.Wave.Select(point => point.X).Take(data.Orbit.Count()).LastOrDefault() * 1000);

            //Cache this last simulator input          
            this.PreviousSimulatorInput = this.CurrentSimulatorInput;

            //Create a copy of this last simulator input so previous / current simulator inputs are different objects.
            this.CurrentSimulatorInput = this.CurrentSimulatorInput.DeepCopy();
        }

        private async Task PlaySoundFile(Wav wav)
        {
            var folderPath = this._fileSystemPathService.GetDownloadsPath();
            var filePath = Path.Combine(folderPath, "tone.wav");
            wav.SaveToFile(filePath);
            await CrossMediaManager.Current.Play(filePath);
        }

        /// <summary>
        /// When the expansion status of the more options section changes, we change the expansion icon.
        /// </summary>
        /// <param name="newStatus"></param>
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
