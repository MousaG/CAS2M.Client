using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.Domain
{
    public class TvnTarrifData
    {
        public int TrfHcode { get; set; }
        public int TarrifCode { get; set; }
        public int DiscountCode { get; set; }
        public decimal pwrCnt { get; set; }
        public decimal SelCode { get; set; }
        public bool IsCng { get; set; }
    }
}
