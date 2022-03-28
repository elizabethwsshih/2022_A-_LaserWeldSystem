namespace WeldControlSystem
{
    partial class FormStageControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStageControl));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.PlcModeRdioBox = new System.Windows.Forms.RadioButton();
            this.PCModeRdioBox = new System.Windows.Forms.RadioButton();
            this.ManualMoveGrpBox = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ManualNoStopRadioBtn = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.ManualStepDistComboBox = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ManualStepRadioBox = new System.Windows.Forms.RadioButton();
            this.ManualZSpeedTxtBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.GoHomeBtn = new System.Windows.Forms.Button();
            this.label70 = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.ManualXNegBtn = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.ManualZNegBtn = new System.Windows.Forms.Button();
            this.ManualYPosBtn = new System.Windows.Forms.Button();
            this.label62 = new System.Windows.Forms.Label();
            this.ManualXPosBtn = new System.Windows.Forms.Button();
            this.ManualYNegBtn = new System.Windows.Forms.Button();
            this.ManualZPosBtn = new System.Windows.Forms.Button();
            this.ManualXSpeedTxtBox = new System.Windows.Forms.TextBox();
            this.ManualYSpeedTxtBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.StageStopBtn = new System.Windows.Forms.Button();
            this.StageErrResetBtn = new System.Windows.Forms.Button();
            this.ManualAsnPosGrpBox = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ZAsnSpeedTxtBox = new System.Windows.Forms.TextBox();
            this.YAsnSpeedTxtBox = new System.Windows.Forms.TextBox();
            this.XAsnSpeedTxtBox = new System.Windows.Forms.TextBox();
            this.ManualZPosTxtBox = new System.Windows.Forms.TextBox();
            this.label59 = new System.Windows.Forms.Label();
            this.ManualAsnPosMoveBtn = new System.Windows.Forms.Button();
            this.label98 = new System.Windows.Forms.Label();
            this.label97 = new System.Windows.Forms.Label();
            this.ManualYPosTxtBox = new System.Windows.Forms.TextBox();
            this.ManualXPosTxtBox = new System.Windows.Forms.TextBox();
            this.ManualAsnPosRadioBtn = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3.SuspendLayout();
            this.ManualMoveGrpBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ManualAsnPosGrpBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.PlcModeRdioBox);
            this.groupBox3.Controls.Add(this.PCModeRdioBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(441, 59);
            this.groupBox3.TabIndex = 166;
            this.groupBox3.TabStop = false;
            // 
            // PlcModeRdioBox
            // 
            this.PlcModeRdioBox.AutoSize = true;
            this.PlcModeRdioBox.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.PlcModeRdioBox.Location = new System.Drawing.Point(7, 21);
            this.PlcModeRdioBox.Name = "PlcModeRdioBox";
            this.PlcModeRdioBox.Size = new System.Drawing.Size(138, 28);
            this.PlcModeRdioBox.TabIndex = 163;
            this.PlcModeRdioBox.Text = "PLC主控模式";
            this.PlcModeRdioBox.UseVisualStyleBackColor = true;
            this.PlcModeRdioBox.Click += new System.EventHandler(this.PlcModeRdioBox_Click);
            // 
            // PCModeRdioBox
            // 
            this.PCModeRdioBox.AutoSize = true;
            this.PCModeRdioBox.Checked = true;
            this.PCModeRdioBox.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.PCModeRdioBox.Location = new System.Drawing.Point(160, 21);
            this.PCModeRdioBox.Name = "PCModeRdioBox";
            this.PCModeRdioBox.Size = new System.Drawing.Size(128, 28);
            this.PCModeRdioBox.TabIndex = 164;
            this.PCModeRdioBox.TabStop = true;
            this.PCModeRdioBox.Text = "PC主控模式";
            this.PCModeRdioBox.UseVisualStyleBackColor = true;
            this.PCModeRdioBox.Click += new System.EventHandler(this.PCModeRdioBox_Click);
            // 
            // ManualMoveGrpBox
            // 
            this.ManualMoveGrpBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ManualMoveGrpBox.Controls.Add(this.label7);
            this.ManualMoveGrpBox.Controls.Add(this.label3);
            this.ManualMoveGrpBox.Controls.Add(this.ManualNoStopRadioBtn);
            this.ManualMoveGrpBox.Controls.Add(this.label2);
            this.ManualMoveGrpBox.Controls.Add(this.ManualStepDistComboBox);
            this.ManualMoveGrpBox.Controls.Add(this.label18);
            this.ManualMoveGrpBox.Controls.Add(this.label1);
            this.ManualMoveGrpBox.Controls.Add(this.ManualStepRadioBox);
            this.ManualMoveGrpBox.Controls.Add(this.ManualZSpeedTxtBox);
            this.ManualMoveGrpBox.Controls.Add(this.groupBox2);
            this.ManualMoveGrpBox.Controls.Add(this.ManualXSpeedTxtBox);
            this.ManualMoveGrpBox.Controls.Add(this.ManualYSpeedTxtBox);
            this.ManualMoveGrpBox.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualMoveGrpBox.Location = new System.Drawing.Point(12, 77);
            this.ManualMoveGrpBox.Name = "ManualMoveGrpBox";
            this.ManualMoveGrpBox.Size = new System.Drawing.Size(441, 340);
            this.ManualMoveGrpBox.TabIndex = 167;
            this.ManualMoveGrpBox.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(60, 283);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 20);
            this.label7.TabIndex = 172;
            this.label7.Text = "(mm/min)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(4, 254);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 171;
            this.label3.Text = "Z速度";
            // 
            // ManualNoStopRadioBtn
            // 
            this.ManualNoStopRadioBtn.AutoSize = true;
            this.ManualNoStopRadioBtn.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualNoStopRadioBtn.Location = new System.Drawing.Point(7, 117);
            this.ManualNoStopRadioBtn.Name = "ManualNoStopRadioBtn";
            this.ManualNoStopRadioBtn.Size = new System.Drawing.Size(104, 28);
            this.ManualNoStopRadioBtn.TabIndex = 167;
            this.ManualNoStopRadioBtn.Text = "連續移動";
            this.ManualNoStopRadioBtn.UseVisualStyleBackColor = true;
            this.ManualNoStopRadioBtn.Click += new System.EventHandler(this.ManualNoStopRadioBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(5, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 170;
            this.label2.Text = "Y速度";
            // 
            // ManualStepDistComboBox
            // 
            this.ManualStepDistComboBox.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualStepDistComboBox.FormattingEnabled = true;
            this.ManualStepDistComboBox.Items.AddRange(new object[] {
            "0.001",
            "0.002",
            "0.005",
            "0.01",
            "0.02",
            "0.05",
            "0.1",
            "0.2",
            "0.5",
            "1",
            "2",
            "5",
            "10"});
            this.ManualStepDistComboBox.Location = new System.Drawing.Point(23, 71);
            this.ManualStepDistComboBox.Name = "ManualStepDistComboBox";
            this.ManualStepDistComboBox.Size = new System.Drawing.Size(80, 32);
            this.ManualStepDistComboBox.TabIndex = 166;
            this.ManualStepDistComboBox.Text = "0.005";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label18.Location = new System.Drawing.Point(105, 80);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(39, 20);
            this.label18.TabIndex = 165;
            this.label18.Text = "mm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(5, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 155;
            this.label1.Text = "X速度";
            // 
            // ManualStepRadioBox
            // 
            this.ManualStepRadioBox.AutoSize = true;
            this.ManualStepRadioBox.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualStepRadioBox.Location = new System.Drawing.Point(7, 38);
            this.ManualStepRadioBox.Name = "ManualStepRadioBox";
            this.ManualStepRadioBox.Size = new System.Drawing.Size(142, 28);
            this.ManualStepRadioBox.TabIndex = 164;
            this.ManualStepRadioBox.Text = "步進吋動距離";
            this.ManualStepRadioBox.UseVisualStyleBackColor = true;
            this.ManualStepRadioBox.Click += new System.EventHandler(this.ManualStepRadioBox_Click);
            // 
            // ManualZSpeedTxtBox
            // 
            this.ManualZSpeedTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualZSpeedTxtBox.Location = new System.Drawing.Point(62, 246);
            this.ManualZSpeedTxtBox.Name = "ManualZSpeedTxtBox";
            this.ManualZSpeedTxtBox.Size = new System.Drawing.Size(83, 34);
            this.ManualZSpeedTxtBox.TabIndex = 169;
            this.ManualZSpeedTxtBox.Text = "1000";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.GoHomeBtn);
            this.groupBox2.Controls.Add(this.label70);
            this.groupBox2.Controls.Add(this.label71);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label65);
            this.groupBox2.Controls.Add(this.ManualXNegBtn);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.ManualZNegBtn);
            this.groupBox2.Controls.Add(this.ManualYPosBtn);
            this.groupBox2.Controls.Add(this.label62);
            this.groupBox2.Controls.Add(this.ManualXPosBtn);
            this.groupBox2.Controls.Add(this.ManualYNegBtn);
            this.groupBox2.Controls.Add(this.ManualZPosBtn);
            this.groupBox2.Location = new System.Drawing.Point(159, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 329);
            this.groupBox2.TabIndex = 163;
            this.groupBox2.TabStop = false;
            // 
            // GoHomeBtn
            // 
            this.GoHomeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.GoHomeBtn.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.GoHomeBtn.ForeColor = System.Drawing.Color.White;
            this.GoHomeBtn.Location = new System.Drawing.Point(96, 127);
            this.GoHomeBtn.Name = "GoHomeBtn";
            this.GoHomeBtn.Size = new System.Drawing.Size(77, 80);
            this.GoHomeBtn.TabIndex = 142;
            this.GoHomeBtn.Text = "HOME";
            this.GoHomeBtn.UseVisualStyleBackColor = false;
            this.GoHomeBtn.Click += new System.EventHandler(this.GoHomeBtn_Click);
            this.GoHomeBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GoHomeBtn_MouseDown);
            this.GoHomeBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GoHomeBtn_MouseUp);
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.BackColor = System.Drawing.Color.Transparent;
            this.label70.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label70.Location = new System.Drawing.Point(17, 307);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(61, 20);
            this.label70.TabIndex = 153;
            this.label70.Text = "Y向(負)";
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.BackColor = System.Drawing.Color.Transparent;
            this.label71.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label71.Location = new System.Drawing.Point(185, 19);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(61, 20);
            this.label71.TabIndex = 154;
            this.label71.Text = "Y向(正)";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label19.Location = new System.Drawing.Point(185, 209);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(61, 20);
            this.label19.TabIndex = 149;
            this.label19.Text = "X向(正)";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.BackColor = System.Drawing.Color.Transparent;
            this.label65.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label65.Location = new System.Drawing.Point(96, 18);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(61, 20);
            this.label65.TabIndex = 152;
            this.label65.Text = "Z向(正)";
            // 
            // ManualXNegBtn
            // 
            this.ManualXNegBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ManualXNegBtn.BackgroundImage")));
            this.ManualXNegBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ManualXNegBtn.Location = new System.Drawing.Point(13, 127);
            this.ManualXNegBtn.Name = "ManualXNegBtn";
            this.ManualXNegBtn.Size = new System.Drawing.Size(77, 80);
            this.ManualXNegBtn.TabIndex = 17;
            this.ManualXNegBtn.UseVisualStyleBackColor = true;
            this.ManualXNegBtn.Click += new System.EventHandler(this.ManualXNegBtn_Click);
            this.ManualXNegBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ManualXNegBtn_MouseDown);
            this.ManualXNegBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ManualXNegBtn_MouseUp);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label23.Location = new System.Drawing.Point(13, 104);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(61, 20);
            this.label23.TabIndex = 150;
            this.label23.Text = "X向(負)";
            // 
            // ManualZNegBtn
            // 
            this.ManualZNegBtn.BackColor = System.Drawing.Color.Gainsboro;
            this.ManualZNegBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ManualZNegBtn.BackgroundImage")));
            this.ManualZNegBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ManualZNegBtn.Location = new System.Drawing.Point(96, 41);
            this.ManualZNegBtn.Name = "ManualZNegBtn";
            this.ManualZNegBtn.Size = new System.Drawing.Size(77, 80);
            this.ManualZNegBtn.TabIndex = 148;
            this.ManualZNegBtn.UseVisualStyleBackColor = false;
            this.ManualZNegBtn.Click += new System.EventHandler(this.ManualZNegBtn_Click);
            this.ManualZNegBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ManualZNegBtn_MouseDown);
            this.ManualZNegBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ManualZNegBtn_MouseUp);
            // 
            // ManualYPosBtn
            // 
            this.ManualYPosBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ManualYPosBtn.BackgroundImage")));
            this.ManualYPosBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ManualYPosBtn.Location = new System.Drawing.Point(185, 38);
            this.ManualYPosBtn.Name = "ManualYPosBtn";
            this.ManualYPosBtn.Size = new System.Drawing.Size(77, 80);
            this.ManualYPosBtn.TabIndex = 15;
            this.ManualYPosBtn.UseVisualStyleBackColor = true;
            this.ManualYPosBtn.Click += new System.EventHandler(this.ManualYPosBtn_Click);
            this.ManualYPosBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ManualYPosBtn_MouseDown);
            this.ManualYPosBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ManualYPosBtn_MouseUp);
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.BackColor = System.Drawing.Color.Transparent;
            this.label62.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label62.Location = new System.Drawing.Point(96, 307);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(61, 20);
            this.label62.TabIndex = 151;
            this.label62.Text = "Z向(負)";
            // 
            // ManualXPosBtn
            // 
            this.ManualXPosBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ManualXPosBtn.BackgroundImage")));
            this.ManualXPosBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ManualXPosBtn.Location = new System.Drawing.Point(185, 124);
            this.ManualXPosBtn.Name = "ManualXPosBtn";
            this.ManualXPosBtn.Size = new System.Drawing.Size(77, 80);
            this.ManualXPosBtn.TabIndex = 16;
            this.ManualXPosBtn.UseVisualStyleBackColor = true;
            this.ManualXPosBtn.Click += new System.EventHandler(this.ManualXPosBtn_Click);
            this.ManualXPosBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ManualXPosBtn_MouseDown);
            this.ManualXPosBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ManualXPosBtn_MouseUp);
            // 
            // ManualYNegBtn
            // 
            this.ManualYNegBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ManualYNegBtn.BackgroundImage")));
            this.ManualYNegBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ManualYNegBtn.Location = new System.Drawing.Point(17, 224);
            this.ManualYNegBtn.Name = "ManualYNegBtn";
            this.ManualYNegBtn.Size = new System.Drawing.Size(77, 80);
            this.ManualYNegBtn.TabIndex = 18;
            this.ManualYNegBtn.UseVisualStyleBackColor = true;
            this.ManualYNegBtn.Click += new System.EventHandler(this.ManualYNegBtn_Click);
            this.ManualYNegBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ManualYNegBtn_MouseDown);
            this.ManualYNegBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ManualYNegBtn_MouseUp);
            // 
            // ManualZPosBtn
            // 
            this.ManualZPosBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ManualZPosBtn.BackgroundImage")));
            this.ManualZPosBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ManualZPosBtn.Location = new System.Drawing.Point(100, 224);
            this.ManualZPosBtn.Name = "ManualZPosBtn";
            this.ManualZPosBtn.Size = new System.Drawing.Size(77, 80);
            this.ManualZPosBtn.TabIndex = 147;
            this.ManualZPosBtn.UseVisualStyleBackColor = true;
            this.ManualZPosBtn.Click += new System.EventHandler(this.ManualZPosBtn_Click);
            this.ManualZPosBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ManualZPosBtn_MouseDown);
            this.ManualZPosBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ManualZPosBtn_MouseUp);
            // 
            // ManualXSpeedTxtBox
            // 
            this.ManualXSpeedTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualXSpeedTxtBox.Location = new System.Drawing.Point(62, 166);
            this.ManualXSpeedTxtBox.Name = "ManualXSpeedTxtBox";
            this.ManualXSpeedTxtBox.Size = new System.Drawing.Size(82, 34);
            this.ManualXSpeedTxtBox.TabIndex = 162;
            this.ManualXSpeedTxtBox.Text = "1000";
            // 
            // ManualYSpeedTxtBox
            // 
            this.ManualYSpeedTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualYSpeedTxtBox.Location = new System.Drawing.Point(62, 206);
            this.ManualYSpeedTxtBox.Name = "ManualYSpeedTxtBox";
            this.ManualYSpeedTxtBox.Size = new System.Drawing.Size(82, 34);
            this.ManualYSpeedTxtBox.TabIndex = 168;
            this.ManualYSpeedTxtBox.Text = "1000";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Yellow;
            this.button1.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(18, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 80);
            this.button1.TabIndex = 179;
            this.button1.Text = "Alarm解除";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // StageStopBtn
            // 
            this.StageStopBtn.BackColor = System.Drawing.Color.Maroon;
            this.StageStopBtn.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.StageStopBtn.ForeColor = System.Drawing.Color.White;
            this.StageStopBtn.Location = new System.Drawing.Point(99, 14);
            this.StageStopBtn.Name = "StageStopBtn";
            this.StageStopBtn.Size = new System.Drawing.Size(77, 80);
            this.StageStopBtn.TabIndex = 155;
            this.StageStopBtn.Text = "Stop";
            this.StageStopBtn.UseVisualStyleBackColor = false;
            this.StageStopBtn.Click += new System.EventHandler(this.StageStopBtn_Click);
            // 
            // StageErrResetBtn
            // 
            this.StageErrResetBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.StageErrResetBtn.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.StageErrResetBtn.ForeColor = System.Drawing.Color.Black;
            this.StageErrResetBtn.Location = new System.Drawing.Point(182, 14);
            this.StageErrResetBtn.Name = "StageErrResetBtn";
            this.StageErrResetBtn.Size = new System.Drawing.Size(75, 80);
            this.StageErrResetBtn.TabIndex = 178;
            this.StageErrResetBtn.Text = "急停\r\n復歸";
            this.StageErrResetBtn.UseVisualStyleBackColor = false;
            this.StageErrResetBtn.Click += new System.EventHandler(this.ErrResetBtn_Click);
            // 
            // ManualAsnPosGrpBox
            // 
            this.ManualAsnPosGrpBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ManualAsnPosGrpBox.Controls.Add(this.label8);
            this.ManualAsnPosGrpBox.Controls.Add(this.label4);
            this.ManualAsnPosGrpBox.Controls.Add(this.label5);
            this.ManualAsnPosGrpBox.Controls.Add(this.label6);
            this.ManualAsnPosGrpBox.Controls.Add(this.ZAsnSpeedTxtBox);
            this.ManualAsnPosGrpBox.Controls.Add(this.YAsnSpeedTxtBox);
            this.ManualAsnPosGrpBox.Controls.Add(this.XAsnSpeedTxtBox);
            this.ManualAsnPosGrpBox.Controls.Add(this.ManualZPosTxtBox);
            this.ManualAsnPosGrpBox.Controls.Add(this.label59);
            this.ManualAsnPosGrpBox.Controls.Add(this.ManualAsnPosMoveBtn);
            this.ManualAsnPosGrpBox.Controls.Add(this.label98);
            this.ManualAsnPosGrpBox.Controls.Add(this.label97);
            this.ManualAsnPosGrpBox.Controls.Add(this.ManualYPosTxtBox);
            this.ManualAsnPosGrpBox.Controls.Add(this.ManualXPosTxtBox);
            this.ManualAsnPosGrpBox.Controls.Add(this.ManualAsnPosRadioBtn);
            this.ManualAsnPosGrpBox.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualAsnPosGrpBox.Location = new System.Drawing.Point(459, 12);
            this.ManualAsnPosGrpBox.Name = "ManualAsnPosGrpBox";
            this.ManualAsnPosGrpBox.Size = new System.Drawing.Size(199, 401);
            this.ManualAsnPosGrpBox.TabIndex = 168;
            this.ManualAsnPosGrpBox.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(60, 337);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 20);
            this.label8.TabIndex = 173;
            this.label8.Text = "(mm/min)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(5, 302);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 20);
            this.label4.TabIndex = 177;
            this.label4.Text = "Z速度";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(6, 266);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 20);
            this.label5.TabIndex = 176;
            this.label5.Text = "Y速度";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(6, 228);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 20);
            this.label6.TabIndex = 172;
            this.label6.Text = "X速度";
            // 
            // ZAsnSpeedTxtBox
            // 
            this.ZAsnSpeedTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ZAsnSpeedTxtBox.Location = new System.Drawing.Point(62, 297);
            this.ZAsnSpeedTxtBox.Name = "ZAsnSpeedTxtBox";
            this.ZAsnSpeedTxtBox.Size = new System.Drawing.Size(83, 34);
            this.ZAsnSpeedTxtBox.TabIndex = 175;
            this.ZAsnSpeedTxtBox.Text = "1000";
            // 
            // YAsnSpeedTxtBox
            // 
            this.YAsnSpeedTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.YAsnSpeedTxtBox.Location = new System.Drawing.Point(62, 259);
            this.YAsnSpeedTxtBox.Name = "YAsnSpeedTxtBox";
            this.YAsnSpeedTxtBox.Size = new System.Drawing.Size(82, 34);
            this.YAsnSpeedTxtBox.TabIndex = 174;
            this.YAsnSpeedTxtBox.Text = "2000";
            // 
            // XAsnSpeedTxtBox
            // 
            this.XAsnSpeedTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.XAsnSpeedTxtBox.Location = new System.Drawing.Point(62, 221);
            this.XAsnSpeedTxtBox.Name = "XAsnSpeedTxtBox";
            this.XAsnSpeedTxtBox.Size = new System.Drawing.Size(82, 34);
            this.XAsnSpeedTxtBox.TabIndex = 173;
            this.XAsnSpeedTxtBox.Text = "3000";
            // 
            // ManualZPosTxtBox
            // 
            this.ManualZPosTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualZPosTxtBox.Location = new System.Drawing.Point(61, 141);
            this.ManualZPosTxtBox.Name = "ManualZPosTxtBox";
            this.ManualZPosTxtBox.Size = new System.Drawing.Size(121, 33);
            this.ManualZPosTxtBox.TabIndex = 141;
            this.ManualZPosTxtBox.Text = "-96";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label59.Location = new System.Drawing.Point(6, 144);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(50, 24);
            this.label59.TabIndex = 140;
            this.label59.Text = "Z軸 :";
            // 
            // ManualAsnPosMoveBtn
            // 
            this.ManualAsnPosMoveBtn.BackColor = System.Drawing.Color.SteelBlue;
            this.ManualAsnPosMoveBtn.Font = new System.Drawing.Font("Microsoft JhengHei", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualAsnPosMoveBtn.Location = new System.Drawing.Point(61, 179);
            this.ManualAsnPosMoveBtn.Name = "ManualAsnPosMoveBtn";
            this.ManualAsnPosMoveBtn.Size = new System.Drawing.Size(121, 39);
            this.ManualAsnPosMoveBtn.TabIndex = 139;
            this.ManualAsnPosMoveBtn.Text = "移動";
            this.ManualAsnPosMoveBtn.UseVisualStyleBackColor = false;
            this.ManualAsnPosMoveBtn.Click += new System.EventHandler(this.ManualAsnPosMoveBtn_Click);
            // 
            // label98
            // 
            this.label98.AutoSize = true;
            this.label98.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label98.Location = new System.Drawing.Point(6, 107);
            this.label98.Name = "label98";
            this.label98.Size = new System.Drawing.Size(49, 24);
            this.label98.TabIndex = 25;
            this.label98.Text = "Y軸 :";
            // 
            // label97
            // 
            this.label97.AutoSize = true;
            this.label97.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label97.Location = new System.Drawing.Point(5, 72);
            this.label97.Name = "label97";
            this.label97.Size = new System.Drawing.Size(50, 24);
            this.label97.TabIndex = 24;
            this.label97.Text = "X軸 :";
            // 
            // ManualYPosTxtBox
            // 
            this.ManualYPosTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualYPosTxtBox.Location = new System.Drawing.Point(61, 102);
            this.ManualYPosTxtBox.Name = "ManualYPosTxtBox";
            this.ManualYPosTxtBox.Size = new System.Drawing.Size(121, 33);
            this.ManualYPosTxtBox.TabIndex = 23;
            this.ManualYPosTxtBox.Text = "193";
            // 
            // ManualXPosTxtBox
            // 
            this.ManualXPosTxtBox.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualXPosTxtBox.Location = new System.Drawing.Point(62, 65);
            this.ManualXPosTxtBox.Name = "ManualXPosTxtBox";
            this.ManualXPosTxtBox.Size = new System.Drawing.Size(121, 33);
            this.ManualXPosTxtBox.TabIndex = 22;
            this.ManualXPosTxtBox.Text = "141";
            // 
            // ManualAsnPosRadioBtn
            // 
            this.ManualAsnPosRadioBtn.AutoSize = true;
            this.ManualAsnPosRadioBtn.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ManualAsnPosRadioBtn.Location = new System.Drawing.Point(10, 31);
            this.ManualAsnPosRadioBtn.Name = "ManualAsnPosRadioBtn";
            this.ManualAsnPosRadioBtn.Size = new System.Drawing.Size(104, 28);
            this.ManualAsnPosRadioBtn.TabIndex = 21;
            this.ManualAsnPosRadioBtn.Text = "指定座標";
            this.ManualAsnPosRadioBtn.UseVisualStyleBackColor = true;
            this.ManualAsnPosRadioBtn.Click += new System.EventHandler(this.ManualAsnPosRadioBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.StageErrResetBtn);
            this.groupBox1.Controls.Add(this.StageStopBtn);
            this.groupBox1.Location = new System.Drawing.Point(172, 423);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 100);
            this.groupBox1.TabIndex = 179;
            this.groupBox1.TabStop = false;
            // 
            // FormStageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 532);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ManualAsnPosGrpBox);
            this.Controls.Add(this.ManualMoveGrpBox);
            this.Controls.Add(this.groupBox3);
            this.Name = "FormStageControl";
            this.Text = "FormStageControl";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormStageControl_FormClosed);
            this.Load += new System.EventHandler(this.FormStageControl_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ManualMoveGrpBox.ResumeLayout(false);
            this.ManualMoveGrpBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ManualAsnPosGrpBox.ResumeLayout(false);
            this.ManualAsnPosGrpBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton PlcModeRdioBox;
        private System.Windows.Forms.RadioButton PCModeRdioBox;
        private System.Windows.Forms.GroupBox ManualMoveGrpBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton ManualNoStopRadioBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ManualStepDistComboBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton ManualStepRadioBox;
        private System.Windows.Forms.TextBox ManualZSpeedTxtBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button GoHomeBtn;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Button ManualXNegBtn;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button ManualZNegBtn;
        private System.Windows.Forms.Button ManualYPosBtn;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Button ManualXPosBtn;
        private System.Windows.Forms.Button ManualYNegBtn;
        private System.Windows.Forms.Button ManualZPosBtn;
        private System.Windows.Forms.TextBox ManualXSpeedTxtBox;
        private System.Windows.Forms.TextBox ManualYSpeedTxtBox;
        private System.Windows.Forms.GroupBox ManualAsnPosGrpBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ZAsnSpeedTxtBox;
        private System.Windows.Forms.TextBox YAsnSpeedTxtBox;
        private System.Windows.Forms.TextBox XAsnSpeedTxtBox;
        private System.Windows.Forms.TextBox ManualZPosTxtBox;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Button ManualAsnPosMoveBtn;
        private System.Windows.Forms.Label label98;
        private System.Windows.Forms.Label label97;
        private System.Windows.Forms.TextBox ManualYPosTxtBox;
        private System.Windows.Forms.TextBox ManualXPosTxtBox;
        private System.Windows.Forms.RadioButton ManualAsnPosRadioBtn;
        private System.Windows.Forms.Button StageStopBtn;
        private System.Windows.Forms.Button StageErrResetBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}