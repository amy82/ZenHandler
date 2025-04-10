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
    public partial class ProductionInfo : UserControl
    {
        public ProductionInfo()
        {
            InitializeComponent();
        }
        public void ProcessSet(string value)
        {
            textBox_ProcessState.Text = value;
        }
        public void BcrSet(string value)
        {
            if (textBox_TopLot.InvokeRequired)
            {
                textBox_TopLot.Invoke(new Action(() => textBox_TopLot.Text = value));
            }
            else
            {
                textBox_TopLot.Text = value;
            }   
            Globalo.dataManage.TaskWork.m_szChipID = value;
            Globalo.yamlManager.TaskData.LotData.BarcodeData = Globalo.dataManage.TaskWork.m_szChipID;
            Globalo.yamlManager.TaskDataSave();

            //string sanitizedFileName = Data.CEEpromData.SanitizeFileName(value);
            //Console.WriteLine($"✅ 사용 가능한 파일명: {sanitizedFileName}");
        }
        public void PinCountInfoSet()
        {
            //Globalo.yamlManager.TaskData.PintCount > Globalo.yamlManager.configData.DrivingSettings.PinCountMax
            string str = $"{Globalo.yamlManager.TaskData.PintCount} / {Globalo.yamlManager.configData.DrivingSettings.PinCountMax}";
            label_PinCount.Text = str;
        }
        public void ProductionInfoSet()
        {
            label_production_ok.Text = Globalo.yamlManager.TaskData.ProductionInfo.OkCount.ToString();
            label_production_ng.Text = Globalo.yamlManager.TaskData.ProductionInfo.NgCount.ToString();
            label_production_total.Text = Globalo.yamlManager.TaskData.ProductionInfo.TotalCount.ToString();
            //
            //label_production_ok.Text = Globalo.dataManage.TaskWork.Judge_Total_Count.ToString();
            //label_production_ng.Text = Globalo.dataManage.TaskWork.Judge_Ok_Count.ToString();
            //label_production_total.Text = Globalo.dataManage.TaskWork.Judge_Ng_Count.ToString();
        }

        private void BTN_MAIN_JUDGE_RESET_Click(object sender, EventArgs e)
        {
            //label_production_ok
            //label_production_ng
            //label_production_total

            string logStr = "생산량 초기화 하시겠습니까 ?";

            MessagePopUpForm messagePopUp = new MessagePopUpForm("", "YES", "NO");
            messagePopUp.MessageSet(Globalo.eMessageName.M_ASK, logStr);
            DialogResult result = messagePopUp.ShowDialog();

            if (result == DialogResult.Yes)
            {
                Globalo.dataManage.TaskWork.Judge_Total_Count = 0;
                Globalo.dataManage.TaskWork.Judge_Ok_Count = 0;
                Globalo.dataManage.TaskWork.Judge_Ng_Count = 0;

                Globalo.yamlManager.TaskData.ProductionInfo.TotalCount = 0;
                Globalo.yamlManager.TaskData.ProductionInfo.OkCount = 0;
                Globalo.yamlManager.TaskData.ProductionInfo.NgCount = 0;
                Globalo.yamlManager.TaskDataSave();

                Globalo.productionInfo.ProductionInfoSet();
            }
        }

        private void BTN_MAIN_PINCOUNT_RESET_Click(object sender, EventArgs e)
        {
            Globalo.yamlManager.TaskData.PintCount = 0;
            Globalo.yamlManager.TaskDataSave();

            PinCountInfoSet();
        }
    }
}
