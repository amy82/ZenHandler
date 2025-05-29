using System;
using System.Collections.Generic;
using System.Drawing;
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
        TestNg2,
        TestNg3,
        TestNg4
    }
    public enum SocketProductState
    {
        Blank = 0,   // 제품 없음
        Test,        //검사 중
        Good,       // 양품
        NG         // 불량
    }
    public class ProductInfo
    {
        public int No { get; set; } = 0;
        public string BcrLot { get; set; } = "";
        public PickedProductState State { get; set; } = PickedProductState.Blank;

        public ProductInfo() { }  // <- 이게 필요해!

        public ProductInfo(int index)
        {
            No = index;
        }
    }
    public class TrayPoint
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
    }
    
    // 트랜스퍼나 피커가 들고 있는 제품 정보
    public class PickedProduct
    {
        public List<ProductInfo> LoadProductInfo { get; set; } = new List<ProductInfo>();       //TODO: 항상 4개가 돼야된다.
        public List<ProductInfo> UnLoadProductInfo { get; set; } = new List<ProductInfo>();

        public TrayPoint LoadTrayPos { get; set; } = new TrayPoint();       //투입, 배출 는 같이 쓰면되려나?  L,R 나눠야 될지?
        public TrayPoint UnloadTrayPos { get; set; } = new TrayPoint();
        public TrayPoint NgTrayPos { get; set; } = new TrayPoint();

        
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

        public int LeftTrayLayer { get; set; }
        public int RightTrayLayer { get; set; }
    }



    //---------------------------------------------------------------------------------------------------------------------------------------------------
    //
    // MAGAZINE UNIT
    //
    //
    public enum LayerState
    {
        //Blank = 0,      //Tray 없음
        Before = 0,         //검사 전
        After          //검사 완료
        //Test         //검사 중      <---하나의 tray 만 적용돼야된다.
    }
    public class MagazineInfo
    {
        public int Index { get; set; }      //위에서 부터 0
        public LayerState State { get; set; } = LayerState.Before;

        public MagazineInfo() { }  // <- 이게 없으면 yaml 로드 안됨
        public MagazineInfo(int No)
        {
            Index = No;
        }
    }
    public class MagazineTray
    {
        public List<MagazineInfo> LeftMagazineInfo { get; set; } = new List<MagazineInfo>();
        public List<MagazineInfo> RightMagazineInfo { get; set; } = new List<MagazineInfo>();
        public int LeftTrayLayer { get; set; } = 0;     //TODO: Tray 로드 상태에선 작업자 변경불가하도록 하자
        public int RightTrayLayer { get; set; } = 0;    //TODO: Tray 로드 상태에선 작업자 변경불가하도록 하자
    }


    //---------------------------------------------------------------------------------------------------------------------------------------------------
    //
    // SOCKET UNIT
    //
    //
    // 소켓 안에 있는 제품 상태 정보
    public class SocketProductInfo
    {
        public int No { get; set; }
        public SocketProductState State { get; set; } = SocketProductState.Blank;
        public string BcrLot { get; set; } = "Empty";

        public SocketProductInfo() { }  // <- 이게 없으면 yaml 로드 안됨
        public SocketProductInfo(int index)
        {
            No = index;
        }
    }
    public class AoiSocketProduct
    {
        public List<SocketProductInfo> SocketInfo_A { get; set; } = new List<SocketProductInfo>();
        public List<SocketProductInfo> SocketInfo_B { get; set; } = new List<SocketProductInfo>();
    }
    public class EEpromSocketProduct
    {
        public List<SocketProductInfo> SocketInfo_A { get; set; } = new List<SocketProductInfo>();
        public List<SocketProductInfo> SocketInfo_B { get; set; } = new List<SocketProductInfo>();
    }
    public class SocketProduct//FwSocketProduct
    {
        public List<SocketProductInfo> SocketInfo_A { get; set; } = new List<SocketProductInfo>();      //aoi , eeprom
        public List<SocketProductInfo> SocketInfo_B { get; set; } = new List<SocketProductInfo>();      //aoi , eeprom
        public List<SocketProductInfo> SocketInfo_C { get; set; } = new List<SocketProductInfo>();
        public List<SocketProductInfo> SocketInfo_D { get; set; } = new List<SocketProductInfo>();      //fw all
    }
}