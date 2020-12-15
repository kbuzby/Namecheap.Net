using System.Xml.Serialization;

namespace Namecheap.Net.Commands.Domains.Dns
{
    public interface IHostsRequest
    {
        [QueryParam("SLD")]
        public string Sld { get; }
        [QueryParam("TLD")]
        public string Tld { get; }
    }
    public class Host
    {
        [XmlAttribute]
        public string? HostId { get; set; }
        [XmlAttribute]
        [QueryParam("HostName")]
        public string? Name { get; set; }
        [XmlAttribute]
        [QueryParam("RecordName")]
        public string? Type { get; set; }
        [XmlAttribute]
        [QueryParam]
        public string? Address { get; set; }
        [XmlAttribute]
        [QueryParam]
        public string? MXPref { get; set; }
        [XmlAttribute]
        [QueryParam]
        public int TTL { get; set; }
        [XmlAttribute]
        public string? AssociatedAppTitle { get; set; }
        [XmlAttribute]
        public string? FriendlyName { get; set; }
        [XmlAttribute]
        public bool IsActive { get; set; }
        [XmlAttribute]
        public bool IsDDNSEnabled { get; set; }
    }
}
