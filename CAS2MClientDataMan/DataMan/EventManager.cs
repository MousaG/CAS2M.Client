using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.DataMan
{
    public class EventManager
    {
        private System.Diagnostics.EventLog eventLog1;
        private string sSource = "Cas2mSender";
        private string sLog = "CAS2M TVNBANK Event Log";
        private static EventManager inst;
        private EventManager()
        {
            if (eventLog1 == null)
            {



                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);
                eventLog1 = new System.Diagnostics.EventLog(sLog);
                eventLog1.Source = sSource;

            }
        }
        public void WriteWarning(string message, int eventid)
        {
            eventLog1.WriteEntry(message, EventLogEntryType.Warning, eventid);
        }
        public void WriteInfo(string message, int eventid)
        {
            eventLog1.WriteEntry(message, EventLogEntryType.Information, eventid);
        }

        public void WriteError(string message, int eventid)
        {
            eventLog1.WriteEntry(message, EventLogEntryType.Error, eventid);
        }
        public void WriteError(string message, Exception ex, int eventid)
        {
            eventLog1.WriteEntry(message + "\r\n" + getFullMessage(ex), EventLogEntryType.Error, eventid);
        }
        public string getFullMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            while (ex != null)
            {
                sb.Append(ex.Message);
                if(ex.StackTrace !=null)
                {
                    sb.Append("\r\n");
                    sb.Append(ex.StackTrace);
                }
                ex = ex.InnerException;

            }
            return sb.ToString();
        }
        public static EventManager Inst
        {
            get
            {
                if (inst == null)
                    inst = new EventManager();
                return inst;

            }
        }

    }
}
