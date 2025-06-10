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

        #region [Auto_Waiting]
        public int Auto_Waiting(int nStep)
        {
            int i = 0;
            string szLog = "";
            bool result = false;
            int nRetStep = nStep;

            switch (nStep)
            {
                case 3000:

                    break;

            }

            return nRetStep;
        }
        #endregion

        #region [운전 준비]

        public int AutoReady(int nStep)                 //  운전준비(2000 ~ 3000)
        {
            string szLog = "";
            bool bRtn = false;
            int i = 0;
            int j = 0;
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
                        }
                        else
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT UP MOTION FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        Thread.Sleep(50);
                    }

                    nRetStep = 2140;
                    nTimeTick = Environment.TickCount;
                    break;
                case 2140:
                    //ALL SOCKET ROTATION 상승
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.socketFwMachine.MultiFlipperUp(i, true) == true)
                        {
                            szLog = $"[ORG] {socketName[i]} FLIPPER UP MOTION [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        else
                        {
                            szLog = $"[ORG] {socketName[i]} FLIPPER UP MOTION FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        Thread.Sleep(50);
                    }
                    nRetStep = 2160;
                    nTimeTick = Environment.TickCount;
                    break;
                case 2160:
                    //ALL SOCKET CONTACT 상승 확인
                    for (i = 0; i < 4; i++)
                    {
                        bRtn = Globalo.motionManager.socketFwMachine.GetMultiContactUp(i, true);
                        if (bRtn)
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT UP CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT UP CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        Thread.Sleep(50);
                    }
                    nTimeTick = Environment.TickCount;
                    nRetStep = 2180;
                    break;
                case 2180:
                    //ALL SOCKET ROTATION 상승 확인
                    for (i = 0; i < 4; i++)
                    {
                        bRtn = Globalo.motionManager.socketFwMachine.GetMultiFlipperUp(i, true);
                        if (bRtn)
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT UP CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            
                            
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT UP CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        Thread.Sleep(50);
                    }
                    nRetStep = 2200;
                    break;
                case 2200:
                    //ALL SOCKET CONTACT 후진
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.socketFwMachine.MultiContactFor(i, false) == true)
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT BACK MOTION [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        else
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT BACK MOTION FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        Thread.Sleep(50);
                    }
                    nTimeTick = Environment.TickCount;
                    nRetStep = 2220;
                    break;

                case 2220:
                    //ALL SOCKET CONTACT 후진 확인
                    for (i = 0; i < 4; i++)
                    {
                        bRtn = Globalo.motionManager.socketFwMachine.GetMultiContactFor(i, false);
                        if (bRtn)
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT BACK CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            
                            nTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT BACK CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        Thread.Sleep(50);
                    }
                    nRetStep = 2240;
                    break;
                case 2240:
                    //제품없으면 언그립

                    for (i = 0; i < 4; i++)
                    {
                        for (j = 0; j < 4; j++)
                        {
                            bRtn = Globalo.motionManager.socketFwMachine.GetIsProductInSocket(i, j, true);
                            if (bRtn == false)
                            {
                                //언그립
                                bRtn = Globalo.motionManager.socketFwMachine.FlipperGrip(i, j, false);
                                if (bRtn)
                                {
                                    szLog = $"[ORG] {socketName[i]} #{j+1} Flipper UnGrip [STEP : {nStep}]";
                                    Globalo.LogPrint("ManualControl", szLog);
                                }
                                else
                                {
                                    szLog = $"[ORG] {socketName[i]} Flipper UnGrip Fail [STEP : {nStep}]";
                                    Globalo.LogPrint("ManualControl", szLog);
                                    nStep *= -1;
                                    break;
                                }
                                Thread.Sleep(50);
                            }
                            Thread.Sleep(10);
                        }
                            
                    }
                        
                    nRetStep = 2260;
                    break;
                case 2260:
                    //제품없으면  원래대로 턴하기
                    for (i = 0; i < 4; i++)
                    {
                        for (j = 0; j < 4; j++)
                        {
                            bRtn = Globalo.motionManager.socketFwMachine.GetIsProductInSocket(i, j, true);
                            if (bRtn == false)
                            {
                                //언그립
                                bRtn = Globalo.motionManager.socketFwMachine.FlipperTurn(i, j, false);
                                if (bRtn)
                                {
                                    szLog = $"[ORG] {socketName[i]} #{j + 1} Flipper Rotate Home [STEP : {nStep}]";
                                    Globalo.LogPrint("ManualControl", szLog);
                                }
                                else
                                {
                                    szLog = $"[ORG] {socketName[i]} Flipper Rotate Fail [STEP : {nStep}]";
                                    Globalo.LogPrint("ManualControl", szLog);
                                    nStep *= -1;
                                    break;
                                }
                                Thread.Sleep(50);
                            }
                            Thread.Sleep(10);
                        }
                    }
                    nRetStep = 2280;
                    break;
                case 2280:
                    nRetStep = 2300;
                    break;
                case 2300:
                    for (i = 0; i < 4; i++)
                    {
                        bRtn = Globalo.motionManager.socketFwMachine.GetIsProductInSocket(0, i, true);
                        if (bRtn)
                        {
                            //제품 감지되는데 상태가 Blank 이면 알람
                            if (Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[0][i].State == Machine.FwProductState.Blank)
                            {
                                szLog = $"[READY] LT SOCKET #{i+1} Product 상태 확인바랍니다.[STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);

                                nRetStep *= -1;
                                break;
                            }
                        }
                    }
                    for (i = 0; i < 4; i++)
                    {
                        bRtn = Globalo.motionManager.socketFwMachine.GetIsProductInSocket(1, i, true);
                        if (bRtn)
                        {
                            //제품 감지되는데 상태가 Blank 이면 알람
                            if (Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[1][i].State == Machine.FwProductState.Blank)
                            {
                                szLog = $"[READY] RT SOCKET #{i + 1} Product 상태 확인바랍니다.[STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);

                                nRetStep *= -1;
                                break;
                            }
                        }
                    }
                    for (i = 0; i < 4; i++)
                    {
                        bRtn = Globalo.motionManager.socketFwMachine.GetIsProductInSocket(2, i, true);
                        if (bRtn)
                        {
                            //제품 감지되는데 상태가 Blank 이면 알람
                            if (Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[2][i].State == Machine.FwProductState.Blank)
                            {
                                szLog = $"[READY] BL SOCKET #{i + 1} Product 상태 확인바랍니다.[STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);

                                nRetStep *= -1;
                                break;
                            }
                        }
                    }
                    for (i = 0; i < 4; i++)
                    {
                        bRtn = Globalo.motionManager.socketFwMachine.GetIsProductInSocket(3, i, true);
                        if (bRtn)
                        {
                            //제품 감지되는데 상태가 Blank 이면 알람
                            if (Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[3][i].State == Machine.FwProductState.Blank)
                            {
                                szLog = $"[READY] BR SOCKET #{i + 1} Product 상태 확인바랍니다.[STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);

                                nRetStep *= -1;
                                break;
                            }
                        }
                    }
                    nRetStep = 2400;
                    break;
                case 2400:
                    

                    //검사 완료이고, 그립상태면 , ->상승 -> turn ->하강 -> UnGrip -> 상승

                    //Machine.FwProductState.Good
                    //Machine.FwProductState.NG

                    //Good , Ng 상태면 Turn 상태로 보자.

                    nRetStep = 2420;
                    break;

                case 2420:
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.socketFwMachine.MultiFlipperUp(i, false) == true)
                        {
                            szLog = $"[ORG] {socketName[i]} FLIPPER DOWN MOTION [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        else
                        {
                            szLog = $"[ORG] {socketName[i]} FLIPPER DOWN MOTION FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        Thread.Sleep(50);
                    }
                    nTimeTick = Environment.TickCount;
                    nRetStep = 2500;
                    break;
                case 2500:
                    for (i = 0; i < 4; i++)
                    {
                        bRtn = Globalo.motionManager.socketFwMachine.GetMultiFlipperUp(i, false);
                        if (bRtn)
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT DOWN CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);


                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[ORG] {socketName[i]} CONTACT DOWN CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        Thread.Sleep(50);
                    }
                    nRetStep = 2600;
                    break;
                case 2600:
                    nRetStep = 2900;
                    break;
                case 2900:

                    //검사 시퀀스
                    //소켓에 제품 투입 -> Grip -> 컨택 -> fw검사 - > 컨택 빠지고 -> 로테이트 상승 -> 회전 -> 하강 - > UnGrip


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
                    Globalo.motionManager.socketFwMachine.RunState = OperationState.OriginDone;
                    szLog = $"[ORIGIN] FW SOCKET UNIT 전체 원점 위치 이동 완료 [STEP : {nStep}]";
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
    }
}
