using Namecheap.Net.Commands.Domains.Dns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Namecheap.Net.Commands.Domains
{
    public partial class DomainsCommandGroup: NamecheapCommandGroup
    {
        private readonly Lazy<DnsCommandGroup> _dns;
        public DnsCommandGroup Dns => _dns.Value;

        public DomainsCommandGroup(NamecheapApi api): base(api) { 
            _dns = new Lazy<DnsCommandGroup>(() => new DnsCommandGroup(Api));
        }
    }
}
