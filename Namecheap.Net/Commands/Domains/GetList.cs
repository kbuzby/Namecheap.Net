using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Namecheap.Net.Commands.Domains
{
    public partial class DomainsCommandGroup
    {
        internal const string GetListCommand = "namecheap.domains.getList";
        public async Task<ApiResponse<GetListResponse>?> GetList(string listType, string searchTerm, int page, int pageSize, string sortBy)
        {
            return await GetList(new GetListRequest(searchTerm) { ListType = listType, Page = page, PageSize = pageSize, SortBy = sortBy });
        }

        public async Task<ApiResponse<GetListResponse>?> GetList(IGetListRequest getListRequest)
        {
            return await ExecuteCommand<IGetListRequest, GetListResponse>(getListRequest);
        }

    }
    [NamecheapApiCommand(DomainsCommandGroup.GetListCommand)]
    public interface IGetListRequest
    {
        [QueryParam(Optional = true)]
        public string? ListType { get; }
        [QueryParam]
        public string SearchTerm { get; }
        [QueryParam(Optional = true)]
        public int? Page { get; }
        [QueryParam(Optional = true)]
        public int? PageSize { get; }
        [QueryParam(Optional = true)]
        public string? SortBy { get; }
    }

    public class GetListRequest : IGetListRequest
    {
        public string? ListType { get; init; }
        public string SearchTerm { get; init; }
        public int? Page { get; init; }
        public int? PageSize { get; init; }
        public string? SortBy { get; init; }
        public GetListRequest(string searchTerm)
        {
            SearchTerm = searchTerm;
        }

    }

    public class GetListResponse : CommandResponse
    {
        public override string Type => DomainsCommandGroup.GetListCommand;

        [XmlElement]
        public DomainGetListResult? DomainGetListResult { get; set; }
        [XmlElement]
        public Paging? Paging { get; set; }
    }
    public class DomainGetListResult
    {
        [XmlElement(ElementName = "Domain")]
        public Domain[]? Domains { get; set; }
    }
    public class Paging
    {
        [XmlElement]
        public string? TotalItems { get; set; }
        [XmlElement]
        public string? CurrentPage { get; set; }
        [XmlElement]
        public string? PageSize { get; set; }
    }
    public class Domain
    {
        [XmlAttribute]
        public string? ID { get; set; }
        [XmlAttribute]
        public string? Name { get; set; }
        [XmlAttribute]
        public string? User { get; set; }
        [XmlAttribute]
        public string? Created { get; set; }
        [XmlAttribute]
        public string? Expires { get; set; }
        [XmlAttribute]
        public string? IsExpired { get; set; }
    }
}
