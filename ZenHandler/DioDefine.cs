using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler
{
    public class DioDefine
    {
        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 0
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH0 : uint
        {
            TEMP1 = 0x01,                   //1
            TEMP2 = 0x02,
            START_PUSH_CHK = 0x04,
            DOOR_PUSH_CHK = 0x08,           //4
            TEMP5 = 0x10,
            TEMP6 = 0x20,
            TEMP7 = 0x40,
            IN_LENS_GRIP_BACK = 0x80,          //8
            //
            IN_LENS_GRIP_FOR = 0x01,           //9
            IN_VACUUM_ON = 0x02,
            TEMP11 = 0x04,
            TEMP12 = 0x08,                  //12
            TEMP13 = 0x10,
            LASER_CYL_DOWN = 0x20,
            LASER_CYL_UP = 0x40,
            TEMP16 = 0x80,                  //16
            //
            DOOR_SENSOR1 = 0x01,            //17
            DOOR_SENSOR2 = 0x02,
            DOOR_SENSOR3 = 0x04,
            DOOR_SENSOR4 = 0x08,            //20
            DOOR_SENSOR5 = 0x10,
            DOOR_SENSOR6 = 0x20,
            DOOR_SENSOR7 = 0x40,
            DOOR_SENSOR8 = 0x80,            //24
            //
            TEMP25 = 0x01,                  //25
            TEMP26 = 0x02,
            TEMP27 = 0x04,
            TEMP28 = 0x08,                  //28
            TEMP29 = 0x10,
            TEMP30 = 0x20,
            TEMP31 = 0x40,
            TEMP32 = 0x80                   //32
        };
        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 2
        //
        //
        //----------------------------------------------------------------------------------------------------------------



        //


        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 1
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH0 : uint
        {
            TOWER_LAMP_Y = 0x01,            //1
            TOWER_LAMP_G = 0x02,
            TOWER_LAMP_R = 0x04,
            TOWER_BUZZER = 0x08,            //4
            TEMP5 = 0x10,
            TEMP6 = 0x20,
            VACUUM_ON = 0x40,
            VACUUM_OFF = 0x80,                 //8
            //
            TEMP9 = 0x01,                  //9
            TEMP10 = 0x02,
            TEMP11 = 0x04,
            LENS_GRIP_BACK = 0x08,          //12
            LENS_GRIP_FOR = 0x10,
            TEMP14 = 0x20,
            TEMP15 = 0x40,
            TEMP16 = 0x80,                  //16
            //
            LASER_CYL_UP = 0x01,            //17
            LASER_CYL_DOWN = 0x02,
            TEMP19 = 0x04,
            START_PB_PRESS = 0x08,          //20
            DOOR_PB_PRESS = 0x10,
            TEMP22 = 0x20,
            TEMP23 = 0x40,
            TEMP24 = 0x80,                  //24
            //
            BUZZER1 = 0x01,                  //25
            BUZZER2 = 0x02,
            BUZZER3 = 0x04,
            BUZZER4 = 0x08,                  //28
            TEMP29 = 0x10,
            TEMP30 = 0x20,
            EPOXY_ON = 0x40,
            UV_ON = 0x80                    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 3
        //
        //
        //----------------------------------------------------------------------------------------------------------------
    }//END
}
