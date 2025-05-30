﻿using System;
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
    public partial class ManualTransfer : UserControl
    {
        public bool bManualStopKey;
        //private int MovePos;
        
        private System.Windows.Forms.Timer ManualTimer;

        private Button[] MotorXY_BtnArr = new Button[15];
        private Button[] MotorZ_BtnArr = new Button[15];

        private Button[] LoadVacuumOnBtnArr = new Button[4];
        private Button[] LoadVacuumOffBtnArr = new Button[4];

        private Button[] UnLoadVacuumOnBtnArr = new Button[4];
        private Button[] UnLoadVacuumOffBtnArr = new Button[4];

        protected CancellationTokenSource cts;  //TODO: <--이름 변경하고 사용가능하게
        private bool isMovingTransfer;
        private int ManualLoadPosx = 0;
        private int ManualLoadPosy = 0;
        //MEMO: 티칭 위치를 어떻게 가져올 것인가
        public ManualTransfer()
        {
            InitializeComponent();
            bManualStopKey = false;
            cts = new CancellationTokenSource();

            isMovingTransfer = false;

            ManualTimer = new System.Windows.Forms.Timer();
            ManualTimer.Interval = 300; // 1초 (1000밀리초) 간격 설정
            ManualTimer.Tick += new EventHandler(Manual_Timer_Tick);


            ManualPcbUiSet();
        }
        private void ManualPcbUiSet()
        {
            int i = 0;
            MotorXY_BtnArr[0] = button_Manual_Transfer_Wait_Pos_XY;
            //
            MotorXY_BtnArr[1] = button_Manual_Transfer_Left_Bcr_Pos_XY;
            MotorXY_BtnArr[2] = button_Manual_Transfer_Right_Bcr_Pos_XY;
            //
            MotorXY_BtnArr[3] = button_Manual_Transfer_Left_Tray_Load_Pos_XY;
            MotorXY_BtnArr[4] = button_Manual_Transfer_Left_Tray_Unload_Pos_XY;
            MotorXY_BtnArr[5] = button_Manual_Transfer_Right_Tray_Load_Pos_XY;
            MotorXY_BtnArr[6] = button_Manual_Transfer_Right_Tray_Unload_Pos_XY;
            //
            MotorXY_BtnArr[7] = button_Manual_Transfer_A_Socket_Load_Pos_XY;
            MotorXY_BtnArr[8] = button_Manual_Transfer_A_Socket_Unload_Pos_XY;
            MotorXY_BtnArr[9] = button_Manual_Transfer_B_Socket_Load_Pos_XY;
            MotorXY_BtnArr[10] = button_Manual_Transfer_B_Socket_Unload_Pos_XY;
            MotorXY_BtnArr[11] = button_Manual_Transfer_C_Socket_Load_Pos_XY;
            MotorXY_BtnArr[12] = button_Manual_Transfer_C_Socket_Unload_Pos_XY;
            MotorXY_BtnArr[13] = button_Manual_Transfer_D_Socket_Load_Pos_XY;
            MotorXY_BtnArr[14] = button_Manual_Transfer_D_Socket_Unload_Pos_XY;
            //
            //
            MotorZ_BtnArr[0] = button_Manual_Transfer_Wait_Pos_Z;
            //
            MotorZ_BtnArr[1] = button_Manual_Transfer_Left_Bcr_Pos_Z;
            MotorZ_BtnArr[2] = button_Manual_Transfer_Right_Bcr_Pos_Z;
            //
            MotorZ_BtnArr[3] = button_Manual_Transfer_Left_Tray_Load_Pos_Z;
            MotorZ_BtnArr[4] = button_Manual_Transfer_Left_Tray_Unload_Pos_Z;
            MotorZ_BtnArr[5] = button_Manual_Transfer_Right_Tray_Load_Pos_Z;
            MotorZ_BtnArr[6] = button_Manual_Transfer_Right_Tray_Unload_Pos_Z;
            //
            MotorZ_BtnArr[7] = button_Manual_Transfer_A_Socket_Load_Pos_Z;
            MotorZ_BtnArr[8] = button_Manual_Transfer_A_Socket_Unload_Pos_Z;
            MotorZ_BtnArr[9] = button_Manual_Transfer_B_Socket_Load_Pos_Z;
            MotorZ_BtnArr[10] = button_Manual_Transfer_B_Socket_Unload_Pos_Z;
            MotorZ_BtnArr[11] = button_Manual_Transfer_C_Socket_Load_Pos_Z;
            MotorZ_BtnArr[12] = button_Manual_Transfer_C_Socket_Unload_Pos_Z;
            MotorZ_BtnArr[13] = button_Manual_Transfer_D_Socket_Load_Pos_Z;
            MotorZ_BtnArr[14] = button_Manual_Transfer_D_Socket_Unload_Pos_Z;


            LoadVacuumOnBtnArr[0] = BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON1;
            LoadVacuumOnBtnArr[1] = BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON2;
            LoadVacuumOnBtnArr[2] = BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON3;
            LoadVacuumOnBtnArr[3] = BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON4;

            LoadVacuumOffBtnArr[0] = BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF1;
            LoadVacuumOffBtnArr[1] = BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF2;
            LoadVacuumOffBtnArr[2] = BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF3;
            LoadVacuumOffBtnArr[3] = BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF4;

            UnLoadVacuumOnBtnArr[0] = BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON1;
            UnLoadVacuumOnBtnArr[1] = BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON2;
            UnLoadVacuumOnBtnArr[2] = BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON3;
            UnLoadVacuumOnBtnArr[3] = BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON4;

            UnLoadVacuumOffBtnArr[0] = BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF1;
            UnLoadVacuumOffBtnArr[1] = BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF2;
            UnLoadVacuumOffBtnArr[2] = BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF3;
            UnLoadVacuumOffBtnArr[3] = BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF4;

            for (i = 0; i < MotorXY_BtnArr.Length; i++)
            {
                MotorXY_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                MotorXY_BtnArr[i].ForeColor = Color.White;

                MotorXY_BtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < MotorZ_BtnArr.Length; i++)
            {
                MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                MotorZ_BtnArr[i].ForeColor = Color.White;

                MotorZ_BtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            ///MotorBtnArr[0].BackColor = ColorTranslator.FromHtml("#4C4743");   //모터 위치 이동 완료시 색


            for (i = 0; i < LoadVacuumOnBtnArr.Length; i++)
            {
                LoadVacuumOnBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                LoadVacuumOnBtnArr[i].ForeColor = Color.White;
                LoadVacuumOnBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }

            for (i = 0; i < LoadVacuumOffBtnArr.Length; i++)
            {
                LoadVacuumOffBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                LoadVacuumOffBtnArr[i].ForeColor = Color.White;
                LoadVacuumOffBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }

            for (i = 0; i < UnLoadVacuumOnBtnArr.Length; i++)
            {
                UnLoadVacuumOnBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                UnLoadVacuumOnBtnArr[i].ForeColor = Color.White;
                UnLoadVacuumOnBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }

            for (i = 0; i < UnLoadVacuumOffBtnArr.Length; i++)
            {
                UnLoadVacuumOffBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                UnLoadVacuumOffBtnArr[i].ForeColor = Color.White;
                UnLoadVacuumOffBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
        }
        

        

        private void ManualLoadVacuumOn(int index, bool bFlag)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.RunState = OperationState.Stopped;
            Globalo.motionManager.transferMachine.LoadVacuumOn(index, bFlag);
        }
        private void ManualUnLoadVacuumOn(int index, bool bFlag)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.RunState = OperationState.Stopped;
            Globalo.motionManager.transferMachine.UnLoadVacuumOn(index, bFlag);
        }


        private async void Manual_Z_Move(Machine.TransferMachine.eTeachingPosList ePos)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Preparing)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 운전 준비 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.OriginRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (isMovingTransfer == true || Globalo.motionManager.transferMachine.IsMoving())
            {
                Globalo.LogPrint("", "TRANSFER Z AXIS MOTOR RUNNING.", Globalo.eMessageName.M_INFO);
                Console.WriteLine("Z motor running...");
                return;
            }
            Globalo.motionManager.transferMachine.RunState = OperationState.Stopped;
            isMovingTransfer = true;        //<---이동후 기다리지 않으면 바로 true로 바껴서 얘로만 체크하면 위험

            cts?.Dispose();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;


            

            string logstr = $"[MANUAL] TRANSFER Z AXIS {ePos.ToString()} Move";

            Globalo.LogPrint("", logstr);
            try
            {
                Task<bool> motorTask = Task.Run(() =>
                {
                    Console.WriteLine(" ------------------> TransFer_Z_Move");
                    bool rtn = Globalo.motionManager.transferMachine.TransFer_Z_Move(ePos);
                    bool bComplete = true;

                    int nTimeTick = Environment.TickCount;
                    while (rtn)
                    {
                        if (bManualStopKey) break;
                        bComplete = Globalo.motionManager.transferMachine.ChkZMotorPos(ePos);

                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                        {
                            bComplete = false;
                            Console.WriteLine(" ===> TransFer_Z_Move TIMEOUT");
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
                    logstr = $"[MANUAL] TRANSFER Z AXIS {ePos.ToString()} Move Complete";
                }
                else
                {
                    Console.WriteLine("Move fail");
                    logstr = $"[MANUAL] TRANSFER Z AXIS {ePos.ToString()} Move Fail";
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
            isMovingTransfer = false;
        }


        private async void Manual_XY_Move(Machine.TransferMachine.eTeachingPosList ePos, int PickernoX = 0 , int PickernoY = 0)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Preparing)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 운전 준비 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.OriginRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (isMovingTransfer || Globalo.motionManager.transferMachine.IsMoving())
            {
                Console.WriteLine("XY motor running...");
                Globalo.LogPrint("", "TRANSFER XY AXIS MOTOR RUNNING.", Globalo.eMessageName.M_INFO);
                return;
            }
            Globalo.motionManager.transferMachine.RunState = OperationState.Stopped;

            isMovingTransfer = true;//<---이동후 기다리지 않으면 바로 true로 바껴서 얘로만 체크하면 위험

            string logstr = $"[MANUAL] TRANSFER XY AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            

            try
            {
                cts?.Dispose();
                cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                Task<bool> motorTask = Task.Run(() =>
                {
                    Console.WriteLine(" ------------------> TransFer_XY_Move");

                    bool rtn = Globalo.motionManager.transferMachine.TransFer_XY_Move(ePos, PickernoX, PickernoY);
                    bool bComplete = true;

                    int nTimeTick = Environment.TickCount;
                    while (rtn)
                    {
                        if (bManualStopKey) break;

                        bComplete = Globalo.motionManager.transferMachine.ChkXYMotorPos(ePos);
                        if (bComplete)
                        {
                            //위치 확인 완료
                            Console.WriteLine(" ===> TransFer_XY_Move Complete");
                            break;
                        }
                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                        {
                            bComplete = false;
                            Console.WriteLine(" ===> TransFer_XY_Move TIMEOUT");
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
                    logstr = $"[MANUAL] TRANSFER XY AXIS {ePos.ToString()} Move Complete";
                    Globalo.LogPrint("", logstr);
                }
                else
                {
                    Console.WriteLine("Move fail");
                    logstr = $"[MANUAL] TRANSFER XY AXIS {ePos.ToString()} Move Fail";
                    Globalo.LogPrint("", logstr);
                }

                bManualStopKey = false;
                isMovingTransfer = false;
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
        
        private void BTN_MANUAL_WAIT_POS_XY_Click_1(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.WAIT_POS);
        }

        // X,Y BCR SCAN
        private void button_Manual_Transfer_Left_Bcr_Pos_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_BCR_POS, ManualLoadPosx, ManualLoadPosy);
        }
        private void button_Manual_Transfer_Right_Bcr_Pos_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.RIGHT_TRAY_BCR_POS, ManualLoadPosx, ManualLoadPosy);
        }
        // X,Y TRAY 제품 로드
        private void BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS, ManualLoadPosx, ManualLoadPosy);
        }
        private void BTN_MANUAL_TRANSFER_RIGHT_LOAD_POS_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.RIGHT_TRAY_LOAD_POS, ManualLoadPosx, ManualLoadPosy);
        }
        private void BTN_MANUAL_TRANSFER_LEFT_UNLOAD_POS_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_UNLOAD_POS, ManualLoadPosx, ManualLoadPosy);
        }
        private void BTN_MANUAL_TRANSFER_LEFT_UNLOAD_POS_Z_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.RIGHT_TRAY_UNLOAD_POS, ManualLoadPosx, ManualLoadPosy);
        }
        private void BTN_MANUAL_TRANSFER_SOCKET1_POS_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_A_LOAD, ManualLoadPosx, ManualLoadPosy);
        }

        // X,Y SOCKET 제품 로드
        private void button_Manual_Transfer_A_Socket_Unload_Pos_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_A_UNLOAD);
        }
        private void BTN_MANUAL_TRANSFER_SOCKET3_POS_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_B_LOAD);
        }
        private void button_Manual_Transfer_B_Socket_Unload_Pos_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_B_UNLOAD);
        }
        private void BTN_MANUAL_TRANSFER_SOCKET_C1_POS_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_C_LOAD);
        }
        private void button_Manual_Transfer_C_Socket_Unload_Pos_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_C_UNLOAD);
        }

        private void BTN_MANUAL_TRANSFER_SOCKET_D1_POS_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_D_LOAD);
        }
        private void button_Manual_Transfer_D_Socket_Unload_Pos_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_D_UNLOAD);
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
        #region [TRANSFER Z MOTOR MOVE]

        private void BTN_MANUAL_WAIT_POS_Z_Click_1(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.WAIT_POS);
        }
        //Z BCR
        private void button_Manual_Transfer_Left_Bcr_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_BCR_POS);
        }
        private void button_Manual_Transfer_Right_Bcr_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.RIGHT_TRAY_BCR_POS);
        }
        //Z TRAY 제품 로드
        private void BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS);
        }
        private void BTN_MANUAL_TRANSFER_RIGHT_LOAD_POS_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.RIGHT_TRAY_LOAD_POS);
        }
        private void BTN_MANUAL_TRANSFER_RIGHT_UNLOAD_POS_XY_Click(object sender, EventArgs e)
        {
            Manual_XY_Move(Machine.TransferMachine.eTeachingPosList.RIGHT_TRAY_UNLOAD_POS);
        }
        private void BTN_MANUAL_TRANSFER_RIGHT_UNLOAD_POS_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.RIGHT_TRAY_UNLOAD_POS);
        }
        //Z SOCKET 
        private void BTN_MANUAL_TRANSFER_SOCKET1_POS_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_A_LOAD);
        }
        private void button_Manual_Transfer_A_Socket_Unload_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_A_UNLOAD);
        }
        private void BTN_MANUAL_TRANSFER_SOCKET3_POS_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_B_LOAD);
        }
        private void button_Manual_Transfer_B_Socket_Unload_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_B_UNLOAD);
        }
        private void BTN_MANUAL_TRANSFER_SOCKET_C1_POS_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_C_LOAD);
        }
        private void button_Manual_Transfer_C_Socket_Unload_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_C_UNLOAD);
        }
        private void BTN_MANUAL_TRANSFER_SOCKET_D1_POS_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_D_LOAD);
        }
        private void button_Manual_Transfer_D_Socket_Unload_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.TransferMachine.eTeachingPosList.SOCKET_D_UNLOAD);
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

            for (i = 0; i < 4; i++)
            {
                bRtn = Globalo.motionManager.transferMachine.GetLoadVacuumState(i, true);

                if (bRtn)
                {
                    LoadVacuumOnBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    LoadVacuumOnBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }


                bRtn = Globalo.motionManager.transferMachine.GetUnLoadVacuumState(i, true);

                if (bRtn)
                {
                    UnLoadVacuumOnBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    UnLoadVacuumOnBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
                Thread.Sleep(10);
            }

            //for (i = 0; i < (int)Machine.TransferMachine.eTeachingPosList.TOTAL_TRANSFER_TEACHING_COUNT; i++)

            for (i = 0; i < MotorXY_BtnArr.Length; i++)
            {
                Machine.TransferMachine.eTeachingPosList pos = (Machine.TransferMachine.eTeachingPosList)i;
                if (Globalo.motionManager.transferMachine.ChkXYMotorPos(pos) == true)
                {
                    MotorXY_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    MotorXY_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }



            //Z 축 모터 위치

            for (i = 0; i < MotorZ_BtnArr.Length; i++)
            {
                Machine.TransferMachine.eTeachingPosList pos = (Machine.TransferMachine.eTeachingPosList)i;
                if (Globalo.motionManager.transferMachine.ChkZMotorPos(pos) == true)
                {
                    MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }

        }

        private void ManualTransfer_PosSetX(int offset)
        {
            ManualLoadPosx += offset;
            if (ManualLoadPosx < 0)
            {
                ManualLoadPosx = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.X-1;
            }
            if (ManualLoadPosx > Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.X-1)
            {
                ManualLoadPosx = 0;
            }

            label_Manual_Transfer_PosX.Text = "X : " + (ManualLoadPosx + 1);
        }
        private void ManualTransfer_PosSetY(int offset)
        {
            ManualLoadPosy += offset;
            if (ManualLoadPosy < 0)
            {
                ManualLoadPosy = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.Y-1;
            }
            if (ManualLoadPosy > Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.Y-1)
            {
                ManualLoadPosy = 0;
            }

            label_Manual_Transfer_PosY.Text = "Y : " + (ManualLoadPosy + 1);
        }
        private void button_Manual_Transfer_PosX_Prev_Click(object sender, EventArgs e)
        {
            ManualTransfer_PosSetX(-1);
        }

        private void button_Manual_Transfer_PosX_Next_Click(object sender, EventArgs e)
        {
            ManualTransfer_PosSetX(1);
        }

        private void button_Manual_Transfer_PosY_Prev_Click(object sender, EventArgs e)
        {
            ManualTransfer_PosSetY(-1);
        }

        private void button_Manual_Transfer_PosY_Next_Click(object sender, EventArgs e)
        {
            ManualTransfer_PosSetY(1);
        }

    }
}
