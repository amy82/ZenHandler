using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public class TransferFlow
    {
        public int nTimeTick = 0;
        public int[] SensorSet = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] OrgOnGoing = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public TransferFlow()
        {

        }
        public int HomeProcess(int nStep)                 //  원점(1000 ~ 2000)
        {
            //string szLog = "";
            //uint duState = 0;

            //bool bRtn = false;
            //int nLensAxis = 0;
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
            switch (nStep)
            {
                case 1000:
                    Console.WriteLine("[ORIGIN] TRANSFER START");
                    nRetStep = 1050;
                    break;
                case 1050:
                    nRetStep = 1060;
                    break;
                case 1060:
                    //실린더 전체 상승
                    break;
                case 1070:
                    //실린더 전체 상승 확인
                    break;
                case 1080:

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

                    dSpeed = (7 * -1);      //-1은 왼쪽 이동

                    bRtn = Globalo.motionManager.transferMachine.MoveAxisLimit( Globalo.motionManager.transferMachine.TransferZ, dSpeed, dAcc,
                        AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.SLOWDOWN_STOP);
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
                    else if (Environment.TickCount - nTimeTick > 30000)//else if ((Environment.TickCount - nTimeTick) / 1000.0 > 5)
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

                    dSpeed = (7 * -1);      //-1은 왼쪽 이동

                    bRtn = Globalo.motionManager.transferMachine.MoveAxisLimit(Globalo.motionManager.transferMachine.TransferX, dSpeed, dAcc,
                        AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.SLOWDOWN_STOP);
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

                    dSpeed = (7 * -1);      //-1은 왼쪽 이동

                    bRtn = Globalo.motionManager.transferMachine.MoveAxisLimit(Globalo.motionManager.transferMachine.TransferY, dSpeed, dAcc,
                        AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.SLOWDOWN_STOP);
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

                    break;
                case 1200:

                    break;
                case 1210:

                    break;
                case 1220:

                    break;
                case 1230:

                    break;
                case 1240:

                    break;
                case 1250:
                    szLog = $"[ORIGIN] Transfer X/Y/Z Limit 위치 이동 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
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
                            Globalo.motorControl.OrgFirstVel[i],    //
                            Globalo.motorControl.OrgSecondVel[i],
                            Globalo.motorControl.OrgThirdVel[i], 
                            50.0,   //LastVel
                            0.3, //Acc Firset
                            0.3);//Acc Second


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

                    nRetStep = 1190;
                    break;
                case 1270:

                    break;
                case 1280:

                    break;
                case 1290:

                    break;
                case 1300:

                    break;


                case 1900:
                    Thread.Sleep(5000);
                    //원점 복귀 완료
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
            int nRetStep = nStep;
            switch (nStep)
            {
                case 2000:

                    break;
                case 1900:

                    break;
            }
            return nRetStep;
        }
        public int Auto_PCBLoading(int nStep)       //PCB 로딩(3000 ~ 40000)
        {
            int nRetStep = nStep;
            switch (nStep)
            {
                case 3000:

                    break;
                case 3900:

                    break;
            }
            return nRetStep;
        }
    }
}