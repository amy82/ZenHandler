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
    public partial class ConfigControl : UserControl
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용
        public enum eConfigBtn : int
        {
            TaskTab = 0, OptionTab
        };
        private eConfigBtn ConfigCurrentTab;
        private Config_Task configTask;
        private Config_Option configOption;
        private int StartControlY = 50;


        public ConfigControl(int _w, int _h)
        {
            InitializeComponent();
            Event.EventManager.LanguageChanged += OnLanguageChanged;

            this.Paint += new PaintEventHandler(Form_Paint);
            
            this.Width = _w;
            this.Height = _h;

            configTask = new Config_Task();
            configOption = new Config_Option();
            ConfigCurrentTab = eConfigBtn.TaskTab;
            configTask.Visible = false;
            configOption.Visible = false;

            this.Controls.Add(configOption);
            this.Controls.Add(configTask);

            configOption.Location = new System.Drawing.Point(0, StartControlY);
            configTask.Location = new System.Drawing.Point(0, StartControlY);

            setInterface();

        }
        private void ConfigBtnChange(eConfigBtn index)
        {
            Btn_Config_Task.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            Btn_Config_Option.BackColor = ColorTranslator.FromHtml("#E1E0DF");

            ConfigCurrentTab = index;
            if (ConfigCurrentTab == eConfigBtn.TaskTab)
            {
                ConfigTitleLabel.Text = "| CONFIG - TASK";
                Btn_Config_Task.BackColor = ColorTranslator.FromHtml("#FFB230");
                configTask.showPanel();
                configOption.hidePanel();
            }
            if (ConfigCurrentTab == eConfigBtn.OptionTab)
            {
                ConfigTitleLabel.Text = "| CONFIG - OPTION";
                Btn_Config_Option.BackColor = ColorTranslator.FromHtml("#FFB230");
                configOption.showPanel();
                configTask.hidePanel();
            }
        }
        private void OnLanguageChanged(object sender, EventArgs e)
        {
            // 이벤트 처리
            Console.WriteLine("ConfigControl - OnLanguageChanged");
        }


        public void setInterface()
        {
            ConfigTitleLabel.ForeColor = ColorTranslator.FromHtml("#6F6F6F");

        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            int lineStartY = ConfigTitleLabel.Location.Y + Globalo.TabLineY;
            // Graphics 객체 가져오기
            Graphics g = e.Graphics;

            // Pen 객체 생성 (색상과 두께 설정)
            Color color = Color.FromArgb(175, 175, 175);//Color.FromArgb(151, 149, 145);
            Pen pen = new Pen(color, 1);

            // 라인 그리기 (시작점과 끝점 설정)
            g.DrawLine(pen, 0, lineStartY, this.Width, lineStartY);

            // 리소스 해제
            pen.Dispose();



           // Graphics g = this.CreateGraphics();
            // 지정된 펜츠로 폼에 사각형은 그립니다.
            //Pen pen1 = new Pen(Color.Red, 1);
            //Pen pen2 = new Pen(Color.Blue, 2);
            //Pen pen3 = new Pen(Color.Magenta, 10);

            //g.DrawLine(pen1, 10, 300, 100, 10);
            //g.DrawLine(pen2, new Point(10, 400), new Point(100, 400));
            //g.DrawLine(pen3, new Point(10, 500), new Point(150, 500));

            //pen1.Dispose();
            //pen2.Dispose();
            //pen3.Dispose();
        }
        

        private void ConfigControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                //RefreshConfig();
                ConfigBtnChange(ConfigCurrentTab);
            }
        }

        

        

        private void Btn_Config_Task_Click(object sender, EventArgs e)
        {
            ConfigBtnChange(eConfigBtn.TaskTab);
        }

        private void Btn_Config_Option_Click(object sender, EventArgs e)
        {
            ConfigBtnChange(eConfigBtn.OptionTab);
        }
    }
}
