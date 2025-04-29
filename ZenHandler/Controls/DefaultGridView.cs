using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Controls
{
    public class DefaultGridView : DataGridView
    {
        private int[] inGridWid;       //GRID Cell Width

        private string PointFormat = "0.0###";
        private int dRowHeight = 26;
        //
        private string ColorSelecttGrid = "#E1E0DF";       //FFB230
        //
        //
        private int nGridRowCount = 0;              //Grid 총 Row / 세로 칸 수
        private int nGridColCount = 0;              //Grid 총 Col / 가로 칸 수

        public int SelectColIndex = -1; 
        //
        //
        public DefaultGridView(int _col , int _row , int[] _inGridWid)
        {
            nGridColCount = _col;
            nGridRowCount = _row;

            inGridWid = _inGridWid;

            InitializeGrid();


        }
       
        private void InitializeGrid()
        {
            //GRID
            int i = 0;
            int RowCount = nGridRowCount;

            int dGridHeight = RowCount * dRowHeight;
            int scrollWidth = 3;// 20;


            int dGridWidth = 0;
            for (i = 0; i < inGridWid.Length; i++)
            {
                dGridWidth += inGridWid[i];
            }

            this.ColumnCount = nGridColCount;
            this.EnableHeadersVisualStyles = false;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; //사이즈 조절 막기
            this.RowCount = nGridRowCount;
            this.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            this.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Yellow;
            this.ColumnHeadersDefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);
            //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;    //마우스 사이즈 조절 막기 Height
            //this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.AllowUserToResizeRows = false;
            this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            this.Name = "Grid";
            this.Size = new Size(dGridWidth + scrollWidth, dGridHeight + dRowHeight + 2);
            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            this.GridColor = Color.Black;
            this.RowHeadersVisible = false;
            //this.CellClick += TeachGrid_CellClick;
            //this.CellDoubleClick += TeachGrid_CellDoubleClick;


            for (i = 0; i < this.ColumnCount; i++)
            {
                this.Columns[i].Resizable = DataGridViewTriState.False;
                this.Columns[i].Width = inGridWid[i];
                this.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }


            this.ColumnHeadersHeight = dRowHeight;
            for (i = 0; i < nGridRowCount; i++)
            {
                this.Rows[i].Height = dRowHeight;
            }

            this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ReadOnly = true;
            this.CurrentCell = null;
            this.MultiSelect = false;
        }
    }

    
}
