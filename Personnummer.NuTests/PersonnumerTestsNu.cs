using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Moq;
using Personnummer.Controller;
using Personnummer.Interfaces;
using NUnit.Framework;
using Personnummer.Exceptions;
using Autofac;
using Personnummer.model;
using FluentAssertions;

namespace Personnummer.Tests;

[TestFixture]
public class PersonnumerTestsNu
{
    private readonly WindsorContainer container;
    // for Autofac
    private static IContainer? Container { get; set; }

    [SetUp]
    public void Setup()
    {
        // Runs before each test. (Optional)
    }

    [TearDown]
    public void TearDown()
    {
        // Runs after each test. (Optional)
    }


    /// <summary>
    ///  https://xunit.net/docs/comparisons
    /// </summary>
    public PersonnumerTestsNu() 
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
    [TestCase("510818-9167", true)]
    [TestCase("19900101-0017", true)]
    [TestCase("19130401+2931", true)]
    [TestCase("130401+2931", true)]
    [TestCase("196408233234", true)]
    [TestCase("0001010107", true)]
    [TestCase("000101-0107", true)]
    [TestCase("550207-3900", true)]
    [TestCase("9701063-2391", false)]
    public void TestPersonnummerString(string value, bool expected)
    {
        // Create object by resolving the IPersonnummerProcessor with WindsorContainer
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        Assert.That(SsnProc.Valid(value), Is.EqualTo(expected));

        // Create the scope with Autofac, resolve your IPersonnummerProcessor, use it, then dispose of the scope.
        using (var scope = Container?.BeginLifetimeScope())
        {
            SsnProc = scope?.Resolve<IPersonnummerValidator>();
            Assert.That(SsnProc?.Valid(value), Is.EqualTo(expected));
        }
    }


    [Theory]
    [TestCase(6403273813, true)]
    [TestCase(5108189167, true)]
    [TestCase(199001010017, true)]
    [TestCase(5502073900, true)]
    public void TestPersonnummerInt(long value, bool expected)
    {
        // Create object by resolving the IPersonnummerProcessor with WindsorContainer
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        Assert.That(SsnProc.Valid(value), Is.EqualTo(expected));

        // Create the scope with Autofac, resolve your IPersonnummerProcessor, use it, then dispose of the scope.
        using (var scope = Container?.BeginLifetimeScope())
        {
            SsnProc = scope?.Resolve<IPersonnummerValidator>();
            Assert.That(SsnProc?.Valid(value), Is.EqualTo(expected));
        }
    }


    [Theory]
    [TestCase(1234567890, false)]
    [TestCase(9987654321, false)]
    public void TestPersonnummerIntNoContainer(long value, bool expected)
    {
        // Create ordinary object
        var SsnProc = new PersonnummerValidator();
        Assert.That(SsnProc.Valid(value), Is.EqualTo(expected));
    }


    /// <summary>
    /// Samordningsnummer 
    /// Om personen exempelvis är född den 1 januari 1970 så är samordningsnumret på formen 700161-XXXX.
    /// Till två sista siffrorna i personens födelsedatum adderas siffran 60
    /// </summary>
    /// <param name="value"></param>
    /// <param name="expected"></param>
    [Theory]
    [TestCase("701063-2391", true)]
    [TestCase("640883-3231", true)]
    [TestCase("900161-0017", false)]
    [TestCase("640893-3231", false)]
    [TestCase("550207-3900", true)]
    public void TestCoOrdinationNumbersString(string value, bool expected)
    {
        // Create object by resolving the IPersonnummerProcessor with WindsorContainer
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        Assert.That(SsnProc.Valid(value), Is.EqualTo(expected));

        // Create the scope with Autofac, resolve your IPersonnummerProcessor, use it, then dispose of the scope.
        using (var scope = Container?.BeginLifetimeScope())
        {
            SsnProc = scope?.Resolve<IPersonnummerValidator>();
            Assert.That(SsnProc?.Valid(value), Is.EqualTo(expected));
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
    [TestCase(7010632391, true)]
    [TestCase(6408833231, true)]
    [TestCase(9001610017, false)]
    [TestCase(6408933231, false)]
    [TestCase(5502073900, true)]
    public void TestCoOrdinationNumbersInt(long value, bool expected)
    {
        // Create object by resolving the IPersonnummerProcessor with WindsorContainer
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        Assert.That(SsnProc.Valid(value), Is.EqualTo(expected));

        // Create the scope with Autofac, resolve your IPersonnummerProcessor, use it, then dispose of the scope.
        using (var scope = Container?.BeginLifetimeScope())
        {
            SsnProc = scope?.Resolve<IPersonnummerValidator>();
            Assert.That(SsnProc?.Valid(value), Is.EqualTo(expected));
        }
    }
}



[TestFixture]
public class PersonnumerBadInput
{
    private readonly WindsorContainer container;

    public PersonnumerBadInput()
    {
        container = new WindsorContainer();
        container.Register(Component.For<IPersonnummerValidator>().ImplementedBy<PersonnummerValidator>());
    }


    [Test]
    public void TestForNullInput()
    {
        // xUnitTest and msTest use Action, NUnit TestDelegate
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        TestDelegate actual = () => SsnProc.Valid(null!);
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Test]
    public void TestForEmptyInput()
    {
        // xUnitTest and msTest use Action, NUnit TestDelegate
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        TestDelegate actual = () => SsnProc.Valid("");
        Assert.Throws<ArgumentException>(actual);
    }

    [Theory]
    [TestCase("640327-381")]
    [TestCase("510818-916")]
    [TestCase("19900101-001")]
    [TestCase("100101+001")]
    public void TestForBadInputsStrings(string value)
    {
        // xUnitTest and msTest use Action, NUnit TestDelegate
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        TestDelegate actual = () => SsnProc.Valid(value);
        Assert.Throws<SsnParsingException>(actual);
    }


    [Theory]
    [TestCase(640327381)]
    [TestCase(510818916)]
    [TestCase(19900101001)]
    [TestCase(100101001)]
    public void TestForBadInputsInt(long value)
    {
        // xUnitTest and msTest use Action, NUnit TestDelegate
        var SsnProc = container.Resolve<IPersonnummerValidator>();
        TestDelegate actual = () => SsnProc.Valid(value);
        Assert.Throws<SsnParsingException>(actual);
    }
}



[TestFixture]
public class PersonnumerMocking
{
    /// <summary>
    /// Hello World Mock, demonstrating Mock
    /// </summary>
    [Test]
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
        Assert.That(actualString, Is.EqualTo(expectedString));
    }


    [Test]
    public void InfoTest()
    {
        string expectedString = "We Can Fake Wathever We Want!";
        var info = new Mock<IInfo>();
        info.Setup(x => x.Show()).Returns("We Can Fake Wathever We Want!");
        string actualString = info.Object.Show();
        Assert.That(actualString, Is.EqualTo(expectedString));
    }


    [Test]
    public void PersonnummerTest()
    {
        string Ssn = "AABBCCDDEEFF";
        var SsnMock = new Mock<IPersonnummerValidator>();
        SsnMock.Setup(x => x.Valid(Ssn)).Returns(true);
        var result = SsnMock.Object.Valid(Ssn);
        Assert.That(result, Is.True);
    }


    [Test]
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
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.SameAs(person));
        Assert.That(actual.Ssn, Is.EqualTo(ssn));
        Assert.That(actual.Name, Is.EqualTo(name));
        // FluentAssertions
        actual.ToString().Should().StartWith(name).And.EndWith(ssn).And.Contain(":");
    }


    [Test]
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
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.SameAs(person));
        Assert.That(ssn.ToString(), Is.EqualTo(actual.Ssn));
        Assert.That(name, Is.EqualTo(actual.Name));
        // FluentAssertions
        actual.ToString().Should().StartWith(name).And.EndWith(ssn.ToString()).And.Contain(":");
    }


    [Test]
    public void PersonnummerAddToDbTest()
    {
        // Arrange
        var ssn = "AABBCCDDEEFF";
        var name = "Fred Flinstone";
        var person = new Person { Name = name, Ssn = ssn };

        var dbMock = new Mock<IPersonnummerDb>();
        dbMock.Setup(x => x.AddRecord(person)).Returns(true);
        // inject mock object into PersonnummerController
        var controller = new PersonnummerDbController(dbMock.Object);

        // Act
        var result = controller.AddPerson(person);
        // Assert
        Assert.That(result, Is.True);
    }


    [Theory]
    [TestCaseSource(nameof(GetData))]
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
        Assert.That(actual, Is.EqualTo(expected));
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


    [Test]
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
        //CollectionAssert.AreEqual(expected, actual);
        Assert.That(actual, Is.EqualTo(expected));
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