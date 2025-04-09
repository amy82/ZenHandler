namespace ZenHandler.Dlg
{
    partial class MainControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ManualTitleLabel = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.textBox_Recipe = new System.Windows.Forms.TextBox();
            this.BTN_MAIN_RECIPE_VIEW = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBox_Model = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridView_Result = new System.Windows.Forms.DataGridView();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Result)).BeginInit();
            this.SuspendLayout();
            // 
            // ManualTitleLabel
            // 
            this.ManualTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ManualTitleLabel.Location = new System.Drawing.Point(3, 0);
            this.ManualTitleLabel.Name = "ManualTitleLabel";
            this.ManualTitleLabel.Size = new System.Drawing.Size(250, 50);
            this.ManualTitleLabel.TabIndex = 2;
            this.ManualTitleLabel.Text = "| MAIN";
            this.ManualTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.White;
            this.groupBox6.Controls.Add(this.textBox_Recipe);
            this.groupBox6.Controls.Add(this.BTN_MAIN_RECIPE_VIEW);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Location = new System.Drawing.Point(424, 406);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(274, 84);
            this.groupBox6.TabIndex = 49;
            this.groupBox6.TabStop = false;
            // 
            // textBox_Recipe
            // 
            this.textBox_Recipe.BackColor = System.Drawing.Color.White;
            this.textBox_Recipe.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Recipe.Location = new System.Drawing.Point(17, 46);
            this.textBox_Recipe.Name = "textBox_Recipe";
            this.textBox_Recipe.ReadOnly = true;
            this.textBox_Recipe.Size = new System.Drawing.Size(241, 26);
            this.textBox_Recipe.TabIndex = 37;
            this.textBox_Recipe.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BTN_MAIN_RECIPE_VIEW
            // 
            this.BTN_MAIN_RECIPE_VIEW.BackColor = System.Drawing.Color.Tan;
            this.BTN_MAIN_RECIPE_VIEW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MAIN_RECIPE_VIEW.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_MAIN_RECIPE_VIEW.ForeColor = System.Drawing.Color.White;
            this.BTN_MAIN_RECIPE_VIEW.Location = new System.Drawing.Point(190, 8);
            this.BTN_MAIN_RECIPE_VIEW.Name = "BTN_MAIN_RECIPE_VIEW";
            this.BTN_MAIN_RECIPE_VIEW.Size = new System.Drawing.Size(74, 32);
            this.BTN_MAIN_RECIPE_VIEW.TabIndex = 36;
            this.BTN_MAIN_RECIPE_VIEW.Text = "VIEW";
            this.BTN_MAIN_RECIPE_VIEW.UseVisualStyleBackColor = false;
            this.BTN_MAIN_RECIPE_VIEW.Visible = false;
            this.BTN_MAIN_RECIPE_VIEW.Click += new System.EventHandler(this.BTN_MAIN_RECIPE_VIEW_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(14, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 23);
            this.label6.TabIndex = 26;
            this.label6.Text = "RECIPE ID";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.White;
            this.groupBox5.Controls.Add(this.textBox_Model);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Location = new System.Drawing.Point(424, 300);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(274, 84);
            this.groupBox5.TabIndex = 48;
            this.groupBox5.TabStop = false;
            // 
            // textBox_Model
            // 
            this.textBox_Model.BackColor = System.Drawing.Color.White;
            this.textBox_Model.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Model.Location = new System.Drawing.Point(17, 46);
            this.textBox_Model.Name = "textBox_Model";
            this.textBox_Model.ReadOnly = true;
            this.textBox_Model.Size = new System.Drawing.Size(241, 26);
            this.textBox_Model.TabIndex = 35;
            this.textBox_Model.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(14, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(145, 23);
            this.label5.TabIndex = 26;
            this.label5.Text = "MODEL";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridView_Result
            // 
            this.dataGridView_Result.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Result.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_Result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.ActiveBorder;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Result.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView_Result.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView_Result.Location = new System.Drawing.Point(549, 510);
            this.dataGridView_Result.Name = "dataGridView_Result";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.RosyBrown;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Result.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView_Result.RowTemplate.Height = 23;
            this.dataGridView_Result.Size = new System.Drawing.Size(68, 29);
            this.dataGridView_Result.TabIndex = 50;
            // 
            // MainControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LightSalmon;
            this.Controls.Add(this.dataGridView_Result);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.ManualTitleLabel);
            this.Controls.Add(this.groupBox5);
            this.Name = "MainControl";
            this.Size = new System.Drawing.Size(751, 783);
            this.VisibleChanged += new System.EventHandler(this.MainControl_VisibleChanged);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Result)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ManualTitleLabel;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BTN_MAIN_RECIPE_VIEW;
        private System.Windows.Forms.TextBox textBox_Model;
        private System.Windows.Forms.TextBox textBox_Recipe;
        private System.Windows.Forms.DataGridView dataGridView_Result;
    }
}
