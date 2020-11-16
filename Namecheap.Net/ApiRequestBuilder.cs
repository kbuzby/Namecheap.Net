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
            if (command != null)
            {
                var commandInfo = FindApiCommandInterface(command.GetType());
                if (commandInfo != null)
                {
                    (Type iface, NamecheapApiCommandAttribute commandAttr) = commandInfo.Value;

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
                    IEnumerable<PropertyInfo> commandQueryParamProps = iface.GetProperties()
                        .Where(prop => Attribute.IsDefined(prop, typeof(QueryParamAttribute)));
                    foreach (var prop in commandQueryParamProps)
                    {
                        QueryParamAttribute? queryAttr = (prop?.GetCustomAttributes(typeof(QueryParamAttribute), true).First() as QueryParamAttribute);
                        if (queryAttr == null) { continue; }

                        string queryKey = queryAttr.Key;
                        object? queryValue = prop?.GetValue(command);

                        // if object is empty / null and it was listed as optional then don't include it in the queryParams
                        if ((queryValue == null || (queryValue is string queryValueStr && string.IsNullOrEmpty(queryValueStr)))
                            && queryAttr.Optional) { continue; }
                        queryParams.Add(queryKey, queryValue?.ToString() ?? "");
                    }

                    return queryParams;
                }
            }

            throw new ArgumentException($"{nameof(command)} must be an object with NamecheapApiCommandAttribute");
        }

        internal static (Type iface, NamecheapApiCommandAttribute commandAttr)? FindApiCommandInterface(Type requestType)
        {
            foreach (var iface in requestType.GetInterfaces())
            {
                var attr = iface.GetCustomAttribute<NamecheapApiCommandAttribute>();
                if (attr != null)
                {
                    return (iface, attr);
                }
            }
            return null;
        }
    }
}
