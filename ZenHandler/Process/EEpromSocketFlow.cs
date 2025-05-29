using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public class EEpromSocketFlow
    {
        public CancellationTokenSource CancelTokenSocket;
        public ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);  // true면 동작 가능
        public Task<int> xSocketTask;
        private readonly SynchronizationContext _syncContext;
        public int nTimeTick = 0;
        private int[] socketStateA = { -1, -1, -1, -1 };     // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출
        private int[] socketState_B = { -1, -1, -1, -1 };    // 0은 공급완료, 배출 완료, 1: 공급 요청, 2: 양품 배출, 3: NG 배출


        private Machine.SocketProductState[] socketProcessState = new Machine.SocketProductState[2];
        public EEpromSocketFlow()
        {
            //대기위치에서 자동 시작 , 실린더는 후진 상태

            //EEPROM 설비는 소켓 하나마다 pc 1대 - 총 8대
            //8대 모두 연결 상태 확인후 진행

            //Back소켓은 좌우로만 이동
            //Fromt 소켓은 Y실린더 앞위이동 + 좌우 이동

            _syncContext = SynchronizationContext.Current;
            CancelTokenSocket = new CancellationTokenSource();
            xSocketTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
        }
        public int Auto_X_Socket(int nStep)
        {
            int i = 0;
            string szLog = "";
            bool result = false;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 100:
                    if (Globalo.motionManager.socketEEpromMachine.IsTesting[0] == false)
                    {
                        break;
                    }
                    //공급 요청 or 배출 요청
                    if (socketProcessState[0] == Machine.SocketProductState.Verifying)
                    {
                        nRetStep = 500;
                        break;
                    }
                    if (socketProcessState[0] == Machine.SocketProductState.Writing)
                    {
                        nRetStep = 400;
                        break;
                    }
                    if (socketProcessState[0] == Machine.SocketProductState.Good)
                    {
                        nRetStep = 300;
                        break;
                    }

                    if (socketProcessState[0] == Machine.SocketProductState.Blank)
                    {
                        nRetStep = 200;
                        break;
                    }
                    break;
                    //
                    //
                    //
                case 200:
                    //공급 요청
                    socketStateA = new int[] { 1, 1, 1, 1 };
                    Globalo.motionManager.socketEEpromMachine.RaiseProductCall(0, socketStateA);        //공급 요청 초기화, Auto_Waiting
                    break;
                case 220:
                    //공급 완료 대기
                    if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(0, 0, true) == true)
                    {
                        if (Globalo.motionManager.GetSocketReq(0, 0) == 0)      //공급완료
                        {
                            //Write 진행
                            socketProcessState[0] = Machine.SocketProductState.Writing;

                            //Y 실린더 확인후 Write 이동 후 진행
                            nRetStep = 400;
                            break;
                        }
                    }

                    break;
                    //
                    //
                    //
                case 300:
                    //배출 요청
                    break;
                case 320:
                    //배출 완료 대기
                    break;
                case 340:
                    //배출 완료
                    nRetStep = 200;
                    break;
                    //
                    //
                    //
                case 400:
                    //Write 진행
                    break;
                case 420:
                    //Write 완료 대기
                    break;
                case 440:
                    //Write 완료
                    //Y 실린더 확인후 Verify 이동 후 진행
                    socketProcessState[0] = Machine.SocketProductState.Verifying;
                    nRetStep = 500;
                    break;
                    //
                    //
                    //
                case 500:
                    //Verify 진행
                    break;
                case 520:
                    break;
                case 540:
                    //Verify 완료 대기
                    nRetStep = 300;
                    break;
                case 560:
                    //Verify 완료
                    socketProcessState[0] = Machine.SocketProductState.Good;
                    socketProcessState[0] = Machine.SocketProductState.NG;
                    break;
            }

            return nRetStep;
        }
        public int Auto_Yx_Socket(int nStep)
        {
            int i = 0;
            string szLog = "";
            bool result = false;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 100:
                    if (Globalo.motionManager.socketEEpromMachine.IsTesting[1] == false)
                    {
                        break;
                    }
                    //공급 요청 or 배출 요청
                    if (socketProcessState[1] == Machine.SocketProductState.Verifying)
                    {

                    }
                    if (socketProcessState[1] == Machine.SocketProductState.Writing)
                    {

                    }
                    if (socketProcessState[1] == Machine.SocketProductState.Good)
                    {

                    }

                    if (socketProcessState[1] == Machine.SocketProductState.Blank)
                    {
                        nRetStep = 200;
                        break;
                    }
                    
                    break;
                case 200:
                    //공급 요청
                    socketState_B = new int[] { 1, 1, 1, 1 };
                    Globalo.motionManager.socketEEpromMachine.RaiseProductCall(1, socketState_B);        //공급 요청 초기화, Auto_Waiting
                    break;
                case 220:
                    //공급 완료 대기
                    if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(10, 0, true) == true)
                    {
                        if (Globalo.motionManager.GetSocketReq(1, 0) == 0)      //공급완료
                        {
                            //Write 진행
                            nRetStep = 400;
                        }
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
                    if (Globalo.motionManager.socketEEpromMachine.IsTesting[0] == true)
                    {
                        break;
                    }
                    if (Globalo.motionManager.socketEEpromMachine.IsTesting[1] == true)
                    {
                        break;
                    }

                    socketProcessState[0] = Machine.SocketProductState.Blank;
                    socketProcessState[1] = Machine.SocketProductState.Blank;

                    socketStateA = new int[] { -1, -1, -1, -1 };
                    socketState_B = new int[] { -1, -1, -1, -1 };

                    Globalo.motionManager.socketEEpromMachine.RaiseProductCall(0, socketStateA);         //공급 요청 초기화, Auto_Waiting
                    Globalo.motionManager.socketEEpromMachine.RaiseProductCall(1, socketState_B);        //공급 요청 초기화, Auto_Waiting

                    //검사는 별도 Task 에서 진행

                    //-----------------------------------------------------------------------------------------------------------------------------------
                    //
                    //
                    //  A소켓 좌우 이동하는 후면 ◀ ▶
                    //
                    //
                    //-----------------------------------------------------------------------------------------------------------------------------------
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(0, i, true) == false)
                        {
                            socketStateA[i] = 1;        //1 = 공급요청
                            Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State = Machine.SocketProductState.Blank;
                            socketProcessState[0] = Machine.SocketProductState.Blank;
                        }
                        else
                        {
                            //있으면 검사 or 배출 요청
                            if (Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State == Machine.SocketProductState.Writing)
                            {
                                //검사 전
                                socketStateA[i] = 0;
                                socketProcessState[0] = Machine.SocketProductState.Writing;
                            }
                            if (Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State == Machine.SocketProductState.Verifying)
                            {
                                //write 완료
                                socketStateA[i] = 0;
                                socketProcessState[0] = Machine.SocketProductState.Verifying;
                            }
                            if (Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State == Machine.SocketProductState.Good)
                            {
                                //write + verify 완료
                                socketStateA[i] = 2; //양품 배출 요청
                                socketProcessState[0] = Machine.SocketProductState.Good;
                            }
                            if (Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[i].State == Machine.SocketProductState.NG)
                            {
                                socketStateA[i] = 3; //불량 배출 요청
                                socketProcessState[0] = Machine.SocketProductState.NG;
                            }
                            else
                            {
                                socketStateA[i] = 0;
                                socketProcessState[0] = Machine.SocketProductState.Writing;
                            }
                        }
                    }

                    //-----------------------------------------------------------------------------------------------------------------------------------
                    //
                    //
                    //  B소켓 위,아래 이동하는 전면  ↑↓
                    //
                    //
                    //-----------------------------------------------------------------------------------------------------------------------------------
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.socketEEpromMachine.GetIsProductInSocket(1, i, true) == false)
                        {
                            //제품 없음
                            socketState_B[i] = 1;       //1 = 공급요청
                            Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_B[i].State = Machine.SocketProductState.Blank;
                        }
                        else
                        {
                            //있으면 검사 or 배출 요청
                            if (Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_B[i].State == Machine.SocketProductState.Writing)
                            {
                                //검사 전 -> write 진행
                                socketState_B[i] = 0;
                            }
                            if (Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_B[i].State == Machine.SocketProductState.Verifying)
                            {
                                //write 완료 -> Verify 진행
                                socketState_B[i] = 0;
                            }
                            if (Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_B[i].State == Machine.SocketProductState.Good)
                            {
                                //write + verify 완료
                                socketState_B[i] = 2; //양품 배출 요청
                            }
                            if (Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_B[i].State == Machine.SocketProductState.NG)
                            {
                                socketState_B[i] = 3; //불량 배출 요청
                            }
                            else
                            {
                                socketState_B[i] = 0;
                            }
                        }
                    }

                    //bool allMinusOne = !socketStateA.Any(v => v != -1);  // 하나라도 -1이 아니면 false

                    //bool allSame_A = socketStateA.All(v => v == socketStateA[0]);       //4개가 전부 동일한지 파악
                    //bool allSame_B = socketState_B.All(v => v == socketState_B[0]);     //4개가 전부 동일한지 파악

                    Globalo.motionManager.socketEEpromMachine.IsTesting[0] = true;      //true로 바꿔야 socket flow 진행 가능
                    Globalo.motionManager.socketEEpromMachine.IsTesting[1] = true;      //true로 바꿔야 socket flow 진행 가능


                    break;
            }

            return nRetStep;
        }

        //X축 공급 요청 Flow - 완료 ->  제품 없으면 X-Y축 공급 요청 Flow
        //X축 Write 시작 Flow - x-y축이 제품있고 write완료 상태면 verify 진행 - X축 Write끝나면 Verify 진행
        //X축 Verify 시작 Flow - X-y축 공급 받았으면 write 시작

        private int X_Socket_LoadReqFlow()
        {
            int nRtn = -1;

            bool bRtn = false;
            int nRetStep = 10;
            string szLog = "";
            while (true)
            {
                if (CancelTokenSocket.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Gantry LoadTray Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                switch (nRetStep)
                {
                    case 10:
                        nRetStep = 20;
                        break;
                    case 20:
                        //X축 공급 요청 후 대기
                        nRetStep = 40;
                        break;
                    case 40:
                        //공급 완료 확인
                        //X-Y 소켓에 제품 없으면 공급 요청
                        nRetStep = 60;
                        break;
                    case 60:
                        //Write 진행
                        //컨택 상승 
                        nRetStep = 80;
                        break;
                    case 80:
                        //컨택 상승  확인
                        nRetStep = 100;
                        break;
                    case 100:
                        //컨택 전진
                        nRetStep = 120;
                        break;
                    case 120:
                        //컨택 전진 확인
                        nRetStep = 140;
                        break;
                    case 140:
                        //컨택 하강
                        nRetStep = 200;
                        break;
                    case 160:
                        //컨택 하강 확인
                        nRetStep = 200;
                        break;
                    case 180:
                        
                        nRetStep = 200;
                        break;
                    case 200:
                        //Write 진행  to Tcp Send  4개
                        nRetStep = 800;
                        break;
                    case 220:

                        break;
                    case 240:

                        break;
                    case 260:

                        break;
                    case 280:

                        break;
                    case 300:
                        //Write 완료 대기
                        nRetStep = 800;
                        break;
                    case 400:
                        //컨택 상승
                        nRetStep = 420;
                        break;
                    case 420:
                        //컨택 상승 확인
                        nRetStep = 440;
                        break;
                    case 440:
                        //컨택 후진
                        nRetStep = 460;
                        break;
                    case 460:
                        //컨택 후진 확인
                        nRetStep = 480;
                        break;
                    case 480:
                        //x-y축 실린더 후진 확인후
                        //Verify 위치로 이동

                        //
                        nRetStep = 500;
                        break;
                    case 500:
                        
                        
                        nRetStep = 800;
                        break;
                    case 600:
                        nRetStep = 800;
                        break;
                    case 800:

                        break;
                    case 900:
                        nRetStep = 1000;
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
        //X-Y축 공급 요청 Flow
        //X-Y축 Write 시작 Flow
        //X-Y축 Verify 시작 Flow
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

                    Globalo.motionManager.socketEEpromMachine.IsTesting[0] = false;
                    Globalo.motionManager.socketEEpromMachine.IsTesting[1] = false;
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
                    nRetStep = 2500;
                    break;
                case 2500:
                    nRetStep = 2600;
                    break;
                case 2600:
                    nRetStep = 2900;
                    break;
                case 2900:
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
    }
}
