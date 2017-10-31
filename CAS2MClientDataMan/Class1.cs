using CAS2MClientDataMan.Domain;
using CAS2MClientDataMan.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan
{
    public class CAS2MDataman
    {

        #region Properties

        string serviceUrlToSendData;
        int SentItemCount;
        int TotalItemCount;
        int recordsPerPage;
        int pageSize;
        int page;


        #endregion


        #region Helper Methods




        /// <summary>
        /// 
        /// </summary>
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



        #endregion
        private void RegisterTotalCount(int totalCount, Guid token, Uri callbackUrl)
        {
            var url = callbackUrl.ToString() + "/registerTotalCount?taskToken=" + token + "&count=" + totalCount;
            var response = new HttpClient().GetAsync(url).Result;
        }

        private void FetchAndSendF10Records(DateTime fromDate, DateTime toDate, Guid taskToken, long taskId, Uri callbackUrl)
        {
            #region دریافت دیتا  

            var f10Records = new List<F10Record>();

            #endregion

            #region ارسال تعدا کل رکوردها

            RegisterTotalCount(f10Records.Count, taskToken, callbackUrl);

            #endregion

            #region شروع ارسال رکوردها

            serviceUrlToSendData = GetServiceUrlToSendData(EntityType.F10, taskToken, callbackUrl);

            SentItemCount = 0;
            TotalItemCount = f10Records.Count();
            pageSize = (int)Math.Ceiling((double)TotalItemCount / recordsPerPage);
            page = 1;

            var stDate = DateTime.Now;

            while (SentItemCount < TotalItemCount)
            {
                page = page > pageSize || page < 1 ? 1 : page;
                var skiped = (page - 1) * recordsPerPage;
                var f10RecordsTaken = f10Records.Skip(skiped).Take(recordsPerPage);

                //رکوردها دسته دسته ارسال میشوند
                using (var http = new HttpClient())
                {
                    
                    var result = http.PostAsJsonAsync(serviceUrlToSendData, f10RecordsTaken).Result;
                }

                SentItemCount += f10RecordsTaken.Count();

                page += 1;
            }


            var fnDate = DateTime.Now;

            var res = (fnDate - stDate).TotalSeconds;

            #endregion

            #region اعلام پایان ارسال دیتا

            FinishTransfer(taskToken, callbackUrl);


            #endregion


        }
        private void FinishTransfer(Guid token, Uri callbackUrl)
        {

            var url = callbackUrl.ToString() + "/FinishTransfer?taskToken=" + token;
            var response = new HttpClient().GetAsync(url).Result;
        }
        private void FetchAndSendF30Records(DateTime fromDate, DateTime toDate, Guid taskToken, long taskId, Uri callbackUrl)
        {
            #region دریافت دیتا  

            var f30Records = new List<F30Record>();

            #endregion

            #region ارسال تعدا کل رکوردها

            RegisterTotalCount(f30Records.Count, taskToken, callbackUrl);

            #endregion

            #region شروع ارسال رکوردها

            serviceUrlToSendData = GetServiceUrlToSendData(EntityType.F30, taskToken, callbackUrl);

            SentItemCount = 0;
            TotalItemCount = f30Records.Count();
            pageSize = (int)Math.Ceiling((double)TotalItemCount / recordsPerPage);
            page = 1;

            var stDate = DateTime.Now;

            while (SentItemCount < TotalItemCount)
            {
                page = page > pageSize || page < 1 ? 1 : page;
                var skiped = (page - 1) * recordsPerPage;
                var f30RecordsTaken = f30Records.Skip(skiped).Take(recordsPerPage);

                //رکوردها دسته دسته ارسال میشوند
                using (var http = new HttpClient())
                {
                    var result = http.PostAsJsonAsync(serviceUrlToSendData, f30RecordsTaken).Result;
                    //  if (result.IsSuccessStatusCode)

                }

                SentItemCount += f30RecordsTaken.Count();

                page += 1;
            }


            var fnDate = DateTime.Now;

            var res = (fnDate - stDate).TotalSeconds;

            #endregion

            #region اعلام پایان ارسال دیتا

            FinishTransfer(taskToken, callbackUrl);


            #endregion

        }






        /// <summary>
        /// 
        /// </summary>
        private void FetchAndSendF50Records(DateTime fromDate, DateTime toDate, Guid taskToken, long taskId, Uri callbackUrl)
        {
            #region دریافت دیتا  

            var f50Records = new List<F50Record>();

            #endregion

            #region ارسال تعدا کل رکوردها

            RegisterTotalCount(f50Records.Count, taskToken, callbackUrl);

            #endregion

            #region شروع ارسال رکوردها

            serviceUrlToSendData = GetServiceUrlToSendData(EntityType.F50, taskToken, callbackUrl);

            SentItemCount = 0;
            TotalItemCount = f50Records.Count();
            pageSize = (int)Math.Ceiling((double)TotalItemCount / recordsPerPage);
            page = 1;

            var stDate = DateTime.Now;

            while (SentItemCount < TotalItemCount)
            {
                page = page > pageSize || page < 1 ? 1 : page;
                var skiped = (page - 1) * recordsPerPage;
                var f50RecordsTaken = f50Records.Skip(skiped).Take(recordsPerPage);

                //رکوردها دسته دسته ارسال میشوند
                using (var http = new HttpClient())
                {
                    var result = http.PostAsJsonAsync(serviceUrlToSendData, f50RecordsTaken).Result;
                    //  if (result.IsSuccessStatusCode)

                }

                SentItemCount += f50RecordsTaken.Count();

                page += 1;
            }


            var fnDate = DateTime.Now;

            var res = (fnDate - stDate).TotalSeconds;

            #endregion

            #region اعلام پایان ارسال دیتا

            FinishTransfer(taskToken, callbackUrl);


            #endregion

        }

        public void FetchAndSendData(DateTime fromDate, DateTime toDate, EntityType entityType, Guid taskToken, long taskId, Uri callbackUrl)
        {
            switch (entityType)
            {
                case EntityType.F10:
                    FetchAndSendF10Records(fromDate, toDate, taskToken, taskId, callbackUrl);
                    break;
                case EntityType.F30:
                    FetchAndSendF30Records(fromDate, toDate, taskToken, taskId, callbackUrl);
                    break;
                case EntityType.F50:
                    FetchAndSendF50Records(fromDate, toDate, taskToken, taskId, callbackUrl);
                    break;
                case EntityType.F60:
                    break;
                case EntityType.F70:
                    break;
            }


        }
    }
}
