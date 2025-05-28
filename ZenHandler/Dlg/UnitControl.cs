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
    public enum euNIT : int
    {
        TRANSFER_UNIT = 0, SOCKET_UNIT, LIFT_UNIT, MAGAZINE_UNIT, MAX_UNIT_CNT
    };
    public partial class UnitControl : UserControl
    {
        private Timer blinkTimer;

        private OperationState[] _prevState = new OperationState[(int)euNIT.MAX_UNIT_CNT];
        private bool[] blinkFlags = new bool[(int)euNIT.MAX_UNIT_CNT];        //0 = Transfer , 1 = socket, 2 = Lift , 3 = Magazine
        private string[] unitStates = new string[(int)euNIT.MAX_UNIT_CNT];
        private Button[] unitReadyButtons;                 // 유닛 버튼 배열
        private Button[] unitAutoRunButtons;               // 유닛 버튼 배열
        private Button[] unitStopButtons;                 // 유닛 버튼 배열
        private Button[] unitPauseButtons;                 // 유닛 버튼 배열

        private Color ColorDefault = Color.SteelBlue;
        private Color ColorStop = Color.Red;
        private Color ColorAutoRun = Color.Green;
        private Color ColorPause = Color.Red;
        private Color ColorReady = Color.Yellow;

        public UnitControl()
        {
            InitializeComponent();
            Event.EventManager.PgExitCall += OnPgExit;

            unitReadyButtons = new Button[] { BTN_TRANSFER_UNIT_READY, BTN_SOCKET_UNIT_READY, BTN_LIFT_UNIT_READY, BTN_MAGAZINE_UNIT_READY };
            unitAutoRunButtons = new Button[] { BTN_TRANSFER_UNIT_AUTORUN, BTN_SOCKET_UNIT_AUTORUN, BTN_LIFT_UNIT_AUTORUN, BTN_MAGAZINE_UNIT_AUTORUN };
            unitStopButtons = new Button[] { BTN_TRANSFER_UNIT_STOP, BTN_SOCKET_UNIT_STOP, BTN_LIFT_UNIT_STOP, BTN_MAGAZINE_UNIT_STOP };
            unitPauseButtons = new Button[] { BTN_TRANSFER_UNIT_PAUSE, BTN_SOCKET_UNIT_PAUSE, BTN_LIFT_UNIT_PAUSE, BTN_MAGAZINE_UNIT_PAUSE };

            for (int i = 0; i < (int)euNIT.MAX_UNIT_CNT; i++)
            {
                blinkFlags[i] = false;
                unitStates[i] = "";
                _prevState[i] = OperationState.Stopped;
            }

            blinkTimer = new Timer();
            blinkTimer.Interval = 500; // 0.5초 간격으로 깜빡
            blinkTimer.Tick += BlinkTimer_Tick;
        }
        private void OnPgExit(object sender, EventArgs e)
        {
            Console.WriteLine("UnitControl - OnPgExit");
            blinkTimer.Stop();      // 타이머 중지
            blinkTimer.Dispose();   // 리소스 해제
            blinkTimer = null;
        }
        public void showPanel()
        {
            blinkTimer.Start();
        }
        public void hidePanel()
        {
            blinkTimer.Stop();
        }

        #region [BlinkTimer_Tick]
       
        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.IsMoving() && (Globalo.motionManager.transferMachine.RunState == OperationState.Preparing))
                   // || Globalo.motionManager.transferMachine.RunState == OperationState.OriginRunning))       //TODO: 원점 동작일때는 일단 빼자
            {

                Console.WriteLine("---ReadyBlinkUnit");
                ReadyBlinkUnit((int)euNIT.TRANSFER_UNIT);
            }
            else
            {
                if (_prevState[(int)euNIT.TRANSFER_UNIT] != Globalo.motionManager.transferMachine.RunState)
                {
                    Console.WriteLine("---_prevState");
                    UpdateBtnUnit((int)euNIT.TRANSFER_UNIT, Globalo.motionManager.transferMachine.RunState);

                    _prevState[(int)euNIT.TRANSFER_UNIT] = Globalo.motionManager.transferMachine.RunState;
                }
            }

                
            
            if (Globalo.motionManager.socketAoiMachine.IsMoving())
            {
                if (Globalo.motionManager.socketAoiMachine.RunState == OperationState.Preparing)
                {
                    ReadyBlinkUnit((int)euNIT.SOCKET_UNIT);
                }
            }
            else
            {
                if (_prevState[(int)euNIT.SOCKET_UNIT] != Globalo.motionManager.socketAoiMachine.RunState)
                {
                    UpdateBtnUnit((int)euNIT.SOCKET_UNIT, Globalo.motionManager.socketAoiMachine.RunState);

                    _prevState[(int)euNIT.SOCKET_UNIT] = Globalo.motionManager.socketAoiMachine.RunState;
                }
            }
            if (Globalo.motionManager.liftMachine.IsMoving())
            {
                if (Globalo.motionManager.liftMachine.RunState == OperationState.Preparing)
                {
                    ReadyBlinkUnit((int)euNIT.LIFT_UNIT);
                }
            }
            else
            {
                if (_prevState[(int)euNIT.LIFT_UNIT] != Globalo.motionManager.liftMachine.RunState)
                {
                    UpdateBtnUnit((int)euNIT.LIFT_UNIT, Globalo.motionManager.liftMachine.RunState);

                    _prevState[(int)euNIT.LIFT_UNIT] = Globalo.motionManager.liftMachine.RunState;
                }
            }

            if (Globalo.motionManager.magazineHandler.IsMoving())
            {
                if (Globalo.motionManager.magazineHandler.RunState == OperationState.Preparing)
                {
                    ReadyBlinkUnit((int)euNIT.MAGAZINE_UNIT);
                }
            }
            else
            {
                if (_prevState[(int)euNIT.MAGAZINE_UNIT] != Globalo.motionManager.magazineHandler.RunState)
                {
                    UpdateBtnUnit((int)euNIT.MAGAZINE_UNIT, Globalo.motionManager.magazineHandler.RunState);

                    _prevState[(int)euNIT.MAGAZINE_UNIT] = Globalo.motionManager.magazineHandler.RunState;
                }
            }


            this.label_TransferUnit_Step_Val.Text = Globalo.motionManager.transferMachine.AutoUnitThread.m_nCurrentStep.ToString();
            this.label_LiftUnit_Step_Val.Text = Globalo.motionManager.liftMachine.AutoUnitThread.m_nCurrentStep.ToString();
            this.label_MagazineUnit_Step_Val.Text = Globalo.motionManager.magazineHandler.AutoUnitThread.m_nCurrentStep.ToString();
            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                this.label_SocketUnit_Step_Val.Text = Globalo.motionManager.socketFwMachine.AutoUnitThread.m_nCurrentStep.ToString();
            }
            if (Program.PG_SELECT == HANDLER_PG.AOI)
            {
                this.label_SocketUnit_Step_Val.Text = Globalo.motionManager.socketAoiMachine.AutoUnitThread.m_nCurrentStep.ToString();
            }
            if (Program.PG_SELECT == HANDLER_PG.EEPROM)
            {
                this.label_SocketUnit_Step_Val.Text = Globalo.motionManager.socketEEpromMachine.AutoUnitThread.m_nCurrentStep.ToString();
            }

            

            this.label_TransferUnit_State_Val.Text = Globalo.motionManager.transferMachine.RunState.ToString();
            this.label_LiftUnit_State_Val.Text = Globalo.motionManager.liftMachine.RunState.ToString();
            this.label_MagazineUnit_State_Val.Text = Globalo.motionManager.magazineHandler.RunState.ToString();
            

            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                this.label_SocketUnit_State_Val.Text = Globalo.motionManager.socketFwMachine.RunState.ToString();
            }
            if (Program.PG_SELECT == HANDLER_PG.AOI)
            {
                this.label_SocketUnit_State_Val.Text = Globalo.motionManager.socketAoiMachine.RunState.ToString();
            }
            if (Program.PG_SELECT == HANDLER_PG.EEPROM)
            {
                this.label_SocketUnit_State_Val.Text = Globalo.motionManager.socketEEpromMachine.RunState.ToString();
            }

        }
        private void ReadyBlinkUnit(int index)
        {
            blinkFlags[index] = !blinkFlags[index];
            unitReadyButtons[index].BackColor = blinkFlags[index] ? ColorReady : ColorDefault;
        }
        private void UpdateBtnUnit(int index, OperationState state)
        {
            if (state == OperationState.Standby)
            {
                unitReadyButtons[index].BackColor = ColorReady;
                unitStopButtons[index].BackColor = ColorDefault;
                unitAutoRunButtons[index].BackColor = ColorDefault;
                unitPauseButtons[index].BackColor = ColorDefault;
            }
            else if (state == OperationState.Stopped)
            {
                unitStopButtons[index].BackColor = ColorStop;
                unitReadyButtons[index].BackColor = ColorDefault;
                unitAutoRunButtons[index].BackColor = ColorDefault;
                unitPauseButtons[index].BackColor = ColorDefault;
            }
            else if (state == OperationState.AutoRunning)
            {
                unitAutoRunButtons[index].BackColor = ColorAutoRun;
                unitReadyButtons[index].BackColor = ColorDefault;
                unitStopButtons[index].BackColor = ColorDefault;
                unitPauseButtons[index].BackColor = ColorDefault;
            }
            else if (state == OperationState.Paused)
            {
                unitPauseButtons[index].BackColor = ColorPause;
                unitReadyButtons[index].BackColor = ColorDefault;
                unitStopButtons[index].BackColor = ColorDefault;
                unitAutoRunButtons[index].BackColor = ColorDefault;
            }
        }
        #endregion

        //

        public bool UnitOrigin(euNIT UNIT)
        {
            bool bRtn = true;
            if (UNIT == euNIT.TRANSFER_UNIT)
            {
                bRtn = Globalo.motionManager.transferMachine.OriginRun();
                if (bRtn == false)
                {
                    Globalo.motionManager.transferMachine.StopAuto();
                }
            }
            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                if (UNIT == euNIT.MAGAZINE_UNIT)
                {
                    bRtn = Globalo.motionManager.magazineHandler.OriginRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.magazineHandler.StopAuto();
                    }
                }
            }
            if (Program.PG_SELECT == HANDLER_PG.EEPROM || Program.PG_SELECT == HANDLER_PG.AOI)
            {
                if (UNIT == euNIT.LIFT_UNIT)
                {
                    bRtn = Globalo.motionManager.liftMachine.OriginRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.liftMachine.StopAuto();
                    }
                }
            }
            else if (UNIT == euNIT.SOCKET_UNIT)
            {
                if (Program.PG_SELECT == HANDLER_PG.FW)
                {
                    bRtn = Globalo.motionManager.socketFwMachine.OriginRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.socketFwMachine.StopAuto();
                    }
                }
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    bRtn = Globalo.motionManager.socketAoiMachine.OriginRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.socketAoiMachine.StopAuto();
                    }
                }
                if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    bRtn = Globalo.motionManager.socketEEpromMachine.OriginRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.socketEEpromMachine.StopAuto();
                    }
                }
            }

            unitReadyButtons[(int)UNIT].BackColor = ColorDefault;
            unitStopButtons[(int)UNIT].BackColor = ColorDefault;
            unitAutoRunButtons[(int)UNIT].BackColor = ColorDefault;
            unitPauseButtons[(int)UNIT].BackColor = ColorDefault;
            return bRtn;
        }

        public bool UnitReady(euNIT UNIT)
        {
            bool bRtn = true;
            if (UNIT == euNIT.TRANSFER_UNIT)
            {
                bRtn = Globalo.motionManager.transferMachine.ReadyRun();
                if (bRtn == false)
                {
                    Globalo.motionManager.transferMachine.StopAuto();
                }
            }
            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                if (UNIT == euNIT.MAGAZINE_UNIT)
                {
                    bRtn = Globalo.motionManager.magazineHandler.ReadyRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.magazineHandler.StopAuto();
                    }
                }
            }
            if (Program.PG_SELECT == HANDLER_PG.EEPROM || Program.PG_SELECT == HANDLER_PG.AOI)
            {
                if (UNIT == euNIT.LIFT_UNIT)
                {
                    bRtn = Globalo.motionManager.liftMachine.ReadyRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.liftMachine.StopAuto();
                    }
                }
            }
            else if (UNIT == euNIT.SOCKET_UNIT)
            {
                if (Program.PG_SELECT == HANDLER_PG.FW)
                {
                    bRtn = Globalo.motionManager.socketFwMachine.ReadyRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.socketFwMachine.StopAuto();
                    }
                }
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    bRtn = Globalo.motionManager.socketAoiMachine.ReadyRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.socketAoiMachine.StopAuto();
                    }
                }
                if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    bRtn = Globalo.motionManager.socketEEpromMachine.ReadyRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.socketEEpromMachine.StopAuto();
                    }
                }
            }

            unitReadyButtons[(int)UNIT].BackColor = ColorReady;
            unitStopButtons[(int)UNIT].BackColor = ColorDefault;
            unitAutoRunButtons[(int)UNIT].BackColor = ColorDefault;
            unitPauseButtons[(int)UNIT].BackColor = ColorDefault;
            return bRtn;
        }

        public bool UnitAutoRun(euNIT UNIT)
        {
            bool bRtn = true;
            if (UNIT == euNIT.TRANSFER_UNIT)
            {
                bRtn = Globalo.motionManager.transferMachine.AutoRun();
                if (bRtn == false)
                {
                    Globalo.motionManager.transferMachine.StopAuto();
                }
            }
            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                if (UNIT == euNIT.MAGAZINE_UNIT)
                {
                    bRtn = Globalo.motionManager.magazineHandler.AutoRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.magazineHandler.StopAuto();
                    }
                }
            }
            if (Program.PG_SELECT == HANDLER_PG.EEPROM || Program.PG_SELECT == HANDLER_PG.AOI)
            {
                if (UNIT == euNIT.LIFT_UNIT)
                {
                    bRtn = Globalo.motionManager.liftMachine.AutoRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.liftMachine.StopAuto();
                    }
                }
            }
            else if (UNIT == euNIT.SOCKET_UNIT)
            {
                if (Program.PG_SELECT == HANDLER_PG.FW)
                {
                    bRtn = Globalo.motionManager.socketFwMachine.AutoRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.socketFwMachine.StopAuto();
                    }
                }
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    bRtn = Globalo.motionManager.socketAoiMachine.AutoRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.socketAoiMachine.StopAuto();
                    }
                }
                if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    bRtn = Globalo.motionManager.socketEEpromMachine.AutoRun();
                    if (bRtn == false)
                    {
                        Globalo.motionManager.socketEEpromMachine.StopAuto();
                    }
                }
            }

            unitAutoRunButtons[(int)UNIT].BackColor = ColorAutoRun;
            unitReadyButtons[(int)UNIT].BackColor = ColorDefault;
            unitStopButtons[(int)UNIT].BackColor = ColorDefault;
            unitPauseButtons[(int)UNIT].BackColor = ColorDefault;
            return bRtn;
        }

        public bool UnitPause(euNIT UNIT)
        {
            bool bRtn = true;
            if (UNIT == euNIT.TRANSFER_UNIT)
            {
                Globalo.motionManager.transferMachine.PauseAuto();

            }
            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                if (UNIT == euNIT.MAGAZINE_UNIT)
                {
                    Globalo.motionManager.magazineHandler.PauseAuto();
                }
            }
            if (Program.PG_SELECT == HANDLER_PG.EEPROM || Program.PG_SELECT == HANDLER_PG.AOI)
            {
                if (UNIT == euNIT.LIFT_UNIT)
                {
                    Globalo.motionManager.liftMachine.PauseAuto();
                }
            }
            else if (UNIT == euNIT.SOCKET_UNIT)
            {
                if (Program.PG_SELECT == HANDLER_PG.FW)
                {
                    Globalo.motionManager.socketFwMachine.PauseAuto();
                }
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    Globalo.motionManager.socketAoiMachine.PauseAuto();
                }
                if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    Globalo.motionManager.socketEEpromMachine.PauseAuto();
                }
            }

            unitPauseButtons[(int)UNIT].BackColor = ColorPause;
            unitStopButtons[(int)UNIT].BackColor = ColorDefault;
            unitAutoRunButtons[(int)UNIT].BackColor = ColorDefault;
            unitReadyButtons[(int)UNIT].BackColor = ColorDefault;
            return bRtn;
        }
        

        public bool UnitStop(euNIT UNIT)
        {
            bool bRtn = true;
            if (UNIT == euNIT.TRANSFER_UNIT)
            {
                Globalo.motionManager.transferMachine.StopAuto();
                
            }
            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                if (UNIT == euNIT.MAGAZINE_UNIT)
                {
                    Globalo.motionManager.magazineHandler.StopAuto();
                }
            }
            if (Program.PG_SELECT == HANDLER_PG.EEPROM || Program.PG_SELECT == HANDLER_PG.AOI)
            {
                if (UNIT == euNIT.LIFT_UNIT)
                {
                    Globalo.motionManager.liftMachine.StopAuto();
                }
            }
            else if (UNIT == euNIT.SOCKET_UNIT)
            {
                if (Program.PG_SELECT == HANDLER_PG.FW)
                {
                    Globalo.motionManager.socketFwMachine.StopAuto();
                }
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    Globalo.motionManager.socketAoiMachine.StopAuto();
                }
                if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    Globalo.motionManager.socketEEpromMachine.StopAuto();
                }
            }

            unitStopButtons[(int)UNIT].BackColor = ColorStop;
            unitAutoRunButtons[(int)UNIT].BackColor = ColorDefault;
            unitReadyButtons[(int)UNIT].BackColor = ColorDefault;
            unitPauseButtons[(int)UNIT].BackColor = ColorDefault;
            return bRtn;
        }
        //---------------------------------------------------------------------------------------------------------------------
        //
        //
        // TRANSFER UNIT
        //
        //
        //---------------------------------------------------------------------------------------------------------------------
        private void BTN_TRANSFER_UNIT_READY_Click(object sender, EventArgs e)
        {
            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, "TRANSFER UNIT 운전준비 하시겠습니까 ?");
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                UnitReady(euNIT.TRANSFER_UNIT);
            }
        }

        private void BTN_TRANSFER_UNIT_AUTORUN_Click(object sender, EventArgs e)
        {
            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, "TRANSFER UNIT 자동운전 하시겠습니까 ?");
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                UnitAutoRun(euNIT.TRANSFER_UNIT);
            }
                
        }

        private void BTN_TRANSFER_UNIT_STOP_Click(object sender, EventArgs e)
        {
            UnitStop(euNIT.TRANSFER_UNIT);
        }

        private void BTN_TRANSFER_UNIT_PAUSE_Click(object sender, EventArgs e)
        {
            UnitPause(euNIT.TRANSFER_UNIT);
        }

        //---------------------------------------------------------------------------------------------------------------------
        //
        //
        // SOCKET UNIT
        //
        //
        //---------------------------------------------------------------------------------------------------------------------
        private void BTN_SOCKET_UNIT_READY_Click(object sender, EventArgs e)
        {
            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, "SOCKET UNIT 운전준비 하시겠습니까 ?");
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                UnitReady(euNIT.SOCKET_UNIT);
            }
        }

        private void BTN_SOCKET_UNIT_AUTORUN_Click(object sender, EventArgs e)
        {
            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, "SOCKET UNIT 자동운전 하시겠습니까 ?");
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                UnitAutoRun(euNIT.SOCKET_UNIT);
            }
        }

        private void BTN_SOCKET_UNIT_STOP_Click(object sender, EventArgs e)
        {
            UnitStop(euNIT.SOCKET_UNIT);
        }

        private void BTN_SOCKET_UNIT_PAUSE_Click(object sender, EventArgs e)
        {
            UnitPause(euNIT.SOCKET_UNIT);
        }
        //---------------------------------------------------------------------------------------------------------------------
        //
        //
        // MAGAZINE UNIT
        //
        //
        //---------------------------------------------------------------------------------------------------------------------

        private void BTN_MAGAZINE_UNIT_READY_Click(object sender, EventArgs e)
        {
            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, "MAGAZINE UNIT 운전준비 하시겠습니까 ?");
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                UnitReady(euNIT.MAGAZINE_UNIT);
            }
        }

        private void BTN_MAGAZINE_UNIT_AUTORUN_Click(object sender, EventArgs e)
        {
            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, "MAGAZINE UNIT 자동운전 하시겠습니까 ?");
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                UnitAutoRun(euNIT.MAGAZINE_UNIT);
            }
        }

        private void BTN_MAGAZINE_UNIT_STOP_Click(object sender, EventArgs e)
        {
            UnitStop(euNIT.MAGAZINE_UNIT);
        }

        private void BTN_MAGAZINE_UNIT_PAUSE_Click(object sender, EventArgs e)
        {
            UnitPause(euNIT.MAGAZINE_UNIT);
        }
        //---------------------------------------------------------------------------------------------------------------------
        //
        //
        // LIFT UNIT
        //
        //
        //---------------------------------------------------------------------------------------------------------------------
        private void BTN_LIFT_UNIT_READY_Click(object sender, EventArgs e)
        {
            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, "LIFT UNIT 운전준비 하시겠습니까 ?");
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                UnitReady(euNIT.LIFT_UNIT);
            }
        }

        private void BTN_LIFT_UNIT_AUTORUN_Click(object sender, EventArgs e)
        {
            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, "LIFT UNIT 자동운전 하시겠습니까 ?");
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                UnitAutoRun(euNIT.LIFT_UNIT);
            }
        }

        private void BTN_LIFT_UNIT_STOP_Click(object sender, EventArgs e)
        {
            UnitStop(euNIT.LIFT_UNIT);
        }

        private void BTN_LIFT_UNIT_PAUSE_Click(object sender, EventArgs e)
        {
            UnitPause(euNIT.LIFT_UNIT);
        }



        

        //
        //
    }//END
}
