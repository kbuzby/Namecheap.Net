#nullable disable
using System.Collections.Generic;

namespace Namecheap.Net.Tests.Integration
{
    namespace Test.Domains
    {
        class GetListRequest : Commands.Domains.IGetListRequest
        {
            public string ListType { get; set; }
            public string SearchTerm { get; set; }
            public int? Page { get; set; }
            public int? PageSize { get; set; }
            public string SortBy { get; set; }
        }

        class GetContactsRequest : Commands.Domains.IGetContactsRequest
        {
            public string DomainName { get; set; }
        }

        namespace Dns
        {
            class GetHostsRequest : Commands.Domains.Dns.IGetHostsRequest
            {
                public string Sld { get; set; }
                public string Tld { get; set; }
            }

            class SetHostsRequest : Commands.Domains.Dns.ISetHostsRequest
            {
                public IEnumerable<Commands.Domains.Dns.Host> Hosts { get; set; }

                public string EmailType { get; set; }

                public ushort? Flag { get; set; }

                public string Tag { get; set; }

                public string Sld { get; set; }

                public string Tld { get; set; }
            }

        }
    }
}
