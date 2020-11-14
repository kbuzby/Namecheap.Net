using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Namecheap.Net.Tests")]

namespace Namecheap.Net
{
    internal static class ApiRequestBuilder
    {
        public static Uri BuildRequest<TCommand>(NamecheapApi api, TCommand command)
        {
            if (api == null) { throw new ArgumentNullException(nameof(api)); }
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            Dictionary<string, string> queryParams = GetQueryParams(api, command);

            return new Uri(QueryHelpers.AddQueryString(api.ApiEndPoint, queryParams));
        }

        private static Dictionary<string, string> GetQueryParams<TCommand>(NamecheapApi api, TCommand command)
        {
            if (command?.GetType().GetCustomAttributes(typeof(NamecheapApiCommandAttribute), true).FirstOrDefault() is NamecheapApiCommandAttribute commandAttr)
            {
                // get the every command params
                Dictionary<string, string> queryParams = new()
                {
                    { "ApiKey", api.ApiKey },
                    { "UserName", api.UserName },
                    { "ApiUser", api.ApiUserName },
                    { "ClientIp", api.ClientIpAddress.ToString() },
                    { "Command", commandAttr.Command }
                };

                // get the params from the command itself
                IEnumerable<PropertyInfo> commandQueryParamProps = command.GetType().GetProperties()
                    .Where(prop => Attribute.IsDefined(prop, typeof(QueryParamAttribute)));
                foreach (var prop in commandQueryParamProps)
                {
                    string? queryKey = (prop?.GetCustomAttributes(typeof(QueryParamAttribute), true).First() as QueryParamAttribute)?.Key;
                    if (queryKey == null) { continue; }

                    queryParams.Add(queryKey, prop?.GetValue(command)?.ToString() ?? "");
                }
                
                return queryParams;
            }

            throw new ArgumentException($"{nameof(command)} must be an object with NamecheapApiCommandAttribute");
        }

    }
}
