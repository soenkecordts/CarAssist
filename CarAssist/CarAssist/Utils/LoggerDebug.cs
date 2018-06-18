using System.Diagnostics;

namespace CarAssist.Utils
{
    public class LoggerDebug : ILogger
    {
        public LoggerDebug(bool overwrite = true)
        {
            Overwrite = overwrite;
        }

        public bool Overwrite { get; set; }

        public void WriteLine(object obj)
        {
            Debug.WriteLine(obj);
        }
    }
}
