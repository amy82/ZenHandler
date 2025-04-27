using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public class OffsetInfo
    {
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
    }
    public class GapInfo
    {
        public double GapX { get; set; }
        public double GapY { get; set; }
    }
    public class ProductLayout
    {
        public List<OffsetInfo> LoadTrayOffset { get; set; } = new List<OffsetInfo>();
        public List<OffsetInfo> UnLoadTrayOffset { get; set; } = new List<OffsetInfo>();
        public List<OffsetInfo> LoadSocketOffset { get; set; } = new List<OffsetInfo>();
        public List<OffsetInfo> UnLoadSocketOffset { get; set; } = new List<OffsetInfo>();
        public List<OffsetInfo> UnLoadNgOffset { get; set; } = new List<OffsetInfo>();

        public GapInfo TrayGap { get; set; } = new GapInfo();
        public GapInfo SocketGap { get; set; } = new GapInfo();
    }
}
