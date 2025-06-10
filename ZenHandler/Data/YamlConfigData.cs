using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Data
{
    public class _IpAddr
    {
        public string BcrIp { get; set; }
        public int BcrPort { get; set; }
    }

    public class _DrivingSettings
    {
        public bool PinCountUse { get; set; }
        public int PinCountMax { get; set; }
        public string Language { get; set; }
        public eDrivingMode drivingMode { get; set; }

    }
    public class ConfigData
    {
        public _IpAddr IpAddr { get; set; }
        public _DrivingSettings DrivingSettings { get; set; }
    }
}
