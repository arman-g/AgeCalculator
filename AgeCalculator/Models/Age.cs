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

namespace AgeCalculator;

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
    private readonly DateTime _fromDateTime;
    private readonly DateTime _toDateTime;
    private TimeSpan? _difference;

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

    /// <summary>
    /// Gets the value of the current <see cref="Age" /> class expressed in whole and fractional days.
    /// </summary>
    /// <returns>The total number of days represented by this instance.</returns>
    public double TotalDays
    {
        get
        {
            _difference ??= _toDateTime.Subtract(_fromDateTime);
            return _difference.Value.TotalDays;
        }
    }

    /// <summary>
    /// Gets the value of the current <see cref="Age" /> class expressed in whole and fractional hours.
    /// </summary>
    /// <returns>The total number of hours represented by this instance.</returns>
    public double TotalHours
    {
        get
        {
            _difference ??= _toDateTime.Subtract(_fromDateTime);
            return _difference.Value.TotalHours;
        }
    }

    /// <summary>
    /// Gets the value of the current <see cref="Age" /> class expressed in whole and fractional minutes.
    /// </summary>
    /// <returns>The total number of minutes represented by this instance.</returns>
    public double TotalMinutes
    {
        get
        {
            _difference ??= _toDateTime.Subtract(_fromDateTime);
            return _difference.Value.TotalMinutes;
        }
    }

    /// <summary>
    /// Gets the value of the current <see cref="Age" /> class expressed in whole and fractional seconds.
    /// </summary>
    /// <returns>The total number of seconds represented by this instance.</returns>
    public double TotalSeconds
    {
        get
        {
            _difference ??= _toDateTime.Subtract(_fromDateTime);
            return _difference.Value.TotalSeconds;
        }
    }

    /// <summary>
    /// Gets the value of the current <see cref="Age" /> class expressed in whole and fractional milliseconds.
    /// </summary>
    /// <returns>The total number of milliseconds represented by this instance.</returns>
    public double TotalMilliseconds
    {
        get
        {
            _difference ??= _toDateTime.Subtract(_fromDateTime);
            return _difference.Value.TotalMilliseconds;
        }
    }

    /// <summary>
    /// Gets the number of ticks that represent the value of the current <see cref="Age" /> class.
    /// </summary>
    /// <returns>The number of ticks contained in this instance.</returns>
    public long Ticks
    {
        get
        {
            _difference ??= _toDateTime.Subtract(_fromDateTime);
            return _difference.Value.Ticks;
        }
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Age"/> class and calculates the age between the specified dates.
    /// </summary>
    /// <param name="fromDate">The age's from date.</param>
    /// <param name="toDate">The age's to date.</param>
    /// <param name="isFeb28AYearCycleForLeapling">A boolean flag indicating whether <b>February 28<sup>th</sup></b> of a non-leap year
    /// is considered the end of 1-year cycle for a leapling. By default, it is false.</param>
    /// <remarks>Supports the <b>Gregorian</b> calendar only.</remarks>
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

        _fromDateTime = fromDate;
        _toDateTime = toDate;

        const byte totalMonths = 12;
        const byte feb28 = 59;
        const byte feb29 = 60;
        var fromDateDay = _fromDateTime.Day;
        var toDateDay = _toDateTime.Day;
        var fromDateMonth = _fromDateTime.Month;
        var toDateMonth = _toDateTime.Month;
        var fromDateYear = _fromDateTime.Year;
        var toDateYear = _toDateTime.Year;
        var fromDateTimeOfDay = _fromDateTime.TimeOfDay;
        var toDateTimeOfDay = _toDateTime.TimeOfDay;

        // Calculate years and months
        var remainderDay = fromDateTimeOfDay > toDateTimeOfDay ? 1 : 0; // One less day
        var remainderMonth = fromDateDay > toDateDay - remainderDay ? 1 : 0; // One less month
        if (fromDateMonth == toDateMonth)
        {
            Years = toDateYear - fromDateYear - remainderMonth;
            Months = (byte)((totalMonths - remainderMonth) * remainderMonth);
        }
        else
        {
            var months = fromDateMonth > toDateMonth ? totalMonths : 0;
            Years = toDateYear - fromDateYear - months / totalMonths;
            Months = (byte)(months + toDateMonth - fromDateMonth - remainderMonth);
        }

        // Calculate days
        var days = (toDateDay - remainderDay - fromDateDay);
        Days = (byte)(days < 0 ? DateTime.DaysInMonth(fromDateYear, fromDateMonth) + days : days);

        // Calculate time
        Time = TimeSpan.FromDays(remainderDay) + toDateTimeOfDay - fromDateTimeOfDay;

        // Adjust years, months and days if Feb 29 of a leap year is considered as Feb 28 of non-leap year.
        if (!isFeb28AYearCycleForLeapling ||
            !DateTime.IsLeapYear(fromDateYear) ||
            DateTime.IsLeapYear(toDateYear) ||
            _fromDateTime.DayOfYear != feb29 ||
            _toDateTime.DayOfYear != feb28 ||
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