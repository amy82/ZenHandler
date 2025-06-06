using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.MotionControl
{
    public enum eBuzzer
    {
        OFF_BUZZER = 0,
        BUZZER1 = 1,
        BUZZER2 = 2,
        BUZZER3 = 3,
        BUZZER4 = 4
    }
    public enum eTowerLamp
    {
        OFF_LAMP = 0,
        RED_LAMP = 1,
        GREEN_LAMP = 2,
        YELLOW_LAMP = 3,
    }
    public enum eDic
    {
        LEFT = 0,
        RIGHT = 1
    }
    public interface IDioDefine
    {
        uint GetOutBuzzer(int ntype);
        uint GetOutTowerLamp(int ntype);
        uint GetOutAllDoor();
        uint GetInAllDoor(int ntype);

        //공용 io - Door , Buzzer , Lamp , Tray
        uint GetOutLiftDoor(int ntype, int index);        //Lift = Magazine, Lift
        uint GetInLiftDoor(int ntype, int index);        //Lift = Magazine, Lift

        uint GetOutNgTrayDoor(int ntype, int index);
        uint GetInNgTrayDoor(int ntype, int index);

        uint GetOutLiftLamp(int ntype, int index);        //Lift = Magazine, Lift
        uint GetOutNgTrayLamp(int ntype, int index);

        //Transfer
        uint GetInLoadPickerUpDown(int index, bool bFlag);
        uint GetInUnloadPickerUpDown(int index, bool bFlag);
        uint GetOutLoadPickerUpDown(int index, bool bFlag);
        uint GetOutUnloadPickerUpDown(int index, bool bFlag);

        uint GetInLoadPickerVacuumOn(int index, bool bFlag);
        uint GetOutLoadPickerVacuumOn(int index, bool bFlag);

        uint GetOutUnloadPickerVacuumOn(int index, bool bFlag);
        uint GetInUnloadPickerVacuumOn(int index, bool bFlag);

        uint GetOutFwUnloadPickerGrip(int index, bool bFlag);
        uint GetInFwUnloadPickerGrip(int index, bool bFlag);

        //-----------------------------------------------------------------------
        //Socket
        uint GetInContactUpDown(int Group, int index, bool bFlag);
        uint GetOutContactUpDown(int Group, int index, bool bFlag);
        uint GetInContactForBack(int Group, int index, bool bFlag);
        uint GetOutContactForBack(int Group, int index, bool bFlag);

        uint GetInRotateUpDown(int Group, int index, bool bFlag);
        uint GetOutRotateUpDown(int Group, int index, bool bFlag);

        uint GetInRotateTurn(int Group, int index, bool bFlag);
        uint GetOutRotateTurn(int Group, int index, bool bFlag);

        uint GetInRotateGrip(int Group, int index, bool bFlag);
        uint GetOutRotateGrip(int Group, int index, bool bFlag);

        uint GetInGoodDetect(int Group, int index);
        //-----------------------------------------------------------------------
        //Lift
        uint GetInGantryClampFor(bool bFlag);
        uint GetOutGantryClampFor(bool bFlag);

        uint GetInGantryCenteringFor(bool bFlag);
        uint GetOutGantryCenteringFor(bool bFlag);

        uint GetInTrayPusherUp(bool bFlag);
        uint GetOutTrayPusherUp(bool bFlag);
        uint GetInTrayPusherFor(bool bFlag);
        uint GetOutTrayPusherFor(bool bFlag);

        uint GetInTrayPusherCentringFor(bool bFlag);
        uint GetOutTrayPusherCentringFor(bool bFlag);

        uint GetInTopTouch(int index);
        uint GetInMiddleWait(int index);
        uint GetInTraySeated(int index);
        uint GetInOnSlide(int index);
        uint GetInTrayLoad(int index);

        //슬라이드 센서부터 추가해야된다.

        //-----------------------------------------------------------------------
        //Magazine

        uint GetInMagazineDocked(int index);
        uint GetInMagazineBottom(int index);
        uint GetInMagazineTrayLoad(int index);
        uint GetInMagazineTrayReady(int index);
    }
}
