using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.TcpSocket
{
    public class TcpServer
    {
        private TcpListener _listener;
        //private bool _isRunning;
        private bool bConnected;

        private readonly List<TcpClient> _clientsList = new List<TcpClient>();
        //public event Action<string> OnMessageReceived; // 메시지 수신 이벤트
        public event Func<string, Task> OnMessageReceivedAsync; // 비동기 이벤트

        public TcpServer(string ip, int port)
        {
            bConnected = false;
            _listener = new TcpListener(IPAddress.Parse(ip), port);

            string logData = $"[tcp] Server Create:{ip} / {port}";
            Globalo.LogPrint("CCdControl", logData);
        }
        public bool bClientConnectedState()
        {
            return bConnected;
        }
        // 🎯 **클라이언트로 메시지 보내는 함수**
        public async Task SendMessageAsync(TcpClient client, string message)
        {
            if(_clientsList.Count < 1)
            {
                return;
            }
            try
            {
                //TcpClient client = _clientsList[0];
                if (client != null && client.Connected)
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await client.GetStream().WriteAsync(data, 0, data.Length);

                    //Console.WriteLine($"클라이언트에게 전송: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"메시지 전송 오류: {ex.Message}");
            }
        }
        // 🎯 **모든 클라이언트에게 메시지 보내는 함수**
        public async Task BroadcastMessageAsync(string message)
        {
            List<TcpClient> disconnectedClients = new List<TcpClient>();

            foreach (var client in _clientsList)
            {
                if (client.Connected)
                {
                    await SendMessageAsync(client, message);
                }
                else
                {
                    disconnectedClients.Add(client); // 연결 끊긴 클라이언트 목록 저장
                }
            }

            // 연결 끊긴 클라이언트 제거
            foreach (var client in disconnectedClients)
            {
                _clientsList.Remove(client);
            }
        }
        // 서버 시작
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _listener.Start();
            Console.WriteLine("서버가 시작되었습니다.");
            string logData = $"[tcp] Server Start";
            Globalo.LogPrint("CCdControl", logData);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (_listener.Pending()) // 대기 중인 연결이 있는지 확인
                    {
                        TcpClient client = await _listener.AcceptTcpClientAsync();
                        _clientsList.Add(client); // 클라이언트 추가

                        Console.WriteLine("클라이언트가 연결되었습니다.");

                        bConnected = true;

                        logData = $"[tcp] Client Connected";
                        Globalo.LogPrint("CCdControl", logData);
                        Globalo.MainForm.ClientConnected(true);
                        _ = HandleClientAsync(client, cancellationToken); // 클라이언트 연결 처리
                    }
                    await Task.Delay(100); // CPU 점유율을 낮추기 위해 약간의 대기
                }
            }
            catch (Exception ex)
            {
                bConnected = false;
                Console.WriteLine($"서버 예외 발생: {ex.Message}");
            }
        }
        
        // 클라이언트 연결 처리
        private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                StringBuilder sb = new StringBuilder(); // 여러 개의 JSON 조각을 합치기 위한 StringBuilder
                try
                {
                    //while (true) // 연결이 유지되는 동안 계속 읽음
                    while (client.Connected)
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

                        // 서버에서 연결을 종료하면 종료
                        if (bytesRead == 0)
                        {
                            Console.WriteLine("서버와의 연결이 종료되었습니다.");
                            break;
                        }

                        string receivedChunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        sb.Append(receivedChunk); // JSON 조각을 합침

                        // JSON이 닫혔는지 확인 (한 개의 JSON이 완성되었는지 확인)
                        if (receivedChunk.TrimEnd().EndsWith("}"))
                        {
                            string receivedData = sb.ToString();
                            sb.Clear(); // StringBuilder 초기화 (다음 JSON 수신을 위해)

                            // 메시지 수신 이벤트 호출
                            //OnMessageReceived?.Invoke(receivedData);
                            // ✅ 메시지 수신 시 비동기 이벤트 호출
                            if (OnMessageReceivedAsync != null)
                            {
                                await OnMessageReceivedAsync.Invoke(receivedData);
                            }
                        }
                    }
                    //while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                    //{
                    //    string receivedChunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    //    sb.Append(receivedChunk); // JSON 조각을 합침

                    //    // JSON이 닫히는지 확인 (마지막 문자가 '}'로 끝나는지)
                    //    if (receivedChunk.TrimEnd().EndsWith("}"))
                    //        break;
                    //}

                    //string receivedData = sb.ToString();
                    //OnMessageReceived?.Invoke(receivedData); // 메시지 수신 이벤트 호출

                    //while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                    //{
                    //    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    //    Console.WriteLine($"수신 메시지: {receivedData}");

                    //    OnMessageReceived?.Invoke(receivedData); // 메시지 수신 이벤트 호출
                    //}
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"클라이언트 처리 중 예외 발생: {ex.Message}");
                }
            }
            bConnected = false;
            string logData = $"[tcp] Client DisConnected";
            Globalo.LogPrint("CCdControl", logData);
            Globalo.MainForm.ClientConnected(false);
            Console.WriteLine("클라이언트 연결이 종료되었습니다.");
        }

        // 서버 중지
        public void Stop()
        {
            _listener.Stop();
            
            Console.WriteLine("서버가 중지되었습니다.");
        }
    }
}
