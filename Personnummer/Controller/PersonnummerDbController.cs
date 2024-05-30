using Personnummer.Exceptions;
using Personnummer.Interfaces;
using Personnummer.model;

namespace Personnummer.Controller;

public class PersonnummerDbController
{
    /// <summary>
    /// PersonnummerController implementation which is fully mocked
    /// </summary>
    public IPersonnummerDb _database;

    /// <summary>
    /// Constructor that takes an IPersonnummerDb interface as argument
    /// </summary>
    public PersonnummerDbController(IPersonnummerDb pdb)
    {
        _database = pdb;
    }

    public Person FindPerson(string value)
    {
        //throw new NotImplementedException();
        try 
        { 
            return _database.FindBySsn(value); 
        }
        catch (Exception ex) 
        { 
            throw new SsnParsingException(ex.Message);
        }
    }
    public Person FindPerson(long value)
    {
        //throw new NotImplementedException();
        try 
        { 
            return _database.FindBySsn(value); 
        }
        catch (Exception ex) 
        { 
            throw new SsnParsingException(ex.Message); 
        }
    }

    public bool AddPerson(Person personObj)
    {
        //throw new NotImplementedException();
        try 
        { 
            return _database.AddRecord(personObj); 
        }
        catch (Exception ex) 
        { 
            throw new SsnParsingException(ex.Message); 
        }
    }

    public List<Person> GetPersons(string sql)
    {
        //throw new NotImplementedException();
        try 
        { 
            return _database.GetRecords<Person>(sql); 
        }
        catch (Exception ex) 
        { 
            throw new SsnParsingException(ex.Message); 
        }
    }
}
