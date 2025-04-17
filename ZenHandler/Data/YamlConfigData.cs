using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Data
{
    public class _SerialPort
    {
        public string Bcr { get; set; }
    }

    public class _DrivingSettings
    {
        public bool IdleReportPass { get; set; }
        public bool EnableAutoStartBcr { get; set; }
        public bool PinCountUse { get; set; }
        public int PinCountMax { get; set; }
        public int CsvScanMonth { get; set; }
        public bool ImageGrabUse { get; set; }
        public string Language { get; set; }



    }
    public class ConfigData
    {
        public _SerialPort SerialPort { get; set; }
        public _DrivingSettings DrivingSettings { get; set; }
    }
}
