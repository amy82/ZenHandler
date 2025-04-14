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
    public partial class TeachingTransfer : UserControl
    {
        private Controls.TeachingGridView myTeachingGrid;

        private Button[] TeachBtnArr;
        private string ColorDefaultBtn = "#C3A279";
        private string ColorSelecttBtn = "#4C4743";

        public int SelectAxisIndex = -1;        //선택 모터 순서
        //
        public TeachingTransfer()
        {
            InitializeComponent();


            //Custom Teaching Grid Add
            //
            int[] inGridWid = new int[] { 100, 80, 80, 80};         //Grid Width

            myTeachingGrid = new Controls.TeachingGridView(
                Globalo.motionManager.transferMachine.MotorAxes, 
                Globalo.yamlManager.teachingDataYaml.teachingHandlerData.TransferMachine, 
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

        public bool MotorRelMove(double dPos)
        {
            //await MoveFromAbsRel(Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex], dPos);
            //Globalo.motionManager.transferMachine.MotorAxes[SelectAxisIndex].
            return true;
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
            Globalo.yamlManager.teachingDataYaml.teachingHandlerData.TransferMachine = myTeachingGrid.GetTeachData();

            Globalo.yamlManager.teachingDataYaml.SaveTeaching();
        }
    }
}
