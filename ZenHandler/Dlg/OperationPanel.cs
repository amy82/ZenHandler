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
    public partial class OperationPanel : UserControl
    {
        public Button[] RunBtnArr = new Button[5];
        public int[] RunBtnSizeArr = { 136, 136, 136, 136, 136 };
        public const int RunButtonHeight = 80;
        private bool ReadyBtnOn = false;
        private bool isTimerRunning = false;  // 타이머 시작 시 실행 중으로 설정
        private System.Windows.Forms.Timer _timerRunButton;

        public OperationPanel()
        {
            InitializeComponent();

            int i = 0;
            RunBtnArr[0] = BTN_MAIN_ORIGIN1;
            RunBtnArr[1] = BTN_MAIN_READY1;
            RunBtnArr[2] = BTN_MAIN_PAUSE1;
            RunBtnArr[3] = BTN_MAIN_STOP1;
            RunBtnArr[4] = BTN_MAIN_START1;


            int MainBtnStartGap = 1;
            int MainBtnStartYGap = 2;

            int BtnPosX = MainBtnStartGap;

            // for (i = RunBtnArr.Length - 1; i > -1; i--)
            //for (i = 0; i < RunBtnArr.Length; i++)
            //{
            //    RunBtnArr[i].BackColor = Color.ForestGreen;
            //    RunBtnArr[i].Width = RunBtnSizeArr[i];
            //    RunBtnArr[i].Height = RunButtonHeight;



            //    //RunBtnArr[i].Location = new System.Drawing.Point((LeftPanel.Width - BtnPosX), LeftPanel.Height - RunButtonHeight - MainBtnHGap-5);


            //    //RunBtnArr[i].Location = new System.Drawing.Point(BtnPosX, LeftPanel.Height - RunButtonHeight - MainBtnHGap-100);
            //    RunBtnArr[i].Location = new System.Drawing.Point(BtnPosX, panel_ProductionInfo.Height + MainBtnStartYGap);
            //    BtnPosX += RunBtnArr[i].Width + MainBtnWGap;

            //}
        }

        public void AutoButtonSet(OperationState operation)
        {
            BTN_MAIN_ORIGIN1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF); //C3A279
            BTN_MAIN_PAUSE1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
            BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
            BTN_MAIN_STOP1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
            BTN_MAIN_START1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
            switch (operation)
            {
                //case OperationState.Originning:
                //    BTN_MAIN_ORIGIN1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);//FFB230
                //    break;
                case OperationState.AutoRunning:
                    BTN_MAIN_START1.BackColor = ButtonColor.BTN_START_ON;// ColorTranslator.FromHtml(ButtonColor.BTN_ON);//FFB230
                    break;
                case OperationState.Paused:
                    BTN_MAIN_PAUSE1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_PAUSE_ON);
                    break;
                case OperationState.PreparationComplete:
                    BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    break;
                case OperationState.Stopped:
                    BTN_MAIN_STOP1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    break;
            }
        }
        private void RunButtonUITimerFn(int Mode)
        {
            if (Mode == 1)
            {
                if (ProgramState.CurrentState == OperationState.Preparing)
                {
                    if (ReadyBtnOn)
                    {
                        BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                    }
                    else
                    {
                        BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    }
                    ReadyBtnOn = !ReadyBtnOn;
                }
            }
            else if (Mode == 2)
            {
                if (ProgramState.CurrentState == OperationState.AutoRunning)
                {
                    if (ReadyBtnOn)
                    {
                        BTN_MAIN_START1.BackColor = Color.LimeGreen;// ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                    }
                    else
                    {
                        BTN_MAIN_START1.BackColor = ButtonColor.BTN_START_ON; //ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    }
                    ReadyBtnOn = !ReadyBtnOn;
                }
            }


            if (Mode == 1)
            {
                if (ProgramState.CurrentState == OperationState.PreparationComplete)
                {
                    BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);

                    //MainGuideTxtSet("운전준비 완료!");
                    AutoRunTimerStop();
                }
            }

            if (ProgramState.CurrentState == OperationState.Stopped)
            {

                //MainGuideTxtSet("설비 정지 상태입니다.");
                AutoRunTimerStop();
            }
        }
        public void AutoRunTimerStop()
        {
            if (_timerRunButton != null)
            {
                _timerRunButton.Stop();
                _timerRunButton.Dispose();
                _timerRunButton = null;
            }

            isTimerRunning = false;
        }
        public void AutoRunBtnUiTimer(int Mode, int interval = 300)
        {
            if (isTimerRunning)
            {
                Console.WriteLine("Timer is already running.");
                return;  // 이미 타이머가 실행 중이면 실행하지 않음
            }
            isTimerRunning = true;  // 타이머 시작 시 실행 중으로 설정
            ReadyBtnOn = false;
            _timerRunButton = new System.Windows.Forms.Timer();
            _timerRunButton.Interval = interval;
            _timerRunButton.Tick += (s, e) => RunButtonUITimerFn(Mode); // 실행할 함수 지정
            _timerRunButton.Start();
        }
    }
}
