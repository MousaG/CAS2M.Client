using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.Domain
{
    public class BaseEntity
    {
        public BaseEntity()
        {
           // id = ObjectId.GenerateNewId().ToString();

        }

        public string Id
        {
            get { return id; }
            set
            {
             //   if (string.IsNullOrEmpty(value))
               //     id = ObjectId.GenerateNewId().ToString();
                //else
                    id = value;
            }
        }

        private string id { get; set; }
    }
    public class F10Record : TVNRECORD
    {
        public F10Record()
        {
        }

        public string taskId { get; set; }
        public bool isUpdate { get; set; }
        public R110Model r110 { get; set; }
        public R120Model r120 { get; set; }
        public ICollection<R125Model> r125 { get; set; }
        public ICollection<R125Model> r125r { get; set; }


    }

    public class TVNRECORD : BaseEntity
    {
        // public string id { get; set; }
        public string refcode { get; set; }

    }


    public class R110Model
    {
        public decimal r10TOTAL_BILL_DEBT { get; set; }
        public decimal r20TOTAL_REGISTER_DEBT { get; set; }
        public decimal r30OTHER_ACCOUNT_BALANCE { get; set; }
        public int r101CUSTOMER_TYPE { get; set; }
        public String r102FIRST_NAME { get; set; }
        public String r103SURNAME { get; set; }
        public String r104FATHER_NAME { get; set; }
        public int? r105BIRTH_CERTIFICATE_ID { get; set; }
        public String r106NATIONAL_CARD_ID { get; set; }
        public DateTime r107BIRTH_DATE { get; set; }
        public String r108ISSUE_PLACE { get; set; }
        public string r109COMPANY_NAME { get; set; }
        public DateTime? r110COMPANY_REGISTRATION_DATE { get; set; }
        public String r111COMPANY_REGISTRATION_ID { get; set; }
        public decimal? r112COMPANY_ISIC_FK { get; set; }
        public int r113COMPANY_CODE_FK { get; set; }
        public int? r114SEX_TYPE { get; set; }
        public string r115PHONE_NUMBEr { get; set; }
        public string r116MOBILE_NUMBEr { get; set; }
        public string r117FAX_NUMBEr { get; set; }
        public string r118EMAIL_ADDRESS { get; set; }
        public string r119ADDRESS { get; set; }
        public string r120POSTAL_CODE { get; set; }
        public decimal? r121BUSINESS_CODE_FK { get; set; }
    }


    public class R120Model
    {
        public string r200BILL_IDENTIFIER { get; set; }
        public decimal r201FILE_SERIAL_NUMBER { get; set; }
        public decimal r202SUBSCRIPTION_ID { get; set; }
        public decimal r204CITY_CODE_FK { get; set; }
        public decimal r205AREA_CODE { get; set; }
        public decimal r206ZONE_CODE { get; set; }
        public string r207IDENTIFYING_NUMBER { get; set; }
        public decimal r208READING_COLLECTION_DAY { get; set; }
        public decimal r209READING_AGENT_CODE { get; set; }
        public decimal r210READING_SEQUENCE { get; set; }
        public decimal r211SERVICE_TYPE { get; set; }
        public decimal r212NO_OF_PHASE { get; set; }
        public decimal r213AMPER { get; set; }
        public decimal r214AGREEMENT_DEMAND { get; set; }
        public decimal r215VOLTAGE_TYPE { get; set; }
        public string r216TARIFF_TYPE { get; set; }
        public decimal r217TARIFF_OPTION_CODE { get; set; }
        public decimal r219PREMISE_LOCATION { get; set; }
        public string r220SERVICE_POINT_ADDRESS { get; set; }
        public string r221SERVICE_POINT_POSTCODE { get; set; }
        public string r222PHONE_NUMBER { get; set; }
        public DateTime? r223INSTALLATION_DATE { get; set; }
        public DateTime? r224CREATION_DATE { get; set; }
        public DateTime? r225AGREEMENT_DATE { get; set; }
        public string r226AGREEMENT_NUMBER { get; set; }
        public int r227SERVICE_POINT_STATUS { get; set; }
        public double r228LICENSE_ALLOWED_POWER { get; set; }
        public DateTime? r229LICENSE_EXPIRE_DATE { get; set; }
        public string r230LICENSE_NUMBER { get; set; }
        public string r231LICENSE_ISSUER { get; set; }
        public int r232ELECTRICITY_SUPPLY_FK { get; set; }
        public int r233POPULATION_NUMBER { get; set; }
        public DateTime? r234POPULATION_EXPIRE_DATE { get; set; }
        public int r235DISCOUNT_CONSUMPTION_FK { get; set; }
        public int r236DISCOUNT_REGISTRATION_FK { get; set; }

        public string r237REGISTRATION_DISCOUNT_REF { get; set; }
        public decimal r238TARIFF_FK { get; set; }
        public DateTime? r239SERVICE_INACTIVE_DATE { get; set; }
        public DateTime? r240SERVICE_DELETE_DATE { get; set; }
        public DateTime? r241TEMPORARY_REDUCE_EXPIRE_DATE { get; set; }
        public decimal r242LAST_POWER_REDUCTION { get; set; }
        public DateTime? r243TEMPORARY_REDUCE_START_DATE { get; set; }
        public int r244TEMPORARY_POWER_REDUCT_COUNT { get; set; }
        public int r245TRACKING_CODE { get; set; }
        public int r246TRANSFORMATOR_COUNT { get; set; }
        public string r247POST_NUMBER { get; set; }
        public string r248FEEDER_NUMBER { get; set; }
        public string r249BASE_NUMBER { get; set; }
        public int r250TRANSFORMATOR_POWER { get; set; }
        public int r251TRANSFORMATOR_NUMBER { get; set; }
        public DateTime? r252LAST_TEST_DATE { get; set; }
        public int r254GAS_ENERGY_STATUS { get; set; }
        public int r255REPEAT_CODE { get; set; }
        public double r256X_DEGREE { get; set; }
        public double r257Y_DEGREE { get; set; }
        public int r258Y_MOSQUE_AREA { get; set; }
        public int r259TRANSIT_DEMAND { get; set; }
        public int r260CONNECTION_TYPE { get; set; }
        public int r261RURAL_DISTRICT{ get; set; }

    }



    public class R125Model
    {
        public string r280SERIAL_NUMBER { get; set; }
        public int r281METER_TYPE { get; set; }
        public int r282DIGIT_NUMBER { get; set; }
        public double r283ADJUSTMENT_FACTOR { get; set; }
        public int r284METER_MODEL_FK { get; set; }
        public int r285METER_MAKER_TYPE_FK { get; set; }
        public DateTime r286INSTALLATION_DATE { get; set; }
        public int r287READING_CLOCK_CODE { get; set; }
        public double r288MAXIMETER_FACTOR { get; set; }
        public int r289METER_STATUS { get; set; }
        public DateTime r290CREATION_DATE { get; set; }
        public DateTime? r291EXPIRATION_DATE { get; set; }
        public int r292CHANGE_REASON { get; set; }

         
    }

    public class F30Record : BaseEntity
    {
        public F30Record()
        {
        }

        public string taskId { get; set; }
        public bool isUpdate { get; set; }
        //   public string id { get; set; }
        public string refcode { get; set; }
        public long srcode { get; set; }
        public string billid { get; set; }
        public R130Model r130 { get; set; }
        public R140Model r140 { get; set; }
        public CALCDATAMODEL rcalc { get; set; }

    }


    //Masraf
    public class R130Model
    {
        internal int r351PREV_REACTIVE_LOWTIME;
        internal int r352PREV_REACTIVE_WEEKENDTIME;
        internal int r353REACTIVE_PEAKTIME_CONSUMPTION;
        internal int r354REACTIVE_LOWTIME_CONSUMPTION;
        internal int r355REACTIVE_WEEKEND_CONSUMPTION;
        internal decimal r356WARM2_PEAKTIME_CONSUMPTION;
        internal decimal r357WARM2_LOWTIME_CONSUMPTION;
        internal decimal r358WARM2_WEEKEND_CONSUMPTION;
        internal int r359CHRGD_REACTIVE_PEAK_CONS;
        internal int r360CHRGD_REACTIVE_LOW_CONS;
        internal int r361CHRGD_REACTIVE_WEEKEND_CONS;
        internal decimal r362CHRGD_WARM2_PEAKTIME_CONS;
        internal decimal r363CHRGD_WARM2_LOWTIME_CONS;
        internal decimal r364CHRGD_WARM2_WEEKEND_CONS;
        internal decimal r365WARM3_NORMALTIME_CONSUMPTION;
        internal decimal r366WARM3_PEAKTIME_CONSUMPTION;
        internal decimal r367WARM3_LOWTIME_CONSUMPTION;
        internal decimal r368WARM3_WEEKEND_CONSUMPTION;
        internal decimal r369WARM4_NORMALTIME_CONSUMPTION;
        internal decimal r370WARM4_PEAKTIME_CONSUMPTION;
        internal decimal r371WARM4_LOWTIME_CONSUMPTION;
        internal decimal r372WARM4_WEEKEND_CONSUMPTION;
        internal decimal r373CHRGD_WARM3_NORMALTIME_CONS;
        internal decimal r374CHRGD_WARM3_PEAKTIME_CONS;
        internal decimal r375CHRGD_WARM3_LOWTIME_CONS;
        internal decimal r376CHRGD_WARM3_WEEKEND_CONS;

        internal decimal r377CHRGD_WARM4_NORMALTIME_CONS;
        internal decimal r378CHRGD_WARM4_PEAKTIME_CONS;
        internal decimal r379CHRGD_WARM4_LOWTIME_CONS;
        internal decimal r380CHRGD_WARM4_WEEKEND_CONS;

        internal decimal r381COLD_USAGE_STEP;
        internal decimal r382WARM1_USAGE_STEP;
        internal decimal r383WARM2_USAGE_STEP;
        internal decimal r384WARM3_USAGE_STEP;
        internal decimal r385WARM4_USAGE_STEP;

        internal decimal r386NORMAL_BOURSE_USAGE;
        internal decimal r387PEEK_BOURSE_USAGE;
        internal decimal r388LOWTIME_BOURSE_USAGE;


        public DateTime r301READING_Date { get; set; }
        public decimal r302ACTIVE_NORMALTIME_READING { get; set; }
        public decimal r303ACTIVE_PEAKTIME_READING { get; set; }
        public decimal r304ACTIVE_LOWTIME_READING { get; set; }
        public decimal r305ACTIVE_WEEKENDTIME_READING { get; set; }
        public decimal r306REACTIVE_NORMALTIME_READING { get; set; }
        public decimal r307MAXIMETR_READING { get; set; }
        public decimal r308PREV_NORMALTIME_READING { get; set; }
        public decimal r309PREV_PEAKTIME_READING { get; set; }
        public decimal r310PREV_LOWTIME_READING { get; set; }
        public decimal r311PREV_WEEKENDTIME_READING { get; set; }
        public decimal r312PREV_REACTIVE_NORMALTIME { get; set; }
        public DateTime r313PREV_READING_Date { get; set; }
        public int r314AGENT_REPORT_CODE_FK { get; set; }
        public int r315PROCESS_STATUS { get; set; }
        public int r316SERVICE_COLLECTION_DAY { get; set; }
        public int r317SERVICE_READING_AGENT_CODE { get; set; }
        public decimal r318COLD_A1_USAGE { get; set; }
        public decimal r319COLD_A2_USAGE { get; set; }
        public decimal r320COLD_A3_USAGE { get; set; }
        public decimal r321COLD_A4_USAGE { get; set; }
        public decimal r322REACT_USAGE { get; set; }
        public decimal r323WARM1_NORMALTIME_CONSUMPTION { get; set; }
        public decimal r324WARM1_PEAKTIME_CONSUMPTION { get; set; }
        public decimal r325WARM1_LOWTIME_CONSUMPTION { get; set; }
        public decimal r326WARM1_WEEKEND_CONSUMPTION { get; set; }
        public decimal r327WARM2_NORMALTIME_CONSUMPTION { get; set; }
        public decimal r328CHARGED_COLD_NORMALTIME_CONS { get; set; }
        public decimal r329CHARGED_COLD_PEAKTIME_CONS { get; set; }
        public decimal r330CHARGED_COLD_LOWTIME_CONS { get; set; }
        public decimal r331CHARGED_COLD_WEEKEND_CONS { get; set; }
        public decimal r332CHRGD_REACTIVE_NORMAL_CONS { get; set; }
        public decimal r333CHRGD_WARM1_NORMALTIME_CONS { get; set; }
        public decimal r334CHRGD_WARM1_PEAKTIME_CONS { get; set; }
        public decimal r335CHRGD_WARM1_LOWTIME_CONS { get; set; }
        public decimal r336CHRGD_WARM1_WEEKEND_CONS { get; set; }
        public decimal r337CHRGD_WARM2_NORMAL_CONS { get; set; }
        public decimal r338MAXIMETR_KW { get; set; }
        public decimal r339MAXIMETR_CALC { get; set; }
        public int r340READING_DURATION { get; set; }
        public int r341COLD_DAYS { get; set; }
        public int r342WARM_DAYS { get; set; }
        public decimal r343WARM_TO_COLD_RATIO { get; set; }
        public decimal r344TOTAL_ACTIVE_USAGE { get; set; }
        public decimal r345TOTAL_CONSUMPTION_REACTIVE { get; set; }
        public decimal r346NOT_INDUSTRIAL_PERSENTAGE { get; set; }
        public decimal r347REACTIVE_PEAKTIME_READING { get; set; }
        public decimal r348REACTIVE_LOWTIME_READING { get; set; }
        public decimal r349REACTIVE_WEEKENDTIME_READING { get; set; }
        public decimal r350PREV_REACTIVE_PEAKTIME { get; set; }


    }



    public class R140Model
    {
        public decimal r406REQUEST_CYCLE { get; set; }
        public decimal r407SERVICE_TYPE { get; set; }
        public DateTime r408REQUEST_DATE { get; set; }
        public decimal r409NET_AMOUNT { get; set; }
        public decimal r410TAX_AMOUNT { get; set; }
        public decimal r411PAYTOLL_AMOUNT { get; set; }
        public decimal r412GROSS_AMOUNT { get; set; }
        public decimal r413PREVIOUS_ACCOUNT_BALANCE { get; set; }
        public decimal r414TOTAL_REQUESTED_AMOUNT { get; set; }
        public decimal r415BILL_TYPE { get; set; }
        public decimal r416POWER_AMOUNT { get; set; }
        public decimal r417SEASON_PEAK_AMOUNT { get; set; }
        public decimal r418SUBSCRIPTION_AMOUNT { get; set; }
        public decimal r419DISCOUNT_AMOUNT { get; set; }
        public decimal r420WARM1_NORMALTIME_AMOUNT { get; set; }
        public decimal r421WARM1_PEAKTIME_AMOUNT { get; set; }
        public decimal r422WARM1_LOWTIME_AMOUNT { get; set; }
        public decimal r423WARM1_WEEKENDTIME_AMOUNT { get; set; }
        public decimal r424COLD_NORMALTIME_AMOUNT { get; set; }
        public decimal r425COLD_PEAKTIME_AMOUNT { get; set; }
        public decimal r426COLD_LOWTIME_AMOUNT { get; set; }
        public decimal r427COLD_WEEKENDTIME_AMOUNT { get; set; }
        public decimal r428WARM2_NORMALTIME_AMOUNT { get; set; }
        public decimal r429NORMAL_REACTIVE_AMOUNT { get; set; }
        public decimal r430PATTERNOVERUSE_FINEAMOUNT_WARM { get; set; }
        public decimal r431PATTERNOVERUSE_FINEAMOUNT_COLD { get; set; }
        public decimal r432POPULATION_NUMBER { get; set; }
        public decimal r433CHARGED_POWER_CONSUMPTION { get; set; }
        public decimal r434POWER_FACTOR { get; set; }
        public decimal r435DAMAGE_FACTOR { get; set; }
        public decimal r436ALLOWED_POWER { get; set; }
        public decimal r437WARM_SEASON_PEAK_DAYS { get; set; }
        public decimal r438COLD_SEASON_PEAK_DAYS { get; set; }
        public DateTime? r439PAYMENT_DUE_DATE { get; set; }
        public decimal r440DEMAND_OVERUSE_COUNT { get; set; }
        public decimal r441ROUND_AMOUNT { get; set; }
        public decimal r442TARIFF_FK { get; set; }
        public decimal r443TARIFF_OPTION_CODE { get; set; }
        public decimal r444COMPANY_CODE_FK { get; set; }
        public decimal r445PROCESS_STATUS { get; set; }
        public decimal r446REJECT_REASON { get; set; }
        public DateTime? r447REJECT_DATE { get; set; }
        public decimal r448METER_FLAG { get; set; }
        public decimal r449FREE_AMOUNT { get; set; }
        public decimal r450DEMAND_OVERUSE_AMOUNT { get; set; }
        public decimal r451NON_INDUSTRIAL_AMT { get; set; }
        public string r401BILL_IDENTIFIER { get; set; }
        public string r402PAYMENT_ID { get; set; }
        public decimal r403BILL_SERIAL { get; set; }
        public decimal r404SALE_YEAR { get; set; }
        public decimal r405REQUEST_PERIOD { get; set; }
        public decimal r452LICENSE_EXPIRE_AMOUNT { get; set; }
        public decimal r453NO_GAS_DISCOUNT_AMOUNT { get; set; }
        public decimal r454SHORA_AMT { get; set; }
        public decimal r455VOLTAGE_DISCOUNT_AMOUNT { get; set; }
        public decimal r456DAYS_BEFORE_PATTERN { get; set; }
        public decimal r457CONSUMPTION_BEFORE_PATTERN { get; set; }
        public decimal r458AMOUNT_BEFORE_PATTERN { get; set; }
        public decimal r459REVISORY_PAYMENT_REQUEST { get; set; }
        public decimal r460WARM2_PEAKTIME_AMOUNT { get; set; }
        public decimal r461WARM2_LOWTIME_AMOUNT { get; set; }
        public decimal r462WARM2_WEEKENDTIME_AMOUNT { get; set; }
        public decimal r463PEAK_REACTIVE_AMOUNT { get; set; }
        public decimal r464LOW_REACTIVE_AMOUNT { get; set; }
        public decimal r465WEEKEND_REACTIVE_AMOUNT { get; set; }
        public decimal? r466BUSINESS_CODE_FK { get; set; }
        public decimal r467SCHOOLS_DISCOUNT_AMOUNT { get; set; }
        public decimal r468SPECIAL_DISEASE_DISCOUNT_AMNT { get; set; }
        public decimal r469INSURANCE_AMOUNT { get; set; }
        public decimal r470WARM3_NORMALTIME_AMOUNT { get; set; }
        public decimal r471WARM3_PEAKTIME_AMOUNT { get; set; }
        public decimal r472WARM3_LOWTIME_AMOUNT { get; set; }
        public decimal r473WARM3_WEEKENDTIME_AMOUNT { get; set; }
        public decimal r474WARM4_NORMALTIME_AMOUNT { get; set; }
        public decimal r475WARM4_PEAKTIME_AMOUNT { get; set; }
        public decimal r476WARM4_LOWTIME_AMOUNT { get; set; }
        public decimal r477WARM4_WEEKENDTIME_AMOUNT { get; set; }
        public decimal r478CITY_CODE_FK { get; set; }
        public decimal r479AREA_CODE { get; set; }
        public decimal r480ZONE_CODE { get; set; }
        public decimal r481POWER_PAYTOLL_AMOUNT { get; set; }
        public decimal r482JANBAZ_DISCOUNT_AMT { get; set; }
        public decimal r483MOSQUE_DISCOUNT_AMT { get; set; }
        public decimal r484TUNNEL_DISCOUNT_AMT { get; set; }
        public decimal r485CNG_DISCOUNT_AMT { get; set; }
        public decimal r486UNINDUSTRIAL_USAGE { get; set; }
        public decimal r487BUSSINESS_LICENSE_AMT { get; set; }
        public decimal r488MOSQUE_AREA { get; set; }
        public decimal r489MOSQUE_DISCOUNT_USAGE { get; set; }
        public decimal r490BILL_REASON { get; set; }
        
        //new
        public decimal r491TRANSIT_AMOUNT { get; set; }

    }




    public class CALCDATAMODEL
    {
        public int r212NO_OF_PHASE { get; set; }
        public decimal r213AMPER { get; set; }
        public decimal r214AGREEMENT_DEMAND { get; set; }
        public decimal r233POPULATION_NUMBER { get; set; }
        public decimal r238TARIFF_FK { get; set; }
        public decimal r228LICENSE_ALLOWED_POWER { get; set; }

        public string r216TARIFF_TYPE { get; set; }
    }

    public class F50Record : TVNRECORD
    {
        public F50Record()
        {
        }

        public string taskId { get; set; }
        public bool isUpdate { get; set; }
        public R150Model r150 { get; set; }

    }




    public class R150Model
    {

        public string r501BILL_IDENTIFIER { get; set; }
        public string r502PAYMENT_ID { get; set; }
        public decimal r503PAYMENT_AMOUNT { get; set; }
        public DateTime r504PAYMENT_DATE { get; set; }
        public string r505FILE_NAME { get; set; }
        public DateTime r506FILE_DATE { get; set; }
        public DateTime r507PROCESS_DATE { get; set; }
        public decimal r508PAYMENT_METHOD_FK { get; set; }
        public DateTime r509CREATION_DATE { get; set; }
        public decimal r510BANK_CODE_FK { get; set; }
        public decimal r511BRANCH_CODE { get; set; }
        public decimal r513TRACKING_NUMBER { get; set; }
        public decimal r512CHANNEL_TYPE_FK { get; set; }
        public decimal r514STATUS { get; set; }
        public decimal r515BILL_SERIAL { get; set; }
        public DateTime? r516CANCEL_DATE { get; set; }
        public decimal r517CANCEL_REASON { get; set; }
        public decimal r518PAYMENT_TYPE { get; set; }
        public decimal r519CITY_CODE_FK { get; set; }
        public decimal r520AREA_CODE { get; set; }
        public decimal r521ZONE_CODE { get; set; }
    }

}
