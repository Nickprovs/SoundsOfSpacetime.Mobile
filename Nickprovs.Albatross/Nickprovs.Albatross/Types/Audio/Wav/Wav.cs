using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Nickprovs.Albatross.Types.Audio.Wav
{
    public class Wav
    {
        #region Properties

        /// <summary>
        /// The header information about the wav object
        /// </summary>
        public WavHeader Header { get; private set; }

        /// <summary>
        /// The format information about the wav object
        /// </summary>
        public WavFormatChunk Format { get; private set; }

        /// <summary>
        /// The data within the wav object
        /// </summary>
        public WavDataChunk Data { get; private set; }

        /// <summary>
        /// The max amplitude of the wav object
        /// </summary>
        public double MaxAmplitude { get; private set;}

        #endregion

        #region Constructors and Destructors

        public Wav(double[] dataPoints, double duration, double maxAmplitude = 32760)
        {
            //Initialize the structure
            this.Header = new WavHeader();
            this.Format = new WavFormatChunk();
            this.Data = new WavDataChunk();
            this.MaxAmplitude = maxAmplitude;

            //Unsigned Values as these things can't be negative.
            uint unsignedTotalNumSamples = 0;
            uint unsignedSamplesPerSec = 0;
            try
            {
                unsignedTotalNumSamples = Convert.ToUInt32(dataPoints.Length);
                unsignedSamplesPerSec = Convert.ToUInt32(unsignedTotalNumSamples / (duration * 2));
            }
            catch(OverflowException e)
            {
                Debug.WriteLine($"Error converting int to uint in Wav Class: {e}");
            }

            //Popuplate are parts with some necessary info
            this.Format.dwSamplesPerSec = unsignedSamplesPerSec;
            this.Data.shortArray = new short[unsignedTotalNumSamples];

            //For all the data points
            for (uint i = 0; i < this.Data.shortArray.Length - 1; i++)
            {
                //Assign a point for each channel.
                for (int channel = 0; channel < this.Format.wChannels; channel++)
                {
                    var dataPointAtLocation = dataPoints[(int)i];
                    var result = this.MaxAmplitude * dataPointAtLocation;
                    var resultShort = Convert.ToInt16(result);
                    this.Data.shortArray[i + channel] = resultShort;
                }
            }

            //Calculate the data chunk size in bites.
            this.Data.dwChunkSize = (uint)(this.Data.shortArray.Length * (this.Format.wBitsPerSample / 8));
        }

        #endregion
    }
}
