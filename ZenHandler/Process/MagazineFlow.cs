using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public enum eMag : int
    {
        ON_LEFT = 0, ON_RIGHT
    };
    public class MagazineFlow
    {
        public CancellationTokenSource CancelTokenMagazine;
        public ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);  // true면 동작 가능
        private readonly SynchronizationContext _syncContext;
        public Task<int> LeftMagazineTask;
        public Task<int> RightMagazineTask;

        public int nTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯
        public int[] nLoadTimeTick = { 0, 0 };           //<-----동시 동작일대 같이 쓰면 안될듯
        public int[] nUnloadTimeTick = { 0, 0 };           //<-----동시 동작일대 같이 쓰면 안될듯

        public int[] nMagazineTimeTick = { 0, 0 };           //<-----동시 동작일대 같이 쓰면 안될듯
        private int waitLeftMagazine = 1;
        private int waitRightMagazine = 1;


        private int[] MagazineLoadIndex = {-1, -1};
        private int[] MagazineUnloadIndex = {-1, -1};

        private bool IsLeftMagazine = true;
        private bool IsRightMagazine = true;
        public MagazineFlow()
        {
            _syncContext = SynchronizationContext.Current;
            CancelTokenMagazine = new CancellationTokenSource();
            LeftMagazineTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
            RightMagazineTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
        }
        #region [원점 동작]
        public int HomeProcess(int nStep)                 //  원점(1000 ~ 2000)
        {
            uint duState = 0;
            bool m_bHomeProc = true;
            bool m_bHomeError = false;
            uint duRetCode = 0;
            string szLog = "";
            bool bRtn = false;
            double dSpeed = 0.0;
            double dAcc = 0.3;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 1000:
                    //메거진 안착 상태 확인해야되나?
                    nRetStep = 1010;
                    break;

                case 1010:
                    nRetStep = 1020;
                    break;

                case 1020:
                    //#1 Y축 -Limit 위치이동
                    //#2 Y축 -Limit 위치이동

                    if (Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Y].GetStopAxis() == false)
                    {
                        Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Y].Stop();
                        break;
                    }
                    if (Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Y].GetStopAxis() == false)
                    {
                        Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Y].Stop();
                        break;
                    }

                    dSpeed = (15 * -1);      //-1은 왼쪽, 하강 이동

                    bRtn = Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Y].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_L_Y (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] MAGAZINE_L_Y (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    bRtn = Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Y].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_R_Y (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] MAGAZINE_R_Y (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nTimeTick = Environment.TickCount;
                    nRetStep = 1040;
                    break;
                case 1040:
                    //#1 Y축 -Limit 위치 이동 확인
                    //#2 Y축 -Limit 위치 이동 확인
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Y].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Y].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_L_Y (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1050;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_L_Y (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1050:
                    //#1 Y축 -Limit 위치 이동 확인
                    //#2 Y축 -Limit 위치 이동 확인
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Y].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Y].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_R_Y (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1060;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_R_Y (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1060:
                    //#1 메거진과 로더 사이에 tray 있으면 알람
                    //#2 메거진과 로더 사이에 tray 있으면 알람
                    if (Globalo.motionManager.magazineHandler.GetTrayUndocked(0) == true)
                    {
                        //PUSHER 상승, 하강 모두 미감지
                        szLog = $"[READY] LEFT MAGAZINE INTERLOCK ERR [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (Globalo.motionManager.magazineHandler.GetTrayUndocked(1) == true)
                    {
                        //PUSHER 상승, 하강 모두 미감지
                        szLog = $"[READY] RIGHT MAGAZINE INTERLOCK ERR [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    nRetStep = 1080;
                    break;
                case 1080:
                    //#1 z축 +Limit 위치이동
                    //#2 z축 +Limit 위치이동
                    if (Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Z].GetStopAxis() == false)
                    {
                        Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Z].Stop();
                        break;
                    }
                    if (Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Z].GetStopAxis() == false)
                    {
                        Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Z].Stop();
                        break;
                    }

                    dSpeed = (15 * 1);      //-1은 왼쪽, 하강 이동

                    bRtn = Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Z].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.PosEndLimit);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_L_Z (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] MAGAZINE_L_Z (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    bRtn = Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Z].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.PosEndLimit);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_R_Z (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] MAGAZINE_R_Z (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nTimeTick = Environment.TickCount;
                    nRetStep = 1100;
                    break;
                case 1100:
                    //#1 Y축 +Limit 위치 이동 확인
                    //#2 Y축 +Limit 위치 이동 확인
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Z].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Z].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_L_Z (+)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1120;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_L_Z (+)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                   
                    break;
                case 1120:
                    if (Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Z].GetStopAxis() == true &&
                        Globalo.motionManager.liftMachine.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Z].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_R_Z (+)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1140;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                    {
                        szLog = $"[ORIGIN] MAGAZINE_R_Z (+)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                   
                    break;
                case 1140:
                    nRetStep = 1160;
                    break;
                case 1160:
                    nRetStep = 1180;
                    break;
                case 1180:
                    nRetStep = 1200;
                    break;
                case 1200:
                    nRetStep = 1300;
                    break;
                case 1260:
                    if (ProgramState.ON_LINE_MOTOR == false)
                    {
                        for (int i = 0; i < Globalo.motionManager.magazineHandler.MotorAxes.Length; i++)
                        {
                            Globalo.motionManager.magazineHandler.MotorAxes[i].OrgState = true;
                        }

                        nRetStep = 1900;
                        break;
                    }
                    bRtn = true;
                    for (int i = 0; i < Globalo.motionManager.magazineHandler.MotorAxes.Length; i++)
                    {
                        //OrgOnGoing[i] = 0;
                        Globalo.motionManager.magazineHandler.MotorAxes[i].OrgState = false;

                        //Home Method Setting
                        uint duZPhaseUse = 0;
                        double dHomeClrTime = 2000.0;
                        double dHomeOffset = 0.0;

                        //++ 지정한 축의 원점검색 방법을 변경합니다.
                        duRetCode = CAXM.AxmHomeSetMethod(
                            Globalo.motionManager.magazineHandler.MotorAxes[i].m_lAxisNo,
                            (int)Globalo.motionManager.magazineHandler.MotorAxes[i].HomeMoveDir,
                            (uint)Globalo.motionManager.magazineHandler.MotorAxes[i].HomeDetect,
                            duZPhaseUse, dHomeClrTime, dHomeOffset);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.magazineHandler.MotorAxes[i].Name} AxmHomeSetMethod Fail [STEP : {nStep}]";
                            Globalo.LogPrint("LIft", szLog);
                        }

                        duRetCode = CAXM.AxmHomeSetVel(
                            Globalo.motionManager.magazineHandler.MotorAxes[i].m_lAxisNo,
                            Globalo.motionManager.magazineHandler.MotorAxes[i].FirstVel,
                            Globalo.motionManager.magazineHandler.MotorAxes[i].SecondVel,
                            Globalo.motionManager.magazineHandler.MotorAxes[i].ThirdVel,
                            50.0, 0.3, 0.3);//LastVel, Acc Firset, Acc Second


                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.magazineHandler.MotorAxes[i].Name} AxmHomeSetVel Fail [STEP : {nStep}]";
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
                    for (int i = 0; i < Globalo.motionManager.magazineHandler.MotorAxes.Length; i++)
                    {
                        duRetCode = CAXM.AxmHomeSetStart(Globalo.motionManager.magazineHandler.MotorAxes[i].m_lAxisNo);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.magazineHandler.MotorAxes[i].Name} AxmHomeSetStart Fail [STEP : {nStep}]";
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
                    for (int i = 0; i < Globalo.motionManager.magazineHandler.MotorAxes.Length; i++)
                    {
                        CAXM.AxmHomeGetResult(Globalo.motionManager.magazineHandler.MotorAxes[i].m_lAxisNo, ref duState);
                        if (duState == (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS)
                        {
                            //원점 완료
                            Globalo.motionManager.magazineHandler.MotorAxes[i].OrgState = true;
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
                            szLog = $"[ORIGIN] {Globalo.motionManager.magazineHandler.MotorAxes[i].Name} HOME 동작 ERROR [STEP : {nStep}]";
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
                    Globalo.motionManager.magazineHandler.RunState = OperationState.OriginDone;
                    szLog = $"[ORIGIN] MAGAZINE UNIT 전체 원점 위치 이동 완료 [STEP : {nStep}]";
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

        #region [운전 준비]
        public int AutoReady(int nStep)                 //  운전준비(2000 ~ 3000)
        {
            int i = 0;
            int LayerNum = 0;
            string szLog = "";
            bool bRtn = false;
            int nRetStep = nStep;

            switch (nStep)
            {
                case 2000:
                    //#1 Left Loader 에 Tray 없으면 로드 , 있으면 Tray 상태 확인(배출 or 진행)
                    //#2 Right Loader 에 Tray없으면 로드 , 있으면 Tray 상태 확인(배출 or 진행)

                    //#1 Left Loader 에 Tray있는지 유무 확인 ---> 없으면 로드하기
                    //#2 Right Loader 에 Tray있는지 유무 확인---> 없으면 로드하기

                    //#1 Left Loader Tray 완료 상태 확인 ---> 다했으면 배출
                    //#2 Right Loader Tray 완료 상태 확인---> 다했으면 배출
                    Globalo.motionManager.magazineHandler.IsTryChanging[0] = false;
                    Globalo.motionManager.magazineHandler.IsTryChanging[1] = false;
                    Globalo.motionManager.magazineHandler.isTrayReadyToLoad[0] = false;
                    Globalo.motionManager.magazineHandler.isTrayReadyToLoad[1] = false;


                    nRetStep = 2010;
                    break;
                case 2010:
                    //#1 TRAY 유무 확인
                    //#2 TRAY 유무 확인
                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == true)
                    {
                        szLog = $"[READY] Left Loader TRAY LOADED [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                    }
                    else
                    {
                        szLog = $"[READY] Left Loader TRAY EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                    }

                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == true)
                    {
                        szLog = $"[READY] Right Loader TRAY LOADED [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                    }
                    else
                    {
                        szLog = $"[READY] Right Loader TRAY EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                    }
                    nRetStep = 2015;
                    break;
                case 2015:
                    //
                    // 매거진 유무 안착 확인
                    //#1 Magazine 5단 전부 완료했는지 체크, 완료했으면 알람
                    //#2 Magazine 5단 전부 완료했는지 체크, 완료했으면 알람
                    //
                    if (Globalo.motionManager.magazineHandler.GetMagazineInPosition((int)eMag.ON_LEFT) == true)
                    {
                        szLog = $"[READY] Left Magazine LOADED [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        IsLeftMagazine = true;
                    }
                    else
                    {
                        szLog = $"[READY] Left Magazine EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        IsLeftMagazine = false;
                    }
                    //
                    //
                    if (Globalo.motionManager.magazineHandler.GetMagazineInPosition((int)eMag.ON_RIGHT) == true)
                    {
                        szLog = $"[READY] Right Magazine LOADED [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        IsRightMagazine = true;
                    }
                    else
                    {
                        szLog = $"[READY] Right Magazine EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        IsRightMagazine = false;
                    }

                    if (IsLeftMagazine == false && IsRightMagazine == false)        //둘다 없을 경우, 진행 불가
                    {
                        szLog = $"[READY] LEFT , RIGHT MAGAZINE EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer == -1 &&
                        Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer == -1)        //둘다 없을 경우, 진행 불가
                    {

                        szLog = $"[READY] LEFT, RIGHT MAGAZINE CHANGE [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    //

                    if (Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer == -1) 
                    {

                        szLog = $"[READY] PLEASE CHANGE THE LEFT MEGAZINE [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_INFO);
                        nRetStep *= -1;
                        break;
                    }
                    if (Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer == -1)
                    {

                        szLog = $"[READY] PLEASE CHANGE THE RIGHT MEGAZINE [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_INFO);
                        nRetStep *= -1;
                        break;
                    }
                    if (IsLeftMagazine == false)
                    {
                        //진행 불가
                        szLog = $"[READY] LEFT MAGAZINE EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_INFO);
                    }

                    if (IsRightMagazine == false)
                    {
                        //진행 불가
                        szLog = $"[READY] RIGHT MAGAZINE EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_INFO);
                    }
                    nRetStep = 2020;
                    break;
                case 2020:
                    MagazineLoadIndex[(int)eMag.ON_LEFT] = 0;
                    MagazineLoadIndex[(int)eMag.ON_RIGHT] = 0;
                    //Magazine Layer 번호랑 , 상태 After , Before 비교
                    // After 상태는 전체 검사 완료 제품 언로드 후 변경
                    if (Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer > -1)
                    {
                        LayerNum = Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer;
                        if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == true)
                        {
                            if (Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[LayerNum].State == Machine.LayerState.After)
                            {
                                //배출해라
                                MagazineLoadIndex[(int)eMag.ON_LEFT] = -1;
                            }
                            else
                            {
                                //진행
                            }
                        }
                        else
                        {
                            //무조건 로드
                            MagazineLoadIndex[(int)eMag.ON_LEFT] = 1;
                        }
                    }
                    if (Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer > -1)
                    {
                        LayerNum = Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer;
                        if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == true)
                        {
                            if (Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[LayerNum].State == Machine.LayerState.After)
                            {
                                //배출해라
                                MagazineLoadIndex[(int)eMag.ON_RIGHT] = -1;
                            }
                            else
                            {
                                //진행
                            }
                        }
                        else
                        {
                            //무조건 로드
                            MagazineLoadIndex[(int)eMag.ON_RIGHT] = 1;
                        }
                    }

                    nRetStep = 2040;
                    break;
                case 2040:
                    

                    nRetStep = 2060;

                    break;
                case 2060:
                    
                    nRetStep = 2080;
                    break;
                case 2080:

                    nRetStep = 2100;
                    break;

                case 2100:
                    
                    
                    nRetStep = 2120;
                    break;
                case 2120:
                    //#1 Left  Loader 배출 시퀀스 To Magazine
                    //
                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == false ||
                        MagazineLoadIndex[(int)eMag.ON_LEFT] != -1)
                    {
                        waitLeftMagazine = 0;
                        nRetStep = 2140;
                        break;
                    }
                    LayerNum = Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer;
                    if (LeftMagazineTask == null || LeftMagazineTask.IsCompleted)
                    {
                        waitLeftMagazine = 1;
                        LeftMagazineTask = Task.Run(() =>
                        {
                            waitLeftMagazine = MagazineTrayUnloadFlow((int)eMag.ON_LEFT, LayerNum);
                            Console.WriteLine($"-------------- MagazineTrayUnloadFlow Task - end {waitLeftMagazine}");

                            return waitLeftMagazine;
                        }, CancelTokenMagazine.Token);

                        nRetStep = 2140;
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
                case 2140:
                    //#2 Right Loader 배출 시퀀스 To Magazine
                    //
                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == false ||
                        MagazineLoadIndex[(int)eMag.ON_RIGHT] != -1)
                    {
                        waitRightMagazine = 0;
                        nRetStep = 2160;
                        break;
                    }
                    LayerNum = Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer;
                    if (RightMagazineTask == null || RightMagazineTask.IsCompleted)
                    {
                        waitRightMagazine = 1;
                        RightMagazineTask = Task.Run(() =>
                        {
                            waitRightMagazine = MagazineTrayUnloadFlow((int)eMag.ON_RIGHT, LayerNum);
                            Console.WriteLine($"-------------- UnloadTray Task - end {waitRightMagazine}");

                            return waitRightMagazine;
                        }, CancelTokenMagazine.Token);

                        nRetStep = 2160;
                        nTimeTick = Environment.TickCount;
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
                case 2160:
                    if (waitLeftMagazine == 1)
                    {
                        //Left tray 배출하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        szLog = $"[AUTO] LEFT TRAY UNLOAD TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    if (waitLeftMagazine == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[READY] LEFT Unload Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitLeftMagazine == 0)
                    {
                        //Tray 배출 완료
                        Console.WriteLine($"waitLeftMagazine - {waitLeftMagazine}");
                        nTimeTick = Environment.TickCount;
                        nRetStep = 2180;
                        break;
                    }
                    break;
                case 2180:
                    if (waitRightMagazine == 1)
                    {
                        //Right tray 배출하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        szLog = $"[AUTO] RIGHT TRAY UNLOAD TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (waitRightMagazine == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[READY] RIGHT Unload Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitRightMagazine == 0)
                    {
                        //Tray 배출 완료
                        Console.WriteLine($"waitRightMagazine - {waitRightMagazine}");
                        nTimeTick = Environment.TickCount;
                        nRetStep = 2200;
                        break;
                    }
                    break;
                case 2200:
                    MagazineLoadIndex[(int)eMag.ON_LEFT] = 0;
                    MagazineLoadIndex[(int)eMag.ON_RIGHT] = 0;
                    // #1 Left 배출 했으면 다시 로드해야돼서 Tray 로드 유무 체크
                    // #2 Right 배출 했으면 다시 로드해야돼서  Tray 로드 유무 체크

                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == true)
                    {
                        szLog = $"[READY] Left Loader TRAY LOADED [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                    }
                    else
                    {
                        szLog = $"[READY] Left Loader TRAY EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        MagazineLoadIndex[(int)eMag.ON_LEFT] = 1;
                    }

                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == true)
                    {
                        szLog = $"[READY] Right Loader TRAY LOADED [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                    }
                    else
                    {
                        szLog = $"[READY] Right Loader TRAY EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        MagazineLoadIndex[(int)eMag.ON_RIGHT] = 1;
                    }

                    nRetStep = 2220;
                    break;

                case 2220:
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
                    //#1 Left  Loader 로드 시퀀스 From Magazine

                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == true ||
                        MagazineLoadIndex[(int)eMag.ON_LEFT] != 1)
                    {
                        waitLeftMagazine = 0;
                        nRetStep = 2320;
                        break;
                    }
                    LayerNum = Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer;
                    if (RightMagazineTask == null || RightMagazineTask.IsCompleted)
                    {
                        waitLeftMagazine = 1;
                        RightMagazineTask = Task.Run(() =>
                        {
                            waitLeftMagazine = MagazineTrayLoadFlow((int)eMag.ON_LEFT, LayerNum);
                            Console.WriteLine($"-------------- UnloadTray Task - end {waitLeftMagazine}");

                            return waitLeftMagazine;
                        }, CancelTokenMagazine.Token);

                        nRetStep = 2320;
                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[READY] Complete Tray Load Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2320:
                    //#2 Right Loader 로드 시퀀스 From Magazine

                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == true ||
                        MagazineLoadIndex[(int)eMag.ON_RIGHT] != 1)
                    {
                        waitRightMagazine = 0;
                        nRetStep = 2340;
                        break;
                    }
                    LayerNum = Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer;
                    if (RightMagazineTask == null || RightMagazineTask.IsCompleted)
                    {
                        waitRightMagazine = 1;
                        RightMagazineTask = Task.Run(() =>
                        {
                            waitRightMagazine = MagazineTrayLoadFlow((int)eMag.ON_RIGHT, LayerNum);
                            Console.WriteLine($"-------------- LoadTray Task - end {waitRightMagazine}");

                            return waitRightMagazine;
                        }, CancelTokenMagazine.Token);

                        nRetStep = 2340;
                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[READY] Complete Tray Load Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2340:
                    if (waitLeftMagazine == 1)
                    {
                        //Left tray 로드하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        szLog = $"[AUTO] LEFT TRAY LOAD TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (waitLeftMagazine == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[READY] LEFT Load Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitLeftMagazine == 0)
                    {
                        //Tray 로드 완료
                        Console.WriteLine($"waitLeftMagazine - {waitLeftMagazine}");
                        nTimeTick = Environment.TickCount;
                        nRetStep = 2360;
                        break;
                    }
                    
                    break;
                case 2360:
                    if (waitRightMagazine == 1)
                    {
                        //Right tray 로드하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        szLog = $"[AUTO] RIGHT TRAY LOAD TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (waitRightMagazine == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[READY] RIGHT Load Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitRightMagazine == 0)
                    {
                        //Tray 배출 완료
                        Console.WriteLine($"waitRightMagazine - {waitRightMagazine}");
                        nTimeTick = Environment.TickCount;
                        nRetStep = 2380;
                        break;
                    }
                    
                    break;
                case 2380:
                    nRetStep = 2400;
                    break;
                case 2400:
                    nRetStep = 2500;
                    break;

                case 2500:
                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == false && 
                        Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == false)
                    {
                        //둘다 없으면 진행 xx
                        szLog = $"[READY] LEFT, RIGHT TRAY EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    nRetStep = 2600;
                    break;
                case 2600:
                    //Tray 로드 상태이면 아래 진행
                    //#1 Left Loader 로드 위치로 이동 , 가장 상단
                    //#2 Right Loader 로드 위치로 이동 , 가장 상단

                    bRtn = Globalo.motionManager.magazineHandler.Magazine_Y_Move(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, Machine.eMagazine.MAGAZINE_L_Y, false);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] LEFT Loader Y WAIT POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[READY] LEFT Loader Y WAIT POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    bRtn = Globalo.motionManager.magazineHandler.Magazine_Y_Move(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, Machine.eMagazine.MAGAZINE_R_Y, false);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] RIGHT Loader Y WAIT POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[READY] RIGHT Loader Y WAIT POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nTimeTick = Environment.TickCount;

                    nRetStep = 2620;
                    break;
                case 2620:
                    if (Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Y].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkYMotorPos(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, Machine.eMagazine.MAGAZINE_L_Y))
                    {
                        szLog = $"[READY] LEFT Loader Y WAIT POS 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2640;

                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] LEFT Loader Y WAIT POS 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    
                    break;
                case 2640:
                    if (Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Y].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkYMotorPos(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, Machine.eMagazine.MAGAZINE_R_Y))
                    {
                        szLog = $"[READY] RIGHT Loader Y WAIT POS 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2660;

                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] RIGHT Loader Y WAIT POS 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 2660:

                    nRetStep = 2680;
                    break;
                case 2680:
                    bRtn = Globalo.motionManager.magazineHandler.Magazine_Z_Move(Machine.MagazineMachine.eTeachingPosList.TRAY_LOAD_POS, Machine.eMagazine.MAGAZINE_L_Z, 0.0, false);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] LEFT Loader Z LOAD POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[READY] LEFT Loader Z LOAD POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    bRtn = Globalo.motionManager.magazineHandler.Magazine_Z_Move(Machine.MagazineMachine.eTeachingPosList.TRAY_LOAD_POS, Machine.eMagazine.MAGAZINE_R_Z, 0.0, false);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] RIGHT Loader Z LOAD POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[READY] RIGHT Loader Z LOAD POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nRetStep = 2700;
                    break;
                case 2700:
                    if (Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Z].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkZMotorPos(Machine.MagazineMachine.eTeachingPosList.TRAY_LOAD_POS, Machine.eMagazine.MAGAZINE_L_Z))
                    {
                        szLog = $"[READY] LEFT Loader Z LOAD POS 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2720;

                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] LEFT Loader Z LOAD POS 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2720:
                    if (Globalo.motionManager.magazineHandler.MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Z].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkZMotorPos(Machine.MagazineMachine.eTeachingPosList.TRAY_LOAD_POS, Machine.eMagazine.MAGAZINE_R_Z))
                    {
                        szLog = $"[READY] RIGHT Loader Z LOAD POS 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2740;

                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] RIGHT Loader Z LOAD POS 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2740:
                    nRetStep = 2760;
                    break;
                case 2760:
                    nRetStep = 2780;
                    break;
                case 2780:
                    nRetStep = 2800;
                    break;
                case 2800:
                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == true)
                    {
                        szLog = $"[READY] Left Loader TRAY LOADED [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        Globalo.motionManager.magazineHandler.isTrayReadyToLoad[0] = true;
                        //Left 로드 위치로 이동

                    }
                    else
                    {
                        Globalo.motionManager.magazineHandler.isTrayReadyToLoad[0] = false;
                        Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer = -1;
                        szLog = $"[READY] Left Loader TRAY EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);


                    }

                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == true)
                    {
                        szLog = $"[READY] Right Loader TRAY LOADED [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        
                        Globalo.motionManager.magazineHandler.isTrayReadyToLoad[1] = true;

                    }
                    else
                    {
                        Globalo.motionManager.magazineHandler.isTrayReadyToLoad[1] = false;
                        Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer = -1;
                        szLog = $"[READY] Right Loader TRAY EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                    }
                    nRetStep = 2900;
                    break;
                case 2900:
                    Globalo.motionManager.magazineHandler.IsTryChanging[0] = false;     //Auto Ready
                    Globalo.motionManager.magazineHandler.IsTryChanging[1] = false;     //Auto Ready


                    Globalo.motionManager.magazineHandler.RunState = OperationState.Standby;
                    szLog = $"[READY] MAGAZINE 운전준비 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 3000;
                    break;
            }
            return nRetStep;
        }
        #endregion

        #region [Auto_Waiting]
        public int Auto_Waiting(int nStep)
        {
            string szLog = "";
            int LayerNum = 0;
            bool result = false;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 3000:
                    //요청 대기
                    //1.Tray 로드 from Magazine
                    //2.Tray 배출 to Magazine
                    //
                    //
                    if (Globalo.motionManager.GetTrayEjectReq(MotionControl.MotorSet.TrayPos.Left) == true && Globalo.motionManager.magazineHandler.IsTryChanging[(int)eMag.ON_LEFT] == false)
                    {
                        //left 배출 후 로드 요청
                        nRetStep = 3100;
                        break;
                    }

                    if (Globalo.motionManager.GetTrayEjectReq(MotionControl.MotorSet.TrayPos.Right) == true && Globalo.motionManager.magazineHandler.IsTryChanging[(int)eMag.ON_RIGHT] == false)
                    {
                        //right 배출 후 로드 요청
                        nRetStep = 3200;
                        break;
                    }

                    if (Globalo.motionManager.magazineHandler.GetLeftCompleteModeButton() == true)      //외부 완료 버튼
                    {
                        if (Globalo.motionManager.magazineHandler.GetMagazineInPosition((int)eMag.ON_LEFT) == false)
                        {
                            Console.WriteLine("Left Magazine Empty");
                            break;
                        }
                        if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == true)
                        {
                            Console.WriteLine("Left Loaded Tray");
                            break;
                        }
                        if (Globalo.motionManager.magazineHandler.IsTryChanging[(int)eMag.ON_LEFT] == true &&
                        Globalo.motionManager.magazineHandler.isTrayReadyToLoad[(int)eMag.ON_LEFT] == false &&
                        Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer == -1)
                        {
                            //TODO: 작업자가 매거진 넣고 외부에서 버튼 누르면 로드하기
                            //비어있어서 로드해야된다.
                            Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer = 0;
                            nRetStep = 3150;
                            break;
                        }
                    }
                    if (Globalo.motionManager.magazineHandler.GetRightCompleteModeButton() == true)     //외부 완료 버튼
                    {
                        if (Globalo.motionManager.magazineHandler.GetMagazineInPosition((int)eMag.ON_RIGHT) == false)
                        {
                            Console.WriteLine("Right Magazine Empty");
                            break;
                        }
                        if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == true)
                        {
                            Console.WriteLine("Right Loaded Tray");
                            break;
                        }
                        if (Globalo.motionManager.magazineHandler.IsTryChanging[(int)eMag.ON_RIGHT] == true &&
                        Globalo.motionManager.magazineHandler.isTrayReadyToLoad[(int)eMag.ON_RIGHT] == false &&
                        Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer == -1)
                        {
                            //TODO: 작업자가 매거진 넣고 외부에서 버튼 누르면 로드하기
                            //비어있어서 로드해야된다.
                            Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer = 0;
                            nRetStep = 3250;
                            break;
                        }
                    }


                        
                    break;
                case 3100:
                    //---------------------------------------------------
                    //  LEFT LOADER 교체 - 배출
                    //---------------------------------------------------
                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == false)
                    {
                        waitLeftMagazine = 0;
                        nRetStep = 3140;        //go load
                        break;
                    }
                    LayerNum = Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer;
                    if (LeftMagazineTask == null || LeftMagazineTask.IsCompleted)
                    {
                        waitLeftMagazine = 1;
                        LeftMagazineTask = Task.Run(() =>
                        {
                            waitLeftMagazine = MagazineTrayUnloadFlow((int)eMag.ON_LEFT, LayerNum);
                            Console.WriteLine($"-------------- MagazineTrayUnloadFlow Task - end {waitLeftMagazine}");

                            return waitLeftMagazine;
                        }, CancelTokenMagazine.Token);

                        Globalo.motionManager.ClearTrayChange(MotionControl.MotorSet.TrayPos.Left);
                        nRetStep = 3120;
                        nTimeTick = Environment.TickCount;
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
                case 3120:
                    if (waitLeftMagazine == 1)
                    {
                        //Left tray 배출하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        szLog = $"[AUTO] LEFT TRAY UNLOAD TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (waitLeftMagazine == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[AUTO] LEFT Unload Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitLeftMagazine == 0)
                    {
                        //Tray 배출 완료
                        Console.WriteLine($"waitLeftMagazine - {waitLeftMagazine}");
                        nTimeTick = Environment.TickCount;
                        nRetStep = 3140;
                        break;
                    }
                    break;
                case 3140:
                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == false)
                    {
                        
                        if (Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer == -1)
                        {
                            //LEFT 매거진 교체 알람
                            szLog = $"[AUTO] LEFT MAGAZINE CHANGE [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_INFO);
                            nRetStep = 3000;
                            break;
                        }
                        else
                        {
                            //투입 진행
                            szLog = $"[AUTO] LEFT Tray Empty [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep = 3150;
                            break;
                        }
                    }
                    break;
                case 3150:
                    //---------------------------------------------------
                    //  LEFT LOADER - 투입
                    //---------------------------------------------------

                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == true)
                    {
                        waitLeftMagazine = 0;
                        nRetStep = 3000;        //load pass
                        break;
                    }
                    LayerNum = Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer;
                    if (LeftMagazineTask == null || LeftMagazineTask.IsCompleted)
                    {
                        waitLeftMagazine = 1;
                        LeftMagazineTask = Task.Run(() =>
                        {
                            waitLeftMagazine = MagazineTrayLoadFlow((int)eMag.ON_LEFT, LayerNum);
                            Console.WriteLine($"-------------- MagazineTrayLoadFlow Task - end {waitLeftMagazine}");

                            return waitLeftMagazine;
                        }, CancelTokenMagazine.Token);

                        nRetStep = 3160;
                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[READY] Complete Tray Load Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 3160:
                    if (waitLeftMagazine == 1)
                    {
                        //Left tray 배출하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        szLog = $"[AUTO] LEFT TRAY LOAD TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    if (waitLeftMagazine == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[AUTO] LEFT Load Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitLeftMagazine == 0)
                    {
                        //Tray 로드 완료
                        Globalo.motionManager.magazineHandler.isTrayReadyToLoad[0] = true;
                        Console.WriteLine($"waitLeftMagazine - {waitLeftMagazine}");
                        nTimeTick = Environment.TickCount;
                        nRetStep = 3190;
                        break;
                    }
                    break;
                case 3190:
                    nRetStep = 3000;
                    break;

                    //
                    //
                    //
                    //
                    //
                case 3200:
                    //---------------------------------------------------
                    //  RIGHT LOADER 교체 - 배출 > 로드
                    //---------------------------------------------------
                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == false)
                    {
                        waitRightMagazine = 0;
                        nRetStep = 3240;        //go load
                        break;
                    }
                    LayerNum = Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer;
                    if (RightMagazineTask == null || RightMagazineTask.IsCompleted)
                    {
                        waitRightMagazine = 1;
                        RightMagazineTask = Task.Run(() =>
                        {
                            waitRightMagazine = MagazineTrayUnloadFlow((int)eMag.ON_RIGHT, LayerNum);
                            Console.WriteLine($"-------------- MagazineTrayUnloadFlow Task - end {waitRightMagazine}");

                            return waitRightMagazine;
                        }, CancelTokenMagazine.Token);

                        Globalo.motionManager.ClearTrayChange(MotionControl.MotorSet.TrayPos.Right);
                        nRetStep = 3220;
                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[AUTO] Complete Tray Unload Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 3220:
                    if (waitRightMagazine == 1)
                    {
                        //Right tray 배출하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        szLog = $"[AUTO] RIGHT TRAY UNLOAD TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (waitRightMagazine == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[AUTO] RIGHT Unload Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitRightMagazine == 0)
                    {
                        //Tray 배출 완료
                        Console.WriteLine($"waitRightMagazine - {waitRightMagazine}");
                        nTimeTick = Environment.TickCount;
                        nRetStep = 3240;
                        break;
                    }
                    break;
                case 3240:
                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == false)
                    {

                        if (Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer == -1)
                        {
                            //RIGHT 매거진 교체 알람
                            szLog = $"[AUTO] RIGHT MAGAZINE CHANGE [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_INFO);
                            nRetStep = 3000;
                            break;
                        }
                        else
                        {
                            //투입 진행

                            szLog = $"[AUTO] RIGHT Tray Empty [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep = 3250;
                            break;
                        }
                    }
                    break;
                case 3250:
                    //---------------------------------------------------
                    //  RIGHT LOADER - 투입
                    //---------------------------------------------------
                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == true)
                    {
                        waitRightMagazine = 0;
                        nRetStep = 3000;        //go load
                        break;
                    }
                    LayerNum = Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer;

                    if (RightMagazineTask == null || RightMagazineTask.IsCompleted)
                    {
                        waitRightMagazine = 1;
                        RightMagazineTask = Task.Run(() =>
                        {
                            waitRightMagazine = MagazineTrayLoadFlow((int)eMag.ON_RIGHT, LayerNum);
                            Console.WriteLine($"-------------- MagazineTrayLoadFlow Task - end {waitRightMagazine}");

                            return waitRightMagazine;
                        }, CancelTokenMagazine.Token);

                        nRetStep = 3260;
                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[AUTO] Complete Tray Load Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 3260:
                    if (waitRightMagazine == 1)
                    {
                        //Right tray 로드하는 중
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_TRAY_CHANGE_TIMEOUT)
                    {
                        szLog = $"[AUTO] RIGHT TRAY LOAD TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    if (waitRightMagazine == -1)
                    {
                        //Gantry 에 Tray 로드 실패
                        szLog = $"[AUTO] RIGHT Load Tray Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    else if (waitRightMagazine == 0)
                    {
                        //Tray 배출 완료
                        Console.WriteLine($"waitRightMagazine - {waitRightMagazine}");
                        nTimeTick = Environment.TickCount;
                        nRetStep = 3290;
                        break;
                    }
                    break;
 
                case 3290:
                    nRetStep = 3000;
                    break;

            }
            return nRetStep;
        }
        #endregion

        #region [Tray_Load]
        private int MagazineTrayLoadFlow(int index , int LoadLayer)
        {
            int nRtn = -1;
            double zOffset = -20.0;
            bool bRtn = false;
            int nRetStep = 10;
            string szLog = "";
            int MagNum = index;
            Machine.eMagazine MotorY;
            Machine.eMagazine MotorZ;

            Machine.MagazineMachine.eTeachingPosList MovePos;

            int LayerIndex = LoadLayer;
            if (MagNum == 0)
            {
                MotorY = Machine.eMagazine.MAGAZINE_L_Y;
                MotorZ = Machine.eMagazine.MAGAZINE_L_Z;
            }
            else
            {
                MotorY = Machine.eMagazine.MAGAZINE_R_Y;
                MotorZ = Machine.eMagazine.MAGAZINE_R_Z;
            }
            if (LayerIndex == 0)
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.LAYER1;
            }
            else if (LayerIndex == 1)
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.LAYER2;
            }
            else if (LayerIndex == 2)
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.LAYER3;
            }
            else if (LayerIndex == 3)
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.LAYER4;
            }
            else if (LayerIndex == 4)
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.LAYER5;
            }
            else
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.WAIT_POS;
            }



            while (true)
            {
                if (CancelTokenMagazine.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("LoadTray Flow cancelled!");
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
                        //Y 대기 위치
                        bRtn = Globalo.motionManager.magazineHandler.Magazine_Y_Move(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, MotorY, false);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] {MotorY.ToString()} WAIT POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nLoadTimeTick[MagNum] = Environment.TickCount;
                        szLog = $"[READY] {MotorY.ToString()} WAIT POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 60;
                        break;
                    case 60:
                        //Y 대기 위치 확인
                        if (Globalo.motionManager.magazineHandler.MotorAxes[(int)MotorY].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkYMotorPos(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, MotorY))
                        {
                            szLog = $"[READY] {MotorY.ToString()} WAIT POS 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 80;

                            nLoadTimeTick[MagNum] = Environment.TickCount;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick[MagNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[READY] {MotorY.ToString()} WAIT POS 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 80:
                        //z 로드 위치 + Offset
                        bRtn = Globalo.motionManager.magazineHandler.Magazine_Z_Move(MovePos, MotorZ, zOffset, false);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] {MotorZ.ToString()} LOAD OFFSET POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nLoadTimeTick[MagNum] = Environment.TickCount;
                        szLog = $"[READY] {MotorZ.ToString()} LOAD OFFSET POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 100;
                        break;

                    case 100:
                        //z 로드 위치 확인 + Offset
                        if (Globalo.motionManager.magazineHandler.MotorAxes[(int)MotorZ].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkZMotorPos(MovePos, MotorZ, zOffset))
                        {
                            szLog = $"[READY] {MotorZ.ToString()} LOAD OFFSET POS 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 120;

                            nTimeTick = Environment.TickCount;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick[MagNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[READY] {MotorZ.ToString()} LOAD OFFSET POS 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 120:
                        //로드할 곳 TRAY 유무 확인
                        nRetStep = 140;
                        break;
                    case 140:
                        
                        nRetStep = 160;
                        break;
                    case 160:
                        
                        nRetStep = 180;
                        break;
                    case 180:
                        //Y 로드 위치 밀어넣기
                        bRtn = Globalo.motionManager.magazineHandler.Magazine_Y_Move(MovePos, MotorY, false);
                        if (bRtn == false)
                        {
                            szLog = $"[AUTO] {MotorY.ToString()} {MovePos.ToString()} POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nLoadTimeTick[MagNum] = Environment.TickCount;
                        szLog = $"[AUTO] {MotorY.ToString()} {MovePos.ToString()} POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 200;
                        break;

                    case 200:
                        //Y 로드 위치 이동 확인
                        if (Globalo.motionManager.magazineHandler.MotorAxes[(int)MotorY].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkYMotorPos(MovePos, MotorY))
                        {
                            szLog = $"[READY] {MotorY.ToString()} {MovePos.ToString()} POS 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 220;

                            nLoadTimeTick[MagNum] = Environment.TickCount;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick[MagNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[READY] {MotorY.ToString()} {MovePos.ToString()} POS 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        break;
                    case 220:
                        //z 로드 위치
                        bRtn = Globalo.motionManager.magazineHandler.Magazine_Z_Move(MovePos, MotorZ, 0.0, false);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] {MotorZ.ToString()} LOAD POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nLoadTimeTick[MagNum] = Environment.TickCount;
                        szLog = $"[READY] {MotorZ.ToString()} LOAD POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 240;
                        break;

                    case 240:
                        //z 로드 위치 확인
                        if (Globalo.motionManager.magazineHandler.MotorAxes[(int)MotorZ].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkZMotorPos(MovePos, MotorZ, 0.0))
                        {
                            szLog = $"[READY] {MotorZ.ToString()} LOAD POS 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 260;

                            nTimeTick = Environment.TickCount;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick[MagNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[READY] {MotorZ.ToString()} LOAD POS 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }

                        
                        break;
                    case 260:

                        nRetStep = 280;
                        break;
                    case 280:
                        //Y 대기 위치
                        bRtn = Globalo.motionManager.magazineHandler.Magazine_Y_Move(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, MotorY, false);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] {MotorY.ToString()} WAIT POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nLoadTimeTick[MagNum] = Environment.TickCount;
                        szLog = $"[READY] {MotorY.ToString()} WAIT POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 300;
                        break;
                    case 300:
                        //Y 대기 위치 확인
                        if (Globalo.motionManager.magazineHandler.MotorAxes[(int)MotorY].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkYMotorPos(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, MotorY))
                        {
                            szLog = $"[READY] {MotorY.ToString()} WAIT POS 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 320;

                            nLoadTimeTick[MagNum] = Environment.TickCount;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick[MagNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[READY] {MotorY.ToString()} WAIT POS 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 320:
                        if (Globalo.motionManager.magazineHandler.GetTrayUndocked(MagNum) == true)
                        {
                            szLog = $"[AUTO] SEPARATION CHECK COMPLETE[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 340;
                        }
                        else
                        {
                            szLog = $"[AUTO] SEPARATION CHECK FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 340:
                        //loader 위 Tray 감지 되면 ok
                        //하부 interlock 감지도 안되면 ok
                        if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader(MagNum) == true)
                        {
                            szLog = $"[AUTO] TRAY LOAD COMPLETE[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 360;
                        }
                        else
                        {
                            szLog = $"[AUTO] TRAY LOAD FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 360:
                        //자동 진행 위치로 상승
                        bRtn = Globalo.motionManager.magazineHandler.Magazine_Z_Move(Machine.MagazineMachine.eTeachingPosList.TRAY_LOAD_POS, MotorZ, 0.0, false);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] {MotorZ.ToString()} LOAD POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nLoadTimeTick[MagNum] = Environment.TickCount;
                        szLog = $"[READY] {MotorZ.ToString()} LOAD POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 380;
                        break;

                    case 380:
                        if (Globalo.motionManager.magazineHandler.MotorAxes[(int)MotorZ].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkZMotorPos(Machine.MagazineMachine.eTeachingPosList.TRAY_LOAD_POS, MotorZ, 0.0))
                        {
                            szLog = $"[READY] {MotorZ.ToString()} LOAD POS 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 400;

                            nTimeTick = Environment.TickCount;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick[MagNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[READY] {MotorZ.ToString()} LOAD POS 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 400:
                        nRetStep = 900;
                        break;
                    case 900:
                        szLog = $"[READY] LOAD COMPLETE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        Globalo.motionManager.magazineHandler.isTrayReadyToLoad[index] = true;
                        Globalo.motionManager.magazineHandler.IsTryChanging[MagNum] = false;          //투입 완료

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

        #region [Tray_Unload]
        private int MagazineTrayUnloadFlow(int index, int UnloadLayer)
        {
            int nRtn = -1;
            double zOffset = -20.0;
            bool bRtn = false;
            int nRetStep = 10;
            string szLog = "";
            int MagNum = index;
            Machine.eMagazine MotorY;
            Machine.eMagazine MotorZ;

            Machine.MagazineMachine.eTeachingPosList MovePos;

            int LayerIndex = UnloadLayer;
            if (MagNum == 0)
            {
                MotorY = Machine.eMagazine.MAGAZINE_L_Y;
                MotorZ = Machine.eMagazine.MAGAZINE_L_Z;
            }
            else
            {
                MotorY = Machine.eMagazine.MAGAZINE_R_Y;
                MotorZ = Machine.eMagazine.MAGAZINE_R_Z;
            }
            if (LayerIndex == 0)
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.LAYER1;
            }
            else if (LayerIndex == 1)
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.LAYER2;
            }
            else if (LayerIndex == 2)
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.LAYER3;
            }
            else if (LayerIndex == 3)
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.LAYER4;
            }
            else if (LayerIndex == 4)
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.LAYER5;
            }
            else
            {
                MovePos = Machine.MagazineMachine.eTeachingPosList.WAIT_POS;
            }


            while (true)
            {
                if (CancelTokenMagazine.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("UnLoadTray Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                switch (nRetStep)
                {
                    case 10:
                        Globalo.motionManager.magazineHandler.IsTryChanging[MagNum] = true;          //배출 중
                        nRetStep = 20;
                        break;
                    case 20:
                        nRetStep = 40;
                        break;
                    case 40:
                        //Y 대기 위치
                        bRtn = Globalo.motionManager.magazineHandler.Magazine_Y_Move(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, MotorY, false);
                        if (bRtn == false)
                        {
                            szLog = $"[AUTO] {MotorY.ToString()} WAIT POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nLoadTimeTick[MagNum] = Environment.TickCount;
                        szLog = $"[AUTO] {MotorY.ToString()} WAIT POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 60;
                        break;
                    case 60:
                        //Y 대기 위치 확인
                        if (Globalo.motionManager.magazineHandler.MotorAxes[(int)MotorY].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkYMotorPos(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, MotorY))
                        {
                            szLog = $"[AUTO] {MotorY.ToString()} WAIT POS 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 80;

                            nLoadTimeTick[MagNum] = Environment.TickCount;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick[MagNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[AUTO] {MotorY.ToString()} WAIT POS 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        break;
                    case 80:

                        nRetStep = 100;
                        break;
                    case 100:
                        //배출할 곳 TRAY 유무 확인
                        nRetStep = 120;

                        break;
                    case 120:

                        nRetStep = 140;


                        break;
                    case 140:
                        
                        nRetStep = 160;
                        break;
                    case 160:
                        //z 배출 위치 + Offset
                        bRtn = Globalo.motionManager.magazineHandler.Magazine_Z_Move(MovePos, MotorZ, zOffset, false);
                        if (bRtn == false)
                        {
                            szLog = $"[AUTO] {MotorZ.ToString()} UNLOAD OFFSET POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nLoadTimeTick[MagNum] = Environment.TickCount;
                        szLog = $"[AUTO] {MotorZ.ToString()} UNLOAD OFFSET POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 180;
                        break;

                    case 180:
                        //z 배출 위치 + Offset 확인
                        if (Globalo.motionManager.magazineHandler.MotorAxes[(int)MotorZ].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkZMotorPos(MovePos, MotorZ, zOffset))
                        {
                            szLog = $"[AUTO] {MotorZ.ToString()} UNLOAD OFFSET POS 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 200;

                            nTimeTick = Environment.TickCount;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick[MagNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[AUTO] {MotorZ.ToString()} UNLOAD OFFSET POS 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 200:
                        //Y 배출 위치 밀어넣기
                        bRtn = Globalo.motionManager.magazineHandler.Magazine_Y_Move(MovePos, MotorY, false);
                        if (bRtn == false)
                        {
                            szLog = $"[AUTO] {MotorY.ToString()} {MovePos.ToString()} POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nLoadTimeTick[MagNum] = Environment.TickCount;
                        szLog = $"[AUTO] {MotorY.ToString()} {MovePos.ToString()} POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 220;
                        break;
                    case 220:
                        //Y 배출 위치 이동 확인
                        if (Globalo.motionManager.magazineHandler.MotorAxes[(int)MotorY].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkYMotorPos(MovePos, MotorY))
                        {
                            szLog = $"[AUTO] {MotorY.ToString()} {MovePos.ToString()} POS 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 240;

                            nLoadTimeTick[MagNum] = Environment.TickCount;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick[MagNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[AUTO] {MotorY.ToString()} {MovePos.ToString()} POS 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        break;

                    case 240:
                        //z 배출 위치
                        bRtn = Globalo.motionManager.magazineHandler.Magazine_Z_Move(MovePos, MotorZ, 0.0, false);
                        if (bRtn == false)
                        {
                            szLog = $"[AUTO] {MotorZ.ToString()} UNLOAD POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nLoadTimeTick[MagNum] = Environment.TickCount;
                        szLog = $"[AUTO] {MotorZ.ToString()} UNLOAD POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 260;
                        break;

                    case 260:
                        //z 배출 위치 확인
                        if (Globalo.motionManager.magazineHandler.MotorAxes[(int)MotorZ].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkZMotorPos(MovePos, MotorZ, 0.0))
                        {
                            szLog = $"[AUTO] {MotorZ.ToString()} UNLOAD POS 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 280;

                            nTimeTick = Environment.TickCount;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick[MagNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[AUTO] {MotorZ.ToString()} UNLOAD POS 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 280:
                        
                        nRetStep = 300;
                        break;
                    case 300:
                        //Y 대기 위치
                        bRtn = Globalo.motionManager.magazineHandler.Magazine_Y_Move(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, MotorY, false);
                        if (bRtn == false)
                        {
                            szLog = $"[AUTO] {MotorY.ToString()} WAIT POS MOVE FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        nLoadTimeTick[MagNum] = Environment.TickCount;
                        szLog = $"[AUTO] {MotorY.ToString()} WAIT POS MOVE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 320;
                        break;
                    case 320:
                        //Y 대기 위치 확인
                        if (Globalo.motionManager.magazineHandler.MotorAxes[(int)MotorY].GetStopAxis() == true &&
                        Globalo.motionManager.magazineHandler.ChkYMotorPos(Machine.MagazineMachine.eTeachingPosList.WAIT_POS, MotorY))
                        {
                            szLog = $"[AUTO] {MotorY.ToString()} WAIT POS 이동 완료 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 340;

                            nLoadTimeTick[MagNum] = Environment.TickCount;
                            break;
                        }
                        else if (Environment.TickCount - nLoadTimeTick[MagNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                        {
                            szLog = $"[AUTO] {MotorY.ToString()} WAIT POS 이동 시간 초과 [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 340:
                        nRetStep = 360;
                        break;
                    case 360:
                        if (Globalo.motionManager.magazineHandler.GetTrayUndocked(MagNum) == true)
                        {
                            szLog = $"[AUTO] SEPARATION CHECK COMPLETE[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 380;
                        }
                        else
                        {
                            szLog = $"[AUTO] SEPARATION CHECK FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        
                        break;
                    case 380:
                        if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader(MagNum) == false)
                        {
                            
                            Globalo.motionManager.magazineHandler.isTrayReadyToLoad[index] = false;
                            szLog = $"[AUTO] TRAY UNLOAD COMPLETE[STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 400;
                        }
                        else
                        {
                            szLog = $"[AUTO] TRAY UNLOAD FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep *= -1;
                            break;
                        }
                        break;
                    case 400:
                        //loader 위 Tray 감지 안되면 ok
                        //하부 interlock 감지도 안되면 ok

                        if (index == 0)
                        {
                            Globalo.motionManager.magazineHandler.LeftMagazineLayerAdd();
                        }
                        else
                        {
                            Globalo.motionManager.magazineHandler.RightMagazineLayerAdd();
                        }
                        nRetStep = 500;
                        break;
                    case 500:
                        nRetStep = 800;
                        break;

                    case 800:
                        nRetStep = 900;
                        break;
                    case 900:
                        szLog = $"[READY] UNLOAD COMPLETE [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        
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
    }
}
