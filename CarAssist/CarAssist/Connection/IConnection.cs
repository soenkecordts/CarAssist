using CarAssist.Utils;
using System;
using System.Threading.Tasks;

namespace CarAssist.Connection
{
    public enum ConnectionType
    {
        TCP, Bluetooth
    }

    public interface IConnection : IDisposable
    {
        bool IsConnected { get; }
        bool IsEnabled { get; }
        ILogger Logger { get; set; }
        int RetryTime { get; set; }
        string Address { get; set; }
        ConnectionType ConnectionType { get; }
        Task<bool> ConnectAsync();
        string Read();
        void Write(string message);
        string WriteAndRead(string message);
    }
}
