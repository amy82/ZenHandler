using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace ZenHandler.Dlg
{
    public partial class LogControl : UserControl
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용
        //private ManualPcb manualPcb = new ManualPcb();
        //private ManualLens manualLens = new ManualLens();

        private enum eManualBtn : int
        {
            pcbTab = 0, lensTab
        };
        public LogControl(int _w, int _h)
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(Form_Paint);
            
            this.Width = _w;
            this.Height = _h;


            setInterface();

        }
        public void RefreshLog()
        { 

        }
        
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            int lineStartY = ManualTitleLabel.Location.Y + Globalo.TabLineY;
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
        public void setInterface()
        {

            ManualTitleLabel.ForeColor = ColorTranslator.FromHtml("#6F6F6F");

            textBox_DumpFile.Text = Data.CPath.BASE_LOG_EEPROMDATA_PATH;
            textBox_EquipLog.Text = Data.CPath.BASE_LOG_EQUIP_PATH;
            textBox_SensorIni.Text = Data.CPath.SENSOR_INI_DIR;
        }
        private void AlarmControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                RefreshLog();
            }
            else
            {

            }
        }
        private void FolerOpen(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // 폴더를 탐색기에서 열기
                // Windows 탐색기를 통해 폴더 열기
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = path,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("폴더를 여는 중 오류가 발생했습니다: " + ex.Message);
            }
        }
        private void button_SensorIni_Click(object sender, EventArgs e)
        {
            FolerOpen(Data.CPath.SENSOR_INI_DIR);
        }

        private void BTN_LOG_DUMP_OPEN_Click(object sender, EventArgs e)
        {
            FolerOpen(Data.CPath.BASE_LOG_EEPROMDATA_PATH);
        }

        private void BTN_LOG_EQUIP_OPEN_Click(object sender, EventArgs e)
        {
            FolerOpen(Data.CPath.BASE_LOG_EQUIP_PATH);
        }
    }
}
