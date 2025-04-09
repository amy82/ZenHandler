namespace ZenHandler.Dlg
{
    partial class CCdControl
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
            this.CcdPanel = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label_ImageInfo = new System.Windows.Forms.Label();
            this.button_Ini_Select = new System.Windows.Forms.Button();
            this.ComboBox_IniList = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BTN_CCD_ROI_SAVE = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.BTN_CCD_EEPROM_VERIFY_TEST = new System.Windows.Forms.Button();
            this.dataGridView_EEpromData = new System.Windows.Forms.DataGridView();
            this.textBox_ReadDataLeng = new System.Windows.Forms.TextBox();
            this.textBox_ReadAddr = new System.Windows.Forms.TextBox();
            this.textBox_SlaveAddr = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BTN_CCD_EEPROM_READ = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BTN_CCD_RAW_SAVE = new System.Windows.Forms.Button();
            this.BTN_CCD_RAW_LOAD = new System.Windows.Forms.Button();
            this.BTN_CCD_BMP_SAVE = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BTN_CCD_BMP_LOAD = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BTN_CCD_GABBER_CLOSE = new System.Windows.Forms.Button();
            this.BTN_CCD_GABBER_STOP = new System.Windows.Forms.Button();
            this.BTN_CCD_GABBER_START = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.BTN_CCD_GABBER_OPEN = new System.Windows.Forms.Button();
            this.BTN_MANUAL_PCB = new System.Windows.Forms.Button();
            this.BTN_MANUAL_LENS = new System.Windows.Forms.Button();
            this.CcdPanel.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_EEpromData)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ManualTitleLabel
            // 
            this.ManualTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ManualTitleLabel.Location = new System.Drawing.Point(16, 16);
            this.ManualTitleLabel.Name = "ManualTitleLabel";
            this.ManualTitleLabel.Size = new System.Drawing.Size(250, 42);
            this.ManualTitleLabel.TabIndex = 2;
            this.ManualTitleLabel.Text = "| CCD";
            this.ManualTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CcdPanel
            // 
            this.CcdPanel.Controls.Add(this.groupBox5);
            this.CcdPanel.Controls.Add(this.groupBox4);
            this.CcdPanel.Controls.Add(this.groupBox3);
            this.CcdPanel.Controls.Add(this.groupBox1);
            this.CcdPanel.Controls.Add(this.groupBox2);
            this.CcdPanel.Location = new System.Drawing.Point(21, 97);
            this.CcdPanel.Name = "CcdPanel";
            this.CcdPanel.Size = new System.Drawing.Size(1008, 904);
            this.CcdPanel.TabIndex = 4;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.White;
            this.groupBox5.Controls.Add(this.label_ImageInfo);
            this.groupBox5.Controls.Add(this.button_Ini_Select);
            this.groupBox5.Controls.Add(this.ComboBox_IniList);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Location = new System.Drawing.Point(17, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(972, 128);
            this.groupBox5.TabIndex = 48;
            this.groupBox5.TabStop = false;
            // 
            // label_ImageInfo
            // 
            this.label_ImageInfo.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_ImageInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_ImageInfo.Location = new System.Drawing.Point(19, 93);
            this.label_ImageInfo.Name = "label_ImageInfo";
            this.label_ImageInfo.Size = new System.Drawing.Size(389, 23);
            this.label_ImageInfo.TabIndex = 35;
            this.label_ImageInfo.Text = "Image Info";
            this.label_ImageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_Ini_Select
            // 
            this.button_Ini_Select.BackColor = System.Drawing.Color.Tan;
            this.button_Ini_Select.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Ini_Select.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Ini_Select.ForeColor = System.Drawing.Color.White;
            this.button_Ini_Select.Location = new System.Drawing.Point(870, 83);
            this.button_Ini_Select.Name = "button_Ini_Select";
            this.button_Ini_Select.Size = new System.Drawing.Size(84, 36);
            this.button_Ini_Select.TabIndex = 31;
            this.button_Ini_Select.Text = "Select";
            this.button_Ini_Select.UseVisualStyleBackColor = false;
            this.button_Ini_Select.Click += new System.EventHandler(this.button_Ini_Select_Click);
            // 
            // ComboBox_IniList
            // 
            this.ComboBox_IniList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_IniList.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ComboBox_IniList.FormattingEnabled = true;
            this.ComboBox_IniList.ItemHeight = 19;
            this.ComboBox_IniList.Items.AddRange(new object[] {
            "Default.ini"});
            this.ComboBox_IniList.Location = new System.Drawing.Point(14, 43);
            this.ComboBox_IniList.Name = "ComboBox_IniList";
            this.ComboBox_IniList.Size = new System.Drawing.Size(940, 27);
            this.ComboBox_IniList.TabIndex = 34;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(11, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 23);
            this.label9.TabIndex = 26;
            this.label9.Text = "Sensor Ini";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.White;
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.BTN_CCD_ROI_SAVE);
            this.groupBox4.Location = new System.Drawing.Point(761, 548);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(169, 133);
            this.groupBox4.TabIndex = 47;
            this.groupBox4.TabStop = false;
            this.groupBox4.Visible = false;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(10, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 23);
            this.label4.TabIndex = 26;
            this.label4.Text = "CHART";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_CCD_ROI_SAVE
            // 
            this.BTN_CCD_ROI_SAVE.BackColor = System.Drawing.Color.Tan;
            this.BTN_CCD_ROI_SAVE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CCD_ROI_SAVE.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CCD_ROI_SAVE.ForeColor = System.Drawing.Color.White;
            this.BTN_CCD_ROI_SAVE.Location = new System.Drawing.Point(26, 58);
            this.BTN_CCD_ROI_SAVE.Name = "BTN_CCD_ROI_SAVE";
            this.BTN_CCD_ROI_SAVE.Size = new System.Drawing.Size(114, 46);
            this.BTN_CCD_ROI_SAVE.TabIndex = 27;
            this.BTN_CCD_ROI_SAVE.Text = "ROI SAVE";
            this.BTN_CCD_ROI_SAVE.UseVisualStyleBackColor = false;
            this.BTN_CCD_ROI_SAVE.Click += new System.EventHandler(this.BTN_CCD_ROI_SAVE_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.White;
            this.groupBox3.Controls.Add(this.BTN_CCD_EEPROM_VERIFY_TEST);
            this.groupBox3.Controls.Add(this.dataGridView_EEpromData);
            this.groupBox3.Controls.Add(this.textBox_ReadDataLeng);
            this.groupBox3.Controls.Add(this.textBox_ReadAddr);
            this.groupBox3.Controls.Add(this.textBox_SlaveAddr);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.BTN_CCD_EEPROM_READ);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Location = new System.Drawing.Point(17, 153);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(502, 731);
            this.groupBox3.TabIndex = 46;
            this.groupBox3.TabStop = false;
            // 
            // BTN_CCD_EEPROM_VERIFY_TEST
            // 
            this.BTN_CCD_EEPROM_VERIFY_TEST.BackColor = System.Drawing.Color.Tan;
            this.BTN_CCD_EEPROM_VERIFY_TEST.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CCD_EEPROM_VERIFY_TEST.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CCD_EEPROM_VERIFY_TEST.ForeColor = System.Drawing.Color.White;
            this.BTN_CCD_EEPROM_VERIFY_TEST.Location = new System.Drawing.Point(344, 115);
            this.BTN_CCD_EEPROM_VERIFY_TEST.Name = "BTN_CCD_EEPROM_VERIFY_TEST";
            this.BTN_CCD_EEPROM_VERIFY_TEST.Size = new System.Drawing.Size(142, 55);
            this.BTN_CCD_EEPROM_VERIFY_TEST.TabIndex = 37;
            this.BTN_CCD_EEPROM_VERIFY_TEST.Text = "EEprom Verify ManualTest";
            this.BTN_CCD_EEPROM_VERIFY_TEST.UseVisualStyleBackColor = false;
            this.BTN_CCD_EEPROM_VERIFY_TEST.Click += new System.EventHandler(this.BTN_CCD_EEPROM_VERIFY_TEST_Click);
            // 
            // dataGridView_EEpromData
            // 
            this.dataGridView_EEpromData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_EEpromData.Location = new System.Drawing.Point(18, 199);
            this.dataGridView_EEpromData.Name = "dataGridView_EEpromData";
            this.dataGridView_EEpromData.RowTemplate.Height = 23;
            this.dataGridView_EEpromData.Size = new System.Drawing.Size(291, 509);
            this.dataGridView_EEpromData.TabIndex = 36;
            // 
            // textBox_ReadDataLeng
            // 
            this.textBox_ReadDataLeng.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_ReadDataLeng.Location = new System.Drawing.Point(158, 121);
            this.textBox_ReadDataLeng.Name = "textBox_ReadDataLeng";
            this.textBox_ReadDataLeng.Size = new System.Drawing.Size(123, 25);
            this.textBox_ReadDataLeng.TabIndex = 35;
            this.textBox_ReadDataLeng.Text = "1";
            this.textBox_ReadDataLeng.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_ReadAddr
            // 
            this.textBox_ReadAddr.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_ReadAddr.Location = new System.Drawing.Point(158, 91);
            this.textBox_ReadAddr.Name = "textBox_ReadAddr";
            this.textBox_ReadAddr.Size = new System.Drawing.Size(123, 25);
            this.textBox_ReadAddr.TabIndex = 34;
            this.textBox_ReadAddr.Text = "0";
            this.textBox_ReadAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_SlaveAddr
            // 
            this.textBox_SlaveAddr.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_SlaveAddr.Location = new System.Drawing.Point(158, 60);
            this.textBox_SlaveAddr.MaxLength = 4;
            this.textBox_SlaveAddr.Name = "textBox_SlaveAddr";
            this.textBox_SlaveAddr.Size = new System.Drawing.Size(123, 25);
            this.textBox_SlaveAddr.TabIndex = 33;
            this.textBox_SlaveAddr.Text = "50";
            this.textBox_SlaveAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(15, 173);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 23);
            this.label8.TabIndex = 32;
            this.label8.Text = "Read Data";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(15, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(145, 23);
            this.label7.TabIndex = 31;
            this.label7.Text = "Read Data Length";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(15, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 23);
            this.label6.TabIndex = 30;
            this.label6.Text = "Read Addr [Hex]";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(15, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 23);
            this.label5.TabIndex = 29;
            this.label5.Text = "Slave Addr [Hex]";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BTN_CCD_EEPROM_READ
            // 
            this.BTN_CCD_EEPROM_READ.BackColor = System.Drawing.Color.Tan;
            this.BTN_CCD_EEPROM_READ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CCD_EEPROM_READ.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CCD_EEPROM_READ.ForeColor = System.Drawing.Color.White;
            this.BTN_CCD_EEPROM_READ.Location = new System.Drawing.Point(344, 58);
            this.BTN_CCD_EEPROM_READ.Name = "BTN_CCD_EEPROM_READ";
            this.BTN_CCD_EEPROM_READ.Size = new System.Drawing.Size(142, 46);
            this.BTN_CCD_EEPROM_READ.TabIndex = 28;
            this.BTN_CCD_EEPROM_READ.Text = "EEprom Read";
            this.BTN_CCD_EEPROM_READ.UseVisualStyleBackColor = false;
            this.BTN_CCD_EEPROM_READ.Click += new System.EventHandler(this.BTN_CCD_EEPROM_READ_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(164, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 23);
            this.label2.TabIndex = 26;
            this.label2.Text = "EEPROM";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Tan;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(360, 420);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(142, 46);
            this.button4.TabIndex = 27;
            this.button4.Text = "Sensor Id Read";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.BTN_CCD_RAW_SAVE);
            this.groupBox1.Controls.Add(this.BTN_CCD_RAW_LOAD);
            this.groupBox1.Controls.Add(this.BTN_CCD_BMP_SAVE);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.BTN_CCD_BMP_LOAD);
            this.groupBox1.Location = new System.Drawing.Point(802, 268);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(169, 258);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // BTN_CCD_RAW_SAVE
            // 
            this.BTN_CCD_RAW_SAVE.BackColor = System.Drawing.Color.Tan;
            this.BTN_CCD_RAW_SAVE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CCD_RAW_SAVE.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CCD_RAW_SAVE.ForeColor = System.Drawing.Color.White;
            this.BTN_CCD_RAW_SAVE.Location = new System.Drawing.Point(27, 196);
            this.BTN_CCD_RAW_SAVE.Name = "BTN_CCD_RAW_SAVE";
            this.BTN_CCD_RAW_SAVE.Size = new System.Drawing.Size(114, 46);
            this.BTN_CCD_RAW_SAVE.TabIndex = 30;
            this.BTN_CCD_RAW_SAVE.Text = "RAW SAVE";
            this.BTN_CCD_RAW_SAVE.UseVisualStyleBackColor = false;
            this.BTN_CCD_RAW_SAVE.Click += new System.EventHandler(this.BTN_CCD_RAW_SAVE_Click);
            // 
            // BTN_CCD_RAW_LOAD
            // 
            this.BTN_CCD_RAW_LOAD.BackColor = System.Drawing.Color.Tan;
            this.BTN_CCD_RAW_LOAD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CCD_RAW_LOAD.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CCD_RAW_LOAD.ForeColor = System.Drawing.Color.White;
            this.BTN_CCD_RAW_LOAD.Location = new System.Drawing.Point(27, 150);
            this.BTN_CCD_RAW_LOAD.Name = "BTN_CCD_RAW_LOAD";
            this.BTN_CCD_RAW_LOAD.Size = new System.Drawing.Size(114, 46);
            this.BTN_CCD_RAW_LOAD.TabIndex = 29;
            this.BTN_CCD_RAW_LOAD.Text = "RAW LOAD";
            this.BTN_CCD_RAW_LOAD.UseVisualStyleBackColor = false;
            this.BTN_CCD_RAW_LOAD.Click += new System.EventHandler(this.BTN_CCD_RAW_LOAD_Click);
            // 
            // BTN_CCD_BMP_SAVE
            // 
            this.BTN_CCD_BMP_SAVE.BackColor = System.Drawing.Color.Tan;
            this.BTN_CCD_BMP_SAVE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CCD_BMP_SAVE.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CCD_BMP_SAVE.ForeColor = System.Drawing.Color.White;
            this.BTN_CCD_BMP_SAVE.Location = new System.Drawing.Point(27, 104);
            this.BTN_CCD_BMP_SAVE.Name = "BTN_CCD_BMP_SAVE";
            this.BTN_CCD_BMP_SAVE.Size = new System.Drawing.Size(114, 46);
            this.BTN_CCD_BMP_SAVE.TabIndex = 28;
            this.BTN_CCD_BMP_SAVE.Text = "BMP SAVE";
            this.BTN_CCD_BMP_SAVE.UseVisualStyleBackColor = false;
            this.BTN_CCD_BMP_SAVE.Click += new System.EventHandler(this.BTN_CCD_BMP_SAVE_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(18, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 23);
            this.label1.TabIndex = 26;
            this.label1.Text = "IMAGE";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_CCD_BMP_LOAD
            // 
            this.BTN_CCD_BMP_LOAD.BackColor = System.Drawing.Color.Tan;
            this.BTN_CCD_BMP_LOAD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CCD_BMP_LOAD.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CCD_BMP_LOAD.ForeColor = System.Drawing.Color.White;
            this.BTN_CCD_BMP_LOAD.Location = new System.Drawing.Point(27, 58);
            this.BTN_CCD_BMP_LOAD.Name = "BTN_CCD_BMP_LOAD";
            this.BTN_CCD_BMP_LOAD.Size = new System.Drawing.Size(114, 46);
            this.BTN_CCD_BMP_LOAD.TabIndex = 27;
            this.BTN_CCD_BMP_LOAD.Text = "BMP LOAD";
            this.BTN_CCD_BMP_LOAD.UseVisualStyleBackColor = false;
            this.BTN_CCD_BMP_LOAD.Click += new System.EventHandler(this.BTN_CCD_BMP_LOAD_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.BTN_CCD_GABBER_CLOSE);
            this.groupBox2.Controls.Add(this.BTN_CCD_GABBER_STOP);
            this.groupBox2.Controls.Add(this.BTN_CCD_GABBER_START);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.BTN_CCD_GABBER_OPEN);
            this.groupBox2.Location = new System.Drawing.Point(565, 153);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(169, 258);
            this.groupBox2.TabIndex = 45;
            this.groupBox2.TabStop = false;
            // 
            // BTN_CCD_GABBER_CLOSE
            // 
            this.BTN_CCD_GABBER_CLOSE.BackColor = System.Drawing.Color.Tan;
            this.BTN_CCD_GABBER_CLOSE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CCD_GABBER_CLOSE.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CCD_GABBER_CLOSE.ForeColor = System.Drawing.Color.White;
            this.BTN_CCD_GABBER_CLOSE.Location = new System.Drawing.Point(27, 196);
            this.BTN_CCD_GABBER_CLOSE.Name = "BTN_CCD_GABBER_CLOSE";
            this.BTN_CCD_GABBER_CLOSE.Size = new System.Drawing.Size(114, 46);
            this.BTN_CCD_GABBER_CLOSE.TabIndex = 30;
            this.BTN_CCD_GABBER_CLOSE.Text = "CLOSE";
            this.BTN_CCD_GABBER_CLOSE.UseVisualStyleBackColor = false;
            this.BTN_CCD_GABBER_CLOSE.Click += new System.EventHandler(this.BTN_CCD_GABBER_CLOSE_Click);
            // 
            // BTN_CCD_GABBER_STOP
            // 
            this.BTN_CCD_GABBER_STOP.BackColor = System.Drawing.Color.Tan;
            this.BTN_CCD_GABBER_STOP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CCD_GABBER_STOP.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CCD_GABBER_STOP.ForeColor = System.Drawing.Color.White;
            this.BTN_CCD_GABBER_STOP.Location = new System.Drawing.Point(27, 150);
            this.BTN_CCD_GABBER_STOP.Name = "BTN_CCD_GABBER_STOP";
            this.BTN_CCD_GABBER_STOP.Size = new System.Drawing.Size(114, 46);
            this.BTN_CCD_GABBER_STOP.TabIndex = 29;
            this.BTN_CCD_GABBER_STOP.Text = "STOP";
            this.BTN_CCD_GABBER_STOP.UseVisualStyleBackColor = false;
            this.BTN_CCD_GABBER_STOP.Click += new System.EventHandler(this.BTN_CCD_GABBER_STOP_Click);
            // 
            // BTN_CCD_GABBER_START
            // 
            this.BTN_CCD_GABBER_START.BackColor = System.Drawing.Color.Tan;
            this.BTN_CCD_GABBER_START.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CCD_GABBER_START.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CCD_GABBER_START.ForeColor = System.Drawing.Color.White;
            this.BTN_CCD_GABBER_START.Location = new System.Drawing.Point(27, 104);
            this.BTN_CCD_GABBER_START.Name = "BTN_CCD_GABBER_START";
            this.BTN_CCD_GABBER_START.Size = new System.Drawing.Size(114, 46);
            this.BTN_CCD_GABBER_START.TabIndex = 28;
            this.BTN_CCD_GABBER_START.Text = "START";
            this.BTN_CCD_GABBER_START.UseVisualStyleBackColor = false;
            this.BTN_CCD_GABBER_START.Click += new System.EventHandler(this.BTN_CCD_GABBER_START_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(13, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 23);
            this.label3.TabIndex = 26;
            this.label3.Text = "GRABBER";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_CCD_GABBER_OPEN
            // 
            this.BTN_CCD_GABBER_OPEN.BackColor = System.Drawing.Color.Tan;
            this.BTN_CCD_GABBER_OPEN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CCD_GABBER_OPEN.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CCD_GABBER_OPEN.ForeColor = System.Drawing.Color.White;
            this.BTN_CCD_GABBER_OPEN.Location = new System.Drawing.Point(27, 58);
            this.BTN_CCD_GABBER_OPEN.Name = "BTN_CCD_GABBER_OPEN";
            this.BTN_CCD_GABBER_OPEN.Size = new System.Drawing.Size(114, 46);
            this.BTN_CCD_GABBER_OPEN.TabIndex = 27;
            this.BTN_CCD_GABBER_OPEN.Text = "OPEN";
            this.BTN_CCD_GABBER_OPEN.UseVisualStyleBackColor = false;
            this.BTN_CCD_GABBER_OPEN.Click += new System.EventHandler(this.BTN_CCD_GABBER_OPEN_Click);
            // 
            // BTN_MANUAL_PCB
            // 
            this.BTN_MANUAL_PCB.BackColor = System.Drawing.Color.Tan;
            this.BTN_MANUAL_PCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MANUAL_PCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MANUAL_PCB.ForeColor = System.Drawing.Color.White;
            this.BTN_MANUAL_PCB.Location = new System.Drawing.Point(616, 16);
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
            this.BTN_MANUAL_LENS.Location = new System.Drawing.Point(776, 16);
            this.BTN_MANUAL_LENS.Name = "BTN_MANUAL_LENS";
            this.BTN_MANUAL_LENS.Size = new System.Drawing.Size(154, 44);
            this.BTN_MANUAL_LENS.TabIndex = 31;
            this.BTN_MANUAL_LENS.Text = "LENS";
            this.BTN_MANUAL_LENS.UseVisualStyleBackColor = false;
            this.BTN_MANUAL_LENS.Visible = false;
            this.BTN_MANUAL_LENS.Click += new System.EventHandler(this.BTN_MANUAL_LENS_Click);
            // 
            // CCdControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.Controls.Add(this.BTN_MANUAL_LENS);
            this.Controls.Add(this.BTN_MANUAL_PCB);
            this.Controls.Add(this.CcdPanel);
            this.Controls.Add(this.ManualTitleLabel);
            this.Name = "CCdControl";
            this.Size = new System.Drawing.Size(1044, 1004);
            this.VisibleChanged += new System.EventHandler(this.CCdControl_VisibleChanged);
            this.CcdPanel.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_EEpromData)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ManualTitleLabel;
        private System.Windows.Forms.Panel CcdPanel;
        private System.Windows.Forms.Button BTN_MANUAL_PCB;
        private System.Windows.Forms.Button BTN_MANUAL_LENS;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BTN_CCD_GABBER_CLOSE;
        private System.Windows.Forms.Button BTN_CCD_GABBER_STOP;
        private System.Windows.Forms.Button BTN_CCD_GABBER_START;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BTN_CCD_GABBER_OPEN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BTN_CCD_RAW_SAVE;
        private System.Windows.Forms.Button BTN_CCD_RAW_LOAD;
        private System.Windows.Forms.Button BTN_CCD_BMP_SAVE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BTN_CCD_BMP_LOAD;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button BTN_CCD_EEPROM_READ;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BTN_CCD_ROI_SAVE;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_ReadDataLeng;
        private System.Windows.Forms.TextBox textBox_ReadAddr;
        private System.Windows.Forms.TextBox textBox_SlaveAddr;
        private System.Windows.Forms.DataGridView dataGridView_EEpromData;
        private System.Windows.Forms.Button BTN_CCD_EEPROM_VERIFY_TEST;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox ComboBox_IniList;
        private System.Windows.Forms.Button button_Ini_Select;
        private System.Windows.Forms.Label label_ImageInfo;
    }
}
