using System;
using System.Collections.Generic;
using System.Text;

namespace SoundsOfSpacetime.Mobile.Types.Audio.Wav
{
    /// <summary>
    /// Wraps the Data chunk of a wave file.
    /// </summary>
    public class WavDataChunk
    {
        public string sChunkID;     // "data"
        public uint dwChunkSize;    // Length of header in bytes
        public short[] shortArray;  // 8-bit audio

        /// <summary>
        /// Initializes a new data chunk with default values.
        /// </summary>
        public WavDataChunk()
        {
            shortArray = new short[0];
            dwChunkSize = 0;
            sChunkID = "data";
        }
    }
}
