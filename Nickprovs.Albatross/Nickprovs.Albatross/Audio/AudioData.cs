using System;
using System.Collections.Generic;
using System.Text;

namespace Nickprovs.Albatross.Audio
{
    public class AudioData
    {
        public double[] dataPoints;
        public double tStop;
        public uint totalPoints;

        public AudioData(double[] userDataPoints, double userTStop, uint userTotalPoints)
        {
            dataPoints = userDataPoints;
            tStop = userTStop;
            totalPoints = userTotalPoints;
        }
    }
}
