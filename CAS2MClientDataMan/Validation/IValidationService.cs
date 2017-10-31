using CAS2MClientDataMan.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.Validation
{
    public interface IValidationService
    {
        IEnumerable<ErrorInput> F10Validate(IEnumerable<F10Record> records, string taskId);
        IEnumerable<ErrorInput> F30Validate(IEnumerable<F30Record> records, string taskId);
        IEnumerable<ErrorInput> F50Validate(IEnumerable<F50Record> records, string taskId);

    }
}
