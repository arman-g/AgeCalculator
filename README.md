# AgeCalculator
AgeCalculator can be used to calculate the age (in a format of years, months, and days) between two dates. This calculator will include the leap years into its calculations and reflect the outcome accordingly. In the calculations, Feb 29th in a leap year will be treated as Feb 28th in a non-leap year.

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
```
