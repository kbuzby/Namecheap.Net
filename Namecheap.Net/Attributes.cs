using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Namecheap.Net
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class NamecheapApiCommandAttribute: Attribute
    {
        public string Command { get; }
        public NamecheapApiCommandAttribute(string command) 
        {
            Command = command;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal class QueryParamAttribute: Attribute
    {
        public string Key { get; }
        public bool Optional { get; init; } = false;
        public QueryParamAttribute([CallerMemberName] string key  = null)
        {
            if (key.Contains(' ')) { throw new ArgumentException("Query param key must not contain whitespace"); }
            Key = key;
        }
    }
}
