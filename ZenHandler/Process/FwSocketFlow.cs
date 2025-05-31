using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public class FwSocketFlow
    {
        public int nTimeTick = 0;
        public int[] nSocketTimeTick = { 0, 0, 0, 0 };

        private int[] socketStateA = { -1, -1, -1, -1 };     // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출
        private int[] socketState_B = { -1, -1, -1, -1 };    // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출
        private int[] socketStateC = { -1, -1, -1, -1 };     // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출
        private int[] socketState_D = { -1, -1, -1, -1 };    // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출

        private string[] socketName = { "LT SOCKET", "RT SOCKET", "BL SOCKET", "BR SOCKET" };
        public FwSocketFlow()
        {

        }
        #region [운전 준비]
        
        public int AutoReady(int nStep)                 //  운전준비(2000 ~ 3000)
        {
            string szLog = "";
            bool bRtn = false;
            int i = 0;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 2000:
                    nRetStep = 2100;
                    break;
                case 2100:
                    nRetStep = 2120;
                    break;
                case 2120:
                    //ALL SOCKET CONTACT 상승
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.socketFwMachine.MultiContactUp(i, true) == true)
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT UP MOTION [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 2140;

                            nTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT UP MOTION FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        Thread.Sleep(100);
                    }
                    break;
                case 2140:
                    //ALL SOCKET ROTATION 상승
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.socketFwMachine.MultiFlipperUp(i, true) == true)
                        {
                            szLog = $"[ORG] {socketName[i]} FLIPPER UP MOTION [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nRetStep = 2160;

                            nTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[ORG] {socketName[i]} FLIPPER UP MOTION FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        Thread.Sleep(100);
                    }


                    break;
                case 2160:
                    //ALL SOCKET CONTACT 상승 확인
                    bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(0, true);
                    if (bRtn)
                    {
                        szLog = $"[ORG] ALL SOCKET CONTACT UP CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2180;
                        nTimeTick = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[ORG] ALL SOCKET CONTACT UP CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }
                    break;
                case 2180:
                    //ALL SOCKET ROTATION 상승 확인
                    break;
                case 2200:
                    //ALL SOCKET CONTACT 후진
                    if (Globalo.motionManager.socketFwMachine.MultiContactFor(0, false) == true)
                    {
                        szLog = $"[ORG] ALL SOCKET CONTACT BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1020;

                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[ORG] ALL SOCKET CONTACT BACK MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2220:
                    //ALL SOCKET CONTACT 후진 확인
                    break;
                case 2240:

                    break;
                case 2260:
                    //ALL SOCKET ROTATION HOME FLIP
                    if (Globalo.motionManager.socketFwMachine.MultiFlipperRotate(0, false) == true)
                    {
                        szLog = $"[ORG] ALL SOCKET FLIPPER HOME MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1030;

                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[ORG] ALL SOCKET FLIPPER HOME MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2280:
                    //ALL SOCKET ROTATION HOME FLIP 확인
                    break;
                case 2300:

                    break;
                case 2400:

                    break;
                case 2500:

                    break;
                    
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

        #region [HomeProcess]

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
            int i = 0;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 1000:
                    nRetStep = 1100;
                    break;
                case 1100:
                    nRetStep = 1200;
                    break;
                case 1200:
                    nRetStep = 1800;
                    break;
                case 1800:
                    nRetStep = 1900;
                    break;
                case 1900:

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
        #endregion
    }
}
