
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
            this.tableLayoutPanel_Tray_L = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel_Tray_R = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel_Ng_L = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel_Ng_R = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.Font = new System.Drawing.Font("나눔고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TitleLabel.ForeColor = System.Drawing.Color.Black;
            this.TitleLabel.Location = new System.Drawing.Point(3, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(127, 35);
            this.TitleLabel.TabIndex = 12;
            this.TitleLabel.Text = "| Tray Info";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel_Tray_L
            // 
            this.tableLayoutPanel_Tray_L.ColumnCount = 1;
            this.tableLayoutPanel_Tray_L.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Tray_L.Location = new System.Drawing.Point(13, 61);
            this.tableLayoutPanel_Tray_L.Name = "tableLayoutPanel_Tray_L";
            this.tableLayoutPanel_Tray_L.RowCount = 1;
            this.tableLayoutPanel_Tray_L.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Tray_L.Size = new System.Drawing.Size(140, 190);
            this.tableLayoutPanel_Tray_L.TabIndex = 13;
            // 
            // tableLayoutPanel_Tray_R
            // 
            this.tableLayoutPanel_Tray_R.ColumnCount = 1;
            this.tableLayoutPanel_Tray_R.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Tray_R.Location = new System.Drawing.Point(336, 61);
            this.tableLayoutPanel_Tray_R.Name = "tableLayoutPanel_Tray_R";
            this.tableLayoutPanel_Tray_R.RowCount = 1;
            this.tableLayoutPanel_Tray_R.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Tray_R.Size = new System.Drawing.Size(140, 190);
            this.tableLayoutPanel_Tray_R.TabIndex = 14;
            // 
            // tableLayoutPanel_Ng_L
            // 
            this.tableLayoutPanel_Ng_L.ColumnCount = 1;
            this.tableLayoutPanel_Ng_L.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Ng_L.Location = new System.Drawing.Point(158, 161);
            this.tableLayoutPanel_Ng_L.Name = "tableLayoutPanel_Ng_L";
            this.tableLayoutPanel_Ng_L.RowCount = 1;
            this.tableLayoutPanel_Ng_L.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Ng_L.Size = new System.Drawing.Size(80, 90);
            this.tableLayoutPanel_Ng_L.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label1.Location = new System.Drawing.Point(13, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 23);
            this.label1.TabIndex = 16;
            this.label1.Text = "L - LOAD TRAY";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label2.Location = new System.Drawing.Point(336, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 23);
            this.label2.TabIndex = 17;
            this.label2.Text = "R - LOAD TRAY";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label3.Location = new System.Drawing.Point(158, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 23);
            this.label3.TabIndex = 18;
            this.label3.Text = "L-NG";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label4.Location = new System.Drawing.Point(251, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 23);
            this.label4.TabIndex = 20;
            this.label4.Text = "R-NG";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel_Ng_R
            // 
            this.tableLayoutPanel_Ng_R.ColumnCount = 1;
            this.tableLayoutPanel_Ng_R.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Ng_R.Location = new System.Drawing.Point(251, 161);
            this.tableLayoutPanel_Ng_R.Name = "tableLayoutPanel_Ng_R";
            this.tableLayoutPanel_Ng_R.RowCount = 1;
            this.tableLayoutPanel_Ng_R.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Ng_R.Size = new System.Drawing.Size(80, 90);
            this.tableLayoutPanel_Ng_R.TabIndex = 19;
            // 
            // TrayStateInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tableLayoutPanel_Ng_R);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableLayoutPanel_Ng_L);
            this.Controls.Add(this.tableLayoutPanel_Tray_R);
            this.Controls.Add(this.tableLayoutPanel_Tray_L);
            this.Controls.Add(this.TitleLabel);
            this.Name = "TrayStateInfo";
            this.Size = new System.Drawing.Size(486, 257);
            this.Load += new System.EventHandler(this.TrayStateInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Tray_L;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Tray_R;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Ng_L;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Ng_R;
    }
}
