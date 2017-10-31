using GolestanData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.DataMan
{
    public class TrfHolder
    {
        private System.Collections.Hashtable trfList;
        private static TrfHolder inst;
        public static TrfHolder GetInst()
        {
            if (inst == null)
                inst = new TrfHolder();
            return inst;
        }
        public LawItemDataTVN GetTrfLawItems(int coCode, int acTrfCode)
        {


            if (!trfList.Contains(acTrfCode))
            {
                using (var ctx = new GolestanData.AndisheDBEntities())
                {

                    LawItemDataTVN result = new LawItemDataTVN();

                    int callawId = ctx.CalcLawTbls.OrderBy(p => p.EndDate).ToList().LastOrDefault().CalLawId;

                    var ds = ctx.CalcTariffTbls.Where(p => p.CalLawId == callawId && p.TrfCode == acTrfCode).FirstOrDefault();
                    result.TrfCode = acTrfCode;
                    result.isCng = false;
                    result.isDisable = false;
                    if (ds != null)
                    {
                        if (ds.LawItem == null)
                            ds.LawItem = 1;
                        result.LawItem = ds.LawItem.Value;
                        if (ds.SubLawItem == null)
                            ds.SubLawItem = 0;
                        result.SubLawITem = ds.SubLawItem.Value;
                        result.isCng = false;
                        result.CallawID = callawId;
                        /////*
                        result.isDisable = ds.IsDisable;
                        result.DiscTrfCode = getTvnDiscountCode(coCode, acTrfCode);
                        if (ds.TrfHCode == 2)
                        {
                            if (ds.OffMinDemand)
                                result.isCng = ds.OffMinDemand;
                        }
                    }
                    else
                    {
                        result.LawItem = 1;
                        result.SubLawITem = 0;
                        result.isCng = false;
                        result.CallawID = callawId;
                    }
                    trfList.Add(acTrfCode, result);
                }
            }

            return (LawItemDataTVN)trfList[acTrfCode];
        }
        private int getTvnDiscountCode(int coCode, int trfCode)
        {
            if (coCode == 43) //Alborz
            {
                switch (trfCode)
                {
                    case 1011: return 1;
                    case 1012: return 2;
                    case 1013: return 3;
                    case 1014: return 5;
                    case 2043:
                    case 2092:
                    case 2039:
                    case 2034:
                    case 2031:
                        return 7;
                    case 2330: return 6;
                    default:
                        break;
                }


            }

            return 0;
        }
        private TrfHolder()
        {
            trfList = new System.Collections.Hashtable();
        }
    }
    public class LawItemDataTVN
    {
        public int CallawID { get; set; }
        public int TrfCode { get; set; }
        public int LawItem { get; set; }
        public int SubLawITem { get; set; }
        public bool isCng { get; set; }
        public bool isDisable { get; set; }
        public int DiscTrfCode { get; set; }

    }
    public class TempratureType
    {
        private static TempratureType inst;

        public static TempratureType GetInst()
        {
            if (inst == null)
                inst = new TempratureType();
            return inst;
        }

        private TempratureType()
        {
            using (var ctx = new GolestanData.AndisheDBEntities())
            {


                var ds = ctx.CalcLawTbls.ToList();
                foreach (var r in ds)
                {
                    if (!callawList.Contains(r.StartDate.Year))
                        callawList.Add(r.StartDate.Year, new List<CalcLawTbl>());
                    else
                    {
                    }
                    var ls = (List<CalcLawTbl>)callawList[r.StartDate.Year];
                    ls.Add(new CalcLawTbl() { Callawid = r.CalLawId, StartDate = r.EndDate });
                }
            }
        }
        public class CalcLawTbl
        {
            public int Callawid { get; set; }
            public DateTime StartDate { get; set; }
        }
        private Hashtable callawList = new Hashtable();
        public int getTempratureType(DateTime rdgDate, int trfhcode, int trftype, short rgnCode, short cityCode)
        {


            if (!callawList.Contains(rdgDate.Year))
                return 0;
            var ls = (List<CalcLawTbl>)callawList[rdgDate.Year];
            int callawId = 0;
            if (ls.Count == 1)
                callawId = ls[0].Callawid;
            else
            {
                foreach (var item in ls)
                {
                    if (rdgDate < item.StartDate)
                        callawId = item.Callawid;
                    else
                    {
                    }
                }
            }

            if (!callawList.Contains(callawId))
                callawList.Add(callawId, new Hashtable());

            Hashtable hlTrf = (Hashtable)callawList[callawId];
            if (!hlTrf.Contains(trfhcode))
                hlTrf.Add(trfhcode, new Hashtable());

            var rtrftype = (Hashtable)hlTrf[trfhcode];
            if (!rtrftype.Contains(rtrftype))
                rtrftype.Add(rtrftype, new Hashtable());

            var rrgn = (Hashtable)rtrftype[rtrftype];
            if (!rrgn.Contains(rgnCode))
                rrgn.Add(rgnCode, new Hashtable());

            var rcity = (Hashtable)rrgn[rgnCode];

            if (!rcity.Contains(cityCode))
            {
                var ds = getTempratures(callawId, trfhcode, trftype, rgnCode, cityCode);
                List<TempratureCityKey> tList = new List<TempratureCityKey>();
                foreach (var r in ds)
                {
                    tList.Add(new TempratureCityKey() { startdate = r.StartDate.Date, trcoef = r.TrCoef.Value });

                }
                rcity.Add(cityCode, tList);
            }
            List<TempratureCityKey> tListr = (List<TempratureCityKey>)rcity[cityCode];
            foreach (var item in tListr)
            {
                if (item.startdate > rdgDate)
                {
                    if (item.trcoef == 4)
                        return 1;
                    else
                        if (item.trcoef == 3)
                        return 2;
                    else
                            if (item.trcoef == 2)
                        return 3;
                    else
                                if (item.trcoef == 1.3m)
                        return 4;
                    return 0;
                }
            }

            return 0;

        }
        private List<CalcRgnCityByTrfTempView> getTempratures(int callawId, int trfhcode, int trftype, short rgnCode, short cityCode)
        {
            using (var xt = new GolestanData.AndisheDBEntities())
            {

                var ds = xt.CalcRgnCityByTrfTempViews.Where(p => p.CalLawId == callawId && p.RgnCode == rgnCode &&
                p.CityCode == cityCode && p.TrfHCode == trfhcode && p.TrfType == trftype).ToList();
                return ds;
            }
        }
        public class TempratureCityKey
        {
            public decimal trcoef { get; set; }
            public DateTime startdate { get; set; }
        }
    }

}
