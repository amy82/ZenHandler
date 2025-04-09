using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Data
{
    public class _CHART
    {
        public int ChartCount { get; set; }
        public List<int> SfrPosX { get; set; }
        public List<int> SfrPosY { get; set; }
        public List<int> SfrSizeX { get; set; }
        public List<int> SfrSizeY { get; set; }
        public List<int> CirClePosX { get; set; }
        public List<int> CirClePosY { get; set; }
        public List<int> CirCleSizeX { get; set; }
        public List<int> CirCleSizeY { get; set; }
    }
    public class ImageData
    {
        public _CHART chartData { get; set; }
    }
}
