using System;

namespace AgeCalculator.Extensions
{
    /// <summary>
    /// Contains <see cref="AgeCalculator"/>'s <see cref="DateTime"/> extension methods.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Calculates the age between this and the specified <see cref="DateTime"/> instance.
        /// </summary>
        /// <inheritdoc cref="Age(DateTime,DateTime)"/>
        public static Age CalculateAge(
            this DateTime fromDate,
            DateTime toDate)
        {
            if (fromDate > toDate) throw new ArgumentOutOfRangeException(
                nameof(fromDate),
                $"This {nameof(DateTime)} must be less or equal to '{nameof(toDate)}'.");

            return new Age(fromDate, toDate);
        }
    }
}
