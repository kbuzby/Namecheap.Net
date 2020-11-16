using Namecheap.Net.Commands.Domains;
using System;
using System.Net;
using System.Net.Http;

namespace Namecheap.Net
{
    public class NamecheapApi
    {
        //TODO allow injection
        internal HttpClient HttpClient { get; } = new HttpClient();

        private readonly bool _useSandbox;
        public string ApiEndPoint => $"https://api.{(_useSandbox ? "sandbox." : "")}namecheap.com/xml.response";
        public string ApiKey { get; }
        public string UserName { get; }
        public string ApiUserName { get; }
        public IPAddress ClientIpAddress { get; }

        private readonly Lazy<DomainsCommandGroup> _domains;
        public DomainsCommandGroup Domains => _domains.Value;

        public NamecheapApi(string apiKey, string userName, IPAddress clientIpAddress, string? apiUserName = null, bool useSandbox = false)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            ApiUserName = apiUserName ?? userName;
            ClientIpAddress = clientIpAddress ?? throw new ArgumentNullException(nameof(clientIpAddress));
            _useSandbox = useSandbox;

            _domains = new Lazy<DomainsCommandGroup>(() => new DomainsCommandGroup(this));
        }

    }
}
