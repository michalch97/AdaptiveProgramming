using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveProgrammingTrace
{
    [Export(typeof(ITrace))]
    public class TraceAP : ITrace
    {
        public void ErrorLog(string message, string source)
        {
            WriteLog(message, "ERROR", source);
        }

        public void WarningLog(string message, string source)
        {
            WriteLog(message, "WARNING", source);
        }

        public void InfoLog(string message, string source)
        {
            WriteLog(message, "INFO", source);
        }

        private void WriteLog(string message, string type, string source)
        {
            Trace.WriteLine(
                string.Format("{0} :: {1} :: {2} :: {3}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    type,
                    source,
                    message));
        }
    }
}
