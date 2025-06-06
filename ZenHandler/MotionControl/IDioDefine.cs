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

        uint GetOutLiftDoor(int ntype, int index);        //Lift = Magazine, Lift
        uint GetOutNgTrayDoor(int ntype, int index);
        uint GetOutLiftLamp(int ntype, int index);        //Lift = Magazine, Lift
        uint GetOutNgTrayLamp(int ntype, int index);
    }
}
