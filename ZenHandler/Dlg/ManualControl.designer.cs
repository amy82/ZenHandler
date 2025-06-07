namespace ZenHandler.Dlg
{
    partial class ManualControl
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
            this.TeachingTitleLabel = new System.Windows.Forms.Label();
            this.button_Manual_Magazine = new System.Windows.Forms.Button();
            this.button_Manual_Transfer = new System.Windows.Forms.Button();
            this.button_Manual_Lift = new System.Windows.Forms.Button();
            this.button_Manual_Socket = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TeachingTitleLabel
            // 
            this.TeachingTitleLabel.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TeachingTitleLabel.Location = new System.Drawing.Point(16, 16);
            this.TeachingTitleLabel.Name = "TeachingTitleLabel";
            this.TeachingTitleLabel.Size = new System.Drawing.Size(250, 42);
            this.TeachingTitleLabel.TabIndex = 31;
            this.TeachingTitleLabel.Text = "| MANUAL";
            this.TeachingTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_Manual_Magazine
            // 
            this.button_Manual_Magazine.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Magazine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Magazine.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Magazine.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Magazine.Location = new System.Drawing.Point(241, 20);
            this.button_Manual_Magazine.Name = "button_Manual_Magazine";
            this.button_Manual_Magazine.Size = new System.Drawing.Size(87, 44);
            this.button_Manual_Magazine.TabIndex = 34;
            this.button_Manual_Magazine.Text = "MAGAZINE";
            this.button_Manual_Magazine.UseVisualStyleBackColor = false;
            this.button_Manual_Magazine.Click += new System.EventHandler(this.BTN_TEACH_LENS_Click);
            // 
            // button_Manual_Transfer
            // 
            this.button_Manual_Transfer.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Transfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Transfer.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Transfer.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Transfer.Location = new System.Drawing.Point(492, 14);
            this.button_Manual_Transfer.Name = "button_Manual_Transfer";
            this.button_Manual_Transfer.Size = new System.Drawing.Size(87, 44);
            this.button_Manual_Transfer.TabIndex = 33;
            this.button_Manual_Transfer.Text = "TRANSFER";
            this.button_Manual_Transfer.UseVisualStyleBackColor = false;
            this.button_Manual_Transfer.Click += new System.EventHandler(this.BTN_TEACH_PCB_Click);
            // 
            // button_Manual_Lift
            // 
            this.button_Manual_Lift.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Lift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Lift.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Lift.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Lift.Location = new System.Drawing.Point(581, 14);
            this.button_Manual_Lift.Name = "button_Manual_Lift";
            this.button_Manual_Lift.Size = new System.Drawing.Size(87, 44);
            this.button_Manual_Lift.TabIndex = 35;
            this.button_Manual_Lift.Text = "LIFT OR M";
            this.button_Manual_Lift.UseVisualStyleBackColor = false;
            this.button_Manual_Lift.Click += new System.EventHandler(this.button_Manual_Lift_Click);
            // 
            // button_Manual_Socket
            // 
            this.button_Manual_Socket.BackColor = System.Drawing.Color.Tan;
            this.button_Manual_Socket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manual_Socket.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Manual_Socket.ForeColor = System.Drawing.Color.White;
            this.button_Manual_Socket.Location = new System.Drawing.Point(670, 14);
            this.button_Manual_Socket.Name = "button_Manual_Socket";
            this.button_Manual_Socket.Size = new System.Drawing.Size(87, 44);
            this.button_Manual_Socket.TabIndex = 36;
            this.button_Manual_Socket.Text = "SOCKET";
            this.button_Manual_Socket.UseVisualStyleBackColor = false;
            this.button_Manual_Socket.Click += new System.EventHandler(this.button_Manual_Socket_Click);
            // 
            // ManualControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.Controls.Add(this.button_Manual_Socket);
            this.Controls.Add(this.button_Manual_Lift);
            this.Controls.Add(this.button_Manual_Magazine);
            this.Controls.Add(this.button_Manual_Transfer);
            this.Controls.Add(this.TeachingTitleLabel);
            this.Name = "ManualControl";
            this.Size = new System.Drawing.Size(770, 1018);
            this.VisibleChanged += new System.EventHandler(this.ManualControl_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label TeachingTitleLabel;
        private System.Windows.Forms.Button button_Manual_Magazine;
        private System.Windows.Forms.Button button_Manual_Transfer;
        private System.Windows.Forms.Button button_Manual_Lift;
        private System.Windows.Forms.Button button_Manual_Socket;
    }
}
