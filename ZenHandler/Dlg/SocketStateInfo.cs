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
        SplitContainer[] splitContainers = new SplitContainer[16];
        public SocketStateInfo()
        {
            InitializeComponent();

            SocketInitialize();
        }
        private void SocketInitialize()
        {
            int i = 0;
            for (i = 0; i < 16; i++)
            {
                //StateLabels[i] = splitContainer1.Panel2.Controls["label_SocketState" + (i + 1)] as Label;
                splitContainers[i] = this.Controls["splitContainer" + (i+1)] as SplitContainer;
            }
            if (Program.PG_SELECT == HANDLER_PG.AOI)
            {
                splitContainers[2].Panel1.Controls["label_SocketTitle3"].Text = "SOCKET #2-1";
                splitContainers[3].Panel1.Controls["label_SocketTitle4"].Text = "SOCKET #2-2";

                for (i = 4; i < 16; i++)
                {
                    splitContainers[i].Visible = false;
                }
            }
            else if (Program.PG_SELECT == HANDLER_PG.EEPROM)
            {
                for (i = 8; i < 16; i++)
                {
                    splitContainers[i].Visible = false;
                }
            }
            else
            {
                //Fw
            }
        }

        private void SetUpdateSocket(int index)
        {

        }
    }
}
