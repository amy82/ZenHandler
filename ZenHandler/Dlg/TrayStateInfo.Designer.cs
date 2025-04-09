
namespace ZenHandler.Dlg
{
    partial class TrayStateInfo
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
            this.TitleLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel_Load = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel_Unload = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel_Ng = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.Font = new System.Drawing.Font("나눔고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TitleLabel.ForeColor = System.Drawing.Color.Black;
            this.TitleLabel.Location = new System.Drawing.Point(3, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(127, 43);
            this.TitleLabel.TabIndex = 12;
            this.TitleLabel.Text = "| Tray Info";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel_Load
            // 
            this.tableLayoutPanel_Load.ColumnCount = 1;
            this.tableLayoutPanel_Load.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Load.Location = new System.Drawing.Point(22, 82);
            this.tableLayoutPanel_Load.Name = "tableLayoutPanel_Load";
            this.tableLayoutPanel_Load.RowCount = 1;
            this.tableLayoutPanel_Load.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Load.Size = new System.Drawing.Size(213, 174);
            this.tableLayoutPanel_Load.TabIndex = 13;
            // 
            // tableLayoutPanel_Unload
            // 
            this.tableLayoutPanel_Unload.ColumnCount = 1;
            this.tableLayoutPanel_Unload.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Unload.Location = new System.Drawing.Point(249, 82);
            this.tableLayoutPanel_Unload.Name = "tableLayoutPanel_Unload";
            this.tableLayoutPanel_Unload.RowCount = 1;
            this.tableLayoutPanel_Unload.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Unload.Size = new System.Drawing.Size(213, 174);
            this.tableLayoutPanel_Unload.TabIndex = 14;
            // 
            // tableLayoutPanel_Ng
            // 
            this.tableLayoutPanel_Ng.ColumnCount = 1;
            this.tableLayoutPanel_Ng.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Ng.Location = new System.Drawing.Point(475, 82);
            this.tableLayoutPanel_Ng.Name = "tableLayoutPanel_Ng";
            this.tableLayoutPanel_Ng.RowCount = 1;
            this.tableLayoutPanel_Ng.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Ng.Size = new System.Drawing.Size(213, 174);
            this.tableLayoutPanel_Ng.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Font = new System.Drawing.Font("나눔고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label1.Location = new System.Drawing.Point(22, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 23);
            this.label1.TabIndex = 16;
            this.label1.Text = "LOAD TRAY";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Font = new System.Drawing.Font("나눔고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label2.Location = new System.Drawing.Point(249, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(213, 23);
            this.label2.TabIndex = 17;
            this.label2.Text = "UNLOAD TRAY";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Font = new System.Drawing.Font("나눔고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label3.Location = new System.Drawing.Point(475, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(213, 23);
            this.label3.TabIndex = 18;
            this.label3.Text = "NG TRAY";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TrayStateInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableLayoutPanel_Ng);
            this.Controls.Add(this.tableLayoutPanel_Unload);
            this.Controls.Add(this.tableLayoutPanel_Load);
            this.Controls.Add(this.TitleLabel);
            this.Name = "TrayStateInfo";
            this.Size = new System.Drawing.Size(800, 325);
            this.Load += new System.EventHandler(this.TrayStateInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Load;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Unload;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Ng;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
