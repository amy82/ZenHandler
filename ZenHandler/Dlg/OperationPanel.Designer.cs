
namespace ZenHandler.Dlg
{
    partial class OperationPanel
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
            this.BTN_MAIN_STOP1 = new System.Windows.Forms.Button();
            this.BTN_MAIN_PAUSE1 = new System.Windows.Forms.Button();
            this.BTN_MAIN_READY1 = new System.Windows.Forms.Button();
            this.BTN_MAIN_ORIGIN1 = new System.Windows.Forms.Button();
            this.ManualTitleLabel = new System.Windows.Forms.Label();
            this.BTN_MAIN_START1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BTN_MAIN_STOP1
            // 
            this.BTN_MAIN_STOP1.BackColor = System.Drawing.Color.Tan;
            this.BTN_MAIN_STOP1.FlatAppearance.BorderSize = 0;
            this.BTN_MAIN_STOP1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MAIN_STOP1.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MAIN_STOP1.ForeColor = System.Drawing.Color.White;
            this.BTN_MAIN_STOP1.Location = new System.Drawing.Point(1, 209);
            this.BTN_MAIN_STOP1.Name = "BTN_MAIN_STOP1";
            this.BTN_MAIN_STOP1.Size = new System.Drawing.Size(170, 67);
            this.BTN_MAIN_STOP1.TabIndex = 9;
            this.BTN_MAIN_STOP1.Text = "STOP";
            this.BTN_MAIN_STOP1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BTN_MAIN_STOP1.UseVisualStyleBackColor = false;
            this.BTN_MAIN_STOP1.Click += new System.EventHandler(this.BTN_MAIN_STOP1_Click);
            // 
            // BTN_MAIN_PAUSE1
            // 
            this.BTN_MAIN_PAUSE1.BackColor = System.Drawing.Color.Red;
            this.BTN_MAIN_PAUSE1.FlatAppearance.BorderSize = 0;
            this.BTN_MAIN_PAUSE1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MAIN_PAUSE1.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MAIN_PAUSE1.ForeColor = System.Drawing.Color.White;
            this.BTN_MAIN_PAUSE1.Location = new System.Drawing.Point(1, 146);
            this.BTN_MAIN_PAUSE1.Name = "BTN_MAIN_PAUSE1";
            this.BTN_MAIN_PAUSE1.Size = new System.Drawing.Size(170, 61);
            this.BTN_MAIN_PAUSE1.TabIndex = 8;
            this.BTN_MAIN_PAUSE1.Text = "PAUSE";
            this.BTN_MAIN_PAUSE1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BTN_MAIN_PAUSE1.UseVisualStyleBackColor = false;
            this.BTN_MAIN_PAUSE1.Click += new System.EventHandler(this.BTN_MAIN_PAUSE1_Click);
            // 
            // BTN_MAIN_READY1
            // 
            this.BTN_MAIN_READY1.BackColor = System.Drawing.Color.Tan;
            this.BTN_MAIN_READY1.FlatAppearance.BorderSize = 0;
            this.BTN_MAIN_READY1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MAIN_READY1.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MAIN_READY1.ForeColor = System.Drawing.Color.White;
            this.BTN_MAIN_READY1.Location = new System.Drawing.Point(1, 93);
            this.BTN_MAIN_READY1.Name = "BTN_MAIN_READY1";
            this.BTN_MAIN_READY1.Size = new System.Drawing.Size(170, 51);
            this.BTN_MAIN_READY1.TabIndex = 7;
            this.BTN_MAIN_READY1.Text = "READY";
            this.BTN_MAIN_READY1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BTN_MAIN_READY1.UseVisualStyleBackColor = false;
            this.BTN_MAIN_READY1.Click += new System.EventHandler(this.BTN_MAIN_READY1_Click);
            // 
            // BTN_MAIN_ORIGIN1
            // 
            this.BTN_MAIN_ORIGIN1.BackColor = System.Drawing.Color.Orange;
            this.BTN_MAIN_ORIGIN1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BTN_MAIN_ORIGIN1.FlatAppearance.BorderSize = 0;
            this.BTN_MAIN_ORIGIN1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MAIN_ORIGIN1.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MAIN_ORIGIN1.ForeColor = System.Drawing.Color.White;
            this.BTN_MAIN_ORIGIN1.Location = new System.Drawing.Point(1, 43);
            this.BTN_MAIN_ORIGIN1.Name = "BTN_MAIN_ORIGIN1";
            this.BTN_MAIN_ORIGIN1.Size = new System.Drawing.Size(170, 48);
            this.BTN_MAIN_ORIGIN1.TabIndex = 6;
            this.BTN_MAIN_ORIGIN1.Text = "ORIGIN";
            this.BTN_MAIN_ORIGIN1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BTN_MAIN_ORIGIN1.UseVisualStyleBackColor = false;
            this.BTN_MAIN_ORIGIN1.Click += new System.EventHandler(this.BTN_MAIN_ORIGIN1_Click);
            // 
            // ManualTitleLabel
            // 
            this.ManualTitleLabel.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ManualTitleLabel.ForeColor = System.Drawing.Color.White;
            this.ManualTitleLabel.Location = new System.Drawing.Point(1, 2);
            this.ManualTitleLabel.Name = "ManualTitleLabel";
            this.ManualTitleLabel.Size = new System.Drawing.Size(170, 37);
            this.ManualTitleLabel.TabIndex = 11;
            this.ManualTitleLabel.Text = "| OPERATION";
            this.ManualTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_MAIN_START1
            // 
            this.BTN_MAIN_START1.BackColor = System.Drawing.Color.Tan;
            this.BTN_MAIN_START1.FlatAppearance.BorderSize = 0;
            this.BTN_MAIN_START1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MAIN_START1.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MAIN_START1.ForeColor = System.Drawing.Color.White;
            this.BTN_MAIN_START1.Location = new System.Drawing.Point(1, 278);
            this.BTN_MAIN_START1.Name = "BTN_MAIN_START1";
            this.BTN_MAIN_START1.Size = new System.Drawing.Size(170, 69);
            this.BTN_MAIN_START1.TabIndex = 10;
            this.BTN_MAIN_START1.Text = "START";
            this.BTN_MAIN_START1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BTN_MAIN_START1.UseVisualStyleBackColor = false;
            this.BTN_MAIN_START1.Click += new System.EventHandler(this.BTN_MAIN_START1_Click);
            // 
            // OperationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.ManualTitleLabel);
            this.Controls.Add(this.BTN_MAIN_START1);
            this.Controls.Add(this.BTN_MAIN_STOP1);
            this.Controls.Add(this.BTN_MAIN_PAUSE1);
            this.Controls.Add(this.BTN_MAIN_READY1);
            this.Controls.Add(this.BTN_MAIN_ORIGIN1);
            this.Name = "OperationPanel";
            this.Size = new System.Drawing.Size(172, 350);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button BTN_MAIN_START1;
        public System.Windows.Forms.Button BTN_MAIN_STOP1;
        private System.Windows.Forms.Button BTN_MAIN_PAUSE1;
        public System.Windows.Forms.Button BTN_MAIN_READY1;
        private System.Windows.Forms.Button BTN_MAIN_ORIGIN1;
        private System.Windows.Forms.Label ManualTitleLabel;
    }
}
