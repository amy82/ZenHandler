
namespace ZenHandler.Dlg
{
    partial class ProductionInfo
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
            this.panel_ProductionInfo = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BTN_MAIN_PINCOUNT_RESET = new System.Windows.Forms.Button();
            this.label_PinCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_ProcessState = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.BTN_MAIN_JUDGE_RESET = new System.Windows.Forms.Button();
            this.textBox_TopLot = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label_production_total = new System.Windows.Forms.Label();
            this.label_production_ng = new System.Windows.Forms.Label();
            this.label_production_ok = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_Model = new System.Windows.Forms.TextBox();
            this.textBox_Recipe = new System.Windows.Forms.TextBox();
            this.BTN_MAIN_RECIPE_VEW = new System.Windows.Forms.Button();
            this.panel_ProductionInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_ProductionInfo
            // 
            this.panel_ProductionInfo.BackColor = System.Drawing.Color.White;
            this.panel_ProductionInfo.Controls.Add(this.groupBox1);
            this.panel_ProductionInfo.Location = new System.Drawing.Point(3, 3);
            this.panel_ProductionInfo.Name = "panel_ProductionInfo";
            this.panel_ProductionInfo.Size = new System.Drawing.Size(994, 318);
            this.panel_ProductionInfo.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BTN_MAIN_RECIPE_VEW);
            this.groupBox1.Controls.Add(this.textBox_Recipe);
            this.groupBox1.Controls.Add(this.textBox_Model);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.BTN_MAIN_PINCOUNT_RESET);
            this.groupBox1.Controls.Add(this.label_PinCount);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_ProcessState);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.BTN_MAIN_JUDGE_RESET);
            this.groupBox1.Controls.Add(this.textBox_TopLot);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label_production_total);
            this.groupBox1.Controls.Add(this.label_production_ng);
            this.groupBox1.Controls.Add(this.label_production_ok);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(4, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(987, 300);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " PRODUCTION INFO";
            // 
            // BTN_MAIN_PINCOUNT_RESET
            // 
            this.BTN_MAIN_PINCOUNT_RESET.BackColor = System.Drawing.Color.Tan;
            this.BTN_MAIN_PINCOUNT_RESET.FlatAppearance.BorderSize = 0;
            this.BTN_MAIN_PINCOUNT_RESET.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MAIN_PINCOUNT_RESET.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MAIN_PINCOUNT_RESET.Location = new System.Drawing.Point(141, 162);
            this.BTN_MAIN_PINCOUNT_RESET.Name = "BTN_MAIN_PINCOUNT_RESET";
            this.BTN_MAIN_PINCOUNT_RESET.Size = new System.Drawing.Size(66, 28);
            this.BTN_MAIN_PINCOUNT_RESET.TabIndex = 21;
            this.BTN_MAIN_PINCOUNT_RESET.Text = "RESET";
            this.BTN_MAIN_PINCOUNT_RESET.UseVisualStyleBackColor = false;
            this.BTN_MAIN_PINCOUNT_RESET.Click += new System.EventHandler(this.BTN_MAIN_PINCOUNT_RESET_Click);
            // 
            // label_PinCount
            // 
            this.label_PinCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_PinCount.Location = new System.Drawing.Point(8, 162);
            this.label_PinCount.Name = "label_PinCount";
            this.label_PinCount.Size = new System.Drawing.Size(129, 28);
            this.label_PinCount.TabIndex = 20;
            this.label_PinCount.Text = "0";
            this.label_PinCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(10, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 14);
            this.label6.TabIndex = 19;
            this.label6.Text = "PINT COUNT :";
            // 
            // textBox_ProcessState
            // 
            this.textBox_ProcessState.BackColor = System.Drawing.Color.White;
            this.textBox_ProcessState.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_ProcessState.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBox_ProcessState.Location = new System.Drawing.Point(7, 97);
            this.textBox_ProcessState.Name = "textBox_ProcessState";
            this.textBox_ProcessState.ReadOnly = true;
            this.textBox_ProcessState.Size = new System.Drawing.Size(130, 33);
            this.textBox_ProcessState.TabIndex = 18;
            this.textBox_ProcessState.Text = "INIT";
            this.textBox_ProcessState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("나눔고딕", 8.999999F);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(10, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 14);
            this.label5.TabIndex = 17;
            this.label5.Text = "Process State";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BTN_MAIN_JUDGE_RESET
            // 
            this.BTN_MAIN_JUDGE_RESET.BackColor = System.Drawing.Color.Tan;
            this.BTN_MAIN_JUDGE_RESET.FlatAppearance.BorderSize = 0;
            this.BTN_MAIN_JUDGE_RESET.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MAIN_JUDGE_RESET.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MAIN_JUDGE_RESET.Location = new System.Drawing.Point(888, 108);
            this.BTN_MAIN_JUDGE_RESET.Name = "BTN_MAIN_JUDGE_RESET";
            this.BTN_MAIN_JUDGE_RESET.Size = new System.Drawing.Size(83, 35);
            this.BTN_MAIN_JUDGE_RESET.TabIndex = 16;
            this.BTN_MAIN_JUDGE_RESET.Text = "RESET";
            this.BTN_MAIN_JUDGE_RESET.UseVisualStyleBackColor = false;
            this.BTN_MAIN_JUDGE_RESET.Click += new System.EventHandler(this.BTN_MAIN_JUDGE_RESET_Click);
            // 
            // textBox_TopLot
            // 
            this.textBox_TopLot.BackColor = System.Drawing.Color.MintCream;
            this.textBox_TopLot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_TopLot.Font = new System.Drawing.Font("나눔고딕", 12.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_TopLot.ForeColor = System.Drawing.Color.Black;
            this.textBox_TopLot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBox_TopLot.Location = new System.Drawing.Point(9, 41);
            this.textBox_TopLot.Name = "textBox_TopLot";
            this.textBox_TopLot.ReadOnly = true;
            this.textBox_TopLot.Size = new System.Drawing.Size(439, 27);
            this.textBox_TopLot.TabIndex = 15;
            this.textBox_TopLot.Text = "0000000000";
            this.textBox_TopLot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("나눔고딕", 8.999999F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 14;
            this.label1.Text = "BARCODE";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_production_total
            // 
            this.label_production_total.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_production_total.Location = new System.Drawing.Point(882, 74);
            this.label_production_total.Name = "label_production_total";
            this.label_production_total.Size = new System.Drawing.Size(89, 22);
            this.label_production_total.TabIndex = 5;
            this.label_production_total.Text = "0";
            this.label_production_total.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_production_ng
            // 
            this.label_production_ng.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_production_ng.Location = new System.Drawing.Point(882, 48);
            this.label_production_ng.Name = "label_production_ng";
            this.label_production_ng.Size = new System.Drawing.Size(89, 22);
            this.label_production_ng.TabIndex = 4;
            this.label_production_ng.Text = "0";
            this.label_production_ng.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_production_ok
            // 
            this.label_production_ok.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_production_ok.Location = new System.Drawing.Point(882, 22);
            this.label_production_ok.Name = "label_production_ok";
            this.label_production_ok.Size = new System.Drawing.Size(89, 22);
            this.label_production_ok.TabIndex = 3;
            this.label_production_ok.Text = "0";
            this.label_production_ok.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(827, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "TOTAL :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(827, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 14);
            this.label3.TabIndex = 1;
            this.label3.Text = "NG :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(827, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "OK :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(10, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 14);
            this.label7.TabIndex = 22;
            this.label7.Text = "MODEL :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(10, 251);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 14);
            this.label8.TabIndex = 23;
            this.label8.Text = "RECIPE ID :";
            // 
            // textBox_Model
            // 
            this.textBox_Model.BackColor = System.Drawing.Color.White;
            this.textBox_Model.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Model.Location = new System.Drawing.Point(13, 218);
            this.textBox_Model.Name = "textBox_Model";
            this.textBox_Model.ReadOnly = true;
            this.textBox_Model.Size = new System.Drawing.Size(241, 26);
            this.textBox_Model.TabIndex = 36;
            this.textBox_Model.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_Recipe
            // 
            this.textBox_Recipe.BackColor = System.Drawing.Color.White;
            this.textBox_Recipe.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Recipe.Location = new System.Drawing.Point(13, 268);
            this.textBox_Recipe.Name = "textBox_Recipe";
            this.textBox_Recipe.ReadOnly = true;
            this.textBox_Recipe.Size = new System.Drawing.Size(241, 26);
            this.textBox_Recipe.TabIndex = 38;
            this.textBox_Recipe.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BTN_MAIN_RECIPE_VEW
            // 
            this.BTN_MAIN_RECIPE_VEW.BackColor = System.Drawing.Color.Tan;
            this.BTN_MAIN_RECIPE_VEW.FlatAppearance.BorderSize = 0;
            this.BTN_MAIN_RECIPE_VEW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MAIN_RECIPE_VEW.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_MAIN_RECIPE_VEW.Location = new System.Drawing.Point(260, 268);
            this.BTN_MAIN_RECIPE_VEW.Name = "BTN_MAIN_RECIPE_VEW";
            this.BTN_MAIN_RECIPE_VEW.Size = new System.Drawing.Size(66, 28);
            this.BTN_MAIN_RECIPE_VEW.TabIndex = 39;
            this.BTN_MAIN_RECIPE_VEW.Text = "VIEW";
            this.BTN_MAIN_RECIPE_VEW.UseVisualStyleBackColor = false;
            this.BTN_MAIN_RECIPE_VEW.Click += new System.EventHandler(this.BTN_MAIN_RECIPE_VEW_Click);
            // 
            // ProductionInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_ProductionInfo);
            this.Name = "ProductionInfo";
            this.Size = new System.Drawing.Size(1000, 324);
            this.panel_ProductionInfo.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_ProductionInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BTN_MAIN_PINCOUNT_RESET;
        private System.Windows.Forms.Label label_PinCount;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox textBox_ProcessState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BTN_MAIN_JUDGE_RESET;
        private System.Windows.Forms.TextBox textBox_TopLot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_production_total;
        private System.Windows.Forms.Label label_production_ng;
        private System.Windows.Forms.Label label_production_ok;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_Model;
        private System.Windows.Forms.TextBox textBox_Recipe;
        private System.Windows.Forms.Button BTN_MAIN_RECIPE_VEW;
    }
}
