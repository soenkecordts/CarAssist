using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CarAssist.Utils;

namespace CarAssist.Connection
{
    public class Tcp : ITcp
    {
        TcpClient _client;

        Stream _reader;
        Stream _writer;

        public bool IsConnected { get; private set; } = false;
        public bool IsEnabled => _client != null;
        public int RetryTime { get; set; } = 5000;
        public ILogger Logger { get; set; }
        public int Port { get; set; } = 35000;
        public string Address { get; set; } = "192.168.0.10";
        public ConnectionType ConnectionType => ConnectionType.TCP;

        public async Task<bool> ConnectAsync()
        {
            await Task.Run(() =>
            {
                if (IsConnected)
                    Dispose();

                try
                {
                    _client = new TcpClient(Address, Port);

                    Logger?.WriteLine($"Local address: {_client.Client.LocalEndPoint}, Remote address: {_client.Client.RemoteEndPoint}");

                    _reader = _client.GetStream();
                    _writer = _client.GetStream();
                }
                catch (Exception ex)
                {
                    Logger?.WriteLine(ex);

                    IsConnected = false;
                    Dispose();
                    throw;
                }
                IsConnected = true;
            });
            return IsConnected;
        }

        public void Dispose()
        {
            _reader?.Close();
            _writer?.Close();
            _client?.Close();

            _reader = _writer = null;
            _client = null;
            Address = null;
            IsConnected = false;
        }

        public string Read()
        {
            string response = null;

            if (_client != null && _client.Connected)
            {
                // data available
                if (_client.Available <= 0)
                {
                    // retry after 5 seconds
                    int iterations = RetryTime / 50;
                    for (int i = 1; i <= iterations; i++)
                    {
                        Task.Delay(50).Wait();
                        if (_client.Available > 0)
                        {
                            break;
                        }
                    }
                }

                // read data
                if (_client.Available > 0)
                {
                    int input;

                    response = "";
                    while((input = _reader.ReadByte()) != -1)
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
            if (_client != null && _client.Connected)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(message);

                _writer.Write(bytes, 0, bytes.Length);
                _writer.Flush();

                Logger?.WriteLine(message);
            }
        }

        public string WriteAndRead(string message)
        {
            if (_client != null && _client.Connected)
            {
                Write(message);
                //await Task.Delay(200) or .Wait(), not neccessary
                return Read();
            }
            return null;
        }
    }
}