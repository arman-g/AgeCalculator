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

        [Fact]
        public void Calculate_age()
        {
            _output.WriteLine($"\r\n-------- General Cases ---------");
            Test("01/02/2019", "01/01/2019", 0, 0, 0, true);
            _output.WriteLine("-- (N-N) --");
            Test("01/01/2019", "01/01/2019", 0, 0, 0);
            Test("01/01/2019", "01/02/2019", 0, 0, 1);
            Test("02/28/2019", "03/01/2019", 0, 0, 1);
            Test("01/01/2019", "02/01/2019", 0, 1, 0);
            Test("01/01/2019", "12/31/2019", 0, 11, 30);

            Test("08/01/2019", "01/02/2021", 1, 5, 1);
            Test("08/01/2019", "02/02/2021", 1, 6, 1);
            Test("08/01/2019", "03/02/2021", 1, 7, 1);
            Test("08/01/2019", "07/02/2021", 1, 11, 1);

            Test("08/01/2019", "08/01/2021", 2, 0, 0);
            Test("08/01/2019", "09/02/2021", 2, 1, 1);

            Test("04/22/2001", "10/27/2019", 18, 6, 5);
            _output.WriteLine("-- (L-L) --");
            Test("01/01/2000", "01/01/2000", 0, 0, 0);
            Test("01/01/2000", "01/02/2000", 0, 0, 1);
            Test("02/29/2000", "03/01/2000", 0, 0, 1);
            Test("01/01/2000", "02/01/2000", 0, 1, 0);
            Test("01/01/2000", "12/31/2000", 0, 11, 30);

            Test("08/01/2016", "01/02/2020", 3, 5, 1);
            Test("08/01/2016", "02/02/2020", 3, 6, 1);
            Test("08/01/2016", "03/02/2020", 3, 7, 1);
            Test("08/01/2016", "07/02/2020", 3, 11, 1);

            Test("08/01/2016", "08/01/2020", 4, 0, 0);
            Test("08/01/2016", "09/02/2020", 4, 1, 1);

            Test("04/22/2016", "10/27/2020", 4, 6, 5);

            _output.WriteLine("-- (L-N) --");
            Test("01/01/2000", "01/01/2001", 1, 0, 0);
            Test("01/01/2000", "01/02/2001", 1, 0, 1);
            Test("02/29/2000", "03/01/2001", 1, 0, 1);
            Test("01/01/2000", "02/01/2001", 1, 1, 0);
            Test("01/01/2000", "12/31/2001", 1, 11, 30);

            Test("08/01/2016", "01/02/2021", 4, 5, 1);
            Test("08/01/2016", "02/02/2021", 4, 6, 1);
            Test("08/01/2016", "03/02/2021", 4, 7, 1);
            Test("08/01/2016", "07/02/2021", 4, 11, 1);

            Test("08/01/2016", "08/01/2021", 5, 0, 0);
            Test("08/01/2016", "09/02/2021", 5, 1, 1);

            Test("04/22/2016", "10/27/2021", 5, 6, 5);

            _output.WriteLine("-- (N-L) --");
            Test("04/22/2001", "01/01/2020", 18, 8, 10);
            Test("04/22/2001", "02/17/2020", 18, 9, 26);
            Test("04/22/2001", "04/22/2020", 19, 0, 0);
            Test("04/22/2001", "07/07/2020", 19, 2, 15);

            _output.WriteLine("\r\n-------- Special  Cases (Feb 29) ---------");
            Test("02/29/1960", "02/28/2020", 59, 11, 30);
            Test("02/29/1960", "02/29/2020", 60, 0, 0);
            Test("02/29/1960", "03/01/2020", 60, 0, 1);
            Test("02/29/1960", "02/27/2021", 60, 11, 29);
            Test("02/29/1960", "02/28/2021", 61, 0, 0);
            Test("02/29/1960", "03/01/2021", 61, 0, 1);
        }

        private void Test(string fromDate, string toDate, ushort expectedYears, byte expectedMoths, byte expectedDays, bool? exception = false)
        {
            var dob = DateTime.Parse(fromDate);
            var endDate = DateTime.Parse(toDate);
            if (exception == true)
            {
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
