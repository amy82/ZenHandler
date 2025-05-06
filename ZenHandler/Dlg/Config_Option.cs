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
    public partial class Config_Option : UserControl
    {
        public Config_Option()
        {
            InitializeComponent();

            setInterface();
            
        }
        public void setInterface()
        {
            int i = 0;
            for (i = 0; i < 20; i++)
            {
                poisonComboBox_BcrPort.Items.Add("COM" + (i + 1).ToString());
            }

            ComboBox_Language.Items.Add("ko");
            ComboBox_Language.Items.Add("en");
            ComboBox_Language.Items.Add("es");
            ComboBox_Language.SelectedIndex = 0;
        }
        private void label_PinCountMax_Click(object sender, EventArgs e)
        {
            string sValue = label_PinCountMax.Text;
            NumPadForm popupForm = new NumPadForm(sValue);

            DialogResult dialogResult = popupForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                if (popupForm.NumPadResult.Contains(".") == true || popupForm.NumPadResult.Length < 1)
                {
                    //Globalo.LogPrint("Recipe", "소수 점이 포함돼 있습니다.", Globalo.eMessageName.M_WARNING);
                    Globalo.LogPrint("Config", "입력이 값 확인바랍니다.", Globalo.eMessageName.M_WARNING);

                }
                else
                {
                    label_PinCountMax.Text = popupForm.NumPadResult;
                }
            }
        }

        private void label_CsvScanMax_Click(object sender, EventArgs e)
        {
            string sValue = label_CsvScanMax.Text;
            NumPadForm popupForm = new NumPadForm(sValue);

            DialogResult dialogResult = popupForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                if (popupForm.NumPadResult.Contains(".") == true || popupForm.NumPadResult.Length < 1)
                {
                    Globalo.LogPrint("Config", "입력이 값 확인바랍니다.", Globalo.eMessageName.M_WARNING);

                }
                else
                {
                    label_CsvScanMax.Text = popupForm.NumPadResult;
                }
            }
        }
        public void ShowOptionData()
        {
            string comData = Globalo.yamlManager.configData.SerialPort.Bcr;
            
            int index = poisonComboBox_BcrPort.Items.IndexOf(comData);
            if (index < 0)
            {
                poisonComboBox_BcrPort.SelectedIndex = 0;  // 첫 번째 항목 선택
            }
            else
            {
                poisonComboBox_BcrPort.SelectedIndex = index;
            }

            comData = Globalo.yamlManager.configData.DrivingSettings.Language;
            index = ComboBox_Language.Items.IndexOf(comData);
            if (index < 0)
            {
                ComboBox_Language.SelectedIndex = 0;  // 첫 번째 항목 선택
            }
            else
            {
                ComboBox_Language.SelectedIndex = index;
            }
            label_PinCountMax.Text = Globalo.yamlManager.configData.DrivingSettings.PinCountMax.ToString();
            label_CsvScanMax.Text = Globalo.yamlManager.configData.DrivingSettings.CsvScanMonth.ToString();

            label_Config_Tray_GapX_Val.Text = Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX.ToString("0.0##");
            label_Config_Tray_GapY_Val.Text = Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY.ToString("0.0##");
            label_Config_Socket_GapX_Val.Text = Globalo.motionManager.transferMachine.productLayout.SocketGap.GapX.ToString("0.0##");
            label_Config_Socket_GapY_Val.Text = Globalo.motionManager.transferMachine.productLayout.SocketGap.GapY.ToString("0.0##");
            label_Config_Ng_GapX_Val.Text = Globalo.motionManager.transferMachine.productLayout.NgGap.GapX.ToString("0.0##");
            label_Config_Ng_GapY_Val.Text = Globalo.motionManager.transferMachine.productLayout.NgGap.GapY.ToString("0.0##");
        }
        public void GetOptionData()
        {
            //Serial Port
            Globalo.yamlManager.configData.SerialPort.Bcr = poisonComboBox_BcrPort.Text;
            Globalo.yamlManager.configData.DrivingSettings.Language = ComboBox_Language.Text;


            Globalo.yamlManager.configData.DrivingSettings.PinCountMax = int.Parse(label_PinCountMax.Text);
            Globalo.yamlManager.configData.DrivingSettings.CsvScanMonth = int.Parse(label_CsvScanMax.Text);

            //제품 간격 - Tray , Socket
            Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX = double.Parse(label_Config_Tray_GapX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY = double.Parse(label_Config_Tray_GapY_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.SocketGap.GapX = double.Parse(label_Config_Socket_GapX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.SocketGap.GapY = double.Parse(label_Config_Socket_GapY_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.NgGap.GapX = double.Parse(label_Config_Ng_GapX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.NgGap.GapY = double.Parse(label_Config_Ng_GapY_Val.Text);
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
