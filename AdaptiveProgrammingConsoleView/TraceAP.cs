using System;
using System.Diagnostics;
using System.Diagnostics.Eventing;

namespace AdaptiveProgrammingConsoleView
{
    public static class TraceAP
    {
        public static void ErrorLog(string message, string source)
        {
            WriteLog(message, "ERROR", source);
        }

        public static void WarningLog(string message, string source)
        {
            WriteLog(message, "WARNING", source);
        }

        public static void InfoLog(string message, string source)
        {
            WriteLog(message, "INFO", source);
        }

        private static void WriteLog(string message, string type, string source)
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