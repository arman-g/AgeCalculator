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
        private const byte Feb28 = 59;
        private const byte Feb29 = 60;

        #region ' Properties '

        /// <summary>
        /// Gets the years component of the <see cref="Age"/> class.
        /// </summary>
        public int Years { get; init; }

        /// <summary>
        /// Gets the months component of the <see cref="Age"/> class.
        /// </summary>
        public byte Months { get; init; }

        /// <summary>
        /// Gets the days component of the <see cref="Age"/> class.
        /// </summary>
        public byte Days { get; init; }

        /// <summary>
        /// Gets the time component of the <see cref="Age"/> class.
        /// </summary>
        public TimeSpan Time { get; init; }

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
        /// <param name="isFeb29AsFeb28ForLeaper">A boolean flag indicating whether Feb 29 of a leap year
        /// is considered as Feb 28 of a non leap year. By default it is false.</param>
        /// <returns>An instance of <see cref="Age"/> class containing years, months, days and time information.</returns>
        public Age(
            DateTime fromDate,
            DateTime toDate,
            bool isFeb29AsFeb28ForLeaper = false)
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

            // Re-Calculate if Feb 29 of a leap year is considered as Feb 28 of non leap year.
            if (!isFeb29AsFeb28ForLeaper ||
                !DateTime.IsLeapYear(fromDate.Year) ||
                DateTime.IsLeapYear(toDate.Year) ||
                fromDate.DayOfYear != Feb29 ||
                toDate.DayOfYear != Feb28 ||
                Days != 28) return;
            ++Years;
            Months = 0;
            Days = 0;
        }

        /// <summary>
        /// Calculates the age between two dates.
        /// </summary>
        /// <inheritdoc cref="Age(DateTime,DateTime,bool)"/>
        public static Age Calculate(
            DateTime fromDate,
            DateTime toDate,
            bool isFeb29AsFeb28ForLeaper = false)
        {
            return new Age(fromDate, toDate, isFeb29AsFeb28ForLeaper);
        }
    }
}
