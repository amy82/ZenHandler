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
    public partial class ManualLift : UserControl
    {
        public bool bManualStopKey;
        //private int MovePos;
        
        private System.Windows.Forms.Timer ManualTimer;

        private Button[] MotorX_BtnArr = new Button[5];
        private Button[] MotorZ_BtnArr = new Button[5];

        private Button[] LiftIoBtnArr = new Button[10];

        protected CancellationTokenSource cts;  //TODO: <--이름 변경하고 사용가능하게
        private bool isMovingLift;
        private int ManualLoadPosx = 0;
        private int ManualLoadPosy = 0;

        //MEMO: 티칭 위치를 어떻게 가져올 것인가

        public ManualLift()
        {
            InitializeComponent();
            bManualStopKey = false;
            cts = new CancellationTokenSource();

            isMovingLift = false;

            ManualTimer = new System.Windows.Forms.Timer();
            ManualTimer.Interval = 300; // 1초 (1000밀리초) 간격 설정
            ManualTimer.Tick += new EventHandler(Manual_Timer_Tick);


            ManualPcbUiSet();
        }
        private void ManualPcbUiSet()
        {
            int i = 0;
            MotorX_BtnArr[0] = button_Manual_Lift_Wait_Pos_X;
            MotorX_BtnArr[1] = button_Manual_Lift_Left_Load_Pos_X;
            MotorX_BtnArr[2] = button_Manual_Lift_Right_Load_Pos_X;
            MotorX_BtnArr[3] = button_Manual_Lift_Left_Tray_Load_Pos_X;
            MotorX_BtnArr[4] = button_Manual_Lift_Right_Tray_Load_Pos_X;



            LiftIoBtnArr[0] = button_Manual_Lift_Gantry_Clamp_For;
            LiftIoBtnArr[1] = button_Manual_Lift_Gantry_Clamp_Back;

            LiftIoBtnArr[2] = button_Manual_Lift_Gantry_Centring_For;
            LiftIoBtnArr[3] = button_Manual_Lift_Gantry_Centring_Back;

            LiftIoBtnArr[4] = button_Manual_Lift_Pusher_For;
            LiftIoBtnArr[5] = button_Manual_Lift_Pusher_Back;

            LiftIoBtnArr[6] = button_Manual_Lift_Pusher_Up;
            LiftIoBtnArr[7] = button_Manual_Lift_Pusher_Down;

            LiftIoBtnArr[8] = button_Manual_Lift_Pusher_Centring_For;
            LiftIoBtnArr[9] = button_Manual_Lift_Pusher_Centring_Back;

            for (i = 0; i < MotorX_BtnArr.Length; i++)
            {
                MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                MotorX_BtnArr[i].ForeColor = Color.White;
                MotorX_BtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            MotorX_BtnArr[0].BackColor = ColorTranslator.FromHtml("#4C4743");   //모터 위치 이동 완료시 색


            for (i = 0; i < LiftIoBtnArr.Length; i++)
            {
                LiftIoBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                LiftIoBtnArr[i].ForeColor = Color.White;
                LiftIoBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
        }




        private void ManualLoadVacuumOn(int index, bool bFlag)
        {
            if (Globalo.motionManager.liftMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.liftMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.liftMachine.RunState = OperationState.Stopped;
            //Globalo.motionManager.transferMachine.LoadVacuumOn(index, bFlag);
        }

        private void ManualUnLoadVacuumOn(int index, bool bFlag)
        {
            if (Globalo.motionManager.liftMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.liftMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.liftMachine.RunState = OperationState.Stopped;
            //Globalo.motionManager.transferMachine.UnLoadVacuumOn(index, bFlag);
        }


        private async void Manual_Lift_Z_Move(Machine.eLift CurrentLift , Machine.eLiftSensor ePos)
        {
            if (Globalo.motionManager.liftMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.liftMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.liftMachine.RunState == OperationState.Preparing)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 운전 준비 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.liftMachine.RunState == OperationState.OriginRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (isMovingLift == true || Globalo.motionManager.liftMachine.IsMoving())
            {
                Globalo.LogPrint("", "LIFT Z AXIS MOTOR RUNNING.", Globalo.eMessageName.M_INFO);
                Console.WriteLine("Z motor running...");
                return;
            }
            Globalo.motionManager.liftMachine.RunState = OperationState.Stopped;
            isMovingLift = true;        //<---이동후 기다리지 않으면 바로 true로 바껴서 얘로만 체크하면 위험

            cts?.Dispose();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;


            

            string logstr = $"[MANUAL] LIFT Z AXIS {ePos.ToString()} Move";

            Globalo.LogPrint("", logstr);
            try
            {
                Task<bool> motorTask = Task.Run(() =>
                {
                    Console.WriteLine(" ------------------> Manual_Lift_Z_Move");
                    bool rtn = true;
                    if (ePos == Machine.eLiftSensor.LIFT_HOME_POS)
                    {
                        rtn = Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(CurrentLift, ePos, true);
                    }
                    else if (ePos == Machine.eLiftSensor.LIFT_READY_POS)
                    {
                        rtn = Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(CurrentLift, ePos, true);
                    }
                    else if (ePos == Machine.eLiftSensor.LIFT_TOPSTOP_POS)
                    {
                        rtn = Globalo.motionManager.liftMachine.LIft_Z_Move_SersonDetected(CurrentLift, ePos, true);
                    }

                    bool bComplete = true;

                    int nTimeTick = Environment.TickCount;
                    while (rtn)
                    {
                        if (bManualStopKey) break;
                        if (ePos == Machine.eLiftSensor.LIFT_HOME_POS)
                        {
                            bComplete = Globalo.motionManager.liftMachine.MotorAxes[(int)CurrentLift].GetNegaSensor();
                        }
                        else if (ePos == Machine.eLiftSensor.LIFT_READY_POS)
                        {
                            bComplete = Globalo.motionManager.liftMachine.GetMiddleWaitSensor((int)CurrentLift);
                        }
                        else if (ePos == Machine.eLiftSensor.LIFT_TOPSTOP_POS)
                        {
                            bComplete = Globalo.motionManager.liftMachine.GetTopTouchSensor((int)CurrentLift);
                        }
                        

                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                        {
                            bComplete = false;
                            Console.WriteLine(" ===> Manual_Lift_Z_Move TIMEOUT");
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
                    logstr = $"[MANUAL] LIFT Z AXIS {ePos.ToString()} Move Complete";
                }
                else
                {
                    Console.WriteLine("Move fail");
                    logstr = $"[MANUAL] LIFT Z AXIS {ePos.ToString()} Move Fail";
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
            isMovingLift = false;
        }


        private async void Manual_X_Move(Machine.LiftMachine.eTeachingPosList ePos)
        {
            if (Globalo.motionManager.liftMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.liftMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.liftMachine.RunState == OperationState.Preparing)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 운전 준비 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.liftMachine.RunState == OperationState.OriginRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (isMovingLift || Globalo.motionManager.liftMachine.IsMoving())
            {
                Console.WriteLine("X motor running...");
                Globalo.LogPrint("", "LIFT X AXIS MOTOR RUNNING.", Globalo.eMessageName.M_INFO);
                return;
            }
            Globalo.motionManager.liftMachine.RunState = OperationState.Stopped;

            isMovingLift = true;//<---이동후 기다리지 않으면 바로 true로 바껴서 얘로만 체크하면 위험

            string logstr = $"[MANUAL] LIFT X AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            

            try
            {
                cts?.Dispose();
                cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                Task<bool> motorTask = Task.Run(() =>
                {
                    Console.WriteLine(" ------------------> TransFer_XY_Move");

                    bool rtn = Globalo.motionManager.liftMachine.Gantry_X_Move(ePos);
                    bool bComplete = true;

                    int nTimeTick = Environment.TickCount;
                    while (rtn)
                    {
                        if (bManualStopKey) break;

                        bComplete = Globalo.motionManager.liftMachine.ChkGantryXMotorPos(ePos);
                        if (bComplete)
                        {
                            //위치 확인 완료
                            Console.WriteLine(" ===> LIFT X Move Complete");
                            break;
                        }
                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                        {
                            bComplete = false;
                            Console.WriteLine(" ===> LIFT X Move TIMEOUT");
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
                    logstr = $"[MANUAL] LIFT X AXIS {ePos.ToString()} Move Complete";
                    Globalo.LogPrint("", logstr);
                }
                else
                {
                    Console.WriteLine("Move fail");
                    logstr = $"[MANUAL] LIFT X AXIS {ePos.ToString()} Move Fail";
                    Globalo.LogPrint("", logstr);
                }

                bManualStopKey = false;
                isMovingLift = false;
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

        private void ManualTransfer_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
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
        #region [TRANSFER X,Y MOTOR MOVE]
        
        private void button_Manual_Lift_Wait_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.LiftMachine.eTeachingPosList.WAIT_POS);
        }

        // X,Y BCR SCAN
        private void button_Manual_Lift_Left_Load_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.LiftMachine.eTeachingPosList.LEFT_LOAD_POS);
        }
        private void button_Manual_Lift_Right_Load_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.LiftMachine.eTeachingPosList.RIGHT_LOAD_POS);
        }
        // X,Y TRAY 제품 로드
        private void button_Manual_Lift_Left_Tray_Load_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.LiftMachine.eTeachingPosList.TRAY_LOAD_LEFT_POS);
        }
        private void button_Manual_Lift_Right_Tray_Load_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.LiftMachine.eTeachingPosList.TRAY_LOAD_RIGHT_POS);
        }

        #endregion
        //-------------------------------------------------------------------------------------------------------------------------------------
        //
        //Z 축 
        //
        //
        //
        //
        //
        #region [LIFT Z MOTOR MOVE]
        //투입 리프트
        private void BTN_MANUAL_WAIT_POS_Z_Click_1(object sender, EventArgs e)
        {
            Manual_Lift_Z_Move(Machine.eLift.LIFT_L_Z, Machine.eLiftSensor.LIFT_HOME_POS);
        }
        private void button_Manual_LoadLift_Ready_Z_Click(object sender, EventArgs e)
        {
            Manual_Lift_Z_Move(Machine.eLift.LIFT_L_Z, Machine.eLiftSensor.LIFT_READY_POS);
        }

        private void button_Manual_LoadLift_Top_Z_Click(object sender, EventArgs e)
        {
            Manual_Lift_Z_Move(Machine.eLift.LIFT_L_Z, Machine.eLiftSensor.LIFT_TOPSTOP_POS);
        }
        //배출 리프트
        private void button_Manual_UnloadLift_Home_Z_Click(object sender, EventArgs e)
        {
            Manual_Lift_Z_Move(Machine.eLift.LIFT_R_Z, Machine.eLiftSensor.LIFT_HOME_POS);
        }
        private void button_Manual_UnloadLift_Ready_Z_Click(object sender, EventArgs e)
        {
            Manual_Lift_Z_Move(Machine.eLift.LIFT_R_Z, Machine.eLiftSensor.LIFT_READY_POS);
        }
        private void button_Manual_UnloadLift_Top_Z_Click(object sender, EventArgs e)
        {
            Manual_Lift_Z_Move(Machine.eLift.LIFT_R_Z, Machine.eLiftSensor.LIFT_TOPSTOP_POS);
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

        private void BTN_MANUAL_VACUUM_ON_Click_1(object sender, EventArgs e)
        {
            ManualLoadVacuumOn(0, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #1 LOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_VACUUM_OFF_Click_1(object sender, EventArgs e)
        {
            ManualLoadVacuumOn(0, false);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #1 LOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON2_Click(object sender, EventArgs e)
        {
            ManualLoadVacuumOn(1, true);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #2 LOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON3_Click(object sender, EventArgs e)
        {
            ManualLoadVacuumOn(2, true);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #3 LOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON4_Click(object sender, EventArgs e)
        {
            ManualLoadVacuumOn(3, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #4 LOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF2_Click(object sender, EventArgs e)
        {
            ManualLoadVacuumOn(1, false);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #2 LOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF3_Click(object sender, EventArgs e)
        {
            ManualLoadVacuumOn(2, false);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #3 LOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF4_Click(object sender, EventArgs e)
        {
            ManualLoadVacuumOn(3, false);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #4 LOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON1_Click(object sender, EventArgs e)
        {
            ManualUnLoadVacuumOn(0, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #1 UNLOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF1_Click(object sender, EventArgs e)
        {
            ManualUnLoadVacuumOn(0, false);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #1 UNLOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON2_Click(object sender, EventArgs e)
        {
            ManualUnLoadVacuumOn(1, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #2 UNLOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF2_Click(object sender, EventArgs e)
        {
            ManualUnLoadVacuumOn(1, false);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #2 UNLOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON3_Click(object sender, EventArgs e)
        {
            ManualUnLoadVacuumOn(2, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #3 UNLOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF3_Click(object sender, EventArgs e)
        {
            ManualUnLoadVacuumOn(2, false);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #3 UNLOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON4_Click(object sender, EventArgs e)
        {
            ManualUnLoadVacuumOn(3, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #4 UNLOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF4_Click(object sender, EventArgs e)
        {
            ManualUnLoadVacuumOn(3, false);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #4 UNLOAD PICKER VACUUM OFF");
        }
        private void Manual_Timer_Tick(object sender, EventArgs e)
        {
            int i = 0;
            bool bRtn = false;
            //IO 동작 상태

            //for (i = 0; i < 4; i++)
            //{
            //    bRtn = Globalo.motionManager.transferMachine.GetLoadVacuumState(i, true);

            //    if (bRtn)
            //    {
            //        LoadVacuumOnBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            //    }
            //    else
            //    {
            //        LoadVacuumOnBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            //    }
            //}

            for (i = 0; i < MotorX_BtnArr.Length; i++)
            {
                Machine.LiftMachine.eTeachingPosList pos = (Machine.LiftMachine.eTeachingPosList)i;

                if (Globalo.motionManager.liftMachine.ChkGantryXMotorPos(pos) == true)
                {
                    MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }

        }

        
    }
}
