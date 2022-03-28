namespace WeldControlSystem
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.銲接主程式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.電池銲接程式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.控制程式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.平台移動ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.雷射控制IO點ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.權限ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripXCurPosTxtBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripXHomeTxtBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripYCurPosTxtBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripYHomeTxtBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripZCurPosTxtBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripZHomeTxtBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox4 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripAlmTxtBox = new System.Windows.Forms.ToolStripTextBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.銲接主程式ToolStripMenuItem,
            this.控制程式ToolStripMenuItem,
            this.權限ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(1216, 26);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Click += new System.EventHandler(this.menuStrip1_Click);
            // 
            // 銲接主程式ToolStripMenuItem
            // 
            this.銲接主程式ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.電池銲接程式ToolStripMenuItem});
            this.銲接主程式ToolStripMenuItem.Name = "銲接主程式ToolStripMenuItem";
            this.銲接主程式ToolStripMenuItem.Size = new System.Drawing.Size(101, 24);
            this.銲接主程式ToolStripMenuItem.Text = "銲接主程式";
            // 
            // 電池銲接程式ToolStripMenuItem
            // 
            this.電池銲接程式ToolStripMenuItem.Name = "電池銲接程式ToolStripMenuItem";
            this.電池銲接程式ToolStripMenuItem.Size = new System.Drawing.Size(174, 24);
            this.電池銲接程式ToolStripMenuItem.Text = "電池銲接程式";
            this.電池銲接程式ToolStripMenuItem.Click += new System.EventHandler(this.電池銲接程式ToolStripMenuItem_Click);
            // 
            // 控制程式ToolStripMenuItem
            // 
            this.控制程式ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.平台移動ToolStripMenuItem,
            this.雷射控制IO點ToolStripMenuItem});
            this.控制程式ToolStripMenuItem.Name = "控制程式ToolStripMenuItem";
            this.控制程式ToolStripMenuItem.Size = new System.Drawing.Size(85, 24);
            this.控制程式ToolStripMenuItem.Text = "控制程式";
            // 
            // 平台移動ToolStripMenuItem
            // 
            this.平台移動ToolStripMenuItem.Name = "平台移動ToolStripMenuItem";
            this.平台移動ToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.平台移動ToolStripMenuItem.Text = "平台移動";
            this.平台移動ToolStripMenuItem.Click += new System.EventHandler(this.平台移動ToolStripMenuItem_Click);
            // 
            // 雷射控制IO點ToolStripMenuItem
            // 
            this.雷射控制IO點ToolStripMenuItem.Name = "雷射控制IO點ToolStripMenuItem";
            this.雷射控制IO點ToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.雷射控制IO點ToolStripMenuItem.Text = "雷射控制(IO點)";
            this.雷射控制IO點ToolStripMenuItem.Click += new System.EventHandler(this.雷射控制IO點ToolStripMenuItem_Click);
            // 
            // 權限ToolStripMenuItem
            // 
            this.權限ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.登出ToolStripMenuItem});
            this.權限ToolStripMenuItem.Name = "權限ToolStripMenuItem";
            this.權限ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.權限ToolStripMenuItem.Text = "權限";
            // 
            // 登出ToolStripMenuItem
            // 
            this.登出ToolStripMenuItem.Name = "登出ToolStripMenuItem";
            this.登出ToolStripMenuItem.Size = new System.Drawing.Size(110, 24);
            this.登出ToolStripMenuItem.Text = "登出";
            this.登出ToolStripMenuItem.Click += new System.EventHandler(this.登出ToolStripMenuItem_Click_1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripXCurPosTxtBox,
            this.toolStripXHomeTxtBox,
            this.toolStripLabel2,
            this.toolStripYCurPosTxtBox,
            this.toolStripYHomeTxtBox,
            this.toolStripLabel3,
            this.toolStripZCurPosTxtBox,
            this.toolStripZHomeTxtBox,
            this.toolStripLabel4,
            this.toolStripTextBox4,
            this.toolStripTextBox1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 26);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1216, 33);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(19, 30);
            this.toolStripLabel1.Text = "X";
            // 
            // toolStripXCurPosTxtBox
            // 
            this.toolStripXCurPosTxtBox.BackColor = System.Drawing.Color.White;
            this.toolStripXCurPosTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripXCurPosTxtBox.Name = "toolStripXCurPosTxtBox";
            this.toolStripXCurPosTxtBox.ReadOnly = true;
            this.toolStripXCurPosTxtBox.Size = new System.Drawing.Size(100, 33);
            this.toolStripXCurPosTxtBox.Text = "0";
            // 
            // toolStripXHomeTxtBox
            // 
            this.toolStripXHomeTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripXHomeTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.toolStripXHomeTxtBox.Name = "toolStripXHomeTxtBox";
            this.toolStripXHomeTxtBox.ReadOnly = true;
            this.toolStripXHomeTxtBox.Size = new System.Drawing.Size(100, 33);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(19, 30);
            this.toolStripLabel2.Text = "Y";
            // 
            // toolStripYCurPosTxtBox
            // 
            this.toolStripYCurPosTxtBox.BackColor = System.Drawing.Color.White;
            this.toolStripYCurPosTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripYCurPosTxtBox.Name = "toolStripYCurPosTxtBox";
            this.toolStripYCurPosTxtBox.ReadOnly = true;
            this.toolStripYCurPosTxtBox.Size = new System.Drawing.Size(100, 33);
            this.toolStripYCurPosTxtBox.Text = "0";
            // 
            // toolStripYHomeTxtBox
            // 
            this.toolStripYHomeTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripYHomeTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.toolStripYHomeTxtBox.Name = "toolStripYHomeTxtBox";
            this.toolStripYHomeTxtBox.ReadOnly = true;
            this.toolStripYHomeTxtBox.Size = new System.Drawing.Size(100, 33);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(19, 30);
            this.toolStripLabel3.Text = "Z";
            // 
            // toolStripZCurPosTxtBox
            // 
            this.toolStripZCurPosTxtBox.BackColor = System.Drawing.Color.White;
            this.toolStripZCurPosTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripZCurPosTxtBox.Name = "toolStripZCurPosTxtBox";
            this.toolStripZCurPosTxtBox.ReadOnly = true;
            this.toolStripZCurPosTxtBox.Size = new System.Drawing.Size(100, 33);
            this.toolStripZCurPosTxtBox.Text = "0";
            // 
            // toolStripZHomeTxtBox
            // 
            this.toolStripZHomeTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripZHomeTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.toolStripZHomeTxtBox.Name = "toolStripZHomeTxtBox";
            this.toolStripZHomeTxtBox.ReadOnly = true;
            this.toolStripZHomeTxtBox.Size = new System.Drawing.Size(100, 33);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(45, 30);
            this.toolStripLabel4.Text = "登入:";
            // 
            // toolStripTextBox4
            // 
            this.toolStripTextBox4.BackColor = System.Drawing.Color.White;
            this.toolStripTextBox4.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripTextBox4.Name = "toolStripTextBox4";
            this.toolStripTextBox4.ReadOnly = true;
            this.toolStripTextBox4.Size = new System.Drawing.Size(68, 33);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BackColor = System.Drawing.Color.White;
            this.toolStripTextBox1.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.ReadOnly = true;
            this.toolStripTextBox1.Size = new System.Drawing.Size(68, 33);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel5,
            this.toolStripAlmTxtBox});
            this.toolStrip2.Location = new System.Drawing.Point(0, 59);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1216, 25);
            this.toolStrip2.TabIndex = 5;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(54, 22);
            this.toolStripLabel5.Text = "Alarm";
            // 
            // toolStripAlmTxtBox
            // 
            this.toolStripAlmTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripAlmTxtBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.toolStripAlmTxtBox.Name = "toolStripAlmTxtBox";
            this.toolStripAlmTxtBox.ReadOnly = true;
            this.toolStripAlmTxtBox.Size = new System.Drawing.Size(1100, 25);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1216, 707);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "0325_Tailift_MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ForeColorChanged += new System.EventHandler(this.MainForm_ForeColorChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 銲接主程式ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 電池銲接程式ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 控制程式ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 平台移動ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 雷射控制IO點ToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox toolStripXCurPosTxtBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox toolStripYCurPosTxtBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox toolStripZCurPosTxtBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        public System.Windows.Forms.ToolStripTextBox toolStripTextBox4;
        private System.Windows.Forms.ToolStripTextBox toolStripXHomeTxtBox;
        private System.Windows.Forms.ToolStripTextBox toolStripYHomeTxtBox;
        private System.Windows.Forms.ToolStripTextBox toolStripZHomeTxtBox;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripTextBox toolStripAlmTxtBox;
        public System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem 權限ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登出ToolStripMenuItem;
    }
}

