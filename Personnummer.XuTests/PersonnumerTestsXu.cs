using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Moq;
using Personnummer.Controller;
using Personnummer.Exceptions;
using Personnummer.Interfaces;
using Xunit;
using Autofac;
using Personnummer.model;
using FluentAssertions;

namespace Personnummer.Tests;

public class PersonnumerTestsXu
{
    private readonly WindsorContainer container;
    // for Autofac
    private static IContainer? Container { get; set; }

    /// <summary>
    ///  https://xunit.net/docs/comparisons
    /// </summary>
    public PersonnumerTestsXu() 
    {
        container = new WindsorContainer();
        container.Register(Component.For<IPersonnummerValidator>().ImplementedBy<PersonnummerValidator>());

        // CREATE AUTFAC CONTAINER
        var builder = new ContainerBuilder();
        builder.RegisterType<PersonnummerValidator>().As<IPersonnummerValidator>();
        // build can only be called once
        Container = builder.Build();
    }

    /* 
     it does not matter what you use here of: container, real object or interface variable
     only showing different usage in this PersonnumerTestsXX class
    */
    /*
    var SsnProc = GetSsnProcessor(container);
    var SsnProc = GetRealSsnObject();
    var SsnProc = GetSsnInterfaceVar();
    */

    [Theory]
    [InlineData("510818-9167", true)]
    [InlineData("19900101-0017", true)]
    [InlineData("19130401+2931", true)]
    [InlineData("130401+2931", true)]
    [InlineData("196408233234", true)]
    [InlineData("0001010107", true)]
    [InlineData("000101-0107", true)]
    [InlineData("550207-3900", true)]
    [InlineData("9701063-2391", false)]
    public void TestPersonnummerString(string value, bool expected)
    {
        // Create object by resolving the IPersonnummerProcessor with WindsorContainer
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        Assert.Equal(SsnProc.Valid(value), expected);

        // Create the scope with Aoutofac, resolve your IPersonnummerProcessor, use it, then dispose of the scope.
        using (var scope = Container?.BeginLifetimeScope())
        {
            SsnProc = scope?.Resolve<IPersonnummerValidator>();
            Assert.Equal(SsnProc?.Valid(value), expected);
        }
    }


    [Theory]
    [InlineData(6403273813, true)]
    [InlineData(5108189167, true)]
    [InlineData(199001010017, true)]
    [InlineData(5502073900, true)]
    public void TestPersonnummerInt(long value, bool expected)
    {
        // Create object by resolving the IPersonnummerProcessor with WindsorContainer
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        Assert.Equal(SsnProc.Valid(value), expected);

        // Create the scope with Aoutofac, resolve your IPersonnummerProcessor, use it, then dispose of the scope.
        using (var scope = Container?.BeginLifetimeScope())
        {
            SsnProc = scope?.Resolve<IPersonnummerValidator>();
            Assert.Equal(SsnProc?.Valid(value), expected);
        }
    }


    [Theory]
    [InlineData(1234567890, false)]
    [InlineData(9987654321, false)]
    public void TestPersonnummerIntNoContainer(long value, bool expected)
    {
        // Create ordinary object
        var SsnProc = new PersonnummerValidator();
        Assert.Equal(SsnProc.Valid(value), expected);
    }


    /// <summary>
    /// Samordningsnummer 
    /// Om personen exempelvis är född den 1 januari 1970 så är samordningsnumret på formen 700161-XXXX.
    /// Till två sista siffrorna i personens födelsedatum adderas siffran 60
    /// </summary>
    /// <param name="value"></param>
    /// <param name="expected"></param>
    [Theory]
    [InlineData("701063-2391", true)]
    [InlineData("640883-3231", true)]
    [InlineData("900161-0017", false)]
    [InlineData("640893-3231", false)]
    [InlineData("550207-3900", true)]
    public void TestCoOrdinationNumbersString(string value, bool expected)
    {
        // Create object by resolving the IPersonnummerProcessor with WindsorContainer
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        Assert.Equal(SsnProc.Valid(value), expected);

        // Create the scope with Aoutofac, resolve your IPersonnummerProcessor, use it, then dispose of the scope.
        using (var scope = Container?.BeginLifetimeScope())
        {
            SsnProc = scope?.Resolve<IPersonnummerValidator>();
            Assert.Equal(SsnProc?.Valid(value), expected);
        }
    }


    /// <summary>
    /// Samordningsnummer 
    /// Om personen exempelvis är född den 1 januari 1970 så är samordningsnumret på formen 700161-XXXX.
    /// Till två sista siffrorna i personens födelsedatum adderas siffran 60
    /// </summary>
    /// <param name="value"></param>
    /// <param name="expected"></param>
    [Theory]
    [InlineData(7010632391, true)]
    [InlineData(6408833231, true)]
    [InlineData(9001610017, false)]
    [InlineData(6408933231, false)]
    [InlineData(5502073900, true)]
    public void TestCoOrdinationNumbersInt(long value, bool expected)
    {
        // Create object by resolving the IPersonnummerProcessor with WindsorContainer
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        Assert.Equal(SsnProc.Valid(value), expected);

        // Create the scope with Aoutofac, resolve your IPersonnummerProcessor, use it, then dispose of the scope.
        using (var scope = Container?.BeginLifetimeScope())
        {
            SsnProc = scope?.Resolve<IPersonnummerValidator>();
            Assert.Equal(SsnProc?.Valid(value), expected);
        }
    }
}


public class PersonnumerBadInput
{
    private readonly WindsorContainer container;

    public PersonnumerBadInput()
    {
        container = new WindsorContainer();
        container.Register(Component.For<IPersonnummerValidator>().ImplementedBy<PersonnummerValidator>());
    }


    [Fact]
    public void TestForNullInput()
    {
        // xUnitTest and msTest use Action, NUnit TestDelegate
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        Action actual = () => SsnProc.Valid(null!);
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact]
    public void TestForEmptyInput()
    {
        // xUnitTest and msTest use Action, NUnit TestDelegate
        var Ssn = container.Resolve<IPersonnummerValidator>();
        Action actual = () => Ssn.Valid("");
        Assert.Throws<ArgumentException>(actual);
    }


    [Theory]
    [InlineData("640327-381")]
    [InlineData("510818-916")]
    [InlineData("19900101-001")]
    [InlineData("100101+001")]
    public void TestForBadInputsStrings(string value)
    {
        // xUnitTest and msTest use Action, NUnit TestDelegate
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        Action actual = () => SsnProc.Valid(value);
        Assert.Throws<SsnParsingException>(actual);
    }


    [Theory]
    [InlineData(640327381)]
    [InlineData(510818916)]
    [InlineData(19900101001)]
    [InlineData(100101001)]
    public void TestForBadInputsInt(long value)
    {
        // xUnitTest and msTest use Action, NUnit TestDelegate
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        Action actual = () => SsnProc.Valid(value);
        Assert.Throws<SsnParsingException>(actual);
    }
}


public class PersonnumerMocking
{
    /// <summary>
    /// Hello World Mock, demonstrating Mock
    /// </summary>
    [Fact]
    public void StubTest()
    {
        string expectedString = "HelloWorldMock";
        var stub = new Mock<IStub>();
        var stub1 = new Mock<IStub>();
        var stub2 = new Mock<IStub>();

        stub1.Setup(x => x.StubMethod1()).Returns("Hello");
        stub.SetupGet(x => x.StubGetSet).Returns("World");
        stub2.Setup(x => x.StubMethod2()).Returns("Mock");
        string actualString = stub1.Object.StubMethod1() + stub.Object.StubGetSet + stub2.Object.StubMethod2();
        Assert.Equal(expectedString, actualString);
    }


    [Fact]
    public void InfoTest()
    {
        string expectedString = "We Can Fake Wathever We Want!";
        var info = new Mock<IInfo>();
        info.Setup(x => x.Show()).Returns("We Can Fake Wathever We Want!");
        string actualString = info.Object.Show();
        Assert.Equal(expectedString, actualString);
    }


    [Fact]
    public void PersonnummerTest()
    {
        var Ssn = "AABBCCDDEEFF";
        var SsnMock = new Mock<IPersonnummerValidator>();
        SsnMock.Setup(x => x.Valid(Ssn)).Returns(true);
        var result = SsnMock.Object.Valid(Ssn);
        Assert.True(result);
    }


    [Fact]
    public void PersonnummerStringSearchTest()
    {
        // Arrange
        var ssn = "AABBCCDDEEFF";
        var name = "Fred Flinstone";
        var person = new Person { Name = name, Ssn = ssn };

        var dbMock = new Mock<IPersonnummerDb>();
        dbMock.Setup(x => x.FindBySsn(It.IsAny<string>())).Returns(person);
        // inject mock object into PersonnummerController
        var controller = new PersonnummerDbController(dbMock.Object);

        // Act
        var actual = controller.FindPerson(ssn);
        // Assert
        Assert.NotNull(actual);
        Assert.Same(person, actual);
        Assert.Equal(ssn, actual.Ssn);
        Assert.Equal(name, actual.Name);
        // FluentAssertions
        actual.ToString().Should().StartWith(name).And.EndWith(ssn).And.Contain(":");
    }

    [Fact]
    public void PersonnummerLongSearchTest()
    {
        // Arrange
        var ssn = 123456789012;
        var name = "Fred Flinstone";
        var person = new Person(name, ssn);

        var dbMock = new Mock<IPersonnummerDb>();
        dbMock.Setup(x => x.FindBySsn(It.IsAny<long>())).Returns(person);
        // inject mock object into PersonnummerController
        var controller = new PersonnummerDbController(dbMock.Object);

        // Act
        var actual = controller.FindPerson(ssn);
        // Assert
        Assert.NotNull(actual);
        Assert.Same(person, actual);
        Assert.Equal(ssn.ToString(), actual.Ssn);
        Assert.Equal(name, actual.Name);
        // FluentAssertions
        actual.ToString().Should().StartWith(name).And.EndWith(ssn.ToString()).And.Contain(":");
    }


    [Theory]
    [MemberData(nameof(GetData))]
    public void PersonnummerAddToDbTest(Person p, bool expected)
    {
        // Arrange
        var dbMock = new Mock<IPersonnummerDb>();
        dbMock.Setup(_ => _.AddRecord(p)).Returns(expected);
        // inject mock object into PersonnummerController
        var controller = new PersonnummerDbController(dbMock.Object);
        // Act 
        var actual = controller.AddPerson(p);
        // Assert
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> GetData()
    {
        yield return new object[] {
            new Person { Name = "Fred Flinstone1", Ssn = "AABBCCDDEEFF" }, true};
        yield return new object[] {
            new Person { Name = "Fred Flinstone2", Ssn = "AABBCCDDEEFF" }, false};
        yield return new object[] {
            new Person { Name = "Fred Flinstone3", Ssn = "AABBCCDDEEFF" }, true};
        yield return new object[] {
            new Person { Name = "Fred Flinstone4", Ssn = "AABBCCDDEEFF" }, false};
    }


    [Fact]
    public void PersonnummerGetRecordsFromDbTest()
    {
        // Arrange
        var dbMock = new Mock<IPersonnummerDb>();
        var expected = GetPersonList();
        dbMock.Setup(_ => _.GetRecords<Person>(It.IsAny<string>())).Returns(expected);
        // inject mock object into PersonnummerController
        var controller = new PersonnummerDbController(dbMock.Object);
        // Act
        var actual = controller.GetPersons(It.IsAny<string>());
        // Assert
        Assert.Equal(expected, actual); // xUnit.Net Assert.Equal recognizes collections
    }

    public static List<Person> GetPersonList()
    {
        return new List<Person> {
            new Person { Name = "Fred Flinstone1", Ssn = "AABBCCDDEEFF" },
            new Person { Name = "Fred Flinstone2", Ssn = "AABBCCDDEEFF" },
            new Person { Name = "Fred Flinstone3", Ssn = "AABBCCDDEEFF" },
            new Person { Name = "Fred Flinstone4", Ssn = "AABBCCDDEEFF" }
        };
    }
}