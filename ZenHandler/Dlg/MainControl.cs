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
        private UnitControl unitControl;

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
            //InitResultGrid();           //eeprom 결과 표시
        }
        public void RefreshMain()
        {
            //ShowModelName();
            //ShowRecipeName();
            //ShowVerifyResultGrid(50);
        }
        private void InitResultGrid()
        {
            //dataGridView_Result
            //EEP_ITEM / ADDRESS / DATA_SIZE / ITEM_VALUE  값을 Table 형태로 동적으로 출력, 항목별 OK / NG  표기
            int i = 0;
            //int j = 0;
            // 열 추가
            // 행 헤더 숨기기
            dataGridView_Result.RowHeadersVisible = false;
            //사이즈 조절 막기
            dataGridView_Result.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            // 행 높이 자동 조정
            dataGridView_Result.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // 모든 셀에 맞게 자동 조정

            // 열 자동 크기 조정
            dataGridView_Result.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//DataGridViewAutoSizeColumnsMode.None;

            // 또는
            // 셀 내용 줄바꿈
            dataGridView_Result.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // 헤더 폰트 설정
            dataGridView_Result.ColumnHeadersDefaultCellStyle.Font = new Font("맑은고딕", 8F, FontStyle.Regular);
            dataGridView_Result.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // 헤더 배경색 설정
            dataGridView_Result.ColumnHeadersDefaultCellStyle.BackColor = Globalo.GridHeaderBackColor;  //  LightGray;// Color.FromArgb(94, 129, 244); //Color.LightBlue;
            // 헤더 폰트 색
            dataGridView_Result.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;// .Gray;

            //dataGridView_Recipe.RowsDefaultCellStyle.BackColor = Color.White;
            //dataGridView_Recipe.RowsDefaultCellStyle.ForeColor = Color.Black;

            dataGridView_Result.RowsDefaultCellStyle.BackColor = Color.White;   //기본 배경
            dataGridView_Result.RowsDefaultCellStyle.ForeColor = Color.Black;    //기본 폰트 색

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

            dataGridView_Result.EnableHeadersVisualStyles = false;
            // 열 헤더 가운데 정렬
            //운영 체제의 기본 시각적 스타일을 무시
            dataGridView_Result.AllowUserToResizeRows = false;

            dataGridView_Result.ReadOnly = true;
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();

            cellStyle.Font = new Font("나눔고딕", 9, FontStyle.Regular); // Change font and size
            cellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; // Center align text
            cellStyle.SelectionBackColor = Color.Aquamarine;
            //cellStyle.SelectionForeColor = Color.Empty;

            dataGridView_Result.DefaultCellStyle = cellStyle;

            //DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();

            int gridWidth = dataGridView_Result.Width;
            DataGridViewTextBoxColumn[] textColumns = new DataGridViewTextBoxColumn[ResultGridColCount];

            for (i = 0; i < ResultGridColCount; i++)
            {
                textColumns[i] = new DataGridViewTextBoxColumn();
                textColumns[i].HeaderText = ResultTitle[i];
                dataGridView_Result.Columns.Add(textColumns[i]);
            }


            //textColumns[0].Name = "No";
            //textColumns[1].Name = "Model";

            //private string[] ResultTitle = { "Result", "EEP_ITEM", "ITEM_VALUE", "EEPROM", "ADDR", "SIZE" };
            //1, 2, 3 번은 가변
            for (i = 0; i < dataGridView_Result.ColumnCount; i++)
            {
                if(i == 1 || i == 2 || i == 3)
                {
                    dataGridView_Result.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView_Result.Columns[i].Resizable = DataGridViewTriState.True;
                }
                else
                {
                    dataGridView_Result.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dataGridView_Result.Columns[i].Resizable = DataGridViewTriState.False;
                }
                dataGridView_Result.Columns[i].Width = ResultGridColWidth[i];

                if(i == 0)
                {
                    dataGridView_Result.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    dataGridView_Result.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                
            }

            // 행 높이 조정
            dataGridView_Result.RowTemplate.Height = ResultGridRowHeight; // 자동 추가되는 행 높이 설정
            dataGridView_Result.ColumnHeadersHeight = ResultGridHeaderHeight;
            //dataGridView_Recipe.ColumnHeadersDefaultCellStyle.ForeColor = Color.Blue;

            //for (i = 0; i < ResultGridRowViewCount; i++)
            //{
            //    string text = $"예시 텍스트 {i}"; // 예시 텍스트 생성
            //    //bool isChecked = (i % 2 == 0); // 짝수인 경우 체크박스가 체크됨
            //    dataGridView_Result.Rows.Add("", "", "", "", ""); // 행 추가
            //    dataGridView_Result.Rows[i].Height = ResultGridRowHeight;

            //    for (j = 0; j < dataGridView_Result.ColumnCount; j++)
            //    {
            //        //dataGridView.Columns[i].Resizable = DataGridViewTriState.False;
            //        //dataGridView_Result.Columns[j].Width = GridColWidth[j];
            //        dataGridView_Result.Columns[j].Resizable = DataGridViewTriState.False;

            //    }
            //}

            dataGridView_Result.Height = ResultGridRowViewCount * ResultGridRowHeight + ResultGridHeaderHeight +2;// + 2;

            if (dataGridView_Result.AllowUserToAddRows == true)
            {
                //dataGridView_Model.Rows[GridRowCount].Height = GridRowHeight;
            }


            dataGridView_Result.MultiSelect = false; // 여러 개 선택 불가능
            dataGridView_Result.AllowUserToAddRows = false; // 빈 행 추가 방지
            dataGridView_Result.ScrollBars = ScrollBars.Both;//ScrollBars.Vertical;      //가로 스크롤 안보이게 설정

            //dataGridView_Result.CellContentClick += new DataGridViewCellEventHandler(RecipeGridView_CellContentClick);     //삭제 버튼 클릭시 사용
            // 버튼 클릭 이벤트 등록
            //dataGridView_Result.CellClick += new DataGridViewCellEventHandler(RecipeGridView_CellClick); //textbox 한번 클릭으로 바로 수정되게 추가
            //dataGridView_Result.SelectionChanged += dataGridView_Result_SelectionChanged;
            // 이벤트 핸들러 추가
            //CardGrid.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(CardGrid_EditingControlShowing);
            //dataGridView_Result.CellFormatting += dataGridView_Model_CellFormatting;

            RecipeGridWidth = dataGridView_Result.Width;     //<---스크롤 생겼을때 사이즈 조절위해 초기 Grid 넓이 저장

            // 각 컬럼의 헤더 텍스트 정렬 설정
            foreach (DataGridViewColumn column in dataGridView_Result.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //dataGridView_Model.Columns[0].ReadOnly = true; // 읽기 전용
            //dataGridView_Result.Columns[0].DefaultCellStyle.BackColor = Color.LightGray; // 배경색 설정
            //dataGridView_Result.Columns[0].DefaultCellStyle.ForeColor = Color.Yellow; // 배경색 설정
            //dataGridView_Result.Columns[0].DefaultCellStyle.Font = new Font("맑은고딕", 9F, FontStyle.Bold); // 굵은 글씨

            

            dataGridView_Result.ClearSelection();
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


        public void ShowVerifyResultGrid(List<Data.MesEEpromCsvData> _Mes_DataList, List<Data.EEpromReadData> _EEp_DataList)
        {
            int i = 0;  //옆

            //int dataCount = Globalo.yamlManager.MesData.SecGemData.Modellist.Count();

            
            if(_Mes_DataList.Count != _EEp_DataList.Count)
            {
                Console.WriteLine($"Mes Data, EEprom Data 개수가 다릅니다. {_Mes_DataList.Count} / {_EEp_DataList.Count}");
                return;
            }
            Console.WriteLine("Actual DataGridView Height: " + dataGridView_Result.Height);
            Console.WriteLine("Height:" + dataGridView_Result.RowTemplate.Height);
            int dataCount = _Mes_DataList.Count;
            if(dataCount< 1)
            {
                return;
            }
            dataGridView_Result.Rows.Clear();
            int gridViewCount = dataCount;

            if (gridViewCount < ResultGridRowViewCount)
            {
                gridViewCount = ResultGridRowViewCount;
            }

            for (i = 0; i < gridViewCount; i++)
            {
                if (i < dataCount)
                {
                    string eepData = _EEp_DataList[i].ITEM_VALUE;
                    string mesData = _Mes_DataList[i].ITEM_VALUE;

                    if (_Mes_DataList[i].DATA_FORMAT == "EMPTY" && _EEp_DataList[i].RESULT == "PASS")
                    {
                        //EMPTY 항목이 PASS 일 경우 0xFF 로만 표시한다.
                        eepData = "0xFF";
                        mesData = "0xFF";
                    }
                    //dataGridView_Result.Rows.Add(_EEp_DataList[i].RESULT, _Mes_DataList[i].EEP_ITEM, mesData, eepData, _Mes_DataList[i].ADDRESS, _Mes_DataList[i].DATA_SIZE);
                    dataGridView_Result.Rows.Add(_EEp_DataList[i].RESULT, _Mes_DataList[i].ITEM_CODE, mesData, eepData, _Mes_DataList[i].ADDRESS, _Mes_DataList[i].DATA_SIZE);

                    if (_EEp_DataList[i].RESULT == "PASS")
                    {
                        dataGridView_Result.Rows[i].Cells[0].Style.BackColor = Color.Green; // 1번 열
                    }
                    else
                    {
                        dataGridView_Result.Rows[i].Cells[0].Style.BackColor = Color.Red; // 1번 열
                    }
                }
                else
                {
                    dataGridView_Result.Rows.Add("", "", "", "", "", "");
                    
                }
                //dataGridView_Result.Rows[i].Cells[1].Style.BackColor = Color.White; // 1번 열
                //dataGridView_Result.Rows[i].Cells[1].Style.ForeColor = Color.Black; // 1번 열
                //dataGridView_Result.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
               // dataGridView_Result.Rows[i].Cells[1].Style.Font = new Font(dataGridView_Result.DefaultCellStyle.Font, FontStyle.Regular);
            }


            //lobalo.dataManage.eepromData.dataList

            dataGridView_Result.ClearSelection();
        }
        //public void ShowModelName()
        //{
        //    if (Globalo.yamlManager.secsGemDataYaml.MesData.SecGemData.CurrentModelName.Length < 1)
        //    {
        //        return;
        //    }
        //    textBox_Model.Text = Globalo.yamlManager.secsGemDataYaml.MesData.SecGemData.CurrentModelName;
        //}
        //public void ShowRecipeName()
        //{
        //    if (Globalo.dataManage.mesData.m_sMesPPID.Length < 1)
        //    {
        //        return;
        //    }

        //    textBox_Recipe.Text = Globalo.dataManage.mesData.m_sMesPPID;
        //}
        
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
