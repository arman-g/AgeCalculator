using AgeCalculator.Extensions;
using System;
using System.Diagnostics;

namespace AgeCalculator
{
    /// <summary>
    /// Represents <see cref="Age"/> object to hold years, months and days information.
    /// </summary>
    [DebuggerDisplay(
        nameof(Years) + " = {" + nameof(Years) + "}, " +
        nameof(Months) + " = {" + nameof(Months) + "}, " +
        nameof(Days) + " = {" + nameof(Days) + "}")]
    public class Age
    {
        /// <summary>
        /// Gets or sets years information.
        /// </summary>
        public int Years { get; set; }
        /// <summary>
        /// Gets or sets months information.
        /// </summary>
        public byte Months { get; set; }
        /// <summary>
        /// Gets or sets days information.
        /// </summary>
        public byte Days { get; set; }

        /// <summary>
        /// Returns the representation of this <see cref="Age"/> object in a format of years, months and days.
        /// </summary>
        /// <returns>Format: {1yr., 11mos., 2d}.</returns>
        public override string ToString()
        {
            return $"{Years}yr., {Months}mos., {Days}d";
        }

        /// <summary>
        /// Calculate the age between two dates.
        /// </summary>
        /// <param name="fromDate">The age's from date.</param>
        /// <param name="toDate">The age's to date.</param>
        /// <returns>An instance of <see cref="Age"/> object containing years, months and days information.</returns>
        public static Age Calculate(DateTime fromDate, DateTime toDate)
        {
            return fromDate.CalculateAge(toDate);
        }
    }
}
