using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public class ReadyProcess
    {
        public int nTimeTick = 0;
        public int[] SensorSet = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] OrgOnGoing = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //private bool pauseTest = true;

        //Stopwatch stopwatch;// = Stopwatch.StartNew(); // 스톱워치 시작
        public ReadyProcess()
        {
        }
        /*
        public int HomeProcess(int nStep)                 //  원점(10000 ~ 20000)
        {
            const int UniqueNum = 10000;
            string szLog = "";
            uint duState = 0;

            bool bRtn = false;
            int nLensAxis = 0;
            bool m_bHomeProc = true;
            bool m_bHomeError = false;
            uint duRetCode = 0;
            double dAcc = 0.3;
            int i = 0;
            nStep -= UniqueNum;


            int nRetStep = nStep;
            switch (nStep)
            {
                case 0:
                    nRetStep = 10;

                    Thread.Sleep(1000);
                    break;
                case 10:
                    nRetStep = 50;
                    Thread.Sleep(1000);
                    break;
                case 50:
                    //실린더 동작
                    nRetStep = 100;
                    break;
                case 100:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_Z].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_Z);
                        break;
                    }

                    SensorSet[0] = (int)MotorControl.ePcbMotor.PCB_Z;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.NegEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.PCB_UNIT, SensorSet[0], (7 * -1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] PCB Z축 (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    //szLog = string.Format("[AUTO] PCB Z축 (-)Limit 위치 구동 성공 [STEP : {0}]", nStep);

                    szLog = $"[ORIGIN] PCB Z축 (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nRetStep = 200;
                    break;
                case 200:
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_Z].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.LENS_UNIT, (int)MotorControl.eLensMotor.LENS_Z);
                        break;
                    }

                    SensorSet[0] = (int)MotorControl.eLensMotor.LENS_Z;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.PosEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.LENS_UNIT, SensorSet[0], (7 * 1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LENS Z축 (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] LENS Z축 (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nTimeTick = Environment.TickCount;
                    nRetStep = 300;
                    break;
                case 300:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_Z].GetStopAxis() == true ||
                        Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_Z].GetNegaSensor() == true)
                    {
                        nTimeTick = Environment.TickCount;
                        nRetStep = 400;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)//else if ((Environment.TickCount - nTimeTick) / 1000.0 > 5)
                    {
                        szLog = $"[ORIGIN] PCB Z축 (-)Limit 위치 확인 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 400:
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_Z].GetStopAxis() == true ||
                        Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_Z].GetPosiSensor() == true)
                    {
                        nRetStep = 500;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)//else if ((Environment.TickCount - nTimeTick) / 1000.0 > 5)
                    {
                        szLog = $"[ORIGIN] LENS Z축 (+)Limit 위치 확인 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 500:
                    nRetStep = 550;
                    break;
                case 550:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_Y].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_Y);
                    }

                    SensorSet[0] = (int)MotorControl.ePcbMotor.PCB_Y;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.NegEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;
                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.PCB_UNIT, SensorSet[0], (5 * -1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] PCB Y축 (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] PCB Y축 (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 700;
                    break;
                case 700:
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_Y].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.LENS_UNIT, (int)MotorControl.eLensMotor.LENS_Y);
                    }

                    SensorSet[0] = (int)MotorControl.eLensMotor.LENS_Y;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.PosEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;
                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.LENS_UNIT, SensorSet[0], (5 * 1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LENS Y축 (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] LENS Y축 (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 800;
                    nTimeTick = Environment.TickCount;
                    break;
                case 800:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_Y].GetHomeSensor() == true)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_Y);
                    }

                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_Y].GetStopAxis() == true ||
                        Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_Y].GetNegaSensor() == true)
                    {
                        nRetStep = 900;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)//else if ((Environment.TickCount - nTimeTick) / 1000.0 > 5)
                    {
                        szLog = $"[ORIGIN] PCB Y축 (-)Limit 위치 확인 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 900:
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_Y].GetHomeSensor() == true)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.LENS_UNIT, (int)MotorControl.eLensMotor.LENS_Y);
                    }

                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_Y].GetStopAxis() == true ||
                        Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_Y].GetPosiSensor() == true)
                    {
                        nRetStep = 950;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)//else if ((Environment.TickCount - nTimeTick) / 1000.0 > 5)
                    {
                        szLog = $"[ORIGIN] LENS Y축 (+)Limit 위치 확인 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    nTimeTick = Environment.TickCount;

                    break;
                case 950:
                    nRetStep = 1000;
                    break;
                case 1000:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_X].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_X);
                    }

                    SensorSet[0] = (int)MotorControl.ePcbMotor.PCB_X;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.NegEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.PCB_UNIT, SensorSet[0], (20 * -1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] PCB X축 (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] PCB X축 (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 1100;
                    break;
                case 1100:
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_X].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.LENS_UNIT, (int)MotorControl.eLensMotor.LENS_X);
                    }

                    SensorSet[0] = (int)MotorControl.eLensMotor.LENS_X;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.PosEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.LENS_UNIT, SensorSet[0], (20 * 1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LENS X축 (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] LENS X축 (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nTimeTick = Environment.TickCount;
                    nRetStep = 1150;
                    break;
                case 1150:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_X].GetHomeSensor() == true)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_X);
                    }
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_X].GetStopAxis() == true ||
                        Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_X].GetNegaSensor() == true)
                    {
                        nRetStep = 1200;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000) //else if ((Environment.TickCount - nTimeTick) / 1000.0 > 5)
                    {
                        szLog = $"[ORIGIN] PCB X축 (-)Limit 위치 확인 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1200:
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_X].GetHomeSensor() == true)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.LENS_UNIT, (int)MotorControl.eLensMotor.LENS_X);
                    }
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_X].GetStopAxis() == true ||
                        Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_X].GetPosiSensor() == true)
                    {
                        nRetStep = 1250;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000) //else if ((Environment.TickCount - nTimeTick) / 1000.0 > 5)
                    {
                        szLog = $"[ORIGIN] LENS X축 (+)Limit 위치 확인 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1250:
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TX].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.LENS_UNIT, (int)MotorControl.eLensMotor.LENS_TX);
                    }
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TY].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.LENS_UNIT, (int)MotorControl.eLensMotor.LENS_TY);
                    }

                    SensorSet[0] = (int)MotorControl.eLensMotor.LENS_TX;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.PosEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.LENS_UNIT, SensorSet[0], (5 * 1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);

                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LENS TX축 (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] LENS TX축 (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    SensorSet[0] = (int)MotorControl.eLensMotor.LENS_TY;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.PosEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.LENS_UNIT, SensorSet[0], (5 * 1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LENS TY축 (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] LENS TY축 (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nTimeTick = Environment.TickCount;
                    nRetStep = 1300;
                    break;

                case 1300:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TX].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_TX);
                    }
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TY].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_TY);
                    }

                    SensorSet[0] = (int)MotorControl.ePcbMotor.PCB_TX;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.PosEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.PCB_UNIT, SensorSet[0], (5 * 1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] PCB TX축 (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] PCB TX축 (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    SensorSet[0] = (int)MotorControl.ePcbMotor.PCB_TY;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.PosEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.PCB_UNIT, SensorSet[0], (5 * 1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] PCB TY축 (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] PCB TY축 (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nTimeTick = Environment.TickCount;
                    nRetStep = 1350;
                    break;
                case 1350:
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TX].GetStopAxis() == true &&
                       Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TX].GetPosiSensor() == true &&
                       Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TY].GetStopAxis() == true &&
                       Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TY].GetPosiSensor() == true)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.LENS_UNIT, (int)MotorControl.eLensMotor.LENS_TX);
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.LENS_UNIT, (int)MotorControl.eLensMotor.LENS_TY);

                        szLog = $"[ORIGIN] LENS TX,TY축 (+)Limit 위치 확인 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1400;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] LENS TX,TY축 (+)Limit 위치 확인 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1400:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TX].GetStopAxis() == true &&
                        Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TX].GetPosiSensor() == true &&
                        Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TY].GetStopAxis() == true &&
                        Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TY].GetPosiSensor() == true)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_TX);
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_TY);

                        szLog = $"[ORIGIN] PCB TX,TY축 (+)Limit 위치 확인 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1450;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000) //else if ((Environment.TickCount - nTimeTick) / 1000.0 > 5)
                    {
                        szLog = $"[ORIGIN] PCB TX,TY축 (+)Limit 위치 확인 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 1450:
                    SensorSet[0] = (int)MotorControl.eLensMotor.LENS_TX;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.HomeSensor;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_HIGH_LEVEL;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.LENS_UNIT, SensorSet[0], (3 * -1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LENS TX축 Home 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] LENS TX축 Home 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    SensorSet[0] = (int)MotorControl.eLensMotor.LENS_TY;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.HomeSensor;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_HIGH_LEVEL;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.LENS_UNIT, SensorSet[0], (3 * -1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] LENS TY축 Home 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] LENS TY축 Home 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nTimeTick = Environment.TickCount;
                    nRetStep = 1500;

                    break;
                case 1500:
                    SensorSet[0] = (int)MotorControl.ePcbMotor.PCB_TX;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.HomeSensor;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_HIGH_LEVEL;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.PCB_UNIT, SensorSet[0], (3 * -1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] PCB TX축 Home 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] PCB TX축 Home 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    //
                    //
                    SensorSet[0] = (int)MotorControl.ePcbMotor.PCB_TY;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.HomeSensor;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_HIGH_LEVEL;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.PCB_UNIT, SensorSet[0], (3 * -1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] PCB TY축 Home 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] PCB TY축 Home 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nTimeTick = Environment.TickCount;
                    nRetStep = 1600;
                    break;
                case 1600:
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TX].GetStopAxis() == false &&
                       Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TX].GetHomeSensor() == true)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.LENS_UNIT, (int)MotorControl.eLensMotor.LENS_TX);
                    }
                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TY].GetStopAxis() == false &&
                         Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TY].GetHomeSensor() == true)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.LENS_UNIT, (int)MotorControl.eLensMotor.LENS_TY);
                    }

                    if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TX].GetHomeSensor() == true &&
                        Globalo.motorControl.LensMotorAxis[(int)MotorControl.eLensMotor.LENS_TY].GetHomeSensor() == true)
                    {
                        szLog = $"[ORIGIN] LENS TX,TY축 HOME 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1700;
                        break;
                    }

                    //if (Globalo.motorControl.LensMotorAxis[(int)MotorControl.ePcbMotor.PCB_TX].GetNegaSensor() == true &&
                    //    Globalo.motorControl.LensMotorAxis[(int)MotorControl.ePcbMotor.PCB_TY].GetNegaSensor() == true)
                    //{
                    //    szLog = $"[ORIGIN] PCB TX,TY축 (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                    //    Globalo.LogPrint("ManualControl", szLog);
                    //    nRetStep = 1700;
                    //    break;
                    //}

                    if (Environment.TickCount - nTimeTick > 30000) //else if ((Environment.TickCount - nTimeTick) / 1000.0 > 5)
                    {
                        szLog = $"[ORIGIN] LENS TX,TY축 HOME 위치 이동 시간초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 1700:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TX].GetStopAxis() == false &&
                        Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TX].GetHomeSensor() == true)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_TX);
                    }
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TY].GetStopAxis() == false &&
                         Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TY].GetHomeSensor() == true)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_TY);
                    }

                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TX].GetHomeSensor() == true &&
                        Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TY].GetHomeSensor() == true)
                    {
                        szLog = $"[ORIGIN] PCB TX,TY축 HOME 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1800;
                        break;
                    }

                    //if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TX].GetNegaSensor() == true &&
                    //    Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TY].GetNegaSensor() == true)
                    //{
                    //    szLog = $"[ORIGIN] PCB TX,TY축 (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                    //    Globalo.LogPrint("ManualControl", szLog);
                    //    nRetStep = 1700;
                    //    break;
                    //}

                    if (Environment.TickCount - nTimeTick > 30000) //else if ((Environment.TickCount - nTimeTick) / 1000.0 > 5)
                    {
                        szLog = $"[ORIGIN] PCB TX,TY축 HOME 위치 이동 시간초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 1800:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TH].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_TH);
                    }

                    SensorSet[0] = (int)MotorControl.ePcbMotor.PCB_TH;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.PosEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.PCB_UNIT, SensorSet[0], (3 * 1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] PCB TH축 (+)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] PCB TH축 (+)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nTimeTick = Environment.TickCount;
                    nRetStep = 1850;
                    break;
                case 1850:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TH].GetStopAxis() == true &&
                        Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TH].GetPosiSensor() == true)
                    {
                        nRetStep = 1900;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] PCB TH축 (+)Limit 위치 확인 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1900:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TH].GetStopAxis() == false)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_TH);
                        break;
                    }

                    SensorSet[0] = (int)MotorControl.ePcbMotor.PCB_TH;
                    SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.NegEndLimit;
                    SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    bRtn = Globalo.motorControl.MoveAxisLimit(MotorControl.eUnit.PCB_UNIT, SensorSet[0], (3 * -1), dAcc, SensorSet[1], SensorSet[2], SensorSet[3]);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] PCB TH축 (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[ORIGIN] PCB TH축 (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nTimeTick = Environment.TickCount;

                    nRetStep = 1950;
                    break;
                case 1950:
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TH].GetStopAxis() == false &&
                        Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TH].GetHomeSensor() == true)
                    {
                        Globalo.motorControl.StopAxis(MotorControl.eUnit.PCB_UNIT, (int)MotorControl.ePcbMotor.PCB_TH);

                        szLog = $"[ORIGIN] PCB TH축 HOME 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2000;
                        break;
                    }
                    if (Globalo.motorControl.PcbMotorAxis[(int)MotorControl.ePcbMotor.PCB_TH].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] PCB TH축 (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2000;
                        break;
                    }
                    if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] PCB TH축 HOME 위치 확인 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2000:
                    nRetStep = 2100;
                    break;
                case 2100:
                    nRetStep = 2200;
                    break;
                case 2200:
                    nRetStep = 5000;
                    break;

                case 5000:
                    szLog = $"[ORIGIN] 전체 HOME 센서 위치 이동 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 5100;
                    break;
                case 5100:

                    bRtn = true;
                    for (i = 0; i < (int)MotorControl.ePcbMotor.MAX_PCB_MOTOR_COUNT; i++)
                    {
                        OrgOnGoing[i] = 0;
                        Globalo.motorControl.PcbMotorAxis[i].bOrgState = false;

                        //Home Method Setting
                        uint duZPhaseUse = 0;
                        double dHomeClrTime = 2000.0;
                        double dHomeOffset = 0.0;

                        //++ 지정한 축의 원점검색 방법을 변경합니다.
                        duRetCode = CAXM.AxmHomeSetMethod(
                            i,
                            (int)Globalo.motorControl.MOTOR_HOME_DIR[i],
                            (uint)Globalo.motorControl.MOTOR_HOME_SENSOR[i],
                            duZPhaseUse,
                            dHomeClrTime,
                            dHomeOffset);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motorControl.PcbMotorAxis[i].Name} AxmHomeSetMethod Fail [STEP : {nStep}]";
                            Globalo.LogPrint("PcbProcess", szLog);
                        }


                        duRetCode = CAXM.AxmHomeSetVel(i,
                            Globalo.motorControl.OrgFirstVel[i], Globalo.motorControl.OrgSecondVel[i],
                            Globalo.motorControl.OrgThirdVel[i], Globalo.motorControl.OrgLastVel[i],
                            Globalo.motorControl.OrgAccFirst[i], Globalo.motorControl.OrgAccSecond[i]);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motorControl.PcbMotorAxis[i].Name} AxmHomeSetVel Fail [STEP : {nStep}]";
                            Globalo.LogPrint("PcbProcess", szLog);
                        }
                        //MessageBox.Show(String.Format("AxmHomeSetMethod return error[Code:{0:d}]", duRetCode));
                        //Home Velocity Setting

                        //Home Method Setting
                    }
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] 원점 설정 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("PcbProcess", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    bRtn = true;
                    for (i = 0; i < (int)MotorControl.ePcbMotor.MAX_PCB_MOTOR_COUNT; i++) 
                    {
                        duRetCode = CAXM.AxmHomeSetStart(i);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motorControl.PcbMotorAxis[i].Name} AxmHomeSetStart Fail [STEP : {nStep}]";
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
                    nRetStep = 5200;
                    break;

                case 5200:
                    bRtn = true;
                    for (i = 0; i < (int)MotorControl.eLensMotor.MAX_LENS_MOTOR_COUNT; i++)
                    {
                        nLensAxis = i + (int)MotorControl.ePcbMotor.MAX_PCB_MOTOR_COUNT;
                        OrgOnGoing[nLensAxis] = 0;
                        Globalo.motorControl.LensMotorAxis[i].bOrgState = false;

                        //Home Method Setting
                        uint duZPhaseUse = 0;
                        double dHomeClrTime = 2000.0;
                        double dHomeOffset = 0.0;

                        //++ 지정한 축의 원점검색 방법을 변경합니다.
                        duRetCode = CAXM.AxmHomeSetMethod(
                            nLensAxis,
                            (int)Globalo.motorControl.MOTOR_HOME_DIR[nLensAxis],
                            (uint)Globalo.motorControl.MOTOR_HOME_SENSOR[nLensAxis],
                            duZPhaseUse,
                            dHomeClrTime,
                            dHomeOffset);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motorControl.LensMotorAxis[i].Name} AxmHomeSetMethod Fail [STEP : {nStep}]";
                            Globalo.LogPrint("PcbProcess", szLog);
                        }


                        duRetCode = CAXM.AxmHomeSetVel(nLensAxis,
                            Globalo.motorControl.OrgFirstVel[nLensAxis], Globalo.motorControl.OrgSecondVel[nLensAxis],
                            Globalo.motorControl.OrgThirdVel[nLensAxis], Globalo.motorControl.OrgLastVel[nLensAxis],
                            Globalo.motorControl.OrgAccFirst[nLensAxis], Globalo.motorControl.OrgAccSecond[nLensAxis]);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motorControl.LensMotorAxis[i].Name} AxmHomeSetVel Fail [STEP : {nStep}]";
                            Globalo.LogPrint("PcbProcess", szLog);
                        }
                    }
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] 원점 설정 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("PcbProcess", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    bRtn = true;
                    for (i = 0; i < (int)MotorControl.eLensMotor.MAX_LENS_MOTOR_COUNT; i++)
                    {
                        nLensAxis = i + (int)MotorControl.ePcbMotor.MAX_PCB_MOTOR_COUNT;
                        duRetCode = CAXM.AxmHomeSetStart(nLensAxis);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motorControl.LensMotorAxis[i].Name} AxmHomeSetStart Fail [STEP : {nStep}]";
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

                    nRetStep = 5300;
                    break;
                case 5300:
                    m_bHomeProc = true;
                    m_bHomeError = false;
                    for (i = 0; i < (int)MotorControl.ePcbMotor.MAX_PCB_MOTOR_COUNT; i++)
                    {
                        CAXM.AxmHomeGetResult(i, ref duState);
                        if (duState == (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS)
                        {
                            //원점 완료
                            Globalo.motorControl.PcbMotorAxis[i].bOrgState = true;
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
                            szLog = $"[ORIGIN] {Globalo.motorControl.PcbMotorAxis[i].Name} HOME 동작 ERROR [STEP : {nStep}]";
                            Globalo.LogPrint("PcbProcess", szLog);
                        }
                        //Globalo.motorControl.TranslateHomeResult(duState);
                    }
                    if (m_bHomeError == true)
                    {
                        nRetStep *= -1;
                        break;
                    }
                    if (m_bHomeProc == true)
                    {
                        nRetStep = 5350;
                    }
                    break;
                case 5350:
                    m_bHomeProc = true;
                    m_bHomeError = false;
                    for (i = 0; i < (int)MotorControl.eLensMotor.MAX_LENS_MOTOR_COUNT; i++)
                    {
                        nLensAxis = i + (int)MotorControl.ePcbMotor.MAX_PCB_MOTOR_COUNT;
                        CAXM.AxmHomeGetResult(nLensAxis, ref duState);
                        if (duState == (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS)
                        {
                            //원점 완료
                            Globalo.motorControl.LensMotorAxis[i].bOrgState = true;
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
                            szLog = $"[ORIGIN] {Globalo.motorControl.LensMotorAxis[i].Name} HOME 동작 ERROR [STEP : {nStep}]";
                            Globalo.LogPrint("PcbProcess", szLog);
                        }
                        //Globalo.motorControl.TranslateHomeResult(duState);
                    }
                    if (m_bHomeError == true)
                    {
                        nRetStep *= -1;
                        break;
                    }
                    if (m_bHomeProc == true)
                    {
                        nRetStep = 5400;
                    }

                    break;
                case 5400:
                    szLog = $"[ORIGIN] 전체 원점 위치 이동 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_INFO);
                    nRetStep = 9000;
                    //// uint duState = 0;
                    // uint duStepMain = 0, duStepSub = 0;
                    // //++ 지정한 축의 원점신호의 상태를 확인합니다.
                    // duRetCode = CAXM.AxmHomeReadSignal(0, ref duState);
                    // //if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) chkHomeState.Checked = Convert.ToBoolean(duState);

                    // //++ 지정한 축의 원점신호의 상태를 확인합니다.
                    // CAXM.AxmHomeGetResult(0, ref duState);
                    // //if (m_duOldResult != duState)
                    // {
                    //     //labelHomeSearch.Text = TranslateHomeResult(duState);
                    //     //Globalo.motorControl.TranslateHomeResult(duState);
                    //     //m_duOldResult = duState;
                    // }
                    // //++ 지정한 축의 원점검색 결과를 확인합니다
                    // duRetCode = CAXM.AxmHomeGetRate(0, ref duStepMain, ref duStepSub);
                    // if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    // {
                    //     //labelHomeStepMain32.Text = Convert.ToString(duStepMain);
                    //     //labelHomeStepSub33.Text = Convert.ToString(duStepSub);
                    // }
                    // //++ 지정한 축의 원점검색 진행율을 확인합니다.
                    // duRetCode = CAXM.AxmHomeGetRate(0, ref duStepMain, ref duStepSub);
                    // if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    // {
                    //     //prgHomeRate.Value = (int)duStepSub;
                    // }
                    break;
                case 9000:

                    //원점 복귀 완료
                    nRetStep = 10000;
                    break;
                default:
                    //[ORIGIN] STEP ERR
                    nRetStep = -10000;
                    break;
            }
            return nRetStep + UniqueNum;
        }
        public int AutoReady(int nStep, CancellationToken token)					//  운전준비(20000 ~ 30000)
        {
            string szLog = "";
            const int UniqueNum = 20000;
            int nRetStep = nStep;
            switch (nStep)
            {
                case UniqueNum:

                    szLog = $"[READY] 운전준비 TEST 1 [STEP : {nStep}]";
                    Globalo.LogPrint("ReadyPrecess", szLog);
                    nRetStep = 20050;
                    stopwatch = Stopwatch.StartNew();
                    break;
                case 20050:
                    
                    
                    if (stopwatch.ElapsedMilliseconds > 500) // 1초 경과 확인
                    {
                        szLog = $"[READY] 운전준비 TEST 2 [STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nRetStep = 20100;
                        stopwatch = Stopwatch.StartNew();
                    }
                    break;
                case 20100:
                    if (pauseTest)
                    {
                        szLog = $"[READY] 운전준비 TEST 3 일시정지[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog, Globalo.eMessageName.M_WARNING);
                        pauseTest = false;
                        nRetStep = -20150;
                        break;

                    }
                    
                    if (stopwatch.ElapsedMilliseconds > 500) // 1초 경과 확인
                    {
                        szLog = $"[READY] 운전준비 TEST 3 [STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nRetStep = 20150;
                        stopwatch = Stopwatch.StartNew();
                    }
                    break;
                case 20150:
                    
                    if (stopwatch.ElapsedMilliseconds > 500) // 1초 경과 확인
                    {
                        szLog = $"[READY] 운전준비 TEST 4 [STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nRetStep = 20200;
                        stopwatch = Stopwatch.StartNew();
                    }
                    
                    break;
                case 20200:
                    

                    if (stopwatch.ElapsedMilliseconds > 500) // 1초 경과 확인
                    {
                        szLog = $"[READY] 운전준비 TEST 5 [STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nRetStep = 29100;
                    }
                    break;
                case 29100:
                    for (int i = 0; i < 50; i++)  // 긴 반복문
                    {
                        if (token.IsCancellationRequested)  // 중간에 취소 요청 확인
                        {
                            Console.WriteLine($"운전준비 token.IsCancellationRequested");
                            break;
                        }
                           
                        Console.WriteLine($"운전준비 Processing {i}");
                        //Thread.Sleep(300); // 작업 시간 가정
                    }
                    ProgramState.CurrentState = OperationState.PreparationComplete;
                    szLog = $"[READY] 운전준비 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("PcbPrecess", szLog);
                    nRetStep = 30000;
                    break;
            }
            return nRetStep;
        }
        */
    }
}
