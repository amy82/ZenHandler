
namespace ZenHandler
{
    partial class MainForm
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TopPanel = new System.Windows.Forms.Panel();
            this.BTN_TOP_LOG = new System.Windows.Forms.Button();
            this.BTN_TOP_CCD = new System.Windows.Forms.Button();
            this.BTN_TOP_CLIENT = new System.Windows.Forms.Button();
            this.BTN_TOP_MES = new System.Windows.Forms.Button();
            this.MainTitlepictureBox = new System.Windows.Forms.PictureBox();
            this.MainTitleLabel = new System.Windows.Forms.Label();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.CenterPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainTitlepictureBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.SetColumnSpan(this.TopPanel, 3);
            this.TopPanel.Controls.Add(this.BTN_TOP_LOG);
            this.TopPanel.Controls.Add(this.BTN_TOP_CCD);
            this.TopPanel.Controls.Add(this.BTN_TOP_CLIENT);
            this.TopPanel.Controls.Add(this.BTN_TOP_MES);
            this.TopPanel.Controls.Add(this.MainTitlepictureBox);
            this.TopPanel.Controls.Add(this.MainTitleLabel);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(1481, 60);
            this.TopPanel.TabIndex = 0;
            // 
            // BTN_TOP_LOG
            // 
            this.BTN_TOP_LOG.BackColor = System.Drawing.Color.LightPink;
            this.BTN_TOP_LOG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TOP_LOG.Font = new System.Drawing.Font("Nirmala UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_TOP_LOG.ForeColor = System.Drawing.Color.White;
            this.BTN_TOP_LOG.Image = global::ZenHandler.Properties.Resources.RightTop_logo;
            this.BTN_TOP_LOG.Location = new System.Drawing.Point(300, 4);
            this.BTN_TOP_LOG.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_TOP_LOG.Name = "BTN_TOP_LOG";
            this.BTN_TOP_LOG.Size = new System.Drawing.Size(150, 56);
            this.BTN_TOP_LOG.TabIndex = 14;
            this.BTN_TOP_LOG.UseVisualStyleBackColor = false;
            // 
            // BTN_TOP_CCD
            // 
            this.BTN_TOP_CCD.BackColor = System.Drawing.Color.Transparent;
            this.BTN_TOP_CCD.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.BTN_TOP_CCD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TOP_CCD.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TOP_CCD.Location = new System.Drawing.Point(1094, 28);
            this.BTN_TOP_CCD.Name = "BTN_TOP_CCD";
            this.BTN_TOP_CCD.Size = new System.Drawing.Size(75, 29);
            this.BTN_TOP_CCD.TabIndex = 13;
            this.BTN_TOP_CCD.Text = "CCD";
            this.BTN_TOP_CCD.UseVisualStyleBackColor = false;
            this.BTN_TOP_CCD.Visible = false;
            this.BTN_TOP_CCD.Click += new System.EventHandler(this.BTN_TOP_CCD_Click);
            // 
            // BTN_TOP_CLIENT
            // 
            this.BTN_TOP_CLIENT.BackColor = System.Drawing.Color.Transparent;
            this.BTN_TOP_CLIENT.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.BTN_TOP_CLIENT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TOP_CLIENT.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TOP_CLIENT.Location = new System.Drawing.Point(700, 31);
            this.BTN_TOP_CLIENT.Name = "BTN_TOP_CLIENT";
            this.BTN_TOP_CLIENT.Size = new System.Drawing.Size(101, 27);
            this.BTN_TOP_CLIENT.TabIndex = 12;
            this.BTN_TOP_CLIENT.Text = "CLIENT";
            this.BTN_TOP_CLIENT.UseVisualStyleBackColor = false;
            // 
            // BTN_TOP_MES
            // 
            this.BTN_TOP_MES.BackColor = System.Drawing.Color.Transparent;
            this.BTN_TOP_MES.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.BTN_TOP_MES.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TOP_MES.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_TOP_MES.Location = new System.Drawing.Point(700, 2);
            this.BTN_TOP_MES.Name = "BTN_TOP_MES";
            this.BTN_TOP_MES.Size = new System.Drawing.Size(101, 27);
            this.BTN_TOP_MES.TabIndex = 11;
            this.BTN_TOP_MES.Text = "MES";
            this.BTN_TOP_MES.UseVisualStyleBackColor = false;
            this.BTN_TOP_MES.Click += new System.EventHandler(this.BTN_TOP_MES_Click);
            // 
            // MainTitlepictureBox
            // 
            this.MainTitlepictureBox.Image = global::ZenHandler.Properties.Resources.mainTitle;
            this.MainTitlepictureBox.Location = new System.Drawing.Point(19, 10);
            this.MainTitlepictureBox.Name = "MainTitlepictureBox";
            this.MainTitlepictureBox.Size = new System.Drawing.Size(43, 36);
            this.MainTitlepictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MainTitlepictureBox.TabIndex = 10;
            this.MainTitlepictureBox.TabStop = false;
            // 
            // MainTitleLabel
            // 
            this.MainTitleLabel.AutoSize = true;
            this.MainTitleLabel.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MainTitleLabel.ForeColor = System.Drawing.Color.DarkGray;
            this.MainTitleLabel.Location = new System.Drawing.Point(74, 14);
            this.MainTitleLabel.Name = "MainTitleLabel";
            this.MainTitleLabel.Size = new System.Drawing.Size(202, 26);
            this.MainTitleLabel.TabIndex = 0;
            this.MainTitleLabel.Text = "ZEN HANDLER V1";
            // 
            // LeftPanel
            // 
            this.LeftPanel.BackColor = System.Drawing.Color.Turquoise;
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftPanel.Location = new System.Drawing.Point(0, 60);
            this.LeftPanel.Margin = new System.Windows.Forms.Padding(0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(1000, 715);
            this.LeftPanel.TabIndex = 1;
            // 
            // CenterPanel
            // 
            this.CenterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.CenterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CenterPanel.Location = new System.Drawing.Point(1000, 60);
            this.CenterPanel.Margin = new System.Windows.Forms.Padding(0);
            this.CenterPanel.Name = "CenterPanel";
            this.tableLayoutPanel1.SetRowSpan(this.CenterPanel, 2);
            this.CenterPanel.Size = new System.Drawing.Size(331, 740);
            this.CenterPanel.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1000F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.Controls.Add(this.TopPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LeftPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.CenterPanel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.RightPanel, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.BottomPanel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1481, 800);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // RightPanel
            // 
            this.RightPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(71)))), ((int)(((byte)(67)))));
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Location = new System.Drawing.Point(1331, 60);
            this.RightPanel.Margin = new System.Windows.Forms.Padding(0);
            this.RightPanel.Name = "RightPanel";
            this.tableLayoutPanel1.SetRowSpan(this.RightPanel, 2);
            this.RightPanel.Size = new System.Drawing.Size(150, 740);
            this.RightPanel.TabIndex = 3;
            // 
            // BottomPanel
            // 
            this.BottomPanel.BackColor = System.Drawing.Color.Gainsboro;
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BottomPanel.Location = new System.Drawing.Point(0, 775);
            this.BottomPanel.Margin = new System.Windows.Forms.Padding(0);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(1000, 25);
            this.BottomPanel.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1481, 800);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "ZenHandler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainTitlepictureBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.Panel CenterPanel;
        private System.Windows.Forms.Label MainTitleLabel;
        private System.Windows.Forms.PictureBox MainTitlepictureBox;
        private System.Windows.Forms.Button BTN_TOP_MES;
        private System.Windows.Forms.Button BTN_TOP_CLIENT;
        private System.Windows.Forms.Button BTN_TOP_CCD;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Button BTN_TOP_LOG;
    }
}

