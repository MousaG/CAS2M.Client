using CAS2MClientDataMan.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.Domain
{
    public class ErrorInput
    {
        public string Id { get; set; }

        public string Tittle { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? ModificationDate { get; set; }

        public ErrorType ErrorType { get; set; }

        public string TaskId { get; set; }
    }
}
