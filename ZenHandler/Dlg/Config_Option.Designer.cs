
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
            this.comboBox_BcrPort = new System.Windows.Forms.ComboBox();
            this.label_ConfigOption_Tray_Max_Count = new System.Windows.Forms.Label();
            this.label_Config_Tray_Max_Count_Y = new System.Windows.Forms.Label();
            this.label_Config_Tray_Max_Count_X = new System.Windows.Forms.Label();
            this.label_Config_Tray_Max_Count = new System.Windows.Forms.Label();
            this.label_Config_Ngtray_Max_Count_Y = new System.Windows.Forms.Label();
            this.label_Config_Ngtray_Max_Count_X = new System.Windows.Forms.Label();
            this.label_Config_Ngtray_Max_Count = new System.Windows.Forms.Label();
            this.label_Config_Tray_Max_Layer = new System.Windows.Forms.Label();
            this.label_Config_Tray_Max_Layer_Val = new System.Windows.Forms.Label();
            this.label_Bcr_Ip1 = new System.Windows.Forms.Label();
            this.label_Bcr_Ip3 = new System.Windows.Forms.Label();
            this.label_Bcr_Ip2 = new System.Windows.Forms.Label();
            this.label_Config_Bcr_Port = new System.Windows.Forms.Label();
            this.label_Bcr_Port = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_Bcr_DisConnect
            // 
            this.button_Bcr_DisConnect.BackColor = System.Drawing.Color.Tan;
            this.button_Bcr_DisConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Bcr_DisConnect.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Bcr_DisConnect.ForeColor = System.Drawing.Color.White;
            this.button_Bcr_DisConnect.Location = new System.Drawing.Point(593, 174);
            this.button_Bcr_DisConnect.Name = "button_Bcr_DisConnect";
            this.button_Bcr_DisConnect.Size = new System.Drawing.Size(103, 44);
            this.button_Bcr_DisConnect.TabIndex = 77;
            this.button_Bcr_DisConnect.Text = "DISCONNECT";
            this.button_Bcr_DisConnect.UseVisualStyleBackColor = false;
            this.button_Bcr_DisConnect.Click += new System.EventHandler(this.button_Bcr_DisConnect_Click);
            // 
            // button_Bcr_Connect
            // 
            this.button_Bcr_Connect.BackColor = System.Drawing.Color.Tan;
            this.button_Bcr_Connect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Bcr_Connect.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Bcr_Connect.ForeColor = System.Drawing.Color.White;
            this.button_Bcr_Connect.Location = new System.Drawing.Point(490, 174);
            this.button_Bcr_Connect.Name = "button_Bcr_Connect";
            this.button_Bcr_Connect.Size = new System.Drawing.Size(103, 44);
            this.button_Bcr_Connect.TabIndex = 76;
            this.button_Bcr_Connect.Text = "CONNECT";
            this.button_Bcr_Connect.UseVisualStyleBackColor = false;
            this.button_Bcr_Connect.Click += new System.EventHandler(this.button_Bcr_Connect_Click);
            // 
            // label_Config_Bcr
            // 
            this.label_Config_Bcr.BackColor = System.Drawing.Color.LightGray;
            this.label_Config_Bcr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Bcr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Config_Bcr.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Bcr.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Bcr.Location = new System.Drawing.Point(490, 50);
            this.label_Config_Bcr.Name = "label_Config_Bcr";
            this.label_Config_Bcr.Size = new System.Drawing.Size(203, 28);
            this.label_Config_Bcr.TabIndex = 75;
            this.label_Config_Bcr.Text = "BCR IP ADDRESS";
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
            this.ComboBox_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.label2.Location = new System.Drawing.Point(426, 373);
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
            this.label_PinCountMax.Location = new System.Drawing.Point(593, 373);
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
            this.label_ConfigOption_Position_Gap.BackColor = System.Drawing.Color.Plum;
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
            this.label_Config_Tray_GapY_Val.Click += new System.EventHandler(this.label_Config_Tray_GapY_Val_Click);
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
            this.label_Config_Tray_GapX_Val.Click += new System.EventHandler(this.label_Config_Tray_GapX_Val_Click);
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
            this.label_Config_Socket_GapY_Val.Click += new System.EventHandler(this.label_Config_Socket_GapY_Val_Click);
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
            this.label_Config_Socket_GapX_Val.Click += new System.EventHandler(this.label_Config_Socket_GapX_Val_Click);
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
            this.label_Config_Ng_GapY_Val.Click += new System.EventHandler(this.label_Config_Ng_GapY_Val_Click);
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
            this.label_Config_Ng_GapX_Val.Click += new System.EventHandler(this.label_Config_Ng_GapX_Val_Click);
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
            // comboBox_BcrPort
            // 
            this.comboBox_BcrPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_BcrPort.Font = new System.Drawing.Font("나눔고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.comboBox_BcrPort.FormattingEnabled = true;
            this.comboBox_BcrPort.ItemHeight = 19;
            this.comboBox_BcrPort.Location = new System.Drawing.Point(617, 580);
            this.comboBox_BcrPort.Name = "comboBox_BcrPort";
            this.comboBox_BcrPort.Size = new System.Drawing.Size(114, 27);
            this.comboBox_BcrPort.TabIndex = 108;
            this.comboBox_BcrPort.Visible = false;
            // 
            // label_ConfigOption_Tray_Max_Count
            // 
            this.label_ConfigOption_Tray_Max_Count.BackColor = System.Drawing.Color.Plum;
            this.label_ConfigOption_Tray_Max_Count.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ConfigOption_Tray_Max_Count.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_ConfigOption_Tray_Max_Count.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label_ConfigOption_Tray_Max_Count.Location = new System.Drawing.Point(21, 220);
            this.label_ConfigOption_Tray_Max_Count.Name = "label_ConfigOption_Tray_Max_Count";
            this.label_ConfigOption_Tray_Max_Count.Size = new System.Drawing.Size(297, 32);
            this.label_ConfigOption_Tray_Max_Count.TabIndex = 109;
            this.label_ConfigOption_Tray_Max_Count.Text = "Tray Max Count";
            this.label_ConfigOption_Tray_Max_Count.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Tray_Max_Count_Y
            // 
            this.label_Config_Tray_Max_Count_Y.BackColor = System.Drawing.Color.White;
            this.label_Config_Tray_Max_Count_Y.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_Max_Count_Y.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_Max_Count_Y.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Tray_Max_Count_Y.Location = new System.Drawing.Point(232, 252);
            this.label_Config_Tray_Max_Count_Y.Name = "label_Config_Tray_Max_Count_Y";
            this.label_Config_Tray_Max_Count_Y.Size = new System.Drawing.Size(86, 40);
            this.label_Config_Tray_Max_Count_Y.TabIndex = 112;
            this.label_Config_Tray_Max_Count_Y.Text = "0";
            this.label_Config_Tray_Max_Count_Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Config_Tray_Max_Count_Y.Click += new System.EventHandler(this.label_Config_Tray_Max_Count_Y_Click);
            // 
            // label_Config_Tray_Max_Count_X
            // 
            this.label_Config_Tray_Max_Count_X.BackColor = System.Drawing.Color.White;
            this.label_Config_Tray_Max_Count_X.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_Max_Count_X.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_Max_Count_X.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Tray_Max_Count_X.Location = new System.Drawing.Point(147, 252);
            this.label_Config_Tray_Max_Count_X.Name = "label_Config_Tray_Max_Count_X";
            this.label_Config_Tray_Max_Count_X.Size = new System.Drawing.Size(86, 40);
            this.label_Config_Tray_Max_Count_X.TabIndex = 111;
            this.label_Config_Tray_Max_Count_X.Text = "0";
            this.label_Config_Tray_Max_Count_X.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Config_Tray_Max_Count_X.Click += new System.EventHandler(this.label_Config_Tray_Max_Count_X_Click);
            // 
            // label_Config_Tray_Max_Count
            // 
            this.label_Config_Tray_Max_Count.BackColor = System.Drawing.Color.PaleTurquoise;
            this.label_Config_Tray_Max_Count.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_Max_Count.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_Max_Count.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Tray_Max_Count.Location = new System.Drawing.Point(21, 252);
            this.label_Config_Tray_Max_Count.Name = "label_Config_Tray_Max_Count";
            this.label_Config_Tray_Max_Count.Size = new System.Drawing.Size(126, 40);
            this.label_Config_Tray_Max_Count.TabIndex = 110;
            this.label_Config_Tray_Max_Count.Text = "Tray Count";
            this.label_Config_Tray_Max_Count.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Ngtray_Max_Count_Y
            // 
            this.label_Config_Ngtray_Max_Count_Y.BackColor = System.Drawing.Color.White;
            this.label_Config_Ngtray_Max_Count_Y.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Ngtray_Max_Count_Y.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Ngtray_Max_Count_Y.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Ngtray_Max_Count_Y.Location = new System.Drawing.Point(232, 292);
            this.label_Config_Ngtray_Max_Count_Y.Name = "label_Config_Ngtray_Max_Count_Y";
            this.label_Config_Ngtray_Max_Count_Y.Size = new System.Drawing.Size(86, 40);
            this.label_Config_Ngtray_Max_Count_Y.TabIndex = 115;
            this.label_Config_Ngtray_Max_Count_Y.Text = "0";
            this.label_Config_Ngtray_Max_Count_Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Config_Ngtray_Max_Count_Y.Click += new System.EventHandler(this.label_Config_Ngtray_Max_Count_Y_Click);
            // 
            // label_Config_Ngtray_Max_Count_X
            // 
            this.label_Config_Ngtray_Max_Count_X.BackColor = System.Drawing.Color.White;
            this.label_Config_Ngtray_Max_Count_X.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Ngtray_Max_Count_X.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Ngtray_Max_Count_X.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Ngtray_Max_Count_X.Location = new System.Drawing.Point(147, 292);
            this.label_Config_Ngtray_Max_Count_X.Name = "label_Config_Ngtray_Max_Count_X";
            this.label_Config_Ngtray_Max_Count_X.Size = new System.Drawing.Size(86, 40);
            this.label_Config_Ngtray_Max_Count_X.TabIndex = 114;
            this.label_Config_Ngtray_Max_Count_X.Text = "0";
            this.label_Config_Ngtray_Max_Count_X.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Config_Ngtray_Max_Count_X.Click += new System.EventHandler(this.label_Config_Ngtray_Max_Count_X_Click);
            // 
            // label_Config_Ngtray_Max_Count
            // 
            this.label_Config_Ngtray_Max_Count.BackColor = System.Drawing.Color.PaleTurquoise;
            this.label_Config_Ngtray_Max_Count.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Ngtray_Max_Count.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Ngtray_Max_Count.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Ngtray_Max_Count.Location = new System.Drawing.Point(21, 292);
            this.label_Config_Ngtray_Max_Count.Name = "label_Config_Ngtray_Max_Count";
            this.label_Config_Ngtray_Max_Count.Size = new System.Drawing.Size(126, 40);
            this.label_Config_Ngtray_Max_Count.TabIndex = 113;
            this.label_Config_Ngtray_Max_Count.Text = "Ngtray Count";
            this.label_Config_Ngtray_Max_Count.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Tray_Max_Layer
            // 
            this.label_Config_Tray_Max_Layer.BackColor = System.Drawing.Color.PaleTurquoise;
            this.label_Config_Tray_Max_Layer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_Max_Layer.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_Max_Layer.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Tray_Max_Layer.Location = new System.Drawing.Point(21, 332);
            this.label_Config_Tray_Max_Layer.Name = "label_Config_Tray_Max_Layer";
            this.label_Config_Tray_Max_Layer.Size = new System.Drawing.Size(126, 40);
            this.label_Config_Tray_Max_Layer.TabIndex = 116;
            this.label_Config_Tray_Max_Layer.Text = "Tray Layer";
            this.label_Config_Tray_Max_Layer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Tray_Max_Layer_Val
            // 
            this.label_Config_Tray_Max_Layer_Val.BackColor = System.Drawing.Color.White;
            this.label_Config_Tray_Max_Layer_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_Max_Layer_Val.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_Max_Layer_Val.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Tray_Max_Layer_Val.Location = new System.Drawing.Point(147, 332);
            this.label_Config_Tray_Max_Layer_Val.Name = "label_Config_Tray_Max_Layer_Val";
            this.label_Config_Tray_Max_Layer_Val.Size = new System.Drawing.Size(86, 40);
            this.label_Config_Tray_Max_Layer_Val.TabIndex = 118;
            this.label_Config_Tray_Max_Layer_Val.Text = "0";
            this.label_Config_Tray_Max_Layer_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Config_Tray_Max_Layer_Val.Click += new System.EventHandler(this.label_Config_Left_Tray_Max_Layer_Val_Click);
            // 
            // label_Bcr_Ip1
            // 
            this.label_Bcr_Ip1.BackColor = System.Drawing.Color.LightGray;
            this.label_Bcr_Ip1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Bcr_Ip1.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Bcr_Ip1.ForeColor = System.Drawing.Color.Black;
            this.label_Bcr_Ip1.Location = new System.Drawing.Point(491, 78);
            this.label_Bcr_Ip1.Name = "label_Bcr_Ip1";
            this.label_Bcr_Ip1.Size = new System.Drawing.Size(72, 40);
            this.label_Bcr_Ip1.TabIndex = 119;
            this.label_Bcr_Ip1.Text = "192.168";
            this.label_Bcr_Ip1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Bcr_Ip3
            // 
            this.label_Bcr_Ip3.BackColor = System.Drawing.Color.White;
            this.label_Bcr_Ip3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Bcr_Ip3.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Bcr_Ip3.ForeColor = System.Drawing.Color.Black;
            this.label_Bcr_Ip3.Location = new System.Drawing.Point(628, 78);
            this.label_Bcr_Ip3.Name = "label_Bcr_Ip3";
            this.label_Bcr_Ip3.Size = new System.Drawing.Size(65, 40);
            this.label_Bcr_Ip3.TabIndex = 121;
            this.label_Bcr_Ip3.Text = "0";
            this.label_Bcr_Ip3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Bcr_Ip3.Click += new System.EventHandler(this.label_Bcr_Ip3_Click);
            // 
            // label_Bcr_Ip2
            // 
            this.label_Bcr_Ip2.BackColor = System.Drawing.Color.White;
            this.label_Bcr_Ip2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Bcr_Ip2.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Bcr_Ip2.ForeColor = System.Drawing.Color.Black;
            this.label_Bcr_Ip2.Location = new System.Drawing.Point(563, 78);
            this.label_Bcr_Ip2.Name = "label_Bcr_Ip2";
            this.label_Bcr_Ip2.Size = new System.Drawing.Size(65, 40);
            this.label_Bcr_Ip2.TabIndex = 120;
            this.label_Bcr_Ip2.Text = "0";
            this.label_Bcr_Ip2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Bcr_Ip2.Click += new System.EventHandler(this.label_Bcr_Ip2_Click);
            // 
            // label_Config_Bcr_Port
            // 
            this.label_Config_Bcr_Port.BackColor = System.Drawing.SystemColors.Window;
            this.label_Config_Bcr_Port.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Bcr_Port.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Config_Bcr_Port.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Bcr_Port.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Bcr_Port.Location = new System.Drawing.Point(490, 119);
            this.label_Config_Bcr_Port.Name = "label_Config_Bcr_Port";
            this.label_Config_Bcr_Port.Size = new System.Drawing.Size(103, 37);
            this.label_Config_Bcr_Port.TabIndex = 122;
            this.label_Config_Bcr_Port.Text = "BCR Port";
            this.label_Config_Bcr_Port.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Bcr_Port
            // 
            this.label_Bcr_Port.BackColor = System.Drawing.Color.White;
            this.label_Bcr_Port.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Bcr_Port.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Bcr_Port.ForeColor = System.Drawing.Color.Black;
            this.label_Bcr_Port.Location = new System.Drawing.Point(595, 119);
            this.label_Bcr_Port.Name = "label_Bcr_Port";
            this.label_Bcr_Port.Size = new System.Drawing.Size(98, 37);
            this.label_Bcr_Port.TabIndex = 123;
            this.label_Bcr_Port.Text = "1";
            this.label_Bcr_Port.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Bcr_Port.Click += new System.EventHandler(this.label_Bcr_Port_Click);
            // 
            // Config_Option
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_Bcr_Port);
            this.Controls.Add(this.label_Config_Bcr_Port);
            this.Controls.Add(this.label_Bcr_Ip3);
            this.Controls.Add(this.label_Bcr_Ip2);
            this.Controls.Add(this.label_Bcr_Ip1);
            this.Controls.Add(this.label_Config_Tray_Max_Layer_Val);
            this.Controls.Add(this.label_Config_Tray_Max_Layer);
            this.Controls.Add(this.label_Config_Ngtray_Max_Count_Y);
            this.Controls.Add(this.label_Config_Ngtray_Max_Count_X);
            this.Controls.Add(this.label_Config_Ngtray_Max_Count);
            this.Controls.Add(this.label_Config_Tray_Max_Count_Y);
            this.Controls.Add(this.label_Config_Tray_Max_Count_X);
            this.Controls.Add(this.label_Config_Tray_Max_Count);
            this.Controls.Add(this.label_ConfigOption_Tray_Max_Count);
            this.Controls.Add(this.comboBox_BcrPort);
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
            this.Controls.Add(this.button_Bcr_Connect);
            this.Controls.Add(this.label_Config_Bcr);
            this.Name = "Config_Option";
            this.Size = new System.Drawing.Size(770, 900);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_Bcr_DisConnect;
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
        private System.Windows.Forms.ComboBox comboBox_BcrPort;
        private System.Windows.Forms.Label label_ConfigOption_Tray_Max_Count;
        private System.Windows.Forms.Label label_Config_Tray_Max_Count_Y;
        private System.Windows.Forms.Label label_Config_Tray_Max_Count_X;
        private System.Windows.Forms.Label label_Config_Tray_Max_Count;
        private System.Windows.Forms.Label label_Config_Ngtray_Max_Count_Y;
        private System.Windows.Forms.Label label_Config_Ngtray_Max_Count_X;
        private System.Windows.Forms.Label label_Config_Ngtray_Max_Count;
        private System.Windows.Forms.Label label_Config_Tray_Max_Layer;
        private System.Windows.Forms.Label label_Config_Tray_Max_Layer_Val;
        private System.Windows.Forms.Label label_Bcr_Ip1;
        private System.Windows.Forms.Label label_Bcr_Ip3;
        private System.Windows.Forms.Label label_Bcr_Ip2;
        public System.Windows.Forms.Label label_Config_Bcr_Port;
        private System.Windows.Forms.Label label_Bcr_Port;
    }
}
