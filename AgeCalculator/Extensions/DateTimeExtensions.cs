using System;

namespace AgeCalculator.Extensions
{
    public static class DateTimeExtensions
    {
        private const byte Feb28 = 59;
        private const byte Feb29 = 60;
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
            var isFeb29 = fromDate.IsInLeapYear() && fromDate.DayOfYear == Feb29;
            SetYears(fromDate, toDate, age);
            var temp = fromDate.AddYears(age.Years);
            SetMonths(temp, toDate, age, isFeb29 && age.Years > 0);
            SetDays(temp.AddMonths(age.Months), toDate, age, isFeb29 && age.Months > 0);
            return age;
        }

        /// <summary>
        /// Set the years component of the age.
        /// </summary>
        /// <param name="fromDate">The years' from date.</param>
        /// <param name="toDate">The years' to date.</param>
        /// <param name="age">The <see cref="Age"/> object to set the years information.</param>
        private static void SetYears(DateTime fromDate, DateTime toDate, Age age)
        {
            if (fromDate.Year >= toDate.Year)
            {
                age.Years = 0;
                return;
            }

            age.Years = toDate.Year - fromDate.Year;
            if (fromDate.AddYears(age.Years) > toDate)
            {
                age.Years -= 1;
            }
        }

        /// <summary>
        /// Set the months component of the age.
        /// </summary>
        /// <param name="fromDate">The months' from date.</param>
        /// <param name="toDate">The months' to date.</param>
        /// <param name="age">The <see cref="Age"/> object to set the months information.</param>
        /// <param name="addExtraDay">Indicates whether an extra day should be added in the calculations.</param>
        private static void SetMonths(DateTime fromDate, DateTime toDate, Age age, bool addExtraDay)
        {
            if (fromDate.Year == toDate.Year)
            {
                age.Months = (byte)(toDate.Month - fromDate.Month);
            }
            else
            {
                age.Months = (byte)(TotalMonths - fromDate.Month);
                age.Months += (byte)(toDate.Month);
            }

            var temp = fromDate.AddMonths(age.Months);
            if (temp.IsInLeapYear() && temp.DayOfYear == Feb28 && addExtraDay)
            {
                temp = temp.AddDays(1);
            }
            if (temp > toDate)
            {
                age.Months -= 1;
            }
        }

        /// <summary>
        /// Set the days component of the age.
        /// </summary>
        /// <param name="fromDate">The days' from date.</param>
        /// <param name="toDate">The days' to date.</param>
        /// <param name="age">The <see cref="Age"/> object to set the days information.</param>
        /// <param name="addExtraDay">Indicates whether an extra day should be added in the calculations.</param>
        private static void SetDays(DateTime fromDate, DateTime toDate, Age age, bool addExtraDay)
        {
            if (fromDate.Year == toDate.Year)
            {
                var dayOfYear = fromDate.DayOfYear;
                if (toDate.IsInLeapYear() && addExtraDay)
                {
                    dayOfYear += 1;
                }
                age.Days = (byte)(toDate.DayOfYear - dayOfYear);
            }
            else
            {
                age.Days = (byte)(DateTime.DaysInMonth(fromDate.Year, fromDate.Month) - fromDate.Day + toDate.Day);
            }
        }
    }
}
