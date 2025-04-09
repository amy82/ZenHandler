
namespace ZenHandler
{
    partial class MessagePopUpForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.warnningImage = new System.Windows.Forms.PictureBox();
            this.labelTop = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BTN_MESSAGE1 = new System.Windows.Forms.Button();
            this.BTN_MESSAGE2 = new System.Windows.Forms.Button();
            this.BTN_MESSAGE3 = new System.Windows.Forms.Button();
            this.MessageBody = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.warnningImage)).BeginInit();
            this.SuspendLayout();
            // 
            // warnningImage
            // 
            this.warnningImage.Image = global::ZenHandler.Properties.Resources.question;
            this.warnningImage.Location = new System.Drawing.Point(9, 11);
            this.warnningImage.Margin = new System.Windows.Forms.Padding(0);
            this.warnningImage.Name = "warnningImage";
            this.warnningImage.Size = new System.Drawing.Size(35, 35);
            this.warnningImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.warnningImage.TabIndex = 0;
            this.warnningImage.TabStop = false;
            // 
            // labelTop
            // 
            this.labelTop.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTop.Location = new System.Drawing.Point(44, 7);
            this.labelTop.Margin = new System.Windows.Forms.Padding(0);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(488, 42);
            this.labelTop.TabIndex = 1;
            this.labelTop.Text = "Info";
            this.labelTop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelTop.VisibleChanged += new System.EventHandler(this.labelTop_VisibleChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Chartreuse;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(-1, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(822, 1);
            this.label2.TabIndex = 4;
            // 
            // BTN_MESSAGE1
            // 
            this.BTN_MESSAGE1.BackColor = System.Drawing.Color.Goldenrod;
            this.BTN_MESSAGE1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.BTN_MESSAGE1.FlatAppearance.BorderSize = 0;
            this.BTN_MESSAGE1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MESSAGE1.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_MESSAGE1.Location = new System.Drawing.Point(681, 189);
            this.BTN_MESSAGE1.Name = "BTN_MESSAGE1";
            this.BTN_MESSAGE1.Size = new System.Drawing.Size(116, 51);
            this.BTN_MESSAGE1.TabIndex = 2;
            this.BTN_MESSAGE1.Text = "취소";
            this.BTN_MESSAGE1.UseVisualStyleBackColor = false;
            this.BTN_MESSAGE1.Click += new System.EventHandler(this.BTN_MESSAGE1_Click);
            // 
            // BTN_MESSAGE2
            // 
            this.BTN_MESSAGE2.BackColor = System.Drawing.Color.Goldenrod;
            this.BTN_MESSAGE2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.BTN_MESSAGE2.FlatAppearance.BorderSize = 0;
            this.BTN_MESSAGE2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MESSAGE2.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_MESSAGE2.Location = new System.Drawing.Point(559, 189);
            this.BTN_MESSAGE2.Name = "BTN_MESSAGE2";
            this.BTN_MESSAGE2.Size = new System.Drawing.Size(116, 51);
            this.BTN_MESSAGE2.TabIndex = 5;
            this.BTN_MESSAGE2.Text = "확인";
            this.BTN_MESSAGE2.UseVisualStyleBackColor = false;
            this.BTN_MESSAGE2.Click += new System.EventHandler(this.BTN_MESSAGE2_Click);
            // 
            // BTN_MESSAGE3
            // 
            this.BTN_MESSAGE3.BackColor = System.Drawing.Color.Goldenrod;
            this.BTN_MESSAGE3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.BTN_MESSAGE3.FlatAppearance.BorderSize = 0;
            this.BTN_MESSAGE3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MESSAGE3.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_MESSAGE3.Location = new System.Drawing.Point(437, 189);
            this.BTN_MESSAGE3.Name = "BTN_MESSAGE3";
            this.BTN_MESSAGE3.Size = new System.Drawing.Size(116, 51);
            this.BTN_MESSAGE3.TabIndex = 6;
            this.BTN_MESSAGE3.Text = "중단";
            this.BTN_MESSAGE3.UseVisualStyleBackColor = false;
            this.BTN_MESSAGE3.Click += new System.EventHandler(this.BTN_MESSAGE3_Click);
            // 
            // MessageBody
            // 
            this.MessageBody.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MessageBody.ForeColor = System.Drawing.Color.White;
            this.MessageBody.Location = new System.Drawing.Point(9, 65);
            this.MessageBody.Margin = new System.Windows.Forms.Padding(0);
            this.MessageBody.Name = "MessageBody";
            this.MessageBody.Size = new System.Drawing.Size(802, 121);
            this.MessageBody.TabIndex = 7;
            this.MessageBody.Text = "자동 운전 중 설정할 수 없습니다.";
            // 
            // MessagePopUpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ClientSize = new System.Drawing.Size(820, 255);
            this.ControlBox = false;
            this.Controls.Add(this.MessageBody);
            this.Controls.Add(this.BTN_MESSAGE3);
            this.Controls.Add(this.BTN_MESSAGE2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BTN_MESSAGE1);
            this.Controls.Add(this.labelTop);
            this.Controls.Add(this.warnningImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MessagePopUpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MessagePopUpForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MessagePopUpForm_FormClosing_1);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MessagePopUpForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.warnningImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox warnningImage;
        private System.Windows.Forms.Label labelTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BTN_MESSAGE1;
        private System.Windows.Forms.Button BTN_MESSAGE2;
        private System.Windows.Forms.Button BTN_MESSAGE3;
        private System.Windows.Forms.Label MessageBody;
    }
}