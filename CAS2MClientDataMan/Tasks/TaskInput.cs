using CAS2MClientDataMan.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.Tasks
{
    public class TaskInput
    {
        public string Id { get; set; }

        public Guid Token { get; set; }


        public EntityType EntityType { get; set; }



        public DateTime FromDate { get; set; }




        public DateTime ToDate { get; set; }


         public TaskState GetDataState { get; set; }


        public TaskState SendDataState { get; set; }


        public int TotalCount { get; set; }


        public int TransferedFromClientCount { get; set; }


        public int TransferedToMasterCount { get; set; }




        public DateTime CreationDate { get; set; }


        public DateTime? ModificationDate { get; set; }


        public DateTime? StartRecivingDateTime { get; set; }


        public DateTime? StartSendingDateTime { get; set; }



        public DateTime? FinishReciveDateTime { get; set; }


        public DateTime? FinishSendDateTime { get; set; }

        public DateTime? TriggerDateTime { get; set; }

    }
}
