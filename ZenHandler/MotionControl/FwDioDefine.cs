using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.MotionControl
{
    public class FwDioDefine
    {
        public uint GetInMagazineDocked(int index)
        {
            switch (index)
            {
                case 0: return (uint)DIO_IN_ADDR_CH0.IN2_LEFT_MAGAZINE_DOCKED;
                case 1: return (uint)DIO_IN_ADDR_CH0.IN3_RIGHT_MAGAZINE_DOCKED;
                default:
                    // 로그 남기기 (예: LogHelper.Write)
                    Console.WriteLine($"[Warning] 잘못된 GetInMagazineDocked 타입 요청됨:{index}");
                    //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                    return 0; // 또는 사용하지 않는 안전한 비트
            }
        }
        public uint GetInMagazineBottom(int index)
        {
            switch (index)
            {
                case 0: return (uint)DIO_IN_ADDR_CH0.IN2_LEFT_MAGAZINE_BOTTOM_DETECTED;
                case 1: return (uint)DIO_IN_ADDR_CH0.IN3_RIGHT_MAGAZINE_BOTTOM_DETECTED;
                default:
                    // 로그 남기기 (예: LogHelper.Write)
                    Console.WriteLine($"[Warning] 잘못된 GetInMagazineDocked 타입 요청됨:{index}");
                    //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                    return 0; // 또는 사용하지 않는 안전한 비트
            }
        }
        public uint GetInMagazineTrayLoad(int index)
        {
            switch (index)
            {
                case 0: return (uint)DIO_IN_ADDR_CH0.IN2_LEFT_MAGAZINE_TRAY_LOAD_DETECTED;
                case 1: return (uint)DIO_IN_ADDR_CH0.IN3_RIGHT_MAGAZINE_TRAY_LOAD_DETECTED;
                default:
                    // 로그 남기기 (예: LogHelper.Write)
                    Console.WriteLine($"[Warning] 잘못된 GetInMagazineDocked 타입 요청됨:{index}");
                    //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                    return 0; // 또는 사용하지 않는 안전한 비트
            }
        }
        public uint GetInMagazineTrayReady(int index)
        {
            switch (index)
            {
                case 0: return (uint)DIO_IN_ADDR_CH0.IN2_LEFT_MAGAZINE_TRAY_REDAY_DETECTED;
                case 1: return (uint)DIO_IN_ADDR_CH0.IN3_RIGHT_MAGAZINE_TRAY_REDAY_DETECTED;
                default:
                    // 로그 남기기 (예: LogHelper.Write)
                    Console.WriteLine($"[Warning] 잘못된 GetInMagazineDocked 타입 요청됨:{index}");
                    //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                    return 0; // 또는 사용하지 않는 안전한 비트
            }
        }
        public uint GetInGoodDetect(int Group, int index)
        {
            if (Group == 0)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH8.IN3_A_SOCKET_GOOD_DETECT1;
                    case 1: return (uint)DIO_IN_ADDR_CH8.IN3_A_SOCKET_GOOD_DETECT2;
                    case 2: return (uint)DIO_IN_ADDR_CH8.IN3_A_SOCKET_GOOD_DETECT3;
                    case 3: return (uint)DIO_IN_ADDR_CH8.IN3_A_SOCKET_GOOD_DETECT4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetInGoodDetect 타입 요청됨:{Group}, {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            if (Group == 1)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH10.IN3_B_SOCKET_GOOD_DETECT1;
                    case 1: return (uint)DIO_IN_ADDR_CH10.IN3_B_SOCKET_GOOD_DETECT2;
                    case 2: return (uint)DIO_IN_ADDR_CH10.IN3_B_SOCKET_GOOD_DETECT3;
                    case 3: return (uint)DIO_IN_ADDR_CH10.IN3_B_SOCKET_GOOD_DETECT4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetInGoodDetect 타입 요청됨:{Group}, {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            if (Group == 2)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH12.IN3_C_SOCKET_GOOD_DETECT1;
                    case 1: return (uint)DIO_IN_ADDR_CH12.IN3_C_SOCKET_GOOD_DETECT2;
                    case 2: return (uint)DIO_IN_ADDR_CH12.IN3_C_SOCKET_GOOD_DETECT3;
                    case 3: return (uint)DIO_IN_ADDR_CH12.IN3_C_SOCKET_GOOD_DETECT4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetInGoodDetect 타입 요청됨:{Group}, {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            if (Group == 3)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH14.IN3_D_SOCKET_GOOD_DETECT1;
                    case 1: return (uint)DIO_IN_ADDR_CH14.IN3_D_SOCKET_GOOD_DETECT2;
                    case 2: return (uint)DIO_IN_ADDR_CH14.IN3_D_SOCKET_GOOD_DETECT3;
                    case 3: return (uint)DIO_IN_ADDR_CH14.IN3_D_SOCKET_GOOD_DETECT4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetInGoodDetect 타입 요청됨:{Group}, {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            return 0; // 또는 사용하지 않는 안전한 비트
        }
        public uint GetInRotateGrip(int Group, int index, bool bFlag)
        {
            if (Group == 0)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH8.IN2_A_SOCKET_ROTATE_GRIP1;
                        case 1: return (uint)DIO_IN_ADDR_CH8.IN2_A_SOCKET_ROTATE_GRIP2;
                        case 2: return (uint)DIO_IN_ADDR_CH8.IN2_A_SOCKET_ROTATE_GRIP3;
                        case 3: return (uint)DIO_IN_ADDR_CH8.IN2_A_SOCKET_ROTATE_GRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH8.IN2_A_SOCKET_ROTATE_UNGRIP1;
                        case 1: return (uint)DIO_IN_ADDR_CH8.IN2_A_SOCKET_ROTATE_UNGRIP2;
                        case 2: return (uint)DIO_IN_ADDR_CH8.IN2_A_SOCKET_ROTATE_UNGRIP3;
                        case 3: return (uint)DIO_IN_ADDR_CH8.IN2_A_SOCKET_ROTATE_UNGRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 1)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH10.IN2_B_SOCKET_ROTATE_GRIP1;
                        case 1: return (uint)DIO_IN_ADDR_CH10.IN2_B_SOCKET_ROTATE_GRIP2;
                        case 2: return (uint)DIO_IN_ADDR_CH10.IN2_B_SOCKET_ROTATE_GRIP3;
                        case 3: return (uint)DIO_IN_ADDR_CH10.IN2_B_SOCKET_ROTATE_GRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH10.IN2_B_SOCKET_ROTATE_UNGRIP1;
                        case 1: return (uint)DIO_IN_ADDR_CH10.IN2_B_SOCKET_ROTATE_UNGRIP2;
                        case 2: return (uint)DIO_IN_ADDR_CH10.IN2_B_SOCKET_ROTATE_UNGRIP3;
                        case 3: return (uint)DIO_IN_ADDR_CH10.IN2_B_SOCKET_ROTATE_UNGRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 2)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH12.IN12_C_SOCKET_ROTATE_GRIP1;
                        case 1: return (uint)DIO_IN_ADDR_CH12.IN12_C_SOCKET_ROTATE_GRIP2;
                        case 2: return (uint)DIO_IN_ADDR_CH12.IN12_C_SOCKET_ROTATE_GRIP3;
                        case 3: return (uint)DIO_IN_ADDR_CH12.IN12_C_SOCKET_ROTATE_GRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH12.IN12_C_SOCKET_ROTATE_UNGRIP1;
                        case 1: return (uint)DIO_IN_ADDR_CH12.IN12_C_SOCKET_ROTATE_UNGRIP2;
                        case 2: return (uint)DIO_IN_ADDR_CH12.IN12_C_SOCKET_ROTATE_UNGRIP3;
                        case 3: return (uint)DIO_IN_ADDR_CH12.IN12_C_SOCKET_ROTATE_UNGRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 3)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH14.IN12_D_SOCKET_ROTATE_GRIP1;
                        case 1: return (uint)DIO_IN_ADDR_CH14.IN12_D_SOCKET_ROTATE_GRIP2;
                        case 2: return (uint)DIO_IN_ADDR_CH14.IN12_D_SOCKET_ROTATE_GRIP3;
                        case 3: return (uint)DIO_IN_ADDR_CH14.IN12_D_SOCKET_ROTATE_GRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH14.IN12_D_SOCKET_ROTATE_UNGRIP1;
                        case 1: return (uint)DIO_IN_ADDR_CH14.IN12_D_SOCKET_ROTATE_UNGRIP2;
                        case 2: return (uint)DIO_IN_ADDR_CH14.IN12_D_SOCKET_ROTATE_UNGRIP3;
                        case 3: return (uint)DIO_IN_ADDR_CH14.IN12_D_SOCKET_ROTATE_UNGRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            return 0; // 또는 사용하지 않는 안전한 비트
        }
        public uint GetOutRotateGrip(int Group, int index, bool bFlag)
        {
            if (Group == 0)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH9.OUT2_A_SOCKET_ROTATE_GRIP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH9.OUT2_A_SOCKET_ROTATE_GRIP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH9.OUT2_A_SOCKET_ROTATE_GRIP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH9.OUT2_A_SOCKET_ROTATE_GRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH9.OUT2_A_SOCKET_ROTATE_UNGRIP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH9.OUT2_A_SOCKET_ROTATE_UNGRIP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH9.OUT2_A_SOCKET_ROTATE_UNGRIP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH9.OUT2_A_SOCKET_ROTATE_UNGRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 1)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH11.OUT2_B_SOCKET_ROTATE_GRIP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH11.OUT2_B_SOCKET_ROTATE_GRIP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH11.OUT2_B_SOCKET_ROTATE_GRIP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH11.OUT2_B_SOCKET_ROTATE_GRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH11.OUT2_B_SOCKET_ROTATE_UNGRIP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH11.OUT2_B_SOCKET_ROTATE_UNGRIP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH11.OUT2_B_SOCKET_ROTATE_UNGRIP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH11.OUT2_B_SOCKET_ROTATE_UNGRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 2)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH13.OUT2_C_SOCKET_ROTATE_GRIP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH13.OUT2_C_SOCKET_ROTATE_GRIP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH13.OUT2_C_SOCKET_ROTATE_GRIP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH13.OUT2_C_SOCKET_ROTATE_GRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH13.OUT2_C_SOCKET_ROTATE_UNGRIP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH13.OUT2_C_SOCKET_ROTATE_UNGRIP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH13.OUT2_C_SOCKET_ROTATE_UNGRIP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH13.OUT2_C_SOCKET_ROTATE_UNGRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 3)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH15.OUT2_D_SOCKET_ROTATE_GRIP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH15.OUT2_D_SOCKET_ROTATE_GRIP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH15.OUT2_D_SOCKET_ROTATE_GRIP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH15.OUT2_D_SOCKET_ROTATE_GRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH15.OUT2_D_SOCKET_ROTATE_UNGRIP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH15.OUT2_D_SOCKET_ROTATE_UNGRIP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH15.OUT2_D_SOCKET_ROTATE_UNGRIP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH15.OUT2_D_SOCKET_ROTATE_UNGRIP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            return 0; // 또는 사용하지 않는 안전한 비트
        }
        public uint GetInRotateTurn(int Group, int index, bool bFlag)
        {
            if (Group == 0)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH8.IN1_A_SOCKET_ROTATE_TURN_LEFT1;
                        case 1: return (uint)DIO_IN_ADDR_CH8.IN1_A_SOCKET_ROTATE_TURN_LEFT2;
                        case 2: return (uint)DIO_IN_ADDR_CH8.IN1_A_SOCKET_ROTATE_TURN_LEFT3;
                        case 3: return (uint)DIO_IN_ADDR_CH8.IN1_A_SOCKET_ROTATE_TURN_LEFT4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH8.IN1_A_SOCKET_ROTATE_TURN_HOME1;
                        case 1: return (uint)DIO_IN_ADDR_CH8.IN1_A_SOCKET_ROTATE_TURN_HOME2;
                        case 2: return (uint)DIO_IN_ADDR_CH8.IN1_A_SOCKET_ROTATE_TURN_HOME3;
                        case 3: return (uint)DIO_IN_ADDR_CH8.IN1_A_SOCKET_ROTATE_TURN_HOME4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 1)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH10.IN1_B_SOCKET_ROTATE_TURN_LEFT1;
                        case 1: return (uint)DIO_IN_ADDR_CH10.IN1_B_SOCKET_ROTATE_TURN_LEFT2;
                        case 2: return (uint)DIO_IN_ADDR_CH10.IN1_B_SOCKET_ROTATE_TURN_LEFT3;
                        case 3: return (uint)DIO_IN_ADDR_CH10.IN1_B_SOCKET_ROTATE_TURN_LEFT4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH10.IN1_B_SOCKET_ROTATE_TURN_HOME1;
                        case 1: return (uint)DIO_IN_ADDR_CH10.IN1_B_SOCKET_ROTATE_TURN_HOME2;
                        case 2: return (uint)DIO_IN_ADDR_CH10.IN1_B_SOCKET_ROTATE_TURN_HOME3;
                        case 3: return (uint)DIO_IN_ADDR_CH10.IN1_B_SOCKET_ROTATE_TURN_HOME4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 2)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH12.IN11_C_SOCKET_ROTATE_TURN_LEFT1;
                        case 1: return (uint)DIO_IN_ADDR_CH12.IN11_C_SOCKET_ROTATE_TURN_LEFT2;
                        case 2: return (uint)DIO_IN_ADDR_CH12.IN11_C_SOCKET_ROTATE_TURN_LEFT3;
                        case 3: return (uint)DIO_IN_ADDR_CH12.IN11_C_SOCKET_ROTATE_TURN_LEFT4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH12.IN11_C_SOCKET_ROTATE_TURN_HOME1;
                        case 1: return (uint)DIO_IN_ADDR_CH12.IN11_C_SOCKET_ROTATE_TURN_HOME2;
                        case 2: return (uint)DIO_IN_ADDR_CH12.IN11_C_SOCKET_ROTATE_TURN_HOME3;
                        case 3: return (uint)DIO_IN_ADDR_CH12.IN11_C_SOCKET_ROTATE_TURN_HOME4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 3)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH14.IN11_D_SOCKET_ROTATE_TURN_LEFT1;
                        case 1: return (uint)DIO_IN_ADDR_CH14.IN11_D_SOCKET_ROTATE_TURN_LEFT2;
                        case 2: return (uint)DIO_IN_ADDR_CH14.IN11_D_SOCKET_ROTATE_TURN_LEFT3;
                        case 3: return (uint)DIO_IN_ADDR_CH14.IN11_D_SOCKET_ROTATE_TURN_LEFT4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH14.IN11_D_SOCKET_ROTATE_TURN_HOME1;
                        case 1: return (uint)DIO_IN_ADDR_CH14.IN11_D_SOCKET_ROTATE_TURN_HOME2;
                        case 2: return (uint)DIO_IN_ADDR_CH14.IN11_D_SOCKET_ROTATE_TURN_HOME3;
                        case 3: return (uint)DIO_IN_ADDR_CH14.IN11_D_SOCKET_ROTATE_TURN_HOME4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            return 0; // 또는 사용하지 않는 안전한 비트
        }
        public uint GetOutRotateTurn(int Group, int index, bool bFlag)
        {
            if (Group == 0)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH9.OUT1_A_SOCKET_ROTATE_TURN_LEFT1;
                        case 1: return (uint)DIO_OUT_ADDR_CH9.OUT1_A_SOCKET_ROTATE_TURN_LEFT2;
                        case 2: return (uint)DIO_OUT_ADDR_CH9.OUT1_A_SOCKET_ROTATE_TURN_LEFT3;
                        case 3: return (uint)DIO_OUT_ADDR_CH9.OUT1_A_SOCKET_ROTATE_TURN_LEFT4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH9.OUT1_A_SOCKET_ROTATE_TURN_HOME1;
                        case 1: return (uint)DIO_OUT_ADDR_CH9.OUT1_A_SOCKET_ROTATE_TURN_HOME2;
                        case 2: return (uint)DIO_OUT_ADDR_CH9.OUT1_A_SOCKET_ROTATE_TURN_HOME3;
                        case 3: return (uint)DIO_OUT_ADDR_CH9.OUT1_A_SOCKET_ROTATE_TURN_HOME4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 1)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH11.OUT1_B_SOCKET_ROTATE_TURN_LEFT1;
                        case 1: return (uint)DIO_OUT_ADDR_CH11.OUT1_B_SOCKET_ROTATE_TURN_LEFT2;
                        case 2: return (uint)DIO_OUT_ADDR_CH11.OUT1_B_SOCKET_ROTATE_TURN_LEFT3;
                        case 3: return (uint)DIO_OUT_ADDR_CH11.OUT1_B_SOCKET_ROTATE_TURN_LEFT4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH11.OUT1_B_SOCKET_ROTATE_TURN_HOME1;
                        case 1: return (uint)DIO_OUT_ADDR_CH11.OUT1_B_SOCKET_ROTATE_TURN_HOME2;
                        case 2: return (uint)DIO_OUT_ADDR_CH11.OUT1_B_SOCKET_ROTATE_TURN_HOME3;
                        case 3: return (uint)DIO_OUT_ADDR_CH11.OUT1_B_SOCKET_ROTATE_TURN_HOME4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 2)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH13.OUT1_C_SOCKET_ROTATE_TURN_LEFT1;
                        case 1: return (uint)DIO_OUT_ADDR_CH13.OUT1_C_SOCKET_ROTATE_TURN_LEFT2;
                        case 2: return (uint)DIO_OUT_ADDR_CH13.OUT1_C_SOCKET_ROTATE_TURN_LEFT3;
                        case 3: return (uint)DIO_OUT_ADDR_CH13.OUT1_C_SOCKET_ROTATE_TURN_LEFT4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH13.OUT1_C_SOCKET_ROTATE_TURN_HOME1;
                        case 1: return (uint)DIO_OUT_ADDR_CH13.OUT1_C_SOCKET_ROTATE_TURN_HOME2;
                        case 2: return (uint)DIO_OUT_ADDR_CH13.OUT1_C_SOCKET_ROTATE_TURN_HOME3;
                        case 3: return (uint)DIO_OUT_ADDR_CH13.OUT1_C_SOCKET_ROTATE_TURN_HOME4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 3)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH15.OUT1_D_SOCKET_ROTATE_TURN_LEFT1;
                        case 1: return (uint)DIO_OUT_ADDR_CH15.OUT1_D_SOCKET_ROTATE_TURN_LEFT2;
                        case 2: return (uint)DIO_OUT_ADDR_CH15.OUT1_D_SOCKET_ROTATE_TURN_LEFT3;
                        case 3: return (uint)DIO_OUT_ADDR_CH15.OUT1_D_SOCKET_ROTATE_TURN_LEFT4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH15.OUT1_D_SOCKET_ROTATE_TURN_HOME1;
                        case 1: return (uint)DIO_OUT_ADDR_CH15.OUT1_D_SOCKET_ROTATE_TURN_HOME2;
                        case 2: return (uint)DIO_OUT_ADDR_CH15.OUT1_D_SOCKET_ROTATE_TURN_HOME3;
                        case 3: return (uint)DIO_OUT_ADDR_CH15.OUT1_D_SOCKET_ROTATE_TURN_HOME4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            return 0; // 또는 사용하지 않는 안전한 비트
        }
        public uint GetInRotateUpDown(int Group, int index, bool bFlag)
        {
            if (Group == 0)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH8.IN0_A_SOCKET_ROTATE_UP1;
                        case 1: return (uint)DIO_IN_ADDR_CH8.IN0_A_SOCKET_ROTATE_UP2;
                        case 2: return (uint)DIO_IN_ADDR_CH8.IN0_A_SOCKET_ROTATE_UP3;
                        case 3: return (uint)DIO_IN_ADDR_CH8.IN0_A_SOCKET_ROTATE_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH8.IN0_A_SOCKET_ROTATE_DOWN1;
                        case 1: return (uint)DIO_IN_ADDR_CH8.IN0_A_SOCKET_ROTATE_DOWN2;
                        case 2: return (uint)DIO_IN_ADDR_CH8.IN0_A_SOCKET_ROTATE_DOWN3;
                        case 3: return (uint)DIO_IN_ADDR_CH8.IN0_A_SOCKET_ROTATE_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 1)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH10.IN0_B_SOCKET_ROTATE_UP1;
                        case 1: return (uint)DIO_IN_ADDR_CH10.IN0_B_SOCKET_ROTATE_UP2;
                        case 2: return (uint)DIO_IN_ADDR_CH10.IN0_B_SOCKET_ROTATE_UP3;
                        case 3: return (uint)DIO_IN_ADDR_CH10.IN0_B_SOCKET_ROTATE_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH10.IN0_B_SOCKET_ROTATE_DOWN1;
                        case 1: return (uint)DIO_IN_ADDR_CH10.IN0_B_SOCKET_ROTATE_DOWN2;
                        case 2: return (uint)DIO_IN_ADDR_CH10.IN0_B_SOCKET_ROTATE_DOWN3;
                        case 3: return (uint)DIO_IN_ADDR_CH10.IN0_B_SOCKET_ROTATE_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 2)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH12.IN10_C_SOCKET_ROTATE_UP1;
                        case 1: return (uint)DIO_IN_ADDR_CH12.IN10_C_SOCKET_ROTATE_UP2;
                        case 2: return (uint)DIO_IN_ADDR_CH12.IN10_C_SOCKET_ROTATE_UP3;
                        case 3: return (uint)DIO_IN_ADDR_CH12.IN10_C_SOCKET_ROTATE_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH12.IN10_C_SOCKET_ROTATE_DOWN1;
                        case 1: return (uint)DIO_IN_ADDR_CH12.IN10_C_SOCKET_ROTATE_DOWN2;
                        case 2: return (uint)DIO_IN_ADDR_CH12.IN10_C_SOCKET_ROTATE_DOWN3;
                        case 3: return (uint)DIO_IN_ADDR_CH12.IN10_C_SOCKET_ROTATE_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 3)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH14.IN10_D_SOCKET_ROTATE_UP1;
                        case 1: return (uint)DIO_IN_ADDR_CH14.IN10_D_SOCKET_ROTATE_UP2;
                        case 2: return (uint)DIO_IN_ADDR_CH14.IN10_D_SOCKET_ROTATE_UP3;
                        case 3: return (uint)DIO_IN_ADDR_CH14.IN10_D_SOCKET_ROTATE_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH14.IN10_D_SOCKET_ROTATE_DOWN1;
                        case 1: return (uint)DIO_IN_ADDR_CH14.IN10_D_SOCKET_ROTATE_DOWN2;
                        case 2: return (uint)DIO_IN_ADDR_CH14.IN10_D_SOCKET_ROTATE_DOWN3;
                        case 3: return (uint)DIO_IN_ADDR_CH14.IN10_D_SOCKET_ROTATE_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            return 0; // 또는 사용하지 않는 안전한 비트
        }
        public uint GetOutRotateUpDown(int Group, int index, bool bFlag)
        {
            if (Group == 0)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH9.OUT0_A_SOCKET_ROTATE_UP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH9.OUT0_A_SOCKET_ROTATE_UP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH9.OUT0_A_SOCKET_ROTATE_UP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH9.OUT0_A_SOCKET_ROTATE_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH9.OUT0_A_SOCKET_ROTATE_DOWN1;
                        case 1: return (uint)DIO_OUT_ADDR_CH9.OUT0_A_SOCKET_ROTATE_DOWN2;
                        case 2: return (uint)DIO_OUT_ADDR_CH9.OUT0_A_SOCKET_ROTATE_DOWN3;
                        case 3: return (uint)DIO_OUT_ADDR_CH9.OUT0_A_SOCKET_ROTATE_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 1)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH11.OUT0_B_SOCKET_ROTATE_UP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH11.OUT0_B_SOCKET_ROTATE_UP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH11.OUT0_B_SOCKET_ROTATE_UP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH11.OUT0_B_SOCKET_ROTATE_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH11.OUT0_B_SOCKET_ROTATE_DOWN1;
                        case 1: return (uint)DIO_OUT_ADDR_CH11.OUT0_B_SOCKET_ROTATE_DOWN2;
                        case 2: return (uint)DIO_OUT_ADDR_CH11.OUT0_B_SOCKET_ROTATE_DOWN3;
                        case 3: return (uint)DIO_OUT_ADDR_CH11.OUT0_B_SOCKET_ROTATE_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 2)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH13.OUT0_C_SOCKET_ROTATE_UP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH13.OUT0_C_SOCKET_ROTATE_UP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH13.OUT0_C_SOCKET_ROTATE_UP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH13.OUT0_C_SOCKET_ROTATE_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH13.OUT0_C_SOCKET_ROTATE_DOWN1;
                        case 1: return (uint)DIO_OUT_ADDR_CH13.OUT0_C_SOCKET_ROTATE_DOWN2;
                        case 2: return (uint)DIO_OUT_ADDR_CH13.OUT0_C_SOCKET_ROTATE_DOWN3;
                        case 3: return (uint)DIO_OUT_ADDR_CH13.OUT0_C_SOCKET_ROTATE_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 3)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH15.OUT0_D_SOCKET_ROTATE_UP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH15.OUT0_D_SOCKET_ROTATE_UP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH15.OUT0_D_SOCKET_ROTATE_UP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH15.OUT0_D_SOCKET_ROTATE_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH15.OUT0_D_SOCKET_ROTATE_DOWN1;
                        case 1: return (uint)DIO_OUT_ADDR_CH15.OUT0_D_SOCKET_ROTATE_DOWN2;
                        case 2: return (uint)DIO_OUT_ADDR_CH15.OUT0_D_SOCKET_ROTATE_DOWN3;
                        case 3: return (uint)DIO_OUT_ADDR_CH15.OUT0_D_SOCKET_ROTATE_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            return 0; // 또는 사용하지 않는 안전한 비트
        }
        public uint GetInContactForBack(int Group, int index, bool bFlag)
        {
            if (Group == 0)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH4.IN1_A_SOCKET_CONTACT_FORWARD1;
                        case 1: return (uint)DIO_IN_ADDR_CH4.IN1_A_SOCKET_CONTACT_FORWARD2;
                        case 2: return (uint)DIO_IN_ADDR_CH4.IN1_A_SOCKET_CONTACT_FORWARD3;
                        case 3: return (uint)DIO_IN_ADDR_CH4.IN1_A_SOCKET_CONTACT_FORWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH4.IN1_A_SOCKET_CONTACT_BACKWARD1;
                        case 1: return (uint)DIO_IN_ADDR_CH4.IN1_A_SOCKET_CONTACT_BACKWARD2;
                        case 2: return (uint)DIO_IN_ADDR_CH4.IN1_A_SOCKET_CONTACT_BACKWARD3;
                        case 3: return (uint)DIO_IN_ADDR_CH4.IN1_A_SOCKET_CONTACT_BACKWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 1)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH4.IN3_B_SOCKET_CONTACT_FORWARD1;
                        case 1: return (uint)DIO_IN_ADDR_CH4.IN3_B_SOCKET_CONTACT_FORWARD2;
                        case 2: return (uint)DIO_IN_ADDR_CH4.IN3_B_SOCKET_CONTACT_FORWARD3;
                        case 3: return (uint)DIO_IN_ADDR_CH4.IN3_B_SOCKET_CONTACT_FORWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH4.IN3_B_SOCKET_CONTACT_BACKWARD1;
                        case 1: return (uint)DIO_IN_ADDR_CH4.IN3_B_SOCKET_CONTACT_BACKWARD2;
                        case 2: return (uint)DIO_IN_ADDR_CH4.IN3_B_SOCKET_CONTACT_BACKWARD3;
                        case 3: return (uint)DIO_IN_ADDR_CH4.IN3_B_SOCKET_CONTACT_BACKWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 2)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH6.IN1_C_SOCKET_CONTACT_FORWARD1;
                        case 1: return (uint)DIO_IN_ADDR_CH6.IN1_C_SOCKET_CONTACT_FORWARD2;
                        case 2: return (uint)DIO_IN_ADDR_CH6.IN1_C_SOCKET_CONTACT_FORWARD3;
                        case 3: return (uint)DIO_IN_ADDR_CH6.IN1_C_SOCKET_CONTACT_FORWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH6.IN1_C_SOCKET_CONTACT_BACKWARD1;
                        case 1: return (uint)DIO_IN_ADDR_CH6.IN1_C_SOCKET_CONTACT_BACKWARD2;
                        case 2: return (uint)DIO_IN_ADDR_CH6.IN1_C_SOCKET_CONTACT_BACKWARD3;
                        case 3: return (uint)DIO_IN_ADDR_CH6.IN1_C_SOCKET_CONTACT_BACKWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 3)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH6.IN3_D_SOCKET_CONTACT_FORWARD1;
                        case 1: return (uint)DIO_IN_ADDR_CH6.IN3_D_SOCKET_CONTACT_FORWARD2;
                        case 2: return (uint)DIO_IN_ADDR_CH6.IN3_D_SOCKET_CONTACT_FORWARD3;
                        case 3: return (uint)DIO_IN_ADDR_CH6.IN3_D_SOCKET_CONTACT_FORWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH6.IN3_D_SOCKET_CONTACT_BACKWARD1;
                        case 1: return (uint)DIO_IN_ADDR_CH6.IN3_D_SOCKET_CONTACT_BACKWARD2;
                        case 2: return (uint)DIO_IN_ADDR_CH6.IN3_D_SOCKET_CONTACT_BACKWARD3;
                        case 3: return (uint)DIO_IN_ADDR_CH6.IN3_D_SOCKET_CONTACT_BACKWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            return 0; // 또는 사용하지 않는 안전한 비트
        }
        public uint GetOutContactForBack(int Group, int index, bool bFlag)
        {
            if (Group == 0)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH5.OUT1_A_SOCKET_CONTACT_FORWARD1;
                        case 1: return (uint)DIO_OUT_ADDR_CH5.OUT1_A_SOCKET_CONTACT_FORWARD2;
                        case 2: return (uint)DIO_OUT_ADDR_CH5.OUT1_A_SOCKET_CONTACT_FORWARD3;
                        case 3: return (uint)DIO_OUT_ADDR_CH5.OUT1_A_SOCKET_CONTACT_FORWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH5.OUT1_A_SOCKET_CONTACT_BACKWARD1;
                        case 1: return (uint)DIO_OUT_ADDR_CH5.OUT1_A_SOCKET_CONTACT_BACKWARD2;
                        case 2: return (uint)DIO_OUT_ADDR_CH5.OUT1_A_SOCKET_CONTACT_BACKWARD3;
                        case 3: return (uint)DIO_OUT_ADDR_CH5.OUT1_A_SOCKET_CONTACT_BACKWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 1)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH5.OUT3_B_SOCKET_CONTACT_FORWARD1;
                        case 1: return (uint)DIO_OUT_ADDR_CH5.OUT3_B_SOCKET_CONTACT_FORWARD2;
                        case 2: return (uint)DIO_OUT_ADDR_CH5.OUT3_B_SOCKET_CONTACT_FORWARD3;
                        case 3: return (uint)DIO_OUT_ADDR_CH5.OUT3_B_SOCKET_CONTACT_FORWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH5.OUT3_B_SOCKET_CONTACT_BACKWARD1;
                        case 1: return (uint)DIO_OUT_ADDR_CH5.OUT3_B_SOCKET_CONTACT_BACKWARD2;
                        case 2: return (uint)DIO_OUT_ADDR_CH5.OUT3_B_SOCKET_CONTACT_BACKWARD3;
                        case 3: return (uint)DIO_OUT_ADDR_CH5.OUT3_B_SOCKET_CONTACT_BACKWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 2)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH7.OUT1_C_SOCKET_CONTACT_FORWARD1;
                        case 1: return (uint)DIO_OUT_ADDR_CH7.OUT1_C_SOCKET_CONTACT_FORWARD2;
                        case 2: return (uint)DIO_OUT_ADDR_CH7.OUT1_C_SOCKET_CONTACT_FORWARD3;
                        case 3: return (uint)DIO_OUT_ADDR_CH7.OUT1_C_SOCKET_CONTACT_FORWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH7.OUT1_C_SOCKET_CONTACT_BACKWARD1;
                        case 1: return (uint)DIO_OUT_ADDR_CH7.OUT1_C_SOCKET_CONTACT_BACKWARD2;
                        case 2: return (uint)DIO_OUT_ADDR_CH7.OUT1_C_SOCKET_CONTACT_BACKWARD3;
                        case 3: return (uint)DIO_OUT_ADDR_CH7.OUT1_C_SOCKET_CONTACT_BACKWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 3)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH7.OUT3_D_SOCKET_CONTACT_FORWARD1;
                        case 1: return (uint)DIO_OUT_ADDR_CH7.OUT3_D_SOCKET_CONTACT_FORWARD2;
                        case 2: return (uint)DIO_OUT_ADDR_CH7.OUT3_D_SOCKET_CONTACT_FORWARD3;
                        case 3: return (uint)DIO_OUT_ADDR_CH7.OUT3_D_SOCKET_CONTACT_FORWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH7.OUT3_D_SOCKET_CONTACT_BACKWARD1;
                        case 1: return (uint)DIO_OUT_ADDR_CH7.OUT3_D_SOCKET_CONTACT_BACKWARD2;
                        case 2: return (uint)DIO_OUT_ADDR_CH7.OUT3_D_SOCKET_CONTACT_BACKWARD3;
                        case 3: return (uint)DIO_OUT_ADDR_CH7.OUT3_D_SOCKET_CONTACT_BACKWARD4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            return 0; // 또는 사용하지 않는 안전한 비트
        }
        public uint GetInContactUpDown(int Group , int index, bool bFlag)
        {
            if (Group == 0)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH4.IN0_A_SOCKET_CONTACT_UP1;
                        case 1: return (uint)DIO_IN_ADDR_CH4.IN0_A_SOCKET_CONTACT_UP2;
                        case 2: return (uint)DIO_IN_ADDR_CH4.IN0_A_SOCKET_CONTACT_UP3;
                        case 3: return (uint)DIO_IN_ADDR_CH4.IN0_A_SOCKET_CONTACT_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH4.IN0_A_SOCKET_CONTACT_DOWN1;
                        case 1: return (uint)DIO_IN_ADDR_CH4.IN0_A_SOCKET_CONTACT_DOWN2;
                        case 2: return (uint)DIO_IN_ADDR_CH4.IN0_A_SOCKET_CONTACT_DOWN3;
                        case 3: return (uint)DIO_IN_ADDR_CH4.IN0_A_SOCKET_CONTACT_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 1)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH4.IN2_B_SOCKET_CONTACT_UP1;
                        case 1: return (uint)DIO_IN_ADDR_CH4.IN2_B_SOCKET_CONTACT_UP2;
                        case 2: return (uint)DIO_IN_ADDR_CH4.IN2_B_SOCKET_CONTACT_UP3;
                        case 3: return (uint)DIO_IN_ADDR_CH4.IN2_B_SOCKET_CONTACT_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH4.IN2_B_SOCKET_CONTACT_DOWN1;
                        case 1: return (uint)DIO_IN_ADDR_CH4.IN2_B_SOCKET_CONTACT_DOWN2;
                        case 2: return (uint)DIO_IN_ADDR_CH4.IN2_B_SOCKET_CONTACT_DOWN3;
                        case 3: return (uint)DIO_IN_ADDR_CH4.IN2_B_SOCKET_CONTACT_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 2)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH6.IN0_C_SOCKET_CONTACT_UP1;
                        case 1: return (uint)DIO_IN_ADDR_CH6.IN0_C_SOCKET_CONTACT_UP2;
                        case 2: return (uint)DIO_IN_ADDR_CH6.IN0_C_SOCKET_CONTACT_UP3;
                        case 3: return (uint)DIO_IN_ADDR_CH6.IN0_C_SOCKET_CONTACT_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH6.IN0_C_SOCKET_CONTACT_DOWN1;
                        case 1: return (uint)DIO_IN_ADDR_CH6.IN0_C_SOCKET_CONTACT_DOWN2;
                        case 2: return (uint)DIO_IN_ADDR_CH6.IN0_C_SOCKET_CONTACT_DOWN3;
                        case 3: return (uint)DIO_IN_ADDR_CH6.IN0_C_SOCKET_CONTACT_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 3)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH6.IN2_D_SOCKET_CONTACT_UP1;
                        case 1: return (uint)DIO_IN_ADDR_CH6.IN2_D_SOCKET_CONTACT_UP2;
                        case 2: return (uint)DIO_IN_ADDR_CH6.IN2_D_SOCKET_CONTACT_UP3;
                        case 3: return (uint)DIO_IN_ADDR_CH6.IN2_D_SOCKET_CONTACT_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_IN_ADDR_CH6.IN2_D_SOCKET_CONTACT_DOWN1;
                        case 1: return (uint)DIO_IN_ADDR_CH6.IN2_D_SOCKET_CONTACT_DOWN2;
                        case 2: return (uint)DIO_IN_ADDR_CH6.IN2_D_SOCKET_CONTACT_DOWN3;
                        case 3: return (uint)DIO_IN_ADDR_CH6.IN2_D_SOCKET_CONTACT_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetInContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            return 0; // 또는 사용하지 않는 안전한 비트
        }
        public uint GetOutContactUpDown(int Group, int index, bool bFlag)
        {
            if (Group == 0)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH5.OUT0_A_SOCKET_CONTACT_UP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH5.OUT0_A_SOCKET_CONTACT_UP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH5.OUT0_A_SOCKET_CONTACT_UP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH5.OUT0_A_SOCKET_CONTACT_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH5.OUT0_A_SOCKET_CONTACT_DOWN1;
                        case 1: return (uint)DIO_OUT_ADDR_CH5.OUT0_A_SOCKET_CONTACT_DOWN2;
                        case 2: return (uint)DIO_OUT_ADDR_CH5.OUT0_A_SOCKET_CONTACT_DOWN3;
                        case 3: return (uint)DIO_OUT_ADDR_CH5.OUT0_A_SOCKET_CONTACT_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 1)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH5.OUT2_B_SOCKET_CONTACT_UP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH5.OUT2_B_SOCKET_CONTACT_UP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH5.OUT2_B_SOCKET_CONTACT_UP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH5.OUT2_B_SOCKET_CONTACT_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH5.OUT2_B_SOCKET_CONTACT_DOWN1;
                        case 1: return (uint)DIO_OUT_ADDR_CH5.OUT2_B_SOCKET_CONTACT_DOWN2;
                        case 2: return (uint)DIO_OUT_ADDR_CH5.OUT2_B_SOCKET_CONTACT_DOWN3;
                        case 3: return (uint)DIO_OUT_ADDR_CH5.OUT2_B_SOCKET_CONTACT_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 2)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH7.OUT0_C_SOCKET_CONTACT_UP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH7.OUT0_C_SOCKET_CONTACT_UP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH7.OUT0_C_SOCKET_CONTACT_UP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH7.OUT0_C_SOCKET_CONTACT_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH7.OUT0_C_SOCKET_CONTACT_DOWN1;
                        case 1: return (uint)DIO_OUT_ADDR_CH7.OUT0_C_SOCKET_CONTACT_DOWN2;
                        case 2: return (uint)DIO_OUT_ADDR_CH7.OUT0_C_SOCKET_CONTACT_DOWN3;
                        case 3: return (uint)DIO_OUT_ADDR_CH7.OUT0_C_SOCKET_CONTACT_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            if (Group == 3)
            {
                if (bFlag)
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH7.OUT2_D_SOCKET_CONTACT_UP1;
                        case 1: return (uint)DIO_OUT_ADDR_CH7.OUT2_D_SOCKET_CONTACT_UP2;
                        case 2: return (uint)DIO_OUT_ADDR_CH7.OUT2_D_SOCKET_CONTACT_UP3;
                        case 3: return (uint)DIO_OUT_ADDR_CH7.OUT2_D_SOCKET_CONTACT_UP4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0: return (uint)DIO_OUT_ADDR_CH7.OUT2_D_SOCKET_CONTACT_DOWN1;
                        case 1: return (uint)DIO_OUT_ADDR_CH7.OUT2_D_SOCKET_CONTACT_DOWN2;
                        case 2: return (uint)DIO_OUT_ADDR_CH7.OUT2_D_SOCKET_CONTACT_DOWN3;
                        case 3: return (uint)DIO_OUT_ADDR_CH7.OUT2_D_SOCKET_CONTACT_DOWN4;
                        default:
                            // 로그 남기기 (예: LogHelper.Write)
                            Console.WriteLine($"[Warning] 잘못된 GetOutContactUpDown 타입 요청됨:{Group}, {index}");
                            //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                            return 0; // 또는 사용하지 않는 안전한 비트
                    }
                }
            }
            return 0; // 또는 사용하지 않는 안전한 비트
        }
        public uint GetInLoadPickerUpDown(int index, bool bFlag)
        {
            if (bFlag)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH2.IN0_TRANSFER_LOAD_PICKER_UP1;
                    case 1: return (uint)DIO_IN_ADDR_CH2.IN0_TRANSFER_LOAD_PICKER_UP2;
                    case 2: return (uint)DIO_IN_ADDR_CH2.IN0_TRANSFER_LOAD_PICKER_UP3;
                    case 3: return (uint)DIO_IN_ADDR_CH2.IN0_TRANSFER_LOAD_PICKER_UP4;
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
                    case 0: return (uint)DIO_IN_ADDR_CH2.IN0_TRANSFER_LOAD_PICKER_DOWN1;
                    case 1: return (uint)DIO_IN_ADDR_CH2.IN0_TRANSFER_LOAD_PICKER_DOWN2;
                    case 2: return (uint)DIO_IN_ADDR_CH2.IN0_TRANSFER_LOAD_PICKER_DOWN3;
                    case 3: return (uint)DIO_IN_ADDR_CH2.IN0_TRANSFER_LOAD_PICKER_DOWN4;
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
                    case 0: return (uint)DIO_IN_ADDR_CH2.IN1_TRANSFER_UNLOAD_PICKER_UP1;
                    case 1: return (uint)DIO_IN_ADDR_CH2.IN1_TRANSFER_UNLOAD_PICKER_UP2;
                    case 2: return (uint)DIO_IN_ADDR_CH2.IN1_TRANSFER_UNLOAD_PICKER_UP3;
                    case 3: return (uint)DIO_IN_ADDR_CH2.IN1_TRANSFER_UNLOAD_PICKER_UP4;
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
                    case 0: return (uint)DIO_IN_ADDR_CH2.IN1_TRANSFER_UNLOAD_PICKER_DOWN1;
                    case 1: return (uint)DIO_IN_ADDR_CH2.IN1_TRANSFER_UNLOAD_PICKER_DOWN2;
                    case 2: return (uint)DIO_IN_ADDR_CH2.IN1_TRANSFER_UNLOAD_PICKER_DOWN3;
                    case 3: return (uint)DIO_IN_ADDR_CH2.IN1_TRANSFER_UNLOAD_PICKER_DOWN4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetInUnloadPickerUpDown 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
        }
        public uint GetOutLoadPickerUpDown(int index, bool bFlag)
        {
            if (bFlag)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH3.OUT0_TRANSFER_LOAD_PICKER_UP1;
                    case 1: return (uint)DIO_OUT_ADDR_CH3.OUT0_TRANSFER_LOAD_PICKER_UP2;
                    case 2: return (uint)DIO_OUT_ADDR_CH3.OUT0_TRANSFER_LOAD_PICKER_UP3;
                    case 3: return (uint)DIO_OUT_ADDR_CH3.OUT0_TRANSFER_LOAD_PICKER_UP4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetOutLoadPickerUpDown 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH3.OUT0_TRANSFER_LOAD_PICKER_DOWN1;
                    case 1: return (uint)DIO_OUT_ADDR_CH3.OUT0_TRANSFER_LOAD_PICKER_DOWN2;
                    case 2: return (uint)DIO_OUT_ADDR_CH3.OUT0_TRANSFER_LOAD_PICKER_DOWN3;
                    case 3: return (uint)DIO_OUT_ADDR_CH3.OUT0_TRANSFER_LOAD_PICKER_DOWN4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetOutLoadPickerUpDown 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
        }
        public uint GetOutUnloadPickerUpDown(int index, bool bFlag)
        {
            if (bFlag)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH3.OUT1_TRANSFER_UNLOAD_PICKER_UP1;
                    case 1: return (uint)DIO_OUT_ADDR_CH3.OUT1_TRANSFER_UNLOAD_PICKER_UP2;
                    case 2: return (uint)DIO_OUT_ADDR_CH3.OUT1_TRANSFER_UNLOAD_PICKER_UP3;
                    case 3: return (uint)DIO_OUT_ADDR_CH3.OUT1_TRANSFER_UNLOAD_PICKER_UP4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetOutUnloadPickerUpDown 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH3.OUT1_TRANSFER_UNLOAD_PICKER_DOWN1;
                    case 1: return (uint)DIO_OUT_ADDR_CH3.OUT1_TRANSFER_UNLOAD_PICKER_DOWN2;
                    case 2: return (uint)DIO_OUT_ADDR_CH3.OUT1_TRANSFER_UNLOAD_PICKER_DOWN3;
                    case 3: return (uint)DIO_OUT_ADDR_CH3.OUT1_TRANSFER_UNLOAD_PICKER_DOWN4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetOutUnloadPickerUpDown 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
        }
        public uint GetOutLoadPickerVacuumOn(int index, bool bFlag)
        {
            if (bFlag)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH3.OUT2_TRANSFER_LOAD_PICKER_VACUUM_ON1;
                    case 1: return (uint)DIO_OUT_ADDR_CH3.OUT2_TRANSFER_LOAD_PICKER_VACUUM_ON2;
                    case 2: return (uint)DIO_OUT_ADDR_CH3.OUT2_TRANSFER_LOAD_PICKER_VACUUM_ON3;
                    case 3: return (uint)DIO_OUT_ADDR_CH3.OUT2_TRANSFER_LOAD_PICKER_VACUUM_ON4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetOutFwUnloadPickerGrip 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH3.OUT2_TRANSFER_LOAD_PICKER_BLOW_ON1;
                    case 1: return (uint)DIO_OUT_ADDR_CH3.OUT2_TRANSFER_LOAD_PICKER_BLOW_ON2;
                    case 2: return (uint)DIO_OUT_ADDR_CH3.OUT2_TRANSFER_LOAD_PICKER_BLOW_ON3;
                    case 3: return (uint)DIO_OUT_ADDR_CH3.OUT2_TRANSFER_LOAD_PICKER_BLOW_ON4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetOutFwUnloadPickerGrip 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
        }

        public uint GetOutUnloadPickerVacuumOn(int index, bool bFlag)
        {
            //Fw 설비는 Unload 는 Grip / UnGrip 방식
            return 0;
        }
        public uint GetOutFwUnloadPickerGrip(int index, bool bFlag)
        {
            if (bFlag)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH3.OUT3_TRANSFER_UNLOAD_PICKER_GRIP1;
                    case 1: return (uint)DIO_OUT_ADDR_CH3.OUT3_TRANSFER_UNLOAD_PICKER_GRIP2;
                    case 2: return (uint)DIO_OUT_ADDR_CH3.OUT3_TRANSFER_UNLOAD_PICKER_GRIP3;
                    case 3: return (uint)DIO_OUT_ADDR_CH3.OUT3_TRANSFER_UNLOAD_PICKER_GRIP4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetOutFwUnloadPickerGrip 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH3.OUT3_TRANSFER_UNLOAD_PICKER_UNGRIP1;
                    case 1: return (uint)DIO_OUT_ADDR_CH3.OUT3_TRANSFER_UNLOAD_PICKER_UNGRIP2;
                    case 2: return (uint)DIO_OUT_ADDR_CH3.OUT3_TRANSFER_UNLOAD_PICKER_UNGRIP3;
                    case 3: return (uint)DIO_OUT_ADDR_CH3.OUT3_TRANSFER_UNLOAD_PICKER_UNGRIP4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetOutFwUnloadPickerGrip 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            
        }
        public uint GetInFwUnloadPickerGrip(int index, bool bFlag)
        {
            if (bFlag)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH2.IN3_TRANSFER_UNLOAD_PICKER_GRIP1;
                    case 1: return (uint)DIO_IN_ADDR_CH2.IN3_TRANSFER_UNLOAD_PICKER_GRIP2;
                    case 2: return (uint)DIO_IN_ADDR_CH2.IN3_TRANSFER_UNLOAD_PICKER_GRIP3;
                    case 3: return (uint)DIO_IN_ADDR_CH2.IN3_TRANSFER_UNLOAD_PICKER_GRIP4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetInFwUnloadPickerGrip 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_IN_ADDR_CH2.IN3_TRANSFER_UNLOAD_PICKER_UNGRIP1;
                    case 1: return (uint)DIO_IN_ADDR_CH2.IN3_TRANSFER_UNLOAD_PICKER_UNGRIP2;
                    case 2: return (uint)DIO_IN_ADDR_CH2.IN3_TRANSFER_UNLOAD_PICKER_UNGRIP3;
                    case 3: return (uint)DIO_IN_ADDR_CH2.IN3_TRANSFER_UNLOAD_PICKER_UNGRIP4;
                    default:
                        // 로그 남기기 (예: LogHelper.Write)
                        Console.WriteLine($"[Warning] 잘못된 GetInFwUnloadPickerGrip 타입 요청됨: {index}");
                        //throw new ArgumentOutOfRangeException(nameof(nType), nType, "Invalid Tower Lamp Type");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            
        }
        public uint GetOutBuzzer(int nType)
        {
            switch (nType)
            {
                case 0: return (uint)DIO_OUT_ADDR_CH.OUT0_BUZZER1;
                case 1: return (uint)DIO_OUT_ADDR_CH.OUT0_BUZZER2;
                case 2: return (uint)DIO_OUT_ADDR_CH.OUT0_BUZZER3;
                case 3: return (uint)DIO_OUT_ADDR_CH.OUT0_BUZZER4;
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
                case 0: return (uint)DIO_OUT_ADDR_CH.OUT0_TOWER_LAMP_R;
                case 1: return (uint)DIO_OUT_ADDR_CH.OUT0_TOWER_LAMP_Y;
                case 2: return (uint)DIO_OUT_ADDR_CH.OUT0_TOWER_LAMP_G;
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
        public uint GetOutLiftDoor(int nType, int index)
        {
            if (nType == 0)
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT1_LEFT_MAGAZINE_DOOR_LOCK_UP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT1_LEFT_MAGAZINE_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutLiftDoor 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT1_RIGHT_MAGAZINE_DOOR_LOCK_UP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT1_RIGHT_MAGAZINE_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutLiftDoor 타입 요청됨: {nType}");
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
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT0_LEFT_MAGAZINE_LOAD_LAMP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT0_LEFT_MAGAZINE_LOAD_COMPLETE_LAMP;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutLiftLamp 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT0_RIGHT_MAGAZINE_LOAD_LAMP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT0_RIGHT_MAGAZINE_LOAD_COMPLETE_LAMP;
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
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT2_NG_TRAY_LEFT_DOOR_LOCK_UP;   //OUT3_LEFT_NG_TRAY_LOCK_UP
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT2_NG_TRAY_LEFT_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutLiftDoor 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT2_NG_TRAY_RIGHT_DOOR_LOCK_UP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT2_NG_TRAY_RIGHT_DOOR_LOCK_DOWN;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutLiftDoor 타입 요청됨: {nType}");
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
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT1_LEFT_NG_TRAY_LOAD_MODE_LAMP;     //OUT2_LEFT_NG_TRAY_LOCK_LAMP
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT1_LEFT_NG_TRAY_COMPLETE_MODE_LAMP;
                    default:
                        Console.WriteLine($"[Warning] 잘못된 GetOutNgTrayLamp 타입 요청됨: {nType}");
                        return 0; // 또는 사용하지 않는 안전한 비트
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return (uint)DIO_OUT_ADDR_CH1.OUT1_RIGHT_NG_TRAY_LOAD_MODE_LAMP;
                    case 1: return (uint)DIO_OUT_ADDR_CH1.OUT1_RIGHT_NG_TRAY_COMPLETE_MODE_LAMP;
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
        public enum DIO_IN_ADDR_CH : uint
        {
            IN0_EMERGENCY1              = 0x00000001,  //1
            IN0_EMERGENCY2              = 0x00000002,
            IN0_EMERGENCY3              = 0x00000004,
            TEMP4           = 0x00000008,  //4
            TEMP5           = 0x00000010,
            TEMP6           = 0x00000020,
            TEMP7           = 0x00000040,
            TEMP8           = 0x00000080,  //8
            //
            TEMP9           = 0x00000001,  //9
            TEMP10          = 0x00000002,
            TEMP11          = 0x00000004,
            TEMP12          = 0x00000008,  //12
            TEMP13          = 0x00000010,
            TEMP14          = 0x00000020,
            TEMP15          = 0x00000040,
            TEMP16          = 0x00000080,  //16
        }
        public enum DIO_OUT_ADDR_CH : uint
        {
            OUT0_TOWER_LAMP_R           = 0x00000001,  //1
            OUT0_TOWER_LAMP_Y           = 0x00000002,
            OUT0_TOWER_LAMP_G           = 0x00000004,
            OUT0_BUZZER1                = 0x00000008,  //4
            OUT0_BUZZER2                = 0x00000010,
            OUT0_BUZZER3                = 0x00000020,
            OUT0_BUZZER4                = 0x00000040,
            TEMP8 = 0x00000080,  //8
            //
            TEMP9 = 0x00000001,  //9
            TEMP10 = 0x00000002,
            TEMP11 = 0x00000004,
            TEMP12 = 0x00000008,  //12
            TEMP13 = 0x00000010,
            TEMP14 = 0x00000020,
            TEMP15 = 0x00000040,
            TEMP16 = 0x00000080,  //16
        }
        public enum DIO_IN_ADDR_CH0 : uint
        {
            IN0_DOOR_UNLOCK_FRONT_L                 = 0x00000001,  //1
            IN0_DOOR_UNLOCK_FRONT_R                 = 0x00000002,
            IN0_DOOR_UNLOCK_BACK_L                  = 0x00000004,
            IN0_DOOR_UNLOCK_BACK_R                  = 0x00000008,  //4
            IN0_LEFT_MAGAZINE_LOAD_MODE             = 0x00000010,
            IN0_LEFT_MAGAZINE_COMPLETE_MODE         = 0x00000020,
            IN0_RIGHT_MAGAZINE_LOAD_MODE            = 0x00000040,
            IN0_RIGHT_MAGAZINE_COMPLETE_MODE        = 0x00000080,  //8
            //
            IN1_LEFT_MAGAZINE_DOOR_LOCK_UP          = 0x00000001,  //9
            IN1_LEFT_MAGAZINE_DOOR_LOCK_DOWN        = 0x00000002,
            IN1_RIGHT_MAGAZINE_DOOR_LOCK_UP         = 0x00000004,
            IN1_RIGHT_MAGAZINE_DOOR_LOCK_DOWN       = 0x00000008,  //12
            IN1_LEFT_NG_DOOR_LOCK_LAMP              = 0x00000010,
            IN1_LEFT_NG_DOOR_UNLOCK_LAMP            = 0x00000020,
            IN1_RIGHT_NG_DOOR_LOCK_LAMP             = 0x00000040,
            IN1_RIGHT_NG_DOOR_UNLOCK_LAMP           = 0x00000080,  //16
            //
            IN2_LEFT_NG_DOOR_LOCK_UP                = 0x00000001,  //17
            IN2_LEFT_NG_DOOR_LOCK_DOWN              = 0x00000002,
            IN2_RIGHT_NG_DOOR_LOCK_UP               = 0x00000004,
            IN2_RIGHT_NG_DOOR_LOCK_DOWN             = 0x00000008,  //20
            IN2_LEFT_MAGAZINE_DOCKED                = 0x00000010,
            IN2_LEFT_MAGAZINE_BOTTOM_DETECTED       = 0x00000020,
            IN2_LEFT_MAGAZINE_TRAY_LOAD_DETECTED    = 0x00000040,
            IN2_LEFT_MAGAZINE_TRAY_REDAY_DETECTED   = 0x00000080,  //24
            //
            IN3_RIGHT_MAGAZINE_DOCKED               = 0x00000001,  //25
            IN3_RIGHT_MAGAZINE_BOTTOM_DETECTED      = 0x00000002,
            IN3_RIGHT_MAGAZINE_TRAY_LOAD_DETECTED   = 0x00000004,
            IN3_RIGHT_MAGAZINE_TRAY_REDAY_DETECTED  = 0x00000008,  //28
            IN3_LEFT_MAGAZINE_MAGNETIC              = 0x00000010,
            IN3_RIGHT_MAGAZINE_MAGNETIC             = 0x00000020,
            IN3_LEFT_NG_TRAY_DETECTED_MAGNETIC      = 0x00000040,
            IN3_RIGHT_NG_TRAY_DETECTED_MAGNETIC     = 0x00000080   //32
        };
        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 2
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH2 : uint
        {
            IN0_TRANSFER_LOAD_PICKER_DOWN1              = 0x00000001,  //1
            IN0_TRANSFER_LOAD_PICKER_DOWN2              = 0x00000002,
            IN0_TRANSFER_LOAD_PICKER_DOWN3              = 0x00000004,
            IN0_TRANSFER_LOAD_PICKER_DOWN4              = 0x00000008,  //4
            IN0_TRANSFER_LOAD_PICKER_UP1                = 0x00000010,
            IN0_TRANSFER_LOAD_PICKER_UP2                = 0x00000020,
            IN0_TRANSFER_LOAD_PICKER_UP3                = 0x00000040,
            IN0_TRANSFER_LOAD_PICKER_UP4                = 0x00000080,  //8
            //
            IN1_TRANSFER_UNLOAD_PICKER_DOWN1            = 0x00000001,  //9
            IN1_TRANSFER_UNLOAD_PICKER_DOWN2            = 0x00000002,
            IN1_TRANSFER_UNLOAD_PICKER_DOWN3            = 0x00000004,
            IN1_TRANSFER_UNLOAD_PICKER_DOWN4            = 0x00000008,  //12
            IN1_TRANSFER_UNLOAD_PICKER_UP1              = 0x00000010,
            IN1_TRANSFER_UNLOAD_PICKER_UP2              = 0x00000020,
            IN1_TRANSFER_UNLOAD_PICKER_UP3              = 0x00000040,
            IN1_TRANSFER_UNLOAD_PICKER_UP4              = 0x00000080,  //16
            //
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON1         = 0x00000001,  //17
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON2         = 0x00000002,
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON3         = 0x00000004,
            IN2_TRANSFER_LOAD_PICKER_VACUUM_ON4         = 0x00000008,  //20
            TEMP21  = 0x00000010,
            TEMP22  = 0x00000020,
            TEMP23  = 0x00000040,
            TEMP24  = 0x00000080,            //24
            //
            IN3_TRANSFER_UNLOAD_PICKER_GRIP1            = 0x00000001,   //25
            IN3_TRANSFER_UNLOAD_PICKER_GRIP2            = 0x00000002,
            IN3_TRANSFER_UNLOAD_PICKER_GRIP3            = 0x00000004,
            IN3_TRANSFER_UNLOAD_PICKER_GRIP4            = 0x00000008,   //28
            IN3_TRANSFER_UNLOAD_PICKER_UNGRIP1          = 0x00000010,
            IN3_TRANSFER_UNLOAD_PICKER_UNGRIP2          = 0x00000020,
            IN3_TRANSFER_UNLOAD_PICKER_UNGRIP3          = 0x00000040,
            IN3_TRANSFER_UNLOAD_PICKER_UNGRIP4          = 0x00000080    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 4
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH4 : uint
        {
            IN0_A_SOCKET_CONTACT_UP1            = 0x00000001,  //1
            IN0_A_SOCKET_CONTACT_UP2            = 0x00000002,
            IN0_A_SOCKET_CONTACT_UP3            = 0x00000004,
            IN0_A_SOCKET_CONTACT_UP4            = 0x00000008,  //4
            IN0_A_SOCKET_CONTACT_DOWN1          = 0x00000010,
            IN0_A_SOCKET_CONTACT_DOWN2          = 0x00000020,
            IN0_A_SOCKET_CONTACT_DOWN3          = 0x00000040,
            IN0_A_SOCKET_CONTACT_DOWN4          = 0x00000080,  //8
            //
            IN1_A_SOCKET_CONTACT_FORWARD1       = 0x00000001,  //9
            IN1_A_SOCKET_CONTACT_FORWARD2       = 0x00000002,
            IN1_A_SOCKET_CONTACT_FORWARD3       = 0x00000004,
            IN1_A_SOCKET_CONTACT_FORWARD4       = 0x00000008,  //12
            IN1_A_SOCKET_CONTACT_BACKWARD1      = 0x00000010,
            IN1_A_SOCKET_CONTACT_BACKWARD2      = 0x00000020,
            IN1_A_SOCKET_CONTACT_BACKWARD3      = 0x00000040,
            IN1_A_SOCKET_CONTACT_BACKWARD4      = 0x00000080,  //16
            //
            IN2_B_SOCKET_CONTACT_UP1            = 0x00000001,  //17
            IN2_B_SOCKET_CONTACT_UP2            = 0x00000002,
            IN2_B_SOCKET_CONTACT_UP3            = 0x00000004,
            IN2_B_SOCKET_CONTACT_UP4            = 0x00000008,  //20
            IN2_B_SOCKET_CONTACT_DOWN1          = 0x00000010,
            IN2_B_SOCKET_CONTACT_DOWN2          = 0x00000020,
            IN2_B_SOCKET_CONTACT_DOWN3          = 0x00000040,
            IN2_B_SOCKET_CONTACT_DOWN4          = 0x00000080,            //24
            //
            IN3_B_SOCKET_CONTACT_FORWARD1       = 0x00000001,   //25
            IN3_B_SOCKET_CONTACT_FORWARD2       = 0x00000002,
            IN3_B_SOCKET_CONTACT_FORWARD3       = 0x00000004,
            IN3_B_SOCKET_CONTACT_FORWARD4       = 0x00000008,   //28
            IN3_B_SOCKET_CONTACT_BACKWARD1      = 0x00000010,
            IN3_B_SOCKET_CONTACT_BACKWARD2      = 0x00000020,
            IN3_B_SOCKET_CONTACT_BACKWARD3      = 0x00000040,
            IN3_B_SOCKET_CONTACT_BACKWARD4      = 0x00000080    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 6
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH6 : uint
        {
            IN0_C_SOCKET_CONTACT_UP1            = 0x00000001,  //1
            IN0_C_SOCKET_CONTACT_UP2            = 0x00000002,
            IN0_C_SOCKET_CONTACT_UP3            = 0x00000004,
            IN0_C_SOCKET_CONTACT_UP4            = 0x00000008,  //4
            IN0_C_SOCKET_CONTACT_DOWN1          = 0x00000010,
            IN0_C_SOCKET_CONTACT_DOWN2          = 0x00000020,
            IN0_C_SOCKET_CONTACT_DOWN3          = 0x00000040,
            IN0_C_SOCKET_CONTACT_DOWN4          = 0x00000080,  //8
            //
            IN1_C_SOCKET_CONTACT_FORWARD1       = 0x00000001,  //9
            IN1_C_SOCKET_CONTACT_FORWARD2       = 0x00000002,
            IN1_C_SOCKET_CONTACT_FORWARD3       = 0x00000004,
            IN1_C_SOCKET_CONTACT_FORWARD4       = 0x00000008,  //12
            IN1_C_SOCKET_CONTACT_BACKWARD1      = 0x00000010,
            IN1_C_SOCKET_CONTACT_BACKWARD2      = 0x00000020,
            IN1_C_SOCKET_CONTACT_BACKWARD3      = 0x00000040,
            IN1_C_SOCKET_CONTACT_BACKWARD4      = 0x00000080,  //16
            //
            IN2_D_SOCKET_CONTACT_UP1            = 0x00000001,  //17
            IN2_D_SOCKET_CONTACT_UP2            = 0x00000002,
            IN2_D_SOCKET_CONTACT_UP3            = 0x00000004,
            IN2_D_SOCKET_CONTACT_UP4            = 0x00000008,  //20
            IN2_D_SOCKET_CONTACT_DOWN1          = 0x00000010,
            IN2_D_SOCKET_CONTACT_DOWN2          = 0x00000020,
            IN2_D_SOCKET_CONTACT_DOWN3          = 0x00000040,
            IN2_D_SOCKET_CONTACT_DOWN4          = 0x00000080,            //24
            //
            IN3_D_SOCKET_CONTACT_FORWARD1       = 0x00000001,   //25
            IN3_D_SOCKET_CONTACT_FORWARD2       = 0x00000002,
            IN3_D_SOCKET_CONTACT_FORWARD3       = 0x00000004,
            IN3_D_SOCKET_CONTACT_FORWARD4       = 0x00000008,   //28
            IN3_D_SOCKET_CONTACT_BACKWARD1      = 0x00000010,
            IN3_D_SOCKET_CONTACT_BACKWARD2      = 0x00000020,
            IN3_D_SOCKET_CONTACT_BACKWARD3      = 0x00000040,
            IN3_D_SOCKET_CONTACT_BACKWARD4      = 0x00000080    //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  IN CH : 8
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_IN_ADDR_CH8 : uint
        {
            IN0_A_SOCKET_ROTATE_UP1             = 0x00000001,  //1
            IN0_A_SOCKET_ROTATE_UP2             = 0x00000002,
            IN0_A_SOCKET_ROTATE_UP3             = 0x00000004,
            IN0_A_SOCKET_ROTATE_UP4             = 0x00000008,  //4
            IN0_A_SOCKET_ROTATE_DOWN1           = 0x00000010,
            IN0_A_SOCKET_ROTATE_DOWN2           = 0x00000020,
            IN0_A_SOCKET_ROTATE_DOWN3           = 0x00000040,
            IN0_A_SOCKET_ROTATE_DOWN4           = 0x00000080,  //8
            //
            IN1_A_SOCKET_ROTATE_TURN_LEFT1      = 0x00000001,  //9
            IN1_A_SOCKET_ROTATE_TURN_LEFT2      = 0x00000002,
            IN1_A_SOCKET_ROTATE_TURN_LEFT3      = 0x00000004,
            IN1_A_SOCKET_ROTATE_TURN_LEFT4      = 0x00000008,  //12
            IN1_A_SOCKET_ROTATE_TURN_HOME1      = 0x00000010,
            IN1_A_SOCKET_ROTATE_TURN_HOME2      = 0x00000020,
            IN1_A_SOCKET_ROTATE_TURN_HOME3      = 0x00000040,
            IN1_A_SOCKET_ROTATE_TURN_HOME4      = 0x00000080,  //16
            //
            IN2_A_SOCKET_ROTATE_GRIP1           = 0x00000001,  //17
            IN2_A_SOCKET_ROTATE_GRIP2           = 0x00000002,
            IN2_A_SOCKET_ROTATE_GRIP3           = 0x00000004,
            IN2_A_SOCKET_ROTATE_GRIP4           = 0x00000008,  //20
            IN2_A_SOCKET_ROTATE_UNGRIP1         = 0x00000010,
            IN2_A_SOCKET_ROTATE_UNGRIP2         = 0x00000020,
            IN2_A_SOCKET_ROTATE_UNGRIP3         = 0x00000040,
            IN2_A_SOCKET_ROTATE_UNGRIP4         = 0x00000080,            //24
            //
            IN3_A_SOCKET_GOOD_DETECT1           = 0x00000001,   //25
            IN3_A_SOCKET_GOOD_DETECT2           = 0x00000002,
            IN3_A_SOCKET_GOOD_DETECT3           = 0x00000004,
            IN3_A_SOCKET_GOOD_DETECT4           = 0x00000008,   //28
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
        public enum DIO_IN_ADDR_CH10 : uint
        {
            IN0_B_SOCKET_ROTATE_UP1             = 0x00000001,  //1
            IN0_B_SOCKET_ROTATE_UP2             = 0x00000002,
            IN0_B_SOCKET_ROTATE_UP3             = 0x00000004,
            IN0_B_SOCKET_ROTATE_UP4             = 0x00000008,  //4
            IN0_B_SOCKET_ROTATE_DOWN1           = 0x00000010,
            IN0_B_SOCKET_ROTATE_DOWN2           = 0x00000020,
            IN0_B_SOCKET_ROTATE_DOWN3           = 0x00000040,
            IN0_B_SOCKET_ROTATE_DOWN4           = 0x00000080,  //8
            //
            IN1_B_SOCKET_ROTATE_TURN_LEFT1      = 0x00000001,  //9
            IN1_B_SOCKET_ROTATE_TURN_LEFT2      = 0x00000002,
            IN1_B_SOCKET_ROTATE_TURN_LEFT3      = 0x00000004,
            IN1_B_SOCKET_ROTATE_TURN_LEFT4      = 0x00000008,  //12
            IN1_B_SOCKET_ROTATE_TURN_HOME1      = 0x00000010,
            IN1_B_SOCKET_ROTATE_TURN_HOME2      = 0x00000020,
            IN1_B_SOCKET_ROTATE_TURN_HOME3      = 0x00000040,
            IN1_B_SOCKET_ROTATE_TURN_HOME4      = 0x00000080,  //16
            //
            IN2_B_SOCKET_ROTATE_GRIP1           = 0x00000001,  //17
            IN2_B_SOCKET_ROTATE_GRIP2           = 0x00000002,
            IN2_B_SOCKET_ROTATE_GRIP3           = 0x00000004,
            IN2_B_SOCKET_ROTATE_GRIP4           = 0x00000008,  //20
            IN2_B_SOCKET_ROTATE_UNGRIP1         = 0x00000010,
            IN2_B_SOCKET_ROTATE_UNGRIP2         = 0x00000020,
            IN2_B_SOCKET_ROTATE_UNGRIP3         = 0x00000040,
            IN2_B_SOCKET_ROTATE_UNGRIP4         = 0x00000080,            //24
            //
            IN3_B_SOCKET_GOOD_DETECT1           = 0x00000001,   //25
            IN3_B_SOCKET_GOOD_DETECT2           = 0x00000002,
            IN3_B_SOCKET_GOOD_DETECT3           = 0x00000004,
            IN3_B_SOCKET_GOOD_DETECT4           = 0x00000008,   //28
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
        public enum DIO_IN_ADDR_CH12 : uint
        {
            IN10_C_SOCKET_ROTATE_UP1             = 0x00000001,  //1
            IN10_C_SOCKET_ROTATE_UP2             = 0x00000002,
            IN10_C_SOCKET_ROTATE_UP3             = 0x00000004,
            IN10_C_SOCKET_ROTATE_UP4             = 0x00000008,  //4
            IN10_C_SOCKET_ROTATE_DOWN1           = 0x00000010,
            IN10_C_SOCKET_ROTATE_DOWN2           = 0x00000020,
            IN10_C_SOCKET_ROTATE_DOWN3           = 0x00000040,
            IN10_C_SOCKET_ROTATE_DOWN4           = 0x00000080,  //8
            //
            IN11_C_SOCKET_ROTATE_TURN_LEFT1      = 0x00000001,  //9
            IN11_C_SOCKET_ROTATE_TURN_LEFT2      = 0x00000002,
            IN11_C_SOCKET_ROTATE_TURN_LEFT3      = 0x00000004,
            IN11_C_SOCKET_ROTATE_TURN_LEFT4      = 0x00000008,  //12
            IN11_C_SOCKET_ROTATE_TURN_HOME1      = 0x00000010,
            IN11_C_SOCKET_ROTATE_TURN_HOME2      = 0x00000020,
            IN11_C_SOCKET_ROTATE_TURN_HOME3      = 0x00000040,
            IN11_C_SOCKET_ROTATE_TURN_HOME4      = 0x00000080,  //16
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
            IN3_C_SOCKET_GOOD_DETECT1           = 0x00000001,   //25
            IN3_C_SOCKET_GOOD_DETECT2           = 0x00000002,
            IN3_C_SOCKET_GOOD_DETECT3           = 0x00000004,
            IN3_C_SOCKET_GOOD_DETECT4           = 0x00000008,   //28
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
        public enum DIO_IN_ADDR_CH14 : uint
        {
            IN10_D_SOCKET_ROTATE_UP1             = 0x00000001,  //1
            IN10_D_SOCKET_ROTATE_UP2             = 0x00000002,
            IN10_D_SOCKET_ROTATE_UP3             = 0x00000004,
            IN10_D_SOCKET_ROTATE_UP4             = 0x00000008,  //4
            IN10_D_SOCKET_ROTATE_DOWN1           = 0x00000010,
            IN10_D_SOCKET_ROTATE_DOWN2           = 0x00000020,
            IN10_D_SOCKET_ROTATE_DOWN3           = 0x00000040,
            IN10_D_SOCKET_ROTATE_DOWN4           = 0x00000080,  //8
            //
            IN11_D_SOCKET_ROTATE_TURN_LEFT1      = 0x00000001,  //9
            IN11_D_SOCKET_ROTATE_TURN_LEFT2      = 0x00000002,
            IN11_D_SOCKET_ROTATE_TURN_LEFT3      = 0x00000004,
            IN11_D_SOCKET_ROTATE_TURN_LEFT4      = 0x00000008,  //12
            IN11_D_SOCKET_ROTATE_TURN_HOME1      = 0x00000010,
            IN11_D_SOCKET_ROTATE_TURN_HOME2      = 0x00000020,
            IN11_D_SOCKET_ROTATE_TURN_HOME3      = 0x00000040,
            IN11_D_SOCKET_ROTATE_TURN_HOME4      = 0x00000080,  //16
            //
            IN12_D_SOCKET_ROTATE_GRIP1           = 0x00000001,  //17
            IN12_D_SOCKET_ROTATE_GRIP2           = 0x00000002,
            IN12_D_SOCKET_ROTATE_GRIP3           = 0x00000004,
            IN12_D_SOCKET_ROTATE_GRIP4           = 0x00000008,  //20
            IN12_D_SOCKET_ROTATE_UNGRIP1         = 0x00000010,
            IN12_D_SOCKET_ROTATE_UNGRIP2         = 0x00000020,
            IN12_D_SOCKET_ROTATE_UNGRIP3         = 0x00000040,
            IN12_D_SOCKET_ROTATE_UNGRIP4         = 0x00000080,            //24
            //
            IN3_D_SOCKET_GOOD_DETECT1            = 0x00000001,   //25
            IN3_D_SOCKET_GOOD_DETECT2            = 0x00000002,
            IN3_D_SOCKET_GOOD_DETECT3            = 0x00000004,
            IN3_D_SOCKET_GOOD_DETECT4            = 0x00000008,   //28
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
        public enum DIO_OUT_ADDR_CH1 : uint
        {
            OUT0_ALL_DOOR_UNLOCK                        = 0x00000001,  //1
            TEMP2 = 0x00000002,
            TEMP3 = 0x00000004,
            TEMP4 = 0x00000008,  //4
            OUT0_LEFT_MAGAZINE_LOAD_LAMP                = 0x00000010,
            OUT0_LEFT_MAGAZINE_LOAD_COMPLETE_LAMP       = 0x00000020,
            OUT0_RIGHT_MAGAZINE_LOAD_LAMP               = 0x00000040,
            OUT0_RIGHT_MAGAZINE_LOAD_COMPLETE_LAMP      = 0x00000080,  //8
            //
            OUT1_LEFT_MAGAZINE_DOOR_LOCK_UP             = 0x00000001,  //9
            OUT1_LEFT_MAGAZINE_DOOR_LOCK_DOWN           = 0x00000002,
            OUT1_RIGHT_MAGAZINE_DOOR_LOCK_UP            = 0x00000004,
            OUT1_RIGHT_MAGAZINE_DOOR_LOCK_DOWN          = 0x00000008,  //12
            OUT1_LEFT_NG_TRAY_LOAD_MODE_LAMP            = 0x00000010,
            OUT1_LEFT_NG_TRAY_COMPLETE_MODE_LAMP        = 0x00000020,
            OUT1_RIGHT_NG_TRAY_LOAD_MODE_LAMP           = 0x00000040,
            OUT1_RIGHT_NG_TRAY_COMPLETE_MODE_LAMP       = 0x00000080,  //16
            //
            OUT2_NG_TRAY_LEFT_DOOR_LOCK_UP              = 0x00000001,  //17
            OUT2_NG_TRAY_LEFT_DOOR_LOCK_DOWN            = 0x00000002,
            OUT2_NG_TRAY_RIGHT_DOOR_LOCK_UP             = 0x00000004,
            OUT2_NG_TRAY_RIGHT_DOOR_LOCK_DOWN           = 0x00000008,  //20
            TEMP21 = 0x00000010,
            TEMP22 = 0x00000020,
            TEMP23 = 0x00000040,
            TEMP24 = 0x00000080,  //24
            //
            TEMP25 = 0x00000001,  //25
            TEMP26 = 0x00000002,
            TEMP27 = 0x00000004,
            TEMP28 = 0x00000008,  //28
            TEMP29 = 0x00000010,
            TEMP30 = 0x00000020,
            TEMP31 = 0x00000040,
            TEMP32 = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 3
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH3 : uint
        {
            OUT0_TRANSFER_LOAD_PICKER_DOWN1         = 0x00000001,            //1
            OUT0_TRANSFER_LOAD_PICKER_DOWN2         = 0x00000002,
            OUT0_TRANSFER_LOAD_PICKER_DOWN3         = 0x00000004,
            OUT0_TRANSFER_LOAD_PICKER_DOWN4         = 0x00000008,            //4
            OUT0_TRANSFER_LOAD_PICKER_UP1           = 0x00000010,
            OUT0_TRANSFER_LOAD_PICKER_UP2           = 0x00000020,
            OUT0_TRANSFER_LOAD_PICKER_UP3           = 0x00000040,
            OUT0_TRANSFER_LOAD_PICKER_UP4           = 0x00000080,                 //8
            //
            OUT1_TRANSFER_UNLOAD_PICKER_DOWN1       = 0x00000001,                  //9
            OUT1_TRANSFER_UNLOAD_PICKER_DOWN2       = 0x00000002,
            OUT1_TRANSFER_UNLOAD_PICKER_DOWN3       = 0x00000004,
            OUT1_TRANSFER_UNLOAD_PICKER_DOWN4       = 0x00000008,          //12
            OUT1_TRANSFER_UNLOAD_PICKER_UP1         = 0x00000010,
            OUT1_TRANSFER_UNLOAD_PICKER_UP2         = 0x00000020,
            OUT1_TRANSFER_UNLOAD_PICKER_UP3         = 0x00000040,
            OUT1_TRANSFER_UNLOAD_PICKER_UP4         = 0x00000080,                  //16
            //                      
            OUT2_TRANSFER_LOAD_PICKER_VACUUM_ON1       = 0x00000001,            //17
            OUT2_TRANSFER_LOAD_PICKER_VACUUM_ON2       = 0x00000002,
            OUT2_TRANSFER_LOAD_PICKER_VACUUM_ON3       = 0x00000004,
            OUT2_TRANSFER_LOAD_PICKER_VACUUM_ON4       = 0x00000008,          //20
            OUT2_TRANSFER_LOAD_PICKER_BLOW_ON1         = 0x00000010,
            OUT2_TRANSFER_LOAD_PICKER_BLOW_ON2         = 0x00000020,
            OUT2_TRANSFER_LOAD_PICKER_BLOW_ON3         = 0x00000040,
            OUT2_TRANSFER_LOAD_PICKER_BLOW_ON4         = 0x00000080,                  //24
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
        public enum DIO_OUT_ADDR_CH5 : uint
        {
            OUT0_A_SOCKET_CONTACT_UP1           = 0x00000001,  //1
            OUT0_A_SOCKET_CONTACT_UP2           = 0x00000002,
            OUT0_A_SOCKET_CONTACT_UP3           = 0x00000004,
            OUT0_A_SOCKET_CONTACT_UP4           = 0x00000008,  //4
            OUT0_A_SOCKET_CONTACT_DOWN1         = 0x00000010,
            OUT0_A_SOCKET_CONTACT_DOWN2         = 0x00000020,
            OUT0_A_SOCKET_CONTACT_DOWN3         = 0x00000040,
            OUT0_A_SOCKET_CONTACT_DOWN4         = 0x00000080,  //8
            //
            OUT1_A_SOCKET_CONTACT_FORWARD1      = 0x00000001,  //9
            OUT1_A_SOCKET_CONTACT_FORWARD2      = 0x00000002,
            OUT1_A_SOCKET_CONTACT_FORWARD3      = 0x00000004,
            OUT1_A_SOCKET_CONTACT_FORWARD4      = 0x00000008,  //12
            OUT1_A_SOCKET_CONTACT_BACKWARD1     = 0x00000010,
            OUT1_A_SOCKET_CONTACT_BACKWARD2     = 0x00000020,
            OUT1_A_SOCKET_CONTACT_BACKWARD3     = 0x00000040,
            OUT1_A_SOCKET_CONTACT_BACKWARD4     = 0x00000080,  //16
            //                      
            OUT2_B_SOCKET_CONTACT_UP1           = 0x00000001,  //17
            OUT2_B_SOCKET_CONTACT_UP2           = 0x00000002,
            OUT2_B_SOCKET_CONTACT_UP3           = 0x00000004,
            OUT2_B_SOCKET_CONTACT_UP4           = 0x00000008,  //20
            OUT2_B_SOCKET_CONTACT_DOWN1         = 0x00000010,
            OUT2_B_SOCKET_CONTACT_DOWN2         = 0x00000020,
            OUT2_B_SOCKET_CONTACT_DOWN3         = 0x00000040,
            OUT2_B_SOCKET_CONTACT_DOWN4         = 0x00000080,  //24
            //
            OUT3_B_SOCKET_CONTACT_FORWARD1      = 0x00000001,  //25
            OUT3_B_SOCKET_CONTACT_FORWARD2      = 0x00000002,
            OUT3_B_SOCKET_CONTACT_FORWARD3      = 0x00000004,
            OUT3_B_SOCKET_CONTACT_FORWARD4      = 0x00000008,  //28
            OUT3_B_SOCKET_CONTACT_BACKWARD1     = 0x00000010,
            OUT3_B_SOCKET_CONTACT_BACKWARD2     = 0x00000020,
            OUT3_B_SOCKET_CONTACT_BACKWARD3     = 0x00000040,
            OUT3_B_SOCKET_CONTACT_BACKWARD4     = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 7
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH7 : uint
        {
            OUT0_C_SOCKET_CONTACT_UP1           = 0x00000001,  //1
            OUT0_C_SOCKET_CONTACT_UP2           = 0x00000002,
            OUT0_C_SOCKET_CONTACT_UP3           = 0x00000004,
            OUT0_C_SOCKET_CONTACT_UP4           = 0x00000008,  //4
            OUT0_C_SOCKET_CONTACT_DOWN1         = 0x00000010,
            OUT0_C_SOCKET_CONTACT_DOWN2         = 0x00000020,
            OUT0_C_SOCKET_CONTACT_DOWN3         = 0x00000040,
            OUT0_C_SOCKET_CONTACT_DOWN4         = 0x00000080,  //8
            //
            OUT1_C_SOCKET_CONTACT_FORWARD1      = 0x00000001,  //9
            OUT1_C_SOCKET_CONTACT_FORWARD2      = 0x00000002,
            OUT1_C_SOCKET_CONTACT_FORWARD3      = 0x00000004,
            OUT1_C_SOCKET_CONTACT_FORWARD4      = 0x00000008,  //12
            OUT1_C_SOCKET_CONTACT_BACKWARD1     = 0x00000010,
            OUT1_C_SOCKET_CONTACT_BACKWARD2     = 0x00000020,
            OUT1_C_SOCKET_CONTACT_BACKWARD3     = 0x00000040,
            OUT1_C_SOCKET_CONTACT_BACKWARD4     = 0x00000080,  //16
            //                      
            OUT2_D_SOCKET_CONTACT_UP1           = 0x00000001,  //17
            OUT2_D_SOCKET_CONTACT_UP2           = 0x00000002,
            OUT2_D_SOCKET_CONTACT_UP3           = 0x00000004,
            OUT2_D_SOCKET_CONTACT_UP4           = 0x00000008,  //20
            OUT2_D_SOCKET_CONTACT_DOWN1         = 0x00000010,
            OUT2_D_SOCKET_CONTACT_DOWN2         = 0x00000020,
            OUT2_D_SOCKET_CONTACT_DOWN3         = 0x00000040,
            OUT2_D_SOCKET_CONTACT_DOWN4         = 0x00000080,  //24
            //
            OUT3_D_SOCKET_CONTACT_FORWARD1      = 0x00000001,  //25
            OUT3_D_SOCKET_CONTACT_FORWARD2      = 0x00000002,
            OUT3_D_SOCKET_CONTACT_FORWARD3      = 0x00000004,
            OUT3_D_SOCKET_CONTACT_FORWARD4      = 0x00000008,  //28
            OUT3_D_SOCKET_CONTACT_BACKWARD1     = 0x00000010,
            OUT3_D_SOCKET_CONTACT_BACKWARD2     = 0x00000020,
            OUT3_D_SOCKET_CONTACT_BACKWARD3     = 0x00000040,
            OUT3_D_SOCKET_CONTACT_BACKWARD4     = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 9
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH9 : uint
        {
            OUT0_A_SOCKET_ROTATE_UP1            = 0x00000001,  //1
            OUT0_A_SOCKET_ROTATE_UP2            = 0x00000002,
            OUT0_A_SOCKET_ROTATE_UP3            = 0x00000004,
            OUT0_A_SOCKET_ROTATE_UP4            = 0x00000008,  //4
            OUT0_A_SOCKET_ROTATE_DOWN1          = 0x00000010,
            OUT0_A_SOCKET_ROTATE_DOWN2          = 0x00000020,
            OUT0_A_SOCKET_ROTATE_DOWN3          = 0x00000040,
            OUT0_A_SOCKET_ROTATE_DOWN4          = 0x00000080,  //8
            //
            OUT1_A_SOCKET_ROTATE_TURN_LEFT1     = 0x00000001,  //9
            OUT1_A_SOCKET_ROTATE_TURN_LEFT2     = 0x00000002,
            OUT1_A_SOCKET_ROTATE_TURN_LEFT3     = 0x00000004,
            OUT1_A_SOCKET_ROTATE_TURN_LEFT4     = 0x00000008,  //12
            OUT1_A_SOCKET_ROTATE_TURN_HOME1     = 0x00000010,
            OUT1_A_SOCKET_ROTATE_TURN_HOME2     = 0x00000020,
            OUT1_A_SOCKET_ROTATE_TURN_HOME3     = 0x00000040,
            OUT1_A_SOCKET_ROTATE_TURN_HOME4     = 0x00000080,  //16
            //                      
            OUT2_A_SOCKET_ROTATE_GRIP1          = 0x00000001,  //17
            OUT2_A_SOCKET_ROTATE_GRIP2          = 0x00000002,
            OUT2_A_SOCKET_ROTATE_GRIP3          = 0x00000004,
            OUT2_A_SOCKET_ROTATE_GRIP4          = 0x00000008,  //20
            OUT2_A_SOCKET_ROTATE_UNGRIP1        = 0x00000010,
            OUT2_A_SOCKET_ROTATE_UNGRIP2        = 0x00000020,
            OUT2_A_SOCKET_ROTATE_UNGRIP3        = 0x00000040,
            OUT2_A_SOCKET_ROTATE_UNGRIP4        = 0x00000080,  //24
            //
            TEMP25        = 0x00000001,  //25
            TEMP26        = 0x00000002,
            TEMP27        = 0x00000004,
            TEMP28        = 0x00000008,  //28
            TEMP29        = 0x00000010,
            TEMP30        = 0x00000020,
            TEMP31        = 0x00000040,
            TEMP32        = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 11
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH11 : uint
        {
            OUT0_B_SOCKET_ROTATE_UP1           = 0x00000001,  //1
            OUT0_B_SOCKET_ROTATE_UP2           = 0x00000002,
            OUT0_B_SOCKET_ROTATE_UP3           = 0x00000004,
            OUT0_B_SOCKET_ROTATE_UP4           = 0x00000008,  //4
            OUT0_B_SOCKET_ROTATE_DOWN1         = 0x00000010,
            OUT0_B_SOCKET_ROTATE_DOWN2         = 0x00000020,
            OUT0_B_SOCKET_ROTATE_DOWN3         = 0x00000040,
            OUT0_B_SOCKET_ROTATE_DOWN4         = 0x00000080,  //8
            //
            OUT1_B_SOCKET_ROTATE_TURN_LEFT1    = 0x00000001,  //9
            OUT1_B_SOCKET_ROTATE_TURN_LEFT2    = 0x00000002,
            OUT1_B_SOCKET_ROTATE_TURN_LEFT3    = 0x00000004,
            OUT1_B_SOCKET_ROTATE_TURN_LEFT4    = 0x00000008,  //12
            OUT1_B_SOCKET_ROTATE_TURN_HOME1    = 0x00000010,
            OUT1_B_SOCKET_ROTATE_TURN_HOME2    = 0x00000020,
            OUT1_B_SOCKET_ROTATE_TURN_HOME3    = 0x00000040,
            OUT1_B_SOCKET_ROTATE_TURN_HOME4    = 0x00000080,  //16
            //                      
            OUT2_B_SOCKET_ROTATE_GRIP1         = 0x00000001,  //17
            OUT2_B_SOCKET_ROTATE_GRIP2         = 0x00000002,
            OUT2_B_SOCKET_ROTATE_GRIP3         = 0x00000004,
            OUT2_B_SOCKET_ROTATE_GRIP4         = 0x00000008,  //20
            OUT2_B_SOCKET_ROTATE_UNGRIP1       = 0x00000010,
            OUT2_B_SOCKET_ROTATE_UNGRIP2       = 0x00000020,
            OUT2_B_SOCKET_ROTATE_UNGRIP3       = 0x00000040,
            OUT2_B_SOCKET_ROTATE_UNGRIP4       = 0x00000080,  //24
            //
            TEMP25         = 0x00000001,  //25
            TEMP26         = 0x00000002,
            TEMP27         = 0x00000004,
            TEMP28         = 0x00000008,  //28
            TEMP29         = 0x00000010,
            TEMP30         = 0x00000020,
            TEMP31         = 0x00000040,
            TEMP32         = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 13
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH13 : uint
        {
            OUT0_C_SOCKET_ROTATE_UP1           = 0x00000001,  //1
            OUT0_C_SOCKET_ROTATE_UP2           = 0x00000002,
            OUT0_C_SOCKET_ROTATE_UP3           = 0x00000004,
            OUT0_C_SOCKET_ROTATE_UP4           = 0x00000008,  //4
            OUT0_C_SOCKET_ROTATE_DOWN1         = 0x00000010,
            OUT0_C_SOCKET_ROTATE_DOWN2         = 0x00000020,
            OUT0_C_SOCKET_ROTATE_DOWN3         = 0x00000040,
            OUT0_C_SOCKET_ROTATE_DOWN4         = 0x00000080,  //8
            //
            OUT1_C_SOCKET_ROTATE_TURN_LEFT1    = 0x00000001,  //9
            OUT1_C_SOCKET_ROTATE_TURN_LEFT2    = 0x00000002,
            OUT1_C_SOCKET_ROTATE_TURN_LEFT3    = 0x00000004,
            OUT1_C_SOCKET_ROTATE_TURN_LEFT4    = 0x00000008,  //12
            OUT1_C_SOCKET_ROTATE_TURN_HOME1    = 0x00000010,
            OUT1_C_SOCKET_ROTATE_TURN_HOME2    = 0x00000020,
            OUT1_C_SOCKET_ROTATE_TURN_HOME3    = 0x00000040,
            OUT1_C_SOCKET_ROTATE_TURN_HOME4    = 0x00000080,  //16
            //                      
            OUT2_C_SOCKET_ROTATE_GRIP1         = 0x00000001,  //17
            OUT2_C_SOCKET_ROTATE_GRIP2         = 0x00000002,
            OUT2_C_SOCKET_ROTATE_GRIP3         = 0x00000004,
            OUT2_C_SOCKET_ROTATE_GRIP4         = 0x00000008,  //20
            OUT2_C_SOCKET_ROTATE_UNGRIP1       = 0x00000010,
            OUT2_C_SOCKET_ROTATE_UNGRIP2       = 0x00000020,
            OUT2_C_SOCKET_ROTATE_UNGRIP3       = 0x00000040,
            OUT2_C_SOCKET_ROTATE_UNGRIP4       = 0x00000080,  //24
            //
            TEMP25         = 0x00000001,  //25
            TEMP26         = 0x00000002,
            TEMP27         = 0x00000004,
            TEMP28         = 0x00000008,  //28
            TEMP29         = 0x00000010,
            TEMP30         = 0x00000020,
            TEMP31         = 0x00000040,
            TEMP32         = 0x00000080   //32
        };

        //----------------------------------------------------------------------------------------------------------------
        //
        //  OUT CH : 15
        //
        //
        //----------------------------------------------------------------------------------------------------------------
        public enum DIO_OUT_ADDR_CH15 : uint
        {
            OUT0_D_SOCKET_ROTATE_UP1           = 0x00000001,  //1
            OUT0_D_SOCKET_ROTATE_UP2           = 0x00000002,
            OUT0_D_SOCKET_ROTATE_UP3           = 0x00000004,
            OUT0_D_SOCKET_ROTATE_UP4           = 0x00000008,  //4
            OUT0_D_SOCKET_ROTATE_DOWN1         = 0x00000010,
            OUT0_D_SOCKET_ROTATE_DOWN2         = 0x00000020,
            OUT0_D_SOCKET_ROTATE_DOWN3         = 0x00000040,
            OUT0_D_SOCKET_ROTATE_DOWN4         = 0x00000080,  //8
            //
            OUT1_D_SOCKET_ROTATE_TURN_LEFT1    = 0x00000001,  //9
            OUT1_D_SOCKET_ROTATE_TURN_LEFT2    = 0x00000002,
            OUT1_D_SOCKET_ROTATE_TURN_LEFT3    = 0x00000004,
            OUT1_D_SOCKET_ROTATE_TURN_LEFT4    = 0x00000008,  //12
            OUT1_D_SOCKET_ROTATE_TURN_HOME1    = 0x00000010,
            OUT1_D_SOCKET_ROTATE_TURN_HOME2    = 0x00000020,
            OUT1_D_SOCKET_ROTATE_TURN_HOME3    = 0x00000040,
            OUT1_D_SOCKET_ROTATE_TURN_HOME4    = 0x00000080,  //16
            //                      
            OUT2_D_SOCKET_ROTATE_GRIP1         = 0x00000001,  //17
            OUT2_D_SOCKET_ROTATE_GRIP2         = 0x00000002,
            OUT2_D_SOCKET_ROTATE_GRIP3         = 0x00000004,
            OUT2_D_SOCKET_ROTATE_GRIP4         = 0x00000008,  //20
            OUT2_D_SOCKET_ROTATE_UNGRIP1       = 0x00000010,
            OUT2_D_SOCKET_ROTATE_UNGRIP2       = 0x00000020,
            OUT2_D_SOCKET_ROTATE_UNGRIP3       = 0x00000040,
            OUT2_D_SOCKET_ROTATE_UNGRIP4       = 0x00000080,  //24
            //
            TEMP25            = 0x00000001,  //25
            TEMP26            = 0x00000002,
            TEMP27            = 0x00000004,
            TEMP28            = 0x00000008,  //28
            TEMP29            = 0x00000010,
            TEMP30            = 0x00000020,
            TEMP31            = 0x00000040,
            TEMP32            = 0x00000080   //32
        };
    }//END
}
