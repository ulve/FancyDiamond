using FancyDiamond.DTO;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyDiamond
{
    public class DiamondParsers
    {
        static readonly Parser<string> NewLine = Parse.String(Environment.NewLine).Text();

        // Line types        
        static readonly Parser<string> SolidLineR = Parse.String("->").Text();

        static readonly Parser<string> DottedLineR = Parse.String("-->").Text();

        static readonly Parser<string> SolidLineL = Parse.String("<-").Text();

        static readonly Parser<string> DottedLineL = Parse.String("<--").Text();

        static readonly Parser<string> LineType = Parse.Or(SolidLineR, DottedLineR).Or(DottedLineL).Or(SolidLineL);

        // Participant
        static readonly Parser<string> ImplicitParticipantEnd = Parse.Or(LineType, Parse.String(":").Text()).Or(NewLine);

        static readonly Parser<Participant> ImplicitParticipant = Parse.AnyChar
                                                                   .Except(ImplicitParticipantEnd)
                                                                   .Many()
                                                                   .Text()
                                                                   .Select(n => new Participant
                                                                   {
                                                                       Name = n.Trim(),
                                                                       Type = "Implicit"
                                                                   });

        static readonly Parser<string> descriptionStart = Parse.String(":").Text();

        static readonly Parser<string> Description = from start in descriptionStart
                                                     from w in Parse.Optional(Parse.WhiteSpace)
                                                     from desc in Parse.CharExcept(Environment.NewLine).Many().Text()
                                                     from e in NewLine
                                                     select desc;

        static Parser<ParticipantOrLine> CreateActorParser(string actorName)
        {
            return from a in Parse.String(actorName).Text()
                   from w in Parse.WhiteSpace
                   from name in Parse.CharExcept(Environment.NewLine).Many().Text()
                   from e in NewLine
                   select new ParticipantOrLine { Participant = new Participant { Name = name, Type = a } };
        }

        // Line
        static readonly Parser<ParticipantOrLine> ImplicitLine = from p1 in ImplicitParticipant
                                                                 from l in LineType
                                                                 from p2 in ImplicitParticipant
                                                                 from d in Parse.Optional(Description)
                                                                 select new ParticipantOrLine
                                                                 {
                                                                     Line = new Line
                                                                     {
                                                                         From = p1,
                                                                         To = p2,
                                                                         Format = l,
                                                                         Description = d.GetOrDefault()
                                                                     }
                                                                 };

        static readonly Parser<ParticipantOrLine> LineParser = CreateActorParser("actor").Or(ImplicitLine);

        // Containers
        static readonly Parser<Loop> LoopParser = from loop in Parse.String("loop")
                                                  from whitespace in Parse.WhiteSpace.Many()
                                                  from desc in Parse.CharExcept(Environment.NewLine).Many().Text()
                                                  from b in NewLine
                                                  from items in ItemParser.Many()
                                                  from end in Parse.String("end")
                                                  from q in NewLine
                                                  select new Loop { Description = desc, Items = items.ToList() };

        static readonly Parser<Item> ContainerParser = LoopParser;

        static readonly Parser<Item> SuperLineParser = from p1 in ImplicitParticipant
                                                       from l in LineType
                                                       from p2 in ImplicitParticipant
                                                       from d in Parse.Optional(Description)
                                                       select new SuperLine
                                                       {
                                                           From = p1,
                                                           To = p2,
                                                           Format = l,
                                                           Description = d.GetOrDefault()
                                                       };
        static readonly Parser<Item> ItemParser = ContainerParser.XOr(SuperLineParser);

        // Diagram
        static readonly Parser<string> Start = Parse.String("@startuml" + Environment.NewLine).Text();

        static readonly Parser<string> End = Parse.String("@enduml").Text();

        static readonly Parser<List<ParticipantOrLine>> DiagramParser = from s in Start
                                                                        from l in LineParser.Many()
                                                                        from e in End
                                                                        select l.ToList();


        public static List<Item> NextGenParser(string text)
        {
            var l = ItemParser.Many().Parse(text);

            return l.ToList();
        }

        public static Diagram ParseDiagram(string text)
        {
            var d = DiagramParser.Parse(text);

            var participants = d.SelectMany(l => l.Participant != null // Get all parsed participants 
                    ? new List<Participant>() { l.Participant }
                    : new List<Participant>() { l.Line.From, l.Line.To })
                .GroupBy(p => p.Name) // Remove duplicates
                .Select(p => p.First());

            var lines = d.Where(l => l.Line != null).Select(l => l.Line);

            return new Diagram
            {
                Name = "text",
                Participants = participants.ToList(),
                Lines = lines.ToList()
            };
        }
    }
}
