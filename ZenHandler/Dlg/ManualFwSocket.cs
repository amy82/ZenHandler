using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Dlg
{
    public partial class ManualFwSocket : UserControl
    {
        public bool bManualStopKey;
        //private int MovePos;
        
        private System.Windows.Forms.Timer ManualTimer;
        private Button[] Fw_Socket_BtnArr = new Button[4];         //소켓 구분



        private Button[] Fw_LT_PiecerBtnArr = new Button[4];         //제품감지 LT
        private Button[] Fw_RT_PiecerBtnArr = new Button[4];         //제품감지 RT
        private Button[] Fw_BL_PiecerBtnArr = new Button[4];         //제품감지 BL
        private Button[] Fw_BR_PiecerBtnArr = new Button[4];         //제품감지 BR


        private Button[] Fw_ContactUpBtnArr = new Button[8];      //컨택 Up / Down
        private Button[] Fw_ContactForBtnArr = new Button[8];     //컨택 For / back
        private Button[] Fw_RotateUpBtnArr = new Button[8];       //로테이트 Up / Down
        private Button[] Fw_RotateFlipBtnArr = new Button[8];     //로테이트 Flip / Down
        private Button[] Fw_RotateGripBtnArr = new Button[8];     //로테이트 Grip / Ungrip



        protected CancellationTokenSource cts;  //TODO: <--이름 변경하고 사용가능하게
        private bool isMovingMagazine;

        private int CurrentFwSocket = 0;        //0 = LFET , 1 = RIGHT
        //MEMO: 티칭 위치를 어떻게 가져올 것인가
        public ManualFwSocket()
        {
            InitializeComponent();
            bManualStopKey = false;
            cts = new CancellationTokenSource();

            isMovingMagazine = false;

            ManualTimer = new System.Windows.Forms.Timer();
            ManualTimer.Interval = 300; // 1초 (1000밀리초) 간격 설정
            ManualTimer.Tick += new EventHandler(Manual_Timer_Tick);


            ManualUiSet();
        }
        private void ManualUiSet()
        {
            int i = 0;
            //소켓 내 제품 감지
            Fw_Socket_BtnArr[0] = button_ManualFw_Socket_Select_LT;
            Fw_Socket_BtnArr[1] = button_ManualFw_Socket_Select_RT;
            Fw_Socket_BtnArr[2] = button_ManualFw_Socket_Select_BL;
            Fw_Socket_BtnArr[3] = button_ManualFw_Socket_Select_BR;

            Fw_LT_PiecerBtnArr[0] = button_ManualFw_Socket_LT_Piece_Detect1;
            Fw_LT_PiecerBtnArr[1] = button_ManualFw_Socket_LT_Piece_Detect2;
            Fw_LT_PiecerBtnArr[2] = button_ManualFw_Socket_LT_Piece_Detect3;
            Fw_LT_PiecerBtnArr[3] = button_ManualFw_Socket_LT_Piece_Detect4;

            Fw_RT_PiecerBtnArr[0] = button_ManualFw_Socket_RT_Piece_Detect1;
            Fw_RT_PiecerBtnArr[1] = button_ManualFw_Socket_RT_Piece_Detect2;
            Fw_RT_PiecerBtnArr[2] = button_ManualFw_Socket_RT_Piece_Detect3;
            Fw_RT_PiecerBtnArr[3] = button_ManualFw_Socket_RT_Piece_Detect4;

            Fw_BL_PiecerBtnArr[0] = button_ManualFw_Socket_BL_Piece_Detect1;
            Fw_BL_PiecerBtnArr[1] = button_ManualFw_Socket_BL_Piece_Detect2;
            Fw_BL_PiecerBtnArr[2] = button_ManualFw_Socket_BL_Piece_Detect3;
            Fw_BL_PiecerBtnArr[3] = button_ManualFw_Socket_BL_Piece_Detect4;

            Fw_BR_PiecerBtnArr[0] = button_ManualFw_Socket_BR_Piece_Detect1;
            Fw_BR_PiecerBtnArr[1] = button_ManualFw_Socket_BR_Piece_Detect2;
            Fw_BR_PiecerBtnArr[2] = button_ManualFw_Socket_BR_Piece_Detect3;
            Fw_BR_PiecerBtnArr[3] = button_ManualFw_Socket_BR_Piece_Detect4;

            //I /O 동작

            //컨택 상승 , 하강
            Fw_ContactUpBtnArr[0] = button_ManualFw_LT_Socket_Contact_Up1;
            Fw_ContactUpBtnArr[1] = button_ManualFw_LT_Socket_Contact_Up2;
            Fw_ContactUpBtnArr[2] = button_ManualFw_LT_Socket_Contact_Up3;
            Fw_ContactUpBtnArr[3] = button_ManualFw_LT_Socket_Contact_Up4;
            Fw_ContactUpBtnArr[4] = button_ManualFw_LT_Socket_Contact_Down1;
            Fw_ContactUpBtnArr[5] = button_ManualFw_LT_Socket_Contact_Down2;
            Fw_ContactUpBtnArr[6] = button_ManualFw_LT_Socket_Contact_Down3;
            Fw_ContactUpBtnArr[7] = button_ManualFw_LT_Socket_Contact_Down4;

            //컨택 전진 , 후진
            Fw_ContactForBtnArr[0] = button_ManualFw_LT_Socket_Contact_For1;
            Fw_ContactForBtnArr[1] = button_ManualFw_LT_Socket_Contact_For2;
            Fw_ContactForBtnArr[2] = button_ManualFw_LT_Socket_Contact_For3;
            Fw_ContactForBtnArr[3] = button_ManualFw_LT_Socket_Contact_For4;
            Fw_ContactForBtnArr[4] = button_ManualFw_LT_Socket_Contact_Back1;
            Fw_ContactForBtnArr[5] = button_ManualFw_LT_Socket_Contact_Back2;
            Fw_ContactForBtnArr[6] = button_ManualFw_LT_Socket_Contact_Back3;
            Fw_ContactForBtnArr[7] = button_ManualFw_LT_Socket_Contact_Back4;


            // ROTATOR 상승 , 하강
            Fw_RotateUpBtnArr[0] = button_ManualFw_LT_Socket_Rotator_Up1;
            Fw_RotateUpBtnArr[1] = button_ManualFw_LT_Socket_Rotator_Up2;
            Fw_RotateUpBtnArr[2] = button_ManualFw_LT_Socket_Rotator_Up3;
            Fw_RotateUpBtnArr[3] = button_ManualFw_LT_Socket_Rotator_Up4;
            Fw_RotateUpBtnArr[4] = button_ManualFw_LT_Socket_Rotator_Down1;
            Fw_RotateUpBtnArr[5] = button_ManualFw_LT_Socket_Rotator_Down2;
            Fw_RotateUpBtnArr[6] = button_ManualFw_LT_Socket_Rotator_Down3;
            Fw_RotateUpBtnArr[7] = button_ManualFw_LT_Socket_Rotator_Down4;

            // ROTATOR FLIP , UNFLIP
            Fw_RotateFlipBtnArr[0] = button_ManualFw_LT_Socket_Rotator_Flip1;
            Fw_RotateFlipBtnArr[1] = button_ManualFw_LT_Socket_Rotator_Flip2;
            Fw_RotateFlipBtnArr[2] = button_ManualFw_LT_Socket_Rotator_Flip3;
            Fw_RotateFlipBtnArr[3] = button_ManualFw_LT_Socket_Rotator_Flip4;
            Fw_RotateFlipBtnArr[4] = button_ManualFw_LT_Socket_Rotator_Unflip1;
            Fw_RotateFlipBtnArr[5] = button_ManualFw_LT_Socket_Rotator_Unflip2;
            Fw_RotateFlipBtnArr[6] = button_ManualFw_LT_Socket_Rotator_Unflip3;
            Fw_RotateFlipBtnArr[7] = button_ManualFw_LT_Socket_Rotator_Unflip4;


            // ROTATOR GRIP , UNGRIP
            Fw_RotateGripBtnArr[0] = button_ManualFw_LT_Socket_Rotator_Grip1;
            Fw_RotateGripBtnArr[1] = button_ManualFw_LT_Socket_Rotator_Grip2;
            Fw_RotateGripBtnArr[2] = button_ManualFw_LT_Socket_Rotator_Grip3;
            Fw_RotateGripBtnArr[3] = button_ManualFw_LT_Socket_Rotator_Grip4;
            Fw_RotateGripBtnArr[4] = button_ManualFw_LT_Socket_Rotator_Ungrip1;
            Fw_RotateGripBtnArr[5] = button_ManualFw_LT_Socket_Rotator_Ungrip2;
            Fw_RotateGripBtnArr[6] = button_ManualFw_LT_Socket_Rotator_Ungrip3;
            Fw_RotateGripBtnArr[7] = button_ManualFw_LT_Socket_Rotator_Ungrip4;

            for (i = 0; i < Fw_Socket_BtnArr.Length; i++)
            {
                Fw_Socket_BtnArr[i].BackColor = ButtonColor.FW_SOCKET_BACK_BTN_OFF;
                Fw_Socket_BtnArr[i].ForeColor = ButtonColor.FW_SOCKET_FORE_BTN_OFF;
            }


            for (i = 0; i < Fw_LT_PiecerBtnArr.Length; i++)
            {
                Fw_LT_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Fw_LT_PiecerBtnArr[i].ForeColor = Color.White;
                Fw_LT_PiecerBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Fw_RT_PiecerBtnArr.Length; i++)
            {
                Fw_RT_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Fw_RT_PiecerBtnArr[i].ForeColor = Color.White;
                Fw_RT_PiecerBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Fw_BL_PiecerBtnArr.Length; i++)
            {
                Fw_BL_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Fw_BL_PiecerBtnArr[i].ForeColor = Color.White;
                Fw_BL_PiecerBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Fw_BR_PiecerBtnArr.Length; i++)
            {
                Fw_BR_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Fw_BR_PiecerBtnArr[i].ForeColor = Color.White;
                Fw_BR_PiecerBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }

            //IO 동작

            for (i = 0; i < Fw_ContactUpBtnArr.Length; i++)
            {
                Fw_ContactUpBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Fw_ContactUpBtnArr[i].ForeColor = Color.White;
                Fw_ContactUpBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Fw_ContactForBtnArr.Length; i++)
            {
                Fw_ContactForBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Fw_ContactForBtnArr[i].ForeColor = Color.White;
                Fw_ContactForBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Fw_RotateUpBtnArr.Length; i++)
            {
                Fw_RotateUpBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Fw_RotateUpBtnArr[i].ForeColor = Color.White;
                Fw_RotateUpBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Fw_RotateGripBtnArr.Length; i++)
            {
                Fw_RotateGripBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Fw_RotateGripBtnArr[i].ForeColor = Color.White;
                Fw_RotateGripBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Fw_RotateFlipBtnArr.Length; i++)
            {
                Fw_RotateFlipBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Fw_RotateFlipBtnArr[i].ForeColor = Color.White;
                Fw_RotateFlipBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
        }




        private void ManualLoadVacuumOn(int index, bool bFlag)
        {
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.magazineHandler.RunState = OperationState.Stopped;
            //Globalo.motionManager.magazineHandler.LoadVacuumOn(index, bFlag);
        }
        private void ManualUnLoadVacuumOn(int index, bool bFlag)
        {
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.magazineHandler.RunState = OperationState.Stopped;
            //Globalo.motionManager.magazineHandler.UnLoadVacuumOn(index, bFlag);
        }


        private async void Manual_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList ePos, Machine.eAoiSocket aoiMotor)
        {
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Preparing)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 운전 준비 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.OriginRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (isMovingMagazine == true || Globalo.motionManager.magazineHandler.IsMoving())
            {
                Globalo.LogPrint("", "AOI SOCKET Z AXIS MOTOR RUNNING.", Globalo.eMessageName.M_INFO);
                Console.WriteLine("Z motor running...");
                return;
            }
            Globalo.motionManager.magazineHandler.RunState = OperationState.Stopped;
            isMovingMagazine = true;        //<---이동후 기다리지 않으면 바로 true로 바껴서 얘로만 체크하면 위험

            cts?.Dispose();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            
            string logstr = $"[MANUAL] AOI SOCKET Z AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);


            try
            {
                Task<bool> motorTask = Task.Run(() =>
                {
                    Console.WriteLine(" ------------------> ManualZ_Move");
                    bool rtn = Globalo.motionManager.socketAoiMachine.Socket_Z_Move(ePos, aoiMotor);
                    bool bComplete = true;

                    int nTimeTick = Environment.TickCount;
                    while (rtn)
                    {
                        if (bManualStopKey) break;
                        bComplete = Globalo.motionManager.socketAoiMachine.ChkMotorPos(ePos, aoiMotor);

                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                        {
                            bComplete = false;
                            Console.WriteLine(" ===> ManualZ_Move TIMEOUT");
                            break;
                        }
                        Thread.Sleep(10);
                    }

                    return bComplete;
                }, cts.Token);

                bool result = await motorTask;      //여기서 대기했다가 Task 빠져나오면 아래 if문으로 이동한다.
                //TODO: 이때 팝업 SHOW모달로 팝업띄우고 거기에 정지 버튼 추가 하는게 좋을 듯

                if (result)
                {
                    Console.WriteLine("Move okok");
                    logstr = $"[MANUAL] AOI SOCKET Z AXIS {ePos.ToString()} Move Complete";
                }
                else
                {
                    Console.WriteLine("Move fail");
                    logstr = $"[MANUAL] AOI SOCKET Z AXIS {ePos.ToString()} Move Fail";
                }
                Globalo.LogPrint("", logstr);
            }
            catch (OperationCanceledException)
            {
                Globalo.LogPrint("ManualControl", $"모터 작업이 취소되었습니다");
            }
            catch (Exception ex)
            {
                // 그 외 예외 처리
                Globalo.LogPrint("ManualControl", $"모터 이동 실패: {ex.Message}");
            }
            bManualStopKey = false;
            isMovingMagazine = false;
        }


        private async void Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList ePos, Machine.eAoiSocket aoiMotor)
        {
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Preparing)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 운전 준비 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.OriginRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (isMovingMagazine || Globalo.motionManager.magazineHandler.IsMoving())
            {
                Console.WriteLine("Y motor running...");
                Globalo.LogPrint("", "AOI SOCKET X AXIS MOTOR RUNNING.", Globalo.eMessageName.M_INFO);
                return;
            }
            Globalo.motionManager.magazineHandler.RunState = OperationState.Stopped;

            isMovingMagazine = true;//<---이동후 기다리지 않으면 바로 true로 바껴서 얘로만 체크하면 위험

            string logstr = $"[MANUAL] AOI SOCKET X  AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);


            try
            {
                cts?.Dispose();
                cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                Task<bool> motorTask = Task.Run(() =>
                {
                    Console.WriteLine(" ------------------> Manual_X_Move");

                    bool rtn = Globalo.motionManager.socketAoiMachine.Socket_X_Move(ePos, aoiMotor);
                    bool bComplete = true;

                    int nTimeTick = Environment.TickCount;
                    while (rtn)
                    {
                        if (bManualStopKey) break;

                        bComplete = Globalo.motionManager.socketAoiMachine.ChkMotorPos(ePos, aoiMotor);
                        if (bComplete)
                        {
                            //위치 확인 완료
                            Console.WriteLine(" ===> SocketX Move Complete");
                            break;
                        }
                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                        {
                            bComplete = false;
                            Console.WriteLine(" ===> SocketX Move TIMEOUT");
                            break;
                        }
                        Thread.Sleep(10);
                    }

                    return bComplete;
                }, cts.Token);

                bool result = await motorTask;      //여기서 대기했다가 Task 빠져나오면 아래 if문으로 이동한다.
                //TODO: 이때 팝업 SHOW모달로 팝업띄우고 거기에 정지 버튼 추가 하는게 좋을 듯

                if (result)
                {
                    Console.WriteLine("Move okok");
                    logstr = $"[MANUAL] AOI SOCKET X AXIS {ePos.ToString()} Move Complete";
                    Globalo.LogPrint("", logstr);
                }
                else
                {
                    Console.WriteLine("Move fail");
                    logstr = $"[MANUAL] AOI SOCKET X AXIS {ePos.ToString()} Move Fail";
                    Globalo.LogPrint("", logstr);
                }

                bManualStopKey = false;
                isMovingMagazine = false;
            }
            catch (OperationCanceledException)
            {
                Globalo.LogPrint("ManualControl", $"모터 작업이 취소되었습니다");
            }
            catch (Exception ex)
            {
                // 그 외 예외 처리
                Globalo.LogPrint("ManualControl", $"모터 이동 실패: {ex.Message}");
            }

            
        }

        
        public void showPanel()
        {
            if (ProgramState.ON_LINE_MOTOR == true)
            {
                //TeachingTimer.Start();
            }

        }
        public void hidePanel()
        {
            ManualTimer.Stop();
        }

        private void ManualMagazine_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                FwSocektSelect(CurrentFwSocket);
                bManualStopKey = false;
                ManualTimer.Start();
            }
            else
            {
                bManualStopKey = true;      //TODO: 테스트 필요 
                ManualTimer.Stop();
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------
        //
        // X, Y 축 
        //
        //
        //
        //
        //
        #region [AOI SOCKET X MOTOR MOVE]
        
        private void button_ManualAoi_Socket_L_Wait_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_L_X);
        }
        private void button_ManualAoi_Socket_L_Load_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.LOAD_POS, Machine.eAoiSocket.SOCKET_L_X);
        }
        private void button_ManualAoi_Socket_L_Unload_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.UN_LOAD_POS, Machine.eAoiSocket.SOCKET_L_X);
        }
        private void button_ManualAoi_Socket_L_TestL_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.CAPTURE_L_POS, Machine.eAoiSocket.SOCKET_L_X);
        }
        private void button_ManualAoi_Socket_L_TestR_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.CAPTURE_R_POS, Machine.eAoiSocket.SOCKET_L_X);
        }

        private void button_ManualAoi_Socket_R_Wait_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X);
        }

        private void button_ManualAoi_Socket_R_Load_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X);
        }

        private void button_ManualAoi_Socket_R_Unload_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X);
        }

        private void button_ManualAoi_Socket_R_TestL_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X);
        }
        private void button_ManualEEprom_Socket_R_Verify_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X);
        }


        #endregion

        //-------------------------------------------------------------------------------------------------------------------------------------
        //
        //IO 동작
        //
        //
        //
        //
        //
        private void ManualContactUp(int Group, int index, bool bFlag)
        {
            if (Globalo.motionManager.socketEEpromMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.socketEEpromMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.socketFwMachine.RunState = OperationState.Stopped;
            Globalo.motionManager.socketFwMachine.ContactPusherUp(Group, index, bFlag);


        }
        private void ManualContactFor(int Group, int index, bool bFlag)
        {
            if (Globalo.motionManager.socketEEpromMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.socketEEpromMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.socketFwMachine.GetIsContactUp(Group, index, true) == false)
            {
                Globalo.LogPrint("ManualControl", "[INFO] CONTACT 하강 상태 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.socketFwMachine.RunState = OperationState.Stopped;
            Globalo.motionManager.socketFwMachine.ContactPusherFor(Group, index, bFlag);


        }
        private void ManualRotatorUp(int Group, int index, bool bFlag)
        {
            if (Globalo.motionManager.socketEEpromMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.socketEEpromMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.socketEEpromMachine.GetContactUp(Group, index, true) == false)
            {
                Globalo.LogPrint("ManualControl", "[INFO] CONTACT 하강 상태 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.socketFwMachine.RunState = OperationState.Stopped;
            Globalo.motionManager.socketFwMachine.FlipperUp(Group, index, bFlag);


        }
        
        private void ManualRotatorFlip(int Group, int index, bool bFlag)
        {
            if (Globalo.motionManager.socketEEpromMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.socketEEpromMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.socketFwMachine.GetIsContactUp(Group, index, true) == false)
            {
                Globalo.LogPrint("ManualControl", "[INFO] CONTACT 하강 상태 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.socketFwMachine.RunState = OperationState.Stopped;
            Globalo.motionManager.socketFwMachine.FlipperRotate(Group, index, bFlag);


        }

        private void ManualRotatorGrip(int Group, int index, bool bFlag)
        {
            if (Globalo.motionManager.socketEEpromMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.socketEEpromMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.socketEEpromMachine.GetContactUp(Group, index, true) == false)
            {
                Globalo.LogPrint("ManualControl", "[INFO] CONTACT 하강 상태 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.socketFwMachine.RunState = OperationState.Stopped;
            Globalo.motionManager.socketFwMachine.FlipperGrip(Group, index, bFlag);


        }
        
        //
        // L
        //
        private void button_ManualFw_Socket_Contact_Up1_Click(object sender, EventArgs e)
        {
            ManualContactUp(CurrentFwSocket, 0, true);
            Globalo.LogPrint("ManualControl", "[SOCKET] #1 CONTACT UP");
        }

        private void button_ManualFw_Socket_Contact_Up2_Click(object sender, EventArgs e)
        {
            ManualContactUp(CurrentFwSocket, 1, true);
            Globalo.LogPrint("ManualControl", "[SOCKET] #2 CONTACT UP");
        }

        private void button_ManualFw_Socket_Contact_Up3_Click(object sender, EventArgs e)
        {
            ManualContactUp(CurrentFwSocket, 2, true);
            Globalo.LogPrint("ManualControl", "[SOCKET] #3 CONTACT UP");
        }

        private void button_ManualFw_Socket_Contact_Up4_Click(object sender, EventArgs e)
        {
            ManualContactUp(CurrentFwSocket, 3, true);
            Globalo.LogPrint("ManualControl", "[SOCKET] #4 CONTACT UP");
        }

        private void button_ManualFw_Socket_Contact_Down1_Click(object sender, EventArgs e)
        {
            ManualContactUp(CurrentFwSocket, 0, false);
            Globalo.LogPrint("ManualControl", "[SOCKET] #1 CONTACT DOWN");
        }

        private void button_ManualFw_Socket_Contact_Down2_Click(object sender, EventArgs e)
        {
            ManualContactUp(CurrentFwSocket, 1, false);
            Globalo.LogPrint("ManualControl", "[SOCKET] #2 CONTACT DOWN");
        }

        private void button_ManualFw_Socket_Contact_Down3_Click(object sender, EventArgs e)
        {
            ManualContactUp(CurrentFwSocket, 2, false);
            Globalo.LogPrint("ManualControl", "[SOCKET] #3 CONTACT DOWN");
        }

        private void button_ManualFw_Socket_Contact_Down4_Click(object sender, EventArgs e)
        {
            ManualContactUp(CurrentFwSocket, 3, false);
            Globalo.LogPrint("ManualControl", "[SOCKET] #4 CONTACT DOWN");
        }






        private void button_ManualFw_Socket_Contact_For1_Click(object sender, EventArgs e)
        {
            ManualContactFor(CurrentFwSocket, 0, true);
            Globalo.LogPrint("ManualControl", "[SOCKET] #1 CONTACT FOR");
        }

        private void button_ManualFw_Socket_Contact_For2_Click(object sender, EventArgs e)
        {
            ManualContactFor(CurrentFwSocket, 1, true);
            Globalo.LogPrint("ManualControl", "[SOCKET] #2 CONTACT FOR");
        }

        private void button_ManualFw_Socket_Contact_For3_Click(object sender, EventArgs e)
        {
            ManualContactFor(CurrentFwSocket, 2, true);
            Globalo.LogPrint("ManualControl", "[SOCKET] #3 CONTACT FOR");
        }

        private void button_ManualFw_Socket_Contact_For4_Click(object sender, EventArgs e)
        {
            ManualContactFor(CurrentFwSocket, 3, true);
            Globalo.LogPrint("ManualControl", "[SOCKET] #4 CONTACT FOR");
        }

        private void button_ManualFw_Socket_Contact_Back1_Click(object sender, EventArgs e)
        {
            ManualContactFor(CurrentFwSocket, 0, false);
            Globalo.LogPrint("ManualControl", "[SOCKET] #1 CONTACT BACK");
        }

        private void button_ManualFw_Socket_Contact_Back2_Click(object sender, EventArgs e)
        {
            ManualContactFor(CurrentFwSocket, 1, false);
            Globalo.LogPrint("ManualControl", "[SOCKET] #2 CONTACT BACK");
        }

        private void button_ManualFw_Socket_Contact_Back3_Click(object sender, EventArgs e)
        {
            ManualContactFor(CurrentFwSocket, 2, false);
            Globalo.LogPrint("ManualControl", "[SOCKET] #3 CONTACT BACK");
        }

        private void button_ManualFw_Socket_Contact_Back4_Click(object sender, EventArgs e)
        {
            ManualContactFor(CurrentFwSocket, 3, false);
            Globalo.LogPrint("ManualControl", "[SOCKET] #4 CONTACT BACK");
        }


        //ROTATOR
        private void button_ManualFw_LT_Socket_Rotator_Up1_Click(object sender, EventArgs e)
        {
            ManualRotatorUp(CurrentFwSocket, 0, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Up2_Click(object sender, EventArgs e)
        {
            ManualRotatorUp(CurrentFwSocket, 1, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Up3_Click(object sender, EventArgs e)
        {
            ManualRotatorUp(CurrentFwSocket, 2, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Up4_Click(object sender, EventArgs e)
        {
            ManualRotatorUp(CurrentFwSocket, 3, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Down1_Click(object sender, EventArgs e)
        {
            ManualRotatorUp(CurrentFwSocket, 0, false);
        }

        private void button_ManualFw_LT_Socket_Rotator_Down2_Click(object sender, EventArgs e)
        {
            ManualRotatorUp(CurrentFwSocket, 1, false);
        }

        private void button_ManualFw_LT_Socket_Rotator_Down3_Click(object sender, EventArgs e)
        {
            ManualRotatorUp(CurrentFwSocket, 2, false);
        }

        private void button_ManualFw_LT_Socket_Rotator_Down4_Click(object sender, EventArgs e)
        {
            ManualRotatorUp(CurrentFwSocket, 3, false);
        }
        private void button_ManualFw_LT_Socket_Rotator_Flip1_Click(object sender, EventArgs e)
        {
            ManualRotatorFlip(CurrentFwSocket, 0, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Flip2_Click(object sender, EventArgs e)
        {
            ManualRotatorFlip(CurrentFwSocket, 1, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Flip3_Click(object sender, EventArgs e)
        {
            ManualRotatorFlip(CurrentFwSocket, 2, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Flip4_Click(object sender, EventArgs e)
        {
            ManualRotatorFlip(CurrentFwSocket, 3, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Unflip1_Click(object sender, EventArgs e)
        {
            ManualRotatorFlip(CurrentFwSocket, 0, false);
        }

        private void button_ManualFw_LT_Socket_Rotator_Unflip2_Click(object sender, EventArgs e)
        {
            ManualRotatorFlip(CurrentFwSocket, 1, false);
        }

        private void button_ManualFw_LT_Socket_Rotator_Unflip3_Click(object sender, EventArgs e)
        {
            ManualRotatorFlip(CurrentFwSocket, 2, false);
        }

        private void button_ManualFw_LT_Socket_Rotator_Unflip4_Click(object sender, EventArgs e)
        {
            ManualRotatorFlip(CurrentFwSocket, 3, false);
        }

        private void button_ManualFw_LT_Socket_Rotator_Grip1_Click(object sender, EventArgs e)
        {
            ManualRotatorGrip(CurrentFwSocket, 0, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Grip2_Click(object sender, EventArgs e)
        {
            ManualRotatorGrip(CurrentFwSocket, 1, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Grip3_Click(object sender, EventArgs e)
        {
            ManualRotatorGrip(CurrentFwSocket, 2, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Grip4_Click(object sender, EventArgs e)
        {
            ManualRotatorGrip(CurrentFwSocket, 3, true);
        }

        private void button_ManualFw_LT_Socket_Rotator_Ungrip1_Click(object sender, EventArgs e)
        {
            ManualRotatorGrip(CurrentFwSocket, 0, false);
        }

        private void button_ManualFw_LT_Socket_Rotator_Ungrip2_Click(object sender, EventArgs e)
        {
            ManualRotatorGrip(CurrentFwSocket, 1, false);
        }

        private void button_ManualFw_LT_Socket_Rotator_Ungrip3_Click(object sender, EventArgs e)
        {
            ManualRotatorGrip(CurrentFwSocket, 2, false);
        }

        private void button_ManualFw_LT_Socket_Rotator_Ungrip4_Click(object sender, EventArgs e)
        {
            ManualRotatorGrip(CurrentFwSocket, 3, false);
        }
        private void Manual_Timer_Tick(object sender, EventArgs e)
        {
            int i = 0;
            bool bRtn = false;
            //------------------------------------------------------------------------------------------------------
            //
            //
            //  제품 감지
            //
            //
            //------------------------------------------------------------------------------------------------------
            int index = 0;
            for (i = 0; i < Fw_LT_PiecerBtnArr.Length; i++)
            {
                if (Globalo.motionManager.socketFwMachine.GetIsProductInSocket(CurrentFwSocket, i, true) == true)
                {
                    Fw_LT_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Fw_LT_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }
            for (i = 0; i < Fw_RT_PiecerBtnArr.Length; i++)
            {
                if (Globalo.motionManager.socketFwMachine.GetIsProductInSocket(CurrentFwSocket, i, true) == true)
                {
                    Fw_RT_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Fw_RT_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }
            for (i = 0; i < Fw_BL_PiecerBtnArr.Length; i++)
            {
                if (Globalo.motionManager.socketFwMachine.GetIsProductInSocket(CurrentFwSocket, i, true) == true)
                {
                    Fw_BL_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Fw_BL_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }
            for (i = 0; i < Fw_BR_PiecerBtnArr.Length; i++)
            {
                if (Globalo.motionManager.socketFwMachine.GetIsProductInSocket(CurrentFwSocket, i, true) == true)
                {
                    Fw_BR_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Fw_BR_PiecerBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }
            //------------------------------------------------------------------------------------------------------
            //
            //
            //  CONTACT
            //
            //
            //------------------------------------------------------------------------------------------------------
            //컨택 상승 , 하강
            for (i = 0; i < Fw_ContactUpBtnArr.Length; i++)
            {
                index = i%4;    //0,1,2,3 반복
                if (i < 4)
                {
                    if (Globalo.motionManager.socketFwMachine.GetIsContactUp(CurrentFwSocket, i, true) == true)
                    {
                        Fw_ContactUpBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                    }
                    else
                    {
                        Fw_ContactUpBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                    }
                }
                else
                {
                    if (Globalo.motionManager.socketFwMachine.GetIsContactUp(CurrentFwSocket, index, false) == true)
                    {
                        Fw_ContactUpBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                    }
                    else
                    {
                        Fw_ContactUpBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                    }
                }
                
            }
            //컨택 전진, 후진
            for (i = 0; i < Fw_ContactForBtnArr.Length; i++)
            {
                index = i % 4;    //0,1,2,3 반복
                if (i < 4)
                {
                    if (Globalo.motionManager.socketFwMachine.GetIsContactForward(CurrentFwSocket, i, true) == true)
                    {
                        Fw_ContactForBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                    }
                    else
                    {
                        Fw_ContactForBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                    }
                }
                else
                {
                    if (Globalo.motionManager.socketFwMachine.GetIsContactForward(CurrentFwSocket, index, false) == true)
                    {
                        Fw_ContactForBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                    }
                    else
                    {
                        Fw_ContactForBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                    }
                }

            }
            //------------------------------------------------------------------------------------------------------
            //
            //
            //  ROTATOR
            //
            //
            //------------------------------------------------------------------------------------------------------
            //로테이션 상승, 하강
            for (i = 0; i < Fw_RotateUpBtnArr.Length; i++)
            {
                index = i % 4;    //0,1,2,3 반복
                if (i < 4)
                {
                    if (Globalo.motionManager.socketFwMachine.GetIsFlipperUp(CurrentFwSocket, i, true) == true)
                    {
                        Fw_RotateUpBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                    }
                    else
                    {
                        Fw_RotateUpBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                    }
                }
                else
                {
                    if (Globalo.motionManager.socketFwMachine.GetIsFlipperUp(CurrentFwSocket, index, false) == true)
                    {
                        Fw_RotateUpBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                    }
                    else
                    {
                        Fw_RotateUpBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                    }
                }

            }

            //로테이션 Flip , Unflip
            for (i = 0; i < Fw_RotateFlipBtnArr.Length; i++)
            {
                index = i % 4;    //0,1,2,3 반복
                if (i < 4)
                {
                    if (Globalo.motionManager.socketFwMachine.GetIsFlipperRotated(CurrentFwSocket, i, true) == true)
                    {
                        Fw_RotateFlipBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                    }
                    else
                    {
                        Fw_RotateFlipBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                    }
                }
                else
                {
                    if (Globalo.motionManager.socketFwMachine.GetIsFlipperRotated(CurrentFwSocket, index, false) == true)
                    {
                        Fw_RotateFlipBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                    }
                    else
                    {
                        Fw_RotateFlipBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                    }
                }

            }

            //로테이션 Grip , UnGrip
            for (i = 0; i < Fw_RotateGripBtnArr.Length; i++)
            {
                index = i % 4;    //0,1,2,3 반복
                if (i < 4)
                {
                    if (Globalo.motionManager.socketFwMachine.GetIsFlipperRotated(CurrentFwSocket, i, true) == true)
                    {
                        Fw_RotateGripBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                    }
                    else
                    {
                        Fw_RotateGripBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                    }
                }
                else
                {
                    if (Globalo.motionManager.socketFwMachine.GetIsFlipperRotated(CurrentFwSocket, index, false) == true)
                    {
                        Fw_RotateGripBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                    }
                    else
                    {
                        Fw_RotateGripBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                    }
                }

            }
        }

        private void FwSocektSelect(int index)
        {
            int i = 0;
            CurrentFwSocket = index;


            for (i = 0; i < Fw_Socket_BtnArr.Length; i++)
            {
                if(i == index)
                {
                    Fw_Socket_BtnArr[i].BackColor = ButtonColor.FW_SOCKET_BACK_BTN_ON;
                    Fw_Socket_BtnArr[i].ForeColor = ButtonColor.FW_SOCKET_FORE_BTN_ON;
                }
                else
                {
                    Fw_Socket_BtnArr[i].BackColor = ButtonColor.FW_SOCKET_BACK_BTN_OFF;
                    Fw_Socket_BtnArr[i].ForeColor = ButtonColor.FW_SOCKET_FORE_BTN_OFF;
                }
                
            }

        }
        private void button_ManualFw_Socket_Select_LT_Click(object sender, EventArgs e)
        {
            FwSocektSelect(0);
            
        }

        private void button_ManualFw_Socket_Select_RT_Click(object sender, EventArgs e)
        {
            FwSocektSelect(1);
        }

        private void button_ManualFw_Socket_Select_BL_Click(object sender, EventArgs e)
        {
            FwSocektSelect(2);
        }

        private void button_ManualFw_Socket_Select_BR_Click(object sender, EventArgs e)
        {
            FwSocektSelect(3);
        }

        
    }
}
