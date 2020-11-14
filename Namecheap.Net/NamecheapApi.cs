using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

namespace Namecheap.Net
{
    public class NamecheapApi
    {
        private readonly bool _useSandbox;

        public string ApiEndPoint => $"https://api.{(_useSandbox ? "sandbox." : "")}namecheap.com/xml.response"; 
        public string ApiKey { get; }
        public string UserName { get; }
        public string ApiUserName { get; }
        public IPAddress ClientIpAddress { get; }

        public NamecheapApi(string apiKey, string userName, IPAddress clientIpAddress, string? apiUserName = null, bool useSandbox = false)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            ApiUserName = apiUserName ?? userName;
            ClientIpAddress = clientIpAddress ?? throw new ArgumentNullException(nameof(clientIpAddress));
            _useSandbox = useSandbox;
        }

        public bool ExecuteCommand<TCommand>(TCommand command)
        {

            // command doesn't have a command attribute
            return false;
        }

    }
}
