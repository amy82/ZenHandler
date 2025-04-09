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
        public enum TRAY_KIND
        {
            LOAD_TRAY = 0, UN_LOAD_TRAY, NG_TRAY
        };

        private TableLayoutPanel[] trayClass;

        public int[] TrayColCount = { 5, 5, 7 };        //Tray 가로 개수 (투입 , 배출 , NG)
        public int[] TrayRowCount = { 7, 7, 2 };        //Tray 세로 개수

        public TrayStateInfo()
        {
            InitializeComponent();

            
        }
        public void uiSet()
        {
            for (int i = 0; i < trayClass.Length; i++)
            {
                TrayInitSet(trayClass[i], TrayColCount[i], TrayRowCount[i]);       //가로 , 세로 개수
                
            }

            UpdateTrayColors(TRAY_KIND.LOAD_TRAY, 2, 3);        //가로 3번째 , 세로 4번째 로드 할 차례
            UpdateTrayColors(TRAY_KIND.UN_LOAD_TRAY, 3, 5); //가로 4 번째 , 세로 6 번째 배출 할 차례
            UpdateTrayColors(TRAY_KIND.NG_TRAY, 1, 1);      //가로 2 번째 , 세로 2 번째 배출 할 차례
        }


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
                            if (index == TRAY_KIND.UN_LOAD_TRAY)       //투입 tray만 index기준으로 제품이 사라진다.
                            {
                                panel.BackColor = Color.LightBlue;
                            }
                            else if (index == TRAY_KIND.NG_TRAY)       //투입 tray만 index기준으로 제품이 사라진다.
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
                            
                            if (index == TRAY_KIND.LOAD_TRAY)       //투입 tray만 index기준으로 제품이 사라진다.
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
        public void TrayInitSet(TableLayoutPanel tray, int widthCnt, int heightCnt)
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
                tableLayoutPanel_Load,
                tableLayoutPanel_Unload,
                tableLayoutPanel_Ng
            };
            
            uiSet();
        }
    }
}
