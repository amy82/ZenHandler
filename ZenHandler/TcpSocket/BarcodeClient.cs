using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.TcpSocket
{
    public class BarcodeClient
    {
        private NetworkStream _stream;
        private TcpClient bcrSocket;
        public event Action<string> DataReceived;
        public event Action<string> ErrorOccurred;
        private Thread BcrreceiveThread;

        public BarcodeClient()
        {
            bcrSocket = new TcpClient();
            DataReceived += OnBcrReceived;
            ErrorOccurred += OnBcrError;
        }
        private void OnBcrReceived(string data)
        {
            Console.WriteLine($"OnBcrReceived : {data}");
        }
        private void OnBcrError(string message)
        {
            Console.WriteLine($"OnBcrError : {message}");
        }
        public bool Connect(string ipAddress, int port)
        {
            try
            {
                bcrSocket = new TcpClient();
                bcrSocket.Connect(ipAddress, port);
                _stream = bcrSocket.GetStream();

                BcrreceiveThread = new Thread(ReceiveLoop);
                BcrreceiveThread.IsBackground = true;
                BcrreceiveThread.Start();

                Console.WriteLine($"[BCR] Connect OK {ipAddress},{port}");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"연결 실패: {ex.Message}");
                Console.WriteLine($"[BCR] Connect Fail");
                return false;
            }

            return true;
        }
        public void Send(string message)
        {
            try
            {
                if (bcrSocket?.Connected == true)
                {
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    _stream.Write(data, 0, data.Length);
                    Console.WriteLine($"[BCR] Send: {message}");
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"[BCR 송신 오류] {ex.Message}");
                Console.WriteLine($"[BCR] Send Fail");
            }
        }
        public bool Disconnect()
        {
            try
            {
                BcrreceiveThread?.Abort(); // Thread 강제 종료
                _stream?.Close();
                bcrSocket?.Close();

            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"해제 실패: {ex.Message}");
                return false;
            }
            return true;
        }
        private void ReceiveLoop()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (bcrSocket.Connected)
                {
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) continue;

                    string received = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    DataReceived?.Invoke(received.Trim());
                    
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"수신 오류: {ex.Message}");
            }
        }
    }
}
