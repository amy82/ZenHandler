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
        private Button[] IoBtnArr = new Button[2];
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

            IoBtnArr[0] = BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON1;
            IoBtnArr[1] = BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF1;

            for (i = 0; i < MotorBtnArr.Length; i++)
            {
                MotorBtnArr[i].BackColor = ColorTranslator.FromHtml("#C3A279");
                MotorBtnArr[i].ForeColor = Color.White;

                MotorBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            //MotorBtnArr[0].BackColor = ColorTranslator.FromHtml("#4C4743");   //모터 위치 이동 완료시 색
            for (i = 0; i < IoBtnArr.Length; i++)
            {
                IoBtnArr[i].BackColor = ColorTranslator.FromHtml("#C3A279");
                IoBtnArr[i].ForeColor = Color.White;
                IoBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            //IoBtnArr[0].BackColor = ColorTranslator.FromHtml("#4C4743");


            // this.groupBox1.ResumeLayout(false);
            //this.groupBox1.PerformLayout();

        }
        private void Manual_Timer_Tick(object sender, EventArgs e)
        {
            //IO 동작 상태

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

            Globalo.motionManager.transferMachine.LensGripOn(0, true);

            //Globalo.dIoControl.DioWriteOutportByte(1, 0, (uint)DioDefine.DIO_OUT_ADDR.LENS_GRIP_FOR, (uint)DioDefine.DIO_OUT_ADDR.LENS_GRIP_BACK);

            //public bool DioWriteOutportByte(int nIndex, int nOffset, uint uOnAddr, uint uOffAddr)
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
            Globalo.motionManager.transferMachine.LensGripOn(0, false);
            //Globalo.dIoControl.DioWriteOutportByte(1, 0, (uint)DioDefine.DIO_OUT_ADDR.LENS_GRIP_BACK, (uint)DioDefine.DIO_OUT_ADDR.LENS_GRIP_FOR);
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

            bool bRtn = Globalo.motionManager.transferMachine.TransFer_Z_Move(ePos);
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON2_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.LensGripOn(1, true);
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON3_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.LensGripOn(2, true);
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_ON4_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.LensGripOn(3, true);
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF2_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.LensGripOn(1, false);
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF3_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.LensGripOn(2, false);
        }

        private void BTN_MANUAL_TRANSFER_LOAD_VACUUM_OFF4_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.LensGripOn(3, false);
        }
    }
}
