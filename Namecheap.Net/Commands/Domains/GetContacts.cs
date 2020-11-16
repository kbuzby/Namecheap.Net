using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Namecheap.Net.Commands.Domains
{
    public partial class DomainsCommandGroup
    {
        internal const string GetContactsCommand = "namecheap.domains.getContacts";

        public async Task<ApiResponse<GetContactsResponse>?> GetContacts(string domainName)
        {
            return await GetContacts(new GetContactsRequest(domainName));
        }

        public async Task<ApiResponse<GetContactsResponse>?> GetContacts(IGetContactsRequest request)
        {
            return await ExecuteCommand<IGetContactsRequest, GetContactsResponse>(request);
        }
    }

    [NamecheapApiCommand(DomainsCommandGroup.GetContactsCommand)]
    public interface IGetContactsRequest
    {
        [QueryParam]
        public string DomainName { get; }
    }

    public class GetContactsRequest : IGetContactsRequest
    {
        public string DomainName { get; }
        public GetContactsRequest(string domainName)
        {
            DomainName = domainName;
        }
    }

    public class GetContactsResponse : CommandResponse
    {
        public override string Type => DomainsCommandGroup.GetContactsCommand;
        [XmlElement]
        public DomainContactsResult? DomainContactsResult { get; set; }
    }
    public class DomainContactsResult
    {
        [XmlAttribute]
        public string? Domain { get; set; }
        [XmlAttribute(AttributeName = "domainnameid")]
        public string? DomainNameId { get; set; }
        [XmlElement]
        public Contact? Registrant { get; set; }
        [XmlElement]
        public Contact? Tech { get; set; }
        [XmlElement]
        public Contact? Admin { get; set; }
        [XmlElement]
        public Contact? AuxBilling { get; set; }
        [XmlElement]
        public Attributes? CurrentAttributes { get; set; }
        [XmlElement]
        public DomainContactsResult? WhoisGuardContact { get; set; }
    }
    public class Attributes
    {
        [XmlElement]
        public string? RegistrantNexus { get; set; }
        [XmlElement]
        public string? RegistrantNexusCountry { get; set; }
        [XmlElement]
        public string? RegistrantPurpose { get; set; }
    }

    public class Contact
    {
        [XmlAttribute]
        public bool? ReadOnly { get; set; }
        [XmlElement]
        public string? OrganizationName { get; set; }
        [XmlElement]
        public string? JobTitle { get; set; }
        [XmlElement]
        public string? FirstName { get; set; }
        [XmlElement]
        public string? LastName { get; set; }
        [XmlElement]
        public string? Address1 { get; set; }
        [XmlElement]
        public string? Address2 { get; set; }
        [XmlElement]
        public string? City { get; set; }
        [XmlElement]
        public string? StateProvince { get; set; }
        [XmlElement]
        public string? StateProvinceChoice { get; set; }
        [XmlElement]
        public string? PostalCode { get; set; }
        [XmlElement]
        public string? Country { get; set; }
        [XmlElement]
        public string? Phone { get; set; }
        [XmlElement]
        public string? Fax { get; set; }
        [XmlElement]
        public string? EmailAddress { get; set; }
        [XmlElement]
        public string? PhoneExt { get; set; }
    }
}
