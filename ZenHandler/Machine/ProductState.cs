using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    //---------------------------------------------------------------------------------------------------------------------------------------------------
    //
    // TRANSFER UNIT
    //
    //
    public enum UnitPicker
    {
        LOAD = 0,
        UNLOAD
    }
    public enum PickedProductState
    {
        Blank = 0,   // 제품 없음    
        Bcr,
        Good,       // 양품
        BcrNg,      // 불량
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
        public int Index { get; set; } = 0;
        public string BcrLot { get; set; } = "";
        public PickedProductState State { get; set; } = PickedProductState.Blank;

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
    //---------------------------------------------------------------------------------------------------------------------------------------------------
    //
    // LIFT UNIT
    //
    //

    public class TrayProduct
    {
        public List<List<int>> LeftLoadTraySlots { get; set; } = new List<List<int>>();
        public List<List<int>> RightLoadTraySlots { get; set; } = new List<List<int>>();
        public List<List<int>> LeftNgTraySlots { get; set; } = new List<List<int>>();
        public List<List<int>> RightNgTraySlots { get; set; } = new List<List<int>>();
    }



    //---------------------------------------------------------------------------------------------------------------------------------------------------
    //
    // MAGAZINE UNIT
    //
    //
    public enum LayerState
    {
        Blank = 0,   // Tray 없음
        Disabled,   //사용 못함
        BeforeTest, //검사 전
        AfterTest,  //검사 완료
        Inspecting, //검사 중      <---하나의 tray 만 적용돼야된다.
        Unknown     // 미확인 (필요 시)
    }
    public class MagazineInfo
    {
        public int Index { get; set; }      //위에서 부터 0
        public LayerState State { get; set; } = LayerState.Blank;

        public MagazineInfo() { }  // <- 이게 없으면 yaml 로드 안됨
        public MagazineInfo(int index)
        {
            Index = index;
        }
    }
    public class MagazineTray
    {
        MagazineInfo LeftMagazineInfo { get; set; } = new MagazineInfo();
        MagazineInfo RightMagazineInfo { get; set; } = new MagazineInfo();
    }


    //---------------------------------------------------------------------------------------------------------------------------------------------------
    //
    // SOCKET UNIT
    //
    //
    // 소켓 안에 있는 제품 상태 정보
    public class SocketProductInfo
    {
        public int SocketIndex { get; set; }
        public SocketProductState State { get; set; } = SocketProductState.Blank;
        public DateTime TimeInserted { get; set; } = DateTime.Now;

        public string BcrId { get; set; } = "Empty";

        public SocketProductInfo(int socketIndex)
        {
            SocketIndex = socketIndex;
        }
    }
}