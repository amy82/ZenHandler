using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace ZenHandler.Serial
{
    public class SerialCommunicator
    {
        private SerialPort _serialPort;
        public string myName { get; set; }

        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public Parity Parity { get; set; }

        //public event Action<string> BarcodeScanned;

        public enum BaudRates
        {
            Baud1200 = 1200,
            Baud2400 = 2400,
            Baud4800 = 4800,
            Baud9600 = 9600,
            Baud19200 = 19200,
            Baud38400 = 38400,
            Baud57600 = 57600,
            Baud115200 = 115200
        }
        public SerialCommunicator(string portName)
        {
            //, int dataBits = 8, StopBits stopBits = StopBits.One, Parity parity = Parity.None
            this.PortName = portName;
            this.BaudRate = (int)BaudRates.Baud9600;
            this.DataBits = 8;
            this.StopBits = StopBits.One;
            this.Parity = Parity.None;

            _serialPort = new SerialPort    //(PortName, BaudRate, Parity, DataBits, StopBits)
            {
                PortName = this.PortName,
                BaudRate = this.BaudRate,
                DataBits = this.DataBits,
                StopBits = this.StopBits,
                Parity = this.Parity,
                Encoding = Encoding.ASCII,   // 기본 인코딩 설정
                NewLine = "\r\n",            // 종료 문자를 설정 (Enter)
                DtrEnable = true,           // Data Terminal Ready 설정 (옵션)
                RtsEnable = true            // Request to Send 설정 (옵션)
            };



            // 2. 데이터 수신 이벤트 핸들러 등록
            _serialPort.DataReceived += SerialPort_DataReceived;
        }
        // 바코드 스캔 발생 시 이벤트 호출
        public void SimulateScan(string barcodeData)
        {
            ///BarcodeScanned?.Invoke(barcodeData);
        }
        // 6. 바코드 데이터 수신 이벤트 처리
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string rawData = _serialPort.ReadExisting(); // 버퍼에 있는 모든 데이터 읽기
                //string scanData = rawData.Trim(); // 불필요한 개행 문자 제거
                string scanData = rawData.Replace("\r", "").Replace("\n", ""); // \r\n 제거


                ///string data = _serialPort.ReadLine();       //<--\r\n 붙을 경우 못 빠져나온다.
                Console.WriteLine("바코드 데이터: " + scanData);
                string logData = $"[Bcr] Scan Data:{scanData}";

                Globalo.LogPrint("Serial", logData);


                Globalo.productionInfo.BcrSet(scanData);


                _serialPort.DiscardInBuffer(); // 입력 버퍼를 비웁니다.

            }
            catch (Exception ex)
            {
                Console.WriteLine("데이터 수신 오류: " + ex.Message);
            }
        }
        // SerialPort 열기
        public bool Open()
        {
            string logData = "";
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                    _serialPort.DiscardInBuffer();  // 연결 전에 버퍼 비우기

                    // 비동기적으로 데이터를 받기 위한 설정
                    //_serialPort.DataReceived += (sender, e) =>
                    //{
                    //    // 데이터 받기 전에 Invoke로 UI 스레드로 전환
                    //    string receivedData = _serialPort.ReadLine();  // 한 줄씩 받기
                    //    OnDataReceived(receivedData);
                    //};
                    //string data = "]011";       //power on
                    // _serialPort.WriteLine(data);

                    //Thread.Sleep(300);
                    //int channel = 1;
                    //int value = 255;

                    //// 채널과 값을 문자열로 변환하여 합치기
                    //data = "[" + channel.ToString("D2") + value.ToString("D3");

                    //_serialPort.WriteLine(data);


                    logData = $"[Serial] Bcr Connect Ok  [{PortName}/{BaudRate}]";
                    Globalo.LogPrint("SerialConnect", logData);
                    return true;
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine("에러: " + ex.Message);
                    
            }
            //finally
            //{
            //    string data = "]011";       //power on
            //    _serialPort.WriteLine(data);
            //}

            logData = $"[Serial] Bcr Connect Fail  {PortName} / {BaudRate}";
            Globalo.LogPrint("SerialConnect", logData);
            return false;

        }
        // DataReceived 이벤트를 UI 스레드에서 안전하게 처리하기 위한 방법
        protected virtual void OnDataReceived(string data)
        {
            if (DataReceived != null)
            {
                if (this.DataReceived.GetInvocationList().Length > 0)
                {
                    // UI 스레드에서 호출하도록 처리 (InvokeRequired)
                    if (this.DataReceived.Method.IsStatic || this.DataReceived.Target is Control)
                    {
                        // Invoke를 사용하여 UI 스레드에서 호출하도록 한다
                        this.DataReceived.Invoke(this, data);
                    }
                    else
                    {
                        DataReceived?.Invoke(this, data);
                    }
                }
            }
        }
        // SerialPort 닫기
        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.DiscardInBuffer(); // 버퍼 비우기
                _serialPort.Close();
                Console.WriteLine("Serial Port closed.");
            }
        }

        // 데이터 전송
        public void SendData(string data)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.WriteLine(data);
                Console.WriteLine("Sent: " + data);
            }
            else
            {
                Console.WriteLine("Serial port is not open.");
            }
        }
        // 데이터 수신 이벤트
        public event EventHandler<string> DataReceived;

        // 데이터 수신 받기 (비동기 처리)
        //public void StartReceiving()
        //{
        //    if (_serialPort.IsOpen)
        //    {
        //        _serialPort.DataReceived += (sender, e) =>
        //        {
        //            string receivedData = _serialPort.ReadLine();  // 한 줄씩 받기
        //            DataReceived?.Invoke(this, receivedData);      // 이벤트 호출
        //        };
        //    }
        //    else
        //    {
        //        Console.WriteLine("Serial port is not open.");
        //    }
        //}
    }
}
