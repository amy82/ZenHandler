using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.MotionControl
{
    
    public static class MotorSet
    {

        public const int MAX_MOTOR_COUNT = 3;//6;       //<-이 수로 아진 모터 세팅

        //
        public static int MOTOR_MOVE_TIMEOUT = 20000;   //20s
        public static int MOTOR_MANUAL_MOVE_TIMEOUT = 10000;   //10s
        public static int IO_TIMEOUT = 3000;   //3s
        public static double ENCORDER_GAP = 0.03;
        public static bool MOTOR_ACC_TYPE_SEC = true;


        public static readonly eTransferMotorList[] ValidTransferMotors =
        {
            eTransferMotorList.TRANSFER_X,
            eTransferMotorList.TRANSFER_Y,
            eTransferMotorList.TRANSFER_Z
        };
        public enum eTransferMotorList : int        
        {
            TRANSFER_X = 0, TRANSFER_Y, TRANSFER_Z, TOTAL_TRANSFER_MOTOR_COUNT
        };
        public static readonly eLiftMotorList[] ValidLiftMotors =
        {
            eLiftMotorList.FRONT_X,
            eLiftMotorList.BACK_X,
            eLiftMotorList.LEFT_Z,
            eLiftMotorList.RIGHT_Z
        };
        public enum eLiftMotorList : int
        {
            FRONT_X = 5, BACK_X, LEFT_Z, RIGHT_Z, TOTAL_LIFT_MOTOR_COUNT
        };


        public static readonly eMagazineMotorList[] ValidMagazineMotors =
        {
            eMagazineMotorList.L_MAGAZINE_Y,
            eMagazineMotorList.L_MAGAZINE_Z,
            eMagazineMotorList.R_MAGAZINE_Y,
            eMagazineMotorList.R_MAGAZINE_Z
        };

        public enum eMagazineMotorList : int
        {
            L_MAGAZINE_Z = 3, L_MAGAZINE_Y, R_MAGAZINE_Z, R_MAGAZINE_Y, TOTAL_MAGAZINE_MOTOR_COUNT
        };

        public static readonly eAoisocketMotorList[] ValidAoiSocketMotors =
        {
            eAoisocketMotorList.AOI_LEFT_X,
            eAoisocketMotorList.AOI_LEFT_Z,
            eAoisocketMotorList.AOI_RIGHT_X,
            eAoisocketMotorList.AOI_RIGHT_Z
        };
        public enum eAoisocketMotorList : int
        {
            AOI_LEFT_X = 3, AOI_LEFT_Z, AOI_RIGHT_X, AOI_RIGHT_Z, TOTAL_AOI_SOCKET_MOTOR_COUNT
        };

        public static readonly eEEpromSocketMotorList[] ValidEEpromSocketMotors =
        {
            eEEpromSocketMotorList.EEPROM_FRONT_X,
            eEEpromSocketMotorList.EEPROM_BACK_X
        };
        public enum eEEpromSocketMotorList : int
        {
            EEPROM_FRONT_X = 3, EEPROM_BACK_X, TOTAL_EEPROM_SOCKET_MOTOR_COUNT
        };
        //
        //EEPROM == LIFT Z 2개, GANTRY X 2개, SOCKET X축 2개 = TOTAL : 6개
        //aoi    == LIFT Z 2개, GANTRY X 2개 ,CAM X축 2개, CAM Z축 2개 = TOTAL : 8개
        //fw     == 매거진 Z 2개, Y축 2개 = TOTAL : 4개
        public enum TrayPosition        //LIFT , MAGAZINE 왼쪽, 오른족 구분
        {
            Left,
            Right
        }
        public enum eJogDic : int
        {
            PLUS_MOVE = 1, MINUS_MOVE = -1
        };

        public static string[] TEACH_SET_MENU = { "원점상태", "ServoOn", "Alarm", "Limit(+)", "HOME", "Limit(-)", "속도(mm/s)", "가속도(sec)", "감속도(sec)" };


        public static double MaxLimitAccDec = 3.0;
        public static double MinLimitAccDec = 0.1;

        public static double MaxLimitSpeed = 1000.0;
        public static double MinLimitspeed = 10.0;
    }
}
