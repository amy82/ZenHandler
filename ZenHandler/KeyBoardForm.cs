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
    public partial class KeyBoardForm : Form
    {
        public string KeyValue { get; set; }
        public KeyBoardForm(string inputTxt = "")
        {
            InitializeComponent();
            this.CenterToScreen();
            KeyValue = inputTxt;
            textBox_KeyVal.Text = KeyValue;
            textBox_KeyVal.Padding = new Padding(0, (textBox_KeyVal.Height - textBox_KeyVal.Font.Height) / 2, 0, 0); // 세로 중앙 정렬


            int cursorPosition = textBox_KeyVal.TextLength;
            // 커서를 텍스트박스의 가장 뒤로 이동
            textBox_KeyVal.SelectionStart = cursorPosition;// textBox_KeyVal.Text.Length;
            textBox_KeyVal.SelectionLength = 0; // 텍스트 선택 영역을 없앰
            textBox_KeyVal.Focus(); // 텍스트박스에 포커스 설정 (필요시)
        }

        private void KeyBoardForm_Load(object sender, EventArgs e)
        {
            // 폼에 있는 모든 버튼에 Click 이벤트를 공통으로 연결
            foreach (Control control in this.Controls)
            {
                if (control is Button && control != textBox_KeyVal) // TextBox는 제외
                {
                    control.Click += KeyboardButton_Click;
                }
            }
        }
        // 공통 이벤트 처리기
        private void KeyboardButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                // 눌린 버튼의 텍스트나 다른 속성을 처리
                string buttonText = clickedButton.Text;

                switch (buttonText)
                {
                    case "ENTER":
                        HandleEnterKey();
                        break;
                    case "CLOSE":
                        HandleCloseKey();
                        break;
                    case "CLEAR":
                        HandleClearKey();
                        break;
                    case "del":
                        HandleDelKey();
                        break;
                    case "Backspace":
                        HandleBackspaceKey();
                        break;
                    default:
                        // 일반 키 입력 처리
                        HandleKeyPress(buttonText);
                        break;
                }
            }
        }
        private void HandleKeyPress(string key)
        {
            int cursorPosition = textBox_KeyVal.SelectionStart;

            // textBox_KeyVal.Text += key; // 텍스트박스에 글자 입력
            // 그 외 키는 텍스트를 추가
            textBox_KeyVal.Text = textBox_KeyVal.Text.Insert(cursorPosition, key);
            cursorPosition++; // 텍스트를 추가한 후 커서를 한 칸 이동


            // 커서를 텍스트박스의 가장 뒤로 이동
            textBox_KeyVal.SelectionStart = cursorPosition;// textBox_KeyVal.Text.Length;
            textBox_KeyVal.SelectionLength = 0; // 텍스트 선택 영역을 없앰
            textBox_KeyVal.Focus(); // 텍스트박스에 포커스 설정 (필요시)
        }
        // Close 키 처리
        private void HandleCloseKey()
        {
            // Enter 키 눌렀을 때의 동작 (예: 텍스트박스의 입력을 제출하거나 포커스를 변경)
            // 폼을 닫음
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        // Enter 키 처리
        private void HandleEnterKey()
        {

            // Enter 키 눌렀을 때의 동작 (예: 텍스트박스의 입력을 제출하거나 포커스를 변경)
            //MessageBox.Show("Enter 키가 눌렸습니다.");
            KeyValue = textBox_KeyVal.Text;

            // 폼을 닫음
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Clear 키 처리
        private void HandleClearKey()
        {
            // Clear 키 눌렀을 때의 동작 (예: 텍스트박스 비우기)
            textBox_KeyVal.Clear();
        }
        // Del 키 처리
        private void HandleDelKey()
        {
            // 현재 커서 위치
            int cursorPosition = textBox_KeyVal.SelectionStart;

            // 커서 위치에서 글자를 삭제 (선택된 텍스트가 있다면 그것을 삭제)
            if (textBox_KeyVal.SelectionLength > 0)
            {
                // 선택된 텍스트 삭제
                textBox_KeyVal.SelectedText = "";
            }
            else if (cursorPosition < textBox_KeyVal.Text.Length)
            {
                // 커서 오른쪽 글자 삭제
                textBox_KeyVal.Text = textBox_KeyVal.Text.Remove(cursorPosition, 1);
            }

            // 삭제 후, 커서 위치를 다시 복원
            textBox_KeyVal.SelectionStart = cursorPosition;
            textBox_KeyVal.SelectionLength = 0; // 선택된 텍스트가 없도록 설정

            // 커서가 보이게 하기 위해 Focus를 다시 설정
            textBox_KeyVal.Focus();
        }
        // Backspace 키 처리
        private void HandleBackspaceKey()
        {
            // 현재 커서 위치
            int cursorPosition = textBox_KeyVal.SelectionStart;

            // Backspace 키 눌렀을 때의 동작 (예: 커서 왼쪽의 글자 삭제)
            if (textBox_KeyVal.SelectionLength > 0)
            {
                textBox_KeyVal.SelectedText = "";
            }
            else if (textBox_KeyVal.SelectionStart > 0)
            {
                textBox_KeyVal.Text = textBox_KeyVal.Text.Remove(cursorPosition - 1, 1);
                textBox_KeyVal.SelectionStart = cursorPosition - 1;
            }
            else
            {
                textBox_KeyVal.SelectionStart = cursorPosition;
            }

            
            textBox_KeyVal.SelectionLength = 0; // 텍스트 선택 영역을 없앰
            textBox_KeyVal.Focus(); // 텍스트박스에 포커스 설정 (필요시)
        }

        private void BTN_KEYBOARD_ENTER_Click(object sender, EventArgs e)
        {

        }
    }
}
