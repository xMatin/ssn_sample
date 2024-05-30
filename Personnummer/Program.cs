
using System.Text;
// https://github.com/tomakita/Colorful.Console
using Console = Colorful.Console;
using System.Drawing;
using Figgle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Personnummer.Interfaces;
using Personnummer.Exceptions;
using Personnummer.Controller;
using Moq;
using Autofac;
using Personnummer.model;


/// <summary>
/// Modified program from: https://github.com/personnummer/csharp
/// This module give some example usage and demonstrate four different ways of run/test the library
// 1. Autofac, 2. Windsor, 3. real object and 4. interface
/// </summary>
namespace Personnummer;

class Program
{
    // for Autofac
    private static IContainer? Container { get; set; }

    /// <summary>
    /// Castle Windsor https://github.com/castleproject/Windsor is best of breed, mature Inversion of Control
    /// https://github.com/castleproject/Windsor/blob/master/docs/ioc.md container available for .NET.
    /// </summary>
    /// <returns></returns>
    private static IPersonnummerValidator GetSsnValidator(WindsorContainer container)
    {
        // CREATE A WINDSOR CONTAINER OBJECT AND REGISTER THE INTERFACES, AND THEIR CONCRETE IMPLEMENTATIONS
        container.Register(Component.For<IPersonnummerValidator>().ImplementedBy<PersonnummerValidator>());
        // CREATE THE MAIN OBJECTS AND INVOKE ITS METHOD(S) AS DESIRED
        return container.Resolve<IPersonnummerValidator>();
    }

    private static IInfo GetInfo(WindsorContainer container)
    {
        // CREATE A WINDSOR CONTAINER OBJECT AND REGISTER THE INTERFACES, AND THEIR CONCRETE IMPLEMENTATIONS
        container.Register(Component.For<IInfo>().ImplementedBy<Info>());
        // CREATE THE MAIN OBJECTS AND INVOKE ITS METHOD(S) AS DESIRED
        return container.Resolve<IInfo>();
    }


    /// <summary>
    /// Autofac https://github.com/autofac/ 
    /// https://autofac.org/
    /// https://autofaccn.readthedocs.io/en/latest/getting-started/index.html
    /// </summary>
    /// <returns></returns>
    private static void BuildContainer(ContainerBuilder builder)
    {
        builder.RegisterType<PersonnummerValidator>().As<IPersonnummerValidator>();
        builder.RegisterType<Info>().As<IInfo>();
        // build can only be called once
        Container = builder.Build();
    }

    /// <summary>
    /// Get real PersonnummerProcessor object, not using any interface
    /// </summary>
    /// <returns></returns>
    private static PersonnummerValidator GetSsnValidatorObject()
    {
        return new PersonnummerValidator();
    }
    private static Info GetInfoObject()
    {
        return new Info();
    }
    /// <summary>
    /// Get an IPersonnummerProcessor interface variable
    /// </summary>
    /// <returns></returns>
    private static IPersonnummerValidator GetSsnValidatorInterface()
    {
        return new PersonnummerValidator();
    }
    private static IInfo GetInfoInterface()
    {
        return new Info();
    }

    static void Main(string[] args)
    {
        // render chars correct in the console output
        Console.OutputEncoding = Encoding.Default;
        bool showMenu = true;

        // CREATE AUTFAC CONTAINER
        var builder = new ContainerBuilder();
        BuildContainer(builder);

        // CREATE A WINDSOR CONTAINER OBJECT AND REGISTER THE INTERFACES, AND THEIR CONCRETE IMPLEMENTATIONS
        var container = new WindsorContainer();
        
        // it does not matter what you use here of container, real object or interface variable
        // for the non-mocking menu alternative C
        
        // 1. Autofac
        //var SsnValidator = Container.Resolve<IPersonnummerValidator>();
        //var Info = Container.Resolve<IInfo>();

        // 2. Windsor
        var SsnValidator = GetSsnValidator(container);
        var Info = GetInfo(container);
        
        // 3. real object
        //var SsnValidator = GetSsnValidatorObject();
        //var Info = GetInfoObject();
        
        // 4. interface
        //var SsnValidator = GetSsnValidatorInterface();
        //var Info = GetInfoInterface();
        
        Console.WriteLine(FiggleFonts.Standard.Render("SSN Check"));

        Console.WriteLine(Info.Show());
        Console.WriteLine("Choose 'C' to Check a SSN", Color.Yellow);
        Console.WriteLine("Choose 'M' to Mock a SSN", Color.Yellow);
        Console.WriteLine("Choose 'D' to Find a Person via Mock, DI and SSN", Color.Yellow);
        //Console.WriteLine("Choose '???' to Perform other command");
        Console.WriteLine("Choose 'Q' to Exit");

        while (showMenu)
        {
            Console.Write("\r\nPerform your choice: ");

            string command = Console.ReadLine();

            try
            {
                switch (command)
                {
                    case "C":
                        {
                            Console.WriteLine("Enter SSN: ", Color.Yellow);
                            string ssn = Console.ReadLine();
                            bool result = SsnValidator.Valid(ssn);
                            if (result)
                                Console.WriteLine(result, Color.Green);
                            else
                                Console.WriteLine(result, Color.Red);
                        }
                        break;
                    case "M":
                        {
                            Console.WriteLine("Enter a Mock SSN: ", Color.Yellow);
                            string ssn = Console.ReadLine();
                            var SsnMock = new Mock<IPersonnummerValidator>();
                            SsnMock.Setup(x => x.Valid(ssn)).Returns(false);
                            bool result = SsnMock.Object.Valid(ssn);
                            if (result)
                                Console.WriteLine(result, Color.Green);
                            else
                                Console.WriteLine(result, Color.Red);
                        }
                        break;
                    case "D":
                        {
                            Console.WriteLine("Find a Person by SSN: ", Color.Yellow);
                            string ssn = Console.ReadLine();
                            string name = "Anders Tegnell";
                            var person = new Person(name, ssn);
                            var DbMock = new Mock<IPersonnummerDb>();
                            DbMock.Setup(x => x.FindBySsn(ssn)).Returns(person);

                            // inject mock object into PersonnummerController
                            var controller = new PersonnummerDbController(DbMock.Object);
                            var result = controller.FindPerson(ssn);
                            if (result != null)
                                Console.WriteLine(result.ToString(), Color.Green);
                            else
                                Console.WriteLine("Person not found!", Color.Red);
                        }
                        break;
                    case "Q":
                        showMenu = false;
                        break;
                    default:
                        Console.WriteLine("Unknown command!", Color.Red);
                        break;
                }
            }
            catch (SsnParsingException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
