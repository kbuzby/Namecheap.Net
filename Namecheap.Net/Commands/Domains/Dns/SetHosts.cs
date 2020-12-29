using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Namecheap.Net.Commands.Domains.Dns
{
    public partial class DnsCommandGroup
    {
        internal const string SetHostsCommand = "namecheap.domains.dns.setHosts";

        public async Task<ApiResponse<SetHostsResponse>?> SetHosts(string sld, string tld, IEnumerable<Host> hosts, string? emailType = null, ushort? flag = null, string? tag = null)
        {
            return await SetHosts(new SetHostsRequest(sld, tld, hosts, emailType, flag, tag));
        }

        public async Task<ApiResponse<SetHostsResponse>?> SetHosts(ISetHostsRequest request)
        {
            return await ExecuteCommand<ISetHostsRequest, SetHostsResponse>(request);
        }
    }

    [NamecheapApiCommand(DnsCommandGroup.SetHostsCommand)]
    public interface ISetHostsRequest : IHostsRequest
    {
        [QueryParam]
        IEnumerable<Host> Hosts { get; }

        [QueryParam(Optional = true)]
        string? EmailType { get; }

        [QueryParam(Optional = true)]
        ushort? Flag { get; }

        [QueryParam(Optional = true)]
        string? Tag { get; }

    }

    public class SetHostsRequest : ISetHostsRequest
    {
        public string Sld { get; }
        public string Tld { get; }
        public IEnumerable<Host> Hosts { get; }
        public string? EmailType { get; }
        public ushort? Flag { get; }
        public string? Tag { get; }

        public SetHostsRequest(string sld, string tld, IEnumerable<Host> hosts, string? emailType = null, ushort? flag = null, string? tag = null)
        {
            Sld = sld;
            Tld = tld;
            Hosts = hosts;
            EmailType = emailType;
            Flag = flag;
            Tag = tag;
        }
    }
    public class SetHostsResponse : CommandResponse
    {
        public override string Type => DnsCommandGroup.SetHostsCommand;

        [XmlElement]
        public DomainDNSSetHostsResult? DomainDNSSetHostsResult { get; set; }
    }

    public class DomainDNSSetHostsResult
    {
        [XmlAttribute]
        public string? Domain { get; set; }
        [XmlAttribute]
        public bool IsSuccess { get; set; }
    }
}
