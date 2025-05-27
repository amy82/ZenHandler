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
        public int nLoadTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯
        public int nUnloadTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯

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

                    if (Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer == -1)        //둘다 없을 경우, 진행 불가
                    {

                        szLog = $"[READY] LPLEASE CHANGE THE LEFT MEGAZINE [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_INFO);
                        nRetStep *= -1;
                        break;
                    }
                    if (Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer == -1)        //둘다 없을 경우, 진행 불가
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

                    break;
                case 2260:

                    break;
                case 2280:

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

                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_LEFT) == true)
                    {
                        szLog = $"[READY] Left Loader TRAY LOADED [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);


                        //Left 로드 위치로 이동

                        Globalo.motionManager.magazineHandler.Magazine_Y_Move(Machine.MagazineHandler.eTeachingPosList.WAIT_POS, Machine.eMagazine.MAGAZINE_L_Y, false);
                    }
                    else
                    {
                        szLog = $"[READY] Left Loader TRAY EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    nRetStep = 2620;
                    break;
                case 2620:
                    if (Globalo.motionManager.magazineHandler.GetIsTrayOnLoader((int)eMag.ON_RIGHT) == true)
                    {
                        szLog = $"[READY] Right Loader TRAY LOADED [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);


                        //Right 로드 위치로 이동

                        Globalo.motionManager.magazineHandler.Magazine_Y_Move(Machine.MagazineHandler.eTeachingPosList.WAIT_POS, Machine.eMagazine.MAGAZINE_R_Y, false);
                    }
                    else
                    {
                        szLog = $"[READY] Right Loader TRAY EMPTY [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    nRetStep = 2640;
                    break;
                case 2640:

                    break;
                case 2800:

                    break;
                case 2900:
                    Globalo.motionManager.magazineHandler.RunState = OperationState.Standby;
                    szLog = $"[READY] LIFT 운전준비 완료 [STEP : {nStep}]";
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
            bool result = false;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 3000:
                    //요청 대기
                    //1.Tray 로드 from Magazine
                    //2.Tray 배출 to Magazine
                    //

                    Globalo.motionManager.magazineHandler.IsLoadingTray[0] = true;
                    Globalo.motionManager.magazineHandler.IsUnloadingTray[0] = true;

                    Globalo.motionManager.magazineHandler.IsLoadingTray[1] = true;
                    Globalo.motionManager.magazineHandler.IsUnloadingTray[1] = true;
                    break;
                case 3100:
                    //---------------------------------------------------
                    //  Tray 로드
                    //---------------------------------------------------
                    //if (LoadTrayTask == null || LoadTrayTask.IsCompleted)
                    //{
                    //    waitLoadTray = 1;
                    //    LoadTrayTask = Task.Run(() =>
                    //    {
                    //        waitLoadTray = MagazineTrayLoadFlow();
                    //        Console.WriteLine($"-------------- LoadTray Task - end {waitLoadTray}");

                    //        return waitLoadTray;
                    //    }, CancelTokenMagazine.Token);
                    //    nRetStep = 3120;
                    //}
                    //else
                    //{
                    //    //일시정지
                    //    szLog = $"[AUTO] Tray Load Move Fail [STEP : {nStep}]";
                    //    Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                    //    nRetStep *= -1;
                    //    break;
                    //}
                    break;
                case 3120:
                    break;
                case 3140:
                    break;
                case 3160:
                    break;
                case 3180:
                    break;
                case 3200:
                    //---------------------------------------------------
                    //  Tray 배출
                    //---------------------------------------------------
                    //if (UnloadTrayTask == null || UnloadTrayTask.IsCompleted)
                    //{
                    //    waitUnloadTray = 1;
                    //    UnloadTrayTask = Task.Run(() =>
                    //    {
                    //        waitUnloadTray = MagazineTrayUnloadFlow();
                    //        Console.WriteLine($"-------------- UnloadTray Task - end {waitUnloadTray}");

                    //        return waitUnloadTray;
                    //    }, CancelTokenMagazine.Token);

                    //    nRetStep = 3220;
                    //}
                    //else
                    //{
                    //    //Globalo.motionManager.liftMachine.IsUnloadingOutputTray = false;
                        
                    //    //일시정지
                    //    szLog = $"[AUTO] Complete Tray Unload Move Fail [STEP : {nStep}]";
                    //    Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                    //    nRetStep *= -1;
                    //    break;
                    //}
                    break;
                case 3220:
                    break;
                case 3240:
                    break;
                case 3260:
                    break;
                case 3280:
                    break;
                case 3300:

                    break;
                case 3400:

                    break;

            }
            return nRetStep;
        }
        #endregion

        #region [Tray_Load]
        private int MagazineTrayLoadFlow(int index , int LoadLayer)
        {
            int nRtn = -1;

            bool bRtn = false;
            int nRetStep = 10;
            string szLog = "";
            while (true)
            {
                if (CancelTokenMagazine.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Gantry LoadTray Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                if (nRetStep != 100 && nRetStep != 120)
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

            bool bRtn = false;
            int nRetStep = 10;
            string szLog = "";
            while (true)
            {
                if (CancelTokenMagazine.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Gantry LoadTray Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                if (nRetStep != 100 && nRetStep != 120)
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
