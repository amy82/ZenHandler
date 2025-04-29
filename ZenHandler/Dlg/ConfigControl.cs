using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ZenHandler.Dlg
{
    public partial class ConfigControl : UserControl
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용

        private enum eManualBtn : int
        {
            pcbTab = 0, lensTab
        };
        public ConfigControl(int _w, int _h)
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(Form_Paint);
            
            this.Width = _w;
            this.Height = _h;

            setInterface();

        }
        public void RefreshConfig()
        {
            ShowDriveSet();
            ShowComPort();
            ShowLanguage();
        }
        public void GetConfigData()
        {
            //
            Globalo.yamlManager.configData.DrivingSettings.PinCountMax = int.Parse(label_PinCountMax.Text);
            Globalo.yamlManager.configData.DrivingSettings.CsvScanMonth = int.Parse(label_CsvScanMax.Text);


            //운전 설정
            Globalo.yamlManager.configData.DrivingSettings.PinCountUse = hopeCheckBox_PinCountUse.Checked;
            Globalo.yamlManager.configData.DrivingSettings.ImageGrabUse = hopeCheckBox_ImageGrabUse.Checked;
            Globalo.yamlManager.configData.DrivingSettings.IdleReportPass = checkBox_IdleReportPass.Checked;
            Globalo.yamlManager.configData.DrivingSettings.EnableAutoStartBcr = checkBox_BcrGo.Checked;

            //Serial Port
            Globalo.yamlManager.configData.SerialPort.Bcr = poisonComboBox_BcrPort.Text;
            Globalo.yamlManager.configData.DrivingSettings.Language = ComboBox_Language.Text;

            //제품 간격 - Tray , Socket
            Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX = double.Parse(label_Config_Tray_GapX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY = double.Parse(label_Config_Tray_GapY_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.SocketGap.GapX = double.Parse(label_Config_Socket_GapX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.SocketGap.GapY = double.Parse(label_Config_Socket_GapY_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.NgGap.GapX = double.Parse(label_Config_Ng_GapX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.NgGap.GapY = double.Parse(label_Config_Ng_GapY_Val.Text);

        }
        public void ShowDriveSet()
        {
            bool setBool = Globalo.yamlManager.configData.DrivingSettings.IdleReportPass;
            hopeCheckBox_PinCountUse.Checked = Globalo.yamlManager.configData.DrivingSettings.PinCountUse;
            hopeCheckBox_ImageGrabUse.Checked = Globalo.yamlManager.configData.DrivingSettings.ImageGrabUse;
            checkBox_IdleReportPass.Checked = Globalo.yamlManager.configData.DrivingSettings.IdleReportPass;
            checkBox_BcrGo.Checked = Globalo.yamlManager.configData.DrivingSettings.EnableAutoStartBcr;

            label_PinCountMax.Text = Globalo.yamlManager.configData.DrivingSettings.PinCountMax.ToString();
            label_CsvScanMax.Text = Globalo.yamlManager.configData.DrivingSettings.CsvScanMonth.ToString();



            
            label_Config_Tray_GapX_Val.Text = Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX.ToString("0.0##");
            label_Config_Tray_GapY_Val.Text = Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY.ToString("0.0##");
            label_Config_Socket_GapX_Val.Text = Globalo.motionManager.transferMachine.productLayout.SocketGap.GapX.ToString("0.0##");
            label_Config_Socket_GapY_Val.Text = Globalo.motionManager.transferMachine.productLayout.SocketGap.GapY.ToString("0.0##");
            label_Config_Ng_GapX_Val.Text = Globalo.motionManager.transferMachine.productLayout.NgGap.GapX.ToString("0.0##");
            label_Config_Ng_GapY_Val.Text = Globalo.motionManager.transferMachine.productLayout.NgGap.GapY.ToString("0.0##");
        }
        public void ShowComPort()
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
        }
        private void ShowLanguage()
        {
            string comData = Globalo.yamlManager.configData.DrivingSettings.Language;

            int index = ComboBox_Language.Items.IndexOf(comData);
            if (index < 0)
            {
                ComboBox_Language.SelectedIndex = 0;  // 첫 번째 항목 선택
            }
            else
            {
                ComboBox_Language.SelectedIndex = index;
            }

        }
        public void setInterface()
        {
            int i = 0;
            ManualTitleLabel.ForeColor = ColorTranslator.FromHtml("#6F6F6F");

            for (i = 0; i < 20; i++)
            {
                poisonComboBox_BcrPort.Items.Add("COM" + (i + 1).ToString());
            }

            ComboBox_Language.Items.Add("ko");
            ComboBox_Language.Items.Add("en");
            ComboBox_Language.Items.Add("es");
            ComboBox_Language.SelectedIndex = 0;
            //string selectedValue = poisonComboBox_BcrPort.SelectedItem.ToString();

        }
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            int lineStartY = ManualTitleLabel.Location.Y + Globalo.TabLineY;
            // Graphics 객체 가져오기
            Graphics g = e.Graphics;

            // Pen 객체 생성 (색상과 두께 설정)
            Color color = Color.FromArgb(175, 175, 175);//Color.FromArgb(151, 149, 145);
            Pen pen = new Pen(color, 1);

            // 라인 그리기 (시작점과 끝점 설정)
            g.DrawLine(pen, 0, lineStartY, this.Width, lineStartY);

            // 리소스 해제
            pen.Dispose();



           // Graphics g = this.CreateGraphics();
            // 지정된 펜츠로 폼에 사각형은 그립니다.
            //Pen pen1 = new Pen(Color.Red, 1);
            //Pen pen2 = new Pen(Color.Blue, 2);
            //Pen pen3 = new Pen(Color.Magenta, 10);

            //g.DrawLine(pen1, 10, 300, 100, 10);
            //g.DrawLine(pen2, new Point(10, 400), new Point(100, 400));
            //g.DrawLine(pen3, new Point(10, 500), new Point(150, 500));

            //pen1.Dispose();
            //pen2.Dispose();
            //pen3.Dispose();
        }
        
        private void ManualBtnChange(eManualBtn index)
        {

            //if (index == eManualBtn.pcbTab)
            //{
            //    BTN_MANUAL_PCB.BackColor = ColorTranslator.FromHtml("#FFB230");
            //    manualPcb.Visible = true;
            //    manualLens.Visible = false;
            //}
            //else
            //{
            //    BTN_MANUAL_LENS.BackColor = ColorTranslator.FromHtml("#FFB230");
            //    manualLens.Visible = true;
            //    manualPcb.Visible = false;
            //}
        }
        private void BTN_CONFIG_SAVE_Click(object sender, EventArgs e)
        {
            //Save

            GetConfigData();
            Globalo.yamlManager.configDataSave();
            Data.TaskDataYaml.TaskSave_Layout(Globalo.motionManager.transferMachine.productLayout, Machine.TransferMachine.LayoutPath);
            //Globalo.motionManager.transferMachine
            //
            RefreshConfig();


            //언어 변경
            string comData = Globalo.yamlManager.configData.DrivingSettings.Language;
            Program.SetLanguage(comData);   
        }

        private void button_Bcr_Connect_Click(object sender, EventArgs e)
        {
            bool connectRtn = Globalo.serialPortManager.Barcode.Open();

            string logData = "";

            if (connectRtn)
            {
                logData = $"[SERIAL] BCR CONNECT OK:{Globalo.yamlManager.configData.SerialPort.Bcr}";
            }
            else
            {
                logData = $"[SERIAL] BCR CONNECT FAIL:{Globalo.yamlManager.configData.SerialPort.Bcr}";
            }

            Globalo.LogPrint("Serial", logData);
        }

        private void button_Bcr_DisConnect_Click(object sender, EventArgs e)
        {
            Globalo.serialPortManager.Barcode.Close();

            string logData = $"[SERIAL] BCR DISCONNECT";

            Globalo.LogPrint("Serial", logData);
        }

        private void ConfigControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                RefreshConfig();
            }
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

        private void ProductSizeInput(Label OffsetLabel)
        {
            string labelValue = OffsetLabel.Text;
            decimal decimalValue = 0;


            if (decimal.TryParse(labelValue, out decimalValue))
            {
                // 소수점 형식으로 변환
                string formattedValue = decimalValue.ToString("0.0##");
                NumPadForm popupForm = new NumPadForm(formattedValue);

                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    double dNumData = Double.Parse(popupForm.NumPadResult);

                    OffsetLabel.Text = dNumData.ToString("0.#");
                }
            }
        }
        private void label_Config_Tray_GapX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }

        private void label_Config_Tray_GapY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }

        private void label_Config_Socket_GapX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }

        private void label_Config_Socket_GapY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }

        private void label_Config_Ng_GapX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }

        private void label_Config_Ng_GapY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }
    }
}
