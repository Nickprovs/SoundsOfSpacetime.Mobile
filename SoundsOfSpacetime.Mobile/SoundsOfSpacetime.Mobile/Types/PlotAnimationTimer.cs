using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace SoundsOfSpacetime.Mobile.Types
{
    public class PlotAnimationTimer : Timer
    {
        #region Properties

        /// <summary>
        /// The plot animation cache
        /// </summary>
        public PlotAnimationCache Cache { get; set; }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a PlotAnimationTimer
        /// </summary>
        public PlotAnimationTimer()
        {
        }

        #endregion
    }
}
