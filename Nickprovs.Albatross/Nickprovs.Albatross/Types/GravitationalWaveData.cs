using Nickprovs.Albatross.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nickprovs.Albatross.Types
{
    public class GravitationalWaveData : IGravitationalWaveData
    {
        public IEnumerable<IPoint> Wave { get; set; }

        public IEnumerable<IPoint> Orbit { get; set; }

        public GravitationalWaveData(IEnumerable<IPoint> wave, IEnumerable<IPoint> orbit)
        {
            this.Wave = wave;
            this.Orbit = orbit;
        }

    }
}
