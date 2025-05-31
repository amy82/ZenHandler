using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public class AoiSocketFlow
    {
        public int nTimeTick = 0;
        public AoiSocketFlow()
        {

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
                    break;
                case 1100:
                    //LEFT Z 축 상승
                    //RIGHT Z 축 상승
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_Z].GetStopAxis() == false)
                    {
                        Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_Z].Stop();
                        break;
                    }
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_Z].GetStopAxis() == false)
                    {
                        Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_Z].Stop();
                        break;
                    }

                    dSpeed = (15 * 1);      //-1은 왼쪽, 하강 이동

                    bRtn = Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_Z].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.PosEndLimit);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LEFT SOCKET Z (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] LEFT SOCKET Z (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);



                    bRtn = Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_Z].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.PosEndLimit);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] RIGHT SOCKET Z (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] RIGHT SOCKET Z (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 1120;
                    nTimeTick = Environment.TickCount;
                    break;
                case 1120:
                    //LEFT Z 축 상승 확인
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_Z].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_Z].GetPosiSensor() == true)
                    {
                        szLog = $"[ORIGIN] LEFT SOCKET Z (+)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1140;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                    {
                        szLog = $"[ORIGIN] LEFT SOCKET Z (+)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 1140:
                    //RIGHT Z 축 상승 확인
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_Z].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_Z].GetPosiSensor() == true)
                    {
                        szLog = $"[ORIGIN] RIGHT SOCKET Z (+)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1160;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                    {
                        szLog = $"[ORIGIN] RIGHT SOCKET Z (+)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1160:
                    //LEFT X 축 - LIMIT 이동
                    //RIGHT X 축 - LIMIT 이동
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_X].GetStopAxis() == false)
                    {
                        Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_X].Stop();
                        break;
                    }
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_X].GetStopAxis() == false)
                    {
                        Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_X].Stop();
                        break;
                    }

                    dSpeed = (15 * 1);      //-1은 왼쪽, 하강 이동

                    bRtn = Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_X].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LEFT SOCKET X (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] LEFT SOCKET X (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);



                    bRtn = Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_X].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] RIGHT SOCKET X (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] RIGHT SOCKET Z (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 1180;
                    nTimeTick = Environment.TickCount;
                    break;
                case 1180:
                    //LEFT X 축 - LIMIT 이동 확인
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_X].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] LEFT SOCKET X (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1200;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                    {
                        szLog = $"[ORIGIN] LEFT SOCKET X (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1200:
                    //RIGHT X 축 - LIMIT 이동 확인
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_X].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] RIGHT SOCKET X (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1220;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                    {
                        szLog = $"[ORIGIN] RIGHT SOCKET X (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
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
                        for (int i = 0; i < Globalo.motionManager.socketAoiMachine.MotorAxes.Length; i++)
                        {
                            Globalo.motionManager.socketAoiMachine.MotorAxes[i].OrgState = true;
                        }

                        nRetStep = 1900;
                        break;
                    }
                    bRtn = true;
                    for (int i = 0; i < Globalo.motionManager.socketAoiMachine.MotorAxes.Length; i++)
                    {
                        Globalo.motionManager.socketAoiMachine.MotorAxes[i].OrgState = false;

                        //Home Method Setting
                        uint duZPhaseUse = 0;
                        double dHomeClrTime = 2000.0;
                        double dHomeOffset = 0.0;

                        //++ 지정한 축의 원점검색 방법을 변경합니다.
                        duRetCode = CAXM.AxmHomeSetMethod(
                            Globalo.motionManager.socketAoiMachine.MotorAxes[i].m_lAxisNo,
                            (int)Globalo.motionManager.socketAoiMachine.MotorAxes[i].HomeMoveDir,
                            (uint)Globalo.motionManager.socketAoiMachine.MotorAxes[i].HomeDetect,
                            duZPhaseUse, dHomeClrTime, dHomeOffset);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.socketAoiMachine.MotorAxes[i].Name} AxmHomeSetMethod Fail [STEP : {nStep}]";
                            Globalo.LogPrint("LIft", szLog);
                        }

                        duRetCode = CAXM.AxmHomeSetVel(
                            Globalo.motionManager.socketAoiMachine.MotorAxes[i].m_lAxisNo,
                            Globalo.motionManager.socketAoiMachine.MotorAxes[i].FirstVel,
                            Globalo.motionManager.socketAoiMachine.MotorAxes[i].SecondVel,
                            Globalo.motionManager.socketAoiMachine.MotorAxes[i].ThirdVel,
                            50.0, 0.3, 0.3);//LastVel, Acc Firset, Acc Second


                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.socketAoiMachine.MotorAxes[i].Name} AxmHomeSetVel Fail [STEP : {nStep}]";
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
                    for (int i = 0; i < Globalo.motionManager.socketAoiMachine.MotorAxes.Length; i++)
                    {
                        duRetCode = CAXM.AxmHomeSetStart(Globalo.motionManager.socketAoiMachine.MotorAxes[i].m_lAxisNo);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.socketAoiMachine.MotorAxes[i].Name} AxmHomeSetStart Fail [STEP : {nStep}]";
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
                    for (int i = 0; i < Globalo.motionManager.socketAoiMachine.MotorAxes.Length; i++)
                    {
                        CAXM.AxmHomeGetResult(Globalo.motionManager.socketAoiMachine.MotorAxes[i].m_lAxisNo, ref duState);
                        if (duState == (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS)
                        {
                            //원점 완료
                            Globalo.motionManager.socketAoiMachine.MotorAxes[i].OrgState = true;
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
                            szLog = $"[ORIGIN] {Globalo.motionManager.socketEEpromMachine.MotorAxes[i].Name} HOME 동작 ERROR [STEP : {nStep}]";
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
                    Globalo.motionManager.socketAoiMachine.RunState = OperationState.OriginDone;
                    szLog = $"[ORIGIN] AOI SOCKET UNIT 전체 원점 위치 이동 완료 [STEP : {nStep}]";
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
            string szLog = "";
            bool bRtn = false;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 2000:


                    break;
                case 2900:
                    Globalo.motionManager.socketAoiMachine.RunState = OperationState.Standby;
                    szLog = $"[READY] AOI SOCKET 운전준비 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 3000;
                    break;
            }

            return nRetStep;
        }
        #endregion
        

    }
}
