using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SoundsOfSpacetime.Mobile.Converters
{
    public class DecimalToPercentageConverter : IValueConverter
    {
        /// <summary>
        /// Takes in a main value as Value (supposed to be a decimal between 0 and 1) and returns that times 100
        /// </summary>
        /// <param name="value">The Main Value</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">The Percentage</param>
        /// <param name="culture"></param>
        /// <returns>Value equal to the percentage of the main value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Get the original size we're going to work with
            double? decimalNumberNullable = value as double?;
            if (decimalNumberNullable == null)
                throw new ArgumentException("Value in DecimalToPercentageConverter must be castable to double");
            double decimalNumber = (double)decimalNumberNullable;

            //Get the percentage we're going to use
            double returnValue = decimalNumber * 100;

            return returnValue;
        }

        /// <summary>
        /// Takes in a main value as Value (supposed to be a percentage between 0 and 100) and returns that divided by 100
        /// </summary>
        /// <param name="value">The Main Value</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">The Percentage</param>
        /// <param name="culture"></param>
        /// <returns>The original main value</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Get the original size we're going to work with
            double percentageNumber = 0;
            bool success = double.TryParse(value.ToString(), out percentageNumber);
            if (!success)
                throw new ArgumentException("Value in DecimalToPercentageConverter must be castable to double");

            //Get the percentage we're going to use
            double returnValue = percentageNumber / 100;

            return returnValue;
        }
    }
}
