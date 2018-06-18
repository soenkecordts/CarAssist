using CarAssist.Connection;
using System;
using System.Collections.Generic;

namespace CarAssist.Obd
{
    public class ObdConnection : IDisposable
    {
        public ObdCommand Command { get; private set; }
        public IConnection Connection { get; private set; }
        public IList<string> ECUs { get; private set; }

        public ObdConnection(IConnection connection)
        {
            Connection = connection;
            Command = new ObdCommand(connection);
        }

        public IList<string> Open()
        {
            if (Command.Open() == true)
                return Command.ECUs;

            return null;
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
