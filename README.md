# AgeCalculator
AgeCalculator is an age calculation library that can be used to calculate the age between two dates and output years, months, days and time components.

## Dependencies
.NET 5+

## NuGet Package
```
PM> Install-Package AgeCalculator
```

## How To Use (Code)
``` csharp
/* There are three ways to calculate the age between two dates */
using AgeCalculator;
using AgeCalculator.Extensions;

public void PrintMyAge()
{
    // Date of birth or from date.
    var dob = DateTime.Parse("10/03/2015");
    
    // #1. Using the Age class constructor.
    var age = new Age(dob, DateTime.Today); // as of 09/19/2021
    Console.WriteLine($"Age: {age.Years} years, {age.Months} months, {age.Days} days, {age.Time}");
    
    // #2. Using DateTime extension.
    age = dob.CalculateAge(DateTime.Today); // as of 09/19/2021
    Console.WriteLine($"Age: {age.Years} years, {age.Months} months, {age.Days} days, {age.Time}");
    
    // #3. Using the Age type's static function.
    age = Age.Calculate(dob, DateTime.Today); // as of 09/19/2021
    Console.WriteLine($"Age: {age.Years} years, {age.Months} months, {age.Days} days, {age.Time}");
}

// Output:
// Age: 5 years, 11 months, 16 days, 00:00:00

// Other Outputs:
// 02/28/2020:L - 02/29/2020:L    Age: 0 years, 0 months, 1 days, 00:00:00
// 02/29/2020:L - 02/29/2020:L    Age: 0 years, 0 months, 0 days, 00:00:00
// 02/29/2020:L - 03/01/2020:L    Age: 0 years, 0 months, 1 days, 00:00:00
// 02/29/1960:L - 02/28/2021:N    Age: 60 years, 11 months, 28 days, 00:00:00
// 02/29/1960:L - 03/01/2021:N    Age: 61 years, 0 months, 1 days, 00:00:00

// 01/05/2020 07:28:12:L - 02/05/2022 06:30:15:N    Age: 2 years, 0 months, 30 days, 23:02:03
// 02/05/2020 07:28:12:L - 01/05/2022 06:30:15:N    Age: 1 years, 10 months, 28 days, 23:02:03
// 02/05/2020 23:59:59:L - 01/06/2022 00:00:00:N    Age: 1 years, 11 months, 0 days, 00:00:01
// 02/05/2021 05:28:12:N - 02/05/2021 06:30:15:N    Age: 0 years, 0 months, 0 days, 01:02:03
// 02/05/2021 07:28:12:N - 02/06/2021 06:30:15:N    Age: 0 years, 0 months, 0 days, 23:02:03
// 02/29/2016 00:00:00:L - 02/28/2021 00:00:01:N    Age: 4 years, 11 months, 28 days, 00:00:01
// 02/29/2016 00:00:02:L - 02/28/2021 00:00:01:N    Age: 4 years, 11 months, 27 days, 23:59:59
// 02/29/2016 00:00:00:L - 03/01/2021 00:00:01:N    Age: 5 years, 0 months, 1 days, 00:00:01
// 02/29/2016 00:00:02:L - 03/01/2021 00:00:01:N    Age: 5 years, 0 months, 0 days, 23:59:59
```
