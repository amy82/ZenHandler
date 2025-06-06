using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Process
{
    public enum eTRAY : int
    {
        ON_GANTRY = 0, ON_PUSHER
    };

    public enum eSLIDE : int
    {
        ON_LEFT = 0, ON_RIGHT
    };

    public class LiftFlow
    {
        public CancellationTokenSource CancelTokenLift;
        public ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);  // true면 동작 가능
        public Task<int> LoadTrayTask;
        public Task<int> UnloadTrayTask;
        private readonly SynchronizationContext _syncContext;
        public int nTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯
        public int nLoadTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯
        public int nUnloadTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯

        public int[] SensorSet = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] OrgOnGoing = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private int waitLoadTray = 1;
        private int waitUnloadTray = 1;
        string[] trayName = { "LEFT", "RIGHT"};
        public LiftFlow()
        {
            _syncContext = SynchronizationContext.Current;
            CancelTokenLift = new CancellationTokenSource();
            LoadTrayTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
            UnloadTrayTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
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
            if (Globalo.motionManager.liftMachine.GetTraySlidePos((int)Machine.eLift.LIFT_L_Z) == false)
            {
                szLog = $"[READY] LEFT SLIDE SENSOR CHECK FAIL[STEP : {nStep}]";
                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                nRetStep *= -1;
                return -1;
            }
            if (Globalo.motionManager.liftMachine.GetTraySlidePos((int)Machine.eLift.LIFT_R_Z) == false)
            {
                szLog = $"[READY] RIGHT SLIDE SENSOR CHECK FAIL[STEP : {nStep}]";
                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                nRetStep *= -1;
                return -1;
            }
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
                    dSpeed = (15 * -1);      //-1은 왼쪽, 하강 이동

                    bRtn = Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LIFT_L_Z (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] LIFT_L_Z (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    bRtn = Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);
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
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_L].GetStopAxis() == false)
                    {
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_L].Stop();
                        break;
                    }
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_R].GetStopAxis() == false)
                    {
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_R].Stop();
                        break;
                    }

                    bRtn = Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_L].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);
                    bRtn2 = Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_R].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);
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
                    nTimeTick = Environment.TickCount;
                    nRetStep = 1120;
                    break;
                case 1120:
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] LIFT_L_Z (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1130;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
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
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                    {
                        szLog = $"[ORIGIN] LIFT_R_Z (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 1140:
                    nTimeTick = Environment.TickCount;
                    nRetStep = 1160;
                    break;
                case 1160:
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_L].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_L].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] LIFT_B_X (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1180;
                        nTimeTick = Environment.TickCount;
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
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_R].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_R].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] LIFT_F_X (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1200;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] LIFT_F_X (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("LIft", szLog);
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
                            Globalo.LogPrint("LIft", szLog);
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
                            Globalo.LogPrint("LIft", szLog);
                        }
                    }

                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] 원점 설정 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("LIft", szLog);
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
                            Globalo.LogPrint("LIft", szLog);
                        }
                    }
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] 원점 시작 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("LIft", szLog);
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
            if (Globalo.motionManager.liftMachine.GetTraySlidePos((int)eSLIDE.ON_LEFT) == false)
            {
                szLog = $"[READY] LIFT_L_Z SLIDE SENSOR CHECK FAIL[STEP : {nStep}]";
                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                nRetStep *= -1;
                return -1;
            }
            if (Globalo.motionManager.liftMachine.GetTraySlidePos((int)eSLIDE.ON_RIGHT) == false)
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
                    //PUSHER  상태 확인
                    if (Globalo.motionManager.liftMachine.GetPusherFor(true) == false && Globalo.motionManager.liftMachine.GetPusherFor(false) == false)
                    {
                        //PUSHER 전진, 후진 모두 미감지
                        szLog = $"[READY] PUSHER FOR/BACK SENSOR ERR [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (Globalo.motionManager.liftMachine.GetPusherUp(true) == false && Globalo.motionManager.liftMachine.GetPusherUp(false) == false)
                    {
                        //PUSHER 상승, 하강 모두 미감지
                        szLog = $"[READY] PUSHER UP/DOWN SENSOR ERR [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (Globalo.motionManager.liftMachine.GetPusherCentringFor(true) == false && Globalo.motionManager.liftMachine.GetPusherCentringFor(false) == false)
                    {
                        //PUSHER 상승, 하강 모두 미감지
                        szLog = $"[READY] PUSHER CENTRING FOR/BACK SENSOR ERR [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    //GANTRY CLAMP, CENTRING  상태 확인
                    if (Globalo.motionManager.liftMachine.GantryClampFor(true) == false && Globalo.motionManager.liftMachine.GantryClampFor(false) == false)
                    {
                        //PUSHER 전진, 후진 모두 미감지
                        szLog = $"[READY] CLAMP FOR/BACK SENSOR ERR [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (Globalo.motionManager.liftMachine.GetGantryCenteringFor(true) == false && Globalo.motionManager.liftMachine.GetGantryCenteringFor(false) == false)
                    {
                        //PUSHER 상승, 하강 모두 미감지
                        szLog = $"[READY] CENTRING FOR/BACK SENSOR ERR [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    nRetStep = 2050;
                    break;
                case 2050:
                    //2.
                    //LEFT TRAY 안착 확인
                    //RIGHT TRAY 안착 확인

                    for (i = 0; i < 2; i++)
                    {
                        if (Globalo.motionManager.liftMachine.GetIsTrayOnSlide(i) == true)  //좌우 SLIDE 위 TRAY 유무 확인
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
                        if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop(i) == true)  //좌우 TRAY 로드 유무 확인 (1.GANTRY 위 , 2. PUSHER 위)
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
                    nRetStep = 2060;
                    break;
                case 2060:
                    //1.
                    //---------------------------------------------------------------------------------------
                    //LEFT LIFT 하강
                    //RIGHT LIFT 하강
                    //---------------------------------------------------------------------------------------

                    bRtn = Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(Machine.eLift.LIFT_L_Z, Machine.eLiftSensor.LIFT_HOME_POS);
                    if(bRtn == false)
                    {
                        szLog = $"[READY] LEFT Z HOME MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    bRtn = Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(Machine.eLift.LIFT_R_Z, Machine.eLiftSensor.LIFT_HOME_POS);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] RIGHT Z HOME MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    nRetStep = 2080;
                    break;
                case 2080:

                    nRetStep = 2090;
                    break;
                case 2090:
                    //Gantry에 Tray 없는지 확인
                    
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_GANTRY) == true)
                    {
                        szLog = $"[READY] TRAY LOADED ON GANTRY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nTimeTick = Environment.TickCount;
                        nRetStep = 2120;        //jump step                 =======------------->
                    }
                    else
                    {
                        //GANTRY 에 들고있는 TRAY 없는 상황
                        szLog = $"[READY] TRAY EMPTY ON GANTRY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_PUSHER) == false)
                        {
                            //PUSHER 에 TRAY 없고, 투입할 TRAY도 없는 경우
                            if (Globalo.motionManager.liftMachine.GetIsTrayOnSlide((int)eSLIDE.ON_LEFT) == false)  //좌우 SLIDE 위 TRAY 유무 확인
                            {
                                //GANTRY 에 제품이 없는데, LIFT에도 TRAY가 없어 알람
                                //
                                szLog = $"[READY] PLEASE INSERT THE INPUT TRAY [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                                nRetStep *= -1;
                                break;
                            }
                        }
                        nRetStep = 2100;
                    }
                    break;
                case 2100:
                    //GANTRY CENTRING 후진 동작
                    //
                    if (Globalo.motionManager.liftMachine.GantryCenteringFor(false) == true)
                    {
                        szLog = $"[READY] GANTRY CENTRING BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2105;

                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[READY] GANTRY CENTRING BACK MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
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
                    //GANTRY CLAMP 후진 동작
                    //
                    if (Globalo.motionManager.liftMachine.GantryClampFor(false) == true)
                    {
                        szLog = $"[READY] GANTRY CLAMP BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2115;

                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[READY] GANTRY CLAMP BACK MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2115:
                    //GANTRY CLAMP 후진 상태 확인
                    //
                    if (Globalo.motionManager.liftMachine.GetGantryClampFor(false) == true)
                    {
                        szLog = $"[READY] GANTRY CLAMP BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2120;
                        nTimeTick = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] GANTRY CLAMP BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2120://jump step 

                    //PUsher에 Tray 있는지 확인
                    //
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_PUSHER) == false)
                    {
                        szLog = $"[READY] TRAY EMPTY ON PUSHER [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);



                        if (Globalo.motionManager.liftMachine.PusherCentringFor(false) == false)
                        {
                            szLog = $"[READY] PUSHER CENTRING BACK MOTION FAIL[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] PUSHER CENTRING BACK MOTION [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

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

                        nRetStep = 2125;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else
                    {
                        szLog = $"[READY] TRAY LOADED ON PUSHER [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2160;    //JUMP STEP
                    }
                    break;
                case 2125:
                    //PUSHER 하강 / 후진 상태 확인
                    if (Globalo.motionManager.liftMachine.GetPusherFor(false) == true)
                    {
                        szLog = $"[READY] PUSHER BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2130;
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
                case 2130:
                    //PUSHER 하강 / 후진 상태 확인
                    if (Globalo.motionManager.liftMachine.GetPUsherUp(false) == true)
                    {
                        szLog = $"[READY] PUSHER DOWN CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2135;
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
                case 2135:
                    if (Globalo.motionManager.liftMachine.GetPusherCentringFor(false) == true)
                    {
                        szLog = $"[READY] PUSHER CENTRING BACK CHECK [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2140;
                        nTimeTick = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] PUSHER CENTRING BACK CHECK TIMEOUT[STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2140:
                    //모터 리밋 확인

                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].GetNegaSensor() == true)
                    {
                        szLog = $"[READY] LEFT LIFT Z (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2145;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[READY] LEFT LIFT Z (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2145:
                    //모터 리밋 확인

                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].GetNegaSensor() == true)
                    {
                        szLog = $"[READY] RIGHT LIFT Z (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);


                        //배출쪽 Tray 가득차서 빼고 진행해야된다.
                        if (Globalo.motionManager.liftMachine.GetMiddleWaitSensor((int)Machine.eLift.LIFT_R_Z) == true)
                        {
                            szLog = $"[READY] PLEASE REMOVE THE OUTPUT TRAY [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }


                        nRetStep = 2150;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[READY] RIGHT LIFT Z (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2150:
                    //GANTRY X1,2 축 RIGHT 투입 위치로 이동
                    bRtn = Globalo.motionManager.liftMachine.Gantry_X_Move(Machine.LiftMachine.eTeachingPosList.RIGHT_LOAD_POS);

                    if (bRtn == false)
                    {
                        szLog = $"[READY] GANTRY X LOAD POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[READY] GANTRY X LOAD POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nTimeTick = Environment.TickCount;
                    nRetStep = 2155;

                    break;
                case 2155:
                    //GANTRY X1,2 위치 RIGHT 투입 이동 확인
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_L].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_R].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.ChkGantryXMotorPos(Machine.LiftMachine.eTeachingPosList.RIGHT_LOAD_POS))
                    {
                        szLog = $"[READY] RIGHT LOAD 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2160;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] RIGHT LOAD 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                
                case 2160:      //jump step
                    CancelTokenLift?.Dispose();
                    CancelTokenLift = new CancellationTokenSource();

                    nRetStep = 2170;
                    break;
                //---------------------------------------------------
                //  Pusher위 Tray 전부 완료된 상태인지 확인 후 배출
                //---------------------------------------------------
                case 2170:
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_PUSHER) == false || 
                        Globalo.motionManager.GetTrayEjectReq(MotionControl.MotorSet.TrayPos.Left) == false)
                    {
                        //Pusher 위에 Tray 없으면 배출 요청 초기화
                        
                        Globalo.motionManager.ClearTrayChange(MotionControl.MotorSet.TrayPos.Left);
                        //배출 요청 없음
                        waitUnloadTray = 0;
                        nRetStep = 2175;
                        break;
                    }

                    if (UnloadTrayTask == null || UnloadTrayTask.IsCompleted)
                    {
                        waitUnloadTray = 1;
                        UnloadTrayTask = Task.Run(() =>
                        {
                            waitUnloadTray = UnLoadTrayFlow();
                            Console.WriteLine($"-------------- UnloadTray Task - end {waitUnloadTray}");

                            return waitUnloadTray;
                        }, CancelTokenLift.Token);

                        nRetStep = 2175;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[READY] Complete Tray Unload Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                //---------------------------------------------------
                //  GANTRY 에 로딩된 Tray 없으면 Tray 공급 시퀀스
                //---------------------------------------------------
                case 2175:
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_GANTRY) == true ||
                        Globalo.motionManager.liftMachine.GetIsTrayOnSlide((int)eSLIDE.ON_LEFT) == false)
                    {
                        //Gantry 에 Tray 있음
                        waitLoadTray = 0;
                        nRetStep = 2180;
                        break;
                    }
                    
                    if (LoadTrayTask == null || LoadTrayTask.IsCompleted)
                    {
                        waitLoadTray = 1;
                        LoadTrayTask = Task.Run(() =>
                        {
                            waitLoadTray = GantryLoadTrayFlow();
                            Console.WriteLine($"-------------- LoadTray Task - end {waitLoadTray}");
                            
                            return waitLoadTray;
                        }, CancelTokenLift.Token);
                        nTimeTick = Environment.TickCount;
                        nRetStep = 2180;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[READY] Gantry Load Tray Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 2180:
                    if (waitUnloadTray == 1)
                    {
                        //Pusher위 tray 배출하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        szLog = $"[AUTO] TRAY UNLOAD TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (waitUnloadTray == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[READY] Unload Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitUnloadTray == 0)
                    {
                        //Tray 배출 완료
                        Console.WriteLine($"waitUnloadTray - {waitUnloadTray}");
                        nTimeTick = Environment.TickCount;
                        nRetStep = 2190;
                        break;
                    }
                    break;
                case 2190:
                    if (waitLoadTray == 1)
                    {
                        //Gantry 에 Tray 로드하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        szLog = $"[AUTO] TRAY LOAD ON GANTRY TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    if (waitLoadTray == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[READY] Gantry Load Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitLoadTray == 0)
                    {
                        //Gantry 에 Tray 로드 완료
                        Console.WriteLine($"waitLoadTray - {waitLoadTray}");
                        nRetStep = 2200;
                        break;
                    }
                    break;

                case 2200:
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_GANTRY) == true)
                    {
                        szLog = $"[READY] ON GANTRY TRAY LOAD COMPLETE  [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        
                    }
                    else
                    {
                        szLog = $"[READY] ON GANTRY TRAY EMPTY  [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);

                        //LEFT 투입할 TRAY도 없는 경우
                        if (Globalo.motionManager.liftMachine.GetIsTrayOnSlide((int)eSLIDE.ON_LEFT) == false)  //좌우 SLIDE 위 TRAY 유무 확인
                        {
                            //GANTRY 에 제품이 없는데, LIFT에도 TRAY가 없어 알람
                            //
                            //TODO: BUZZER
                            szLog = $"[READY] PLEASE INSERT THE INPUT TRAY [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);


                            if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_PUSHER) == false)
                            {
                                //GANTRY 에 제품이 없는데, LIFT에도 TRAY가 없어 알람
                                //
                                //TODO: BUZZER
                                szLog = $"[READY] PLEASE INSERT THE INPUT TRAY. [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                                nRetStep *= -1;
                                break;
                            }
                        }

                        
                    }

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
                    //---------------------------------------------------
                    //  GANTRY 에 TRAY LOAD OR EMPTY
                    //---------------------------------------------------

                    //---------------------------------------------------
                    //  PUSHER 에 TRAY가 있을수도있고 , 배출한 직 후 일수 있다.
                    //---------------------------------------------------

                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_PUSHER) == true)
                    {
                        Globalo.motionManager.transferMachine.TrayPosition = MotionControl.MotorSet.TrayPos.Right;      //Gantry에서 Pusher로 Tray 이동 완료
                        szLog = $"[READY] TRAY LOADED ON PUSHER [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2400;    //jump step ------>
                    }
                    else
                    {
                        szLog = $"[AUTO] Empty Tray On Pusher [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep = 2320;
                        break;
                    }
                    break;

                case 2320:
                    //---------------------------------------------------
                    // Gantry에서  PUSHER 로 TRAY 공급하는 시퀀스
                    //---------------------------------------------------
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_GANTRY) == false)
                    {
                        //PUSHER 위 , GANTRY 위 둘다 TRAY 없는 상황
                        szLog = $"[READY] TRAY EMPTY ON GANTRY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nTimeTick = Environment.TickCount;
                        nRetStep *= -1;
                        break;
                    }

                    nRetStep = 2330;
                    nTimeTick = Environment.TickCount;

                    break;
                case 2330:
                    if (LoadTrayTask == null || LoadTrayTask.IsCompleted)
                    {
                        waitLoadTray = 1;
                        LoadTrayTask = Task.Run(() =>
                        {
                            waitLoadTray = LoadTrayOnPusherFlow();

                            return waitLoadTray;
                        }, CancelTokenLift.Token);
                        nTimeTick = Environment.TickCount;
                        nRetStep = 2340;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[READY] OnPusher Load Tray Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 2340:
                    if (waitLoadTray == 1)
                    {
                        //Gantry 에 Tray 로드하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        szLog = $"[AUTO] TRAY LOAD ON PUSHER TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    if (waitLoadTray == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[READY] Puser Load Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitLoadTray == 0)
                    {
                        //Gantry 에 Tray 로드 완료
                        Console.WriteLine($"waitLoadTray - {waitLoadTray}");
                        nRetStep = 2360;
                        break;
                    }
                    break;

                case 2360:
                    //---------------------------------------------------
                    //  PUSHER 에 TRAY 로드 완료 상황
                    //---------------------------------------------------
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_PUSHER) == true)
                    {
                        Globalo.motionManager.transferMachine.TrayPosition = MotionControl.MotorSet.TrayPos.Right;      //Gantry에서 Pusher로 Tray 이동 완료
                        szLog = $"[READY] TRAY LOADED ON PUSHER [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2380;
                    }
                    else
                    {
                        szLog = $"[AUTO] Tray Load Fail On Pusher [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;

                case 2380:
                    nRetStep = 2400;
                    break;
                case 2400: //jump step

                    nRetStep = 2540;
                    break;
                case 2540:
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_GANTRY) == true)
                    {
                        //마지막 Tray 1판일때 운전 준비 돼야된다.
                        //GANTRY 에 TRAY가 있으면 푸셔에 공급하는 시퀀스
                        szLog = $"[READY] TRAY LOADED ON GANTRY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2600;
                        break;
                    }
                    else
                    {
                        //GANTRY 에 TRAY가 없어도 진행해야돼서 알람창만 띄우기
                        szLog = $"[READY] TRAY EMPTY ON GANTRY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        if (Globalo.motionManager.liftMachine.GetIsTrayOnSlide((int)eSLIDE.ON_LEFT) == true)
                        {
                            //GANTRY 에 TRAY 투입 시퀀스

                            nRetStep = 2560;
                        }
                        else
                        {
                            //PUSHER에만 TRAY 로드되면 진행가능
                            //투입할 TRAY도 없을때 진행 한건지 묻자.
                            szLog = $"[READY] TRAY EMPTY ON LEFT LIFT  [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);

                            nRetStep = 2600;        //jump step
                        }
                    }
                    break;

                //---------------------------------------------------
                //  GANTRY 에 TRAY 로드하는 시퀀스
                //---------------------------------------------------
                case 2560:
                    if (LoadTrayTask == null || LoadTrayTask.IsCompleted)
                    {
                        waitLoadTray = 1;
                        LoadTrayTask = Task.Run(() =>
                        {
                            waitLoadTray = GantryLoadTrayFlow();
                            Console.WriteLine($"-------------- LoadTray Task - end {waitLoadTray}");

                            return waitLoadTray;
                        }, CancelTokenLift.Token);

                        nRetStep = 2580;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[READY] Gantry Load Tray Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 2580:
                    if (waitLoadTray == 1)
                    {
                        //Gantry 에 Tray 로드 되는 동안 대기
                        break;
                    }
                    if (waitLoadTray == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[READY] Gantry Load Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitLoadTray == 0)
                    {
                        //Gantry 에 Tray 로드 완료
                        Console.WriteLine($"waitLoadTray - {waitLoadTray}");
                        nRetStep = 2590;
                        break;
                    }
                    break;
                case 2590:
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_GANTRY) == true)
                    {
                        szLog = $"[READY] ON GANTRY TRAY LOAD COMPLETE  [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep = 2600;
                    }
                    else
                    {
                        szLog = $"[READY] ON GANTRY TRAY LOAD FAIL  [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nTimeTick = Environment.TickCount;
                        nRetStep *= -1;
                        break;
                    }
                    break;

                case 2600:      //jump step
                    if (Globalo.motionManager.liftMachine.GetIsTrayOnSlide((int)eSLIDE.ON_LEFT) == false)
                    {
                        //PUSHER에만 TRAY 로드되면 진행가능
                        //투입할 TRAY도 없을때 진행 한건지 묻자.
                        szLog = $"[READY] TRAY EMPTY ON LEFT LIFT  [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_INFO);
                    }
                    nRetStep = 2620;
                    break;
                case 2620:
                    nRetStep = 2640;
                    break;
                case 2640:
                    nRetStep = 2660;
                    break;
                case 2660:
                    nRetStep = 2680;
                    break;
                case 2680:
                    //배출하는 시쿼스로 옮기자
                    //RIGHT LIFT -LIMIT 위치인데, Middle 센서 감지하고 있으면 꽉 찬 상태라 배출 신호
                    //if (Globalo.motionManager.liftMachine.GetMiddleWaitSensor((int)eSLIDE.ON_LEFT) == true)
                    //{
                    //    szLog = $"[READY] PLEASE REMOVE THE OUTPUT TRAY [STEP : {nStep}]";
                    //    Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                    //    nRetStep *= -1;
                    //    break;
                    //}
                    if (Globalo.motionManager.liftMachine.GetMiddleWaitSensor((int)eSLIDE.ON_RIGHT) == true)
                    {
                        szLog = $"[READY] PLEASE REMOVE THE INPUT TRAY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    nRetStep = 2700;
                    break;
                case 2700:
                    if (Globalo.motionManager.liftMachine.GetIsTrayOnSlide((int)eSLIDE.ON_LEFT) == true)    //Tray 없으면 올릴 필요 없음
                    {
                        bRtn = Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(Machine.eLift.LIFT_L_Z, Machine.eLiftSensor.LIFT_READY_POS);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] LEFT Z HOME MOVE FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                    }
                        
                    bRtn = Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(Machine.eLift.LIFT_R_Z, Machine.eLiftSensor.LIFT_READY_POS);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] RIGHT Z HOME MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    nTimeTick = Environment.TickCount;
                    nRetStep = 2720;
                    break;
                case 2720:
                    if (Globalo.motionManager.liftMachine.GetIsTrayOnSlide((int)eSLIDE.ON_LEFT) == false)
                    {
                        nRetStep = 2740;
                        break;
                    }

                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].GetStopAxis() == true &&
                    Globalo.motionManager.liftMachine.GetMiddleWaitSensor((int)eSLIDE.ON_LEFT) == true)
                    {
                        szLog = $"[READY] LEFT LIFT MID SENSOR 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2740;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] LEFT LIFT Z MID SENSOR 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 2740:
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].GetStopAxis() == true &&
                    Globalo.motionManager.liftMachine.GetMiddleWaitSensor((int)eSLIDE.ON_RIGHT) == true)
                    {
                        szLog = $"[READY] RIGHT LIFT MID SENSOR 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2760;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] RIGHT LIFT Z MID SENSOR 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2760:
                    nRetStep = 2780;
                    break;
                case 2780:
                    nRetStep = 2800;
                    break;
                case 2800:
                    nRetStep = 2900;
                    break;
                case 2900:
                    Globalo.motionManager.liftMachine.IsMovingTrayOnPusher = false;         //Pusher로 Tray 이동중 , 초기화
                    Globalo.motionManager.liftMachine.IsLoadingTrayOnGangry = false;         //투입 LIft 투입중 , 초기화
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

        #region [LIFT Auto_Waiting]

        public int Auto_Waiting(int nStep)
        {
            string szLog = "";
            bool result = false;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 3000:
                    //요청 대기
                    //1.GANTRY에 TRAY 공급
                    //2.GANTRY에서 우측 푸셔로 TRAY 공급 후 GANTRY 는 LEFT로 이동
                    //3.RIGHT 완료 TRAY 배출

                    //Gantry위에는 운전준비때 로드되거나
                    //자동중 버튼으로 로드 시키기
                    //외부 버튼 클릭시  ---->nRetStep = 3100;
                    if (Globalo.motionManager.liftMachine.ChkButtonLoadTray(true) == true && Globalo.motionManager.liftMachine.IsLoadingTrayOnGangry == false &&
                        Globalo.motionManager.liftMachine.GetTraySlidePos((int)eSLIDE.ON_LEFT) == true && 
                        Globalo.motionManager.liftMachine.GetIsTrayOnSlide((int)eSLIDE.ON_LEFT) == true &&
                        Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_GANTRY) == false)
                    {
                        Globalo.motionManager.liftMachine.IsLoadingTrayOnGangry = true;
                        CancelTokenLift?.Dispose();
                        CancelTokenLift = new CancellationTokenSource();
                        nRetStep = 3100;    //L Lift에서 Gantry로 Tray 공급
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    if (Globalo.motionManager.GetTrayEjectReq(MotionControl.MotorSet.TrayPos.Right) == true && 
                        Globalo.motionManager.liftMachine.GetTraySlidePos((int)eSLIDE.ON_RIGHT) == true)        //GetTrayEjectReq = 우측 배출할때만사용
                    {
                        Globalo.motionManager.liftMachine.IsUnloadingOutputTray = true;
                        CancelTokenLift?.Dispose();
                        CancelTokenLift = new CancellationTokenSource();
                        nRetStep = 3200;    //Push위 Tray R Lift로 배출하기
                        nTimeTick = Environment.TickCount;
                        break;
                    }

                    if (Globalo.motionManager.GetTrayEjectReq(MotionControl.MotorSet.TrayPos.Left) == true && Globalo.motionManager.liftMachine.IsMovingTrayOnPusher == false &&
                        Globalo.motionManager.liftMachine.GetTraySlidePos((int)eSLIDE.ON_LEFT) == true)        //Left는 Gantry에서 Pusher로 공급
                    {
                        Globalo.motionManager.liftMachine.IsMovingTrayOnPusher = true;
                        CancelTokenLift?.Dispose();
                        CancelTokenLift = new CancellationTokenSource();
                        nRetStep = 3300;    //Gantry에서 Push로 Tray 이동시키기
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    //투입 or 배출 대기 누르면 리프트 하강 ?
                    //투입 or 배출 완료 누르면 다시 상승 ?
                    if (Globalo.motionManager.liftMachine.ChkButtonUnloadTray(true) == true && 
                        Globalo.motionManager.liftMachine.IsUnloadingOutputTray == false &&
                        Globalo.motionManager.liftMachine.GetTraySlidePos((int)eSLIDE.ON_RIGHT) == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].GetNegaSensor() == false)
                    {
                        //배출 리프트 Home까지 하강
                        Globalo.motionManager.liftMachine.IsUnloadingOutputTray = true;
                        Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(Machine.eLift.LIFT_R_Z, Machine.eLiftSensor.LIFT_HOME_POS, true);
                        Globalo.motionManager.liftMachine.IsUnloadingOutputTray = false;
                    }
                    
                    break;
                case 3100:
                    //---------------------------------------------------
                    //  Gantry에 Tray 로드
                    //---------------------------------------------------
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_GANTRY) == true)
                    {
                        Globalo.motionManager.liftMachine.IsLoadingTrayOnGangry = false;
                        szLog = $"[AUTO] Tray Loaded On Gantry [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (LoadTrayTask == null || LoadTrayTask.IsCompleted)
                    {
                        waitLoadTray = 1;
                        LoadTrayTask = Task.Run(() =>
                        {
                            waitLoadTray = GantryLoadTrayFlow();
                            Console.WriteLine($"-------------- LoadTray Task - end {waitLoadTray}");

                            return waitLoadTray;
                        }, CancelTokenLift.Token);
                        nRetStep = 3120;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[AUTO] Gantry Load Tray Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 3120:
                    if (waitLoadTray == 1)
                    {
                        //Gantry 에 Tray 로드하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        Globalo.motionManager.liftMachine.IsLoadingTrayOnGangry = false;
                        szLog = $"[AUTO] TRAY LOAD ON GANTRY TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    if (waitLoadTray == -1)
                    {
                        Globalo.motionManager.liftMachine.IsLoadingTrayOnGangry = false;
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[AUTO] Gantry Load Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitLoadTray == 0)
                    {
                        Globalo.motionManager.liftMachine.IsLoadingTrayOnGangry = false;
                        //Gantry 에 Tray 로드 완료
                        Console.WriteLine($"waitLoadTray - {waitLoadTray}");
                        nRetStep = 3000;
                        break;
                    }
                    break;
                case 3200:
                    //---------------------------------------------------
                    //  PUSHER 위 Tray 배출
                    //---------------------------------------------------
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_PUSHER) == false)
                    {
                        Globalo.motionManager.liftMachine.IsUnloadingOutputTray = false;
                        Globalo.motionManager.ClearTrayChange(MotionControl.MotorSet.TrayPos.Right);
                        szLog = $"[AUTO] Empty Tray On Pusher [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (UnloadTrayTask == null || UnloadTrayTask.IsCompleted)
                    {
                        waitUnloadTray = 1;
                        UnloadTrayTask = Task.Run(() =>
                        {
                            waitUnloadTray = UnLoadTrayFlow();
                            Console.WriteLine($"-------------- UnloadTray Task - end {waitUnloadTray}");

                            return waitUnloadTray;
                        }, CancelTokenLift.Token);

                        nRetStep = 3220;
                    }
                    else
                    {
                        Globalo.motionManager.liftMachine.IsUnloadingOutputTray = false;
                        //일시정지
                        szLog = $"[AUTO] Complete Tray Unload Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 3220:
                    if (waitUnloadTray == 1)        //TODO: TIME OUT 필요
                    {
                        //Pusher위 tray 배출하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        Globalo.motionManager.liftMachine.IsUnloadingOutputTray = false;
                        szLog = $"[AUTO] UNLOAD TRAY TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (waitUnloadTray == -1)
                    {
                        Globalo.motionManager.liftMachine.IsUnloadingOutputTray = false;
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[AUTO] Unload Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitUnloadTray == 0)
                    {
                        Globalo.motionManager.liftMachine.IsUnloadingOutputTray = false;
                        //Tray 배출 완료
                        Console.WriteLine($"waitUnloadTray - {waitUnloadTray}");
                        nRetStep = 3000;
                        break;
                    }
                    break;
                case 3300:
                    //---------------------------------------------------
                    //  Gantry ---> Pusher로 Tray 이동
                    //---------------------------------------------------
                    if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_PUSHER) == true)
                    {
                        Globalo.motionManager.liftMachine.IsMovingTrayOnPusher = false;
                        szLog = $"[AUTO] Tray Loaded On Pusher [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (LoadTrayTask == null || LoadTrayTask.IsCompleted)
                    {
                        waitLoadTray = 1;
                        LoadTrayTask = Task.Run(() =>
                        {
                            waitLoadTray = LoadTrayOnPusherFlow();
                            Console.WriteLine($"-------------- LoadTray OnPusherFlow - end {waitLoadTray}");

                            return waitLoadTray;
                        }, CancelTokenLift.Token);
                        nRetStep = 3320;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[AUTO] Gantry LoadTray OnPusherFlow Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 3320:
                    if (waitLoadTray == 1)
                    {
                        //Gantry 에 Tray 로드하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        Globalo.motionManager.liftMachine.IsMovingTrayOnPusher = false;
                        szLog = $"[AUTO] TRAY LOAD ON PUSHER TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    if (waitLoadTray == -1)
                    {
                        Globalo.motionManager.liftMachine.IsMovingTrayOnPusher = false;
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[AUTO] Pusher Load Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitLoadTray == 0)
                    {
                        Globalo.motionManager.liftMachine.IsMovingTrayOnPusher = false;
                        //Gantry 에 Tray 로드 완료
                        Console.WriteLine($"waitLoadTray - {waitLoadTray}");
                        nRetStep = 3000;
                        break;
                    }
                    break;

            }
            return nRetStep;
        }
        #endregion

        #region [GANTRY 에 TRAY 로드]
        
        private int GantryLoadTrayFlow()
        {
            int nRtn = -1;

            bool bRtn = false;
            int nRetStep = 10;
            string szLog = "";
            while (true)
            {
                if (CancelTokenLift.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Gantry LoadTray Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                if(nRetStep != 100 && nRetStep != 120)      
                {
                    //리프트 z축이 상승하는 스텝 에서는 일시정지 걸어도 센서 확인하고 정지할대까지 일시정지안함
                    //정지하면 정지함수에서 리프트 정지 시킴
                    pauseEvent.Wait();  // 일시정지시 여기서 멈춰 있음
                }
                

                switch (nRetStep)
                {
                    case 10:
                        nRetStep = 20;
                        break;
                    case 20:
                        nRetStep = 40;
                        break;
                    case 40:
                        //GANTRY X 축 LEFT TRAY LOAD 위치로 이동
                        bRtn = Globalo.motionManager.liftMachine.Gantry_X_Move(Machine.LiftMachine.eTeachingPosList.TRAY_LOAD_LEFT_POS);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] GANTRY X LOAD POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }

                        szLog = $"[READY] GANTRY X LOAD POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        nLoadTimeTick = Environment.TickCount;
                        nRetStep = 60;
                        break;
                    case 60:
                        //GANTRY X 축 LEFT TRAY LOAD 위치 이동 확인
                        //GANTRY X1,2 위치 RIGHT 투입 이동 확인
                        if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_L].GetStopAxis() == true &&
                            Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_R].GetStopAxis() == true &&
                            Globalo.motionManager.liftMachine.ChkGantryXMotorPos(Machine.LiftMachine.eTeachingPosList.TRAY_LOAD_LEFT_POS))
                        {
                            szLog = $"[READY] GANTRY LEFT LOAD 위치 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 2240;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[READY] LEFT LOAD 위치 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 80:
                        nRetStep = 100;
                        break;
                    case 100:
                        //LEFT Z 상단 터치 센서까지 상승하기
                        bRtn = Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(Machine.eLift.LIFT_L_Z, Machine.eLiftSensor.LIFT_TOPSTOP_POS, true);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] LEFT LIFT Z UP MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] LEFT LIFT Z UP MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nLoadTimeTick = Environment.TickCount;

                        nRetStep = 120;
                        break;
                    case 120:
                        //LEFT Z 상단 터치 센서 감지시 정지시키기
                        if (Globalo.motionManager.liftMachine.GetTopTouchSensor((int)Machine.eLift.LIFT_L_Z) == true)
                        {
                            szLog = $"[READY] LEFT LIFT Z STOP [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);

                            Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].Stop(1);

                            nRetStep = 140;
                        }
                        else if (Environment.TickCount - nLoadTimeTick > MotionControl.MotorSet.LIFT_MOVE_TIMEOUT)
                        {
                            Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].Stop(1);
                            szLog = $"[READY] LEFT LIFT Z LOAD 위치 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 140:
                        //모터 정지 상태 확인 , // (+) Limit 센서 확인 (오버 확인)
                        if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_L].GetPosiSensor() == true)
                        {
                            //알람
                            szLog = $"[READY] LIFT_F_X (+) Limit Detect [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nRetStep = 160;
                        break;
                    case 160:
                        //tray 유무 확인
                        if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop(0) == true)  //좌우 안착 TRAY 유무 확인
                        {
                            Globalo.motionManager.liftMachine.IsTopLoadOnTray[0] = true;
                            szLog = $"[READY] LEFT TOP TRAY LOADED [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);

                            nRetStep = 180;
                        }
                        else
                        {
                            Globalo.motionManager.liftMachine.IsTopLoadOnTray[0] = false;
                            szLog = $"[READY] LEFT TOP TRAY EMPTY [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        break;
                    case 180:
                        //CLAMP 전진
                        if (Globalo.motionManager.liftMachine.GantryClampFor(true) == true)
                        {
                            szLog = $"[READY] GANTRY CLAMP FOR MOTION [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 200;

                            nLoadTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[READY] GANTRY CLAMP FOR MOTION FAIL[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 200:
                        //CLAMP 전진 확인
                        if (Globalo.motionManager.liftMachine.GetGantryClampFor(true) == true)
                        {
                            szLog = $"[READY] GANTRY CLAMP FOR CHECK [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 220;
                            nLoadTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nLoadTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] GANTRY CLAMP FOR CHECK TIMEOUT[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 220:
                        //GANTRY CENTRING 전진 동작
                        //
                        if (Globalo.motionManager.liftMachine.GantryCenteringFor(true) == true)
                        {
                            szLog = $"[READY] GANTRY CENTRING FOR MOTION [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 240;

                            nLoadTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[READY] GANTRY CENTRING FOR MOTION FAIL[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 240:
                        //GANTRY CENTRING 전진 상태 확인
                        if (Globalo.motionManager.liftMachine.GetGantryCenteringFor(true) == true)
                        {
                            szLog = $"[READY] GANTRY CENTRING FOR CHECK [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 260;
                        }
                        else if (Environment.TickCount - nLoadTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] GANTRY CENTRING FOR CHECK TIMEOUT[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 260:
                        //LEFT Z HOME까지 하강
                        bRtn = Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(Machine.eLift.LIFT_L_Z, Machine.eLiftSensor.LIFT_HOME_POS, true);

                        if (bRtn == false)
                        {
                            szLog = $"[READY] LEFT LIFT Z HOME MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] LEFT LIFT Z HOME MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 280;
                        break;
                    case 280:
                        //딜레이
                        nLoadTimeTick = Environment.TickCount;
                        nRetStep = 300;
                        break;
                    case 300:
                        //리프트 - Limit 까지 하강
                        if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_L_Z].GetNegaSensor() == true)
                        {
                            //알람
                            szLog = $"[READY] LEFT LIFT (-) Limit Detect [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 320;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick > MotionControl.MotorSet.LIFT_MOVE_TIMEOUT)
                        {
                            szLog = $"[READY] LEFT LIFT (-) Limit Detect TIMEOUT [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 320:
                        nRetStep = 900;
                        break;
                    case 900:
                        nRetStep = 1000;
                        break;
                    default:
                        break;
                }
                if (nRetStep < 0)
                {
                    Console.WriteLine("Gantry LoadTray Flow - fail");
                    break;
                }

                if (nRetStep == 1000)
                {
                    Console.WriteLine("Gantry LoadTray Flow - end");
                    break;
                }
                Thread.Sleep(10);       //TODO: while문안에서는 최소 10ms 꼭 필요
            }

            if (nRetStep == 1000)
            {
                nRtn = 0;
                Console.WriteLine("Gantry LoadTray Flow - ok");
            }
            else
            {
                nRtn = -1;
                Console.WriteLine("Gantry LoadTray Flow - ng");
            }
            return nRtn;
        }

        #endregion

        #region [PUSHER 에 TRAY 로드]

        private int LoadTrayOnPusherFlow()
        {
            int nRtn = -1;
            bool bRtn = false;
            int nRetStep = 10;
            string szLog = "";
            while (true)
            {
                if (CancelTokenLift.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("LoadTray OnPusherFlow cancelled!");
                    nRtn = -1;
                    break;
                }
                switch (nRetStep)
                {
                    case 10:
                        nRetStep = 20;
                        break;
                    case 20:
                        nRetStep = 40;
                        break;
                    case 40:
                        //GANTRY X 축 배출 위치 이동
                        bRtn = Globalo.motionManager.liftMachine.Gantry_X_Move(Machine.LiftMachine.eTeachingPosList.TRAY_LOAD_RIGHT_POS);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] GANTRY X UNLOAD POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }

                        szLog = $"[READY] GANTRY X UNLOAD POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        nTimeTick = Environment.TickCount;
                        nRetStep = 60;
                        break;
                    case 60:
                        //GANTRY X 축 배출 위치 이동 확인
                        //GANTRY X 축 RIGHT TRAY LOAD 위치 이동 확인
                        //GANTRY X1,2 위치 RIGHT 투입 이동 확인
                        if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_L].GetStopAxis() == true &&
                            Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_R].GetStopAxis() == true &&
                            Globalo.motionManager.liftMachine.ChkGantryXMotorPos(Machine.LiftMachine.eTeachingPosList.TRAY_LOAD_RIGHT_POS))
                        {
                            szLog = $"[READY] GANTRY LEFT UNLOAD 위치 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 80;
                            break;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[READY] LEFT UNLOAD 위치 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                       
                        break;
                    case 65:
                        //Gantry 센터링 후진
                        //GANTRY CENTRING 전진 동작
                        //
                        if (Globalo.motionManager.liftMachine.GantryCenteringFor(true) == true)
                        {
                            szLog = $"[READY] GANTRY CENTRING FOR MOTION [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 70;

                            nLoadTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[READY] GANTRY CENTRING FOR MOTION FAIL[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        break;
                    case 70:
                        //GANTRY CENTERING 후진 확인
                        if (Globalo.motionManager.liftMachine.GetGantryCenteringFor(false) == true)
                        {
                            szLog = $"[READY] GANTRY CENTRING BACK CHECK [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 80;

                            //TODO: 초기화 LOAD X,Y POS 
                            nTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] GANTRY CENTRING BACK CHECK TIMEOUT[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        break;
                    case 80:
                        //PUSHER 전진
                        if (Globalo.motionManager.liftMachine.PusherFor(true) == false)
                        {
                            szLog = $"[READY] PUSHER FOR MOTION FAIL[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] PUSHER FOR MOTION [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nTimeTick = Environment.TickCount;
                        nRetStep = 100;
                        break;
                    case 100:
                        //PUSHER 전진 확인
                        //PUSHER 하강 / 후진 상태 확인
                        if (Globalo.motionManager.liftMachine.GetPusherFor(true) == true)
                        {
                            szLog = $"[READY] PUSHER FOR CHECK [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 120;
                            nTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] PUSHER FOR CHECK TIMEOUT[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 120:
                        //PUSHER 상승
                        if (Globalo.motionManager.liftMachine.PusherUp(true) == false)
                        {
                            szLog = $"[READY] PUSHER UP MOTION FAIL[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] PUSHER UP MOTION [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nTimeTick = Environment.TickCount;
                        nRetStep = 140;
                        break;
                    case 140:
                        //PUSHER 상승 확인
                        if (Globalo.motionManager.liftMachine.GetPusherUp(true) == true)
                        {
                            szLog = $"[READY] PUSHER UP CHECK [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 160;
                            nTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] PUSHER UP CHECK TIMEOUT[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 160:
                        //GANTRY CLAMP 후진
                        if (Globalo.motionManager.liftMachine.GantryClampFor(false) == true)
                        {
                            szLog = $"[READY] GANTRY CLAMP BACK MOTION [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 180;

                            nTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[READY] GANTRY CLAMP BACK MOTION FAIL[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        break;
                    case 180:
                        //GANTRY CLAMP 후진 확인
                        if (Globalo.motionManager.liftMachine.GetGantryClampFor(false) == true)
                        {

                            Globalo.motionManager.transferMachine.TrayPosition = MotionControl.MotorSet.TrayPos.Right;      //Gantry에서 Pusher로 Tray 이동 완료

                            szLog = $"[READY] GANTRY CLAMP BACK CHECK [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 200;
                            nTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] GANTRY CLAMP BACK CHECK TIMEOUT[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 200:
                        //Pusher Centering 전진
                        if (Globalo.motionManager.liftMachine.PusherCentringFor(true) == false)
                        {
                            szLog = $"[READY] PUSHER CENTRING FOR MOTION FAIL[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] PUSHER CENTRING FOR MOTION [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 220;
                        break;
                    case 220:
                        
                        //Pusher Centering 전진 확인
                        if (Globalo.motionManager.liftMachine.GetPusherCentringFor(true) == true)
                        {
                            szLog = $"[READY] PUSHER CENTRING FOR CHECK [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 240;
                            nTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] PUSHER CENTRING FOR CHECK TIMEOUT[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        break;
                    case 240:
                        //딜레이?
                        if (Environment.TickCount - nTimeTick > 300)
                        {
                            nRetStep = 260;
                        }
                        
                        break;
                    case 260:
                        //Gantry_X_Move
                        //GANTRY X1,2 축 RIGHT 투입 위치로 이동
                        //운전준비시에는 무조건 RIGHT LIFT위로 이동해서 투입

                        bRtn = Globalo.motionManager.liftMachine.Gantry_X_Move(Machine.LiftMachine.eTeachingPosList.TRAY_LOAD_LEFT_POS);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] GANTRY X LOAD POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }

                        szLog = $"[READY] GANTRY X LOAD POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nTimeTick = Environment.TickCount;
                        nRetStep = 280;
                        break;
                    case 280:
                        //GANTRY X1,2 위치 RIGHT 투입 이동 확인
                        if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_L].GetStopAxis() == true &&
                            Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.GANTRYX_R].GetStopAxis() == true &&
                            Globalo.motionManager.liftMachine.ChkGantryXMotorPos(Machine.LiftMachine.eTeachingPosList.TRAY_LOAD_LEFT_POS))
                        {
                            szLog = $"[READY] RIGHT LOAD 위치 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 300;
                            break;
                        }
                        else if (Environment.TickCount - nTimeTick > 30000)
                        {
                            szLog = $"[READY] RIGHT LOAD 위치 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 300:
                        //TODO: pusher 위에 Tray 있는 확인필요
                        if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_PUSHER) == true)
                        {
                            szLog = $"[AUTO] Tray Loaded On Pusher [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep = 800;
                        }
                        else
                        {
                            szLog = $"[READY] Tray Load Fail Pusher [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 800:
                        nRetStep = 900;
                        break;
                    case 900:
                        nRetStep = 1000;
                        break;
                    default:
                        break;
                }

                if (nRetStep < 0)
                {
                    Console.WriteLine("LoadTray OnPusherFlow Flow - fail");
                    break;
                }

                if (nRetStep == 1000)
                {
                    Console.WriteLine("LoadTray OnPusherFlow Flow - end");
                    break;
                }
                Thread.Sleep(10);       //TODO: while문안에서는 최소 10ms 꼭 필요
            }
            if (nRetStep == 1000)
            {
                nRtn = 0;
                Console.WriteLine("LoadTray OnPusherFlow Flow - ok");
            }
            else
            {
                nRtn = -1;
                Console.WriteLine("LoadTray OnPusherFlow Flow - ng");
            }
            return nRtn;
        }
        #endregion

        #region [GRANTRY에서 PUSHER로 TRAY 이동]

        private int UnLoadTrayFlow()
        {
            int nRtn = -1;
            bool bRtn = false;
            string szLog = "";
            int nRetStep = 10;
            while (true)
            {
                if (CancelTokenLift.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("LiftChange cancelled!");
                    nRtn = -1;
                    break;
                }
                pauseEvent.Wait();  // 일시정지시 여기서 멈춰 있음
                switch (nRetStep)
                {
                    case 10:
                        nRetStep = 20;
                        break;
                    case 20:
                        nRetStep = 40;
                        break;
                    case 40:
                        //Pusher Centering 후진
                        if (Globalo.motionManager.liftMachine.PusherCentringFor(false) == false)
                        {
                            szLog = $"[READY] PUSHER CENTRING BACK MOTION FAIL[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] PUSHER CENTRING BACK MOTION [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        nTimeTick = Environment.TickCount;
                        nRetStep = 60;
                        break;
                    case 60:
                        //Pusher Centering 후진 확인
                        if (Globalo.motionManager.liftMachine.GetPusherCentringFor(false) == true)
                        {
                            szLog = $"[READY] PUSHER CENTRING BACK CHECK [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 80;
                            nTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] PUSHER CENTRING BACK CHECK TIMEOUT[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 80:
                        //RIGHT LIFT TOUCH 센서까지 상승
                        bRtn = Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(Machine.eLift.LIFT_R_Z, Machine.eLiftSensor.LIFT_TOPSTOP_POS, true);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] RIGHT LIFT Z UP MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] RIGHT LIFT Z UP MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nLoadTimeTick = Environment.TickCount;
                        nRetStep = 100;
                        break;
                    case 100:
                        //RIGHT LIFT TOUCH 센서 감지 확인
                        if (Globalo.motionManager.liftMachine.GetTopTouchSensor((int)Machine.eLift.LIFT_R_Z) == true)
                        {
                            szLog = $"[READY] RIGHT LIFT Z STOP [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);

                            Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].Stop(1);

                            nRetStep = 120;
                        }
                        else if (Environment.TickCount - nLoadTimeTick > MotionControl.MotorSet.LIFT_MOVE_TIMEOUT)
                        {
                            Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eLift.LIFT_R_Z].Stop(1);
                            szLog = $"[READY] RIGHT LIFT Z LOAD 위치 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }

                        
                        break;
                    case 120:
                        nRetStep = 140;
                        break;
                    case 140:
                        //Pusher 하강
                        if (Globalo.motionManager.liftMachine.PusherUp(false) == false)
                        {
                            szLog = $"[READY] PUSHER DOWN MOTION FAIL[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] PUSHER DOWN MOTION [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nTimeTick = Environment.TickCount;
                        nRetStep = 160;
                        break;
                    case 160:
                        //Pusher 하강 확인
                        if (Globalo.motionManager.liftMachine.GetPusherUp(false) == true)
                        {
                            szLog = $"[READY] PUSHER DOWN CHECK [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 180;
                            nTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] PUSHER DOWN CHECK TIMEOUT[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        break;
                    case 180:
                        //Pusher 후진
                        if (Globalo.motionManager.liftMachine.PusherFor(false) == false)
                        {
                            szLog = $"[READY] PUSHER BACK MOTION FAIL[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] PUSHER BACK MOTION [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nTimeTick = Environment.TickCount;
                        nRetStep = 200;
                        break;
                    case 200:
                        //Pusher 후진 확인
                        if (Globalo.motionManager.liftMachine.GetPusherFor(false) == true)
                        {
                            szLog = $"[READY] PUSHER BACK CHECK [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 220;
                            nTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] PUSHER BACK CHECK TIMEOUT[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        break;
                    case 220:
                        nRetStep = 240;
                        break;
                    case 240:
                        //리프트 ?? mm 하강
                        bRtn = Globalo.motionManager.liftMachine.LIft_Z_Height_Move(Machine.eLift.LIFT_R_Z, 70.0, true);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] RIGHT LIFT Z 70mm DOWN FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        szLog = $"[READY] RIGHT LIFT Z 70mm DOWN [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nLoadTimeTick = Environment.TickCount;
                        nRetStep = 260;
                        break;
                    case 260:
                        //리프트 ?? mm 하강 완료 - 내부에서 확인
                        nRetStep = 280;
                        break;
                    case 280:
                        //Pusher 위 tray 미감지 확인
                        if (Globalo.motionManager.liftMachine.GetIsLoadTrayOnTop((int)eTRAY.ON_PUSHER) == true)
                        {
                            Globalo.motionManager.ClearTrayChange(MotionControl.MotorSet.TrayPos.Right);
                            szLog = $"[AUTO] Tray Unload Fail On Pusher [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }

                        szLog = $"[AUTO] Empty Tray On Pusher [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 300;
                        break;
                    case 300:
                        //미들센서 감지하는지 확인
                        if (Globalo.motionManager.liftMachine.GetMiddleWaitSensor((int)Machine.eLift.LIFT_R_Z) == true)
                        {
                            //TODO: RIGHT LIFT 홈까지 다운후, 알람 후 팝업 선택하도록

                            bRtn = Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(Machine.eLift.LIFT_R_Z, Machine.eLiftSensor.LIFT_HOME_POS);
                            if (bRtn == false)
                            {
                                szLog = $"[READY] RIGHT Z HOME MOVE FAIL [STEP : {nRetStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                                nRetStep *= -1;
                                break;
                            }
                            szLog = $"[READY] PLEASE REMOVE THE OUTPUT TRAY [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);

                            DialogResult result = DialogResult.None;
                            _syncContext.Send(_ =>
                            {
                                result = Globalo.MessageAskPopup(szLog);
                            }, null);

                            if (result == DialogResult.Yes)
                            {
                                nRetStep = 300;
                                break;
                            }
                            nRetStep *= -1;
                            break;
                        }
                        else
                        {
                            szLog = $"[READY] TRAY UNLOAD COMPLETE [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 320;
                        }
                        break;
                    case 320:
                        nRetStep = 800;
                        break;
                    case 800:
                        nRetStep = 900;
                        break;
                    case 900:
                        nRetStep = 1000;
                        break;
                    default:
                        break;
                }

                if (nRetStep < 0)
                {
                    Console.WriteLine("UnLoad TrayFlow Flow - fail");
                    break;
                }

                if (nRetStep == 1000)
                {
                    Console.WriteLine("UnLoad TrayFlow Flow - end");
                    break;
                }
                Thread.Sleep(10);       //TODO: while문안에서는 최소 10ms 꼭 필요
            }
            if (nRetStep == 1000)
            {
                nRtn = 0;
                Console.WriteLine("UnLoad TrayFlow Flow - ok");
            }
            else
            {
                nRtn = -1;
                Console.WriteLine("UnLoad TrayFlow Flow - ng");
            }
            return nRtn;
        }


        #endregion
    }
}
