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
                //splitContainers[2].Panel1.Controls["label_SocketTitle3"].Text = "SOCKET #2-1";
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
                //Fw
            }
        }

        private void SetUpdateSocket(int index)
        {
            //socketProduct
            if (Program.PG_SELECT == HANDLER_PG.AOI)
            {
                //2개씩 2세트 = 4개
            }
            else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
            {
                //4개씩 2세트 = 8개
            }
            else
            {
                //Fw
                //4개씩 4세트 = 16개
            }
        }
    }
}
