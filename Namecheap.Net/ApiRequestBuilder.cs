using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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
                    IEnumerable<PropertyInfo> commandQueryParamProps = command.GetType().GetInterfaces()
                        .SelectMany(iface => iface.GetProperties())
                        .Where(prop => Attribute.IsDefined(prop, typeof(QueryParamAttribute)));
                    foreach (var prop in commandQueryParamProps)
                    {
                        Type? subIface = null;
                        if (prop.PropertyType != typeof(string)) // make sure to exclude strings because they are enumerables of chars
                        {
                            // If we have an enumerable object then we want to add its properties with a incrementing number suffix for each instance
                            subIface = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                            ? prop.PropertyType
                            : prop.PropertyType.GetInterfaces().FirstOrDefault(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                        }

                        if (subIface is not null)
                        {
                            // FUTURE - support enumerable of value types?

                            if (prop.GetValue(command) is IEnumerable<object> enumerableObject)
                            {
                                var enumerableType = subIface.GetGenericArguments()[0];
                                IEnumerable<PropertyInfo> objectQueryProps = enumerableType.GetProperties()
                                    .Concat(enumerableType.GetInterfaces().SelectMany(iface => iface.GetProperties()))
                                    .Where(prop => Attribute.IsDefined(prop, typeof(QueryParamAttribute)));

                                var i = 1;
                                foreach (var obj in enumerableObject)
                                {
                                    foreach (var subProp in objectQueryProps)
                                    {
                                        TryAddQueryParam(queryParams, subProp, obj, i);
                                    }
                                    i++;
                                }
                            }
                        }
                        else
                        {
                            TryAddQueryParam(queryParams, prop, command);
                        }
                    }

                    return queryParams;
                }
            }

            throw new ArgumentException($"{nameof(command)} must be an object with NamecheapApiCommandAttribute");
        }

        private static void TryAddQueryParam<T>(Dictionary<string, string> queryParams, PropertyInfo prop, T queryObject, int? increment = null)
        {
            QueryParamAttribute? queryAttr = (prop?.GetCustomAttributes(typeof(QueryParamAttribute), true).First() as QueryParamAttribute);
            if (queryAttr == null) { return; }

            string queryKey = queryAttr.Key + increment?.ToString() ?? ""; // append increment if necessary
            object? queryValue = prop?.GetValue(queryObject);

            // if object is empty / null and it was listed as optional then don't include it in the queryParams
            if ((queryValue == null || (queryValue is string queryValueStr && string.IsNullOrEmpty(queryValueStr)))
                && queryAttr.Optional) { return; }

            queryParams.Add(queryKey, queryValue?.ToString() ?? "");
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
