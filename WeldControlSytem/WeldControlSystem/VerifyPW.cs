using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WeldControlSystem
{
    public partial class VerifyPW : Form
    {
       
        FileIO FileIO = new FileIO();
        public string UserPW;
        public string UserAccount;
        public string UserCategory;
        public int VP_result = 0; //新版:登入是否成功
        private string _PWStr;


        public string PWStr
        {
            set { _PWStr = value; }
            get { return _PWStr; }
        }
        public VerifyPW()
        {
            InitializeComponent();
            this.PW_OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.PW_CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void PW_OKBtn_Click(object sender, EventArgs e)
        {
            string SettingFilePath = @"D://MemberControl//MemberControl.csv";
            VP_result = FileIO.GetMemberPW(SettingFilePath, User_TxtBox.Text, PW_TxtBox.Text, out UserCategory);
            if (VP_result == 1)
            {
                 UserAccount = User_TxtBox.Text;
                 UserPW = PW_TxtBox.Text;

                 MainForm lForm1 = (MainForm)this.Owner;
                 lForm1.toolStripTextBox4.Text = UserAccount;
                 lForm1.toolStripTextBox1.Text = UserCategory;
                 lForm1.UserCategory = UserCategory;
            }
            else if (VP_result == 2)
            {
                MessageBox.Show(this, "您可能正在編輯Member.csv檔,請關閉!");
                UserAccount = "ERROR";
                UserPW = "ERROR";
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "帳號或密碼輸入錯誤!");
                UserAccount = "ERROR";
                UserPW = "ERROR";
                this.Close();
            }

        }

        private void PW_CancelBtn_Click(object sender, EventArgs e)
        {
            
        }

        private void PW_TxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               PW_OKBtn_Click(this,null);
            }
        }

        private void VerifyPW_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Form1 lForm1 = (Form1)this.Owner;
            //lForm1._PWStr = "CLOSE";
        }

        private void VerifyPW_Load(object sender, EventArgs e)
        {
          
        }

        private void VerifyPW_Shown(object sender, EventArgs e)
        {

            User_TxtBox.Select();
        }

        private void User_TxtBox_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void User_TxtBox_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
