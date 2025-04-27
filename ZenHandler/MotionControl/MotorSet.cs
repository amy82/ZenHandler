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
            eLiftMotorList.LOAD_Z_L,
            eLiftMotorList.UNLOAD_Z_L,
            eLiftMotorList.GANTRY_Y_L,
            eLiftMotorList.LOAD_Z_R,
            eLiftMotorList.UNLOAD_Z_R,
            eLiftMotorList.GANTRY_Y_R
        };
        public enum eLiftMotorList : int
        {
            LOAD_Z_L = 3, UNLOAD_Z_L, GANTRY_Y_L, LOAD_Z_R, UNLOAD_Z_R, GANTRY_Y_R, TOTAL_LIFT_MOTOR_COUNT
        };


        public static readonly eMagazineMotorList[] ValidMagazineMotors =
        {
            eMagazineMotorList.L_MAGAZINE_Z,
            eMagazineMotorList.L_MAGAZINE_Y,
            eMagazineMotorList.R_MAGAZINE_Z,
            eMagazineMotorList.R_MAGAZINE_Y
        };
        public enum eMagazineMotorList : int
        {
            L_MAGAZINE_Z = 3, L_MAGAZINE_Y, R_MAGAZINE_Z, R_MAGAZINE_Y, TOTAL_MAGAZINE_MOTOR_COUNT
        };

        public static readonly eSocketMotorList[] ValidSocketMotors =
        {
            eSocketMotorList.FRONT_X,
            eSocketMotorList.BACK_X,
            eSocketMotorList.BACK_Y,
            eSocketMotorList.CAMZ_L,
            eSocketMotorList.CAMZ_R
        };
        public enum eSocketMotorList : int
        {
            FRONT_X = 7, BACK_X, BACK_Y, CAMZ_L, CAMZ_R, TOTAL_SOCKET_MOTOR_COUNT
        };
        //AOI
        //
        //eeprom == 투입 LIFT 2개 , 배출 LIFT 2개
        //aoi    == 투입 LIFT 2개 , 배출 LIFT 2개 , CAM z축 2개
        //fw     == 매거진 Z축 + Y축 2세트
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
