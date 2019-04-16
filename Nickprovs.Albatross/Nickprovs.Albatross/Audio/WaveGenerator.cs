using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;


namespace Nickprovs.Albatross.Audio
{
    //-----------------------------------------------------
    // WaveGenerator.cs
    // A structure that wraps a wave file, generates 
    //   audio data and saves it to a file
    //
    // Copyright (c) 2009 Dan Waters
    // This code is provided AS-IS and is written by me,
    // with no endorsement from Microsoft.
    //-----------------------------------------------------

    /// <summary>
    /// Possible example waves to generate
    /// </summary>
    public enum WaveExampleType
    {
        ExampleSineWave = 0,
        Circular = 1
    }

    /// <summary>
    /// Wraps a WAV file struture and auto-generates some canned waveforms.
    /// </summary>
    public class WaveGenerator
    {
        // Header, Format, Data chunks
        WaveHeader header;
        WaveFormatChunk format;
        WaveDataChunk data;

        /// <summary>
        /// Initializes the object and generates a wave.
        /// </summary>
        /// <param name="type">The type of wave to generate</param>
        public WaveGenerator(WaveExampleType type, double freq, int amp, uint seconds)
        {
            // Init chunks
            header = new WaveHeader();
            format = new WaveFormatChunk();
            data = new WaveDataChunk();

            // Fill the data array with sample data
            switch (type)
            {
                case WaveExampleType.ExampleSineWave:

                    // Number of samples = sample rate * channels * bytes per sample
                    uint numSamples = format.dwSamplesPerSec * format.wChannels * seconds;

                    // Initialize the 16-bit array
                    data.shortArray = new short[numSamples];

                    int amplitude = amp;  //32760 Max amplitude for 16-bit audio

                    // The "angle" used in the function, adjusted for the number of channels and sample rate.
                    // This value is like the period of the wave.
                    double t = (Math.PI * 2 * freq) / (format.dwSamplesPerSec * format.wChannels);

                    for (uint i = 0; i < numSamples - 1; i++)
                    {
                        // Fill with a simple sine wave at max amplitude
                        for (int channel = 0; channel < format.wChannels; channel++)
                        {
                            data.shortArray[i + channel] = Convert.ToInt16(amplitude * Math.Sin(t * i));
                        }
                    }

                    // Calculate data chunk size in bytes
                    data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));

                    break;
            }
        }

        public WaveGenerator(WaveExampleType type, List<Double> datapoints, double seconds, uint totalPoints)
        {
            // Init chunks
            header = new WaveHeader();
            format = new WaveFormatChunk();
            data = new WaveDataChunk();

            // Fill the data array with sample data

            switch (type)
            {
                case WaveExampleType.Circular:

                    //dont take shit in a seconds to compute points.... take in as regular seconds... do some calculation... then convert to uint second
                    // Number of samples = sample rate * channels * bytes per sample
                    uint numSamples = totalPoints;


                    double dubsps = (totalPoints / (seconds * 2));
                    format.dwSamplesPerSec = (uint)(dubsps);


                    // Initialize the 16-bit array
                    data.shortArray = new short[numSamples];

                    int amplitude = 32760;  //32760 Max amplitude for 16-bit audio


                    try
                    {
                        for (uint i = 0; i < numSamples - 1; i++)
                        {
                            // Fill with a simple sine wave at max amplitude
                            for (int channel = 0; channel < format.wChannels; channel++)
                            {
                                data.shortArray[i + channel] = Convert.ToInt16(amplitude * datapoints.ElementAt((int)i));
                            }
                        }
                    }

                    catch (Exception e)
                    {

                    }

                    // Calculate data chunk size in bytes
                    data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));

                    break;
            }
        }
        //Used in newest page
        public WaveGenerator(AudioData audioData)
        {
            // Init chunks
            header = new WaveHeader();
            format = new WaveFormatChunk();
            data = new WaveDataChunk();

            //dont take shit in a seconds to compute points.... take in as regular seconds... do some calculation... then convert to uint second
            // Number of samples = sample rate * channels * bytes per sample
            uint numSamples = audioData.totalPoints;
            double seconds = audioData.tStop;

            double dubsps = (numSamples / (seconds * 2));
            format.dwSamplesPerSec = (uint)(dubsps);


            // Initialize the 16-bit array
            data.shortArray = new short[numSamples];

            int amplitude = 32760;  //32760 Max amplitude for 16-bit audio


            try
            {
                for (uint i = 0; i < numSamples - 1; i++)
                {
                    // Fill with a simple sine wave at max amplitude
                    for (int channel = 0; channel < format.wChannels; channel++)
                    {
                        data.shortArray[i + channel] = Convert.ToInt16(amplitude * audioData.dataPoints[(int)i]);
                    }
                }
            }

            catch (Exception e)
            {

            }

            // Calculate data chunk size in bytes
            data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));
        }

        public WaveGenerator(WaveExampleType type, double mass1, double mass2, double reducedMassRatio, double totalMass, double chirpMass, double startFrequency, double endFrequency)
        {
            // Init chunks
            header = new WaveHeader();
            format = new WaveFormatChunk();
            data = new WaveDataChunk();


            double tcoal = (5 / 256f) * (Math.Pow((Math.PI * startFrequency), (-8 / 3f))) * Math.Pow(chirpMass, (-5 / 3f)); //This is the time at which the frequency approaches infinity.
            double tstop = tcoal - ((5 / 256f) * Math.Pow((Math.PI * endFrequency), (-8 / 3f)) * Math.Pow(chirpMass, (-5 / 3f))); //This is the time at which we want to stop... just before we approach infinity
            double tauEND = tcoal - tstop; //Tau represents the difference between the coalescing time and the stop time.

            uint seconds = 0; //This value represents the total length in time of our .wav file
            if (tstop < 1) seconds = 1; //A .wav file's lengths has to be an integer. So if our wave takes less than that to reach peak frequency... just set file length to 1 second.
            else seconds = Convert.ToUInt32(tstop);

            // Fill the data array with sample data
            switch (type)
            {
                case WaveExampleType.Circular:

                    // Number of samples = sample rate * channels * bytes per sample
                    uint numSamples = format.dwSamplesPerSec * format.wChannels * seconds; //Get total # of samples based on how many seconds our file is.

                    // Initialize the 16-bit array
                    data.shortArray = new short[numSamples];

                    int amplitude = 32760;  //32760 Max amplitude for 16-bit audio

                    double angle; //angle represents The "angle" used in the function, adjusted for the number of channels and sample rate. This value is like the period of the wave.
                    double freq = 0; //frequency represents the frequency we want to play for that sample
                    double tau = 0; //tau is the difference between whatever tcoalesing and t is for a given t
                    double eye = 0;
                    double tauRatio = 0;
                    int iloopdebug = 0;

                    try
                    {
                        for (uint i = 0; i < numSamples - 1; i++)
                        {

                            iloopdebug = (int)i;
                            if (i == 0) freq = 0; //The below equation can go to infinity if our i is 0. So we ignore computing this case.
                            else freq = (1 / Math.PI) * Math.Pow((5f / 256f) * (1f / (tstop - (i / 44100f))), 0.375f) * Math.Pow(chirpMass, -0.625f);

                            tau = tstop - (i / 44100f); //(i/441000) i is the sample number we're at, i / the number of samples we have in a second will give us what second we're at.

                            //You need a break condition here because there's a chance that the time to reach peak frequency is shorter than 1 second. A .wav file total length has to be at least 1 second. 
                            //So we need to potentially stop generating sound data based on some condition. I chose a tau comparison. You could also compare current frequency to calculated end frequency.

                            //if (tau < tauEND) //If current tau is less than calculated tauEND (tcoalescing - tstop) it means we no longer need to generate anymore samples. 
                            //{
                            //    Console.WriteLine("Current Frequency: " +freq + " should nearly match calculated endFrequency: " + endFrequency);
                            //    break;
                            //}

                            // Fill with a simple sine wave at max amplitude
                            tauRatio = Math.Pow((tauEND / tau), 1 / 4f);
                            eye = -2 * Math.Pow(5 * chirpMass, (-5 / 8f)) * Math.Pow((tau), (5 / 8f));
                            for (int channel = 0; channel < format.wChannels; channel++)
                            {
                                data.shortArray[i + channel] = Convert.ToInt16(amplitude * tauRatio * Math.Cos(eye));
                            }


                        }
                    }
                    catch (Exception e)
                    {

                    }

                    // Calculate data chunk size in bytes
                    data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));

                    break;
            }
        }

        /// <summary>
        /// Saves the current wave data to the specified file.
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(string filePath)
        {
            // Create a file (it always overwrites)
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                Save(fileStream);
            }
        }

        public void Save(Stream stream)
        {
            // Use BinaryWriter to write the bytes to the file
            using (var writer = new BinaryWriter(stream))
            {
                // Write the header
                writer.Write(header.sGroupID.ToCharArray());
                writer.Write(header.dwFileLength);
                writer.Write(header.sRiffType.ToCharArray());

                // Write the format chunk
                writer.Write(format.sChunkID.ToCharArray());
                writer.Write(format.dwChunkSize);
                writer.Write(format.wFormatTag);
                writer.Write(format.wChannels);
                writer.Write(format.dwSamplesPerSec);
                writer.Write(format.dwAvgBytesPerSec);
                writer.Write(format.wBlockAlign);
                writer.Write(format.wBitsPerSample);

                // Write the data chunk
                writer.Write(data.sChunkID.ToCharArray());
                writer.Write(data.dwChunkSize);
                foreach (short dataPoint in data.shortArray)
                {
                    writer.Write(dataPoint);
                }

                writer.Seek(4, SeekOrigin.Begin);
                uint filesize = (uint)writer.BaseStream.Length;
                writer.Write(filesize - 8);

            }
        }

    }
}
