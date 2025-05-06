
namespace ZenHandler.Dlg
{
    partial class Config_Task
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
            this.label_ConfigTask_Title = new System.Windows.Forms.Label();
            this.hopeCheckBox_ImageGrabUse = new ReaLTaiizor.Controls.HopeCheckBox();
            this.hopeCheckBox_PinCountUse = new ReaLTaiizor.Controls.HopeCheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox_BcrGo = new System.Windows.Forms.CheckBox();
            this.checkBox_IdleReportPass = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label_ConfigTask_Title
            // 
            this.label_ConfigTask_Title.BackColor = System.Drawing.Color.Black;
            this.label_ConfigTask_Title.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_ConfigTask_Title.ForeColor = System.Drawing.Color.White;
            this.label_ConfigTask_Title.Location = new System.Drawing.Point(30, 12);
            this.label_ConfigTask_Title.Name = "label_ConfigTask_Title";
            this.label_ConfigTask_Title.Size = new System.Drawing.Size(442, 23);
            this.label_ConfigTask_Title.TabIndex = 64;
            this.label_ConfigTask_Title.Text = "TASK";
            this.label_ConfigTask_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hopeCheckBox_ImageGrabUse
            // 
            this.hopeCheckBox_ImageGrabUse.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_ImageGrabUse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.hopeCheckBox_ImageGrabUse.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.hopeCheckBox_ImageGrabUse.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.hopeCheckBox_ImageGrabUse.Enable = true;
            this.hopeCheckBox_ImageGrabUse.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_ImageGrabUse.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.hopeCheckBox_ImageGrabUse.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.hopeCheckBox_ImageGrabUse.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hopeCheckBox_ImageGrabUse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.hopeCheckBox_ImageGrabUse.Location = new System.Drawing.Point(420, 305);
            this.hopeCheckBox_ImageGrabUse.Name = "hopeCheckBox_ImageGrabUse";
            this.hopeCheckBox_ImageGrabUse.Size = new System.Drawing.Size(159, 20);
            this.hopeCheckBox_ImageGrabUse.TabIndex = 67;
            this.hopeCheckBox_ImageGrabUse.Text = "CCD GRAB USE";
            this.hopeCheckBox_ImageGrabUse.UseVisualStyleBackColor = true;
            // 
            // hopeCheckBox_PinCountUse
            // 
            this.hopeCheckBox_PinCountUse.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_PinCountUse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.hopeCheckBox_PinCountUse.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.hopeCheckBox_PinCountUse.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.hopeCheckBox_PinCountUse.Enable = true;
            this.hopeCheckBox_PinCountUse.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_PinCountUse.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.hopeCheckBox_PinCountUse.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.hopeCheckBox_PinCountUse.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hopeCheckBox_PinCountUse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.hopeCheckBox_PinCountUse.Location = new System.Drawing.Point(420, 269);
            this.hopeCheckBox_PinCountUse.Name = "hopeCheckBox_PinCountUse";
            this.hopeCheckBox_PinCountUse.Size = new System.Drawing.Size(163, 20);
            this.hopeCheckBox_PinCountUse.TabIndex = 66;
            this.hopeCheckBox_PinCountUse.Text = "PIN COUNT USE";
            this.hopeCheckBox_PinCountUse.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(451, 225);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(204, 23);
            this.label3.TabIndex = 65;
            this.label3.Text = "운전 설정";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox_BcrGo
            // 
            this.checkBox_BcrGo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.checkBox_BcrGo.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.checkBox_BcrGo.FlatAppearance.BorderSize = 2;
            this.checkBox_BcrGo.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray;
            this.checkBox_BcrGo.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_BcrGo.Location = new System.Drawing.Point(454, 392);
            this.checkBox_BcrGo.Name = "checkBox_BcrGo";
            this.checkBox_BcrGo.Size = new System.Drawing.Size(194, 21);
            this.checkBox_BcrGo.TabIndex = 69;
            this.checkBox_BcrGo.Text = "Start Automation on Barcode";
            this.checkBox_BcrGo.UseVisualStyleBackColor = false;
            this.checkBox_BcrGo.Visible = false;
            // 
            // checkBox_IdleReportPass
            // 
            this.checkBox_IdleReportPass.BackColor = System.Drawing.Color.WhiteSmoke;
            this.checkBox_IdleReportPass.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.checkBox_IdleReportPass.FlatAppearance.BorderSize = 2;
            this.checkBox_IdleReportPass.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray;
            this.checkBox_IdleReportPass.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_IdleReportPass.Location = new System.Drawing.Point(454, 365);
            this.checkBox_IdleReportPass.Name = "checkBox_IdleReportPass";
            this.checkBox_IdleReportPass.Size = new System.Drawing.Size(194, 21);
            this.checkBox_IdleReportPass.TabIndex = 68;
            this.checkBox_IdleReportPass.Text = "IDLE REASON REPORT PASS";
            this.checkBox_IdleReportPass.UseVisualStyleBackColor = false;
            this.checkBox_IdleReportPass.Visible = false;
            // 
            // Config_Task
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_BcrGo);
            this.Controls.Add(this.checkBox_IdleReportPass);
            this.Controls.Add(this.hopeCheckBox_ImageGrabUse);
            this.Controls.Add(this.hopeCheckBox_PinCountUse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_ConfigTask_Title);
            this.Name = "Config_Task";
            this.Size = new System.Drawing.Size(770, 496);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_ConfigTask_Title;
        private ReaLTaiizor.Controls.HopeCheckBox hopeCheckBox_ImageGrabUse;
        private ReaLTaiizor.Controls.HopeCheckBox hopeCheckBox_PinCountUse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_BcrGo;
        private System.Windows.Forms.CheckBox checkBox_IdleReportPass;
    }
}
