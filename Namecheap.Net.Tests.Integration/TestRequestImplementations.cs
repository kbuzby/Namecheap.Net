#nullable disable
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

        namespace Dns
        {
            class GetHostsRequest : Commands.Domains.Dns.IGetHostsRequest
            {
                public string Sld { get; set; }
                public string Tld { get; set; }
            }

        }
    }
}
