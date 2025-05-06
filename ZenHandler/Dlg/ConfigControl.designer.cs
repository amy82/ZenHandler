namespace ZenHandler.Dlg
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
            this.BTN_CONFIG_SAVE = new System.Windows.Forms.Button();
            this.Btn_Config_Task = new System.Windows.Forms.Button();
            this.Btn_Config_Option = new System.Windows.Forms.Button();
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
            // BTN_CONFIG_SAVE
            // 
            this.BTN_CONFIG_SAVE.BackColor = System.Drawing.Color.Tan;
            this.BTN_CONFIG_SAVE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CONFIG_SAVE.Font = new System.Drawing.Font("나눔고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CONFIG_SAVE.ForeColor = System.Drawing.Color.White;
            this.BTN_CONFIG_SAVE.Location = new System.Drawing.Point(608, 929);
            this.BTN_CONFIG_SAVE.Name = "BTN_CONFIG_SAVE";
            this.BTN_CONFIG_SAVE.Size = new System.Drawing.Size(122, 53);
            this.BTN_CONFIG_SAVE.TabIndex = 28;
            this.BTN_CONFIG_SAVE.Text = "SAVE";
            this.BTN_CONFIG_SAVE.UseVisualStyleBackColor = false;
            this.BTN_CONFIG_SAVE.Click += new System.EventHandler(this.BTN_CONFIG_SAVE_Click);
            // 
            // Btn_Config_Task
            // 
            this.Btn_Config_Task.BackColor = System.Drawing.Color.Tan;
            this.Btn_Config_Task.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Config_Task.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_Config_Task.ForeColor = System.Drawing.Color.White;
            this.Btn_Config_Task.Location = new System.Drawing.Point(563, 18);
            this.Btn_Config_Task.Name = "Btn_Config_Task";
            this.Btn_Config_Task.Size = new System.Drawing.Size(87, 44);
            this.Btn_Config_Task.TabIndex = 63;
            this.Btn_Config_Task.Text = "TASK";
            this.Btn_Config_Task.UseVisualStyleBackColor = false;
            // 
            // Btn_Config_Option
            // 
            this.Btn_Config_Option.BackColor = System.Drawing.Color.Tan;
            this.Btn_Config_Option.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Config_Option.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_Config_Option.ForeColor = System.Drawing.Color.White;
            this.Btn_Config_Option.Location = new System.Drawing.Point(655, 18);
            this.Btn_Config_Option.Name = "Btn_Config_Option";
            this.Btn_Config_Option.Size = new System.Drawing.Size(87, 44);
            this.Btn_Config_Option.TabIndex = 64;
            this.Btn_Config_Option.Text = "OPTION";
            this.Btn_Config_Option.UseVisualStyleBackColor = false;
            // 
            // ConfigControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.Controls.Add(this.Btn_Config_Option);
            this.Controls.Add(this.Btn_Config_Task);
            this.Controls.Add(this.ManualTitleLabel);
            this.Controls.Add(this.BTN_CONFIG_SAVE);
            this.Name = "ConfigControl";
            this.Size = new System.Drawing.Size(770, 1000);
            this.VisibleChanged += new System.EventHandler(this.ConfigControl_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ManualTitleLabel;
        private System.Windows.Forms.Button BTN_CONFIG_SAVE;
        private System.Windows.Forms.Button Btn_Config_Task;
        private System.Windows.Forms.Button Btn_Config_Option;
    }
}
