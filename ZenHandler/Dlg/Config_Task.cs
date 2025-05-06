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
    public partial class Config_Task : UserControl
    {
        public Config_Task()
        {
            InitializeComponent();
        }

        public void ShowTaskData()
        {
            bool setBool = Globalo.yamlManager.configData.DrivingSettings.IdleReportPass;

            


            hopeCheckBox_PinCountUse.Checked = Globalo.yamlManager.configData.DrivingSettings.PinCountUse;
            hopeCheckBox_ImageGrabUse.Checked = Globalo.yamlManager.configData.DrivingSettings.ImageGrabUse;
            checkBox_IdleReportPass.Checked = Globalo.yamlManager.configData.DrivingSettings.IdleReportPass;
            checkBox_BcrGo.Checked = Globalo.yamlManager.configData.DrivingSettings.EnableAutoStartBcr;
        }

        public void GetTaskData()
        {
            //운전 설정
            Globalo.yamlManager.configData.DrivingSettings.PinCountUse = hopeCheckBox_PinCountUse.Checked;
            Globalo.yamlManager.configData.DrivingSettings.ImageGrabUse = hopeCheckBox_ImageGrabUse.Checked;
            Globalo.yamlManager.configData.DrivingSettings.IdleReportPass = checkBox_IdleReportPass.Checked;
            Globalo.yamlManager.configData.DrivingSettings.EnableAutoStartBcr = checkBox_BcrGo.Checked;
        }
        public void showPanel()
        {
            this.Visible = true;
            if (ProgramState.ON_LINE_MOTOR == true)
            {
                //myTeachingGrid.MotorStateRun(true);
            }
            //myTeachingGrid.ShowTeachingData();
            //TeachResolution(Globalo.motionManager.transferMachine.teachingConfig.Resolution[SelectAxisIndex].ToString("0.#"));
        }
        public void hidePanel()
        {
            this.Visible = false;
            //myTeachingGrid.MotorStateRun(false);
        }
    }
}
