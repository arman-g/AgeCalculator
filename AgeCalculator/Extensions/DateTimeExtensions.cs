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
        /// Indicates whether this date is in a leap year.
        /// </summary>
        /// <param name="value">This <see cref="DateTime"/> instance.</param>
        /// <returns>A boolean value indicating whether this date is in a leap your.</returns>
        public static bool IsInLeapYear(this DateTime value)
        {
            return DateTime.IsLeapYear(value.Year);
        }

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

            var age = new Age();
            // Same year
            if (fromDate.Year == toDate.Year)
            {
                if (fromDate.Month == toDate.Month)
                {
                    age.Days = (byte)Math.Abs(toDate.Day - fromDate.Day);
                }
                else
                {
                    age.Months = (byte)Math.Abs(toDate.Month - fromDate.Month);
                    if (fromDate.Day > toDate.Day)
                    {
                        --age.Months;
                        age.Days = (byte)(fromDate.GetRemainingDaysOfMonth() + toDate.Day);
                    }
                    else
                    {
                        age.Days = (byte)Math.Abs(toDate.Day - fromDate.Day);
                    }
                }
            }
            else
            {
                // Different years
                age.Years = toDate.Year - fromDate.Year;
                if (fromDate.Month == toDate.Month)
                {
                    var days = toDate.Day - fromDate.Day;
                    if (days < 0)
                    {
                        --age.Years;
                        age.Months = (byte)(TotalMonths - fromDate.Month + toDate.Month - 1);
                        age.Days = (byte)(fromDate.GetRemainingDaysOfMonth() + toDate.Day);
                    }
                    else
                    {
                        age.Days = (byte)days;
                    }
                }
                else
                {
                    if (fromDate.Month > toDate.Month)
                    {
                        --age.Years;
                        age.Months = (byte)(TotalMonths - fromDate.Month + toDate.Month);
                    }
                    else
                    {
                        age.Months = (byte)(toDate.Month - fromDate.Month);
                    }

                    var days = toDate.Day - fromDate.Day;
                    if (days < 0)
                    {
                        --age.Months;
                        age.Days = (byte)(fromDate.GetRemainingDaysOfMonth() + toDate.Day);
                    }
                    else
                    {
                        age.Days = (byte)days;
                    }
                }
            }

            return age;
        }

        /// <summary>
        /// Gets the number of remaining days of this <see cref="DateTime"/> month.
        /// </summary>
        /// <param name="dt">This <see cref="DateTime"/> object.</param>
        /// <returns>The number of remaining days left to the end of the month.</returns>
        private static int GetRemainingDaysOfMonth(this DateTime dt)
        {
            return DateTime.DaysInMonth(dt.Year, dt.Month) - dt.Day;
        }
    }
}
