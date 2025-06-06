using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.MotionControl
{
    public class AoiDioDefine : IDioDefine
    {
        public uint GetInLoadPickerUpDown(int index, bool bFlag)
        {
            if (bFlag)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH1.IN2_TRANSFER_LOAD_PICKER_UP1;
                    case 1: return (uint)DIO_IN_ADDR_CH1.IN2_TRANSFER_LOAD_PICKER_UP2;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetInLoadPickerUpDown 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH1.IN2_TRANSFER_LOAD_PICKER_DOWN1;
                    case 1: return (uint)DIO_IN_ADDR_CH1.IN2_TRANSFER_LOAD_PICKER_DOWN2;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetInLoadPickerUpDown 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            
        }
        public uint GetInUnloadPickerUpDown(int index, bool bFlag)
        {
            if (bFlag)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH1.IN2_TRANSFER_UNLOAD_PICKER_UP1;
                    case 1: return (uint)DIO_IN_ADDR_CH1.IN2_TRANSFER_UNLOAD_PICKER_UP2;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetInUnloadPickerUpDown 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH1.IN2_TRANSFER_UNLOAD_PICKER_DOWN1;
                    case 1: return (uint)DIO_IN_ADDR_CH1.IN2_TRANSFER_UNLOAD_PICKER_DOWN2;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetInUnloadPickerUpDown 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }

        }
        public uint GetOutBuzzer(int nType)
        {
            switch (nType)
            {
                case 0: return (uint)DIO_OUT_ADDR_CH1.OUT0_BUZZER1;
                case 1: return (uint)DIO_OUT_ADDR_CH1.OUT0_BUZZER2;
                case 2: return (uint)DIO_OUT_ADDR_CH1.OUT0_BUZZER3;
                case 3: return (uint)DIO_OUT_ADDR_CH1.OUT0_BUZZER4;
                default:
                    // 로그 남기기 (예: LogHelper.Write)
                    Console.WriteLine($"[Warning] 잘못된 Buzzer 타입 요청됨: {nType}");
                    //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                    return 0; // 또는 사용하지 않는 안전한 비트
            }
        }
        public uint GetOutTowerLamp(int nType)
        {
            switch (nType)
            {
                case 0: return (uint)DIO_OUT_ADDR_CH1.OUT0_TOWER_LAMP_R;
                case 1: return (uint)DIO_OUT_ADDR_CH1.OUT0_TOWER_LAMP_Y;
                case 2: return (uint)DIO_OUT_ADDR_CH1.OUT0_TOWER_LAMP_G;
                default:
                    // 로그 남기기 (예: LogHelper.Write)
                    Console.WriteLine($"[Warning] 잘못된 TowerLamp 타입 요청됨: {nType}");
                    //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                    return 0; // 또는 사용하지 않는 안전한 비트
            }
        }
        public uint GetOutAllDoor()
        {
            return (uint)DIO_OUT_ADDR_CH1.OUT0_ALL_DOOR_UNLOCK;
        }
        public uint GetInAllDoor(int nType)
        {
            switch (nType)
            {
                case 0: return (uint)DIO_IN_ADDR_CH0.IN0_DOOR_UNLOCK_FRONT_L;
                case 1: return (uint)DIO_IN_ADDR_CH0.IN0_DOOR_UNLOCK_FRONT_R;
                case 2: return (uint)DIO_IN_ADDR_CH0.IN0_DOOR_UNLOCK_BACK_L;
                case 3: return (uint)DIO_IN_ADDR_CH0.IN0_DOOR_UNLOCK_BACK_R;
                default:
                    // 로그 남기기 (예: LogHelper.Write)
                    Console.WriteLine($"[Warning] 잘못된 GetInAllDoor 타입 요청됨: {nType}");
                    //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                    return 0; // 또는 사용하지 않는 안전한 비트
            }

        }
        public uint GetOutLiftDoor(int nType, int index)
        {
            if (nType == 0)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT2_LIFT_LEFT_DOOR_LOCK_UP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT2_LIFT_LEFT_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutLiftDoor 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT2_LIFT_RIGHT_DOOR_LOCK_UP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT2_LIFT_RIGHT_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutLiftDoor 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
        }
        public uint GetInLiftDoor(int nType, int index)
        {
            if (nType == 0)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH0.IN0_LIFT_LEFT_DOOR_LOCK_UP;
                    case 1: return (uint)DIO_IN_ADDR_CH0.IN0_LIFT_LEFT_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetInLiftDoor 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH0.IN0_LIFT_RIGHT_DOOR_LOCK_UP;
                    case 1: return (uint)DIO_IN_ADDR_CH0.IN0_LIFT_RIGHT_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetInLiftDoor 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
        }
        public uint GetOutLiftLamp(int nType, int index)
        {
            if (nType == 0)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT1_LEFT_LIFT_LOAD_MODE_LAMP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT1_LEFT_LIFT_COMPLETE_MODE_LAMP;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutLiftLamp 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT1_RIGHT_LIFT_LOAD_MODE_LAMP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT1_RIGHT_LIFT_COMPLETE_MODE_LAMP;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutLiftLamp 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
        }
        public uint GetOutNgTrayDoor(int nType, int index)
        {
            if (nType == 0)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT3_NG_TRAY_LEFT_DOOR_LOCK_UP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT3_NG_TRAY_LEFT_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutNgTrayDoor 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT3_NG_TRAY_RIGHT_DOOR_LOCK_UP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT3_NG_TRAY_RIGHT_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutNgTrayDoor 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
        }
        public uint GetInNgTrayDoor(int nType, int index)
        {
            if (nType == 0)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH0.IN0_NG_TRAY_LEFT_DOOR_LOCK_UP;
                    case 1: return (uint)DIO_IN_ADDR_CH0.IN0_NG_TRAY_LEFT_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetInNgTrayDoor 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH0.IN0_NG_TRAY_RIGHT_DOOR_LOCK_UP;
                    case 1: return (uint)DIO_IN_ADDR_CH0.IN0_NG_TRAY_RIGHT_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetInNgTrayDoor 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
        }
        public uint GetOutNgTrayLamp(int nType, int index)
        {
            if (nType == 0)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT3_LEFT_NG_TRAY_LOAD_MODE_LAMP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT3_LEFT_NG_TRAY_COMPLETE_MODE_LAMP;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutNgTrayLamp 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT3_RIGHT_NG_TRAY_LOAD_MODE_LAMP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT3_RIGHT_NG_TRAY_COMPLETE_MODE_LAMP;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutNgTrayLamp 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
        }
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
            IN0_DOOR_UNLOCK_FRONT_L             = 0x00000010,
            IN0_DOOR_UNLOCK_FRONT_R             = 0x00000020,
            IN0_DOOR_UNLOCK_BACK_L              = 0x00000040,
            IN0_DOOR_UNLOCK_BACK_R              = 0x00000080,  //8
            //
            IN0_LEFT_LIFT_LOAD_MODE             = 0x00000001,  //9
            IN0_LEFT_LIFT_COMPLETE_MODE         = 0x00000002,
            IN0_RIGHT_LIFT_LOAD_MODE            = 0x00000004,
            IN0_RIGHT_LIFT_COMPLETE_MODE        = 0x00000008,  //12
            IN0_A_SOCKET_VACUUM_ON1             = 0x00000010,
            IN0_A_SOCKET_VACUUM_ON2             = 0x00000020,
            IN0_B_SOCKET_VACUUM_ON1             = 0x00000040,
            IN0_B_SOCKET_VACUUM_ON2             = 0x00000080,  //16
            //
            IN0_LIFT_LEFT_DOOR_LOCK_UP          = 0x00000001,  //17
            IN0_LIFT_LEFT_DOOR_LOCK_DOWN        = 0x00000002,
            IN0_LIFT_RIGHT_DOOR_LOCK_UP         = 0x00000004,
            IN0_LIFT_RIGHT_DOOR_LOCK_DOWN       = 0x00000008,  //20
            IN0_LEFT_LIFT_DOOR_CLOSE            = 0x00000010,
            IN0_RIGHT_LIFT_DOOR_CLOSE           = 0x00000020,
            TEMP23                              = 0x00000040,
            TEMP24                              = 0x00000080,  //24
            //
            IN0_NG_TRAY_LEFT_DOOR_CLOSE         = 0x00000001,  //25
            IN0_NG_TRAY_LEFT_DOOR_OPEN          = 0x00000002,
            IN0_NG_TRAY_RIGHT_DOOR_CLOSE        = 0x00000004,
            IN0_NG_TRAY_RIGHT_DOOR_OPEN         = 0x00000008,  //28
            IN0_NG_TRAY_LEFT_DOOR_LOCK_UP       = 0x00000010,
            IN0_NG_TRAY_LEFT_DOOR_LOCK_DOWN     = 0x00000020,
            IN0_NG_TRAY_RIGHT_DOOR_LOCK_UP      = 0x00000040,
            IN0_NG_TRAY_RIGHT_DOOR_LOCK_DOWN    = 0x00000080   //32
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
            IN2_TRANSFER_LOAD_PICKER_UP1        = 0x00000004,
            IN2_TRANSFER_LOAD_PICKER_UP2        = 0x00000008,  //4
            IN2_TRANSFER_UNLOAD_PICKER_DOWN1    = 0x00000010,
            IN2_TRANSFER_UNLOAD_PICKER_DOWN2    = 0x00000020,
            IN2_TRANSFER_UNLOAD_PICKER_UP1      = 0x00000040,
            IN2_TRANSFER_UNLOAD_PICKER_UP2      = 0x00000080,  //8
            //
            TEMP9 = 0x00000001,  //9
            TEMP10 = 0x00000002,
            TEMP11 = 0x00000004,
            TEMP12 = 0x00000008,  //12
            TEMP13 = 0x00000010,
            TEMP14 = 0x00000020,
            TEMP15 = 0x00000040,
            TEMP16 = 0x00000080,  //16
            //
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON1     = 0x00000001,  //17
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON2     = 0x00000002,
            TEMP19 = 0x00000004,
            TEMP20 = 0x00000008,  //20
            IN2_TRANSFER_UNLOAD_PICKER_VACUUM_ON1   = 0x00000010,
            IN2_TRANSFER_UNLOAD_PICKER_VACUUM_ON2   = 0x00000020,
            TEMP23 = 0x00000040,
            TEMP24 = 0x00000080,  //24
            //
            IN0_A_SOCKET_GOOD_DETECT_L              = 0x00000001,   //25
            IN0_A_SOCKET_GOOD_DETECT_R              = 0x00000002,
            IN0_B_SOCKET_GOOD_DETECT_L              = 0x00000004,
            IN0_B_SOCKET_GOOD_DETECT_R              = 0x00000008,   //28
            TEMP29 = 0x00000010,
            TEMP30 = 0x00000020,
            IN0_NG_TRAY_DETECTED1                   = 0x00000040,
            IN0_NG_TRAY_DETECTED2                   = 0x00000080    //32
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
        public enum DIO_OUT_ADDR_CH1 : uint
        {
            OUT0_TOWER_LAMP_R                       = 0x00000001,  //1
            OUT0_TOWER_LAMP_Y                       = 0x00000002,
            OUT0_TOWER_LAMP_G                       = 0x00000004,
            OUT0_ALL_DOOR_UNLOCK                    = 0x00000008,  //4
            OUT0_BUZZER1                            = 0x00000010,
            OUT0_BUZZER2                            = 0x00000020,
            OUT0_BUZZER3                            = 0x00000040,
            OUT0_BUZZER4                            = 0x00000080,  //8
            //
            OUT1_LEFT_LIFT_LOAD_MODE_LAMP           = 0x00000001,  //9
            OUT1_LEFT_LIFT_COMPLETE_MODE_LAMP       = 0x00000002,
            OUT1_RIGHT_LIFT_LOAD_MODE_LAMP          = 0x00000004,
            OUT1_RIGHT_LIFT_COMPLETE_MODE_LAMP      = 0x00000008,   //12
            OUT1_LEFT_SOCKET_VACUUM_ON1             = 0x00000010,
            OUT1_LEFT_SOCKET_VACUUM_ON2             = 0x00000020,
            OUT1_RIGHT_SOCKET_VACUUM_ON1            = 0x00000040,
            OUT1_RIGHT_SOCKET_VACUUM_ON2            = 0x00000080,  //16
            //
            OUT2_LEFT_SOCKET_BLOW_ON1               = 0x00000001,  //17
            OUT2_LEFT_SOCKET_BLOW_ON2               = 0x00000002,
            OUT2_RIGHT_SOCKET_BLOW_ON1              = 0x00000004,
            OUT2_RIGHT_SOCKET_BLOW_ON2              = 0x00000008,  //20
            OUT2_LIFT_LEFT_DOOR_LOCK_UP             = 0x00000010,
            OUT2_LIFT_LEFT_DOOR_LOCK_DOWN           = 0x00000020,
            OUT2_LIFT_RIGHT_DOOR_LOCK_UP            = 0x00000040,
            OUT2_LIFT_RIGHT_DOOR_LOCK_DOWN          = 0x00000080,  //24
            //
            OUT3_LEFT_NG_TRAY_LOAD_MODE_LAMP            = 0x00000001,  //25
            OUT3_LEFT_NG_TRAY_COMPLETE_MODE_LAMP        = 0x00000002,
            OUT3_RIGHT_NG_TRAY_LOAD_MODE_LAMP           = 0x00000004,
            OUT3_RIGHT_NG_TRAY_COMPLETE_MODE_LAMP       = 0x00000008,  //28
            OUT3_NG_TRAY_LEFT_DOOR_LOCK_UP              = 0x00000010,
            OUT3_NG_TRAY_LEFT_DOOR_LOCK_DOWN            = 0x00000020,
            OUT3_NG_TRAY_RIGHT_DOOR_LOCK_UP             = 0x00000040,
            OUT3_NG_TRAY_RIGHT_DOOR_LOCK_DOWN           = 0x00000080   //32
        };
        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 3
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH3 : uint
        {
            OUT3_TRANSFER_LOAD_PICKER_DOWN1         = 0x00000001,   //1
            OUT3_TRANSFER_LOAD_PICKER_DOWN2         = 0x00000002,
            TEMP3                                   = 0x00000004,
            TEMP4                                   = 0x00000008,   //4
            OUT3_TRANSFER_LOAD_PICKER_UP1           = 0x00000010,
            OUT3_TRANSFER_LOAD_PICKER_UP2           = 0x00000020,
            TEMP7                                   = 0x00000040,
            TEMP8                                   = 0x00000080,   //8
            //
            OUT3_TRANSFER_UNLOAD_PICKER_DOWN1       = 0x00000001,   //9
            OUT3_TRANSFER_UNLOAD_PICKER_DOWN2       = 0x00000002,
            TEMP11                                  = 0x00000004,
            TEMP12                                  = 0x00000008,   //12
            OUT3_TRANSFER_UNLOAD_PICKER_UP1         = 0x00000010,
            OUT3_TRANSFER_UNLOAD_PICKER_UP2         = 0x00000020,
            TEMP15                                  = 0x00000040,
            TEMP16                                  = 0x00000080,   //16
            //                      
            OUT3_TRANSFER_LOAD_PICKER_VACUUM_ON1    = 0x00000001,   //17
            OUT3_TRANSFER_LOAD_PICKER_VACUUM_ON2    = 0x00000002,
            TEMP19                                  = 0x00000004,
            TEMP20                                  = 0x00000008,   //20
            OUT3_TRANSFER_LOAD_PICKER_BLOW_ON1      = 0x00000010,
            OUT3_TRANSFER_LOAD_PICKER_BLOW_ON2      = 0x00000020,
            TEMP23                                  = 0x00000040,
            TEMP24                                  = 0x00000080,   //24
            //
            OUT3_TRANSFER_UNLOAD_PICKER_VACUUM_ON1  = 0x00000001,   //25
            OUT3_TRANSFER_UNLOAD_PICKER_VACUUM_ON2  = 0x00000002,
            TEMP27                                  = 0x00000004,
            TEMP28                                  = 0x00000008,   //28
            OUT3_TRANSFER_UNLOAD_PICKER_BLOW_ON1    = 0x00000010,
            OUT3_TRANSFER_UNLOAD_PICKER_BLOW_ON2    = 0x00000020,
            TEMP31                                  = 0x00000040,
            TEMP32                                  = 0x00000080    //32
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
    }//END
}
