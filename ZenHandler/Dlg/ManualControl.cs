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
    public partial class ManualControl : UserControl
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용
        private eManualBtn manualBtnTab;
        private ManualTransfer manualTransfer;
        private ManualMagazine manualMagazine;
        private ManualLift manualLift;

        private ManualFwSocket manualFwSocket;
        private ManualEEpromSocket manualEEpromSocket;
        private ManualAoiSocket manualAoiSocket;

        public enum eManualBtn : int
        {
            TransferTab = 0, MagazineTab, LiftTab, SocketTab
        };
        public ManualControl(int _w , int _h)
        {
            InitializeComponent();

            manualTransfer = new ManualTransfer();
            manualMagazine = new ManualMagazine();
            manualLift = new ManualLift();
            manualAoiSocket = new ManualAoiSocket();
            manualEEpromSocket = new ManualEEpromSocket();
            manualFwSocket = new ManualFwSocket();

            //teachingLens = new TeachingLens();
            this.Paint += new PaintEventHandler(Form_Paint);


            this.Width = _w;
            this.Height = _h;


            manualTransfer.Visible = false;
            manualMagazine.Visible = false;
            manualLift.Visible = false;
            manualAoiSocket.Visible = false;
            manualEEpromSocket.Visible = false;
            manualFwSocket.Visible = false;

            this.Controls.Add(manualTransfer);
            this.Controls.Add(manualMagazine);
            this.Controls.Add(manualLift);
            this.Controls.Add(manualAoiSocket);
            this.Controls.Add(manualEEpromSocket);
            this.Controls.Add(manualFwSocket);

            manualTransfer.Location = new System.Drawing.Point(0, 89);
            manualMagazine.Location = new System.Drawing.Point(0, 89);
            manualLift.Location = new System.Drawing.Point(0, 89);
            manualAoiSocket.Location = new System.Drawing.Point(0, 89);
            manualEEpromSocket.Location = new System.Drawing.Point(0, 89);
            manualFwSocket.Location = new System.Drawing.Point(0, 89);


            setInterface();

            manualBtnTab = eManualBtn.TransferTab;
            //TeachingBtnChange(manualBtnTab);
        }
        public void ManualDlgStop()
        {
            manualTransfer.bManualStopKey = true;       //수동 모터 이동 중 빠져나오게
            manualMagazine.bManualStopKey = true;       //수동 모터 이동 중 빠져나오게
            manualLift.bManualStopKey = true;       //수동 모터 이동 중 빠져나오게

            //manualSocket.bManualStopKey = true;       //수동 모터 이동 중 빠져나오게
            //manualLift.bManualStopKey = true;       //수동 모터 이동 중 빠져나오게
            //manualMagazine.bManualStopKey = true;       //수동 모터 이동 중 빠져나오게
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
            button_Manual_Magazine.Visible = false;
            TeachingTitleLabel.ForeColor = ColorTranslator.FromHtml("#6F6F6F");

            button_Manual_Transfer.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            button_Manual_Magazine.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            button_Manual_Lift.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            button_Manual_Socket.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");

            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                button_Manual_Lift.Text = "MAGAZINE";
            }
            else
            {
                button_Manual_Lift.Text = "LIFT";
            }
            //

        }
        private void TeachingBtnChange(eManualBtn index)
        {
            button_Manual_Transfer.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            button_Manual_Magazine.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            button_Manual_Lift.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            button_Manual_Socket.BackColor = ColorTranslator.FromHtml("#E1E0DF");

            manualBtnTab = index;

            if (manualBtnTab == eManualBtn.TransferTab)
            {
                button_Manual_Transfer.BackColor = ColorTranslator.FromHtml("#FFB230");
                manualTransfer.Visible = true;
                manualMagazine.Visible = false;
                manualLift.Visible = false;
                manualFwSocket.Visible = false;
                manualEEpromSocket.Visible = false;
                manualAoiSocket.Visible = false;

                manualMagazine.hidePanel();
                manualLift.hidePanel();
                manualFwSocket.hidePanel();
                manualEEpromSocket.hidePanel();
                manualAoiSocket.hidePanel();
                manualTransfer.showPanel();
            }

            else if (manualBtnTab == eManualBtn.MagazineTab)
            {
                button_Manual_Magazine.BackColor = ColorTranslator.FromHtml("#FFB230");
                manualMagazine.Visible = true;
                manualTransfer.Visible = false;
                manualLift.Visible = false;
                manualFwSocket.Visible = false;
                manualEEpromSocket.Visible = false;
                manualAoiSocket.Visible = false;

                manualTransfer.hidePanel();
                manualLift.hidePanel();
                manualFwSocket.hidePanel();
                manualEEpromSocket.hidePanel();
                manualAoiSocket.hidePanel();
                manualMagazine.showPanel();
            }
            else if (manualBtnTab == eManualBtn.LiftTab)
            {
                button_Manual_Lift.BackColor = ColorTranslator.FromHtml("#FFB230");
                manualLift.Visible = true;
                manualTransfer.Visible = false;
                manualMagazine.Visible = false;
                manualFwSocket.Visible = false;
                manualEEpromSocket.Visible = false;
                manualAoiSocket.Visible = false;

                manualFwSocket.hidePanel();
                manualEEpromSocket.hidePanel();
                manualAoiSocket.hidePanel();
                manualTransfer.hidePanel();
                manualMagazine.hidePanel();
                manualLift.showPanel();
            }
            else if (manualBtnTab == eManualBtn.SocketTab)
            {
                button_Manual_Socket.BackColor = ColorTranslator.FromHtml("#FFB230");
                manualLift.Visible = false;
                manualTransfer.Visible = false;
                manualMagazine.Visible = false;
                manualAoiSocket.Visible = false;
                manualEEpromSocket.Visible = false;
                manualFwSocket.Visible = false;

                if (Program.PG_SELECT == HANDLER_PG.FW)
                {
                    manualFwSocket.Visible = true;
                    manualFwSocket.showPanel();
                }
                if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    manualEEpromSocket.Visible = true;
                    manualEEpromSocket.showPanel();
                }
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    manualAoiSocket.Visible = true;
                    manualAoiSocket.showPanel();
                }

                manualTransfer.hidePanel();
                manualMagazine.hidePanel();
                manualLift.hidePanel();
            }
            else
            {
                button_Manual_Magazine.BackColor = ColorTranslator.FromHtml("#FFB230");
                manualTransfer.Visible = false;
                manualMagazine.Visible = false;
                manualLift.Visible = false;

                manualTransfer.hidePanel();
                manualMagazine.hidePanel();
                manualLift.hidePanel();
            }
        }
        private void BTN_TEACH_PCB_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eManualBtn.TransferTab);
        }
        private void BTN_TEACH_LENS_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eManualBtn.MagazineTab);
        }
        private void button_Manual_Lift_Click(object sender, EventArgs e)
        {
            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                TeachingBtnChange(eManualBtn.MagazineTab);
            }
            else
            {
                TeachingBtnChange(eManualBtn.LiftTab);
            }
            
        }
        private void button_Manual_Socket_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eManualBtn.SocketTab);
        }
        private void ManualControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                TeachingBtnChange(manualBtnTab);
            }
            else
            {
                manualTransfer.Visible = false;
                manualMagazine.Visible = false;
                manualLift.Visible = false;
                manualFwSocket.Visible = false;
                manualEEpromSocket.Visible = false;
                manualEEpromSocket.Visible = false;
                manualAoiSocket.Visible = false;


                manualTransfer.hidePanel();
                manualMagazine.hidePanel();
                manualLift.hidePanel();
                manualFwSocket.hidePanel();
                manualEEpromSocket.hidePanel();
                manualAoiSocket.hidePanel();
            }
        }

        
    }
}
