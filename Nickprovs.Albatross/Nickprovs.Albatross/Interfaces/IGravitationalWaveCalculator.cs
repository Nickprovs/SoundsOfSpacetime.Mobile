using System;
using System.Collections.Generic;
using System.Text;

namespace Nickprovs.Albatross.Interfaces
{
    public interface IGravitationalWaveCalculator
    {
        IGravitationalWaveData GenerateGravitationalWaveData(IGravitationalWaveInput input);
    }
}
