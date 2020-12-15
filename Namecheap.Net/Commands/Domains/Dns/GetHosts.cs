using System.Threading.Tasks;
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
    public interface IGetHostsRequest : IHostsRequest
    {
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

}
