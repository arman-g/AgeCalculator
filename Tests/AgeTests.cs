﻿using AgeCalculator;
using AgeCalculator.Extensions;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class AgeTests(ITestOutputHelper output)
    {
        [Fact]
        public void TotalValues()
        {
            var fromDate = DateTime.Parse("01/01/2023");
            var toDate = DateTime.Parse("01/01/2024");
            var age = fromDate.CalculateAge(toDate);

            Assert.StrictEqual(365, age.TotalDays);
            Assert.StrictEqual(8760, age.TotalHours);
            Assert.StrictEqual(525600, age.TotalMinutes);
            Assert.StrictEqual(31536000, age.TotalSeconds);
            Assert.StrictEqual(31536000000, age.TotalMilliseconds);
            Assert.StrictEqual(315360000000000, age.Ticks);
        }

        [Theory]
        [InlineData("01/05/2021", "01/03/2021")]
        [InlineData("02/05/2021 7:28:12", "02/05/2021 6:30:15")]
        public void InvalidDates(DateTime fromDate, DateTime toDate)
        {
            const string paramName = "fromDate";
            output.WriteLine("Should throw an exception.");

            // Test initialization method
            Assert.Throws<ArgumentOutOfRangeException>(paramName, () => new Age(fromDate, toDate));

            // Test type's static method
            Assert.Throws<ArgumentOutOfRangeException>(paramName, () => Age.Calculate(fromDate, toDate));

            // Test DateTime extension method
            Assert.Throws<ArgumentOutOfRangeException>(paramName, () => fromDate.CalculateAge(toDate));
        }

        [Theory]
        // -- N-N -- y1 = y2, m1 = m2
        [InlineData("02/05/2021", "02/05/2021", 0, 0, 0)]
        [InlineData("02/05/2021", "02/07/2021", 0, 0, 2)]

        // -- N-N -- y1 = y2, m1 < m2
        [InlineData("02/28/2019", "03/28/2019", 0, 1, 0)]
        [InlineData("02/28/2019", "03/01/2019", 0, 0, 1)]
        [InlineData("01/30/2021", "02/28/2021", 0, 0, 29)]
        [InlineData("01/31/2021", "02/28/2021", 0, 0, 28)]
        [InlineData("01/31/2021", "03/31/2021", 0, 2, 0)]
        [InlineData("01/31/2021", "04/30/2021", 0, 2, 30)]
        [InlineData("01/31/2021", "05/01/2021", 0, 3, 1)]
        [InlineData("02/05/2021", "03/03/2021", 0, 0, 26)]
        [InlineData("02/05/2021", "03/05/2021", 0, 1, 0)]
        [InlineData("02/05/2021", "03/07/2021", 0, 1, 2)]
        [InlineData("02/05/2021", "05/03/2021", 0, 2, 26)]
        [InlineData("02/05/2021", "05/05/2021", 0, 3, 0)]
        [InlineData("02/05/2021", "05/07/2021", 0, 3, 2)]

        // -- N-L -- y1 < y2, m1 = m2
        [InlineData("02/28/2017", "02/29/2020", 3, 0, 1)]

        // -- N-L -- y1 < y2, m1 < m2
        [InlineData("02/01/2019", "08/31/2020", 1, 6, 30)]
        [InlineData("02/02/2019", "08/31/2020", 1, 6, 29)]
        [InlineData("02/28/2019", "08/05/2020", 1, 5, 5)]

        // -- L-L -- y1 = y2, m1 = m2
        [InlineData("02/28/2020", "02/28/2020", 0, 0, 0)]
        [InlineData("02/28/2020", "02/29/2020", 0, 0, 1)]
        [InlineData("02/29/2020", "02/29/2020", 0, 0, 0)]

        // -- L-L -- y1 = y2, m1 < m2
        [InlineData("02/28/2020", "03/01/2020", 0, 0, 2)]
        [InlineData("02/29/2020", "03/01/2020", 0, 0, 1)]
        [InlineData("02/29/2020", "03/29/2020", 0, 1, 0)]
        [InlineData("02/29/2020", "03/31/2020", 0, 1, 2)]

        // -- N-N -- y1 < y2, m1 = m2, d1 = d2
        [InlineData("01/02/2017", "01/02/2018", 1, 0, 0)]
        [InlineData("01/02/2017", "01/02/2019", 2, 0, 0)]
        [InlineData("04/02/2017", "04/02/2019", 2, 0, 0)]

        // -- N-N -- y1 < y2, m1 = m2, d1 < d2
        [InlineData("01/02/2017", "01/03/2018", 1, 0, 1)]
        [InlineData("01/02/2017", "01/03/2019", 2, 0, 1)]

        // -- N-N -- y1 < y2, m1 = m2, d1 > d2
        [InlineData("01/02/2017", "01/01/2018", 0, 11, 30)]
        [InlineData("01/02/2017", "01/01/2019", 1, 11, 30)]
        [InlineData("03/10/2017", "03/03/2019", 1, 11, 24)]
        [InlineData("04/04/2017", "04/03/2019", 1, 11, 29)]

        // -- N-N -- y1 < y2, m1 < m2
        [InlineData("05/02/2017", "06/01/2018", 1, 0, 30)]
        [InlineData("05/02/2017", "06/02/2018", 1, 1, 0)]
        [InlineData("05/02/2017", "06/03/2018", 1, 1, 1)]

        // -- N-N -- y1 < y2, m1 > m2
        [InlineData("08/01/2019", "02/02/2021", 1, 6, 1)]
        [InlineData("06/02/2017", "05/01/2018", 0, 10, 29)]
        [InlineData("06/02/2017", "05/02/2018", 0, 11, 0)]
        [InlineData("06/02/2017", "05/03/2018", 0, 11, 1)]
        [InlineData("12/31/2018", "01/31/2019", 0, 1, 0)]

        // Special cases
        // -- L-L -- y1 < y2, m1 = m2
        [InlineData("02/29/1960", "02/28/2020", 59, 11, 28)]
        [InlineData("02/29/1960", "02/29/2020", 60, 0, 0)]

        // -- L-L -- y1 < y2, m1 < m2
        [InlineData("02/29/1960", "03/01/2020", 60, 0, 1)]
        [InlineData("02/29/1960", "03/29/2020", 60, 1, 0)]
        [InlineData("02/29/1960", "03/31/2020", 60, 1, 2)]

        // -- L-L -- y1 < y2, m1 > m2
        [InlineData("05/02/1960", "02/29/2020", 59, 9, 27)]
        [InlineData("05/27/1960", "02/29/2020", 59, 9, 2)]

        // -- L-N -- y1 < y2, m1 = m2
        [InlineData("02/29/1960", "02/27/2021", 60, 11, 27)]
        [InlineData("02/29/1960", "02/28/2021", 60, 11, 28)]

        //-- L-N -- y1 < y2, m1 < m2
        [InlineData("02/29/2020", "03/01/2021", 1, 0, 1)]
        [InlineData("02/29/2020", "04/01/2021", 1, 1, 1)]
        [InlineData("01/01/2000", "12/31/2001", 1, 11, 30)]
        [InlineData("02/29/1960", "03/01/2021", 61, 0, 1)]

        // Include time component
        [InlineData("02/05/2021 5:28:12", "02/05/2021 6:30:15", 0, 0, 0, 1, 2, 3)]
        [InlineData("02/05/2021 7:28:12", "02/06/2021 6:30:15", 0, 0, 0, 23, 2, 3)]
        [InlineData("01/05/2020 7:28:12", "02/05/2022 6:30:15", 2, 0, 30, 23, 2, 3)]
        [InlineData("02/05/2020 07:28:12", "01/05/2022 06:30:15", 1, 10, 28, 23, 2, 3)]
        [InlineData("02/05/2020 23:59:59", "01/06/2022 00:00:00", 1, 11, 0, 0, 0, 1)]
        [InlineData("02/29/2016", "02/28/2021 00:00:01", 4, 11, 28, 0, 0, 1)]
        [InlineData("02/29/2016", "03/01/2021 00:00:01", 5, 0, 1, 0, 0, 1, true)]
        [InlineData("02/29/2016 00:00:02", "02/28/2021 00:00:01", 4, 11, 27, 23, 59, 59)]
        [InlineData("02/29/2016 00:00:02", "03/01/2021 00:00:01", 5, 0, 0, 23, 59, 59, true)]

        // For leapers
        [InlineData("02/29/2016", "02/28/2021", 5, 0, 0, 0, 0, 0, true)]
        [InlineData("02/29/2016", "02/28/2021 00:00:01", 5, 0, 0, 0, 0, 1, true)]
        [InlineData("02/29/2016 00:00:02", "02/28/2021 00:00:01", 4, 11, 27, 23, 59, 59, true)]
        public void CalculateAge(
            DateTime fromDate,
            DateTime toDate,
            int expectedYears,
            int expectedMonths,
            int expectedDays,
            int? expectedHours = null,
            int? expectedMinutes = null,
            int? expectedSeconds = null,
            bool isFeb28AYearCycleForLeapling = false)
        {
            var age = Age.Calculate(fromDate, toDate, isFeb28AYearCycleForLeapling);
            output.WriteLine($"{fromDate:MM/dd/yyyy HH:mm:ss}:{GetLOrNYear(fromDate)} - {toDate:MM/dd/yyyy HH:mm:ss}:{GetLOrNYear(toDate)}");
            if (DateTime.IsLeapYear(fromDate.Year))
            {
                output.WriteLine($"{nameof(isFeb28AYearCycleForLeapling)}: {isFeb28AYearCycleForLeapling}");
            }
            output.WriteLine($"Age: {age.Years} years, {age.Months} months, {age.Days} days, {age.Time}");
            Assert.StrictEqual(expectedYears, age.Years);
            Assert.StrictEqual(expectedMonths, age.Months);
            Assert.StrictEqual(expectedDays, age.Days);
            Assert.StrictEqual(0, age.Time.Days);
            if (expectedHours.HasValue)
            {
                Assert.StrictEqual(expectedHours, age.Time.Hours);
            }

            if (expectedMinutes.HasValue)
            {
                Assert.StrictEqual(expectedMinutes, age.Time.Minutes);
            }

            if (expectedSeconds.HasValue)
            {
                Assert.StrictEqual(expectedSeconds, age.Time.Seconds);
            }
        }

        /// <summary>
        /// Returns 'L' for a leap year and 'N' for normal year.
        /// </summary>
        private static string GetLOrNYear(DateTime dt)
        {
            return DateTime.IsLeapYear(dt.Year) ? "L" : "N";
        }
    }
}
