using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public enum AoiSocketState
    {
        Wait = 0,
        LoadReq,        // 공급 요청
        UnLoadReq,      //배출 요청 (양품 + 불량 섞여 있을 수도 있다.)
        Testing,        //검사 전
    }
    public class AoiSocketFlow
    {
        public string[] axisName = { "Left Socekt", "Right Socket" };

        public int nTimeTick = 0;
        private TcpSocket.MessageWrapper aoiEqipData;
        private TcpSocket.TesterData tData;
        private AoiSocketState[] socketProcessState = new AoiSocketState[2];
        private MotionControl.SocketReqArgs[] FlowSocketState = new MotionControl.SocketReqArgs[2];

        public AoiSocketFlow()
        {
            int i = 0;
            aoiEqipData = new TcpSocket.MessageWrapper();
            aoiEqipData.Type = "AOI";

            tData = new TcpSocket.TesterData();

            for (i = 0; i < 2; i++)
            {
                FlowSocketState[i] = new MotionControl.SocketReqArgs
                {
                    Index = i,
                    States = new int[] { -1, -1 },      //aoi 2개
                    Barcode = new string[] { string.Empty, string.Empty }
                };
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        //◀ ▶
        //
        // LEFT 소켓 Flow
        //
        //
        //-----------------------------------------------------------------------------------------------------------
        public int Auto_Common_Socket(int nStep, int SocketIndex)
        {
            int i = 0;
            string szLog = "";
            bool result = false;
            bool bRtn = false;
            bool bEmptyChk = false;
            int nRetStep = nStep;
            const int SocketMaxCnt = 2;
            int ANum = SocketIndex;     //SocketIndex = 0 or 1

            Machine.eAoiSocket Xmotor = Machine.eAoiSocket.SOCKET_L_X;
            Machine.eAoiSocket Zmotor = Machine.eAoiSocket.SOCKET_L_Z;

            if (ANum == 0)
            {
                Xmotor = Machine.eAoiSocket.SOCKET_L_X;
                Zmotor = Machine.eAoiSocket.SOCKET_L_Z;
            }
            else
            {
                Xmotor = Machine.eAoiSocket.SOCKET_R_X;
                Zmotor = Machine.eAoiSocket.SOCKET_R_Z;
            }

            switch (nStep)
            {
                case 100:
                    if (Globalo.motionManager.socketAoiMachine.IsTesting[ANum] == false)
                    {
                        break;
                    }
                    nRetStep = 120;
                    break;

                case 120:
                    if (socketProcessState[ANum] == AoiSocketState.UnLoadReq)        //전부가 검사가 끝났을때 (양품 + NG)
                    {
                        Console.WriteLine("Good Unload Req");
                        nRetStep = 300;
                        break;
                    }
                    if (socketProcessState[ANum] == AoiSocketState.Testing)        //검사 진행
                    {
                        Console.WriteLine("Test Run");
                        nRetStep = 400;
                        break;
                    }
                    if (socketProcessState[ANum] == AoiSocketState.LoadReq)   //전부다 비었을때만, 공급 요청
                    {
                        Console.WriteLine("Load Req");
                        nRetStep = 200;
                        break;
                    }

                    Globalo.motionManager.socketAoiMachine.IsTesting[ANum] = false;  //다시 Waiting로 가기위해
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
                    nRetStep = 210;
                    break;
                case 210:
                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Xmotor, false);

                    if (bRtn == false)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z WAIT POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[AUTO] {axisName[ANum]} Z WAIT POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 220;
                    nTimeTick = Environment.TickCount;
                    break;

                case 220:
                    //Z 축 대기위치 확인
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Zmotor].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Zmotor))
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z WAIT 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 230;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z WAIT 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 230:

                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.LOAD_POS, Xmotor, false);

                    if (bRtn == false)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} LOAD POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[AUTO] {axisName[ANum]} LOAD POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 235;
                    nTimeTick = Environment.TickCount;
                    break;
                case 235:
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Xmotor].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.LOAD_POS, Xmotor))
                    {
                        szLog = $"[AUTO] {axisName[ANum]} X LOAD 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 240;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} X LOAD 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 240:
                    bEmptyChk = true;
                    for (i = 0; i < SocketMaxCnt; i++)
                    {
                        if (Globalo.motionManager.socketAoiMachine.GetIsProductInSocket(ANum, i, true) == false)
                        {
                            //둘다 비어야 공급 요청 가능
                            FlowSocketState[ANum].States[i] = 1;
                        }
                        else
                        {
                            bEmptyChk = false;
                            FlowSocketState[ANum].States[i] = -1;
                        }
                        
                    }

                    if (bEmptyChk)
                    {
                        Globalo.motionManager.InitSocketDone(ANum);             //공급요청 변수 초기화
                        Globalo.motionManager.socketAoiMachine.RaiseProductCall(FlowSocketState[ANum]);        //공급 요청 초기화, Auto_Waiting

                        szLog = $"[AUTO] {axisName[ANum]} LOAD REQ [{string.Join(", ", FlowSocketState[ANum].States)}][STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 245;

                    }
                    else
                    {
                        szLog = $"[AUTO] {axisName[ANum]} EMPTY CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;

                case 245:
                    //공급 완료 대기
                    if (Globalo.motionManager.GetSocketDone(ANum) == 0)
                    {
                        //공급 완료
                        MotionControl.SocketReqArgs group = Globalo.motionManager.GetSocketReq(ANum);    //소켓별 공급 상태 받기

                        bool bErrChk = false;
                        for (i = 0; i < SocketMaxCnt; i++)
                        {

                            //공급한 개수만큼 인식이 되는지 체크
                            if (group.States[i] == 0)//if (socketStates[ANum, i] == 1)
                            {
                                if (Globalo.motionManager.socketAoiMachine.GetIsProductInSocket(ANum, i, true) == false)
                                {
                                    //공급했는 소켓인데 제품이 없으면 알람
                                    Console.WriteLine($"#{i + 1} Socket Product Empty err");
                                    bErrChk = true;

                                    Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][i].BcrLot = string.Empty;
                                    Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][i].State = Machine.AoiSocketProductState.Blank;

                                }
                                else
                                {
                                    Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][i].BcrLot = group.Barcode[i];
                                    Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][i].State = Machine.AoiSocketProductState.Testing;
                                }
                            }
                        }

                        if (bErrChk)
                        {
                            szLog = $"[AUTO] {axisName[ANum]} PRODUCT LOAD FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }

                        szLog = $"[AUTO] {axisName[ANum]} PRODUCT LOAD COMPLETE [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 250;
                    }
                    break;

                case 250:
                    nRetStep = 290;
                    break;
                case 290:
                    //공급 완료
                    Globalo.motionManager.socketAoiMachine.IsTesting[ANum] = false;
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
                    //배출 요청
                    nRetStep = 305;
                    break;

                case 305:
                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Xmotor, false);

                    if (bRtn == false)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z WAIT POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[AUTO] {axisName[ANum]} Z WAIT POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 310;
                    nTimeTick = Environment.TickCount;
                    break;
                case 310:
                    //Z 축 대기위치 확인
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Zmotor].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Zmotor))
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z WAIT 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 315;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z WAIT 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 315:
                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.UN_LOAD_POS, Xmotor, false);

                    if (bRtn == false)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} UNLOAD POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[AUTO] {axisName[ANum]} UNLOAD POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 320;
                    nTimeTick = Environment.TickCount;
                    break;
                case 320:
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Xmotor].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.UN_LOAD_POS, Xmotor))
                    {
                        szLog = $"[AUTO] {axisName[ANum]} X LOUNLOADAD 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 330;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} X UNLOAD 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 330:
                    nRetStep = 340;
                    break;
                case 340:
                    bEmptyChk = true;

                    for (i = 0; i < SocketMaxCnt; i++)
                    {
                        if (Globalo.motionManager.socketAoiMachine.GetIsProductInSocket(ANum, i, true) == true)
                        {
                            if (Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][i].State == Machine.AoiSocketProductState.Good)
                            {
                                FlowSocketState[ANum].States[i] = 2;        //양품
                            }
                            if (Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][i].State == Machine.AoiSocketProductState.NG)
                            {
                                FlowSocketState[ANum].States[i] = 3;        //불량
                            }
                            FlowSocketState[ANum].Barcode[i] = Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][i].BcrLot;
                        }
                        
                    }

                    if (bEmptyChk)
                    {
                        Globalo.motionManager.InitSocketDone(ANum);                 //배출요청 변수 초기화
                        Globalo.motionManager.socketAoiMachine.RaiseProductCall(FlowSocketState[ANum]);        //공급 요청 초기화, Auto_Waiting

                        szLog = $"[AUTO] {axisName[ANum]} UNLOAD REQ [{string.Join(", ", FlowSocketState[ANum].States)}][STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 345;

                        Globalo.motionManager.socketAoiMachine.RaiseProductCall(FlowSocketState[ANum]);        //공급 요청
                    }
                    else
                    {
                        szLog = $"[AUTO] {axisName[ANum]} EMPTY CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 345:
                    //배출 완료 대기
                    if (Globalo.motionManager.GetSocketDone(ANum) == 0)
                    {
                        //배출 완료
                        
                        MotionControl.SocketReqArgs group = Globalo.motionManager.GetSocketReq(ANum);     //소켓별 배출 상태 받기
                        
                        //배출완료 확인
                        bool bErrChk = false;

                        for (i = 0; i < SocketMaxCnt; i++)
                        {
                            if (group.States[i] == 0) //if (socketStates[ANum, i] == 0)     //배출완료
                            {
                                if (Globalo.motionManager.socketAoiMachine.GetIsProductInSocket(ANum, i, true) == true)
                                {
                                    Console.WriteLine($"#{i + 1} Socket Product Unload Fail");
                                    bErrChk = true;
                                }

                                Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][i].State = Machine.AoiSocketProductState.Blank;
                            }
                        }


                        if (bErrChk)
                        {
                            szLog = $"[AUTO] {axisName[ANum]} PRODUCT UNLOAD FAIL[STEP : {nStep}]";
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
                    Globalo.motionManager.socketAoiMachine.IsTesting[ANum] = false;      //배출 완료 후
                    nRetStep = 100;
                    break;
                //--------------------------------------------------------------------------------------------------------------------------
                //
                //
                //  AOI 검사 진행
                //
                //
                //--------------------------------------------------------------------------------------------------------------------------
                case 400:

                    nRetStep = 405;
                    break;
                case 405:
                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.HOUSING_OUT_POS, Xmotor, false);

                    if (bRtn == false)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z HOUSING OUT POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[AUTO] {axisName[ANum]} Z HOUSING OUT POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 410;
                    nTimeTick = Environment.TickCount;
                    break;
                case 410:
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Zmotor].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.HOUSING_OUT_POS, Zmotor))
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z HOUSING OUT POS 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 415;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z HOUSING OUT POS 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 415:
                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.CAPTURE_R_POS, Xmotor, false);

                    if (bRtn == false)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} CAPTURE_R POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[AUTO] {axisName[ANum]} CAPTURE_R POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nTimeTick = Environment.TickCount;
                    nRetStep = 420;
                    break;
                case 420:
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Xmotor].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.CAPTURE_R_POS, Xmotor))
                    {
                        szLog = $"[AUTO] {axisName[ANum]} X CAPTURE_R POS 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 425;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} X CAPTURE_R 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 425:
                    //딜레이
                    if (Environment.TickCount - nTimeTick > 300)
                    {
                        nRetStep = 430;
                    }
                    break;
                case 430:
                    nRetStep = 440;
                    break;
                case 440:
                    //
                    //검사 진행 - Socket Left , Right 인지 = 그 안에서 왼쪽 소켓인지 , 오른쪽 소켓인지
                    //
                    //
                    //
                    Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] = -1;

                    tData.Cmd = "CMD_TEST_STEP1";       //RESP_TEST_STEP1,  RESP_TEST_STEP2
                    tData.socketIndex = 1 + (ANum * 2);              //Left - R Socket
                    tData.Name = "";
                    tData.LotId = Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][1].BcrLot;
                    aoiEqipData.Data = tData;

                    Globalo.tcpManager.SendMsgToTester(aoiEqipData, ANum); // pc 0 or pc 1
                    nRetStep = 442;
                    break;

                case 442:
                    //z축 이동 요청?
                    if (Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] == 0)       //Step1 검사 대기
                    {
                        Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] = -1;


                        tData.Name = "";
                        tData.Cmd = "CMD_TEST_STEP2";       //RESP_TEST_STEP1,  RESP_TEST_STEP2
                        aoiEqipData.Data = tData;

                        Globalo.tcpManager.SendMsgToTester(aoiEqipData, ANum); // pc 0 or pc 1

                        nRetStep = 444;
                    }
                    break;
                case 444:
                    //apd 보고 lot complete 다하고 보내는건가?
                    //검사 완료 대기
                    //Left 소켓에 제품 있으면 Capture L위치로 이동 없으면 검사 종료
                    if (Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] != -1)       //Step2 검사 대기
                    {
                        //여기서 양불 판정받기
                        if (Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] == 1)
                        {
                            //양품
                            Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][1].State = Machine.AoiSocketProductState.Good;
                        }
                        else if (Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] == 2)
                        {
                            //ng
                            Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][1].State = Machine.AoiSocketProductState.NG;
                        }
                        Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] = -1;
                        nRetStep = 446;
                    }
                    break;
                case 446:
                    nRetStep = 448;
                    break;
                case 448:
                    nRetStep = 450;
                    break;
                case 450:
                    
                    nRetStep = 460;
                    break;
                case 460:
                    
                    nRetStep = 480;
                    break;
                case 480:
                    //Left 소켓에 제품이 있으면 검사 위치로 이동
                    //보고 대기

                    //검사 쪽에서 APD만 보고해주면 , SecsGem ---> Handler 로 양불 판정 받으면 될듯
                    //APD 보고, Lot Complete 보고 대기
                    nRetStep = 490;
                    break;

                case 490:
                    //Z 축 원복
                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.HOUSING_OUT_POS, Xmotor, false);

                    if (bRtn == false)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z HOUSING OUT POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[AUTO] {axisName[ANum]} Z HOUSING OUT POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 495;
                    nTimeTick = Environment.TickCount;
                    break;
                case 495:
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Zmotor].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.HOUSING_OUT_POS, Zmotor))
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z HOUSING OUT POS 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nTimeTick = Environment.TickCount;

                        nRetStep = 500;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} Z HOUSING OUT POS 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 500:

                    nRetStep = 505;
                    break;
                case 505:

                    nRetStep = 510;
                    break;
                case 510:

                    nRetStep = 515;
                    break;
                case 515:
                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.CAPTURE_L_POS, Xmotor, false);

                    if (bRtn == false)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} CAPTURE_L POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[AUTO] {axisName[ANum]} CAPTURE_L POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nTimeTick = Environment.TickCount;
                    nRetStep = 520;
                    break;
                case 520:
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Xmotor].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.CAPTURE_L_POS, Xmotor))
                    {
                        szLog = $"[AUTO] {axisName[ANum]} X CAPTURE_L POS 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 525;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] {axisName[ANum]} X CAPTURE_L 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 525:
                    //딜레이
                    if (Environment.TickCount - nTimeTick > 300)
                    {
                        nRetStep = 530;
                    }
                    
                    break;
                case 530:
                    
                    nRetStep = 535;
                    break;
                case 535:

                    nRetStep = 540;
                    break;

                case 540:

                    Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] = -1;

                    tData.Name = "";
                    tData.Cmd = "CMD_TEST_STEP1";               //RESP_TEST_STEP1,  RESP_TEST_STEP2
                    tData.socketIndex = 0 + (ANum * 2);         //Left - R Socket
                    tData.LotId = Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][0].BcrLot;
                    aoiEqipData.Data = tData;

                    Globalo.tcpManager.SendMsgToTester(aoiEqipData, ANum); // pc 0 or pc 1

                    nRetStep = 542;
                    break;
                case 542:
                    if (Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] == 0)       //Step1 검사 대기
                    {

                        Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] = -1;
                        tData.Name = "";
                        tData.Cmd = "CMD_TEST_STEP2";       //RESP_TEST_STEP1,  RESP_TEST_STEP2
                        aoiEqipData.Data = tData;

                        Globalo.tcpManager.SendMsgToTester(aoiEqipData, ANum); // pc 0 or pc 1

                        nRetStep = 544;
                    }
                    break;
                case 544:
                    if (Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] != -1)       //Step2 검사 대기
                    {
                        //여기서 양불 판정받기
                        //마지막 양불 판정은 SecsGem으로부터 Lot Complete 결과 보고 결정

                        //SecsGem로 apd 보고하기
                        tData.Cmd = "CMD_APD";
                        tData.Name = "";
                        tData.LotId = Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][0].BcrLot;
                        aoiEqipData.Data = tData;

                        Globalo.tcpManager.SendMsgToTester(aoiEqipData, ANum);


                        Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] = -1;
                        nRetStep = 546;
                    }
                    break;

                case 546:
                    if (Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] != -1)
                    {
                        //SecsGem으로 부터 Lot Complete 받기.
                        nRetStep = 600;
                    }
                        
                    break;
                case 600:
                    //완공 보고 여기서 기다려야되나?
                    if (Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] == 1)
                    {
                        //양품
                        Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][0].State = Machine.AoiSocketProductState.Good;
                    }
                    else if (Globalo.motionManager.socketAoiMachine.Tcp_Req_Result[ANum] == 2)
                    {
                        //ng
                        Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][0].State = Machine.AoiSocketProductState.NG;
                    }
                    nRetStep = 700;
                    break;
                case 700:
                    nRetStep = 800;
                    break;
                case 800:
                    //RIGHT , LEFT 제품 APD 보고하고 , Lot Complete 완공까지 하고,
                    //배출 신호 올 때까지 대기
                    nRetStep = 900;
                    break;
                case 900:

                    Globalo.motionManager.socketAoiMachine.IsTesting[ANum] = false;      //검사 완료 후
                    nRetStep = 100;
                    break;
            }

            return nRetStep;
        }
        private int[] RtnAoiSocketState(int index, List<Machine.AoiSocketProductInfo> socketState)
        {
            int i = 0;
            int[] tempState = { -1, -1 };
            for (i = 0; i < 2; i++)
            {
                if (Globalo.motionManager.socketAoiMachine.GetIsProductInSocket(index, i, true) == false)
                {
                    tempState[i] = 1;        //1 = 공급요청
                    socketState[i].State = Machine.AoiSocketProductState.Blank;
                }
                else
                {
                    //있으면 검사 or 배출 요청
                    if (socketState[i].State == Machine.AoiSocketProductState.Testing)        //검사 할 차례
                    {
                        //검사 전
                        tempState[i] = 3;
                    }
                    else if (socketState[i].State == Machine.AoiSocketProductState.Good || socketState[i].State == Machine.AoiSocketProductState.NG)
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
                    if (Globalo.motionManager.socketAoiMachine.IsTesting[0] == true && Globalo.motionManager.socketAoiMachine.IsTesting[1] == true)
                    {
                        break;
                    }

                    for (i = 0; i < 2; i++)
                    {
                        if (Globalo.motionManager.socketAoiMachine.IsTesting[i] == false)
                        {
                            socketProcessState[i] = AoiSocketState.Wait;
                            FlowSocketState[i].States = new int[] { -1, -1 };
                            FlowSocketState[i].Barcode = new string[] { string.Empty, string.Empty };

                            Globalo.motionManager.socketAoiMachine.RaiseProductCall(FlowSocketState[i]);         //공급 요청 초기화, Auto_Waiting

                            FlowSocketState[i].States = RtnAoiSocketState(i, Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[i]);


                            //1개라도 검사 안된게 있으면 검사 진행
                            //1개라도 검사 완료된 제품이 있으면 배출 요청
                            //아무 것도 없으면 공급 요청

                            bool containdTest3 = Array.Exists(FlowSocketState[i].States, state => state == 3);      //3을 하나라도 포함하고 있는지
                            bool containdTest2 = Array.Exists(FlowSocketState[i].States, state => state == 2);      //2 하나라도 포함하고 있는지
                            int socketAll = RtnSocketAll(FlowSocketState[i].States);

                            if (socketAll == 1)       //2개 전부 없음
                            {
                                //공급 요청
                                socketProcessState[i] = AoiSocketState.LoadReq;         //공급 요청
                            }
                            else if (containdTest3)
                            {
                                //검사 진행
                                socketProcessState[i] = AoiSocketState.Testing;         //AOI 검사 진행
                            }
                            else if(containdTest2)
                            {
                                //배출
                                socketProcessState[i] = AoiSocketState.UnLoadReq;       //배출 요청
                            }
                            if (socketProcessState[i] != AoiSocketState.Wait)
                            {
                                Globalo.motionManager.socketAoiMachine.IsTesting[i] = true;
                            }

                        }
                    }
                    break;
            }

            return nRetStep;
        }

        #endregion
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
                    nRetStep = 1100;
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
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_Z].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_Z].GetPosiSensor() == true)
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
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_Z].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_Z].GetPosiSensor() == true)
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
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_X].GetNegaSensor() == true)
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
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_X].GetNegaSensor() == true)
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
                            szLog = $"[ORIGIN] {Globalo.motionManager.socketAoiMachine.MotorAxes[i].Name} HOME 동작 ERROR [STEP : {nStep}]";
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

                    nRetStep = 2100;
                    break;
                case 2100:
                    //Z 축 대기위치
                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_L_Z, false);

                    if (bRtn == false)
                    {
                        szLog = $"[READY] LEFT SOCKET Z WAIT POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[READY] LEFT SOCKET Z WAIT POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 2120;
                    break;
                case 2120:
                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_Z, false);

                    if (bRtn == false)
                    {
                        szLog = $"[READY] RIGHT SOCKET Z WAIT POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[READY] RIGHT SOCKET Z WAIT POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 2200;

                    nTimeTick = Environment.TickCount;
                    break;
                case 2200:
                    //Z 축 대기위치 확인
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_Z].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_L_Z))
                    {
                        szLog = $"[READY] LEFT SOCKET Z WAIT 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2220;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] LEFT SOCKET Z WAIT 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2220:
                    //Z 축 대기위치 확인
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_Z].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_Z))
                    {
                        szLog = $"[READY] RIGHT SOCKET Z WAIT 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2300;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] RIGHT SOCKET Z WAIT 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 2300:
                    //x축 대기 위치 이동
                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_L_X, false);

                    if (bRtn == false)
                    {
                        szLog = $"[READY] LEFT SOCKET X WAIT POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[READY] LEFT SOCKET X WAIT POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 2320;
                    break;
                case 2320:
                    //x축 대기 위치 이동
                    bRtn = Globalo.motionManager.socketAoiMachine.Socket_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X, false);

                    if (bRtn == false)
                    {
                        szLog = $"[READY] RIGHT SOCKET X WAIT POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[READY] RIGHT SOCKET X WAIT POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 2400;
                    nTimeTick = Environment.TickCount;
                    break;
                case 2400:
                    //x축 대기 위치 이동 확인
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_L_X))
                    {
                        szLog = $"[READY] LEFT SOCKET X WAIT 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2420;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] LEFT SOCKET X WAIT 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2420:
                    //x축 대기 위치 이동 확인
                    if (Globalo.motionManager.socketAoiMachine.MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketAoiMachine.ChkMotorPos(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X))
                    {
                        szLog = $"[READY] RIGHT SOCKET X WAIT 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2500;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] RIGHT SOCKET X WAIT 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2500:
                    //조명 켜져있는지 확인?
                    nRetStep = 2600;
                    break;
                case 2600:

                    nRetStep = 2900;
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
//X축 SOCKET_R TEST POS 이동
//조명 ON
//Z축 검사 위치 이동
//TEST1
//조명 CHANGE
//TEST2

//X축 SOCKET_L TEST POS 이동
//조명 ON
//Z축 검사 위치 이동
//TEST1
//조명 CHANGE
//TEST2