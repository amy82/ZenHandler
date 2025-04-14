namespace ZenHandler.Dlg
{
    partial class LogControl
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_SensorIni = new System.Windows.Forms.TextBox();
            this.button_SensorIni = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_EquipLog = new System.Windows.Forms.TextBox();
            this.BTN_LOG_EQUIP_OPEN = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_DumpFile = new System.Windows.Forms.TextBox();
            this.BTN_LOG_DUMP_OPEN = new System.Windows.Forms.Button();
            this.label_LogTitle = new System.Windows.Forms.Label();
            this.listBox_Log = new System.Windows.Forms.ListBox();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ManualTitleLabel
            // 
            this.ManualTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ManualTitleLabel.Location = new System.Drawing.Point(3, 0);
            this.ManualTitleLabel.Name = "ManualTitleLabel";
            this.ManualTitleLabel.Size = new System.Drawing.Size(250, 50);
            this.ManualTitleLabel.TabIndex = 2;
            this.ManualTitleLabel.Text = "| LOG";
            this.ManualTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.White;
            this.groupBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox_SensorIni);
            this.groupBox3.Controls.Add(this.button_SensorIni);
            this.groupBox3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox3.Location = new System.Drawing.Point(40, 476);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(495, 96);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sensor ini Path";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(6, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(399, 24);
            this.label3.TabIndex = 38;
            this.label3.Text = "센서 INI 설정 파일 경로";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_SensorIni
            // 
            this.textBox_SensorIni.BackColor = System.Drawing.Color.White;
            this.textBox_SensorIni.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_SensorIni.Location = new System.Drawing.Point(8, 33);
            this.textBox_SensorIni.Multiline = true;
            this.textBox_SensorIni.Name = "textBox_SensorIni";
            this.textBox_SensorIni.ReadOnly = true;
            this.textBox_SensorIni.Size = new System.Drawing.Size(397, 21);
            this.textBox_SensorIni.TabIndex = 34;
            this.textBox_SensorIni.Text = "D:\\EVMS\\Log\\Step";
            // 
            // button_SensorIni
            // 
            this.button_SensorIni.BackColor = System.Drawing.Color.Tan;
            this.button_SensorIni.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_SensorIni.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_SensorIni.ForeColor = System.Drawing.Color.White;
            this.button_SensorIni.Location = new System.Drawing.Point(411, 29);
            this.button_SensorIni.Name = "button_SensorIni";
            this.button_SensorIni.Size = new System.Drawing.Size(78, 30);
            this.button_SensorIni.TabIndex = 33;
            this.button_SensorIni.Text = "OPEN";
            this.button_SensorIni.UseVisualStyleBackColor = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_EquipLog);
            this.groupBox2.Controls.Add(this.BTN_LOG_EQUIP_OPEN);
            this.groupBox2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(40, 365);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(495, 96);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Equip Log Path";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(399, 24);
            this.label2.TabIndex = 37;
            this.label2.Text = "설비 프로그램 로그 경로";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_EquipLog
            // 
            this.textBox_EquipLog.BackColor = System.Drawing.Color.White;
            this.textBox_EquipLog.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_EquipLog.Location = new System.Drawing.Point(8, 33);
            this.textBox_EquipLog.Multiline = true;
            this.textBox_EquipLog.Name = "textBox_EquipLog";
            this.textBox_EquipLog.ReadOnly = true;
            this.textBox_EquipLog.Size = new System.Drawing.Size(397, 21);
            this.textBox_EquipLog.TabIndex = 34;
            this.textBox_EquipLog.Text = "D:\\EVMS\\Log\\Step";
            // 
            // BTN_LOG_EQUIP_OPEN
            // 
            this.BTN_LOG_EQUIP_OPEN.BackColor = System.Drawing.Color.Tan;
            this.BTN_LOG_EQUIP_OPEN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_LOG_EQUIP_OPEN.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_LOG_EQUIP_OPEN.ForeColor = System.Drawing.Color.White;
            this.BTN_LOG_EQUIP_OPEN.Location = new System.Drawing.Point(411, 29);
            this.BTN_LOG_EQUIP_OPEN.Name = "BTN_LOG_EQUIP_OPEN";
            this.BTN_LOG_EQUIP_OPEN.Size = new System.Drawing.Size(78, 30);
            this.BTN_LOG_EQUIP_OPEN.TabIndex = 33;
            this.BTN_LOG_EQUIP_OPEN.Text = "OPEN";
            this.BTN_LOG_EQUIP_OPEN.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_DumpFile);
            this.groupBox1.Controls.Add(this.BTN_LOG_DUMP_OPEN);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(40, 251);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(495, 93);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EEprom / Dump File Path";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(6, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(399, 24);
            this.label1.TabIndex = 36;
            this.label1.Text = "EEPROM 에서 읽은 BINARY 파일 저장 경로";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_DumpFile
            // 
            this.textBox_DumpFile.BackColor = System.Drawing.Color.White;
            this.textBox_DumpFile.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_DumpFile.Location = new System.Drawing.Point(8, 30);
            this.textBox_DumpFile.Multiline = true;
            this.textBox_DumpFile.Name = "textBox_DumpFile";
            this.textBox_DumpFile.ReadOnly = true;
            this.textBox_DumpFile.Size = new System.Drawing.Size(397, 22);
            this.textBox_DumpFile.TabIndex = 35;
            this.textBox_DumpFile.Text = "D:\\EVMS\\Log\\Dump";
            // 
            // BTN_LOG_DUMP_OPEN
            // 
            this.BTN_LOG_DUMP_OPEN.BackColor = System.Drawing.Color.Tan;
            this.BTN_LOG_DUMP_OPEN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_LOG_DUMP_OPEN.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_LOG_DUMP_OPEN.ForeColor = System.Drawing.Color.White;
            this.BTN_LOG_DUMP_OPEN.Location = new System.Drawing.Point(411, 28);
            this.BTN_LOG_DUMP_OPEN.Name = "BTN_LOG_DUMP_OPEN";
            this.BTN_LOG_DUMP_OPEN.Size = new System.Drawing.Size(78, 30);
            this.BTN_LOG_DUMP_OPEN.TabIndex = 32;
            this.BTN_LOG_DUMP_OPEN.Text = "OPEN";
            this.BTN_LOG_DUMP_OPEN.UseVisualStyleBackColor = false;
            // 
            // label_LogTitle
            // 
            this.label_LogTitle.AutoSize = true;
            this.label_LogTitle.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_LogTitle.Location = new System.Drawing.Point(37, 636);
            this.label_LogTitle.Name = "label_LogTitle";
            this.label_LogTitle.Size = new System.Drawing.Size(68, 14);
            this.label_LogTitle.TabIndex = 39;
            this.label_LogTitle.Text = " LOG VIEW";
            // 
            // listBox_Log
            // 
            this.listBox_Log.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listBox_Log.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.listBox_Log.FormattingEnabled = true;
            this.listBox_Log.HorizontalScrollbar = true;
            this.listBox_Log.ItemHeight = 15;
            this.listBox_Log.Location = new System.Drawing.Point(40, 659);
            this.listBox_Log.Margin = new System.Windows.Forms.Padding(0);
            this.listBox_Log.Name = "listBox_Log";
            this.listBox_Log.Size = new System.Drawing.Size(692, 289);
            this.listBox_Log.TabIndex = 40;
            // 
            // LogControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.Controls.Add(this.label_LogTitle);
            this.Controls.Add(this.listBox_Log);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ManualTitleLabel);
            this.Name = "LogControl";
            this.Size = new System.Drawing.Size(770, 1000);
            this.VisibleChanged += new System.EventHandler(this.AlarmControl_VisibleChanged);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ManualTitleLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_SensorIni;
        private System.Windows.Forms.Button button_SensorIni;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_EquipLog;
        private System.Windows.Forms.Button BTN_LOG_EQUIP_OPEN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_DumpFile;
        private System.Windows.Forms.Button BTN_LOG_DUMP_OPEN;
        private System.Windows.Forms.Label label_LogTitle;
        public System.Windows.Forms.ListBox listBox_Log;
    }
}
