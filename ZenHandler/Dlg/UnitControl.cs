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
        public UnitControl()
        {
            InitializeComponent();
        }

        private void BTN_TRANSFER_UNIT_READY_Click(object sender, EventArgs e)
        {
            //bool bRtn = Globalo.motionManager.transferMachine.OriginRun();
            bool bRtn = Globalo.motionManager.transferMachine.ReadyRun();

        }

        private void BTN_TRANSFER_UNIT_AUTORUN_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.AutoRun();
        }

        private void BTN_TRANSFER_UNIT_STOP_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.StopAuto();
        }

        private void BTN_TRANSFER_UNIT_PAUSE_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.PauseAuto();
        }
    }
}
