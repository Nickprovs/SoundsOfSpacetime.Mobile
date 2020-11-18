using System;
using System.Collections.Generic;
using System.Text;

namespace SoundsOfSpacetime.Mobile.Types.Audio.Wav
{
    /// <summary>
    /// Wraps the header portion of a WAVE file.
    /// </summary>
    public class WavHeader
    {
        public string sGroupID; // RIFF
        public uint dwFileLength; // total file length minus 8, which is taken up by RIFF
        public string sRiffType; // always WAVE

        /// <summary>
        /// Initializes a WaveHeader object with the default values.
        /// </summary>
        public WavHeader()
        {
            dwFileLength = 0;
            sGroupID = "RIFF";
            sRiffType = "WAVE";
        }
    }
}
