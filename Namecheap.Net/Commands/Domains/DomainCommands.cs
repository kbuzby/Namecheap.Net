using Namecheap.Net.Commands.Domains.Dns;
using System;

namespace Namecheap.Net.Commands.Domains
{
    public partial class DomainsCommandGroup : NamecheapCommandGroup
    {
        private readonly Lazy<DnsCommandGroup> _dns;
        public DnsCommandGroup Dns => _dns.Value;

        public DomainsCommandGroup(NamecheapApi api) : base(api)
        {
            _dns = new Lazy<DnsCommandGroup>(() => new DnsCommandGroup(Api));
        }
    }
}
