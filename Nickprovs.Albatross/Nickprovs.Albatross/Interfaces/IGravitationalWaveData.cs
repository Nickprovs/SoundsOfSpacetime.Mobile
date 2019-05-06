using System;
using System.Collections.Generic;
using System.Text;

namespace Nickprovs.Albatross.Interfaces
{
    public interface IGravitationalWaveData
    {
        IEnumerable<IPoint> Orbit { get; set; }

        IEnumerable<IPoint> Wave { get; set; }
    }
}
