using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Data
{
    public class _TaskData
    {
        public LOTDATA LotData;
        public PRODUCTION_INFO ProductionInfo;
        public int PintCount;
    }

    public class LOTDATA
    {
        public string BarcodeData { get; set; }
    }
    public class PRODUCTION_INFO
    {
        public int OkCount { get; set; }
        public int NgCount { get; set; }
        public int TotalCount { get; set; }
    }
}
