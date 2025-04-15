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
        private Controls.TeachingGridView myTeachingGrid;

        private Button[] TeachBtnArr;
        private string ColorDefaultBtn = "#C3A279";
        private string ColorSelecttBtn = "#4C4743";

        protected CancellationTokenSource cts;
        public int SelectAxisIndex = -1;        //선택 모터 순서
        //
        public TeachingTransfer()
        {
            InitializeComponent();
            cts = new CancellationTokenSource();

            //Custom Teaching Grid Add
            //
            int[] inGridWid = new int[] { 110, 80, 80, 80};         //Grid Width

            myTeachingGrid = new Controls.TeachingGridView(
                Globalo.motionManager.transferMachine.MotorAxes, 
                Globalo.yamlManager.teachingDataYaml.handler.TransferMachine, 
                inGridWid);
            myTeachingGrid.Location = new System.Drawing.Point(150, 28);
            this.groupTeachPcb.Controls.Add(myTeachingGrid);
            //
            //nGridRowCount = nGridSensorRowCount + nGridSpeedRowCount;

            TeachTransferUiSet();


            changeBtnMotorNo(0);
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

        }
        public void showPanel()
        {
            if (ProgramState.ON_LINE_MOTOR == true)
            {
                //TeachingTimer.Start();
            }
            myTeachingGrid.ShowTeachingData();
        }
        public void hidePanel()
        {
            //TeachingTimer.Stop();
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
                    //g_clSysData.m_dMotorSpeed[m_nUnit][m_nSelectAxis] * g_clSysData.m_dMotorResol[m_nUnit][m_nSelectAxis] * m_dJogSpeed

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

                    isSuccess = Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex].MoveAxis(AXT_MOTION_ABSREL.POS_REL_MODE, dPos, 5.0, false);

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

            Globalo.LogPrint("ManualControl", $"[FUNCTION] MoveFromAbsRel End");
            return isSuccess;
        }

        private void BTN_TEACH_SERVO_ON_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex].ServoOn();
        }

        private void BTN_TEACH_SERVO_OFF_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex].ServoOff();
        }

        private void BTN_TEACH_SERVO_RESET_Click(object sender, EventArgs e)
        {
            Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex].AmpFaultReset();
        }

        private void BTN_TEACH_PCB_X_Click(object sender, EventArgs e)
        {
            changeBtnMotorNo(0);   //TODO: index 확인 필요 안전장치 필요
        }

        private void BTN_TEACH_PCB_Y_Click(object sender, EventArgs e)
        {
            changeBtnMotorNo(1);
        }

        private void BTN_TEACH_PCB_Z_Click(object sender, EventArgs e)
        {
            changeBtnMotorNo(2);
        }

        private void BTN_TEACH_DATA_SAVE_Click(object sender, EventArgs e)
        {
            Globalo.yamlManager.teachingDataYaml.handler.TransferMachine = myTeachingGrid.GetTeachData(Globalo.yamlManager.teachingDataYaml.handler.TransferMachine);

            //Motor Speed 적용
            //int length = Globalo.yamlManager.teachingDataYaml.handler.TransferMachine.Speed.Count;
            int length = Globalo.motionManager.transferMachine.MotorAxes.Length;



            for (int i = 0; i < length; i++)
            {
                Globalo.motionManager.transferMachine.MotorAxes[i].Velocity = Globalo.yamlManager.teachingDataYaml.handler.TransferMachine.Speed[i];
                Globalo.motionManager.transferMachine.MotorAxes[i].Acceleration = Globalo.yamlManager.teachingDataYaml.handler.TransferMachine.Accel[i];
                Globalo.motionManager.transferMachine.MotorAxes[i].Deceleration = Globalo.yamlManager.teachingDataYaml.handler.TransferMachine.Decel[i];
            }

            Globalo.LogPrint("", "[TEACH] TRANSFER UNIT SAVE"); 
            Globalo.yamlManager.teachingDataYaml.SaveTeaching();
        }
    }
}
