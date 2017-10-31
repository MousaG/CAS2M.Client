using CAS2MClient.Models;
using CAS2MClientDataMan.Domain;
using CAS2MClientDataMan.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace CAS2MClient.Helper
{
    public class DataUploader<T>
    {
        private ConcurrentQueue<DataPackage<T>> formque = new ConcurrentQueue<DataPackage<T>>();
        public DataUploader()
        {
            finished = false;
            System.Threading.Thread th = new System.Threading.Thread(ListenQue);
            th.Start();
        }
        public bool forceClose { get; set; }
        public bool finished { get; set; }
        public bool progressing
        {
            get
            {
                return !formque.IsEmpty;
            }
        }
        private void ListenQue()
        {

            while (1 == 1)
            {
                DataPackage<T> mrs;
                while (formque.TryDequeue(out mrs))
                {

                    if (forceClose)
                        return;

                    SendData(mrs);//.taskToken,mrs.c,(List<F50Record>)Convert.ChangeType(mrs.data, typeof(List<F50Record>)));


                }
                if (finished && formque.IsEmpty)
                    return;
            }


        }



        public void UploadData(DataPackage<T> mrs)
        {
            formque.Enqueue(mrs);

        }
        private string GetServiceUrlToSendData(EntityType entityType, Guid Token, Uri callbackUrl)
        {
            var url = callbackUrl.ToString();
            switch (entityType)
            {
                case EntityType.F10:
                    url += "/F10RecordAddRange";
                    break;
                case EntityType.F30:
                    url += "/F30RecordAddRange";
                    break;
                case EntityType.F50:
                    url += "/F50RecordAddRange";
                    break;
                case EntityType.F60:
                    url += "/F60RecordAddRange";
                    break;
                case EntityType.F70:
                    url += "/F70RecordAddRange";
                    break;

            }
            return url + "?Token=" + Token;
        }

        public void SendData(DataPackage<T> mrs)
        {
            try
            {
                var url = GetServiceUrlToSendData(mrs.entityType, mrs.taskToken, mrs.callbackUrl);
                //    WebConnection.remoteAddr = url;
                //   var response = WebConnection.UploadData(url, mrs.data);
               // serviceUrlToSendData = GetServiceUrlToSendData(EntityType.F30, taskToken, callbackUrl);


                using (var http = new HttpClient())
                {
                    var result = http.PostAsJsonAsync(url, mrs.data).Result;
                    //  if (result.IsSuccessStatusCode)
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


    }
}