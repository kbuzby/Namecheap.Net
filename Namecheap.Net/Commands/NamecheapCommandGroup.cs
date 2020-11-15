using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Namecheap.Net
{
    public abstract class NamecheapCommandGroup
    {
        protected NamecheapApi Api { get; }
        public NamecheapCommandGroup(NamecheapApi api)
        {
            Api = api;
        }
        internal async Task<ApiResponse<TResponse>?> ExecuteCommand<TCommand,TResponse>(TCommand command) where TResponse : CommandResponse
        {
            Uri requestUri = ApiRequestBuilder.BuildRequest(Api, command);

            XmlSerializer xmlSerializer = new(typeof(ApiResponse<TResponse>));

            using Stream responseStream = await Api.HttpClient.GetStreamAsync(requestUri);

            return xmlSerializer.Deserialize(responseStream) as ApiResponse<TResponse>;
        }
    }

    [XmlRoot(ElementName = "ApiResponse",Namespace = "http://api.namecheap.com/xml.response")]
    public abstract class ApiResponse<TContent> where TContent : CommandResponse
    {
        [XmlAttribute]
        public string? Status { get; private set; }

        [XmlElement]
        public abstract string RequestedCommand { get; protected set; }

        [XmlElement]
        public abstract TContent CommandResponse { get; protected set; }

        [XmlElement]
        public string? Server { get; private set; }

        [XmlElement]
        public string? GMTTimeDifference { get; private set; }

        [XmlElement]
        public string? ExecutionTime { get; private set; }
    }

    public abstract class CommandResponse
    {
        [XmlAttribute]
        public abstract string Type { get; }
    }

}
