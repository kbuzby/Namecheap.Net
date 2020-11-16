using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namecheap.Net.Tests.Integration
{
#nullable disable
    class DnsGetHostsRequest : DnsCommands.IGetHostsRequest
    {
        public string Sld { get; set; } 

        public string Tld  { get; set; }
    }
}
