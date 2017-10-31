
using CAS2MClientDataMan.Domain;
using GolestanData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.DataMan
{
    public class DataLoader
    {
        public bool cancelation { get; set; }
        public IEnumerable<List<T>> FData<T, TSource>(int formCode, string taskId, int blockCount, DateTime fromDate, DateTime toDate, Expression<Func<TSource, bool>> predicate)
        {
            //   var rs = F10Data(taskId, 1000, fromDate, toDate, null);
            // return rs;
            cancelation = false;

            MethodInfo method = this.GetType().GetMethod("F" + formCode.ToString() + "Data", BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
                throw new Exception("Method not found >>" + "CountF" + formCode.ToString());

            var r = method.Invoke(this, new object[] { taskId, blockCount, fromDate, toDate, predicate });
            foreach (var item in (IEnumerable<List<T>>)r)
            {
                yield return item;
            }

        }

        private IEnumerable<List<F10Record>> F10Data(string taskid, int blockCount, DateTime fromDate, DateTime toDate, Expression<Func<TVNBranchDataView, bool>> predicate)
        {
            var result = new List<F10Record>();
            //using (
            var tx = new AndisheDBEntities();
            {
                EventManager.Inst.WriteInfo("start calling F10Data", 10);
                tx.Database.CommandTimeout = 3600;
                var brnchs = tx.TVNBranchDataViews.AsNoTracking().Where(predicate).OrderBy(p => p.BranchCode);
                var dataCntr = tx.TvnCounterViews.AsNoTracking().Join(tx.TVNBranchDataViews.Where(predicate), b => b.BranchCode, c => c.BranchCode, (x, y) => x).OrderBy(p => p.BranchCode).ThenBy(p => p.CounterCode).AsQueryable();
                var enumrtrCntr = dataCntr.GetEnumerator();
                enumrtrCntr.MoveNext();

                var cntr = enumrtrCntr.Current;
                foreach (var b in brnchs)
                {
                    if (cancelation)
                        throw new OperationCanceledException();
                    var f10 = new F10Record() { taskId = taskid, Id = b.BillId.Value.ToString("F0") };
                    f10.r110 = new R110Model();
                    f10.r120 = new R120Model();
                    f10.r125 = new List<R125Model>();
                    f10.r125r = new List<R125Model>();
                    try
                    {
                        #region 110

                        f10.r110.r10TOTAL_BILL_DEBT = b.CrDbTot;
                        f10.r110.r20TOTAL_REGISTER_DEBT = b.CrDBBranchSale.Value;
                        f10.r110.r30OTHER_ACCOUNT_BALANCE = b.CrDbOtherCost == null ? 0 : b.CrDbOtherCost.Value;
                        f10.r110.r101CUSTOMER_TYPE = 1;
                        f10.r110.r102FIRST_NAME = b.OwnerName;
                        f10.r110.r103SURNAME = b.OwnerFamily;
                        f10.r110.r104FATHER_NAME = b.FatherName;
                        f10.r110.r105BIRTH_CERTIFICATE_ID = b.ShomareShenasname;
                        f10.r110.r106NATIONAL_CARD_ID = b.OwnerNatCode;
                        f10.r110.r107BIRTH_DATE = DateTime.Now;
                        f10.r110.r108ISSUE_PLACE = b.IssuedFrom;
                        f10.r110.r109COMPANY_NAME = "";
                        f10.r110.r112COMPANY_ISIC_FK = b.ActTypeCode;
                        f10.r110.r114SEX_TYPE = b.Gender;
                        f10.r110.r115PHONE_NUMBEr = b.FixedTel;
                        f10.r110.r116MOBILE_NUMBEr = b.MobileNo;
                        f10.r110.r117FAX_NUMBEr = "0";// b.Fax;
                        f10.r110.r118EMAIL_ADDRESS = b.Email;
                        f10.r110.r119ADDRESS = b.Adress;
                        f10.r110.r120POSTAL_CODE = b.HomePoNum == null ? "0" : b.HomePoNum.Value.ToString("F0");
                        f10.r110.r121BUSINESS_CODE_FK = b.OrgCode;
                        #endregion
                    }
                    catch (Exception e)
                    {
                        EventManager.Inst.WriteError(string.Format(" branchcode {0} ", b.BranchCode), e, 10110);

                        throw e;

                    }
                    try
                    {
                        #region 120
                        if ((b.TrfCode == 0) || (b.TrfCode == -1))
                            continue;
                        var trfc = TavanirHelper.getTrfCodeTVN(b.CoCode, Convert.ToInt32(b.TrfCode), b.PwrCnt, b.SelCode);


                        f10.r120.r200BILL_IDENTIFIER = b.BillId.Value.ToString("F0");
                        f10.r120.r201FILE_SERIAL_NUMBER = b.BranchCode;
                        f10.r120.r202SUBSCRIPTION_ID = b.BranchSrl;
                        f10.r120.r204CITY_CODE_FK = b.CoCode;
                        f10.r120.r205AREA_CODE = TavanirHelper.GetRegion(b.CoCode, b.RgnCode, b.CityCode);
                        f10.r120.r206ZONE_CODE = TavanirHelper.GetRegion(b.CoCode, b.RgnCode, b.CityCode);
                        f10.r120.r207IDENTIFYING_NUMBER = b.IdentityCode;
                        f10.r120.r208READING_COLLECTION_DAY = b.WorkDayCode;
                        f10.r120.r209READING_AGENT_CODE = b.RdrCode;
                        f10.r120.r210READING_SEQUENCE = b.RdgSrl;
                        f10.r120.r211SERVICE_TYPE = b.BranchTypeCode;
                        f10.r120.r212NO_OF_PHASE = b.Phs.Value;
                        f10.r120.r213AMPER = b.Amp;
                        f10.r120.r214AGREEMENT_DEMAND = b.PwrCnt;
                        f10.r120.r215VOLTAGE_TYPE = b.VoltCode == null ? 2 : b.VoltCode.Value;
                        var TvnTrfType = b.TrfHCode.ToString();
                        if (b.TrfHCode == -1)
                            TvnTrfType = "1";
                        TvnTrfType += b.TrfType == null ? "0" : b.TrfType.Value.ToString();
                        f10.r120.r216TARIFF_TYPE = TavanirHelper.getTrfType(TvnTrfType);
                        f10.r120.r217TARIFF_OPTION_CODE = b.SelCode;
                        f10.r120.r219PREMISE_LOCATION = b.GeoAreaCode == null ? 0 : b.GeoAreaCode.Value;
                        f10.r120.r220SERVICE_POINT_ADDRESS = b.Adress;
                        f10.r120.r221SERVICE_POINT_POSTCODE = b.HomePoNum.ToString();
                        f10.r120.r222PHONE_NUMBER = b.FixedTel;
                        if (b.InstallDate != null)
                            f10.r120.r223INSTALLATION_DATE = b.InstallDate.Value;
                        else
                        if (b.UpdInstDate != null)
                            f10.r120.r223INSTALLATION_DATE = b.UpdInstDate.Value;

                        if (b.BranchCreatDate != null)
                            f10.r120.r224CREATION_DATE = b.BranchCreatDate.Value;

                        if (b.BranchCntractDate != null)
                            f10.r120.r225AGREEMENT_DATE = b.BranchCntractDate.Value;
                        f10.r120.r226AGREEMENT_NUMBER = "";

                        f10.r120.r227SERVICE_POINT_STATUS = TavanirHelper.getBranchStatus(b.BranchStatCode.Value);
                        f10.r120.r228LICENSE_ALLOWED_POWER = (double)b.PwrIcn.Value;
                        if (b.PwrIcnExpDate != null)
                            f10.r120.r229LICENSE_EXPIRE_DATE = b.PwrIcnExpDate.Value;
                        // f10.r120.r230LICENSE_NUMBER = b.LetterAndicator;
                        f10.r120.r231LICENSE_ISSUER = "";
                        f10.r120.r232ELECTRICITY_SUPPLY_FK = b.ServiceCode == null ? 1 : b.ServiceCode.Value;
                        f10.r120.r233POPULATION_NUMBER = b.FmlCode;
                        if (b.TrfHCode == 1)
                            if (b.PwrIcnExpDate != null)
                                f10.r120.r234POPULATION_EXPIRE_DATE = b.PwrIcnExpDate.Value;
                        f10.r120.r235DISCOUNT_CONSUMPTION_FK = b.SpecialCode.Value;
                        //   f10.r120.r236DISCOUNT_REGISTRATION_FK = b.IndivisualCode;
                        f10.r120.r237REGISTRATION_DISCOUNT_REF = b.IndivisualCode;
                        f10.r120.r238TARIFF_FK = trfc.TarrifCode;
                        if (b.UpdUninstDate != null)
                            f10.r120.r239SERVICE_INACTIVE_DATE = b.UpdUninstDate.Value;

                        //f10.r120.r240SERVICE_DELETE_DATE
                        if (b.TmpExpireDate != null)
                            f10.r120.r241TEMPORARY_REDUCE_EXPIRE_DATE = b.TmpExpireDate.Value;
                        if (b.TmpFld5 != null)
                            f10.r120.r242LAST_POWER_REDUCTION = b.TmpFld5.Value;
                        //f10.r120.r243TEMPORARY_REDUCE_START_DATE
                        f10.r120.r244TEMPORARY_POWER_REDUCT_COUNT = 0;
                        f10.r120.r245TRACKING_CODE = 0;
                        f10.r120.r246TRANSFORMATOR_COUNT = 0;
                        f10.r120.r247POST_NUMBER = "0";
                        f10.r120.r248FEEDER_NUMBER = "0";
                        f10.r120.r249BASE_NUMBER = "0";
                        f10.r120.r250TRANSFORMATOR_POWER = 0;
                        f10.r120.r251TRANSFORMATOR_NUMBER = 0;
                        //f10.r120.r252LAST_TEST_DATE
                        f10.r120.r254GAS_ENERGY_STATUS = 0;// b.GasStatus;
                        f10.r120.r255REPEAT_CODE = 0;// b.ResaleCode;
                        if (b.LatitueX != null)
                            f10.r120.r256X_DEGREE = b.LatitueX.Value;
                        else
                            f10.r120.r256X_DEGREE = 0;
                        if (b.LongitudeY != null)
                            f10.r120.r257Y_DEGREE = b.LongitudeY.Value;
                        else
                            f10.r120.r257Y_DEGREE = 0;
                        f10.r120.r258Y_MOSQUE_AREA = b.BuiltUpArea;
                        f10.r120.r259TRANSIT_DEMAND = 0;
                        if (b.BranchKindCode != null)
                            f10.r120.r260CONNECTION_TYPE = b.BranchKindCode.Value;
                        else
                            f10.r120.r260CONNECTION_TYPE = 0;
                        f10.r120.r261RURAL_DISTRICT = 0;
                        #endregion
                    }
                    catch (Exception e)
                    {
                        EventManager.Inst.WriteError(string.Format(" branchcode {0} ", b.BranchCode), e, 10120);

                        throw e;
                    }


                    try
                    {
                        if (b.BranchCode < cntr.BranchCode)
                        {
                            EventManager.Inst.WriteWarning(string.Format("Skip Branch branchcode {0}", b.BranchCode), 10);
                            continue;
                        }
                        while (b.BranchCode > cntr.BranchCode)
                        {
                            EventManager.Inst.WriteWarning(string.Format("Skip Counter branchcode {0}", cntr.BranchCode), 10);

                            enumrtrCntr.MoveNext();
                            cntr = enumrtrCntr.Current;
                        }
                        while ((cntr.BranchCode == b.BranchCode) && enumrtrCntr.MoveNext())
                        {
                            //  listHist.Add(hstr);
                            cntr = enumrtrCntr.Current;

                            #region 125
                            if (cntr.CounterTypeCode == 1)
                            {
                                var m125 = new R125Model();
                                m125.r280SERIAL_NUMBER = string.IsNullOrEmpty(cntr.FabrikNumber.Trim()) ? "1" : cntr.FabrikNumber;
                                m125.r281METER_TYPE = cntr.CounterTypeCode;
                                m125.r282DIGIT_NUMBER = cntr.QtyDgt;
                                m125.r283ADJUSTMENT_FACTOR = cntr.CntrCoef;
                                m125.r284METER_MODEL_FK = cntr.CntrKind.Value;
                                m125.r285METER_MAKER_TYPE_FK = cntr.PrdcrCode;
                                if (cntr.InstallDate != null)
                                    m125.r286INSTALLATION_DATE = cntr.InstallDate.Value;
                                else
                                    m125.r286INSTALLATION_DATE = cntr.UpdInstDate;
                                m125.r287READING_CLOCK_CODE = TavanirHelper.getTimerCode(cntr.TimerCode.ToString());
                                m125.r288MAXIMETER_FACTOR = cntr.CntrCoef;
                                m125.r289METER_STATUS = cntr.CntrStatCode.Value;
                                m125.r290CREATION_DATE = cntr.UpdInstDate;
                                if (cntr.CntrUninsDate != null)
                                    m125.r291EXPIRATION_DATE = cntr.CntrUninsDate.Value;
                                if (cntr.ChangeTypeCode != null)
                                    m125.r292CHANGE_REASON = cntr.ChangeTypeCode.Value;
                                else
                                    m125.r292CHANGE_REASON = 0;

                                f10.r125.Add(m125);
                            }
                            #endregion

                            if (cntr.CounterTypeCode == 2)
                            {
                                var m125 = new R125Model();
                                #region 125
                                m125 = new R125Model();
                                m125.r280SERIAL_NUMBER = string.IsNullOrEmpty(cntr.FabrikNumber.Trim()) ? "1" : cntr.FabrikNumber;
                                m125.r281METER_TYPE = cntr.CounterTypeCode;
                                m125.r282DIGIT_NUMBER = cntr.QtyDgt;
                                m125.r283ADJUSTMENT_FACTOR = cntr.CntrCoef;
                                m125.r284METER_MODEL_FK = cntr.CntrKind.Value;
                                m125.r285METER_MAKER_TYPE_FK = cntr.PrdcrCode;
                                if (cntr.InstallDate != null)
                                    m125.r286INSTALLATION_DATE = cntr.InstallDate.Value;
                                m125.r287READING_CLOCK_CODE = TavanirHelper.getTimerCode(cntr.TimerCode.ToString());
                                m125.r288MAXIMETER_FACTOR = cntr.CntrCoef;
                                m125.r290CREATION_DATE = cntr.UpdInstDate;
                                if (cntr.CntrUninsDate != null)
                                    m125.r291EXPIRATION_DATE = cntr.CntrUninsDate.Value;
                                #endregion
                                f10.r125r.Add(m125);
                            }
                        }
                        if (f10.r125.Count == 0)
                        {
                            EventManager.Inst.WriteWarning(string.Format("No Counter branchcode {0}", b.BranchCode), 10);

                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        EventManager.Inst.WriteError(string.Format(" branchcode {0} ", b.BranchCode), e, 10);

                        throw e;

                    }
                    result.Add(f10);
                    if (result.Count >= blockCount)
                    {
                        yield return result;
                        result = new List<F10Record>();
                    }
                }
                if (result.Count > 0)
                {
                    yield return result;
                }
                //    return null;
            }
            //  return result;
        }

        private IEnumerable<List<F30Record>> F30Data(string taskid, int blockCount, DateTime fromDate, DateTime toDate, Expression<Func<TVNNEWForm30View, bool>> predicate)
        {
            EventManager.Inst.WriteInfo("start calling F30Data", 30);
            var wcities = new int[] { 28, 34, 39, 46, 47, 48, 49, 50, 52 };

            using (var tx = new GolestanData.AndisheDBEntities())
            {
                tx.Database.CommandTimeout = 3600;
                tx.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                var sales = tx.TVNNEWForm30View.AsNoTracking().Where(predicate).OrderBy(p => p.BranchCode);
                var result = new List<F30Record>();
                foreach (var b in sales)
                {
                    if (cancelation)
                        throw new OperationCanceledException();
                    var trfc = TavanirHelper.getTrfCodeTVN(b.CoCode, Convert.ToInt32(b.TrfCode), b.PwrCnt, b.SelCode.Value);


                    var f30 = new F30Record() { taskId = taskid.ToString(), srcode = b.SRCode, billid = b.BillID, Id = '1' + b.CoCode.ToString().PadLeft(3, '0') + b.SRCode.ToString("F0") };
                    f30.r130 = new R130Model();
                    f30.r140 = new R140Model();
                    f30.rcalc = new CALCDATAMODEL();
                    try
                    {
                        #region RCALC

                        #endregion

                        f30.rcalc.r212NO_OF_PHASE = b.Phs.Value;
                        f30.rcalc.r213AMPER = b.Amp;
                        f30.rcalc.r214AGREEMENT_DEMAND = b.PwrCnt;
                        f30.rcalc.r233POPULATION_NUMBER = b.FmlCode;
                        f30.rcalc.r238TARIFF_FK = trfc.TarrifCode;
                        f30.rcalc.r228LICENSE_ALLOWED_POWER = b.PwrIcn;
                        var TvnTrfType = b.TrfHCode.ToString();
                        if (b.TrfHCode == -1)
                            TvnTrfType = "1";
                        TvnTrfType += b.TrfType == null ? "0" : b.TrfType.Value.ToString();
                        f30.rcalc.r216TARIFF_TYPE = TavanirHelper.getTrfType(TvnTrfType);


                        #region 130
                        try
                        {
                            f30.r130.r301READING_Date = b.CurrRdgDate.Value;
                            f30.r130.r302ACTIVE_NORMALTIME_READING = b.A1KWCurr;
                            f30.r130.r303ACTIVE_PEAKTIME_READING = b.A2KWCurr;
                            f30.r130.r304ACTIVE_LOWTIME_READING = b.A3KWCurr;
                            f30.r130.r305ACTIVE_WEEKENDTIME_READING = b.A4KWCurr;
                            f30.r130.r306REACTIVE_NORMALTIME_READING = b.R1KWCurr;
                            f30.r130.r307MAXIMETR_READING = b.DemandMeterKW;
                            f30.r130.r308PREV_NORMALTIME_READING = b.A1KWPrior;
                            f30.r130.r309PREV_PEAKTIME_READING = b.A2KWPrior;
                            f30.r130.r310PREV_LOWTIME_READING = b.A3KWPrior;
                            f30.r130.r311PREV_WEEKENDTIME_READING = b.A4KWPrior;
                            f30.r130.r312PREV_REACTIVE_NORMALTIME = b.R1KWPrior;
                            f30.r130.r313PREV_READING_Date = b.Prev_rdg_date.Value;
                            f30.r130.r314AGENT_REPORT_CODE_FK = b.SeeCode;
                            f30.r130.r315PROCESS_STATUS = b.RejectDate == null ? 1 : -1;
                            f30.r130.r316SERVICE_COLLECTION_DAY = b.WorkDayCode;
                            f30.r130.r317SERVICE_READING_AGENT_CODE = b.RdrCode;
                            f30.r130.r318COLD_A1_USAGE = b.ColdA1Use == null ? 0 : b.ColdA1Use.Value;
                            f30.r130.r319COLD_A2_USAGE = b.ColdA2Use == null ? 0 : b.ColdA2Use.Value;
                            f30.r130.r320COLD_A3_USAGE = b.ColdA3Use == null ? 0 : b.ColdA3Use.Value;
                            f30.r130.r321COLD_A4_USAGE = b.ColdA4Use == null ? 0 : b.ColdA4Use.Value;
                            f30.r130.r322REACT_USAGE = b.UseReact == null ? 0 : b.UseReact.Value;

                            f30.r130.r323WARM1_NORMALTIME_CONSUMPTION = b.WarmA1Used;
                            f30.r130.r324WARM1_PEAKTIME_CONSUMPTION = b.WarmA2Used;
                            f30.r130.r325WARM1_LOWTIME_CONSUMPTION = b.WarmA3Used;
                            f30.r130.r326WARM1_WEEKEND_CONSUMPTION = b.WarmA4Used;

                            f30.r130.r327WARM2_NORMALTIME_CONSUMPTION = b.WarmA1Used;

                            f30.r130.r328CHARGED_COLD_NORMALTIME_CONS = b.ColdA1Use == null ? 0 : b.ColdA1Use.Value;
                            f30.r130.r329CHARGED_COLD_PEAKTIME_CONS = b.ColdA2Use == null ? 0 : b.ColdA2Use.Value;
                            f30.r130.r330CHARGED_COLD_LOWTIME_CONS = b.ColdA3Use == null ? 0 : b.ColdA3Use.Value;
                            f30.r130.r331CHARGED_COLD_WEEKEND_CONS = b.ColdA4Use == null ? 0 : b.ColdA4Use.Value;

                            f30.r130.r332CHRGD_REACTIVE_NORMAL_CONS = b.UseReact == null ? 0 : b.UseReact.Value;

                            f30.r130.r333CHRGD_WARM1_NORMALTIME_CONS = b.WarmA1Used;
                            f30.r130.r334CHRGD_WARM1_PEAKTIME_CONS = b.WarmA2Used;
                            f30.r130.r335CHRGD_WARM1_LOWTIME_CONS = b.WarmA3Used;
                            f30.r130.r336CHRGD_WARM1_WEEKEND_CONS = b.WarmA4Used;

                            f30.r130.r337CHRGD_WARM2_NORMAL_CONS = b.WarmA1Used;

                            f30.r130.r338MAXIMETR_KW = b.PwrRead == null ? 0 : b.PwrRead.Value;
                            f30.r130.r339MAXIMETR_CALC = b.PwrCal;

                            f30.r130.r340READING_DURATION = b.TotalDays == null ? 0 : b.TotalDays.Value;

                            f30.r130.r341COLD_DAYS = b.ColdDaysNet == null ? 0 : b.ColdDaysNet.Value;
                            f30.r130.r342WARM_DAYS = b.WarmDays;
                            f30.r130.r343WARM_TO_COLD_RATIO = b.WarmToColdRate == null ? 0 : b.WarmToColdRate.Value;
                            f30.r130.r344TOTAL_ACTIVE_USAGE = b.UseAct == null ? 0 : b.UseAct.Value;
                            f30.r130.r345TOTAL_CONSUMPTION_REACTIVE = b.UseReact == null ? 0 : b.UseReact.Value;

                            f30.r130.r346NOT_INDUSTRIAL_PERSENTAGE = 0;
                            f30.r130.r347REACTIVE_PEAKTIME_READING = 0;
                            f30.r130.r348REACTIVE_LOWTIME_READING = 0;
                            f30.r130.r349REACTIVE_WEEKENDTIME_READING = 0;
                            f30.r130.r350PREV_REACTIVE_PEAKTIME = 0;

                            f30.r130.r351PREV_REACTIVE_LOWTIME = 0;
                            f30.r130.r352PREV_REACTIVE_WEEKENDTIME = 0;

                            f30.r130.r353REACTIVE_PEAKTIME_CONSUMPTION = 0;
                            f30.r130.r354REACTIVE_LOWTIME_CONSUMPTION = 0;

                            f30.r130.r355REACTIVE_WEEKEND_CONSUMPTION = 0;

                            f30.r130.r356WARM2_PEAKTIME_CONSUMPTION = b.WarmA2Used;
                            f30.r130.r357WARM2_LOWTIME_CONSUMPTION = b.WarmA3Used;
                            f30.r130.r358WARM2_WEEKEND_CONSUMPTION = b.WarmA4Used;

                            f30.r130.r359CHRGD_REACTIVE_PEAK_CONS = 0;
                            f30.r130.r360CHRGD_REACTIVE_LOW_CONS = 0;
                            f30.r130.r361CHRGD_REACTIVE_WEEKEND_CONS = 0;


                            f30.r130.r362CHRGD_WARM2_PEAKTIME_CONS = b.WarmA2Used;
                            f30.r130.r363CHRGD_WARM2_LOWTIME_CONS = b.WarmA3Used;
                            f30.r130.r364CHRGD_WARM2_WEEKEND_CONS = b.WarmA4Used;

                            f30.r130.r365WARM3_NORMALTIME_CONSUMPTION = b.WarmA1Used;
                            f30.r130.r366WARM3_PEAKTIME_CONSUMPTION = b.WarmA2Used;
                            f30.r130.r367WARM3_LOWTIME_CONSUMPTION = b.WarmA3Used;
                            f30.r130.r368WARM3_WEEKEND_CONSUMPTION = b.WarmA3Used;

                            f30.r130.r369WARM4_NORMALTIME_CONSUMPTION = b.WarmA1Used;
                            f30.r130.r370WARM4_PEAKTIME_CONSUMPTION = b.WarmA2Used;
                            f30.r130.r371WARM4_LOWTIME_CONSUMPTION = b.WarmA3Used;
                            f30.r130.r372WARM4_WEEKEND_CONSUMPTION = b.WarmA4Used;

                            f30.r130.r373CHRGD_WARM3_NORMALTIME_CONS = 0;
                            f30.r130.r374CHRGD_WARM3_PEAKTIME_CONS = 0;
                            f30.r130.r375CHRGD_WARM3_LOWTIME_CONS = 0;
                            f30.r130.r376CHRGD_WARM3_WEEKEND_CONS = 0;

                            f30.r130.r377CHRGD_WARM4_NORMALTIME_CONS = 0;
                            f30.r130.r378CHRGD_WARM4_PEAKTIME_CONS = 0;
                            f30.r130.r379CHRGD_WARM4_LOWTIME_CONS = 0;
                            f30.r130.r380CHRGD_WARM4_WEEKEND_CONS = 0;


                            f30.r130.r381COLD_USAGE_STEP = b.ColdUsageStep;

                            f30.r130.r382WARM1_USAGE_STEP = 0;
                            f30.r130.r383WARM2_USAGE_STEP = 0;
                            f30.r130.r384WARM3_USAGE_STEP = 0;
                            f30.r130.r385WARM4_USAGE_STEP = 0;

                            if (wcities.Contains(b.CityCode))
                                f30.r130.r384WARM3_USAGE_STEP = b.WarmUsageStep;
                            else
                                f30.r130.r385WARM4_USAGE_STEP = b.WarmUsageStep;

                            f30.r130.r386NORMAL_BOURSE_USAGE = 0;
                            f30.r130.r387PEEK_BOURSE_USAGE = 0;
                            f30.r130.r388LOWTIME_BOURSE_USAGE = 0;


                        }
                        catch (Exception e)
                        {
                            EventManager.Inst.WriteError(string.Format(" branchcode {0} ", b.BranchCode), e, 30130);

                            throw e;

                        }
                        #endregion

                        #region 140
                        try
                        {
                            f30.r140.r406REQUEST_CYCLE = b.SalePrd.Value;
                            f30.r140.r407SERVICE_TYPE = b.BranchTypeCode;
                            f30.r140.r408REQUEST_DATE = b.SaleDateTime.Value;
                            f30.r140.r409NET_AMOUNT = b.PrdAmtNet.Value;
                            f30.r140.r410TAX_AMOUNT = b.TmpFld9 == null ? 0 : b.TmpFld9.Value;
                            f30.r140.r411PAYTOLL_AMOUNT = b.TaxAmt;
                            f30.r140.r412GROSS_AMOUNT = b.PrdAmt;
                            f30.r140.r413PREVIOUS_ACCOUNT_BALANCE = b.CrDbTot;
                            f30.r140.r414TOTAL_REQUESTED_AMOUNT = b.BilAmt;
                            f30.r140.r415BILL_TYPE = b.SaleType; //???????????????????????
                            f30.r140.r416POWER_AMOUNT = b.PwrAmt == null ? 0 : b.PwrAmt.Value;
                            f30.r140.r417SEASON_PEAK_AMOUNT = b.SesnPeekAmt;
                            f30.r140.r418SUBSCRIPTION_AMOUNT = b.SubscAmt;
                            f30.r140.r419DISCOUNT_AMOUNT = b.DiscAmt;

                            f30.r140.r420WARM1_NORMALTIME_AMOUNT = b.WarmA1Amount;
                            f30.r140.r421WARM1_PEAKTIME_AMOUNT = b.WarmA2Amount;
                            f30.r140.r422WARM1_LOWTIME_AMOUNT = b.WarmA3Amount;
                            f30.r140.r423WARM1_WEEKENDTIME_AMOUNT = b.WarmA4Amount;

                            f30.r140.r424COLD_NORMALTIME_AMOUNT = b.ColdA1Amt == null ? 0 : b.ColdA1Amt.Value;
                            f30.r140.r425COLD_PEAKTIME_AMOUNT = b.ColdA2Amt == null ? 0 : b.ColdA2Amt.Value;
                            f30.r140.r426COLD_LOWTIME_AMOUNT = b.ColdA3Amt == null ? 0 : b.ColdA3Amt.Value;
                            f30.r140.r427COLD_WEEKENDTIME_AMOUNT = b.ColdA4Amt == null ? 0 : b.ColdA4Amt.Value;

                            f30.r140.r428WARM2_NORMALTIME_AMOUNT = b.WarmA1Amount;

                            f30.r140.r429NORMAL_REACTIVE_AMOUNT = b.UseAmountR == null ? 0 : b.UseAmountR.Value;
                            f30.r140.r430PATTERNOVERUSE_FINEAMOUNT_WARM = b.PenaltyWarm == null ? 0 : b.PenaltyWarm.Value;
                            f30.r140.r431PATTERNOVERUSE_FINEAMOUNT_COLD = b.PenaltyCold == null ? 0 : b.PenaltyCold.Value;

                            f30.r140.r432POPULATION_NUMBER = b.FmlCode;
                            f30.r140.r433CHARGED_POWER_CONSUMPTION = b.PwrCal;
                            f30.r140.r434POWER_FACTOR = Convert.ToDecimal(b.CosFi == null ? 0 : b.CosFi.Value);
                            f30.r140.r435DAMAGE_FACTOR = Convert.ToDecimal(b.DemageCoef);
                            f30.r140.r436ALLOWED_POWER = b.PwrCnt;
                            f30.r140.r437WARM_SEASON_PEAK_DAYS = b.WarmDays;
                            f30.r140.r438COLD_SEASON_PEAK_DAYS = b.ColdDays;
                            f30.r140.r439PAYMENT_DUE_DATE = b.PAYLIMITDATE;
                            f30.r140.r440DEMAND_OVERUSE_COUNT = b.PwrOverNum;
                            f30.r140.r441ROUND_AMOUNT = Convert.ToDecimal(b.BilRndAmt);
                            f30.r140.r442TARIFF_FK = b.TrfCode;
                            f30.r140.r443TARIFF_OPTION_CODE = b.SelCode == null ? 0 : b.SelCode.Value;
                            f30.r140.r444COMPANY_CODE_FK = b.OrgCode == null ? 0 : b.OrgCode.Value;
                            f30.r140.r445PROCESS_STATUS = b.RejectDate != null ? -1 : 1;
                            f30.r140.r446REJECT_REASON = b.TmpFld7 == null ? 0 : b.TmpFld7.Value;
                            f30.r140.r447REJECT_DATE = b.RejectDate;
                            f30.r140.r448METER_FLAG = 1;
                            f30.r140.r449FREE_AMOUNT = b.FreeAmount == null ? 0 : b.FreeAmount.Value;
                            f30.r140.r450DEMAND_OVERUSE_AMOUNT = 0;
                            f30.r140.r451NON_INDUSTRIAL_AMT = 0;
                            f30.r140.r401BILL_IDENTIFIER = b.BillID;
                            f30.r140.r402PAYMENT_ID = b.PaymentId;
                            f30.r140.r403BILL_SERIAL = b.SaleCode;
                            f30.r140.r404SALE_YEAR = b.SaleYear;
                            f30.r140.r405REQUEST_PERIOD = b.SalePrd == null ? 0 : b.SalePrd.Value;
                            f30.r140.r452LICENSE_EXPIRE_AMOUNT = b.PwrExpAmt == null ? 0 : b.PwrExpAmt.Value;
                            f30.r140.r453NO_GAS_DISCOUNT_AMOUNT = b.DiscGasAmt == null ? 0 : b.DiscGasAmt.Value;
                            f30.r140.r454SHORA_AMT = b.DiscAmt;
                            f30.r140.r455VOLTAGE_DISCOUNT_AMOUNT = b.DiscHighPower == null ? 0 : b.DiscHighPower.Value;
                            f30.r140.r456DAYS_BEFORE_PATTERN = 0;
                            f30.r140.r457CONSUMPTION_BEFORE_PATTERN = 0;
                            f30.r140.r458AMOUNT_BEFORE_PATTERN = 0;
                            f30.r140.r459REVISORY_PAYMENT_REQUEST = b.SaleType == 3 ? 1 : 0;

                            f30.r140.r460WARM2_PEAKTIME_AMOUNT = b.WarmA2Amount;
                            f30.r140.r461WARM2_LOWTIME_AMOUNT = b.WarmA3Amount;
                            f30.r140.r462WARM2_WEEKENDTIME_AMOUNT = b.WarmA4Amount;

                            f30.r140.r463PEAK_REACTIVE_AMOUNT = 0;
                            f30.r140.r464LOW_REACTIVE_AMOUNT = 0;
                            f30.r140.r465WEEKEND_REACTIVE_AMOUNT = 0;
                            f30.r140.r466BUSINESS_CODE_FK = b.ActTypeCode;
                            f30.r140.r467SCHOOLS_DISCOUNT_AMOUNT = trfc.DiscountCode != 6 ? 0 : b.DiscAmt;
                            f30.r140.r468SPECIAL_DISEASE_DISCOUNT_AMNT = trfc.DiscountCode != 2 ? 0 : b.DiscAmt;
                            f30.r140.r469INSURANCE_AMOUNT = b.InsuranceAmount == null ? 0 : b.InsuranceAmount.Value;

                            f30.r140.r470WARM3_NORMALTIME_AMOUNT = b.WarmA1Amount;
                            f30.r140.r471WARM3_PEAKTIME_AMOUNT = b.WarmA2Amount;
                            f30.r140.r472WARM3_LOWTIME_AMOUNT = b.WarmA3Amount;
                            f30.r140.r473WARM3_WEEKENDTIME_AMOUNT = b.WarmA4Amount;

                            f30.r140.r474WARM4_NORMALTIME_AMOUNT = b.WarmA1Amount;
                            f30.r140.r475WARM4_PEAKTIME_AMOUNT = b.WarmA2Amount;
                            f30.r140.r476WARM4_LOWTIME_AMOUNT = b.WarmA3Amount;
                            f30.r140.r477WARM4_WEEKENDTIME_AMOUNT = b.WarmA3Amount;

                            f30.r140.r478CITY_CODE_FK = b.CoCode;
                            f30.r140.r479AREA_CODE = TavanirHelper.GetRegion(b.CoCode, b.RgnCode, b.CityCode);
                            f30.r140.r480ZONE_CODE = TavanirHelper.GetRegion(b.CoCode, b.RgnCode, b.CityCode);
                            f30.r140.r481POWER_PAYTOLL_AMOUNT = b.AvarezBarghAmt == null ? 0 : b.AvarezBarghAmt.Value;
                            f30.r140.r482JANBAZ_DISCOUNT_AMT = b.DisableAmt == null ? 0 : b.DisableAmt.Value;
                            f30.r140.r483MOSQUE_DISCOUNT_AMT = trfc.DiscountCode != 7 ? 0 : b.DiscAmt;
                            f30.r140.r484TUNNEL_DISCOUNT_AMT = b.DiscAmt;
                            f30.r140.r485CNG_DISCOUNT_AMT = b.PwrExpAmt == null ? 0 : b.PwrExpAmt.Value;
                            f30.r140.r486UNINDUSTRIAL_USAGE = 0;
                            f30.r140.r487BUSSINESS_LICENSE_AMT = 0;
                            f30.r140.r488MOSQUE_AREA = 0;
                            f30.r140.r489MOSQUE_DISCOUNT_USAGE = 0;
                            f30.r140.r490BILL_REASON = b.CrctnResonCode;
                            f30.r140.r491TRANSIT_AMOUNT = 0;
                        }
                        catch (Exception e)
                        {
                            EventManager.Inst.WriteError(string.Format(" branchcode {0} ", b.BranchCode), e, 30140);

                            throw e;

                        }
                        #endregion



                    }
                    catch (Exception e)
                    {
                        EventManager.Inst.WriteError(string.Format(" branchcode {0} ", b.BranchCode), e, 30);

                        throw e;

                    }
                    result.Add(f30);
                    if (result.Count >= blockCount)
                    {
                        yield return result;
                        result = new List<F30Record>();
                    }
                }

                if (result.Count > 0)
                {
                    yield return result;
                }
            }

        }


        private IEnumerable<List<F50Record>> F50Data(string taskid, int blockCount, DateTime fromDate, DateTime toDate, Expression<Func<TVNF50FormView, bool>> predicate)
        {

            EventManager.Inst.WriteInfo("start calling F50Data", 50);
            using (var tx = new GolestanData.AndisheDBEntities())
            {
                tx.Database.CommandTimeout = 3600;
                var rcpts = tx.TVNF50FormView.AsNoTracking().Where(p => p.RcptDateTime > new DateTime(2016, 03, 21)).OrderBy(p => p.BranchCode);
                var result = new List<F50Record>();
                foreach (var b in rcpts)
                {
                    if (cancelation)
                        throw new OperationCanceledException();
                    var f50 = new F50Record() { taskId = taskid.ToString(), Id = '1' + b.CoCode.ToString().PadLeft(3, '0') + b.SRCode.ToString("F0") };
                    f50.r150 = new R150Model();
                    try
                    {

                        #region r150
                        f50.r150.r501BILL_IDENTIFIER = b.BillID;
                        f50.r150.r502PAYMENT_ID = b.PaymentID;
                        f50.r150.r503PAYMENT_AMOUNT = b.RcptAmt;
                        f50.r150.r504PAYMENT_DATE = b.BnkDate.Value;
                        if (!string.IsNullOrEmpty(b.IEFileName))
                            f50.r150.r505FILE_NAME = b.IEFileName;
                        if (b.FileDate != null)
                            f50.r150.r506FILE_DATE = b.FileDate.Value;
                        f50.r150.r507PROCESS_DATE = b.RcptDateTime.Value;
                        var val = b.RcptType;
                        int k = Convert.ToInt32(val);
                        if ((k != 2) && (k != 3) && (k != 5) && (k != 6) && (k != 7) && (k != 8) && (k != 9) && (k != 13)
                            && (k != 14) && (k != 59) && (k != 72) && (k != 99))
                            k = 3;

                        val = Convert.ToInt16(b.ReceiptSource);

                        if (k == 3)
                        {
                            val = 1;
                        }
                        else
                        {
                            val = 2;
                        }
                        f50.r150.r508PAYMENT_METHOD_FK = val;
                        f50.r150.r509CREATION_DATE = b.RcptDateTime.Value;
                        f50.r150.r510BANK_CODE_FK = b.BankCode;
                        f50.r150.r511BRANCH_CODE = b.BankBranchCode;


                        f50.r150.r512CHANNEL_TYPE_FK = k;
                        f50.r150.r513TRACKING_NUMBER = b.RefrenceCode;
                        f50.r150.r514STATUS = b.RcptAmt < 0 ? -1 : 1;
                        f50.r150.r515BILL_SERIAL = b.SaleCode;
                        if (b.RcptAmt < 0)
                        {
                            f50.r150.r516CANCEL_DATE = b.RcptDateTime.Value;
                            f50.r150.r517CANCEL_REASON = -1;
                        }
                        f50.r150.r518PAYMENT_TYPE = 1;
                        f50.r150.r519CITY_CODE_FK = b.CoCode;
                        f50.r150.r520AREA_CODE = b.RgnCode;
                        f50.r150.r521ZONE_CODE = b.CityCode;

                        #endregion
                    }
                    catch (Exception e)
                    {
                        EventManager.Inst.WriteError(string.Format(" branchcode {0} ", b.BranchCode), e, 50150);

                        throw e;
                    }
                    result.Add(f50);
                    if (result.Count >= blockCount)
                    {
                        yield return result;
                        result = new List<F50Record>();
                    }
                }

                if (result.Count > 0)
                {
                    yield return result;
                }

            }
        }
    }
}
