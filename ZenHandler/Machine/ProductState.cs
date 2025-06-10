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
        Bcr,        //투입할 제품 로드 상태
        Good,       // 양품
        BcrNg,      // 불량
        TestNg1,        //aoi , fw , write
        TestNg2         //verify
    }
    public enum FwProductState
    {
        Blank = 0,   // 제품 없음
        Testing,     //fw 다운전
        Good,        // 양품
        NG           // 불량
    }
    public enum EEpromProductState
    {
        Blank = 0,   // 제품 없음
        Writing,     //Write 검사 전
        Verifying,   //Verify 검사 전
        Good,        // Write + Verify 둘다 완료
        NG_Write,           // 불량
        NG_Verify
    }
    public enum AoiSocketProductState
    {
        Blank = 0,   // 제품 없음
        Testing,     // Aoi 검사 전
        Good,        // 양품
        NG           // 불량
    }

    public class ProductInfo
    {
        public int No { get; set; } = 0;
        public string BcrLot { get; set; } = "";
        public PickedProductState State { get; set; } = PickedProductState.Blank;

        public FwProductState FwResultState { get; set; } = FwProductState.Blank;
        public EEpromProductState EEpromResultState { get; set; } = EEpromProductState.Blank;
        public AoiSocketProductState AoiResultState { get; set; } = AoiSocketProductState.Blank;
        public ProductInfo() { }  // <- 이게 필요해!
        public ProductInfo(int index)
        {
            No = index;
        }
        public ProductInfo Clone()
        {
            return new ProductInfo
            {
                No = this.No,
                BcrLot = this.BcrLot,
                State = this.State
            };
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
    public class AoiSocketProductInfo
    {
        public int No { get; set; }
        public AoiSocketProductState State { get; set; } = AoiSocketProductState.Blank;
        public string BcrLot { get; set; } = "Empty";

        public AoiSocketProductInfo() { }  // <- 이게 없으면 yaml 로드 안됨
        public AoiSocketProductInfo(int index)
        {
            No = index;
        }

        public AoiSocketProductInfo Clone()
        {
            return new AoiSocketProductInfo
            {
                No = this.No,
                State = this.State,
                BcrLot = this.BcrLot
            };
        }
    }
    

    public class EEpromSocketProductInfo
    {
        public int No { get; set; }
        public EEpromProductState State { get; set; } = EEpromProductState.Blank;
        public string BcrLot { get; set; } = "Empty";

        public EEpromSocketProductInfo() { }  // <- 이게 없으면 yaml 로드 안됨
        public EEpromSocketProductInfo(int index)
        {
            No = index;
        }

        public EEpromSocketProductInfo Clone()
        {
            return new EEpromSocketProductInfo
            {
                No = this.No,
                State = this.State,
                BcrLot = this.BcrLot
            };
        }
    }

    public class FwSocketProductInfo
    {
        public int No { get; set; }
        public FwProductState State { get; set; } = FwProductState.Blank;
        public string BcrLot { get; set; } = "Empty";

        public FwSocketProductInfo() { }  // <- 이게 없으면 yaml 로드 안됨
        public FwSocketProductInfo(int index)
        {
            No = index;
        }

        public FwSocketProductInfo Clone()
        {
            return new FwSocketProductInfo
            {
                No = this.No,
                State = this.State,
                BcrLot = this.BcrLot
            };
        }
    }
    public class AoiSocketProduct
    {
        public List<AoiSocketProductInfo>[] AoiSocketInfo { get; set; }
        public AoiSocketProduct()
        {
            AoiSocketInfo = new List<AoiSocketProductInfo>[2];
            for (int i = 0; i < 2; i++)
            {
                AoiSocketInfo[i] = new List<AoiSocketProductInfo>();
            }
        }
    }
    public class EEpromSocketProduct
    {
        public List<EEpromSocketProductInfo>[] EEpromSocketInfo { get; set; }
        public EEpromSocketProduct()
        {
            EEpromSocketInfo = new List<EEpromSocketProductInfo>[4];
            for (int i = 0; i < 4; i++)
            {
                EEpromSocketInfo[i] = new List<EEpromSocketProductInfo>();
            }
        }
    }
    public class FwSocketProduct
    {
        public List<FwSocketProductInfo>[] FwSocketInfo { get; set; }
        public FwSocketProduct()
        {
            FwSocketInfo = new List<FwSocketProductInfo>[4];
            for (int i = 0; i < 4; i++)
            {
                FwSocketInfo[i] = new List<FwSocketProductInfo>();
            }
        }
    }
}