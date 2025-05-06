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
            this.Btn_Config_Task = new System.Windows.Forms.Button();
            this.Btn_Config_Option = new System.Windows.Forms.Button();
            this.ConfigTitleLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            this.Btn_Config_Task.Click += new System.EventHandler(this.Btn_Config_Task_Click);
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
            this.Btn_Config_Option.Click += new System.EventHandler(this.Btn_Config_Option_Click);
            // 
            // ConfigTitleLabel
            // 
            this.ConfigTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ConfigTitleLabel.Location = new System.Drawing.Point(16, 16);
            this.ConfigTitleLabel.Name = "ConfigTitleLabel";
            this.ConfigTitleLabel.Size = new System.Drawing.Size(286, 42);
            this.ConfigTitleLabel.TabIndex = 65;
            this.ConfigTitleLabel.Text = "| CONFIG - TASK";
            this.ConfigTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConfigControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.Controls.Add(this.ConfigTitleLabel);
            this.Controls.Add(this.Btn_Config_Option);
            this.Controls.Add(this.Btn_Config_Task);
            this.Name = "ConfigControl";
            this.Size = new System.Drawing.Size(770, 1000);
            this.VisibleChanged += new System.EventHandler(this.ConfigControl_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Btn_Config_Task;
        private System.Windows.Forms.Button Btn_Config_Option;
        private System.Windows.Forms.Label ConfigTitleLabel;
    }
}
