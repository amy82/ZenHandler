using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public enum PickedProductState
    {
        Blank = 0,   // 제품 없음    
        BcrOk,
        Good,       // 양품
        BcrNg,         // 불량
        TestNg,
        Unknown     // 미확인 (필요 시)
    }
    public enum SocketProductState
    {
        Blank = 0,   // 제품 없음    
        Good,       // 양품
        NG,         // 불량
        Unknown     // 미확인 (필요 시)
    }
    public class ProductInfo
    {
        public int Index { get; set; }
        public PickedProductState State { get; set; } = PickedProductState.Blank;
        //public double[] Position { get; set; } = new double[3];

        public ProductInfo() { }  // <- 이게 필요해!
        public ProductInfo(int index)
        {
            Index = index;
        }
    }
    // 트랜스퍼나 피커가 들고 있는 제품 정보
    public class PickedProduct
    {
        public List<ProductInfo> LoadProductInfo { get; set; } = new List<ProductInfo>();
        public List<ProductInfo> UnLoadProductInfo { get; set; } = new List<ProductInfo>();
    }

    // 소켓 안에 있는 제품 상태 정보
    public class SocketProductInfo
    {
        public int SocketIndex { get; set; }
        public SocketProductState State { get; set; } = SocketProductState.Blank;
        public DateTime TimeInserted { get; set; } = DateTime.Now;

        public string BcrId { get; set; } = string.Empty;

        public SocketProductInfo(int socketIndex)
        {
            SocketIndex = socketIndex;
        }
    }
}