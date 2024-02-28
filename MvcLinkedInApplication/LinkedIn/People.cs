using System;
using System.Xml.Serialization;

namespace MvcLinkedInApplication.LinkedIn
{
    [Serializable, XmlRoot("person")]
    public class Person
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("first-name")]
        public string FirstName { get; set; }

        [XmlElement("last-name")]
        public string LastName { get; set; }

        [XmlElement("headline")]
        public string Headline { get; set; }

        [XmlElement("url")]
        public string ProfileUrl { get; set; }
    }

    [Serializable]
    public class People
    {
        [System.Xml.Serialization.XmlElement("person")]
        public Person[] Persons { get; set; }

        [XmlAttribute("total")]
        public int Total { get; set; }

        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlAttribute("start")]
        public int Start { get; set; }
    }

    [Serializable, XmlRoot("people-search")]
    public class PeopleSearchResult
    {
        [XmlElement("people")]
        public People People { get; set; }

        [XmlElement("num-results")]
        public int Count { get; set; }
    }
}