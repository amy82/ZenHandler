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
    public partial class TabMenuForm : UserControl
    {
        public enum TABFORM
        {
            MAIN_FORM = 0, TEACH_FORM, MANUAL_FORM, IO_FORM, CONFIG_FORM, ALARM_FORM, LOG_FORM
        };
        //CCD_FORM, 
        public Button[] BtnArr = new Button[9];

        public bool MenuChangeInterLock = false;
        private int parentW = 0;
        private int parentH = 0;
        public TabMenuForm(int _w , int _h)
        {
            InitializeComponent();
            parentW = _w;
            parentH = _h;
            this.Height = _h;
            uiSet();


            Run_Mode_Change(ProgramState.eRunMode.ENGINEER);//OPERATOR);
            MenuButtonSet(TABFORM.MAIN_FORM);
        }
        private void MenuButtonSet(TABFORM index)
        {
            if (MenuChangeInterLock) return;        //모터 이동에 메뉴 이동안되는 용도로 일단 추가만

            int i = 0;
            Globalo.mMainPanel.Visible = false;
            Globalo.mTeachPanel.Visible = false;
            Globalo.mManualPanel.Visible = false;
            Globalo.mioPanel.Visible = false;
            Globalo.mCCdPanel.Visible = false;
            Globalo.mConfigPanel.Visible = false;
            Globalo.mAlarmPanel.Visible = false;
            Globalo.mlogControl.Visible = false;
            switch (index)
            {
                case TABFORM.MAIN_FORM:
                    Globalo.mMainPanel.Visible = true;
                    break;
                case TABFORM.TEACH_FORM:
                    Globalo.mTeachPanel.Visible = true;
                    break;
                case TABFORM.CONFIG_FORM:
                    Globalo.mConfigPanel.Visible = true;
                    break;
                case TABFORM.MANUAL_FORM:
                    Globalo.mManualPanel.Visible = true;
                    break;
                case TABFORM.IO_FORM:
                    Globalo.mioPanel.Visible = true;
                    break;
                case TABFORM.ALARM_FORM:
                    Globalo.mAlarmPanel.Visible = true;
                    break;
                case TABFORM.LOG_FORM:
                    Globalo.mlogControl.Visible = true;
                    break;
                default:
                    break;
            }


            for (i = 0; i < BtnArr.Length; i++)
            {
                BtnArr[i].BackColor = Color.Transparent;
                BtnArr[i].ForeColor = ColorTranslator.FromHtml("#979591");
            }
            BtnArr[(int)index].BackColor = ColorTranslator.FromHtml("#F8F3F0");
            BtnArr[(int)index].ForeColor = Color.DimGray;//ColorTranslator.FromHtml("#D7C1A6");
        }
        private void uiSet()
        {
            int i = 0;

            //-----------------------------------------------
            //우측 버튼 
            //-----------------------------------------------
            int BottomBtnHeight = 50;
            int BottomBtnStartX = 0;
            int BottomBtnHGap = 0;

            //MAIN_FORM = 0, TEACH_FORM, MANUAL_FORM, IO_FORM, CCD_FORM, CONFIG_FORM, ALARM_FORM, LOG_FORM
            //MAIN_FORM = 0, TEACH_FORM, MANUAL_FORM, IO_FORM, CONFIG_FORM, ALARM_FORM, LOG_FORM
            BtnArr[0] = BTN_BOTTOM_MAIN;
            BtnArr[1] = BTN_BOTTOM_TEACH;
            BtnArr[2] = BTN_BOTTOM_MANUAL;
            BtnArr[3] = BTN_BOTTOM_IO;
            BtnArr[4] = BTN_BOTTOM_SETUP;
            BtnArr[5] = BTN_BOTTOM_ALARM;
            BtnArr[6] = BTN_BOTTOM_LOG;
            //
            BtnArr[7] = BTN_BOTTOM_WALLPAPER;   //바탕화면
            BtnArr[8] = BTN_BOTTOM_EXIT;        //종료

            BTN_BOTTOM_CCD.Visible = false;
            BTN_BOTTOM_LIGHT.Visible = false;

            for (i = 0; i < BtnArr.Length; i++)
            {
                //BtnArr[i].Width = BottomBtnWidth;
                //BtnArr[i].Height = BottomBtnHeight;
                BtnArr[i].BackColor = Color.Transparent;
                BtnArr[i].ForeColor = ColorTranslator.FromHtml("#979591");


                //BtnArr[i].FlatStyle = FlatStyle.Flat;//FlatStyle.Popup;
                //BtnArr[i].Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);


                BtnArr[i].Location = new System.Drawing.Point(BottomBtnStartX, (BottomBtnHeight + BottomBtnHGap) * i);
                BtnArr[i].Width = parentW;
                BtnArr[i].Height = BottomBtnHeight;
            }

            

            //TIME SET
            //TimeLabel.ForeColor = Color.White;
            //TimeLabel.Font = new Font("Microsoft Sans Serif", 20, FontStyle.Regular);
            //TimeLabel.Location = new System.Drawing.Point(TopPanel.Width - 180, 10);
            TimeLabel.Width = parentW;
            TimeLabel.ForeColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
            TimeLabel.Location = new System.Drawing.Point(0, parentH - TimeLabel.Height - 10);

            DateLabel.Width = parentW;
            DateLabel.ForeColor = ColorTranslator.FromHtml(ButtonColor.BTN_ON);
            DateLabel.Location = new System.Drawing.Point(0, TimeLabel.Location.Y - DateLabel.Height - 1);


            //VERSION SET
            label_version.Location = new System.Drawing.Point(0, DateLabel.Location.Y - label_version.Height - 20);
            label_build.Location = new System.Drawing.Point(0, label_version.Location.Y - label_build.Height - 5);
            label_version.Text = "versionInfo : \n" + Program.VERSION_INFO;
            label_build.Text = "buildInfo : \n" + Program.BUILD_DATE;
            //
            //
            //RUN MODE CHANGE
            BTN_RIGHT_OP_MODE.Width = parentW;
            BTN_RIGHT_EN_MODE.Width = parentW;

            BTN_RIGHT_OP_MODE.Location = new System.Drawing.Point(0, label_build.Location.Y - BTN_RIGHT_OP_MODE.Height - 50);
            BTN_RIGHT_EN_MODE.Location = new System.Drawing.Point(0, BTN_RIGHT_OP_MODE.Location.Y - BTN_RIGHT_EN_MODE.Height - 2);
        }
        private void Run_Mode_Change(ProgramState.eRunMode runMode)
        {
            ProgramState.CurrentRunMode = runMode;
            if (ProgramState.CurrentRunMode == ProgramState.eRunMode.OPERATOR)
            {
                BTN_RIGHT_OP_MODE.BackColor = Color.Bisque;
                BTN_RIGHT_EN_MODE.BackColor = Color.Transparent;

                BTN_RIGHT_OP_MODE.ForeColor = Color.Black;
                BTN_RIGHT_EN_MODE.ForeColor = Color.Gray;

            }
            else
            {
                //BTN_RIGHT_EN_MODE
                BTN_RIGHT_EN_MODE.BackColor = Color.Bisque;
                BTN_RIGHT_OP_MODE.BackColor = Color.Transparent;

                BTN_RIGHT_EN_MODE.ForeColor = Color.Black;
                BTN_RIGHT_OP_MODE.ForeColor = Color.Gray;

            }
        }

        private void BTN_BOTTOM_MAIN_Click_1(object sender, EventArgs e)
        {
            MenuButtonSet(TABFORM.MAIN_FORM);
        }

        private void BTN_BOTTOM_CCD_Click_1(object sender, EventArgs e)
        {
            //MenuButtonSet(TABFORM.CCD_FORM);
        }
        private void BTN_BOTTOM_TEACH_Click_1(object sender, EventArgs e)
        {
            if (ProgramState.CurrentRunMode == ProgramState.eRunMode.OPERATOR)
            {
                //엔지니어 모드만 접근 가능합니다.
                Globalo.LogPrint("ManualControl", "[INFO] 엔지니어 모드만 접근 가능합니다.", Globalo.eMessageName.M_WARNING);
                return;
            }
            MenuButtonSet(TABFORM.TEACH_FORM);
        }

        private void BTN_BOTTOM_IO_Click_1(object sender, EventArgs e)
        {
            MenuButtonSet(TABFORM.IO_FORM);
        }
        private void BTN_BOTTOM_SETUP_Click_1(object sender, EventArgs e)
        {
            if (ProgramState.CurrentRunMode == ProgramState.eRunMode.OPERATOR)
            {
                //엔지니어 모드만 접근 가능합니다.
                Globalo.LogPrint("ManualControl", "[INFO] 엔지니어 모드만 접근 가능합니다.", Globalo.eMessageName.M_WARNING);
                return;
            }
            MenuButtonSet(TABFORM.CONFIG_FORM);
        }

        private void BTN_BOTTOM_ALARM_Click_1(object sender, EventArgs e)
        {
            MenuButtonSet(TABFORM.ALARM_FORM);
        }

        private void BTN_BOTTOM_LOG_Click_1(object sender, EventArgs e)
        {
            MenuButtonSet(TABFORM.LOG_FORM);
        }

        private void BTN_BOTTOM_WALLPAPER_Click_1(object sender, EventArgs e)
        {
            Globalo.MainForm.WindowState = FormWindowState.Minimized;
        }

        private void BTN_BOTTOM_EXIT_Click(object sender, EventArgs e)
        {
            MessagePopUpForm messagePopUp3 = new MessagePopUpForm("", "YES", "NO");
            messagePopUp3.MessageSet(Globalo.eMessageName.M_ASK, "프로그램 종료 하시겠습니까?");

            DialogResult result = messagePopUp3.ShowDialog();
            if (result == DialogResult.Yes)
            {
                Globalo.MainForm.FuncExit();
            }
        }

        private void BTN_RIGHT_EN_MODE_Click_1(object sender, EventArgs e)
        {
            //if (ProgramState.CurrentState == OperationState.AutoRunning)
            //{
            //    eLogPrint("MainForm", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
            //    return;
            //}
            //if (ProgramState.CurrentState == OperationState.Paused)
            //{
            //    eLogPrint("ManualCMainFormontrol", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
            //    return;
            //}

            InputForm passwordInputForm = new InputForm("PASSWORD INPUT", "비밀번호를 입력해주세요:", "", 1);
            Globalo.MainForm.keyMessageFilter.RegisterKeyHandler(passwordInputForm.Name.ToString(), passwordInputForm.OnInputFormCloseEvent);
            //Globalo.MainForm.keyMessageFilter.KeyEvent += passwordInputForm.OnInputFormCloseEvent;

            DialogResult result = passwordInputForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                Run_Mode_Change(ProgramState.eRunMode.ENGINEER);
            }
        }

        private void BTN_RIGHT_OP_MODE_Click_1(object sender, EventArgs e)
        {
            if (ProgramState.CurrentState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("MainForm", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (ProgramState.CurrentState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualCMainFormontrol", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Run_Mode_Change(ProgramState.eRunMode.OPERATOR);
        }

        private void BTN_BOTTOM_MANUAL_Click(object sender, EventArgs e)
        {
            MenuButtonSet(TABFORM.MANUAL_FORM);
        }
    }
}
