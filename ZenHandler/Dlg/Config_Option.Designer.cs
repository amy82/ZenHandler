
namespace ZenHandler.Dlg
{
    partial class Config_Option
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
            this.button_Bcr_DisConnect = new System.Windows.Forms.Button();
            this.poisonComboBox_BcrPort = new System.Windows.Forms.ComboBox();
            this.button_Bcr_Connect = new System.Windows.Forms.Button();
            this.label_Config_Bcr = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ComboBox_Language = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label_PinCountMax = new System.Windows.Forms.Label();
            this.Btn_ConfigOption_Save = new System.Windows.Forms.Button();
            this.label_ConfigOption_Position_Gap = new System.Windows.Forms.Label();
            this.label_Config_Tray_GapY_Val = new System.Windows.Forms.Label();
            this.label_Config_Tray_GapX_Val = new System.Windows.Forms.Label();
            this.label_Config_Tray_Gap = new System.Windows.Forms.Label();
            this.label_Config_Socket_GapY_Val = new System.Windows.Forms.Label();
            this.label_Config_Socket_GapX_Val = new System.Windows.Forms.Label();
            this.label_Config_Socket_Gap = new System.Windows.Forms.Label();
            this.label_Config_Ng_GapY_Val = new System.Windows.Forms.Label();
            this.label_Config_Ng_GapX_Val = new System.Windows.Forms.Label();
            this.label_Config_Ng_Gap = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_Bcr_DisConnect
            // 
            this.button_Bcr_DisConnect.BackColor = System.Drawing.Color.Tan;
            this.button_Bcr_DisConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Bcr_DisConnect.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Bcr_DisConnect.ForeColor = System.Drawing.Color.White;
            this.button_Bcr_DisConnect.Location = new System.Drawing.Point(490, 78);
            this.button_Bcr_DisConnect.Name = "button_Bcr_DisConnect";
            this.button_Bcr_DisConnect.Size = new System.Drawing.Size(103, 44);
            this.button_Bcr_DisConnect.TabIndex = 77;
            this.button_Bcr_DisConnect.Text = "DISCONNECT";
            this.button_Bcr_DisConnect.UseVisualStyleBackColor = false;
            this.button_Bcr_DisConnect.Click += new System.EventHandler(this.button_Bcr_Connect_Click);
            // 
            // poisonComboBox_BcrPort
            // 
            this.poisonComboBox_BcrPort.Cursor = System.Windows.Forms.Cursors.Hand;
            this.poisonComboBox_BcrPort.DropDownHeight = 120;
            this.poisonComboBox_BcrPort.Font = new System.Drawing.Font("나눔고딕", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.poisonComboBox_BcrPort.FormattingEnabled = true;
            this.poisonComboBox_BcrPort.IntegralHeight = false;
            this.poisonComboBox_BcrPort.ItemHeight = 20;
            this.poisonComboBox_BcrPort.Location = new System.Drawing.Point(594, 44);
            this.poisonComboBox_BcrPort.Name = "poisonComboBox_BcrPort";
            this.poisonComboBox_BcrPort.Size = new System.Drawing.Size(105, 28);
            this.poisonComboBox_BcrPort.TabIndex = 78;
            // 
            // button_Bcr_Connect
            // 
            this.button_Bcr_Connect.BackColor = System.Drawing.Color.Tan;
            this.button_Bcr_Connect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Bcr_Connect.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Bcr_Connect.ForeColor = System.Drawing.Color.White;
            this.button_Bcr_Connect.Location = new System.Drawing.Point(593, 78);
            this.button_Bcr_Connect.Name = "button_Bcr_Connect";
            this.button_Bcr_Connect.Size = new System.Drawing.Size(103, 44);
            this.button_Bcr_Connect.TabIndex = 76;
            this.button_Bcr_Connect.Text = "CONNECT";
            this.button_Bcr_Connect.UseVisualStyleBackColor = false;
            this.button_Bcr_Connect.Click += new System.EventHandler(this.button_Bcr_DisConnect_Click);
            // 
            // label_Config_Bcr
            // 
            this.label_Config_Bcr.BackColor = System.Drawing.SystemColors.Window;
            this.label_Config_Bcr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Bcr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Config_Bcr.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Bcr.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Bcr.Location = new System.Drawing.Point(485, 44);
            this.label_Config_Bcr.Name = "label_Config_Bcr";
            this.label_Config_Bcr.Size = new System.Drawing.Size(107, 29);
            this.label_Config_Bcr.TabIndex = 75;
            this.label_Config_Bcr.Text = "BCR";
            this.label_Config_Bcr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Window;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(440, 434);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 29);
            this.label5.TabIndex = 79;
            this.label5.Text = "Language";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ComboBox_Language
            // 
            this.ComboBox_Language.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComboBox_Language.DropDownHeight = 120;
            this.ComboBox_Language.Font = new System.Drawing.Font("나눔고딕", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ComboBox_Language.FormattingEnabled = true;
            this.ComboBox_Language.IntegralHeight = false;
            this.ComboBox_Language.ItemHeight = 20;
            this.ComboBox_Language.Location = new System.Drawing.Point(554, 434);
            this.ComboBox_Language.Name = "ComboBox_Language";
            this.ComboBox_Language.Size = new System.Drawing.Size(142, 28);
            this.ComboBox_Language.TabIndex = 80;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(44, 330);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 29);
            this.label2.TabIndex = 81;
            this.label2.Text = "PIN COUNT MAX :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_PinCountMax
            // 
            this.label_PinCountMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_PinCountMax.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_PinCountMax.Location = new System.Drawing.Point(211, 330);
            this.label_PinCountMax.Name = "label_PinCountMax";
            this.label_PinCountMax.Size = new System.Drawing.Size(100, 29);
            this.label_PinCountMax.TabIndex = 82;
            this.label_PinCountMax.Text = "0";
            this.label_PinCountMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_PinCountMax.Click += new System.EventHandler(this.label_PinCountMax_Click);
            // 
            // Btn_ConfigOption_Save
            // 
            this.Btn_ConfigOption_Save.BackColor = System.Drawing.Color.Tan;
            this.Btn_ConfigOption_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_ConfigOption_Save.Font = new System.Drawing.Font("나눔고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_ConfigOption_Save.ForeColor = System.Drawing.Color.White;
            this.Btn_ConfigOption_Save.Location = new System.Drawing.Point(617, 816);
            this.Btn_ConfigOption_Save.Name = "Btn_ConfigOption_Save";
            this.Btn_ConfigOption_Save.Size = new System.Drawing.Size(122, 53);
            this.Btn_ConfigOption_Save.TabIndex = 89;
            this.Btn_ConfigOption_Save.Text = "SAVE";
            this.Btn_ConfigOption_Save.UseVisualStyleBackColor = false;
            this.Btn_ConfigOption_Save.Click += new System.EventHandler(this.Btn_ConfigOption_Save_Click);
            // 
            // label_ConfigOption_Position_Gap
            // 
            this.label_ConfigOption_Position_Gap.BackColor = System.Drawing.Color.SlateGray;
            this.label_ConfigOption_Position_Gap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ConfigOption_Position_Gap.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_ConfigOption_Position_Gap.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label_ConfigOption_Position_Gap.Location = new System.Drawing.Point(21, 29);
            this.label_ConfigOption_Position_Gap.Name = "label_ConfigOption_Position_Gap";
            this.label_ConfigOption_Position_Gap.Size = new System.Drawing.Size(297, 32);
            this.label_ConfigOption_Position_Gap.TabIndex = 90;
            this.label_ConfigOption_Position_Gap.Text = "Position Gap (mm)";
            this.label_ConfigOption_Position_Gap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Tray_GapY_Val
            // 
            this.label_Config_Tray_GapY_Val.BackColor = System.Drawing.Color.White;
            this.label_Config_Tray_GapY_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_GapY_Val.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_GapY_Val.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Tray_GapY_Val.Location = new System.Drawing.Point(232, 61);
            this.label_Config_Tray_GapY_Val.Name = "label_Config_Tray_GapY_Val";
            this.label_Config_Tray_GapY_Val.Size = new System.Drawing.Size(86, 40);
            this.label_Config_Tray_GapY_Val.TabIndex = 101;
            this.label_Config_Tray_GapY_Val.Text = "0";
            this.label_Config_Tray_GapY_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Tray_GapX_Val
            // 
            this.label_Config_Tray_GapX_Val.BackColor = System.Drawing.Color.White;
            this.label_Config_Tray_GapX_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_GapX_Val.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_GapX_Val.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Tray_GapX_Val.Location = new System.Drawing.Point(147, 61);
            this.label_Config_Tray_GapX_Val.Name = "label_Config_Tray_GapX_Val";
            this.label_Config_Tray_GapX_Val.Size = new System.Drawing.Size(86, 40);
            this.label_Config_Tray_GapX_Val.TabIndex = 100;
            this.label_Config_Tray_GapX_Val.Text = "0";
            this.label_Config_Tray_GapX_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Tray_Gap
            // 
            this.label_Config_Tray_Gap.BackColor = System.Drawing.Color.PaleTurquoise;
            this.label_Config_Tray_Gap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_Gap.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_Gap.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Tray_Gap.Location = new System.Drawing.Point(21, 61);
            this.label_Config_Tray_Gap.Name = "label_Config_Tray_Gap";
            this.label_Config_Tray_Gap.Size = new System.Drawing.Size(126, 40);
            this.label_Config_Tray_Gap.TabIndex = 99;
            this.label_Config_Tray_Gap.Text = "Tray Gap";
            this.label_Config_Tray_Gap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Socket_GapY_Val
            // 
            this.label_Config_Socket_GapY_Val.BackColor = System.Drawing.Color.White;
            this.label_Config_Socket_GapY_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Socket_GapY_Val.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Socket_GapY_Val.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Socket_GapY_Val.Location = new System.Drawing.Point(232, 101);
            this.label_Config_Socket_GapY_Val.Name = "label_Config_Socket_GapY_Val";
            this.label_Config_Socket_GapY_Val.Size = new System.Drawing.Size(86, 40);
            this.label_Config_Socket_GapY_Val.TabIndex = 104;
            this.label_Config_Socket_GapY_Val.Text = "0";
            this.label_Config_Socket_GapY_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Socket_GapX_Val
            // 
            this.label_Config_Socket_GapX_Val.BackColor = System.Drawing.Color.White;
            this.label_Config_Socket_GapX_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Socket_GapX_Val.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Socket_GapX_Val.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Socket_GapX_Val.Location = new System.Drawing.Point(147, 101);
            this.label_Config_Socket_GapX_Val.Name = "label_Config_Socket_GapX_Val";
            this.label_Config_Socket_GapX_Val.Size = new System.Drawing.Size(86, 40);
            this.label_Config_Socket_GapX_Val.TabIndex = 103;
            this.label_Config_Socket_GapX_Val.Text = "0";
            this.label_Config_Socket_GapX_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Socket_Gap
            // 
            this.label_Config_Socket_Gap.BackColor = System.Drawing.Color.PaleTurquoise;
            this.label_Config_Socket_Gap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Socket_Gap.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Socket_Gap.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Socket_Gap.Location = new System.Drawing.Point(21, 101);
            this.label_Config_Socket_Gap.Name = "label_Config_Socket_Gap";
            this.label_Config_Socket_Gap.Size = new System.Drawing.Size(126, 40);
            this.label_Config_Socket_Gap.TabIndex = 102;
            this.label_Config_Socket_Gap.Text = "Socket Gap";
            this.label_Config_Socket_Gap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Ng_GapY_Val
            // 
            this.label_Config_Ng_GapY_Val.BackColor = System.Drawing.Color.White;
            this.label_Config_Ng_GapY_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Ng_GapY_Val.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Ng_GapY_Val.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Ng_GapY_Val.Location = new System.Drawing.Point(232, 141);
            this.label_Config_Ng_GapY_Val.Name = "label_Config_Ng_GapY_Val";
            this.label_Config_Ng_GapY_Val.Size = new System.Drawing.Size(86, 40);
            this.label_Config_Ng_GapY_Val.TabIndex = 107;
            this.label_Config_Ng_GapY_Val.Text = "0";
            this.label_Config_Ng_GapY_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Ng_GapX_Val
            // 
            this.label_Config_Ng_GapX_Val.BackColor = System.Drawing.Color.White;
            this.label_Config_Ng_GapX_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Ng_GapX_Val.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Ng_GapX_Val.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Ng_GapX_Val.Location = new System.Drawing.Point(147, 141);
            this.label_Config_Ng_GapX_Val.Name = "label_Config_Ng_GapX_Val";
            this.label_Config_Ng_GapX_Val.Size = new System.Drawing.Size(86, 40);
            this.label_Config_Ng_GapX_Val.TabIndex = 106;
            this.label_Config_Ng_GapX_Val.Text = "0";
            this.label_Config_Ng_GapX_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Ng_Gap
            // 
            this.label_Config_Ng_Gap.BackColor = System.Drawing.Color.PaleTurquoise;
            this.label_Config_Ng_Gap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Ng_Gap.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Ng_Gap.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Ng_Gap.Location = new System.Drawing.Point(21, 141);
            this.label_Config_Ng_Gap.Name = "label_Config_Ng_Gap";
            this.label_Config_Ng_Gap.Size = new System.Drawing.Size(126, 40);
            this.label_Config_Ng_Gap.TabIndex = 105;
            this.label_Config_Ng_Gap.Text = "Ng Gap";
            this.label_Config_Ng_Gap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Config_Option
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_Config_Ng_GapY_Val);
            this.Controls.Add(this.label_Config_Ng_GapX_Val);
            this.Controls.Add(this.label_Config_Ng_Gap);
            this.Controls.Add(this.label_Config_Socket_GapY_Val);
            this.Controls.Add(this.label_Config_Socket_GapX_Val);
            this.Controls.Add(this.label_Config_Socket_Gap);
            this.Controls.Add(this.label_Config_Tray_GapY_Val);
            this.Controls.Add(this.label_Config_Tray_GapX_Val);
            this.Controls.Add(this.label_Config_Tray_Gap);
            this.Controls.Add(this.label_ConfigOption_Position_Gap);
            this.Controls.Add(this.Btn_ConfigOption_Save);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_PinCountMax);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ComboBox_Language);
            this.Controls.Add(this.button_Bcr_DisConnect);
            this.Controls.Add(this.poisonComboBox_BcrPort);
            this.Controls.Add(this.button_Bcr_Connect);
            this.Controls.Add(this.label_Config_Bcr);
            this.Name = "Config_Option";
            this.Size = new System.Drawing.Size(770, 900);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_Bcr_DisConnect;
        private System.Windows.Forms.ComboBox poisonComboBox_BcrPort;
        private System.Windows.Forms.Button button_Bcr_Connect;
        public System.Windows.Forms.Label label_Config_Bcr;
        public System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ComboBox_Language;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_PinCountMax;
        private System.Windows.Forms.Button Btn_ConfigOption_Save;
        private System.Windows.Forms.Label label_ConfigOption_Position_Gap;
        private System.Windows.Forms.Label label_Config_Tray_GapY_Val;
        private System.Windows.Forms.Label label_Config_Tray_GapX_Val;
        private System.Windows.Forms.Label label_Config_Tray_Gap;
        private System.Windows.Forms.Label label_Config_Socket_GapY_Val;
        private System.Windows.Forms.Label label_Config_Socket_GapX_Val;
        private System.Windows.Forms.Label label_Config_Socket_Gap;
        private System.Windows.Forms.Label label_Config_Ng_GapY_Val;
        private System.Windows.Forms.Label label_Config_Ng_GapX_Val;
        private System.Windows.Forms.Label label_Config_Ng_Gap;
    }
}
