using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public enum FwSocketState
    {
        Wait = 0,
        LoadReq,        // 공급 요청
        UnLoadReq,      //배출 요청 (양품 + 불량 섞여 있을 수도 있다.)
        Testing,        //검사 전
    }
    public class FwSocketFlow
    {
        public int nTimeTick = 0;

        //private int[] socketStateA = { -1, -1, -1, -1 };     // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출
        //private int[] socketState_B = { -1, -1, -1, -1 };    // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출
        //private int[] socketStateC = { -1, -1, -1, -1 };     // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출
        //private int[] socketState_D = { -1, -1, -1, -1 };    // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출

        public int[] nSocketTimeTick = { 0, 0, 0, 0 };

        private TcpSocket.MessageWrapper fwEqipData;
        private TcpSocket.TesterData tData;
        private string[] socketName = { "LT SOCKET", "RT SOCKET", "BL SOCKET", "BR SOCKET" };


        private FwSocketState[] socketProcessState = new FwSocketState[4];
        private MotionControl.SocketReqArgs[] FlowSocketState = new MotionControl.SocketReqArgs[4];
        public FwSocketFlow()
        {
            int i = 0;
            fwEqipData = new TcpSocket.MessageWrapper();
            fwEqipData.Type = "FW";
            tData = new TcpSocket.TesterData();

            for (i = 0; i < 4; i++)
            {
                FlowSocketState[i] = new MotionControl.SocketReqArgs
                {
                    Index = i,
                    States = new int[] { -1, -1, -1, -1 },      //FW 4개
                    Barcode = new string[] { string.Empty, string.Empty, string.Empty, string.Empty }
                };
            }
        }
        public int Auto_Common_FwSocket(int nStep, int SocketIndex)
        {
            int i = 0;
            string szLog = "";
            bool result = false;
            bool bRtn = false;
            bool bEmptyChk = false;
            int nRetStep = nStep;
            const int SocketMaxCnt = 4;
            int FNum = SocketIndex;     //SocketIndex = 0 or 1
            switch (nStep)
            {
                case 100:
                    if (Globalo.motionManager.socketFwMachine.IsTesting[FNum] == false)
                    {
                        break;
                    }
                    nRetStep = 120;
                    break;
                case 120:
                    if (socketProcessState[FNum] == FwSocketState.UnLoadReq)        //전부가 검사가 끝났을때 (양품 + NG)
                    {
                        Console.WriteLine("Good Unload Req");
                        nRetStep = 300;
                        break;
                    }
                    if (socketProcessState[FNum] == FwSocketState.Testing)        //검사 진행
                    {
                        Console.WriteLine("Test Run");
                        nRetStep = 400;
                        break;
                    }
                    if (socketProcessState[FNum] == FwSocketState.LoadReq)   //전부다 비었을때만, 공급 요청
                    {
                        Console.WriteLine("Load Req");
                        nRetStep = 200;
                        break;
                    }

                    Globalo.motionManager.socketFwMachine.IsTesting[FNum] = false;  //다시 Waiting로 가기위해
                    nRetStep = 100;
                    break;
                //--------------------------------------------------------------------------------------------------------------------------
                //
                //
                //  공급 요청
                //
                //
                //--------------------------------------------------------------------------------------------------------------------------
                case 200:
                    nRetStep = 205;
                    break;
                case 205:
                    if (Globalo.motionManager.socketFwMachine.MultiContactUp(FNum, true) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT UP MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 206;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT UP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 206:
                    if (Globalo.motionManager.socketFwMachine.MultiFlipperGrip(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UNGRIP MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 210;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT UP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 210:
                    bRtn = Globalo.motionManager.socketFwMachine.GetMultiContactUp(FNum, true);
                    if (bRtn)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT UP CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 211;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT UP CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }
                    break;
                case 211:
                    bRtn = Globalo.motionManager.socketFwMachine.GetMultiFlipperGrip(FNum, false);
                    if (bRtn)
                    {
                        szLog = $"[AUTO] {socketName[i]} FLIPPER UNGRIP CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 212;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[i]} FLIPPER UNGRIP CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }
                    break;
                case 212:
                    if (Globalo.motionManager.socketFwMachine.MultiContactFor(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 215;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT BACK MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 215:
                    if (Globalo.motionManager.socketFwMachine.MultiFlipperUp(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER DOWN MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 216;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER DOWN MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 216:
                    if (Globalo.motionManager.socketFwMachine.GetMultiFlipperUp(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER DOWN CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 220;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER DOWN CEHCK FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 220:
                    bRtn = Globalo.motionManager.socketFwMachine.GetMultiContactFor(FNum, false);
                    if (bRtn)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT BACK CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 230;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT BACK CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }
                    
                    break;
                case 230:
                    nRetStep = 240;
                    break;
                case 240:
                    bEmptyChk = true;
                    for (i = 0; i < SocketMaxCnt; i++)
                    {
                        if (Globalo.motionManager.socketFwMachine.GetIsProductInSocket(FNum, i, true) == false)
                        {
                            //둘다 비어야 공급 요청 가능
                            FlowSocketState[FNum].States[i] = 1;
                        }
                        else
                        {
                            bEmptyChk = false;
                            FlowSocketState[FNum].States[i] = -1;
                        }
                    }
                    if (bEmptyChk)
                    {
                        Globalo.motionManager.InitSocketDone(FNum);             //공급요청 변수 초기화 -1로 초기화
                        Globalo.motionManager.socketFwMachine.RaiseProductCall(FlowSocketState[FNum]);        //공급 요청 초기화, Auto_Waiting

                        szLog = $"[AUTO] {socketName[FNum]} LOAD REQ [{string.Join(", ", FlowSocketState[FNum].States)}][STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 245;

                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} EMPTY CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 245:
                    //공급 완료 대기
                    if (Globalo.motionManager.GetSocketDone(FNum) == 0)
                    {
                        //공급 완료
                        MotionControl.SocketReqArgs group = Globalo.motionManager.GetSocketReq(FNum);    //소켓별 공급 상태 받기

                        bool bErrChk = false;
                        for (i = 0; i < SocketMaxCnt; i++)
                        {

                            //공급한 개수만큼 인식이 되는지 체크
                            if (group.States[i] == 0)       //if (socketStates[ANum, i] == 1)
                            {
                                if (Globalo.motionManager.socketFwMachine.GetIsProductInSocket(FNum, i, true) == false)
                                {
                                    //공급했는 소켓인데 제품이 없으면 알람
                                    Console.WriteLine($"#{i + 1} Socket Product Empty err");
                                    bErrChk = true;

                                    Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].BcrLot = string.Empty;
                                    Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].State = Machine.FwProductState.Blank;
                                    Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].specialData.Clear();
                                }
                                else
                                {
                                    Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].BcrLot = group.Barcode[i];
                                    Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].specialData = group.specialData[i].Select(item => item.DeepCopy()).ToList();
                                    Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].State = Machine.FwProductState.Testing;
                                }
                            }
                        }

                        if (bErrChk)
                        {
                            szLog = $"[AUTO] {socketName[FNum]} PRODUCT LOAD FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }

                        szLog = $"[AUTO] {socketName[FNum]} PRODUCT LOAD COMPLETE [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 250;
                    }
                    break;
                case 250:

                    nRetStep = 255;
                    break;
                case 255:

                    nRetStep = 260;
                    break;
                case 260:
                    if (Globalo.motionManager.socketFwMachine.MultiFlipperGrip(FNum, true) == true)
                    {
                        szLog = $"[ORG] {socketName[FNum]} FLIPPER GRIP MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 265;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[ORG] {socketName[FNum]} FLIPPER GRIP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 265:
                    bRtn = Globalo.motionManager.socketFwMachine.GetMultiFlipperGrip(FNum, true);
                    if (bRtn)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER GRIP CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 290;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER GRIP CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }
                    
                    break;
                case 290:
                    //제품 그립 -> 컨택 전진 -> 컨택 하강 - > 공급완료
                    Globalo.motionManager.socketFwMachine.IsTesting[FNum] = false;
                    nRetStep = 100;
                    break;
                //--------------------------------------------------------------------------------------------------------------------------
                //
                //
                //  배출 요청
                //
                //
                //--------------------------------------------------------------------------------------------------------------------------
                case 300:
                    nRetStep = 305;
                    break;
                case 305:
                    if (Globalo.motionManager.socketFwMachine.MultiContactUp(FNum, true) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT UP MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 306;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT UP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 306:
                    bRtn = Globalo.motionManager.socketFwMachine.GetMultiContactUp(FNum, true);
                    if (bRtn)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT UP CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 310;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT UP CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }
                    break;
                case 310:
                    if (Globalo.motionManager.socketFwMachine.MultiContactFor(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 312;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT BACK MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 312:
                    bRtn = Globalo.motionManager.socketFwMachine.GetMultiContactFor(FNum, false);
                    if (bRtn)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT BACK CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 315;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT BACK CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }
                    break;
                case 315:
                    if (Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].State != Machine.FwProductState.GoodTurn)
                    {
                        //TODO: 턴상태 체크?
                        //턴 상태가 아니면 돌려야되나?
                        //알람?
                    }
                    nRetStep = 320;
                    break;
                case 320:
                    //언그립
                    //하강
                    if (Globalo.motionManager.socketFwMachine.MultiFlipperGrip(FNum, false) == true)
                    {
                        szLog = $"[ORG] {socketName[FNum]} FLIPPER UNGRIP MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 325;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[ORG] {socketName[FNum]} FLIPPER UNGRIP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }

                    
                    break;
                case 325:
                    bRtn = Globalo.motionManager.socketFwMachine.GetMultiFlipperGrip(FNum, false);
                    if (bRtn)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UNGRIP CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 330;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UNGRIP CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }

                    
                    break;
                case 330:
                    nRetStep = 335;
                    break;
                case 335:
                    nRetStep = 337;
                    break;
                case 337:
                    //그립된 채로, 로테이션 상승 -> Turn -> 로테이션 하강 -> UnGrip -> 배출 요청 
                    nRetStep = 340;
                    break;
                case 340:
                    bEmptyChk = true;

                    for (i = 0; i < SocketMaxCnt; i++)
                    {
                        if (Globalo.motionManager.socketFwMachine.GetIsProductInSocket(FNum, i, true) == true)
                        {
                            if (Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].State == Machine.FwProductState.Good)
                            {
                                FlowSocketState[FNum].States[i] = 2;        //양품
                            }
                            if (Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].State == Machine.FwProductState.NG)
                            {
                                FlowSocketState[FNum].States[i] = 3;        //불량
                            }
                            FlowSocketState[FNum].Barcode[i] = Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].BcrLot;
                        }

                    }

                    if (bEmptyChk)
                    {
                        Globalo.motionManager.InitSocketDone(FNum);                 //배출요청 변수 초기화 - GetSocketDone
                        Globalo.motionManager.socketFwMachine.RaiseProductCall(FlowSocketState[FNum]);        //공급 요청 초기화, Auto_Waiting

                        szLog = $"[AUTO] {socketName[FNum]} UNLOAD REQ [{string.Join(", ", FlowSocketState[FNum].States)}][STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 345;

                        Globalo.motionManager.socketFwMachine.RaiseProductCall(FlowSocketState[FNum]);        //공급 요청
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} EMPTY CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 345:
                    //배출 완료 대기
                    if (Globalo.motionManager.GetSocketDone(FNum) == 0)
                    {
                        //배출 완료

                        MotionControl.SocketReqArgs group = Globalo.motionManager.GetSocketReq(FNum);     //소켓별 배출 상태 받기

                        //배출완료 확인
                        bool bErrChk = false;

                        for (i = 0; i < SocketMaxCnt; i++)
                        {
                            if (group.States[i] == 0) //if (socketStates[ANum, i] == 0)     //배출완료
                            {
                                if (Globalo.motionManager.socketFwMachine.GetIsProductInSocket(FNum, i, true) == true)
                                {
                                    Console.WriteLine($"#{i + 1} Socket Product Unload Fail");
                                    bErrChk = true;
                                }

                                Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].State = Machine.FwProductState.Blank;
                            }
                        }


                        if (bErrChk)
                        {
                            szLog = $"[AUTO] {socketName[FNum]} PRODUCT UNLOAD FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        nRetStep = 350;
                        break;
                    }
                    break;
                case 350:
                    nRetStep = 390;
                    break;
                case 390:
                    Globalo.motionManager.socketFwMachine.IsTesting[FNum] = false;      //배출 완료 후
                    nRetStep = 100;
                    break;
                //--------------------------------------------------------------------------------------------------------------------------
                //
                //
                //  Firmware Download 진행
                //
                //
                //--------------------------------------------------------------------------------------------------------------------------
                case 400:
                    nRetStep = 405;
                    break;
                case 405:
                    if (Globalo.motionManager.socketFwMachine.MultiContactUp(FNum, true) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT UP MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 406;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT UP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 406:
                    bRtn = Globalo.motionManager.socketFwMachine.GetMultiContactUp(FNum, true);
                    if (bRtn)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT UP CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 407;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT UP CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }
                    break;
                case 407:
                    if (Globalo.motionManager.socketFwMachine.MultiContactFor(FNum, true) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT FOR MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 408;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT FOR MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 408:
                    bRtn = Globalo.motionManager.socketFwMachine.GetMultiContactFor(FNum, true);
                    if (bRtn)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT FOR CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 409;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT FOR CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }
                    break;
                case 409:
                    if (Environment.TickCount - nSocketTimeTick[FNum] > 300)
                    {
                        nRetStep = 410;
                    }
                    break;
                case 410:
                    if (Globalo.motionManager.socketFwMachine.MultiContactUp(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT DOWN MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 411;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT DOWN MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 411:
                    if (Globalo.motionManager.socketFwMachine.GetMultiFlipperUp(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER DOWN CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 412;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER DOWN CEHCK FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 412:
                    if (Environment.TickCount - nSocketTimeTick[FNum] > 300)
                    {
                        nRetStep = 415;
                    }
                    break;
                case 415:
                    nRetStep = 420;
                    break;
                case 420:
                    //검사 시작 ->Send Tester pg
                    //TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
                    //sendEqipData.Command = "FW_GO";

                    
                    for (i = 0; i < 4; i++)
                    {
                        TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
                        EqipData.Type = "Tester";

                        TcpSocket.TesterData tData = new TcpSocket.TesterData();
                        tData.Cmd = "FW_GO";
                        tData.LotId[i] = Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].BcrLot;
                        tData.CommandParameter = Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[FNum][i].specialData.Select(item => item.DeepCopy()).ToList();
                        EqipData.Data = tData;

                        Globalo.motionManager.socketFwMachine.Tester_Result_All[FNum][i] = 0;


                        if (FlowSocketState[FNum].States[i] == 0)
                        {
                            Globalo.motionManager.socketFwMachine.Tester_Result_All[FNum][i] = -1;
   
                            //Globalo.tcpManager.SendMessageToClient(sendEqipData, FNum);
                            Globalo.tcpManager.SendMsgToTester(EqipData, FNum);
                        }
                    }
                    nRetStep = 440;
                    break;
                case 440:
                    //컨택 전진 -> 컨택 하강 -> firmware download 진행 -> 완료 -> 컨택 상승 -> 컨택 후진 -> 로테이션 상승 - > 로테이션 턴 -> 하강 -> UNGRIP
                    nRetStep = 500;

                    break;
                case 500:
                    bool allchk = false;
                    allchk = Globalo.motionManager.socketFwMachine.Tester_Result_All[FNum].All(x => x != -1);      //전부 -1이 아닌지 , eeprom 으로부터 결과 받았는지 체크

                    
                    if (allchk)
                    {
   
                        for (i = 0; i < Globalo.motionManager.socketFwMachine.Tester_Result_All[FNum].Length; i++)
                        {
                            if (Globalo.motionManager.socketFwMachine.Tester_Result_All[FNum][i] == 1)  //1 양품
                            {
                                Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[0][i].State = Machine.FwProductState.Good;
                            }
                            else if (Globalo.motionManager.socketFwMachine.Tester_Result_All[FNum][i] == 2)  //2 Ng
                            {
                                Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[0][i].State = Machine.FwProductState.NG;
                            }
                            else
                            {
                                Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[0][i].State = Machine.FwProductState.Blank;
                            }
                        }
                    }
                    nRetStep = 600;
                    break;
                case 600:
                    if (Globalo.motionManager.socketFwMachine.MultiContactUp(FNum, true) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT UP MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 602;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT UP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 602:
                    bRtn = Globalo.motionManager.socketFwMachine.GetMultiContactUp(FNum, true);
                    if (bRtn)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT UP CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 604;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT UP CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }
                    break;
                case 604:
                    if (Globalo.motionManager.socketFwMachine.MultiContactFor(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 606;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} CONTACT BACK MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 606:
                    bRtn = Globalo.motionManager.socketFwMachine.GetMultiContactFor(FNum, false);
                    if (bRtn)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT BACK CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 608;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[i]} CONTACT BACK CHECK TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nStep *= -1;
                        break;
                    }
                    break;
                case 608:
                    if (Globalo.motionManager.socketFwMachine.MultiFlipperUp(FNum, true) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UP MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 609;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 609:
                    if (Globalo.motionManager.socketFwMachine.GetMultiFlipperUp(FNum, true) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UP CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 610;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UP CEHCK FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 610:
                    if (Environment.TickCount - nSocketTimeTick[FNum] > 300)
                    {
                        nRetStep = 612;
                    }
                    break;
                case 612:
                    if (Globalo.motionManager.socketFwMachine.MultiFlipperTurn(FNum, true) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER TURN MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 614;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 614:
                    if (Globalo.motionManager.socketFwMachine.GetMultiFlipperTurn(FNum, true) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER TURN CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 616;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER TURN CEHCK FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 616:
                    if (Environment.TickCount - nSocketTimeTick[FNum] > 300)
                    {
                        nRetStep = 618;
                    }
                    
                    break;
                case 618:
                    if (Globalo.motionManager.socketFwMachine.MultiFlipperUp(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER DOWN MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 620;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER DOWN MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 620:
                    if (Globalo.motionManager.socketFwMachine.GetMultiFlipperUp(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER DOWN CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 622;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER DOWN CEHCK FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 622:
                    nRetStep = 624;
                    break;
                case 624:
                    if (Globalo.motionManager.socketFwMachine.MultiFlipperGrip(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UNGRIP MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 626;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UNGRIP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 626:
                    if (Globalo.motionManager.socketFwMachine.GetMultiFlipperGrip(FNum, false) == true)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UNGRIP CEHCK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 628;
                        nSocketTimeTick[FNum] = Environment.TickCount;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[FNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] {socketName[FNum]} FLIPPER UNGRIP CEHCK FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 628:
                    nRetStep = 700;
                    break;
                case 700:
                    nRetStep = 900;
                    break;
                case 900:

                    Globalo.motionManager.socketFwMachine.IsTesting[FNum] = false;      //검사 완료 후
                    nRetStep = 100;
                    break;
            }
            return nRetStep;
        }
        private int[] RtnFwSocketState(int index, List<Machine.FwSocketProductInfo> socketState)
        {
            int i = 0;
            int[] tempState = { -1, -1, -1, -1 };
            for (i = 0; i < 4; i++)
            {
                if (Globalo.motionManager.socketFwMachine.GetIsProductInSocket(index, i, true) == false)
                {
                    tempState[i] = 1;        //1 = 공급요청
                    socketState[i].State = Machine.FwProductState.Blank;
                }
                else
                {
                    //있으면 검사 or 배출 요청
                    if (socketState[i].State == Machine.FwProductState.Testing)        //검사 할 차례
                    {
                        //검사 전
                        tempState[i] = 3;
                    }
                    else if (socketState[i].State == Machine.FwProductState.Good || socketState[i].State == Machine.FwProductState.NG)
                    {
                        //검사 완료
                        tempState[i] = 2; //양품 or 불량 배출 요청
                    }
                    else
                    {
                        //상태 이상
                        tempState[i] = 0;
                    }
                }
            }

            return tempState;
        }
        private int RtnSocketAll(int[] socketState)
        {
            int nRtn = -1;
            if (socketState.Length < 1)
            {
                return -1;
            }

            int first = socketState[0];
            for (int i = 1; i < socketState.Length; i++)
            {
                if (socketState[i] != first)
                {
                    return -1;
                }
            }

            return first; // 모두 같음 (1 또는 2)
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
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.socketFwMachine.IsTesting[i] == false)
                        {
                            socketProcessState[i] = FwSocketState.Wait;
                            FlowSocketState[i].States = new int[] { -1, -1, -1, -1 };
                            FlowSocketState[i].Barcode = new string[] { string.Empty, string.Empty, string.Empty, string.Empty };

                            Globalo.motionManager.socketFwMachine.RaiseProductCall(FlowSocketState[i]);         //공급 요청 초기화, Auto_Waiting

                            FlowSocketState[i].States = RtnFwSocketState(i, Globalo.motionManager.socketFwMachine.socketProduct.FwSocketInfo[i]);


                            //1개라도 검사 안된게 있으면 검사 진행
                            //1개라도 검사 완료된 제품이 있으면 배출 요청
                            //아무 것도 없으면 공급 요청

                            bool containdTest3 = Array.Exists(FlowSocketState[i].States, state => state == 3);      //3을 하나라도 포함하고 있는지
                            bool containdTest2 = Array.Exists(FlowSocketState[i].States, state => state == 2);      //2 하나라도 포함하고 있는지
                            int socketAll = RtnSocketAll(FlowSocketState[i].States);

                            if (socketAll == 1)       //2개 전부 없음
                            {
                                //공급 요청
                                socketProcessState[i] = FwSocketState.LoadReq;         //공급 요청
                            }
                            else if (containdTest3)
                            {
                                //검사 진행
                                socketProcessState[i] = FwSocketState.Testing;         //펌웨어 다운로드 진행
                            }
                            else if (containdTest2)
                            {
                                //배출
                                socketProcessState[i] = FwSocketState.UnLoadReq;       //배출 요청
                            }
                            if (socketProcessState[i] != FwSocketState.Wait)
                            {
                                Globalo.motionManager.socketFwMachine.IsTesting[i] = true;
                            }
                        }
                    }
  
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


                    Globalo.motionManager.socketFwMachine.RunState = OperationState.Standby;
                    szLog = $"[READY] FW SOCKET 운전준비 완료 [STEP : {nStep}]";
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
