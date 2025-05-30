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
    public enum ClientSlotIndex
    {
        Tester1 = 0,    // IP 뒷자리 1
        Tester2 = 1,    // IP 뒷자리 2
        Tester3 = 2,    // IP 뒷자리 3
        Tester4 = 3,    // IP 뒷자리 4
        Tester5 = 4,    // IP 뒷자리 5
        Tester6 = 5,    // IP 뒷자리 6
        Tester7 = 6,    // IP 뒷자리 7
        Tester8 = 7,    // IP 뒷자리 8

        SecsGem = 10     // IP 뒷자리 100 → 배열엔 5개니까 마지막 인덱스 4
    }
    public class TcpServer
    {
        private TcpListener _listener;
        //private bool _isRunning;
        private bool bConnected;

        //private readonly List<TcpClient> _clientsList = new List<TcpClient>();
        //private Dictionary<int, TcpClient> _clientMap = new Dictionary<int, TcpClient>();
        private readonly TcpClient[] _clients = new TcpClient[5];
        private readonly Dictionary<int, ClientSlotIndex> ipToSlotIndex = new Dictionary<int, ClientSlotIndex>
        {
            { 1, ClientSlotIndex.Tester1 },
            { 2, ClientSlotIndex.Tester2 },
            { 3, ClientSlotIndex.Tester3 },
            { 4, ClientSlotIndex.Tester4 },
            { 5, ClientSlotIndex.Tester5 },
            { 6, ClientSlotIndex.Tester6 },
            { 7, ClientSlotIndex.Tester7 },
            { 8, ClientSlotIndex.Tester8 },
            { 100, ClientSlotIndex.SecsGem }
        };
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
            if(_clients.Length < 1)
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
            List<int> disconnectedClientKeys = new List<int>();

            for (int i = 0; i < _clients.Length; i++)
            {
                var client = _clients[i];
                if (client != null && client.Connected)
                {
                    await SendMessageAsync(client, message);
                }
                else
                {
                    // 연결 끊긴 클라이언트 처리
                    _clients[i] = null;
                }
            }
            //foreach (var kvp in _clientMap) // Key: 클라이언트 ID (ex. IP 뒷자리), Value: TcpClient
            //{
            //    int key = kvp.Key;
            //    TcpClient client = kvp.Value;

            //    if (client.Connected)
            //    {
            //        await SendMessageAsync(client, message);
            //    }
            //    else
            //    {
            //        disconnectedClientKeys.Add(key); // 연결 끊긴 클라이언트 key 저장
            //    }
            //}

                // 연결 끊긴 클라이언트 제거
            //foreach (int key in disconnectedClientKeys)
            //{
            //    _clientMap.Remove(key);
            //}
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

                        // 클라이언트 IP 주소 가져오기
                        IPEndPoint remoteIpEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                        string clientIP = remoteIpEndPoint?.Address.ToString();

                        // 출력
                        
                        // IP 주소의 마지막 자리 추출
                        int lastOctet = -1;
                        if (!string.IsNullOrEmpty(clientIP))
                        {
                            string[] parts = clientIP.Split('.');
                            if (parts.Length == 4 && int.TryParse(parts[3], out int parsed))
                            {
                                lastOctet = parsed;
                                Console.WriteLine($"Client Connect IP: {clientIP},ID: {lastOctet}");
                            }
                        }
                        //_clientsList.Add(client); // 클라이언트 추가
                        //// _clientMap[lastOctet] = client;
                        // 클라이언트 접속 시 (IP 뒷자리 lastOctet)
                        int clientNo = -1;
                        if (ipToSlotIndex.TryGetValue(lastOctet, out ClientSlotIndex slot))
                        {
                            clientNo = (int)slot;
                            _clients[(int)slot] = client; // 배열 인덱스에 저장
                        }
                        else
                        {
                            // 정의되지 않은 IP 뒷자리 처리
                        }

                        if(clientNo == -1)
                        {
                            return;
                        }
                        
                        bConnected = true;

                        logData = $"[tcp] Client Connected";
                        Globalo.LogPrint("CCdControl", logData);

                        if (clientNo == (int)ClientSlotIndex.SecsGem)
                        {
                            Globalo.MainForm.ClientConnected(true);
                        }

                        _ = HandleClientAsync(client, clientNo, cancellationToken); // 클라이언트 연결 처리
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
        private async Task HandleClientAsync(TcpClient client, int clientIndex, CancellationToken cancellationToken)
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

            // 클라이언트 연결 종료 시 배열에서 null 처리 및 소켓 닫기
            if (_clients[clientIndex] == client)
            {
                client.Close();
                _clients[clientIndex] = null;
                Console.WriteLine($"클라이언트 인덱스 {clientIndex} 연결 종료, 배열에서 제거");
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
