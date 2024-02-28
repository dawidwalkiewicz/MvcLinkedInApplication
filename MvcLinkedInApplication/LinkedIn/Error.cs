using System;
using System.Xml.Serialization;

namespace MvcLinkedInApplication.LinkedIn
{
    [Serializable, XmlRoot("error")]
    public class Error
    {
        [XmlElement("status")]
        public byte Status { get; set; }

        [XmlElement("timestamp")]
        public int Timestamp { get; set; }

        [XmlElement("request-id")]
        public string RequestId { get; set; }

        [XmlElement("error-code")]
        public byte ErrorCode { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }
    }
}