using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Process
{
    public class LiftFlow
    {
        private readonly SynchronizationContext _syncContext;
        public int nTimeTick = 0;
        public int[] SensorSet = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] OrgOnGoing = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        string[] trayName = { "LEFT", "RIGHT"};
        public LiftFlow()
        {
            _syncContext = SynchronizationContext.Current;

        }
        #region [LIFT 원점 동작]
        public int HomeProcess(int nStep)                 //  원점(1000 ~ 2000)
        {
            uint duState = 0;
            bool m_bHomeProc = true;
            bool m_bHomeError = false;
            uint duRetCode = 0;
            string szLog = "";
            bool bRtn = false;
            bool bRtn2 = false;
            double dSpeed = 0.0;
            double dAcc = 0.3;
            int nRetStep = nStep;
            
            //LEFT SLIDE 정위치 체크
            //RIGHT SLIDE 정위치 체크

            switch (nStep)
            {
                case 1000:
                    nRetStep = 1020;
                    break;
                case 1020:
                    nRetStep = 1040;
                    break;
                case 1040:
                    nRetStep = 1060;
                    break;
                case 1060:
                    //LEFT, RIGHT LIFT 동시 하강 - LIMT MOVE - 확인은 나중
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].GetStopAxis() == false)
                    {
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].Stop();
                        break;
                    }
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].GetStopAxis() == false)
                    {
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].Stop();
                        break;
                    }
                    dSpeed = (15 * -1);      //-1은 왼쪽 이동

                    bRtn = Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.EMERGENCY_STOP);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LIFT_L_Z (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] LIFT_L_Z (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    bRtn = Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.EMERGENCY_STOP);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LIFT_R_Z (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] LIFT_R_Z (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);


                    nRetStep = 1080;
                    break;
                case 1080:
                    //LEFT, RIGHT GANTRY X  동시 이동 - LIMT MOVE - 확인은 나중
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_B_X].GetStopAxis() == false)
                    {
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_B_X].Stop();
                        break;
                    }
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_F_X].GetStopAxis() == false)
                    {
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_F_X].Stop();
                        break;
                    }

                    bRtn = Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_B_X].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.EMERGENCY_STOP);
                    bRtn2 = Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_F_X].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.EMERGENCY_STOP);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LIFT_B_X (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] LIFT_B_X (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    
                    if (bRtn2 == false)
                    {
                        szLog = $"[ORIGIN] LIFT_F_X (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] LIFT_F_X (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 1100;
                    break;
                case 1100:
                    nRetStep = 1120;
                    break;
                case 1120:
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] LIFT_L_Z (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1130;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] LIFT_L_Z (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 1130:
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] LIFT_R_Z (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1140;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] LIFT_R_Z (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 1140:
                    nRetStep = 1160;
                    break;
                case 1160:
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_B_X].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_B_X].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] LIFT_B_X (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1180;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] LIFT_B_X (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 1180:
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_F_X].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_F_X].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] LIFT_F_X (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1200;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] LIFT_F_X (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1200:
                    nRetStep = 1220;
                    break;
                case 1220:
                    nRetStep = 1240;
                    break;
                case 1240:
                    nRetStep = 1260;
                    break;
                case 1260:
                    if (ProgramState.ON_LINE_MOTOR == false)
                    {
                        for (int i = 0; i < Globalo.motionManager.liftMachine.MotorAxes.Length; i++)
                        {
                            Globalo.motionManager.liftMachine.MotorAxes[i].OrgState = true;
                        }

                        nRetStep = 1900;
                        break;
                    }
                    bRtn = true;
                    for (int i = 0; i < Globalo.motionManager.liftMachine.MotorAxes.Length; i++)
                    {
                        OrgOnGoing[i] = 0;
                        Globalo.motionManager.liftMachine.MotorAxes[i].OrgState = false;

                        //Home Method Setting
                        uint duZPhaseUse = 0;
                        double dHomeClrTime = 2000.0;
                        double dHomeOffset = 0.0;

                        //++ 지정한 축의 원점검색 방법을 변경합니다.
                        duRetCode = CAXM.AxmHomeSetMethod(
                            Globalo.motionManager.liftMachine.MotorAxes[i].m_lAxisNo,
                            (int)Globalo.motionManager.liftMachine.MotorAxes[i].HomeMoveDir,
                            (uint)Globalo.motionManager.liftMachine.MotorAxes[i].HomeDetect,
                            duZPhaseUse, dHomeClrTime, dHomeOffset);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.liftMachine.MotorAxes[i].Name} AxmHomeSetMethod Fail [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }

                        duRetCode = CAXM.AxmHomeSetVel(
                            Globalo.motionManager.liftMachine.MotorAxes[i].m_lAxisNo,
                            Globalo.motionManager.liftMachine.MotorAxes[i].FirstVel,
                            Globalo.motionManager.liftMachine.MotorAxes[i].SecondVel,
                            Globalo.motionManager.liftMachine.MotorAxes[i].ThirdVel,
                            50.0, 0.3, 0.3);//LastVel, Acc Firset, Acc Second


                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.liftMachine.MotorAxes[i].Name} AxmHomeSetVel Fail [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                    }

                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] 원점 설정 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("PcbProcess", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    nRetStep = 1270;
                    break;
                case 1270:
                    bRtn = true;
                    for (int i = 0; i < Globalo.motionManager.liftMachine.MotorAxes.Length; i++)
                    {
                        duRetCode = CAXM.AxmHomeSetStart(Globalo.motionManager.liftMachine.MotorAxes[i].m_lAxisNo);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.liftMachine.MotorAxes[i].Name} AxmHomeSetStart Fail [STEP : {nStep}]";
                            Globalo.LogPrint("PcbProcess", szLog);
                        }
                    }
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] 원점 시작 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("PcbProcess", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    nRetStep = 1280;
                    break;
                case 1280:
                    nRetStep = 1290;
                    break;
                case 1290:
                    m_bHomeProc = true;
                    m_bHomeError = false;
                    for (int i = 0; i < Globalo.motionManager.liftMachine.MotorAxes.Length; i++)
                    {
                        CAXM.AxmHomeGetResult(Globalo.motionManager.liftMachine.MotorAxes[i].m_lAxisNo, ref duState);
                        if (duState == (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS)
                        {
                            //원점 완료
                            Globalo.motionManager.liftMachine.MotorAxes[i].OrgState = true;
                        }
                        else if (duState == (uint)AXT_MOTION_HOME_RESULT.HOME_SEARCHING)
                        {
                            //검색중
                            m_bHomeProc = false;
                        }
                        else if (duState == (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_AMP_FAULT || duState == (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_NOT_DETECT ||
                            duState == (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_NEG_LIMIT || duState == (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_POS_LIMIT ||
                            duState == (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_UNKNOWN)
                        {
                            //fail
                            m_bHomeError = true;
                            szLog = $"[ORIGIN] {Globalo.motionManager.liftMachine.MotorAxes[i].Name} HOME 동작 ERROR [STEP : {nStep}]";
                            Globalo.LogPrint("PcbProcess", szLog);
                        }
                    }
                    if (m_bHomeError == true)
                    {
                        nRetStep *= -1;
                        break;
                    }
                    if (m_bHomeProc == true)
                    {
                        nRetStep = 1300;
                    }
                    break;
                case 1300:
                    nRetStep = 1400;
                    break;
                case 1400:

                    nRetStep = 1900;
                    break;

                case 1900:
                    Globalo.motionManager.liftMachine.RunState = OperationState.OriginDone;
                    szLog = $"[ORIGIN] LIFT UNIT 전체 원점 위치 이동 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 2000;
                    break;
                default:
                    //[ORIGIN] STEP ERR
                    nRetStep = -1;
                    break;
            }
            return nRetStep;
        }
        #endregion

        
        #region [LIFT 운전 준비]
        public int AutoReady(int nStep)					//  운전준비(2000 ~ 3000)
        {
            int i = 0;
            string szLog = "";
            bool bRtn = false;
            int nRetStep = nStep;

            //LEFT SLIDE 정위치 체크
            //RIGHT SLIDE 정위치 체크
            switch (nStep)
            {
                case 2000:
                    nRetStep = 2020;
                    break;
                case 2020:
                    nRetStep = 2040;
                    break;
                case 2040:
                    //GANTRY 에 TRAY 유무 확인
                    //TRAY 없으면 후진


                    //
                    //클램프 +  센터링 전후진

                    nRetStep = 2060;
                    break;
                case 2060:
                    //LEFT LIFT 하강
                    //RIGHT LIFT 하강

                    nRetStep = 2080;
                    break;
                case 2080:
                    //LEFT TRAY 안착 확인
                    //RIGHT TRAY 안착 확인

                    for (i = 0; i < 2; i++)
                    {
                        if (Globalo.motionManager.liftMachine.GetIsTrayOnSlide(i) == true)
                        {
                            Globalo.motionManager.liftMachine.IsOnTray[i] = true;
                            szLog = $"[READY] {trayName[i]} LIFT TRAY LOADED [STEP : {nStep}]";
                        }
                        else
                        {
                            Globalo.motionManager.liftMachine.IsOnTray[i] = false;
                            szLog = $"[READY] {trayName[i]} LIFT TRAY EMPTY [STEP : {nStep}]";
                        }
                        Globalo.LogPrint("ManualControl", szLog);
                    }

                    nRetStep = 2100;
                    break;
                case 2100:
                    if (Globalo.motionManager.liftMachine.GetIsTrayOnTop((int)Machine.eLift.LIFT_L_Z) == false)
                    {
                        //GANTRY  에 들고있는 TRAY 없으면 센터링 후진
                        if (Globalo.motionManager.liftMachine.GantryCenteringFor(false) == true)
                        {
                            szLog = $"[READY] GANTRY CENTRING BACK MOTION [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 2110;

                            nTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[READY] GANTRY CENTRING BACK MOTION FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                    }
                    else
                    {
                        szLog = $"[READY] GANTRY CENTRING BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2120;        //jump step
                    }
                    break;
                case 2110:
                    //GANTRY CENTRING 후진 상태 확인
                    if (Globalo.motionManager.liftMachine.GetGantryCenteringFor(false) == true)
                    {
                        szLog = $"[READY] GANTRY CENTRING BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2120;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] GANTRY CENTRING BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                        
                    break;
                case 2120:
                    //GANTRY 클램프 상태 확인
                    if (Globalo.motionManager.liftMachine.GetGantryClampFor(false) == true)
                    {
                        //Clamp 후진 상태 - 제품 없음
                        szLog = $"[READY] GANTRY CLAMP BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        if (Globalo.motionManager.liftMachine.IsOnTray[(int)Machine.eLift.LIFT_L_Z] == false)
                        {
                            szLog = $"[READY] PLEASE INSERT THE INPUT TRAY [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                    }
                    else if (Globalo.motionManager.liftMachine.GetGantryClampFor(true) == true)
                    {
                        //Clamp 후진 상태
                        szLog = $"[READY] GANTRY CLAMP FORWARD CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        if (Globalo.motionManager.liftMachine.GetIsTrayOnTop((int)Machine.eLift.LIFT_L_Z) == false)
                        {
                            //GANTRY CLAMP 전진 상태인데 , 제품이 없는 상태
                            szLog = $"[READY] TRAY NOT FOUND ON GANTRY [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }

                    }
                    else
                    {
                        //CLAMP 전진 , 후진 모두 감지 실패 상태
                        szLog = $"[READY] GANTRY CLAMP STATE SENSOR ERR [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2140:
                    nRetStep = 2160;
                    break;
                case 2160:
                    nRetStep = 2180;
                    break;
                case 2180:
                    nRetStep = 2190;
                    break;
                case 2200:
                    nRetStep = 2240;
                    break;
                case 2240:
                    nRetStep = 2260;
                    break;
                case 2260:
                    nRetStep = 2280;
                    break;
                case 2280:
                    nRetStep = 2300;
                    break;
                case 2300:
                    nRetStep = 2220;
                    break;
                case 2220:
                    break;
                case 2900:
                    Globalo.motionManager.transferMachine.RunState = OperationState.Standby;
                    szLog = $"[READY] TRANSFER 운전준비 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 3000;
                    break;
            }
            return nRetStep;
        }
        #endregion
    }
}
