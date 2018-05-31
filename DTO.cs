using System;
using System.Collections.Generic;

namespace FancyDiamond.DTO
{
    public class ParticipantOrLine
    {
        public Participant Participant { get; set; }

        public Line Line { get; set; }
    }

    public class Participant
    {
        public string Name { get; set; }

        public string Type { get; set; }   
    }

    public class Line
    {
        public string Format { get; set; }

        public string Description { get; set; }

        public Participant From { get; set; }

        public Participant To { get; set; }
    }

    public class Diagram
    {
        public List<Participant> Participants { get; set; }

        public string Name { get; set; }

        public List<Line> Lines { get; set; }
    }

    public class Item
    {
        public string Description { get; set; }
    }

    public class ContainerItem : Item 
    {
        public List<Item> Items { get; set; }        

    }

    public class SuperLine : Item
    {
        public Participant From { get; set; }
        public Participant To { get; set; }
        public string LineType { get; set; }
        public string Format { get; set; }
    }

    public class Loop : ContainerItem
    {
    }

    public class Alt : ContainerItem
    {
        public Alt Next { get; set; }
    }

    public class Group : ContainerItem
    {
    }
}
