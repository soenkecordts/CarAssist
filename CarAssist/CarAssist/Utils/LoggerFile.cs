using System;
using System.IO;

namespace CarAssist.Utils
{
    public class LoggerFile : ILogger
    {
        public string ProtocolFile { get; set; }
        public bool Overwrite { get; set; }

        public LoggerFile(string file, bool deleteFile = true, bool overwrite = false)
        {
            if (File.Exists(file) == true && deleteFile == true)
                File.Delete(file);

            ProtocolFile = file;

            Overwrite = overwrite;
        }
        public void WriteLine(object obj)
        {
            File.AppendAllText(ProtocolFile, DateTime.Now.ToString() + ": " + obj.ToString());
        }
    }
}
