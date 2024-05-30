
namespace Personnummer.Exceptions;

/// <summary>
/// https://docs.microsoft.com/en-us/dotnet/standard/exceptions/how-to-create-localized-exception-messages
/// </summary>
[Serializable]
public class SsnParsingException : Exception
{
    public string? Ssn { get; }

    public SsnParsingException()
    { }

    public SsnParsingException(string message) 
        : base(message) 
    { }

    public SsnParsingException(string message, Exception inner)
        : base(message, inner)
    { }

    /// <summary>
    /// Define any additional properties and constructors
    /// </summary>
    public SsnParsingException(string message, string ssn)
        : this(message)
    {
        Ssn = ssn;
    }
}
