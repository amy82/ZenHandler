using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Controls
{
    public class TeachingGridView : DataGridView
    {
        private Timer TeachingTimer;

        private int[] inGridWid;       //GRID Cell Width
        private const int nGridSensorRowCount = 6;          //감지 센서 표시 칸  개수 - Home , Limit...
        private const int nGridSpeedRowCount = 4;           //모터 설정 표시 칸 개수 - 속도, 가감속
        private string PointFormat = "0.0###";
        private int selectStartRow = nGridSensorRowCount;     //모터 선택하면 색 변하는 Cell
        private int dRowSensorHeight = 16;
        private int dRowHeight = 26;
        //
        private string ColorSelecttGrid = "#E1E0DF";       //FFB230
        //
        //
        private int nGridRowCount = 0;              //Grid 총 Row / 세로 칸 수
        public int SelectAxisIndex = -1;        //선택 모터 순서
        private Data.TeachingConfig teachingData;
        private MotionControl.MotorAxis[] motorList;
        //
        //
        public TeachingGridView(MotionControl.MotorAxis[] motorAxes , Data.TeachingConfig _teachingData, int[] _inGridWid)
        {
            //어떤 Machine 의 티칭정보인지
            //
            //
            //모터정보 List , 티칭 정보 List
            motorList = motorAxes;
            teachingData = _teachingData;
            inGridWid = _inGridWid;
            nGridRowCount = nGridSensorRowCount + nGridSpeedRowCount;

            InitializeGrid();



            TeachingTimer = new Timer();
            TeachingTimer.Interval = 300; // 1초 (1000밀리초) 간격 설정
            TeachingTimer.Tick += new EventHandler(Teaching_Timer_Tick);

            changeMotorNo(0);
        }
        public void MotorStateRun(bool bFlag)
        {
            if(bFlag)
            {
                TeachingTimer.Start();
            }
            else
            {
                TeachingTimer.Stop();
            }
        }
        public Data.TeachingConfig GetTeachData(Data.TeachingConfig tData)
        {
            int i = 0;
            int j = 0;
            Data.TeachingConfig tempData = tData;       // new Data.TeachingDataList();
            tempData.Speed = new List<double>();
            tempData.Accel = new List<double>();
            tempData.Decel = new List<double>();
            tempData.Teaching = new List<Data.TeachingPos>();


            double doubleValue = 0.0;
            string cellValue = "";

            for (i = nGridSensorRowCount; i < nGridRowCount - 1; i++)
            {
                Data.TeachingPos posTemp = new Data.TeachingPos();
                posTemp.Pos = new List<double>();


                for (j = 0; j < motorList.Length; j++)        //모터 수
                {
                    cellValue = this.Rows[i].Cells[j + 1].Value.ToString();
                    //
                    if (double.TryParse(cellValue, out doubleValue))
                    {
                        switch (i)
                        {
                            case 6: //속도
                                tempData.Speed.Add(doubleValue);
                                break;
                            case 7: //가속도
                                tempData.Accel.Add(doubleValue);
                                break;
                            case 8: //감속도
                                tempData.Decel.Add(doubleValue);
                                break;
                            default:
                                posTemp.Name = this.Rows[i].Cells[0].Value.ToString();
                                posTemp.Pos.Add(doubleValue);

                                
                                break;
                        }
                    }
                   
                }
                if (i > nGridSensorRowCount + nGridSpeedRowCount - 1 || posTemp.Pos.Count > 0)
                {
                    tempData.Teaching.Add(posTemp);
                }
                
            }
            return tempData;
        }
        public void ShowTeachingData()
        {
            int i = 0;
            int j = 0;
            //double dpos = 0.0;
            string formattedValue = "";

            for (i = 0; i < motorList.Length; i++)
            {
                formattedValue = teachingData.Speed[i].ToString(PointFormat);//, CultureInfo.InvariantCulture);     //
                this[1 + i, selectStartRow + 0].Value = formattedValue;
                formattedValue = teachingData.Accel[i].ToString(PointFormat);//, CultureInfo.InvariantCulture);
                this[1 + i, selectStartRow + 1].Value = formattedValue;
                formattedValue = teachingData.Decel[i].ToString(PointFormat);//, CultureInfo.InvariantCulture);
                this[1 + i, selectStartRow + 2].Value = formattedValue;
            }

            for (i = 0; i < teachingData.Teaching.Count; i++)
            {
                for (j = 0; j < teachingData.Teaching[i].Pos.Count; j++)
                {
                    formattedValue = teachingData.Teaching[i].Pos[j].ToString(PointFormat, CultureInfo.InvariantCulture);
                    this[1 + j, 9 + i].Value = formattedValue;
                }
            }
        }
        private void InitializeGrid()
        {
            //GRID
            int i = 0;
            int TeachingPosCount = teachingData.Teaching.Count;

            int dGridHeight = (nGridSpeedRowCount * dRowHeight) + (nGridSensorRowCount * dRowSensorHeight) + (TeachingPosCount * dRowHeight);
            int scrollWidth = 3;// 20;


            int dGridWidth = 0;
            for (i = 0; i < inGridWid.Length; i++)
            {
                dGridWidth += inGridWid[i];
            }

            nGridRowCount += TeachingPosCount;

            this.ColumnCount = motorList.Length + 1;
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

            this.Name = "TransferTeachGrid";
            this.Size = new Size(dGridWidth + scrollWidth, dGridHeight + dRowHeight + 2);
            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            this.GridColor = Color.Black;
            this.RowHeadersVisible = false;
            this.CellClick += TeachGrid_CellClick;
            this.CellDoubleClick += TeachGrid_CellDoubleClick;


            for (i = 0; i < this.ColumnCount; i++)
            {
                if (i > 0)
                {
                    this.Columns[i].Name = Globalo.motionManager.transferMachine.axisName[i - 1];
                    this.Columns[i].DefaultCellStyle.Format = "N3";     //소수점 3째자리 표현
                }
                this.Columns[i].Resizable = DataGridViewTriState.False;
                this.Columns[i].Width = inGridWid[i];
                this.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                if (i == 0)
                {
                    this.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else
                {
                    this.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                
            }


            this.ColumnHeadersHeight = dRowHeight;
            for (i = 0; i < nGridRowCount; i++)
            {
                if (i < nGridSensorRowCount)    //센서 Cell , 값 표시 Cell 높이 구분
                {
                    this.Rows[i].Height = dRowSensorHeight;
                }
                else
                {
                    this.Rows[i].Height = dRowHeight;
                }
            }

            for (i = 0; i < MotionControl.MotorSet.TEACH_SET_MENU.Length; i++)
            {
                this.Rows[i].SetValues(MotionControl.MotorSet.TEACH_SET_MENU[i]);      //원전  , home ,limit 등
            }

            for (i = 0; i < nGridSensorRowCount; i++)
            {
                //row header 선택 색 변화 금지
                this.Rows[i].DefaultCellStyle.SelectionBackColor = this.DefaultCellStyle.BackColor;
                this.Rows[i].DefaultCellStyle.SelectionForeColor = this.DefaultCellStyle.ForeColor;
            }
            string posName = "";
            for (i = 0; i < TeachingPosCount; i++)
            {
                //posName = Globalo.motionManager.transferMachine.TeachingPos[i];
                posName = teachingData.Teaching[i].Name;

                this.Rows[i + (nGridSensorRowCount + nGridSpeedRowCount - 1)].SetValues(posName);
            }


            this.Rows[nGridRowCount - 1].SetValues("현재위치");
            this[1, nGridRowCount - 1].Value = "0.0";
            this[2, nGridRowCount - 1].Value = "0.0";
            this[3, nGridRowCount - 1].Value = "0.0";

            this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ReadOnly = true;
            this.CurrentCell = null;
            this.MultiSelect = false;
        }
        private void Teaching_Timer_Tick(object sender, EventArgs e)
        {
            int i = 0;
            for (i = 0; i < motorList.Length; i++)
            {
                //this[i + 1, 0] = 원점 상태

                if (motorList[i].OrgState == true)
                {
                    this[i + 1, 0].Style.BackColor = Color.LightGreen;
                }
                else
                {
                    this[i + 1, 0].Style.BackColor = Color.White;
                }
                if (motorList[i].GetServoState() == true)
                {
                    this[i + 1, 1].Style.BackColor = Color.LightGreen;
                }
                else
                {
                    this[i + 1, 1].Style.BackColor = Color.White;
                }
                if (motorList[i].GetAmpFault() == true)
                {
                    this[i + 1, 2].Style.BackColor = Color.Red;
                }
                else
                {
                    this[i + 1, 2].Style.BackColor = Color.White;
                }
                if (motorList[i].GetPosiSensor() == true)
                {
                    this[i + 1, 3].Style.BackColor = Color.Red;
                }
                else
                {
                    this[i + 1, 3].Style.BackColor = Color.White;
                }
                if (motorList[i].GetHomeSensor() == true)
                {
                    this[i + 1, 4].Style.BackColor = Color.Green;
                }
                else
                {
                    this[i + 1, 4].Style.BackColor = Color.White;
                }
                if (motorList[i].GetNegaSensor() == true)
                {
                    this[i + 1, 5].Style.BackColor = Color.Red;
                }
                else
                {
                    this[i + 1, 5].Style.BackColor = Color.White;
                }

                this[i + 1, nGridRowCount - 1].Value = motorList[i].EncoderPos;// GetEncoderPos();
            }

        }
        public void changeMotorNo(int MotorNo)
        {
            int i = 0;
            if (MotorNo < 0)
            {
                return;
            }
            //
            for (i = 0; i < teachingData.Teaching.Count + nGridSpeedRowCount; i++)
            {
                this[SelectAxisIndex + 1, selectStartRow + i].Style.BackColor = Color.White;
            }

            SelectAxisIndex = (int)MotorNo;

            for (i = 0; i < teachingData.Teaching.Count + nGridSpeedRowCount; i++)
            {
                this[SelectAxisIndex + 1, selectStartRow + i].Style.BackColor = ColorTranslator.FromHtml(ColorSelecttGrid);
            }

            for (i = 0; i < motorList.Length + 1; i++)
            {
                this.Columns[i].HeaderCell.Style.BackColor = Color.White;
            }
        }
        /// <summary>
        /// 
        /// </summary>

        private void TeachGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int nRow = e.RowIndex;      //세로줄 티칭위치
            int nCol = e.ColumnIndex;   //가로줄 모터

            int RowLimit = nGridSensorRowCount + nGridSpeedRowCount - 1;    //-1은 현재위치 칸

            if ((nRow >= RowLimit && nRow < nGridRowCount - 1) && nCol == 0)
            {
                string cellStr = this.Rows[nGridRowCount - 1].Cells[SelectAxisIndex + 1].Value.ToString();
                this[SelectAxisIndex + 1, e.RowIndex].Value = cellStr;

            }
            if (nRow == -1 && nCol > 0)
            {
                ///changeMotorNo(nCol - 1);    //2번째부터 시작이라서 -1 , 바깥 축선택 에 전달이 안돼서 주석처리
            }
        }
        private void TeachGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int nRow = e.RowIndex;      //세로줄 티칭위치
            int nCol = e.ColumnIndex;   //가로줄 모터

            ////changeMotorNo(nCol - 1);        //Grid Cell Click

            int RowLimit = 0;

            RowLimit = nGridSensorRowCount - 1;         //센서 감지 Cell 제외

            if ((nRow > RowLimit && nRow < nGridRowCount - 1) && nCol >= 1)
            {
                string cellStr = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                decimal decimalValue = 0;

                if (decimal.TryParse(cellStr, out decimalValue))
                {
                    // 소수점 형식으로 변환
                    string formattedValue = decimalValue.ToString(PointFormat);
                    NumPadForm popupForm = new NumPadForm(formattedValue);

                    if (popupForm.ShowDialog() == DialogResult.OK)
                    {
                        double dNumData = double.Parse(popupForm.NumPadResult);
                        switch (nRow)
                        {
                            case 6: //속도
                                if (dNumData < MotionControl.MotorSet.MinLimitspeed)
                                {
                                    dNumData = MotionControl.MotorSet.MinLimitspeed;
                                }
                                if (dNumData > MotionControl.MotorSet.MaxLimitSpeed)
                                {
                                    dNumData = MotionControl.MotorSet.MaxLimitSpeed;
                                }
                                break;
                            case 7: //가속도
                            case 8: //감속도
                                if (dNumData < MotionControl.MotorSet.MinLimitAccDec)
                                {
                                    dNumData = MotionControl.MotorSet.MinLimitAccDec;
                                }
                                if (dNumData > MotionControl.MotorSet.MaxLimitAccDec)
                                {
                                    dNumData = MotionControl.MotorSet.MaxLimitAccDec;
                                }
                                break;
                        }
                        this[e.ColumnIndex, e.RowIndex].Value = dNumData.ToString(PointFormat);
                    }
                }
            }
        }
    }

    
}
