
Sample exam in software testing.
Interfaces and mocking are present together with Dependency Injection using Castle Windsor or Autofac

# Personnummer
- https://workinginsweden.se/personal-identity-number-and-coordination-number/
- https://sv.wikipedia.org/wiki/Personnummer_i_Sverige
- https://en.wikipedia.org/wiki/Personal_identity_number_(Sweden)
- https://sv.wikipedia.org/wiki/Samordningsnummer
- Samordningsnummer ser ut som ett personnummer, men till tv� sista siffrorna i personens f�delsedatum 
  adderas siffran 60, vilket ger ett tal mellan 61 och 91 ist�llet f�r mellan 01 och 31.
  Om personen exempelvis �r f�dd den 1 januari 1970 s� �r samordningsnumret p� formen 700161-XXXX.
- Bindestrecket (-) �ndras till plustecken (+) det �r som personen fyller 100. 
  D�rmed kan man skilja p� en 1-�ring och en 101-�ring.

The first goal is to create a set of Unit Tests that can test the current functionality of the Personnummer library.
- Test for bad input
- Test that the algorithms works (Personnnummer, Samordningsnummer, �lder)
- Test that exceptons are thrown when needed
By running the current program (Program.cs) you can examine some usage of the library.

Task 1
You have to implement missing code and create Unit Tests that can validate and verify the Personnummer algorithm. 
One can for example use the TDD (Test Driven Development) methodology for this work.
Using IoC with some kind of Dependency Injection is desirable.

Task 2
Mocking
Some of the requirements and or functionality can be mocked using the current interfaces etc.
Using IoC with some kind of Dependency Injection is desirable.
- See the IPersonnummerDb interface for more info

Task 3
Generate reports for code coverage of your performed Unit Tests and code complexity metrics.
