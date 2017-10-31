using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.Enums
{
    public enum EntityType
    {
        F10 = 10,//F10
        F30 = 30,//F30
        F50 = 50,//F50
        F60 = 60,//F60
        F70 = 70,//F70
    }
    public enum TaskState
    {
        InQueue = 1,
        InProgress = 2,
        Done = 3,
        WithError = 4,
    }
    public enum ErrorType
    {
  
        System = 1,
        Validation = 2,


    }
    public enum ErrorSortBy
    {
        Title = 1,
        CreateDate = 2,
    }
}
