using System.IO;

namespace SoundsOfSpacetime.Mobile.Types.Audio.Wav
{
    public static class WavExtensions
    {
        /// <summary>
        /// Saves the current wav data to the specified file.
        /// </summary>
        /// <param name="filePath"></param>
        public static void SaveToFile(this Wav wav, string filePath)
        {
            // Create a file (it always overwrites)
            // Just creates a file stream and leverages the public SaveToStream function
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                wav.SaveToStream(fileStream);
            }
        }

        /// <summary>
        /// Saves the current wav data to a specified stream
        /// </summary>
        /// <param name="wav"></param>
        /// <param name="stream"></param>
        public static void SaveToStream(this Wav wav,Stream stream)
        {
            // Use BinaryWriter to write the bytes to the file
            using (var writer = new BinaryWriter(stream))
            {
                // Write the header
                writer.Write(wav.Header.sGroupID.ToCharArray());
                writer.Write(wav.Header.dwFileLength);
                writer.Write(wav.Header.sRiffType.ToCharArray());

                // Write the format chunk
                writer.Write(wav.Format.sChunkID.ToCharArray());
                writer.Write(wav.Format.dwChunkSize);
                writer.Write(wav.Format.wFormatTag);
                writer.Write(wav.Format.wChannels);
                writer.Write(wav.Format.dwSamplesPerSec);
                writer.Write(wav.Format.dwAvgBytesPerSec);
                writer.Write(wav.Format.wBlockAlign);
                writer.Write(wav.Format.wBitsPerSample);

                // Write the data chunk
                writer.Write(wav.Data.sChunkID.ToCharArray());
                writer.Write(wav.Data.dwChunkSize);
                foreach (short dataPoint in wav.Data.shortArray)
                    writer.Write(dataPoint);
                
                //Magic?
                writer.Seek(4, SeekOrigin.Begin);
                uint filesize = (uint)writer.BaseStream.Length;
                writer.Write(filesize - 8);
            }
        }
    }
}
