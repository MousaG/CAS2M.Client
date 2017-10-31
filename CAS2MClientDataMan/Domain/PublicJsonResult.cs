using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.Domain
{
    public class PublicJsonResult
    {
        public string message { get; set; }
        public bool result { get; set; }
        public string id { get; set; }
        public Guid guid { get; set; }
    }
}
