# Age Calculator

[![Nuget](https://img.shields.io/nuget/v/AgeCalculator)](https://www.nuget.org/packages/AgeCalculator/)
[![GitHub](https://img.shields.io/github/license/arman-g/AgeCalculator)](https://github.com/arman-g/AgeCalculator/blob/main/LICENSE)
[![GitHub Workflow Status (branch)](https://img.shields.io/github/workflow/status/arman-g/AgeCalculator/.NET/main)](https://github.com/arman-g/AgeCalculator/actions)
[![GitHub issues](https://img.shields.io/github/issues/arman-g/AgeCalculator)](https://github.com/arman-g/AgeCalculator/issues)

AgeCalculator is an age calculation library that can be used to calculate the age between two dates and output years, months, days and time components.

## Dependencies
.NET 5+

## How To Use (C# Code)
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
```


``` csharp
/* In this example, Feb 29 of a leap year is considered as Feb 28 of a non leap year. */
using AgeCalculator;

public void PrintAge()
{
    // Example 1
    var dob = DateTime.Parse("02/29/2016");
    var toDay = DateTime.Parse("02/28/2021");
    var age = new Age(dob, toDay, true); // <- set 'isFeb29AsFeb28ForLeaper' flag to True. Default is False.
    Console.WriteLine($"Example 1 - Age: {age.Years} years, {age.Months} months, {age.Days} days, {age.Time}");
    
    // Example 2
    dob = DateTime.Parse("02/29/2016");
    toDay = DateTime.Parse("02/28/2021 00:00:01");
    age = new Age(dob, toDay, true); // <- set 'isFeb29AsFeb28ForLeaper' flag to True. Default is False.
    Console.WriteLine($"Example 2 - Age: {age.Years} years, {age.Months} months, {age.Days} days, {age.Time}");
    
    // Example 3
    dob = DateTime.Parse("02/29/2016 00:00:02");
    toDay = DateTime.Parse("02/28/2021 00:00:01");
    age = new Age(dob, toDay, true); // <- set 'isFeb29AsFeb28ForLeaper' flag to True. Default is False.
    Console.WriteLine($"Example 3 - Age: {age.Years} years, {age.Months} months, {age.Days} days, {age.Time}");
}

// Output:
// Example 1 - Age: 5 years, 0 months, 0 days, 00:00:00
// Example 2 - Age: 5 years, 0 months, 0 days, 00:00:01
// Example 3 - Age: 4 years, 11 months, 27 days, 23:59:59
```

## Default Outputs
```
// 02/28/2020:L - 02/29/2020:L    Age: 0 years, 0 months, 1 days, 00:00:00
// 02/29/2020:L - 02/29/2020:L    Age: 0 years, 0 months, 0 days, 00:00:00
// 02/29/2020:L - 03/01/2020:L    Age: 0 years, 0 months, 1 days, 00:00:00
// 02/29/1960:L - 02/28/2021:N    Age: 60 years, 11 months, 28 days, 00:00:00
// 02/29/1960:L - 03/01/2021:N    Age: 61 years, 0 months, 1 days, 00:00:00

// With Time Component:
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
