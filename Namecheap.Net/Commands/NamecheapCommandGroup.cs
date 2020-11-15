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

            /*
            using StreamReader sr = new(responseStream);

            string responseString = sr.ReadToEnd();
            */

            return xmlSerializer.Deserialize(responseStream) as ApiResponse<TResponse>;
        }
    }

    [XmlRoot(ElementName = "ApiResponse",Namespace = "http://api.namecheap.com/xml.response")]
    public class ApiResponse<TContent> where TContent : CommandResponse
    {
        [XmlAttribute]
        public string? Status { get; set; }

        [XmlElement]
        public string[]? Errors { get; set; }

        [XmlElement]
        public string? RequestedCommand { get; set; }

        [XmlElement]
        public TContent? CommandResponse { get; set; }

        [XmlElement]
        public string? Server { get; set; }

        [XmlElement]
        public string? GMTTimeDifference { get; set; }

        [XmlElement]
        public double ExecutionTime { get; set; }
    }

    public abstract class CommandResponse
    {
        [XmlAttribute]
        public abstract string Type { get; }
    }

}
