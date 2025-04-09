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
    public partial class InputForm : Form
    {
        public string InputText = "";

        public int InputMode = -1;  //1 = Password
        public InputForm(string title, string firstlabel, string value , int mode = 0)    //0이면 입력값 리턴
        {
            InitializeComponent();
            this.Text = title;
            this.InputMode = mode;
            this.Label1.Text = firstlabel;
            this.textBox_Input.Text = value;
            // KeyDown 이벤트 연결
            this.KeyPreview = true;  // 폼에서 키 입력을 먼저 감지하도록 설정
        }
        private bool passwordChk()
        {
            InputText = textBox_Input.Text;
            if (InputText == "1234")//Globalo.MODE_PASSWORD)     //1234
            {
                return true;
            }
            return false;
        }
        //public void OnInputFormCloseEvent(Keys key)
        public void OnInputFormCloseEvent(Keys key, string eventId)
        {
            

            if (this.InputMode == 1)
            {
                if (passwordChk() == false)
                {
                    SecretLabel.Visible = true;
                    return;
                }
            }

            Globalo.MainForm.keyMessageFilter.UnregisterKeyHandler(this.Name.ToString());

            //Globalo.MainForm.keyMessageFilter.KeyEvent -= this.OnInputFormCloseEvent;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void crownButton_ok_Click(object sender, EventArgs e)
        {
            if (this.InputMode == 1)
            {
                if (passwordChk() == false)
                {
                    SecretLabel.Visible = true;
                    return;
                }
            }
            else
            {
                //Input Text Return
                InputText = textBox_Input.Text;
            }
                
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void crownButton_cancel_Click(object sender, EventArgs e)
        {
            InputText = "";
            Globalo.MainForm.keyMessageFilter.UnregisterKeyHandler(this.Name.ToString());
            //Globalo.MainForm.keyMessageFilter.KeyEvent -= this.OnInputFormCloseEvent;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
