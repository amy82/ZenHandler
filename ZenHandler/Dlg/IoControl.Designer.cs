namespace ZenHandler.Dlg
{
    partial class IoControl
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
            this.InDataGridView = new System.Windows.Forms.DataGridView();
            this.OutDataGridView = new System.Windows.Forms.DataGridView();
            this.BTN_IO_IN_PREV = new System.Windows.Forms.Button();
            this.BTN_IO_IN_NEXT = new System.Windows.Forms.Button();
            this.BTN_IO_OUT_NEXT = new System.Windows.Forms.Button();
            this.BTN_IO_OUT_PREV = new System.Windows.Forms.Button();
            this.InIndexLabel = new System.Windows.Forms.Label();
            this.OutIndexLabel = new System.Windows.Forms.Label();
            this.DioTitleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.InDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // InDataGridView
            // 
            this.InDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InDataGridView.Location = new System.Drawing.Point(21, 99);
            this.InDataGridView.Name = "InDataGridView";
            this.InDataGridView.RowTemplate.Height = 23;
            this.InDataGridView.Size = new System.Drawing.Size(359, 730);
            this.InDataGridView.TabIndex = 3;
            // 
            // OutDataGridView
            // 
            this.OutDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OutDataGridView.Location = new System.Drawing.Point(396, 99);
            this.OutDataGridView.Name = "OutDataGridView";
            this.OutDataGridView.RowTemplate.Height = 23;
            this.OutDataGridView.Size = new System.Drawing.Size(354, 730);
            this.OutDataGridView.TabIndex = 4;
            // 
            // BTN_IO_IN_PREV
            // 
            this.BTN_IO_IN_PREV.BackColor = System.Drawing.Color.Tan;
            this.BTN_IO_IN_PREV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_IO_IN_PREV.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_IO_IN_PREV.ForeColor = System.Drawing.Color.White;
            this.BTN_IO_IN_PREV.Location = new System.Drawing.Point(63, 922);
            this.BTN_IO_IN_PREV.Name = "BTN_IO_IN_PREV";
            this.BTN_IO_IN_PREV.Size = new System.Drawing.Size(85, 56);
            this.BTN_IO_IN_PREV.TabIndex = 5;
            this.BTN_IO_IN_PREV.Text = "Prev";
            this.BTN_IO_IN_PREV.UseVisualStyleBackColor = false;
            this.BTN_IO_IN_PREV.Click += new System.EventHandler(this.BTN_IO_IN_PREV_Click);
            // 
            // BTN_IO_IN_NEXT
            // 
            this.BTN_IO_IN_NEXT.BackColor = System.Drawing.Color.Tan;
            this.BTN_IO_IN_NEXT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_IO_IN_NEXT.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_IO_IN_NEXT.ForeColor = System.Drawing.Color.White;
            this.BTN_IO_IN_NEXT.Location = new System.Drawing.Point(232, 922);
            this.BTN_IO_IN_NEXT.Name = "BTN_IO_IN_NEXT";
            this.BTN_IO_IN_NEXT.Size = new System.Drawing.Size(85, 56);
            this.BTN_IO_IN_NEXT.TabIndex = 6;
            this.BTN_IO_IN_NEXT.Text = "Next";
            this.BTN_IO_IN_NEXT.UseVisualStyleBackColor = false;
            this.BTN_IO_IN_NEXT.Click += new System.EventHandler(this.BTN_IO_IN_NEXT_Click);
            // 
            // BTN_IO_OUT_NEXT
            // 
            this.BTN_IO_OUT_NEXT.BackColor = System.Drawing.Color.Tan;
            this.BTN_IO_OUT_NEXT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_IO_OUT_NEXT.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_IO_OUT_NEXT.ForeColor = System.Drawing.Color.White;
            this.BTN_IO_OUT_NEXT.Location = new System.Drawing.Point(601, 922);
            this.BTN_IO_OUT_NEXT.Name = "BTN_IO_OUT_NEXT";
            this.BTN_IO_OUT_NEXT.Size = new System.Drawing.Size(85, 56);
            this.BTN_IO_OUT_NEXT.TabIndex = 8;
            this.BTN_IO_OUT_NEXT.Text = "Next";
            this.BTN_IO_OUT_NEXT.UseVisualStyleBackColor = false;
            this.BTN_IO_OUT_NEXT.Click += new System.EventHandler(this.BTN_IO_OUT_NEXT_Click);
            // 
            // BTN_IO_OUT_PREV
            // 
            this.BTN_IO_OUT_PREV.BackColor = System.Drawing.Color.Tan;
            this.BTN_IO_OUT_PREV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_IO_OUT_PREV.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_IO_OUT_PREV.ForeColor = System.Drawing.Color.White;
            this.BTN_IO_OUT_PREV.Location = new System.Drawing.Point(432, 922);
            this.BTN_IO_OUT_PREV.Name = "BTN_IO_OUT_PREV";
            this.BTN_IO_OUT_PREV.Size = new System.Drawing.Size(85, 56);
            this.BTN_IO_OUT_PREV.TabIndex = 7;
            this.BTN_IO_OUT_PREV.Text = "Prev";
            this.BTN_IO_OUT_PREV.UseVisualStyleBackColor = false;
            this.BTN_IO_OUT_PREV.Click += new System.EventHandler(this.BTN_IO_OUT_PREV_Click);
            // 
            // InIndexLabel
            // 
            this.InIndexLabel.BackColor = System.Drawing.Color.White;
            this.InIndexLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InIndexLabel.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.InIndexLabel.Location = new System.Drawing.Point(149, 922);
            this.InIndexLabel.Name = "InIndexLabel";
            this.InIndexLabel.Size = new System.Drawing.Size(82, 56);
            this.InIndexLabel.TabIndex = 9;
            this.InIndexLabel.Text = "1 / 1";
            this.InIndexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OutIndexLabel
            // 
            this.OutIndexLabel.BackColor = System.Drawing.Color.White;
            this.OutIndexLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutIndexLabel.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.OutIndexLabel.Location = new System.Drawing.Point(518, 922);
            this.OutIndexLabel.Name = "OutIndexLabel";
            this.OutIndexLabel.Size = new System.Drawing.Size(82, 56);
            this.OutIndexLabel.TabIndex = 10;
            this.OutIndexLabel.Text = "1 / 1";
            this.OutIndexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DioTitleLabel
            // 
            this.DioTitleLabel.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.DioTitleLabel.Location = new System.Drawing.Point(16, 16);
            this.DioTitleLabel.Name = "DioTitleLabel";
            this.DioTitleLabel.Size = new System.Drawing.Size(250, 42);
            this.DioTitleLabel.TabIndex = 11;
            this.DioTitleLabel.Text = "| IO";
            this.DioTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.Controls.Add(this.DioTitleLabel);
            this.Controls.Add(this.OutIndexLabel);
            this.Controls.Add(this.InIndexLabel);
            this.Controls.Add(this.BTN_IO_OUT_NEXT);
            this.Controls.Add(this.BTN_IO_OUT_PREV);
            this.Controls.Add(this.BTN_IO_IN_NEXT);
            this.Controls.Add(this.BTN_IO_IN_PREV);
            this.Controls.Add(this.OutDataGridView);
            this.Controls.Add(this.InDataGridView);
            this.Name = "IoControl";
            this.Size = new System.Drawing.Size(770, 1000);
            this.Load += new System.EventHandler(this.IoControl_Load);
            this.VisibleChanged += new System.EventHandler(this.IoControl_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.InDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView InDataGridView;
        private System.Windows.Forms.DataGridView OutDataGridView;
        private System.Windows.Forms.Button BTN_IO_IN_PREV;
        private System.Windows.Forms.Button BTN_IO_IN_NEXT;
        private System.Windows.Forms.Button BTN_IO_OUT_NEXT;
        private System.Windows.Forms.Button BTN_IO_OUT_PREV;
        private System.Windows.Forms.Label InIndexLabel;
        private System.Windows.Forms.Label OutIndexLabel;
        private System.Windows.Forms.Label DioTitleLabel;
    }
}
