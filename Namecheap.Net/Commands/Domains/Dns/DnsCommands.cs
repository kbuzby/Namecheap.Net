using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namecheap.Net
{
    public partial class DnsCommands: NamecheapCommandGroup
    {
        public DnsCommands(NamecheapApi api): base(api) { }
    }
}
