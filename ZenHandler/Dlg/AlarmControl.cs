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
    public partial class AlarmControl : UserControl
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용
        //private ManualPcb manualPcb = new ManualPcb();
        //private ManualLens manualLens = new ManualLens();

        private const int AlarmGridRowViewCount = 25;       //MAX ALARM COUNT

        private int CurrentAlarmPage;       //현재 알람 페이지
        private int TotalAlarmPage;           //총 알람 페이지 수

        private int[] GridColWidth = { 50, 150, 210, 70, 270, 50, 50, 1 };
        private int AlarmGridWidth = 0;
        private int GridRowHeight = 30;
        private int GridHeaderHeight = 30;
        //private int GridInitWidth = 0;
        private enum eManualBtn : int
        {
            pcbTab = 0, lensTab
        };
        public AlarmControl(int _w, int _h)
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(Form_Paint);
            
            this.Width = _w;
            this.Height = _h;

            
            setInterface();
            InitAlarmGrid();
        }
        public void RefreshAlarm()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(ShowAlarmGrid));
                this.Invoke(new Action(ShowAlarmPageLabel));
            }
            else
            {
                ShowAlarmGrid();
                ShowAlarmPageLabel();
            }
            //ShowAlarmGrid();
            //ShowAlarmPageLabel();
        }
        private void ShowAlarmPageLabel()
        {
            string pageStr = (CurrentAlarmPage + 1).ToString() + " / " + TotalAlarmPage.ToString();
            label_AlarmPage.Text = pageStr;
        }
        private void ShowAlarmGrid()
        {
            int i = 0;  //옆

            int nCol = dataGridView_Alarm.ColumnCount;         //7 옆으로 행
            int nRow = dataGridView_Alarm.RowCount;        //0 아래로 열 빈칸 -1
            int dataCount = Globalo.yamlManager.alarmData.Alarms.Count;



            TotalAlarmPage = (int)(dataCount / AlarmGridRowViewCount);
            int AlarmDetailsRemain = (int)(dataCount % AlarmGridRowViewCount);
            if(AlarmDetailsRemain > 0)
            {
                TotalAlarmPage++;
            }
            dataGridView_Alarm.Rows.Clear();


            int gridViewCount = dataCount;
            if (gridViewCount < AlarmGridRowViewCount)
            {
                gridViewCount = AlarmGridRowViewCount;
            }
            int index = 0;

            for (i = 0; i < AlarmGridRowViewCount; i++)
            {
                index = i + (CurrentAlarmPage * AlarmGridRowViewCount);
                if(index < dataCount)
                {
                    //dataGridView_Alarm.Rows.Add((i + 1).ToString(), Globalo.yamlManager.alarmData.Alarms[index].Time, Globalo.yamlManager.alarmData.Alarms[index].Details);
                    dataGridView_Alarm.Rows.Add(Globalo.yamlManager.alarmData.Alarms[index].No, Globalo.yamlManager.alarmData.Alarms[index].Time, Globalo.yamlManager.alarmData.Alarms[index].Details);
                }
                else
                {
                    dataGridView_Alarm.Rows.Add("", "", "");
                }
                dataGridView_Alarm.Rows[i].Cells[1].Style.BackColor = Color.White; // 1번 열
                dataGridView_Alarm.Rows[i].Cells[1].Style.ForeColor = Color.Black; // 1번 열
                dataGridView_Alarm.Rows[i].Cells[1].Style.Font = new Font(dataGridView_Alarm.DefaultCellStyle.Font, FontStyle.Regular);


                index += i;
            }


            dataGridView_Alarm.ClearSelection();
        }
        private void InitAlarmGrid()
        {
            //BankGrid
            int i = 0;
            int j = 0;
            // 열 추가
            // 행 헤더 숨기기
            dataGridView_Alarm.RowHeadersVisible = false;
            //사이즈 조절 막기
            dataGridView_Alarm.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            // 행 높이 자동 조정
            dataGridView_Alarm.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // 모든 셀에 맞게 자동 조정

            // 열 자동 크기 조정
            dataGridView_Alarm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // 또는
            // 셀 내용 줄바꿈
            dataGridView_Alarm.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // 헤더 폰트 설정
            dataGridView_Alarm.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
            dataGridView_Alarm.DefaultCellStyle.Font = new Font("나눔고딕", 10);


            // 헤더 배경색 설정
            dataGridView_Alarm.ColumnHeadersDefaultCellStyle.BackColor = Color.GhostWhite;// LightGray;// Color.FromArgb(94, 129, 244); //Color.LightBlue;
            // 헤더 폰트 색
            dataGridView_Alarm.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;// .Gray;

            dataGridView_Alarm.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView_Alarm.RowsDefaultCellStyle.ForeColor = Color.Black;

            // Set the selection background color for all the cells.
            //dataGridView_Recipe.DefaultCellStyle.SelectionBackColor = Color.White;
            // dataGridView_Recipe.DefaultCellStyle.SelectionForeColor = Color.Black;
            // dataGridView_Recipe.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Empty;
            // dataGridView_Recipe.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;

            //dataGridView_Recipe.DefaultCellStyle.SelectionForeColor = Color.Empty;

            //dataGridView_Recipe.DefaultCellStyle.SelectionBackColor = Color.Empty;
            // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
            // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
            // dataGridView_Recipe.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            // dataGridView_Recipe.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Empty;

            dataGridView_Alarm.EnableHeadersVisualStyles = false;
            // 열 헤더 가운데 정렬
            dataGridView_Alarm.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //운영 체제의 기본 시각적 스타일을 무시
            dataGridView_Alarm.AllowUserToResizeRows = false;

            dataGridView_Alarm.ReadOnly = true;
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();

            cellStyle.Font = new Font("나눔고딕", 10, FontStyle.Regular); // Change font and size
            cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Center align text
            cellStyle.SelectionBackColor = Color.LightBlue;
            //cellStyle.SelectionForeColor = Color.Empty;

            dataGridView_Alarm.DefaultCellStyle = cellStyle;

            DataGridViewTextBoxColumn[] textColumns = new DataGridViewTextBoxColumn[3];

            for (i = 0; i < 3; i++)
            {
                textColumns[i] = new DataGridViewTextBoxColumn();
            }

            //DataGridView

            textColumns[0].HeaderText = "No";
            textColumns[1].HeaderText = "Time";
            textColumns[2].HeaderText = "Alarm details";



            dataGridView_Alarm.Columns.Add(textColumns[0]);
            dataGridView_Alarm.Columns.Add(textColumns[1]);
            dataGridView_Alarm.Columns.Add(textColumns[2]);

            for (i = 0; i < dataGridView_Alarm.ColumnCount; i++)
            {
                dataGridView_Alarm.Columns[i].Resizable = DataGridViewTriState.False;
            }
            int gridWidth = dataGridView_Alarm.Width;
            dataGridView_Alarm.Columns[0].Width = GridColWidth[0];
            dataGridView_Alarm.Columns[1].Width = GridColWidth[1];
            dataGridView_Alarm.Columns[2].Width = gridWidth - GridColWidth[0] - GridColWidth[1];




            // 행 높이 조정
            dataGridView_Alarm.RowTemplate.Height = GridRowHeight; // 자동 추가되는 행 높이 설정
            dataGridView_Alarm.ColumnHeadersHeight = GridHeaderHeight;
            //dataGridView_Alarm.ColumnHeadersDefaultCellStyle.ForeColor = Color.Blue;

            for (i = 0; i < AlarmGridRowViewCount; i++)
            {

                string text = $"예시 텍스트 {i}"; // 예시 텍스트 생성
                //bool isChecked = (i % 2 == 0); // 짝수인 경우 체크박스가 체크됨
                dataGridView_Alarm.Rows.Add("", "", ""); // 행 추가
                dataGridView_Alarm.Rows[i].Height = GridRowHeight;

                for (j = 0; j < dataGridView_Alarm.ColumnCount; j++)
                {
                    //dataGridView.Columns[i].Resizable = DataGridViewTriState.False;
                    //dataGridView_Alarm.Columns[j].Width = GridColWidth[j];
                    dataGridView_Alarm.Columns[j].Resizable = DataGridViewTriState.False;

                }
            }

            dataGridView_Alarm.Height = AlarmGridRowViewCount * GridRowHeight + GridRowHeight + 2;
            if (dataGridView_Alarm.AllowUserToAddRows == true)
            {
                //dataGridView_Model.Rows[GridRowCount].Height = GridRowHeight;
            }


            dataGridView_Alarm.MultiSelect = false; // 여러 개 선택 불가능
            dataGridView_Alarm.AllowUserToAddRows = false; // 빈 행 추가 방지
            dataGridView_Alarm.ScrollBars = ScrollBars.Vertical;      //가로 스크롤 안보이게 설정

            //dataGridView_Alarm.CellContentClick += new DataGridViewCellEventHandler(RecipeGridView_CellContentClick);     //삭제 버튼 클릭시 사용
            // 버튼 클릭 이벤트 등록
            //dataGridView_Alarm.CellClick += new DataGridViewCellEventHandler(RecipeGridView_CellClick); //textbox 한번 클릭으로 바로 수정되게 추가
            //dataGridView_Alarm.SelectionChanged += dataGridView1_SelectionChanged;
            // 이벤트 핸들러 추가
            //CardGrid.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(CardGrid_EditingControlShowing);
            //dataGridView_Alarm.CellFormatting += dataGridView_Model_CellFormatting;

            AlarmGridWidth = dataGridView_Alarm.Width;     //<---스크롤 생겼을때 사이즈 조절위해 초기 Grid 넓이 저장

            // 각 컬럼의 헤더 텍스트 정렬 설정
            foreach (DataGridViewColumn column in dataGridView_Alarm.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }



            //dataGridView_Model.Columns[0].ReadOnly = true; // 읽기 전용
            dataGridView_Alarm.Columns[0].DefaultCellStyle.BackColor = Color.LightGray; // 배경색 설정
            dataGridView_Alarm.Columns[0].DefaultCellStyle.ForeColor = Color.Yellow; // 배경색 설정
            //dataGridView_Alarm.Columns[0].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold); // 굵은 글씨

            dataGridView_Alarm.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView_Alarm.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView_Alarm.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView_Alarm.ClearSelection();
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


            //BTN_MANUAL_PCB.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            //BTN_MANUAL_LENS.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            //ManualTitleLabel.Text = "MANUAL";
            //ManualTitleLabel.ForeColor = Color.Khaki;     
            //ManualTitleLabel.BackColor = Color.Maroon;
            //ManualTitleLabel.Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular);
            //ManualTitleLabel.Width = this.Width;
            //ManualTitleLabel.Height = 45;
            //ManualTitleLabel.Location = new Point(0, 0);



            //ManualPanel.Location = new Point(BTN_MANUAL_PCB.Location.X, BTN_MANUAL_PCB.Location.Y + panelYGap);

            int AlarmDetailsRemain = 0;

            
            if (Globalo.yamlManager.alarmData == null || Globalo.yamlManager.alarmData.Alarms.Count < 1)
            {
                TotalAlarmPage = 1;
                CurrentAlarmPage = 0;
            }
            else
            {
                TotalAlarmPage = (int)(Globalo.yamlManager.alarmData.Alarms.Count / AlarmGridRowViewCount);
                AlarmDetailsRemain = (int)(Globalo.yamlManager.alarmData.Alarms.Count % AlarmGridRowViewCount);

                if (AlarmDetailsRemain > 0)
                {
                    TotalAlarmPage++;
                }
                CurrentAlarmPage = TotalAlarmPage - 1;       // 0 이 첫페이지
            }

            
            

        }
        private void ManualBtnChange(eManualBtn index)
        {
            BTN_MANUAL_PCB.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            BTN_MANUAL_LENS.BackColor = ColorTranslator.FromHtml("#E1E0DF");


        }
        private void BTN_MANUAL_PCB_Click(object sender, EventArgs e)
        {
            ManualBtnChange(eManualBtn.pcbTab);
        }

        private void BTN_MANUAL_LENS_Click(object sender, EventArgs e)
        {
            ManualBtnChange(eManualBtn.lensTab);
        }

        private void AlarmControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                RefreshAlarm();
            }
            else
            {

            }
        }
        private void AlarmPageChange(int Dic)
        {
            if(Dic == 1)
            {
                //Prev Page
                if(CurrentAlarmPage > 0)
                {
                    CurrentAlarmPage--;
                }
            }
            else
            {
                //Next Page
                if (CurrentAlarmPage < TotalAlarmPage - 1)
                {
                    CurrentAlarmPage++;
                }
            }
            RefreshAlarm();
        }
        private void BTN_ALARM_PREV_Click(object sender, EventArgs e)
        {
            AlarmPageChange(1);
        }

        private void BTN_ALARM_NEXT_Click(object sender, EventArgs e)
        {
            AlarmPageChange(2);
            
        }
    }
}
