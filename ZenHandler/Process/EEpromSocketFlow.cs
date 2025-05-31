using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public enum SocketState
    {
        Wait = 0,
        LoadReq,   // 공급 요청
        UnLoadReq,     // 배출 요청 (양품 + 불량 섞여 있을 수도 있다.)
        Write,     //Write 검사 전
        Verify,  //Verify 검사 전
    }
    public class EEpromSocketFlow
    {
        public CancellationTokenSource CancelTokenSocket;
        public ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);  // true면 동작 가능

        private Task<int> Write_Task;
        private Task<int> Verify_Task;
        private int Task_Wait_Write = 1;
        private int Task_Wait___Verify = 1;
        private readonly SynchronizationContext _syncContext;

        public int[] nSocketTimeTick = { 0, 0 };

        public int nTimeTick = 0;

        private int[] socketStateA = { -1, -1, -1, -1 };     // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출
        private int[] socketState_B = { -1, -1, -1, -1 };    // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출

        private SocketState[] socketProcessState = new SocketState[2];
        private bool[] InterLock = { false, false };

        private bool isWriteBusy = false;                   //Write 사용 유무
        private bool WritePositionOccupied = false;         //Write 위치 점유
        private bool isVerifyBusy = false;                  //Verify 사용 유무
        private bool VerifyPositionOccupied = false;         //Verify 위치 점유
        public EEpromSocketFlow()
        {
            //EEPROM 설비는 소켓 하나마다 pc 1대 - 총 8대
            //8대 모두 연결 상태 확인후 진행

            //Back소켓은 좌우로만 이동
            //Fromt 소켓은 Y실린더 앞위이동 + 좌우 이동

            _syncContext = SynchronizationContext.Current;
            CancelTokenSocket = new CancellationTokenSource();
            Write_Task = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
            Verify_Task = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
        }
        
        //-----------------------------------------------------------------------------------------------------------
        //◀ ▶
        //
        //  좌우 X축 소켓 Flow
        //
        //
        //-----------------------------------------------------------------------------------------------------------
        public int Auto_X_Socket(int nStep)
        {
            int i = 0;
            string szLog = "";
            bool result = false;
            bool bRtn = false;
            int nRetStep = nStep;
            const int sNum = 0;

            switch (nStep)
            {
                case 100:
                    if (Globalo.motionManager.socketEEpromMachine.IsTesting[sNum] == false)
                    {
                        break;
                    }
                    nRetStep = 120;
                    break;

                case 120:
                    //TODO: 한 시퀀스 끝나고 여기 모여서 다시 진행
                    if (socketProcessState[sNum] == SocketState.Write)       //전부다 Verify 완료 될때까지
                    {
                        Console.WriteLine("x Write Start");
                        
                        nRetStep = 400;
                        break;
                    }
                    if (socketProcessState[sNum] == SocketState.Verify)       //전부다 Write 완료 될 때까지
                    {
                        Console.WriteLine("Verify Start");
                        nRetStep = 500;
                        break;
                    }
                    
                    if (socketProcessState[sNum] == SocketState.UnLoadReq)        //전부가 검사가 끝났을때 (양품 + NG)
                    {
                        Console.WriteLine("Good Unload Req");
                        nRetStep = 300;
                        break;
                    }

                    if (socketProcessState[sNum] == SocketState.LoadReq)   //전부다 비었을때만, 공급 요청
                    {
                        Console.WriteLine("Load Req");
                        nRetStep = 200;
                        break;
                    }

                    Globalo.motionManager.socketEEpromMachine.IsTesting[sNum] = false;
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
                    nSocketTimeTick[sNum] = Environment.TickCount;
                    break;
                case 205:
                    //Y 실린더 확인후 Write 이동 후 진행
                    if (Globalo.motionManager.socketEEpromMachine.GetSocketFor(false) == true)
                    {
                        szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 206;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[sNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 206:
                    //컨택 실린더 상승 확인 8개 전체
                    bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(0, true);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] WRITE CONTACT UP CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nStep *= -1;
                        break;
                    }
                    szLog = $"[READY] WRITE CONTACT UP CHECK [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);


                    bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(1, true);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] VERIFY CONTACT UP CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nStep *= -1;
                        break;
                    }

                    szLog = $"[READY] VERIFY CONTACT UP CHECK [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactFor(1, false);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] VERIFY CONTACT BACK CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nStep *= -1;
                        break;
                    }

                    szLog = $"[READY] VERIFY CONTACT CHECK CHECK [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 207;
                    break;
                case 207:
                    bRtn = Globalo.motionManager.socketEEpromMachine.Socket_X_Move(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.BACK_X, false);

                    if (bRtn == false)
                    {
                        szLog = $"[AUTO] BACK SOCKET X LOAD POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[AUTO] BACK SOCKET X LOAD POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nSocketTimeTick[sNum] = Environment.TickCount;
                    nRetStep = 208;
                    break;
                case 208:
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.BACK_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.ChkMotorXPos(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.BACK_X))
                    {

                        VerifyPositionOccupied = false;
                        szLog = $"[AUTO] BACK SOCKET LOAD 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 210;
                        break;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[sNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] BACK SOCKET LOAD 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 210:
                    //공급 요청
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(sNum, i, true) == false)
                        {
                            //socketStateA = new int[] { 1, 1, 1, 1 };        //1 = 공급 요청
                            socketStateA[i] = 1;
                        }
                        else
                        {
                            socketStateA[i] = -1;
                        }
                    }

                    Globalo.motionManager.socketEEpromMachine.RaiseProductCall(sNum, socketStateA);        //공급 요청 초기화, Auto_Waiting

                    szLog = $"[AUTO] X SOCKET LOAD REQ [{string.Join(", ", socketStateA)}][STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nRetStep = 215;
                    break;
                case 215:
                    nRetStep = 220;
                    break;
                case 220:
                    //공급 완료 대기
                    if (Globalo.motionManager.GetSocketDone(sNum) == 0)
                    {
                        //공급 완료
                        Globalo.motionManager.InitSocketDone(sNum);             //공급요청 변수 초기화
                        socketStateA = Globalo.motionManager.GetSocketReq(sNum);    //소켓별 공급 상태 받기

                        //공급완료
                        bool bErrChk = false;
                        for (i = 0; i < socketStateA.Length; i++)
                        {
                            if (socketStateA[i] == 0)       //0 = 공급완료
                            {
                                if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(sNum, i, true) == false)
                                {
                                    //공급했는 소켓인데 제품이 없으면 알람
                                    Console.WriteLine($"#{i+1} Socket Product Empty err");
                                    bErrChk = true;
                                    Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State = Machine.SocketProductState.Blank;
                                }
                                else
                                {
                                    Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State = Machine.SocketProductState.Writing;
                                }
                            }
                        }
                        if (bErrChk)
                        {
                            szLog = $"[AUTO] SOCKET PRODUCT LOAD FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        Globalo.motionManager.socketEEpromMachine.IsTesting[sNum] = false;      //공급 완료 후
                        nRetStep = 100;
                        break;
                    }
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
                    nSocketTimeTick[sNum] = Environment.TickCount;
                    nRetStep = 305;
                    break;
                case 305:
                    //Y 실린더 확인후 Write 이동 후 진행
                    if (Globalo.motionManager.socketEEpromMachine.GetSocketFor(false) == true)
                    {
                        szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 310;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[sNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 310:
                    //컨택 실린더 상승 확인 8개 전체
                    bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(0, true);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] WRITE CONTACT UP CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nStep *= -1;
                        break;
                    }
                    szLog = $"[READY] WRITE CONTACT UP CHECK [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);


                    bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(1, true);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] VERIFY CONTACT UP CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nStep *= -1;
                        break;
                    }

                    szLog = $"[READY] VERIFY CONTACT UP CHECK [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactFor(1, false);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] VERIFY CONTACT BACK CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nStep *= -1;
                        break;
                    }

                    szLog = $"[READY] VERIFY CONTACT CHECK CHECK [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 315;
                    break;
                case 315:
                    bRtn = Globalo.motionManager.socketEEpromMachine.Socket_X_Move(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.BACK_X, false);

                    if (bRtn == false)
                    {
                        szLog = $"[AUTO] BACK SOCKET X UNLOAD POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[AUTO] BACK SOCKET X UNLOAD POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nSocketTimeTick[sNum] = Environment.TickCount;
                    nRetStep = 320;
                    break;
                case 320:
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.BACK_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.ChkMotorXPos(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.BACK_X))
                    {

                        VerifyPositionOccupied = false;
                        szLog = $"[AUTO] BACK SOCKET UNLOAD 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 325;
                        break;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[sNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] BACK SOCKET UNLOAD 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 325:
                    //배출 요청
                    for (i = 0; i < 4; i++)
                    {
                        socketStateA[i] = 1;
                        if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(sNum, i, true) == true)
                        {
                            if (Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State == Machine.SocketProductState.Good)
                            {
                                socketStateA[i] = 2;
                            }
                            if (Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State == Machine.SocketProductState.NG)
                            {
                                socketStateA[i] = 3;
                            }

                        }
                    }
                    //2 = 양품
                    //3 = 불량
                    Globalo.motionManager.socketEEpromMachine.RaiseProductCall(sNum, socketStateA);        //배출 요청 초기화, Auto_Waiting

                    szLog = $"[AUTO] X SOCKET UNLOAD REQ [{string.Join(", ", socketStateA)}][STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 330;
                    break;
                case 330:
                    //배출 완료 대기
                    if (Globalo.motionManager.GetSocketDone(sNum) == 0)
                    {
                        //배출 완료
                        Globalo.motionManager.InitSocketDone(sNum);             //배출요청 변수 초기화
                        socketStateA = Globalo.motionManager.GetSocketReq(sNum);    //소켓별 공급 상태 받기

                        //배출완료
                        bool bErrChk = false;
                        for (i = 0; i < socketStateA.Length; i++)
                        {
                            if (socketStateA[i] == 0)       //0 = 배출완료
                            {
                                if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(sNum, i, true) == true)
                                {
                                    //배출했는 소켓인데 제품이 없으면 알람
                                    Console.WriteLine($"#{i + 1} Socket Product Unload Fail");
                                    bErrChk = true;
                                }
                                Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State = Machine.SocketProductState.Blank;
                            }
                        }
                        if (bErrChk)
                        {
                            szLog = $"[AUTO] SOCKET PRODUCT UNLOAD FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep *= -1;
                            break;
                        }
                        nRetStep = 390;
                        break;
                    }
                    break;
                case 390:
                    Globalo.motionManager.socketEEpromMachine.IsTesting[sNum] = false;      //배출 완료 후
                    nRetStep = 100;
                    break;
                //--------------------------------------------------------------------------------------------------------------------------
                //
                //
                //  WRITE
                //
                //
                //--------------------------------------------------------------------------------------------------------------------------
                case 400:
                    //Write 진행
                    WritePositionOccupied = true;       //#1 SOCKET WRITE 위치 점유
                    
                    nRetStep = 410;
                    nSocketTimeTick[sNum] = Environment.TickCount;
                    break;
                case 410:
                    //Y 실린더 확인후 Write 이동 후 진행
                    if (Globalo.motionManager.socketEEpromMachine.GetSocketFor(false) == true)
                    {
                        szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 412;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[sNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 412:
                    //컨택 실린더 상승 확인 8개 전체
                    bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(0, true);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] WRITE CONTACT UP CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nStep *= -1;
                        break;
                    }
                    szLog = $"[READY] WRITE CONTACT UP CHECK [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);


                    bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(1, true);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] VERIFY CONTACT UP CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nStep *= -1;
                        break;
                    }

                    szLog = $"[READY] VERIFY CONTACT UP CHECK [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 413;
                    break;
                case 413:
                    nRetStep = 414;
                    break;
                case 414:
                    nRetStep = 416;
                    break;
                case 416:
                    
                    bRtn = Globalo.motionManager.socketEEpromMachine.Socket_X_Move(Machine.EEpromSocketMachine.eTeachingPosList.WRITE_POS, Machine.eEEpromSocket.BACK_X, false);

                    if (bRtn == false)
                    {
                        szLog = $"[AUTO] BACK SOCKET X WRITE POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[AUTO] BACK SOCKET X WRITE POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nSocketTimeTick[sNum] = Environment.TickCount;
                    nRetStep = 418;
                    break;
                case 418:
                    //BACK SOCKET X 축 대기위치 이동 확인
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.BACK_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.ChkMotorXPos(Machine.EEpromSocketMachine.eTeachingPosList.WRITE_POS, Machine.eEEpromSocket.BACK_X))
                    {

                        VerifyPositionOccupied = false;
                        szLog = $"[AUTO] BACK SOCKET WRITE 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 420;
                        break;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[sNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] BACK SOCKET WRITE 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 420:
                    //#1 WRITE 진행
                    if (Write_Task == null || Write_Task.IsCompleted)
                    {
                        Task_Wait_Write = 1;
                        Write_Task = Task.Run(() =>
                        {
                            Task_Wait_Write = Task_EEpromWrite_Flow(sNum);
                            Console.WriteLine($"-------------- Task EEpromWrite Flow Task - end {Task_Wait_Write}");

                            return Task_Wait_Write;
                        }, CancelTokenSocket.Token);

                        nRetStep = 430;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[AUTO] Complete Task EEpromWrite Flow Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;

                case 430:
                    if (Task_Wait_Write == 1)
                    {
                        break;
                    }
                    if (Task_Wait_Write == -1)       //완료 실패
                    {
                        //Write 완료
                        szLog = $"[AUTO] BACK SOCKET WRITE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = -420;
                        break;
                    }
                    if (Task_Wait_Write == 0)       //정상종료
                    {
                        //Write 완료
                        szLog = $"[AUTO] #{sNum+1} SOCKET WRITE COMPLETE [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 450;
                        break;
                    }
                    break;
                case 450:
                    nRetStep = 490;
                    break;
                case 490:
                    isWriteBusy = false;    //X소켓 Write 완료
                    nRetStep = 100;
                    Globalo.motionManager.socketEEpromMachine.IsTesting[sNum] = false;
                    break;
                //--------------------------------------------------------------------------------------------------------------------------
                //
                //
                //  VERIFY
                //
                //
                //--------------------------------------------------------------------------------------------------------------------------
                case 500:
                    //Verify 진행
                    //Y 소켓 대기중이고 Y 실린더 후진 상태일때
                    VerifyPositionOccupied = true;       //#1 SOCKET WRITE 위치 점유
                    nRetStep = 505;
                    nSocketTimeTick[sNum] = Environment.TickCount;
                    break;
                case 510:
                    //Y 실린더 확인후 Write 이동 후 진행
                    if (Globalo.motionManager.socketEEpromMachine.GetSocketFor(false) == true)
                    {
                        szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 512;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[sNum] > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 512:
                    //컨택 실린더 상승 확인 8개 전체
                    bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(0, true);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] WRITE CONTACT UP CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nStep *= -1;
                        break;
                    }
                    szLog = $"[READY] WRITE CONTACT UP CHECK [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);


                    bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(1, true);
                    if (bRtn == false)
                    {
                        szLog = $"[READY] VERIFY CONTACT UP CHECK FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nStep *= -1;
                        break;
                    }

                    szLog = $"[READY] VERIFY CONTACT UP CHECK [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nRetStep = 513;
                    break;
                case 513:
                    nRetStep = 514;
                    break;
                case 514:
                    nRetStep = 516;
                    break;
                case 516:
                    szLog = $"[AUTO] FRONT SOCKET X VERIFY POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    bRtn = Globalo.motionManager.socketEEpromMachine.Socket_X_Move(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.BACK_X, false);

                    if (bRtn == false)
                    {
                        szLog = $"[READY] BACK SOCKET X VERIFY POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[READY] BACK SOCKET X VERIFY POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nSocketTimeTick[sNum] = Environment.TickCount;
                    nRetStep = 518;
                    break;
                case 518:
                    //BACK SOCKET X 축 대기위치 이동 확인
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.BACK_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.ChkMotorXPos(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.BACK_X))
                    {

                        WritePositionOccupied = false;
                        szLog = $"[AUTO] BACK SOCKET VERIFY 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 520;
                        break;
                    }
                    else if (Environment.TickCount - nSocketTimeTick[sNum] > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[AUTO] BACK SOCKET VERIFY 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 520:
                    //#1 VERIFY 진행

                    if (Verify_Task == null || Verify_Task.IsCompleted)
                    {
                        Task_Wait___Verify = 1;
                        Verify_Task = Task.Run(() =>
                        {
                            Task_Wait___Verify = Task_EEprom_Verify_Flow(sNum);


                            Console.WriteLine($"-------------- Task EEprom Verify Flow Task - end {Task_Wait___Verify}");

                            return Task_Wait___Verify;
                        }, CancelTokenSocket.Token);

                        nRetStep = 430;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[AUTO] Complete Task EEpromVerify Flow Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 530:
                    if (Task_Wait___Verify == 1)
                    {
                        break;
                    }
                    if (Task_Wait___Verify == -1)       //완료 실패
                    {
                        //#1 Verify 완료
                        nRetStep = -520;
                        szLog = $"[AUTO] #{sNum + 1} SOCKET VERIFY FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        break;
                    }
                    if (Task_Wait___Verify == 0)       //정상종료
                    {
                        //Write 완료
                        szLog = $"[AUTO] #{sNum + 1} SOCKET VERIFY COMPLETE [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        nRetStep = 550;
                        break;
                    }
                    
                    break;
                case 550:
                    
                    nRetStep = 590;
                    break;

                case 590:
                    //Verify 완료
                    isVerifyBusy = false;   //X소켓 Verify 완료
                    Globalo.motionManager.socketEEpromMachine.IsTesting[sNum] = false;
                    nRetStep = 100;
                    break;
            }

            return nRetStep;
        }

        //-----------------------------------------------------------------------------------------------------------
        //▲
        //▼
        //  위, 아래 Y Cylinder + X축 소켓 Flow
        //
        //
        //-----------------------------------------------------------------------------------------------------------
        public int Auto_Yx_Socket(int nStep)
        {
            int i = 0;
            string szLog = "";
            bool result = false;
            int nRetStep = nStep;
            const int sNum = 1;
            if (InterLock[sNum] == true)
            {
                return nRetStep;
            }
            switch (nStep)
            {
                case 100:
                    if (Globalo.motionManager.socketEEpromMachine.IsTesting[sNum] == false)
                    {
                        break;
                    }
                    if (InterLock[sNum] == true)
                    {
                        break;
                    }
                    nRetStep = 120;
                    break;
                case 120:
                    if (socketProcessState[sNum] == SocketState.Write)       //전부다 Verify 완료 될때까지
                    {
                        Console.WriteLine("Write Start");
                        nRetStep = 400;
                        break;
                    }
                    if (socketProcessState[sNum] == SocketState.Verify)       //전부다 Write 완료 될 때까지
                    {
                        Console.WriteLine("Verify Start");
                        nRetStep = 500;
                        break;
                    }

                    if (socketProcessState[sNum] == SocketState.UnLoadReq)        //전부가 검사가 끝났을때 (양품 + NG)
                    {
                        Console.WriteLine("Good Unload Req");
                        nRetStep = 300;
                        break;
                    }

                    if (socketProcessState[sNum] == SocketState.LoadReq)   //전부다 비었을때만, 공급 요청
                    {
                        Console.WriteLine("Load Req");
                        nRetStep = 200;
                        break;
                    }
                    break;
                case 200:
                    //공급 요청
                    socketState_B = new int[] { 1, 1, 1, 1 };
                    Globalo.motionManager.socketEEpromMachine.RaiseProductCall(sNum, socketState_B);        //공급 요청 초기화, Auto_Waiting
                    nRetStep = 220;
                    break;
                case 220:
                    //공급 완료 대기
                    socketState_B = Globalo.motionManager.GetSocketReq(sNum);  //공급완료

                    if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(sNum, 0, true) == true)
                    {
                        //Write 진행
                        nRetStep = 400;
                    }
                    
                    break;
                case 300:
                    //배출 요청
                    break;
                case 320:
                    //배출 완료 대기
                    break;
                case 400:
                    //Write 진행
                    break;
                case 500:
                    //Verify 진행
                    break;
            }

            return nRetStep;
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

                    if (Globalo.motionManager.socketEEpromMachine.IsTesting[0] == false)
                    {
                        socketProcessState[0] = SocketState.Wait;
                        socketStateA = new int[] { -1, -1, -1, -1 };
                        Globalo.motionManager.socketEEpromMachine.RaiseProductCall(0, socketStateA);         //공급 요청 초기화, Auto_Waiting

                        //-----------------------------------------------------------------------------------------------------------------------------------
                        //
                        //
                        //  A소켓 좌우 이동하는 후면 ◀ ▶
                        //
                        //
                        //-----------------------------------------------------------------------------------------------------------------------------------
                        socketStateA = RtnSocketState(0, Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A);
                        //
                        //
                        bool containsWrite3 = Array.Exists(socketStateA, state => state == 3);
                        bool containsVerify4 = Array.Exists(socketStateA, state => state == 4);
                        int socketA = RtnSocketAll(socketStateA);

                        if (socketA == 1)       //4개 전부 없음
                        {
                            if (VerifyPositionOccupied == false)        //verify 영역에서 공급
                            {
                                VerifyPositionOccupied = true;
                                socketProcessState[0] = SocketState.LoadReq;    //공급 요청
                            }
                        }
                        else if (containsWrite3 == true)
                        {
                            if (isWriteBusy == false && WritePositionOccupied == false)
                            {
                                isWriteBusy = true;
                                socketProcessState[0] = SocketState.Write;  //Write 진행 - Write 해야될 제품이 1개 이상 존재
                            }
                        }
                        else if (containsVerify4 == true)
                        {
                            if(isVerifyBusy == false && VerifyPositionOccupied == false)
                            {
                                isVerifyBusy = true;
                                socketProcessState[0] = SocketState.Verify; //Verify 진행 - Verify 해야될 제품이 1개 이상 존재
                            }
                            
                        }
                        else
                        {
                            if(VerifyPositionOccupied == false)     //verify 영역에서 배출
                            {
                                VerifyPositionOccupied = true;
                                socketProcessState[0] = SocketState.UnLoadReq;  //배출, 양품 or 불량
                            }
                            
                        }

                        if(socketProcessState[0] != SocketState.Wait)
                        {
                            Globalo.motionManager.socketEEpromMachine.IsTesting[0] = true;      //true로 바꿔야 socket flow 진행 가능
                        }
                        
                    }
                    //
                    //===============================================================================================================================================
                    //
                    if (Globalo.motionManager.socketEEpromMachine.IsTesting[1] == false)
                    {
                        socketProcessState[1] = SocketState.Wait;
                        socketState_B = new int[] { -1, -1, -1, -1 };
                        Globalo.motionManager.socketEEpromMachine.RaiseProductCall(1, socketState_B);        //공급 요청 초기화, Auto_Waiting

                        //-----------------------------------------------------------------------------------------------------------------------------------
                        //
                        //
                        //  B소켓 위,아래 이동하는 전면  ↑↓
                        //
                        //
                        //-----------------------------------------------------------------------------------------------------------------------------------
                        socketState_B = RtnSocketState(1, Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_B);
                        //
                        //
                        bool containsWrite3 = Array.Exists(socketState_B, state => state == 3);
                        bool containsVerify4 = Array.Exists(socketState_B, state => state == 4);

                        int socket_B = RtnSocketAll(socketState_B);

                        if (socket_B == 1)       //4개 전부 없음
                        {
                            //Y실린더 후진이면 가능
                            socketProcessState[1] = SocketState.LoadReq;    //공급 요청
                        }
                        else if (containsWrite3 == true)
                        {
                            if (isWriteBusy == false && WritePositionOccupied == false)
                            {
                                socketProcessState[1] = SocketState.Write;  //Write 진행 - Write 해야될 제품이 1개 이상 존재
                            }
                                
                        }
                        else if (containsVerify4 == true)
                        {
                            if (isVerifyBusy == false && VerifyPositionOccupied == false)
                            {
                                socketProcessState[1] = SocketState.Verify; //Verify 진행 - Verify 해야될 제품이 1개 이상 존재
                            }
                                
                        }
                        else
                        {
                            //Y실린더 후진이면 가능
                            socketProcessState[1] = SocketState.UnLoadReq;  //배출, 양품 or 불량
                        }

                        Globalo.motionManager.socketEEpromMachine.IsTesting[1] = true;      //true로 바꿔야 socket flow 진행 가능
                    }
                    break;
            }

            return nRetStep;
        }
        #endregion
        #region [TASK EEPROM WRITE]
        
        private int Task_EEpromWrite_Flow(int socketIndex)
        {
            int i = 0;
            int nRtn = -1;
            bool bRtn = false;
            int nStep = 10;
            int nTaskTimeTick = 0;
            string szLog = "";
            const int sTaskNum = 0;
            while (true)
            {
                if (CancelTokenSocket.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Task EEpromWrite Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                switch (nStep)
                {
                    case 10:
                        //모터 와 실린더 Y 움직임은 다른 곳에서
                        //여기선 컨택 동작과 검사만 진행
                        //
                        nTaskTimeTick = Environment.TickCount;
                        nStep = 15;
                        break;
                    case 15:
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(0, true);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] WRITE CONTACT UP CHECK FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nStep *= -1;
                            break;
                        }
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(1, true);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] VERIFY CONTACT UP CHECK FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nStep *= -1;
                            break;
                        }
                        nStep = 20;
                        break;
                    case 20:
                        if (socketIndex == 0)
                        {
                            //소켓 x축 Write 위치인지 확인
                            if (Globalo.motionManager.socketEEpromMachine.ChkMotorXPos(Machine.EEpromSocketMachine.eTeachingPosList.WRITE_POS, Machine.eEEpromSocket.BACK_X) == false)
                            {
                                szLog = $"[READY] BACK SOCKET WRITE 위치 확인 실패 [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                                nStep *= -1;
                                break;
                            }
                            else
                            {
                                szLog = $"[READY] BACK SOCKET WRITE 위치 확인 [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog);
                            }
                        }
                        else
                        {
                            //소켓 Yx축 Write 위치인지 확인 - Y실린더 전진상태인지 확인
                            if (Globalo.motionManager.socketEEpromMachine.ChkMotorXPos(Machine.EEpromSocketMachine.eTeachingPosList.WRITE_POS, Machine.eEEpromSocket.FRONT_XY) == false)
                            {
                                szLog = $"[READY] BACK SOCKET WRITE 위치 확인 실패 [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                                nStep *= -1;
                                break;
                            }
                            else
                            {
                                szLog = $"[READY] FRONT SOCKET WRITE 위치 확인 [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog);
                            }

                            if (Globalo.motionManager.socketEEpromMachine.GetSocketFor(true) == false)
                            {
                                szLog = $"[AUTO] SOCKET CYLINDER FOR CHECK FAIL[STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                                nStep *= -1;
                                break;
                            }
                            else
                            {
                                szLog = $"[READY] SOCKET CYLINDER 전진 확인 [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog);
                            }
                        }
                        nStep = 30;
                        break;
                    case 30:
                        bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactUp(sTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] WRITE CONTACT UP [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 40;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[AUTO] WRITE CONTACT UP FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        break;
                    case 40:
                        //컨택 상승 확인
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(sTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] WRITE CONTACT UP CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 60;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTaskTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] WRITE CONTACT UP CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        
                        break;
                    case 60:
                        //컨택 전진
                        bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactFor(sTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] WRITE CONTACT FOR [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 80;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[AUTO] WRITE CONTACT FOR FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        
                        break;
                    case 80:
                        //컨택 전진 확인
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactFor(sTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] WRITE CONTACT FOR CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 100;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTaskTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] WRITE CONTACT FOR CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        
                        break;
                    case 100:
                        if (Environment.TickCount - nTaskTimeTick > 300)
                        {
                            nStep = 120;
                        }
                        
                        break;
                    case 120:
                        //컨택 하강
                        bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactUp(sTaskNum, false);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] WRITE CONTACT DOWN [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 140;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[AUTO] WRITE CONTACT DOWN FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        
                        break;
                    case 140:
                        //컨택 하강 확인
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(sTaskNum, false);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] WRITE CONTACT DOWN CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 160;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTaskTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] WRITE CONTACT DOWN CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        
                        break;
                    case 160:
                        if (Environment.TickCount - nTaskTimeTick > 300)
                        {
                            nStep = 200;
                        }
                        break;
                    case 200:
                        //Write Start
                        //Client Send

                        //WRITE 진행
                        TcpSocket.EquipmentData sendEqipData2 = new TcpSocket.EquipmentData();
                        sendEqipData2.Command = "WRITE_GO";

                        for (i = 0; i < 4; i++)     //for (i = 0; i < socketStateA.Length; i++)
                        {
                            Globalo.motionManager.socketEEpromMachine.Tester_A_Result[i] = 0;
                            if (socketIndex == 0)
                            {
                                if (socketStateA[i] == 0)
                                {
                                    Globalo.motionManager.socketEEpromMachine.Tester_A_Result[i] = -1;
                                    Globalo.tcpManager.SendMessageToClient(sendEqipData2, i);
                                }
                            }
                            else
                            {
                                if (socketState_B[i] == 0)
                                {
                                    Globalo.motionManager.socketEEpromMachine.Tester_A_Result[i] = -1;
                                    Globalo.tcpManager.SendMessageToClient(sendEqipData2, i + 4);
                                }
                            }
                        }
                        nStep = 220;
                        break;
                    case 220:
                        nStep = 240;
                        break;
                    case 240:
                        //bool allZero = Globalo.motionManager.socketEEpromMachine.Tester_A_Result.All(x => x == 0);    //모두 0이면 검사 완료
                        bool allchk = Globalo.motionManager.socketEEpromMachine.Tester_A_Result.All(x => x != -1);      //전부 -1이 아닌지 , eeprom 으로부터 결과 받았는지 체크
                        if (allchk)    
                        {
                            for (i = 0; i < Globalo.motionManager.socketEEpromMachine.Tester_A_Result.Length; i++)
                            {
                                if (Globalo.motionManager.socketEEpromMachine.Tester_A_Result[i] == 1)  //1 양품
                                {
                                    Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State = Machine.SocketProductState.Verifying;
                                }
                                else if (Globalo.motionManager.socketEEpromMachine.Tester_A_Result[i] == 2)  //2 Ng
                                {
                                    Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State = Machine.SocketProductState.NG;
                                }
                                else
                                {
                                    Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State = Machine.SocketProductState.Blank;
                                }
                            }
                            //
                            //Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A
                            nStep = 260;
                        }
                        //Write 종료 대기
                        break;
                    case 260:
                        nStep = 300;
                        break;
                    case 300:
                        //Write 컨택 상승
                        bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactUp(sTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] WRITE CONTACT UP [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 320;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[AUTO] WRITE CONTACT UP FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }

                        
                        break;
                    case 320:
                        //컨택 상승 확인
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(sTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] WRITE CONTACT UP CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 340;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTaskTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] WRITE CONTACT UP CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        break;
                    case 340:
                        //컨택 후진
                        bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactFor(sTaskNum, false);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] WRITE CONTACT BACK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 360;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[AUTO] WRITE CONTACT BACK FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }

                        break;
                    case 360:
                        //컨택 후진 확인
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactFor(sTaskNum, false);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] WRITE CONTACT BACK CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 380;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTaskTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[AUTO] WRITE CONTACT BACK CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        break;
                    case 380:
                        if (socketIndex == 0)
                        {
                            nStep = 500;
                            break;
                        }
                        else
                        {
                            //Yx 소켓만 Y축 후진
                            //Y 실린더 후진
                            if (Globalo.motionManager.socketEEpromMachine.SocketFor(false) == true)
                            {
                                szLog = $"[AUTO] SOCKET CYLINDER BACK MOTION [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog);
                                nStep = 400;

                                nTimeTick = Environment.TickCount;
                            }
                            else
                            {
                                szLog = $"[AUTO] SOCKET CYLINDER BACK MOTION FAIL[STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                                nStep *= -1;
                                break;
                            }

                        }
                        
                        break;
                    case 400:
                        //Y 실린더 후진 확인
                        if (Globalo.motionManager.socketEEpromMachine.GetSocketFor(false) == true)
                        {
                            szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 500;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK TIMEOUT[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nStep *= -1;
                            break;
                        }

                        
                        break;
                    case 500:
                        nStep = 800;
                        break;
                    case 800:
                        nStep = 900;
                        break;
                    case 900:
                        
                        nStep = 1000;
                        break;
                    default:
                        break;
                }
                if (nStep < 0)
                {
                    Console.WriteLine("Gantry LoadTray Flow - fail");
                    break;
                }

                if (nStep == 1000)
                {
                    Console.WriteLine("Gantry LoadTray Flow - end");
                    break;
                }
                Thread.Sleep(10);       //TODO: while문안에서는 최소 10ms 꼭 필요
            }

            if (nStep == 1000)
            {
                nRtn = 0;
                Console.WriteLine("Task EEpromWrite Flow ok!");
            }
            else
            {
                nRtn = -1;
                Console.WriteLine("Task EEpromWrite Flow Fail!");
            }
            return nRtn;
            
        }
        #endregion

        #region [TASK EEPROM VERIFY]

        private int Task_EEprom_Verify_Flow(int socketIndex)
        {
            int i = 0;
            int nRtn = -1;
            bool bRtn = false;
            int nStep = 10;
            int nTaskTimeTick = 0;
            string szLog = "";
            const int sVTaskNum = 1;     //1 = Verify
            while (true)
            {
                if (CancelTokenSocket.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Task EEprom Verifyte Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                switch (nStep)
                {
                    case 10:
                        //모터 와 실린더 Y 움직임은 다른 곳에서
                        //여기선 컨택 동작과 검사만 진행
                        //
                        nTaskTimeTick = Environment.TickCount;
                        nStep = 15;
                        break;
                    case 15:
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(0, true);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] WRITE CONTACT UP CHECK FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nStep *= -1;
                            break;
                        }
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(1, true);
                        if (bRtn == false)
                        {
                            szLog = $"[READY] VERIFY CONTACT UP CHECK FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nStep *= -1;
                            break;
                        }
                        nStep = 20;
                        break;
                    case 20:
                        if (socketIndex == 0)
                        {
                            //소켓 x축 Verify 위치인지 확인
                            if (Globalo.motionManager.socketEEpromMachine.ChkMotorXPos(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.BACK_X) == false)
                            {
                                szLog = $"[READY] BACK SOCKET VERIFY 위치 확인 실패 [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                                nStep *= -1;
                                break;
                            }
                            else
                            {
                                szLog = $"[READY] BACK SOCKET VERIFY 위치 확인 [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog);
                            }
                        }
                        else
                        {
                            //소켓 Yx축 Verify 위치인지 확인 - Y실린더 전진상태인지 확인
                            if (Globalo.motionManager.socketEEpromMachine.ChkMotorXPos(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.FRONT_XY) == false)
                            {
                                szLog = $"[READY] BACK SOCKET VERIFY 위치 확인 실패 [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                                nStep *= -1;
                                break;
                            }
                            else
                            {
                                szLog = $"[READY] FRONT SOCKET VERIFY 위치 확인 [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog);
                            }

                            if (Globalo.motionManager.socketEEpromMachine.GetSocketFor(true) == false)
                            {
                                szLog = $"[AUTO] SOCKET CYLINDER FOR CHECK FAIL[STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                                nStep *= -1;
                                break;
                            }
                            else
                            {
                                szLog = $"[READY] SOCKET CYLINDER 전진 확인 [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog);
                            }
                        }
                        nStep = 30;
                        break;
                    case 30:
                        bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactUp(sVTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] VERIFY CONTACT UP [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 40;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[AUTO] VERIFY CONTACT UP FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        break;
                    case 40:
                        //컨택 상승 확인
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(sVTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] VERIFY CONTACT UP CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 60;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTaskTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] VERIFY CONTACT UP CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }

                        break;
                    case 60:
                        //컨택 전진
                        bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactFor(sVTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] VERIFY CONTACT FOR [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 80;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[AUTO] VERIFY CONTACT FOR FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }

                        break;
                    case 80:
                        //컨택 전진 확인
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactFor(sVTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] VERIFY CONTACT FOR CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 100;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTaskTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] VERIFY CONTACT FOR CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }

                        break;
                    case 100:
                        if (Environment.TickCount - nTaskTimeTick > 300)
                        {
                            nStep = 120;
                        }

                        break;
                    case 120:
                        //컨택 하강
                        bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactUp(sVTaskNum, false);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] VERIFY CONTACT DOWN [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 140;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[AUTO] VERIFY CONTACT DOWN FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }

                        break;
                    case 140:
                        //컨택 하강 확인
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(sVTaskNum, false);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] VERIFY CONTACT DOWN CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 160;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTaskTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] VERIFY CONTACT DOWN CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }

                        break;
                    case 160:
                        if (Environment.TickCount - nTaskTimeTick > 300)
                        {
                            nStep = 200;
                        }
                        break;
                    case 200:
                        //Write Start
                        //Client Send

                        //WRITE 진행
                        TcpSocket.EquipmentData sendEqipData2 = new TcpSocket.EquipmentData();
                        sendEqipData2.Command = "VERIFY_GO";

                        for (i = 0; i < 4; i++)     //for (i = 0; i < socketStateA.Length; i++)
                        {
                            Globalo.motionManager.socketEEpromMachine.Tester_B_Result[i] = 0;
                            if (socketIndex == 0)
                            {
                                if (socketStateA[i] == 0)
                                {
                                    Globalo.motionManager.socketEEpromMachine.Tester_B_Result[i] = -1;
                                    Globalo.tcpManager.SendMessageToClient(sendEqipData2, i);
                                }
                            }
                            else
                            {
                                if (socketState_B[i] == 0)
                                {
                                    Globalo.motionManager.socketEEpromMachine.Tester_B_Result[i] = -1;
                                    Globalo.tcpManager.SendMessageToClient(sendEqipData2, i + 4);
                                }
                            }
                        }
                        nStep = 220;
                        break;
                    case 220:
                        nStep = 240;
                        break;
                    case 240:
                        //bool allZero = Globalo.motionManager.socketEEpromMachine.Tester_A_Result.All(x => x == 0);    //모두 0이면 검사 완료
                        bool allchk = Globalo.motionManager.socketEEpromMachine.Tester_B_Result.All(x => x != -1);      //전부 -1이 아닌지 , eeprom 으로부터 결과 받았는지 체크
                        if (allchk)
                        {
                            for (i = 0; i < Globalo.motionManager.socketEEpromMachine.Tester_B_Result.Length; i++)
                            {
                                if (Globalo.motionManager.socketEEpromMachine.Tester_B_Result[i] == 1)  //1 양품
                                {
                                    Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State = Machine.SocketProductState.Good;
                                }
                                else if (Globalo.motionManager.socketEEpromMachine.Tester_B_Result[i] == 2)  //2 Ng
                                {
                                    Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State = Machine.SocketProductState.NG;
                                }
                                else
                                {
                                    Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State = Machine.SocketProductState.Blank;
                                }
                            }
                            //
                            nStep = 260;
                        }
                        //Verify 종료 대기
                        break;
                    case 260:
                        nStep = 300;
                        break;
                    case 300:
                        //Verify 컨택 상승
                        bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactUp(sVTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] VERIFY CONTACT UP [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 320;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[AUTO] VERIFY CONTACT UP FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }


                        break;
                    case 320:
                        //컨택 상승 확인
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactUp(sVTaskNum, true);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] VERIFY CONTACT UP CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 340;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTaskTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[READY] VERIFY CONTACT UP CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        break;
                    case 340:
                        //컨택 후진
                        bRtn = Globalo.motionManager.socketEEpromMachine.MultiContactFor(sVTaskNum, false);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] VERIFY CONTACT BACK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 360;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else
                        {
                            szLog = $"[AUTO] VERIFY CONTACT BACK FAIL [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }

                        break;
                    case 360:
                        //컨택 후진 확인
                        bRtn = Globalo.motionManager.socketEEpromMachine.GetMultiContactFor(sVTaskNum, false);
                        if (bRtn)
                        {
                            szLog = $"[AUTO] VERIFY CONTACT BACK CEHCK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 380;
                            nTaskTimeTick = Environment.TickCount;
                        }
                        else if (Environment.TickCount - nTaskTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[AUTO] VERIFY CONTACT BACK CHECK TIMEOUT [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep *= -1;
                            break;
                        }
                        break;
                    case 380:
                        if (socketIndex == 0)
                        {
                            nStep = 500;
                            break;
                        }
                        else
                        {
                            //Yx 소켓만 Y축 후진
                            //Y 실린더 후진
                            if (Globalo.motionManager.socketEEpromMachine.SocketFor(false) == true)
                            {
                                szLog = $"[AUTO] SOCKET CYLINDER BACK MOTION [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog);
                                nStep = 400;

                                nTimeTick = Environment.TickCount;
                            }
                            else
                            {
                                szLog = $"[AUTO] SOCKET CYLINDER BACK MOTION FAIL[STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                                nStep *= -1;
                                break;
                            }

                        }

                        break;
                    case 400:
                        //Y 실린더 후진 확인
                        if (Globalo.motionManager.socketEEpromMachine.GetSocketFor(false) == true)
                        {
                            szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK [STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                            nStep = 500;
                        }
                        else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            szLog = $"[AUTO] SOCKET CYLINDER BACK CHECK TIMEOUT[STEP : {nStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nStep *= -1;
                            break;
                        }


                        break;
                    case 500:
                        nStep = 900;
                        break;
                    case 900:

                        nStep = 1000;
                        break;
                    default:
                        break;
                }
                if (nStep < 0)
                {
                    Console.WriteLine("Task EEprom Verify Flow - fail");
                    break;
                }

                if (nStep == 1000)
                {
                    Console.WriteLine("Task EEprom Verify Flow - end");
                    break;
                }
                Thread.Sleep(10);       //TODO: while문안에서는 최소 10ms 꼭 필요
            }

            if (nStep == 1000)
            {
                nRtn = 0;
                Console.WriteLine("Task EEprom Verify Flow ok!");
            }
            else
            {
                nRtn = -1;
                Console.WriteLine("Task EEprom Verify Flow Fail!");
            }
            return nRtn;

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
                    Globalo.motionManager.socketEEpromMachine.RaiseProductCall(0, new int[] { -1, -1, -1, -1 });         //#1 Socket 요청 초기화 , Ready
                    Globalo.motionManager.socketEEpromMachine.RaiseProductCall(1, new int[] { -1, -1, -1, -1 });         //#2 Socket 요청 초기화 , Ready

                    Globalo.motionManager.socketEEpromMachine.IsTesting[0] = false; //Ready
                    Globalo.motionManager.socketEEpromMachine.IsTesting[1] = false; //Ready

                    

                    nRetStep = 2020;
                    break;
                case 2020:
                    nRetStep = 2040;
                    break;
                case 2040:
                    //WRITE 컨택 상승
                    if (Globalo.motionManager.socketEEpromMachine.MultiContactUp(0, true) == true)
                    {
                        szLog = $"[READY] WRITE SOCKET CONTACT UP MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2050;

                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[READY] WRITE SOCKET CONTACT UP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2050:
                    //WRITE 컨택 상승 확인
                    if (Globalo.motionManager.socketEEpromMachine.MultiContactUp(0, true) == true)
                    {
                        szLog = $"[READY] WRITE SOCKET CONTACT UP CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2060;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] WRITE SOCKET CONTACT UP CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2060:
                    //VERIFY 컨택 상승
                    if (Globalo.motionManager.socketEEpromMachine.MultiContactUp(1, true) == true)
                    {
                        szLog = $"[READY] VERIFY SOCKET CONTACT UP MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2070;

                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[READY] VERIFY SOCKET CONTACT UP MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2070:
                    //VERIFY 컨택 상승 확인
                    if (Globalo.motionManager.socketEEpromMachine.MultiContactUp(1, true) == true)
                    {
                        szLog = $"[READY] VERIFY SOCKET CONTACT UP CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2080;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] VERIFY SOCKET CONTACT UP CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2080:
                    //WRITE 컨택 후진
                    if (Globalo.motionManager.socketEEpromMachine.MultiContactFor(0, false) == true)
                    {
                        szLog = $"[READY] WRITE SOCKET CONTACT BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2090;

                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[READY] WRITE SOCKET CONTACT BACK MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2090:
                    //WRITE 컨택 후진 확인
                    if (Globalo.motionManager.socketEEpromMachine.GetMultiContactFor(0, false) == true)
                    {
                        szLog = $"[READY] WRITE SOCKET CONTACT BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2100;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] WRITE SOCKET CONTACT BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2100:
                    //VERIFY 컨택 후진
                    if (Globalo.motionManager.socketEEpromMachine.MultiContactFor(1, false) == true)
                    {
                        szLog = $"[READY] VERIFY SOCKET CONTACT BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2110;

                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[READY] VERIFY SOCKET CONTACT BACK MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2110:
                    //VERIFY 컨택 상승 확인
                    if (Globalo.motionManager.socketEEpromMachine.MultiContactFor(1, false) == true)
                    {
                        szLog = $"[READY] VERIFY SOCKET CONTACT BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2120;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] VERIFY SOCKET CONTACT BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2120:
                    nRetStep = 2140;
                    break;
                case 2140:
                    //Y 실린더 후진
                    if (Globalo.motionManager.socketEEpromMachine.SocketFor(false) == true)
                    {
                        szLog = $"[READY] SOCKET CYLINDER BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2160;

                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[READY] SOCKET CYLINDER BACK MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }


                    break;
                case 2160:
                    //Y 실린더 후진 확인
                    if (Globalo.motionManager.socketEEpromMachine.GetSocketFor(false) == true)
                    {
                        szLog = $"[READY] SOCKET CYLINDER BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2180;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] SOCKET CYLINDER BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2180:
                    nRetStep = 2200;
                    break;
                case 2200:
                    //FRONT SOCKET X 축 대기위치 이동
                    //BACK SOCKET X 축 대기위치 이동
                    bRtn = Globalo.motionManager.socketEEpromMachine.Socket_X_Move(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.FRONT_XY, false);

                    if (bRtn == false)
                    {
                        szLog = $"[READY] FRONT SOCKET X VERIFY POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[READY] FRONT SOCKET X VERIFY POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    bRtn = Globalo.motionManager.socketEEpromMachine.Socket_X_Move(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.BACK_X, false);

                    if (bRtn == false)
                    {
                        szLog = $"[READY] BACK SOCKET X VERIFY POS MOVE FAIL [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    szLog = $"[READY] BACK SOCKET X VERIFY POS MOVE [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nTimeTick = Environment.TickCount;

                    nRetStep = 2220;
                    break;
                case 2220:
                    //FRONT SOCKET X 축 대기위치 이동 확인
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.FRONT_XY].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.ChkMotorXPos(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.FRONT_XY))
                    {
                        szLog = $"[READY] FRONT SOCKET VERIFY 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2240;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] FRONT VERIFY 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2240:
                    //BACK SOCKET X 축 대기위치 이동 확인
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.BACK_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.ChkMotorXPos(Machine.EEpromSocketMachine.eTeachingPosList.VERIFY_POS, Machine.eEEpromSocket.BACK_X))
                    {
                        szLog = $"[READY] BACK SOCKET VERIFY 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2260;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] BACK SOCKET VERIFY 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2260:
                    nRetStep = 2280;
                    break;
                case 2280:
                    //BACK SOCKET 1,2,3,4 제품 감지 확인
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(0, i, true) == true)
                        {
                            szLog = $"[READY] BACK SOCKET {i+1} PRODUCT DETECTED [STEP : {nStep}]";
                            //제품 있는데 BLANK면 알람
                            if(Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State == Machine.SocketProductState.Blank)
                            {
                                szLog = $"[READY] X Socket #{i+1} Product status error [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog);
                                nRetStep *= -1;
                                break;
                            }
                        }
                        else
                        {
                            szLog = $"[READY] BACK SOCKET {i + 1} PRODUCT EMPTY [STEP : {nStep}]";
                            Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State = Machine.SocketProductState.Blank;
                            //BLANK로 변경
                        }
                        Globalo.LogPrint("ManualControl", szLog);
                    }


                    //socketProcessState[0] = Machine.SocketProductState.Blank;
                    //socketProcessState[1] = Machine.SocketProductState.Blank;

                    nRetStep = 2300;
                    break;
                case 2300:
                    //FRONT SOCKET 1,2,3,4 제품 감지 확인
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(1, i, true) == true)
                        {
                            szLog = $"[READY] FRONT SOCKET {i + 1} PRODUCT DETECTED [STEP : {nStep}]";
                            //제품 있는데 BLANK면 알람
                            if (Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_B[i].State == Machine.SocketProductState.Blank)
                            {
                                szLog = $"[READY] Yx Socket #{i + 1} Product status error [STEP : {nStep}]";
                                Globalo.LogPrint("ManualControl", szLog);
                                nRetStep *= -1;
                                break;
                            }
                        }
                        else
                        {
                            szLog = $"[READY] FRONT SOCKET {i + 1} PRODUCT EMPTY [STEP : {nStep}]";
                            Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_B[i].State = Machine.SocketProductState.Blank;
                            //BLANK로 변경
                        }
                        Globalo.LogPrint("ManualControl", szLog);
                    }
                    nRetStep = 2400;
                    break;

                case 2400:
                    //TODO: 소켓 4개중에서 없으면 괜찮은데, 제품이 있는경우 모두 일치하지 않으면 진행 XXX
                    nRetStep = 2500;
                    break;
                case 2500:
                    nRetStep = 2600;
                    break;
                case 2600:
                    nRetStep = 2900;
                    break;
                case 2900:

                    isWriteBusy = false;
                    isVerifyBusy = false;
                    WritePositionOccupied = false;
                    VerifyPositionOccupied = true;      //x축 소켓이 점유

                    Globalo.motionManager.socketEEpromMachine.RunState = OperationState.Standby;
                    szLog = $"[READY] EEPROM SOCKET 운전준비 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 3000;
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
                    //좌측이 Verify
                    //우측이 Write
                    nRetStep = 1020;
                    break;
                case 1020:
                    nRetStep = 1040;
                    break;
                case 1040:
                    //Write 컨택 4개 전체 상승
                    //Verify 컨택 4개 전체 상승
                    nRetStep = 1060;
                    break;
                case 1060:
                    //Write 컨택 4개 전체 상승 확인
                    nRetStep = 1080;
                    break;
                case 1080:
                    //Verify 컨택 4개 전체 상승 확인
                    nRetStep = 1100;
                    break;
                case 1100:
                    //Write 컨택 4개 전체 후진
                    //Verify 컨택 4개 전체 후진
                    nRetStep = 1120;
                    break;
                case 1120:
                    //Y 소켓 실린더 후진
                    if (Globalo.motionManager.socketEEpromMachine.SocketFor(false) == true)
                    {
                        szLog = $"[READY] SOCKET CYLINDER BACK MOTION [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1140;

                        nTimeTick = Environment.TickCount;
                    }
                    else
                    {
                        szLog = $"[READY] SOCKET CYLINDER BACK MOTION FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 1140:
                    //Y 소켓 실린더 후진 확인
                    if (Globalo.motionManager.socketEEpromMachine.GetSocketFor(false) == true)
                    {
                        szLog = $"[READY] SOCKET CYLINDER BACK CHECK [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1160;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                    {
                        szLog = $"[READY] SOCKET CYLINDER BACK CHECK TIMEOUT[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 1160:
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.FRONT_XY].GetStopAxis() == false)
                    {
                        Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.FRONT_XY].Stop();
                        break;
                    }
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.BACK_X].GetStopAxis() == false)
                    {
                        Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.BACK_X].Stop();
                        break;
                    }

                    dSpeed = (15 * -1);      //-1은 왼쪽, 하강 이동

                    bRtn = Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.FRONT_XY].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] FRONT SOCKET (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] FRONT SOCKET (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);



                    bRtn = Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.BACK_X].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit);
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] BACK SOCKET (-)Limit 위치 구동 실패 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    szLog = $"[ORIGIN] BACK SOCKET (-)Limit 위치 구동 성공 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 1180;
                    break;
                case 1180:

                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.FRONT_XY].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.FRONT_XY].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] FRONT SOCKET (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1200;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                    {
                        szLog = $"[ORIGIN] FRONT SOCKET (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 1200:
                    if (Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.BACK_X].GetStopAxis() == true &&
                        Globalo.motionManager.socketEEpromMachine.MotorAxes[(int)Machine.eEEpromSocket.BACK_X].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] BACK SOCKET (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1220;
                        nTimeTick = Environment.TickCount;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                    {
                        szLog = $"[ORIGIN] BACK SOCKET (-)Limit 위치 이동 시간 초과 [STEP : {nStep}]";
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
                        for (int i = 0; i < Globalo.motionManager.socketEEpromMachine.MotorAxes.Length; i++)
                        {
                            Globalo.motionManager.socketEEpromMachine.MotorAxes[i].OrgState = true;
                        }

                        nRetStep = 1900;
                        break;
                    }
                    bRtn = true;
                    for (int i = 0; i < Globalo.motionManager.socketEEpromMachine.MotorAxes.Length; i++)
                    {
                        Globalo.motionManager.socketEEpromMachine.MotorAxes[i].OrgState = false;

                        //Home Method Setting
                        uint duZPhaseUse = 0;
                        double dHomeClrTime = 2000.0;
                        double dHomeOffset = 0.0;

                        //++ 지정한 축의 원점검색 방법을 변경합니다.
                        duRetCode = CAXM.AxmHomeSetMethod(
                            Globalo.motionManager.socketEEpromMachine.MotorAxes[i].m_lAxisNo,
                            (int)Globalo.motionManager.socketEEpromMachine.MotorAxes[i].HomeMoveDir,
                            (uint)Globalo.motionManager.socketEEpromMachine.MotorAxes[i].HomeDetect,
                            duZPhaseUse, dHomeClrTime, dHomeOffset);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.socketEEpromMachine.MotorAxes[i].Name} AxmHomeSetMethod Fail [STEP : {nStep}]";
                            Globalo.LogPrint("LIft", szLog);
                        }

                        duRetCode = CAXM.AxmHomeSetVel(
                            Globalo.motionManager.socketEEpromMachine.MotorAxes[i].m_lAxisNo,
                            Globalo.motionManager.socketEEpromMachine.MotorAxes[i].FirstVel,
                            Globalo.motionManager.socketEEpromMachine.MotorAxes[i].SecondVel,
                            Globalo.motionManager.socketEEpromMachine.MotorAxes[i].ThirdVel,
                            50.0, 0.3, 0.3);//LastVel, Acc Firset, Acc Second


                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.socketEEpromMachine.MotorAxes[i].Name} AxmHomeSetVel Fail [STEP : {nStep}]";
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
                    for (int i = 0; i < Globalo.motionManager.socketEEpromMachine.MotorAxes.Length; i++)
                    {
                        duRetCode = CAXM.AxmHomeSetStart(Globalo.motionManager.socketEEpromMachine.MotorAxes[i].m_lAxisNo);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.socketEEpromMachine.MotorAxes[i].Name} AxmHomeSetStart Fail [STEP : {nStep}]";
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
                    for (int i = 0; i < Globalo.motionManager.socketEEpromMachine.MotorAxes.Length; i++)
                    {
                        CAXM.AxmHomeGetResult(Globalo.motionManager.socketEEpromMachine.MotorAxes[i].m_lAxisNo, ref duState);
                        if (duState == (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS)
                        {
                            //원점 완료
                            Globalo.motionManager.socketEEpromMachine.MotorAxes[i].OrgState = true;
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
                    Globalo.motionManager.socketEEpromMachine.RunState = OperationState.OriginDone;
                    szLog = $"[ORIGIN] SOCKET UNIT 전체 원점 위치 이동 완료 [STEP : {nStep}]";
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


        private int[] RtnSocketState(int index, List<Machine.SocketProductInfo> socketState)
        {
            int i = 0;
            int[] tempState = { -1, -1, -1, -1 };
            for (i = 0; i < 4; i++)
            {
                if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(index, i, true) == false)
                {
                    tempState[i] = 1;        //1 = 공급요청
                    socketState[i].State = Machine.SocketProductState.Blank;
                }
                else
                {
                    //있으면 검사 or 배출 요청
                    if (socketState[i].State == Machine.SocketProductState.Writing)        //Writing 할 차례
                    {
                        //write 미완료
                        tempState[i] = 3;
                    }
                    else if (socketState[i].State == Machine.SocketProductState.Verifying)        //verify 할 차례
                    {
                        //verify 미완료
                        tempState[i] = 4;
                    }
                    else if (socketState[i].State == Machine.SocketProductState.Good || socketState[i].State == Machine.SocketProductState.NG)
                    {
                        //write + verify 완료
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

                if (socketStateA[i] != first)
                {
                    return -1;
                }
            }

            return first; // 모두 같음 (1 또는 2)
        }
    }
}
