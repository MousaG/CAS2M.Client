using CAS2MClientDataMan.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.DataMan
{
    public class TavanirHelper
    {
        public static int getTimerCode(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return 2;
            }
            int tCode = Convert.ToInt32(val);
            if (tCode == 0 || tCode == 1 || tCode == 2)
                tCode = 1;
            else if (tCode == 3)
                tCode = 2;
            else if (tCode == 4)
                tCode = 3;
            else if (tCode == 5)
                tCode = 4;
            return tCode;
        }
        public static TvnTarrifData getTrfCodeTVN(int coCode, int acTrfCode, decimal pwrCnt, int selCode)
        {
            TvnTarrifData result = new TvnTarrifData();
            #region tariff_code
            int trfHcode = Convert.ToInt32((acTrfCode - (acTrfCode % 1000)) / 1000);
            if (selCode < 1 || selCode > 3)
                selCode = 1;

            result.TarrifCode = 100;
            result.SelCode = selCode;
            result.TrfHcode = trfHcode;
            var itemData = TrfHolder.GetInst().GetTrfLawItems(coCode, acTrfCode);
            result.DiscountCode = itemData.DiscTrfCode;
            result.pwrCnt = pwrCnt;
            result.IsCng = itemData.isCng;
            #region 1
            if (trfHcode == 1)
            {
                result.TarrifCode = 100;
            }
            #endregion

            #region 2
            else if (trfHcode == 2)
            {
                if (itemData.LawItem == 1)
                {
                    if (itemData.SubLawITem == 0)
                    {
                        if (pwrCnt > 30)
                        {
                            result.TarrifCode = 211;
                        }
                        else if (pwrCnt <= 30)
                        {
                            result.TarrifCode = 210;
                        }
                    }
                    else if (itemData.SubLawITem == 1)
                    {
                        if (pwrCnt > 30)
                        {
                            result.TarrifCode = 221;
                        }
                        else if (pwrCnt <= 30)
                        {
                            result.TarrifCode = 220;
                        }
                    }
                }
                else if (itemData.LawItem == 2)
                {
                    if (pwrCnt > 30)
                    {
                        result.TarrifCode = 231;
                    }
                    else if (pwrCnt <= 30)
                    {
                        result.TarrifCode = 230;
                    }
                }
                /////*
                if (itemData.isCng && pwrCnt <= 30)
                    result.TarrifCode = 290;
                else if (itemData.isCng && pwrCnt > 30)
                    result.TarrifCode = 291;
                /////*

                int discount = TavanirHelper.GetDiscount(coCode, acTrfCode, pwrCnt);
                if (discount != 0)
                    result.TarrifCode = discount;
            }
            #endregion

            #region 3
            else if (trfHcode == 3)
            {
                if (itemData.LawItem == 1)
                {
                    if (pwrCnt > 30)
                    {
                        result.TarrifCode = 311;
                    }
                    else
                    {
                        result.TarrifCode = 310;
                    }
                }
                else if (itemData.LawItem == 2)
                {
                    if (pwrCnt > 30)
                    {
                        result.TarrifCode = 321;
                    }
                    else
                    {
                        result.TarrifCode = 320;
                    }
                }
                else if (itemData.LawItem == 3)
                {
                    if (itemData.SubLawITem == 0)
                    {
                        if (pwrCnt > 30)
                        {
                            result.TarrifCode = 331;
                        }
                        else
                        {
                            result.TarrifCode = 330;
                        }

                    }
                    else if (itemData.SubLawITem == 1)
                    {
                        if (pwrCnt > 30)
                        {
                            result.TarrifCode = 341;
                        }
                    }
                }
            }
            #endregion

            #region 4
            else if (trfHcode == 4)
            {
                if (itemData.LawItem == 1)
                {
                    if (itemData.SubLawITem == 0)
                    {
                        if (pwrCnt > 30)
                        {
                            result.TarrifCode = 411;
                        }
                        else if (pwrCnt <= 30)
                        {
                            result.TarrifCode = 410;
                        }
                        if (selCode == 3 && pwrCnt > 30)
                            /////*
                            result.TarrifCode = 413;
                    }
                    else if (itemData.SubLawITem == 1)
                    {
                        if (pwrCnt > 30)
                        {
                            result.TarrifCode = 421;
                        }
                    }
                }
                else if (itemData.LawItem == 2)
                {
                    if (itemData.SubLawITem == 0)
                    {
                        if (pwrCnt > 30)
                        {
                            result.TarrifCode = 431;
                        }
                        else if (pwrCnt <= 30)
                        {
                            result.TarrifCode = 420;
                        }
                        if (selCode == 3 && pwrCnt > 30)
                            /////*
                            result.TarrifCode = 433;
                    }
                    else if (itemData.SubLawITem == 1)
                    {
                        if (pwrCnt > 30)
                        {
                            result.TarrifCode = 441;
                        }
                    }
                }
            }
            #endregion

            #region 5

            if (trfHcode == 5)
            {
                if (pwrCnt > 30)
                {
                    result.TarrifCode = 510;
                }

                else if (pwrCnt <= 30)
                {
                    result.TarrifCode = 511;
                }
            }
            #endregion




            #endregion
            return result;
        }

        public static int getBranchStatus(int acStatus)
        {
            switch (acStatus)
            {
                case 0: return 1;
                case 1: return 2;
                case 3: return 5;
                case 4: return 4;
                case 5: return 4;
                case 6: return 5;
                case 7: return 1;
                case 8: return 2;
                case 9: return 2;
                case 10: return 5;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    return 2;
                case 21:
                case 22:
                case 23:
                    return 5;
                default:
                    return 2;
            }
        }

        public static short GetRegion(int coCode, short rgnCode, short cityCode)
        {
            if (coCode == 61) //Ahwaz
                return 11;
            if ((coCode == 72)     //Qazvin
                || (coCode == 42)) //Navahi Tehran
                return 0;
            if (coCode == 141)
            {
                if (
(rgnCode == 1 && cityCode == 3) ||  //Kiasar
(rgnCode == 1 && cityCode == 15) || //Mohamad abad
(rgnCode == 4 && cityCode == 14) || //Alasht
(rgnCode == 7 && cityCode == 48) || //Polor
(rgnCode == 7 && cityCode == 49) || //Aghzben
(rgnCode == 7 && cityCode == 50) || //Panj ab
(rgnCode == 7 && cityCode == 53) || //Baladeh
(rgnCode == 7 && cityCode == 54))   //Rineh
                {
                    return 0; //Sard
                }
                else
                    return 48;// Garmsir 4 mazan
            }
            if (coCode == 142) //Chalous
            {
                if (
                    (rgnCode == 18 && cityCode == 75) //Baladeh
                  || (rgnCode == 19 && cityCode == 82) //Kojor
                  || (rgnCode == 20 && cityCode == 79) //Kelardasht
                  || (rgnCode == 20 && cityCode == 80))//MarzanAbad
                    return 0; //Sard
                else
                    return 48; // Garmsir 4 Chalous

            }
            if (coCode == 143) //Golestan
            {
                if (
                     (rgnCode == 75) ||  //MinoDasht
                    (rgnCode == 85) ||  //Kalale
                    (rgnCode == 50) ||  //Agh Ghala
                    (rgnCode == 70))    //Gonbad
                {
                    return 33; //Garmsir 3
                }
                else
                    return 43; //Garmsir 4
            }
            if (coCode == 161) // Yazd
            {
                if (rgnCode == 40) //Bafgh
                    return 44;
                if (rgnCode == 73) //Tabas
                    return 41;
            }
            return 0;
        }

        public static string getTrfType(string val)
        {
            if (val == "60" || val == "70")
            {
                return "50";
            }

            else
                          if (val == "00")
            { return "10"; }
            else
                          if ((val != "10") && (val != "11") && (val != "20") && (val != "21")
                              && (val != "30") && (val != "31") && (val != "40") && (val != "41") && (val != "50")
                              && (val != "51") && (val != "99"))
            {
                throw new Exception(string.Format("Invalid TrfCode {0} for Branch {1}", val, ""));
            }
            return val;
        }
        public static int GetDiscount(int coCode, int trfcode, decimal pwrcnt)
        {
            #region chalous
            if (coCode == 142) //Chalous
            {
                if (trfcode == 2402 || trfcode == 2322 || trfcode == 2222)
                {
                    if (pwrcnt <= 30)
                        return 286;
                    else
                        return 287;
                }
                else if (trfcode == 2220 || trfcode == 2218)
                {
                    if (pwrcnt <= 30)
                        return 280;
                    else
                        return 281;
                }
                else if (trfcode == 2114)
                {
                    if (pwrcnt <= 30)
                        return 282;
                    else
                        return 283;
                }
            }
            #endregion

            #region yazd
            else if (coCode == 161) // Yazd
            {
                if (trfcode == 2202 || trfcode == 2203 || trfcode == 2288 || trfcode == 2290 || trfcode == 2313 || trfcode == 2388 || trfcode == 2390)
                {
                    if (pwrcnt <= 30)
                        return 286;
                    else
                        return 287;
                }
                else if (trfcode == 2361)
                {
                    if (pwrcnt <= 30)
                        return 280;
                    else
                        return 281;
                }
            }
            #endregion

            #region Qazvin
            else if (coCode == 72) // Qazvin
            {
                if (trfcode == 2131 || trfcode == 2132 || trfcode == 2130)
                {
                    if (pwrcnt <= 30)
                        return 282;
                    else
                        return 283;
                }
                else if (trfcode == 2999)
                {
                    if (pwrcnt <= 30)
                        return 280;
                    else
                        return 281;
                }
                else if (trfcode == 2332 || trfcode == 2330 || trfcode == 2331)
                {
                    if (pwrcnt <= 30)
                        return 284;
                    else
                        return 285;
                }
                else if (trfcode == 2310 || trfcode == 2321 || trfcode == 2322)
                {
                    if (pwrcnt <= 30)
                        return 286;
                    else
                        return 287;
                }
            }
            #endregion

            #region ahwaz
            else if (coCode == 61) // ahwaz
            {
                if (trfcode == 2410 || trfcode == 2440)
                {
                    if (pwrcnt <= 30)
                        return 286;
                    else
                        return 287;
                }
                else if (trfcode == 2900)
                {
                    if (pwrcnt <= 30)
                        return 280;
                    else
                        return 281;
                }
                else if (trfcode == 2150)
                {
                    if (pwrcnt <= 30)
                        return 282;
                    else
                        return 283;
                }

            }
            #endregion

            #region Golestan
            else if (coCode == 143) //Golestan
            {
                if (trfcode == 2222 || trfcode == 2322 || trfcode == 2402)
                {
                    if (pwrcnt <= 30)
                        return 286;
                    else
                        return 287;
                }
                else if (trfcode == 2113 || trfcode == 2218 || trfcode == 2220)
                {
                    if (pwrcnt <= 30)
                        return 280;
                    else
                        return 281;
                }
                else if (trfcode == 2114 || trfcode == 2116)
                {
                    if (pwrcnt <= 30)
                        return 282;
                    else
                        return 283;
                }
            }
            #endregion

            #region tehran
            else if (coCode == 42) //Navahi Tehran
            {
                if (trfcode == 2090)
                {
                    if (pwrcnt <= 30)
                        return 286;
                    else
                        return 287;
                }
            }
            #endregion

            #region mazandaran
            else if (coCode == 141) //mazandaran
            {
                if (trfcode == 2322 || trfcode == 2402 || trfcode == 2222)
                {
                    if (pwrcnt <= 30)
                        return 286;
                    else
                        return 287;
                }
                else if (trfcode == 2114)
                {
                    if (pwrcnt <= 30)
                        return 282;
                    else
                        return 283;
                }
                else if (trfcode == 2220 || trfcode == 2218)
                {
                    if (pwrcnt <= 30)
                        return 280;
                    else
                        return 281;
                }
            }
            #endregion


            #region Qom
            else if (coCode == 44) // Qom
            {
                if (trfcode == 2093)
                {
                    if (pwrcnt <= 30)
                        return 282;
                    else
                        return 283;
                }
                else if (trfcode == 2090)
                {
                    if (pwrcnt <= 30)
                        return 280;
                    else
                        return 281;
                }
                else if (trfcode == 2092)
                {
                    if (pwrcnt <= 30)
                        return 286;
                    else
                        return 287;
                }
            }
            #endregion

            #region karaj
            else if (coCode == 43) // karaj
            {
                if (trfcode == 2093)
                {
                    if (pwrcnt <= 30)
                        return 282;
                    else
                        return 283;
                }
                else if (trfcode == 2090)
                {
                    if (pwrcnt <= 30)
                        return 280;
                    else
                        return 281;
                }
                else if (trfcode == 2092)
                {
                    if (pwrcnt <= 30)
                        return 284;
                    else
                        return 285;
                }
                else if (trfcode == 2039)
                {
                    if (pwrcnt <= 30)
                        return 286;
                    else
                        return 287;
                }
            }
            #endregion
            return 0;
        }

        public static short GetCycle(short SalePrd, short DurationType)
        {
            if (DurationType == 1)
            {
                if (SalePrd == 1 || SalePrd == 2)
                    return 1;
                else if (SalePrd == 3 || SalePrd == 4)
                    return 2;
                else if (SalePrd == 5 || SalePrd == 6)
                    return 3;
                else if (SalePrd == 7 || SalePrd == 8)
                    return 4;
                else if (SalePrd == 9 || SalePrd == 10)
                    return 5;
                else if (SalePrd == 11 || SalePrd == 12)
                    return 6;
            }
            else if (DurationType == 2)
            {
                return SalePrd;
            }
            return SalePrd;
        }

        public static bool GetDiscAmtSchool(int coCode, int trfcode)
        {
            #region yazd
            if (coCode == 161) // Yazd
            {
                if (trfcode == 2118 || trfcode == 2218 || trfcode == 2318)
                {
                    return true;
                }
            }
            #endregion

            #region Qom
            if (coCode == 44) // Qom
            {
                if (trfcode == 2330)
                {
                    return true;
                }
            }
            #endregion

            #region karj
            if (coCode == 43) // Qom
            {
                if (trfcode == 2330)
                {
                    return true;
                }
            }
            #endregion
            return false;
        }

    }
}
