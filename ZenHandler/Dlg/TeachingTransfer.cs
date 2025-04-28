using System;
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
        //
        public TeachingTransfer()
        {
            InitializeComponent();
            _syncContext = SynchronizationContext.Current;
            cts = new CancellationTokenSource();

            int[] inGridWid = new int[] { 150, 80, 80, 80};         //Grid Width

            myTeachingGrid = new Controls.TeachingGridView( Globalo.motionManager.transferMachine.MotorAxes, Globalo.motionManager.transferMachine.teachingConfig, inGridWid);

            myTeachingGrid.Location = new System.Drawing.Point(150, 10);
            this.groupTeachPcb.Controls.Add(myTeachingGrid);

            TeachTransferUiSet();
            
            changeBtnMotorNo(SelectAxisIndex);

            TeachResolution(Globalo.motionManager.transferMachine.teachingConfig.Resolution[SelectAxisIndex].ToString("0.#"));
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



            comboBox_Teach_Picker.Items.Add("Load Picker #1");
            comboBox_Teach_Picker.Items.Add("Load Picker #2");
            comboBox_Teach_Picker.Items.Add("Load Picker #3");
            comboBox_Teach_Picker.Items.Add("Load Picker #4");
            comboBox_Teach_Picker.Items.Add("UnLoad Picker #1");
            comboBox_Teach_Picker.Items.Add("UnLoad Picker #2");
            comboBox_Teach_Picker.Items.Add("UnLoad Picker #3");
            comboBox_Teach_Picker.Items.Add("UnLoad Picker #4");

            comboBox_Teach_Picker.SelectedIndex = 0;  // 첫 번째 항목 선택
        }

        public void showPanel()
        {
            if (ProgramState.ON_LINE_MOTOR == true)
            {
                myTeachingGrid.MotorStateRun(true);
            }
            myTeachingGrid.ShowTeachingData();
            TeachResolution(Globalo.motionManager.transferMachine.teachingConfig.Resolution[SelectAxisIndex].ToString("0.#"));
        }
        public void hidePanel()
        {
            myTeachingGrid.MotorStateRun(false);
            //TeachingTimer.Stop();
        }
        private void comboBox_Teach_Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox_Teach_Picker.SelectedIndex;
            string value = comboBox_Teach_Picker.SelectedItem.ToString();
            Console.WriteLine($"comboBox_Teach_Picker 선택된 인덱스: {index}, 값: {value}");

            changeComboBoxPickerNo(index);
        }
        private void GetPickerOffsetData()
        {
            int PickerNo = comboBox_Teach_Picker.SelectedIndex;
            Globalo.motionManager.transferMachine.productLayout.LoadTrayOffset[PickerNo].OffsetX = double.Parse(label_Teach_LoadTray_OffsetX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.LoadTrayOffset[PickerNo].OffsetY = double.Parse(label_Teach_LoadTray_OffsetY_Val.Text);

            Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetX = double.Parse(label_Teach_UnloadTray_OffsetX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetY = double.Parse(label_Teach_UnloadTray_OffsetY_Val.Text);

            Globalo.motionManager.transferMachine.productLayout.LoadSocketOffset[PickerNo].OffsetX = double.Parse(label_Teach_LoadSocket_OffsetX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.LoadSocketOffset[PickerNo].OffsetY = double.Parse(label_Teach_LoadSocket_OffsetY_Val.Text);

            Globalo.motionManager.transferMachine.productLayout.UnLoadSocketOffset[PickerNo].OffsetX = double.Parse(label_Teach_UnloadSocket_OffsetX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.UnLoadSocketOffset[PickerNo].OffsetY = double.Parse(label_Teach_UnloadSocket_OffsetY_Val.Text);

            Globalo.motionManager.transferMachine.productLayout.NgOffset[PickerNo].OffsetX = double.Parse(label_Teach_Ng_OffsetX_Val.Text);
            Globalo.motionManager.transferMachine.productLayout.NgOffset[PickerNo].OffsetY = double.Parse(label_Teach_Ng_OffsetY_Val.Text);
        }
        private void changeComboBoxPickerNo(int PickerNo)
        {
            //LoadPicker : 0 ~ 3
            //UnloadPicket : 4 ~ 7

            label_Teach_LoadTray_OffsetX_Val.Text = Globalo.motionManager.transferMachine.productLayout.LoadTrayOffset[PickerNo].OffsetX.ToString("0.0##");
            label_Teach_LoadTray_OffsetY_Val.Text = Globalo.motionManager.transferMachine.productLayout.LoadTrayOffset[PickerNo].OffsetY.ToString("0.0##");

            label_Teach_UnloadTray_OffsetX_Val.Text = Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetX.ToString("0.0##");
            label_Teach_UnloadTray_OffsetY_Val.Text = Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetY.ToString("0.0##");

            label_Teach_LoadSocket_OffsetX_Val.Text = Globalo.motionManager.transferMachine.productLayout.LoadSocketOffset[PickerNo].OffsetX.ToString("0.0##");
            label_Teach_LoadSocket_OffsetY_Val.Text = Globalo.motionManager.transferMachine.productLayout.LoadSocketOffset[PickerNo].OffsetY.ToString("0.0##");

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
                GetPickerOffsetData();

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
    }
}
