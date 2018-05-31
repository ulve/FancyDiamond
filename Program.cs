using System;
using FancyDiamond.DTO;

namespace FancyDiamond
{
    public class Program
    {
        static readonly string testStr =
@"@startuml
Alice -> Bob: Authentication Request
alt successful case
	Bob -> Alice: Authentication Accepted
else some kind of failure
	Bob -> Alice: Authentication Failure
	group My own label
		Alice -> Log : Log attack start
	    loop 1000 times
	        Alice -> Bob: DNS Attack
	    end
		Alice -> Log : Log attack end
	end
else Another type of failure
   Bob -> Alice: Please repeat
end
@enduml
";

        static readonly string test2 =

@"Alice -> Bob: ett
loop looooopen
Alice -> Bob: två
loop andra loopen
Räkan -> Hatta: Booob
end
Alice -> Bob: tre
end
Alice -> Bob: fyra
";

        static void Main(string[] args)
        {
            Console.WriteLine("Parsar diagram");
            var l = DiamondParsers.NextGenParser(test2);
            foreach (var a in l)
            {
                Console.WriteLine(a.Description);
                if (a is ContainerItem)
                {
                    foreach (var aa in (a as ContainerItem).Items)
                    {
                        Console.WriteLine(" " + aa.Description);
                        if (aa is ContainerItem)
                        {
                                Console.WriteLine("  " +aa.Description);                            
                            foreach (var aaa in (aa as ContainerItem).Items)
                            {
                                Console.WriteLine("   " +aaa.Description);
                            }
                        }
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
