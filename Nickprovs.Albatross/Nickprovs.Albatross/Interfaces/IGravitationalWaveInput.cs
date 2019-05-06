using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Nickprovs.Albatross.Interfaces
{
    public interface IGravitationalWaveInput : INotifyPropertyChanged
    {
        bool Inspiral { get; set; }

        bool PeriastronPrecession { get; set; }

        double SolarMass1 { get; set; }

        double SolarMass2 { get; set; }

        double InitialEccentricity { get; set; }

        double DetectorAngleLittleTheta { get; set; }

        double DetectorAngleBigTheta { get; set; }

        double DetectorAngleLittlePhi { get; set; }

        double DetectorAngleBigPhi { get; set; }

        double DetectorAnglePsi { get; set; }

        bool Equals(IGravitationalWaveInput simulatorInput);

        IGravitationalWaveInput DeepCopy();
    }
}
