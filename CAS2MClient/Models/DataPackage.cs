using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS2MClientDataMan.Enums;

namespace CAS2MClient.Models
{
    public class DataPackage<T>
    {
        public EntityType entityType;

        public Guid taskToken { get; set; }
        public Uri callbackUrl { get; set; }
        public List<T> data { get; set; }
    }
}