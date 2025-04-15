﻿using System;
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


        //EEPROM
        //
        public enum eMotorList : int        
        {
            TRANSFER_X = 0, TRANSFER_Y, TRANSFER_Z, L_IN_LIFT, L_OUT_LIFT, R_IN_LIFT, R_OUT_LIFT, MAX_MOTOR_LIST_COUNT
        };

        //AOI
        //
        public enum eAOI_MotorList : int
        {
            TRANSFER_X = 0, TRANSFER_Y, TRANSFER_Z, L_IN_LIFT, L_OUT_LIFT, R_IN_LIFT, R_OUT_LIFT, CAM_Z1, CAM_Z2 //MAX_MOTOR_LIST_COUNT
        };

        //FW
        //
        public enum eFW_MotorList : int
        {
            TRANSFER_X = 0, TRANSFER_Y, TRANSFER_Z, L_MAGAZINE_Z, L_MAGAZINE_Y, R_MAGAZINE_Z, R_MAGAZINE_Y, //MAX_MOTOR_LIST_COUNT
        };
        //eeprom == 투입 LIFT 2개 , 배출 LIFT 2개
        //aoi    == 투입 LIFT 2개 , 배출 LIFT 2개 , CAM z축 2개
        //fw     == 매거진 Z축 + Y축 2세트


        public static AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = {
            AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor,
            AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor,
            AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor,
            AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, 
            AXT_MOTION_HOME_DETECT.HomeSensor
        };

        public static AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = {
            AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW,
            AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW,
            AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW,
            AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW,
            AXT_MOTION_MOVE_DIR.DIR_CCW
        };

        public static MotorDefine.eMotorType[] MOTOR_TYPE =
            {
            MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR,
            MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR,
            MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR,
            MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR,
            MotorDefine.eMotorType.LINEAR

        };
        
        public static int[] MOTOR_MAX_SPEED = { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 };

        public static double[] OrgFirstVel = { 20000.0, 10000.0, 5000.0, 3000.0, 3000.0, 3000.0 };
        public static double[] OrgSecondVel = { 5000.0, 7000.0, 2000.0, 1000.0, 1000.0, 1000.0 };
        public static double[] OrgThirdVel = { 2000.0, 2000.0, 500.0, 500.0, 500.0, 500.0 };
        public static double[] OrgLastVel = { 100.0, 100.0, 50.0, 50.0, 50.0, 50.0 };
        public static double[] OrgAccFirst = { 0.3, 0.3, 0.3, 0.3, 0.3, 0.3 };
        public static double[] OrgAccSecond = { 0.3, 0.3, 0.3, 0.3, 0.3, 0.3 };


        public static string[] TEACH_SET_MENU = { "원점상태", "ServoOn", "Alarm", "Limit(+)", "HOME", "Limit(-)", "속도(mm/s)", "가속도(sec)", "감속도(sec)" };


        public static double MaxLimitAccDec = 3.0;
        public static double MinLimitAccDec = 0.1;

        public static double MaxLimitSpeed = 1000.0;
        public static double MinLimitspeed = 10.0;
    }
}
