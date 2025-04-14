namespace ZenHandler.Dlg
{
    partial class AlarmControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.ManualTitleLabel = new System.Windows.Forms.Label();
            this.dataGridView_Alarm = new System.Windows.Forms.DataGridView();
            this.BTN_MANUAL_PCB = new System.Windows.Forms.Button();
            this.BTN_MANUAL_LENS = new System.Windows.Forms.Button();
            this.label_AlarmPage = new System.Windows.Forms.Label();
            this.BTN_ALARM_NEXT = new System.Windows.Forms.Button();
            this.BTN_ALARM_PREV = new System.Windows.Forms.Button();
            this.BTN_ALARM_CLEAR = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Alarm)).BeginInit();
            this.SuspendLayout();
            // 
            // ManualTitleLabel
            // 
            this.ManualTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ManualTitleLabel.Location = new System.Drawing.Point(3, 0);
            this.ManualTitleLabel.Name = "ManualTitleLabel";
            this.ManualTitleLabel.Size = new System.Drawing.Size(250, 50);
            this.ManualTitleLabel.TabIndex = 2;
            this.ManualTitleLabel.Text = "| ALARM";
            this.ManualTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridView_Alarm
            // 
            this.dataGridView_Alarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Alarm.Location = new System.Drawing.Point(17, 78);
            this.dataGridView_Alarm.Name = "dataGridView_Alarm";
            this.dataGridView_Alarm.RowTemplate.Height = 23;
            this.dataGridView_Alarm.Size = new System.Drawing.Size(740, 810);
            this.dataGridView_Alarm.TabIndex = 0;
            // 
            // BTN_MANUAL_PCB
            // 
            this.BTN_MANUAL_PCB.BackColor = System.Drawing.Color.Tan;
            this.BTN_MANUAL_PCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MANUAL_PCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MANUAL_PCB.ForeColor = System.Drawing.Color.White;
            this.BTN_MANUAL_PCB.Location = new System.Drawing.Point(579, 14);
            this.BTN_MANUAL_PCB.Name = "BTN_MANUAL_PCB";
            this.BTN_MANUAL_PCB.Size = new System.Drawing.Size(154, 44);
            this.BTN_MANUAL_PCB.TabIndex = 30;
            this.BTN_MANUAL_PCB.Text = "PCB";
            this.BTN_MANUAL_PCB.UseVisualStyleBackColor = false;
            this.BTN_MANUAL_PCB.Visible = false;
            this.BTN_MANUAL_PCB.Click += new System.EventHandler(this.BTN_MANUAL_PCB_Click);
            // 
            // BTN_MANUAL_LENS
            // 
            this.BTN_MANUAL_LENS.BackColor = System.Drawing.Color.Tan;
            this.BTN_MANUAL_LENS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MANUAL_LENS.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MANUAL_LENS.ForeColor = System.Drawing.Color.White;
            this.BTN_MANUAL_LENS.Location = new System.Drawing.Point(559, 19);
            this.BTN_MANUAL_LENS.Name = "BTN_MANUAL_LENS";
            this.BTN_MANUAL_LENS.Size = new System.Drawing.Size(154, 44);
            this.BTN_MANUAL_LENS.TabIndex = 31;
            this.BTN_MANUAL_LENS.Text = "LENS";
            this.BTN_MANUAL_LENS.UseVisualStyleBackColor = false;
            this.BTN_MANUAL_LENS.Visible = false;
            this.BTN_MANUAL_LENS.Click += new System.EventHandler(this.BTN_MANUAL_LENS_Click);
            // 
            // label_AlarmPage
            // 
            this.label_AlarmPage.BackColor = System.Drawing.Color.White;
            this.label_AlarmPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_AlarmPage.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_AlarmPage.Location = new System.Drawing.Point(298, 904);
            this.label_AlarmPage.Name = "label_AlarmPage";
            this.label_AlarmPage.Size = new System.Drawing.Size(100, 50);
            this.label_AlarmPage.TabIndex = 40;
            this.label_AlarmPage.Text = "1 / 1";
            this.label_AlarmPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_ALARM_NEXT
            // 
            this.BTN_ALARM_NEXT.BackColor = System.Drawing.Color.Tan;
            this.BTN_ALARM_NEXT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_ALARM_NEXT.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_ALARM_NEXT.ForeColor = System.Drawing.Color.White;
            this.BTN_ALARM_NEXT.Location = new System.Drawing.Point(404, 904);
            this.BTN_ALARM_NEXT.Name = "BTN_ALARM_NEXT";
            this.BTN_ALARM_NEXT.Size = new System.Drawing.Size(116, 50);
            this.BTN_ALARM_NEXT.TabIndex = 39;
            this.BTN_ALARM_NEXT.Text = "NEXT";
            this.BTN_ALARM_NEXT.UseVisualStyleBackColor = false;
            // 
            // BTN_ALARM_PREV
            // 
            this.BTN_ALARM_PREV.BackColor = System.Drawing.Color.Tan;
            this.BTN_ALARM_PREV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_ALARM_PREV.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_ALARM_PREV.ForeColor = System.Drawing.Color.White;
            this.BTN_ALARM_PREV.Location = new System.Drawing.Point(176, 904);
            this.BTN_ALARM_PREV.Name = "BTN_ALARM_PREV";
            this.BTN_ALARM_PREV.Size = new System.Drawing.Size(116, 50);
            this.BTN_ALARM_PREV.TabIndex = 38;
            this.BTN_ALARM_PREV.Text = "PREV";
            this.BTN_ALARM_PREV.UseVisualStyleBackColor = false;
            // 
            // BTN_ALARM_CLEAR
            // 
            this.BTN_ALARM_CLEAR.BackColor = System.Drawing.Color.Tan;
            this.BTN_ALARM_CLEAR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_ALARM_CLEAR.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.BTN_ALARM_CLEAR.ForeColor = System.Drawing.Color.White;
            this.BTN_ALARM_CLEAR.Location = new System.Drawing.Point(645, 904);
            this.BTN_ALARM_CLEAR.Name = "BTN_ALARM_CLEAR";
            this.BTN_ALARM_CLEAR.Size = new System.Drawing.Size(112, 50);
            this.BTN_ALARM_CLEAR.TabIndex = 37;
            this.BTN_ALARM_CLEAR.Text = "CLEAR";
            this.BTN_ALARM_CLEAR.UseVisualStyleBackColor = false;
            // 
            // AlarmControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.Controls.Add(this.dataGridView_Alarm);
            this.Controls.Add(this.label_AlarmPage);
            this.Controls.Add(this.BTN_ALARM_NEXT);
            this.Controls.Add(this.BTN_ALARM_PREV);
            this.Controls.Add(this.BTN_ALARM_CLEAR);
            this.Controls.Add(this.BTN_MANUAL_LENS);
            this.Controls.Add(this.BTN_MANUAL_PCB);
            this.Controls.Add(this.ManualTitleLabel);
            this.Name = "AlarmControl";
            this.Size = new System.Drawing.Size(770, 986);
            this.VisibleChanged += new System.EventHandler(this.AlarmControl_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Alarm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ManualTitleLabel;
        private System.Windows.Forms.Button BTN_MANUAL_PCB;
        private System.Windows.Forms.Button BTN_MANUAL_LENS;
        private System.Windows.Forms.DataGridView dataGridView_Alarm;
        private System.Windows.Forms.Label label_AlarmPage;
        private System.Windows.Forms.Button BTN_ALARM_NEXT;
        private System.Windows.Forms.Button BTN_ALARM_PREV;
        private System.Windows.Forms.Button BTN_ALARM_CLEAR;
    }
}
