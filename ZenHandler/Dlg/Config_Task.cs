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
        private Label[] LoadLabel;
        private Label[] UnloadLabel;

        public Config_Task()
        {
            InitializeComponent();

            LoadLabel = new Label[] { label_ConfigTask_Load_P1, label_ConfigTask_Load_P2, label_ConfigTask_Load_P3, label_ConfigTask_Load_P4 };
            UnloadLabel = new Label[] { label_ConfigTask_Unload_P1, label_ConfigTask_Unload_P2, label_ConfigTask_Unload_P3, label_ConfigTask_Unload_P4 };
        }

        public void ShowTaskData()
        {
            int i = 0;
            bool setBool = Globalo.yamlManager.configData.DrivingSettings.IdleReportPass;

            hopeCheckBox_PinCountUse.Checked = Globalo.yamlManager.configData.DrivingSettings.PinCountUse;
            hopeCheckBox_ImageGrabUse.Checked = Globalo.yamlManager.configData.DrivingSettings.ImageGrabUse;
            checkBox_IdleReportPass.Checked = Globalo.yamlManager.configData.DrivingSettings.IdleReportPass;
            checkBox_BcrGo.Checked = Globalo.yamlManager.configData.DrivingSettings.EnableAutoStartBcr;


            ShowTaskPicker();
        }
        public void ShowTaskPicker()
        {
            int i = 0;
            for (i = 0; i < 4; i++)
            {
                LoadLabel[i].Text = Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State.ToString();

                if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Bcr)
                {
                    LoadLabel[i].BackColor = Color.Yellow;
                }
                else
                {
                    LoadLabel[i].BackColor = Color.White;
                }


                UnloadLabel[i].Text = Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State.ToString();
                if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Good)
                {
                    UnloadLabel[i].BackColor = Color.Green;
                }
                else if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Blank)
                {
                    UnloadLabel[i].BackColor = Color.White;
                }
                else
                {
                    UnloadLabel[i].BackColor = Color.Red;
                }
            }
        }
        public void GetTaskData()
        {
            int i = 0;
            //운전 설정
            Globalo.yamlManager.configData.DrivingSettings.PinCountUse = hopeCheckBox_PinCountUse.Checked;
            Globalo.yamlManager.configData.DrivingSettings.ImageGrabUse = hopeCheckBox_ImageGrabUse.Checked;
            Globalo.yamlManager.configData.DrivingSettings.IdleReportPass = checkBox_IdleReportPass.Checked;
            Globalo.yamlManager.configData.DrivingSettings.EnableAutoStartBcr = checkBox_BcrGo.Checked;

            for (i = 0; i < 4; i++)
            {
                //  LOAD
                //
                if (Enum.TryParse(LoadLabel[i].Text, out Machine.PickedProductState LoadState))
                {
                    Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State = LoadState;
                }
                else
                {
                    // 예외 처리 또는 기본값 설정
                    Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State = Machine.PickedProductState.Blank;
                }
                //
                //  UN_LOAD
                //
                if (Enum.TryParse(UnloadLabel[i].Text, out Machine.PickedProductState UnloadState))
                {
                    Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State = UnloadState;
                }
                else
                {
                    // 예외 처리 또는 기본값 설정
                    Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State = Machine.PickedProductState.Blank;
                }
            }
        }
        public void showPanel()
        {
            this.Visible = true;
            if (ProgramState.ON_LINE_MOTOR == true)
            {
                //myTeachingGrid.MotorStateRun(true);
            }
            ShowTaskData();
            //myTeachingGrid.ShowTeachingData();
            //TeachResolution(Globalo.motionManager.transferMachine.teachingConfig.Resolution[SelectAxisIndex].ToString("0.#"));
        }
        public void hidePanel()
        {
            this.Visible = false;
            //myTeachingGrid.MotorStateRun(false);
        }
        private void SetLoadPicker(int index, Label label , bool UserSet = false)
        {
            if (UserSet)
            {
                if (Globalo.motionManager.transferMachine.RunState != OperationState.Stopped)
                {
                    //Transfer Unit 정지상태에서만 설정 가능합니다.
                    Globalo.LogPrint("ManualControl", "[INFO] TRANSFER UNIT 정지 상태에서 변경 가능합니다.", Globalo.eMessageName.M_WARNING);
                    return;
                }
            }
            

            if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[index].State == Machine.PickedProductState.Blank)
            {
                Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[index].State = Machine.PickedProductState.Bcr;
                label.BackColor = Color.Yellow;
            }
            else
            {
                Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[index].State = Machine.PickedProductState.Blank;
                label.BackColor = Color.White;
            }

            label.Text = Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[index].State.ToString();
        }

        private void SetUnloadPicker(int index, Label label, bool UserSet = false)
        {
            if (UserSet)
            {
                if (Globalo.motionManager.transferMachine.RunState != OperationState.Stopped)
                {
                    //Transfer Unit 정지상태에서만 설정 가능합니다.
                    Globalo.LogPrint("ManualControl", "[INFO] TRANSFER UNIT 정지 상태에서 변경 가능합니다.", Globalo.eMessageName.M_WARNING);
                    return;
                }
            }
            /*
             Blank = 0,   // 제품 없음    
        Bcr,
        Good,       // 양품
        BcrNg,      // 불량
        TestNg,
        Unknown     // 미확인 (필요 시) 
             */

            //Blank, Bcr, Good, BcrNg, TestNg, TestNg_2, TestNg_3, TestNg_4

            if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State == Machine.PickedProductState.Blank)
            {
                Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State = Machine.PickedProductState.Good;
                label.BackColor = Color.Yellow;
            }
            else
            {
                var values = (Machine.PickedProductState[])Enum.GetValues(typeof(Machine.PickedProductState));
                int maxCount = values.Length;
                int currentIndex = Array.IndexOf(values, Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State);

                if (currentIndex < maxCount - 1)
                {
                    label.BackColor = Color.Red;
                    currentIndex++;
                    Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State = values[currentIndex];
                }
                else
                {
                    label.BackColor = Color.White;
                    Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State = Machine.PickedProductState.Blank;
                }
                
            }
            label.Text = Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State.ToString();
        }
        //로드
        private void label_ConfigTask_Load_P1_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetLoadPicker(0, label , true);
        }

        private void label_ConfigTask_Load_P2_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetLoadPicker(1, label, true);
        }

        private void label_ConfigTask_Load_P3_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetLoadPicker(2, label, true);
        }

        private void label_ConfigTask_Load_P4_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetLoadPicker(3, label, true);
        }
        //
        //
        //배출
        private void label_ConfigTask_Unload_P1_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetUnloadPicker(0, label, true);
        }

        private void label_ConfigTask_Unload_P2_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetUnloadPicker(1, label, true);
        }

        private void label_ConfigTask_Unload_P3_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetUnloadPicker(2, label, true);
        }

        private void label_ConfigTask_Unload_P4_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetUnloadPicker(3, label, true);
        }

        private void Btn_ConfigTask_Save_Click(object sender, EventArgs e)
        {
            GetTaskData();
            Globalo.motionManager.transferMachine.TaskSave();

            Data.TaskDataYaml.TaskSave_Layout(Globalo.motionManager.transferMachine.productLayout, Machine.TransferMachine.LayoutPath);
        }
    }
}
