﻿using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Namecheap.Net.Commands.Domains.Dns
{
    public partial class DnsCommandGroup
    {
        internal const string GetHostsCommand = "namecheap.domains.dns.getHosts";

        public async Task<ApiResponse<GetHostsResponse>?> GetHosts(string sld, string tld)
        {
            return await GetHosts(new GetHostsRequest(sld, tld));
        }

        public async Task<ApiResponse<GetHostsResponse>?> GetHosts(IGetHostsRequest hostsRequest)
        {
            return await ExecuteCommand<IGetHostsRequest, GetHostsResponse>(hostsRequest);
        }

    }
    [NamecheapApiCommand(DnsCommandGroup.GetHostsCommand)]
    public interface IGetHostsRequest
    {
        [QueryParam("SLD")]
        public string Sld { get; }
        [QueryParam("TLD")]
        public string Tld { get; }
    }

    public record GetHostsRequest : IGetHostsRequest
    {
        public string Sld { get; }
        public string Tld { get; }

        public GetHostsRequest(string sld, string tld)
        {
            Sld = sld;
            Tld = tld;
        }
    }

    public class GetHostsResponse : CommandResponse
    {
        public override string Type => DnsCommandGroup.GetHostsCommand;

        [XmlElement]
        public DomainDNSGetHostsResult? DomainDNSGetHostsResult { get; set; }
    }

    public class DomainDNSGetHostsResult
    {
        [XmlAttribute]
        public string? Domain { get; set; }

        [XmlAttribute]
        public bool IsUsingOurDNS { get; set; }

        [XmlElement(ElementName = "host")]
        public Host[]? Host { get; set; }
    }

    public class Host
    {
        [XmlAttribute]
        public string? HostId { get; set; }
        [XmlAttribute]
        public string? Name { get; set; }
        [XmlAttribute]
        public string? Type { get; set; }
        [XmlAttribute]
        public string? Address { get; set; }
        [XmlAttribute]
        public string? MXPref { get; set; }
        [XmlAttribute]
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
