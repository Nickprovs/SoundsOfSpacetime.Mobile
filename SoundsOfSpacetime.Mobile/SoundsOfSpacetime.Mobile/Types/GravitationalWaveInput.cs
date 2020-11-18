using SoundsOfSpacetime.Mobile.Interfaces;
using Prism.Mvvm;

namespace SoundsOfSpacetime.Mobile.Types
{
    public class GravitationalWaveInput : BindableBase, IGravitationalWaveInput
    {
        #region Fields

        private bool _inspiral;

        private bool _periastronPrecession;

        private double _solarMass1;

        private double _solarMass2;

        private double _initialEccentricity;

        private double _detectorAngleLittleTheta;

        private double _detectorAngleBigTheta;

        private double _detectorAngleLittlePhi;

        private double _detectorAngleBigPhi;

        private double _detectorAnglePsi;

        #endregion

        #region Properties

        public bool Inspiral
        {
            get { return this._inspiral; }
            set { this.SetProperty(ref this._inspiral, value); }
        }

        public bool PeriastronPrecession
        {
            get { return this._periastronPrecession; }
            set { this.SetProperty(ref this._periastronPrecession, value); }
        }

        public double SolarMass1
        {
            get { return this._solarMass1; }
            set { this.SetProperty(ref this._solarMass1, value); }
        }

        public double SolarMass2
        {
            get { return this._solarMass2; }
            set { this.SetProperty(ref this._solarMass2, value); }
        }

        public double InitialEccentricity
        {
            get { return this._initialEccentricity; }
            set { this.SetProperty(ref this._initialEccentricity, value); }
        }

        public double InitialEccentricityNonZero
        {
            get
            {
                if (this._initialEccentricity < 0.001)
                    return 0.001;
                else
                    return this._initialEccentricity;
            }
        }

        public double DetectorAngleLittleTheta
        {
            get { return this._detectorAngleLittleTheta; }
            set { this.SetProperty(ref this._detectorAngleLittleTheta, value); }
        }

        public double DetectorAngleBigTheta
        {
            get { return this._detectorAngleBigTheta; }
            set { this.SetProperty(ref this._detectorAngleBigTheta, value); }
        }

        public double DetectorAngleLittlePhi
        {
            get { return this._detectorAngleLittlePhi; }
            set { this.SetProperty(ref this._detectorAngleLittlePhi, value); }
        }

        public double DetectorAngleBigPhi
        {
            get { return this._detectorAngleBigPhi; }
            set { this.SetProperty(ref this._detectorAngleBigPhi, value); }
        }

        public double DetectorAnglePsi
        {
            get { return this._detectorAnglePsi; }
            set { this.SetProperty(ref this._detectorAnglePsi, value); }
        }
        #endregion

        #region Constructors and Destructors

        public GravitationalWaveInput(bool inspiral, bool periastronPrecession, double solarMass1, double solarMass2, double initialEccentricity,
                              double detectorAngleLittleTheta, double detectorAngleBigTheta, double detectorAngleLittlePhi, double detectorAngleBigPhi, double detectorAnglePsi)
        {
            this.Inspiral = inspiral;
            this.PeriastronPrecession = periastronPrecession;
            this.SolarMass1 = solarMass1;
            this.SolarMass2 = solarMass2;
            this.InitialEccentricity = initialEccentricity;
            this.DetectorAngleLittleTheta = detectorAngleLittleTheta;
            this.DetectorAngleBigTheta = detectorAngleBigTheta;
            this.DetectorAngleLittlePhi = detectorAngleLittlePhi;
            this.DetectorAngleBigPhi = detectorAngleBigPhi;
            this.DetectorAnglePsi = detectorAnglePsi;
        }

        #endregion

        #region Methods

        public bool Equals(IGravitationalWaveInput simulatorInput)
        {
            if (this.Inspiral == simulatorInput.Inspiral &&
               this.PeriastronPrecession == simulatorInput.PeriastronPrecession &&
               this.SolarMass1 == simulatorInput.SolarMass1 &&
               this.SolarMass2 == simulatorInput.SolarMass2 &&
               this.InitialEccentricity == simulatorInput.InitialEccentricity &&
               this.DetectorAngleLittleTheta == simulatorInput.DetectorAngleLittleTheta &&
               this.DetectorAngleBigTheta == simulatorInput.DetectorAngleBigTheta &&
               this.DetectorAngleLittlePhi == simulatorInput.DetectorAngleLittlePhi &&
               this.DetectorAngleBigPhi == simulatorInput.DetectorAngleBigPhi &&
               this.DetectorAnglePsi == simulatorInput.DetectorAnglePsi)
                return true;
            else
                return false;
        }

        public IGravitationalWaveInput DeepCopy()
        {
            return new GravitationalWaveInput(this.Inspiral, this.PeriastronPrecession, this.SolarMass1, this.SolarMass2, this.InitialEccentricity, 
                this.DetectorAngleLittleTheta, this.DetectorAngleBigTheta, this._detectorAngleLittlePhi, this.DetectorAngleBigPhi, this.DetectorAnglePsi);
        }

        #endregion

    }
}
