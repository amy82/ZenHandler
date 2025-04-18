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
        private int PickerCount = 8;                            //Load Picket 4ea + UnLoad 4ea
        private int GridCol = 3;                                //picker , bcr , state
        private int[] StartPos = new int[] { 10, 40 };          //Grid Width
        private int[] inGridWid = new int[] { 80, 300, 70 };    //Grid Width

        private Controls.DefaultGridView dataGridView;

        public PickerInfo()
        {
            InitializeComponent();
            InitPickerGrid();
        }

        public void SetPickerInfo()
        {
            int i = 0;

            for (i = 0; i < 4; i++)
            {
                dataGridView[1, i].Value = Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].BcrLot;
                dataGridView[1, i + 4].Value = Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].BcrLot;

                if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Blank)
                {
                    dataGridView[2, i].Style.BackColor = Color.White;
                }
                else if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Bcr)
                {
                    dataGridView[2, i].Style.BackColor = Color.Yellow;
                }
                else if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Good)
                {
                    dataGridView[2, i].Style.BackColor = Color.LightGreen;
                }
                else
                {
                    dataGridView[2, i].Style.BackColor = Color.Red;
                }


                if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Blank)
                {
                    dataGridView[2, i+4].Style.BackColor = Color.White;
                }
                else if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Bcr)
                {
                    dataGridView[2, i + 4].Style.BackColor = Color.Yellow;
                }
                else if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Good)
                {
                    dataGridView[2, i + 4].Style.BackColor = Color.LightGreen;
                }
                else
                {
                    dataGridView[2, i + 4].Style.BackColor = Color.Red;
                }
                dataGridView[2, i].Value = Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State;
                dataGridView[2, i + 4].Value = Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State;
            }

            //dataGridView
            //dataGridView[0, 0] = Picker 제일 위에칸

            //dataGridView[1, (0 ~ 7)] = Lot 제일 위에칸 0 ~ 3 = Load , 4 ~ 7 = Unload

            //dataGridView[2, 0] = State 제일 위에칸

            //string formattedValue = "lot data";
            //dataGridView[1, 0].Value = formattedValue;
        }

        public void InitPickerGrid()
        {
            int i = 0;
            dataGridView = new Controls.DefaultGridView(GridCol, PickerCount, inGridWid);
            dataGridView.Location = new Point(StartPos[0], StartPos[1]);
            this.Controls.Add(dataGridView);

            string[] title = new string[] { "Picker", "Lot", "State" };         //Grid Width
            for (i = 0; i < dataGridView.ColumnCount; i++)
            {
                dataGridView.Columns[i].Name = title[i];
            }
            string posName = "";
            for (i = 0; i < PickerCount; i++)
            {
                if (i < 4)
                {
                    posName = "Load " + (i + 1).ToString();
                }
                else
                {
                    posName = "UnLoad " + (i + 1).ToString();
                }
                dataGridView.Rows[i].SetValues(posName);
            }
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
