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
    public partial class TrayStateInfo : UserControl
    {
        public const int LoadTrayCol = 4;
        public const int LoadTrayRows = 11;

        public const int NgTrayCol = 2;
        public const int NgTrayRows = 4;


        public const int LeftRunPos = 1;    //0 = 왼쪽에서 부터 시작, 1 = 오른쪽에서 부터 시작
        public const int RightRunPos = 1;    //0 = 왼쪽에서 부터 시작, 1 = 오른쪽에서 부터 시작
        public enum TRAY_KIND
        {
            LOAD_TRAY_L = 0, LOAD_TRAY_R, NG_TRAY_L, NG_TRAY_R
        };

        public enum LoadTraySlotState
        {
            Empty = -1,
            BeforeTest = 0,
            AfterTest = 1
        }

        public enum NgTraySlotState
        {
            Empty = 0,
            NgInspection = 1
        }

        private TableLayoutPanel[] trayClass;
        public LoadTraySlotState[,] LeftTraySlots { get; set; }
        public LoadTraySlotState[,] RightTraySlots { get; set; }

        public NgTraySlotState[,] LeftNgTraySlots { get; private set; }
        public NgTraySlotState[,] RightNgTraySlots { get; private set; }



        public int[] TrayColCount = { LoadTrayCol, LoadTrayCol, NgTrayCol, NgTrayCol };        //Tray 가로 개수 (투입 , 배출 , NG)
        public int[] TrayRowCount = { LoadTrayRows, LoadTrayRows, NgTrayRows, NgTrayRows };        //Tray 세로 개수

        public TrayStateInfo()
        {
            InitializeComponent();

            LeftTraySlots = new LoadTraySlotState[LoadTrayRows, LoadTrayCol];
            RightTraySlots = new LoadTraySlotState[LoadTrayRows, LoadTrayCol];

            LeftNgTraySlots = new NgTraySlotState[NgTrayRows, NgTrayCol];
            RightNgTraySlots = new NgTraySlotState[NgTrayRows, NgTrayCol];


            LoadTrayInitialize(true);
            NgTrayInitialize(false);
        }

        public void LoadTrayInitialize(bool bFull)
        {
            for (int y = 0; y < LoadTrayRows; y++)
            {
                for (int x = 0; x < LoadTrayCol; x++)
                {
                    if (bFull)
                    {
                        LeftTraySlots[y, x] = LoadTraySlotState.BeforeTest;
                        RightTraySlots[y, x] = LoadTraySlotState.BeforeTest;
                    }
                    else
                    {
                        LeftTraySlots[y, x] = LoadTraySlotState.Empty;
                        RightTraySlots[y, x] = LoadTraySlotState.Empty;
                    }
                    
                }
            }
        }
        private void SetUpdateLoadTray(TRAY_KIND index)
        {
            int rows = trayClass[(int)index].RowCount;
            int cols = trayClass[(int)index].ColumnCount;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++) 
                {
                    // (col, row) 위치에 있는 Panel 찾기
                    Control panel = trayClass[(int)index].GetControlFromPosition(col, row);


                    if (panel is Panel)
                    {
                        if (index == TRAY_KIND.LOAD_TRAY_L)
                        {
                            if (Globalo.motionManager.liftMachine.trayProduct.LeftLoadTraySlots[row][col] == (int)LoadTraySlotState.BeforeTest)
                            {
                                panel.BackColor = Color.SkyBlue;
                            }
                            else if (Globalo.motionManager.liftMachine.trayProduct.LeftLoadTraySlots[row][col] == (int)LoadTraySlotState.AfterTest)
                            {
                                panel.BackColor = Color.Green;
                            }
                            else
                            {
                                panel.BackColor = Color.White;
                            }

                        }
                        else if (index == TRAY_KIND.LOAD_TRAY_R)
                        {
                            if (Globalo.motionManager.liftMachine.trayProduct.RightLoadTraySlots[row][col] == (int)LoadTraySlotState.BeforeTest)
                            {
                                panel.BackColor = Color.SkyBlue;
                            }
                            else if (Globalo.motionManager.liftMachine.trayProduct.RightLoadTraySlots[row][col] == (int)LoadTraySlotState.AfterTest)
                            {
                                panel.BackColor = Color.Green;
                            }
                            else
                            {
                                panel.BackColor = Color.White;
                            }
                        }
                        else if (index == TRAY_KIND.NG_TRAY_L)
                        {
                            if (Globalo.motionManager.liftMachine.trayProduct.LeftNgTraySlots[row][col] == (int)NgTraySlotState.NgInspection)
                            {
                                panel.BackColor = Color.Red;
                            }
                            else
                            {
                                panel.BackColor = Color.White;
                            }
                        }
                        else if (index == TRAY_KIND.NG_TRAY_R)
                        {
                            if (Globalo.motionManager.liftMachine.trayProduct.RightNgTraySlots[row][col] == (int)NgTraySlotState.NgInspection)
                            {
                                panel.BackColor = Color.Red;
                            }
                            else
                            {
                                panel.BackColor = Color.White;
                            }
                        }
                    }
                }
            }
        }
        
        public void SetLoadTraySlot(int row, int col, TRAY_KIND index, LoadTraySlotState state)
        {
            if (row >= 0 && row < LoadTrayRows && col >= 0 && col < LoadTrayCol)
            {
                if (index == TRAY_KIND.LOAD_TRAY_L)
                {
                    if (LeftRunPos == 0)
                    {
                        LeftTraySlots[row, col] = state;
                    }
                    else
                    {
                        LeftTraySlots[row, LoadTrayCol - col - 1] = state;
                    }
                    
                }
                else
                {
                    if (RightRunPos == 0)
                    {
                        RightTraySlots[row, col] = state;
                    }
                    else
                    {
                        RightTraySlots[row, LoadTrayCol - col - 1] = state;
                    }
                }
                
            }
        }
        public void SetNgTraySlot(int row, int col, TRAY_KIND index, NgTraySlotState state)
        {
            if (row >= 0 && row < NgTrayCol && col >= 0 && col < NgTrayRows)
            {
                if (index == TRAY_KIND.NG_TRAY_L)
                {
                    if (LeftRunPos == 0)
                    {
                        LeftNgTraySlots[row, col] = state;
                    }
                    else
                    {
                        LeftNgTraySlots[row, NgTrayCol - col - 1] = state;
                    }

                }
                else if (index == TRAY_KIND.NG_TRAY_R)
                {
                    if (RightRunPos == 0)
                    {
                        RightNgTraySlots[row, col] = state;
                    }
                    else
                    {
                        RightNgTraySlots[row, NgTrayCol - col - 1] = state;
                    }
                }

            }
        }
        public void NgTrayInitialize(bool bFull)
        {
            for (int y = 0; y < NgTrayRows; y++)
            {
                for (int x = 0; x < NgTrayCol; x++)
                {
                    if (bFull)
                    {
                        LeftNgTraySlots[y, x] = NgTraySlotState.NgInspection;
                        RightNgTraySlots[y, x] = NgTraySlotState.NgInspection;
                    }
                    else
                    {
                        LeftNgTraySlots[y, x] = NgTraySlotState.Empty;
                        RightNgTraySlots[y, x] = NgTraySlotState.Empty;
                    }
                    
                }
            }
        }
        public void uiSet()
        {
            for (int i = 0; i < trayClass.Length; i++)
            {
                TrayInitSet(trayClass[i], TrayColCount[i], TrayRowCount[i]);       //가로 , 세로 개수
                
            }

            SetLoadTraySlot(0, 0, TRAY_KIND.LOAD_TRAY_L, LoadTraySlotState.AfterTest);

            SetLoadTraySlot(0, 0, TRAY_KIND.LOAD_TRAY_R, LoadTraySlotState.AfterTest);
            SetLoadTraySlot(0, 1, TRAY_KIND.LOAD_TRAY_R, LoadTraySlotState.AfterTest);
            SetLoadTraySlot(0, 2, TRAY_KIND.LOAD_TRAY_R, LoadTraySlotState.AfterTest);

            SetNgTraySlot(0, 0, TRAY_KIND.NG_TRAY_L, NgTraySlotState.NgInspection);

            SetNgTraySlot(0, 0, TRAY_KIND.NG_TRAY_R, NgTraySlotState.NgInspection);
            SetNgTraySlot(0, 1, TRAY_KIND.NG_TRAY_R, NgTraySlotState.NgInspection);


            SetUpdateLoadTray(TRAY_KIND.LOAD_TRAY_L);
            SetUpdateLoadTray(TRAY_KIND.LOAD_TRAY_R);
            SetUpdateLoadTray(TRAY_KIND.NG_TRAY_L);
            SetUpdateLoadTray(TRAY_KIND.NG_TRAY_R);

            //UpdateTrayColors(TRAY_KIND.LOAD_TRAY_L, 2, 3);      //가로 3번째 , 세로 4번째 로드 할 차례
            //UpdateTrayColors(TRAY_KIND.LOAD_TRAY_R, 3, 5);      //가로 4 번째 , 세로 6 번째 배출 할 차례
            //UpdateTrayColors(TRAY_KIND.NG_TRAY_L, 1, 1);        //가로 2 번째 , 세로 2 번째 배출 할 차례
            //UpdateTrayColors(TRAY_KIND.NG_TRAY_R, 1, 1);        //가로 2 번째 , 세로 2 번째 배출 할 차례
        }
        
        //검사 전 제품 , 검사 후 제품 , 빈칸  = 3종류다
        private void UpdateTrayColors(TRAY_KIND index, int startCol, int startRow)
        {
            int rows = trayClass[(int)index].RowCount;
            int cols = trayClass[(int)index].ColumnCount;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    // (col, row) 위치에 있는 Panel 찾기
                    Control panel = trayClass[(int)index].GetControlFromPosition(col, row);


                    if (panel is Panel)
                    {
                        // 기준점 이전은 빈칸(연한 회색), 이후는 카메라(파란색)
                        if (row < startRow || (row == startRow && col < startCol))
                        {
                            //LOAD TRAY 는 제품 없는 상황
                            //배출 TRAY는 제품 있는 상황
                            if (index == TRAY_KIND.LOAD_TRAY_R)       //투입 tray만 index기준으로 제품이 사라진다.
                            {
                                panel.BackColor = Color.LightBlue;
                            }
                            else if (index == TRAY_KIND.NG_TRAY_L || index == TRAY_KIND.NG_TRAY_R)       //투입 tray만 index기준으로 제품이 사라진다.
                            {
                                panel.BackColor = Color.Red;
                            }
                            else
                            {
                                panel.BackColor = Color.White;
                            }
                            
                        }
                        else
                        {
                            if (index == TRAY_KIND.LOAD_TRAY_L)       //투입 tray만 index기준으로 제품이 사라진다.
                            {
                                panel.BackColor = Color.Aqua;
                            }
                            else
                            {
                                panel.BackColor = Color.White;
                            }
                        }
                    }
                }
            }
        }
        public void TrayInitSet(TableLayoutPanel tray, int widthCnt, int heightCnt)     //처음 칸 조절
        {
            tray.ColumnCount = widthCnt;
            tray.RowCount = heightCnt;
            tray.Dock = DockStyle.None;
            tray.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;

            tray.RowStyles.Clear();
            tray.ColumnStyles.Clear();

            // 각 행을 동일한 비율로 설정 (5행)
            for (int i = 0; i < tray.RowCount; i++)
            {
                tray.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / tray.RowCount));
            }

            // 각 열을 동일한 비율로 설정 (7열)
            for (int i = 0; i < tray.ColumnCount; i++)
            {
                tray.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / tray.ColumnCount));
            }

            // 각 셀에 Panel 추가
            for (int row = 0; row < tray.RowCount; row++)
            {
                for (int col = 0; col < tray.ColumnCount; col++)
                {
                    Panel panel = new Panel
                    {
                        Margin = new Padding(0),
                        Padding = new Padding(0),
                        BackColor = Color.White,  // 패널 색상 설정
                        Dock = DockStyle.Fill         // 셀 크기에 맞게 자동 조정
                    };
                    //        panel.Click += (s, e) =>
                    //        {
                    //            panel.BackColor = (panel.BackColor == Color.Gray) ? Color.Green : Color.Gray;
                    //        };
                    tray.Controls.Add(panel, col, row);
                }
            }
        }

        private void TrayStateInfo_Load(object sender, EventArgs e)
        {
            trayClass = new TableLayoutPanel[]
            {
                tableLayoutPanel_Tray_L,
                tableLayoutPanel_Tray_R,
                tableLayoutPanel_Ng_L,
                tableLayoutPanel_Ng_R
            };
            
            uiSet();
        }
    }
}
