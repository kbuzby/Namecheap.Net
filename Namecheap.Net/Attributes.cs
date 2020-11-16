using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Namecheap.Net.Tests.Integration")]

namespace Namecheap.Net
{
    [AttributeUsage(AttributeTargets.Interface)]
    internal class NamecheapApiCommandAttribute : Attribute
    {
        public string Command { get; }
        public NamecheapApiCommandAttribute(string command)
        {
            Command = command;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal class QueryParamAttribute : Attribute
    {
        public string Key { get; }
        public bool Optional { get; init; } = false;
        public QueryParamAttribute([CallerMemberName] string key = null)
        {
            if (key.Contains(' ')) { throw new ArgumentException("Query param key must not contain whitespace"); }
            Key = key;
        }
    }
}
