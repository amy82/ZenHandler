using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Serial
{
    public class SerialPortManager
    {
        public SerialCommunicator Barcode;

        private List<SerialCommunicator> _serialPorts;

        public SerialPortManager()
        {
            _serialPorts = new List<SerialCommunicator>();
        }
        // 데이터 수신 처리 (이벤트 핸들러 등록)
        public void StartReceivingFromAllPorts()
        {
            //foreach (var serialPort in _serialPorts)
            //{
            //    serialPort.DataReceived += (sender, data) =>
            //    {
            //        Console.WriteLine("Data received from port " + ((SerialCommunicator)sender).PortName + ": " + data);
            //    };
            //    serialPort.StartReceiving();
            //}
        }

        // SerialPort 추가
        public void AddSerialPort(SerialCommunicator communicator)
        {
            _serialPorts.Add(communicator);
            Console.WriteLine("SerialPort added: " + communicator.PortName);
        }
        // SerialPort 열기
        public void OpenAllPorts()
        {
            //foreach (var serialPort in _serialPorts)
            //{
            //    serialPort.Open();
            //}
        }

        // SerialPort 닫기
        public void CloseAllPorts()
        {
            foreach (var serialPort in _serialPorts)
            {
                serialPort.Close();
            }
        }

        // 데이터 전송 (모든 SerialPort로)
        public void SendDataToAllPorts(string data)
        {
            //foreach (var serialPort in _serialPorts)
            //{
            //    serialPort.SendData(data);
            //}
        }
    }
}
