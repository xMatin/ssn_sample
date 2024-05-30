using System.Text;
using Personnummer.Interfaces;

namespace Personnummer.Controller;

public class Info : IInfo
{
    /// <summary>
    /// Shows the first entry visit text 
    /// </summary>
    /// <returns></returns>
    public string Show()
    {
        var mainString = new StringBuilder();

        mainString.Append("Welcome to SSN Check®");
        mainString.Append("\n" + "Used to check if a Social Security Number is correct:");

        mainString.Append("\n" + "Please type in your command (Check, ..., Quit etc.)");
        mainString.Append("\n" + "and SSN (case insensitive) as follows:" + "\n");
        mainString.Append("Example: First 'C' and enter, then '19801224-1234' and enter" + "\n");

        return mainString.ToString();
    }
}
