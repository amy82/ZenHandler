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
        private OperationState _AllState = OperationState.Stopped;
        private OperationState _prevState = OperationState.Stopped;

        public Button[] RunBtnArr = new Button[5];
        public int[] RunBtnSizeArr = { 136, 136, 136, 136, 136 };
        public const int RunButtonHeight = 80;
        private bool OrgBtnOn = false;
        private bool ReadyBtnOn = false;
        private bool RunBtnOn = false;
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

            _AllState = OperationState.Stopped;
            _prevState = _AllState;

            _timerRunButton = new System.Windows.Forms.Timer();
            _timerRunButton.Interval = 500;
            _timerRunButton.Tick += (s, e) => RunButtonUITimerFn(); // 실행할 함수 지정
            _timerRunButton.Start();


            AutoButtonSet(_AllState);
        }

        public bool StartHomeProcess()
        {
            //TRANSFER UNIT
            
            if (Globalo.mMainPanel.unitControl.UnitOrigin(euNIT.TRANSFER_UNIT) == false)
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] TRANSFER UNIT ORIGIN FAIL", Globalo.eMessageName.M_WARNING);
            }
            else
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] TRANSFER UNIT ORIGIN START");
            }
            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                //MAGAZINE UNIT
                if (Globalo.mMainPanel.unitControl.UnitOrigin(euNIT.MAGAZINE_UNIT) == false)
                {
                    Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] MAGAZINE UNIT ORIGIN FAIL", Globalo.eMessageName.M_WARNING);
                }
                else
                {
                    Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] MAGAZINE UNIT ORIGIN START");
                }
                
            }
            else
            {
                //LIFT UNIT
                if (Globalo.mMainPanel.unitControl.UnitOrigin(euNIT.LIFT_UNIT) == false)
                {
                    Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] LIFT UNIT ORIGIN FAIL", Globalo.eMessageName.M_WARNING);
                }
                else
                {
                    Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] LIFT UNIT ORIGIN START");
                }
            }
            //SOCKET UNIT
            if (Globalo.mMainPanel.unitControl.UnitOrigin(euNIT.SOCKET_UNIT) == false)
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] SOCKET UNIT ORIGIN FAIL", Globalo.eMessageName.M_WARNING);
            }
            else
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[ORIGIN] SOCKET UNIT ORIGIN START");
            }





            //Globalo.operationPanel.AutoButtonSet(ProgramState.CurrentState);
            return true;

        }
        public void StartAutoReadyProcess()
        {
            //TRANSFER UNIT

            if (Globalo.mMainPanel.unitControl.UnitReady(euNIT.TRANSFER_UNIT) == false)
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[READY] TRANSFER UNIT READY FAIL", Globalo.eMessageName.M_WARNING);
            }
            else
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[READY] TRANSFER UNIT READY START");
            }

            //SOCKET UNIT

            //MAGAZINE UNIT

            //LIFT UNIT

            //Globalo.operationPanel.AutoRunBtnUiTimer(1);
            //Globalo.operationPanel.AutoButtonSet(ProgramState.CurrentState);

        }
        public void StopAutoProcess()
        {
            Globalo.mMainPanel.unitControl.UnitStop(euNIT.TRANSFER_UNIT);
            Globalo.mMainPanel.unitControl.UnitStop(euNIT.LIFT_UNIT);
            Globalo.mMainPanel.unitControl.UnitStop(euNIT.MAGAZINE_UNIT);
            Globalo.mMainPanel.unitControl.UnitStop(euNIT.SOCKET_UNIT);
        }
        public void PauseAutoProcess()
        {
            Globalo.mMainPanel.unitControl.UnitPause(euNIT.TRANSFER_UNIT);
            Globalo.mMainPanel.unitControl.UnitPause(euNIT.LIFT_UNIT);
            Globalo.mMainPanel.unitControl.UnitPause(euNIT.MAGAZINE_UNIT);
            Globalo.mMainPanel.unitControl.UnitPause(euNIT.SOCKET_UNIT);
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

            //Globalo.camControl.setOverlayText("PAUS", Color.Red);         //초기화

            //Globalo.threadControl.autoRunthread.Pause();

        }
        public bool StartAutoProcess()
        {
            //모터 구동중 체크
            //운전준비 체크

            

            //TRANSFER UNIT
            if (Globalo.mMainPanel.unitControl.UnitAutoRun(euNIT.TRANSFER_UNIT) == true)
            {
                Globalo.LogPrint("MainForm", "[AUTO] TRANSFER UNIT AUTO RUN START");
            }
            else
            {
                Globalo.LogPrint("MainForm", "[AUTO] TRANSFER UNIT AUTO RUN FAIL", Globalo.eMessageName.M_WARNING);
            }

            //SOCKET UNIT
            if (Globalo.mMainPanel.unitControl.UnitAutoRun(euNIT.SOCKET_UNIT) == true)
            {
                Globalo.LogPrint("MainForm", "[AUTO] SOCKET UNIT AUTO RUN START");
            }
            else
            {
                Globalo.LogPrint("MainForm", "[AUTO] SOCKET UNIT AUTO RUN FAIL", Globalo.eMessageName.M_WARNING);
            }


            //MAGAZINE UNIT
            if (Globalo.mMainPanel.unitControl.UnitAutoRun(euNIT.MAGAZINE_UNIT) == true)
            {
                Globalo.LogPrint("MainForm", "[AUTO] MAGAZINE UNIT AUTO RUN START");
            }
            else
            {
                Globalo.LogPrint("MainForm", "[AUTO] MAGAZINE UNIT AUTO RUN FAIL", Globalo.eMessageName.M_WARNING);
            }


            //LIFT UNIT
            if (Globalo.mMainPanel.unitControl.UnitAutoRun(euNIT.LIFT_UNIT) == true)
            {
                Globalo.LogPrint("MainForm", "[AUTO] LIFT UNIT AUTO RUN START");
            }
            else
            {
                Globalo.LogPrint("MainForm", "[AUTO] LIFT UNIT AUTO RUN FAIL", Globalo.eMessageName.M_WARNING);
            }

            return true;
        }
        public void AutoButtonSet(OperationState operation)
        {
            BTN_MAIN_ORIGIN1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
            BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
            BTN_MAIN_PAUSE1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
            BTN_MAIN_STOP1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
            BTN_MAIN_START1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
            switch (operation)
            {
                case OperationState.OriginRunning:
                    BTN_MAIN_ORIGIN1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    break;
                case OperationState.Preparing:
                    BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    break;
                case OperationState.AutoRunning:
                    BTN_MAIN_START1.BackColor = ButtonColor.BTN_START_ON;
                    break;
                case OperationState.Paused:
                    BTN_MAIN_PAUSE1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_PAUSE_ON);
                    break;
                case OperationState.Standby:
                    BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    break;
                case OperationState.Stopped:
                    BTN_MAIN_STOP1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    break;
            }
        }
        private void RunButtonUITimerFn()
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.OriginRunning && Globalo.motionManager.socketAoiMachine.RunState == OperationState.OriginRunning &&
                Globalo.motionManager.liftMachine.RunState == OperationState.OriginRunning && Globalo.motionManager.magazineHandler.RunState == OperationState.OriginRunning)
            {
                _AllState = OperationState.OriginRunning;
            }
            else if (Globalo.motionManager.transferMachine.RunState == OperationState.Preparing && Globalo.motionManager.socketAoiMachine.RunState == OperationState.Preparing &&
                Globalo.motionManager.liftMachine.RunState == OperationState.Preparing && Globalo.motionManager.magazineHandler.RunState == OperationState.Preparing)
            {
                _AllState = OperationState.Preparing;
            }
            else if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning && Globalo.motionManager.socketAoiMachine.RunState == OperationState.AutoRunning &&
                Globalo.motionManager.liftMachine.RunState == OperationState.AutoRunning && Globalo.motionManager.magazineHandler.RunState == OperationState.AutoRunning)
            {
                _AllState = OperationState.AutoRunning;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Standby && Globalo.motionManager.socketAoiMachine.RunState == OperationState.Standby &&
                Globalo.motionManager.liftMachine.RunState == OperationState.Standby && Globalo.motionManager.magazineHandler.RunState == OperationState.Standby)
            {
                _AllState = OperationState.Standby;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused || 
                Globalo.motionManager.socketAoiMachine.RunState == OperationState.Paused ||
                Globalo.motionManager.liftMachine.RunState == OperationState.Paused || 
                Globalo.motionManager.magazineHandler.RunState == OperationState.Paused)
            {
                _AllState = OperationState.Paused;
            }

            if (Globalo.motionManager.transferMachine.RunState == OperationState.Stopped ||
                Globalo.motionManager.socketAoiMachine.RunState == OperationState.Stopped ||
                Globalo.motionManager.liftMachine.RunState == OperationState.Stopped ||
                Globalo.motionManager.magazineHandler.RunState == OperationState.Stopped)
            {
                _AllState = OperationState.Stopped;
            }

            //-----------------------------------------------------------------------------------------------------------------------
            //
            //  동작중
            //
            //-----------------------------------------------------------------------------------------------------------------------
            if (_AllState == OperationState.OriginRunning)
            {
                if (OrgBtnOn)
                {
                    BTN_MAIN_ORIGIN1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                }
                else
                {
                    BTN_MAIN_ORIGIN1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                }
                OrgBtnOn = !OrgBtnOn;
            }
            else if (_AllState == OperationState.Preparing)
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
            else if (_AllState == OperationState.AutoRunning)
            {
                if (RunBtnOn)
                {
                    BTN_MAIN_START1.BackColor = ButtonColor.BTN_START_ON;
                }
                else
                {
                    BTN_MAIN_START1.BackColor = ButtonColor.BTN_START_OFF;
                }
                RunBtnOn = !RunBtnOn;
            }
            //-----------------------------------------------------------------------------------------------------------------------
            //
            // 상태
            //
            //-----------------------------------------------------------------------------------------------------------------------
            if(_prevState != _AllState)
            {
                if (_AllState == OperationState.AutoRunning)
                {

                    BTN_MAIN_START1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                    BTN_MAIN_PAUSE1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                    BTN_MAIN_STOP1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                }
                if (_AllState == OperationState.Standby)
                {
                    BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    BTN_MAIN_START1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                    BTN_MAIN_PAUSE1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                    BTN_MAIN_STOP1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                }
                if (_AllState == OperationState.Paused)
                {
                    BTN_MAIN_PAUSE1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                    BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                    BTN_MAIN_STOP1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                }
                if (_AllState == OperationState.Stopped)
                {
                    BTN_MAIN_STOP1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    BTN_MAIN_READY1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                    BTN_MAIN_PAUSE1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                    BTN_MAIN_PAUSE1.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
                }

                _prevState = _AllState;
            }
            
        }


        
        private void BTN_MAIN_STOP1_Click(object sender, EventArgs e)
        {
            StopAutoProcess();
            Globalo.mManualPanel.ManualDlgStop();//정지하면 Manual Tab 에서 구동중인 모터 구동함수 빠져나오는 용도
            Globalo.LogPrint("MainForm", "[AUTO] AUTO RUN STOP.");
        }

        private void BTN_MAIN_ORIGIN1_Click(object sender, EventArgs e)
        {
            if (ChkRunState() == false)
            {
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
            if(ChkRunState() == false)
            {
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
        
        private void BTN_MAIN_PAUSE1_Click(object sender, EventArgs e)
        {
            //if (ProgramState.CurrentState == OperationState.Stopped)
            //{
            //    Globalo.LogPrint("ManualCMainFormontrol", "[INFO] 설비 정지 상태입니다.", Globalo.eMessageName.M_WARNING);
            //    return;
            //}
            //if (ProgramState.CurrentState == OperationState.Paused)
            //{
            //    Globalo.LogPrint("ManualCMainFormontrol", "[INFO] 일시 정지 상태입니다.", Globalo.eMessageName.M_WARNING);
            //    return;
            //}
            //if (ProgramState.CurrentState != OperationState.AutoRunning)
            //{
            //    Globalo.LogPrint("ManualCMainFormontrol", "[INFO] 자동 운전 중이 아닙니다..", Globalo.eMessageName.M_WARNING);
            //    return;
            //}

            //각 머신 정지일때만 일시정지하면될듯


            PauseAutoProcess();
        }

        private void BTN_MAIN_START1_Click(object sender, EventArgs e)
        {
            if (ChkRunState() == false)
            {
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

            string logStr = "전체 자동운전 진행 하시겠습니까 ?";
            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, logStr);
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                StartAutoProcess();     //자동 운전 시작
            }

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
        private bool ChkRunState()
        {
            if (Globalo.motionManager.transferMachine.RunState != OperationState.Stopped && Globalo.motionManager.transferMachine.RunState != OperationState.OriginDone
                && Globalo.motionManager.transferMachine.RunState != OperationState.Standby)
            {
                Globalo.LogPrint("MainForm", "[INFO] 설비 정지 상태에서 사용 가능", Globalo.eMessageName.M_WARNING);
                return false;
            }
            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                if (Globalo.motionManager.magazineHandler.RunState != OperationState.Stopped && Globalo.motionManager.magazineHandler.RunState != OperationState.OriginDone
                    && Globalo.motionManager.magazineHandler.RunState != OperationState.Standby)
                {
                    Globalo.LogPrint("MainForm", "[INFO] 설비 정지 상태에서 사용 가능", Globalo.eMessageName.M_WARNING);
                    return false;
                }
            }
            else
            {
                if (Globalo.motionManager.liftMachine.RunState != OperationState.Stopped && Globalo.motionManager.liftMachine.RunState != OperationState.OriginDone
                    && Globalo.motionManager.liftMachine.RunState != OperationState.Standby)
                {
                    Globalo.LogPrint("MainForm", "[INFO] 설비 정지 상태에서 사용 가능", Globalo.eMessageName.M_WARNING);
                    return false;
                }
                if (Globalo.motionManager.socketEEpromMachine.RunState != OperationState.Stopped && Globalo.motionManager.socketEEpromMachine.RunState != OperationState.OriginDone
                    && Globalo.motionManager.socketEEpromMachine.RunState != OperationState.Standby)
                {
                    Globalo.LogPrint("MainForm", "[INFO] 설비 정지 상태에서 사용 가능", Globalo.eMessageName.M_WARNING);
                    return false;
                }
                if (Globalo.motionManager.socketAoiMachine.RunState != OperationState.Stopped && Globalo.motionManager.socketAoiMachine.RunState != OperationState.OriginDone
                    && Globalo.motionManager.socketAoiMachine.RunState != OperationState.Standby)
                {
                    Globalo.LogPrint("MainForm", "[INFO] 설비 정지 상태에서 사용 가능", Globalo.eMessageName.M_WARNING);
                    return false;
                }
            }
                
            
            
            return true;
        }
    }//end
}
