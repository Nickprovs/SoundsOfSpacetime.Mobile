using System;
using System.Collections.Generic;
using System.Text;

namespace SoundsOfSpacetime.Mobile.Interfaces
{
    public interface IGravitationalWaveCalculator
    {
        IGravitationalWaveData GenerateGravitationalWaveData(IGravitationalWaveInput input);
    }
}
