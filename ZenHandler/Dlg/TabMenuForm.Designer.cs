
namespace ZenHandler.Dlg
{
    partial class TabMenuForm
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
            this.DateLabel = new System.Windows.Forms.Label();
            this.BTN_RIGHT_EN_MODE = new System.Windows.Forms.Button();
            this.BTN_RIGHT_OP_MODE = new System.Windows.Forms.Button();
            this.BTN_BOTTOM_LOG = new System.Windows.Forms.Button();
            this.label_build = new System.Windows.Forms.Label();
            this.label_version = new System.Windows.Forms.Label();
            this.BTN_BOTTOM_WALLPAPER = new System.Windows.Forms.Button();
            this.BTN_BOTTOM_EXIT = new System.Windows.Forms.Button();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.BTN_BOTTOM_ALARM = new System.Windows.Forms.Button();
            this.BTN_BOTTOM_LIGHT = new System.Windows.Forms.Button();
            this.BTN_BOTTOM_IO = new System.Windows.Forms.Button();
            this.BTN_BOTTOM_CCD = new System.Windows.Forms.Button();
            this.BTN_BOTTOM_TEACH = new System.Windows.Forms.Button();
            this.BTN_BOTTOM_MAIN = new System.Windows.Forms.Button();
            this.BTN_BOTTOM_SETUP = new System.Windows.Forms.Button();
            this.BTN_BOTTOM_MANUAL = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DateLabel
            // 
            this.DateLabel.BackColor = System.Drawing.Color.Transparent;
            this.DateLabel.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateLabel.ForeColor = System.Drawing.Color.DimGray;
            this.DateLabel.Location = new System.Drawing.Point(-1, 723);
            this.DateLabel.Margin = new System.Windows.Forms.Padding(0);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(131, 25);
            this.DateLabel.TabIndex = 32;
            this.DateLabel.Text = "00 : 00 : 00";
            this.DateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_RIGHT_EN_MODE
            // 
            this.BTN_RIGHT_EN_MODE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_RIGHT_EN_MODE.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_RIGHT_EN_MODE.ForeColor = System.Drawing.Color.Gray;
            this.BTN_RIGHT_EN_MODE.Location = new System.Drawing.Point(1, 504);
            this.BTN_RIGHT_EN_MODE.Name = "BTN_RIGHT_EN_MODE";
            this.BTN_RIGHT_EN_MODE.Size = new System.Drawing.Size(133, 43);
            this.BTN_RIGHT_EN_MODE.TabIndex = 31;
            this.BTN_RIGHT_EN_MODE.Text = "Engineer Mode";
            this.BTN_RIGHT_EN_MODE.UseVisualStyleBackColor = true;
            this.BTN_RIGHT_EN_MODE.Click += new System.EventHandler(this.BTN_RIGHT_EN_MODE_Click_1);
            // 
            // BTN_RIGHT_OP_MODE
            // 
            this.BTN_RIGHT_OP_MODE.BackColor = System.Drawing.Color.Bisque;
            this.BTN_RIGHT_OP_MODE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_RIGHT_OP_MODE.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_RIGHT_OP_MODE.ForeColor = System.Drawing.Color.Black;
            this.BTN_RIGHT_OP_MODE.Location = new System.Drawing.Point(1, 548);
            this.BTN_RIGHT_OP_MODE.Name = "BTN_RIGHT_OP_MODE";
            this.BTN_RIGHT_OP_MODE.Size = new System.Drawing.Size(133, 43);
            this.BTN_RIGHT_OP_MODE.TabIndex = 30;
            this.BTN_RIGHT_OP_MODE.Text = "Operator Mode";
            this.BTN_RIGHT_OP_MODE.UseVisualStyleBackColor = false;
            this.BTN_RIGHT_OP_MODE.Click += new System.EventHandler(this.BTN_RIGHT_OP_MODE_Click_1);
            // 
            // BTN_BOTTOM_LOG
            // 
            this.BTN_BOTTOM_LOG.BackColor = System.Drawing.Color.Transparent;
            this.BTN_BOTTOM_LOG.FlatAppearance.BorderSize = 0;
            this.BTN_BOTTOM_LOG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_BOTTOM_LOG.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_BOTTOM_LOG.ForeColor = System.Drawing.Color.White;
            this.BTN_BOTTOM_LOG.Image = global::ZenHandler.Properties.Resources.log;
            this.BTN_BOTTOM_LOG.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_LOG.Location = new System.Drawing.Point(7, 228);
            this.BTN_BOTTOM_LOG.Name = "BTN_BOTTOM_LOG";
            this.BTN_BOTTOM_LOG.Size = new System.Drawing.Size(133, 36);
            this.BTN_BOTTOM_LOG.TabIndex = 29;
            this.BTN_BOTTOM_LOG.Text = "  LOG";
            this.BTN_BOTTOM_LOG.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTN_BOTTOM_LOG.UseVisualStyleBackColor = false;
            this.BTN_BOTTOM_LOG.Click += new System.EventHandler(this.BTN_BOTTOM_LOG_Click_1);
            // 
            // label_build
            // 
            this.label_build.BackColor = System.Drawing.Color.Transparent;
            this.label_build.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_build.ForeColor = System.Drawing.Color.Gray;
            this.label_build.Location = new System.Drawing.Point(0, 649);
            this.label_build.Name = "label_build";
            this.label_build.Size = new System.Drawing.Size(144, 42);
            this.label_build.TabIndex = 28;
            this.label_build.Text = "buildInfo : 25.02.11.01";
            this.label_build.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_version
            // 
            this.label_version.BackColor = System.Drawing.Color.Transparent;
            this.label_version.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_version.ForeColor = System.Drawing.Color.Gray;
            this.label_version.Location = new System.Drawing.Point(0, 606);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(144, 42);
            this.label_version.TabIndex = 27;
            this.label_version.Text = "VersionInfo : V1.1.0";
            this.label_version.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BTN_BOTTOM_WALLPAPER
            // 
            this.BTN_BOTTOM_WALLPAPER.BackColor = System.Drawing.Color.Transparent;
            this.BTN_BOTTOM_WALLPAPER.FlatAppearance.BorderSize = 0;
            this.BTN_BOTTOM_WALLPAPER.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_BOTTOM_WALLPAPER.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_BOTTOM_WALLPAPER.ForeColor = System.Drawing.Color.White;
            this.BTN_BOTTOM_WALLPAPER.Image = global::ZenHandler.Properties.Resources.Desktop;
            this.BTN_BOTTOM_WALLPAPER.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_WALLPAPER.Location = new System.Drawing.Point(13, 294);
            this.BTN_BOTTOM_WALLPAPER.Name = "BTN_BOTTOM_WALLPAPER";
            this.BTN_BOTTOM_WALLPAPER.Size = new System.Drawing.Size(133, 36);
            this.BTN_BOTTOM_WALLPAPER.TabIndex = 26;
            this.BTN_BOTTOM_WALLPAPER.Text = "  DESKTOP";
            this.BTN_BOTTOM_WALLPAPER.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTN_BOTTOM_WALLPAPER.UseVisualStyleBackColor = false;
            this.BTN_BOTTOM_WALLPAPER.Click += new System.EventHandler(this.BTN_BOTTOM_WALLPAPER_Click_1);
            // 
            // BTN_BOTTOM_EXIT
            // 
            this.BTN_BOTTOM_EXIT.BackColor = System.Drawing.Color.Transparent;
            this.BTN_BOTTOM_EXIT.FlatAppearance.BorderSize = 0;
            this.BTN_BOTTOM_EXIT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_BOTTOM_EXIT.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_BOTTOM_EXIT.ForeColor = System.Drawing.Color.White;
            this.BTN_BOTTOM_EXIT.Image = global::ZenHandler.Properties.Resources.Exit;
            this.BTN_BOTTOM_EXIT.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_EXIT.Location = new System.Drawing.Point(7, 336);
            this.BTN_BOTTOM_EXIT.Name = "BTN_BOTTOM_EXIT";
            this.BTN_BOTTOM_EXIT.Size = new System.Drawing.Size(133, 36);
            this.BTN_BOTTOM_EXIT.TabIndex = 24;
            this.BTN_BOTTOM_EXIT.Text = "  EXIT";
            this.BTN_BOTTOM_EXIT.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTN_BOTTOM_EXIT.UseVisualStyleBackColor = false;
            this.BTN_BOTTOM_EXIT.Click += new System.EventHandler(this.BTN_BOTTOM_EXIT_Click);
            // 
            // TimeLabel
            // 
            this.TimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.TimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TimeLabel.ForeColor = System.Drawing.SystemColors.Info;
            this.TimeLabel.Location = new System.Drawing.Point(-1, 748);
            this.TimeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(131, 25);
            this.TimeLabel.TabIndex = 25;
            this.TimeLabel.Text = "00 : 00 : 00";
            this.TimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_BOTTOM_ALARM
            // 
            this.BTN_BOTTOM_ALARM.BackColor = System.Drawing.Color.Transparent;
            this.BTN_BOTTOM_ALARM.FlatAppearance.BorderSize = 0;
            this.BTN_BOTTOM_ALARM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_BOTTOM_ALARM.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_BOTTOM_ALARM.ForeColor = System.Drawing.Color.White;
            this.BTN_BOTTOM_ALARM.Image = global::ZenHandler.Properties.Resources.Alarm;
            this.BTN_BOTTOM_ALARM.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_ALARM.Location = new System.Drawing.Point(6, 186);
            this.BTN_BOTTOM_ALARM.Name = "BTN_BOTTOM_ALARM";
            this.BTN_BOTTOM_ALARM.Size = new System.Drawing.Size(133, 36);
            this.BTN_BOTTOM_ALARM.TabIndex = 23;
            this.BTN_BOTTOM_ALARM.Text = "  ALARM";
            this.BTN_BOTTOM_ALARM.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTN_BOTTOM_ALARM.UseVisualStyleBackColor = false;
            this.BTN_BOTTOM_ALARM.Click += new System.EventHandler(this.BTN_BOTTOM_ALARM_Click_1);
            // 
            // BTN_BOTTOM_LIGHT
            // 
            this.BTN_BOTTOM_LIGHT.BackColor = System.Drawing.Color.Transparent;
            this.BTN_BOTTOM_LIGHT.FlatAppearance.BorderSize = 0;
            this.BTN_BOTTOM_LIGHT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_BOTTOM_LIGHT.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_BOTTOM_LIGHT.ForeColor = System.Drawing.Color.White;
            this.BTN_BOTTOM_LIGHT.Image = global::ZenHandler.Properties.Resources.Light;
            this.BTN_BOTTOM_LIGHT.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_LIGHT.Location = new System.Drawing.Point(13, 378);
            this.BTN_BOTTOM_LIGHT.Name = "BTN_BOTTOM_LIGHT";
            this.BTN_BOTTOM_LIGHT.Size = new System.Drawing.Size(133, 36);
            this.BTN_BOTTOM_LIGHT.TabIndex = 22;
            this.BTN_BOTTOM_LIGHT.Text = "  LIGHT";
            this.BTN_BOTTOM_LIGHT.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTN_BOTTOM_LIGHT.UseVisualStyleBackColor = false;
            // 
            // BTN_BOTTOM_IO
            // 
            this.BTN_BOTTOM_IO.BackColor = System.Drawing.Color.Transparent;
            this.BTN_BOTTOM_IO.FlatAppearance.BorderSize = 0;
            this.BTN_BOTTOM_IO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_BOTTOM_IO.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_BOTTOM_IO.ForeColor = System.Drawing.Color.Gainsboro;
            this.BTN_BOTTOM_IO.Image = global::ZenHandler.Properties.Resources.Io;
            this.BTN_BOTTOM_IO.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_IO.Location = new System.Drawing.Point(7, 113);
            this.BTN_BOTTOM_IO.Name = "BTN_BOTTOM_IO";
            this.BTN_BOTTOM_IO.Size = new System.Drawing.Size(133, 36);
            this.BTN_BOTTOM_IO.TabIndex = 21;
            this.BTN_BOTTOM_IO.Text = "  IO";
            this.BTN_BOTTOM_IO.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTN_BOTTOM_IO.UseVisualStyleBackColor = false;
            this.BTN_BOTTOM_IO.Click += new System.EventHandler(this.BTN_BOTTOM_IO_Click_1);
            // 
            // BTN_BOTTOM_CCD
            // 
            this.BTN_BOTTOM_CCD.BackColor = System.Drawing.Color.Transparent;
            this.BTN_BOTTOM_CCD.FlatAppearance.BorderSize = 0;
            this.BTN_BOTTOM_CCD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_BOTTOM_CCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_BOTTOM_CCD.ForeColor = System.Drawing.Color.White;
            this.BTN_BOTTOM_CCD.Image = global::ZenHandler.Properties.Resources.Ccd;
            this.BTN_BOTTOM_CCD.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_CCD.Location = new System.Drawing.Point(17, 438);
            this.BTN_BOTTOM_CCD.Name = "BTN_BOTTOM_CCD";
            this.BTN_BOTTOM_CCD.Size = new System.Drawing.Size(133, 36);
            this.BTN_BOTTOM_CCD.TabIndex = 20;
            this.BTN_BOTTOM_CCD.Text = "  CCD";
            this.BTN_BOTTOM_CCD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTN_BOTTOM_CCD.UseVisualStyleBackColor = false;
            this.BTN_BOTTOM_CCD.Click += new System.EventHandler(this.BTN_BOTTOM_CCD_Click_1);
            // 
            // BTN_BOTTOM_TEACH
            // 
            this.BTN_BOTTOM_TEACH.BackColor = System.Drawing.Color.Transparent;
            this.BTN_BOTTOM_TEACH.FlatAppearance.BorderSize = 0;
            this.BTN_BOTTOM_TEACH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_BOTTOM_TEACH.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_BOTTOM_TEACH.ForeColor = System.Drawing.Color.White;
            this.BTN_BOTTOM_TEACH.Image = global::ZenHandler.Properties.Resources.Teaching1;
            this.BTN_BOTTOM_TEACH.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_TEACH.Location = new System.Drawing.Point(3, 45);
            this.BTN_BOTTOM_TEACH.Name = "BTN_BOTTOM_TEACH";
            this.BTN_BOTTOM_TEACH.Size = new System.Drawing.Size(133, 36);
            this.BTN_BOTTOM_TEACH.TabIndex = 19;
            this.BTN_BOTTOM_TEACH.Text = "  TEACHING";
            this.BTN_BOTTOM_TEACH.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTN_BOTTOM_TEACH.UseVisualStyleBackColor = false;
            this.BTN_BOTTOM_TEACH.Click += new System.EventHandler(this.BTN_BOTTOM_TEACH_Click_1);
            // 
            // BTN_BOTTOM_MAIN
            // 
            this.BTN_BOTTOM_MAIN.BackColor = System.Drawing.Color.Transparent;
            this.BTN_BOTTOM_MAIN.FlatAppearance.BorderSize = 0;
            this.BTN_BOTTOM_MAIN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_BOTTOM_MAIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_BOTTOM_MAIN.ForeColor = System.Drawing.Color.White;
            this.BTN_BOTTOM_MAIN.Image = global::ZenHandler.Properties.Resources.Manual;
            this.BTN_BOTTOM_MAIN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_MAIN.Location = new System.Drawing.Point(3, 3);
            this.BTN_BOTTOM_MAIN.Name = "BTN_BOTTOM_MAIN";
            this.BTN_BOTTOM_MAIN.Size = new System.Drawing.Size(133, 36);
            this.BTN_BOTTOM_MAIN.TabIndex = 18;
            this.BTN_BOTTOM_MAIN.Text = "  MAIN";
            this.BTN_BOTTOM_MAIN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_MAIN.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTN_BOTTOM_MAIN.UseVisualStyleBackColor = false;
            this.BTN_BOTTOM_MAIN.Click += new System.EventHandler(this.BTN_BOTTOM_MAIN_Click_1);
            // 
            // BTN_BOTTOM_SETUP
            // 
            this.BTN_BOTTOM_SETUP.BackColor = System.Drawing.Color.Transparent;
            this.BTN_BOTTOM_SETUP.FlatAppearance.BorderSize = 0;
            this.BTN_BOTTOM_SETUP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_BOTTOM_SETUP.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_BOTTOM_SETUP.ForeColor = System.Drawing.Color.White;
            this.BTN_BOTTOM_SETUP.Image = global::ZenHandler.Properties.Resources.Config;
            this.BTN_BOTTOM_SETUP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_SETUP.Location = new System.Drawing.Point(4, 144);
            this.BTN_BOTTOM_SETUP.Name = "BTN_BOTTOM_SETUP";
            this.BTN_BOTTOM_SETUP.Size = new System.Drawing.Size(133, 36);
            this.BTN_BOTTOM_SETUP.TabIndex = 17;
            this.BTN_BOTTOM_SETUP.Text = "  CONFIG";
            this.BTN_BOTTOM_SETUP.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTN_BOTTOM_SETUP.UseVisualStyleBackColor = false;
            this.BTN_BOTTOM_SETUP.Click += new System.EventHandler(this.BTN_BOTTOM_SETUP_Click_1);
            // 
            // BTN_BOTTOM_MANUAL
            // 
            this.BTN_BOTTOM_MANUAL.BackColor = System.Drawing.Color.Transparent;
            this.BTN_BOTTOM_MANUAL.FlatAppearance.BorderSize = 0;
            this.BTN_BOTTOM_MANUAL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_BOTTOM_MANUAL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_BOTTOM_MANUAL.ForeColor = System.Drawing.Color.White;
            this.BTN_BOTTOM_MANUAL.Image = global::ZenHandler.Properties.Resources.Teaching1;
            this.BTN_BOTTOM_MANUAL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_BOTTOM_MANUAL.Location = new System.Drawing.Point(4, 87);
            this.BTN_BOTTOM_MANUAL.Name = "BTN_BOTTOM_MANUAL";
            this.BTN_BOTTOM_MANUAL.Size = new System.Drawing.Size(133, 36);
            this.BTN_BOTTOM_MANUAL.TabIndex = 33;
            this.BTN_BOTTOM_MANUAL.Text = "  MANUAL";
            this.BTN_BOTTOM_MANUAL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTN_BOTTOM_MANUAL.UseVisualStyleBackColor = false;
            this.BTN_BOTTOM_MANUAL.Click += new System.EventHandler(this.BTN_BOTTOM_MANUAL_Click);
            // 
            // TabMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.BTN_BOTTOM_MANUAL);
            this.Controls.Add(this.DateLabel);
            this.Controls.Add(this.BTN_RIGHT_EN_MODE);
            this.Controls.Add(this.BTN_RIGHT_OP_MODE);
            this.Controls.Add(this.BTN_BOTTOM_LOG);
            this.Controls.Add(this.label_build);
            this.Controls.Add(this.label_version);
            this.Controls.Add(this.BTN_BOTTOM_WALLPAPER);
            this.Controls.Add(this.BTN_BOTTOM_EXIT);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.BTN_BOTTOM_ALARM);
            this.Controls.Add(this.BTN_BOTTOM_LIGHT);
            this.Controls.Add(this.BTN_BOTTOM_IO);
            this.Controls.Add(this.BTN_BOTTOM_CCD);
            this.Controls.Add(this.BTN_BOTTOM_TEACH);
            this.Controls.Add(this.BTN_BOTTOM_MAIN);
            this.Controls.Add(this.BTN_BOTTOM_SETUP);
            this.Name = "TabMenuForm";
            this.Size = new System.Drawing.Size(150, 800);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Label DateLabel;
        private System.Windows.Forms.Button BTN_RIGHT_EN_MODE;
        private System.Windows.Forms.Button BTN_RIGHT_OP_MODE;
        private System.Windows.Forms.Button BTN_BOTTOM_LOG;
        private System.Windows.Forms.Label label_build;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.Button BTN_BOTTOM_WALLPAPER;
        private System.Windows.Forms.Button BTN_BOTTOM_EXIT;
        public System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Button BTN_BOTTOM_ALARM;
        private System.Windows.Forms.Button BTN_BOTTOM_LIGHT;
        private System.Windows.Forms.Button BTN_BOTTOM_IO;
        private System.Windows.Forms.Button BTN_BOTTOM_CCD;
        private System.Windows.Forms.Button BTN_BOTTOM_TEACH;
        private System.Windows.Forms.Button BTN_BOTTOM_MAIN;
        private System.Windows.Forms.Button BTN_BOTTOM_SETUP;
        private System.Windows.Forms.Button BTN_BOTTOM_MANUAL;
    }
}
