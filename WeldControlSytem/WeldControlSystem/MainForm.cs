using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace WeldControlSystem
{

    public partial class MainForm : Form
    {
        VerifyPW VP = new VerifyPW();
        PLCAction PLCAction = new PLCAction();
        double XCurPos, YCurPos, ZCurPos;
        int XGoHomeStatus, YGoHomeStatus, ZGoHomeStatus;
        System.Threading.Timer timer1; //聽 位置
        System.Threading.Timer timer2; //聽 Home
        public string UserCategory = "";
        //Thread ListenPLCThread;

        string AlmMsg = "";

        double[] MainPLCReadVal = new double[14];
        int[] MainPLCReadXYval = new int[95];


        double[] testPLCReadVal = new double[1];


        //ui
        FormStageControl StageControlForm = new FormStageControl();
        FormLaser LaserForm = new FormLaser("3");
        WeldSystem WeldForm = new WeldSystem();
        PLCAlarm PLCAlm = new PLCAlarm();

        //鎖定flag
        int HomeFlag = -1;

        //alm msg
        string HomeMsg = "";

        //---------------MDI傳直-----------------
        string LoginAccount;
        public string ValAccount
        {
            set { LoginAccount = value; }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void 登入登出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAccount AcountForm = new FormAccount();
            AcountForm.MdiParent = this;
            AcountForm.Show();
        }

        private void 電池銲接程式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Threading.Thread thh;
            thh = new Thread(new ThreadStart(delegate()
            {
                FrmLaserWait f = new FrmLaserWait();
                f.ShowDialog();
            }));
            thh.Start();

            if (HomeFlag == 0)
            {
                MessageBox.Show("需要賦歸");

            }
            //if (MainPLCReadXYval[7] == 1)// || AlmMsg != "") // X07== LASER ERR
            //{
            //    MessageBox.Show("記得 Reset 雷射相關 Error!");
            //}
            //else
            //{

                //防重開寫法
                bool isFind = false;
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Name == "WeldSystem")
                    {
                        isFind = true;
                        form.MdiParent = this;
                        form.Focus();
                    }
                }

                if (isFind == false)
                {
                    WeldForm = new WeldSystem();
                    WeldForm.MdiParent = this;
                    WeldForm.ValUserCategoryControl = UserCategory;
                    //by user power control 
                    //if (UserCategory == "OP")
                    //{
                    //    this.tabPage2.Parent = null;
                    //}
                    //else if (UserCategory == "ENG")
                    //{
                    //    this.tabPage1.Parent = ControlTabBox;
                    //    this.tabPage2.Parent = ControlTabBox;

                    //}
                    WeldForm.Show();

                }
                thh.Abort();

            //}
        }

        private void 平台移動ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (toolStripTextBox1.Text!= "ENG") 
            //{
            //    MessageBox.Show("非工程師無法開啟!");
            //}
            //else
            //{
            //防重開寫法
            bool isFind = false;
            foreach (Form form in this.MdiChildren)
            {
                if (form.Name == "FormStageControl")
                {
                    isFind = true;
                    form.MdiParent = this;
                    form.Focus();
                }
            }

            if (isFind == false)
            {
                StageControlForm = new FormStageControl();
                StageControlForm.MdiParent = this;
                StageControlForm.Show();
            }
            //}
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //1. 通訊
            PLCAction.PLC_Connect(0);

            //2.主控權PLC FOR DEBUG
            PLCAction.MainControl(0);//1:pc,0:plc



            //2.初始化:清0 + err reset + 重啟伺服馬達
            PLCAction.PLC_AllInitial();



            //2.主控權PLC FOR DEBUG
            PLCAction.MainControl(1);//1:pc,0:plc

            PLCAction.PLC_StageMotor(1);

            //先停止交握,出機再開
            //A+好像沒設定這個
            //PLCAction.PLC_HandShaking(0);




            //4.聽PLC訊號
            TimerCallback callback1 = new TimerCallback(_listen);
            timer1 = new System.Threading.Timer(callback1, null, 0, 200);//250豪秒起來一次

            //ListenPLCThread = new Thread(new ThreadStart(ListenPLC));
            //Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
            //ListenPLCThread.Start();


            TimerCallback callback2 = new TimerCallback(_UIupdate);
            timer2 = new System.Threading.Timer(callback2, null, 0, 300);//250豪秒起來一次

            //5.UI處理



        }


        private void _UIupdate(object state)
        {

            this.BeginInvoke(new updateUI(UIupdate));//委派

        }

        delegate void updateUI();

        //private void _ListenPLC(object state)
        //{
        //    this.BeginInvoke(new PLCListen(ListenPLC));//委派
        //}
        //delegate void PLCListen();

        private static int num = 0;

        //private void ListenPLC()
        private void _listen(object state)
        {

            this.BeginInvoke(new Listen2(Listen3));//委派

        }
        delegate void Listen2();
        private void Listen3()
        {
            DateTime t1 = DateTime.Now;
            //int t = ++num;
            //Console.WriteLine("t1=" + t + " ,time=" + DateTime.Now.ToString());
            //MainPLCReadVal = new double[12];
            //MainPLCReadVal = PLCAction.ReadPLCDataRandom("D1000\nD1001\nD1010\nD1011\nD1020\nD1021\n"//1-2-3  (6)--MainForm & StageForm--
            //                                              + "M1107\nM1117\nM1127\n"//4-5-6 --MainForm & StageForm--(9)
            //                                              + "M1205\nM1215\nM1206\nM1216\n" //7-8-9-10 --Laser & Welding--(13)
            //                                              + "M1138\nM1139\nM1202\nM1204", 17); //11-12-13-14-- Welding--(16)

            //A+
            MainPLCReadVal = PLCAction.ReadPLCDataRandom("D1000\nD1001\nD1010\nD1011\nD1020\nD1021\n"//1-2-3  (6)--MainForm & StageForm--
                                                          + "M1107\nM1117\nM1127\n"//4-5-6 --MainForm & StageForm--(9)
                                                          + "M1017\nM1215\nM1018\nM1156\n" //7-8-9-10 --Laser & Welding--(13)
                                                          + "M1138\nM1139\nM1241\nM1006", 17); //11-12-13-14-- Welding--(16)

            //A+ 20211220 D碼
            MainPLCReadVal = PLCAction.ReadPLCDataRandom("D1048\nD1049\nD1052\nD1053\nD1060\nD1061\n"//1-2-3  (6)--MainForm & StageForm--
                                                       + "M1107\nM1117\nM1127\n"//4-5-6 --MainForm & StageForm--(9)
                                                       + "M1017\nM1215\nM1018\nM1156\n" //7-8-9-10 --Laser & Welding--(13)
                                                       + "M1138\nM1139\nM1241\nM1006", 17); //11-12-13-14-- Welding--(16)


            //testPLCReadVal = PLCAction.ReadPLCDataRandom("D1048\nD1049", 2); //11-12-13-14-- Welding--(16)

            //Console.WriteLine("t2=" + t + "start io ReadDeviceRandom" + " ,time=" + DateTime.Now.ToString());

            //因為有X0D,D會被上面函示誤判    
            //MainPLCReadXYval = new int[23];
            PLCAction.axActUtlType1.ReadDeviceRandom("X100\nX101\nX102\nX103\nX104\nX105\nX106\nX107\nX108\nX10D\nX10E\n" +  //0~10
                                                     "Y104\nY105\nY108\nY109\nY10A\nY10B\nY10C\nY10E\nY10D\n" + //11~19 (20)
                                                     "F85\n" +//21
                                                     "F71\nF72\nF73\nF74\nF76\nF77\nF78\nF79\nF80\nF81\nF82\nF83\nF84\n" +//33
                                                     "F1\nF2\nF23\nF24\nF25\nF26\nF31\nF32\nF33\n" +//41
                                                     "F400\nF401\nF402\nF403\nF404\nF405\nF406\nF407\nF408\nF409\nF411\nF412\nF420\n" +//54
                                                     "F421\nF422\nF423\nF424\nF425\nF426\nF427\nF428\nF429\nF430\nF431\nF432\nF440\n" +//67
                                                     "F441\nF442\nF443\nF444\nF445\nF446\nF447\nF448\nF449\nF450\nF451\nF452\nF460\n" +//80
                                                     "F461\nF462\nF463\nF464\nF465\nF466\nF467\nF468\nF469\nF470\nF471\nF472\nF9\n"//93
                                                   , 95, out  MainPLCReadXYval[0]);
            //Thread.Sleep(200);
            //Console.WriteLine("t3=" + t + "end" + " ,time=" + DateTime.Now.ToString());
            DateTime t2 = DateTime.Now;
            TimeSpan ts = t2 - t1;
            if (ts.TotalMilliseconds > 300)
                Console.WriteLine(DateTime.Now.ToShortTimeString() + "ts=" + Convert.ToDouble(ts.TotalMilliseconds.ToString()));


        }
        private void UIupdate()
        {
            //// hardcode 上面順序比較節省尋找時間
            XCurPos = Math.Round(MainPLCReadVal[0], 5); //D1000
            YCurPos = Math.Round(MainPLCReadVal[1], 5); //D1010
            ZCurPos = Math.Round(MainPLCReadVal[2], 5); //D1020

            double testXPos = testPLCReadVal[0];

            //SET VALUE
            //----------------stage--------------
            StageControlForm.ValXCurPos = XCurPos;
            StageControlForm.ValYCurPos = YCurPos;
            StageControlForm.ValZCurPos = ZCurPos;
            //----------------laser--------------
            LaserForm.ValM1205 = Convert.ToInt32(MainPLCReadVal[6]);
            LaserForm.ValM1215 = Convert.ToInt32(MainPLCReadVal[7]);
            LaserForm.ValM1206 = Convert.ToInt32(MainPLCReadVal[8]);
            LaserForm.ValM1216 = Convert.ToInt32(MainPLCReadVal[9]);
            //----------------weld--------------
            WeldForm.ValM1138 = Convert.ToInt32(MainPLCReadVal[10]);
            WeldForm.ValM1139 = Convert.ToInt32(MainPLCReadVal[11]);
            WeldForm.ValX0E = MainPLCReadXYval[10];
            WeldForm.ValM1204 = Convert.ToInt32(MainPLCReadVal[13]);
            //WeldForm.ValM1202 = Convert.ToInt32(MainPLCReadVal[12]);
            //----------------laser--------------
            LaserForm.ValX00 = MainPLCReadXYval[0];
            LaserForm.ValX01 = MainPLCReadXYval[1];
            LaserForm.ValX02 = MainPLCReadXYval[2];
            LaserForm.ValX03 = MainPLCReadXYval[3];
            LaserForm.ValX04 = MainPLCReadXYval[4];
            LaserForm.ValX05 = MainPLCReadXYval[5];
            LaserForm.ValX06 = MainPLCReadXYval[6];
            LaserForm.ValX07 = MainPLCReadXYval[7];
            LaserForm.ValX08 = MainPLCReadXYval[8];
            LaserForm.ValX0D = MainPLCReadXYval[9];
            LaserForm.ValX0E = MainPLCReadXYval[10];

            LaserForm.ValY04 = MainPLCReadXYval[11];
            LaserForm.ValY05 = MainPLCReadXYval[12];
            LaserForm.ValY08 = MainPLCReadXYval[13];
            LaserForm.ValY09 = MainPLCReadXYval[14];
            LaserForm.ValY0A = MainPLCReadXYval[15];
            LaserForm.ValY0B = MainPLCReadXYval[16];
            LaserForm.ValY0C = MainPLCReadXYval[17];
            LaserForm.ValY0E = MainPLCReadXYval[18];
            LaserForm.ValY0D = MainPLCReadXYval[19];
            //----------------ALM------------------------------
            XGoHomeStatus = Convert.ToInt32(MainPLCReadVal[3]); //M1107
            YGoHomeStatus = Convert.ToInt32(MainPLCReadVal[4]); //M1117
            ZGoHomeStatus = Convert.ToInt32(MainPLCReadVal[5]); //M1127



            toolStripXCurPosTxtBox.Text = XCurPos.ToString();
            toolStripYCurPosTxtBox.Text = YCurPos.ToString();
            toolStripZCurPosTxtBox.Text = ZCurPos.ToString();

            if (XGoHomeStatus == 0)
            {
                toolStripXHomeTxtBox.Text = "未歸Home";
                HomeFlag = 0;
            }
            else
            {
                toolStripXHomeTxtBox.Text = "已歸Home";
                HomeFlag = 1;
            }

            if (YGoHomeStatus == 0)
            {
                toolStripYHomeTxtBox.Text = "未歸Home";
                HomeFlag = 0;
            }
            else
            {
                toolStripYHomeTxtBox.Text = "已歸Home";
                HomeFlag = 1;
            }

            if (ZGoHomeStatus == 0)
            {
                toolStripZHomeTxtBox.Text = "未歸Home";
                HomeFlag = 0;
            }
            else
            {
                toolStripZHomeTxtBox.Text = "已歸Home";
                HomeFlag = 1;
            }
            //----------------ALM------------------------------
            AlmMsg = "";
            //if (MainPLCReadXYval[10] == 1) //  X0E
            //    AlmMsg = AlmMsg + PLCAlm.AlarmFind("F85");
            if (MainPLCReadXYval[20] == 1) //F85
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F85");
            if (MainPLCReadXYval[21] == 1) //F71
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F71");
            if (MainPLCReadXYval[22] == 1) //F72
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F72");
            if (MainPLCReadXYval[23] == 1) //F73
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F73");
            if (MainPLCReadXYval[24] == 1) //F74
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F74");
            if (MainPLCReadXYval[25] == 1) //F76
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F76");
            if (MainPLCReadXYval[26] == 1) //F77
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F77");
            if (MainPLCReadXYval[27] == 1) //F78
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F78");
            if (MainPLCReadXYval[28] == 1) //F79
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F79");
            if (MainPLCReadXYval[29] == 1) //F80
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F80");
            if (MainPLCReadXYval[30] == 1) //F81
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F81");
            if (MainPLCReadXYval[31] == 1) //F82
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F82");
            if (MainPLCReadXYval[32] == 1) //F83
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F83");
            if (MainPLCReadXYval[33] == 1) //F84
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F84");


            if (MainPLCReadXYval[34] == 1) //F1
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F1");
            if (MainPLCReadXYval[35] == 1) //F2
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F2");

            if (MainPLCReadXYval[36] == 1) //F23
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F23");
            if (MainPLCReadXYval[37] == 1) //F24
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F24");
            if (MainPLCReadXYval[38] == 1) //F25
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F25");
            if (MainPLCReadXYval[39] == 1) //F26
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F26");
            if (MainPLCReadXYval[40] == 1) //F31
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F31");
            if (MainPLCReadXYval[41] == 1) //F32
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F32");
            if (MainPLCReadXYval[42] == 1) //F33
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F33");



            if (MainPLCReadXYval[43] == 1) //F400
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F400");
            if (MainPLCReadXYval[44] == 1) //F401
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F401");
            if (MainPLCReadXYval[45] == 1) //402
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F402");
            if (MainPLCReadXYval[46] == 1) //F403
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F403");
            if (MainPLCReadXYval[47] == 1) //F404
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F404");
            if (MainPLCReadXYval[48] == 1) //F405
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F405");
            if (MainPLCReadXYval[49] == 1) //F406
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F406");
            if (MainPLCReadXYval[50] == 1) //F407
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F407");
            if (MainPLCReadXYval[51] == 1) //F408
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F408");
            if (MainPLCReadXYval[52] == 1) //F409
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F409");
            if (MainPLCReadXYval[53] == 1) //F411
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F411");
            if (MainPLCReadXYval[54] == 1) //F412
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F412");
            if (MainPLCReadXYval[55] == 1) //F42
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F420");



            if (MainPLCReadXYval[56] == 1) //F421
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F421");
            if (MainPLCReadXYval[57] == 1) //F422
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F422");
            if (MainPLCReadXYval[58] == 1) //F423
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F423");
            if (MainPLCReadXYval[59] == 1) //F424
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F424");
            if (MainPLCReadXYval[60] == 1) //F425
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F425");
            if (MainPLCReadXYval[61] == 1) //F426
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F426");
            if (MainPLCReadXYval[62] == 1) //F427
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F427");
            if (MainPLCReadXYval[63] == 1) //F428
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F428");
            if (MainPLCReadXYval[64] == 1) //F429
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F429");
            if (MainPLCReadXYval[65] == 1) //F430
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F430");
            if (MainPLCReadXYval[66] == 1) //F411
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F431");
            if (MainPLCReadXYval[67] == 1) //F412
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F432");
            if (MainPLCReadXYval[68] == 1) //F42
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F440");


            if (MainPLCReadXYval[69] == 1) //F421
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F441");
            if (MainPLCReadXYval[70] == 1) //F422
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F442");
            if (MainPLCReadXYval[71] == 1) //F423
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F443");
            if (MainPLCReadXYval[72] == 1) //F424
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F444");
            if (MainPLCReadXYval[73] == 1) //F425
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F445");
            if (MainPLCReadXYval[74] == 1) //F426
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F446");
            if (MainPLCReadXYval[75] == 1) //F427
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F447");
            if (MainPLCReadXYval[76] == 1) //F428
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F448");
            if (MainPLCReadXYval[77] == 1) //F429
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F449");
            if (MainPLCReadXYval[78] == 1) //F430
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F450");
            if (MainPLCReadXYval[79] == 1) //F421
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F451");
            if (MainPLCReadXYval[80] == 1) //F422
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F452");
            if (MainPLCReadXYval[81] == 1) //F423
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F460");




            if (MainPLCReadXYval[82] == 1) //F424
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F461");
            if (MainPLCReadXYval[83] == 1) //F425
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F462");
            if (MainPLCReadXYval[84] == 1) //F426
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F463");
            if (MainPLCReadXYval[85] == 1) //F427
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F464");
            if (MainPLCReadXYval[86] == 1) //F428
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F465");
            if (MainPLCReadXYval[87] == 1) //F428
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F466");
            if (MainPLCReadXYval[88] == 1) //F429
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F467");
            if (MainPLCReadXYval[89] == 1) //F430
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F468");
            if (MainPLCReadXYval[90] == 1) //F430
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F469");
            if (MainPLCReadXYval[91] == 1) //F430
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F470");
            if (MainPLCReadXYval[92] == 1) //F430
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F471");
            if (MainPLCReadXYval[93] == 1) //F430
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F472");
            if (MainPLCReadXYval[94] == 1) //F9
                AlmMsg = AlmMsg + PLCAlm.AlarmFind("F9");

            if (XGoHomeStatus == 0 || YGoHomeStatus == 0 || ZGoHomeStatus == 0)
                AlmMsg = AlmMsg + "_未完成Home";


            toolStripAlmTxtBox.Text = AlmMsg;




        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {


            ////全部清0
            PLCAction.PLC_AllClear();

            //關閉伺服馬達
            PLCAction.PLC_StageMotor(0);

            //停止交握
            PLCAction.PLC_HandShaking(0);

            //交還主控權給PLC
            PLCAction.MainControl(0);

            //關閉PLC元件
            PLCAction.PLC_Close();

            //timer 全部關閉
            timer1.Dispose();
            timer2.Dispose();

            if (StageControlForm.IsDisposed == false && StageControlForm.IsHandleCreated == true)
                StageControlForm.FormStageControl_FormClosed(sender, e);

            if (LaserForm.IsDisposed == false && LaserForm.IsHandleCreated == true)
                LaserForm.FormLaser_FormClosed(sender, e);

            if (WeldForm.IsDisposed == false && WeldForm.IsHandleCreated == true)
                WeldForm.WeldSystem_FormClosed(sender, e);


        }

        private void menuStrip1_Click(object sender, EventArgs e)
        {



        }

        private void 雷射控制IO點ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (toolStripTextBox1.Text != "ENG")
            //{
            //    MessageBox.Show("非工程師無法開啟!");
            //}
            //else
            //{
            if (HomeFlag == 0)
                MessageBox.Show("需要復歸");
            else
            {

                //防重開寫法
                bool isFind = false;
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Name == "FormLaser")
                    {
                        isFind = true;
                        form.MdiParent = this;
                        form.Focus();
                    }
                }

                if (isFind == false)
                {
                    LaserForm = new FormLaser("3");
                    LaserForm.MdiParent = this;
                    LaserForm.Show();

                }


            }
            //}
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // ---------login---------
            this.Hide();
            if ((VP.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                &&
                (VP.UserAccount != "ERROR" && VP.UserAccount != "")
                && (VP.UserPW != "ERROR") && VP.UserPW != "")
            {
                VP.PW_TxtBox.Text = "";
                VP.User_TxtBox.Text = "";
                // VP.Dispose();
                VP.Hide();
                this.Show();
                // ----------------讀取生管單路徑---------------------
            }
            else
            {

                // VP.Dispose();
                //this.Close();
                VP.Hide();
                this.Close();
            }
        }

        private void 登出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // this.FormClosed(sender, e);
        }

        private void 登出ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (StageControlForm.IsDisposed == false && StageControlForm.IsHandleCreated == true)
                StageControlForm.Close();

            if (LaserForm.IsDisposed == false && LaserForm.IsHandleCreated == true)
                LaserForm.Close();

            if (WeldForm.IsDisposed == false && WeldForm.IsHandleCreated == true)
                WeldForm.Close();

            // ---------login---------
            this.Hide();
            if ((VP.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                &&
                (VP.UserAccount != "ERROR" && VP.UserAccount != "")
                && (VP.UserPW != "ERROR") && VP.UserPW != "")
            {
                VP.PW_TxtBox.Text = "";
                VP.User_TxtBox.Text = "";
                VP.Hide();
                this.Show();
                // ----------------讀取生管單路徑---------------------
            }
            else
            {
                VP.Hide();
                this.Close();
            }
        }

        private void MainForm_ForeColorChanged(object sender, EventArgs e)
        {

        }






    }
}
