using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.Domain
{
   public class TaskData
    {
        public bool result{ get; set; }
        public string message{ get; set; }
        public string taskid { get; set; }
        public Guid taskToken { get; set; }
        public string callbackurl { get; set; }
    }
}
