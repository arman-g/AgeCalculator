/*
 * C# Age Calculation Library
 * https://github.com/arman-g/AgeCalculator
 *
 * Copyright 2021-2022 Arman Ghazanchyan
 * Licensed under The MIT License
 * http://www.opensource.org/licenses/mit-license
 */

using System;
using System.Diagnostics;

namespace AgeCalculator
{
    /// <summary>
    /// Represents <see cref="Age"/> class to hold years, months, days and time information.
    /// </summary>
    [DebuggerDisplay(
        nameof(Years) + " = {" + nameof(Years) + "}, " +
        nameof(Months) + " = {" + nameof(Months) + "}, " +
        nameof(Days) + " = {" + nameof(Days) + "}, " +
        nameof(Time) + " = {" + nameof(Time) + "}")]
    public class Age
    {
        private const byte TotalMonths = 12;

        #region ' Properties '

        /// <summary>
        /// Gets the years component of the <see cref="Age"/> class.
        /// </summary>
        public int Years { get; }

        /// <summary>
        /// Gets the months component of the <see cref="Age"/> class.
        /// </summary>
        public byte Months { get; }

        /// <summary>
        /// Gets the days component of the <see cref="Age"/> class.
        /// </summary>
        public byte Days { get; }

        /// <summary>
        /// Gets the time component of the <see cref="Age"/> class.
        /// </summary>
        public TimeSpan Time { get; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Age"/> class.
        /// </summary>
        public Age() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Age"/> class and calculates the age between the specified dates.
        /// </summary>
        /// <param name="fromDate">The age's from date.</param>
        /// <param name="toDate">The age's to date.</param>
        /// <returns>An instance of <see cref="Age"/> class containing years, months, days and time information.</returns>
        public Age(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate) throw new ArgumentOutOfRangeException(
                nameof(fromDate),
                $"The '{nameof(fromDate)}' must be less or equal to '{nameof(toDate)}'.");

            var remainderDay = 0;
            if (fromDate.TimeOfDay > toDate.TimeOfDay)
            {
                --remainderDay;
            }

            // Calculate years and months components.
            var isOneMonthLess = fromDate.Day > (toDate.Day + remainderDay);
            if (fromDate.Month < toDate.Month)
            {
                Years = toDate.Year - fromDate.Year;
                Months = (byte)(toDate.Month - fromDate.Month - (isOneMonthLess ? 1 : 0));
            }
            else if (fromDate.Month > toDate.Month)
            {
                Years = toDate.Year - fromDate.Year - 1;
                Months = (byte)((TotalMonths - fromDate.Month + toDate.Month - (isOneMonthLess ? 1 : 0)));
            }
            else
            {
                Years = toDate.Year - fromDate.Year - (isOneMonthLess ? 1 : 0);
                Months = (byte)(isOneMonthLess ? TotalMonths - 1 : 0);
            }

            // Calculate days component
            var daysInMonth = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);
            var days = toDate.Day + remainderDay - fromDate.Day;
            Days = days < 0 ? (byte)(daysInMonth - fromDate.Day + toDate.Day + remainderDay) : (byte)days;

            // Calculate time component
            if (fromDate.TimeOfDay < toDate.TimeOfDay)
            {
                Time = toDate.TimeOfDay - fromDate.TimeOfDay;
            }
            else if (fromDate.TimeOfDay > toDate.TimeOfDay)
            {
                Time = new TimeSpan(24, 0, 0) - fromDate.TimeOfDay + toDate.TimeOfDay;
            }
        }

        /// <summary>
        /// Calculates the age between two dates.
        /// </summary>
        /// <inheritdoc cref="Age(DateTime,DateTime)"/>
        public static Age Calculate(DateTime fromDate, DateTime toDate)
        {
            return new Age(fromDate, toDate);
        }
    }
}
