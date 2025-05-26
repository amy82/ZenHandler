using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public class MagazineFlow
    {
        public CancellationTokenSource CancelTokenMagazine;
        public ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);  // true면 동작 가능
        private readonly SynchronizationContext _syncContext;
        public Task<int> LoadTrayTask;
        public Task<int> UnloadTrayTask;

        private int waitLoadTray = 1;
        private int waitUnloadTray = 1;
        public MagazineFlow()
        {
            _syncContext = SynchronizationContext.Current;
            CancelTokenMagazine = new CancellationTokenSource();
            LoadTrayTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
            UnloadTrayTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
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
                    nRetStep = 1900;
                    break;
                case 1900:
                    Globalo.motionManager.magazineHandler.RunState = OperationState.OriginDone;
                    szLog = $"[ORIGIN] MAGAZINE UNIT 전체 원점 위치 이동 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
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
                    nRetStep = 2020;
                    break;
                case 2900:
                    Globalo.motionManager.magazineHandler.RunState = OperationState.Standby;
                    szLog = $"[READY] LIFT 운전준비 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 3000;
                    break;
            }
            return nRetStep;
        }
        #endregion

        #region [Auto_Waiting]
        public int Auto_Waiting(int nStep)
        {
            string szLog = "";
            bool result = false;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 3000:
                    //요청 대기
                    //1.Tray 로드 from Magazine
                    //2.Tray 배출 to Magazine
                    //

                    Globalo.motionManager.magazineHandler.IsLoadingTray[0] = true;
                    Globalo.motionManager.magazineHandler.IsUnloadingTray[0] = true;

                    Globalo.motionManager.magazineHandler.IsLoadingTray[1] = true;
                    Globalo.motionManager.magazineHandler.IsUnloadingTray[1] = true;
                    break;
                case 3100:
                    //---------------------------------------------------
                    //  Tray 로드
                    //---------------------------------------------------
                    if (LoadTrayTask == null || LoadTrayTask.IsCompleted)
                    {
                        waitLoadTray = 1;
                        LoadTrayTask = Task.Run(() =>
                        {
                            waitLoadTray = MagazineTrayLoadFlow();
                            Console.WriteLine($"-------------- LoadTray Task - end {waitLoadTray}");

                            return waitLoadTray;
                        }, CancelTokenMagazine.Token);
                        nRetStep = 3120;
                    }
                    else
                    {
                        //일시정지
                        szLog = $"[AUTO] Tray Load Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 3120:
                    break;
                case 3140:
                    break;
                case 3160:
                    break;
                case 3180:
                    break;
                case 3200:
                    //---------------------------------------------------
                    //  Tray 배출
                    //---------------------------------------------------
                    if (UnloadTrayTask == null || UnloadTrayTask.IsCompleted)
                    {
                        waitUnloadTray = 1;
                        UnloadTrayTask = Task.Run(() =>
                        {
                            waitUnloadTray = MagazineTrayUnloadFlow();
                            Console.WriteLine($"-------------- UnloadTray Task - end {waitUnloadTray}");

                            return waitUnloadTray;
                        }, CancelTokenMagazine.Token);

                        nRetStep = 3220;
                    }
                    else
                    {
                        //Globalo.motionManager.liftMachine.IsUnloadingOutputTray = false;
                        
                        //일시정지
                        szLog = $"[AUTO] Complete Tray Unload Move Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 3220:
                    break;
                case 3240:
                    break;
                case 3260:
                    break;
                case 3280:
                    break;
                case 3300:

                    break;
                case 3400:

                    break;

            }
            return nRetStep;
        }
        #endregion

        #region [Tray_Load]
        private int MagazineTrayLoadFlow()
        {
            int nRtn = -1;

            bool bRtn = false;
            int nRetStep = 10;
            string szLog = "";
            while (true)
            {
                if (CancelTokenMagazine.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Gantry LoadTray Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                if (nRetStep != 100 && nRetStep != 120)
                {
                    //리프트 z축이 상승하는 스텝 에서는 일시정지 걸어도 센서 확인하고 정지할대까지 일시정지안함
                    //정지하면 정지함수에서 리프트 정지 시킴
                    pauseEvent.Wait();  // 일시정지시 여기서 멈춰 있음
                }
                switch (nRetStep)
                {
                    case 10:
                        nRetStep = 20;
                        break;
                    case 20:
                        nRetStep = 40;
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
        #endregion

        #region [Tray_Unload]
        private int MagazineTrayUnloadFlow()
        {
            int nRtn = -1;

            bool bRtn = false;
            int nRetStep = 10;
            string szLog = "";
            while (true)
            {
                if (CancelTokenMagazine.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Gantry LoadTray Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                if (nRetStep != 100 && nRetStep != 120)
                {
                    //리프트 z축이 상승하는 스텝 에서는 일시정지 걸어도 센서 확인하고 정지할대까지 일시정지안함
                    //정지하면 정지함수에서 리프트 정지 시킴
                    pauseEvent.Wait();  // 일시정지시 여기서 멈춰 있음
                }
                switch (nRetStep)
                {
                    case 10:
                        nRetStep = 20;
                        break;
                    case 20:
                        nRetStep = 40;
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
        #endregion
    }
}
