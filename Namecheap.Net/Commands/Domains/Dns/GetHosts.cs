using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Namecheap.Net
{
    public partial class DnsCommands
    {
        private const string CommandName = "namecheap.domains.dns.getHosts";

        public async Task<ApiResponse<GetHostsResponse>?> GetHosts(string sld, string tld)
        {
            return await GetHosts(new GetHostsRequest(sld, tld));
        }

        public async Task<ApiResponse<GetHostsResponse>?> GetHosts(GetHostsRequest hostsRequest)
        {
            return await ExecuteCommand<GetHostsRequest,GetHostsResponse>(hostsRequest);
        }

        [NamecheapApiCommand(CommandName)]
        public record GetHostsRequest
        {
            [QueryParam("SLD")]
            public string Sld { get; }
            [QueryParam("TLD")]
            public string Tld { get; }

            public GetHostsRequest(string sld, string tld)
            {
                Sld = sld;
                Tld = tld;
            }
        }

        public class GetHostsResponse : CommandResponse
        {
            public override string Type => CommandName;

            [XmlElement]
            public DomainDNSGetHostsResult? DomainDNSGetHostsResult { get; set; }
        }

        public class DomainDNSGetHostsResult
        {
            [XmlAttribute]
            public string? Domain { get; set; }

            [XmlAttribute]
            public string? IsUsingOurDNS { get; set; }

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
            public string? TTL { get; set; }
            [XmlAttribute]
            public string? AssociatedAppTitle { get; set; }
            [XmlAttribute]
            public string? FriendlyName { get; set; }
            [XmlAttribute]
            public string? IsActive { get; set; }
            [XmlAttribute]
            public string? IsDDNSEnabled { get; set; }
        }
    }

}
