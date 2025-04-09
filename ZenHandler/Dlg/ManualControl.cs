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
        public event delLogSender eLogSender;       //외부에서 호출할때 사용
        private eTeachingBtn TeachCurrentTab;

        private enum eTeachingBtn : int
        {
            pcbTab = 0, lensTab
        };

        public ManualControl(int _w , int _h)
        {
            InitializeComponent();

            //teachingPcb = new TeachingPcb();
            //teachingLens = new TeachingLens();
            this.Paint += new PaintEventHandler(Form_Paint);


            this.Width = _w;
            this.Height = _h;


            //teachingPcb.Visible = false;
            //teachingLens.Visible = false;
            //TeachingPanel.Controls.Add(teachingPcb);
            //TeachingPanel.Controls.Add(teachingLens);

            setInterface();


            TeachingBtnChange(eTeachingBtn.pcbTab);
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

            BTN_TEACH_PCB.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            BTN_TEACH_LENS.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");

        }
        private void TeachingBtnChange(eTeachingBtn index)
        {
            BTN_TEACH_PCB.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            BTN_TEACH_LENS.BackColor = ColorTranslator.FromHtml("#E1E0DF");

            TeachCurrentTab = index;

            if (TeachCurrentTab == eTeachingBtn.pcbTab)
            {
                BTN_TEACH_PCB.BackColor = ColorTranslator.FromHtml("#FFB230");
                //teachingPcb.Visible = true;
                //teachingLens.Visible = false;

                //teachingLens.hidePanel();
                //teachingPcb.showPanel();
            }
            else
            {
                BTN_TEACH_LENS.BackColor = ColorTranslator.FromHtml("#FFB230");
                //teachingLens.Visible = true;
                //teachingPcb.Visible = false;

                //teachingPcb.hidePanel();
                //teachingLens.showPanel();
            }
        }
        private void BTN_TEACH_PCB_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eTeachingBtn.pcbTab);
        }

        private void BTN_TEACH_LENS_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eTeachingBtn.lensTab);
        }
    }
}
