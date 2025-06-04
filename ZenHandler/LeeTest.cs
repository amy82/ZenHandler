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

        private void button3_Click(object sender, EventArgs e)
        {
            int ANum = 0;
            TcpSocket.MessageWrapper aoiEqipData = new TcpSocket.MessageWrapper();
            TcpSocket.TesterData tData = new TcpSocket.TesterData();
            tData.Cmd = "CMD_TEST";       //RESP_TEST_STEP1,  RESP_TEST_STEP2
            tData.socketNum = 1 + (ANum * 2);              //Left - R Socket
            tData.Name = "";
            tData.LotId[0] = "bcr1112";// Globalo.motionManager.socketAoiMachine.socketProduct.AoiSocketInfo[ANum][1].BcrLot;

            aoiEqipData.Type = "Tester";
            aoiEqipData.Data = tData;

            Globalo.tcpManager.SendMsgToTester(aoiEqipData, ANum); // pc 0 or pc 1
        }
    }
}
