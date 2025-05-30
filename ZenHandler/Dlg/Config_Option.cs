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
                comboBox_BcrPort.Items.Add("COM" + (i + 1).ToString());
            }

            ComboBox_Language.Items.Add("ko");
            ComboBox_Language.Items.Add("en");
            ComboBox_Language.Items.Add("es");
            ComboBox_Language.SelectedIndex = 0;

            maskedTextBox_BcrIp.Mask = "192\\.168\\.255\\.255"; // 0~999 까지
            maskedTextBox_BcrIp.ValidatingType = typeof(System.Net.IPAddress);
            maskedTextBox_BcrIp.PromptChar = '_';
            maskedTextBox_BcrIp.Width = 150;
            maskedTextBox_BcrIp.Text = "192.168.100.1";

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

        private void button_Bcr_Connect_Click(object sender, EventArgs e)
        {
            //Globalo.serialPortManager.Barcode.Close();
            Globalo.tcpManager.BcrClient.Connect("192.168.0.15", 9004);
            string logData = $"[SERIAL] BCR DISCONNECT";

            Globalo.LogPrint("Serial", logData);

        }

        private void button_Bcr_DisConnect_Click(object sender, EventArgs e)
        {

            //bool connectRtn = Globalo.serialPortManager.Barcode.Open();
            bool connectRtn = Globalo.tcpManager.BcrClient.Disconnect();

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

                    OffsetLabel.Text = dNumData.ToString("0.0##");
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
        public void ShowOptionData()
        {
            string comData = Globalo.yamlManager.configData.SerialPort.Bcr;
            
            int index = comboBox_BcrPort.Items.IndexOf(comData);
            if (index < 0)
            {
                comboBox_BcrPort.SelectedIndex = 0;  // 첫 번째 항목 선택
            }
            else
            {
                comboBox_BcrPort.SelectedIndex = index;
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

            label_Config_Tray_GapX_Val.Text = Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX.ToString("0.0##");
            label_Config_Tray_GapY_Val.Text = Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY.ToString("0.0##");
            label_Config_Socket_GapX_Val.Text = Globalo.motionManager.transferMachine.productLayout.SocketGap.GapX.ToString("0.0##");
            label_Config_Socket_GapY_Val.Text = Globalo.motionManager.transferMachine.productLayout.SocketGap.GapY.ToString("0.0##");
            label_Config_Ng_GapX_Val.Text = Globalo.motionManager.transferMachine.productLayout.NgGap.GapX.ToString("0.0##");
            label_Config_Ng_GapY_Val.Text = Globalo.motionManager.transferMachine.productLayout.NgGap.GapY.ToString("0.0##");


            label_Config_Tray_Max_Count_X.Text = Globalo.motionManager.transferMachine.productLayout.TotalTrayPos.X.ToString();
            label_Config_Tray_Max_Count_Y.Text = Globalo.motionManager.transferMachine.productLayout.TotalTrayPos.Y.ToString();
            label_Config_Ngtray_Max_Count_X.Text = Globalo.motionManager.transferMachine.productLayout.TotalNgTrayPos.X.ToString();
            label_Config_Ngtray_Max_Count_Y.Text = Globalo.motionManager.transferMachine.productLayout.TotalNgTrayPos.Y.ToString();

            label_Config_Tray_Max_Layer_Val.Text = Globalo.motionManager.transferMachine.productLayout.TotalTrayLayer.ToString();

        }
        public void GetOptionData()
        {
            //Serial Port
            Globalo.yamlManager.configData.SerialPort.Bcr = comboBox_BcrPort.Text;
            Globalo.yamlManager.configData.DrivingSettings.Language = ComboBox_Language.Text;

            Globalo.yamlManager.configData.DrivingSettings.PinCountMax = int.Parse(label_PinCountMax.Text);

            //제품 간격 - Tray , Socket
            Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX = double.Parse(label_Config_Tray_GapX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY = double.Parse(label_Config_Tray_GapY_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.SocketGap.GapX = double.Parse(label_Config_Socket_GapX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.SocketGap.GapY = double.Parse(label_Config_Socket_GapY_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.NgGap.GapX = double.Parse(label_Config_Ng_GapX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.NgGap.GapY = double.Parse(label_Config_Ng_GapY_Val.Text);

            //Tray 최대 로드 개수
            Globalo.motionManager.transferMachine.productLayout.TotalTrayPos.X = int.Parse(label_Config_Tray_Max_Count_X.Text);
            Globalo.motionManager.transferMachine.productLayout.TotalTrayPos.Y = int.Parse(label_Config_Tray_Max_Count_Y.Text);

            Globalo.motionManager.transferMachine.productLayout.TotalNgTrayPos.X = int.Parse(label_Config_Ngtray_Max_Count_X.Text);
            Globalo.motionManager.transferMachine.productLayout.TotalNgTrayPos.Y = int.Parse(label_Config_Ngtray_Max_Count_Y.Text);

            Globalo.motionManager.transferMachine.productLayout.TotalTrayLayer = int.Parse(label_Config_Tray_Max_Layer_Val.Text);

        }
        public void showPanel()
        {
            this.Visible = true;
            if (ProgramState.ON_LINE_MOTOR == true)
            {
                //myTeachingGrid.MotorStateRun(true);
            }
            ShowOptionData();
            //myTeachingGrid.ShowTeachingData();
            //TeachResolution(Globalo.motionManager.transferMachine.teachingConfig.Resolution[SelectAxisIndex].ToString("0.#"));
        }
        public void hidePanel()
        {
            this.Visible = false;
            //myTeachingGrid.MotorStateRun(false);
        }

        private void Btn_ConfigOption_Save_Click(object sender, EventArgs e)
        {
            GetOptionData();

            Globalo.yamlManager.configDataSave();
            Data.TaskDataYaml.TaskSave_Layout(Globalo.motionManager.transferMachine.productLayout, Machine.TransferMachine.LayoutPath);

            //언어 변경
            string comData = Globalo.yamlManager.configData.DrivingSettings.Language;
            Program.SetLanguage(comData);

            Globalo.trayStateInfo.InitTableLayoutSet();
        }
        private void ProductMaxCountInput(Label label)
        {
            string labelValue = label.Text;
            int decimalValue = 0;


            if (int.TryParse(labelValue, out decimalValue))
            {
                // 소수점 형식으로 변환
                string formattedValue = decimalValue.ToString();
                NumPadForm popupForm = new NumPadForm(formattedValue);

                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    int dNumData = int.Parse(popupForm.NumPadResult);
                    if (dNumData < 1)
                    {
                        dNumData = 1;
                    }
                    label.Text = dNumData.ToString();
                }
            }
        }
        private void label_Config_Tray_Max_Count_X_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductMaxCountInput(clickedLabel);
        }

        private void label_Config_Tray_Max_Count_Y_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductMaxCountInput(clickedLabel);
        }

        private void label_Config_Ngtray_Max_Count_X_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductMaxCountInput(clickedLabel);
        }

        private void label_Config_Ngtray_Max_Count_Y_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductMaxCountInput(clickedLabel);
        }

        private void label_Config_Left_Tray_Max_Layer_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductMaxCountInput(clickedLabel);
        }

        private void label_Config_Right_Tray_Max_Layer_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductMaxCountInput(clickedLabel);
        }
    }
}
