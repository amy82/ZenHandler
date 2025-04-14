
namespace ZenHandler.Dlg
{
    partial class TeachingTransfer
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
            this.BTN_TEACH_DATA_SAVE = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BTN_TEACH_TRANSFER_Y = new System.Windows.Forms.Button();
            this.BTN_TEACH_SERVO_ON = new System.Windows.Forms.Button();
            this.BTN_TEACH_SERVO_OFF = new System.Windows.Forms.Button();
            this.BTN_TEACH_SERVO_RESET = new System.Windows.Forms.Button();
            this.BTN_TEACH_TRANSFER_Z = new System.Windows.Forms.Button();
            this.BTN_TEACH_TRANSFER_X = new System.Windows.Forms.Button();
            this.groupTeachPcb.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupTeachPcb
            // 
            this.groupTeachPcb.BackColor = System.Drawing.Color.White;
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_DATA_SAVE);
            this.groupTeachPcb.Controls.Add(this.label4);
            this.groupTeachPcb.Controls.Add(this.label3);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_TRANSFER_Y);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_SERVO_ON);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_SERVO_OFF);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_SERVO_RESET);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_TRANSFER_Z);
            this.groupTeachPcb.Controls.Add(this.BTN_TEACH_TRANSFER_X);
            this.groupTeachPcb.Location = new System.Drawing.Point(8, 3);
            this.groupTeachPcb.Name = "groupTeachPcb";
            this.groupTeachPcb.Size = new System.Drawing.Size(756, 694);
            this.groupTeachPcb.TabIndex = 45;
            this.groupTeachPcb.TabStop = false;
            // 
            // BTN_TEACH_DATA_SAVE
            // 
            this.BTN_TEACH_DATA_SAVE.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_DATA_SAVE.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_DATA_SAVE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_DATA_SAVE.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_DATA_SAVE.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_DATA_SAVE.Location = new System.Drawing.Point(619, 616);
            this.BTN_TEACH_DATA_SAVE.Name = "BTN_TEACH_DATA_SAVE";
            this.BTN_TEACH_DATA_SAVE.Size = new System.Drawing.Size(117, 48);
            this.BTN_TEACH_DATA_SAVE.TabIndex = 34;
            this.BTN_TEACH_DATA_SAVE.Text = "SAVE";
            this.BTN_TEACH_DATA_SAVE.UseVisualStyleBackColor = false;
            this.BTN_TEACH_DATA_SAVE.Click += new System.EventHandler(this.BTN_TEACH_DATA_SAVE_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.DimGray;
            this.label4.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(17, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 23);
            this.label4.TabIndex = 33;
            this.label4.Text = "MOTOR AXIS";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.DimGray;
            this.label3.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(15, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 23);
            this.label3.TabIndex = 32;
            this.label3.Text = "MOTOR SET";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_TEACH_TRANSFER_Y
            // 
            this.BTN_TEACH_TRANSFER_Y.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_TRANSFER_Y.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_TRANSFER_Y.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_TRANSFER_Y.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_TEACH_TRANSFER_Y.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_TRANSFER_Y.Location = new System.Drawing.Point(17, 277);
            this.BTN_TEACH_TRANSFER_Y.Name = "BTN_TEACH_TRANSFER_Y";
            this.BTN_TEACH_TRANSFER_Y.Size = new System.Drawing.Size(107, 42);
            this.BTN_TEACH_TRANSFER_Y.TabIndex = 23;
            this.BTN_TEACH_TRANSFER_Y.Text = "Y";
            this.BTN_TEACH_TRANSFER_Y.UseVisualStyleBackColor = false;
            this.BTN_TEACH_TRANSFER_Y.Click += new System.EventHandler(this.BTN_TEACH_PCB_Y_Click);
            // 
            // BTN_TEACH_SERVO_ON
            // 
            this.BTN_TEACH_SERVO_ON.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_SERVO_ON.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_SERVO_ON.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_SERVO_ON.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_SERVO_ON.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_SERVO_ON.Location = new System.Drawing.Point(15, 54);
            this.BTN_TEACH_SERVO_ON.Name = "BTN_TEACH_SERVO_ON";
            this.BTN_TEACH_SERVO_ON.Size = new System.Drawing.Size(107, 45);
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
            this.BTN_TEACH_SERVO_OFF.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_SERVO_OFF.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_SERVO_OFF.Location = new System.Drawing.Point(15, 100);
            this.BTN_TEACH_SERVO_OFF.Name = "BTN_TEACH_SERVO_OFF";
            this.BTN_TEACH_SERVO_OFF.Size = new System.Drawing.Size(107, 45);
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
            this.BTN_TEACH_SERVO_RESET.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_SERVO_RESET.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_SERVO_RESET.Location = new System.Drawing.Point(15, 146);
            this.BTN_TEACH_SERVO_RESET.Name = "BTN_TEACH_SERVO_RESET";
            this.BTN_TEACH_SERVO_RESET.Size = new System.Drawing.Size(107, 45);
            this.BTN_TEACH_SERVO_RESET.TabIndex = 20;
            this.BTN_TEACH_SERVO_RESET.Text = "SERVO RESET";
            this.BTN_TEACH_SERVO_RESET.UseVisualStyleBackColor = false;
            this.BTN_TEACH_SERVO_RESET.Click += new System.EventHandler(this.BTN_TEACH_SERVO_RESET_Click);
            // 
            // BTN_TEACH_TRANSFER_Z
            // 
            this.BTN_TEACH_TRANSFER_Z.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_TRANSFER_Z.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_TRANSFER_Z.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_TRANSFER_Z.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_TEACH_TRANSFER_Z.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_TRANSFER_Z.Location = new System.Drawing.Point(17, 320);
            this.BTN_TEACH_TRANSFER_Z.Name = "BTN_TEACH_TRANSFER_Z";
            this.BTN_TEACH_TRANSFER_Z.Size = new System.Drawing.Size(107, 42);
            this.BTN_TEACH_TRANSFER_Z.TabIndex = 24;
            this.BTN_TEACH_TRANSFER_Z.Text = "Z";
            this.BTN_TEACH_TRANSFER_Z.UseVisualStyleBackColor = false;
            this.BTN_TEACH_TRANSFER_Z.Click += new System.EventHandler(this.BTN_TEACH_PCB_Z_Click);
            // 
            // BTN_TEACH_TRANSFER_X
            // 
            this.BTN_TEACH_TRANSFER_X.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_TRANSFER_X.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_TRANSFER_X.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_TRANSFER_X.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_TEACH_TRANSFER_X.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_TRANSFER_X.Location = new System.Drawing.Point(17, 234);
            this.BTN_TEACH_TRANSFER_X.Name = "BTN_TEACH_TRANSFER_X";
            this.BTN_TEACH_TRANSFER_X.Size = new System.Drawing.Size(107, 42);
            this.BTN_TEACH_TRANSFER_X.TabIndex = 22;
            this.BTN_TEACH_TRANSFER_X.Text = "X";
            this.BTN_TEACH_TRANSFER_X.UseVisualStyleBackColor = false;
            this.BTN_TEACH_TRANSFER_X.Click += new System.EventHandler(this.BTN_TEACH_PCB_X_Click);
            // 
            // TeachingTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupTeachPcb);
            this.Name = "TeachingTransfer";
            this.Size = new System.Drawing.Size(770, 700);
            this.groupTeachPcb.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupTeachPcb;
        private System.Windows.Forms.Button BTN_TEACH_TRANSFER_Y;
        private System.Windows.Forms.Button BTN_TEACH_SERVO_ON;
        private System.Windows.Forms.Button BTN_TEACH_SERVO_OFF;
        private System.Windows.Forms.Button BTN_TEACH_SERVO_RESET;
        private System.Windows.Forms.Button BTN_TEACH_TRANSFER_Z;
        private System.Windows.Forms.Button BTN_TEACH_TRANSFER_X;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BTN_TEACH_DATA_SAVE;
    }
}
