﻿namespace ZenHandler.Dlg
{
    partial class ConfigControl
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
            this.ManualPanel = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label_CsvScanMax = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_PinCountMax = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BTN_CONFIG_SAVE = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Bcr_DisConnect = new System.Windows.Forms.Button();
            this.button_Bcr_Connect = new System.Windows.Forms.Button();
            this.poisonComboBox_BcrPort = new ReaLTaiizor.Controls.PoisonComboBox();
            this.label_Config_Bcr = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.hopeCheckBox_ImageGrabUse = new ReaLTaiizor.Controls.HopeCheckBox();
            this.hopeCheckBox_PinCountUse = new ReaLTaiizor.Controls.HopeCheckBox();
            this.checkBox_BcrGo = new System.Windows.Forms.CheckBox();
            this.checkBox_IdleReportPass = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BTN_MANUAL_PCB = new System.Windows.Forms.Button();
            this.BTN_MANUAL_LENS = new System.Windows.Forms.Button();
            this.ManualPanel.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ManualTitleLabel
            // 
            this.ManualTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ManualTitleLabel.Location = new System.Drawing.Point(3, 0);
            this.ManualTitleLabel.Name = "ManualTitleLabel";
            this.ManualTitleLabel.Size = new System.Drawing.Size(250, 50);
            this.ManualTitleLabel.TabIndex = 2;
            this.ManualTitleLabel.Text = "| CONFIG";
            this.ManualTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ManualPanel
            // 
            this.ManualPanel.Controls.Add(this.groupBox3);
            this.ManualPanel.Controls.Add(this.BTN_CONFIG_SAVE);
            this.ManualPanel.Controls.Add(this.groupBox1);
            this.ManualPanel.Controls.Add(this.groupBox2);
            this.ManualPanel.Location = new System.Drawing.Point(21, 97);
            this.ManualPanel.Name = "ManualPanel";
            this.ManualPanel.Size = new System.Drawing.Size(909, 737);
            this.ManualPanel.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.White;
            this.groupBox3.Controls.Add(this.label_CsvScanMax);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label_PinCountMax);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(22, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(298, 297);
            this.groupBox3.TabIndex = 46;
            this.groupBox3.TabStop = false;
            // 
            // label_CsvScanMax
            // 
            this.label_CsvScanMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CsvScanMax.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_CsvScanMax.Location = new System.Drawing.Point(184, 67);
            this.label_CsvScanMax.Name = "label_CsvScanMax";
            this.label_CsvScanMax.Size = new System.Drawing.Size(100, 29);
            this.label_CsvScanMax.TabIndex = 34;
            this.label_CsvScanMax.Text = "0";
            this.label_CsvScanMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_CsvScanMax.Click += new System.EventHandler(this.label_CsvScanMax_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.Window;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(17, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 29);
            this.label4.TabIndex = 33;
            this.label4.Text = "CSV 검색 기간 (month)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_PinCountMax
            // 
            this.label_PinCountMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_PinCountMax.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_PinCountMax.Location = new System.Drawing.Point(184, 28);
            this.label_PinCountMax.Name = "label_PinCountMax";
            this.label_PinCountMax.Size = new System.Drawing.Size(100, 29);
            this.label_PinCountMax.TabIndex = 32;
            this.label_PinCountMax.Text = "0";
            this.label_PinCountMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_PinCountMax.Click += new System.EventHandler(this.label_PinCountMax_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(17, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 29);
            this.label2.TabIndex = 31;
            this.label2.Text = "PIN COUNT MAX :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_CONFIG_SAVE
            // 
            this.BTN_CONFIG_SAVE.BackColor = System.Drawing.Color.Tan;
            this.BTN_CONFIG_SAVE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CONFIG_SAVE.Font = new System.Drawing.Font("나눔고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CONFIG_SAVE.ForeColor = System.Drawing.Color.White;
            this.BTN_CONFIG_SAVE.Location = new System.Drawing.Point(755, 656);
            this.BTN_CONFIG_SAVE.Name = "BTN_CONFIG_SAVE";
            this.BTN_CONFIG_SAVE.Size = new System.Drawing.Size(122, 53);
            this.BTN_CONFIG_SAVE.TabIndex = 28;
            this.BTN_CONFIG_SAVE.Text = "SAVE";
            this.BTN_CONFIG_SAVE.UseVisualStyleBackColor = false;
            this.BTN_CONFIG_SAVE.Click += new System.EventHandler(this.BTN_CONFIG_SAVE_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.button_Bcr_DisConnect);
            this.groupBox1.Controls.Add(this.button_Bcr_Connect);
            this.groupBox1.Controls.Add(this.poisonComboBox_BcrPort);
            this.groupBox1.Controls.Add(this.label_Config_Bcr);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(363, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 297);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            // 
            // button_Bcr_DisConnect
            // 
            this.button_Bcr_DisConnect.BackColor = System.Drawing.Color.Tan;
            this.button_Bcr_DisConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Bcr_DisConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Bcr_DisConnect.ForeColor = System.Drawing.Color.White;
            this.button_Bcr_DisConnect.Location = new System.Drawing.Point(188, 148);
            this.button_Bcr_DisConnect.Name = "button_Bcr_DisConnect";
            this.button_Bcr_DisConnect.Size = new System.Drawing.Size(95, 40);
            this.button_Bcr_DisConnect.TabIndex = 48;
            this.button_Bcr_DisConnect.Text = "DISCONNECT";
            this.button_Bcr_DisConnect.UseVisualStyleBackColor = false;
            this.button_Bcr_DisConnect.Click += new System.EventHandler(this.button_Bcr_DisConnect_Click);
            // 
            // button_Bcr_Connect
            // 
            this.button_Bcr_Connect.BackColor = System.Drawing.Color.Tan;
            this.button_Bcr_Connect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Bcr_Connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Bcr_Connect.ForeColor = System.Drawing.Color.White;
            this.button_Bcr_Connect.Location = new System.Drawing.Point(188, 102);
            this.button_Bcr_Connect.Name = "button_Bcr_Connect";
            this.button_Bcr_Connect.Size = new System.Drawing.Size(95, 44);
            this.button_Bcr_Connect.TabIndex = 47;
            this.button_Bcr_Connect.Text = "CONNECT";
            this.button_Bcr_Connect.UseVisualStyleBackColor = false;
            this.button_Bcr_Connect.Click += new System.EventHandler(this.button_Bcr_Connect_Click);
            // 
            // poisonComboBox_BcrPort
            // 
            this.poisonComboBox_BcrPort.Cursor = System.Windows.Forms.Cursors.Hand;
            this.poisonComboBox_BcrPort.ForeColor = System.Drawing.Color.Black;
            this.poisonComboBox_BcrPort.FormattingEnabled = true;
            this.poisonComboBox_BcrPort.ItemHeight = 23;
            this.poisonComboBox_BcrPort.Location = new System.Drawing.Point(94, 53);
            this.poisonComboBox_BcrPort.Name = "poisonComboBox_BcrPort";
            this.poisonComboBox_BcrPort.Size = new System.Drawing.Size(189, 29);
            this.poisonComboBox_BcrPort.TabIndex = 32;
            this.poisonComboBox_BcrPort.UseSelectable = true;
            // 
            // label_Config_Bcr
            // 
            this.label_Config_Bcr.BackColor = System.Drawing.SystemColors.Window;
            this.label_Config_Bcr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Bcr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Config_Bcr.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Bcr.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Bcr.Location = new System.Drawing.Point(12, 53);
            this.label_Config_Bcr.Name = "label_Config_Bcr";
            this.label_Config_Bcr.Size = new System.Drawing.Size(76, 29);
            this.label_Config_Bcr.TabIndex = 30;
            this.label_Config_Bcr.Text = "BCR";
            this.label_Config_Bcr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(42, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 23);
            this.label1.TabIndex = 26;
            this.label1.Text = "COM PORT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.hopeCheckBox_ImageGrabUse);
            this.groupBox2.Controls.Add(this.hopeCheckBox_PinCountUse);
            this.groupBox2.Controls.Add(this.checkBox_BcrGo);
            this.groupBox2.Controls.Add(this.checkBox_IdleReportPass);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(363, 341);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 297);
            this.groupBox2.TabIndex = 45;
            this.groupBox2.TabStop = false;
            // 
            // hopeCheckBox_ImageGrabUse
            // 
            this.hopeCheckBox_ImageGrabUse.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_ImageGrabUse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.hopeCheckBox_ImageGrabUse.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.hopeCheckBox_ImageGrabUse.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.hopeCheckBox_ImageGrabUse.Enable = true;
            this.hopeCheckBox_ImageGrabUse.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_ImageGrabUse.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.hopeCheckBox_ImageGrabUse.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.hopeCheckBox_ImageGrabUse.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hopeCheckBox_ImageGrabUse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.hopeCheckBox_ImageGrabUse.Location = new System.Drawing.Point(11, 90);
            this.hopeCheckBox_ImageGrabUse.Name = "hopeCheckBox_ImageGrabUse";
            this.hopeCheckBox_ImageGrabUse.Size = new System.Drawing.Size(159, 20);
            this.hopeCheckBox_ImageGrabUse.TabIndex = 30;
            this.hopeCheckBox_ImageGrabUse.Text = "CCD GRAB USE";
            this.hopeCheckBox_ImageGrabUse.UseVisualStyleBackColor = true;
            // 
            // hopeCheckBox_PinCountUse
            // 
            this.hopeCheckBox_PinCountUse.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_PinCountUse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.hopeCheckBox_PinCountUse.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.hopeCheckBox_PinCountUse.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.hopeCheckBox_PinCountUse.Enable = true;
            this.hopeCheckBox_PinCountUse.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_PinCountUse.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.hopeCheckBox_PinCountUse.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.hopeCheckBox_PinCountUse.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hopeCheckBox_PinCountUse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.hopeCheckBox_PinCountUse.Location = new System.Drawing.Point(11, 54);
            this.hopeCheckBox_PinCountUse.Name = "hopeCheckBox_PinCountUse";
            this.hopeCheckBox_PinCountUse.Size = new System.Drawing.Size(163, 20);
            this.hopeCheckBox_PinCountUse.TabIndex = 29;
            this.hopeCheckBox_PinCountUse.Text = "PIN COUNT USE";
            this.hopeCheckBox_PinCountUse.UseVisualStyleBackColor = true;
            // 
            // checkBox_BcrGo
            // 
            this.checkBox_BcrGo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.checkBox_BcrGo.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.checkBox_BcrGo.FlatAppearance.BorderSize = 2;
            this.checkBox_BcrGo.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray;
            this.checkBox_BcrGo.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_BcrGo.Location = new System.Drawing.Point(11, 250);
            this.checkBox_BcrGo.Name = "checkBox_BcrGo";
            this.checkBox_BcrGo.Size = new System.Drawing.Size(194, 21);
            this.checkBox_BcrGo.TabIndex = 28;
            this.checkBox_BcrGo.Text = "Start Automation on Barcode";
            this.checkBox_BcrGo.UseVisualStyleBackColor = false;
            this.checkBox_BcrGo.Visible = false;
            // 
            // checkBox_IdleReportPass
            // 
            this.checkBox_IdleReportPass.BackColor = System.Drawing.Color.WhiteSmoke;
            this.checkBox_IdleReportPass.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.checkBox_IdleReportPass.FlatAppearance.BorderSize = 2;
            this.checkBox_IdleReportPass.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray;
            this.checkBox_IdleReportPass.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_IdleReportPass.Location = new System.Drawing.Point(11, 223);
            this.checkBox_IdleReportPass.Name = "checkBox_IdleReportPass";
            this.checkBox_IdleReportPass.Size = new System.Drawing.Size(194, 21);
            this.checkBox_IdleReportPass.TabIndex = 27;
            this.checkBox_IdleReportPass.Text = "IDLE REASON REPORT PASS";
            this.checkBox_IdleReportPass.UseVisualStyleBackColor = false;
            this.checkBox_IdleReportPass.Visible = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(42, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(204, 23);
            this.label3.TabIndex = 26;
            this.label3.Text = "운전 설정";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_MANUAL_PCB
            // 
            this.BTN_MANUAL_PCB.BackColor = System.Drawing.Color.Tan;
            this.BTN_MANUAL_PCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MANUAL_PCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MANUAL_PCB.ForeColor = System.Drawing.Color.White;
            this.BTN_MANUAL_PCB.Location = new System.Drawing.Point(776, 16);
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
            this.BTN_MANUAL_LENS.Location = new System.Drawing.Point(744, 16);
            this.BTN_MANUAL_LENS.Name = "BTN_MANUAL_LENS";
            this.BTN_MANUAL_LENS.Size = new System.Drawing.Size(154, 44);
            this.BTN_MANUAL_LENS.TabIndex = 31;
            this.BTN_MANUAL_LENS.Text = "LENS";
            this.BTN_MANUAL_LENS.UseVisualStyleBackColor = false;
            this.BTN_MANUAL_LENS.Visible = false;
            this.BTN_MANUAL_LENS.Click += new System.EventHandler(this.BTN_MANUAL_LENS_Click);
            // 
            // ConfigControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.Controls.Add(this.BTN_MANUAL_LENS);
            this.Controls.Add(this.BTN_MANUAL_PCB);
            this.Controls.Add(this.ManualPanel);
            this.Controls.Add(this.ManualTitleLabel);
            this.Name = "ConfigControl";
            this.Size = new System.Drawing.Size(955, 938);
            this.VisibleChanged += new System.EventHandler(this.ConfigControl_VisibleChanged);
            this.ManualPanel.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ManualTitleLabel;
        private System.Windows.Forms.Panel ManualPanel;
        private System.Windows.Forms.Button BTN_MANUAL_PCB;
        private System.Windows.Forms.Button BTN_MANUAL_LENS;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BTN_CONFIG_SAVE;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label_Config_Bcr;
        private System.Windows.Forms.CheckBox checkBox_IdleReportPass;
        private System.Windows.Forms.CheckBox checkBox_BcrGo;
        private ReaLTaiizor.Controls.HopeCheckBox hopeCheckBox_PinCountUse;
        private ReaLTaiizor.Controls.PoisonComboBox poisonComboBox_BcrPort;
        private System.Windows.Forms.Button button_Bcr_Connect;
        private System.Windows.Forms.Button button_Bcr_DisConnect;
        private ReaLTaiizor.Controls.HopeCheckBox hopeCheckBox_ImageGrabUse;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label_PinCountMax;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_CsvScanMax;
        public System.Windows.Forms.Label label4;
    }
}
