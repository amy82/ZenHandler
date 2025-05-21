
namespace ZenHandler.Dlg
{
    partial class ManualMagazine
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
            this.label_Manual_Magazine_Sensor_Title = new System.Windows.Forms.Label();
            this.button_Manual_Magazine_Layer3_Pos_Z = new System.Windows.Forms.Button();
            this.button_Manual_Magazine_Layer3_Pos_Y = new System.Windows.Forms.Button();
            this.button_Manual_Magazine_Wait_Pos_Z = new System.Windows.Forms.Button();
            this.label_Manual_Magazine_Y_Title = new System.Windows.Forms.Label();
            this.button_Manual_Magazine_Wait_Pos_Y = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Manual_Magazine_Layer5_Pos_Z = new System.Windows.Forms.Button();
            this.button_Manual_Magazine_Layer2_Pos_Z = new System.Windows.Forms.Button();
            this.button_Manual_Magazine_Layer4_Pos_Z = new System.Windows.Forms.Button();
            this.label_Manual_Magazine_Z_Title = new System.Windows.Forms.Label();
            this.button_Manual_Magazine_Layer1_Pos_Z = new System.Windows.Forms.Button();
            this.comboBox_Manual_Magazine_Motor = new System.Windows.Forms.ComboBox();
            this.button_Manual_Magazine_Layer2_Pos_Y = new System.Windows.Forms.Button();
            this.button_Manual_Magazine_Layer1_Pos_Y = new System.Windows.Forms.Button();
            this.button_Manual_Magazine_Layer4_Pos_Y = new System.Windows.Forms.Button();
            this.button_Manual_Magazine_Layer5_Pos_Y = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_Manual_Magazine_Tray_Front_Detect = new System.Windows.Forms.Button();
            this.button_Manual_Magazine_Tray_Bottom_Detect = new System.Windows.Forms.Button();
            this.button_Manual_Magazine_Tray_Seat_Detect_ = new System.Windows.Forms.Button();
            this.button_Manual_Magazine_Seat_Detect = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_Manual_Magazine_Sensor_Title
            // 
            this.label_Manual_Magazine_Sensor_Title.BackColor = System.Drawing.Color.Transparent;
            this.label_Manual_Magazine_Sensor_Title.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Manual_Magazine_Sensor_Title.ForeColor = System.Drawing.Color.DimGray;
            this.label_Manual_Magazine_Sensor_Title.Location = new System.Drawing.Point(20, 13);
            this.label_Manual_Magazine_Sensor_Title.Name = "label_Manual_Magazine_Sensor_Title";
            this.label_Manual_Magazine_Sensor_Title.Size = new System.Drawing.Size(156, 23);
            this.label_Manual_Magazine_Sensor_Title.TabIndex = 26;
            this.label_Manual_Magazine_Sensor_Title.Text = "Detect Sensor";
            this.label_Manual_Magazine_Sensor_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Manual_Magazine_Layer3_Pos_Z
            // 
            this.button_Manual_Magazine_Layer3_Pos_Z.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Layer3_Pos_Z.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Layer3_Pos_Z.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Layer3_Pos_Z.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Layer3_Pos_Z.Location = new System.Drawing.Point(255, 263);
            this.button_Manual_Magazine_Layer3_Pos_Z.Name = "button_Manual_Magazine_Layer3_Pos_Z";
            this.button_Manual_Magazine_Layer3_Pos_Z.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Layer3_Pos_Z.TabIndex = 37;
            this.button_Manual_Magazine_Layer3_Pos_Z.Text = "Z LAYER 3";
            this.button_Manual_Magazine_Layer3_Pos_Z.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Layer3_Pos_Z.Click += new System.EventHandler(this.button_Manual_Magazine_Layer3_Pos_Z_Click);
            // 
            // button_Manual_Magazine_Layer3_Pos_Y
            // 
            this.button_Manual_Magazine_Layer3_Pos_Y.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Layer3_Pos_Y.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Layer3_Pos_Y.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Layer3_Pos_Y.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Layer3_Pos_Y.Location = new System.Drawing.Point(42, 263);
            this.button_Manual_Magazine_Layer3_Pos_Y.Name = "button_Manual_Magazine_Layer3_Pos_Y";
            this.button_Manual_Magazine_Layer3_Pos_Y.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Layer3_Pos_Y.TabIndex = 33;
            this.button_Manual_Magazine_Layer3_Pos_Y.Text = "LAYER 3";
            this.button_Manual_Magazine_Layer3_Pos_Y.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Layer3_Pos_Y.Click += new System.EventHandler(this.BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_XY_Click);
            // 
            // button_Manual_Magazine_Wait_Pos_Z
            // 
            this.button_Manual_Magazine_Wait_Pos_Z.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Wait_Pos_Z.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Wait_Pos_Z.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Wait_Pos_Z.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Wait_Pos_Z.Location = new System.Drawing.Point(255, 104);
            this.button_Manual_Magazine_Wait_Pos_Z.Name = "button_Manual_Magazine_Wait_Pos_Z";
            this.button_Manual_Magazine_Wait_Pos_Z.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Wait_Pos_Z.TabIndex = 32;
            this.button_Manual_Magazine_Wait_Pos_Z.Text = "Z WAIT POS";
            this.button_Manual_Magazine_Wait_Pos_Z.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Wait_Pos_Z.Click += new System.EventHandler(this.button_Manual_Magazine_Wait_Pos_Z_Click);
            // 
            // label_Manual_Magazine_Y_Title
            // 
            this.label_Manual_Magazine_Y_Title.BackColor = System.Drawing.Color.PapayaWhip;
            this.label_Manual_Magazine_Y_Title.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Manual_Magazine_Y_Title.ForeColor = System.Drawing.Color.Black;
            this.label_Manual_Magazine_Y_Title.Location = new System.Drawing.Point(42, 78);
            this.label_Manual_Magazine_Y_Title.Name = "label_Manual_Magazine_Y_Title";
            this.label_Manual_Magazine_Y_Title.Size = new System.Drawing.Size(153, 23);
            this.label_Manual_Magazine_Y_Title.TabIndex = 30;
            this.label_Manual_Magazine_Y_Title.Text = "Y AXIS";
            this.label_Manual_Magazine_Y_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Manual_Magazine_Wait_Pos_Y
            // 
            this.button_Manual_Magazine_Wait_Pos_Y.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Wait_Pos_Y.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Wait_Pos_Y.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Wait_Pos_Y.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Wait_Pos_Y.Location = new System.Drawing.Point(42, 104);
            this.button_Manual_Magazine_Wait_Pos_Y.Name = "button_Manual_Magazine_Wait_Pos_Y";
            this.button_Manual_Magazine_Wait_Pos_Y.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Wait_Pos_Y.TabIndex = 29;
            this.button_Manual_Magazine_Wait_Pos_Y.Text = "WAIT POS";
            this.button_Manual_Magazine_Wait_Pos_Y.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Wait_Pos_Y.Click += new System.EventHandler(this.button_Manual_Magazine_Wait_Pos_Y_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Layer5_Pos_Z);
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Layer2_Pos_Z);
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Layer4_Pos_Z);
            this.groupBox1.Controls.Add(this.label_Manual_Magazine_Z_Title);
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Layer1_Pos_Z);
            this.groupBox1.Controls.Add(this.comboBox_Manual_Magazine_Motor);
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Layer3_Pos_Z);
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Layer2_Pos_Y);
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Layer1_Pos_Y);
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Layer4_Pos_Y);
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Layer5_Pos_Y);
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Wait_Pos_Z);
            this.groupBox1.Controls.Add(this.label_Manual_Magazine_Y_Title);
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Wait_Pos_Y);
            this.groupBox1.Controls.Add(this.button_Manual_Magazine_Layer3_Pos_Y);
            this.groupBox1.Location = new System.Drawing.Point(299, 172);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(451, 464);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            // 
            // button_Manual_Magazine_Layer5_Pos_Z
            // 
            this.button_Manual_Magazine_Layer5_Pos_Z.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Layer5_Pos_Z.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Layer5_Pos_Z.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Layer5_Pos_Z.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Layer5_Pos_Z.Location = new System.Drawing.Point(255, 369);
            this.button_Manual_Magazine_Layer5_Pos_Z.Name = "button_Manual_Magazine_Layer5_Pos_Z";
            this.button_Manual_Magazine_Layer5_Pos_Z.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Layer5_Pos_Z.TabIndex = 60;
            this.button_Manual_Magazine_Layer5_Pos_Z.Text = "Z LAYER 5";
            this.button_Manual_Magazine_Layer5_Pos_Z.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Layer5_Pos_Z.Click += new System.EventHandler(this.button_Manual_Magazine_Layer5_Pos_Z_Click);
            // 
            // button_Manual_Magazine_Layer2_Pos_Z
            // 
            this.button_Manual_Magazine_Layer2_Pos_Z.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Layer2_Pos_Z.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Layer2_Pos_Z.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Layer2_Pos_Z.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Layer2_Pos_Z.Location = new System.Drawing.Point(255, 210);
            this.button_Manual_Magazine_Layer2_Pos_Z.Name = "button_Manual_Magazine_Layer2_Pos_Z";
            this.button_Manual_Magazine_Layer2_Pos_Z.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Layer2_Pos_Z.TabIndex = 55;
            this.button_Manual_Magazine_Layer2_Pos_Z.Text = "Z LAYER 2";
            this.button_Manual_Magazine_Layer2_Pos_Z.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Layer2_Pos_Z.Click += new System.EventHandler(this.button_Manual_Magazine_Layer2_Pos_Z_Click);
            // 
            // button_Manual_Magazine_Layer4_Pos_Z
            // 
            this.button_Manual_Magazine_Layer4_Pos_Z.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Layer4_Pos_Z.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Layer4_Pos_Z.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Layer4_Pos_Z.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Layer4_Pos_Z.Location = new System.Drawing.Point(255, 316);
            this.button_Manual_Magazine_Layer4_Pos_Z.Name = "button_Manual_Magazine_Layer4_Pos_Z";
            this.button_Manual_Magazine_Layer4_Pos_Z.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Layer4_Pos_Z.TabIndex = 41;
            this.button_Manual_Magazine_Layer4_Pos_Z.Text = "Z LAYER 4";
            this.button_Manual_Magazine_Layer4_Pos_Z.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Layer4_Pos_Z.Click += new System.EventHandler(this.button_Manual_Magazine_Layer4_Pos_Z_Click);
            // 
            // label_Manual_Magazine_Z_Title
            // 
            this.label_Manual_Magazine_Z_Title.BackColor = System.Drawing.Color.PapayaWhip;
            this.label_Manual_Magazine_Z_Title.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Manual_Magazine_Z_Title.ForeColor = System.Drawing.Color.Black;
            this.label_Manual_Magazine_Z_Title.Location = new System.Drawing.Point(255, 78);
            this.label_Manual_Magazine_Z_Title.Name = "label_Manual_Magazine_Z_Title";
            this.label_Manual_Magazine_Z_Title.Size = new System.Drawing.Size(155, 23);
            this.label_Manual_Magazine_Z_Title.TabIndex = 59;
            this.label_Manual_Magazine_Z_Title.Text = "Z AXIS";
            this.label_Manual_Magazine_Z_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Manual_Magazine_Layer1_Pos_Z
            // 
            this.button_Manual_Magazine_Layer1_Pos_Z.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Layer1_Pos_Z.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Layer1_Pos_Z.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Layer1_Pos_Z.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Layer1_Pos_Z.Location = new System.Drawing.Point(255, 157);
            this.button_Manual_Magazine_Layer1_Pos_Z.Name = "button_Manual_Magazine_Layer1_Pos_Z";
            this.button_Manual_Magazine_Layer1_Pos_Z.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Layer1_Pos_Z.TabIndex = 54;
            this.button_Manual_Magazine_Layer1_Pos_Z.Text = "Z LAYER 1";
            this.button_Manual_Magazine_Layer1_Pos_Z.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Layer1_Pos_Z.Click += new System.EventHandler(this.button_Manual_Magazine_Layer1_Pos_Z_Click);
            // 
            // comboBox_Manual_Magazine_Motor
            // 
            this.comboBox_Manual_Magazine_Motor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Manual_Magazine_Motor.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.comboBox_Manual_Magazine_Motor.FormattingEnabled = true;
            this.comboBox_Manual_Magazine_Motor.ItemHeight = 24;
            this.comboBox_Manual_Magazine_Motor.Location = new System.Drawing.Point(128, 20);
            this.comboBox_Manual_Magazine_Motor.Name = "comboBox_Manual_Magazine_Motor";
            this.comboBox_Manual_Magazine_Motor.Size = new System.Drawing.Size(201, 32);
            this.comboBox_Manual_Magazine_Motor.TabIndex = 53;
            this.comboBox_Manual_Magazine_Motor.DropDownClosed += new System.EventHandler(this.comboBox_Manual_Magazine_Motor_DropDownClosed);
            // 
            // button_Manual_Magazine_Layer2_Pos_Y
            // 
            this.button_Manual_Magazine_Layer2_Pos_Y.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Layer2_Pos_Y.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Layer2_Pos_Y.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Layer2_Pos_Y.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Layer2_Pos_Y.Location = new System.Drawing.Point(42, 210);
            this.button_Manual_Magazine_Layer2_Pos_Y.Name = "button_Manual_Magazine_Layer2_Pos_Y";
            this.button_Manual_Magazine_Layer2_Pos_Y.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Layer2_Pos_Y.TabIndex = 54;
            this.button_Manual_Magazine_Layer2_Pos_Y.Text = "LAYER 2";
            this.button_Manual_Magazine_Layer2_Pos_Y.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Layer2_Pos_Y.Click += new System.EventHandler(this.button_Manual_Transfer_Right_Bcr_Pos_XY_Click);
            // 
            // button_Manual_Magazine_Layer1_Pos_Y
            // 
            this.button_Manual_Magazine_Layer1_Pos_Y.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Layer1_Pos_Y.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Layer1_Pos_Y.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Layer1_Pos_Y.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Layer1_Pos_Y.Location = new System.Drawing.Point(42, 157);
            this.button_Manual_Magazine_Layer1_Pos_Y.Name = "button_Manual_Magazine_Layer1_Pos_Y";
            this.button_Manual_Magazine_Layer1_Pos_Y.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Layer1_Pos_Y.TabIndex = 53;
            this.button_Manual_Magazine_Layer1_Pos_Y.Text = "LAYER 1";
            this.button_Manual_Magazine_Layer1_Pos_Y.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Layer1_Pos_Y.Click += new System.EventHandler(this.button_Manual_Transfer_Left_Bcr_Pos_XY_Click);
            // 
            // button_Manual_Magazine_Layer4_Pos_Y
            // 
            this.button_Manual_Magazine_Layer4_Pos_Y.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Layer4_Pos_Y.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Layer4_Pos_Y.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Layer4_Pos_Y.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Layer4_Pos_Y.Location = new System.Drawing.Point(42, 316);
            this.button_Manual_Magazine_Layer4_Pos_Y.Name = "button_Manual_Magazine_Layer4_Pos_Y";
            this.button_Manual_Magazine_Layer4_Pos_Y.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Layer4_Pos_Y.TabIndex = 40;
            this.button_Manual_Magazine_Layer4_Pos_Y.Text = "LAYER 4";
            this.button_Manual_Magazine_Layer4_Pos_Y.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Layer4_Pos_Y.Click += new System.EventHandler(this.BTN_MANUAL_TRANSFER_LEFT_UNLOAD_POS_XY_Click);
            // 
            // button_Manual_Magazine_Layer5_Pos_Y
            // 
            this.button_Manual_Magazine_Layer5_Pos_Y.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Layer5_Pos_Y.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Layer5_Pos_Y.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Layer5_Pos_Y.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Layer5_Pos_Y.Location = new System.Drawing.Point(42, 369);
            this.button_Manual_Magazine_Layer5_Pos_Y.Name = "button_Manual_Magazine_Layer5_Pos_Y";
            this.button_Manual_Magazine_Layer5_Pos_Y.Size = new System.Drawing.Size(155, 51);
            this.button_Manual_Magazine_Layer5_Pos_Y.TabIndex = 38;
            this.button_Manual_Magazine_Layer5_Pos_Y.Text = "LAYER 5";
            this.button_Manual_Magazine_Layer5_Pos_Y.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine_Layer5_Pos_Y.Click += new System.EventHandler(this.BTN_MANUAL_TRANSFER_RIGHT_LOAD_POS_XY_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.button_Manual_Magazine_Tray_Front_Detect);
            this.groupBox2.Controls.Add(this.button_Manual_Magazine_Tray_Bottom_Detect);
            this.groupBox2.Controls.Add(this.button_Manual_Magazine_Tray_Seat_Detect_);
            this.groupBox2.Controls.Add(this.button_Manual_Magazine_Seat_Detect);
            this.groupBox2.Controls.Add(this.label_Manual_Magazine_Sensor_Title);
            this.groupBox2.Location = new System.Drawing.Point(10, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(753, 144);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            // 
            // button_Manual_Magazine_Tray_Front_Detect
            // 
            this.button_Manual_Magazine_Tray_Front_Detect.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Tray_Front_Detect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Tray_Front_Detect.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Tray_Front_Detect.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Tray_Front_Detect.Location = new System.Drawing.Point(377, 49);
            this.button_Manual_Magazine_Tray_Front_Detect.Name = "button_Manual_Magazine_Tray_Front_Detect";
            this.button_Manual_Magazine_Tray_Front_Detect.Size = new System.Drawing.Size(107, 46);
            this.button_Manual_Magazine_Tray_Front_Detect.TabIndex = 35;
            this.button_Manual_Magazine_Tray_Front_Detect.Text = "Magazine Front";
            this.button_Manual_Magazine_Tray_Front_Detect.UseVisualStyleBackColor = false;
            // 
            // button_Manual_Magazine_Tray_Bottom_Detect
            // 
            this.button_Manual_Magazine_Tray_Bottom_Detect.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Tray_Bottom_Detect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Tray_Bottom_Detect.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Tray_Bottom_Detect.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Tray_Bottom_Detect.Location = new System.Drawing.Point(264, 49);
            this.button_Manual_Magazine_Tray_Bottom_Detect.Name = "button_Manual_Magazine_Tray_Bottom_Detect";
            this.button_Manual_Magazine_Tray_Bottom_Detect.Size = new System.Drawing.Size(107, 46);
            this.button_Manual_Magazine_Tray_Bottom_Detect.TabIndex = 34;
            this.button_Manual_Magazine_Tray_Bottom_Detect.Text = "Magazine Bottom";
            this.button_Manual_Magazine_Tray_Bottom_Detect.UseVisualStyleBackColor = false;
            // 
            // button_Manual_Magazine_Tray_Seat_Detect_
            // 
            this.button_Manual_Magazine_Tray_Seat_Detect_.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Tray_Seat_Detect_.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Tray_Seat_Detect_.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Tray_Seat_Detect_.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Tray_Seat_Detect_.Location = new System.Drawing.Point(153, 49);
            this.button_Manual_Magazine_Tray_Seat_Detect_.Name = "button_Manual_Magazine_Tray_Seat_Detect_";
            this.button_Manual_Magazine_Tray_Seat_Detect_.Size = new System.Drawing.Size(107, 46);
            this.button_Manual_Magazine_Tray_Seat_Detect_.TabIndex = 33;
            this.button_Manual_Magazine_Tray_Seat_Detect_.Text = "Tray Seat";
            this.button_Manual_Magazine_Tray_Seat_Detect_.UseVisualStyleBackColor = false;
            // 
            // button_Manual_Magazine_Seat_Detect
            // 
            this.button_Manual_Magazine_Seat_Detect.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine_Seat_Detect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine_Seat_Detect.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine_Seat_Detect.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine_Seat_Detect.Location = new System.Drawing.Point(40, 49);
            this.button_Manual_Magazine_Seat_Detect.Name = "button_Manual_Magazine_Seat_Detect";
            this.button_Manual_Magazine_Seat_Detect.Size = new System.Drawing.Size(107, 46);
            this.button_Manual_Magazine_Seat_Detect.TabIndex = 32;
            this.button_Manual_Magazine_Seat_Detect.Text = "Magazine Seat";
            this.button_Manual_Magazine_Seat_Detect.UseVisualStyleBackColor = false;
            // 
            // ManualMagazine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ManualMagazine";
            this.Size = new System.Drawing.Size(770, 900);
            this.VisibleChanged += new System.EventHandler(this.ManualMagazine_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label_Manual_Magazine_Sensor_Title;
        private System.Windows.Forms.Button button_Manual_Magazine_Layer3_Pos_Z;
        private System.Windows.Forms.Button button_Manual_Magazine_Layer3_Pos_Y;
        private System.Windows.Forms.Button button_Manual_Magazine_Wait_Pos_Z;
        private System.Windows.Forms.Label label_Manual_Magazine_Y_Title;
        private System.Windows.Forms.Button button_Manual_Magazine_Wait_Pos_Y;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_Manual_Magazine_Layer5_Pos_Y;
        private System.Windows.Forms.Button button_Manual_Magazine_Layer4_Pos_Z;
        private System.Windows.Forms.Button button_Manual_Magazine_Layer4_Pos_Y;
        private System.Windows.Forms.Button button_Manual_Magazine_Layer2_Pos_Y;
        private System.Windows.Forms.Button button_Manual_Magazine_Layer1_Pos_Y;
        private System.Windows.Forms.Button button_Manual_Magazine_Layer2_Pos_Z;
        private System.Windows.Forms.Button button_Manual_Magazine_Layer1_Pos_Z;
        private System.Windows.Forms.Label label_Manual_Magazine_Z_Title;
        private System.Windows.Forms.ComboBox comboBox_Manual_Magazine_Motor;
        private System.Windows.Forms.Button button_Manual_Magazine_Layer5_Pos_Z;
        private System.Windows.Forms.Button button_Manual_Magazine_Tray_Front_Detect;
        private System.Windows.Forms.Button button_Manual_Magazine_Tray_Bottom_Detect;
        private System.Windows.Forms.Button button_Manual_Magazine_Tray_Seat_Detect_;
        private System.Windows.Forms.Button button_Manual_Magazine_Seat_Detect;
    }
}
