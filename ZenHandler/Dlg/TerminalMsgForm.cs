﻿using System;
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
    public partial class TerminalMsgForm : Form
    {
        private const int TermianlGridRowViewCount = 8;       //MAX ALARM COUNT
        public TerminalMsgForm()
        {
            InitializeComponent();
            //dataGridView_TerminalMsg
            InitTerminalGrid();
        }

        private void button_Terminal_Close_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
        private void ShowTMsgGrid()
        {
            
        }
        private void InitTerminalGrid()
        {
            // 데이터 그리드 뷰 설정
            dataGridView_TerminalMsg.ColumnCount = 2; // 컬럼 2개 생성
            dataGridView_TerminalMsg.Columns[0].Name = "Time"; // 첫 번째 컬럼 (시간)
            dataGridView_TerminalMsg.Columns[1].Name = "Message"; // 두 번째 컬럼 (메시지)

            // 컬럼 크기 자동 조정
            dataGridView_TerminalMsg.Columns[0].Width = 130; // "시간" 컬럼 너비
            dataGridView_TerminalMsg.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // "메시지"는 남은 공간 차지

            // 읽기 전용 설정 (데이터 편집 불가)
            dataGridView_TerminalMsg.ReadOnly = true;

            // 행 추가 버튼 비활성화
            dataGridView_TerminalMsg.AllowUserToAddRows = false;

            // 정렬 기능 추가 (시간 컬럼을 문자열이 아닌 DateTime으로 처리할 경우)
            dataGridView_TerminalMsg.Columns[0].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridView_TerminalMsg.RowHeadersVisible = false;
            dataGridView_TerminalMsg.AllowUserToResizeRows = false;

            // 각 컬럼에 대해 정렬 비활성화
            dataGridView_TerminalMsg.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_TerminalMsg.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            // 빈 행 추가 (예: 5개)
            for (int i = 0; i < TermianlGridRowViewCount; i++)
            {
                dataGridView_TerminalMsg.Rows.Add("", "");
            }

            dataGridView_TerminalMsg.ClearSelection();
            //dataGridView_TerminalMsg.EnableHeadersVisualStyles = false;
            //dataGridView_TerminalMsg.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            //dataGridView_TerminalMsg.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            //dataGridView_TerminalMsg.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Regular);
            //dataGridView_TerminalMsg.DefaultCellStyle.Font = new Font("나눔고딕", 9);
            //dataGridView_TerminalMsg.DefaultCellStyle.ForeColor = Color.Black;
            //dataGridView_TerminalMsg.DefaultCellStyle.BackColor = Color.LightGray;
            //dataGridView_TerminalMsg.GridColor = Color.Gray;
        }
        // 메시지 추가 (DataTable을 사용)
        public void AddMessage(string message)
        {

            


        }
        private void TerminalMsgForm_Load(object sender, EventArgs e)
        {
            
        }

        private void TerminalMsgForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                ShowTMsgGrid();
            }
        }
    }
}
