

using CAS2MClient.Controllers;
using CAS2MClient.Helper;
using CAS2MClient.Models;
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
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;


namespace CAS2MClient.Controllers
{
    // [Route("api/task")]
    public class TaskController : BaseController
    {
        #region Properties



        #endregion

        #region Ctor


        public TaskController()
        {
 
        }

        #endregion

        #region Public Methods






        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("Its working!");
        }





        /// <summary>
        /// 
        /// </summary>
        [HttpGet]

        [ActionName("Trigger")]
        public IHttpActionResult Trigger(DateTime fromDate, DateTime toDate, int? entityType, string taskToken, string taskId, string callbackUrl)
        {

            EventManager.Inst.WriteInfo(string.Format("Trigger has been Called formcode{0} ,taskId={1},taskToken={2},datefrom={3},dateto={4}", entityType, taskId, taskToken,fromDate,toDate), 0);
            //if (fromDate == null)
            //  return BadRequest("Invalid fromDate");

            //if (toDate == null)
            //  return BadRequest("Invalid toDate");

            if (entityType == null)
                return BadRequest("Invalid entityType");
            Guid f = Guid.Empty;
            if (!Guid.TryParse(taskToken, out f))
                return BadRequest("Invalid taskToken");
            Uri callbacku;
            if (!Uri.TryCreate(callbackUrl, UriKind.RelativeOrAbsolute, out callbacku))
            {
                return BadRequest("Invalid callbackUrl");
            }

            new Thread(() =>
            {
                try
                {
                    HostingEnvironment.QueueBackgroundWorkItem(ct=>new DataSender().
                    FetchAndSendData(fromDate, toDate, (EntityType)entityType, f, taskId, new Uri(callbackUrl)));
                }
                catch (Exception ex)
                {

                    new DataSender(). ReportError(token: f, callbackUrl: new Uri(callbackUrl), title: "خطا در هنگام واکشی و ارسال اطلاعات در منبع", description: "این خطا در سرویس منبع رخ داده و باعث شده عملیات دریافت و ثبت رکوردها با شکست روبه رو شود. برای شناسایی و رفع مشکل مربوطه می بایست تا لاگ های سیستم مذکور توسط توسعه دهنده آن بررسی شود. ", systemMsg: EventManager.Inst.getFullMessage(ex));
                }
            }).Start();

            return Ok("Task Started");
        }




        [ActionName("sampledata")]
        [HttpGet]
        public IHttpActionResult sampledata(string taskId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {

            try
            {

                if (fromDate == null)
                    return BadRequest(" دارای مقدار نمیباشدfromDate  تاریخ ");

                if (toDate == null)
                    return BadRequest(" دارای مقدار toDate  تاریخ ");
                var list = new List<F10Record>();
                foreach (var item in new DataLoader().FData<F10Record, TVNBranchDataView>(10, taskId, 1000, fromDate.Value, toDate.Value, (p => p.BranchCreatDate >= fromDate && p.BranchCreatDate < toDate)))
                {
                    foreach (var x in item)
                    {
                        list.Add(x);

                    }
                }

                return Ok(list);
                //string json = JsonConvert.SerializeObject(
                //        f10Records,
                //        new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
                //return json;

                //  return Ok(f10Records);
            }
            catch (Exception ex)
            {

                new DataSender().ReportError(token: Guid.NewGuid(), callbackUrl: null, title: "خطا در هنگام واکشی و ارسال اطلاعات در منبع", description: "این خطا در سرویس منبع رخ داده و باعث شده عملیات دریافت و ثبت رکوردها با شکست روبه رو شود. برای شناسایی و رفع مشکل مربوطه می بایست تا لاگ های سیستم مذکور توسط توسعه دهنده آن بررسی شود. ", systemMsg: ex.Message);
                return BadRequest(ex.Message);
            }

            //   return string.Empty;

        }

        #endregion


        #region Private Methods





        /// <summary>
        /// 
        /// </summary>
   
         public void PostHttp1(string url, object item)
        {
            using (var client = new ExtendedWebClient())
            {
                var srtr = JsonConvert.SerializeObject(item);

                var response =  client.UploadString(url, srtr);

                    var m = (PublicJsonResult)JsonConvert.DeserializeObject<PublicJsonResult>(response);

                    if (!m.result)
                    {
                        throw new Exception(m.message);
                    }
            }
        }

        #endregion

    }
}
