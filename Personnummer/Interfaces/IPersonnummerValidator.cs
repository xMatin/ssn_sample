
namespace Personnummer.Interfaces;

/// <summary>
/// IPersonnummerProcessor
/// </summary>
public interface IPersonnummerValidator
{
    bool Valid(string value);
    bool Valid(long value);
}
