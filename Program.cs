using System;

namespace FancyDiamond
{    
    public class Program
    {
        static readonly string testStr =
@"@startuml
Alice -> Bob: Katthår
actor Fredde fisk
actor Anders apa
Bob --> Alice: Authentication Response
Alice -> Bob: Another authentication Request
actor Olov ocelot
Alice <-- Bob: another authentication Response
@enduml";

        static void Main(string[] args)
        {
            Console.WriteLine("Parsar diagram");
            var l = DiamondParsers.ParseDiagram(testStr);
            Console.ReadKey();
        }
    }
}
