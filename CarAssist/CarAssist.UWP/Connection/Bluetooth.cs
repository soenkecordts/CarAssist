using System;
using System.Threading.Tasks;
using CarAssist.Connection;
using CarAssist.UWP.Connection;
using CarAssist.Utils;

[assembly: Xamarin.Forms.Dependency(typeof(Bluetooth))]
namespace CarAssist.UWP.Connection
{
    public class Bluetooth : IBluetooth
    {
        public bool IsConnected { get; private set; } = false;
        public bool IsEnabled => false;
        public int RetryTime { get; set; } = 5000;
        public ILogger Logger { get; set; }
        public string Uuid { get; set; } = "00001101-0000-1000-8000-00805F9B34FB"; // Default: ELM327-Devices
        public string Address { get; set; }
        public ConnectionType ConnectionType => ConnectionType.Bluetooth;


        public BluetoothInfo[] PairedDevices
        {
            get
            {
                return null;
            }
        }
        public async Task<bool> ConnectByNameAsync(string name)
        {
            await Task.Run(() =>
            {
                throw new NotImplementedException();
            });
            return false;
        }

        public async Task<bool> ConnectAsync()
        {
            await Task.Run(() =>
            {
                throw new NotImplementedException();
            });
            return false;
        }

        public void Dispose()
        {
        }

        public string Read()
        {
            throw new NotImplementedException();
        }

        public void Write(string message)
        {
            throw new NotImplementedException();
        }

        public string WriteAndRead(string message)
        {
            throw new NotImplementedException();
        }
    }
}