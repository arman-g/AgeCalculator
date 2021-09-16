# AgeCalculator
AgeCalculator can be used to calculate the age between two dates in years, months, and days format.

## Dependencies
.NET 5+

## NuGet Package
```
PM> Install-Package AgeCalculator
```

## How To Use (Code)
``` csharp
/* There are two ways to use Age calculator */
using AgeCalculator;
using AgeCalculator.Extensions;

public void PrintMyAge()
{
    // Date of birth or from date object.
    var dob = DateTime.Parse("10/03/2015");
    
    // Using DateTime extension.
    var myAge = dob.CalculateAge(DateTime.Now);
    Console.WriteLine($"Age: {myAge}");
    
    // Using Age type's static function.
    myAge = Age.Calculate(dob, DateTime.Now);
    Console.WriteLine($"Age: {myAge}");
}

// Output:
// Age: 5yr., 11mos., 11d

// Other Outputs:
// 02/28/2020:L - 02/29/2020:L Age: 0yr., 0mos., 1d
// 02/29/2020:L - 02/29/2020:L Age: 0yr., 0mos., 0d
// 02/29/2020:L - 03/01/2020:L Age: 0yr., 0mos., 1d
// 02/29/1960:L - 02/28/2021:N Age: 60yr., 11mos., 28d
// 02/29/1960:L - 03/01/2021:N Age: 61yr., 0mos., 1d
```
