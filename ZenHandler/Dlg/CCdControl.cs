using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using OpenCvSharp;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using ReaLTaiizor.Drawing.Poison;

namespace ZenHandler.Dlg
{
    public partial class CCdControl : UserControl
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용
        //private ManualPcb manualPcb = new ManualPcb();
        //private ManualLens manualLens = new ManualLens();
        private const int EEpromGridRowViewCount = 15;//10;

        private int[] GridColWidth = { 50, 80, 65, 65, 70, 270, 50, 50, 1 };
        private int GridRowHeight = 30;
        private int GridHeaderHeight = 30;
        private int GridInitWidth = 0;
        private int SelectedCellRow = 0;
        private int SelectedCellCol = 0;

        //Rectangle[] m_clRectROI = new Rectangle[Globalo.CHART_ROI_COUNT];
        //Rectangle[] m_clRectCircle = new Rectangle[4];

        //public int[] m_iOffsetX_SFR;
        //public int[] m_iOffsetY_SFR;

        //public int[] m_iSizeX_SFR;
        //public int[] m_iSizeY_SFR;
        //public int m_iCurNo_SFR;
        //public Rectangle[] m_rcRoiBox;						// 원형 마크 검색 영역
        

        List<byte> CcdEEpromReadData = new List<byte>();

        private enum eManualBtn : int
        {
            pcbTab = 0, lensTab
        };
        public CCdControl(int _w, int _h)
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(Form_Paint);
            
            this.Width = _w;
            this.Height = _h;

            setInterface();
            InitEEpromGrid();
        }
        public void SetSensorIni()
        {
            int i = 0;
            //ComboBox_IniList
            int Count = Globalo.taskWork.vSensorIniList.Count;
            if (Count < 1 || Globalo.mLaonGrabberClass.currentIniFile.Length < 1)
            {
                return;
            }

            ComboBox_IniList.Items.Clear();
            for (i = 0; i < Count; i++)
            {
                ComboBox_IniList.Items.Add(Globalo.taskWork.vSensorIniList[i]);
            }

            if (ComboBox_IniList.Items.Contains(Globalo.mLaonGrabberClass.currentIniFile))
            {
                ComboBox_IniList.SelectedItem = Globalo.mLaonGrabberClass.currentIniFile; // 해당 값을 선택
            }

            
            ///////ComboBox_IniList.SelectedIndex = 0;
        }

        public void SetImageInfo()
        {
            string info = $"Width:{Globalo.mLaonGrabberClass.m_nWidth} Height:{Globalo.mLaonGrabberClass.m_nHeight}";
            label_ImageInfo.Text = info;
        }
        private void button_Ini_Select_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.ManualTesting)
            {
                Globalo.LogPrint("ManualControl", "[INFO] MANUAL 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            string selectedValue = ComboBox_IniList.SelectedItem.ToString();


            DialogResult result = Globalo.MessageAskPopup($"[{selectedValue}] \n파일로 변경하시겠습니까?");

            if (result == DialogResult.Yes)
            {
                //ini Set
                try
                {
                    Globalo.mLaonGrabberClass.CloseDevice();

                    Globalo.mLaonGrabberClass.UiconfigSave(selectedValue);
                    Globalo.mLaonGrabberClass.UiconfigLoad();
                    Globalo.mLaonGrabberClass.SelectSensor();
                    Globalo.mLaonGrabberClass.AllocImageBuff();

                    Globalo.vision.UISet(Globalo.camControl.CcdPanel.Width, Globalo.camControl.CcdPanel.Height);

                    if (ProgramState.ON_LINE_MIL == true)
                    {
                        Globalo.threadControl.imageGrabThread.RawInit();
                    }
                    else if (ProgramState.ON_LINE_MIL == true)
                    {
                        Globalo.vision.AllocMilCCdBuffer(0, Globalo.mLaonGrabberClass.m_nWidth, Globalo.mLaonGrabberClass.m_nHeight);
                        Globalo.vision.AllocMilCcdDisplay(Globalo.camControl.CcdPanel.Handle);
                    }

                    Globalo.threadControl.ccdGrabThread.RawInit();

                    SetImageInfo();
                    
                    //AllocMilCCdBuffer

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ini Set 처리 중 예외 발생: {ex.Message}");
                }
            }

        }

        private void InitEEpromGrid()
        {
            int i = 0;
            int j = 0;
            // 열 추가
            // 행 헤더 숨기기
            dataGridView_EEpromData.RowHeadersVisible = false;
            //사이즈 조절 막기
            dataGridView_EEpromData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            // 행 높이 자동 조정
            dataGridView_EEpromData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // 모든 셀에 맞게 자동 조정

            // 열 자동 크기 조정
            dataGridView_EEpromData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // 또는
            // 셀 내용 줄바꿈
            dataGridView_EEpromData.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // 헤더 폰트 설정
            dataGridView_EEpromData.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", 9F, FontStyle.Bold);

            // 헤더 배경색 설정
            dataGridView_EEpromData.ColumnHeadersDefaultCellStyle.BackColor = Color.GhostWhite;// Color.FromArgb(94, 129, 244); //Color.LightBlue;
            // 헤더 폰트 색
            dataGridView_EEpromData.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;

            dataGridView_EEpromData.RowsDefaultCellStyle.BackColor = Color.GhostWhite;
            dataGridView_EEpromData.RowsDefaultCellStyle.ForeColor = Color.Gray;

            // Set the selection background color for all the cells.
            //dataGridView_EEpromData.DefaultCellStyle.SelectionBackColor = Color.White;
            // dataGridView_EEpromData.DefaultCellStyle.SelectionForeColor = Color.Black;
            // dataGridView_EEpromData.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Empty;
            // dataGridView_EEpromData.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;

            //dataGridView_EEpromData.DefaultCellStyle.SelectionForeColor = Color.Empty;

            //dataGridView_EEpromData.DefaultCellStyle.SelectionBackColor = Color.Empty;
            // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
            // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
            // dataGridView_EEpromData.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            // dataGridView_EEpromData.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Empty;

            dataGridView_EEpromData.EnableHeadersVisualStyles = false;
            // 열 헤더 가운데 정렬
            dataGridView_EEpromData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //운영 체제의 기본 시각적 스타일을 무시
            dataGridView_EEpromData.AllowUserToResizeRows = false;

            dataGridView_EEpromData.ReadOnly = true;
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();

            cellStyle.Font = new Font("나눔고딕", 10, FontStyle.Regular); // Change font and size
            cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Center align text
            cellStyle.SelectionBackColor = Color.LightBlue;
            //cellStyle.SelectionForeColor = Color.Empty;

            dataGridView_EEpromData.DefaultCellStyle = cellStyle;

            DataGridViewTextBoxColumn[] textColumns = new DataGridViewTextBoxColumn[4];

            for (i = 0; i < 4; i++)
            {
                textColumns[i] = new DataGridViewTextBoxColumn();
            }

            //DataGridView
            textColumns[0].HeaderText = "No";
            textColumns[1].HeaderText = "Addr";
            textColumns[2].HeaderText = "Hex";
            textColumns[3].HeaderText = "ASCII";

            textColumns[0].Name = "No";
            textColumns[1].Name = "Addr";
            textColumns[2].Name = "Hex";
            textColumns[3].Name = "ASCII";




            dataGridView_EEpromData.Columns.Add(textColumns[0]);
            dataGridView_EEpromData.Columns.Add(textColumns[1]);
            dataGridView_EEpromData.Columns.Add(textColumns[2]);
            dataGridView_EEpromData.Columns.Add(textColumns[3]);

            for (i = 0; i < dataGridView_EEpromData.ColumnCount; i++)
            {
                dataGridView_EEpromData.Columns[i].Resizable = DataGridViewTriState.False;
            }

            dataGridView_EEpromData.Width = GridColWidth[0] + GridColWidth[1] + GridColWidth[2] + GridColWidth[3];

            int gridWidth = dataGridView_EEpromData.Width;

            dataGridView_EEpromData.Columns[0].Width = GridColWidth[0];
            dataGridView_EEpromData.Columns[1].Width = GridColWidth[1];
            dataGridView_EEpromData.Columns[2].Width = GridColWidth[2];
            dataGridView_EEpromData.Columns[3].Width = GridColWidth[3];



            // 행 높이 조정
            dataGridView_EEpromData.RowTemplate.Height = GridRowHeight; // 자동 추가되는 행 높이 설정
            dataGridView_EEpromData.ColumnHeadersHeight = GridHeaderHeight;

            //dataGridView_EEpromData.ColumnHeadersDefaultCellStyle.ForeColor = Color.Blue;

            for (i = 0; i < EEpromGridRowViewCount; i++)
            {
                string text = $"예시 텍스트 {i}"; // 예시 텍스트 생성
                //bool isChecked = (i % 2 == 0); // 짝수인 경우 체크박스가 체크됨
                dataGridView_EEpromData.Rows.Add(""); // 행 추가
                dataGridView_EEpromData.Rows[i].Height = GridRowHeight;

                for (j = 0; j < dataGridView_EEpromData.ColumnCount; j++)
                {
                    //dataGridView.Columns[i].Resizable = DataGridViewTriState.False;
                    //dataGridView_EEpromData.Columns[j].Width = GridColWidth[j];
                    dataGridView_EEpromData.Columns[j].Resizable = DataGridViewTriState.False;
                    dataGridView_EEpromData.Rows[i].Cells[j].Value = "";
                }
            }
            dataGridView_EEpromData.Height = EEpromGridRowViewCount * GridRowHeight + GridRowHeight + 2;
            if (dataGridView_EEpromData.AllowUserToAddRows == true)
            {
                //dataGridView_Model.Rows[GridRowCount].Height = GridRowHeight;
            }


            dataGridView_EEpromData.MultiSelect = false; // 여러 개 선택 불가능
            dataGridView_EEpromData.AllowUserToAddRows = false; // 빈 행 추가 방지
            dataGridView_EEpromData.ScrollBars = ScrollBars.Vertical;      //가로 스크롤 안보이게 설정

            //dataGridView_EEpromData.CellContentClick += ModelGridView_CellContentClick;     //삭제 버튼 클릭시 사용
            // 버튼 클릭 이벤트 등록
            dataGridView_EEpromData.CellClick += new DataGridViewCellEventHandler(GridView_CellClick); //textbox 한번 클릭으로 바로 수정되게 추가
            //dataGridView_EEpromData.SelectionChanged += dataGridView1_SelectionChanged;
            // 이벤트 핸들러 추가
            //CardGrid.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(CardGrid_EditingControlShowing);
            //dataGridView_EEpromData.CellFormatting += dataGridView_Model_CellFormatting;

            GridInitWidth = dataGridView_EEpromData.Width;     //<---스크롤 생겼을때 사이즈 조절위해 초기 Grid 넓이 저장

            // 각 컬럼의 헤더 텍스트 정렬 설정
            foreach (DataGridViewColumn column in dataGridView_EEpromData.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


            //dataGridView_EEpromData.Columns[0].ReadOnly = true; // 읽기 전용


            dataGridView_EEpromData.Columns[0].DefaultCellStyle.BackColor = Color.LightGray; // 배경색 설정
            dataGridView_EEpromData.Columns[0].DefaultCellStyle.ForeColor = Color.Yellow; // 배경색 설정
            dataGridView_EEpromData.Columns[0].DefaultCellStyle.Font = new Font("나눔고딕", 10F, FontStyle.Bold); // 굵은 글씨
            dataGridView_EEpromData.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 가운데 정렬

            dataGridView_EEpromData.ClearSelection();
        }

        private void GridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Console.WriteLine($"DataGridView_CellClick");
            SelectedCellCol = e.ColumnIndex;
            SelectedCellRow = e.RowIndex;           //세로
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
            if (ComboBox_IniList.Items.Count > 0)
            {
                ComboBox_IniList.SelectedIndex = 0;
            }
        }

        private void ManualBtnChange(eManualBtn index)
        {
            BTN_MANUAL_PCB.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            BTN_MANUAL_LENS.BackColor = ColorTranslator.FromHtml("#E1E0DF");

            

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

        private void BTN_CCD_GABBER_OPEN_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.ManualTesting)
            {
                Globalo.LogPrint("ManualControl", "[INFO] MANUAL 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.threadControl.manualThread.GetThreadRun() == false)
            {
                Globalo.LogPrint("", "[CCD] MANUAL CCD OPEN");
                Globalo.threadControl.manualThread.runfn(FThread.ManualThread.eManualType.M_CCD_OPEN);// 2);
            }
        }

        private void BTN_CCD_GABBER_START_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.ManualTesting)
            {
                Globalo.LogPrint("ManualControl", "[INFO] MANUAL 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.mLaonGrabberClass.StartGrab();
        }

        private void BTN_CCD_GABBER_STOP_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.ManualTesting)
            {
                Globalo.LogPrint("ManualControl", "[INFO] MANUAL 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.mLaonGrabberClass.StopGrab();
        }

        private void BTN_CCD_GABBER_CLOSE_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            if (Globalo.motionManager.transferMachine.RunState == OperationState.ManualTesting)
            {
                Globalo.LogPrint("ManualControl", "[INFO] MANUAL 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.threadControl.manualThread.GetThreadRun() == false)
            {
                Globalo.LogPrint("", "[CCD] MANUAL CCD CLOSE");
                Globalo.threadControl.manualThread.runfn(FThread.ManualThread.eManualType.M_CCD_CLOSE);//3);
            }
            //Globalo.mLaonGrabberClass.CloseDevice();
        }

        private void BTN_CCD_BMP_LOAD_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
        }

        private void BTN_CCD_BMP_SAVE_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
        }

        private void BTN_CCD_RAW_LOAD_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "d:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    byte[] m_pImgBuff;


                    //Read the contents of the file into a stream
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        int fileSize = (int)fileStream.Length;
                        m_pImgBuff = new byte[fileSize];
                        fileStream.Read(m_pImgBuff, 0, fileSize);

                        //화면에 보여주기
                        CCARawImageLoad(ref m_pImgBuff, 0, false);
                    }

                }
            }
        }

        private void BTN_CCD_RAW_SAVE_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "raw Image|*.raw";
            saveFileDialog1.Title = "Save an Image Raw File";
            saveFileDialog1.ShowDialog();
            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

                fs.Write(Globalo.mLaonGrabberClass.m_pFrameRawBuffer, 0, Globalo.mLaonGrabberClass.nFrameRawSize);
            }
        }
        public void CCARawImageLoad(ref byte[] LoadImg, int index, bool autoRun)
        {
            double dZoomX = 0.0;
            double dZoomY = 0.0;

            dZoomX = Globalo.vision.M_CcdReduceFactorX;
            dZoomY = Globalo.vision.M_CcdReduceFactorY;

            //oGlobal.mLaonGrabberClass.m_pFrameRawBuffer = LoadImg;
            int rSize = Globalo.mLaonGrabberClass.nFrameRawSize;    // MIU.m_pBoard->GetFrameRawSize();
            Array.Copy(LoadImg, Globalo.mLaonGrabberClass.m_pFrameRawBuffer, rSize);


            //TDATASPEC tspec;
            //tspec.eOutMode = gMIUDevice.dTDATASPEC_n.eOutMode; ;
            //tspec.eDataFormat = gMIUDevice.dTDATASPEC_n.eDataFormat; ;
            //tspec.eSensorType = gMIUDevice.dTDATASPEC_n.eSensorType; ;



            //memcpy(MIU.m_pFrameRawBuffer, LoadImg, rSize);


            IntPtr RawPtr = Marshal.UnsafeAddrOfPinnedArrayElement(LoadImg, 0);
            IntPtr BmpPtr = Marshal.UnsafeAddrOfPinnedArrayElement(Globalo.mLaonGrabberClass.m_pFrameBMPBuffer, 0);

            //ACMISSoftISP::xMakeBMP(MIU.m_pFrameRawBuffer, (byte*)MIU.m_pFrameBMPBuffer, gMIUDevice.nWidth, gMIUDevice.nHeight, tspec);
            //if (Globalo.mLaonGrabberClass.GrabberDll.mGetFrame((byte*)RawPtr.ToPointer(), (byte*)BmpPtr.ToPointer()) == true)
            unsafe
            {
                TDATASPEC tSpec = new TDATASPEC();
                tSpec.eDataFormat = Globalo.mLaonGrabberClass.mGrabSpec.eDataFormat;    // EDATAFORMAT.DATAFORMAT_BAYER_12BIT;     
                tSpec.eSensorType = Globalo.mLaonGrabberClass.mGrabSpec.eSensorType;    //ESENSORTYPE.SENSORTYPE_RGGB;       
                tSpec.eOutMode = Globalo.mLaonGrabberClass.mGrabSpec.eOutMode;          //EOUTMODE.OUTMODE_BAYER_BGGR;
                tSpec.nBlackLevel = 0;

                IntPtr structPtr = Marshal.AllocHGlobal(Marshal.SizeOf<TDATASPEC>());
                Marshal.StructureToPtr(tSpec, structPtr, false);

                Globalo.GrabberDll.mxMakeBMP((byte*)RawPtr.ToPointer(), (byte*)BmpPtr.ToPointer(), Globalo.mLaonGrabberClass.m_nWidth, Globalo.mLaonGrabberClass.m_nHeight, structPtr);
            }


            Globalo.mLaonGrabberClass.imageItp = new Mat(Globalo.mLaonGrabberClass.m_nHeight, Globalo.mLaonGrabberClass.m_nWidth, MatType.CV_8UC3);//, Globalo.mLaonGrabberClass.m_pFrameBMPBuffer);
            Globalo.mLaonGrabberClass.imageItp.SetArray<byte>(Globalo.mLaonGrabberClass.m_pFrameBMPBuffer); // 배열 데이터를 Mat에 복사


            Cv2.ExtractChannel(Globalo.mLaonGrabberClass.imageItp, Globalo.mLaonGrabberClass.m_pImageBuff[0], 0);
            Cv2.ExtractChannel(Globalo.mLaonGrabberClass.imageItp, Globalo.mLaonGrabberClass.m_pImageBuff[1], 1);
            Cv2.ExtractChannel(Globalo.mLaonGrabberClass.imageItp, Globalo.mLaonGrabberClass.m_pImageBuff[2], 2);

            byte[] bytes2 = new byte[Globalo.mLaonGrabberClass.m_nHeight * Globalo.mLaonGrabberClass.m_nWidth];

            Marshal.Copy(Globalo.mLaonGrabberClass.m_pImageBuff[2].Data, bytes2, 0, bytes2.Length); // Mat 데이터를 바이트 배열로 복사
            MIL.MbufPut(Globalo.vision.m_MilCcdProcChild[0, 0], bytes2);
            Marshal.Copy(Globalo.mLaonGrabberClass.m_pImageBuff[1].Data, bytes2, 0, bytes2.Length); // Mat 데이터를 바이트 배열로 복사
            MIL.MbufPut(Globalo.vision.m_MilCcdProcChild[0, 1], bytes2);
            Marshal.Copy(Globalo.mLaonGrabberClass.m_pImageBuff[0].Data, bytes2, 0, bytes2.Length); // Mat 데이터를 바이트 배열로 복사
            MIL.MbufPut(Globalo.vision.m_MilCcdProcChild[0, 2], bytes2);

            //Globalo.mLaonGrabberClass.imageItp.SaveImage("d:\\imageItp.jpg");
            //Globalo.mLaonGrabberClass.m_pImageBuff[2].SaveImage("d:\\m_pImageBuff.jpg");

            MIL.MimResize(Globalo.vision.m_MilCcdProcImage[0], Globalo.vision.m_MilSmallImage[0], dZoomX, dZoomY, MIL.M_DEFAULT);
        }
   
        private void button4_Click(object sender, EventArgs e)
        {
            bool kk = CLaonGrabberClass.SensorIdRead_Head_Fn();
        }
        public void ShowEEpromGrid(ushort startAddr = 0, int dataLenght = 0)
        {
            Console.WriteLine("dataLenght: " + dataLenght.ToString());


            int i = 0;  //옆

            int nCol = dataGridView_EEpromData.ColumnCount;         //7 옆으로 행
            int nRow = dataGridView_EEpromData.RowCount;        //0 아래로 열 빈칸 -1

            int dataCount = dataLenght;
            if(dataCount < 1)
            {
                return;
            }
            //Globalo.mCCdPanel.CcdEEpromReadData.AddRange(EEpromReadData);


            dataGridView_EEpromData.Rows.Clear();

            int gridViewCount = dataCount;

            if (gridViewCount < EEpromGridRowViewCount)
            {
                gridViewCount = EEpromGridRowViewCount;
            }

            for (i = 0; i < gridViewCount; i++)
            {
                if(i < dataCount)
                {
                    char displayChar = (char)Globalo.mCCdPanel.CcdEEpromReadData[i];
                    if (displayChar == '\0') // null 문자 처리
                    {
                        displayChar = ' '; // 공백으로 대체
                    }
                    dataGridView_EEpromData.Rows.Add((i + 1).ToString(), "0x" + (startAddr + i).ToString("X2"), "0x" + Globalo.mCCdPanel.CcdEEpromReadData[i].ToString("X2"), displayChar);// (char)Globalo.mCCdPanel.CcdEEpromReadData[i]);

                    //Globalo.mLaonGrabberClass.eepromDicData
                }
                else
                {
                    dataGridView_EEpromData.Rows.Add("", "", "", ""); // 행 추가
                    
                }
                dataGridView_EEpromData.Rows[i].Cells[1].Style.BackColor = Color.White; // 1번 열
                dataGridView_EEpromData.Rows[i].Cells[1].Style.ForeColor = Color.Black; // 1번 열
                dataGridView_EEpromData.Rows[i].Cells[2].Style.BackColor = Color.White; // 1번 열
                dataGridView_EEpromData.Rows[i].Cells[2].Style.ForeColor = Color.Black; // 1번 열
                dataGridView_EEpromData.Rows[i].Cells[3].Style.BackColor = Color.White; // 1번 열
                dataGridView_EEpromData.Rows[i].Cells[3].Style.ForeColor = Color.Black; // 1번 열
                dataGridView_EEpromData.Rows[i].Cells[1].Style.Font = new Font(dataGridView_EEpromData.DefaultCellStyle.Font, FontStyle.Regular);
                dataGridView_EEpromData.Rows[i].Cells[2].Style.Font = new Font(dataGridView_EEpromData.DefaultCellStyle.Font, FontStyle.Regular);
                dataGridView_EEpromData.Rows[i].Cells[3].Style.Font = new Font(dataGridView_EEpromData.DefaultCellStyle.Font, FontStyle.Regular);
            }


            if (gridViewCount > EEpromGridRowViewCount)
            {
                dataGridView_EEpromData.Width = GridInitWidth + 20; //스크롤 추가시 grid Width 조정
            }
            dataGridView_EEpromData.ClearSelection();
        }
        private void BTN_CCD_EEPROM_VERIFY_TEST_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.ManualTesting)
            {
                Globalo.LogPrint("ManualControl", "[INFO] MANUAL 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.threadControl.manualThread.GetThreadRun() == false)
            {
                Globalo.motionManager.transferMachine.RunState = OperationState.ManualTesting;
                Globalo.LogPrint("", "[CCD] MANUAL EEPROM VERIFY");
                Globalo.threadControl.manualThread.runfn(FThread.ManualThread.eManualType.M_MANUAL_VERIFY);//10);
            }
        }
        public void EEpromRead()
        {
            //testEEpromRead();
            if (this.InvokeRequired)
            {
                //this.Invoke(new Action(testEEpromRead));
                bool Rtn = (bool)this.Invoke(new Func<bool>(() =>testEEpromRead()));
            }
            else
            {
                testEEpromRead();
            }

            
        }
        public static unsafe bool testEEpromRead()
        {
            int i = 0;

            Console.WriteLine("testEEpromRead run");
            string slaveAddr = "";
            string readAddr = "";
            ushort readDataLength = 0;
            try
            {
                slaveAddr = Regex.Replace(Globalo.mCCdPanel.textBox_SlaveAddr.Text, @"\D", "");
                readAddr = Regex.Replace(Globalo.mCCdPanel.textBox_ReadAddr.Text, @"\D", "");

                //string numericPart = Regex.Replace(input, @"\D", "");  // 숫자가 아닌 부분(\D)을 제거
                readDataLength = Convert.ToUInt16(Globalo.mCCdPanel.textBox_ReadDataLeng.Text);  //읽어야될 길이
            }
            catch (Exception ex)
            {
                Console.WriteLine($"testEEpromRead 처리 중 예외 발생: {ex.Message}");
            }

            

            if(readDataLength < 1)
            {
                return false;
            }

            ushort maxReadLength = CLaonGrabberClass.MAX_READ_WRITE_LENGTH;     //1024


            if (maxReadLength > readDataLength)
            {
                maxReadLength = readDataLength;
            }

            
            //Int32.Parse(input);
            int errorCode = 0;


            int endAddress = readDataLength;//// 0xE0;  //       241
            //0x513;     //1299
            ushort SlaveAddr = Convert.ToUInt16(slaveAddr, 16); // 0x50;
            ushort StartAddr = Convert.ToUInt16(readAddr, 16); //0x00;

            //ushort checkAddr = 0x3C06;

            byte[] EEpromReadData = new byte[endAddress]; // EEPROM 데이터 읽기
            //if(Globalo.mLaonGrabberClass.EEpromReadData == null)
            //{
            //    Globalo.mLaonGrabberClass.EEpromReadData = new byte[endAddress + 0];
            //}
            //else
            //{
            //    if (Globalo.mLaonGrabberClass.EEpromReadData == null || Globalo.mLaonGrabberClass.EEpromReadData.Length != (endAddress + 0))
            //    {
            //        Array.Resize(ref Globalo.mLaonGrabberClass.EEpromReadData, endAddress + 0);
            //    }
            //}
            
            //byte[] pReadData = new byte[260]; // MAX_PATH 대신 일반적인 크기(예: 260) 사용

            //Array.Clear(Globalo.mLaonGrabberClass.EEpromReadData, 0, Globalo.mLaonGrabberClass.EEpromReadData.Length);
            //Array.Clear(pReadData, 0, pReadData.Length);



            Globalo.mCCdPanel.CcdEEpromReadData.Clear();
            

            for (i = 0; i < endAddress; i+= maxReadLength)     // 0;  i < 129;  i += 30; 
            {
                fixed (byte* pData = EEpromReadData)
                {
                    if((i + maxReadLength) > endAddress)
                    {
                        //if( ( 0 + 30 ) > 129
                        //if( ( 30 + 30 ) > 129
                        //if( ( 60 + 30 ) > 129
                        //if( ( 90 + 30 ) > 129
                        //if( ( 120 + 30 ) > 129
                        //150

                        maxReadLength = (ushort)((endAddress - i) + 0);    //120 ~ 129 는 10개라서 + 1
                    }
                    errorCode = Globalo.GrabberDll.mReadI2CBurst(SlaveAddr, (ushort)(StartAddr + i), 2, pData + i, (ushort)maxReadLength);
                    if (errorCode != 0)
                    {
                        Console.WriteLine("mReadI2CBurst errorCode");
                        break;
                    }
                }
            }

            //Globalo.mLaonGrabberClass.eepromDicData.Clear();
            //for (i = 0; i < endAddress; i++)
            //{
            //    Globalo.mLaonGrabberClass.eepromDicData.Add((ushort)i, EEpromReadData[i]);
            //}
            Globalo.mCCdPanel.CcdEEpromReadData.AddRange(EEpromReadData);

            Globalo.mCCdPanel.ShowEEpromGrid(StartAddr, Globalo.mCCdPanel.CcdEEpromReadData.Count);

            return true;
        }
        private void BTN_CCD_EEPROM_READ_Click(object sender, EventArgs e)
        {
            if (Globalo.motionManager.transferMachine.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.transferMachine.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            if (Globalo.motionManager.transferMachine.RunState == OperationState.ManualTesting)
            {
                Globalo.LogPrint("ManualControl", "[INFO] MANUAL 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }

            if (Globalo.threadControl.manualThread.GetThreadRun() == false)
            {
                Globalo.threadControl.manualThread.runfn(FThread.ManualThread.eManualType.M_EEPROM_READ);
            }
            

            

            //EEPROM_TotalRead_Type2(0x0000, 0x513, CompareEEpromData, 512);//최대 32씩만	0x512	0x46D
        }
        
        private void CCdControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                ShowEEpromGrid();
                //Chart roi on
                if (ComboBox_IniList.Items.Contains(Globalo.mLaonGrabberClass.currentIniFile))
                {
                    ComboBox_IniList.SelectedItem = Globalo.mLaonGrabberClass.currentIniFile; // 해당 값을 선택
                }
                SetImageInfo();
                if (ProgramState.ON_LINE_MIL)
                {
                    //SetSfrRoi();
                    //DrawRectSfr(999);
                }

            }
            else
            {
                //if (Globalo.threadControl.manualThread.GetThreadRun())
                //{
                //    Globalo.threadControl.manualThread.Stop();
                //}
            }
        }


        private void BTN_CCD_ROI_SAVE_Click(object sender, EventArgs e)
        {
            //GetSfrRoi();

            //Globalo.yamlManager.imageDataSave();
        }

        
    }
}
