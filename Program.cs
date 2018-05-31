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
            //  var a = @"Alice -> Bob: Katthår";
            //var l = DiamondParsers.DiagramParser.Parse(testStr);
            var l = DiamondParsers.ParseDiagram(testStr);
            //var a = DiagramParser.Parse(testStr);

            //Console.WriteLine(a.Name);
            //foreach(var l in a.Lines)
            //{
            //    Console.WriteLine($"{l.From.Name} {l.Format} {l.To.Name}: {l.Description}");
            //}
            Console.ReadKey();
        }
    }
}
