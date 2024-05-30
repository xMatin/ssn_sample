using Personnummer.Exceptions;
using Personnummer.Interfaces;
using System.Globalization;
using System.Text.RegularExpressions;


/// <summary>
/// Modified program from: https://github.com/personnummer/csharp
/// </summary>
namespace Personnummer.Controller;

/// <summary>
/// Class used to verify Swedish social security numbers.
/// </summary>
public class PersonnummerValidator : IPersonnummerValidator
{
    private static readonly Regex regex;
    private static readonly CultureInfo cultureInfo;

    static PersonnummerValidator()
    {
        cultureInfo = new CultureInfo("sv-SE");
        regex       = new Regex(@"^(\d{2}){0,1}(\d{2})(\d{2})(\d{2})([-|+]{0,1})?(\d{3})(\d{0,1})$");
    }


    /// <summary>
    /// Calculates the checksum value of a given digit-sequence as string by using the luhn/mod10 algorithm.
    /// </summary>
    /// <param name="value">Sequense of digits as a string.</param>
    /// <returns>Resulting checksum value.</returns>
    private static int Luhn(string value)
    {
        // Luhm algorithm doubles every other number in the value.
        // To get the correct checksum digit we aught to append a 0 on the sequence.
        // If the result becomes a two digit number, subtract 9 from the value.
        // If the total sum is not a 0, the last checksum value should be subtracted from 10.
        // The resulting value is the check value that we use as control number.
        
        // The value passed is a string, so we aught to get the actual integer value from each char (i.e., subtract '0' which is 48).
        
        int[] t = value.ToCharArray().Select(d => d - 48).ToArray();
        int sum = 0;
        int temp;
        for (int i = t.Length; i-- > 0;)
        {
            temp = t[i];
            sum += (i % 2 == t.Length % 2) 
                ? ((temp * 2) % 10) + temp / 5 
                : temp;
        }

        return sum % 10;
    }


    /// <summary>
    /// Function to make sure that the passed year, month and day is parseable to a date.
    /// </summary>
    /// <param name="year">Years as string.</param>
    /// <param name="month">Month as int.</param>
    /// <param name="day">Day as int.</param>
    /// <returns>Result.</returns>
    private static bool TestDate(string year, int month, int day)
    {
        try
        {
            var dt = new DateTime(cultureInfo.Calendar.ToFourDigitYear(int.Parse(year)), month, day);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
            //throw new FormatException(e.Message);
        }
    }


    /// <summary>
    /// Validate Swedish social security number.
    /// </summary>
    /// <param name="value">Value as string.</param>
    /// <returns>Result.</returns>
    public bool Valid(string value)
    {
        if (value != null)
        {
            if (value.Length == 0)
                throw new ArgumentException(nameof(value));


            var matches = regex.Matches(value);

            if (matches.Count < 1 || matches[0].Groups.Count < 7)
            {
                return false;
            }

            var groups = matches[0].Groups;
            int month, day, check;
            string yStr;
            try
            {
                yStr = (groups[2].Value.Length == 4) ? groups[2].Value.Substring(2) : groups[2].Value;
                month = int.Parse(groups[3].Value);
                day = int.Parse(groups[4].Value);
                check = int.Parse(groups[7].Value);
            }
            catch (Exception e)
            {
                // Could not parse. So invalid.
                Console.WriteLine(e.Message);
                throw new SsnParsingException("Failed to parse personal identity number. Invalid input.");
            }

            bool valid = Luhn($"{yStr}{groups[3].Value}{groups[4].Value}{groups[6].Value}{check}") == 0;
            return valid && (TestDate(yStr, month, day) || TestDate(yStr, month, day - 60));
        }
        else
            throw new ArgumentNullException(nameof(value));
    }


    /// <summary>
    /// Validate Swedish social security number.
    /// </summary>
    /// <param name="value">Value as long.</param>
    /// <returns>Result.</returns>
    public bool Valid(long value)
    {
        return Valid(value.ToString());
    }
}
