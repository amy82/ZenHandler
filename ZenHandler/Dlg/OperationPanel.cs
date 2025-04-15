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

            //int i = 0;
            RunBtnArr[0] = BTN_MAIN_ORIGIN1;
            RunBtnArr[1] = BTN_MAIN_READY1;
            RunBtnArr[2] = BTN_MAIN_PAUSE1;
            RunBtnArr[3] = BTN_MAIN_STOP1;
            RunBtnArr[4] = BTN_MAIN_START1;


            int MainBtnStartGap = 1;
            //int MainBtnStartYGap = 2;

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
        public bool StartHomeProcess()
        {
            //int i = 0;
            //for (i = 0; i < MotorControl.PCB_UNIT_COUNT; i++)
            //{
            //    if (Globalo.motorControl.PcbMotorAxis[i].AmpEnable() == false)
            //    {
            //        eLogPrint("ManualCMainFormontrol", $"[ORIGIN] {Globalo.motorControl.PcbMotorAxis[i].Name} AXIS SERVO ON FAIL", Globalo.eMessageName.M_WARNING);
            //        return false;
            //    }
            //    if (Globalo.motorControl.PcbMotorAxis[i].GetStopAxis() == false)
            //    {
            //        eLogPrint("ManualCMainFormontrol", $"[ORIGIN] {Globalo.motorControl.PcbMotorAxis[i].Name} AXIS 구동중입니다.", Globalo.eMessageName.M_WARNING);
            //        return false;
            //    }
            //}

            //for (i = 0; i < MotorControl.LENS_UNIT_COUNT; i++)
            //{
            //    if (Globalo.motorControl.LensMotorAxis[i].AmpEnable() == false)
            //    {
            //        eLogPrint("ManualCMainFormontrol", $"[ORIGIN] {Globalo.motorControl.LensMotorAxis[i].Name} AXIS SERVO ON FAIL", Globalo.eMessageName.M_WARNING);
            //        return false;
            //    }
            //    if (Globalo.motorControl.LensMotorAxis[i].GetStopAxis() == false)
            //    {
            //        eLogPrint("ManualCMainFormontrol", $"[ORIGIN] {Globalo.motorControl.LensMotorAxis[i].Name} AXIS 구동중입니다.", Globalo.eMessageName.M_WARNING);
            //        return false;
            //    }
            //}
            if (Globalo.threadControl.autoRunthread.GetThreadRun() == true)
            {
                if (Globalo.threadControl.autoRunthread.GetThreadPause() == true)
                {
                    //eLogPrint("ManualCMainFormontrol", "[ORIGIN] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                    //g_clTaskWork[nUnit].m_nCurrentStep = abs(g_clTaskWork[nUnit].m_nCurrentStep);

                    // g_clTaskWork[nUnit].m_nAutoFlag = MODE_AUTO;
                    Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] ORIGIN PAUSE RELEASE");

                    Globalo.taskWork.m_nCurrentStep = Math.Abs(Globalo.taskWork.m_nCurrentStep);
                    ProgramState.CurrentState = OperationState.Originning;
                    //Globalo.threadControl.readyRunthread.Pause();
                    return true;
                }



                Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] 동작중 사용 불가", Globalo.eMessageName.M_WARNING);
                return false;
            }

            Globalo.taskWork.m_nStartStep = 10000;
            Globalo.taskWork.m_nEndStep = 20000;
            Globalo.taskWork.m_nCurrentStep = 10000;

            ProgramState.CurrentState = OperationState.Originning;
            bool bRtn = Globalo.threadControl.autoRunthread.Start();
            if (bRtn == false)
            {
                ProgramState.CurrentState = OperationState.Stopped;
            }

            Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] ORIGIN RUN START");
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

        public bool StartAutoProcess()
        {
            //모터 구동중 체크
            //운전준비 체크
            if (Globalo.threadControl.autoRunthread.GetThreadRun() == true)
            {
                if (ProgramState.CurrentState == OperationState.Paused)
                {
                    Globalo.taskWork.m_nCurrentStep = Math.Abs(Globalo.taskWork.m_nCurrentStep);
                    Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN RESUME");
                }
                else
                {
                    Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN START FAIL");
                    return false;
                }
            }
            else
            {
                Globalo.taskWork.m_nCurrentStep = 30000;
            }
            //
            //

            //if (Globalo.threadControl.autoRunthread.GetThreadRun() == true)
            //{
            //    if (Globalo.threadControl.autoRunthread.GetThreadPause() == true)
            //    {
            //        Globalo.taskWork.m_nCurrentStep = Math.Abs(Globalo.taskWork.m_nCurrentStep);

            //        ProgramState.CurrentState = OperationState.AutoRunning;

            //        return false;
            //    }
            //    else
            //    {

            //        return false;
            //    }
            //}


            Globalo.taskWork.m_nStartStep = 30000;
            Globalo.taskWork.m_nEndStep = 70000;

            ProgramState.CurrentState = OperationState.AutoRunning;
            bool bRtn = Globalo.threadControl.autoRunthread.Start();
            if (bRtn == false)
            {
                ProgramState.CurrentState = OperationState.Stopped;
                return false;
            }
            Globalo.operationPanel.AutoRunBtnUiTimer(2, 500);
            //MainGuideTxtSet("자동 운전 중입니다.");
            Globalo.operationPanel.AutoButtonSet(ProgramState.CurrentState);


            Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN START");
            return true;
        }

        public void StopAutoProcess()
        {
            Globalo.operationPanel.AutoRunTimerStop();
            Globalo.mManualPanel.ManualDlgStop();

            ProgramState.CurrentState = OperationState.Stopped;

            //MainGuideTxtSet("설비 정지 상태입니다.");

            if (Globalo.threadControl.autoRunthread.GetThreadRun() == true)
            {
                Globalo.threadControl.autoRunthread.Stop();
            }
            Globalo.motionManager.AllMotorStop();
            
            Globalo.operationPanel.AutoButtonSet(ProgramState.CurrentState);
        }
        private void BTN_MAIN_STOP1_Click(object sender, EventArgs e)
        {
            StopAutoProcess();

            if (Globalo.threadControl.manualThread.GetThreadRun() == false)
            {
                //Globalo.LogPrint("", "[CCD] MANUAL CCD CLOSE");
                //Globalo.threadControl.manualThread.runfn(FThread.ManualThread.eManualType.M_CCD_CLOSE);// 3);
            }
            Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN STOP.");
        }

        private void BTN_MAIN_ORIGIN1_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.Originning)
            {
                Globalo.LogPrint("MainForm", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("MainForm", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, "전체 원점복귀 하시겠습니까 ?");
            DialogResult result = messagePopUp.ShowDialog();
            if (result == DialogResult.Yes)
            {
                StartHomeProcess();
            }
            //DialogResult.Cancel
        }
        public void StartAutoReadyProcess()
        {
            if (Globalo.threadControl.autoRunthread.GetThreadRun() == true)
            {
                if (ProgramState.CurrentState == OperationState.Paused)
                {
                    Globalo.taskWork.m_nCurrentStep = Math.Abs(Globalo.taskWork.m_nCurrentStep);
                    Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN RESUME");
                }
                else
                {
                    Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN START FAIL");
                    return;
                }
            }
            else
            {
                Globalo.taskWork.m_nCurrentStep = 20000;

                if (Globalo.threadControl.autoRunthread.GetThreadRun() == true)
                {
                    Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN START FAIL");
                    return;
                }
                Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN START");
            }


            Globalo.taskWork.m_nStartStep = 20000;
            Globalo.taskWork.m_nEndStep = 30000;


            ProgramState.CurrentState = OperationState.Preparing;



            bool bRtn = Globalo.threadControl.autoRunthread.Start();

            if (bRtn == false)
            {
                Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN START FAIL");
                ProgramState.CurrentState = OperationState.Stopped;
                return;

            }
            Globalo.operationPanel.AutoRunBtnUiTimer(1);

            //MainGuideTxtSet("설비 운전준비중 입니다.");
            Globalo.operationPanel.AutoButtonSet(ProgramState.CurrentState);

        }
        private void BTN_MAIN_READY1_Click(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.Originning)
            {
                Globalo.LogPrint("MainForm", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("MainForm", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Preparing)
            {
                Globalo.LogPrint("MainForm", "[INFO] 운전 준비 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            //if (ProgramState.CurrentState == OperationState.Paused)
            //{
            //    eLogPrint("ManualCMainFormontrol", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
            //    return;
            //}

            string logStr = "운전준비 하시겠습니까 ?";

            if (ProgramState.CurrentState == OperationState.Paused)
            {
                if (Math.Abs(Globalo.taskWork.m_nCurrentStep) < 20000 || Math.Abs(Globalo.taskWork.m_nCurrentStep) >= 30000)
                {
                    Globalo.LogPrint("MainForm", "[INFO] 설비 정지 상태가 아닙니다.", Globalo.eMessageName.M_WARNING);
                    return;
                }
                logStr = "운전준비 재개 하시겠습니까 ?";    //이때 스텝이 20000보다 작아야된다.
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
