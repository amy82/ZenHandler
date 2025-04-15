using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace ZenHandler
{
    public class MotorControl
    {
        public event delLogSender eLogSender;       //외부에서 호출할때 사용

        //public bool bConnected = false;
        //public const int MAX_MOTOR_COUNT = 11;

        //public const int PCB_UNIT_COUNT = (int)ePcbMotor.PCB_TY + 1;
        //public const int LENS_UNIT_COUNT = MAX_MOTOR_COUNT - PCB_UNIT_COUNT;

        //public MotorAxisold[] PcbMotorAxis = new MotorAxisold[PCB_UNIT_COUNT];
        //public MotorAxisold[] LensMotorAxis = new MotorAxisold[LENS_UNIT_COUNT];
        //public enum eUnit : int
        //{
        //    PCB_UNIT = 0,
        //    LENS_UNIT,
        //};
        //public enum eUnit_Name : int
        //{
        //    PCB_AXIS = 0, LENS_AXIS, MAX_UNIT_COUNT
        //};
        //public enum ePcbMotor : int
        //{
        //    PCB_X = 0, PCB_Y, PCB_Z, PCB_TH, PCB_TX, PCB_TY, MAX_PCB_MOTOR_COUNT
        //};
        //public enum eLensMotor : int
        //{
        //    LENS_X = 0,LENS_Y, LENS_TY, LENS_TX, LENS_Z, MAX_LENS_MOTOR_COUNT
        //};
        //public string[] PCB_MOTOR_NAME = { "PCB X", "PCB Y", "PCB Z", "PCB TH", "PCB TX", "PCB TY"};
        //public string[] LENS_MOTOR_NAME = { "LENS X", "LENS Y", "LENS TY" , "LENS TX", "LENS Z"};
        //public const double ENCORDER_GAP = 1.0;

        //public AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { 
        //    AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor
        //, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.PosEndLimit};

        //public AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW
        //, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CW};

        //public MotorDefine.eMotorType[] PCB_MOTOR_TYPE = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.STEPING, MotorDefine.eMotorType.STEPING, MotorDefine.eMotorType.STEPING, MotorDefine.eMotorType.STEPING};

        //public MotorDefine.eMotorType[] LENS_MOTOR_TYPE = {MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.STEPING, MotorDefine.eMotorType.STEPING, MotorDefine.eMotorType.STEPING};
        //public int[] PCB_MOTOR_MAX_SPEED = { 100, 100, 10, 5, 5, 5};
        //public int[] LENS_MOTOR_MAX_SPEED = { 100, 100, 10, 5, 5 };

        //public double[] OrgFirstVel = { 20000.0, 10000.0, 5000.0, 3000.0, 3000.0, 3000.0, 20000.0, 10000.0, 3000.0, 3000.0, 5000.0};
        //public double[] OrgSecondVel = { 5000.0, 7000.0, 2000.0, 1000.0, 1000.0, 1000.0, 5000.0, 7000.0, 1000.0, 1000.0, 2000.0 };
        //public double[] OrgThirdVel = { 2000.0, 2000.0, 500.0, 500.0, 500.0, 500.0,     2000.0, 2000.0, 500.0, 500.0, 500.0 };
        //public double[] OrgLastVel = { 100.0, 100.0, 50.0, 50.0, 50.0, 50.0, 100.0, 50.0, 50.0, 50.0, 50.0 };
        //public double[] OrgAccFirst = { 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3 };
        //public double[] OrgAccSecond = { 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3 };


        public int m_lAxisCounts = 0;                          // 제어 가능한 축갯수 선언 및 초기화
        public MotorControl()
        {
            
        }
        public void Motor_Init()        //최초 실행
        {
            //int i = 0;
            //int nAxisNum = 0;
            //for (i = 0; i < PCB_UNIT_COUNT; i++)//for (i = 0; i < MAX_MOTOR_COUNT; i++)
            //{
            //    //축번호 , 축이름 , 분해능 , 모터타입 , 원점 방향 , 원점 센서(Neg,Pos,Home)
            //    PcbMotorAxis[i] = new MotorAxis(i, PCB_MOTOR_NAME[i],
            //        Globalo.dataManage.teachingData.PcbMotorData.dMotorVel[i],
            //        Globalo.dataManage.teachingData.PcbMotorData.dMotorResol[i],
            //        PCB_MOTOR_TYPE[i],
            //        MOTOR_HOME_DIR[i],
            //        MOTOR_HOME_SENSOR[i]);

            //    PcbMotorAxis[i].bOrgState = false;
            //}


            //for (i = 0; i < LENS_UNIT_COUNT; i++)
            //{
            //    nAxisNum = i + (int)ePcbMotor.MAX_PCB_MOTOR_COUNT;

            //    LensMotorAxis[i] = new MotorAxis(nAxisNum,
            //        LENS_MOTOR_NAME[i],
            //        Globalo.dataManage.teachingData.LensMotorData.dMotorVel[i],
            //        Globalo.dataManage.teachingData.LensMotorData.dMotorResol[i],
            //        LENS_MOTOR_TYPE[i], 
            //        MOTOR_HOME_DIR[nAxisNum], 
            //        MOTOR_HOME_SENSOR[nAxisNum]);

            //    LensMotorAxis[i].bOrgState = false;

            //}
        }
        public bool Axl_Init()
        {
            int lBoardCount = 0;
            int i = 0;
            // ※ [CAUTION] 아래와 다른 Mot파일(모션 설정파일)을 사용할 경우 경로를 변경하십시요.
            //String szFilePath = "C:\\Program Files\\EzSoftware RM\\EzSoftware\\MotionDefault.mot";
            //++ AXL(AjineXtek Library)을 사용가능하게 하고 장착된 보드들을 초기화합니다.
            //if (CAXL.AxlOpen(7) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            //{
            //    //초기화 성공
            //    bConnected = true;
            //}
            //else
            //{
            //    //MessageBox.Show("Motion Intialize Fail..!!");
            //    eLogSender("CAxlMotion", "Motion Intialize Fail..!!", Globalo.eMessageName.M_ERROR);
            //    return false;
            //}
            //CAXL.AxlGetBoardCount(ref lBoardCount);
            //if (lBoardCount < 1)
            //{
            //    //MessageBox.Show("모션 보드 인식을 실패");
            //    eLogSender("CAxlMotion", $"모션 보드 인식을 실패", Globalo.eMessageName.M_ERROR);
            //    return false;
            //}
            //보드개수 확인 AxlGetBoardCount
            //++ 지정한 Mot파일의 설정값들로 모션보드의 설정값들을 일괄변경 적용합니다.
            //if (CAXM.AxmMotLoadParaAll(szFilePath) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            //{
            //    MessageBox.Show("Mot File Not Found.");
            //}

            AmpDisableAll();

            bool bAxlInit = true;
            ////for (i = 0; i < (int)eUnit_Name.MAX_UNIT_COUNT; i++)
            ////{
            ////    if (Axl_Axisconfig(i) != 1)
            ////    {
            ////        //fail
            ////        bAxlInit = false;
            ////    }
            ////}
            


            return bAxlInit;
        }
        
        public int Axl_Axisconfig(int nUnit)
        {
            int i = 0;
            //int j = 0;
            int nUseAxis = 0;
            int nMaxSpeed = 0;
            uint duRetCode = 0;
            MotorDefine.eMotorType nMotorType = MotorDefine.eMotorType.LINEAR;
            //++ 유효한 전체 모션축수를 반환합니다.

            CAXM.AxmInfoGetAxisCount(ref m_lAxisCounts);
            if (m_lAxisCounts < MAX_MOTOR_COUNT)
            {
                // _stprintf_s(szLog, SIZE_OF_1K, _T("모터 드라이브 개수 불일치[%d / %d]"), lAxisCount, (MAX_MOTOR_COUNT * MAX_UNIT_COUNT));
                eLogSender("CAxlMotion", $"[{m_lAxisCounts} / {(MAX_MOTOR_COUNT)}] 모터 드라이브 개수 불일치", Globalo.eMessageName.M_ERROR);

                return -4;
            }


            double dResol = 0.0;
            int nMotorCount = 0;

            if (nUnit == (int)eUnit_Name.PCB_AXIS)
            {
                nMotorCount = (int)ePcbMotor.MAX_PCB_MOTOR_COUNT;
            }
            else if (nUnit == (int)eUnit_Name.LENS_AXIS)
            {
                nMotorCount = (int)eLensMotor.MAX_LENS_MOTOR_COUNT;
            }
            else
            {
                eLogSender("CAxlMotion", "UNIT 설정 실패", Globalo.eMessageName.M_ERROR);
                return -5;
            }
            string sMotorName = "";

            for (i = 0; i < nMotorCount; i++)//for (i = 0; i < MAX_MOTOR_COUNT; i++)
            {
                
                if (nUnit == (int)eUnit_Name.PCB_AXIS)
                {
                    nUseAxis = i;
                    sMotorName = PCB_MOTOR_NAME[nUseAxis];
                    nMotorType = PCB_MOTOR_TYPE[i];
                    nMaxSpeed = PCB_MOTOR_MAX_SPEED[i];

                    //dResol = Globalo.dataManage.teachingData.PcbMotorData.dMotorResol[i];
                }
                else if (nUnit == (int)eUnit_Name.LENS_AXIS)
                {
                    nUseAxis = i + (int)ePcbMotor.MAX_PCB_MOTOR_COUNT;
                    sMotorName = LENS_MOTOR_NAME[i];
                    nMotorType = LENS_MOTOR_TYPE[i];
                    nMaxSpeed = LENS_MOTOR_MAX_SPEED[i];
                    //dResol = Globalo.dataManage.teachingData.LensMotorData.dMotorResol[i];
                }
                else
                {
                    eLogSender("CAxlMotion", "UNIT 설정 실패", Globalo.eMessageName.M_ERROR);
                    return -5;
                }

                //duRetCode = CAXM.AxmInterruptSetAxisEnable(nUseAxis, (uint)AXT_USE.DISABLE);
                //if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                //{
                //    eLogSender("CAxlMotion", $"AxmInterruptSetAxisEnable Fail");//, Globalo.eMessageName.M_ERROR);
                //}
                duRetCode = CAXM.AxmInfoIsInvalidAxisNo(nUseAxis);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 인식 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetMoveUnitPerPulse(nUseAxis, 1, 1);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 전자기어비 초기화 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetMinVel(nUseAxis, 1);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 최소 속도 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetAccelUnit(nUseAxis, (uint)AXT_MOTION_ACC_UNIT.SEC);//SEC UNIT_SEC2
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 가속도 단위 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetProfileMode(nUseAxis, (uint)AXT_MOTION_PROFILE_MODE.ASYM_S_CURVE_MODE);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    eLogSender("CAxlMotion", $"[{sMotorName}]모터 비대칭 S-Curve 가감속 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetAccelJerk(nUseAxis, 30);
                duRetCode = CAXM.AxmMotSetDecelJerk(nUseAxis, 30);

                double dAccelJerk = 0.0;
                double dDecelJerk = 0.0;
                uint dwHomLevel = 0;
                CAXM.AxmMotGetAccelJerk(nUseAxis, ref dAccelJerk);
                CAXM.AxmMotGetDecelJerk(nUseAxis, ref dDecelJerk);

                if (dAccelJerk != 30.0 || dDecelJerk != 30.0)
                {
                    eLogSender("CAxlMotion", $"[{sMotorName}]모터 지정축 가속 저크값 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmMotSetAbsRelMode(nUseAxis, (uint)AXT_MOTION_ABSREL.POS_ABS_MODE);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    eLogSender("CAxlMotion", $"[{sMotorName}]모터 절대 위치 이동 모드 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmHomeSetSignalLevel(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                duRetCode = CAXM.AxmHomeGetSignalLevel(nUseAxis, ref dwHomLevel);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    eLogSender("CAxlMotion", $"[{sMotorName}]모터 HOME LEVEL 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmMotSetMaxVel(nUseAxis, nMaxSpeed * dResol);

                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    eLogSender("CAxlMotion", $"[{sMotorName}]모터 최고 속도 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                switch (nMotorType)
                {
                    case MotorDefine.eMotorType.LINEAR:     //LINEAR
                        duRetCode = CAXM.AxmSignalSetInpos(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            eLogSender("CAxlMotion", $"[{sMotorName}]모터 Inposition 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmSignalSetStop(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.LOW);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            eLogSender("CAxlMotion", $"[{sMotorName}]모터 비상 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.HIGH, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            eLogSender("CAxlMotion", $"[{sMotorName}]모터 Limit 감지 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        
                        duRetCode = CAXM.AxmSignalSetServoAlarm(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            eLogSender("CAxlMotion", $"[{sMotorName}]모터 SERVER Alarm 감지 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        break;
                    //
                    //
                    //Steping
                    //
                    case MotorDefine.eMotorType.STEPING:
                        duRetCode = CAXM.AxmSignalSetInpos(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.UNUSED);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            eLogSender("CAxlMotion", $"[{sMotorName}]모터 Inposition 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmMotSetPulseOutMethod(nUseAxis, (uint)AXT_MOTION_PULSE_OUTPUT.TwoCcwCwHigh);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            eLogSender("CAxlMotion", $"[{sMotorName}]모터 CW/CCW 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmSignalSetStop(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //_stprintf_s
                        }
                        //pcb z만 high , high
                        if (nUseAxis == (int)ePcbMotor.PCB_Z)
                        {
                            duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.HIGH, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        }
                        else
                        {
                            duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.LOW, (uint)AXT_MOTION_LEVEL_MODE.LOW);
                        }
                        
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            eLogSender("CAxlMotion", $"[{sMotorName}]모터 비상 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmMotSetEncInputMethod(nUseAxis, (uint)AXT_MOTION_EXTERNAL_COUNTER_INPUT.ObverseSqr4Mode);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            eLogSender("CAxlMotion", $"[{sMotorName}]모터 4체배 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmSignalSetServoOnLevel(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            eLogSender("CAxlMotion", $"[{sMotorName}]모터 Servo On Level 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmSignalSetServoAlarm(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            eLogSender("CAxlMotion", $"[{sMotorName}]모터 SERVER Alarm 감지 설정 실패", Globalo.eMessageName.M_ERROR);
                        }

                        break;
                }//switch end
            }//for end
            return 1;
        }
        public bool GetMultiAxisPosCheck(MotorControl.eUnit nUnit, int nMultiAxisCnt, int[] Axis, int dPos)
        {
            int i = 0;
            double dCurPos = 0.0;
            double dTeachingPos = 0.0;
            bool bPosCheck = true;

            if (nUnit == MotorControl.eUnit.PCB_UNIT)
            {
                for (i = 0; i < nMultiAxisCnt; i++)
                {
                    dCurPos = PcbMotorAxis[Axis[i]].GetEncoderPos();
                    //dTeachingPos = Globalo.dataManage.teachingData.PcbTeachData[Axis[i]].dPos[dPos];

                    if (Math.Abs(dCurPos - dTeachingPos) > ENCORDER_GAP)
                    {
                        bPosCheck = false;
                    }
                }
            }
            else if (nUnit == MotorControl.eUnit.LENS_UNIT)
            {
                for (i = 0; i < nMultiAxisCnt; i++)
                {
                    dCurPos = LensMotorAxis[Axis[i]].GetEncoderPos();
                    //dTeachingPos = Globalo.dataManage.teachingData.LensTeachData[Axis[i]].dPos[dPos];

                    if (Math.Abs(dCurPos - dTeachingPos) > ENCORDER_GAP)
                    {
                        bPosCheck = false;
                    }
                }
            }
            else
            {
                eLogSender("CAxlMotion", "UNIT 설정 실패", Globalo.eMessageName.M_ERROR);
                return false;
            }

            
            return bPosCheck;
        }
        public bool GetAxisPosCheck(MotorControl.eUnit nUnit, int nAxis, int dPos)
        {
            double dCurPos = 0.0;
            double dTeachingPos = 0.0;
            int nUseAxis = nAxis;

            if (nUnit == MotorControl.eUnit.PCB_UNIT)
            {
                dCurPos = PcbMotorAxis[nAxis].GetEncoderPos();
                //dTeachingPos = Globalo.dataManage.teachingData.PcbTeachData[dPos].dPos[nAxis];
            }
            else if (nUnit == MotorControl.eUnit.LENS_UNIT)
            {
                dCurPos = LensMotorAxis[nAxis].GetEncoderPos();
                //dTeachingPos = Globalo.dataManage.teachingData.LensTeachData[dPos].dPos[nAxis];
            }
            else
            {
                eLogSender("CAxlMotion", "UNIT 설정 실패", Globalo.eMessageName.M_ERROR);
                return false;
            }
            

            if (Math.Abs(dCurPos - dTeachingPos) > ENCORDER_GAP)
            {
                return false;
            }

            return true;
        }
        
        public bool Motor_TxTy_MultiAxis_Move(MotorControl.eUnit nUnit, int nPos, double[] dOffsetPos)
        {

            return true;
        }
        public bool Motor_XY_MultiAxis_Move(MotorControl.eUnit nUnit, int nPos, double[] dOffsetPos)
        {

            return true;
        }
        public bool MoveAxisLimit(MotorControl.eUnit nUnit, int nAxis, double dVel, double dAcc, int nLimit, int nSignal, int nStopMode)
        {

            uint duRetCode = 0;
            int nUseAxis = nAxis;
            if (nUseAxis < 0)
            {
                return false;
            }

            if (nUnit == MotorControl.eUnit.PCB_UNIT)
            {
                nUseAxis = nAxis;
            }
            else if (nUnit == MotorControl.eUnit.LENS_UNIT)
            {
                nUseAxis = nAxis + (int)ePcbMotor.MAX_PCB_MOTOR_COUNT;
            }
            else
            {
                eLogSender("CAxlMotion", "UNIT 설정 실패", Globalo.eMessageName.M_ERROR);
                return false;
            }


            if (Math.Abs(dVel) > 100)
            {
                return false;
            }
            if (Math.Abs(dAcc) > 2)
            {
                return false;
            }
            if (nUnit == eUnit.PCB_UNIT)
            {

                dVel = dVel * PcbMotorAxis[nAxis].Resolution;
            }
            else if (nUnit == eUnit.LENS_UNIT)
            {
                dVel = dVel * LensMotorAxis[nAxis].Resolution;
            }
            else
            {
                return false;   
            }

            duRetCode = CAXM.AxmMoveSignalSearch(nUseAxis, dVel, dAcc, nLimit, nSignal, nStopMode);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {

                eLogSender("MotorControl", $"AxmMoveSignalSearch return error[Code:{duRetCode}]", Globalo.eMessageName.M_ERROR);

                return false;
            }
            return true;
        }
        public bool Pcb_Motor_XYT_Move(int nPos, double[] dOffsetPos)
        {
            int i = 0;
            int[] m_lMultiAxis = { -1, -1, -1 };     // 다축구동할 축 배열 갯수 선언 및 초기화
            double[] dMultiPos = { 0.0, 0.0, 0.0 };
            const int AxisCount = 3;
            
            
            string[] offsetName = { "OffsetX", "OffsetY", "OffsetTh" };
            for (i = 0; i < AxisCount; i++)
            {
                if (Math.Abs(dOffsetPos[i]) > 3.0)
                {
                    //_stprintf_s(szLog, SIZE_OF_1K, _T("PCB Y축 보정값 허용 범위[± %.03lf]를 초과 하였습니다. 보정값 :%.03lf"), g_clModelData[nUnit].m_dErrLimit[MOTOR_PCB_Y], dOffsetY);
                    eLogSender("MotorControl", $"{offsetName[i]} 보정값 { dOffsetPos[i].ToString("0.0##")} 허용 범위[± 3.0] 초과", Globalo.eMessageName.M_ERROR);
                    return false;
                }
                if (PcbMotorAxis[i].bOrgState == false)
                {
                    //_stprintf_s(szLog, SIZE_OF_1K, _T("[%s] 모터 원점 복귀가 되지 않았습니다."), MOTOR_NAME[nAxis[i]]);
                    eLogSender("MotorControl", $"{PCB_MOTOR_NAME[i]} 모터 원점 복귀가 되지 않았습니다.", Globalo.eMessageName.M_ERROR);
                    return false;
                }
                if (PcbMotorAxis[i].GetStopAxis() == false)
                {
                    //_stprintf_s(szLog, SIZE_OF_1K, _T("[%s] 모터 동작 중 이동 명령이 호출되었습니다.(MoveAxisMulti)"), MOTOR_NAME[nAxis[i]]);
                    eLogSender("MotorControl", $"{PCB_MOTOR_NAME[i]} 모터 동작 중입니다.", Globalo.eMessageName.M_ERROR);
                    return false;
                }
            }

            //Z축 대기 위치 인지 확인 추가
            if (GetAxisPosCheck(MotorControl.eUnit.PCB_UNIT, (int)ePcbMotor.PCB_Z, (int)Data.TeachingData.eTeachPosName.WAIT_POS) == false)
            {
                eLogSender("MotorControl", $"PCB Z축 대기위치가 아닙니다.", Globalo.eMessageName.M_ERROR);
                return false;
            }

            m_lMultiAxis[0] = (int)ePcbMotor.PCB_X;
            m_lMultiAxis[1] = (int)ePcbMotor.PCB_Y;
            m_lMultiAxis[2] = (int)ePcbMotor.PCB_TH;

            //dMultiPos[0] = Globalo.dataManage.teachingData.PcbTeachData[nPos].dPos[m_lMultiAxis[0]] + dOffsetPos[0];
            //dMultiPos[1] = Globalo.dataManage.teachingData.PcbTeachData[nPos].dPos[m_lMultiAxis[1]] + dOffsetPos[1];
            //dMultiPos[2] = Globalo.dataManage.teachingData.PcbTeachData[nPos].dPos[m_lMultiAxis[2]] + dOffsetPos[2];



            //bool bMoveBlock = false;
            //Globalo.motorControl.MoveAxisMultiAxes(MotorControl.eUnit.PCB_UNIT, AxisCount, m_lMultiAxis, dMultiPos, bMoveBlock);

            return true;
        }
        public bool Motor_Axis_Move(MotorControl.eUnit nUnit, MotorControl.ePcbMotor nAxis, int nPos, double dOffsetPos)
        {
            //int i = 0;
            //uint duRetCode = 0;
            //double dMultiPos = 0.0;
            
            int nUseAxis = (int)nAxis;
            if (nUseAxis < 0)
            {
                return false;
            }

            if (nUnit == MotorControl.eUnit.PCB_UNIT)
            {
                //dMultiPos = Globalo.dataManage.teachingData.PcbTeachData[nPos].dPos[nUseAxis] + dOffsetPos;
            }
            else if (nUnit == MotorControl.eUnit.LENS_UNIT)
            {
                //dMultiPos = Globalo.dataManage.teachingData.LensTeachData[nPos].dPos[nUseAxis] + dOffsetPos;
            }
            else
            {
                eLogSender("CAxlMotion", "UNIT 설정 실패", Globalo.eMessageName.M_ERROR);
                return false;
            }


            string offsetName = "Offset";

            if (Math.Abs(dOffsetPos) > 3.0)
            {
                eLogSender("MotorControl", $"{offsetName} 보정값 { dOffsetPos.ToString("0.0##")} 허용 범위[± 3.0] 초과", Globalo.eMessageName.M_ERROR);
                return false;
            }

            //bool bMoveBlock = false;
            //Globalo.motorControl.MoveAxis(nUnit, nUseAxis, AXT_MOTION_ABSREL.POS_ABS_MODE, dMultiPos, bMoveBlock);


            return true;
        }
        private bool MoveAxis(MotorControl.eUnit nUnit, int nAxis, AXT_MOTION_ABSREL nAbsFlag, double dPos, bool chkMoveBlock)
        {
            uint duRetCode = 0;
            int nUseAxis = nAxis;

            double dPosition1 = 0.0;
            double dVelocity = 0.0;
            double dResol = 0.0;
            double dAccel = 0.0;
            double dDecel = 0.0;
            double dCurrPos = 0.0;

            

            if (nUnit == MotorControl.eUnit.PCB_UNIT)
            {
                nUseAxis = nAxis;

                dCurrPos = PcbMotorAxis[nAxis].GetEncoderPos();
                dResol = PcbMotorAxis[nUseAxis].Resolution;
                dVelocity = PcbMotorAxis[nUseAxis].dVelocity * dResol;
                dAccel = PcbMotorAxis[nUseAxis].dAccel;
                dDecel = PcbMotorAxis[nUseAxis].dDecel;


            }
            else if (nUnit == MotorControl.eUnit.LENS_UNIT)
            {
                nUseAxis = nAxis + (int)ePcbMotor.MAX_PCB_MOTOR_COUNT;

                dCurrPos = LensMotorAxis[nAxis].GetEncoderPos();
                dResol = LensMotorAxis[nUseAxis].Resolution;
                dVelocity = LensMotorAxis[nAxis].dVelocity * dResol;
                dAccel = LensMotorAxis[nAxis].dAccel;
                dDecel = LensMotorAxis[nAxis].dDecel;
            }
            else
            {
                eLogSender("CAxlMotion", "UNIT 설정 실패", Globalo.eMessageName.M_ERROR);
                return false;
            }


            if (nAbsFlag == AXT_MOTION_ABSREL.POS_ABS_MODE)
            {
                //절대

                if (Math.Abs(dCurrPos - dPos) < 0.0001)
                {
                    return true;
                }
            }
            else
            {
                //현재위치 + dPos
                dPos += dCurrPos;
            }


            dPosition1 = dPos * dResol;

            //++ 지정한 축을 지정한 거리(또는 위치)/속도/가속도/감속도로 모션구동하고 모션 종료여부와 상관없이 함수를 빠져나옵니다.
            duRetCode = CAXM.AxmMoveStartPos(nAxis, dPosition1, dVelocity, dAccel, dDecel);


            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //MessageBox.Show(String.Format("AxmMoveStartPos return error[Code:{0:d}]", duRetCode));
                return false;
            }
            return true;
        }
        private bool MoveAxisMultiAxes(MotorControl.eUnit nUnit, int nMultiAxisCnt, int[] nAxis, double[] dPos, bool chkMoveBlock)
        {
            uint duRetCode = 0;
            int i = 0;

            double dCurrPos = 0.0;

            int[] m_lMoveMultiAxes = { -1, -1, -1, -1 };     // 다축구동할 축 배열 갯수 선언 및 초기화
            double[] dMultiPos = { 0.0, 0.0, 0.0, 0.0 };
            double[] dMultiVel = { 0.0, 0.0, 0.0, 0.0 };
            double[] dMultiAcc = { 0.0, 0.0, 0.0, 0.0 };
            double[] dMultiDec = { 0.0, 0.0, 0.0, 0.0 };


            
            for (i = 0; i < nMultiAxisCnt; i++)
            {
                m_lMoveMultiAxes[i] = nAxis[i];
                if (nUnit == MotorControl.eUnit.PCB_UNIT)
                {
                    dCurrPos = PcbMotorAxis[i].GetEncoderPos();
                    dMultiPos[i] = dPos[i] * PcbMotorAxis[i].Resolution;
                    dMultiVel[i] = PcbMotorAxis[i].dVelocity * PcbMotorAxis[i].Resolution;
                    dMultiAcc[i] = PcbMotorAxis[i].dAccel;
                    dMultiDec[i] = PcbMotorAxis[i].dDecel;
                }
                else if (nUnit == MotorControl.eUnit.LENS_UNIT)
                {
                    dCurrPos = LensMotorAxis[i].GetEncoderPos();
                    dMultiPos[i] = dPos[i] * LensMotorAxis[i].Resolution;
                    dMultiVel[i] = LensMotorAxis[i].dVelocity * LensMotorAxis[i].Resolution;
                    dMultiAcc[i] = LensMotorAxis[i].dAccel;
                    dMultiDec[i] = LensMotorAxis[i].dDecel;
                }
                else
                {
                    eLogSender("CAxlMotion", "UNIT 설정 실패", Globalo.eMessageName.M_ERROR);
                    return false;
                }
            }

            if (chkMoveBlock)
            {
                //++ 지정한 축들을 지정한 거리(또는 위치)/속도/가속도/감속도로 각각 모션구동하고
                //  모션이 종료되면 함수를 빠져나옵니다.

                duRetCode = CAXM.AxmMoveMultiPos(nMultiAxisCnt, m_lMoveMultiAxes, dMultiPos, dMultiVel, dMultiAcc, dMultiDec);

                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //MessageBox.Show(String.Format("AxmMoveMultiPos return error[Code:{0:d}]", duRetCode));
                    eLogSender("MotorControl",  $"AxmMoveMultiPos return error[Code:{ duRetCode}]", Globalo.eMessageName.M_ERROR);
                }
            }
            else
            {
                //++ 지정한 축들을 지정한 거리(또는 위치)/속도/가속도/감속도로 각각 모션구동하고
                //  모션 종료여부와 상관없이 함수를 빠져나옵니다.

                duRetCode = CAXM.AxmMoveStartMultiPos(nMultiAxisCnt, m_lMoveMultiAxes, dMultiPos, dMultiVel, dMultiAcc, dMultiDec);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //MessageBox.Show(String.Format("AxmMoveStartMultiPos return error[Code:{0:d}]", duRetCode));
                    eLogSender("MotorControl", $"AxmMoveStartMultiPos return error[Code:{duRetCode}]", Globalo.eMessageName.M_ERROR);
                    //$"[{m_lAxisCounts} / {(Data.TeachingData.MAX_MOTOR_COUNT)}] 모터 드라이브 개수 불일치");
                }
            }

            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }
            return true;
        }
        public bool GetStopAxis(int nUnit, int nAxis)
        {
            uint duLevel = 0;
            uint duRetCode = 0;
            int nUseAxis = 0;
            bool bRetVal = true;

            nUseAxis = nAxis;

            duRetCode = CAXM.AxmStatusReadInMotion(nUseAxis, ref duLevel);
            if (duLevel != (uint)AXT_MOTION_SIGNAL_LEVEL.ACTIVE)
            {
                bRetVal = false;
            }

            return bRetVal;
        }
        public bool GetStopMultiAxis(MotorControl.eUnit nUnit, int nMultiAxisCnt, int[] nAxis)
        {
            int i = 0;
            bool bStopRtn = true;
            if (nUnit == MotorControl.eUnit.PCB_UNIT)
            {
                for (i = 0; i < nMultiAxisCnt; i++)
                {
                    if (PcbMotorAxis[nAxis[i]].GetStopAxis() == false)
                    {
                        bStopRtn = false;
                    }
                }
            }
            else if (nUnit == MotorControl.eUnit.LENS_UNIT)
            {
                for (i = 0; i < nMultiAxisCnt; i++)
                {
                    if (LensMotorAxis[nAxis[i]].GetStopAxis() == false)
                    {
                        bStopRtn = false;
                    }
                }
            }
            else
            {
                eLogSender("CAxlMotion", "UNIT 설정 실패", Globalo.eMessageName.M_ERROR);
                return false;
            }


            
            return bStopRtn;
        }
        public bool StopAxis(MotorControl.eUnit nUnit, int nAxis)
        {
            //uint duLevel = 0;
            //uint duRetCode = 0;
            int nUseAxis = 0;

            if (nUnit == MotorControl.eUnit.PCB_UNIT)
            {
                nUseAxis = nAxis;
            }
            else if (nUnit == MotorControl.eUnit.LENS_UNIT)
            {
                nUseAxis = nAxis + (int)ePcbMotor.MAX_PCB_MOTOR_COUNT;
            }
            else
            {
                eLogSender("CAxlMotion", "UNIT 설정 실패", Globalo.eMessageName.M_ERROR);
                return false;
            }

            CAXM.AxmMoveEStop(nUseAxis);

            return true;
        }
        public bool StopAxisAll(int nUnit)
        {
            int i = 0;
            int nUseAxis = 0;
            //uint duLevel = (uint)AXT_USE.ENABLE;
            //uint duRetCode = 0;
            //bool bRetVal = true;

            for (i = 0; i < MAX_MOTOR_COUNT; i++)
            {
                nUseAxis = i;
                CAXM.AxmMoveEStop(nUseAxis);
            }
            return true;
        }

        
        
        public void Axl_Close()
        {
            int i;
            // 모든 모터를 정지한다.
            for (i = 0; i < PCB_UNIT_COUNT; i++)
            {
                CAXM.AxmMoveSStop(PcbMotorAxis[i].AxisNo);
            }
            for (i = 0; i < LENS_UNIT_COUNT; i++)
            {
                CAXM.AxmMoveSStop(LensMotorAxis[i].AxisNo);
            }

            //TCHAR* pszPath = NULL;
            //pszPath = m_sMotionSettingFilePath.GetBuffer(m_sMotionSettingFilePath.GetLength() + 1);
            ////! 모든 축의 현재 설정값을 파일에 저장 한다. 
            //AxmMotSaveParaAll(CT2A(pszPath));
            //m_sMotionSettingFilePath.ReleaseBuffer();

            CAXL.AxlClose();
        }
        
        public bool AmpDisableAll()
        {
            int i = 0;
            int AxisIndex = 0;
            double dDecel = 0.0;
            for (i = 0; i < PCB_UNIT_COUNT; i++)
            {
                PcbMotorAxis[i].bOrgState = false;
                AxisIndex = PcbMotorAxis[i].AxisNo;
                CAXM.AxmMoveStop(AxisIndex, dDecel);

                CAXM.AxmSignalServoOn(AxisIndex, (uint)AXT_USE.DISABLE);

                if (PCB_MOTOR_TYPE[AxisIndex] == MotorDefine.eMotorType.LINEAR)
                {
                    CAXM.AxmSignalServoAlarmReset(AxisIndex, (uint)AXT_USE.ENABLE);
                }
            }

            for (i = 0; i < LENS_UNIT_COUNT; i++)
            {
                LensMotorAxis[i].bOrgState = false;
                AxisIndex = LensMotorAxis[i].AxisNo;
                CAXM.AxmMoveStop(AxisIndex, dDecel);

                CAXM.AxmSignalServoOn(AxisIndex, (uint)AXT_USE.DISABLE);

                if (LENS_MOTOR_TYPE[i] == MotorDefine.eMotorType.LINEAR)
                {
                    CAXM.AxmSignalServoAlarmReset(AxisIndex, (uint)AXT_USE.ENABLE);
                }
            }

            Thread.Sleep(500);

            for (i = 0; i < PCB_UNIT_COUNT; i++)
            {
                AxisIndex = PcbMotorAxis[i].AxisNo;
                if (PCB_MOTOR_TYPE[AxisIndex] == MotorDefine.eMotorType.LINEAR)
                {
                    CAXM.AxmSignalServoAlarmReset(AxisIndex, (uint)AXT_USE.DISABLE);
                }
            }
            for (i = 0; i < LENS_UNIT_COUNT; i++)
            {
                AxisIndex = LensMotorAxis[i].AxisNo;
                if (LENS_MOTOR_TYPE[i] == MotorDefine.eMotorType.LINEAR)
                {
                    CAXM.AxmSignalServoAlarmReset(AxisIndex, (uint)AXT_USE.DISABLE);
                }
            }
            return true;
        }

        // ++ =======================================================================
        // >> TranslateHomeResult(...) : 지정한 원점검색 결과에 해당하는 문자열을 반환
        //    하는 함수.
        //  - "AXHS.h"에 정의되어있는 AXT_MOTION_HOME_RESULT 구조체를 기반으로 합니다.
        // --------------------------------------------------------------------------
        public String TranslateHomeResult(uint duHomeResult)
        {
            string m_strResult = "";
            switch (duHomeResult)
            {
                case (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS: m_strResult = ("[01H] HOME_SUCCESS"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_SEARCHING: m_strResult = ("([02H] HOME_SEARCHING"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_GNT_RANGE: m_strResult = ("[10H] HOME_ERR_GNT_RANGE"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_USER_BREAK: m_strResult = ("[11H] HOME_ERR_USER_BREAK"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_VELOCITY: m_strResult = ("[12H] HOME_ERR_VELOCITY"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_AMP_FAULT: m_strResult = ("[13H] HOME_ERR_AMP_FAULT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_NEG_LIMIT: m_strResult = ("[14H] HOME_ERR_NEG_LIMIT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_POS_LIMIT: m_strResult = ("[15H] HOME_ERR_POS_LIMIT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_NOT_DETECT: m_strResult = ("[16H] HOME_ERR_NOT_DETECT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_UNKNOWN: m_strResult = ("[FFH] HOME_ERR_UNKNOWN"); break;
            }
            return m_strResult;
        }
    }
    public delegate void delLogSender(object oSender, string strLog, Globalo.eMessageName bPopUpView = Globalo.eMessageName.M_NULL);    //선언 로그 출력
}
