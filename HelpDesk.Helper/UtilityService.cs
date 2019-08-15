using HelpDesk.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Helper
{
    /// <summary>
    /// The Utility Service Class.
    /// </summary>
    public static class UtilityService
    {

        /// <summary>
        /// Initial Error Result and Message to return.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static ResultViewModel InitialResultError(string message, int statusCode = 500)
        {
            return new ResultViewModel
            {
                IsError = true,
                StatusCode = statusCode,
                Message = message
            };
        }

        /// <summary>
        /// Convert string to date time using parameter format.
        /// </summary>
        /// <param name="value">The string datetime.</param>
        /// <param name="format">The datetime format.</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string value, string format)
        {
            return DateTime.TryParseExact(value, format,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None, out DateTime temp) ? temp : throw new ArgumentException($"DateTime incorrect format : {value}");
        }

    }
}
