
namespace ZenHandler.Dlg
{
    partial class TeachingLens
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
            this.groupTeachLens = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BTN_TEACH_LENS_Y = new System.Windows.Forms.Button();
            this.BTN_TEACH_LENS_TY = new System.Windows.Forms.Button();
            this.BTN_TEACH_SERVO_ON = new System.Windows.Forms.Button();
            this.BTN_TEACH_LENS_TX = new System.Windows.Forms.Button();
            this.BTN_TEACH_SERVO_OFF = new System.Windows.Forms.Button();
            this.BTN_TEACH_SERVO_RESET = new System.Windows.Forms.Button();
            this.BTN_TEACH_LENS_Z = new System.Windows.Forms.Button();
            this.BTN_TEACH_LENS_X = new System.Windows.Forms.Button();
            this.LensTeachGridView = new System.Windows.Forms.DataGridView();
            this.groupTeachLens.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LensTeachGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupTeachLens
            // 
            this.groupTeachLens.BackColor = System.Drawing.Color.White;
            this.groupTeachLens.Controls.Add(this.label4);
            this.groupTeachLens.Controls.Add(this.label3);
            this.groupTeachLens.Controls.Add(this.BTN_TEACH_LENS_Y);
            this.groupTeachLens.Controls.Add(this.BTN_TEACH_LENS_TY);
            this.groupTeachLens.Controls.Add(this.BTN_TEACH_SERVO_ON);
            this.groupTeachLens.Controls.Add(this.BTN_TEACH_LENS_TX);
            this.groupTeachLens.Controls.Add(this.BTN_TEACH_SERVO_OFF);
            this.groupTeachLens.Controls.Add(this.BTN_TEACH_SERVO_RESET);
            this.groupTeachLens.Controls.Add(this.BTN_TEACH_LENS_Z);
            this.groupTeachLens.Controls.Add(this.BTN_TEACH_LENS_X);
            this.groupTeachLens.Controls.Add(this.LensTeachGridView);
            this.groupTeachLens.Location = new System.Drawing.Point(0, 3);
            this.groupTeachLens.Name = "groupTeachLens";
            this.groupTeachLens.Size = new System.Drawing.Size(908, 670);
            this.groupTeachLens.TabIndex = 46;
            this.groupTeachLens.TabStop = false;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(17, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 23);
            this.label4.TabIndex = 44;
            this.label4.Text = "MOTOR SELECT";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(15, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 23);
            this.label3.TabIndex = 43;
            this.label3.Text = "MOTOR SET";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_TEACH_LENS_Y
            // 
            this.BTN_TEACH_LENS_Y.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_LENS_Y.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_LENS_Y.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_LENS_Y.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_TEACH_LENS_Y.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_LENS_Y.Location = new System.Drawing.Point(17, 282);
            this.BTN_TEACH_LENS_Y.Name = "BTN_TEACH_LENS_Y";
            this.BTN_TEACH_LENS_Y.Size = new System.Drawing.Size(138, 47);
            this.BTN_TEACH_LENS_Y.TabIndex = 38;
            this.BTN_TEACH_LENS_Y.Text = "LENS Y";
            this.BTN_TEACH_LENS_Y.UseVisualStyleBackColor = false;
            this.BTN_TEACH_LENS_Y.Click += new System.EventHandler(this.BTN_TEACH_LENS_Y_Click);
            // 
            // BTN_TEACH_LENS_TY
            // 
            this.BTN_TEACH_LENS_TY.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_LENS_TY.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_LENS_TY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_LENS_TY.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_TEACH_LENS_TY.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_LENS_TY.Location = new System.Drawing.Point(17, 426);
            this.BTN_TEACH_LENS_TY.Name = "BTN_TEACH_LENS_TY";
            this.BTN_TEACH_LENS_TY.Size = new System.Drawing.Size(138, 47);
            this.BTN_TEACH_LENS_TY.TabIndex = 42;
            this.BTN_TEACH_LENS_TY.Text = "LENS TY";
            this.BTN_TEACH_LENS_TY.UseVisualStyleBackColor = false;
            this.BTN_TEACH_LENS_TY.Click += new System.EventHandler(this.BTN_TEACH_LENS_TY_Click);
            // 
            // BTN_TEACH_SERVO_ON
            // 
            this.BTN_TEACH_SERVO_ON.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_SERVO_ON.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_SERVO_ON.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_SERVO_ON.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_TEACH_SERVO_ON.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_SERVO_ON.Location = new System.Drawing.Point(15, 54);
            this.BTN_TEACH_SERVO_ON.Name = "BTN_TEACH_SERVO_ON";
            this.BTN_TEACH_SERVO_ON.Size = new System.Drawing.Size(138, 45);
            this.BTN_TEACH_SERVO_ON.TabIndex = 34;
            this.BTN_TEACH_SERVO_ON.Text = "SERVO ON";
            this.BTN_TEACH_SERVO_ON.UseVisualStyleBackColor = false;
            this.BTN_TEACH_SERVO_ON.Click += new System.EventHandler(this.BTN_TEACH_SERVO_ON_Click_1);
            // 
            // BTN_TEACH_LENS_TX
            // 
            this.BTN_TEACH_LENS_TX.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_LENS_TX.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_LENS_TX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_LENS_TX.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_TEACH_LENS_TX.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_LENS_TX.Location = new System.Drawing.Point(17, 378);
            this.BTN_TEACH_LENS_TX.Name = "BTN_TEACH_LENS_TX";
            this.BTN_TEACH_LENS_TX.Size = new System.Drawing.Size(138, 47);
            this.BTN_TEACH_LENS_TX.TabIndex = 41;
            this.BTN_TEACH_LENS_TX.Text = "LENS TX";
            this.BTN_TEACH_LENS_TX.UseVisualStyleBackColor = false;
            this.BTN_TEACH_LENS_TX.Click += new System.EventHandler(this.BTN_TEACH_LENS_TX_Click);
            // 
            // BTN_TEACH_SERVO_OFF
            // 
            this.BTN_TEACH_SERVO_OFF.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_SERVO_OFF.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_SERVO_OFF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_SERVO_OFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_TEACH_SERVO_OFF.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_SERVO_OFF.Location = new System.Drawing.Point(15, 100);
            this.BTN_TEACH_SERVO_OFF.Name = "BTN_TEACH_SERVO_OFF";
            this.BTN_TEACH_SERVO_OFF.Size = new System.Drawing.Size(138, 45);
            this.BTN_TEACH_SERVO_OFF.TabIndex = 35;
            this.BTN_TEACH_SERVO_OFF.Text = "SERVO OFF";
            this.BTN_TEACH_SERVO_OFF.UseVisualStyleBackColor = false;
            this.BTN_TEACH_SERVO_OFF.Click += new System.EventHandler(this.BTN_TEACH_SERVO_OFF_Click_1);
            // 
            // BTN_TEACH_SERVO_RESET
            // 
            this.BTN_TEACH_SERVO_RESET.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_SERVO_RESET.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_SERVO_RESET.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_SERVO_RESET.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_TEACH_SERVO_RESET.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_SERVO_RESET.Location = new System.Drawing.Point(15, 146);
            this.BTN_TEACH_SERVO_RESET.Name = "BTN_TEACH_SERVO_RESET";
            this.BTN_TEACH_SERVO_RESET.Size = new System.Drawing.Size(138, 45);
            this.BTN_TEACH_SERVO_RESET.TabIndex = 36;
            this.BTN_TEACH_SERVO_RESET.Text = "SERVO RESET";
            this.BTN_TEACH_SERVO_RESET.UseVisualStyleBackColor = false;
            this.BTN_TEACH_SERVO_RESET.Click += new System.EventHandler(this.BTN_TEACH_SERVO_RESET_Click_1);
            // 
            // BTN_TEACH_LENS_Z
            // 
            this.BTN_TEACH_LENS_Z.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_LENS_Z.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_LENS_Z.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_LENS_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_TEACH_LENS_Z.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_LENS_Z.Location = new System.Drawing.Point(17, 330);
            this.BTN_TEACH_LENS_Z.Name = "BTN_TEACH_LENS_Z";
            this.BTN_TEACH_LENS_Z.Size = new System.Drawing.Size(138, 47);
            this.BTN_TEACH_LENS_Z.TabIndex = 39;
            this.BTN_TEACH_LENS_Z.Text = "LENS Z";
            this.BTN_TEACH_LENS_Z.UseVisualStyleBackColor = false;
            this.BTN_TEACH_LENS_Z.Click += new System.EventHandler(this.BTN_TEACH_LENS_Z_Click);
            // 
            // BTN_TEACH_LENS_X
            // 
            this.BTN_TEACH_LENS_X.BackColor = System.Drawing.Color.Tan;
            this.BTN_TEACH_LENS_X.FlatAppearance.BorderSize = 0;
            this.BTN_TEACH_LENS_X.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_TEACH_LENS_X.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.BTN_TEACH_LENS_X.ForeColor = System.Drawing.Color.White;
            this.BTN_TEACH_LENS_X.Location = new System.Drawing.Point(17, 234);
            this.BTN_TEACH_LENS_X.Name = "BTN_TEACH_LENS_X";
            this.BTN_TEACH_LENS_X.Size = new System.Drawing.Size(138, 47);
            this.BTN_TEACH_LENS_X.TabIndex = 37;
            this.BTN_TEACH_LENS_X.Text = "LENS X";
            this.BTN_TEACH_LENS_X.UseVisualStyleBackColor = false;
            this.BTN_TEACH_LENS_X.Click += new System.EventHandler(this.BTN_TEACH_LENS_X_Click);
            // 
            // LensTeachGridView
            // 
            this.LensTeachGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LensTeachGridView.Location = new System.Drawing.Point(185, 28);
            this.LensTeachGridView.Name = "LensTeachGridView";
            this.LensTeachGridView.RowTemplate.Height = 23;
            this.LensTeachGridView.Size = new System.Drawing.Size(240, 150);
            this.LensTeachGridView.TabIndex = 28;
            // 
            // TeachingLens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupTeachLens);
            this.Name = "TeachingLens";
            this.Size = new System.Drawing.Size(952, 724);
            this.groupTeachLens.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LensTeachGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupTeachLens;
        private System.Windows.Forms.DataGridView LensTeachGridView;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BTN_TEACH_LENS_Y;
        private System.Windows.Forms.Button BTN_TEACH_LENS_TY;
        private System.Windows.Forms.Button BTN_TEACH_SERVO_ON;
        private System.Windows.Forms.Button BTN_TEACH_LENS_TX;
        private System.Windows.Forms.Button BTN_TEACH_SERVO_OFF;
        private System.Windows.Forms.Button BTN_TEACH_SERVO_RESET;
        private System.Windows.Forms.Button BTN_TEACH_LENS_Z;
        private System.Windows.Forms.Button BTN_TEACH_LENS_X;
    }
}
