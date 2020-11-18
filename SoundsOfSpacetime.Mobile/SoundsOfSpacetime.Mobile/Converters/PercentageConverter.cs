using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SoundsOfSpacetime.Mobile.Converters
{
    /// <summary>
    /// The Percentage Converter. 
    /// Used to get a percentage of another value.
    /// </summary>
    public class PercentageConverter : IValueConverter
    {
        /// <summary>
        /// Takes in a main value as Value and the Percentage as the parameter and returns the percentage value.
        /// </summary>
        /// <param name="value">The Main Value</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">The Percentage</param>
        /// <param name="culture"></param>
        /// <returns>Value equal to the percentage of the main value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Get the original size we're going to work with
            double? originalSizeNullable = value as double?;
            if (originalSizeNullable == null)
                throw new ArgumentException("Original Size must be convertable to double in SizeToSizePercentageConverter");
            double originalSize = (double) originalSizeNullable;

            //Get the percentage we're going to use
            double percentage = 0;
            bool success= double.TryParse(parameter.ToString(), out percentage);
            if (!success)
                throw new ArgumentException("Parameter as string must be convertable to double in SizeToSizePercentageConverter");

            //Calculate the new value and return
            var returnValue = originalSize * percentage;
            if (returnValue < 0)
            {
                Debug.WriteLine("Got a value less than 0 in PercentageConverter");
                return 0;
            }
            return returnValue;
        }

        /// <summary>
        /// Takes in a main value as Value and the Percentage as the parameter and returns the percentage value.
        /// </summary>
        /// <param name="value">The Main Value</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">The Percentage</param>
        /// <param name="culture"></param>
        /// <returns>The original main value</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Get the original size we're going to work with
            double? originalSizeNullable = value as double?;
            if (originalSizeNullable == null)
                throw new ArgumentException("Original Size must be convertable to double in SizeToSizePercentageConverter");
            double originalSize = (double)originalSizeNullable;

            //Get the percentage we're going to use
            double percentage = 0;
            bool success = double.TryParse(parameter.ToString(), out percentage);
            if (!success)
                throw new ArgumentException("Parameter as string must be convertable to double in SizeToSizePercentageConverter");

            //Calculate the new value and return
            //Note: Convert back performs a division instead of multiplication
            //Calculate the new value and return
            var returnValue = originalSize / percentage;
            if (returnValue < 0)
            {
                Debug.WriteLine("Got a value less than 0 in PercentageConverter");
                return 0;
            }
            return returnValue;
        }
    }
}
