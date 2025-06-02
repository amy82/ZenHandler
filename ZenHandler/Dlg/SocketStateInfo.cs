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
    public partial class SocketStateInfo : UserControl
    {
        //private Label[] StateLabels = new Label[16];
        SplitContainer[] AsplitContainers = new SplitContainer[4];
        SplitContainer[] BsplitContainers = new SplitContainer[4];
        SplitContainer[] CsplitContainers = new SplitContainer[4];
        SplitContainer[] DsplitContainers = new SplitContainer[4];
        public SocketStateInfo()
        {
            InitializeComponent();

            SocketInitialize();
        }
        private void SocketInitialize()
        {
            int i = 0;
            for (i = 0; i < 4; i++)
            {
                //StateLabels[i] = splitContainer1.Panel2.Controls["label_SocketState" + (i + 1)] as Label;
                //AsplitContainer4
                AsplitContainers[i] = this.Controls["AsplitContainer" + (i+1)] as SplitContainer;
                BsplitContainers[i] = this.Controls["BsplitContainer" + (i+1)] as SplitContainer;
                CsplitContainers[i] = this.Controls["CsplitContainer" + (i+1)] as SplitContainer;
                DsplitContainers[i] = this.Controls["DsplitContainer" + (i+1)] as SplitContainer;
            }
            //
            //
            if (Program.PG_SELECT == HANDLER_PG.AOI)
            {
                //splitContainers[3].Panel1.Controls["label_SocketTitle4"].Text = "SOCKET #2-2";

                for (i = 0; i < 2; i++)
                {
                    AsplitContainers[i + 2].Visible = false;
                    BsplitContainers[i + 2].Visible = false;
                }
                for (i = 0; i < 4; i++)
                {
                    CsplitContainers[i].Visible = false;
                    DsplitContainers[i].Visible = false;
                }
            }
            else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
            {
                for (i = 0; i < 4; i++)
                {
                    CsplitContainers[i].Visible = false;
                    DsplitContainers[i].Visible = false;
                }
            }
            else
            {
                //Fw = 16개 전부다 사용 
                //A 1 ~ 4
                //B 1 ~ 4
                //C 1 ~ 4
                //D 1 ~ 4
                //splitContainer1.Panel2.Controls["label_SocketState" + (i + 1)] as Label;
            }
        }

        public void SetUpdateSocket()
        {
            int i = 0;
            int j = 0;

            int TotalCnt = 4;
            int SocketCol = 2;
            int SocketRow = 4;
            //index 0 = A , 1 = B , 2 = C , 3 = D
            //socketProduct
            if (Program.PG_SELECT == HANDLER_PG.AOI)
            {
                //2개씩 2세트 = 4개
                TotalCnt = 2;
                SocketCol = 2;
                SocketRow = 2;
            }
            else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
            {
                //4개씩 2세트 = 8개.
                SocketCol = 4;
                SocketRow = 2;
            }
            else
            {
                //Fw
                //4개씩 4세트 = 16개
                SocketCol = 4;
                SocketRow = 4;

            }
            Label stateLabel = new Label();
            Machine.SocketProductState sState = new Machine.SocketProductState();
            Machine.AoiSocketProductState sAoiState = new Machine.AoiSocketProductState();

            for (i = 0; i < SocketRow; i++)
            {
                for (j = 0; j < SocketCol; j++)
                {
                    if (Program.PG_SELECT == HANDLER_PG.AOI)
                    {
                        if (i == 0)
                        {
                            stateLabel = AsplitContainers[j].Panel2.Controls["ASocketState" + (j + 1)] as Label;
                            stateLabel.Text = Globalo.motionManager.socketAoiMachine.socketProduct.SocketInfo_A[j].State.ToString();
                            sAoiState = Globalo.motionManager.socketAoiMachine.socketProduct.SocketInfo_A[j].State;
                        }
                        else if (i == 1)
                        {
                            stateLabel = BsplitContainers[j].Panel2.Controls["BSocketState" + (j + 1)] as Label;
                            stateLabel.Text = Globalo.motionManager.socketAoiMachine.socketProduct.SocketInfo_B[j].State.ToString();
                            sAoiState = Globalo.motionManager.socketAoiMachine.socketProduct.SocketInfo_B[j].State;
                        }
                    }
                    if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                    {
                        if (i == 0)
                        {
                            stateLabel = AsplitContainers[j].Panel2.Controls["ASocketState" + (j + 1)] as Label;
                            stateLabel.Text = Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[j].State.ToString();
                            sState = Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_A[j].State;
                        }
                        else if (i == 1)
                        {
                            stateLabel = BsplitContainers[j].Panel2.Controls["BSocketState" + (j + 1)] as Label;
                            stateLabel.Text = Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_B[j].State.ToString();
                            sState = Globalo.motionManager.socketEEpromMachine.socketProduct.SocketInfo_B[j].State;
                        }
                    }
                    if (Program.PG_SELECT == HANDLER_PG.FW)
                    {
                        if (i == 0)
                        {
                            stateLabel = AsplitContainers[j].Panel2.Controls["ASocketState" + (j + 1)] as Label;
                            stateLabel.Text = Globalo.motionManager.socketFwMachine.socketProduct.SocketInfo_A[j].State.ToString();
                            sState = Globalo.motionManager.socketFwMachine.socketProduct.SocketInfo_A[j].State;
                        }
                        else if (i == 1)
                        {
                            stateLabel = BsplitContainers[j].Panel2.Controls["BSocketState" + (j + 1)] as Label;
                            stateLabel.Text = Globalo.motionManager.socketFwMachine.socketProduct.SocketInfo_B[j].State.ToString();
                            sState = Globalo.motionManager.socketFwMachine.socketProduct.SocketInfo_B[j].State;
                        }
                        else if (i == 2)
                        {
                            stateLabel = CsplitContainers[j].Panel2.Controls["CSocketState" + (j + 1)] as Label;
                            stateLabel.Text = Globalo.motionManager.socketFwMachine.socketProduct.SocketInfo_C[j].State.ToString();
                            sState = Globalo.motionManager.socketFwMachine.socketProduct.SocketInfo_C[j].State;
                        }
                        else if (i == 3)
                        {
                            stateLabel = DsplitContainers[j].Panel2.Controls["DSocketState" + (j + 1)] as Label;
                            stateLabel.Text = Globalo.motionManager.socketFwMachine.socketProduct.SocketInfo_D[j].State.ToString();
                            sState = Globalo.motionManager.socketFwMachine.socketProduct.SocketInfo_D[j].State;
                        }
                    }


                    if (Program.PG_SELECT == HANDLER_PG.AOI)
                    {
                        if (sAoiState == Machine.AoiSocketProductState.Good)
                        {
                            stateLabel.BackColor = Color.Green;
                            stateLabel.ForeColor = Color.Yellow;
                        }
                        else if (sAoiState == Machine.AoiSocketProductState.NG)
                        {
                            stateLabel.BackColor = Color.Red;
                            stateLabel.ForeColor = Color.Black;
                        }
                        else
                        {
                            stateLabel.BackColor = Color.White;
                            stateLabel.ForeColor = Color.Black;
                        }
                    }
                    else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                    {

                    }
                    else
                    {
                        if (sState == Machine.SocketProductState.Good)
                        {
                            stateLabel.BackColor = Color.Green;
                            stateLabel.ForeColor = Color.Yellow;
                        }
                        else if (sState == Machine.SocketProductState.NG)
                        {
                            stateLabel.BackColor = Color.Red;
                            stateLabel.ForeColor = Color.Black;
                        }
                        else
                        {
                            stateLabel.BackColor = Color.White;
                            stateLabel.ForeColor = Color.Black;
                        }
                    }

                        
                }
            }

        }

        private void SocketStateInfo_Load(object sender, EventArgs e)
        {
            SetUpdateSocket();

        }
    }
}
