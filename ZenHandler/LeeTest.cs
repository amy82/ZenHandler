using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler
{
    public partial class LeeTest : Form
    {
        public LeeTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Transfer 에서 Aoi 소켓으로 공급 완료 신호 보내기
            int i = 0;
            MotionControl.SocketReqArgs testsp = new MotionControl.SocketReqArgs(2);
            testsp.Index = 0;// Globalo.motionManager.transferMachine.NoSocketPos;

            for (i = 0; i < 2; i++)
            {
                if (Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].State == Machine.PickedProductState.Bcr)
                {
                    testsp.States[i] = 0;
                }

                //testsp.Barcode[i] = Globalo.motionManager.transferMachine.pickedProduct.LoadProductInfo[i].BcrLot;
            }
            testsp.Barcode[0] = "바코드 1번";
            testsp.Barcode[1] = "바코드 2번";
            Globalo.motionManager.transferMachine.CallSocketReqComplete(testsp);
        }
    }
}
