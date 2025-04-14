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
            this.BTN_TEACH_LENS = new System.Windows.Forms.Button();
            this.BTN_TEACH_PCB = new System.Windows.Forms.Button();
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
            // BTN_TEACH_LENS
            // 
            this.BTN_TEACH_LENS.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_LENS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_LENS.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_LENS.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_LENS.Location = new System.Drawing.Point(677, 14);
            this.BTN_TEACH_LENS.Name = "BTN_TEACH_LENS";
            this.BTN_TEACH_LENS.Size = new System.Drawing.Size(87, 44);
            this.BTN_TEACH_LENS.TabIndex = 34;
            this.BTN_TEACH_LENS.Text = "LENS";
            this.BTN_TEACH_LENS.UseVisualStyleBackColor = false;
            this.BTN_TEACH_LENS.Click += new System.EventHandler(this.BTN_TEACH_LENS_Click);
            // 
            // BTN_TEACH_PCB
            // 
            this.BTN_TEACH_PCB.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_PCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_PCB.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TEACH_PCB.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_PCB.Location = new System.Drawing.Point(584, 14);
            this.BTN_TEACH_PCB.Name = "BTN_TEACH_PCB";
            this.BTN_TEACH_PCB.Size = new System.Drawing.Size(87, 44);
            this.BTN_TEACH_PCB.TabIndex = 33;
            this.BTN_TEACH_PCB.Text = "PCB";
            this.BTN_TEACH_PCB.UseVisualStyleBackColor = false;
            this.BTN_TEACH_PCB.Click += new System.EventHandler(this.BTN_TEACH_PCB_Click);
            // 
            // ManualControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.Controls.Add(this.BTN_TEACH_LENS);
            this.Controls.Add(this.BTN_TEACH_PCB);
            this.Controls.Add(this.TeachingTitleLabel);
            this.Name = "ManualControl";
            this.Size = new System.Drawing.Size(770, 1018);
            this.VisibleChanged += new System.EventHandler(this.ManualControl_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label TeachingTitleLabel;
        private System.Windows.Forms.Button BTN_TEACH_LENS;
        private System.Windows.Forms.Button BTN_TEACH_PCB;
    }
}
