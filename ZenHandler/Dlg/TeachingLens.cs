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
    public partial class TeachingLens : UserControl
    {
        private MotorControl motorControl;
        //public DataGridView LensTeachGridView = new DataGridView();
        private int[] inGridLensWid = new int[] { 150, 80, 80, 80, 80, 80 };
        private int[] inGridWid = new int[] { 150, 80, 80, 80, 80, 80, 80 };

        private Button[] TeachLensBtnArr = new Button[5];
        private const int nGridSensorRowCount = 6;
        private const int nGridSpeedRowCount = 3;
        private int nGridRowCount = Data.TeachingData.MAX_TEACHPOS_COUNT + nGridSensorRowCount + nGridSpeedRowCount;
        private Timer TeachingLensTimer;

        public int SelectLensAxis = -1;
        int dGridStartX = 180;
        int dGridStartY = 30;


        int dRowSensorHeight = 30;
        int dRowHeight = 45;

        public TeachingLens()
        {
            InitializeComponent();

            //motorControl = Globalo.motorControl;

            TeachLensGridInit();
            ManualLensUiSet();

            TeachingLensTimer = new Timer();
            TeachingLensTimer.Interval = 300; // 1초 (1000밀리초) 간격 설정
            TeachingLensTimer.Tick += new EventHandler(TeachingLens_Timer_Tick);


            changeMotorNo((int)MotorControl.eLensMotor.LENS_X);
        }

        public void ManualLensUiSet()
        {
            int i = 0;

            TeachLensBtnArr[0] = BTN_TEACH_LENS_X;
            TeachLensBtnArr[1] = BTN_TEACH_LENS_Y;
            TeachLensBtnArr[2] = BTN_TEACH_LENS_TY;
            TeachLensBtnArr[3] = BTN_TEACH_LENS_TX;
            TeachLensBtnArr[4] = BTN_TEACH_LENS_Z;

            for (i = 0; i < TeachLensBtnArr.Length; i++)
            {
                //TeachLensBtnArr[i].Text = motorControl.LENS_MOTOR_NAME[i];
                TeachLensBtnArr[i].BackColor = ColorTranslator.FromHtml("#C3A279");
                TeachLensBtnArr[i].ForeColor = Color.White;
            }
            
            BTN_TEACH_SERVO_ON.BackColor = ColorTranslator.FromHtml("#C3A279");
            BTN_TEACH_SERVO_ON.ForeColor = Color.White;

            BTN_TEACH_SERVO_OFF.BackColor = ColorTranslator.FromHtml("#C3A279");
            BTN_TEACH_SERVO_OFF.ForeColor = Color.White;

            BTN_TEACH_SERVO_RESET.BackColor = ColorTranslator.FromHtml("#C3A279");
            BTN_TEACH_SERVO_RESET.ForeColor = Color.White;

            // MotorBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
        }



        public void TeachLensGridInit()
        {
            //GRID
            int i = 0;
            int dGridWidth = 0;
            int dGridHeight = (nGridSpeedRowCount * dRowHeight) + (nGridSensorRowCount * dRowSensorHeight) + (Data.TeachingData.MAX_TEACHPOS_COUNT * dRowHeight);
            int scrollWidth = 3;// 20;
            //
            this.groupTeachLens.Controls.Add(LensTeachGridView);
            dGridWidth = inGridLensWid[0] + inGridLensWid[1] + inGridLensWid[2] + inGridLensWid[3] + inGridLensWid[4] + inGridLensWid[5];
            LensTeachGridView.ColumnCount = MotorControl.LENS_UNIT_COUNT + 1;// oGlobal.MAX_MOTOR_COUNT + 1;
            LensTeachGridView.EnableHeadersVisualStyles = false;
            LensTeachGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; //사이즈 조절 막기
            LensTeachGridView.RowCount = nGridRowCount;
            LensTeachGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            LensTeachGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Yellow;
            LensTeachGridView.ColumnHeadersDefaultCellStyle.Font = new Font(LensTeachGridView.Font, FontStyle.Bold);
            LensTeachGridView.AllowUserToResizeRows = false;
            LensTeachGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            LensTeachGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            LensTeachGridView.Name = "LensTeachGridView";
            LensTeachGridView.Location = new Point(dGridStartX, dGridStartY);
            LensTeachGridView.Size = new Size(dGridWidth + scrollWidth, dGridHeight + dRowHeight + 2);// dRowHeight * (nGridRowCount + 2));//dGridHeight);



            LensTeachGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            LensTeachGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            LensTeachGridView.GridColor = Color.Black;
            LensTeachGridView.RowHeadersVisible = false;
            LensTeachGridView.CellClick += TeachGrid_CellClick;
            //InGridContentChange(oGlobal.Maindata.dCurReadModuleCh);

            for (i = 0; i < MotorControl.LENS_UNIT_COUNT + 1; i++)
            {
                if (i > 0)
                {
                    //LensTeachGridView.Columns[i].Name = motorControl.LensMotorAxis[i - 1].Name;
                    LensTeachGridView.Columns[i].DefaultCellStyle.Format = "N3";     //소수점 3째자리 표현
                }
                LensTeachGridView.Columns[i].Resizable = DataGridViewTriState.False;
                LensTeachGridView.Columns[i].Width = inGridWid[i];
                LensTeachGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                LensTeachGridView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            LensTeachGridView.ColumnHeadersHeight = dRowHeight;
            for (i = 0; i < nGridRowCount; i++)
            {
                if (i < 6)
                {
                    LensTeachGridView.Rows[i].Height = dRowSensorHeight;
                }
                else
                {
                    LensTeachGridView.Rows[i].Height = dRowHeight;
                }
            }
            int index = 0;
            LensTeachGridView.Rows[0].SetValues("원점상태");
            LensTeachGridView.Rows[1].SetValues("ServoOn");
            LensTeachGridView.Rows[2].SetValues("Alarm");
            LensTeachGridView.Rows[3].SetValues("Limit(+)");
            LensTeachGridView.Rows[4].SetValues("HOME");
            LensTeachGridView.Rows[5].SetValues("Limit(-)");
            LensTeachGridView.Rows[6].SetValues("속도(mm/s)");
            LensTeachGridView.Rows[7].SetValues("가속도(sec)");

            for (i = 0; i < 6; i++)
            {
                //row header 선택 색 변화 금지
                LensTeachGridView.Rows[i].DefaultCellStyle.SelectionBackColor = LensTeachGridView.DefaultCellStyle.BackColor;
                LensTeachGridView.Rows[i].DefaultCellStyle.SelectionForeColor = LensTeachGridView.DefaultCellStyle.ForeColor;
            }
            for (i = 0; i < Data.TeachingData.MAX_TEACHPOS_COUNT; i++)
            {
                //LensTeachGridView.Rows[i + 8].SetValues(Globalo.dataManage.teachingData.TEACH_POS_NAME[index]);//TEACH_POS_NAME[index]);
                index++;


            }

            LensTeachGridView.Rows[nGridRowCount - 1].SetValues("현재위치");

            LensTeachGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            LensTeachGridView.ReadOnly = true;
            LensTeachGridView.CurrentCell = null;
            LensTeachGridView.MultiSelect = false;
        }
        private void TeachGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int nRow = e.RowIndex;      //세로줄 티칭위치
            int nCol = e.ColumnIndex;   //가로줄 모터
            string cellStr = "";
            changeMotorNo(nCol - 1, nRow);        //Grid Cell Click

            int RowLimit = 0;
            RowLimit = MotorControl.LENS_UNIT_COUNT;

            if ((nRow >= RowLimit && nRow < nGridRowCount - 1) && nCol >= 1)//if ((nRow >= 6 && nRow < nGridRowCount - 1) && nCol >= 1)
            {
                cellStr = LensTeachGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();


                decimal decimalValue = 0;
                if (decimal.TryParse(cellStr, out decimalValue))
                {
                    // 소수점 형식으로 변환
                    string formattedValue = decimalValue.ToString("0.00");//("0.0##");
                    NumPadForm popupForm = new NumPadForm(formattedValue);
                    if (popupForm.ShowDialog() == DialogResult.OK)
                    {
                        double dNumData = Double.Parse(popupForm.NumPadResult);
                        switch (nRow)
                        {
                            case 6: //속도
                                if (dNumData < 1.0)
                                {
                                    dNumData = 1.0;
                                }
                                if (dNumData > 100.0)
                                {
                                    dNumData = 100.0;
                                }
                                break;
                            case 7: //가속도
                                if (dNumData <= 0.0)
                                {
                                    dNumData = 0.1;
                                }
                                if (dNumData > 1.0)
                                {
                                    dNumData = 1.0;
                                }
                                break;
                        }
                        LensTeachGridView[e.ColumnIndex, e.RowIndex].Value = dNumData.ToString("0.00"); ;

                    }
                }
            }
        }
        public void showPanel()
        {
            if (ProgramState.ON_LINE_MOTOR == true)
            {
                TeachingLensTimer.Start();
            }
            ShowTeachingData();
        }
        public void hidePanel()
        {
            TeachingLensTimer.Stop();
        }
        public void getDataPanel()
        {
            //////GetTeachData();
        }
        private void ShowTeachingData()
        {
            //int i = 0;
            //int j = 0;
            //string formattedValue = "";

            //for (i = 0; i < MotorControl.LENS_UNIT_COUNT; i++)        //모터 수
            //{
            //    formattedValue = Globalo.dataManage.teachingData.LensMotorData.dMotorVel[i].ToString("0.00#");
            //    LensTeachGridView[i + 1, 6].Value = formattedValue;  //속도
            //    formattedValue = Globalo.dataManage.teachingData.LensMotorData.dMotorAcc[i].ToString("0.00#");
            //    LensTeachGridView[i + 1, 7].Value = formattedValue;     //가속도
            //}


            //for (j = 0; j < Data.TeachingData.MAX_TEACHPOS_COUNT; j++)
            //{
            //    for (i = 0; i < MotorControl.LENS_UNIT_COUNT; i++)        //모터 수
            //    {
            //        formattedValue = Globalo.dataManage.teachingData.LensTeachData[j].dPos[i].ToString("0.00#");
            //        LensTeachGridView[i + 1, 8 + j].Value = formattedValue;
            //    }

            //}


        }
        private void GetTeachData()
        {
            //int i = 0;
            //int j = 0;
            //double doubleValue = 0.0;
            //int mUnitCount = 0;
            //string cellValue = "";

            //mUnitCount = MotorControl.LENS_UNIT_COUNT;

            //for (i = 6; i < nGridRowCount - 1; i++)
            //{
            //    //for (j = 0; j < MotorControl.MAX_MOTOR_COUNT; j++)
            //    for (j = 0; j < mUnitCount; j++)
            //    {
            //        cellValue = LensTeachGridView.Rows[i].Cells[j + 1].Value.ToString();
            //        if (double.TryParse(cellValue, out doubleValue))
            //        {
            //            switch (i)
            //            {

            //                case 6: //속도(mm/s)
            //                    Globalo.dataManage.teachingData.LensMotorData.dMotorVel[j] = (int)doubleValue;
            //                    break;
            //                case 7: //가속도(sec)
            //                    Globalo.dataManage.teachingData.LensMotorData.dMotorAcc[j] = doubleValue;
            //                    break;
            //                case 8: //WaitPos
            //                case 9: //alignPos
            //                case 10: //LaserPos
            //                case 11: //ChartPos
            //                case 12: //OcPos
            //                    Globalo.dataManage.teachingData.LensTeachData[i - 8].dPos[j] = doubleValue;
            //                    break;
            //            }
            //        }
            //    }
            //}
        }
        private void TeachingLens_Timer_Tick(object sender, EventArgs e)
        {
            int i = 0;
            // 현재 시간을 Label에 표시
            //timeLabel.Text = "Current Time: " + DateTime.Now.ToString("HH:mm:ss");
            //모터 센서 감지 상태
            //모터 현재 위치

             //LENS UNIT
            for (i = 0; i < MotorControl.LENS_UNIT_COUNT; i++)
            {
                //LensTeachGridView[i + 1, 0] = 원점 상태
                if (motorControl.LensMotorAxis[i].bOrgState == true)
                {
                    LensTeachGridView[i + 1, 0].Style.BackColor = Color.LightGreen;
                }
                else
                {
                    LensTeachGridView[i + 1, 0].Style.BackColor = Color.White;
                }
                if (motorControl.LensMotorAxis[i].GetServoOn() == true)
                {
                    LensTeachGridView[i + 1, 1].Style.BackColor = Color.LightGreen;
                }
                else
                {
                    LensTeachGridView[i + 1, 1].Style.BackColor = Color.White;
                }
                if (motorControl.LensMotorAxis[i].GetAmpFault() == true)
                {
                    LensTeachGridView[i + 1, 2].Style.BackColor = Color.Red;
                }
                else
                {
                    LensTeachGridView[i + 1, 2].Style.BackColor = Color.White;
                }
                if (motorControl.LensMotorAxis[i].GetPosiSensor() == true)
                {
                    LensTeachGridView[i + 1, 3].Style.BackColor = Color.Red;
                }
                else
                {
                    LensTeachGridView[i + 1, 3].Style.BackColor = Color.White;
                }
                if (motorControl.LensMotorAxis[i].GetHomeSensor() == true)
                {
                    LensTeachGridView[i + 1, 4].Style.BackColor = Color.Green;
                }
                else
                {
                    LensTeachGridView[i + 1, 4].Style.BackColor = Color.White;
                }
                if (motorControl.LensMotorAxis[i].GetNegaSensor() == true)
                {
                    LensTeachGridView[i + 1, 5].Style.BackColor = Color.Red;
                }
                else
                {
                    LensTeachGridView[i + 1, 5].Style.BackColor = Color.White;
                }

                LensTeachGridView[i + 1, nGridRowCount - 1].Value = motorControl.LensMotorAxis[i].GetEncoderPos();
            }
            

        }

        private void changeMotorNo(int MotorNo, int nRow = -1)
        {
            int i = 0;
            if (MotorNo < 0)
            {
                return;
            }


            for (i = 0; i < TeachLensBtnArr.Length; i++)
            {
                TeachLensBtnArr[i].BackColor = ColorTranslator.FromHtml("#C3A279");
                TeachLensBtnArr[i].ForeColor = Color.White;
            }
            TeachLensBtnArr[MotorNo].BackColor = ColorTranslator.FromHtml("#4C4743");


            for (i = 0; i < Data.TeachingData.MAX_TEACHPOS_COUNT + 3; i++)
            {
                LensTeachGridView[SelectLensAxis + 1, 6 + i].Style.BackColor = Color.White;
            }
            SelectLensAxis = (int)MotorNo;
            for (i = 0; i < Data.TeachingData.MAX_TEACHPOS_COUNT + 3; i++)
            {
                LensTeachGridView[SelectLensAxis + 1, 6 + i].Style.BackColor = ColorTranslator.FromHtml("#E1E0DF");     //E1E0DF, FFB230
                //Color.BurlyWood;
            }

            //
            for (i = 0; i < MotorControl.LENS_UNIT_COUNT + 1; i++)
            {
                LensTeachGridView.Columns[i].HeaderCell.Style.BackColor = Color.White; //ColorTranslator.FromHtml("#E1E0DF");
                //Color.Aqua;
            }

            //int nCol = (int)MotorNo + 1;
            //LensTeachGridView.Columns[nCol].HeaderCell.Style.BackColor = Color.BurlyWood;

        }

        private void BTN_TEACH_LENS_X_Click(object sender, EventArgs e)
        {
            changeMotorNo((int)MotorControl.eLensMotor.LENS_X);
        }

        private void BTN_TEACH_LENS_Y_Click(object sender, EventArgs e)
        {
            changeMotorNo((int)MotorControl.eLensMotor.LENS_Y);
        }

        private void BTN_TEACH_LENS_Z_Click(object sender, EventArgs e)
        {
            changeMotorNo((int)MotorControl.eLensMotor.LENS_Z);
        }

        private void BTN_TEACH_LENS_TX_Click(object sender, EventArgs e)
        {
            changeMotorNo((int)MotorControl.eLensMotor.LENS_TX);
        }

        private void BTN_TEACH_LENS_TY_Click(object sender, EventArgs e)
        {
            changeMotorNo((int)MotorControl.eLensMotor.LENS_TY);
        }

        private void BTN_TEACH_SERVO_ON_Click_1(object sender, EventArgs e)
        {
            motorControl.LensMotorAxis[SelectLensAxis].AmpEnable();
        }

        private void BTN_TEACH_SERVO_OFF_Click_1(object sender, EventArgs e)
        {
            motorControl.LensMotorAxis[SelectLensAxis].AmpDisable();
        }

        private void BTN_TEACH_SERVO_RESET_Click_1(object sender, EventArgs e)
        {
            motorControl.LensMotorAxis[SelectLensAxis].AmpFaultReset();
        }
    }
}
