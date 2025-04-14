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
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime - startTime;         // 경과 시간 계산

            if (elapsedTime.Seconds > 30)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 모터 이동 시간 초과");
                ManualTimer.Stop();
            }

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

            MovePos = (int)Data.TeachingData.eTeachPosName.WAIT_POS;
            double[] dOffsetPos = { 0.0, 0.0, 0.0 };



            bool bRtn = false;// Globalo.motorControl.Pcb_Motor_XYT_Move(MovePos, dOffsetPos);
            if (bRtn)
            {
                MoveMotors[0] = (int)MotorControl.ePcbMotor.PCB_X;
                MoveMotors[1] = (int)MotorControl.ePcbMotor.PCB_Y;
                MoveMotors[2] = (int)MotorControl.ePcbMotor.PCB_TH;

                startTime = DateTime.Now;
                ManualTimer.Start();
            }
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

            MovePos = (int)Data.TeachingData.eTeachPosName.WAIT_POS;
            //double dOffsetPos = 0.0;

            bool bRtn = false;// Globalo.motorControl.Motor_Axis_Move(MotorControl.eUnit.PCB_UNIT, MotorControl.ePcbMotor.PCB_Z, MovePos, dOffsetPos);

            if (bRtn)
            {
                MoveMotorCount = 1;
                MoveMotors[0] = (int)MotorControl.ePcbMotor.PCB_Z;

                startTime = DateTime.Now;
                ManualTimer.Start();
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
            //TeachingTimer.Stop();
        }

    }
}
