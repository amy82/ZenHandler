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

namespace ZenHandler.Dlg
{
    public partial class MainControl : UserControl
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용
        //private ManualPcb manualPcb = new ManualPcb();
        //private ManualLens manualLens = new ManualLens();
        public UnitControl unitControl;

        private const int ResultGridRowViewCount = 25;//20;

        private const int ModelGridRowViewCount = 5;
        private const int RecipeGridRowViewCount = 5;

        private int[] GridColWidth = { 30, 160, 210, 70, 270, 50, 50, 1 };
        private int[] ResultGridColWidth = { 50, 200, 200, 200, 50, 50, 50 };

        private const int ResultGridColCount = 6;
        private string[] ResultTitle = { "Result", "EEP_ITEM", "ITEM_VALUE", "EEPROM", "ADDR", "SIZE" };
        private int RecipeGridWidth = 0;

        private int ResultGridRowHeight = 30;
        private int ResultGridHeaderHeight = 30;



        private enum eManualBtn : int
        {
            pcbTab = 0, lensTab
        };
        public MainControl(int _w, int _h)
        {
            InitializeComponent();

            unitControl = new UnitControl();
            this.Controls.Add(unitControl);

            unitControl.Location = new System.Drawing.Point(0, 50);
            this.Paint += new PaintEventHandler(Form_Paint);
            
            this.Width = _w;
            this.Height = _h;

            setInterface();
        }
        public void RefreshMain()
        {
            //ShowModelName();
            //ShowRecipeName();
        }
        private void MainControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                RefreshMain();
                unitControl.showPanel();
            }
            else
            {
                unitControl.hidePanel();
            }
        }


        
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            int lineStartY = ManualTitleLabel.Location.Y + Globalo.TabLineY;// 60;
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

            ManualTitleLabel.ForeColor = ColorTranslator.FromHtml("#6F6F6F");


 

            //ManualTitleLabel.Text = "MANUAL";
            //ManualTitleLabel.ForeColor = Color.Khaki;     
            //ManualTitleLabel.BackColor = Color.Maroon;
            //ManualTitleLabel.Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular);
            //ManualTitleLabel.Width = this.Width;
            //ManualTitleLabel.Height = 45;
            //ManualTitleLabel.Location = new Point(0, 0);



            //ManualPanel.Location = new Point(BTN_MANUAL_PCB.Location.X, BTN_MANUAL_PCB.Location.Y + panelYGap);



        }
        private void ManualBtnChange(eManualBtn index)
        {

            //if (index == eManualBtn.pcbTab)
            //{
            //    BTN_MANUAL_PCB.BackColor = ColorTranslator.FromHtml("#FFB230");
            //    manualPcb.Visible = true;
            //    manualLens.Visible = false;
            //}
            //else
            //{
            //    BTN_MANUAL_LENS.BackColor = ColorTranslator.FromHtml("#FFB230");
            //    manualLens.Visible = true;
            //    manualPcb.Visible = false;
            //}
        }
        private void BTN_MANUAL_PCB_Click(object sender, EventArgs e)
        {
            ManualBtnChange(eManualBtn.pcbTab);
        }

        private void BTN_MANUAL_LENS_Click(object sender, EventArgs e)
        {
            ManualBtnChange(eManualBtn.lensTab);
        }

        private void BTN_MAIN_MODEL_ADD_Click(object sender, EventArgs e)
        {
            //dataGridView_Model.Rows.Add("1", "model1");
            //System.Diagnostics.Process.Start("osk.exe");

            KeyBoardForm keyBoardForm = new KeyBoardForm();

            // 모달로 폼을 띄우고, 사용자가 OK를 클릭했을 때 KeyValue 값을 받음
            if (keyBoardForm.ShowDialog() == DialogResult.OK)
            {
                // KeyBoardForm에서 선택된 키 값을 받아옴
                //string selectedKey = keyBoardForm.KeyValue;
                //int addCount = Globalo.yamlManager.MesData.SecGemData.ModelData.Modellist.Count();
                //Globalo.yamlManager.MesData.SecGemData.ModelData.Modellist.Add(selectedKey);

                //Globalo.yamlManager.MesSave();

                //RefreshMain();
                //MessageBox.Show("선택된 키: " + selectedKey);
            }



            
        }

        private void BTN_MAIN_OPID_SAVE_Click(object sender, EventArgs e)
        {
            

        }

        private void BTN_MAIN_OFFLINE_REQ_Click(object sender, EventArgs e)
        {
            MessagePopUpForm messagePopUp3 = new MessagePopUpForm("", "YES", "NO");
            messagePopUp3.MessageSet(Globalo.eMessageName.M_ASK, "설비 오프라인 전환하시겠습니까?");

            DialogResult result = messagePopUp3.ShowDialog();
            if (result == DialogResult.Yes)
            {
                //Globalo.ubisamForm.RequestOfflineFn();
            }
        }

        private void BTN_MAIN_ONLINE_REMOTE_REQ_Click(object sender, EventArgs e)
        {
            MessagePopUpForm messagePopUp3 = new MessagePopUpForm("", "YES", "NO");
            messagePopUp3.MessageSet(Globalo.eMessageName.M_ASK, "설비 온라인 전환하시겠습니까?");

            DialogResult result = messagePopUp3.ShowDialog();
            if (result == DialogResult.Yes)
            {
                //Globalo.ubisamForm.RequestOnlineRemoteFn();
            }
        }



    }
}
