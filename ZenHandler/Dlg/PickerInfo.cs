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
        int[] inGridWid = new int[] { 80, 230, 70 };         //Grid Width
        public PickerInfo()
        {
            InitializeComponent();
            InitializeGrid();
            InitializePicker();
        }

        public void InitializeGrid()
        {
            //GRID
            int i = 0;
            int LotCount = 3;// teachingData.Teaching.Count;
            int dGridHeight = LotCount * dRowHeight;
            int scrollWidth = 3;// 20;


            int dGridWidth = 0;
            for (i = 0; i < inGridWid.Length; i++)
            {
                dGridWidth += inGridWid[i];
            }

            nGridRowCount += LotCount;

            dataGridView1.ColumnCount = LotCount;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; //사이즈 조절 막기
            dataGridView1.RowCount = nGridRowCount;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Yellow;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);
            //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;    //마우스 사이즈 조절 막기 Height
            //this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dataGridView1.Name = "TransferTeachGrid";
            dataGridView1.Size = new Size(dGridWidth + scrollWidth, dGridHeight + dRowHeight + 2);
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Black;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.CellClick += TeachGrid_CellClick;
            //dataGridView1.CellDoubleClick += TeachGrid_CellDoubleClick;



            string[] title = new string[] { "Picker", "Lot", "State" };         //Grid Width
            for (i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].Name = title[i];
                dataGridView1.Columns[i].Resizable = DataGridViewTriState.False;
                dataGridView1.Columns[i].Width = inGridWid[i];
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }


            dataGridView1.ColumnHeadersHeight = dRowHeight;
            for (i = 0; i < nGridRowCount; i++)
            {
                dataGridView1.Rows[i].Height = dRowHeight;
            }

            //for (i = 0; i < MotionControl.MotorSet.TEACH_SET_MENU.Length; i++)
            //{
            //    dataGridView1.Rows[i].SetValues(MotionControl.MotorSet.TEACH_SET_MENU[i]);      //원전  , home ,limit 등
            //}

            //for (i = 0; i < nGridSensorRowCount; i++)
            //{
            //    //row header 선택 색 변화 금지
            //    dataGridView1.Rows[i].DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
            //    dataGridView1.Rows[i].DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;
            //}
            string posName = "";
            for (i = 0; i < LotCount; i++)
            {
                posName = "Load "+(i+1).ToString();// teachingData.Teaching[i].Name;

                dataGridView1.Rows[i].SetValues(posName);
            }


            //dataGridView1.Rows[nGridRowCount - 1].SetValues("현재위치");
            //dataGridView1[1, nGridRowCount - 1].Value = "0.0";
            //dataGridView1[2, nGridRowCount - 1].Value = "0.0";
            //dataGridView1[3, nGridRowCount - 1].Value = "0.0";

            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ReadOnly = true;
            dataGridView1.CurrentCell = null;
            dataGridView1.MultiSelect = false;
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
