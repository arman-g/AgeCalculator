using AgeCalculator;
using AgeCalculator.Extensions;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Extensions
{
    public class DateTimeExtensionsTests
    {
        private readonly ITestOutputHelper _output;

        public DateTimeExtensionsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("01/05/2021", "01/03/2021", 0, 0, 0, true)]

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

        // -- L-L -- y1 = y2, m1 = m2
        [InlineData("02/28/2020", "02/28/2020", 0, 0, 0)]
        [InlineData("02/28/2020", "02/29/2020", 0, 0, 1)]
        [InlineData("02/29/2020", "02/29/2020", 0, 0, 0)]

        // -- L-L -- y1 = y2, m1 < m2
        [InlineData("02/28/2020", "03/01/2020", 0, 0, 2)]
        [InlineData("02/29/2020", "03/01/2020", 0, 0, 1)]
        [InlineData("02/29/2020", "03/29/2020", 0, 1, 0)]
        [InlineData("02/29/2020", "03/31/2020", 0, 1, 2)]

        // -- N-N -- y1 < y2, m1 = m2
        [InlineData("01/02/2017", "01/01/2018", 0, 11, 30)]
        [InlineData("01/02/2017", "01/02/2018", 1, 0, 0)]
        [InlineData("01/02/2017", "01/03/2018", 1, 0, 1)]
        [InlineData("01/02/2017", "01/01/2019", 1, 11, 30)]
        [InlineData("01/02/2017", "01/02/2019", 2, 0, 0)]
        [InlineData("01/02/2017", "01/03/2019", 2, 0, 1)]

        // -- N-N -- y1 < y2, m1 = m2, d1 > d2
        [InlineData("03/10/2017", "03/03/2019", 1, 11, 24)]

        // -- N-N -- y1 < y2, m1 < m2
        [InlineData("05/02/2017", "06/01/2018", 1, 0, 30)]
        [InlineData("05/02/2017", "06/02/2018", 1, 1, 0)]
        [InlineData("05/02/2017", "06/03/2018", 1, 1, 1)]
        [InlineData("08/01/2019", "02/02/2021", 1, 6, 1)]

        // -- N-N -- y1 < y2, m1 > m2
        [InlineData("06/02/2017", "05/01/2018", 0, 10, 29)]
        [InlineData("06/02/2017", "05/02/2018", 0, 11, 0)]
        [InlineData("06/02/2017", "05/03/2018", 0, 11, 1)]

        // Special cases
        // -- L-L -- y1 < y2, m1 = m2
        [InlineData("02/29/1960", "02/28/2020", 59, 11, 28)]
        [InlineData("02/29/1960", "02/29/2020", 60, 0, 0)]

        // -- L-L -- y1 < y2, m1 < m2
        [InlineData("02/29/1960", "03/01/2020", 60, 0, 1)]
        [InlineData("02/29/1960", "03/29/2020", 60, 1, 0)]
        [InlineData("02/29/1960", "03/31/2020", 60, 1, 2)]

        // -- L-L -- y1 < y2, m1 > m2
        [InlineData("05/2/1960", "02/29/2020", 59, 9, 27)]
        [InlineData("05/27/1960", "02/29/2020", 59, 9, 2)]

        // -- L-N -- y1 < y2, m1 = m2
        [InlineData("02/29/1960", "02/27/2021", 60, 11, 27)]
        [InlineData("02/29/1960", "02/28/2021", 60, 11, 28)]

        // -- L-N -- y1 < y2, m1 < m2
        [InlineData("01/01/2000", "12/31/2001", 1, 11, 30)]
        [InlineData("02/29/1960", "03/01/2021", 61, 0, 1)]

        public void Calculate_age(string fromDate, string toDate, int expectedYears, byte expectedMoths, byte expectedDays, bool? exception = false)
        {
            var dob = DateTime.Parse(fromDate);
            var endDate = DateTime.Parse(toDate);
            if (exception == true)
            {
                _output.WriteLine("Should throw an exception.");
                Assert.Throws<ArgumentOutOfRangeException>(() => Age.Calculate(dob, endDate));
            }
            else
            {
                var age = Age.Calculate(dob, endDate);
                _output.WriteLine($"{dob:MM/dd/yyyy}:{(dob.IsInLeapYear() ? 'L' : 'N')} - {endDate:MM/dd/yyyy}:{(endDate.IsInLeapYear() ? 'L' : 'N')} Age: {age}");
                Assert.Equal(expectedYears, age.Years);
                Assert.Equal(expectedMoths, age.Months);
                Assert.Equal(expectedDays, age.Days);
            }
        }
    }
}
