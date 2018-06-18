using System;

namespace CarAssist.Obd
{
    public class ObdException : Exception
    {
        public ObdException(string message) : base(message)
        {
        }
    }
}
