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
        //
        private TeachingTransfer TransferTeach;
        private TeachingMagazine MagazineTeach;
        private TeachingLift LiftTeach;

        //private TeachingSocket transferTeach;

        //liftTeach
        //socketTeach
        private TeachingAoiSocket AoiSocketTeach;      //TODO: 소켓 추가
        private TeachingEEpromSocket EEpromSocketTeach;
        //private TeachingFwSocket FwSocket;


        private double m_dJogSpeed = 0.1;
        private List<UserControl> MachineControl = new List<UserControl>();


        public enum eTeachingBtn : int
        {
            TransferTab = 0, MagazineTab , LiftTab , SocketTab
        };

        public TeachingControl(int _w , int _h)
        {
            InitializeComponent();

            Event.EventManager.LanguageChanged += OnLanguageChanged;
            MachineControl.Clear();


            TransferTeach = new TeachingTransfer();
            MagazineTeach = new TeachingMagazine();
            LiftTeach = new TeachingLift();
            AoiSocketTeach = new TeachingAoiSocket();
            EEpromSocketTeach = new TeachingEEpromSocket();
            if (Program.PG_SELECT == HANDLER_PG.AOI)
            {

            }
            if (Program.PG_SELECT == HANDLER_PG.EEPROM)
            {

            }
            if (Program.PG_SELECT == HANDLER_PG.FW)
            {

            }
            TeachCurrentTab = eTeachingBtn.TransferTab;
            TransferTeach.Visible = false;
            MagazineTeach.Visible = false;
            LiftTeach.Visible = false;
            AoiSocketTeach.Visible = false;
            EEpromSocketTeach.Visible = false;

            MachineControl.Add(TransferTeach);
            MachineControl.Add(MagazineTeach);
            MachineControl.Add(LiftTeach);
            MachineControl.Add(AoiSocketTeach);
            MachineControl.Add(EEpromSocketTeach);

            
            this.Paint += new PaintEventHandler(Form_Paint);

            this.Width = _w;
            this.Height = _h;


            
            this.Controls.Add(TransferTeach);
            this.Controls.Add(MagazineTeach);
            this.Controls.Add(LiftTeach);
            this.Controls.Add(AoiSocketTeach);
            this.Controls.Add(EEpromSocketTeach);

            TransferTeach.Location = new System.Drawing.Point(this.TeachingPanel.Location.X, this.TeachingPanel.Location.Y);
            MagazineTeach.Location = new System.Drawing.Point(this.TeachingPanel.Location.X, this.TeachingPanel.Location.Y);
            LiftTeach.Location = new System.Drawing.Point(this.TeachingPanel.Location.X, this.TeachingPanel.Location.Y);
            AoiSocketTeach.Location = new System.Drawing.Point(this.TeachingPanel.Location.X, this.TeachingPanel.Location.Y);
            EEpromSocketTeach.Location = new System.Drawing.Point(this.TeachingPanel.Location.X, this.TeachingPanel.Location.Y);

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
            BTN_TEACH_MAGAZINE.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            BTN_TEACH_LIFT.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            BTN_TEACH_SOCKET.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");


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

            Globalo.LogPrint("CTeachingControl", "Teach Visible True raised!!!");

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
                bool result = await TransferTeach.MotorRelMove(dMovePos * -1);
            }
            else if (TeachCurrentTab == eTeachingBtn.MagazineTab)
            {
                bool result = await MagazineTeach.MotorRelMove(dMovePos * -1);
            }
            else if (TeachCurrentTab == eTeachingBtn.LiftTab)
            {
                bool result = await LiftTeach.MotorRelMove(dMovePos * -1);
            }
            else if (TeachCurrentTab == eTeachingBtn.SocketTab)
            {
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    bool result = await AoiSocketTeach.MotorRelMove(dMovePos * -1);
                }
                else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    bool result = await EEpromSocketTeach.MotorRelMove(dMovePos * -1);
                }
                else
                {
                    //Fw는 모터 없음
                }
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
                bool result = await TransferTeach.MotorRelMove(dMovePos);   //TODO: 머신안에 함수로 바꿔야된다.
            }
            else if (TeachCurrentTab == eTeachingBtn.MagazineTab)
            {
                bool result = await MagazineTeach.MotorRelMove(dMovePos);
            }
            else if (TeachCurrentTab == eTeachingBtn.LiftTab)
            {
                bool result = await LiftTeach.MotorRelMove(dMovePos);
            }
            else if (TeachCurrentTab == eTeachingBtn.SocketTab)
            {
                bool result = false;
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    result = await AoiSocketTeach.MotorRelMove(dMovePos);
                }
                else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    result = await EEpromSocketTeach.MotorRelMove(dMovePos);
                }
                else
                {
                    //Fw는 모터 없음
                }
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
            bool result = false;
            if (TeachCurrentTab == eTeachingBtn.TransferTab)
            {
                result = await TransferTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.MINUS_MOVE, m_dJogSpeed);
            }
            else if (TeachCurrentTab == eTeachingBtn.MagazineTab)
            {
                result = await MagazineTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.MINUS_MOVE, m_dJogSpeed);
            }
            else if (TeachCurrentTab == eTeachingBtn.LiftTab)
            {
                result = await LiftTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.MINUS_MOVE, m_dJogSpeed);
            }
            else if (TeachCurrentTab == eTeachingBtn.SocketTab)
            {
                
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    result = await AoiSocketTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.MINUS_MOVE, m_dJogSpeed);
                }
                else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    result = await EEpromSocketTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.MINUS_MOVE, m_dJogSpeed);
                }
                else
                {
                    //Fw는 모터 없음
                }
            }
        }
        private async void BTN_TEACH_JOG_PLUS_MouseDown(object sender, MouseEventArgs e)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return;
            }
            bool result = false;
            if (TeachCurrentTab == eTeachingBtn.TransferTab)
            {
                result = await TransferTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.PLUS_MOVE, m_dJogSpeed);
            }
            else if (TeachCurrentTab == eTeachingBtn.MagazineTab)
            {
                result = await MagazineTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.PLUS_MOVE, m_dJogSpeed);
            }
            else if (TeachCurrentTab == eTeachingBtn.LiftTab)
            {
                result = await LiftTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.PLUS_MOVE, m_dJogSpeed);
            }
            else if (TeachCurrentTab == eTeachingBtn.SocketTab)
            {

                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    result = await AoiSocketTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.PLUS_MOVE, m_dJogSpeed);
                }
                else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    result = await EEpromSocketTeach.MotorJogMove((int)MotionControl.MotorSet.eJogDic.PLUS_MOVE, m_dJogSpeed);
                }
                else
                {
                    //Fw는 모터 없음
                }
            }
        }
        private void JOG_STOP_FN()
        {
            if (TeachCurrentTab == eTeachingBtn.TransferTab)
            {
                TransferTeach.MotorJogStop();
            }
            else if (TeachCurrentTab == eTeachingBtn.MagazineTab)
            {
                MagazineTeach.MotorJogStop();
            }
            else if (TeachCurrentTab == eTeachingBtn.LiftTab)
            {
                LiftTeach.MotorJogStop();
            }
            else if (TeachCurrentTab == eTeachingBtn.SocketTab)
            {
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    AoiSocketTeach.MotorJogStop();
                }
                else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    EEpromSocketTeach.MotorJogStop();
                }
                else
                {
                    //Fw는 모터 없음
                }
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
            BTN_TEACH_MAGAZINE.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            BTN_TEACH_LIFT.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            BTN_TEACH_SOCKET.BackColor = ColorTranslator.FromHtml("#E1E0DF");

            TeachCurrentTab = index;

            if (TeachCurrentTab == eTeachingBtn.TransferTab)
            {
                BTN_TEACH_TRANSFER.BackColor = ColorTranslator.FromHtml("#FFB230");
                //TransferTeach.Visible = true;
                TransferTeach.showPanel();
                MagazineTeach.hidePanel();
                LiftTeach.hidePanel();
                AoiSocketTeach.hidePanel();
                EEpromSocketTeach.hidePanel();
            }

            if (TeachCurrentTab == eTeachingBtn.MagazineTab)
            {
                BTN_TEACH_MAGAZINE.BackColor = ColorTranslator.FromHtml("#FFB230");
                MagazineTeach.showPanel();
                TransferTeach.hidePanel();
                LiftTeach.hidePanel();
                AoiSocketTeach.hidePanel();
                EEpromSocketTeach.hidePanel();
            }
            if (TeachCurrentTab == eTeachingBtn.LiftTab)
            {
                BTN_TEACH_LIFT.BackColor = ColorTranslator.FromHtml("#FFB230");
                LiftTeach.showPanel();
                TransferTeach.hidePanel();
                MagazineTeach.hidePanel();
                AoiSocketTeach.hidePanel();
                EEpromSocketTeach.hidePanel();
            }
            
            if (TeachCurrentTab == eTeachingBtn.SocketTab)
            {
                BTN_TEACH_SOCKET.BackColor = ColorTranslator.FromHtml("#FFB230");
                if(Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    AoiSocketTeach.showPanel();
                }
                else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    EEpromSocketTeach.showPanel();
                }
                else
                {
                    //Fw는 모터 없음
                }
                
                
                LiftTeach.hidePanel();
                TransferTeach.hidePanel();
                MagazineTeach.hidePanel();
            }
        }
        private void TeachingControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                TeachingBtnChange(TeachCurrentTab);
            }

            


        }
        private void BTN_TEACH_PCB_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eTeachingBtn.TransferTab);
        }

        private void BTN_TEACH_LENS_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eTeachingBtn.MagazineTab);
        }

        private void BTN_TEACH_LIFT_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eTeachingBtn.LiftTab);
        }

        private void BTN_TEACH_SOCKET_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eTeachingBtn.SocketTab);
        }
    }
}
