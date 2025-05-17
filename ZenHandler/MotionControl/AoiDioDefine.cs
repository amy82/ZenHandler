using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.MotionControl
{
    public class AoiDioDefine
    {
        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 0
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH0 : uint
        {
            IN0_EMERGENCY1                      = 0x00000001,  //1
            IN0_EMERGENCY2                      = 0x00000002,
            IN0_EMERGENCY3                      = 0x00000004,
            TEMP4                               = 0x00000008,  //4
            TEMP5                               = 0x00000010,
            TEMP6                               = 0x00000020,
            TEMP7                               = 0x00000040,
            TEMP8                               = 0x00000080,  //8
            //
            IN0_DOOR_UNLOCK_FRONT_L             = 0x00000001,  //9
            IN0_DOOR_UNLOCK_FRONT_R             = 0x00000002,
            IN0_DOOR_UNLOCK_BACK_L              = 0x00000004,
            IN0_DOOR_UNLOCK_BACK_R              = 0x00000008,  //12
            TEMP13                              = 0x00000010,
            TEMP14                              = 0x00000020,
            TEMP15                              = 0x00000040,
            TEMP16                              = 0x00000080,  //16
            //
            IN0_LEFT_MAGAZINE_LOAD_MODE         = 0x00000001,  //17
            IN0_LEFT_MAGAZINE_COMPLETE_MODE     = 0x00000002,
            IN0_RIGHT_MAGAZINE_LOAD_MODE        = 0x00000004,
            IN0_RIGHT_MAGAZINE_COMPLETE_MODE    = 0x00000008,  //20
            TEMP21                              = 0x00000010,
            TEMP22                              = 0x00000020,
            TEMP23                              = 0x00000040,
            TEMP24                              = 0x00000080,  //24
            //
            IN0_MAGAZINE_DOCKED1                = 0x00000001,  //25
            IN0_MAGAZINE_DOCKED2                = 0x00000002,
            IN0_MAGAZINE_DOCKED3                = 0x00000004,
            IN0_MAGAZINE_DOCKED4                = 0x00000008,  //28
            IN0_MAGAZINE_BOTTOM_DETECTED        = 0x00000010,
            IN0_MAGAZINE_TRAY_DETECTED          = 0x00000020,
            IN0_NG_TRAY_DETECTED1               = 0x00000040,
            IN0_NG_TRAY_DETECTED2               = 0x00000080   //32
        };
        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 2
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH1 : uint
        {
            IN2_TRANSFER_LOAD_PICKER_DOWN1      = 0x00000001,  //1
            IN2_TRANSFER_LOAD_PICKER_DOWN2      = 0x00000002,
            IN2_TRANSFER_LOAD_PICKER_DOWN3      = 0x00000004,
            IN2_TRANSFER_LOAD_PICKER_DOWN4      = 0x00000008,  //4
            IN2_TRANSFER_LOAD_PICKER_UP1        = 0x00000010,
            IN2_TRANSFER_LOAD_PICKER_UP2        = 0x00000020,
            IN2_TRANSFER_LOAD_PICKER_UP3        = 0x00000040,
            IN2_TRANSFER_LOAD_PICKER_UP4        = 0x00000080,  //8
            //
            IN2_TRANSFER_UNLOAD_PICKER_DOWN1    = 0x00000001,  //9
            IN2_TRANSFER_UNLOAD_PICKER_DOWN2    = 0x00000002,
            IN2_TRANSFER_UNLOAD_PICKER_DOWN3    = 0x00000004,
            IN2_TRANSFER_UNLOAD_PICKER_DOWN4    = 0x00000008,  //12
            IN2_TRANSFER_UNLOAD_PICKER_UP1      = 0x00000010,
            IN2_TRANSFER_UNLOAD_PICKER_UP2      = 0x00000020,
            IN2_TRANSFER_UNLOAD_PICKER_UP3      = 0x00000040,
            IN2_TRANSFER_UNLOAD_PICKER_UP4      = 0x00000080,  //16
            //
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON1 = 0x00000001,  //17
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON2 = 0x00000002,
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON3 = 0x00000004,
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON4 = 0x00000008,  //20
            TEMP21                              = 0x00000010,
            TEMP22                              = 0x00000020,
            TEMP23                              = 0x00000040,
            TEMP24                              = 0x00000080,  //24
            //
            IN2_TRANSFER_UNLOAD_PICKER_GRIP1    = 0x00000001,   //25
            IN2_TRANSFER_UNLOAD_PICKER_GRIP2    = 0x00000002,
            IN2_TRANSFER_UNLOAD_PICKER_GRIP3    = 0x00000004,
            IN2_TRANSFER_UNLOAD_PICKER_GRIP4    = 0x00000008,   //28
            IN2_TRANSFER_UNLOAD_PICKER_UNGRIP1  = 0x00000010,
            IN2_TRANSFER_UNLOAD_PICKER_UNGRIP2  = 0x00000020,
            IN2_TRANSFER_UNLOAD_PICKER_UNGRIP3  = 0x00000040,
            IN2_TRANSFER_UNLOAD_PICKER_UNGRIP4  = 0x00000080    //32
        };


        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 4
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH2 : uint
        {
            IN4_FRONT_GANTRY_CLAMP_FOR              = 0x00000001,  //1
            IN4_FRONT_GANTRY_CLAMP_BACK             = 0x00000002,
            IN4_BACK_GANTRY_CLAMP_FOR               = 0x00000004,
            IN4_BACK_GANTRY_CLAMP_BACK              = 0x00000008,  //4
            IN4_FRONT_GANTRY_CENTRING_FOR           = 0x00000010,
            IN4_FRONT_GANTRY_CENTRING_BACK          = 0x00000020,
            IN4_BACK_GANTRY_CENTRING_FOR            = 0x00000040,
            IN4_BACK_GANTRY_CENTRING_BACK           = 0x00000080,  //8
            //
            IN4_TRAY_PUSHER_LEFT_UP                 = 0x00000001,  //9
            IN4_TRAY_PUSHER_LEFT_DOWN               = 0x00000002,
            IN4_TRAY_PUSHER_RIGHT_UP                = 0x00000004,
            IN4_TRAY_PUSHER_RIGHT_DOWN              = 0x00000008,  //12
            IN4_TRAY_PUSHER_LEFT_FOR                = 0x00000010,
            IN4_TRAY_PUSHER_LEFT_BACK               = 0x00000020,
            IN4_TRAY_PUSHER_RIGHT_FOR               = 0x00000040,
            IN4_TRAY_PUSHER_RIGHT_BACK              = 0x00000080,  //16
            //
            IN4_TRAY_PUSHER_CENTRING_LEFT_FOR       = 0x00000001,  //17
            IN4_TRAY_PUSHER_CENTRING_LEFT_BACK      = 0x00000002,
            IN4_TRAY_PUSHER_CENTRING_RIGHT_FOR      = 0x00000004,
            IN4_TRAY_PUSHER_CENTRING_RIGHT_BACK     = 0x00000008,  //20
            IN4_LEFT_TOP_STOP_TOUCH                 = 0x00000010,
            IN4_LEFT_UPPER_WAIT                     = 0x00000020,
            IN4_LEFT_LIFT_TRAY_SEATED               = 0x00000040,
            IN4_LEFT_LIFT_SIDE_IN_POS               = 0x00000080,  //24
            //
            IN4_RIGHT_TOP_STOP_TOUCH                = 0x00000001,   //25
            IN4_RIGHT_UPPER_WAIT                    = 0x00000002,
            IN4_RIGHT_LIFT_TRAY_SEATED              = 0x00000004,
            IN4_RIGHT_LIFT_SIDE_IN_POS              = 0x00000008,   //28
            TEMP29                                  = 0x00000010,
            TEMP30                                  = 0x00000020,
            IN4_GANTRY_TRAY_DETECTED                = 0x00000040,
            IN4_PUSHER_TRAY_DETECTED                = 0x00000080    //32
        };



        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 1
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH0 : uint
        {
            OUT1_TOWER_LAMP_Y                   = 0x00000001,  //1
            OUT1_TOWER_LAMP_G                   = 0x00000002,
            OUT1_TOWER_LAMP_R                   = 0x00000004,
            TEMP4                               = 0x00000008,  //4
            OUT1_BUZZER1                        = 0x00000010,
            OUT1_BUZZER2                        = 0x00000020,
            OUT1_BUZZER3                        = 0x00000040,
            OUT1_BUZZER4                        = 0x00000080,  //8
            //
            OUT1_DOOR_UNLOCK_FRONT_L            = 0x00000001,  //9
            OUT1_DOOR_UNLOCK_FRONT_R            = 0x00000002,
            OUT1_DOOR_UNLOCK_BACK_L             = 0x00000004,
            OUT1_DOOR_UNLOCK_BACK_R             = 0x00000008,  //12
            TEMP13                              = 0x00000010,
            TEMP14                              = 0x00000020,
            TEMP15                              = 0x00000040,
            TEMP16                              = 0x00000080,  //16
            //
            OUT1_LOAD_LIFT_LOAD_REQ             = 0x00000001,  //17
            OUT1_LOAD_LIFT_LOAD_COMPLETE        = 0x00000002,
            OUT1_UNLOAD_LIFT_LOAD_REQ           = 0x00000004,
            OUT1_UNLOAD_LIFT_LOAD_COMPLETE      = 0x00000008,  //20
            TEMP21                              = 0x00000010,
            TEMP22                              = 0x00000020,
            TEMP23                              = 0x00000040,
            TEMP24                              = 0x00000080,  //24
            //
            TEMP25                              = 0x00000001,  //25
            TEMP26                              = 0x00000002,
            TEMP27                              = 0x00000004,
            TEMP28                              = 0x00000008,  //28
            TEMP29                              = 0x00000010,
            TEMP30                              = 0x00000020,
            TEMP31                              = 0x00000040,
            TEMP32                              = 0x00000080   //32
        };
        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 3
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH1 : uint
        {
            OUT3_TRANSFER_LOAD_PICKER_DOWN1         = 0x00000001,   //1
            OUT3_TRANSFER_LOAD_PICKER_DOWN2         = 0x00000002,
            OUT3_TRANSFER_LOAD_PICKER_DOWN3         = 0x00000004,
            OUT3_TRANSFER_LOAD_PICKER_DOWN4         = 0x00000008,   //4
            OUT3_TRANSFER_LOAD_PICKER_UP1           = 0x00000010,
            OUT3_TRANSFER_LOAD_PICKER_UP2           = 0x00000020,
            OUT3_TRANSFER_LOAD_PICKER_UP3           = 0x00000040,
            OUT3_TRANSFER_LOAD_PICKER_UP4           = 0x00000080,   //8
            //
            OUT3_TRANSFER_UNLOAD_PICKER_DOWN1       = 0x00000001,   //9
            OUT3_TRANSFER_UNLOAD_PICKER_DOWN2       = 0x00000002,
            OUT3_TRANSFER_UNLOAD_PICKER_DOWN3       = 0x00000004,
            OUT3_TRANSFER_UNLOAD_PICKER_DOWN4       = 0x00000008,   //12
            OUT3_TRANSFER_UNLOAD_PICKER_UP1         = 0x00000010,
            OUT3_TRANSFER_UNLOAD_PICKER_UP2         = 0x00000020,
            OUT3_TRANSFER_UNLOAD_PICKER_UP3         = 0x00000040,
            OUT3_TRANSFER_UNLOAD_PICKER_UP4         = 0x00000080,   //16
            //                      
            OUT3_TRANSFER_LOAD_PICKER_VACUUM_ON1    = 0x00000001,   //17
            OUT3_TRANSFER_LOAD_PICKER_VACUUM_ON2    = 0x00000002,
            OUT3_TRANSFER_LOAD_PICKER_VACUUM_ON3    = 0x00000004,
            OUT3_TRANSFER_LOAD_PICKER_VACUUM_ON4    = 0x00000008,   //20
            OUT3_TRANSFER_LOAD_PICKER_BLOW_ON1      = 0x00000010,
            OUT3_TRANSFER_LOAD_PICKER_BLOW_ON2      = 0x00000020,
            OUT3_TRANSFER_LOAD_PICKER_BLOW_ON3      = 0x00000040,
            OUT3_TRANSFER_LOAD_PICKER_BLOW_ON4      = 0x00000080,   //24
            //
            OUT3_TRANSFER_UNLOAD_PICKER_VACUUM_ON1  = 0x00000001,   //25
            OUT3_TRANSFER_UNLOAD_PICKER_VACUUM_ON2  = 0x00000002,
            OUT3_TRANSFER_UNLOAD_PICKER_VACUUM_ON3  = 0x00000004,
            OUT3_TRANSFER_UNLOAD_PICKER_VACUUM_ON4  = 0x00000008,   //28
            OUT3_TRANSFER_UNLOAD_PICKER_BLOW_ON1    = 0x00000010,
            OUT3_TRANSFER_UNLOAD_PICKER_BLOW_ON2    = 0x00000020,
            OUT3_TRANSFER_UNLOAD_PICKER_BLOW_ON3    = 0x00000040,
            OUT3_TRANSFER_UNLOAD_PICKER_BLOW_ON4    = 0x00000080    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 5
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH2 : uint
        {
            OUT5_FRONT_GANTRY_CLAMP_FOR             = 0x00000001,  //1
            OUT5_FRONT_GANTRY_CLAMP_BACK            = 0x00000002,
            OUT5_BACK_GANTRY_CLAMP_FOR              = 0x00000004,
            OUT5_BACK_GANTRY_CLAMP_BACK             = 0x00000008,  //4
            OUT5_FRONT_GANTRY_CENTRING_FOR          = 0x00000010,
            OUT5_FRONT_GANTRY_CENTRING_BACK         = 0x00000020,
            OUT5_BACK_GANTRY_CENTRING_FOR           = 0x00000040,
            OUT5_BACK_GANTRY_CENTRING_BACK          = 0x00000080,  //8
            //
            OUT5_TRAY_PUSHER_LEFT_UP                = 0x00000001,  //9
            OUT5_TRAY_PUSHER_LEFT_DOWN              = 0x00000002,
            OUT5_TRAY_PUSHER_RIGHT_UP               = 0x00000004,
            OUT5_TRAY_PUSHER_RIGHT_DOWN             = 0x00000008,  //12
            OUT5_TRAY_PUSHER_LEFT_FOR               = 0x00000010,
            OUT5_TRAY_PUSHER_LEFT_BACK              = 0x00000020,
            OUT5_TRAY_PUSHER_RIGHT_FOR              = 0x00000040,
            OUT5_TRAY_PUSHER_RIGHT_BACK             = 0x00000080,  //16
            //                      
            OUT5_TRAY_PUSHER_CENTRING_LEFT_FOR      = 0x00000001,  //17
            OUT5_TRAY_PUSHER_CENTRING_LEFT_BACK     = 0x00000002,
            OUT5_TRAY_PUSHER_CENTRING_RIGHT_FOR     = 0x00000004,
            OUT5_TRAY_PUSHER_CENTRING_RIGHT_BACK    = 0x00000008,  //20
            TEMP21                                  = 0x00000010,
            TEMP22                                  = 0x00000020,
            TEMP23                                  = 0x00000040,
            TEMP24                                  = 0x00000080,  //24
            //
            TEMP25                                  = 0x00000001,  //25
            TEMP26                                  = 0x00000002,
            TEMP27                                  = 0x00000004,
            TEMP28                                  = 0x00000008,  //28
            TEMP29                                  = 0x00000010,
            TEMP30                                  = 0x00000020,
            TEMP31                                  = 0x00000040,
            TEMP32                                  = 0x00000080   //32
        };
        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 7
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH3 : uint
        {
            OUT7_A_SOCKET_CONTACT_UP1               = 0x00000001,  //1
            OUT7_A_SOCKET_CONTACT_UP2               = 0x00000002,
            OUT7_A_SOCKET_CONTACT_UP3               = 0x00000004,
            OUT7_A_SOCKET_CONTACT_UP4               = 0x00000008,  //4
            OUT7_A_SOCKET_CONTACT_DOWN1             = 0x00000010,
            OUT7_A_SOCKET_CONTACT_DOWN2             = 0x00000020,
            OUT7_A_SOCKET_CONTACT_DOWN3             = 0x00000040,
            OUT7_A_SOCKET_CONTACT_DOWN4             = 0x00000080,  //8
            //
            OUT7_A_SOCKET_CONTACT_FORWARD1          = 0x00000001,  //9
            OUT7_A_SOCKET_CONTACT_FORWARD2          = 0x00000002,
            OUT7_A_SOCKET_CONTACT_FORWARD3          = 0x00000004,
            OUT7_A_SOCKET_CONTACT_FORWARD4          = 0x00000008,  //12
            OUT7_A_SOCKET_CONTACT_BACKWARD1         = 0x00000010,
            OUT7_A_SOCKET_CONTACT_BACKWARD2         = 0x00000020,
            OUT7_A_SOCKET_CONTACT_BACKWARD3         = 0x00000040,
            OUT7_A_SOCKET_CONTACT_BACKWARD4         = 0x00000080,  //16
            //                      
            OUT7_B_SOCKET_CONTACT_UP1               = 0x00000001,  //17
            OUT7_B_SOCKET_CONTACT_UP2               = 0x00000002,
            OUT7_B_SOCKET_CONTACT_UP3               = 0x00000004,
            OUT7_B_SOCKET_CONTACT_UP4               = 0x00000008,  //20
            OUT7_B_SOCKET_CONTACT_DOWN1             = 0x00000010,
            OUT7_B_SOCKET_CONTACT_DOWN2             = 0x00000020,
            OUT7_B_SOCKET_CONTACT_DOWN3             = 0x00000040,
            OUT7_B_SOCKET_CONTACT_DOWN4             = 0x00000080,  //24
            //
            OUT7_B_SOCKET_CONTACT_FORWARD1          = 0x00000001,  //25
            OUT7_B_SOCKET_CONTACT_FORWARD2          = 0x00000002,
            OUT7_B_SOCKET_CONTACT_FORWARD3          = 0x00000004,
            OUT7_B_SOCKET_CONTACT_FORWARD4          = 0x00000008,  //28
            OUT7_B_SOCKET_CONTACT_BACKWARD1         = 0x00000010,
            OUT7_B_SOCKET_CONTACT_BACKWARD2         = 0x00000020,
            OUT7_B_SOCKET_CONTACT_BACKWARD3         = 0x00000040,
            OUT7_B_SOCKET_CONTACT_BACKWARD4         = 0x00000080   //32
        };
    }//END
}
