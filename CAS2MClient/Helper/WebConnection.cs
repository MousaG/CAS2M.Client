using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace CAS2MClient.Helper
{
    public class WebConnection
    {
      //  public static string remoteAddr;
        public static string UploadData(string url, object data)
        {
            ExtendedWebClient client = new ExtendedWebClient();
            client.Headers.Add("Content-Type", "application/json; charset=utf-8");
            client.Encoding = System.Text.Encoding.UTF8;
            string json = JsonConvert.SerializeObject(
                      data,
                      new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });


            var result = client.UploadString(url, json);

            if (client.StatusCode.ToString().ToLower() != "0")
            {
                throw new Exception(result);
            }
            return result;


        }
        public void PostAsJsonAsync<T>(string url,T data)
        {

       

            ExtendedWebClient client = new ExtendedWebClient();
            client.Headers.Add("Content-Type", "application/json; charset=utf-8");
            client.Encoding = System.Text.Encoding.UTF8;
            string json = JsonConvert.SerializeObject(
                      data,
                      new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });


          //  var result = client.UploadString(remoteAddr + funcName, json);
            var result = client.UploadString(url, json);
            if (client.StatusCode.ToString().ToLower() != "0")
            {
                throw new Exception(result);
            }
        //    return result;
        }

    }
}