using CAS2MClientDataMan.DataMan;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web;

namespace CAS2MClient.Helper
{
    public class ExtendedWebClient : WebClient
    {
        private WebRequest _Request = null;

        protected override WebRequest GetWebRequest(Uri address)
        {
            this._Request = base.GetWebRequest(address);

            if (this._Request is HttpWebRequest)
            {
                ((HttpWebRequest)this._Request).AllowAutoRedirect = false;
            }
            this._Request.Timeout = 60 * 60 * 1000;
            return this._Request;
        }
        //HttpStatusCode statusCode = HttpStatusCode.OK;
        public HttpStatusCode StatusCode { get; private set; }
        //public HttpStatusCode StatusCode()
        //{
        //    HttpStatusCode result;

        //    if (this._Request == null)
        //    {
        //        throw (new InvalidOperationException(@"Unable to retrieve the status 
        //               code, maybe you haven't made a request yet."));
        //    }

        //    HttpWebResponse response = base.GetWebResponse(this._Request)
        //                               as HttpWebResponse;

        //    if (response != null)
        //    {
        //        result = response.StatusCode;
        //    }
        //    else
        //    {
        //        throw (new InvalidOperationException(@"Unable to retrieve the status 
        //               code, maybe you haven't made a request yet."));
        //    }

        //    return result;
        //}

        private byte[] GZipBytes(string data)
        {
            //Transform string into byte[]  
            byte[] ret = null;

            using (System.IO.MemoryStream outputStream = new System.IO.MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(outputStream, System.IO.Compression.CompressionMode.Compress))
                {
                    //write to gzipper
                    StreamWriter writer = new StreamWriter(gzip);
                    writer.Write(data);
                    writer.Flush();

                    //write to output stream
                    gzip.Flush();
                    gzip.Close();

                    ret = outputStream.ToArray();
                }
            }

            return ret;
        }

        /// <summary>
        /// Overriden method using GZip compressed data upload.
        /// </summary>
        /// <param name="address">Remote server address.</param>
        /// <param name="data">String data.</param>
        /// <returns>Server response string.</returns>
        public new string UploadString(string address, string data)
        {
            try
            {
                string ret = null;
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);//  GZipBytes(data);

                this.Headers.Add("content-encoding", "application/json");
                bytes = base.UploadData(address, bytes);
                WebHeaderCollection headers = base.ResponseHeaders;
                ret = System.Text.Encoding.UTF8.GetString(bytes);

                return ret;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ReceiveFailure)
                {
                    var msg = string.Format("remote service not avialable, {0}", address);
                    EventManager.Inst.WriteError(msg, ex, 0);

                    throw new Exception(msg);
                }
                else
                {
                    byte[] resul = new byte[ex.Response.GetResponseStream().Length];
                    var bt = ex.Response.GetResponseStream().Read(resul, 0, resul.Length);
                    var msg = System.Text.Encoding.UTF8.GetString(resul);
                    StatusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    return msg;
                }
            }
        }

        /// <summary>
        /// Overriden method using GZip compressed data upload.
        /// </summary>
        /// <param name="address">Remote server URI.</param>
        /// <param name="data">String data.</param>
        /// <returns>Server response string.</returns>
        public new string UploadString(Uri address, string data)
        {
            string ret = null;
            byte[] bytes = GZipBytes(data);

            this.Headers.Add("content-encoding", "gzip");
            bytes = base.UploadData(address, bytes);
            ret = System.Text.Encoding.UTF8.GetString(bytes);

            return ret;
        }

        /// <summary>
        /// Overriden method using GZip compressed data upload.
        /// </summary>
        /// <param name="address">Remote server address.</param>
        /// <param name="method">HTTP method (e.g. POST, PUT, DELETE, GET).</param>
        /// <param name="data">String data.</param>
        /// <returns>Server response string.</returns>
        public new string UploadString(string address, string method, string data)
        {
            string ret = null;
            byte[] bytes = GZipBytes(data);

            this.Headers.Add("content-encoding", "gzip");
            bytes = base.UploadData(address, method, bytes);
            ret = System.Text.Encoding.UTF8.GetString(bytes);

            return ret;
        }


        /// <summary>
        /// Overriden method using GZip compressed data upload.
        /// </summary>
        /// <param name="address">Remote server URI.</param>
        /// <param name="method">HTTP method (e.g. POST, PUT, DELETE, GET).</param>
        /// <param name="data">String data.</param>
        /// <returns>Server response string.</returns>
        public new string UploadString(Uri address, string method, string data)
        {
            string ret = null;
            byte[] bytes = GZipBytes(data);

            this.Headers.Add("content-encoding", "gzip");
            bytes = base.UploadData(address, method, bytes);
            ret = System.Text.Encoding.UTF8.GetString(bytes);

            return ret;
        }
    }
}