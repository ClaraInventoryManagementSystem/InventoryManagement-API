using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Common
{
    internal class WebRequestContext : IRequestContext
    {
        public string GetHeaderValue(string headerKey, HttpRequest Request)
        {
            return Request.Headers.ContainsKey(headerKey) ?
                Request.Headers[headerKey].ToString() : string.Empty;
        }

       

        public bool ContainsHeader(string headerKey, HttpRequest Request)
        {
            return Request.Headers.ContainsKey(headerKey);
        }
    }

    public interface IRequestContext
    {
        string GetHeaderValue(string header, HttpRequest Request);        
        bool ContainsHeader(string headerKey, HttpRequest Request);
    }


    public interface IResponseContext
    {
        void SetProperties(IDictionary<string, object> properties, string headerKey, string headerValue);
    }

    internal class WebResponseContext : IResponseContext
    {
        public void SetProperties(IDictionary<string, object> properties, string headerKey, string headerValue)
        {
            properties.Add("Header." + headerKey, headerValue);
        }
    }
}
