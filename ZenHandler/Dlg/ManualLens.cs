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
    public partial class ManualLens : UserControl
    {
        private int MoveMotorCount = 0;
        private int MovePos = 0;
        private int[] MoveMotors;
        private DateTime startTime = DateTime.Now;
        private Timer ManualTimer;

        private Button[] MotorBtnArr = new Button[10];
        private Button[] IoBtnArr = new Button[2];
        public ManualLens()
        {
            InitializeComponent();

            MoveMotorCount = 0;
            MoveMotors = new int[MotorControl.LENS_UNIT_COUNT];
            ManualTimer = new Timer();
            ManualTimer.Interval = 300; // 1초 (1000밀리초) 간격 설정
            ManualTimer.Tick += new EventHandler(Manual_Timer_Tick);


            ManualLensUiSet();
        }
        private void ManualLensUiSet()
        {
            int i = 0;
            MotorBtnArr[0] = BTN_MANUAL_LENS_WAIT_POS_XY;
            MotorBtnArr[1] = BTN_MANUAL_LENS_LOAD_POS_XY;
            MotorBtnArr[2] = BTN_MANUAL_LENS_LASER_POS_XY;
            MotorBtnArr[3] = BTN_MANUAL_LENS_OC_POS_XY;
            MotorBtnArr[4] = BTN_MANUAL_LENS_CHART_POS_XY;
            MotorBtnArr[5] = BTN_MANUAL_LENS_WAIT_POS_Z;
            MotorBtnArr[6] = BTN_MANUAL_LENS_LOAD_POS_Z;
            MotorBtnArr[7] = BTN_MANUAL_LENS_LASER_POS_Z;
            MotorBtnArr[8] = BTN_MANUAL_LENS_OC_POS_Z;
            MotorBtnArr[9] = BTN_MANUAL_LENS_CHART_POS_Z;

            IoBtnArr[0] = BTN_MANUAL_LENS_VACUUM_ON;
            IoBtnArr[1] = BTN_MANUAL_LENS_VACUUM_OFF;

            for (i = 0; i < MotorBtnArr.Length; i++)
            {
                MotorBtnArr[i].BackColor = ColorTranslator.FromHtml("#C3A279");
                MotorBtnArr[i].ForeColor = Color.White;

                MotorBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            MotorBtnArr[0].BackColor = ColorTranslator.FromHtml("#4C4743");
            for (i = 0; i < IoBtnArr.Length; i++)
            {
                IoBtnArr[i].BackColor = ColorTranslator.FromHtml("#C3A279");
                IoBtnArr[i].ForeColor = Color.White;
                IoBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            IoBtnArr[0].BackColor = ColorTranslator.FromHtml("#4C4743");


            // this.groupBox1.ResumeLayout(false);
            //this.groupBox1.PerformLayout();

        }
        private void Manual_Timer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime - startTime; // 경과 시간 계산
            if (elapsedTime.Seconds > 30)
            {
                ManualTimer.Stop();
            }

            //if (Globalo.motorControl.GetStopMultiAxis(0, MoveMotorCount, MoveMotors) &&
            //    Globalo.motorControl.GetMultiAxisPosCheck(0, MoveMotorCount, MoveMotors, MovePos))
            //{
            //    ManualTimer.Stop();
            //    Globalo.LogPrint("ManualControl", "[INFO] 모터 이동 완료");
            //}

        }
    }
}
