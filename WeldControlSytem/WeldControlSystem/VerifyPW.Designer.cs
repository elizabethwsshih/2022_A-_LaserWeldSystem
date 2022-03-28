namespace WeldControlSystem
{
    partial class VerifyPW
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
            this.PW_OKBtn = new System.Windows.Forms.Button();
            this.PW_CancelBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.PW_TxtBox = new System.Windows.Forms.TextBox();
            this.User_TxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PW_OKBtn
            // 
            this.PW_OKBtn.Location = new System.Drawing.Point(66, 116);
            this.PW_OKBtn.Name = "PW_OKBtn";
            this.PW_OKBtn.Size = new System.Drawing.Size(92, 27);
            this.PW_OKBtn.TabIndex = 3;
            this.PW_OKBtn.Text = "確認";
            this.PW_OKBtn.UseVisualStyleBackColor = true;
            this.PW_OKBtn.Click += new System.EventHandler(this.PW_OKBtn_Click);
            // 
            // PW_CancelBtn
            // 
            this.PW_CancelBtn.Location = new System.Drawing.Point(164, 116);
            this.PW_CancelBtn.Name = "PW_CancelBtn";
            this.PW_CancelBtn.Size = new System.Drawing.Size(92, 27);
            this.PW_CancelBtn.TabIndex = 4;
            this.PW_CancelBtn.Text = "取消";
            this.PW_CancelBtn.UseVisualStyleBackColor = true;
            this.PW_CancelBtn.Click += new System.EventHandler(this.PW_CancelBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "密碼";
            // 
            // PW_TxtBox
            // 
            this.PW_TxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.PW_TxtBox.Location = new System.Drawing.Point(66, 71);
            this.PW_TxtBox.Name = "PW_TxtBox";
            this.PW_TxtBox.PasswordChar = '*';
            this.PW_TxtBox.Size = new System.Drawing.Size(187, 29);
            this.PW_TxtBox.TabIndex = 2;
            this.PW_TxtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PW_TxtBox_KeyDown);
            // 
            // User_TxtBox
            // 
            this.User_TxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.User_TxtBox.Location = new System.Drawing.Point(66, 27);
            this.User_TxtBox.Name = "User_TxtBox";
            this.User_TxtBox.Size = new System.Drawing.Size(187, 29);
            this.User_TxtBox.TabIndex = 1;
            this.User_TxtBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.User_TxtBox_MouseClick);
            this.User_TxtBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.User_TxtBox_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "工號";
            // 
            // VerifyPW
            // 
            this.AcceptButton = this.PW_OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 155);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.User_TxtBox);
            this.Controls.Add(this.PW_TxtBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PW_CancelBtn);
            this.Controls.Add(this.PW_OKBtn);
            this.Name = "VerifyPW";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "請輸入帳密";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VerifyPW_FormClosed);
            this.Load += new System.EventHandler(this.VerifyPW_Load);
            this.Shown += new System.EventHandler(this.VerifyPW_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PW_OKBtn;
        private System.Windows.Forms.Button PW_CancelBtn;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox PW_TxtBox;
        public System.Windows.Forms.TextBox User_TxtBox;
        private System.Windows.Forms.Label label2;
    }
}