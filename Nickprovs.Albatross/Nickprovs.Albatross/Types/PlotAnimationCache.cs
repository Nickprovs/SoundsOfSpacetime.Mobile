using Nickprovs.Albatross.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nickprovs.Albatross.Types
{
    public class PlotAnimationCache
    {
        #region Properties

        /// <summary>
        /// The Data Series in Animation
        /// </summary>
        public List<IPoint> DataSeries { get; set; }

        /// <summary>
        /// The Batch Size (How many points we should plot at a time)
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// The Offset (or current point in the animation)
        /// </summary>
        public int Offset { get; set; }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a plot animation cache.
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <param name="batchSize"></param>
        public PlotAnimationCache(List<IPoint> dataSeries, int batchSize)
        {
            this.DataSeries = dataSeries;
            this.BatchSize = batchSize;
        }

        #endregion
    }
}
