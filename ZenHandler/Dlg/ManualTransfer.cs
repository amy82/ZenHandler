using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Dlg
{
    public partial class ManualTransfer : UserControl
    {
        private int MoveMotorCount;
        private int MovePos;
        private int[] MoveMotors;
        private DateTime startTime;
        private Timer ManualTimer;

        private Button[] MotorBtnArr = new Button[4];

        private Button[] LoadVacuumOnBtnArr = new Button[4];
        private Button[] LoadVacuumOffBtnArr = new Button[4];

        private Button[] UnLoadVacuumOnBtnArr = new Button[4];
        private Button[] UnLoadVacuumOffBtnArr = new Button[4];


        public ManualTransfer()
        {
            InitializeComponent();

            MoveMotorCount = 0;
            MoveMotors = new int[MotorControl.PCB_UNIT_COUNT];
            ManualTimer = new Timer();
            ManualTimer.Interval = 300; // 1초 (1000밀리초) 간격 설정
            ManualTimer.Tick += new EventHandler(Manual_Timer_Tick);


            ManualPcbUiSet();
        }
        private void ManualPcbUiSet()
        {
            int i = 0;
            MotorBtnArr[0] = BTN_MANUAL_TRANSFER_WAIT_POS_XY;
            MotorBtnArr[1] = BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_XY;
            MotorBtnArr[2] = BTN_MANUAL_TRANSFER_WAIT_POS_Z;
            MotorBtnArr[3] = BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_Z;

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

            for (i = 0; i < MotorBtnArr.Length; i++)
            {
                MotorBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                MotorBtnArr[i].ForeColor = Color.White;

                MotorBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            //MotorBtnArr[0].BackColor = ColorTranslator.FromHtml("#4C4743");   //모터 위치 이동 완료시 색


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
            }



            //WAIT_POS = 0, LEFT_TRAY_LOAD_POS, RIGHT_TRAY_LOAD_POS, SOCKET_POS1, SOCKET_POS2, SOCKET_POS3, SOCKET_POS4
            //X,Y 축 모터 위치
            BTN_MANUAL_TRANSFER_WAIT_POS_XY.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_XY.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_RIGHT_LOAD_POS_XY.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_SOCKET1_POS_XY.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_SOCKET2_POS_XY.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_SOCKET3_POS_XY.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_SOCKET4_POS_XY.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);

            if (Globalo.motionManager.transferMachine.ChkXYMotorPos(Data.eTeachPosName.WAIT_POS) == true)
            {
                BTN_MANUAL_TRANSFER_WAIT_POS_XY.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else if (Globalo.motionManager.transferMachine.ChkXYMotorPos(Data.eTeachPosName.LEFT_TRAY_LOAD_POS) == true)
            {
                BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_XY.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else if (Globalo.motionManager.transferMachine.ChkXYMotorPos(Data.eTeachPosName.RIGHT_TRAY_LOAD_POS) == true)
            {
                BTN_MANUAL_TRANSFER_RIGHT_LOAD_POS_XY.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else if (Globalo.motionManager.transferMachine.ChkXYMotorPos(Data.eTeachPosName.SOCKET_POS1) == true)
            {
                BTN_MANUAL_TRANSFER_SOCKET1_POS_XY.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }


            //Z 축 모터 위치

            BTN_MANUAL_TRANSFER_WAIT_POS_Z.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_Z.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_RIGHT_LOAD_POS_Z.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_SOCKET1_POS_Z.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_SOCKET2_POS_Z.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_SOCKET3_POS_Z.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            BTN_MANUAL_TRANSFER_SOCKET4_POS_Z.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);


            if (Globalo.motionManager.transferMachine.ChkZMotorPos(Data.eTeachPosName.WAIT_POS) == true)
            {
                BTN_MANUAL_TRANSFER_WAIT_POS_Z.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else if (Globalo.motionManager.transferMachine.ChkZMotorPos(Data.eTeachPosName.LEFT_TRAY_LOAD_POS) == true)
            {
                BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_Z.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else if (Globalo.motionManager.transferMachine.ChkZMotorPos(Data.eTeachPosName.RIGHT_TRAY_LOAD_POS) == true)
            {
                BTN_MANUAL_TRANSFER_RIGHT_LOAD_POS_Z.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else if (Globalo.motionManager.transferMachine.ChkZMotorPos(Data.eTeachPosName.SOCKET_POS1) == true)
            {
                BTN_MANUAL_TRANSFER_SOCKET1_POS_Z.BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }


            //DateTime currentTime = DateTime.Now;
            //TimeSpan elapsedTime = currentTime - startTime;         // 경과 시간 계산

            //if (elapsedTime.Seconds > 30)
            //{
            //    Globalo.LogPrint("ManualControl", "[INFO] 모터 이동 시간 초과");
            //    //ManualTimer.Stop();
            //}

            //if (Globalo.motorControl.GetStopMultiAxis(MotorControl.eUnit.PCB_UNIT, MoveMotorCount, MoveMotors) &&
            //    Globalo.motorControl.GetMultiAxisPosCheck(MotorControl.eUnit.PCB_UNIT, MoveMotorCount, MoveMotors, MovePos))
            //{
            //    ManualTimer.Stop();
            //    Globalo.LogPrint("ManualControl", "[INFO] 모터 이동 완료");
            //}

        }
        private void BTN_MANUAL_LOAD_POS_XY_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            MovePos = (int)Data.TeachingData.eTeachPosName.LOAD_POS;
            double[] dOffsetPos = { 0.0, 0.0, 0.0 };

            bool bRtn = false;// Globalo.motorControl.Pcb_Motor_XYT_Move(MovePos, dOffsetPos);
            if (bRtn)
            {
                MoveMotorCount = 3;
                MoveMotors[0] = (int)MotorControl.ePcbMotor.PCB_X;
                MoveMotors[1] = (int)MotorControl.ePcbMotor.PCB_Y;
                MoveMotors[2] = (int)MotorControl.ePcbMotor.PCB_TH;

                startTime = DateTime.Now;
                ManualTimer.Start();
            }
        }


        private void BTN_MANUAL_VACUUM_ON_Click_1(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Globalo.motionManager.transferMachine.LoadVacuumOn(0, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #1 LOAD PICKER VACUUM ON");
        }
        private void BTN_MANUAL_VACUUM_OFF_Click_1(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.LoadVacuumOn(0, false);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #1 LOAD PICKER VACUUM OFF");
        }
        private void BTN_MANUAL_WAIT_POS_XY_Click_1(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Data.eTeachPosName ePos = Data.eTeachPosName.WAIT_POS;
            string logstr = $"[MANUAL] TRANSFER XY AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_XY_Move(ePos);
        }

        private void BTN_MANUAL_WAIT_POS_Z_Click_1(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Data.eTeachPosName ePos = Data.eTeachPosName.WAIT_POS;
            string logstr = $"[MANUAL] TRANSFER Z AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);
            bool bRtn = Globalo.motionManager.transferMachine.TransFer_Z_Move(ePos);
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
                ManualTimer.Start();
            }
            else
            {
                ManualTimer.Stop();
            }
        }

        private void BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_XY_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Data.eTeachPosName ePos = Data.eTeachPosName.LEFT_TRAY_LOAD_POS;

            string logstr = $"[MANUAL] TRANSFER XY AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_XY_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_RIGHT_LOAD_POS_XY_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Data.eTeachPosName ePos = Data.eTeachPosName.RIGHT_TRAY_LOAD_POS;

            string logstr = $"[MANUAL] TRANSFER XY AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_XY_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_SOCKET1_POS_XY_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Data.eTeachPosName ePos = Data.eTeachPosName.SOCKET_POS1;

            string logstr = $"[MANUAL] TRANSFER XY AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);


            bool bRtn = Globalo.motionManager.transferMachine.TransFer_XY_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_SOCKET2_POS_XY_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Data.eTeachPosName ePos = Data.eTeachPosName.SOCKET_POS2;

            string logstr = $"[MANUAL] TRANSFER XY AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_XY_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_SOCKET3_POS_XY_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Data.eTeachPosName ePos = Data.eTeachPosName.SOCKET_POS3;

            string logstr = $"[MANUAL] TRANSFER XY AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);


            bool bRtn = Globalo.motionManager.transferMachine.TransFer_XY_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_SOCKET4_POS_XY_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Data.eTeachPosName ePos = Data.eTeachPosName.SOCKET_POS4;

            string logstr = $"[MANUAL] TRANSFER XY AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_XY_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_Z_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Data.eTeachPosName ePos = Data.eTeachPosName.LEFT_TRAY_LOAD_POS;

            string logstr = $"[MANUAL] TRANSFER Z AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_Z_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_RIGHT_LOAD_POS_Z_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Data.eTeachPosName ePos = Data.eTeachPosName.RIGHT_TRAY_LOAD_POS;

            string logstr = $"[MANUAL] TRANSFER Z AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_Z_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_SOCKET1_POS_Z_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Data.eTeachPosName ePos = Data.eTeachPosName.SOCKET_POS1;

            string logstr = $"[MANUAL] TRANSFER Z AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_Z_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_SOCKET2_POS_Z_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Data.eTeachPosName ePos = Data.eTeachPosName.SOCKET_POS2;

            string logstr = $"[MANUAL] TRANSFER Z AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_Z_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_SOCKET3_POS_Z_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Data.eTeachPosName ePos = Data.eTeachPosName.SOCKET_POS3;

            string logstr = $"[MANUAL] TRANSFER Z AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_Z_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_SOCKET4_POS_Z_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Data.eTeachPosName ePos = Data.eTeachPosName.SOCKET_POS4;

            string logstr = $"[MANUAL] TRANSFER Z AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_Z_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON2_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.LoadVacuumOn(1, true);


            Globalo.LogPrint("ManualControl", "[TRANSFER] #2 LOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON3_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.LoadVacuumOn(2, true);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #3 LOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON4_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.LoadVacuumOn(3, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #4 LOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF2_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.LoadVacuumOn(1, false);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #2 LOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF3_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.LoadVacuumOn(2, false);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #3 LOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF4_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.LoadVacuumOn(3, false);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #4 LOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON1_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Globalo.motionManager.transferMachine.UnLoadVacuumOn(0, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #1 UNLOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF1_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.UnLoadVacuumOn(0, false);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #1 UNLOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON2_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Globalo.motionManager.transferMachine.UnLoadVacuumOn(1, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #2 UNLOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF2_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.UnLoadVacuumOn(1, false);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #2 UNLOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON3_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Globalo.motionManager.transferMachine.UnLoadVacuumOn(2, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #3 UNLOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF3_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.UnLoadVacuumOn(2, false);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #3 UNLOAD PICKER VACUUM OFF");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_ON4_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            Globalo.motionManager.transferMachine.UnLoadVacuumOn(3, true);
            Globalo.LogPrint("ManualControl", "[TRANSFER] #4 UNLOAD PICKER VACUUM ON");
        }

        private void BTN_MANUAL_TRANSFER_UNLOAD_VACUUM_OFF4_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.UnLoadVacuumOn(3, false);

            Globalo.LogPrint("ManualControl", "[TRANSFER] #4 UNLOAD PICKER VACUUM OFF");
        }
    }
}
