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
            IN0_EMERGENCY1                          = 0x00000001,  //1
            IN0_EMERGENCY2                          = 0x00000002,
            IN0_EMERGENCY3                          = 0x00000004,
            TEMP4                                   = 0x00000008,  //4
            IN0_LEFT_MAGAZINE_LOAD_MODE             = 0x00000010,
            IN0_LEFT_MAGAZINE_COMPLETE_MODE         = 0x00000020,
            IN0_RIGHT_MAGAZINE_LOAD_MODE            = 0x00000040,
            IN0_RIGHT_MAGAZINE_COMPLETE_MODE        = 0x00000080,  //8
            //
            IN0_DOOR_UNLOCK_FRONT_L                 = 0x00000001,  //9
            IN0_DOOR_UNLOCK_FRONT_R                 = 0x00000002,
            IN0_DOOR_UNLOCK_BACK_L                  = 0x00000004,
            IN0_DOOR_UNLOCK_BACK_R                  = 0x00000008,  //12
            IN0_LEFT_MAGAZINE_DOOR_LOCK_UP          = 0x00000010,
            IN0_LEFT_MAGAZINE_DOOR_LOCK_DOWN        = 0x00000020,
            IN0_RIGHT_MAGAZINE_DOOR_LOCK_UP         = 0x00000040,
            IN0_RIGHT_MAGAZINE_DOOR_LOCK_DOWN       = 0x00000080,  //16
            //
            IN0_LEFT_MAGAZINE_MAGNETIC              = 0x00000001,  //17
            IN0_RIGHT_MAGAZINE_MAGNETIC             = 0x00000002,
            IN0_LEFT_NG_DOOR_LOCK_LAMP              = 0x00000004,
            IN0_LEFT_NG_DOOR_UNLOCK_LAMP            = 0x00000008,  //20
            IN0_RIGHT_NG_DOOR_LOCK_LAMP             = 0x00000010,
            IN0_RIGHT_NG_DOOR_UNLOCK_LAMP           = 0x00000020,
            IN0_LEFT_NG_DOOR_LOCK_UP                = 0x00000040,
            IN0_LEFT_NG_DOOR_LOCK_DOWN              = 0x00000080,  //24
            //
            IN0_RIGHT_NG_DOOR_LOCK_UP               = 0x00000001,  //25
            IN0_RIGHT_NG_DOOR_LOCK_DOWN             = 0x00000002,
            IN0_LEFT_MAGAZINE_DOCKED1               = 0x00000004,
            IN0_LEFT_MAGAZINE_BOTTOM_DETECTED       = 0x00000008,  //28
            IN0_LEFT_MAGAZINE_TRAY_LOAD_DETECTED    = 0x00000010,
            IN0_LEFT_MAGAZINE_TRAY_REDAY_DETECTED   = 0x00000020,
            IN0_RIGHT_MAGAZINE_DOCKED1              = 0x00000040,
            IN0_RIGHT_MAGAZINE_BOTTOM_DETECTED      = 0x00000080   //32
        };
        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 2
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH1 : uint
        {
            IN0_RIGHT_MAGAZINE_TRAY_LOAD_DETECTED       = 0x00000001,  //1
            IN0_RIGHT_MAGAZINE_TRAY_REDAY_DETECTED      = 0x00000002,
            IN2_LEFT_NG_TRAY_DETECTED_MAGNETIC          = 0x00000004,
            IN2_RIGHT_NG_TRAY_DETECTED_MAGNETIC         = 0x00000008,  //4
            IN2_TRANSFER_LOAD_PICKER_DOWN1              = 0x00000010,
            IN2_TRANSFER_LOAD_PICKER_DOWN2              = 0x00000020,
            IN2_TRANSFER_LOAD_PICKER_DOWN3              = 0x00000040,
            IN2_TRANSFER_LOAD_PICKER_DOWN4              = 0x00000080,  //8
            //
            IN2_TRANSFER_LOAD_PICKER_UP1                = 0x00000001,  //9
            IN2_TRANSFER_LOAD_PICKER_UP2                = 0x00000002,
            IN2_TRANSFER_LOAD_PICKER_UP3                = 0x00000004,
            IN2_TRANSFER_LOAD_PICKER_UP4                = 0x00000008,  //12
            IN2_TRANSFER_UNLOAD_PICKER_DOWN1            = 0x00000010,
            IN2_TRANSFER_UNLOAD_PICKER_DOWN2            = 0x00000020,
            IN2_TRANSFER_UNLOAD_PICKER_DOWN3            = 0x00000040,
            IN2_TRANSFER_UNLOAD_PICKER_DOWN4            = 0x00000080,  //16
            //
            IN2_TRANSFER_UNLOAD_PICKER_UP1              = 0x00000001,  //17
            IN2_TRANSFER_UNLOAD_PICKER_UP2              = 0x00000002,
            IN2_TRANSFER_UNLOAD_PICKER_UP3              = 0x00000004,
            IN2_TRANSFER_UNLOAD_PICKER_UP4              = 0x00000008,  //20
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON1         = 0x00000010,
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON2         = 0x00000020,
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON3         = 0x00000040,
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON4         = 0x00000080,            //24
            //
            IN2_TRANSFER_UNLOAD_PICKER_GRIP1        = 0x00000001,   //25
            IN2_TRANSFER_UNLOAD_PICKER_GRIP2        = 0x00000002,
            IN2_TRANSFER_UNLOAD_PICKER_GRIP3        = 0x00000004,
            IN2_TRANSFER_UNLOAD_PICKER_GRIP4        = 0x00000008,   //28
            IN2_TRANSFER_UNLOAD_PICKER_UNGRIP1      = 0x00000010,
            IN2_TRANSFER_UNLOAD_PICKER_UNGRIP2      = 0x00000020,
            IN2_TRANSFER_UNLOAD_PICKER_UNGRIP3      = 0x00000040,
            IN2_TRANSFER_UNLOAD_PICKER_UNGRIP4      = 0x00000080    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 4
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH2 : uint
        {
            IN4_A_SOCKET_CONTACT_UP1            = 0x00000001,  //1
            IN4_A_SOCKET_CONTACT_UP2            = 0x00000002,
            IN4_A_SOCKET_CONTACT_UP3            = 0x00000004,
            IN4_A_SOCKET_CONTACT_UP4            = 0x00000008,  //4
            IN4_A_SOCKET_CONTACT_DOWN1          = 0x00000010,
            IN4_A_SOCKET_CONTACT_DOWN2          = 0x00000020,
            IN4_A_SOCKET_CONTACT_DOWN3          = 0x00000040,
            IN4_A_SOCKET_CONTACT_DOWN4          = 0x00000080,  //8
            //
            IN4_A_SOCKET_CONTACT_FORWARD1       = 0x00000001,  //9
            IN4_A_SOCKET_CONTACT_FORWARD2       = 0x00000002,
            IN4_A_SOCKET_CONTACT_FORWARD3       = 0x00000004,
            IN4_A_SOCKET_CONTACT_FORWARD4       = 0x00000008,  //12
            IN4_A_SOCKET_CONTACT_BACKWARD1      = 0x00000010,
            IN4_A_SOCKET_CONTACT_BACKWARD2      = 0x00000020,
            IN4_A_SOCKET_CONTACT_BACKWARD3      = 0x00000040,
            IN4_A_SOCKET_CONTACT_BACKWARD4      = 0x00000080,  //16
            //
            IN4_B_SOCKET_CONTACT_UP1            = 0x00000001,  //17
            IN4_B_SOCKET_CONTACT_UP2            = 0x00000002,
            IN4_B_SOCKET_CONTACT_UP3            = 0x00000004,
            IN4_B_SOCKET_CONTACT_UP4            = 0x00000008,  //20
            IN4_B_SOCKET_CONTACT_DOWN1          = 0x00000010,
            IN4_B_SOCKET_CONTACT_DOWN2          = 0x00000020,
            IN4_B_SOCKET_CONTACT_DOWN3          = 0x00000040,
            IN4_B_SOCKET_CONTACT_DOWN4          = 0x00000080,            //24
            //
            IN4_B_SOCKET_CONTACT_FORWARD1       = 0x00000001,   //25
            IN4_B_SOCKET_CONTACT_FORWARD2       = 0x00000002,
            IN4_B_SOCKET_CONTACT_FORWARD3       = 0x00000004,
            IN4_B_SOCKET_CONTACT_FORWARD4       = 0x00000008,   //28
            IN4_B_SOCKET_CONTACT_BACKWARD1      = 0x00000010,
            IN4_B_SOCKET_CONTACT_BACKWARD2      = 0x00000020,
            IN4_B_SOCKET_CONTACT_BACKWARD3      = 0x00000040,
            IN4_B_SOCKET_CONTACT_BACKWARD4      = 0x00000080    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 6
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH3 : uint
        {
            IN6_C_SOCKET_CONTACT_UP1            = 0x00000001,  //1
            IN6_C_SOCKET_CONTACT_UP2            = 0x00000002,
            IN6_C_SOCKET_CONTACT_UP3            = 0x00000004,
            IN6_C_SOCKET_CONTACT_UP4            = 0x00000008,  //4
            IN6_C_SOCKET_CONTACT_DOWN1          = 0x00000010,
            IN6_C_SOCKET_CONTACT_DOWN2          = 0x00000020,
            IN6_C_SOCKET_CONTACT_DOWN3          = 0x00000040,
            IN6_C_SOCKET_CONTACT_DOWN4          = 0x00000080,  //8
            //
            IN6_C_SOCKET_CONTACT_FORWARD1       = 0x00000001,  //9
            IN6_C_SOCKET_CONTACT_FORWARD2       = 0x00000002,
            IN6_C_SOCKET_CONTACT_FORWARD3       = 0x00000004,
            IN6_C_SOCKET_CONTACT_FORWARD4       = 0x00000008,  //12
            IN6_C_SOCKET_CONTACT_BACKWARD1      = 0x00000010,
            IN6_C_SOCKET_CONTACT_BACKWARD2      = 0x00000020,
            IN6_C_SOCKET_CONTACT_BACKWARD3      = 0x00000040,
            IN6_C_SOCKET_CONTACT_BACKWARD4      = 0x00000080,  //16
            //
            IN6_D_SOCKET_CONTACT_UP1            = 0x00000001,  //17
            IN6_D_SOCKET_CONTACT_UP2            = 0x00000002,
            IN6_D_SOCKET_CONTACT_UP3            = 0x00000004,
            IN6_D_SOCKET_CONTACT_UP4            = 0x00000008,  //20
            IN6_D_SOCKET_CONTACT_DOWN1          = 0x00000010,
            IN6_D_SOCKET_CONTACT_DOWN2          = 0x00000020,
            IN6_D_SOCKET_CONTACT_DOWN3          = 0x00000040,
            IN6_D_SOCKET_CONTACT_DOWN4          = 0x00000080,            //24
            //
            IN6_D_SOCKET_CONTACT_FORWARD1       = 0x00000001,   //25
            IN6_D_SOCKET_CONTACT_FORWARD2       = 0x00000002,
            IN6_D_SOCKET_CONTACT_FORWARD3       = 0x00000004,
            IN6_D_SOCKET_CONTACT_FORWARD4       = 0x00000008,   //28
            IN6_D_SOCKET_CONTACT_BACKWARD1      = 0x00000010,
            IN6_D_SOCKET_CONTACT_BACKWARD2      = 0x00000020,
            IN6_D_SOCKET_CONTACT_BACKWARD3      = 0x00000040,
            IN6_D_SOCKET_CONTACT_BACKWARD4      = 0x00000080    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 8
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH4 : uint
        {
            IN8_A_SOCKET_ROTATE_UP1             = 0x00000001,  //1
            IN8_A_SOCKET_ROTATE_UP2             = 0x00000002,
            IN8_A_SOCKET_ROTATE_UP3             = 0x00000004,
            IN8_A_SOCKET_ROTATE_UP4             = 0x00000008,  //4
            IN8_A_SOCKET_ROTATE_DOWN1           = 0x00000010,
            IN8_A_SOCKET_ROTATE_DOWN2           = 0x00000020,
            IN8_A_SOCKET_ROTATE_DOWN3           = 0x00000040,
            IN8_A_SOCKET_ROTATE_DOWN4           = 0x00000080,  //8
            //
            IN8_A_SOCKET_ROTATE_TURN_LEFT1      = 0x00000001,  //9
            IN8_A_SOCKET_ROTATE_TURN_LEFT2      = 0x00000002,
            IN8_A_SOCKET_ROTATE_TURN_LEFT3      = 0x00000004,
            IN8_A_SOCKET_ROTATE_TURN_LEFT4      = 0x00000008,  //12
            IN8_A_SOCKET_ROTATE_TURN_HOME1      = 0x00000010,
            IN8_A_SOCKET_ROTATE_TURN_HOME2      = 0x00000020,
            IN8_A_SOCKET_ROTATE_TURN_HOME3      = 0x00000040,
            IN8_A_SOCKET_ROTATE_TURN_HOME4      = 0x00000080,  //16
            //
            IN8_A_SOCKET_ROTATE_GRIP1           = 0x00000001,  //17
            IN8_A_SOCKET_ROTATE_GRIP2           = 0x00000002,
            IN8_A_SOCKET_ROTATE_GRIP3           = 0x00000004,
            IN8_A_SOCKET_ROTATE_GRIP4           = 0x00000008,  //20
            IN8_A_SOCKET_ROTATE_UNGRIP1         = 0x00000010,
            IN8_A_SOCKET_ROTATE_UNGRIP2         = 0x00000020,
            IN8_A_SOCKET_ROTATE_UNGRIP3         = 0x00000040,
            IN8_A_SOCKET_ROTATE_UNGRIP4         = 0x00000080,            //24
            //
            IN8_A_SOCKET_VACUUM_ON1             = 0x00000001,   //25
            IN8_A_SOCKET_VACUUM_ON2             = 0x00000002,
            IN8_A_SOCKET_VACUUM_ON3             = 0x00000004,
            IN8_A_SOCKET_VACUUM_ON4             = 0x00000008,   //28
            TEMP29 = 0x00000010,
            TEMP30 = 0x00000020,
            TEMP31 = 0x00000040,
            TEMP32 = 0x00000080    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 10
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH5 : uint
        {
            IN10_B_SOCKET_ROTATE_UP1             = 0x00000001,  //1
            IN10_B_SOCKET_ROTATE_UP2             = 0x00000002,
            IN10_B_SOCKET_ROTATE_UP3             = 0x00000004,
            IN10_B_SOCKET_ROTATE_UP4             = 0x00000008,  //4
            IN10_B_SOCKET_ROTATE_DOWN1           = 0x00000010,
            IN10_B_SOCKET_ROTATE_DOWN2           = 0x00000020,
            IN10_B_SOCKET_ROTATE_DOWN3           = 0x00000040,
            IN10_B_SOCKET_ROTATE_DOWN4           = 0x00000080,  //8
            //
            IN10_B_SOCKET_ROTATE_TURN_LEFT1      = 0x00000001,  //9
            IN10_B_SOCKET_ROTATE_TURN_LEFT2      = 0x00000002,
            IN10_B_SOCKET_ROTATE_TURN_LEFT3      = 0x00000004,
            IN10_B_SOCKET_ROTATE_TURN_LEFT4      = 0x00000008,  //12
            IN10_B_SOCKET_ROTATE_TURN_HOME1      = 0x00000010,
            IN10_B_SOCKET_ROTATE_TURN_HOME2      = 0x00000020,
            IN10_B_SOCKET_ROTATE_TURN_HOME3      = 0x00000040,
            IN10_B_SOCKET_ROTATE_TURN_HOME4      = 0x00000080,  //16
            //
            IN10_B_SOCKET_ROTATE_GRIP1           = 0x00000001,  //17
            IN10_B_SOCKET_ROTATE_GRIP2           = 0x00000002,
            IN10_B_SOCKET_ROTATE_GRIP3           = 0x00000004,
            IN10_B_SOCKET_ROTATE_GRIP4           = 0x00000008,  //20
            IN10_B_SOCKET_ROTATE_UNGRIP1         = 0x00000010,
            IN10_B_SOCKET_ROTATE_UNGRIP2         = 0x00000020,
            IN10_B_SOCKET_ROTATE_UNGRIP3         = 0x00000040,
            IN10_B_SOCKET_ROTATE_UNGRIP4         = 0x00000080,            //24
            //
            IN10_B_SOCKET_VACUUM_ON1             = 0x00000001,   //25
            IN10_B_SOCKET_VACUUM_ON2             = 0x00000002,
            IN10_B_SOCKET_VACUUM_ON3             = 0x00000004,
            IN10_B_SOCKET_VACUUM_ON4             = 0x00000008,   //28
            TEMP29                               = 0x00000010,
            TEMP30                               = 0x00000020,
            TEMP31                               = 0x00000040,
            TEMP32                               = 0x00000080    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 12
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH6 : uint
        {
            IN12_C_SOCKET_ROTATE_UP1             = 0x00000001,  //1
            IN12_C_SOCKET_ROTATE_UP2             = 0x00000002,
            IN12_C_SOCKET_ROTATE_UP3             = 0x00000004,
            IN12_C_SOCKET_ROTATE_UP4             = 0x00000008,  //4
            IN12_C_SOCKET_ROTATE_DOWN1           = 0x00000010,
            IN12_C_SOCKET_ROTATE_DOWN2           = 0x00000020,
            IN12_C_SOCKET_ROTATE_DOWN3           = 0x00000040,
            IN12_C_SOCKET_ROTATE_DOWN4           = 0x00000080,  //8
            //
            IN12_C_SOCKET_ROTATE_TURN_LEFT1      = 0x00000001,  //9
            IN12_C_SOCKET_ROTATE_TURN_LEFT2      = 0x00000002,
            IN12_C_SOCKET_ROTATE_TURN_LEFT3      = 0x00000004,
            IN12_C_SOCKET_ROTATE_TURN_LEFT4      = 0x00000008,  //12
            IN12_C_SOCKET_ROTATE_TURN_HOME1      = 0x00000010,
            IN12_C_SOCKET_ROTATE_TURN_HOME2      = 0x00000020,
            IN12_C_SOCKET_ROTATE_TURN_HOME3      = 0x00000040,
            IN12_C_SOCKET_ROTATE_TURN_HOME4      = 0x00000080,  //16
            //
            IN12_C_SOCKET_ROTATE_GRIP1           = 0x00000001,  //17
            IN12_C_SOCKET_ROTATE_GRIP2           = 0x00000002,
            IN12_C_SOCKET_ROTATE_GRIP3           = 0x00000004,
            IN12_C_SOCKET_ROTATE_GRIP4           = 0x00000008,  //20
            IN12_C_SOCKET_ROTATE_UNGRIP1         = 0x00000010,
            IN12_C_SOCKET_ROTATE_UNGRIP2         = 0x00000020,
            IN12_C_SOCKET_ROTATE_UNGRIP3         = 0x00000040,
            IN12_C_SOCKET_ROTATE_UNGRIP4         = 0x00000080,            //24
            //
            IN12_C_SOCKET_VACUUM_ON1             = 0x00000001,   //25
            IN12_C_SOCKET_VACUUM_ON2             = 0x00000002,
            IN12_C_SOCKET_VACUUM_ON3             = 0x00000004,
            IN12_C_SOCKET_VACUUM_ON4             = 0x00000008,   //28
            TEMP29                              = 0x00000010,
            TEMP30                              = 0x00000020,
            TEMP31                              = 0x00000040,
            TEMP32                              = 0x00000080    //32
        };
        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 14
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH7 : uint
        {
            IN14_D_SOCKET_ROTATE_UP1             = 0x00000001,  //1
            IN14_D_SOCKET_ROTATE_UP2             = 0x00000002,
            IN14_D_SOCKET_ROTATE_UP3             = 0x00000004,
            IN14_D_SOCKET_ROTATE_UP4             = 0x00000008,  //4
            IN14_D_SOCKET_ROTATE_DOWN1           = 0x00000010,
            IN14_D_SOCKET_ROTATE_DOWN2           = 0x00000020,
            IN14_D_SOCKET_ROTATE_DOWN3           = 0x00000040,
            IN14_D_SOCKET_ROTATE_DOWN4           = 0x00000080,  //8
            //
            IN14_D_SOCKET_ROTATE_TURN_LEFT1      = 0x00000001,  //9
            IN14_D_SOCKET_ROTATE_TURN_LEFT2      = 0x00000002,
            IN14_D_SOCKET_ROTATE_TURN_LEFT3      = 0x00000004,
            IN14_D_SOCKET_ROTATE_TURN_LEFT4      = 0x00000008,  //12
            IN14_D_SOCKET_ROTATE_TURN_HOME1      = 0x00000010,
            IN14_D_SOCKET_ROTATE_TURN_HOME2      = 0x00000020,
            IN14_D_SOCKET_ROTATE_TURN_HOME3      = 0x00000040,
            IN14_D_SOCKET_ROTATE_TURN_HOME4      = 0x00000080,  //16
            //
            IN14_D_SOCKET_ROTATE_GRIP1           = 0x00000001,  //17
            IN14_D_SOCKET_ROTATE_GRIP2           = 0x00000002,
            IN14_D_SOCKET_ROTATE_GRIP3           = 0x00000004,
            IN14_D_SOCKET_ROTATE_GRIP4           = 0x00000008,  //20
            IN14_D_SOCKET_ROTATE_UNGRIP1         = 0x00000010,
            IN14_D_SOCKET_ROTATE_UNGRIP2         = 0x00000020,
            IN14_D_SOCKET_ROTATE_UNGRIP3         = 0x00000040,
            IN14_D_SOCKET_ROTATE_UNGRIP4         = 0x00000080,            //24
            //
            IN14_D_SOCKET_VACUUM_ON1             = 0x00000001,   //25
            IN14_D_SOCKET_VACUUM_ON2             = 0x00000002,
            IN14_D_SOCKET_VACUUM_ON3             = 0x00000004,
            IN14_D_SOCKET_VACUUM_ON4             = 0x00000008,   //28
            TEMP29                               = 0x00000010,
            TEMP30                               = 0x00000020,
            TEMP31                               = 0x00000040,
            TEMP32                               = 0x00000080    //32
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
            OUT1_TOWER_LAMP_Y                       = 0x00000001,  //1
            OUT1_TOWER_LAMP_G                       = 0x00000002,
            OUT1_TOWER_LAMP_R                       = 0x00000004,
            TEMP4                                   = 0x00000008,  //4
            OUT1_BUZZER1                            = 0x00000010,
            OUT1_BUZZER2                            = 0x00000020,
            OUT1_BUZZER3                            = 0x00000040,
            OUT1_BUZZER4                            = 0x00000080,  //8
            //
            OUT1_ALL_DOOR_UNLOCK                    = 0x00000001,  //9
            TEMP10                                  = 0x00000002,
            TEMP11                                  = 0x00000004,
            TEMP12                                  = 0x00000008,  //12
            OUT1_LEFT_MAGAZINE_DOOR_LOCK_UP         = 0x00000010,
            OUT1_LEFT_MAGAZINE_DOOR_LOCK_DOWN       = 0x00000020,
            OUT1_RIGHT_MAGAZINE_DOOR_LOCK_UP        = 0x00000040,
            OUT1_RIGHT_MAGAZINE_DOOR_LOCK_DOWN      = 0x00000080,  //16
            //
            OUT1_LEFT_MAGAZINE_LOAD_REQ             = 0x00000001,  //17
            OUT1_LEFT_MAGAZINE_LOAD_COMPLETE        = 0x00000002,
            OUT1_RIGHT_MAGAZINE_LOAD_REQ            = 0x00000004,
            OUT1_RIGHT_MAGAZINE_LOAD_COMPLETE       = 0x00000008,  //20
            OUT1_LEFT_NG_TRAY_LOCK_LAMP             = 0x00000010,
            OUT1_LEFT_NG_TRAY_UNLOCK_LAMP           = 0x00000020,
            OUT1_RIGHT_NG_TRAY_LOCK_LAMP            = 0x00000040,
            OUT1_RIGHT_NG_TRAY_UNLOCK_LAMP          = 0x00000080,  //24
            //
            OUT1_LEFT_NG_TRAY_LOCK_UP               = 0x00000001,  //25
            OUT1_LEFT_NG_TRAY_LOCK_DOWN             = 0x00000002,
            OUT1_RIGHT_NG_TRAY_LOCK_UP              = 0x00000004,
            OUT1_RIGHT_NG_TRAY_LOCK_DOWN            = 0x00000008,  //28
            TEMP29                                  = 0x00000010,
            TEMP30                                  = 0x00000020,
            TEMP31                                  = 0x00000040,
            TEMP32                                  = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 3
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH1 : uint
        {
            OUT3_TRANSFER_LOAD_PICKER_DOWN1         = 0x00000001,            //1
            OUT3_TRANSFER_LOAD_PICKER_DOWN2         = 0x00000002,
            OUT3_TRANSFER_LOAD_PICKER_DOWN3         = 0x00000004,
            OUT3_TRANSFER_LOAD_PICKER_DOWN4         = 0x00000008,            //4
            OUT3_TRANSFER_LOAD_PICKER_UP1           = 0x00000010,
            OUT3_TRANSFER_LOAD_PICKER_UP2           = 0x00000020,
            OUT3_TRANSFER_LOAD_PICKER_UP3           = 0x00000040,
            OUT3_TRANSFER_LOAD_PICKER_UP4           = 0x00000080,                 //8
            //
            OUT3_TRANSFER_UNLOAD_PICKER_DOWN1       = 0x00000001,                  //9
            OUT3_TRANSFER_UNLOAD_PICKER_DOWN2       = 0x00000002,
            OUT3_TRANSFER_UNLOAD_PICKER_DOWN3       = 0x00000004,
            OUT3_TRANSFER_UNLOAD_PICKER_DOWN4       = 0x00000008,          //12
            OUT3_TRANSFER_UNLOAD_PICKER_UP1         = 0x00000010,
            OUT3_TRANSFER_UNLOAD_PICKER_UP2         = 0x00000020,
            OUT3_TRANSFER_UNLOAD_PICKER_UP3         = 0x00000040,
            OUT3_TRANSFER_UNLOAD_PICKER_UP4         = 0x00000080,                  //16
            //                      
            OUT3_TRANSFER_LOAD_PICKER_VACUUM_ON1       = 0x00000001,            //17
            OUT3_TRANSFER_LOAD_PICKER_VACUUM_ON2       = 0x00000002,
            OUT3_TRANSFER_LOAD_PICKER_VACUUM_ON3       = 0x00000004,
            OUT3_TRANSFER_LOAD_PICKER_VACUUM_ON4       = 0x00000008,          //20
            OUT3_TRANSFER_LOAD_PICKER_BLOW_ON1         = 0x00000010,
            OUT3_TRANSFER_LOAD_PICKER_BLOW_ON2         = 0x00000020,
            OUT3_TRANSFER_LOAD_PICKER_BLOW_ON3         = 0x00000040,
            OUT3_TRANSFER_LOAD_PICKER_BLOW_ON4         = 0x00000080,                  //24
            //
            OUT3_TRANSFER_UNLOAD_PICKER_GRIP1       = 0x00000001,                  //25
            OUT3_TRANSFER_UNLOAD_PICKER_GRIP2       = 0x00000002,
            OUT3_TRANSFER_UNLOAD_PICKER_GRIP3       = 0x00000004,
            OUT3_TRANSFER_UNLOAD_PICKER_GRIP4       = 0x00000008,                  //28
            OUT3_TRANSFER_UNLOAD_PICKER_UNGRIP1     = 0x00000010,
            OUT3_TRANSFER_UNLOAD_PICKER_UNGRIP2     = 0x00000020,
            OUT3_TRANSFER_UNLOAD_PICKER_UNGRIP3     = 0x00000040,
            OUT3_TRANSFER_UNLOAD_PICKER_UNGRIP4     = 0x00000080                    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 5
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH2 : uint
        {
            OUT5_A_SOCKET_CONTACT_UP1           = 0x00000001,  //1
            OUT5_A_SOCKET_CONTACT_UP2           = 0x00000002,
            OUT5_A_SOCKET_CONTACT_UP3           = 0x00000004,
            OUT5_A_SOCKET_CONTACT_UP4           = 0x00000008,  //4
            OUT5_A_SOCKET_CONTACT_DOWN1         = 0x00000010,
            OUT5_A_SOCKET_CONTACT_DOWN2         = 0x00000020,
            OUT5_A_SOCKET_CONTACT_DOWN3         = 0x00000040,
            OUT5_A_SOCKET_CONTACT_DOWN4         = 0x00000080,  //8
            //
            OUT5_A_SOCKET_CONTACT_FORWARD1      = 0x00000001,  //9
            OUT5_A_SOCKET_CONTACT_FORWARD2      = 0x00000002,
            OUT5_A_SOCKET_CONTACT_FORWARD3      = 0x00000004,
            OUT5_A_SOCKET_CONTACT_FORWARD4      = 0x00000008,  //12
            OUT5_A_SOCKET_CONTACT_BACKWARD1     = 0x00000010,
            OUT5_A_SOCKET_CONTACT_BACKWARD2     = 0x00000020,
            OUT5_A_SOCKET_CONTACT_BACKWARD3     = 0x00000040,
            OUT5_A_SOCKET_CONTACT_BACKWARD4     = 0x00000080,  //16
            //                      
            OUT5_B_SOCKET_CONTACT_UP1           = 0x00000001,  //17
            OUT5_B_SOCKET_CONTACT_UP2           = 0x00000002,
            OUT5_B_SOCKET_CONTACT_UP3           = 0x00000004,
            OUT5_B_SOCKET_CONTACT_UP4           = 0x00000008,  //20
            OUT5_B_SOCKET_CONTACT_DOWN1         = 0x00000010,
            OUT5_B_SOCKET_CONTACT_DOWN2         = 0x00000020,
            OUT5_B_SOCKET_CONTACT_DOWN3         = 0x00000040,
            OUT5_B_SOCKET_CONTACT_DOWN4         = 0x00000080,  //24
            //
            OUT5_B_SOCKET_CONTACT_FORWARD1      = 0x00000001,  //25
            OUT5_B_SOCKET_CONTACT_FORWARD2      = 0x00000002,
            OUT5_B_SOCKET_CONTACT_FORWARD3      = 0x00000004,
            OUT5_B_SOCKET_CONTACT_FORWARD4      = 0x00000008,  //28
            OUT5_B_SOCKET_CONTACT_BACKWARD1     = 0x00000010,
            OUT5_B_SOCKET_CONTACT_BACKWARD2     = 0x00000020,
            OUT5_B_SOCKET_CONTACT_BACKWARD3     = 0x00000040,
            OUT5_B_SOCKET_CONTACT_BACKWARD4     = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 7
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH3 : uint
        {
            OUT7_C_SOCKET_CONTACT_UP1           = 0x00000001,  //1
            OUT7_C_SOCKET_CONTACT_UP2           = 0x00000002,
            OUT7_C_SOCKET_CONTACT_UP3           = 0x00000004,
            OUT7_C_SOCKET_CONTACT_UP4           = 0x00000008,  //4
            OUT7_C_SOCKET_CONTACT_DOWN1         = 0x00000010,
            OUT7_C_SOCKET_CONTACT_DOWN2         = 0x00000020,
            OUT7_C_SOCKET_CONTACT_DOWN3         = 0x00000040,
            OUT7_C_SOCKET_CONTACT_DOWN4         = 0x00000080,  //8
            //
            OUT7_C_SOCKET_CONTACT_FORWARD1      = 0x00000001,  //9
            OUT7_C_SOCKET_CONTACT_FORWARD2      = 0x00000002,
            OUT7_C_SOCKET_CONTACT_FORWARD3      = 0x00000004,
            OUT7_C_SOCKET_CONTACT_FORWARD4      = 0x00000008,  //12
            OUT7_C_SOCKET_CONTACT_BACKWARD1     = 0x00000010,
            OUT7_C_SOCKET_CONTACT_BACKWARD2     = 0x00000020,
            OUT7_C_SOCKET_CONTACT_BACKWARD3     = 0x00000040,
            OUT7_C_SOCKET_CONTACT_BACKWARD4     = 0x00000080,  //16
            //                      
            OUT7_D_SOCKET_CONTACT_UP1           = 0x00000001,  //17
            OUT7_D_SOCKET_CONTACT_UP2           = 0x00000002,
            OUT7_D_SOCKET_CONTACT_UP3           = 0x00000004,
            OUT7_D_SOCKET_CONTACT_UP4           = 0x00000008,  //20
            OUT7_D_SOCKET_CONTACT_DOWN1         = 0x00000010,
            OUT7_D_SOCKET_CONTACT_DOWN2         = 0x00000020,
            OUT7_D_SOCKET_CONTACT_DOWN3         = 0x00000040,
            OUT7_D_SOCKET_CONTACT_DOWN4         = 0x00000080,  //24
            //
            OUT7_D_SOCKET_CONTACT_FORWARD1      = 0x00000001,  //25
            OUT7_D_SOCKET_CONTACT_FORWARD2      = 0x00000002,
            OUT7_D_SOCKET_CONTACT_FORWARD3      = 0x00000004,
            OUT7_D_SOCKET_CONTACT_FORWARD4      = 0x00000008,  //28
            OUT7_D_SOCKET_CONTACT_BACKWARD1     = 0x00000010,
            OUT7_D_SOCKET_CONTACT_BACKWARD2     = 0x00000020,
            OUT7_D_SOCKET_CONTACT_BACKWARD3     = 0x00000040,
            OUT7_D_SOCKET_CONTACT_BACKWARD4     = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 9
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH4 : uint
        {
            OUT9_A_SOCKET_ROTATE_UP1            = 0x00000001,  //1
            OUT9_A_SOCKET_ROTATE_UP2            = 0x00000002,
            OUT9_A_SOCKET_ROTATE_UP3            = 0x00000004,
            OUT9_A_SOCKET_ROTATE_UP4            = 0x00000008,  //4
            OUT9_A_SOCKET_ROTATE_DOWN1          = 0x00000010,
            OUT9_A_SOCKET_ROTATE_DOWN2          = 0x00000020,
            OUT9_A_SOCKET_ROTATE_DOWN3          = 0x00000040,
            OUT9_A_SOCKET_ROTATE_DOWN4          = 0x00000080,  //8
            //
            OUT9_A_SOCKET_ROTATE_TURN_LEFT1     = 0x00000001,  //9
            OUT9_A_SOCKET_ROTATE_TURN_LEFT2     = 0x00000002,
            OUT9_A_SOCKET_ROTATE_TURN_LEFT3     = 0x00000004,
            OUT9_A_SOCKET_ROTATE_TURN_LEFT4     = 0x00000008,  //12
            OUT9_A_SOCKET_ROTATE_TURN_HOME1     = 0x00000010,
            OUT9_A_SOCKET_ROTATE_TURN_HOME2     = 0x00000020,
            OUT9_A_SOCKET_ROTATE_TURN_HOME3     = 0x00000040,
            OUT9_A_SOCKET_ROTATE_TURN_HOME4     = 0x00000080,  //16
            //                      
            OUT9_A_SOCKET_ROTATE_GRIP1          = 0x00000001,  //17
            OUT9_A_SOCKET_ROTATE_GRIP2          = 0x00000002,
            OUT9_A_SOCKET_ROTATE_GRIP3          = 0x00000004,
            OUT9_A_SOCKET_ROTATE_GRIP4          = 0x00000008,  //20
            OUT9_A_SOCKET_ROTATE_UNGRIP1        = 0x00000010,
            OUT9_A_SOCKET_ROTATE_UNGRIP2        = 0x00000020,
            OUT9_A_SOCKET_ROTATE_UNGRIP3        = 0x00000040,
            OUT9_A_SOCKET_ROTATE_UNGRIP4        = 0x00000080,  //24
            //
            OUT9_A_SOCKET_ROTATE_VACUUM1        = 0x00000001,  //25
            OUT9_A_SOCKET_ROTATE_VACUUM2        = 0x00000002,
            OUT9_A_SOCKET_ROTATE_VACUUM3        = 0x00000004,
            OUT9_A_SOCKET_ROTATE_VACUUM4        = 0x00000008,  //28
            OUT9_A_SOCKET_ROTATE_BLOW1          = 0x00000010,
            OUT9_A_SOCKET_ROTATE_BLOW2          = 0x00000020,
            OUT9_A_SOCKET_ROTATE_BLOW3          = 0x00000040,
            OUT9_A_SOCKET_ROTATE_BLOW4          = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 11
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH5 : uint
        {
            OUT11_B_SOCKET_ROTATE_UP1           = 0x00000001,  //1
            OUT11_B_SOCKET_ROTATE_UP2           = 0x00000002,
            OUT11_B_SOCKET_ROTATE_UP3           = 0x00000004,
            OUT11_B_SOCKET_ROTATE_UP4           = 0x00000008,  //4
            OUT11_B_SOCKET_ROTATE_DOWN1         = 0x00000010,
            OUT11_B_SOCKET_ROTATE_DOWN2         = 0x00000020,
            OUT11_B_SOCKET_ROTATE_DOWN3         = 0x00000040,
            OUT11_B_SOCKET_ROTATE_DOWN4         = 0x00000080,  //8
            //
            OUT11_B_SOCKET_ROTATE_TURN_LEFT1    = 0x00000001,  //9
            OUT11_B_SOCKET_ROTATE_TURN_LEFT2    = 0x00000002,
            OUT11_B_SOCKET_ROTATE_TURN_LEFT3    = 0x00000004,
            OUT11_B_SOCKET_ROTATE_TURN_LEFT4    = 0x00000008,  //12
            OUT11_B_SOCKET_ROTATE_TURN_HOME1    = 0x00000010,
            OUT11_B_SOCKET_ROTATE_TURN_HOME2    = 0x00000020,
            OUT11_B_SOCKET_ROTATE_TURN_HOME3    = 0x00000040,
            OUT11_B_SOCKET_ROTATE_TURN_HOME4    = 0x00000080,  //16
            //                      
            OUT11_B_SOCKET_ROTATE_GRIP1         = 0x00000001,  //17
            OUT11_B_SOCKET_ROTATE_GRIP2         = 0x00000002,
            OUT11_B_SOCKET_ROTATE_GRIP3         = 0x00000004,
            OUT11_B_SOCKET_ROTATE_GRIP4         = 0x00000008,  //20
            OUT11_B_SOCKET_ROTATE_UNGRIP1       = 0x00000010,
            OUT11_B_SOCKET_ROTATE_UNGRIP2       = 0x00000020,
            OUT11_B_SOCKET_ROTATE_UNGRIP3       = 0x00000040,
            OUT11_B_SOCKET_ROTATE_UNGRIP4       = 0x00000080,  //24
            //
            OUT11_B_SOCKET_ROTATE_VACUUM1       = 0x00000001,  //25
            OUT11_B_SOCKET_ROTATE_VACUUM2       = 0x00000002,
            OUT11_B_SOCKET_ROTATE_VACUUM3       = 0x00000004,
            OUT11_B_SOCKET_ROTATE_VACUUM4       = 0x00000008,  //28
            OUT11_B_SOCKET_ROTATE_BLOW1         = 0x00000010,
            OUT11_B_SOCKET_ROTATE_BLOW2         = 0x00000020,
            OUT11_B_SOCKET_ROTATE_BLOW3         = 0x00000040,
            OUT11_B_SOCKET_ROTATE_BLOW4         = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 13
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH6 : uint
        {
            OUT13_C_SOCKET_ROTATE_UP1           = 0x00000001,  //1
            OUT13_C_SOCKET_ROTATE_UP2           = 0x00000002,
            OUT13_C_SOCKET_ROTATE_UP3           = 0x00000004,
            OUT13_C_SOCKET_ROTATE_UP4           = 0x00000008,  //4
            OUT13_C_SOCKET_ROTATE_DOWN1         = 0x00000010,
            OUT13_C_SOCKET_ROTATE_DOWN2         = 0x00000020,
            OUT13_C_SOCKET_ROTATE_DOWN3         = 0x00000040,
            OUT13_C_SOCKET_ROTATE_DOWN4         = 0x00000080,  //8
            //
            OUT13_C_SOCKET_ROTATE_TURN_LEFT1    = 0x00000001,  //9
            OUT13_C_SOCKET_ROTATE_TURN_LEFT2    = 0x00000002,
            OUT13_C_SOCKET_ROTATE_TURN_LEFT3    = 0x00000004,
            OUT13_C_SOCKET_ROTATE_TURN_LEFT4    = 0x00000008,  //12
            OUT13_C_SOCKET_ROTATE_TURN_HOME1    = 0x00000010,
            OUT13_C_SOCKET_ROTATE_TURN_HOME2    = 0x00000020,
            OUT13_C_SOCKET_ROTATE_TURN_HOME3    = 0x00000040,
            OUT13_C_SOCKET_ROTATE_TURN_HOME4    = 0x00000080,  //16
            //                      
            OUT13_C_SOCKET_ROTATE_GRIP1         = 0x00000001,  //17
            OUT13_C_SOCKET_ROTATE_GRIP2         = 0x00000002,
            OUT13_C_SOCKET_ROTATE_GRIP3         = 0x00000004,
            OUT13_C_SOCKET_ROTATE_GRIP4         = 0x00000008,  //20
            OUT13_C_SOCKET_ROTATE_UNGRIP1       = 0x00000010,
            OUT13_C_SOCKET_ROTATE_UNGRIP2       = 0x00000020,
            OUT13_C_SOCKET_ROTATE_UNGRIP3       = 0x00000040,
            OUT13_C_SOCKET_ROTATE_UNGRIP4       = 0x00000080,  //24
            //
            OUT13_C_SOCKET_ROTATE_VACUUM1       = 0x00000001,  //25
            OUT13_C_SOCKET_ROTATE_VACUUM2       = 0x00000002,
            OUT13_C_SOCKET_ROTATE_VACUUM3       = 0x00000004,
            OUT13_C_SOCKET_ROTATE_VACUUM4       = 0x00000008,  //28
            OUT13_C_SOCKET_ROTATE_BLOW1         = 0x00000010,
            OUT13_C_SOCKET_ROTATE_BLOW2         = 0x00000020,
            OUT13_C_SOCKET_ROTATE_BLOW3         = 0x00000040,
            OUT13_C_SOCKET_ROTATE_BLOW4         = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 15
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH7 : uint
        {
            OUT15_D_SOCKET_ROTATE_UP1           = 0x00000001,  //1
            OUT15_D_SOCKET_ROTATE_UP2           = 0x00000002,
            OUT15_D_SOCKET_ROTATE_UP3           = 0x00000004,
            OUT15_D_SOCKET_ROTATE_UP4           = 0x00000008,  //4
            OUT15_D_SOCKET_ROTATE_DOWN1         = 0x00000010,
            OUT15_D_SOCKET_ROTATE_DOWN2         = 0x00000020,
            OUT15_D_SOCKET_ROTATE_DOWN3         = 0x00000040,
            OUT15_D_SOCKET_ROTATE_DOWN4         = 0x00000080,  //8
            //
            OUT15_D_SOCKET_ROTATE_TURN_LEFT1    = 0x00000001,  //9
            OUT15_D_SOCKET_ROTATE_TURN_LEFT2    = 0x00000002,
            OUT15_D_SOCKET_ROTATE_TURN_LEFT3    = 0x00000004,
            OUT15_D_SOCKET_ROTATE_TURN_LEFT4    = 0x00000008,  //12
            OUT15_D_SOCKET_ROTATE_TURN_HOME1    = 0x00000010,
            OUT15_D_SOCKET_ROTATE_TURN_HOME2    = 0x00000020,
            OUT15_D_SOCKET_ROTATE_TURN_HOME3    = 0x00000040,
            OUT15_D_SOCKET_ROTATE_TURN_HOME4    = 0x00000080,  //16
            //                      
            OUT15_D_SOCKET_ROTATE_GRIP1         = 0x00000001,  //17
            OUT15_D_SOCKET_ROTATE_GRIP2         = 0x00000002,
            OUT15_D_SOCKET_ROTATE_GRIP3         = 0x00000004,
            OUT15_D_SOCKET_ROTATE_GRIP4         = 0x00000008,  //20
            OUT15_D_SOCKET_ROTATE_UNGRIP1       = 0x00000010,
            OUT15_D_SOCKET_ROTATE_UNGRIP2       = 0x00000020,
            OUT15_D_SOCKET_ROTATE_UNGRIP3       = 0x00000040,
            OUT15_D_SOCKET_ROTATE_UNGRIP4       = 0x00000080,  //24
            //
            OUT15_D_SOCKET_ROTATE_VACUUM1       = 0x00000001,  //25
            OUT15_D_SOCKET_ROTATE_VACUUM2       = 0x00000002,
            OUT15_D_SOCKET_ROTATE_VACUUM3       = 0x00000004,
            OUT15_D_SOCKET_ROTATE_VACUUM4       = 0x00000008,  //28
            OUT15_D_SOCKET_ROTATE_BLOW1         = 0x00000010,
            OUT15_D_SOCKET_ROTATE_BLOW2         = 0x00000020,
            OUT15_D_SOCKET_ROTATE_BLOW3         = 0x00000040,
            OUT15_D_SOCKET_ROTATE_BLOW4         = 0x00000080   //32
        };
    }//END
}
