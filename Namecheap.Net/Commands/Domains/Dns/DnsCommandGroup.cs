using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namecheap.Net.Commands.Domains.Dns
{
    public partial class DnsCommandGroup: NamecheapCommandGroup
    {
        public DnsCommandGroup(NamecheapApi api): base(api) { }
    }
}
