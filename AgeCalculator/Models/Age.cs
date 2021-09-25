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
        "{" + nameof(Years) + "} " + nameof(Years) + ", " +
        "{" + nameof(Months) + "} " + nameof(Months) + ", " +
        "{" + nameof(Days) + "} " + nameof(Days) + ", " +
        "{" + nameof(Time) + "}")]
    public class Age
    {
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
        /// <param name="isFeb28AYearCycleForLeapling">A boolean flag indicating whether <b>February 28<sup>th</sup></b> of a non-leap year
        /// is considered the end of 1-year cycle for a leapling. By default it is false.</param>
        /// <remarks>Supports <b>Gregorian</b> calendar only.</remarks>
        /// <returns>An instance of <see cref="Age"/> class containing years, months, days and time information.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="fromDate"/> is considered greater than <paramref name="toDate"/>.</exception>
        public Age(
            DateTime fromDate,
            DateTime toDate,
            bool isFeb28AYearCycleForLeapling = false)
        {
            if (fromDate > toDate) throw new ArgumentOutOfRangeException(
                nameof(fromDate),
                $"The '{nameof(fromDate)}' must be less or equal to '{nameof(toDate)}'.");

            const byte totalMonths = 12;
            const byte feb28 = 59;
            const byte feb29 = 60;

            // Calculate years and months
            var remainderDay = fromDate.TimeOfDay > toDate.TimeOfDay ? 1 : 0; // One less day
            var remainderMonth = (byte)(fromDate.Day > toDate.Day - remainderDay ? 1 : 0); // One less month
            if (fromDate.Month == toDate.Month)
            {
                Years = toDate.Year - fromDate.Year - remainderMonth;
                Months = (byte)((totalMonths - remainderMonth) * remainderMonth);
            }
            else
            {
                var months = fromDate.Month > toDate.Month ? totalMonths : 0;
                Years = toDate.Year - fromDate.Year - months / totalMonths;
                Months = (byte)(months + toDate.Month - fromDate.Month - remainderMonth);
            }

            // Calculate days
            var days = (toDate.Day - remainderDay - fromDate.Day);
            Days = (byte)(days < 0 ? DateTime.DaysInMonth(fromDate.Year, fromDate.Month) + days : days);

            // Calculate time
            Time = TimeSpan.FromDays(remainderDay) + toDate.TimeOfDay - fromDate.TimeOfDay;

            // Adjust years, months and days if Feb 29 of a leap year is considered as Feb 28 of non-leap year.
            if (!isFeb28AYearCycleForLeapling ||
                !DateTime.IsLeapYear(fromDate.Year) ||
                DateTime.IsLeapYear(toDate.Year) ||
                fromDate.DayOfYear != feb29 ||
                toDate.DayOfYear != feb28 ||
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
            bool isFeb28AYearCycleForLeapling = false)
        {
            return new Age(fromDate, toDate, isFeb28AYearCycleForLeapling);
        }
    }
}
