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
    public partial class Config_Task : UserControl
    {
        private Label[] LoadLabel;
        private Label[] UnloadLabel;
        private Controls.DefaultGridView DelayGridView;
        private int GridCol = 2;                                //name , time
        private int[] StartPos = new int[] { 379, 50 };        //Grid Pos
        private int[] inGridWid = new int[] { 226, 70 };        //Grid Col Width


        public List<Machine.ProductInfo> tempLoadInfo { get; set; } = new List<Machine.ProductInfo>();
        public List<Machine.ProductInfo> tempUnloadInfo { get; set; } = new List<Machine.ProductInfo>();
        public Config_Task()
        {
            InitializeComponent();

            LoadLabel = new Label[] { label_ConfigTask_Load_P1, label_ConfigTask_Load_P2, label_ConfigTask_Load_P3, label_ConfigTask_Load_P4 };
            UnloadLabel = new Label[] { label_ConfigTask_Unload_P1, label_ConfigTask_Unload_P2, label_ConfigTask_Unload_P3, label_ConfigTask_Unload_P4 };

            InitDelayGrid();
        }
        public void InitDelayGrid()
        {
            int i = 0;
            int count = Globalo.yamlManager.taskDataYaml.TaskData.delayData.Count;
            DelayGridView = new Controls.DefaultGridView(GridCol, count, inGridWid);
            DelayGridView.Location = new Point(label_ConfigTask_Delay.Location.X, label_ConfigTask_Delay.Location.Y + label_ConfigTask_Delay.Height + 1);
            this.Controls.Add(DelayGridView);

            string[] title = new string[] { "name", "time(s)" };         //Grid Width
            for (i = 0; i < DelayGridView.ColumnCount; i++)
            {
                DelayGridView.Columns[i].Name = title[i];
            }

            string posName = "";
            for (i = 0; i < count; i++)
            {
                posName = Globalo.yamlManager.taskDataYaml.TaskData.delayData[i].Name;
                //DelayGridView.Rows[i].SetValues(posName);
                DelayGridView[0, i].Value = posName;
                posName = Globalo.yamlManager.taskDataYaml.TaskData.delayData[i].Delay.ToString("0.0#");
                DelayGridView[1, i].Value = posName;
            }


            DelayGridView.CellClick += DelayGrid_CellClick;
        }
        private void DelayGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int nRow = e.RowIndex;      //세로줄 티칭위치
            int nCol = e.ColumnIndex;   //가로줄 모터

            if (nRow >= 0 && nCol >= 1)//(nRow > RowLimit && nRow < nGridRowCount - 1) && )
            {
                string cellStr = DelayGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                decimal decimalValue = 0;
                if (decimal.TryParse(cellStr, out decimalValue))
                {
                    string formattedValue = decimalValue.ToString("0.0#");
                    NumPadForm popupForm = new NumPadForm(formattedValue);

                    if (popupForm.ShowDialog() == DialogResult.OK)
                    {
                        double dNumData = 0.0;// double.Parse(popupForm.NumPadResult);
                        if (!string.IsNullOrWhiteSpace(popupForm.NumPadResult) && double.TryParse(popupForm.NumPadResult, out dNumData))
                        {
                            // dNumData 값이 성공적으로 변환됨
                            if (dNumData < 0.0)
                            {
                                dNumData = 0.0;
                            }
                            if (dNumData > 600.0)
                            {
                                dNumData = 600.0;
                            }
                        }
                        else
                        {
                            // 변환 실패: 기본값 사용 또는 사용자에게 알림
                            dNumData = 0.0; // 기본값
                        }
                        

                        DelayGridView[e.ColumnIndex, e.RowIndex].Value = dNumData.ToString("0.0#");
                    }
                }
            }
        }
        public void ShowTaskData()
        {
            int i = 0;

            hopeCheckBox_PinCountUse.Checked = Globalo.yamlManager.configData.DrivingSettings.PinCountUse;
            Btn_ConfigTask_Driving_Mode.Text = Globalo.yamlManager.configData.DrivingSettings.drivingMode.ToString();
            tempLoadInfo.Clear();
            tempUnloadInfo.Clear();
            tempLoadInfo.Add(Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[0].Clone());
            tempLoadInfo.Add(Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[1].Clone());
            tempLoadInfo.Add(Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[2].Clone());
            tempLoadInfo.Add(Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[3].Clone());

            tempUnloadInfo.Add(Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[0].Clone());
            tempUnloadInfo.Add(Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[1].Clone());
            tempUnloadInfo.Add(Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[2].Clone());
            tempUnloadInfo.Add(Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[3].Clone());




            ShowTaskPicker();
            ShowTrayPos();
            ShowMagazineLayer();
        }
        public void ShowMagazineLayer()
        {
            label_ConfigTask_Left_Magazine_Layer1.Text = Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[0].State.ToString();
            label_ConfigTask_Left_Magazine_Layer2.Text = Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[1].State.ToString();
            label_ConfigTask_Left_Magazine_Layer3.Text = Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[2].State.ToString();
            label_ConfigTask_Left_Magazine_Layer4.Text = Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[3].State.ToString();
            label_ConfigTask_Left_Magazine_Layer5.Text = Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[4].State.ToString();

            label_ConfigTask_Right_Magazine_Layer1.Text = Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[0].State.ToString();
            label_ConfigTask_Right_Magazine_Layer2.Text = Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[1].State.ToString();
            label_ConfigTask_Right_Magazine_Layer3.Text = Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[2].State.ToString();
            label_ConfigTask_Right_Magazine_Layer4.Text = Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[3].State.ToString();
            label_ConfigTask_Right_Magazine_Layer5.Text = Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[4].State.ToString();



        }
        public void ShowTrayPos()
        {
            label_ConfigTask_Load_Tray_X.Text = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.X.ToString();
            label_ConfigTask_Unload_Tray_X.Text = Globalo.motionManager.transferMachine.pickedProduct.UnloadTrayPos.X.ToString();
            label_ConfigTask_NgTray_X.Text = Globalo.motionManager.transferMachine.pickedProduct.NgTrayPos.X.ToString();


            label_ConfigTask_Load_Tray_Y.Text = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.Y.ToString();
            label_ConfigTask_Unload_Tray_Y.Text = Globalo.motionManager.transferMachine.pickedProduct.UnloadTrayPos.Y.ToString();
            label_ConfigTask_NgTray_Y.Text = Globalo.motionManager.transferMachine.pickedProduct.NgTrayPos.Y.ToString();

            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                label_ConfigTask_Left_Tray_Layer_Val.Text = Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer.ToString();
                label_ConfigTask_Right_Tray_Layer_Val.Text = Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer.ToString();
            }
            else
            {
                label_ConfigTask_Left_Tray_Layer_Val.Text = Globalo.motionManager.liftMachine.trayProduct.LeftTrayLayer.ToString();
                label_ConfigTask_Right_Tray_Layer_Val.Text = Globalo.motionManager.liftMachine.trayProduct.RightTrayLayer.ToString();
            }
                

        }
        public void ShowTaskPicker()
        {
            int i = 0;
            for (i = 0; i < 4; i++)
            {
                //LoadLabel[i].Text = Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State.ToString();
                LoadLabel[i].Text = tempLoadInfo[i].State.ToString();

               // if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Bcr)
                if (tempLoadInfo[i].State == Machine.PickedProductState.Bcr)
                {
                    LoadLabel[i].BackColor = Color.Yellow;
                }
                else
                {
                    LoadLabel[i].BackColor = Color.White;
                }


                //UnloadLabel[i].Text = Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State.ToString();
                UnloadLabel[i].Text = tempUnloadInfo[i].State.ToString();
                //if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Good)
                if (tempUnloadInfo[i].State == Machine.PickedProductState.Good)
                {
                    UnloadLabel[i].BackColor = Color.Green;
                }
                //else if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Blank)
                else if (tempUnloadInfo[i].State == Machine.PickedProductState.Blank)
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

            Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.X = int.Parse(label_ConfigTask_Load_Tray_X.Text);
            Globalo.motionManager.transferMachine.pickedProduct.UnloadTrayPos.X = int.Parse(label_ConfigTask_Unload_Tray_X.Text);
            Globalo.motionManager.transferMachine.pickedProduct.NgTrayPos.X = int.Parse(label_ConfigTask_NgTray_X.Text);

            Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.Y = int.Parse(label_ConfigTask_Load_Tray_Y.Text);
            Globalo.motionManager.transferMachine.pickedProduct.UnloadTrayPos.Y = int.Parse(label_ConfigTask_Unload_Tray_Y.Text);
            Globalo.motionManager.transferMachine.pickedProduct.NgTrayPos.Y = int.Parse(label_ConfigTask_NgTray_Y.Text);


            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                Globalo.motionManager.magazineHandler.magazineTray.LeftTrayLayer = int.Parse(label_ConfigTask_Left_Tray_Layer_Val.Text);
                Globalo.motionManager.magazineHandler.magazineTray.RightTrayLayer = int.Parse(label_ConfigTask_Right_Tray_Layer_Val.Text);

                Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[0].State = (Machine.LayerState)Enum.Parse(typeof(Machine.LayerState), label_ConfigTask_Left_Magazine_Layer1.Text);
                Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[1].State = (Machine.LayerState)Enum.Parse(typeof(Machine.LayerState), label_ConfigTask_Left_Magazine_Layer2.Text);
                Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[2].State = (Machine.LayerState)Enum.Parse(typeof(Machine.LayerState), label_ConfigTask_Left_Magazine_Layer3.Text);
                Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[3].State = (Machine.LayerState)Enum.Parse(typeof(Machine.LayerState), label_ConfigTask_Left_Magazine_Layer4.Text);
                Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[4].State = (Machine.LayerState)Enum.Parse(typeof(Machine.LayerState), label_ConfigTask_Left_Magazine_Layer5.Text);

                Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[0].State = (Machine.LayerState)Enum.Parse(typeof(Machine.LayerState), label_ConfigTask_Right_Magazine_Layer1.Text);
                Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[1].State = (Machine.LayerState)Enum.Parse(typeof(Machine.LayerState), label_ConfigTask_Right_Magazine_Layer2.Text);
                Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[2].State = (Machine.LayerState)Enum.Parse(typeof(Machine.LayerState), label_ConfigTask_Right_Magazine_Layer3.Text);
                Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[3].State = (Machine.LayerState)Enum.Parse(typeof(Machine.LayerState), label_ConfigTask_Right_Magazine_Layer4.Text);
                Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[4].State = (Machine.LayerState)Enum.Parse(typeof(Machine.LayerState), label_ConfigTask_Right_Magazine_Layer5.Text);
            }
            else
            {
                Globalo.motionManager.liftMachine.trayProduct.LeftTrayLayer = int.Parse(label_ConfigTask_Left_Tray_Layer_Val.Text);
                Globalo.motionManager.liftMachine.trayProduct.RightTrayLayer = int.Parse(label_ConfigTask_Right_Tray_Layer_Val.Text);
            }



            string delaydata = "";
            for (i = 0; i < DelayGridView.RowCount; i++)
            {
                Globalo.yamlManager.taskDataYaml.TaskData.delayData[i].Delay = double.Parse(DelayGridView[1, i].Value.ToString());

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
        private void SetMagazineLayer(int Pos, int index, Label label, bool UserSet = false)
        {
            if (Globalo.motionManager.magazineHandler.RunState != OperationState.Stopped)
            {
                //Transfer Unit 정지상태에서만 설정 가능합니다.
                Globalo.LogPrint("ManualControl", "[INFO] MAGAZINE UNIT 정지 상태에서 변경 가능합니다.", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Pos == 0)
            {
                //Left Magazine
                if (Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[index].State == Machine.LayerState.After)
                {
                    Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[index].State = Machine.LayerState.Before;
                    label.BackColor = Color.White;
                }
                else
                {
                    Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[index].State = Machine.LayerState.After;
                    label.BackColor = Color.Green;
                    //Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[index].State = (Machine.LayerState)((int)Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[index].State + 1);
                }
                label.Text = Globalo.motionManager.magazineHandler.magazineTray.LeftMagazineInfo[index].State.ToString();
            }
            else
            {
                //Right Magazine
                if (Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[index].State == Machine.LayerState.After)
                {
                    Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[index].State = Machine.LayerState.Before;
                    label.BackColor = Color.White;
                }
                else
                {
                    //Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[index].State = (Machine.LayerState)((int)Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[index].State + 1);
                    Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[index].State = Machine.LayerState.After;
                    label.BackColor = Color.Green;
                }
                label.Text = Globalo.motionManager.magazineHandler.magazineTray.RightMagazineInfo[index].State.ToString();
            }
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
            //if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[index].State == Machine.PickedProductState.Blank)
            if (tempLoadInfo[index].State == Machine.PickedProductState.Blank)
            {
                //Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[index].State = Machine.PickedProductState.Bcr;
                tempLoadInfo[index].State = Machine.PickedProductState.Bcr;
                label.BackColor = Color.Yellow;
            }
            else
            {
                //Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[index].State = Machine.PickedProductState.Blank;
                tempLoadInfo[index].State = Machine.PickedProductState.Blank;
                label.BackColor = Color.White;
            }

            //label.Text = Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[index].State.ToString();
            label.Text = tempLoadInfo[index].State.ToString();
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
            //Machine.PickedProductState tempState = Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State;



            //if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State == Machine.PickedProductState.Blank)
            if (tempUnloadInfo[index].State == Machine.PickedProductState.Blank)
            {
                //Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State = Machine.PickedProductState.Good;
                tempUnloadInfo[index].State = Machine.PickedProductState.Good;
                label.BackColor = Color.Yellow;
            }
            else
            {
                var values = (Machine.PickedProductState[])Enum.GetValues(typeof(Machine.PickedProductState));
                int maxCount = values.Length;
                //int currentIndex = Array.IndexOf(values, Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State);
                int currentIndex = Array.IndexOf(values, tempUnloadInfo[index].State);

                if (currentIndex < maxCount - 1)
                {
                    label.BackColor = Color.Red;
                    currentIndex++;
                    //Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State = values[currentIndex];
                    tempUnloadInfo[index].State = values[currentIndex];
                }
                else
                {
                    label.BackColor = Color.White;
                    // Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State = Machine.PickedProductState.Blank;
                    tempUnloadInfo[index].State = Machine.PickedProductState.Blank;
                }
                
            }
            //label.Text = Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[index].State.ToString();
            label.Text = tempUnloadInfo[index].State.ToString();
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
            Globalo.yamlManager.taskDataYaml.TaskDataSave();

            Globalo.pickerInfo.SetLoadPickerInfo();
            Globalo.pickerInfo.SetUnloadPickerInfo();


            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                Globalo.motionManager.magazineHandler.TaskSave();
            }
            else if (Program.PG_SELECT == HANDLER_PG.AOI)
            {
                Globalo.motionManager.liftMachine.TaskSave();
            }
            else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
            {
                Globalo.motionManager.liftMachine.TaskSave();
            }
        }

        private void Btn_ConfigTask_Driving_Mode_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState != OperationState.Stopped ||
                Globalo.motionManager.socketAoiMachine.RunState != OperationState.Stopped ||
                Globalo.motionManager.magazineHandler.RunState != OperationState.Stopped ||
                Globalo.motionManager.liftMachine.RunState != OperationState.Stopped)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 전체 유닛 정지상태에서 변경 가능합니다.", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.yamlManager.configData.DrivingSettings.drivingMode == eDrivingMode.NOMAL)
            {
                Globalo.yamlManager.configData.DrivingSettings.drivingMode = eDrivingMode.DRY_RUN;
            }
            else
            {
                Globalo.yamlManager.configData.DrivingSettings.drivingMode = eDrivingMode.NOMAL;
            }

            Btn_ConfigTask_Driving_Mode.Text = Globalo.yamlManager.configData.DrivingSettings.drivingMode.ToString();
        }

        private void SetTrayPosition(int index, Label label, bool UserSet = false)
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


            string labelValue = label.Text;
            int decimalValue = 0;


            if (int.TryParse(labelValue, out decimalValue))
            {
                string formattedValue = decimalValue.ToString();
                NumPadForm popupForm = new NumPadForm(formattedValue);
                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    int dNumData = int.Parse(popupForm.NumPadResult);
                    if (index == 0)
                    {
                        //Tray x Max Count Check
                        if (dNumData > Globalo.motionManager.transferMachine.productLayout.TotalTrayPos.X - 1)
                        {
                            dNumData = Globalo.motionManager.transferMachine.productLayout.TotalTrayPos.X - 1;
                        }
                    }
                    else
                    {
                        //Tray y Max Count Check
                        if (dNumData > Globalo.motionManager.transferMachine.productLayout.TotalTrayPos.Y - 1)
                        {
                            dNumData = Globalo.motionManager.transferMachine.productLayout.TotalTrayPos.Y - 1;
                        }
                    }
                    label.Text = dNumData.ToString();
                }
            }
        }
        private void SetNgTrayPosition(int index, Label label, bool UserSet = false)
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
            string labelValue = label.Text;
            int decimalValue = 0;


            if (int.TryParse(labelValue, out decimalValue))
            {
                string formattedValue = decimalValue.ToString();
                NumPadForm popupForm = new NumPadForm(formattedValue);
                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    int dNumData = int.Parse(popupForm.NumPadResult);
                    if (index == 0)
                    {
                        //Ng Tray x Max Count Check
                        if (dNumData > Globalo.motionManager.transferMachine.productLayout.TotalNgTrayPos.X - 1)
                        {
                            dNumData = Globalo.motionManager.transferMachine.productLayout.TotalNgTrayPos.X - 1;
                        }
                    }
                    else
                    {
                        //Ng Tray y Max Count Check
                        if (dNumData > Globalo.motionManager.transferMachine.productLayout.TotalNgTrayPos.Y - 1)
                        {
                            dNumData = Globalo.motionManager.transferMachine.productLayout.TotalNgTrayPos.Y - 1;
                        }
                    }
                    label.Text = dNumData.ToString();
                }
            }
        }
        private void SetTrayLayer(Label label, bool UserSet = false)
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

            string labelValue = label.Text;
            int decimalValue = 0;


            if (int.TryParse(labelValue, out decimalValue))
            {
                string formattedValue = decimalValue.ToString();
                NumPadForm popupForm = new NumPadForm(formattedValue);
                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    int dNumData = 0;// int.Parse(popupForm.NumPadResult);
                    if (!string.IsNullOrWhiteSpace(popupForm.NumPadResult) && int.TryParse(popupForm.NumPadResult, out dNumData))
                    {
                        if (dNumData < -1) 
                        {
                            dNumData = -1;
                        }
                        if (dNumData > Globalo.motionManager.transferMachine.productLayout.TotalTrayLayer - 1)
                        {
                            dNumData = Globalo.motionManager.transferMachine.productLayout.TotalTrayLayer - 1;
                        }
                    }
                    else
                    {
                        dNumData = 0;
                    }
                    
                    label.Text = dNumData.ToString();
                }
            }
        }
        private void label_ConfigTask_Load_Tray_X_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetTrayPosition(0, label, true);
        }
        private void label_ConfigTask_Unload_Tray_X_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetTrayPosition(0, label, true);
        }

        private void label_ConfigTask_Load_Tray_Y_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetTrayPosition(1, label, true);
        }

        private void label_ConfigTask_Unload_Tray_Y_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetTrayPosition(1, label, true);
        }

        private void label_ConfigTask_NgTray_X_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetNgTrayPosition(0, label, true);
        }

        private void label_ConfigTask_NgTray_Y_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetNgTrayPosition(1, label, true);
        }

        private void label_ConfigTask_Left_Tray_Layer_Val_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetTrayLayer(label, true);
        }

        private void label_ConfigTask_Right_Tray_Layer_Val_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            SetTrayLayer(label, true);
        }

        private void label_ConfigTask_Left_Magazine_Layer1_Click(object sender, EventArgs e)
        {
            //Left Magazine1
            Label label = sender as Label;
            SetMagazineLayer(0, 0, label);
        }

        private void label_ConfigTask_Left_Magazine_Layer2_Click(object sender, EventArgs e)
        {
            //Left Magazine2
            Label label = sender as Label;
            SetMagazineLayer(0, 1, label);
        }

        private void label_ConfigTask_Left_Magazine_Layer3_Click(object sender, EventArgs e)
        {
            //Left Magazine3
            Label label = sender as Label;
            SetMagazineLayer(0, 2, label);
        }

        private void label_ConfigTask_Left_Magazine_Layer4_Click(object sender, EventArgs e)
        {
            //Left Magazine4
            Label label = sender as Label;
            SetMagazineLayer(0, 3, label);
        }

        private void label_ConfigTask_Left_Magazine_Layer5_Click(object sender, EventArgs e)
        {
            //Left Magazine5
            Label label = sender as Label;
            SetMagazineLayer(0, 4, label);
        }

        private void label_ConfigTask_Right_Magazine_Layer1_Click(object sender, EventArgs e)
        {
            //Right Magazine1
            Label label = sender as Label;
            SetMagazineLayer(1, 0, label);
        }

        private void label_ConfigTask_Right_Magazine_Layer2_Click(object sender, EventArgs e)
        {
            //Right Magazine2
            Label label = sender as Label;
            SetMagazineLayer(1, 1, label);
        }

        private void label_ConfigTask_Right_Magazine_Layer3_Click(object sender, EventArgs e)
        {
            //Right Magazine3
            Label label = sender as Label;
            SetMagazineLayer(1, 2, label);
        }

        private void label_ConfigTask_Right_Magazine_Layer4_Click(object sender, EventArgs e)
        {
            //Right Magazine4
            Label label = sender as Label;
            SetMagazineLayer(1, 3, label);
        }

        private void label_ConfigTask_Right_Magazine_Layer5_Click(object sender, EventArgs e)
        {
            //Right Magazine5
            Label label = sender as Label;
            SetMagazineLayer(1, 4, label);
        }
    }
}
