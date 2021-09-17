/*
 * C# Age Calculation Library
 * https://github.com/arman-g/AgeCalculator
 *
 * Copyright 2021-2022 Arman Ghazanchyan
 * Licensed under The MIT License
 * http://www.opensource.org/licenses/mit-license
 */

using System;

namespace AgeCalculator.Extensions
{
    /// <summary>
    /// Contains <see cref="DateTime"/> extension methods.
    /// </summary>
    public static class DateTimeExtensions
    {
        private const byte TotalMonths = 12;

        /// <summary>
        /// Calculate the age between two dates.
        /// </summary>
        /// <param name="fromDate">The age's from date.</param>
        /// <param name="toDate">The age's to date.</param>
        /// <remarks>This function supports leaper years.</remarks>
        /// <returns>An instance of <see cref="Age"/> object containing years, months and days information.</returns>
        public static Age CalculateAge(
            this DateTime fromDate,
            DateTime toDate)
        {
            if (fromDate > toDate) throw new ArgumentOutOfRangeException(
                nameof(fromDate),
                "This date instance must be less or equal to 'toDate'.");

            var daysInMonth = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);
            var days = toDate.Day - fromDate.Day;
            var age = new Age
            {
                Days = days < 0 ? (byte)(daysInMonth - fromDate.Day + toDate.Day) : (byte)days
            };

            if (fromDate.Month < toDate.Month)
            {
                age.Months = (byte)(toDate.Month - fromDate.Month - (fromDate.Day > toDate.Day ? 1 : 0));
                age.Years = toDate.Year - fromDate.Year;
            }
            else if (fromDate.Month > toDate.Month)
            {
                age.Months = (byte)((TotalMonths - fromDate.Month + toDate.Month - (fromDate.Day > toDate.Day ? 1 : 0)));
                age.Years = toDate.Year - fromDate.Year - 1;
            }
            else
            {
                age.Months = (byte)(fromDate.Day > toDate.Day ? TotalMonths - 1 : 0);
                age.Years = toDate.Year - fromDate.Year - (fromDate.Day > toDate.Day ? 1 : 0);
            }

            return age;
        }
    }
}
