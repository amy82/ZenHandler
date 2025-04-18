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
            Event.EventManager.LanguageChanged += OnLanguageChanged;
            Event.EventManager.PgExitCall += OnPgExit;
            //int i = 0;
            RunBtnArr[0] = BTN_MAIN_ORIGIN1;
            RunBtnArr[1] = BTN_MAIN_READY1;
            RunBtnArr[2] = BTN_MAIN_PAUSE1;
            RunBtnArr[3] = BTN_MAIN_STOP1;
            RunBtnArr[4] = BTN_MAIN_START1;

        }
        private void OnLanguageChanged(object sender, EventArgs e)
        {
            // 이벤트 처리
            Console.WriteLine("OperationPanel - OnLanguageChanged");
            ManualTitleLabel.Text = Resource.Strings.OP_TITLE;
            BTN_MAIN_ORIGIN1.Text = Resource.Strings.OP_ORIGIN;
            BTN_MAIN_READY1.Text = Resource.Strings.OP_READY;
            BTN_MAIN_PAUSE1.Text = Resource.Strings.OP_PAUSE;
            BTN_MAIN_STOP1.Text = Resource.Strings.OP_STOP;
            BTN_MAIN_START1.Text = Resource.Strings.OP_START;
        }
        private void OnPgExit(object sender, EventArgs e)
        {
            Console.WriteLine("OperationPanel - OnPgExit");
            _timerRunButton.Stop();      // 타이머 중지
            _timerRunButton.Dispose();   // 리소스 해제
            _timerRunButton = null;
        }
        public bool StartHomeProcess()
        {
            bool bRtn = false;
            bRtn = Globalo.motionManager.transferMachine.OriginRun();
            if (bRtn == false)
            {
                Globalo.motionManager.transferMachine.StopAuto();
                Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] TRANSFER UNIT ORIGIN FAIL", Globalo.eMessageName.M_WARNING);
            }
            else
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] TRANSFER UNIT ORIGIN START");
            }



            //Globalo.operationPanel.AutoButtonSet(ProgramState.CurrentState);
            return true;

        }
        public void StartAutoReadyProcess()
        {
            bool bRtn = false;
            bRtn = Globalo.motionManager.transferMachine.ReadyRun();
            if (bRtn == false)
            {
                Globalo.motionManager.transferMachine.StopAuto();
                Globalo.LogPrint("ManualCMainFormontrol", "[READY] TRANSFER UNIT READY FAIL", Globalo.eMessageName.M_WARNING);
            }
            else
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[READY] TRANSFER UNIT READY START");
            }
            //Globalo.operationPanel.AutoRunBtnUiTimer(1);
            //Globalo.operationPanel.AutoButtonSet(ProgramState.CurrentState);

        }
        public void StopAutoProcess()
        {
            Globalo.operationPanel.AutoRunTimerStop();
            Globalo.mManualPanel.ManualDlgStop();               //정지하면 Manual Tab 에서 구동중인 모터 구동함수 빠져나오는 용도

            Globalo.motionManager.AllMotorStop();

            //Globalo.operationPanel.AutoButtonSet(ProgramState.CurrentState);
        }
        public bool StartAutoProcess()
        {
            //모터 구동중 체크
            //운전준비 체크

            bool bRtn = false;
            bRtn = Globalo.motionManager.transferMachine.AutoRun();
            if (bRtn)
            {
                Globalo.LogPrint("MainForm", "[AUTO] TRANSFER UNIT AUTO RUN START");
            }
            else
            {
                Globalo.LogPrint("MainForm", "[AUTO] TRANSFER UNIT AUTO RUN FAIL");
            }

            //if (Globalo.threadControl.autoRunthread.GetThreadRun() == true)
            //{
            //    if (ProgramState.CurrentState == OperationState.Paused)
            //    {
            //        Globalo.taskWork.m_nCurrentStep = Math.Abs(Globalo.taskWork.m_nCurrentStep);
            //        Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN RESUME");
            //    }
            //    else
            //    {
            //        Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN START FAIL");
            //        return false;
            //    }
            //}
            //else
            //{
            //    Globalo.taskWork.m_nCurrentStep = 30000;
            //}

            //Globalo.taskWork.m_nStartStep = 30000;
            //Globalo.taskWork.m_nEndStep = 70000;

            //ProgramState.CurrentState = OperationState.AutoRunning;
            //bool bRtn = Globalo.threadControl.autoRunthread.Start();
            //if (bRtn == false)
            //{
            //    ProgramState.CurrentState = OperationState.Stopped;
            //    return false;
            //}


            Globalo.operationPanel.AutoRunBtnUiTimer(2, 500);
            Globalo.operationPanel.AutoButtonSet(ProgramState.CurrentState);
            
            return true;
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

        

        
        private void BTN_MAIN_STOP1_Click(object sender, EventArgs e)
        {
            StopAutoProcess();

            Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN STOP.");
        }

        private void BTN_MAIN_ORIGIN1_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Originning)
            {
                Globalo.LogPrint("MainForm", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Preparing)
            {
                Globalo.LogPrint("MainForm", "[INFO] 운전 준비 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("MainForm", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, "전체 원점동작 하시겠습니까 ?");
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                StartHomeProcess();
            }
            
        }
        
        private void BTN_MAIN_READY1_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Originning)
            {
                Globalo.LogPrint("MainForm", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Preparing)
            {
                Globalo.LogPrint("MainForm", "[INFO] 운전 준비 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("MainForm", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }


            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, "전체 운전준비 하시겠습니까 ?");
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                StartAutoReadyProcess();
            }
        }
        public void PauseAutoProcess()
        {
            ProgramState.CurrentState = OperationState.Paused;
            //labelGuide.Text = "설비 일시정지 상태입니다.";

            //if (labelGuide.InvokeRequired)
            //{
            //    //labelGuide.BeginInvoke(new Action(() => labelGuide.Text = "설비 일시정지 상태입니다."));
            //    labelGuide.BeginInvoke(new Action(() => MainGuideTxtSet("설비 일시정지 상태입니다.")));
            //}
            //else
            //{
            //    MainGuideTxtSet("설비 일시정지 상태입니다.");
            //}

            Globalo.camControl.setOverlayText("PAUS", Color.Red);         //초기화

            Globalo.threadControl.autoRunthread.Pause();

            Globalo.operationPanel.AutoRunTimerStop();

            Globalo.operationPanel.AutoButtonSet(ProgramState.CurrentState);
        }
        private void BTN_MAIN_PAUSE1_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.Stopped)
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[INFO] 설비 정지 상태입니다.", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[INFO] 일시 정지 상태입니다.", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.PreparationComplete)
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[INFO] 자동 운전 중이 아닙니다..", Globalo.eMessageName.M_WARNING);
                return;
            }



            PauseAutoProcess();
        }

        private void BTN_MAIN_START1_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("MainForm", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.ManualTesting)
            {
                Globalo.LogPrint("ManualControl", "[INFO] MANUAL 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.STATE_DRIVER_CONNECT == false)
            {
                Globalo.LogPrint("ManualControl", "[INFO] DRIVER 미연결 상태입니다.", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.STATE_CLINET_CONNECT == false)
            {
                Globalo.LogPrint("ManualControl", "[INFO] CLINET 미연결 상태입니다.", Globalo.eMessageName.M_WARNING);
                return;
            }

            //if (ProgramState.CurrentState == OperationState.Stopped)
            //{
            //    Globalo.LogPrint("MainForm", "[INFO] 운전준비가 완료되지 않았습니다.", Globalo.eMessageName.M_WARNING);
            //    return;
            //}

            string logStr = "자동운전 진행 하시겠습니까 ?";

            if (ProgramState.CurrentState == OperationState.Paused)
            {
                if (Math.Abs(Globalo.taskWork.m_nCurrentStep) < 30000 || Math.Abs(Globalo.taskWork.m_nCurrentStep) >= 90000)
                {
                    Globalo.LogPrint("MainForm", "[INFO] 운전 준비 상태가 아닙니다.", Globalo.eMessageName.M_WARNING);
                    return;
                }
                logStr = "자동운전 재개 하시겠습니까 ?";
                //if (Globalo.taskWork.m_nCurrentStep >= 20000 && Globalo.taskWork.m_nCurrentStep < 30000)
            }
            else
            {
                if (Globalo.threadControl.autoRunthread.GetThreadRun() == true)
                {
                    Globalo.LogPrint("MainForm", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                    return;
                }
            }

            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, logStr);
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                //StartAutoProcess();     //자동 운전 시작
            }
        }
    }
}
