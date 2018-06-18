using System;

namespace CarAssist.Connection
{
    public interface ITcp : IConnection, IDisposable
    {
        int Port { get; set; }
    }
}
