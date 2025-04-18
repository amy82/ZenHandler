﻿using System;
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
    public partial class UnitControl : UserControl
    {
        private Timer blinkTimer;

        private OperationState[] _prevState = new OperationState[4];
        private bool[] blinkFlags = new bool[4];        //0 = Transfer , 1 = socket, 2 = Lift , 3 = Magazine
        private string[] unitStates = new string[4];
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

            for (int i = 0; i < 4; i++)
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

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.IsMoving() && (Globalo.motionManager.transferMachine.RunState == OperationState.Preparing ||
                    Globalo.motionManager.transferMachine.RunState == OperationState.Originning))
            {

                Console.WriteLine("---ReadyBlinkUnit");
                ReadyBlinkUnit(0);
            }
            else
            {
                if (_prevState[0] != Globalo.motionManager.transferMachine.RunState)
                {
                    Console.WriteLine("---_prevState");
                    UpdateBtnUnit(0, Globalo.motionManager.transferMachine.RunState);

                    _prevState[0] = Globalo.motionManager.transferMachine.RunState;
                }
            }

                
            
            if (Globalo.motionManager.socketMachine.IsMoving())
            {
                if (Globalo.motionManager.socketMachine.RunState == OperationState.Preparing)
                {
                    ReadyBlinkUnit(1);
                }
            }
            else
            {
                if (_prevState[1] != Globalo.motionManager.socketMachine.RunState)
                {
                    UpdateBtnUnit(1, Globalo.motionManager.socketMachine.RunState);

                    _prevState[1] = Globalo.motionManager.socketMachine.RunState;
                }
            }
            if (Globalo.motionManager.liftMachine.IsMoving())
            {
                if (Globalo.motionManager.liftMachine.RunState == OperationState.Preparing)
                {
                    ReadyBlinkUnit(2);
                }
            }
            else
            {
                if (_prevState[2] != Globalo.motionManager.liftMachine.RunState)
                {
                    UpdateBtnUnit(2, Globalo.motionManager.liftMachine.RunState);

                    _prevState[2] = Globalo.motionManager.liftMachine.RunState;
                }
            }

            if (Globalo.motionManager.magazineHandler.IsMoving())
            {
                if (Globalo.motionManager.magazineHandler.RunState == OperationState.Preparing)
                {
                    ReadyBlinkUnit(3);
                }
            }
            else
            {
                if (_prevState[3] != Globalo.motionManager.magazineHandler.RunState)
                {
                    UpdateBtnUnit(3, Globalo.motionManager.magazineHandler.RunState);

                    _prevState[3] = Globalo.motionManager.magazineHandler.RunState;
                }
            }
        }
        private void ReadyBlinkUnit(int index)
        {
            blinkFlags[index] = !blinkFlags[index];
            unitReadyButtons[index].BackColor = blinkFlags[index] ? ColorReady : ColorDefault;
        }
        private void UpdateBtnUnit(int index, OperationState state)
        {
            if (state == OperationState.PreparationComplete)
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
        //---------------------------------------------------------------------------------------------------------------------
        //
        //
        // TRANSFER UNIT
        //
        //
        //---------------------------------------------------------------------------------------------------------------------
        private void BTN_TRANSFER_UNIT_READY_Click(object sender, EventArgs e)
        {
            bool bRtn = Globalo.motionManager.transferMachine.ReadyRun();
            if(bRtn)
            {
                unitReadyButtons[0].BackColor = ColorReady;
                unitStopButtons[0].BackColor = ColorDefault;
                unitAutoRunButtons[0].BackColor = ColorDefault;
                unitPauseButtons[0].BackColor = ColorDefault;
            }
            
        }

        private void BTN_TRANSFER_UNIT_AUTORUN_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.AutoRun();
        }

        private void BTN_TRANSFER_UNIT_STOP_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.StopAuto();
            unitStopButtons[0].BackColor = ColorStop;
            unitReadyButtons[0].BackColor = ColorDefault;
            unitAutoRunButtons[0].BackColor = ColorDefault;
            unitPauseButtons[0].BackColor = ColorDefault;
        }

        private void BTN_TRANSFER_UNIT_PAUSE_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.PauseAuto();
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

        }

        private void BTN_SOCKET_UNIT_AUTORUN_Click(object sender, EventArgs e)
        {

        }

        private void BTN_SOCKET_UNIT_STOP_Click(object sender, EventArgs e)
        {

        }

        private void BTN_SOCKET_UNIT_PAUSE_Click(object sender, EventArgs e)
        {

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

        }

        private void BTN_MAGAZINE_UNIT_AUTORUN_Click(object sender, EventArgs e)
        {

        }

        private void BTN_MAGAZINE_UNIT_STOP_Click(object sender, EventArgs e)
        {

        }

        private void BTN_MAGAZINE_UNIT_PAUSE_Click(object sender, EventArgs e)
        {

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

        }

        private void BTN_LIFT_UNIT_AUTORUN_Click(object sender, EventArgs e)
        {

        }

        private void BTN_LIFT_UNIT_STOP_Click(object sender, EventArgs e)
        {

        }

        private void BTN_LIFT_UNIT_PAUSE_Click(object sender, EventArgs e)
        {

        }



        

        //
        //
    }//END
}
