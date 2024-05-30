using Personnummer.model;

namespace Personnummer.Interfaces;

/// <summary>
/// Mocked
/// IPersonnummerDb - find Person by ssn, add a Person and get a list of Persons
/// See PersonnummerController.cs for more info
/// </summary>
public interface IPersonnummerDb
{
    Person FindBySsn(string ssn);
    Person FindBySsn(long ssn);
    bool AddRecord(Person person);
    List<T> GetRecords<T>(string sql);
}
