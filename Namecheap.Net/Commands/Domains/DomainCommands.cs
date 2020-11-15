using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Namecheap.Net
{
    public class DomainCommands: NamecheapCommandGroup
    {
        private readonly Lazy<DnsCommands> _dns;
        public DnsCommands Dns => _dns.Value;

        public DomainCommands(NamecheapApi api): base(api) { 
            _dns = new Lazy<DnsCommands>(() => new DnsCommands(Api));
        }
    }
}
