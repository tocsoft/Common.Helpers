using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Common.Helpers
{


    public enum HttpVerbs
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public class RestClient
    {
        public RestClient(Uri url, ICredentials credentials)
        {
            Url = url;
            Credentials = credentials;
        }
        public RestClient(Uri url)
            : this(url, null)
        {
        }
        public RestClient(string url)
            : this(url, null)
        {
        }
        public RestClient(string url, ICredentials credentials)
            : this(new Uri(url), credentials)
        {
        }

        public Uri Url { get; set; }
        public ICredentials Credentials { get; set; }


        public RestClient SubClient(string path)
        {
            return new RestClient(new Uri(Url,path), Credentials);
        }

        public string DoRequest(string query, HttpVerbs method)
        {
            return DoRequest(query, method, "", null);
        }
        public string DoRequest(string query, HttpVerbs method, string data, string contenttype)
        {
            Action<Stream> a = null;
            if (!string.IsNullOrEmpty(data))
                a = (s => (new BinaryWriter(s)).Write(Encoding.UTF8.GetBytes(data)));


            return DoRequest(query, method, a, contenttype);

        }
        public string DoRequest(string query, HttpVerbs method, Action<Stream> data, string contenttype)
        {

            Stream stream = DoDataRequest(query, method, data, contenttype);
            System.IO.StreamReader sr = new StreamReader(stream, Encoding.UTF8);

            return sr.ReadToEnd();
        }

        public Stream DoDataRequest(string query, HttpVerbs method)
        {
            Action<Stream> a = null;
            return DoDataRequest(query, method, a, null);
        }
        public Stream DoDataRequest(string query, HttpVerbs method, Action<Stream> data, string contenttype)
        {
            HttpWebRequest req = WebRequest.Create(new Uri(Url,query)) as HttpWebRequest;

            if (Credentials != null)
                req.Credentials = Credentials;

            req.Method = method.ToString();

            //req.Timeout = System.Threading.Timeout.Infinite;
            if (!string.IsNullOrEmpty(contenttype))
                req.ContentType = contenttype;

            if (data != null)
            {
                using (Stream ps = req.GetRequestStream())
                {
                    data.Invoke(ps);
                }
            }
            try
            {
                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

                if (resp.StatusCode == HttpStatusCode.OK || resp.StatusCode == HttpStatusCode.Created)
                    return resp.GetResponseStream();

                throw new RestException(resp.StatusCode, resp.StatusDescription);
            }
            catch (WebException ex)
            {
                HttpWebResponse resp = ex.Response as HttpWebResponse;
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string contents = sr.ReadToEnd();
                throw new RestException(resp.StatusCode, resp.StatusDescription);
            }


        }

    }
    public class RestException : Exception
    {
        public HttpStatusCode Status { get; private set; }
        public string StatusDescription { get; private set; }

        public RestException(HttpStatusCode status, Exception innerException)
            : base(string.Format("Server returned {0}", status), innerException)
        {
            Status = status;
        }
        public RestException(HttpStatusCode status)
            : this(status, (Exception)null)
        {
        }

        public RestException(HttpStatusCode status, string statusDescription, Exception innerException)
            : base(string.Format("Server returned {0} - \"{1}\"", status, statusDescription), innerException)
        {
            Status = status;
            StatusDescription = statusDescription;
        }
        public RestException(HttpStatusCode status, string statusDescription)
            : this(status, statusDescription, (Exception)null)
        {
        }
    }
}
