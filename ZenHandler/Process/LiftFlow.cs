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
            if (Globalo.motionManager.liftMachine.GetTraySlidePos((int)Machine.eLift.LIFT_L_Z) == false)
            {
                szLog = $"[READY] LIFT_L_Z SLIDE SENSOR CHECK FAIL[STEP : {nStep}]";
                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                nRetStep *= -1;
                return -1;
            }
            if (Globalo.motionManager.liftMachine.GetTraySlidePos((int)Machine.eLift.LIFT_R_Z) == false)
            {
                szLog = $"[READY] LIFT_R_Z LIFT SLIDE SENSOR CHECK FAIL[STEP : {nStep}]";
                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                nRetStep *= -1;
                return -1;
            }
            switch (nStep)
            {
                case 2000:
                    nRetStep = 2020;
                    break;
                case 2020:
                    //1. 좌우 리프트 하강
                    //2. GANTRY 위 , 우측 푸셔위 TRAY 로드 상태 확인
                    //3. 우선 GANTRY 에 TRAY 로드
                    //4-1. PUSHER에 비어있으면 PUSH로 TRAY 이동
                    //4-2. PUSHER에 TRAY 있으면 PASS
                    //5-1. GANTRY - PUSHER 둘다 TRAY 로드 된 상태에서 시작 ?
                    //5-2. PUSHER 에만 TRAY 로드 된 상태에서 시작 ?

                    //

                    nRetStep = 2040;
                    break;
                case 2040:
                    nRetStep = 2060;
                    break;
                case 2060:

                    //LEFT LIFT 하강
                    //RIGHT LIFT 하강

                    double dSpeed = 10.0;
                    double dAcc = 0.3;
                    Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);
                    Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);

                    nRetStep = 2080;
                    break;
                case 2080:
                    //LEFT TRAY 안착 확인
                    //RIGHT TRAY 안착 확인

                    for (i = 0; i < 2; i++)
                    {
                        if (Globalo.motionManager.liftMachine.GetIsTrayOnSlide(i) == true)  //좌우 안착 TRAY 유무 확인
                        {
                            Globalo.motionManager.liftMachine.IsLiftOnTray[i] = true;
                            szLog = $"[READY] {trayName[i]} LIFT TRAY LOADED [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        else
                        {
                            Globalo.motionManager.liftMachine.IsLiftOnTray[i] = false;
                            szLog = $"[READY] {trayName[i]} LIFT TRAY EMPTY [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop(i) == true)  //좌우 안착 TRAY 유무 확인
                        {
                            Globalo.motionManager.liftMachine.IsTopLoadOnTray[i] = true;
                            szLog = $"[READY] {trayName[i]} TOP TRAY LOADED [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        else
                        {
                            Globalo.motionManager.liftMachine.IsTopLoadOnTray[i] = false;
                            szLog = $"[READY] {trayName[i]} TOP TRAY EMPTY [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                    }

                    nRetStep = 2100;
                    break;
                case 2100:
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)Machine.eLift.LIFT_L_Z) == false)        //Gantry에 Tray 없는지 확인
                    {
                        szLog = $"[READY] TRAY EMPTY ON GANTRY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

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
                        szLog = $"[READY] TRAY LOADED ON GANTRY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2110;        //jump step
                    }
                    break;
                case 2105:
                    //GANTRY CENTRING 후진 상태 확인
                    if (Globalo.motionManager.liftMachine.GetGantryCenteringFor(false) == true)
                    {
                        szLog = $"[READY] GANTRY CENTRING BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2110;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] GANTRY CENTRING BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;

                case 2110:
                    //PUsher에 Tray 없는지 확인
                    //
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)Machine.eLift.LIFT_R_Z) == false)        
                    {
                        szLog = $"[READY] TRAY EMPTY ON PUSHER [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);


                        //GANTRY  에 들고있는 TRAY 없으면 센터링 후진
                        if (Globalo.motionManager.liftMachine.PusherUp(false) == false)
                        {
                            szLog = $"[READY] PUSHER DOWN MOTION FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] PUSHER DOWN MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        if (Globalo.motionManager.liftMachine.PusherFor(false) == false)
                        {
                            szLog = $"[READY] PUSHER BACK MOTION FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] PUSHER BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        nRetStep = 2115;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else
                    {
                        szLog = $"[READY] TRAY LOADED ON PUSHER [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2115;        //jump step
                    }
                    break;

                case 2115:
                    //PUSHER 하강 / 후진 상태 확인
                    if (Globalo.motionManager.liftMachine.GetPUsherFor(false) == true)
                    {
                        szLog = $"[READY] PUSHER BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2116;
                        nTimeTick = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] PUSHER BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 2116:
                    //PUSHER  상태 확인
                    if (Globalo.motionManager.liftMachine.GetPUsherFor(true) == true && Globalo.motionManager.liftMachine.GetPUsherFor(false) == true)
                    {
                        //PUSHER 전진, 후진 모두 미감지
                        szLog = $"[READY] PUSHER FOR/BACK SENSOR ERR [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    if (Globalo.motionManager.liftMachine.GetPUsherUp(true) == true && Globalo.motionManager.liftMachine.GetPUsherUp(false) == true)
                    {
                        //PUSHER 상승, 하강 모두 미감지
                        szLog = $"[READY] PUSHER UP/DOWN SENSOR ERR [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    nRetStep = 2117;
                    break;
                case 2117:
                    //PUSHER 하강 / 후진 상태 확인
                    if (Globalo.motionManager.liftMachine.GetPUsherUp(false) == true)
                    {
                        szLog = $"[READY] PUSHER DOWN CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2120;
                        nTimeTick = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] PUSHER DOWN CHECK TIMEOUT[STEP : {nStep}]";
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

                        if (Globalo.motionManager.liftMachine.IsLiftOnTray[(int)Machine.eLift.LIFT_L_Z] == false)
                        {
                            //GANTRY 에 제품이 없는데, LIFT에도 TRAY가 없어 알람
                            szLog = $"[READY] PLEASE INSERT THE INPUT TRAY [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        nRetStep = 2140;
                    }
                    else if (Globalo.motionManager.liftMachine.GetGantryClampFor(true) == true)
                    {
                        //Clamp 전진 상태

                        szLog = $"[READY] GANTRY CLAMP FORWARD CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)Machine.eLift.LIFT_L_Z) == false)
                        {
                            //GANTRY CLAMP 전진 상태인데 , 제품이 없는 상태
                            szLog = $"[READY] TRAY NOT FOUND ON GANTRY [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        nRetStep = 2600;    //JUMP STEP - Gantry 로드 상태
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
                    //GANTRY 에 잡고있는 TRAY 없어서 로드하는 시퀀스
                    //PUSHER에 TRAY를 잡고있으면 운전준비에서는 PASS??? - 자동시작후 GANTRY에 로드 시작

                    Globalo.motionManager.liftMachine.GantryClampFor(false);
                    Globalo.motionManager.liftMachine.GantryCenteringFor(false);

                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)Machine.eLift.LIFT_R_Z) == true)  //PUSHER 위 TRAY 유무 확인
                    {
                        //푸셔에 tray 가 있어서 gantry에 공급할 필요 없음 - 자동때 공급해
                        nRetStep = 2600;    //jump Step
                    }
                    else
                    {
                        nRetStep = 2160;

                    }
                    
                    break;
                case 2160:
                    //CLAMP , CENTRING 후진 확인
                    nRetStep = 2180;
                    break;
                case 2180:
                    //GANTRY X 축 LEFT TRAY LOAD 위치로 이동
                    nRetStep = 2190;
                    break;
                case 2200:
                    //GANTRY X 축 LEFT TRAY LOAD 위치 이동 확인
                    nRetStep = 2240;
                    break;
                case 2240:
                    //LEFT Z 상단 터치 센서까지 상승하기
                    nRetStep = 2260;
                    break;
                case 2260:
                    //LEFT Z 상단 터치 센서 감지시 정지시키기
                    
                    nRetStep = 2280;
                    break;
                case 2280:
                    //모터 정지 상태 확인 , // (+) Limit 센서 확인
                    nRetStep = 2300;
                    break;
                case 2300:

                    //tray 유무 확인
                    nRetStep = 2320;
                    break;
                case 2320:
                    //CLAMP 전진
                    nRetStep = 2340;
                    break;
                case 2340:
                    //CLAMP 전진 확인
                    nRetStep = 2360;
                    break;
                case 2360:
                    //CENTRING 전진
                    nRetStep = 2380;
                    break;
                case 2380:
                    //CENTRING 전진 확인
                    nRetStep = 2400;
                    break;
                case 2400:
                    //딜레이
                    break;
                case 2420:
                    //리프트 - Limit 까지 하강
                    break;
                case 2440:
                    //리프트 - Limit 하강 확인
                    break;
                case 2460:
                    //gantry 에 tray 있는지 유무 확인
                    break;
                case 2480:
                    //리프트 하강 후 -Limit 감지 하는지 확인 , tray 가 많은 상황
                    break;
                case 2500:

                    break;
                case 2520:

                    break;
                case 2540:

                    break;
                case 2560:

                    break;
                case 2580:
                    nRetStep = 2600;
                    break;
                //---------------------------------------------------
                //  JUMP STEP
                //---------------------------------------------------
                case 2600:  //jump Step
                    //Gantry_X_Move
                    //GANTRY X1,2 축 RIGHT 투입 위치로 이동
                    //운전준비시에는 무조건 RIGHT LIFT위로 이동해서 투입
                    Globalo.motionManager.liftMachine.Gantry_X_Move(Machine.LiftMachine.eTeachingPosList.UNLOAD_POS);
                    nTimeTick = Environment.TickCount;
                    nRetStep = 2320;
                    break;

                case 2620:
                    //GANTRY X1,2 위치 RIGHT 투입 이동 확인
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_F_X].GetStopAxis() == true && 
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_F_X].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.ChkGantryXMotorPos(Machine.LiftMachine.eTeachingPosList.WAIT_POS))
                    {
                        szLog = $"[READY] RIGHT LOAD 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2160;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[READY] RIGHT LOAD 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2640:

                    break;
                case 2660:
                    
                    break;
                case 2680:
                    //RIGHT LIFT -LIMIT 위치인데, Middle 센서 감지하고 있으면 꽉 찬 상태라 배출 신호
                    if (Globalo.motionManager.liftMachine.GetMiddleWaitSensor((int)Machine.eLift.LIFT_R_Z) == true)
                    {
                        szLog = $"[READY] PLEASE REMOVE THE OUTPUT TRAY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    nRetStep = 2700;
                    break;
                case 2700:
                    //LEFT LIFT 위에 TRAY 가 없으면 하강하고, 알람 추가하기
                    if (Globalo.motionManager.liftMachine.IsLiftOnTray[(int)Machine.eLift.LIFT_L_Z] == false)
                    {
                        //LEFT LIFT 하강 후 , 일시정지 없이 알람 후 진행
                    }
                    else
                    {
                        //LEFT , RIGHT LIFT 중간 센서위치까지 상승
                    }
                    break;
                case 2720:

                    break;
                case 2740:

                    break;
                case 2760:

                    break;
                case 2780:

                    break;
                case 2800:

                    break;
                case 2900:
                    Globalo.motionManager.liftMachine.IsLoadingInputTray = false;         //투입 LIft 투입중 , 초기화
                    Globalo.motionManager.liftMachine.IsUnloadingOutputTray = false;      //배출 Lift 배출중 , 초기화

                    Globalo.motionManager.liftMachine.RunState = OperationState.Standby;
                    szLog = $"[READY] LIFT 운전준비 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 3000;
                    break;
            }
            return nRetStep;
        }
        #endregion
    }
}
