﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Dlg
{
    public partial class TeachingTransfer : UserControl
    {
        private readonly SynchronizationContext _syncContext;
        private Controls.TeachingGridView myTeachingGrid;

        private Button[] TeachBtnArr;
        private string ColorDefaultBtn = "#C3A279";
        private string ColorSelecttBtn = "#4C4743";

        protected CancellationTokenSource cts;
        public int SelectAxisIndex = 0;        //선택 모터 순서
        private ToolTip TeachingTransferTooltip;
        //
        public TeachingTransfer()
        {
            InitializeComponent();
            _syncContext = SynchronizationContext.Current;
            cts = new CancellationTokenSource();

            int[] inGridWid = new int[] { 150, 80, 80, 80};         //Grid Width

            myTeachingGrid = new Controls.TeachingGridView( Globalo.motionManager.transferMachine.MotorAxes, Globalo.motionManager.transferMachine.teachingConfig, inGridWid, 26);

            myTeachingGrid.Location = new System.Drawing.Point(150, 10);
            this.groupTeachPcb.Controls.Add(myTeachingGrid);

            TeachTransferUiSet();
            
            changeBtnMotorNo(SelectAxisIndex);

            //TeachResolution(Globalo.motionManager.transferMachine.teachingConfig.Resolution[SelectAxisIndex].ToString("0.#"));

            TeachingTransferTooltip = new ToolTip();

        }
        public void TeachResolution(string val)
        {

            label_Resolution.Visible = true;
            LABEL_TEACH_ROSOLUTION_VALUE.Visible = true;
            LABEL_TEACH_ROSOLUTION_VALUE.Text = val;

        }
        public void TeachTransferUiSet()
        {
            int i = 0;
            TeachBtnArr = new Button[] { BTN_TEACH_TRANSFER_X, BTN_TEACH_TRANSFER_Y, BTN_TEACH_TRANSFER_Z };

            for (i = 0; i < TeachBtnArr.Length; i++)
            {
                TeachBtnArr[i].Text = Globalo.motionManager.transferMachine.MotorAxes[i].Name;
                TeachBtnArr[i].BackColor = ColorTranslator.FromHtml(ColorDefaultBtn);
                TeachBtnArr[i].ForeColor = Color.White;
            }

            BTN_TEACH_SERVO_ON.BackColor = ColorTranslator.FromHtml(ColorDefaultBtn);
            BTN_TEACH_SERVO_ON.ForeColor = Color.White;
            BTN_TEACH_SERVO_OFF.BackColor = ColorTranslator.FromHtml(ColorDefaultBtn);
            BTN_TEACH_SERVO_OFF.ForeColor = Color.White;
            BTN_TEACH_SERVO_RESET.BackColor = ColorTranslator.FromHtml(ColorDefaultBtn);
            BTN_TEACH_SERVO_RESET.ForeColor = Color.White;



            comboBox_Teach_LoadPicker.Items.Add("Load Picker #1");
            comboBox_Teach_LoadPicker.Items.Add("Load Picker #2");
            comboBox_Teach_LoadPicker.Items.Add("Load Picker #3");
            comboBox_Teach_LoadPicker.Items.Add("Load Picker #4");

            comboBox_Teach_UnloadPicker.Items.Add("UnLoad Picker #1");
            comboBox_Teach_UnloadPicker.Items.Add("UnLoad Picker #2");
            comboBox_Teach_UnloadPicker.Items.Add("UnLoad Picker #3");
            comboBox_Teach_UnloadPicker.Items.Add("UnLoad Picker #4");

            
            comboBox_Teach_LoadPicker.SelectedIndex = 0;  // 첫 번째 항목 선택
            comboBox_Teach_UnloadPicker.SelectedIndex = 0;  // 첫 번째 항목 선택
        }

        public void showPanel()
        {
            this.Visible = true;
            if (ProgramState.ON_LINE_MOTOR == true)
            {
                myTeachingGrid.MotorStateRun(true);
            }
            myTeachingGrid.ShowTeachingData();
            changeBtnMotorNo(SelectAxisIndex);
            //TeachResolution(Globalo.motionManager.transferMachine.teachingConfig.Resolution[SelectAxisIndex].ToString("0.#"));
        }
        public void hidePanel()
        {
            this.Visible = false;
            myTeachingGrid.MotorStateRun(false);
        }
        private void comboBox_Teach_Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox_Teach_LoadPicker.SelectedIndex;
            string value = comboBox_Teach_LoadPicker.SelectedItem.ToString();
            Console.WriteLine($"comboBox_Teach_Picker 선택된 인덱스: {index}, 값: {value}");

            changeComboBoxLoadPickerNo(index);
        }

        private void comboBox_Teach_UnloadPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox_Teach_UnloadPicker.SelectedIndex;
            string value = comboBox_Teach_UnloadPicker.SelectedItem.ToString();
            Console.WriteLine($"comboBox_Teach_UnloadPicker 선택된 인덱스: {index}, 값: {value}");

            changeComboBoxUnloadPickerNo(index);
        }
        private void GetLoadPickerOffsetData()
        {
            int PickerNo = comboBox_Teach_LoadPicker.SelectedIndex;
            Globalo.motionManager.transferMachine.productLayout.LoadTrayOffset[PickerNo].OffsetX = double.Parse(label_Teach_LoadTray_OffsetX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.LoadTrayOffset[PickerNo].OffsetY = double.Parse(label_Teach_LoadTray_OffsetY_Val.Text);

            Globalo.motionManager.transferMachine.productLayout.LoadSocketOffset[PickerNo].OffsetX = double.Parse(label_Teach_LoadSocket_OffsetX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.LoadSocketOffset[PickerNo].OffsetY = double.Parse(label_Teach_LoadSocket_OffsetY_Val.Text);
        }
        private void GetUnloadPickerOffsetData()
        {
            int PickerNo = comboBox_Teach_UnloadPicker.SelectedIndex;
            Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetX = double.Parse(label_Teach_UnloadTray_OffsetX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetY = double.Parse(label_Teach_UnloadTray_OffsetY_Val.Text);

            Globalo.motionManager.transferMachine.productLayout.UnLoadSocketOffset[PickerNo].OffsetX = double.Parse(label_Teach_UnloadSocket_OffsetX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.UnLoadSocketOffset[PickerNo].OffsetY = double.Parse(label_Teach_UnloadSocket_OffsetY_Val.Text);

            Globalo.motionManager.transferMachine.productLayout.NgOffset[PickerNo].OffsetX = double.Parse(label_Teach_Ng_OffsetX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.NgOffset[PickerNo].OffsetY = double.Parse(label_Teach_Ng_OffsetY_Val.Text);
        }
        
        private void changeComboBoxLoadPickerNo(int PickerNo)
        {
            //LoadPicker : 0 ~ 3
            //UnloadPicket : 4 ~ 7
            if (Globalo.motionManager.transferMachine.productLayout.LoadTrayOffset.Count < 1) return;

            label_Teach_LoadTray_OffsetX_Val.Text = Globalo.motionManager.transferMachine.productLayout.LoadTrayOffset[PickerNo].OffsetX.ToString("0.0##");
            label_Teach_LoadTray_OffsetY_Val.Text = Globalo.motionManager.transferMachine.productLayout.LoadTrayOffset[PickerNo].OffsetY.ToString("0.0##");

            label_Teach_LoadSocket_OffsetX_Val.Text = Globalo.motionManager.transferMachine.productLayout.LoadSocketOffset[PickerNo].OffsetX.ToString("0.0##");
            label_Teach_LoadSocket_OffsetY_Val.Text = Globalo.motionManager.transferMachine.productLayout.LoadSocketOffset[PickerNo].OffsetY.ToString("0.0##");

        }
        private void changeComboBoxUnloadPickerNo(int PickerNo)
        {
            //LoadPicker : 0 ~ 3
            //UnloadPicket : 4 ~ 7
            if (Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset.Count < 1) return;

            label_Teach_UnloadTray_OffsetX_Val.Text = Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetX.ToString("0.0##");
            label_Teach_UnloadTray_OffsetY_Val.Text = Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetY.ToString("0.0##");

            label_Teach_UnloadSocket_OffsetX_Val.Text = Globalo.motionManager.transferMachine.productLayout.UnLoadSocketOffset[PickerNo].OffsetX.ToString("0.0##");
            label_Teach_UnloadSocket_OffsetY_Val.Text = Globalo.motionManager.transferMachine.productLayout.UnLoadSocketOffset[PickerNo].OffsetY.ToString("0.0##");

            label_Teach_Ng_OffsetX_Val.Text = Globalo.motionManager.transferMachine.productLayout.NgOffset[PickerNo].OffsetX.ToString("0.0##");
            label_Teach_Ng_OffsetY_Val.Text = Globalo.motionManager.transferMachine.productLayout.NgOffset[PickerNo].OffsetY.ToString("0.0##");
        }
        private void changeBtnMotorNo(int MotorNo)
        {
            int i = 0;
            if (MotorNo < 0)
            {
                return;
            }
            for (i = 0; i < TeachBtnArr.Length; i++)
            {
                TeachBtnArr[i].BackColor = ColorTranslator.FromHtml(ColorDefaultBtn);
                TeachBtnArr[i].ForeColor = Color.White;
            }

            TeachBtnArr[MotorNo].BackColor = ColorTranslator.FromHtml(ColorSelecttBtn);
            SelectAxisIndex = (int)MotorNo;


            myTeachingGrid.changeMotorNo(SelectAxisIndex);

            if (Globalo.motionManager.transferMachine.teachingConfig.Resolution.Count < 1)
            {
                Console.WriteLine("transferMachine Resolution Zero");
                return;
            }
            TeachResolution(Globalo.motionManager.transferMachine.teachingConfig.Resolution[MotorNo].ToString("0.#"));


        }
        public void MotorJogStop()
        {
            Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex].Stop();
        }
        public async Task<bool> MotorJogMove(int nDic, double dSpeed)
        {
            cts?.Dispose();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            bool isSuccess = false;
            try
            {
                await Task.Run(() =>
                {

                    isSuccess = Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex].JogMove(nDic, dSpeed);

                    Globalo.LogPrint("ManualControl", $"[TASK] MotorRelMove End");
                }, token);
            }
            catch (OperationCanceledException)
            {
                Globalo.LogPrint("ManualControl", $"모터 작업이 취소되었습니다");
                isSuccess = false;
            }
            catch (Exception ex)
            {
                // 그 외 예외 처리
                Globalo.LogPrint("ManualControl", $"모터 이동 실패: {ex.Message}");
                isSuccess = false;
            }
            finally
            {
                // 리소스 정리
                cts?.Dispose();  // cts가 null이 아닐 때만 Dispose 호출
                ////cts = null;      // cts를 null로 설정하여 다음 작업에서 새로 생성할 수 있게
            }

            Globalo.LogPrint("ManualControl", $"[FUNCTION] MotorJogMove End");
            return isSuccess;
        }

        public async Task<bool> MotorRelMove(double dPos)
        {
            cts?.Dispose();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            bool isSuccess = false;
            try
            {
                await Task.Run(() =>
                {
                    isSuccess = Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex].MoveAxis(dPos, AXT_MOTION_ABSREL.POS_REL_MODE,  false);

                    Globalo.LogPrint("ManualControl", $"[TASK] MotorRelMove End");
                }, token);
            }
            catch (OperationCanceledException)
            {
                Globalo.LogPrint("ManualControl", $"모터 작업이 취소되었습니다");
                isSuccess = false;
            }
            catch (Exception ex)
            {
                // 그 외 예외 처리
                Globalo.LogPrint("ManualControl", $"모터 이동 실패: {ex.Message}");
                isSuccess = false;
            }
            finally
            {
                // 리소스 정리
                cts?.Dispose();  // cts가 null이 아닐 때만 Dispose 호출
                ////cts = null;      // cts를 null로 설정하여 다음 작업에서 새로 생성할 수 있게
            }

            Globalo.LogPrint("ManualControl", $"[FUNCTION] MotorRelMove End");
            return isSuccess;
        }

        private void BTN_TEACH_SERVO_ON_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex].ServoOn();
        }

        private void BTN_TEACH_SERVO_OFF_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex].ServoOff();
        }

        private void BTN_TEACH_SERVO_RESET_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex].AmpFaultReset();
        }

        private void BTN_TEACH_PCB_X_Click(object sender, EventArgs e)
        {
            changeBtnMotorNo((int)Machine.eTransfer.TRANSFER_X);
        }

        private void BTN_TEACH_PCB_Y_Click(object sender, EventArgs e)
        {
            changeBtnMotorNo((int)Machine.eTransfer.TRANSFER_Y);
        }

        private void BTN_TEACH_PCB_Z_Click(object sender, EventArgs e)
        {
            changeBtnMotorNo((int)Machine.eTransfer.TRANSFER_Z);
        }

        private void BTN_TEACH_DATA_SAVE_Click(object sender, EventArgs e)
        {
            //Teaching 저장하시겠습니까?
            string szLog = $"[TRANSFER] Teaching Save?";

            DialogResult result = DialogResult.None;

            _syncContext.Send(_ =>
            {
                result = Globalo.MessageAskPopup(szLog);
            }, null);

            if (result == DialogResult.Yes)
            {
                Globalo.motionManager.transferMachine.teachingConfig = myTeachingGrid.GetTeachData(Globalo.motionManager.transferMachine.teachingConfig);
                
                
                double dResol = double.Parse(LABEL_TEACH_ROSOLUTION_VALUE.Text);
                Globalo.motionManager.transferMachine.teachingConfig.Resolution[SelectAxisIndex] = dResol;
                
                //Motor Speed 적용
                int length = Globalo.motionManager.transferMachine.MotorAxes.Length;

                for (int i = 0; i < length; i++)
                {
                    Globalo.motionManager.transferMachine.MotorAxes[i].Velocity = Globalo.motionManager.transferMachine.teachingConfig.Speed[i];
                    Globalo.motionManager.transferMachine.MotorAxes[i].Acceleration = Globalo.motionManager.transferMachine.teachingConfig.Accel[i];
                    Globalo.motionManager.transferMachine.MotorAxes[i].Deceleration = Globalo.motionManager.transferMachine.teachingConfig.Decel[i];
                }

                Globalo.LogPrint("", "[TEACH] TRANSFER UNIT SAVE");

                Globalo.motionManager.transferMachine.teachingConfig.SaveTeach(Machine.TransferMachine.teachingPath);

                //Picket Offset Save
                GetLoadPickerOffsetData();
                GetUnloadPickerOffsetData();

                Data.TaskDataYaml.TaskSave_Layout(Globalo.motionManager.transferMachine.productLayout, Machine.TransferMachine.LayoutPath);

            }
                
        }


        private void LABEL_TEACH_ROSOLUTION_VALUE_Click(object sender, EventArgs e)
        {
            string labelValue = LABEL_TEACH_ROSOLUTION_VALUE.Text;
            decimal decimalValue = 0;


            if (decimal.TryParse(labelValue, out decimalValue))
            {
                // 소수점 형식으로 변환
                string formattedValue = decimalValue.ToString("0.#");
                NumPadForm popupForm = new NumPadForm(formattedValue);

                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    double dNumData = Double.Parse(popupForm.NumPadResult);
                    if (dNumData > 1000000.0)
                    {
                        dNumData = 1000000.0;
                    }
                    if (dNumData < 1.0)
                    {
                        dNumData = 1.0;
                    }
                    LABEL_TEACH_ROSOLUTION_VALUE.Text = dNumData.ToString("0.#");
                }
                // popupForm.Show(); // 비모달로 팝업 폼 표시
            }
        }
        private void PicketOffsetInput(Label OffsetLabel)
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
        private void label_Teach_LoadTray_OffsetX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            PicketOffsetInput(clickedLabel);
        }

        private void label_Teach_LoadTray_OffsetY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            PicketOffsetInput(clickedLabel);
        }

        private void label_Teach_UnloadTray_OffsetX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            PicketOffsetInput(clickedLabel);
        }

        private void label_Teach_UnloadTray_OffsetY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            PicketOffsetInput(clickedLabel);
        }

        private void label_Teach_LoadSocket_OffsetX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            PicketOffsetInput(clickedLabel);
        }

        private void label_Teach_LoadSocket_OffsetY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            PicketOffsetInput(clickedLabel);
        }

        private void label_Teach_UnloadSocket_OffsetX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            PicketOffsetInput(clickedLabel);
        }

        private void label_Teach_UnloadSocket_OffsetY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            PicketOffsetInput(clickedLabel);
        }

        private void label_Teach_Ng_OffsetX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            PicketOffsetInput(clickedLabel);
        }

        private void label_Teach_Ng_OffsetY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            PicketOffsetInput(clickedLabel);
        }
        private void label_Teach_LoadTray_OffsetX_MouseEnter(object sender, EventArgs e)
        {
            TeachingTransferTooltip.SetToolTip(label_Teach_LoadTray_OffsetX, "클릭시 현재 Offset 적용!");
            TeachingTransferTooltip.AutoPopDelay = 3000;
            TeachingTransferTooltip.InitialDelay = 10;
            TeachingTransferTooltip.ShowAlways = false;
        }
        private void label_Teach_UnloadTray_OffsetX_MouseEnter(object sender, EventArgs e)
        {
            TeachingTransferTooltip.SetToolTip(label_Teach_UnloadTray_OffsetX, "클릭시 현재 Offset 적용!");
            TeachingTransferTooltip.AutoPopDelay = 3000;
            TeachingTransferTooltip.InitialDelay = 10;
            TeachingTransferTooltip.ShowAlways = false;
        }
        private void label_Teach_LoadTray_OffsetX_Click(object sender, EventArgs e)
        {
            int PickerNo = comboBox_Teach_LoadPicker.SelectedIndex;
            //x축 현재위치
            //Tray 간격
            //TRAY LOAD POS 가 L, R 두개 있지만 간격 동일해서 아무위치에서 해도될듯
            //double dOffset = (10.0 - Load Pos X) - (Tray x 간격 * PickerNo); 이동시 Tray 간격은 더해지니 빼준다.
            //(현재위치 - 티칭위치) - (Tray X 간격 * PickerNo)

            
            double TechingPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS].Pos[(int)Machine.eTransfer.TRANSFER_X];     //x Axis
            double CurrentX = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].EncoderPos;
            double dOffset = (CurrentX - TechingPos) - (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX * PickerNo);   //TODO: 마무리안됨
            Console.WriteLine($"[Load X]Current X:{CurrentX} , TechingPos:{TechingPos} , dOffset : {dOffset}");
            
            label_Teach_LoadTray_OffsetX_Val.Text = dOffset.ToString("0.0##");
        }

        

        private void label_Teach_LoadTray_OffsetY_Click(object sender, EventArgs e)
        {
            int PickerNo = comboBox_Teach_LoadPicker.SelectedIndex;

            double TechingPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS].Pos[(int)Machine.eTransfer.TRANSFER_Y];     //x Axis
            double CurrentY = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].EncoderPos;
            double dOffset = (CurrentY - TechingPos) - (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY * PickerNo);   //TODO: 마무리안됨
            Console.WriteLine($"[Load Y] Current Y:{CurrentY} , TechingPos:{TechingPos} , dOffset : {dOffset}");

            label_Teach_LoadTray_OffsetY_Val.Text = dOffset.ToString("0.0##");
        }

        private void label_Teach_UnloadTray_OffsetX_Click(object sender, EventArgs e)
        {
            int PickerNo = comboBox_Teach_UnloadPicker.SelectedIndex;
            double TechingPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_UNLOAD_POS].Pos[(int)Machine.eTransfer.TRANSFER_X];     //x Axis
            double CurrentX = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].EncoderPos;
            double dOffset = (CurrentX - TechingPos) - (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX * PickerNo);   //TODO: 마무리안됨
            Console.WriteLine($"[Unload X] Current X:{CurrentX} , TechingPos:{TechingPos} , dOffset : {dOffset}");

            label_Teach_UnloadTray_OffsetX_Val.Text = dOffset.ToString("0.0##");
        }

        private void label_Teach_UnloadTray_OffsetY_Click(object sender, EventArgs e)
        {
            int PickerNo = comboBox_Teach_UnloadPicker.SelectedIndex;

            double TechingPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_UNLOAD_POS].Pos[(int)Machine.eTransfer.TRANSFER_Y];     //x Axis
            double CurrentY = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].EncoderPos;
            double dOffset = (CurrentY - TechingPos) - (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY * PickerNo);   //TODO: 마무리안됨
            Console.WriteLine($"[Unload Y] Current Y:{CurrentY} , TechingPos:{TechingPos} , dOffset : {dOffset}");

            label_Teach_UnloadTray_OffsetY_Val.Text = dOffset.ToString("0.0##");
        }

        private void label_Teach_Ng_OffsetX_Click(object sender, EventArgs e)
        {
            int PickerNo = comboBox_Teach_UnloadPicker.SelectedIndex;
            double TechingPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)Machine.TransferMachine.eTeachingPosList.NG_A_UNLOAD].Pos[(int)Machine.eTransfer.TRANSFER_X];     //x Axis
            double CurrentX = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].EncoderPos;
            double dOffset = (CurrentX - TechingPos) - (Globalo.motionManager.transferMachine.productLayout.NgGap.GapX * PickerNo);
            Console.WriteLine($"[Ng X] Current X:{CurrentX} , TechingPos:{TechingPos} , dOffset : {dOffset}");

            label_Teach_Ng_OffsetX_Val.Text = dOffset.ToString("0.0##");
        }

        private void label_Teach_Ng_OffsetY_Click(object sender, EventArgs e)
        {
            int PickerNo = comboBox_Teach_UnloadPicker.SelectedIndex;

            double TechingPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)Machine.TransferMachine.eTeachingPosList.NG_A_UNLOAD].Pos[(int)Machine.eTransfer.TRANSFER_Y];     //x Axis
            double CurrentY = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].EncoderPos;
            double dOffset = (CurrentY - TechingPos) - (Globalo.motionManager.transferMachine.productLayout.NgGap.GapY * PickerNo);
            Console.WriteLine($"[Ng Y] Current Y:{CurrentY} , TechingPos:{TechingPos} , dOffset : {dOffset}");

            label_Teach_Ng_OffsetY_Val.Text = dOffset.ToString("0.0##");
        }

        private void label_Teach_LoadSocket_OffsetX_Click(object sender, EventArgs e)
        {
            int PickerNo = comboBox_Teach_LoadPicker.SelectedIndex;

            double TechingPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)Machine.TransferMachine.eTeachingPosList.SOCKET_A_LOAD].Pos[(int)Machine.eTransfer.TRANSFER_X];     //x Axis
            double CurrentX = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].EncoderPos;
            double dOffset = (CurrentX - TechingPos) - (Globalo.motionManager.transferMachine.productLayout.SocketGap.GapX * PickerNo);   //TODO: 마무리안됨
            Console.WriteLine($"[Socket Load X]Current X:{CurrentX} , TechingPos:{TechingPos} , dOffset : {dOffset}");

            label_Teach_LoadSocket_OffsetX_Val.Text = dOffset.ToString("0.0##");
        }

        private void label_Teach_LoadSocket_OffsetY_Click(object sender, EventArgs e)
        {
            int PickerNo = comboBox_Teach_LoadPicker.SelectedIndex;

            double TechingPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)Machine.TransferMachine.eTeachingPosList.SOCKET_A_LOAD].Pos[(int)Machine.eTransfer.TRANSFER_Y];     //x Axis
            double CurrentY = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].EncoderPos;
            double dOffset = (CurrentY - TechingPos) - (Globalo.motionManager.transferMachine.productLayout.SocketGap.GapY * PickerNo);   //TODO: 마무리안됨
            Console.WriteLine($"[Socket Load Y] Current Y:{CurrentY} , TechingPos:{TechingPos} , dOffset : {dOffset}");

            label_Teach_LoadSocket_OffsetY_Val.Text = dOffset.ToString("0.0##");
        }

        private void label_Teach_UnloadSocket_OffsetX_Click(object sender, EventArgs e)
        {
            int PickerNo = comboBox_Teach_UnloadPicker.SelectedIndex;
            double TechingPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)Machine.TransferMachine.eTeachingPosList.SOCKET_A_UNLOAD].Pos[(int)Machine.eTransfer.TRANSFER_X];     //x Axis
            double CurrentX = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_X].EncoderPos;
            double dOffset = (CurrentX - TechingPos) - (Globalo.motionManager.transferMachine.productLayout.SocketGap.GapX * PickerNo);
            Console.WriteLine($"[Socket Unload X] Current X:{CurrentX} , TechingPos:{TechingPos} , dOffset : {dOffset}");

            label_Teach_UnloadSocket_OffsetX_Val.Text = dOffset.ToString("0.0##");
        }

        private void label_Teach_UnloadSocket_OffsetY_Click(object sender, EventArgs e)
        {
            int PickerNo = comboBox_Teach_UnloadPicker.SelectedIndex;
            double TechingPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)Machine.TransferMachine.eTeachingPosList.SOCKET_A_UNLOAD].Pos[(int)Machine.eTransfer.TRANSFER_Y];     //x Axis
            double CurrentY = Globalo.motionManager.transferMachine.MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].EncoderPos;
            double dOffset = (CurrentY - TechingPos) - (Globalo.motionManager.transferMachine.productLayout.SocketGap.GapY * PickerNo);
            Console.WriteLine($"[Socket Unload Y] Current Y:{CurrentY} , TechingPos:{TechingPos} , dOffset : {dOffset}");

            label_Teach_UnloadSocket_OffsetY_Val.Text = dOffset.ToString("0.0##");
        }
    }
}
