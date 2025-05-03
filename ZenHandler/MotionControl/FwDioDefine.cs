using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.MotionControl
{
    public class FwDioDefine
    {
        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 0
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH0 : uint
        {
            TEMP1                   = 0x00000001,            //1
            TEMP2                   = 0x00000002,
            START_PUSH_CHK          = 0x00000004,
            DOOR_PUSH_CHK           = 0x00000008,           
            TEMP5                   = 0x00000010,
            TEMP6                   = 0x00000020,
            TEMP7                   = 0x00000040,
            IN_LENS_GRIP_BACK       = 0x00000080,            //8
            //
            IN_LENS_GRIP_FOR        = 0x00000001,            //9
            IN_VACUUM_ON            = 0x00000002,
            TEMP11                  = 0x00000004,
            TEMP12                  = 0x00000008,            //12
            TEMP13                  = 0x00000010,
            LASER_CYL_DOWN          = 0x00000020,
            LASER_CYL_UP            = 0x00000040,
            TEMP16                  = 0x00000080,            //16
            //
            DOOR_SENSOR1            = 0x00000001,            //17
            DOOR_SENSOR2            = 0x00000002,
            DOOR_SENSOR3            = 0x00000004,
            DOOR_SENSOR4            = 0x00000008,            //20
            DOOR_SENSOR5            = 0x00000010,
            DOOR_SENSOR6            = 0x00000020,
            DOOR_SENSOR7            = 0x00000040,
            DOOR_SENSOR8            = 0x00000080,            //24
            //
            TEMP25                  = 0x00000001,            //25
            TEMP26                  = 0x00000002,
            TEMP27                  = 0x00000004,
            TEMP28                  = 0x00000008,            //28
            TEMP29                  = 0x00000010,
            TEMP30                  = 0x00000020,
            TEMP31                  = 0x00000040,
            TEMP32                  = 0x00000080             //32
        };
        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 2
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH1 : uint
        {
            IN_LOAD_PICKER_UP1      = 0x00000001,                   //1
            IN_LOAD_PICKER_UP2      = 0x00000002,
            IN_LOAD_PICKER_UP3      = 0x00000004,
            IN_LOAD_PICKER_UP4      = 0x00000008,           //4
            IN_LOAD_PICKER_DOWN1    = 0x00000010,
            IN_LOAD_PICKER_DOWN2    = 0x00000020,
            IN_LOAD_PICKER_DOWN3    = 0x00000040,
            IN_LOAD_PICKER_DOWN4    = 0x00000080,          //8
            //
            IN_UNLOAD_PICKER_UP1    = 0x00000001,           //9
            IN_UNLOAD_PICKER_UP2    = 0x00000002,
            IN_UNLOAD_PICKER_UP3    = 0x00000004,
            IN_UNLOAD_PICKER_UP4    = 0x00000008,                  //12
            IN_UNLOAD_PICKER_DOWN1  = 0x00000010,
            IN_UNLOAD_PICKER_DOWN2  = 0x00000020,
            IN_UNLOAD_PICKER_DOWN3  = 0x00000040,
            IN_UNLOAD_PICKER_DOWN4  = 0x00000080,                  //16
            //
            DOOR_SENSOR1            = 0x00000001,            //17
            DOOR_SENSOR2            = 0x00000002,
            DOOR_SENSOR3            = 0x00000004,
            DOOR_SENSOR4            = 0x00000008,            //20
            DOOR_SENSOR5            = 0x00000010,
            DOOR_SENSOR6            = 0x00000020,
            DOOR_SENSOR7            = 0x00000040,
            DOOR_SENSOR8            = 0x00000080,            //24
            //
            TEMP25                  = 0x00000001,                  //25
            TEMP26                  = 0x00000002,
            TEMP27                  = 0x00000004,
            TEMP28                  = 0x00000008,                  //28
            TEMP29                  = 0x00000010,
            TEMP30                  = 0x00000020,
            TEMP31                  = 0x00000040,
            TEMP32                  = 0x00000080                   //32
        };


        //


        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 1
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH0 : uint
        {
            TOWER_LAMP_Y            = 0x00000001,            //1
            TOWER_LAMP_G            = 0x00000002,
            TOWER_LAMP_R            = 0x00000004,
            TOWER_BUZZER            = 0x00000008,            //4
            TEMP5                   = 0x00000010,
            TEMP6                   = 0x00000020,
            VACUUM_ON               = 0x00000040,
            VACUUM_OFF              = 0x00000080,                 //8
            //
            TEMP9                   = 0x00000001,                  //9
            TEMP10                  = 0x00000002,
            TEMP11                  = 0x00000004,
            LENS_GRIP_BACK          = 0x00000008,          //12
            LENS_GRIP_FOR           = 0x00000010,
            TEMP14                  = 0x00000020,
            TEMP15                  = 0x00000040,
            TEMP16                  = 0x00000080,                  //16
            //
            LASER_CYL_UP            = 0x00000001,            //17
            LASER_CYL_DOWN          = 0x00000002,
            TEMP19                  = 0x00000004,
            START_PB_PRESS          = 0x00000008,          //20
            DOOR_PB_PRESS           = 0x00000010,
            TEMP22                  = 0x00000020,
            TEMP23                  = 0x00000040,
            TEMP24                  = 0x00000080,                  //24
            //
            BUZZER1                 = 0x00000001,                  //25
            BUZZER2                 = 0x00000002,
            BUZZER3                 = 0x00000004,
            BUZZER4                 = 0x00000008,                  //28
            TEMP29                  = 0x00000010,
            TEMP30                  = 0x00000020,
            EPOXY_ON                = 0x00000040,
            UV_ON                   = 0x00000080                    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 3
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH1 : uint
        {
            LOAD_PICKER_UP1         = 0x00000001,            //1
            LOAD_PICKER_UP2         = 0x00000002,
            LOAD_PICKER_UP3         = 0x00000004,
            LOAD_PICKER_UP4         = 0x00000008,            //4
            LOAD_PICKER_DOWN1       = 0x00000010,
            LOAD_PICKER_DOWN2       = 0x00000020,
            LOAD_PICKER_DOWN3       = 0x00000040,
            LOAD_PICKER_DOWN4       = 0x00000080,                 //8
            //
            UNLOAD_PICKER_UP1       = 0x00000001,                  //9
            UNLOAD_PICKER_UP2       = 0x00000002,
            UNLOAD_PICKER_UP3       = 0x00000004,
            UNLOAD_PICKER_UP4       = 0x00000008,          //12
            UNLOAD_PICKER_DOWN1     = 0x00000010,
            UNLOAD_PICKER_DOWN2     = 0x00000020,
            UNLOAD_PICKER_DOWN3     = 0x00000040,
            UNLOAD_PICKER_DOWN4     = 0x00000080,                  //16
            //                      
            LASER_CYL_UP            = 0x00000001,            //17
            LASER_CYL_DOWN          = 0x00000002,
            TEMP19                  = 0x00000004,
            START_PB_PRESS          = 0x00000008,          //20
            DOOR_PB_PRESS           = 0x00000010,
            TEMP22                  = 0x00000020,
            TEMP23                  = 0x00000040,
            TEMP24                  = 0x00000080,                  //24
            //
            BUZZER1                 = 0x00000001,                  //25
            BUZZER2                 = 0x00000002,
            BUZZER3                 = 0x00000004,
            BUZZER4                 = 0x00000008,                  //28
            TEMP29                  = 0x00000010,
            TEMP30                  = 0x00000020,
            TEMP31                  = 0x00000040,
            TEMP32                  = 0x00000080                    //32
        };
    }//END
}
