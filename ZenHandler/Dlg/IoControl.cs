using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ZenHandler.Dlg
{
    public partial class IoControl : UserControl
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용

        //타이머
        System.Windows.Forms.Timer mIoTimer;
        int dGridWidth = 280;

        int dRowHeaderHeight = 30;
        int dRowHeight = 24;
        int dInGridHeight = 0;
        int dOutGridHeight = 0;

        int mOldInSelectedRow = -1;
        //int mOldOutSelectedRow = -1;
        uint[] m_dwPrevDIn = new uint[] { 0, 0, 0, 0, 0 };
        uint[] m_dwPrevDOut = new uint[] { 0, 0, 0, 0, 0 };

        int MaxInModuleCount;
        int MaxOutModuleCount;
        public List<int> InModuleChannel = new List<int>();
        public List<int> OutModuleChannel = new List<int>();
        //
        private int dCurReadModuleCh = 0;
        private int dCurOutModuleCh = 0;

        public IoControl(int _w , int _h)
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(Form_Paint);

            this.Width = _w;
            this.Height = _h;


            //MaxInModuleCount = Globalo.motionManager.ioController.m_dwDInDict.Count;
            //Console.WriteLine($"In 모듈 개수: {MaxInModuleCount}");
            //foreach (var entry in Globalo.motionManager.ioController.m_dwDInDict)
            //{
            //    InModuleChannel.Add(entry.Key);
            //    //Console.WriteLine($"Key: {entry.Key}, 배열 크기: {entry.Value.Length}");
            //}

            ////MaxOutModuleCount = Globalo.motionManager.ioController.m_dwDOutDict.Count;
            //foreach (var entry in Globalo.motionManager.ioController.m_dwDOutDict)
            //{
            //    OutModuleChannel.Add(entry.Key);
            //}

            
            setInterface();
        }
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            int lineStartY = DioTitleLabel.Location.Y + 60;
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

            DioTitleLabel.ForeColor = ColorTranslator.FromHtml("#6F6F6F");

            //IOTitleLabel.Text = "IO";
            //IOTitleLabel.ForeColor = Color.Khaki;
            //IOTitleLabel.BackColor = Color.Maroon;
            //IOTitleLabel.Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular);
            //IOTitleLabel.Width = this.Width;
            //IOTitleLabel.Height = 45;
            //IOTitleLabel.Location = new Point(0, 0);

            ControlSet();
            InGridDraw();
            OutGridDraw();
            ButtonEventSet();

            mIoTimer = new System.Windows.Forms.Timer();
            mIoTimer.Interval = 100;
            mIoTimer.Tick += new EventHandler(update);
            //mIoTimer.Start();
            //Thread Start
            //Thread t1 = new Thread(new ThreadStart(update));
            //t1.Start();
        }
        private void IoDisplay(object sender, System.EventArgs e)
        {

        }
        private void IoControl_Load(object sender, EventArgs e)
        {
            mIoTimer.Start();
        }
        private void IoControl_VisibleChanged(object sender, EventArgs e)
        {
            //사라질때 이벤트
            if (this.Visible == false)
            {
                mIoTimer.Stop();
            }
            else
            {

            }
            
        }

        public void ControlSet()
        {
            BTN_IO_IN_PREV.Text = "◀";
            BTN_IO_IN_NEXT.Text = "▶";
            BTN_IO_OUT_PREV.Text = "◀";
            BTN_IO_OUT_NEXT.Text = "▶";
            //MODULE NAME
            //this.Controls.Add(inTxt);
            //inTxt.Location = new Point(0, 8);
            //inTxt.Size = new Size(150, 30);
            //BUTTON
            //this.Controls.Add(mInLeftButton);
            //this.Controls.Add(mInRightButton);
            //this.Controls.Add(inIndexTxt);

            //mInLeftButton.Location = new Point(0, 50);
            //mInRightButton.Location = new Point(bBtnWidth + 10, 50);
            //mInLeftButton.Size = new Size(bBtnWidth, bBtnHeight);
            //mInRightButton.Size = new Size(bBtnWidth, bBtnHeight);



            //inIndexTxt.Location = new Point(0, 120);

            //OUT MODULE NAME
            //this.Controls.Add(OutTxt);
            //OutTxt.Location = new Point(dButtonGap + dGridStartX + dGridGap + dGridWidth * 2, 8);
            //OutTxt.Size = new Size(150, 30);
            //OutTxt.Focus();// = false;
            //BUTTON
            //this.Controls.Add(mOutLeftButton);
            //this.Controls.Add(mOutRightButton);
            //this.Controls.Add(OutIndexTxt);
            //mOutLeftButton.Location = new Point(dButtonGap + dGridStartX + dGridGap + dGridWidth * 2, 50);
            //mOutRightButton.Location = new Point(dButtonGap + dGridStartX + dGridGap + dGridWidth * 2 + bBtnWidth + 10, 50);
            //mOutLeftButton.Size = new Size(bBtnWidth, bBtnHeight);
            //mOutRightButton.Size = new Size(bBtnWidth, bBtnHeight);


            //OutIndexTxt.Location = new Point(dButtonGap + dGridStartX + dGridGap + dGridWidth * 2, 120);
        }
        public void ButtonEventSet()
        {
            //버튼 이벤트 설정
            //
            //BTN_IO_IN_PREV.Name = "InL";
            //BTN_IO_IN_NEXT.Name = "InR";
            //BTN_IO_IN_PREV.Click += BtnClick_ModuleBtn;
            //BTN_IO_IN_NEXT.Click += BtnClick_ModuleBtn;

            //BTN_IO_OUT_PREV.Name = "OutL";
            //BTN_IO_OUT_NEXT.Name = "OutR";
            //BTN_IO_OUT_PREV.Click += BtnClick_ModuleBtn;
            //BTN_IO_OUT_NEXT.Click += BtnClick_ModuleBtn;
        }
        
        void OutGridSelectionChanged(object sender, EventArgs e)
        {
            if (OutDataGridView.CurrentCell != null)
            {
                if (OutDataGridView.CurrentCell.RowIndex < 0)
                {
                    //OutDataGridView.CurrentCell.Selected = false;
                    return;
                }
                //OutDataGridView.CurrentCell.Selected = false;


                if (mOldInSelectedRow > -1)
                {
                    OutDataGridView.Rows[mOldInSelectedRow].Cells[0].Style.SelectionBackColor = Color.White;
                    OutDataGridView.Rows[mOldInSelectedRow].Cells[1].Style.SelectionBackColor = Color.White;
                }
                OutDataGridView.Rows[OutDataGridView.CurrentCell.RowIndex].Cells[0].Style.SelectionBackColor = Color.SkyBlue;
                OutDataGridView.Rows[OutDataGridView.CurrentCell.RowIndex].Cells[1].Style.SelectionBackColor = Color.SkyBlue;


                mOldInSelectedRow = OutDataGridView.CurrentCell.RowIndex;
                //OutDataGridView[2, OutDataGridView.CurrentCell.RowIndex].Selected = false;
            }
        }
        void InGridSelectionChanged(object sender, EventArgs e)
        {
            if (InDataGridView.CurrentCell != null)
            {
                if (InDataGridView.CurrentCell.RowIndex < 0)
                {
                    //InDataGridView.CurrentCell.Selected = false;
                    return;
                }
                //InDataGridView.CurrentCell.Selected = false;
                if(mOldInSelectedRow > -1)
                {
                    InDataGridView.Rows[mOldInSelectedRow].Cells[0].Style.SelectionBackColor = Color.White;
                    InDataGridView.Rows[mOldInSelectedRow].Cells[1].Style.SelectionBackColor = Color.White;
                }
                InDataGridView.Rows[InDataGridView.CurrentCell.RowIndex].Cells[0].Style.SelectionForeColor = Color.Black;
                InDataGridView.Rows[InDataGridView.CurrentCell.RowIndex].Cells[1].Style.SelectionForeColor = Color.Black;
                InDataGridView.Rows[InDataGridView.CurrentCell.RowIndex].Cells[0].Style.SelectionBackColor = Color.SkyBlue;
                InDataGridView.Rows[InDataGridView.CurrentCell.RowIndex].Cells[1].Style.SelectionBackColor = Color.SkyBlue;


                mOldInSelectedRow = InDataGridView.CurrentCell.RowIndex;
                //songsDataGridView[2, songsDataGridView.CurrentCell.RowIndex].Selected = false;
            }
        }
        public void InGridDraw()
        {
            //GRID
            int i = 0;
            //InDataGridView.SelectionChanged += new EventHandler(InGridSelectionChanged);
            int[] inGridWid = new int[]{ 40, 260, 60 };
            //this.Controls.Add(InDataGridView);
            dGridWidth = inGridWid[0] + inGridWid[1] + inGridWid[2];
            

            InDataGridView.EnableHeadersVisualStyles = false;
            InDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;    //마우스 사이즈 조절 막기 Height
            InDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            InDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            InDataGridView.AllowUserToResizeRows = false;
            InDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            InDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            InDataGridView.GridColor = Color.Black;
            InDataGridView.RowHeadersVisible = false;
            InDataGridView.ReadOnly = true;
            //InDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            InDataGridView.MultiSelect = false;

            InDataGridView.ColumnCount = 3;
            InDataGridView.RowCount = 32;
            dInGridHeight = (InDataGridView.RowCount + 0) * dRowHeight + dRowHeaderHeight + 2;
            InDataGridView.Size = new Size(dGridWidth + 3, dInGridHeight);
            //InDataGridView.Location = new Point(dGridStartX, dGridStartY);
            InDataGridView.Name = "InDataGridView";

            //Color Set
            //
            //InDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Crimson;
            InDataGridView.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E1E0DF");

            InDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            InDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(InDataGridView.Font, FontStyle.Bold);
            InDataGridView.Columns[0].Name = "NO";
            InDataGridView.Columns[1].Name = "IN IO NAME";
            InDataGridView.Columns[2].Name = "SIGNAL";


            InDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            InGridContentChange(dCurReadModuleCh);
            for (i = 0; i < 3; i++)
            {
                InDataGridView.Columns[i].Resizable = DataGridViewTriState.False;
                InDataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                InDataGridView.Columns[i].Width = inGridWid[i];
                //    //songsDataGridView.Rows.Add((i + 1).ToString(), "", "");
                //    string kkk = DataTable.Rows[i][0].ToString();
                //    songsDataGridView.Rows[i].SetValues((i + 1).ToString(), kkk, "");
                //    songsDataGridView.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            InDataGridView.ColumnHeadersHeight = dRowHeaderHeight;// dRowHeight;
            //InDataGridView.RowTemplate.Height = dRowHeaderHeight;
            for (i = 0; i < InDataGridView.RowCount; i++)
            {
                InDataGridView.Rows[i].Height = dRowHeight;

                InDataGridView[0, i].Style.BackColor = Color.White;//.Crimson;   //배경색
                InDataGridView[0, i].Style.ForeColor = Color.Black ;     //글자색
            }
            InDataGridView.CurrentCell = null;

        }
        public void OutGridDraw()
        {
            int i = 0;
            //GIRD
            //OutDataGridView.SelectionChanged += new EventHandler(OutGridSelectionChanged);
            int[] outGridWid = new int[] { 40, 260, 60 };
            OutDataGridView.ColumnCount = 3;
            OutDataGridView.RowCount = 32;

            dOutGridHeight = (InDataGridView.RowCount + 0) * dRowHeight + dRowHeaderHeight + 2;
            //this.Controls.Add(OutDataGridView);
            //CLICK EVENT
            OutDataGridView.CellClick += Click_Grid;
            //
            dGridWidth = outGridWid[0] + outGridWid[1] + outGridWid[2];
            OutDataGridView.Name = "OutDataGridView";
            //OutDataGridView.Location = new Point(dGridStartX + dGridGap + dGridWidth, dGridStartY);

            OutDataGridView.Size = new Size(dGridWidth + 3, dOutGridHeight);
            OutDataGridView.EnableHeadersVisualStyles = false;
            OutDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; //사이즈 조절 막기
            OutDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            OutDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            OutDataGridView.AllowUserToResizeRows = false;
            OutDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            OutDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            OutDataGridView.GridColor = Color.Black;
            OutDataGridView.RowHeadersVisible = false;
            OutDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            OutDataGridView.ReadOnly = true;
            OutDataGridView.MultiSelect = false;


            OutDataGridView.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E1E0DF"); //Color.Navy;
            OutDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            OutDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(OutDataGridView.Font, FontStyle.Bold);

            OutDataGridView.Columns[0].Name = "NO";
            OutDataGridView.Columns[1].Name = "OUT IO NAME";
            OutDataGridView.Columns[2].Name = "Signal";

            OutGridContentChange(dCurOutModuleCh);

            for (i = 0; i < 3; i++)
            {
                OutDataGridView.Columns[i].Width = outGridWid[i];
                OutDataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                OutDataGridView.Columns[i].Resizable = DataGridViewTriState.False;
                //    string kkk = DataTable.Rows[i][1].ToString();
                //    //OutDataGridView.Rows.Add((i + 1).ToString(), "", "");
                //    OutDataGridView.Rows[i].SetValues((i + 1).ToString(), kkk, "");
                //    OutDataGridView.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            OutDataGridView.ColumnHeadersHeight = dRowHeaderHeight;
            for (i = 0; i < OutDataGridView.RowCount; i++)
            {
                OutDataGridView.Rows[i].Height = dRowHeight;
                OutDataGridView[0, i].Style.BackColor = Color.White;   //배경색
                OutDataGridView[0, i].Style.ForeColor = Color.Black;     //글자색
            }


            OutDataGridView.CurrentCell = null;
            

        }
        public void update(object sender , System.EventArgs e)
        {
            //if (Globalo.motorControl.bConnected == false)
            if (Globalo.motionManager.bConnected == false)
            {
                return;
            }
            uint uFlagHigh = 0;
            int nIndex = 0;


            //AxdiReadInportDword
            //uint upIn = Globalo.dIoControl.m_dwDIn[Globalo.dIoControl.dCurReadModuleCh];

            int InCh = InModuleChannel[dCurReadModuleCh];
            

            uint upIn = Globalo.motionManager.ioController.m_dwDInDict[InCh][0];


            
            if (upIn != m_dwPrevDIn[InCh])
            {
                m_dwPrevDIn[InCh] = upIn;
                for (nIndex = 0; nIndex < 32; nIndex++)
                {
                    uFlagHigh = upIn & 0x01;
                    if (uFlagHigh == 1)
                    {
                        // songsDataGridView.Columns[0].
                        // oGlobal.Maindata.songsDataGridView.Rows[nIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                        InDataGridView[2, nIndex].Style.BackColor = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        InDataGridView[2, nIndex].Style.BackColor = System.Drawing.Color.White;
                    }
                    upIn = upIn >> 1;
                }
            }
            //OUTPUT
           // CAXD.AxdoReadOutportDword(0, 0, ref oGlobal.mDioControl.m_dwDIn[oGlobal.Maindata.dCurOutModuleCh]);


            int i = 0;
            //int moutIndex = 0;
            //uint mtempOut = 0;
            int OutCh = OutModuleChannel[dCurOutModuleCh];
            //for (i = 0; i < 4; i++)
            for (i = 0; i < Globalo.motionManager.ioController.m_dwDOutDict[OutCh].Length; i++)
            {
                //CAXD.AxdoReadOutportByte(1, i, ref Globalo.dIoControl.m_dwDOut[Globalo.dIoControl.dCurReadModuleCh, i]);
                // uint upOut = Globalo.dIoControl.m_dwDOut[Globalo.dIoControl.dCurOutModuleCh, i];
                
                uint upOut = Globalo.motionManager.ioController.m_dwDOutDict[OutCh][i];

                if (upOut != m_dwPrevDOut[i])
                {
                    m_dwPrevDOut[i] = upOut;
                    for (nIndex = 0; nIndex < 8; nIndex++)
                    {
                        int nRow = (nIndex + 0);
                        if ((upOut & 0x01) == 0x01)//if (uFlagHigh == 1)
                        {
                            OutDataGridView[2, (nRow + (i * 8))].Style.BackColor = System.Drawing.Color.Yellow;
                        }
                        else
                        {
                            OutDataGridView[2, (nRow + (i * 8))].Style.BackColor = System.Drawing.Color.White;
                        }
                        upOut = upOut >> 1;

                    }
                }
                Thread.Sleep(10);
            }
        }
        public void Click_Grid(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1)
            {
                return;
            }
            //if (Globalo.motorControl.bConnected == false)

            if (Globalo.motionManager.bConnected == false)
            {
                //MessageBox.Show("not Connected.");
                return;
            }
            
            DataGridView Grid = (DataGridView)sender;
            DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)Grid.Rows[e.RowIndex].Cells[e.ColumnIndex];

            int mOffset = (e.RowIndex) / 8;
            int outIndex = (e.RowIndex) % 8;

            uint dwPivot =  0x01;//0x00000001;

            int OutCh = OutModuleChannel[dCurOutModuleCh];
            //uint upOut = Globalo.motionManager.m_dwDOutDict[OutCh][i];
            //uint dOut = Globalo.dIoControl.m_dwDOut[Globalo.dIoControl.dCurOutModuleCh , mOffset];
            int subCnt = Globalo.motionManager.ioController.m_dwDOutDict[OutCh].Length;
            if(mOffset >= subCnt)
            {
                return;     //16채널짜리라서 0,1만 offset 접근 가능
            }

            uint dOut = Globalo.motionManager.ioController.m_dwDOutDict[OutCh][mOffset];

            if (Grid.Name == "OutDataGridView")
            {
                //OUT IO
                dwPivot = dwPivot << outIndex;// (e.RowIndex);
                if ((dOut & dwPivot) == dwPivot)
                {
                    dOut &= ~dwPivot;
                }
                else
                {
                    dOut |= dwPivot;
                }
                //CAXD.AxdoWriteOutportByte(1, mOffset, dOut);
                CAXD.AxdoWriteOutportByte(OutCh, mOffset, dOut);
                //Globalo.dIoControl.m_dwDOut[Globalo.dIoControl.dCurOutModuleCh , mOffset] = dOut;

                Globalo.motionManager.ioController.m_dwDOutDict[OutCh][mOffset] = dOut;
            }
        }
        public void InGridContentChange(int index)
        {
            for (int i = 0; i < 32; i++)
            {
                //string str = Globalo.dataManage.ioData.dataTable.Rows[i + (32 * index)][0].ToString();
                //InDataGridView.Rows[i].SetValues((i + 1).ToString(), str, "");
                //InDataGridView.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
        public void OutGridContentChange(int index)
        {
            for (int i = 0; i < 32; i++)
            {
                //string str = Globalo.dataManage.ioData.dataTable.Rows[i + (32 * index)][1].ToString();
                //OutDataGridView.Rows[i].SetValues((i + 1).ToString(), str, "");
                //OutDataGridView.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            }
        }

        private void BTN_IO_IN_PREV_Click(object sender, EventArgs e)
        {
            if (dCurReadModuleCh > 0)
            {
                dCurReadModuleCh--;
            }
            //InIndexLabel.Text = (Globalo.dIoControl.dCurReadModuleCh+1).ToString() + " / " + Globalo.dIoControl.nInModuleCount.ToString();
            InIndexLabel.Text = (dCurReadModuleCh+1).ToString() + " / " + MaxInModuleCount.ToString();
            InGridContentChange(dCurReadModuleCh);
        }

        private void BTN_IO_IN_NEXT_Click(object sender, EventArgs e)
        {
            //if (Globalo.dIoControl.dCurReadModuleCh < Globalo.dIoControl.nInModuleCount - 1)
            if (dCurReadModuleCh < MaxInModuleCount - 1)
            {
                dCurReadModuleCh++;
            }

            //InIndexLabel.Text = (Globalo.dIoControl.dCurReadModuleCh+1).ToString() + " / " + Globalo.dIoControl.nInModuleCount.ToString();
            InIndexLabel.Text = (dCurReadModuleCh+1).ToString() + " / " + MaxInModuleCount.ToString();
            //
            InGridContentChange(dCurReadModuleCh);
        }

        private void BTN_IO_OUT_PREV_Click(object sender, EventArgs e)
        {
            if (dCurOutModuleCh > 0)
            {
                dCurOutModuleCh--;
            }

            //OutIndexLabel.Text = (Globalo.dIoControl.dCurOutModuleCh + 1).ToString() + " / " + Globalo.dIoControl.nOutModuleCount.ToString();
            OutIndexLabel.Text = (dCurOutModuleCh + 1).ToString() + " / " + MaxOutModuleCount.ToString();
            OutGridContentChange(dCurOutModuleCh);

        }

        private void BTN_IO_OUT_NEXT_Click(object sender, EventArgs e)
        {
            //if (Globalo.dIoControl.dCurOutModuleCh < Globalo.dIoControl.nOutModuleCount - 1)
            if (dCurOutModuleCh < MaxOutModuleCount - 1)
            {
                dCurOutModuleCh++;
            }
            //OutIndexLabel.Text = (Globalo.dIoControl.dCurOutModuleCh + 1).ToString() + " / " + Globalo.dIoControl.nOutModuleCount.ToString();
            OutIndexLabel.Text = (dCurOutModuleCh + 1).ToString() + " / " + MaxOutModuleCount.ToString();
            OutGridContentChange(dCurOutModuleCh);
        }
    }
}
