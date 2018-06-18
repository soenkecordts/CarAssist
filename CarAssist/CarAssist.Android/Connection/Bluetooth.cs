using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Bluetooth;
using CarAssist.Connection;
using CarAssist.Droid.Connection;
using CarAssist.Utils;
using Java.Util;

[assembly: Xamarin.Forms.Dependency(typeof(Bluetooth))]
namespace CarAssist.Droid.Connection
{
    public class Bluetooth : IBluetooth
    {
        private BluetoothAdapter _adapter = BluetoothAdapter.DefaultAdapter;
        private BluetoothDevice _device;
        private BluetoothSocket _socket;
        private Stream _reader, _writer;

        public bool IsConnected { get; private set; } = false;
        public bool IsEnabled => _adapter != null && _adapter.IsEnabled;
        public int RetryTime { get; set; } = 5000;
        public ILogger Logger { get; set; }
        public string Uuid { get; set; } = "00001101-0000-1000-8000-00805F9B34FB"; // Default: ELM327-Devices
        public string Address { get; set; }
        public ConnectionType ConnectionType => ConnectionType.Bluetooth;

        public BluetoothInfo[] PairedDevices
        {
            get
            {
                return BluetoothAdapter.DefaultAdapter?.BondedDevices
                    .Select((device) => new BluetoothInfo(device.Name, device.Address, device.BluetoothClass.DeviceClass.ToString(), device.Type.ToString()))
                    .ToArray();
            }
        }

        public async Task<bool> ConnectAsync()
        {
            await Task.Run(() =>
            {
                if (IsConnected)
                    Dispose();

                if (_adapter != null)
                {
                    try
                    {
                        _device = _adapter.GetRemoteDevice(Address);
                        if (_device != null)
                        {
                            _adapter.CancelDiscovery();

                            _socket = _device.CreateRfcommSocketToServiceRecord(UUID.FromString(Uuid));
                            if (_socket == null)
                                _socket = _device.CreateInsecureRfcommSocketToServiceRecord(UUID.FromString(Uuid));
                            if (_socket != null)
                                Logger?.WriteLine($"Address: {_socket.RemoteDevice.Address}, Name: {_socket.RemoteDevice.Name}, Paired: {_socket.RemoteDevice.BondState}");
                            _socket.Connect();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger?.WriteLine(ex);

                        IsConnected = false;
                        Dispose();
                        throw;
                    }
                    #region possibly activate
                    //catch (Java.IO.IOException)
                    //{
                    //    Java.Lang.Reflect.Method method = _device.Class.GetMethod(
                    //        "createRfcommSocket", new Java.Lang.Class[] { Java.Lang.Integer.Type });

                    //    // auf BT1 bis BT4 probieren
                    //    for (int channel = 1; channel <= 4; channel++)
                    //    {
                    //        try
                    //        {
                    //            Logger?.WriteLine($"Trying BT{channel}...");
                    //            _socket = (BluetoothSocket)method.Invoke(_device, channel);

                    //            _socket.Connect();
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            if (channel == 4)
                    //            {
                    //                Logger?.WriteLine(ex);

                    //                IsConnected = false;
                    //                Dispose();
                    //                throw;
                    //            }
                    //        }
                    //    }
                    //} 
                    #endregion

                    _reader = _socket.InputStream;
                    _writer = _socket.OutputStream;

                    IsConnected = true;
                }
            });
            return IsConnected;
        }

        public void Dispose()
        {
            _reader?.Close();
            _writer?.Close();
            _socket?.Close();
            _device?.Dispose();

            _reader = _writer = null;
            _socket = null;
            _device = null;

            IsConnected = false;
        }

        public string Read()
        {
            string response = null;

            if (_socket != null && _socket.IsConnected)
            {
                // data available
                if (!_reader.IsDataAvailable())
                {
                    // retry after 5 seconds
                    int iterations = RetryTime / 50;
                    for (int i = 1; i <= iterations; i++)
                    {
                        Task.Delay(50).Wait();
                        if (_reader.IsDataAvailable())
                        {
                            break;
                        }
                    }
                }

                // read data
                if (_reader.IsDataAvailable())
                {
                    int input;

                    response = "";
                    while ((input = _reader.ReadByte()) != -1)
                    {
                        response += (char)input;

                        if (((char)input) == '>')
                            break;
                    }
                    Logger?.WriteLine(response);
                }
                else
                {
                    Logger?.WriteLine("NO RESPONSE");
                }
            }
            return response;
        }

        public void Write(string message)
        {
            if (_socket != null && _socket.IsConnected)
            {
                var bytes = Encoding.ASCII.GetBytes(message);
                _writer.Write(bytes, 0, bytes.Length);
                _writer.Flush();

                Logger?.WriteLine(message);
            }
        }

        public string WriteAndRead(string message)
        {
            if (_socket != null && _socket.IsConnected)
            {
                Write(message);
                //await Task.Delay(200) or .Wait(), not neccessary
                return Read();
            }
            return null;
        }
    }
}