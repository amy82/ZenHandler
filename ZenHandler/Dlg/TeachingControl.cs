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
    public partial class TeachingControl : UserControl
    {
        private eTeachingBtn TeachCurrentTab;
        private TeachingTransfer transferTeach;

        //liftTeach
        //socketTeach



        private double m_dJogSpeed = 0.1;
        private List<UserControl> MachineControl = new List<UserControl>();


        public enum eTeachingBtn : int
        {
            TransferTab = 0, MagazineTab , LiftTab , pcbTab, lensTab
        };

        public TeachingControl(int _w , int _h)
        {
            InitializeComponent();

            Event.EventManager.LanguageChanged += OnLanguageChanged;
            MachineControl.Clear();


            transferTeach = new TeachingTransfer();
            //teachingLens = new TeachingLens();

            TeachCurrentTab = eTeachingBtn.TransferTab;

            MachineControl.Add(transferTeach);

            
            this.Paint += new PaintEventHandler(Form_Paint);

            //motorControl = motor;
            this.Width = _w;
            this.Height = _h;


            transferTeach.Visible = false;
            //teachingLens.Visible = false;
            this.Controls.Add(transferTeach);
            //this.Controls.Add(teachingLens);

            transferTeach.Location = new System.Drawing.Point(this.TeachingPanel.Location.X, this.TeachingPanel.Location.Y);
            setInterface();

            changeSpeedNo(0);

            TeachingBtnChange(TeachCurrentTab);
        }
        private void OnLanguageChanged(object sender, EventArgs e)
        {
            // 이벤트 처리
            Console.WriteLine("TeachingControl - OnLanguageChanged");
        }
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            int lineStartY = TeachingTitleLabel.Location.Y + Globalo.TabLineY;
            // Graphics 객체 가져오기
            Graphics g = e.Graphics;

            // Pen 객체 생성 (색상과 두께 설정)
            Color color = Color.FromArgb(175, 175, 175);//Color.FromArgb(151, 149, 145);
            Pen pen = new Pen(color, 1);

            // 라인 그리기 (시작점과 끝점 설정)
            g.DrawLine(pen, 0, lineStartY, this.Width, lineStartY);

            // 리소스 해제
            pen.Dispose();
            
        }
        public void setInterface()
        {
            TeachingTitleLabel.ForeColor = ColorTranslator.FromHtml("#6F6F6F");

            BTN_TEACH_TRANSFER.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            BTN_TEACH_LENS.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");


            BTN_TEACH_SPEED_LOW.BackColor = ColorTranslator.FromHtml("#C3A279");
            BTN_TEACH_SPEED_LOW.ForeColor = Color.White;

            BTN_TEACH_SPEED_MID.BackColor = ColorTranslator.FromHtml("#C3A279");
            BTN_TEACH_SPEED_MID.ForeColor = Color.White;

            BTN_TEACH_SPEED_HIGH.BackColor = ColorTranslator.FromHtml("#C3A279");
            BTN_TEACH_SPEED_HIGH.ForeColor = Color.White;

            BTN_TEACH_JOG_MINUS.BackColor = ColorTranslator.FromHtml("#C3A279");
            BTN_TEACH_JOG_MINUS.ForeColor = Color.White;

            BTN_TEACH_JOG_STOP.BackColor = ColorTranslator.FromHtml("#C3A279");
            BTN_TEACH_JOG_STOP.ForeColor = Color.White;

            BTN_TEACH_JOG_PLUS.BackColor = ColorTranslator.FromHtml("#C3A279");     //BTN_TEACH_JOG_PLUS_MouseDown
            BTN_TEACH_JOG_PLUS.ForeColor = Color.White;                             //BTN_TEACH_JOG_PLUS_MouseUp

            BTN_TEACH_MOVE_MINUS.BackColor = ColorTranslator.FromHtml("#C3A279");
            BTN_TEACH_MOVE_MINUS.ForeColor = Color.White;

            BTN_TEACH_MOVE_PLUS.BackColor = ColorTranslator.FromHtml("#C3A279");
            BTN_TEACH_MOVE_PLUS.ForeColor = Color.White;

        }
        
        private void TeachTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 현재 선택된 탭의 인덱스
            //currentTabIndex = TeachTabControl.SelectedIndex;
            //if (currentTabIndex == 0)
            //{
            //    //[0] Pcb Unit
            //    changeMotorNo(m_nSelectPcbAxis);  //Teaching Dlg Visible on
            //    TeachTabControl.Width = 150 + (80 * MotorControl.PCB_UNIT_COUNT) + 12;
            //    TeachTabControl.Height = dRowHeight * (nGridRowCount + 2) - 7;// 560;
            //}
            //else
            //{
            //    //[1] Lens Unit
            //    changeMotorNo(m_nSelectLensAxis);
            //    TeachTabControl.Width = 150 + (80 * MotorControl.LENS_UNIT_COUNT) + 12;
            //    TeachTabControl.Height = dRowHeight * (nGridRowCount + 2) - 7;// 560;
            //}

            //changeSpeedNo(0);



            //ShowTeachingData();

            ///MessageBox.Show($"Selected Tab Index: {selectedIndex}");
        }
        

        private void TeachingControl_Load(object sender, EventArgs e)
        {
            changeSpeedNo(0);

            //changeMotorNo(m_nSelectPcbAxis);      //Teaching Dlg Load

            //ShowTeachingData();

            Globalo.LogPrint("CTeachingControl", "Teach Visible True raised!!!");
            if (ProgramState.ON_LINE_MOTOR)
            {
                //TeachingTimer.Start(); // 타이머 시작
            }
        }
        private void TeachingControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {

            }


            if (TeachCurrentTab == eTeachingBtn.TransferTab)
            {
                transferTeach.showPanel();
            }
            else if (TeachCurrentTab == eTeachingBtn.lensTab)
            {
                transferTeach.hidePanel();

            }


        }

        


        private async void BTN_TEACH_MOVE_MINUS_Click(object sender, EventArgs e)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return;
            }
            double dMovePos = double.Parse(LABEL_TEACH_MOVE_VALUE.Text);

            if (TeachCurrentTab == eTeachingBtn.TransferTab)
            {
                bool result = await transferTeach.MotorRelMove(dMovePos * -1);
            }

            
        }

        private async void BTN_TEACH_MOVE_PLUS_Click(object sender, EventArgs e)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return;
            }
            double dMovePos = double.Parse(LABEL_TEACH_MOVE_VALUE.Text);

            if (TeachCurrentTab == eTeachingBtn.TransferTab)
            {   
                bool result = await transferTeach.MotorRelMove(dMovePos);   //TODO: 머신안에 함수로 바꿔야된다.
            }
            
        }

        private void LABEL_TEACH_MOVE_VALUE_Click(object sender, EventArgs e)
        {
            string labelValue = LABEL_TEACH_MOVE_VALUE.Text;
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
                    if (dNumData > 5.0)
                    {
                        dNumData = 5.0;
                    }
                    if (dNumData < -5.0)
                    {
                        dNumData = -5.0;
                    }
                    LABEL_TEACH_MOVE_VALUE.Text = dNumData.ToString("0.0##");
                }
            }
            //popupForm.NumberEntered += PopupForm_NumberEntered; // 이벤트 핸들러 등록

            
        }
        private void PopupForm_NumberEntered(object sender, double number , bool bUser)
        {
            MessageBox.Show($"Entered number: {number.ToString("0.0##")}");
            //LABEL_TEACH_MOVE_VALUE.Text = number.ToString();
        }
        private void changeSpeedNo(int nSpeedNo)
        {
            BTN_TEACH_SPEED_LOW.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
            BTN_TEACH_SPEED_MID.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);
            BTN_TEACH_SPEED_HIGH.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_OFF);

            switch (nSpeedNo)
            {
                case 0:
                    BTN_TEACH_SPEED_LOW.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    m_dJogSpeed = 0.1;
                    break;
                case 1:
                    BTN_TEACH_SPEED_MID.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    m_dJogSpeed = 0.5;
                    break;
                case 2:
                    BTN_TEACH_SPEED_HIGH.BackColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
                    m_dJogSpeed = 1.0;
                    break;
            }
        }

        private async void BTN_TEACH_JOG_MINUS_MouseDown(object sender, MouseEventArgs e)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return;
            }
            if (TeachCurrentTab == eTeachingBtn.TransferTab)
            {
                bool result = await transferTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.MINUS_MOVE, m_dJogSpeed);
            }
        }
        private async void BTN_TEACH_JOG_PLUS_MouseDown(object sender, MouseEventArgs e)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return;
            }
            if (TeachCurrentTab == eTeachingBtn.TransferTab)
            {
                bool result = await transferTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.PLUS_MOVE, m_dJogSpeed);
            }
        }
        private void JOG_STOP_FN()
        {
            if (TeachCurrentTab == eTeachingBtn.TransferTab)
            {
                transferTeach.MotorJogStop();
            }
        }
        private void BTN_TEACH_JOG_STOP_Click(object sender, EventArgs e)
        {
            JOG_STOP_FN();
        }
        private  void BTN_TEACH_JOG_MINUS_MouseUp(object sender, MouseEventArgs e)
        {
            JOG_STOP_FN();
        }
        private void BTN_TEACH_JOG_PLUS_MouseUp(object sender, MouseEventArgs e)
        {
            JOG_STOP_FN();
        }

        private void BTN_TEACH_SPEED_LOW_Click(object sender, EventArgs e)
        {
            changeSpeedNo(0);
        }

        private void BTN_TEACH_SPEED_MID_Click(object sender, EventArgs e)
        {
            changeSpeedNo(1);
        }

        private void BTN_TEACH_SPEED_HIGH_Click(object sender, EventArgs e)
        {
            changeSpeedNo(2);
        }
        private void TeachingBtnChange(eTeachingBtn index)
        {
            BTN_TEACH_TRANSFER.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            BTN_TEACH_LENS.BackColor = ColorTranslator.FromHtml("#E1E0DF");

            TeachCurrentTab = index;

            if (TeachCurrentTab == eTeachingBtn.TransferTab)
            {
                BTN_TEACH_TRANSFER.BackColor = ColorTranslator.FromHtml("#FFB230");
                transferTeach.Visible = true;
                //teachingLens.Visible = false;

                //teachingLens.hidePanel();
                transferTeach.showPanel();
            }
            else
            {
                BTN_TEACH_LENS.BackColor = ColorTranslator.FromHtml("#FFB230");
                //teachingLens.Visible = true;
                transferTeach.Visible = false;

                transferTeach.hidePanel();
                //teachingLens.showPanel();
            }
        }
        private void BTN_TEACH_PCB_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eTeachingBtn.TransferTab);
        }

        private void BTN_TEACH_LENS_Click(object sender, EventArgs e)
        {
            //TeachingBtnChange(eTeachingBtn.lensTab);
        }
    }
}
