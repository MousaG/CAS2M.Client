
using CAS2MClientDataMan.DataMan;
using CAS2MClientDataMan.Domain;
using CAS2MClientDataMan.Enums;
using GolestanData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace CAS2MClientDataMan.DataMan
{

    public class DataSender
    {
        string serviceUrlToSendData;
        int SentItemCount;
        int TotalItemCount=0;
        int recordsPerPage=0;
        int pageSize=100;
        int page=1;

        public EventHandler OnProgress;
        public EventHandler OnError;
        public DataSender()
        {
            recordsPerPage = 1000;
            page = 1;
        }
        public void GetHttp(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //  HttpResponseMessage response = client.posta("api/useraccount").Result;
                var response = client.GetAsync(serviceUrlToSendData).Result;

                if (response.IsSuccessStatusCode)
                {
                    var t = response.Content.ReadAsAsync<object>().Result;
                    var m = (PublicJsonResult)JsonConvert.DeserializeObject<PublicJsonResult>(t.ToString());

                    if (!m.result)
                    {
                        throw new Exception(m.message);
                    }
                }
                else
                {
                    EventManager.Inst.WriteError("Invalid status Code" + response.StatusCode.ToString(), 1);
                    throw new Exception("failed to call PostHttp");
                    //Something has gone wrong, handle it here
                }
            }
        }


        public async Task<bool> PostHttp(string url, object item)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //  HttpResponseMessage response = client.posta("api/useraccount").Result;
                var response = await client.PostAsJsonAsync(serviceUrlToSendData, item);
                if (response.IsSuccessStatusCode)
                {
                    var t = response.Content.ReadAsAsync<object>().Result;
                    var m = (PublicJsonResult)JsonConvert.DeserializeObject<PublicJsonResult>(t.ToString());

                    if (!m.result)
                    {
                        throw new Exception(m.message);
                    }
                }
                else
                {
                    EventManager.Inst.WriteError("Invalid status Code" + response.StatusCode.ToString(), 1);
                    throw new Exception("failed to call PostHttp");
                    //Something has gone wrong, handle it here
                }
                return true;
            }
        }
        private void RegisterTotalCount(long totalCount, Guid token, Uri callbackUrl)
        {
            var url = callbackUrl.ToString() + "/registerTotalCount?taskToken=" + token + "&count=" + totalCount;
            //GetHttp(url);
            var response = new HttpClient().GetAsync(url).Result;

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
        public TaskData SetTask(Uri callbackUrl, DateTime fromDate, DateTime toDate,EntityType tp)
        {
            var url = callbackUrl.ToString() + "/SetTask?fromDate=" + fromDate + "&toDate=" + toDate + "&entityType="+tp.GetHashCode().ToString();
            //GetHttp(url);
            var response = new HttpClient().GetAsync(url).Result;
         if (response.IsSuccessStatusCode)
            {
                var t = response.Content.ReadAsAsync<object>().Result;
                var m = JsonConvert.DeserializeObject<TaskData>(t.ToString());

                if (!m.result)
                {
                    throw new Exception(m.message);
                }
                return m;
            }
            else
            {
                EventManager.Inst.WriteError("Invalid status Code" + response.StatusCode.ToString(), 1);
                throw new Exception("failed to call PostHttp");
                //Something has gone wrong, handle it here
            }
        }
        private async Task GetFdata<T, TSource>(int formCode, DateTime fromDate, DateTime toDate, Expression<Func<TSource, bool>> predicate, Guid taskToken, string taskId, Uri callbackUrl)
        {
            int totalcount = 0;
            try
            {
                EventManager.Inst.WriteInfo(string.Format("start sending data GetFdata {0}", formCode), 800);

                int blockcount = 1000;
                int progress = 0;
                long ct = new DataCounter().Count<TSource>(formCode, predicate);

                EventManager.Inst.WriteInfo(string.Format("start sending data formcode={0} , count={1}", formCode, ct), 801);
                RegisterTotalCount(ct, taskToken, callbackUrl);

                //  var du = new DataUploader<T>();
                var dl = new DataLoader();
                //   du.finished = false;
                //  du.forceClose = false;
                // bool cancelation = false;
                serviceUrlToSendData = GetServiceUrlToSendData((EntityType)formCode, taskToken, callbackUrl);
                foreach (var item in dl.FData<T, TSource>(formCode, taskId, blockcount, fromDate, toDate, predicate))
                {


                    totalcount += item.Count;
                    progress += item.Count;
                    if(OnProgress!=null)
                    {
                        OnProgress(totalcount, new EventArgs());
                    }
                    //  new DataUploader<T>().SendData(new DataPackage<T>() { entityType = (EntityType)formCode, callbackUrl = callbackUrl, data = item });
                    EventManager.Inst.WriteInfo(string.Format(" sending data formcode={0} , count={1}", formCode, item.Count), 802);
                    bool issuscess = await this.PostHttp(serviceUrlToSendData, item);//.Result;
                    EventManager.Inst.WriteInfo(string.Format("end sending data formcode={0} , count={1}", formCode, item.Count), 803);

                    //using (var http = new HttpClient())
                    //{
                    //    EventManager.Inst.WriteInfo(string.Format(" sending data formcode={0} , count={1}", formCode, item.Count), 802);

                    //    var result = http.PostAsJsonAsync(serviceUrlToSendData, item).Result;

                    //    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    //    {
                    //        ReportError(taskToken, callbackUrl, serviceUrlToSendData, taskToken.ToString(), result.ReasonPhrase);
                    //        return;
                    //        // throw new Exception(serviceUrlToSendData + "\r\n" + taskToken.ToString() + "\r\n" + result.ReasonPhrase);
                    //    }
                    //    EventManager.Inst.WriteInfo(string.Format("end sending data formcode={0} , count={1}", formCode, item.Count), 803);

                    //}

                    SentItemCount += item.Count();

                    //  du.UploadData(pkg);


                }
                // while (du.progressing)
                //    continue;
                FinishTransfer(taskToken, callbackUrl);
                EventManager.Inst.WriteInfo(string.Format("end sending data formcode={0} , count={1}", formCode, ct), formCode);
                //  du.finished = true;

            }
            catch (Exception e)
            {

                EventManager.Inst.WriteError("Error in Sending data" + EventManager.Inst.getFullMessage(e), formCode);

                ReportError(taskToken, callbackUrl, "Error in Sending data", e.Message, EventManager.Inst.getFullMessage(e));
            }
            //   return result110;
        }
        public void ReportError(Guid token, Uri callbackUrl, string title, string description, string systemMsg)
        {
            try
            {

                if(OnError != null)
                {
                    OnError(description + systemMsg,new EventArgs());
                }
                EventManager.Inst.WriteError(description + systemMsg, null, 500);
                var url = callbackUrl.ToString() + "/ReportError?taskToken=" + token + "&title=" + title + "&description=" + description + "&systemMsg=" + systemMsg;

                var response = new HttpClient().GetAsync(url).Result;
            }
            catch (Exception ex)
            {
                EventManager.Inst.WriteError("error in ReportError", ex, 500);
            }
        }



        private async Task MakeForms<T, TSource>(EntityType formCode, string taskId, DateTime fromDate, DateTime toDate, Expression<Func<TSource, bool>> predicate, Guid taskToken, Uri callbackUrl)
        {

            await GetFdata<T, TSource>(formCode.GetHashCode(), fromDate, toDate, predicate, taskToken, taskId, callbackUrl);

        }

        public async Task FetchAndSendData(DateTime fromDate, DateTime toDate, EntityType entityType, Guid taskToken, string taskId, Uri callbackUrl)
        {

            switch (entityType)
            {
                case EntityType.F10:

                    await MakeForms<F10Record, TVNBranchDataView>(entityType, taskId, fromDate, toDate, (p => p.BranchCreatDate >= fromDate && p.BranchCreatDate < toDate), taskToken, callbackUrl);

                    //                    FetchAndSendF10Records(fromDate, toDate, taskToken, taskId, callbackUrl);
                    break;
                case EntityType.F30:
                    await MakeForms<F30Record, TVNNEWForm30View>(entityType, taskId, fromDate, toDate, (p => p.CurrRdgDate >= fromDate && p.CurrRdgDate < toDate), taskToken, callbackUrl);

                    //FetchAndSendF30Records(fromDate, toDate, taskToken, taskId, callbackUrl);
                    break;
                case EntityType.F50:
                    await MakeForms<F50Record, TVNF50FormView>(entityType, taskId, fromDate, toDate, (p => p.RcptDateTime >= fromDate && p.RcptDateTime < toDate), taskToken, callbackUrl);

                    //      FetchAndSendF50Records(fromDate, toDate, taskToken, taskId, callbackUrl);
                    break;
                case EntityType.F60:
                    break;
                case EntityType.F70:
                    break;
            }

            //*/
        }

        private void FinishTransfer(Guid token, Uri callbackUrl)
        {

            var url = callbackUrl.ToString() + "/FinishTransfer?taskToken=" + token;
            var response = new HttpClient().GetAsync(url).Result;
        }



    }
}