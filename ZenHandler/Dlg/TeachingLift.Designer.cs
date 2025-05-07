
namespace ZenHandler.Dlg
{
    partial class TeachingLift
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
            this.groupTeachPcb = new System.Windows.Forms.GroupBox();
            this.BTN_TEACH_LIFT_BACK_X = new System.Windows.Forms.Button();
            this.LABEL_TEACH_ROSOLUTION_VALUE = new System.Windows.Forms.Label();
            this.BTN_TEACH_DATA_SAVE = new System.Windows.Forms.Button();
            this.label_Resolution = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BTN_TEACH_LIFT_RIGHT_Z = new System.Windows.Forms.Button();
            this.BTN_TEACH_SERVO_ON = new System.Windows.Forms.Button();
            this.BTN_TEACH_SERVO_OFF = new System.Windows.Forms.Button();
            this.BTN_TEACH_SERVO_RESET = new System.Windows.Forms.Button();
            this.BTN_TEACH_LIFT_FRONT_X = new System.Windows.Forms.Button();
            this.BTN_TEACH_LIFT_LEFT_Z = new System.Windows.Forms.Button();
            this.groupTeachPcb.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupTeachPcb
            // 
            this.groupTeachPcb.BackColor = System.Drawing.Color.White;
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_LIFT_BACK_X);
            this.groupTeachPcb.Controls.Add(this.LABEL_TEACH_ROSOLUTION_VALUE);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_DATA_SAVE);
            this.groupTeachPcb.Controls.Add(this.label_Resolution);
            this.groupTeachPcb.Controls.Add(this.label4);
            this.groupTeachPcb.Controls.Add(this.label3);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_LIFT_RIGHT_Z);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_SERVO_ON);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_SERVO_OFF);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_SERVO_RESET);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_LIFT_FRONT_X);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_LIFT_LEFT_Z);
            this.groupTeachPcb.Location = new System.Drawing.Point(8, 3);
            this.groupTeachPcb.Name = "groupTeachPcb";
            this.groupTeachPcb.Size = new System.Drawing.Size(756, 750);
            this.groupTeachPcb.TabIndex = 45;
            this.groupTeachPcb.TabStop = false;
            // 
            // BTN_TEACH_LIFT_BACK_X
            // 
            this.BTN_TEACH_LIFT_BACK_X.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_LIFT_BACK_X.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_LIFT_BACK_X.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_LIFT_BACK_X.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_LIFT_BACK_X.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_LIFT_BACK_X.Location = new System.Drawing.Point(17, 346);
            this.BTN_TEACH_LIFT_BACK_X.Name = "BTN_TEACH_LIFT_BACK_X";
            this.BTN_TEACH_LIFT_BACK_X.Size = new System.Drawing.Size(117, 42);
            this.BTN_TEACH_LIFT_BACK_X.TabIndex = 48;
            this.BTN_TEACH_LIFT_BACK_X.Text = "BACK X";
            this.BTN_TEACH_LIFT_BACK_X.UseVisualStyleBackColor = false;
            this.BTN_TEACH_LIFT_BACK_X.Click += new System.EventHandler(this.BTN_TEACH_MAGAZINE_RIGHT_Z_Click);
            // 
            // LABEL_TEACH_ROSOLUTION_VALUE
            // 
            this.LABEL_TEACH_ROSOLUTION_VALUE.BackColor = System.Drawing.SystemColors.Window;
            this.LABEL_TEACH_ROSOLUTION_VALUE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LABEL_TEACH_ROSOLUTION_VALUE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LABEL_TEACH_ROSOLUTION_VALUE.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LABEL_TEACH_ROSOLUTION_VALUE.ForeColor = System.Drawing.Color.DimGray;
            this.LABEL_TEACH_ROSOLUTION_VALUE.Location = new System.Drawing.Point(17, 447);
            this.LABEL_TEACH_ROSOLUTION_VALUE.Name = "LABEL_TEACH_ROSOLUTION_VALUE";
            this.LABEL_TEACH_ROSOLUTION_VALUE.Size = new System.Drawing.Size(117, 37);
            this.LABEL_TEACH_ROSOLUTION_VALUE.TabIndex = 47;
            this.LABEL_TEACH_ROSOLUTION_VALUE.Text = "2.0";
            this.LABEL_TEACH_ROSOLUTION_VALUE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LABEL_TEACH_ROSOLUTION_VALUE.Click += new System.EventHandler(this.LABEL_TEACH_ROSOLUTION_VALUE_Click);
            // 
            // BTN_TEACH_DATA_SAVE
            // 
            this.BTN_TEACH_DATA_SAVE.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_DATA_SAVE.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_DATA_SAVE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_DATA_SAVE.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_DATA_SAVE.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_DATA_SAVE.Location = new System.Drawing.Point(601, 693);
            this.BTN_TEACH_DATA_SAVE.Name = "BTN_TEACH_DATA_SAVE";
            this.BTN_TEACH_DATA_SAVE.Size = new System.Drawing.Size(135, 48);
            this.BTN_TEACH_DATA_SAVE.TabIndex = 34;
            this.BTN_TEACH_DATA_SAVE.Text = "SAVE";
            this.BTN_TEACH_DATA_SAVE.UseVisualStyleBackColor = false;
            this.BTN_TEACH_DATA_SAVE.Click += new System.EventHandler(this.BTN_TEACH_DATA_SAVE_Click);
            // 
            // label_Resolution
            // 
            this.label_Resolution.BackColor = System.Drawing.Color.Gray;
            this.label_Resolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Resolution.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label_Resolution.Location = new System.Drawing.Point(17, 420);
            this.label_Resolution.Name = "label_Resolution";
            this.label_Resolution.Size = new System.Drawing.Size(117, 23);
            this.label_Resolution.TabIndex = 46;
            this.label_Resolution.Text = "Resolution";
            this.label_Resolution.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.DimGray;
            this.label4.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(17, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 23);
            this.label4.TabIndex = 33;
            this.label4.Text = "MOTOR AXIS";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.DimGray;
            this.label3.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(15, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 23);
            this.label3.TabIndex = 32;
            this.label3.Text = "MOTOR SET";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_TEACH_LIFT_RIGHT_Z
            // 
            this.BTN_TEACH_LIFT_RIGHT_Z.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_LIFT_RIGHT_Z.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_LIFT_RIGHT_Z.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_LIFT_RIGHT_Z.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_LIFT_RIGHT_Z.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_LIFT_RIGHT_Z.Location = new System.Drawing.Point(17, 260);
            this.BTN_TEACH_LIFT_RIGHT_Z.Name = "BTN_TEACH_LIFT_RIGHT_Z";
            this.BTN_TEACH_LIFT_RIGHT_Z.Size = new System.Drawing.Size(117, 42);
            this.BTN_TEACH_LIFT_RIGHT_Z.TabIndex = 23;
            this.BTN_TEACH_LIFT_RIGHT_Z.Text = "RIGHT Z";
            this.BTN_TEACH_LIFT_RIGHT_Z.UseVisualStyleBackColor = false;
            this.BTN_TEACH_LIFT_RIGHT_Z.Click += new System.EventHandler(this.BTN_TEACH_PCB_Y_Click);
            // 
            // BTN_TEACH_SERVO_ON
            // 
            this.BTN_TEACH_SERVO_ON.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_SERVO_ON.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_SERVO_ON.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_SERVO_ON.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_SERVO_ON.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_SERVO_ON.Location = new System.Drawing.Point(15, 37);
            this.BTN_TEACH_SERVO_ON.Name = "BTN_TEACH_SERVO_ON";
            this.BTN_TEACH_SERVO_ON.Size = new System.Drawing.Size(117, 45);
            this.BTN_TEACH_SERVO_ON.TabIndex = 18;
            this.BTN_TEACH_SERVO_ON.Text = "SERVO ON";
            this.BTN_TEACH_SERVO_ON.UseVisualStyleBackColor = false;
            this.BTN_TEACH_SERVO_ON.Click += new System.EventHandler(this.BTN_TEACH_SERVO_ON_Click);
            // 
            // BTN_TEACH_SERVO_OFF
            // 
            this.BTN_TEACH_SERVO_OFF.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_SERVO_OFF.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_SERVO_OFF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_SERVO_OFF.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_SERVO_OFF.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_SERVO_OFF.Location = new System.Drawing.Point(15, 83);
            this.BTN_TEACH_SERVO_OFF.Name = "BTN_TEACH_SERVO_OFF";
            this.BTN_TEACH_SERVO_OFF.Size = new System.Drawing.Size(117, 45);
            this.BTN_TEACH_SERVO_OFF.TabIndex = 19;
            this.BTN_TEACH_SERVO_OFF.Text = "SERVO OFF";
            this.BTN_TEACH_SERVO_OFF.UseVisualStyleBackColor = false;
            this.BTN_TEACH_SERVO_OFF.Click += new System.EventHandler(this.BTN_TEACH_SERVO_OFF_Click);
            // 
            // BTN_TEACH_SERVO_RESET
            // 
            this.BTN_TEACH_SERVO_RESET.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_SERVO_RESET.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_SERVO_RESET.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_SERVO_RESET.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_SERVO_RESET.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_SERVO_RESET.Location = new System.Drawing.Point(15, 129);
            this.BTN_TEACH_SERVO_RESET.Name = "BTN_TEACH_SERVO_RESET";
            this.BTN_TEACH_SERVO_RESET.Size = new System.Drawing.Size(117, 45);
            this.BTN_TEACH_SERVO_RESET.TabIndex = 20;
            this.BTN_TEACH_SERVO_RESET.Text = "SERVO RESET";
            this.BTN_TEACH_SERVO_RESET.UseVisualStyleBackColor = false;
            this.BTN_TEACH_SERVO_RESET.Click += new System.EventHandler(this.BTN_TEACH_SERVO_RESET_Click);
            // 
            // BTN_TEACH_LIFT_FRONT_X
            // 
            this.BTN_TEACH_LIFT_FRONT_X.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_LIFT_FRONT_X.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_LIFT_FRONT_X.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_LIFT_FRONT_X.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_LIFT_FRONT_X.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_LIFT_FRONT_X.Location = new System.Drawing.Point(17, 303);
            this.BTN_TEACH_LIFT_FRONT_X.Name = "BTN_TEACH_LIFT_FRONT_X";
            this.BTN_TEACH_LIFT_FRONT_X.Size = new System.Drawing.Size(117, 42);
            this.BTN_TEACH_LIFT_FRONT_X.TabIndex = 24;
            this.BTN_TEACH_LIFT_FRONT_X.Text = "FRONT X";
            this.BTN_TEACH_LIFT_FRONT_X.UseVisualStyleBackColor = false;
            this.BTN_TEACH_LIFT_FRONT_X.Click += new System.EventHandler(this.BTN_TEACH_PCB_Z_Click);
            // 
            // BTN_TEACH_LIFT_LEFT_Z
            // 
            this.BTN_TEACH_LIFT_LEFT_Z.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_LIFT_LEFT_Z.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_LIFT_LEFT_Z.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_LIFT_LEFT_Z.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_LIFT_LEFT_Z.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_LIFT_LEFT_Z.Location = new System.Drawing.Point(17, 217);
            this.BTN_TEACH_LIFT_LEFT_Z.Name = "BTN_TEACH_LIFT_LEFT_Z";
            this.BTN_TEACH_LIFT_LEFT_Z.Size = new System.Drawing.Size(117, 42);
            this.BTN_TEACH_LIFT_LEFT_Z.TabIndex = 22;
            this.BTN_TEACH_LIFT_LEFT_Z.Text = "LEFT Z";
            this.BTN_TEACH_LIFT_LEFT_Z.UseVisualStyleBackColor = false;
            this.BTN_TEACH_LIFT_LEFT_Z.Click += new System.EventHandler(this.BTN_TEACH_PCB_X_Click);
            // 
            // TeachingLift
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupTeachPcb);
            this.Name = "TeachingLift";
            this.Size = new System.Drawing.Size(770, 780);
            this.groupTeachPcb.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupTeachPcb;
        private System.Windows.Forms.Button BTN_TEACH_LIFT_RIGHT_Z;
        private System.Windows.Forms.Button BTN_TEACH_SERVO_ON;
        private System.Windows.Forms.Button BTN_TEACH_SERVO_OFF;
        private System.Windows.Forms.Button BTN_TEACH_SERVO_RESET;
        private System.Windows.Forms.Button BTN_TEACH_LIFT_FRONT_X;
        private System.Windows.Forms.Button BTN_TEACH_LIFT_LEFT_Z;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BTN_TEACH_DATA_SAVE;
        public System.Windows.Forms.Label LABEL_TEACH_ROSOLUTION_VALUE;
        private System.Windows.Forms.Label label_Resolution;
        private System.Windows.Forms.Button BTN_TEACH_LIFT_BACK_X;
    }
}
