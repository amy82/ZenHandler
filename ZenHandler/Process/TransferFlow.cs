using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Process
{
    public class TransferFlow
    {
        private readonly SynchronizationContext _syncContext;
        public int nTimeTick = 0;
        public int[] SensorSet = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] OrgOnGoing = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public TransferFlow()
        {
            _syncContext = SynchronizationContext.Current;
        }
        public int HomeProcess(int nStep)                 //  원점(1000 ~ 2000)
        {
            //string szLog = "";
            //uint duState = 0;

            //bool bRtn = false;
            //int nLensAxis = 0;
            uint duState = 0;
            bool m_bHomeProc = true;
            bool m_bHomeError = false;
            //double dAcc = 0.3;
            //int i = 0;


            uint duRetCode = 0;
            string szLog = "";
            bool bRtn = false;
            double dSpeed = 0.0;
            double dAcc = 0.3;
            int nRetStep = nStep;

            //TODO: 모터 알람뜨는지 체크해서 정지해야될듯
            switch (nStep)
            {
                case 1000:
                    Console.WriteLine("[ORIGIN] TRANSFER START");
                    nRetStep = 1050;
                    break;
                case 1050:
                    bRtn = true;

                    for (int i = 0; i < Globalo.motionManager.transferMachine.MotorAxes.Length; i++)
                    {
                        if (Globalo.motionManager.transferMachine.MotorAxes[i].AmpEnable() == false)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.transferMachine.MotorAxes[i].Name} AmpEnable Fail]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                    }
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] Servo On Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    nRetStep = 1060;
                    break;
                case 1060:
                    //실린더 전체 상승
                    nRetStep = 1070;
                    break;
                case 1070:
                    //실린더 전체 상승 확인
                    nRetStep = 1080;
                    break;
                case 1080:
                    nRetStep = 1090;
                    break;
                case 1090:
                    //z축 Limit 이동

                    if (Globalo.motionManager.transferMachine.TransferZ.GetStopAxis() == false)
                    {
                        Globalo.motionManager.transferMachine.TransferZ.Stop();
                        break;
                    }

                    //SensorSet[0] = (int)Globalo.motionManager.transferMachine.TransferZ.m_lAxisNo;
                    //SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.NegEndLimit;
                    //SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    //SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    dSpeed = (15 * -1);      //-1은 왼쪽 이동

                    bRtn = Globalo.motionManager.transferMachine.TransferZ.MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.EMERGENCY_STOP);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] TransferZ (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] TransferZ (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 1100;
                    break;

                case 1100:
                    nTimeTick = Environment.TickCount;
                    nRetStep = 1110;
                    break;
                case 1110:

                    //z축 Limit 이동 확인

                    if (Globalo.motionManager.transferMachine.TransferZ.GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.TransferZ.GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] TransferZ (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1120;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] TransferZ (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1120:
                    nRetStep = 1130;
                    break;
                case 1130:
                    //x축 Limit 이동
                    if (Globalo.motionManager.transferMachine.TransferX.GetStopAxis() == false)
                    {
                        Globalo.motionManager.transferMachine.TransferX.Stop();
                        break;
                    }

                    dSpeed = (10 * -1);      //-1은 왼쪽 이동

                    bRtn = Globalo.motionManager.transferMachine.TransferX.MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.EMERGENCY_STOP);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] TransferX (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] TransferX (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nRetStep = 1140;
                    break;

                case 1140:
                    //y축 Limit 이동
                    if (Globalo.motionManager.transferMachine.TransferY.GetStopAxis() == false)
                    {
                        Globalo.motionManager.transferMachine.TransferY.Stop();
                        break;
                    }

                    dSpeed = (10 * -1);      //-1은 왼쪽 이동

                    bRtn = Globalo.motionManager.transferMachine.TransferY.MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.EMERGENCY_STOP);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] TransferY (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] TransferY (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nRetStep = 1150;
                    break;
                case 1150:
                    nTimeTick = Environment.TickCount;
                    nRetStep = 1160;
                    break;
                case 1160:
                    //y축 Limit 이동 확인

                    if (Globalo.motionManager.transferMachine.TransferY.GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.TransferY.GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] TransferY (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1170;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] TransferY (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1170:
                    nTimeTick = Environment.TickCount;
                    nRetStep = 1180;
                    break;
                case 1180:
                    //x축 Limit 이동 확인

                    if (Globalo.motionManager.transferMachine.TransferX.GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.TransferX.GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] TransferX (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1190;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] TransferX (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1190:
                    nRetStep = 1200;
                    break;
                case 1200:
                    nRetStep = 1250;
                    break;
                case 1250:
                    szLog = $"[ORIGIN] Transfer X/Y/Z Limit 위치 이동 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 1260;
                    break;
                case 1260:
                    bRtn = true;
                    for (int i = 0; i < Globalo.motionManager.transferMachine.MotorAxes.Length; i++)
                    {
                        OrgOnGoing[i] = 0;
                        Globalo.motionManager.transferMachine.MotorAxes[i].OrgState = false;

                        //Home Method Setting
                        uint duZPhaseUse = 0;
                        double dHomeClrTime = 2000.0;
                        double dHomeOffset = 0.0;

                        //++ 지정한 축의 원점검색 방법을 변경합니다.
                        duRetCode = CAXM.AxmHomeSetMethod(
                            Globalo.motionManager.transferMachine.MotorAxes[i].m_lAxisNo,
                            (int)Globalo.motionManager.transferMachine.MotorAxes[i].HomeMoveDir,
                            (uint)Globalo.motionManager.transferMachine.MotorAxes[i].HomeDetect,
                            duZPhaseUse, dHomeClrTime, dHomeOffset);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.transferMachine.MotorAxes[i].Name} AxmHomeSetMethod Fail [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }

                        duRetCode = CAXM.AxmHomeSetVel(
                            Globalo.motionManager.transferMachine.MotorAxes[i].m_lAxisNo,
                            Globalo.motionManager.transferMachine.MotorAxes[i].FirstVel,
                            Globalo.motionManager.transferMachine.MotorAxes[i].SecondVel,
                            Globalo.motionManager.transferMachine.MotorAxes[i].ThirdVel,
                            50.0, 0.3, 0.3);//LastVel, Acc Firset, Acc Second


                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.transferMachine.MotorAxes[i].Name} AxmHomeSetVel Fail [STEP : {nStep}]";
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
                    for (int i = 0; i < Globalo.motionManager.transferMachine.MotorAxes.Length; i++)
                    {
                        duRetCode = CAXM.AxmHomeSetStart(Globalo.motionManager.transferMachine.MotorAxes[i].m_lAxisNo);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.transferMachine.MotorAxes[i].Name} AxmHomeSetStart Fail [STEP : {nStep}]";
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
                    for (int i = 0; i < Globalo.motionManager.transferMachine.MotorAxes.Length; i++)
                    {
                        CAXM.AxmHomeGetResult(Globalo.motionManager.transferMachine.MotorAxes[i].m_lAxisNo, ref duState);
                        if (duState == (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS)
                        {
                            //원점 완료
                            Globalo.motionManager.transferMachine.MotorAxes[i].OrgState = true;
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
                            szLog = $"[ORIGIN] {Globalo.motionManager.transferMachine.MotorAxes[i].Name} HOME 동작 ERROR [STEP : {nStep}]";
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
                    szLog = $"[ORIGIN] TRANSFER UNIT 전체 원점 위치 이동 완료 [STEP : {nStep}]";
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
        public int AutoReady(int nStep)					//  운전준비(2000 ~ 3000)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 2000:
                    DialogResult result = DialogResult.None;
                    _syncContext.Send(_ =>
                    {
                        result = Globalo.MessageAskPopup("READY?!\n(SPACE KEY START)");
                    }, null);
                    if (result == DialogResult.Yes)
                    {
                        nRetStep = 2010;
                    }
                    else
                    {
                        szLog = $"[AUTO] 자동운전 일시정지[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 2010:
                    nRetStep = 2020;
                    break;
                case 2020:
                    Globalo.motionManager.transferMachine.TransFer_Z_Move(Machine.TransferMachine.eTeachingPosList.WAIT_POS, true);
                    nRetStep = 2030;
                    nTimeTick = Environment.TickCount;
                    break;
                case 2030:
                    if (Globalo.motionManager.transferMachine.TransferZ.GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.ChkZMotorPos(Machine.TransferMachine.eTeachingPosList.WAIT_POS))
                    {
                        szLog = $"[READY] TRANSFER Z WAIT_POS 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2040;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] TRANSFER Z WAIT_POS  이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 2040:
                    Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.WAIT_POS);
                    nRetStep = 2050;
                    break;
                case 2050:
                    if (Globalo.motionManager.transferMachine.TransferX.GetStopAxis() == true && Globalo.motionManager.transferMachine.TransferY.GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.ChkXYMotorPos(Machine.TransferMachine.eTeachingPosList.WAIT_POS))
                    {
                        szLog = $"[READY] WAIT_POS 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2060;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[READY] WAIT_POS 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                   
                    break;
                case 2060:
                    nRetStep = 2070;
                    break;
                case 2070:
                    nRetStep = 2080;
                    break;
                case 2080:
                    nRetStep = 2090;
                    break;
                case 2090:
                    nRetStep = 2100;
                    break;
                case 2100:
                    nRetStep = 2900;
                    break;
                case 2900:
                    Globalo.motionManager.transferMachine.RunState = OperationState.PreparationComplete;
                    szLog = $"[READY] TRANSFER 운전준비 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 3000;
                    break;
            }
            return nRetStep;
        }
        //
        //  3000
        //
        public int Auto_Waiting(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 3000:
                    nRetStep = 3100;
                    break;
                case 3100:
                    nRetStep = 3200;
                    break;
                case 3200:
                    nRetStep = 3300;
                    break;
                case 3300:
                    nRetStep = 3400;
                    break;
                case 3400:
                    nRetStep = 3500;
                    break;
                case 3500:
                    DialogResult result = DialogResult.None;
                    _syncContext.Send(_ =>
                    {
                        result = Globalo.MessageAskPopup("제품 투입 후 진행해주세요!\n(SPACE KEY START)");
                    }, null);
                    if (result == DialogResult.Yes)
                    {
                        nRetStep = 3600;
                    }
                    else
                    {
                        szLog = $"[AUTO] 자동운전 일시정지[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 3600:
                    Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS);
                    nRetStep = 3700;
                    nTimeTick = Environment.TickCount;
                    break;
                case 3700:
                    if (Globalo.motionManager.transferMachine.TransferX.GetStopAxis() == true && Globalo.motionManager.transferMachine.TransferY.GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.ChkXYMotorPos(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS))
                    {
                        szLog = $"[ORIGIN] LEFT_TRAY_LOAD_POS 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 3720;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] LEFT_TRAY_LOAD_POS 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 3720:
                    Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.WAIT_POS);
                    nRetStep = 3740;
                    nTimeTick = Environment.TickCount;
                    break;
                case 3740:
                    if (Globalo.motionManager.transferMachine.TransferX.GetStopAxis() == true && Globalo.motionManager.transferMachine.TransferY.GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.ChkXYMotorPos(Machine.TransferMachine.eTeachingPosList.WAIT_POS))
                    {
                        szLog = $"[ORIGIN] WAIT_POS 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 3760;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] WAIT_POS 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 3760:
                    nRetStep = 3780;
                    break;
                case 3780:
                    nRetStep = 3800;
                    break;
                case 3800:
                    nRetStep = 3820;
                    break;
                case 3820:
                    nRetStep = 3840;
                    break;
                case 3840:
                    nRetStep = 3900;
                    break;

                case 3900:
                    nRetStep = 4000;
                    break;
            }
            return nRetStep;
        }
        //
        //  4000
        //
        public int Auto_LoadInTray(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 4000:
                    nRetStep = 4900;
                    break;

                case 4900:
                    
                    break;
            }
            return nRetStep;
        }
        //
        //  5000
        //
        public int Auto_SocketInsert(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 5000:
                    nRetStep = 5900;
                    break;

                case 5900:
                    
                    break;
            }
            return nRetStep;
        }
        //
        //  6000
        //
        public int Auto_SocketOutput(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 6000:
                    nRetStep = 6900;
                    break;

                case 6900:

                    break;
            }
            return nRetStep;
        }
        //
        //  7000
        //
        public int Auto_UnLoadInTray(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 7000:
                    nRetStep = 7900;
                    break;

                case 7900:

                    break;
            }
            return nRetStep;
        }
        //
        //  8000
        //
        public int Auto_Ng_UnLoading(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 8000:
                    nRetStep = 8900;
                    break;

                case 8900:
                    nRetStep = 3000;
                    break;
            }
            return nRetStep;
        }
        //
        //  9000
        //
        public int Auto_Cancel(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 9000:
                    nRetStep = 9900;
                    break;

                case 9900:
                    nRetStep = 3000;
                    break;
            }
            return nRetStep;
        }
    }
}