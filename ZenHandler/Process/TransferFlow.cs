using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Process
{
    public class TransferFlow
    {
        private readonly SynchronizationContext _syncContext;
        public int nTimeTick = 0;
        public int[] SensorSet = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] OrgOnGoing = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public TransferFlow()
        {
            _syncContext = SynchronizationContext.Current;
        }
        //
        //  3000
        //
        #region [TRANSFER 작업 분기]
       
        public int Auto_Waiting(int nStep)
        {
            string szLog = "";
            bool PickerCheck = false;
            int i = 0;
            Machine.TransferMachine.eTeachingPosList Move_Pos;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 3000:
                    nRetStep = 3100;
                    break;
                case 3100:
                    //확인 순서
                    //0.TRAY가 제품 배출할 수 있는 상태인지
                    //1.배출 피커에 배출할 제품을 들고 있는지
                    //
                    //2.로드 피커에 투입할 제품이 4개 다 찼는지 , 항상 가로 한줄씩 4개 다 채워서 시작해야된다..
                    //
                    //2.TRAY가 제품 로드할 수 있는 상태인지
                    //3.로드 피커가 비었으면 제품 로드하기

                    //소켓의 투입 요청인지 , 배출 요청인지 판단해서 진행
                    nRetStep = 3120;
                    break;
                case 3120:
                    nRetStep = 3140;
                    break;
                case 3140:
                    nRetStep = 3160;
                    break;
                case 3160:
                    nRetStep = 3180;
                    break;
                case 3180:
                    nRetStep = 3200;
                    break;
                case 3200:
                    //NG 배출 제품 로드
                    PickerCheck = false;
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.TestNg ||
                            Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.TestNg2 ||
                            Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.TestNg3 ||
                            Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.TestNg4)
                        {
                            PickerCheck = true;
                            break;
                        }
                    }
                    if (PickerCheck == true)
                    {
                        nRetStep = 8000;
                        break;
                    }
                    nRetStep = 3220;
                    break;
                case 3220:
                    //양품 배출 제품 로드
                    PickerCheck = false;
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Good ||
                            Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Good ||
                            Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Good ||
                            Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Good)
                        {
                            PickerCheck = true;
                            break;
                        }
                    }
                    if (PickerCheck == true)
                    {
                        nRetStep = 7000;
                        break;
                    }
                    nRetStep = 3240;
                    break;
                case 3240:
                    nRetStep = 3260;
                    break;
                case 3260:
                    //투입할 제품 로드한 상태

                    //1.소켓 배출 요청 체크
                    //2.소켓 투입 요청 체크
                    PickerCheck = false;
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Bcr &&
                            Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Bcr &&
                            Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Bcr &&
                            Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Bcr)
                        {
                            PickerCheck = true;
                            break;
                        }
                    }
                    if (PickerCheck == true)
                    {
                        
                        if (i == 0)
                        {
                            //6000 - 소켓 배출 우선 진행
                            //몇 번 소켓인지 검색 (0 , 1 , 2 , 3)
                            nRetStep = 6000;
                            break;

                        }
                        else if (i == 1)
                        {
                            //5000 - 소켓 투입
                            nRetStep = 5000;
                            break;
                        }

                    }

                    nRetStep = 3280;
                    break;
                case 3280:
                    nRetStep = 3300;
                    break;
                case 3300:
                    //양쪽 다 들고있는 제품 없는 경우
                    //리프트 로드가능할 때 -->리프트로 바코드 스캔 이동
                    //배출 요청오면 소켓에 제품 배출하러
                    PickerCheck = false;
                    for (i = 0; i < 4; i++)
                    {
                        if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Blank &&
                            Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Blank &&
                            Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Blank &&
                            Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Blank &&
                            Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Blank &&
                            Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Blank &&
                            Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Blank &&
                            Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Blank)
                        {
                            PickerCheck = true;
                            break;
                        }
                    }
                    if (PickerCheck == true)
                    {
                        if (i == 0) //TRAY 교체중이 아니고 , 자동운전 중이고, 로드가능한 상태 일때
                        {
                            nRetStep = 3400;
                            break;
                        }

                        if (i == 0)
                        {
                            //6000 - 소켓 배출 우선 진행
                            //몇 번 소켓인지 검색 (0 , 1 , 2 , 3)
                            nRetStep = 6000;
                            break;

                        }
                    }
                    nRetStep = 3000;    //Back to First
                    break;

                //-----------------------------------------------------------------------
                //
                //  바코드 로드 진행
                //
                //-----------------------------------------------------------------------
                case 3400:
                    
                    nRetStep = 3500;
                    break;
                case 3500:

                    nRetStep = 3600;
                    break;
                case 3600:
                    if (Globalo.motionManager.transferMachine.TrayPosition == MotionControl.MotorSet.TrayPosition.Left)
                    {
                        Move_Pos = Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_BCR_POS;
                    }
                    else
                    {
                        Move_Pos = Machine.TransferMachine.eTeachingPosList.RIGHT_TRAY_BCR_POS;
                    }
                    Globalo.motionManager.transferMachine.TransFer_XY_Move(Move_Pos);

                    nRetStep = 3700;
                    nTimeTick = Environment.TickCount;
                    break;
                case 3700://MotorAxes[(int)eTransfer.TRANSFER_X]
                    if (Globalo.motionManager.transferMachine.TrayPosition == MotionControl.MotorSet.TrayPosition.Left)
                    {
                        Move_Pos = Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_BCR_POS;
                    }
                    else
                    {
                        Move_Pos = Machine.TransferMachine.eTeachingPosList.RIGHT_TRAY_BCR_POS;
                    }


                    if (Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.ChkXYMotorPos(Move_Pos))
                    {
                        szLog = $"[ORIGIN] {Move_Pos.ToString()} 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 3720;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] {Move_Pos.ToString()} 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 3720:
                    Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.WAIT_POS);
                    nRetStep = 3740;
                    nTimeTick = Environment.TickCount;
                    break;
                case 3740:
                    if (Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.ChkXYMotorPos(Machine.TransferMachine.eTeachingPosList.WAIT_POS))
                    {
                        szLog = $"[ORIGIN] WAIT_POS 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 3760;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[ORIGIN] WAIT_POS 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 3760:
                    nRetStep = 3780;
                    break;
                case 3780:
                    nRetStep = 3800;
                    break;
                case 3800:
                    nRetStep = 3820;
                    break;
                case 3820:
                    nRetStep = 3840;
                    break;
                case 3840:
                    nRetStep = 3900;
                    break;

                case 3900:
                    nRetStep = 4000;
                    break;
            }
            return nRetStep;
        }
        #endregion
        //
        //  4000
        //
        #region [TRANSFER BCR SCAN]
        
        public int Auto_BcrLoadInTray(int nStep)
        {
            string szLog = "";
            int i = 0;
            int LoadPosx = 0;
            int LoadPosy = 0;
            int nRetStep = nStep;
            bool bRtn = false;

            //TODO: LIFT , MAGAZINE 유닛 자동중인지 체크, TRAY 교체 중인지 꼭 체크 
            switch (nStep)
            {
                case 4000:
                    //Bcr x , y, z 모터 이동
                    //바코드 스캔
                    //Load x , y 모터 이동
                    //제품 픽업
                    //next Bcr x, y 모터 이동
                    //반복
                    //픽업 4개 모두 로드시 or x 좌표 4일때
                    //
                    //소켓 배출 요청 확인 / 투입 요청 확인
                    nRetStep = 4100;
                    break;
                case 4100:
                    nRetStep = 4200;
                    break;
                case 4200:
                    //Bcr x , y 모터 이동
                    LoadPosx = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.X;
                    LoadPosy = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.Y;
                    //public bool TransFer_XY_Move(eTeachingPosList ePos, int PickerNo = 0, int CountX = 0, int CountY = 0,  bool bWait = true)
                    Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_BCR_POS , LoadPosx, LoadPosy);
                    nRetStep = 4220;
                    break;
                case 4220:
                    //x, y 위치 확인
                    nRetStep = 4240;
                    break;
                case 4240:
                    // Bcr z 모터 이동
                    Globalo.motionManager.transferMachine.TransFer_Z_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_BCR_POS);
                    nRetStep = 4260;
                    break;
                case 4260:
                    //z 위치 확인
                    bRtn = Globalo.motionManager.transferMachine.ChkZMotorPos(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_BCR_POS);
                    nRetStep = 4300;
                    break;
                case 4300:
                    Globalo.motionManager.transferMachine.CurrentScanBcr = "";
                    //바코드 스캔
                    Globalo.tcpManager.BcrClient.bRecvBcrScan = false;
                    Globalo.tcpManager.BcrClient.Send("LON\r");
                    nRetStep = 4320;
                    break;
                case 4320:
                    //스캔 대기
                    if (Globalo.tcpManager.BcrClient.bRecvBcrScan == true)
                    {
                        //if (Globalo.motionManager.transferMachine.CurrentScanBcr.Length < 1)
                        //{
                        //    //Scan Fail
                        //    szLog = $"[ORIGIN] BCR SCAN DATA ERR [STEP : {nStep}]";
                        //    Globalo.LogPrint("ManualControl", szLog);
                        //    nRetStep *= -1;
                        //    break;
                        //}
                        szLog = $"[ORIGIN] BCR SCAN OK:({Globalo.motionManager.transferMachine.CurrentScanBcr}) [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 4340;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.BCR_SCAN_TIMEOUT)
                    {
                        //TODO: RETRY ?? , PASS ?? , 
                        DialogResult result = DialogResult.None;
                        _syncContext.Send(_ =>
                        {
                            result = Globalo.MessageAskPopup("Retry barcode scan?");
                        }, null);
                        if (result == DialogResult.Yes)
                        {
                            nRetStep = 4300;
                        }
                        szLog = $"[ORIGIN] BCR SCAN TIMEOUT [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 4340:
                    nRetStep = 4360;
                    break;
                case 4360:
                    nRetStep = 4380;
                    break;
                case 4380:
                    //z 축 대기 위치로 이동
                    Globalo.motionManager.transferMachine.TransFer_Z_Move(Machine.TransferMachine.eTeachingPosList.WAIT_POS);
                    nRetStep = 4390;
                    break;
                case 4390:
                    //z 축 대기 위치로 이동
                    bRtn = Globalo.motionManager.transferMachine.ChkZMotorPos(Machine.TransferMachine.eTeachingPosList.WAIT_POS);
                    nRetStep = 4400;
                    break;
                case 4400:
                    //Load x , y 모터 이동
                    LoadPosx = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.X;
                    LoadPosy = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.Y;

                    Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS, LoadPosx, LoadPosy);
                    nRetStep = 4420;
                    break;
                case 4420:
                    //Load x , y 위치 확인
                    nRetStep = 4440;
                    break;
                case 4440:
                    Globalo.motionManager.transferMachine.TransFer_Z_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS);
                    nRetStep = 4460;
                    break;
                case 4460:
                    //Load z 위치 확인
                    bRtn = Globalo.motionManager.transferMachine.ChkZMotorPos(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS);
                    nRetStep = 4480;
                    break;
                case 4480:
                    nRetStep = 4500;
                    break;
                case 4500:
                    //Picker 하강
                    LoadPosx = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.X;

                    Globalo.motionManager.transferMachine.LoadPickerUp(LoadPosx, false);        //Picker 하강
                    nRetStep = 4510;
                    break;

                case 4510:
                    //Picker 하강 확인
                    nRetStep = 4520;
                    break;
                case 4520:
                    //흡착 or 그립
                    nRetStep = 4540;
                    break;
                case 4540:
                    //흡착 or 그립 확인
                    LoadPosx = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.X;
                    Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[LoadPosx].BcrLot = Globalo.motionManager.transferMachine.CurrentScanBcr;       //Bcr Scan ,Load
                    Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[LoadPosx].State = Machine.PickedProductState.Bcr;       //Bcr Scan ,Load
                    Globalo.pickerInfo.SetLoadPickerInfo();                 //피커 상태 Display
                    Globalo.motionManager.transferMachine.TaskSave();       //피커 상태 Save
                    //
                    //
                    //흡착 확인되면 Count 1 증가
                    //
                    // ++1
                    LoadPosx = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.X;
                    LoadPosy = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.Y;

                    if (Globalo.motionManager.transferMachine.TrayPosition == MotionControl.MotorSet.TrayPosition.Left)
                    {
                        Globalo.motionManager.liftMachine.trayProduct.LeftLoadTraySlots[LoadPosy][LoadPosx] = -1;
                    }
                    else
                    {
                        Globalo.motionManager.liftMachine.trayProduct.RightLoadTraySlots[LoadPosy][LoadPosx] = -1;
                    }
                    Globalo.motionManager.liftMachine.trayProduct.LeftLoadTraySlots[LoadPosy][LoadPosx] = -1;// (int)Dlg.TrayStateInfo.LoadTraySlotState.Empty;
                    Globalo.trayStateInfo.SetUpdateLoadTray(Dlg.TrayStateInfo.TRAY_KIND.LOAD_TRAY_L);
                    Globalo.motionManager.transferMachine.LoadTryAdd(1);        //여기서 Load 픽업 위치 로드한 개수만큼 증가
                    nRetStep = 4560;
                    break;
                case 4560:
                    //Picker 상승
                    Globalo.motionManager.transferMachine.LoadPickerUp(LoadPosx, true);        //Picker 상승
                    nRetStep = 4580;
                    break;
                case 4580:
                    //Picker 상승 확인
                    nRetStep = 4590;
                    break;
                case 4590:
                    nRetStep = 4600;
                    break;
                case 4600:
                    //Z 축 대기 위치이동
                    Globalo.motionManager.transferMachine.TransFer_Z_Move(Machine.TransferMachine.eTeachingPosList.WAIT_POS);
                    nRetStep = 4620;
                    break;
                case 4620:
                    //Z 축 대기 위치이동 확인
                    bRtn = Globalo.motionManager.transferMachine.ChkZMotorPos(Machine.TransferMachine.eTeachingPosList.WAIT_POS);
                    nRetStep = 4640;
                    break;
                case 4640:
                    nRetStep = 4700;
                    break;
                case 4700:
                    nRetStep = 4800;
                    break;
                case 4800:
                    nRetStep = 4850;
                    break;
                case 4850:
                    
                    nRetStep = 4900;
                    break;

                case 4900:
                    int NextLoadX = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.X;  //0 -> 1 -> 2 -> 3   1씩 더해진 상황
                    if (NextLoadX < 0 || NextLoadX > 4)
                    {
                        szLog = $"[AUTO] BCR POS ERROR - {NextLoadX} [STEP : {nStep}]";
                        Globalo.LogPrint("TransferUnit", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    //피커 4개가 다 로드해야 , 한줄씩은다 로드하고 완료
                    //4개 모두다 확인해야된다.
                    bool ChkBlank = false;
                    for (i = NextLoadX; i < 4; i++)
                    {
                        if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Blank)   //TODO : 어떻게 반복할지 확인필요
                        {
                            Console.WriteLine("Blank Index : {i}");
                            ChkBlank = true;
                            break;
                        }
                    }

                    if (ChkBlank == true)
                    {
                        nRetStep = 4000;        //다음 제품 바코드 스캔후, 제품 로드 , 반복
                    }
                    else
                    {
                        nRetStep = 3000;        //대기 위치로 이동
                    }
                    break;
            }
            return nRetStep;
        }
        #endregion
        //
        //  5000
        //
        public int Auto_SocketInsert(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 5000:
                    nRetStep = 5900;
                    break;

                case 5900:

                    break;
            }
            return nRetStep;
        }
        //
        //  6000
        //
        public int Auto_SocketOutput(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 6000:
                    nRetStep = 6900;
                    break;

                case 6900:

                    break;
            }
            return nRetStep;
        }
        //
        //  7000
        //
        public int Auto_UnLoadInTray(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 7000:
                    nRetStep = 7020;
                    break;
                case 7020:
                    nRetStep = 7040;
                    break;
                case 7040:
                    int UnloadPosx = Globalo.motionManager.transferMachine.pickedProduct.UnloadTrayPos.X;
                    int UnloadPosy = Globalo.motionManager.transferMachine.pickedProduct.UnloadTrayPos.Y;

                    int CntUnload = Machine.TransferMachine.UnLoadCount;//1 or 2 or 4 개씩만 배출 /    3 = xxxxx

                    int StartIndex = UnloadPosx;// % CntUnload;
                    int EndIndex = UnloadPosx + CntUnload;/// - UnloadCnt;
                    if (EndIndex > 4)
                    {
                        EndIndex = 4;
                    }

                    int[] UnloadPicker = { -1, -1, -1, -1 };
                    //
                    for (int i = StartIndex; i < EndIndex; i++)
                    {
                        if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Good)
                        {
                            UnloadPicker[i] = 1;
                        }
                    }

                    Console.WriteLine("----------------------------------------------------");
                    Console.WriteLine($"배출개수 : {CntUnload}, Pos x : {UnloadPosx}");
                    Console.WriteLine($"피커 다운 범위 : {StartIndex} ~ {EndIndex}");
                    nRetStep = 7060;
                    break;
                case 7060:
                    nRetStep = 7080;
                    break;
                case 7080:
                    nRetStep = 7100;
                    break;
                case 7100:
                    nRetStep = 7120;
                    break;
                case 7120:
                    nRetStep = 7140;
                    break;
                case 7140:
                    nRetStep = 7900;
                    break;

                case 7900:
                    if (Globalo.motionManager.transferMachine.uphStartTime == DateTime.MinValue)
                    {
                        // 초기화되지 않음
                        TimeSpan elapsed = DateTime.Now - Globalo.motionManager.transferMachine.uphStartTime;
                        //double elapsedMinutes = elapsed.TotalMinutes;
                        //double elapsedSeconds = elapsed.TotalSeconds;
                        Globalo.productionInfo.ShowUphTime(elapsed.Seconds);

                        Globalo.motionManager.transferMachine.uphStartTime = DateTime.Now;
                    }
                    else
                    {
                        // 이미 값이 설정됨
                    }
                    
                    break;
            }
            return nRetStep;
        }
        #region [TRANSFER Home 원점 동작]
        
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

            //TODO: 모터 알람뜨는지 체크해서 정지해야될듯
            switch (nStep)
            {
                case 1000:
                    Console.WriteLine("[ORIGIN] TRANSFER START");
                    nRetStep = 1050;
                    break;
                case 1050:
                    bRtn = true;

                    for (int i = 0; i < Globalo.motionManager.transferMachine.MotorAxes.Length; i++)
                    {
                        if (Globalo.motionManager.transferMachine.MotorAxes[i].AmpEnable() == false)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.transferMachine.MotorAxes[i].Name} AmpEnable Fail]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                    }
                    if (bRtn == false)
                    {
                        szLog = $"[ORIGIN] Servo On Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    nRetStep = 1060;
                    break;
                case 1060:
                    //Load 실린더 전체 상승
                    bRtn = Globalo.motionManager.transferMachine.LoadMultiPickerUp(new int[] { 1, 1, 1, 1 }, true);//new int[] { 1, 1, 1, 1 }, true);
                    if (bRtn)
                    {
                        szLog = $"[ORIGIN] Transfer Load PIcker All Up [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1065;
                    }
                    else
                    {
                        szLog = $"[ORIGIN] Transfer Load PIcker All Up Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    
                    
                    break;
                case 1065:
                    //UnLoad 실린더 전체 상승
                    bRtn = Globalo.motionManager.transferMachine.UnloadMultiPickerUp(new int[] { 1, 1, 1, 1 }, true);
                    if (bRtn)
                    {
                        szLog = $"[ORIGIN] Transfer Unload PIcker All Up [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1070;
                    }
                    else
                    {
                        szLog = $"[ORIGIN] Transfer Unload PIcker All Up Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }


                    break;
                case 1070:
                    //Load 실린더 전체 상승 확인
                    bRtn = Globalo.motionManager.transferMachine.GetLoadMultiPickerUp(new int[] { 1, 1, 1, 1 }, true);
                    if (bRtn)
                    {
                        szLog = $"[ORIGIN] Transfer Load PIcker All Up Complete[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1080;
                    }
                    else
                    {
                        szLog = $"[ORIGIN] Transfer Load PIcker All Up Complete Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 1080:
                    bRtn = Globalo.motionManager.transferMachine.GetUnloadMultiPickerUp(new int[] { 1, 1, 1, 1 }, true);
                    if (bRtn)
                    {
                        szLog = $"[ORIGIN] Transfer Unload PIcker All Up Complete[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1090;
                    }
                    else
                    {
                        szLog = $"[ORIGIN] Transfer Unload PIcker All Up Complete Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 1090:
                    //z축 Limit 이동

                    if (Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Z].GetStopAxis() == false)
                    {
                        Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Z].Stop();
                        break;
                    }

                    //SensorSet[0] = (int)Globalo.motionManager.transferMachine.TransferZ.m_lAxisNo;
                    //SensorSet[1] = (int)AXT_MOTION_HOME_DETECT.NegEndLimit;
                    //SensorSet[2] = (int)AXT_MOTION_EDGE.SIGNAL_UP_EDGE;
                    //SensorSet[3] = (int)AXT_MOTION_STOPMODE.SLOWDOWN_STOP;

                    dSpeed = (15 * -1);      //-1은 왼쪽 이동

                    bRtn = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Z].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.EMERGENCY_STOP);
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

                    if (Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Z].GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Z].GetNegaSensor() == true)
                    {
                        szLog = $"[ORIGIN] TransferZ (-)Limit 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 1120;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
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
                    if (Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].GetStopAxis() == false)
                    {
                        Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].Stop();
                        break;
                    }

                    dSpeed = (10 * -1);      //-1은 왼쪽 이동

                    bRtn = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.EMERGENCY_STOP);
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
                    if (Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].GetStopAxis() == false)
                    {
                        Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].Stop();
                        break;
                    }

                    dSpeed = (10 * -1);      //-1은 왼쪽 이동

                    bRtn = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].MoveAxisLimit(dSpeed, dAcc, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_EDGE.SIGNAL_UP_EDGE, AXT_MOTION_STOPMODE.EMERGENCY_STOP);
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

                    if (Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].GetNegaSensor() == true)
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

                    if (Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].GetNegaSensor() == true)
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
                    nRetStep = 1200;
                    break;
                case 1200:
                    nRetStep = 1250;
                    break;
                case 1250:
                    szLog = $"[ORIGIN] Transfer X/Y/Z Limit 위치 이동 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 1260;
                    break;
                case 1260:
                    if (ProgramState.ON_LINE_MOTOR == false)
                    {
                        for (int i = 0; i < Globalo.motionManager.transferMachine.MotorAxes.Length; i++)
                        {
                            Globalo.motionManager.transferMachine.MotorAxes[i].OrgState = true;
                        }
                            
                        nRetStep = 1900;
                        break;
                    }
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
                            Globalo.motionManager.transferMachine.MotorAxes[i].FirstVel,
                            Globalo.motionManager.transferMachine.MotorAxes[i].SecondVel,
                            Globalo.motionManager.transferMachine.MotorAxes[i].ThirdVel,
                            50.0, 0.3, 0.3);//LastVel, Acc Firset, Acc Second


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

                    nRetStep = 1270;
                    break;
                case 1270:
                    bRtn = true;
                    for (int i = 0; i < Globalo.motionManager.transferMachine.MotorAxes.Length; i++)
                    {
                        duRetCode = CAXM.AxmHomeSetStart(Globalo.motionManager.transferMachine.MotorAxes[i].m_lAxisNo);

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            bRtn = false;
                            szLog = $"[ORIGIN] {Globalo.motionManager.transferMachine.MotorAxes[i].Name} AxmHomeSetStart Fail [STEP : {nStep}]";
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

                    nRetStep = 1280;
                    break;
                case 1280:
                    nRetStep = 1290;
                    break;
                case 1290:
                    m_bHomeProc = true;
                    m_bHomeError = false;
                    for (int i = 0; i < Globalo.motionManager.transferMachine.MotorAxes.Length; i++)
                    {
                        CAXM.AxmHomeGetResult(Globalo.motionManager.transferMachine.MotorAxes[i].m_lAxisNo, ref duState);
                        if (duState == (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS)
                        {
                            //원점 완료
                            Globalo.motionManager.transferMachine.MotorAxes[i].OrgState = true;
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
                            szLog = $"[ORIGIN] {Globalo.motionManager.transferMachine.MotorAxes[i].Name} HOME 동작 ERROR [STEP : {nStep}]";
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
                    Globalo.motionManager.transferMachine.RunState = OperationState.OriginDone;
                    szLog = $"[ORIGIN] TRANSFER UNIT 전체 원점 위치 이동 완료 [STEP : {nStep}]";
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

        #region [TRANSFER 운전 준비]

        public int AutoReady(int nStep)					//  운전준비(2000 ~ 3000)
        {
            string szLog = "";
            bool bRtn = false;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 2000:
                    DialogResult result = DialogResult.None;
                    _syncContext.Send(_ =>
                    {
                        result = Globalo.MessageAskPopup("READY?!\n(SPACE KEY START)");
                    }, null);
                    if (result == DialogResult.Yes)
                    {
                        nRetStep = 2020;
                    }
                    else
                    {
                        szLog = $"[READY] 자동운전 일시정지[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 2020:
                    //로드 실린더 전체 상승
                    //Load 실린더 전체 상승
                    bRtn = Globalo.motionManager.transferMachine.LoadMultiPickerUp(new int[] { 1, 1, 1, 1 }, true);
                    if (bRtn)
                    {
                        szLog = $"[READY] Transfer Load PIcker All Up [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2040;
                    }
                    else
                    {
                        szLog = $"[READY] Transfer Load PIcker All Up Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2040:
                    //배출 실린더 전체 상승
                    bRtn = Globalo.motionManager.transferMachine.UnloadMultiPickerUp(new int[] { 1, 1, 1, 1 }, true);
                    if (bRtn)
                    {
                        szLog = $"[READY] Transfer Unload PIcker All Up [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2060;
                    }
                    else
                    {
                        szLog = $"[READY] Transfer Unload PIcker All Up Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2060:
                    //로드 / 배출 실린더 상승 확인
                    bRtn = Globalo.motionManager.transferMachine.GetLoadMultiPickerUp(new int[] { 1, 1, 1, 1 }, true);
                    if (bRtn)
                    {
                        szLog = $"[READY] Transfer Load PIcker All Up Complete[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2080;
                    }
                    else
                    {
                        szLog = $"[READY] Transfer Load PIcker All Up Complete Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }
                    break;
                case 2080:
                    bRtn = Globalo.motionManager.transferMachine.GetUnloadMultiPickerUp(new int[] { 1, 1, 1, 1 }, true);
                    if (bRtn)
                    {
                        szLog = $"[READY] Transfer Unload PIcker All Up Complete[STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2100;
                    }
                    else
                    {
                        szLog = $"[READY] Transfer Unload PIcker All Up Complete Fail [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep *= -1;
                        break;
                    }

                    break;
                case 2100:
                    nRetStep = 2110;
                    break;
                case 2110:
                    nRetStep = 2120;
                    break;
                case 2120:
                    Globalo.motionManager.transferMachine.TransFer_Z_Move(Machine.TransferMachine.eTeachingPosList.WAIT_POS, true);
                    nTimeTick = Environment.TickCount;
                    nRetStep = 2130;
                    break;
                case 2130:
                    if (Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Z].GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.ChkZMotorPos(Machine.TransferMachine.eTeachingPosList.WAIT_POS))
                    {
                        szLog = $"[READY] TRANSFER Z WAIT_POS 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2140;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        szLog = $"[READY] TRANSFER Z WAIT_POS  이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                    
                    break;
                case 2140:
                    Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.WAIT_POS);
                    nTimeTick = Environment.TickCount;
                    nRetStep = 2150;
                    break;
                case 2150:
                    if (Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].GetStopAxis() == true && Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].GetStopAxis() == true &&
                        Globalo.motionManager.transferMachine.ChkXYMotorPos(Machine.TransferMachine.eTeachingPosList.WAIT_POS))
                    {
                        szLog = $"[READY] WAIT_POS 위치 이동 완료 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 2160;
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 30000)
                    {
                        szLog = $"[READY] WAIT_POS 위치 이동 시간 초과 [STEP : {nStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }
                   
                    break;
                case 2160:
                    //TODO: Picker 상태 확인하고 Blank 인데, 흡착감지면 알람
                    //TODO: Picker 상태가 제품이 있는데 , 탈착상태이면 알람
                    if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[0].State == Machine.PickedProductState.Blank)
                    {

                    }
                    if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[1].State == Machine.PickedProductState.Blank)
                    {

                    }
                    if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[2].State == Machine.PickedProductState.Blank)
                    {

                    }
                    if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[3].State == Machine.PickedProductState.Blank)
                    {

                    }
                    nRetStep = 2170;
                    break;
                case 2170:
                    if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[0].State == Machine.PickedProductState.Blank)
                    {

                    }
                    if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[1].State == Machine.PickedProductState.Blank)
                    {

                    }
                    if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[2].State == Machine.PickedProductState.Blank)
                    {

                    }
                    if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[3].State == Machine.PickedProductState.Blank)
                    {

                    }
                    nRetStep = 2180;
                    break;
                case 2180:
                    nRetStep = 2190;
                    break;
                case 2190:
                    nRetStep = 2200;
                    break;
                case 2200:
                    nRetStep = 2900;
                    break;
                case 2900:
                    Globalo.motionManager.transferMachine.RunState = OperationState.Standby;
                    szLog = $"[READY] TRANSFER 운전준비 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    nRetStep = 3000;
                    break;
            }
            return nRetStep;
        }
        #endregion

        //
        //  8000
        //
        public int Auto_Ng_UnLoading(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 8000:
                    nRetStep = 8900;
                    break;

                case 8900:
                    nRetStep = 3000;
                    break;
            }
            return nRetStep;
        }
        //
        //  9000
        //
        public int Auto_Cancel(int nStep)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 9000:
                    nRetStep = 9900;
                    break;

                case 9900:
                    nRetStep = 3000;
                    break;
            }
            return nRetStep;
        }
    }
}