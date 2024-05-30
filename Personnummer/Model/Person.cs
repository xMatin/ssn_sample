
namespace Personnummer.model;

/// <summary>
/// Person implementation
/// </summary>
public class Person
{
    public string Name { get; set; }
    public string Ssn { get; set; }

    /// <summary>
    /// Constructor that takes no arguments
    /// </summary>
    public Person() : this("unknown", "unknown")
    { }

    /// <summary>
    /// Constructor that takes two arguments
    /// </summary>
    /// <param name="name"></param>
    /// <param ssn="ssn"></param>
    public Person(string name, string ssn)
    {
        Name = name;
        Ssn = ssn;
    }

    public Person(string name, long ssn) : this(name, ssn.ToString())
    { }

    /// <summary>
    /// Method that overrides the base class (System.Object) implementation.
    /// </summary>
    public override string ToString()
    {
        return Name + ":" + Ssn;
    }
}