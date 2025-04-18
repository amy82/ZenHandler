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
    public partial class PickerInfo : UserControl
    {
        private int dRowHeight = 26;
        private int nGridRowCount = 0;              //Grid 총 Row / 세로 칸 수

        int PickerCount = 8;   //Load Picket 4ea + UnLoad 4ea
        int[] inGridWid = new int[] { 80, 250, 70 };         //Grid Width

        private Controls.DefaultGridView dataGridView;

        public PickerInfo()
        {
            InitializeComponent();


            SetGrid();
            InitializePicker();
        }
        public void SetGrid()
        {
            int i = 0;
            dataGridView = new Controls.DefaultGridView(3, 8, inGridWid);
            dataGridView.Location = new Point(10, 40);
            this.Controls.Add(dataGridView);

            string[] title = new string[] { "Picker", "Lot", "State" };         //Grid Width
            for (i = 0; i < dataGridView.ColumnCount; i++)
            {
                dataGridView.Columns[i].Name = title[i];
            }
            string posName = "";
            for (i = 0; i < PickerCount; i++)
            {
                if (i < 4)
                {
                    posName = "Load " + (i + 1).ToString();
                }
                else
                {
                    posName = "UnLoad " + (i + 1).ToString();
                }


                dataGridView.Rows[i].SetValues(posName);
            }


        }


        public void InitializePicker()
        {
            //dataGridView1.ColumnCount = 3;
            //dataGridView1.Columns[0].Name = "PICKER";
            //dataGridView1.Columns[1].Name = "BCR";
            //dataGridView1.Columns[2].Name = "STATE";

            //// 줄 추가
            //dataGridView1.Rows.Add("Picker1", "BCR1", "BeforeInspection");
            //dataGridView1.Rows.Add("Picker2", "BCR2", "Inspecting");
            //dataGridView1.Rows.Add("Picker3", "BCR3", "Inspecting");
            //dataGridView1.Rows.Add("Picker4", "BCR4", "Inspecting");
            //// 스타일 변경
            //dataGridView1.EnableHeadersVisualStyles = false;
            //dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            //dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }
    }
}
