using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace WeldControlSystem
{

    public partial class WeldSystem : Form
    {

        //單部動作獨立執行 thread
        Thread TrackInThread, TrackOutThread, BoundingThread, ReleaseThread;
        Thread RepairMoveThread;
        //AB mode 交握相關
        System.Threading.Timer timer1;
        System.Threading.Timer timer2;
        System.Threading.Timer timer3;
        int FrozenOK = 0;

        int ABPortMode = -1;   //AB PORT 模式: A=0,B=1,AB=2
        int GreenAPortBtnFlag = 0, GreenBPortBtnFlag = 0; //聽user是不是已經按了綠色按鈕
        int PCAPortBtnFlag = 0, PCBPortBtnFlag = 0; //user按了畫面按鈕


        //重要交握FLAG
        //int ScannerIsWorkFlag = 0; //Scanner 被使用中:1  沒人用:0
        string ScannerUser = "";// scanner誰占用 A/B
        string ScannerRank = "";//誰來排隊 scanner
        int ScannerIsWork = 0; // scanner加工中:1,加工結束: 0

        int NowProcessPort = -1;//目前是誰正在執行任務: A=0,B=1

        //開關門交握flag
        int CloseDoorFlag = 0;//0:有關門,1:沒關門


        //AB thread
        Thread APortStartWorkThread, BPortStartWorkThread;

        //A製程設定相關
        String ARecipeName, BRecipeName;
        int AIniAreaCnt; //新版的A區域總數
        int BIniAreaCnt; //新版的B區域總數
        // int IniAreaCnt;

        List<double> AIniXPointList; //新版的所有區域座標點集合
        List<double> AIniYPointList; //新版的所有區域座標點集合
        List<double> AIniZPointList; //新版的所有區域座標點集合
        //     List<double> AIniSpeedRateList; //新版的所有區域示意圖集合
        List<string> AIniMMFileList; //新版的所有區域mm file集合
        List<string> AIniAreaNumList = new List<string>(); //新版的所有區示意圖域移動順序集合(temp str)
        List<int> AIniAreaNumXList = new List<int>(); //新版的所有區域示意圖X移動順序集合
        List<int> AIniAreaNumYList = new List<int>(); //新版的所有區域示意圖Y移動順序集合
        List<string> AIniPicFileList; //新版的所有區域示意圖檔案路徑集合

        //其他系統設定-----------------------------------------------------------
        List<int> IniXdirList = new List<int>();//新版決定X軸向所乘倍率(1or-1)
        List<int> IniYdirList = new List<int>();//新版決定Y軸向所乘倍率(1or-1)
        List<int> IniZdirList = new List<int>();//新版決定Z軸向所乘倍率(1or-1)
        List<int> IniXoriList = new List<int>();//新版決定X軸原點
        List<int> IniYoriList = new List<int>();//新版決定Y軸原點
        List<int> IniZoriList = new List<int>();//新版決定Z軸原點
        List<int> IniXUpBoundList = new List<int>();//新版決定X軸上限
        List<int> IniXDownBoundList = new List<int>();//新版決定X軸下限
        List<int> IniYUpBoundList = new List<int>();//新版決定Y軸上限
        List<int> IniYDownBoundList = new List<int>();//新版決定Y軸下限
        List<int> IniZUpBoundList = new List<int>();//新版決定Z軸上限
        List<int> IniZDownBoundList = new List<int>();//新版決定Z軸下限


        //B製程設定相關
        List<double> BIniXPointList; //新版的所有區域座標點集合
        List<double> BIniYPointList; //新版的所有區域座標點集合
        List<double> BIniZPointList; //新版的所有區域座標點集合
        //  List<double> BIniSpeedRateList; //新版的所有區域示意圖集合
        List<string> BIniMMFileList; //新版的所有區域mm file集合
        List<string> BIniAreaNumList = new List<string>(); //新版的所有區示意圖域移動順序集合(temp str)
        List<int> BIniAreaNumXList = new List<int>(); //新版的所有區域示意圖X移動順序集合
        List<int> BIniAreaNumYList = new List<int>(); //新版的所有區域示意圖Y移動順序集合
        List<string> BIniPicFileList; //新版的所有區域示意圖檔案路徑集合



        //UI:動畫
        double Aw, Ah, AOffsetW, AOffsetH;//新版的動畫區域長寬分配數量單位
        double Bw, Bh, BOffsetW, BOffsetH;//新版的動畫區域長寬分配數量單位
        Bitmap Abmp, Bbmp;

        //CLASS相關
        FileIO FileIO = new FileIO();
        PLCAction PLCAction = new PLCAction();
        DrawItems DrawItems = new DrawItems();


        //202107 自動補銲相關宣告
        List<EZMLayerObjInfo> AllEZMLayerObjList;
        List<string> LabelObjList = new List<string>();
        int AllLayer = 0; // user 選擇整個圖層都要補銲


        //---------------MDI傳直-----------------
        int M1138, M1139, M1202, X0E, M1204;
        string UserCategory;
        public int ValM1138
        {
            set { M1138 = value; }
        }
        public int ValM1139
        {
            set { M1139 = value; }
        }
        public int ValM1204
        {
            set { M1204 = value; }
        }
        //public int ValM1202
        //{
        //    set { M1202 = value; }
        //}
        public string ValUserCategoryControl
        {
            set { UserCategory = value; }
        }
        public int ValX0E
        {
            set { X0E = value; }
        }

        private OpenFileDialog OpenFileDialog;
        public WeldSystem()
        {

            InitializeComponent();
        }


        private void button12_Click(object sender, EventArgs e)
        {



        }
        void AllUIModeUnable()
        {

            //按鈕全關
            SetAiniBtn.Enabled = false;
            SetAtxtBox.Enabled = false;
            OperatorABtn.Enabled = false;
            //SetBiniBtn.Enabled = false;
            //SetBtxtBox.Enabled = false;
            //OperatorBBtn.Enabled = false;
            //EngAPortRadBtn.Enabled = false;
            //EngBPortRadBtn.Enabled = false;

            EnabkeEngBtn(0);

            //RECIPE重選相關
            ARcpNameLabel.Text = "NONE";
            //BRcpNameLabel.Text = "NONE";
            SetAtxtBox.Clear();
            //SetBtxtBox.Clear();
            ARecipeName = "";
            BRecipeName = "";

            //圖片清空
            Bitmap EmptyBmp = new Bitmap(1, 1);
            AShowProgressPicBox.Image = EmptyBmp;
            EmptyBmp = new Bitmap(1, 1);
            //BShowProgressPicBox.Image = EmptyBmp;


            //關閉 REPAIR BOX
            EnableRepairGrpBox(0);
            EnableRepairBtn(0);
            //EngRepairChkBox.Checked = false;
            RedLightOff();
            axMMMark1.LoadFile("");

            RepairRedLightBtn.Enabled = false;
            RepairLaserBtn.Enabled = false;
            //RepairTrackOutBtn.Enabled = false;

        }
        private void ModeARadBtn_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ModeBRadBtn_CheckedChanged(object sender, EventArgs e)
        {
            //if (ModeBRadBtn.Checked == true)
            //{
            //    ModeARadBtn.Checked = false;
            //    ModeABRadBtn.Checked = false;
            //}

            AllUIModeUnable();

            //SetBiniBtn.Enabled = true;
            //SetBtxtBox.Enabled = true;
            //OperatorBBtn.Enabled = true;

            //EngBPortRadBtn.Enabled = true;
            //EngAPortRadBtn.Checked = false;
            //EngBPortRadBtn.Checked = true;

        }

        private void ModeABRadBtn_CheckedChanged(object sender, EventArgs e)
        {
            //if (ModeABRadBtn.Checked == true)
            //{
            //    ModeARadBtn.Checked = false;
            //    ModeBRadBtn.Checked = false;
            //}

            SetAiniBtn.Enabled = true;
            SetAtxtBox.Enabled = true;
            OperatorABtn.Enabled = true;

            //SetBiniBtn.Enabled = true;
            //SetBtxtBox.Enabled = true;
            //OperatorBBtn.Enabled = true;

            //EngAPortRadBtn.Enabled = true;
            //EngBPortRadBtn.Enabled = true;
            //EngAPortRadBtn.Checked = true;
            //EngBPortRadBtn.Checked = false;

        }

        private void SetAiniBtn_Click(object sender, EventArgs e)
        {
            string OpenFilePath = "";
            OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.InitialDirectory = ".\\";
            OpenFileDialog.Filter = "All files (*.*)|*.*";
            OpenFileDialog.FilterIndex = 2;
            OpenFileDialog.RestoreDirectory = true;
            DialogResult result = OpenFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                OpenFilePath = OpenFileDialog.FileName;
                SetAtxtBox.Text = OpenFilePath;// OpenFilePath.Substring(OpenFilePath.LastIndexOf("\\") + 1, (OpenFilePath.LastIndexOf(".") - OpenFilePath.LastIndexOf("\\")) - 1);

            }
        }

        private void SetBiniBtn_Click(object sender, EventArgs e)
        {

            string OpenFilePath = "";
            OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.InitialDirectory = ".\\";
            OpenFileDialog.Filter = "All files (*.*)|*.*";
            OpenFileDialog.FilterIndex = 2;
            OpenFileDialog.RestoreDirectory = true;
            DialogResult result = OpenFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                OpenFilePath = OpenFileDialog.FileName;
                // SetBtxtBox.Text = OpenFilePath;// OpenFilePath.Substring(OpenFilePath.LastIndexOf("\\") + 1, (OpenFilePath.LastIndexOf(".") - OpenFilePath.LastIndexOf("\\")) - 1);

            }
        }
        void EnableRepairBtn(int OnOff)//0:IFF 1:只開AB COMBPO 2:全開
        {
            if (OnOff == 0)//off
            {
                RepairRedLightBtn.Enabled = false;
                RepairLaserBtn.Enabled = false;
                //RepairTrackOutBtn.Enabled = false;
            }
            else if (OnOff == 1)
            {
                RepairRedLightBtn.Enabled = true;
                RepairLaserBtn.Enabled = true;
                //RepairTrackOutBtn.Enabled = true;
            }

        }
        void EnableRepairGrpBox(int OnOff)//0:IFF 1:只開AB COMBPO 2:全開
        {
            if (OnOff == 0)//off
            {
                RedLightOff();
                foreach (Control ctl in tabPage2.Controls)
                {

                    RepairXcomboBox.Text = "";
                    RepairYcomboBox.Text = "";
                    //RepairABcomboBox.Text = "";
                    ctl.Enabled = false;
                }
            }
            else if (OnOff == 1)
            {
                RepairXcomboBox.SelectedItem = "1";
                RepairYcomboBox.SelectedItem = "1";

                foreach (Control ctl in tabPage2.Controls)
                {
                    ctl.Enabled = true;
                }

                //if ((ModeARadBtn.Checked == true && ARecipeName != "") || (ModeBRadBtn.Checked == true && BRecipeName != "")
                //    || (ModeABRadBtn.Checked == true && ARecipeName != "" && BRecipeName != ""))
                if (ARecipeName != "")
                {
                    //RepairABLabel.Enabled = true;
                    //RepairABcomboBox.Enabled = true;
                    OperatorABtn.Enabled = false;
                    //OperatorBBtn.Enabled = false;
                    RepairXcomboBox.Text = "";
                    RepairYcomboBox.Text = "";
                    RedLightOff();
                    axMMMark1.LoadFile("");
                }
            }
            //if (OnOff == 0)//off
            //{
            //    RedLightOff();
            //    foreach (Control ctl in RepairGrpBox.Controls)
            //    {

            //        RepairXcomboBox.Text = "";
            //        RepairYcomboBox.Text = "";
            //        RepairABcomboBox.Text = "";
            //        ctl.Enabled = false;
            //    }

            //}
            //else if (OnOff == 1)
            //{
            //    RepairXcomboBox.SelectedItem = "1";
            //    RepairYcomboBox.SelectedItem = "1";

            //    //if ((ModeARadBtn.Checked == true && ARecipeName != "") || (ModeBRadBtn.Checked == true && BRecipeName != "")
            //    //    || (ModeABRadBtn.Checked == true && ARecipeName != "" && BRecipeName != ""))
            //    if (ARecipeName != "")
            //    {
            //        RepairABLabel.Enabled = true;
            //        RepairABcomboBox.Enabled = true;
            //        OperatorABtn.Enabled = false;
            //        //OperatorBBtn.Enabled = false;
            //        RepairXcomboBox.Text = "";
            //        RepairYcomboBox.Text = "";
            //        RedLightOff();
            //        axMMMark1.LoadFile("");
            //    }
            //    else
            //    {
            //        RepairRadioBtn.Checked = false;
            //        MessageBox.Show("請指定好完整 Recipe, 否則無法開啟補銲功能!");
            //        return;
            //    }

            //}
            //else if (OnOff == 2)
            //{
            //    RepairXcomboBox.Text = "";
            //    RepairYcomboBox.Text = "";
            //    RedLightOff();
            //    foreach (Control ctl in RepairGrpBox.Controls)
            //    {

            //        axMMMark1.LoadFile("");
            //        ctl.Enabled = true;
            //    }

            //}
        }
        private void RepairRadioBtn_Click(object sender, EventArgs e)
        {
            //RepairRadioBtn.Checked = true;
            AutoWeldRadBtn.Checked = false;
            RedLightRadioBtn.Checked = false;


            EnableRepairGrpBox(1);
            AutoRepairRadBtn_Click(sender, e);

        }

        private void ModeABRadBtn_CheckedChanged_1(object sender, EventArgs e)
        {



        }

        private void SetAiniBtn_Click_1(object sender, EventArgs e)
        {
            NowProcessPort = 0;

            string OpenFilePath;

            OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.InitialDirectory = ".\\";
            OpenFileDialog.Filter = "ini files (*.ini)|*.ini";
            OpenFileDialog.FilterIndex = 2;
            OpenFileDialog.RestoreDirectory = true;
            DialogResult result = OpenFileDialog.ShowDialog();
            OpenFilePath = OpenFileDialog.FileName;

            SetAtxtBox.Text = OpenFilePath;

            if (result == DialogResult.OK && OpenFileDialog.FileName != "")
            {
                //擷取檔名
                ARecipeName = FileIO.GetFileName(OpenFilePath);
                ARcpNameLabel.Text = ARecipeName;


                //ReadSection
                List<string> SecList = new List<string>();
                SecList = FileIO.IniDoubleReadSec(OpenFileDialog.FileName);
                AIniAreaCnt = SecList.Count();

                AIniXPointList = new List<double>();
                AIniYPointList = new List<double>();
                AIniZPointList = new List<double>();

                AIniMMFileList = new List<string>();

                AIniAreaNumList = new List<string>();
                AIniAreaNumXList = new List<int>();
                AIniAreaNumYList = new List<int>();

                AIniPicFileList = new List<string>();

                //A XYZ座標
                AIniXPointList = FileIO.IniDoubleReadKey(SecList, "XPoint", OpenFileDialog.FileName);
                AIniYPointList = FileIO.IniDoubleReadKey(SecList, "YPoint", OpenFileDialog.FileName);
                AIniZPointList = FileIO.IniDoubleReadKey(SecList, "ZPoint", OpenFileDialog.FileName);

                //A MM file
                AIniMMFileList = FileIO.IniStrReadKey(SecList, "MMFile", OpenFileDialog.FileName);
                AIniPicFileList = FileIO.IniStrReadKey(SecList, "PicFile", OpenFileDialog.FileName);
                AIniAreaNumList = FileIO.IniStrReadKey(SecList, "AreaNum", OpenFileDialog.FileName);

                //動畫順序
                AIniAreaNumXList = FileIO.IniAreaNumGet(AIniAreaNumList, "x");
                AIniAreaNumYList = FileIO.IniAreaNumGet(AIniAreaNumList, "y");


                //------------------- load 第一個 MM 檔----------------------------
                RedLightOff();
                axMMMark1.LoadFile(AIniMMFileList[0]);



                //-------------------畫第一張動畫圖-----------------------------
                //抓長寬最多各幾單位,才能分配圖片大小
                int w_cnt = -1, h_cnt = -1;
                for (int i = 0; i < AIniAreaCnt; i++)
                {
                    if (AIniAreaNumXList[i] > w_cnt) w_cnt = AIniAreaNumXList[i];
                    if (AIniAreaNumYList[i] > h_cnt) h_cnt = AIniAreaNumYList[i];
                }


                //預估最佳化的圖片大小
                double UnitW = AShowProgressPicBox.Width / w_cnt;
                double UnitH = AShowProgressPicBox.Height / h_cnt;
                double Unit;
                if (UnitW > UnitH) Unit = UnitH;
                else Unit = UnitW;


                //評估置中offset多少
                AOffsetW = 0;
                AOffsetH = 0;
                if ((int)(AShowProgressPicBox.Width - Unit * w_cnt) < 5)
                {
                    double MiddleH = AShowProgressPicBox.Height / 2;
                    AOffsetH = MiddleH - Unit * h_cnt / 2;

                }
                else if ((int)(AShowProgressPicBox.Height - Unit * h_cnt) < 5)
                {
                    double MiddleW = AShowProgressPicBox.Width / 2;
                    AOffsetW = MiddleW - Unit * w_cnt / 2;

                }

                Aw = Unit;
                Ah = Unit;

                //畫空白外框
                Abmp = new Bitmap(AShowProgressPicBox.Width, AShowProgressPicBox.Height);
                Abmp = DrawAEmpthRect(Abmp, AIniAreaCnt);
                AShowProgressPicBox.Image = Abmp;
                AShowProgressPicBox.Refresh();
                GC.Collect();




            }





        }
        private Bitmap DrawAEmpthRect(Bitmap bmp, int TTLAreaCnt)
        {


            for (int i = 0; i < TTLAreaCnt; i++)
                bmp = DrawItems.DrawRect(bmp, Aw * (AIniAreaNumXList[i] - 1) + AOffsetW, Ah * (AIniAreaNumYList[i] - 1) + AOffsetH, Aw, Ah);


            return bmp;
        }
        private Bitmap DrawBEmpthRect(Bitmap bmp, int TTLAreaCnt)
        {


            for (int i = 0; i < TTLAreaCnt; i++)
                bmp = DrawItems.DrawRect(bmp, Bw * (BIniAreaNumXList[i] - 1) + BOffsetW, Bh * (BIniAreaNumYList[i] - 1) + BOffsetH, Bw, Bh);


            return bmp;
        }
        void OpenM1205()
        {
            int LaserGoFlag;
            // PLCAction.axActUtlType1.GetDevice("M1205", out LaserGoFlag);
            PLCAction.axActUtlType1.GetDevice("M1017", out LaserGoFlag);
            if (LaserGoFlag == 0)
            {
                //PLCAction.axActUtlType1.SetDevice("M1205", 1);
                PLCAction.axActUtlType1.SetDevice("M1017", 1);
                int LaserGoFlag2;
                while (true)
                {

                    //PLCAction.axActUtlType1.GetDevice("M1215", out LaserGoFlag2);
                    PLCAction.axActUtlType1.GetDevice("M1154", out LaserGoFlag2);
                    if (LaserGoFlag2 == 1)
                    {
                        //PLCAction.axActUtlType1.SetDevice("M1226", 1); //啟動雷射輸出命令 EMISSION ENABLE
                        PLCAction.axActUtlType1.SetDevice("M1048", 1); //啟動雷射輸出命令 EMISSION ENABLE
                        break;
                    }
                }
            }
            // PLCAction.axActUtlType1.SetDevice("M1206", 0);
            PLCAction.axActUtlType1.SetDevice("M1018", 0);

        }
        private void WeldSystem_Load(object sender, EventArgs e)
        {


            //PLC initial
            //1. 通訊
            PLCAction.PLC_Connect(0);

            //2.初始化:清0 + err reset + 重啟伺服馬達

            //3.主控權
            PLCAction.MainControl(1);//1:pc,0:plc

            //MM initial
            //initializing the MMLIBXXXX.ocx
            int MMIniResult, MMStandbyResult;
            MMIniResult = axMMMark1.Initial();
            ////standby the system for marking
            MMStandbyResult = axMMMark1.MarkStandBy();
            //可圈選要打的局部區域
            axMMMark1.SetCurEditFun(2);

            //先拿掉
            //Emmision on 1205=1
            // OpenM1205();


            //--------------------------------------------------------------------------------------------

            ////thread3:聽 USER 按A按鈕
            //TimerCallback callback1 = new TimerCallback(_ListenAPort);
            //timer1 = new System.Threading.Timer(callback1, null, 0, 200);//500豪秒起來一次

            ////thread4:聽 USER 按B按鈕
            //TimerCallback callback2 = new TimerCallback(_ListenBPort);
            //timer2 = new System.Threading.Timer(callback2, null, 0, 200);//500豪秒起來一次


            //先拿掉
            ////thread5:聽 急停按鈕
            //TimerCallback callback3 = new TimerCallback(_ListenAir);
            //timer3 = new System.Threading.Timer(callback3, null, 0, 300);//500豪秒起來一次


            //ui load
            KeyUIRadBtn_Click(sender, e);
            ModeARadBtn_Click(sender, e);
            ModePCRadBox_Click(sender, e);
            EngAPortRadBtn_Click(sender, e);
            ControlTabBox.SelectedIndex = 0;
            ControlTabBox_SelectedIndexChanged(sender, e);
            tabPage2.Parent = null;
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


            EnableRepairGrpBox(0);
        }



        //------------------ //------------------ //------------------ //------------------ //------------------
        private void _ListenAPort(object state)
        {

            if (PCAPortBtnFlag == 0 && GreenAPortBtnFlag == 0)//pc沒有按按鈕或自己沒有執行才能繼續
            {
                if (X0E == 0)
                {
                    CloseDoorFlag = 1;


                }
                else if (X0E == 1)
                {
                    CloseDoorFlag = 0;

                    if (M1138 == 1)
                    {

                        if (ARecipeName != "")
                        {
                            GreenAPortBtnFlag = 1;
                            InitialAShowPicBox();


                            //2.按鈕按完後自動入料,入料完成後plc自動解除1138
                            PLCAction.axActUtlType1.SetDevice("M1040", 1);

                            //3.分配SCANNER 使用權
                            if (ScannerUser == "")//如果沒人用 scanner
                            {
                                ScannerUser = "A";//A申請使用SCANEER
                                Console.WriteLine("沒人占用, A註冊 scanner 了");
                            }
                            else if (ScannerUser == "B") //如果B再用
                            {
                                ScannerRank = "A";//A申請使用SCANEER
                                Console.WriteLine("B占用, A排隊");

                            }
                            AWorkFlow();
                        }
                        else
                        {
                            PLCAction.axActUtlType1.SetDevice("M1138", 0);
                        }
                    }
                    else
                    {

                        GreenAPortBtnFlag = 0;
                    }

                }

            }



        }
        private void _ListenBPort(object state)
        {

            if (PCBPortBtnFlag == 0 && GreenBPortBtnFlag == 0)//pc沒有按按鈕或自己沒有執行才能繼續
            {
                if (X0E == 0)
                {
                    CloseDoorFlag = 1;
                    // Console.WriteLine("實體按鈕CloseDoorFlag = 1");

                }
                else if (X0E == 1)
                {
                    CloseDoorFlag = 0;

                    if (M1139 == 1)
                    {

                        if (BRecipeName != "")
                        {
                            GreenBPortBtnFlag = 1;
                            //  InitialBShowPicBox();

                            //2.按鈕按完後自動入料,入料完成後plc自動解除1139
                            PLCAction.axActUtlType1.SetDevice("M1041", 1);

                            //3.分配SCANNER 使用權
                            if (ScannerUser == "")//如果沒人用 scanner
                            {
                                ScannerUser = "B";//A申請使用SCANEER
                                Console.WriteLine("沒人占用, B註冊 scanner 了");
                            }
                            else if (ScannerUser == "A") //如果B再用
                            {
                                ScannerRank = "B";//A申請使用SCANEER
                                Console.WriteLine("A占用, B排隊");
                            }

                            // BWorkFlow();
                        }
                        else
                        {
                            PLCAction.axActUtlType1.SetDevice("M1139", 0);
                        }

                    }
                    else
                    {

                        GreenBPortBtnFlag = 0;
                    }
                }




            }


        }

        private void _ListenAir(object state)
        {

            this.BeginInvoke(new AirListen(ListenAir));//委派

        }
        delegate void AirListen();
        private void ListenAir()
        {
            //double[] _AirData = PLCAction.ReadPLCDataRandom("M1204", 1);
            if (M1204 == 1)
            {
                // PLCAction.axActUtlType1.SetDevice("M1204", 1); //z
                AirBtn.BackColor = Color.Green;
            }
            else if (M1204 == 0)
            {
                //PLCAction.axActUtlType1.SetDevice("M1204", 0); //z
                AirBtn.BackColor = Color.White;
            }
            //if (M1202 == 1 && FrozenOK == 0)
            //{
            //    StopFrozenUI(0);
            //    FrozenOK = 1;
            //}
            //else if (M1202 == 0)
            //{
            //    StopFrozenUI(1);
            //    FrozenOK = 0;
            //}


        }

        public void WeldSystem_FormClosed(object sender, FormClosedEventArgs e)
        {
            //shutdown the system
            RedLightOff();
            Thread.Sleep(200);
            axMMMark1.Finish();
            axMMMark1.MarkShutdown();
            //timer1.Dispose();
            //timer2.Dispose();
            //timer3.Dispose();
            // timer3.Dispose();
        }

        private void KeyUIRadBtn_Click(object sender, EventArgs e)
        {
            KeyUIRadBtn.Checked = true;
            KeyMMRadBtn.Checked = false;

            RedLightOff();
            axMMMark1.LoadFile("");

            int MMIniResult, MMStandbyResult, rls;

            MMIniResult = axMMMark1.Initial();
            Thread.Sleep(500);

            ////standby the system for marking
            MMStandbyResult = axMMMark1.MarkStandBy();
            Thread.Sleep(500);

            //可圈選要打的局部區域
            rls = axMMMark1.SetCurEditFun(2);
            Thread.Sleep(500);


        }

        private void KeyMMRadBtn_Click(object sender, EventArgs e)
        {
            KeyUIRadBtn.Checked = false;
            KeyMMRadBtn.Checked = true;

            RedLightOff();
            axMMMark1.LoadFile("");
            axMMMark1.Initial();

            axMMMark1.Finish();
            axMMMark1.MarkShutdown();



            SetIniGrpBox.Enabled = true;
            //SetPortGrpBox.Enabled = true;
            RepairGrpBox.Enabled = true;
            ControlTabBox.Enabled = true;
        }

        private void ModePLCRadBox_Click(object sender, EventArgs e)
        {
            ModePLCRadBox.Checked = true;
            ModePCRadBox.Checked = false;

            PLCAction.MainControl(0);

            SetIniGrpBox.Enabled = false;
            //SetPortGrpBox.Enabled = false;
            RepairGrpBox.Enabled = false;
            ControlTabBox.Enabled = false;



        }

        private void ModePCRadBox_Click(object sender, EventArgs e)
        {

            ModePLCRadBox.Checked = false;
            ModePCRadBox.Checked = true;

            PLCAction.MainControl(1);

            SetIniGrpBox.Enabled = true;
            //SetPortGrpBox.Enabled = true;
            RepairGrpBox.Enabled = true;
            ControlTabBox.Enabled = true;



        }

        private void ModeARadBtn_Click(object sender, EventArgs e)
        {
            ABPortMode = 0;

            AllUIModeUnable();

            EngAPortRadBtn_Click(sender, e);
            AutoWeldRadBtn_Click(sender, e);

            SetAiniBtn.Enabled = true;
            SetAtxtBox.Enabled = true;
            OperatorABtn.Enabled = true;

            //EngAPortRadBtn.Enabled = true;
            //EngAPortRadBtn.Checked = true;
            //EngBPortRadBtn.Checked = false;
            EngAPortRadBtn_Click(sender, e);

            //補銲功能
            //RepairABcomboBox.Items.Clear();
            //RepairABcomboBox.Text = "";
            //RepairABcomboBox.Items.Add("A");



        }

        private void ModePLCRadBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void EngOutBtn_Click(object sender, EventArgs e)
        {

            //使用 thread 才能按鈕中止

            TrackOutThread = new Thread(new ThreadStart(SingleTrackOut));
            Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)

            TrackOutThread.Start();


        }
        void SingleTrackOut()//要獨立出來,因為自動控制的while用這個不會等待
        {
            double[] _ReadVal = new double[1];
            if (NowProcessPort == 0)//A
            {

                _ReadVal = PLCAction.ReadPLCDataRandom("M1133", 1);
                if (_ReadVal[0] == 0)
                    // Console.WriteLine("A Port尚未鬆開到位!");
                    MessageBox.Show("A Port尚未鬆開到位!");  //防呆
                else
                {
                    PLCAction.TrackOut("A");
                    GreenAPortBtnFlag = 0; //用手動方式把異常關閉造成清不掉的FLAG清掉
                }


            }//----------------------------------------------------------
            else if (NowProcessPort == 1)//B
            {
                _ReadVal = new double[1];

                _ReadVal = PLCAction.ReadPLCDataRandom("M1137", 1);
                if (_ReadVal[0] == 0)
                    // Console.WriteLine("B Port尚未鬆開到位!");
                    MessageBox.Show("B Port尚未鬆開到位!");
                else
                {
                    PLCAction.TrackOut("B");
                    GreenBPortBtnFlag = 0; //用手動方式把異常關閉造成清不掉的FLAG清掉
                }


            }

        }




        private void EngBoundBtn_Click(object sender, EventArgs e)
        {

            //使用 thread 才能按鈕中止
            BoundingThread = new Thread(new ThreadStart(SingleBounding));
            Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)

            BoundingThread.Start();


        }
        void SingleBounding()//要獨立出來,因為自動控制的while用這個不會等待
        {

            //if (NowProcessPort == 0)//A
            //{
            //double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1131", 1);
            //if (_ReadVal[0] == 0)
            //    // Console.WriteLine("A Port尚未推入到位!");
            //    MessageBox.Show("A Port尚未推入到位!");
            //else
            PLCAction.Bounding("A");
            //}
            //else if (NowProcessPort == 1)//B
            //{
            //    double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1135", 1);
            //    if (_ReadVal[0] == 0)
            //        // Console.WriteLine("B Port尚未推入到位!");
            //        MessageBox.Show("B Port尚未推入到位!");
            //    else
            //        PLCAction.Bounding("B");
            //}



        }
        private void EngLaserWeldBtn_Click(object sender, EventArgs e)
        {

        }

        private void EngInBtn_Click(object sender, EventArgs e)
        {
            //使用 thread 才能按鈕中止
            TrackInThread = new Thread(new ThreadStart(SingleTrackIn));
            Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
            TrackInThread.Start();
        }

        void SingleTrackIn()//要獨立出來,因為自動控制的while用這個不會等待
        {

            if (NowProcessPort == 0)//A
            {
                double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1130", 1);
                if (_ReadVal[0] == 0)
                    //Console.WriteLine("A Port尚未到達上料位置!");
                    MessageBox.Show("A Port尚未到達上料位置!");
                else

                    PLCAction.TrackIn("A");

            }
            else if (NowProcessPort == 1)//B
            {
                double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1134", 1);
                if (_ReadVal[0] == 0)
                    //Console.WriteLine("B Port尚未到達上料位置!");
                    MessageBox.Show("B Port尚未到達上料位置!");
                else
                    PLCAction.TrackIn("B");

            }
        }

        void EnabkeEngBtn(int OnOff)
        {
            if (OnOff == 0)
            {
                //EngInBtn.Enabled = false;
                //EngOutBtn.Enabled = false;
                EngBoundBtn.Enabled = false;
                ReleaseBtn.Enabled = false;
            }
            else if (OnOff == 1)
            {
                //EngInBtn.Enabled = true;
                //EngOutBtn.Enabled = true;
                EngBoundBtn.Enabled = true;
                ReleaseBtn.Enabled = true;

            }
        }
        private void EngAPortRadBtn_Click(object sender, EventArgs e)
        {
            //EngAPortRadBtn.Checked = true;
            //EngBPortRadBtn.Checked = false;
            NowProcessPort = 0;

            EnabkeEngBtn(1);
        }

        private void ModeBRadBtn_Click(object sender, EventArgs e)
        {

            ABPortMode = 1;
            AllUIModeUnable();

            EngBPortRadBtn_Click(sender, e);
            AutoWeldRadBtn_Click(sender, e);
            //SetBiniBtn.Enabled = true;
            //SetBtxtBox.Enabled = true;
            //OperatorBBtn.Enabled = true;

            //EngBPortRadBtn.Enabled = true;
            //EngBPortRadBtn.Checked = true;
            //EngAPortRadBtn.Checked = false;
            EngBPortRadBtn_Click(sender, e);


            //RepairABcomboBox.Items.Clear();
            //RepairABcomboBox.Text = "";
            //RepairABcomboBox.Items.Add("B");

        }

        private void EngBPortRadBtn_Click(object sender, EventArgs e)
        {
            //EngBPortRadBtn.Checked = true;
            //EngAPortRadBtn.Checked = false;
            NowProcessPort = 1;
            EnabkeEngBtn(1);
        }

        private void ModeABRadBtn_Click(object sender, EventArgs e)
        {
            ABPortMode = 2;

            AllUIModeUnable();


            SetAiniBtn.Enabled = true;
            //SetBiniBtn.Enabled = true;
            SetAtxtBox.Enabled = true;
            //SetBtxtBox.Enabled = true;
            //OperatorBBtn.Enabled = true;
            OperatorABtn.Enabled = true;

            //EngAPortRadBtn.Enabled = true;
            //EngBPortRadBtn.Enabled = true;
            //EngBPortRadBtn.Checked = false;
            //EngAPortRadBtn.Checked = false;
            //EngAPortRadBtn_Click(sender, e);

            //RepairABcomboBox.Items.Clear();
            //RepairABcomboBox.Text = "";
            //RepairABcomboBox.Items.Add("A");
            //RepairABcomboBox.Items.Add("B");

        }

        private void ReleaseBtn_Click(object sender, EventArgs e)
        {

            //使用 thread 才能按鈕中止
            ReleaseThread = new Thread(new ThreadStart(SingleRelease));
            Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
            ReleaseThread.Start();


        }
        void SingleRelease()//要獨立出來,因為自動控制的while用這個不會等待
        {
            //if (NowProcessPort == 0)//A
            //{
            //double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1136", 1);
            //if (_ReadVal[0] == 0)
            //    //Console.WriteLine("A Port尚未壓合到位!");
            //    MessageBox.Show("A Port尚未壓合到位!");
            //else
            PLCAction.Release("A");

            //}
            //else if (NowProcessPort == 1)//B
            //{
            //    double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1136", 1);
            //    if (_ReadVal[0] == 0)
            //        //Console.WriteLine("B Port尚未壓合到位!");
            //        MessageBox.Show("B Port尚未壓合到位!");
            //    else
            //        PLCAction.Release("B");

            //}

        }

        void InitialAShowPicBox()
        {

            Abmp = new Bitmap(AShowProgressPicBox.Width, AShowProgressPicBox.Height);
            Abmp = DrawAEmpthRect(Abmp, AIniAreaCnt);
            AShowProgressPicBox.Image = Abmp;
            GC.Collect();
            //AShowProgressPicBox.Refresh();

        }
        //void InitialBShowPicBox()
        //{

        //    Bbmp = new Bitmap(BShowProgressPicBox.Width, BShowProgressPicBox.Height);
        //    Bbmp = DrawBEmpthRect(Bbmp, BIniAreaCnt);
        //    BShowProgressPicBox.Image = Bbmp;
        //    GC.Collect();
        //    //BShowProgressPicBox.Refresh();

        //}
        private void OperatorABtn_Click(object sender, EventArgs e)
        {

            //先拿掉
            //if (X0E == 0)
            //{
            //    CloseDoorFlag = 1;
            //    MessageBox.Show("安全門未關好!!\n請關閉好再重新加工!!");
            //}
            //else if (X0E == 1)
            //{
            CloseDoorFlag = 0;
            if (ARecipeName == "")
            {
                MessageBox.Show("未指定 A port recipe!");
                return;
            }

            if (AutoWeldRadBtn.Checked == true)
            {
                DialogResult AutoLaserChkResult = MessageBox.Show(this, "確定要激發雷射做加工嗎?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (AutoLaserChkResult == DialogResult.No)
                    return;
            }


            InitialAShowPicBox();

            PCAPortBtnFlag = 1;
            //if (GreenAPortBtnFlag == 0)
            //{

            //double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1130", 1);//A推出到位
            //if (_ReadVal[0] == 1)
            //{
            APortStartWorkThread = new Thread(new ThreadStart(APortStartWork));
            Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
            APortStartWorkThread.Start();
            //}
            //else
            //{
            //    MessageBox.Show("A還沒推出來!");
            //}
            //}
            //else
            //{
            //    MessageBox.Show("已經按了A實體按鈕,GreenAPortBtnFlag=" + GreenAPortBtnFlag);
            //}
            //-------------------------------
            //}
        }
        void APortStartWork()
        {

            ////1.按壓A區按鈕,綠色按鈕發亮
            //PLCAction.axActUtlType1.SetDevice("M1138", 1);
            //2.按鈕按完後自動入料,入料完成後plc自動解除1138
            //PLCAction.axActUtlType1.SetDevice("M1040", 1);
            //Console.WriteLine("=============A開始加工=============");
            //Console.WriteLine("1.A已經確認加工推入");

            //if (ScannerUser == "")//如果沒人用 scanner
            //{
            //    ScannerUser = "A";//A申請使用SCANEER
            //    Console.WriteLine("沒人占用, A註冊 scanner 了");
            //}
            //else if (ScannerUser == "B") //如果B再用
            //{
            //    ScannerRank = "A";//A申請使用SCANEER
            //    Console.WriteLine("B占用, A排隊");

            //}

            //NowProcessPort = 0; //A使用中
            AWorkFlow();

        }
        void AScannerWeldProcess()
        {

            if (ScannerIsWork == 0)
            {
                ScannerIsWork = 1;//scanner 加工中
                Console.WriteLine("ScannerIsWork = 1");

                Bitmap Abmp = new Bitmap(AShowProgressPicBox.Image);

                int LaserGoFlag, SWIRedLightReqFlag;
                //==================20220114紅光先處理COVER PLC得部分====================
                if (RedLightRadioBtn.Checked == true)//20220114紅光先處理COVER PLC得部分
                {
                    //1.先判斷紅光有沒有開(M1206國揚好像PLC沒改到不管了)
                    PLCAction.axActUtlType1.GetDevice("M1018", out SWIRedLightReqFlag);
                    //2.紅光沒開: 檢查1017雷射輸出有沒有開
                    if (SWIRedLightReqFlag == 0)
                    {
                        //20220112 如果m1017 keep
                        PLCAction.axActUtlType1.GetDevice("M1017", out LaserGoFlag);

                        //20220112 幫國陽直接把emmision off
                        //2.1 如果1017有開則EMISSION OFF
                        if (LaserGoFlag == 1)
                            PLCAction.axActUtlType1.SetDevice("M1048", 0);

                        //2.2 開啟紅光
                        PLCAction.axActUtlType1.SetDevice("M1018", 1);

                        ////2.3開MM紅光
                        //axMMMark1.StopMarking();
                        //axMMMark1.SetMarkSelect(1);
                        //axMMMark1.StartMarking(3);
                    }
                    //3.紅光有開:  keep住就好


                    //PLCAction.axActUtlType1.SetDevice("M1018", 1);
                }
                else if (AutoWeldRadBtn.Checked == true)
                {
                    int RedLightFlag, SWILaserReqFlag;

                    PLCAction.axActUtlType1.GetDevice("M1205", out SWILaserReqFlag);

                    if (SWILaserReqFlag == 0)//雷射輸出還沒開
                    {
                        PLCAction.axActUtlType1.GetDevice("M1018", out RedLightFlag);
                        if (RedLightFlag == 1)
                        {
                            PLCAction.axActUtlType1.SetDevice("M1018", 0);

                        }

                        PLCAction.axActUtlType1.SetDevice("M1017", 1);

                        while (true)
                        {
                            double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1017", 1);
                            {
                                PLCAction.axActUtlType1.SetDevice("M1048", 1); //啟動雷射輸出命令 EMISSION ENABLE
                                break;
                            }
                        }

                        ////-----前面都在COVER PLC,這段才開始正式MM------------------------
                        //Thread.Sleep(1000);
                        //axMMMark1.StopMarking();
                        //axMMMark1.SetMarkSelect(1);
                        //axMMMark1.StartMarking(1);

                        //MessageBox.Show("已完成銲接!!");
                        //--------------------------------------------------------------

                    }
                    //3.雷射有開:  keep住就好

                }
                for (int i = 0; i < AIniAreaCnt; i++)
                {

                    axMMMark1.StopMarking();
                    //if (CloseDoorFlag == 0)//關門狀態下才能繼續下一步
                    //{
                    axMMMark1.LoadFile(AIniMMFileList[i]);
                    AAutoMark_LaserMark(i);
                    //if (CloseDoorFlag == 0)//關門狀態下才能繼續下一步
                    //{
                    //3.畫動畫
                    Abmp = DrawItems.DrawBlock(Abmp,
                          Aw * (AIniAreaNumXList[i] - 1) + AOffsetW, Ah * (AIniAreaNumYList[i] - 1) + AOffsetH,
                          Aw * AIniAreaNumXList[i] + AOffsetW, Ah * (AIniAreaNumYList[i] - 1) + AOffsetH,
                          Aw * (AIniAreaNumXList[i] - 1) + AOffsetW, Ah * AIniAreaNumYList[i] + AOffsetH,
                          AIniPicFileList[i]);
                    Thread.Sleep(100);

                    //補畫方A框
                    Abmp = DrawAEmpthRect(Abmp, AIniAreaCnt);
                    AShowProgressPicBox.Image = Abmp;
                    GC.Collect();
                    //}
                    //}



                }
                ScannerIsWork = 0;//scanner 結束加工
                Console.WriteLine("ScannerIsWork = 0");

                //最後移動到出料位置
                //第一區的(X,0,Z)
                PLCAction.AutoStageMove(AIniXPointList[0], 0, AIniZPointList[0], 100);


                //紅光要關1206,emssion才會on
                if (RedLightRadioBtn.Checked == true)
                    PLCAction.axActUtlType1.SetDevice("M1206", 0);
            }
        }
        //void BScannerWeldProcess()
        //{
        //    if (ScannerIsWork == 0)
        //    {
        //        ScannerIsWork = 1;//scanner 加工中
        //        Console.WriteLine("ScannerIsWork = 1");
        //        Bitmap Bbmp = new Bitmap(BShowProgressPicBox.Image);

        //        //紅光要開1206,出光不用動因為已經開好囉
        //        if (RedLightRadioBtn.Checked == true)
        //            PLCAction.axActUtlType1.SetDevice("M1206", 1);


        //        for (int i = 0; i < BIniAreaCnt; i++)
        //        {
        //            axMMMark1.StopMarking();
        //            if (CloseDoorFlag == 0)//關門狀態下才能繼續下一步
        //            {
        //                axMMMark1.LoadFile(BIniMMFileList[i]);
        //                BAutoMark_LaserMark(i);

        //                if (CloseDoorFlag == 0)//關門狀態下才能繼續下一步
        //                {

        //                    //3.畫動畫
        //                    Bbmp = DrawItems.DrawBlock(Bbmp,
        //                          Bw * (BIniAreaNumXList[i] - 1) + BOffsetW, Bh * (BIniAreaNumYList[i] - 1) + BOffsetH,
        //                          Bw * BIniAreaNumXList[i] + BOffsetW, Bh * (BIniAreaNumYList[i] - 1) + BOffsetH,
        //                          Bw * (BIniAreaNumXList[i] - 1) + BOffsetW, Bh * BIniAreaNumYList[i] + BOffsetH,
        //                          BIniPicFileList[i]);

        //                    //補畫方框
        //                    Bbmp = DrawBEmpthRect(Bbmp, BIniAreaCnt);
        //                    BShowProgressPicBox.Image = Bbmp;
        //                    GC.Collect();
        //                }
        //            }
        //            // BShowProgressPicBox.Refresh();
        //        }
        //        ScannerIsWork = 0;//scanner 結束加工
        //        Console.WriteLine("ScannerIsWork = 0");

        //        //紅光要關1206,emssion才會on
        //        if (RedLightRadioBtn.Checked == true)
        //            PLCAction.axActUtlType1.SetDevice("M1206", 0);


        //    }



        //}
        private void AAutoMark_LaserMark(int i)
        {
            //if (X0E == 1)
            //{
            //    //if (RedLightRadioBtn.Checked == true)
            //{
            //    PLCAction.AutoStageMove(AIniXPointList[i], AIniYPointList[i], AIniZPointList[i] - 13, 100);
            //}
            //else if (AutoWeldRadBtn.Checked == true)
            //{
            PLCAction.AutoStageMove(AIniXPointList[i], AIniYPointList[i], AIniZPointList[i], 100);
            //}
            Console.WriteLine("A--第 " + i + " 區要加工了--");
            //if (X0E == 1)
            //{
            //先拿掉
            //Thread.Sleep(2000);
            if (RedLightRadioBtn.Checked == true)//紅光
            {

                axMMMark1.StartMarking(3);

            }
            else if (AutoWeldRadBtn.Checked == true)//雷射
            {

                axMMMark1.StartMarking(1);
            }
            //}
            ////else
            //{
            //    MessageBox.Show("安全門未關好!!\n請關閉好再重新加工!!");
            //    CloseDoorFlag = 1;
            //}
            //}
            //else
            //{
            //    MessageBox.Show("安全門未關好!!\n請關閉好再重新加工!!");
            //    CloseDoorFlag = 1;
            //}

        }

        //private void BAutoMark_LaserMark(int i)
        //{
        //    ////平台走位, 輸入讀檔的xyz點位
        //    if (X0E == 1)
        //    {
        //        //if (RedLightRadioBtn.Checked == true)
        //        //{
        //        //    PLCAction.AutoStageMove(BIniXPointList[i], BIniYPointList[i], BIniZPointList[i] - 13, 100);
        //        //}
        //        //else if (AutoWeldRadBtn.Checked == true)
        //        //{
        //        PLCAction.AutoStageMove(BIniXPointList[i], BIniYPointList[i], BIniZPointList[i], 100);
        //        //}
        //        Console.WriteLine("B--第 " + i + " 區要加工了--");

        //        //double[] _ReadVal = PLCAction.ReadPLCDataRandom("D1000\nD1001\nD1010\nD1011\nD1020\nD1021", 6);//1-2-3  (6)--MainForm & StageForm--
        //        //// Thread.Sleep(5000);

        //        //while (true)
        //        //{
        //        //    if (_ReadVal[0] == BIniXPointList[i] && _ReadVal[1] == BIniYPointList[i] && _ReadVal[2] == BIniZPointList[i])
        //        //    {
        //        if (X0E == 1)
        //        {
        //            if (RedLightRadioBtn.Checked == true)
        //            {
        //                axMMMark1.StartMarking(3);
        //                //  break;
        //            }
        //            else if (AutoWeldRadBtn.Checked == true)
        //            {
        //                axMMMark1.StartMarking(1);
        //                //   break;
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("安全門未關好!!\n請關閉好再重新加工!!");
        //            CloseDoorFlag = 1;
        //        }
        //        //    }
        //        //}
        //    }
        //    else
        //    {
        //        MessageBox.Show("安全門未關好!!\n請關閉好再重新加工!!");
        //        CloseDoorFlag = 1;
        //    }

        //}
        void AWorkFlow()//while必須展開寫,不然不會等..
        {


            double[] _ReadVal;

            _ReadVal = new double[1];

            //if (CloseDoorFlag == 0)//關門狀態下才能繼續下一步
            //{
            //如果要加工一進來就先開氣了
            //鏡片吹氣保護on
            if (AutoWeldRadBtn.Checked == true)
                PLCAction.axActUtlType1.SetDevice("M1204", 1); //z

            //要等A推入到位後才能壓合
            //while (true)
            //{
            //    _ReadVal = PLCAction.ReadPLCDataRandom("M1131", 1);
            //    if (_ReadVal[0] == 1)
            //    {
            //        PLCAction.axActUtlType1.SetDevice("M1040", 0);
            //        break;
            //    }
            //}
            //Console.WriteLine("2.A已經進入到位");


            //A壓
            PLCAction.axActUtlType1.SetDevice("M1038", 1);
            _ReadVal = new double[1];
            while (true)
            {
                _ReadVal = PLCAction.ReadPLCDataRandom("M1138", 1);
                if (_ReadVal[0] == 1)
                {
                    PLCAction.axActUtlType1.SetDevice("M1038", 0);
                    break;
                }
            }
            Console.WriteLine("3.A已經頂升壓合,等 scanner 來加工,ScannerIsWork =" + ScannerIsWork);

            //進入scanner 自動加工流程
            while (true)
            {

                //if (ScannerUser == "A" && ScannerIsWork == 0)
                //{

                PLCAction.ManualSet(0.0, 3000, 3000, 1500, 100);
                Console.WriteLine("5.A SCANNER前往加工");
                AScannerWeldProcess();
                Console.WriteLine("5.A完成加工");

                //if (ScannerRank == "B")//如果A在排隊,給A用
                //{
                //    ScannerUser = "B";
                //    ScannerRank = "";
                //    Console.WriteLine("B在排隊,指定給B用了");
                //}
                //else //沒人排隊
                //{
                //    ScannerUser = "";
                //    ScannerRank = "";
                //    Console.WriteLine("都沒人排隊清空控制權");
                //}


                break;
                //}
            }
            //}
            //if (CloseDoorFlag == 0)
            //{
            //4. A鬆開
            PLCAction.axActUtlType1.SetDevice("M1039", 1);
            _ReadVal = new double[1];
            while (true)
            {
                _ReadVal = PLCAction.ReadPLCDataRandom("M1139", 1);
                if (_ReadVal[0] == 1)
                {
                    PLCAction.axActUtlType1.SetDevice("M1039", 0);
                    break;
                }
            }
            Console.WriteLine("6.A鬆開了");

            //5.A退出
            //EngAPortRadBtn.Checked = true;
            // TrackOut();
            //A出
            //PLCAction.axActUtlType1.SetDevice("M1030", 1);
            //_ReadVal = new double[1];
            //while (true)
            //{
            //    _ReadVal = PLCAction.ReadPLCDataRandom("M1130", 1);
            //    if (_ReadVal[0] == 1)
            //    {
            //        PLCAction.axActUtlType1.SetDevice("M1030", 0);

            //        break;
            //    }
            //}
            //GreenAPortBtnFlag = 0;
            //PCAPortBtnFlag = 0;

            //Console.WriteLine("7.A退出了");

            //鏡片吹氣保護off: 兩個都要推出來的狀況才能關
            double[] _AirOffCondition = PLCAction.ReadPLCDataRandom("M1130\nM1134", 2);
            if (_AirOffCondition[0] == 1 && _AirOffCondition[1] == 1)
                PLCAction.axActUtlType1.SetDevice("M1204", 0);
            //}

            MessageBox.Show("已完成銲接!!");

        }

        //void BWorkFlow()//while必須展開寫,不然不會等..
        //{
        //    double[] _ReadVal;
        //    _ReadVal = new double[1];

        //    if (CloseDoorFlag == 0)//關門狀態下才能繼續下一步
        //    {
        //        //鏡片吹氣保護on
        //        if (AutoWeldRadBtn.Checked == true)
        //            PLCAction.axActUtlType1.SetDevice("M1204", 1); //z

        //        //要等B推入到位後才能壓合
        //        while (true)
        //        {
        //            _ReadVal = PLCAction.ReadPLCDataRandom("M1135", 1);
        //            if (_ReadVal[0] == 1)
        //            {
        //                PLCAction.axActUtlType1.SetDevice("M1041", 0);
        //                break;
        //            }
        //        }
        //        Console.WriteLine("2.B已經進入到位");
        //        // 2. B頂升

        //        PLCAction.axActUtlType1.SetDevice("M1036", 1);
        //        _ReadVal = new double[1];
        //        while (true)
        //        {
        //            _ReadVal = PLCAction.ReadPLCDataRandom("M1136", 1);
        //            if (_ReadVal[0] == 1)
        //            {
        //                PLCAction.axActUtlType1.SetDevice("M1036", 0);
        //                break;
        //            }
        //        }
        //        Console.WriteLine("3.B已經頂升壓合,等 scanner 來加工,ScannerIsWork =" + ScannerIsWork);
        //        //進入scanner 自動加工流程
        //        while (true)
        //        {
        //            if (ScannerUser == "B" && ScannerIsWork == 0)
        //            {


        //                PLCAction.ManualSet(0.0, 3000, 3000, 1500, 100);
        //                Console.WriteLine("5.B SCANNER前往加工");
        //                BScannerWeldProcess();
        //                Console.WriteLine("5.B完成加工");

        //                if (ScannerRank == "A")//如果A在排隊,給A用
        //                {
        //                    ScannerUser = "A";
        //                    ScannerRank = "";
        //                    Console.WriteLine("A在排隊,指定給A用了");
        //                }
        //                else //沒人排隊
        //                {
        //                    ScannerUser = "";
        //                    ScannerRank = "";
        //                    Console.WriteLine("都沒人排隊清空控制權");
        //                }

        //                break;
        //            }

        //        }

        //    }
        //    if (CloseDoorFlag == 0)
        //    {
        //        Thread.Sleep(50);
        //        //4. B鬆開
        //        //B鬆開
        //        PLCAction.axActUtlType1.SetDevice("M1037", 1);
        //        _ReadVal = new double[1];
        //        while (true)
        //        {
        //            _ReadVal = PLCAction.ReadPLCDataRandom("M1137", 1);
        //            if (_ReadVal[0] == 1)
        //            {
        //                PLCAction.axActUtlType1.SetDevice("M1037", 0);
        //                break;
        //            }
        //        }
        //        Console.WriteLine("6.B鬆開了");
        //        //5.B退出
        //        //TrackOut();
        //        //B出
        //        PLCAction.axActUtlType1.SetDevice("M1034", 1);
        //        _ReadVal = new double[1];
        //        while (true)
        //        {
        //            _ReadVal = PLCAction.ReadPLCDataRandom("M1134", 1);
        //            if (_ReadVal[0] == 1)
        //            {
        //                PLCAction.axActUtlType1.SetDevice("M1034", 0);

        //                break;
        //            }
        //        }
        //        GreenBPortBtnFlag = 0;
        //        PCBPortBtnFlag = 0;

        //        Console.WriteLine("7.B退出了");

        //        //鏡片吹氣保護on
        //        PLCAction.axActUtlType1.SetDevice("M1204", 0); //z
        //    }

        //}



        //private void OperatorBBtn_Click(object sender, EventArgs e)
        //{
        //    if (X0E == 0)
        //    {
        //        CloseDoorFlag = 1;
        //        MessageBox.Show("安全門未關好!!\n請關閉好再重新加工!!");
        //    }
        //    else if (X0E == 1)
        //    {

        //        CloseDoorFlag = 0;
        //        if (BRecipeName == "")
        //        {
        //            MessageBox.Show("未指定 B port recipe!");
        //            return;
        //        }
        //        if (AutoWeldRadBtn.Checked == true)
        //        {
        //            DialogResult AutoLaserChkResult = MessageBox.Show(this, "確定要激發雷射做加工嗎?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //            if (AutoLaserChkResult == DialogResult.No)
        //                return;
        //        }
        //       // InitialBShowPicBox();

        //        PCBPortBtnFlag = 1;
        //        if (GreenAPortBtnFlag == 0)
        //        {

        //            double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1134", 1);//B推出到位
        //            if (_ReadVal[0] == 1)
        //            {

        //                BPortStartWorkThread = new Thread(new ThreadStart(BPortStartWork));
        //                Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
        //                BPortStartWorkThread.Start();
        //            }
        //            else
        //            {
        //                MessageBox.Show("B還沒推出來!");
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("已經按了B實體按鈕,GreenAPortBtnFlag=" + GreenBPortBtnFlag);
        //        }
        //    }

        ////}
        //void BPortStartWork()
        //{

        //    //1.按壓 B 區按鈕,綠色按鈕發亮
        //    PLCAction.axActUtlType1.SetDevice("M1139", 1);

        //    //2.按鈕按完後自動入料,入料完成後plc自動解除1139
        //    PLCAction.axActUtlType1.SetDevice("M1041", 1);
        //    Console.WriteLine("=============B開始加工=============");
        //    Console.WriteLine("1.B已經確認加工推入");

        //    if (ScannerUser == "")//如果沒人用 scanner
        //    {
        //        ScannerUser = "B";//A申請使用SCANEER
        //        Console.WriteLine("沒人占用, B註冊 scanner 了");
        //    }
        //    else if (ScannerUser == "A") //如果B再用
        //    {
        //        ScannerRank = "B";//A申請使用SCANEER
        //        Console.WriteLine("A占用, B排隊");

        //    }

        //    NowProcessPort = 1; //B使用中
        //    BWorkFlow();

        //}

        private void ControlTabBox_Click(object sender, EventArgs e)
        {

        }

        private void ControlTabBox_Selected(object sender, TabControlEventArgs e)
        {
            //if (ControlTabBox.SelectedIndex == 1)
            //{
            //    //if ((ARecipeName == "" && ModeARadBtn.Checked == true) || (BRecipeName == "" && ModeBRadBtn.Checked == true)
            //    //    || ((ARecipeName == "" || BRecipeName == "") && ModeABRadBtn.Checked == true))
            //    //{
            //    //    MessageBox.Show("未指定完整 RECIPE!");
            //    //    ControlTabBox.SelectedIndex = 0;
            //    //    return;
            //    //}
            //    //else
            //    //    EnableRepairGrpBox(1);//補銲打開
            //}
            //else
            //    EnableRepairGrpBox(0);
        }

        private void ControlTabBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ControlTabBox.SelectedIndex == 2)
            {
                if (SetAtxtBox.Text != "")
                {
                    //帶入Arecipe XY區域編號
                    ManualRepairXcomboBox.Items.Clear();
                    for (int i = 0; i < AIniAreaNumXList.Max(); i++)
                        ManualRepairXcomboBox.Items.Add(i + 1);

                    ManualRepairYcomboBox.Items.Clear();
                    for (int i = 0; i < AIniAreaNumYList.Max(); i++)
                        ManualRepairYcomboBox.Items.Add(i + 1);
                }
                else
                {
                    MessageBox.Show("未選擇 recipe!");

                }
            }

        }

        private void EngStopBtn_Click(object sender, EventArgs e)
        {

        }

        private void ErrResetBtn_Click(object sender, EventArgs e)
        {
            //綠色實體按鈕復原
            //清調開門開關


            PLCAction.axActUtlType1.SetDevice("M1138", 0);
            PLCAction.axActUtlType1.SetDevice("M1139", 0);
            PLCAction.axActUtlType1.SetDevice("M1140", 0);
            PLCAction.axActUtlType1.SetDevice("M1141", 0);
            PCAPortBtnFlag = 0;
            PCBPortBtnFlag = 0;
            GreenAPortBtnFlag = 0;
            GreenBPortBtnFlag = 0;




            PLCAction.ErrorReset();
            PLCAction.MainControl(1);

            MessageBox.Show("Error Reset完成!請記得做:\n1.消除雷射UI與PLC alarm\n2.復原出光M1205\n2.歸HOME");

            AllUIModeUnable();
            ScannerIsWork = 0;
            GoHomeBtn.Enabled = true;



        }
        void StopFrozenUI(int OnOff)
        {
            if (OnOff == 0)
            {
                //SetPortGrpBox.Enabled = false;
                SetIniGrpBox.Enabled = false;
                RepairGrpBox.Enabled = false;
                ControlTabBox.Enabled = false;
                //ExceptionGrpBox.Enabled = false;
            }
            else if (OnOff == 1)
            {
                // SetPortGrpBox.Enabled = true;
                SetIniGrpBox.Enabled = true;
                RepairGrpBox.Enabled = true;
                ControlTabBox.Enabled = true;
                // ExceptionGrpBox.Enabled = true;



            }

        }

        private void OperatorStopBtn_Click(object sender, EventArgs e)
        {
            // 1.急停
            PLCAction.axActUtlType1.SetDevice("M1202", 1);

            //2.伺服馬達 off
            PLCAction.axActUtlType1.SetDevice("M1207", 0);
        }

        private void OPResetErrBtn_Click(object sender, EventArgs e)
        {
            ErrResetBtn_Click(sender, e);

        }

        //private void SetBiniBtn_Click_1(object sender, EventArgs e)
        //{
        //    NowProcessPort = 1;

        //    string OpenFilePath;

        //    OpenFileDialog = new OpenFileDialog();
        //    OpenFileDialog.InitialDirectory = ".\\";
        //    OpenFileDialog.Filter = "ini files (*.ini)|*.ini";
        //    OpenFileDialog.FilterIndex = 2;
        //    OpenFileDialog.RestoreDirectory = true;
        //    DialogResult result = OpenFileDialog.ShowDialog();
        //    OpenFilePath = OpenFileDialog.FileName;

        //    SetBtxtBox.Text = OpenFilePath;

        //    if (result == DialogResult.OK && OpenFileDialog.FileName != "")
        //    {

        //        //擷取檔名
        //        BRecipeName = FileIO.GetFileName(OpenFilePath);
        //       // BRcpNameLabel.Text = BRecipeName;

        //        //ReadSection
        //        List<string> SecList = new List<string>();
        //        SecList = FileIO.IniDoubleReadSec(OpenFileDialog.FileName);
        //        BIniAreaCnt = SecList.Count();

        //        BIniXPointList = new List<double>();
        //        BIniYPointList = new List<double>();
        //        BIniZPointList = new List<double>();

        //        BIniMMFileList = new List<string>();

        //        BIniAreaNumList = new List<string>();
        //        BIniAreaNumXList = new List<int>();
        //        BIniAreaNumYList = new List<int>();

        //        BIniPicFileList = new List<string>();

        //        //B XYZ座標
        //        BIniXPointList = FileIO.IniDoubleReadKey(SecList, "XPoint", OpenFileDialog.FileName);
        //        BIniYPointList = FileIO.IniDoubleReadKey(SecList, "YPoint", OpenFileDialog.FileName);
        //        BIniZPointList = FileIO.IniDoubleReadKey(SecList, "ZPoint", OpenFileDialog.FileName);

        //        //B MM file
        //        BIniMMFileList = FileIO.IniStrReadKey(SecList, "MMFile", OpenFileDialog.FileName);
        //        BIniPicFileList = FileIO.IniStrReadKey(SecList, "PicFile", OpenFileDialog.FileName);
        //        BIniAreaNumList = FileIO.IniStrReadKey(SecList, "AreaNum", OpenFileDialog.FileName);

        //        //動畫順序
        //        BIniAreaNumXList = FileIO.IniAreaNumGet(BIniAreaNumList, "x");
        //        BIniAreaNumYList = FileIO.IniAreaNumGet(BIniAreaNumList, "y");

        //        //------------------- load 第一個 MM 檔----------------------------
        //        RedLightOff();
        //        axMMMark1.LoadFile(BIniMMFileList[0]);



        //        //-------------------畫第一張動畫圖-----------------------------
        //        //抓長寬最多各幾單位,才能分配圖片大小
        //        int w_cnt = -1, h_cnt = -1;
        //        for (int i = 0; i < BIniAreaCnt; i++)
        //        {
        //            if (BIniAreaNumXList[i] > w_cnt) w_cnt = BIniAreaNumXList[i];
        //            if (BIniAreaNumYList[i] > h_cnt) h_cnt = BIniAreaNumYList[i];
        //        }


        //        //預估最佳化的圖片大小
        //        double UnitW = BShowProgressPicBox.Width / w_cnt;
        //        double UnitH = BShowProgressPicBox.Height / h_cnt;
        //        double Unit;
        //        if (UnitW > UnitH) Unit = UnitH;
        //        else Unit = UnitW;


        //        //評估置中offset多少
        //        BOffsetW = 0;
        //        BOffsetH = 0;
        //        if ((int)(BShowProgressPicBox.Width - Unit * w_cnt) < 5)
        //        {
        //            double MiddleH = BShowProgressPicBox.Height / 2;
        //            BOffsetH = MiddleH - Unit * h_cnt / 2;

        //        }
        //        else if ((int)(BShowProgressPicBox.Height - Unit * h_cnt) < 5)
        //        {
        //            double MiddleW = BShowProgressPicBox.Width / 2;
        //            BOffsetW = MiddleW - Unit * w_cnt / 2;

        //        }

        //        Bw = Unit;
        //        Bh = Unit;

        //        //畫空白外框
        //        Bbmp = new Bitmap(BShowProgressPicBox.Width, BShowProgressPicBox.Height);
        //        Bbmp = DrawBEmpthRect(Bbmp, BIniAreaCnt);
        //        BShowProgressPicBox.Image = Bbmp;
        //        BShowProgressPicBox.Refresh();
        //        GC.Collect();

        //    }



        //}

        private void OPResetErrBtn_Click()
        {

        }

        private void RepairGrpBox_Enter(object sender, EventArgs e)
        {

        }

        private void RepairStopBtn_Click(object sender, EventArgs e)
        {

        }
        void EnableWeldBtn()
        {
            //if (ModeARadBtn.Checked == true)
            //{
            //    OperatorABtn.Enabled = true;
            //    OperatorBBtn.Enabled = false;
            //}
            //else if (ModeBRadBtn.Checked == true)
            //{
            //    OperatorABtn.Enabled = false;
            //    OperatorBBtn.Enabled = true;
            //}

            //else if (ModeABRadBtn.Checked == true)
            //{
            //    OperatorABtn.Enabled = true;
            //    OperatorBBtn.Enabled = true;
            //}


        }
        private void AutoWeldRadBtn_Click(object sender, EventArgs e)
        {
            //RepairRadioBtn.Checked = false;
            AutoWeldRadBtn.Checked = true;
            RedLightRadioBtn.Checked = false;

            EnableWeldBtn();

            EnableRepairGrpBox(0);
        }

        private void RedLightRadioBtn_Click(object sender, EventArgs e)
        {
            //RepairRadioBtn.Checked = false;
            AutoWeldRadBtn.Checked = false;
            RedLightRadioBtn.Checked = true;

            //if (ModeARadBtn.Checked== true)ModeARadBtn_Click(sender, e);
            //else if (ModeBRadBtn.Checked == true) ModeBRadBtn_Click(sender, e);
            //else if (ModeABRadBtn.Checked == true) ModeABRadBtn_Click(sender, e);

            EnableWeldBtn();
            EnableRepairGrpBox(0);
        }

        private void RepairABcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RepairABcomboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //if (RepairABcomboBox.SelectedItem.ToString() == "A")
            //{
            //    //帶入Arecipe XY區域編號
            //    RepairXcomboBox.Items.Clear();
            //    for (int i = 0; i < AIniAreaNumXList.Max(); i++)
            //        RepairXcomboBox.Items.Add(i + 1);

            //    RepairYcomboBox.Items.Clear();
            //    for (int i = 0; i < AIniAreaNumYList.Max(); i++)
            //        RepairYcomboBox.Items.Add(i + 1);


            //}
            //else if (RepairABcomboBox.SelectedItem.ToString() == "B")
            //{
            //帶入Brecipe XY區域編號
            RepairXcomboBox.Items.Clear();
            for (int i = 0; i < BIniAreaNumXList.Max(); i++)
                RepairXcomboBox.Items.Add(i + 1);

            RepairYcomboBox.Items.Clear();
            for (int i = 0; i < BIniAreaNumYList.Max(); i++)
                RepairYcomboBox.Items.Add(i + 1);


            //}

            EnableRepairGrpBox(2);
            EnableRepairBtn(0);
        }

        private void RepairStageMoveBtn_Click(object sender, EventArgs e)
        {
            if (//RepairABcomboBox.SelectedItem != null
                //&& 
                ManualRepairXcomboBox.SelectedItem != null
                && ManualRepairYcomboBox.SelectedItem != null)
            {
                RepairMoveThread = new Thread(new ThreadStart(RepairMove));
                Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                RepairMoveThread.Start();
            }
            else
            {
                MessageBox.Show("請設定完整要補銲的區域!");

            }


        }
        void RepairMove()
        {
            int RepairAreaIdx;
            //if (RepairABcomboBox.SelectedItem.ToString() == "A")
            //{

            RepairAreaIdx = FindRepairIdx();
            axMMMark1.StopMarking();
            Thread.Sleep(200);
            axMMMark1.LoadFile("");

            string AMMName = FileIO.GetFileName(AIniMMFileList[RepairAreaIdx]);
            MMfileNameLbl.Text = AMMName;

            Bitmap Abmp = new Bitmap(AShowProgressPicBox.Width, AShowProgressPicBox.Height);
            AShowProgressPicBox.Image = Abmp;
            //畫出該區示意圖
            //3.畫動畫
            Abmp = DrawItems.DrawBlock(Abmp,
                  Aw * (AIniAreaNumXList[RepairAreaIdx] - 1) + AOffsetW, Ah * (AIniAreaNumYList[RepairAreaIdx] - 1) + AOffsetH,
                  Aw * AIniAreaNumXList[RepairAreaIdx] + AOffsetW, Ah * (AIniAreaNumYList[RepairAreaIdx] - 1) + AOffsetH,
                  Aw * (AIniAreaNumXList[RepairAreaIdx] - 1) + AOffsetW, Ah * AIniAreaNumYList[RepairAreaIdx] + AOffsetH,
                  AIniPicFileList[RepairAreaIdx]);

            //補畫方框
            Abmp = DrawAEmpthRect(Abmp, AIniAreaCnt);
            AShowProgressPicBox.Image = Abmp;
            GC.Collect();

            //A如果還沒進料 先幫忙進料 有進就不用動
            //string RepairSelectPort = RepairABcomboBox.SelectedItem.ToString();
            double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1131", 1);//是否推入到位
            //if (_ReadVal[0] == 0)//還沒推入到位
            //{
            //    _ReadVal = new double[1];
            //    _ReadVal = PLCAction.ReadPLCDataRandom("M1130", 1);//是否推出到位
            //    if (_ReadVal[0] == 1) //推出到位就直接推入
            //    {
            //        PLCAction.TrackIn(RepairSelectPort);
            //    }
            //    else
            //        Console.WriteLine(RepairSelectPort + "Port尚未到達上料位置!請先移動至上料位置");//沒推入 手動處理

            //}
            //else
            //{
            //    Console.WriteLine(RepairSelectPort + "Port已經推入到位!");

            //}
            //移動XYZ
            PLCAction.ManualSet(0.0, 3000, 3000, 1500, 100);
            PLCAction.AutoStageMove(AIniXPointList[RepairAreaIdx], AIniYPointList[RepairAreaIdx], AIniZPointList[RepairAreaIdx], 100);

            axMMMark1.LoadFile(AIniMMFileList[RepairAreaIdx]);

            //}
            //else if (RepairABcomboBox.SelectedItem.ToString() == "B")
            //{
            //RepairAreaIdx = FindRepairIdx();
            //axMMMark1.StopMarking();
            //axMMMark1.LoadFile("");

            //string BMMName = FileIO.GetFileName(BIniMMFileList[RepairAreaIdx]);
            //MMfileNameLbl.Text = BMMName;

            //Bitmap Bbmp = new Bitmap(BShowProgressPicBox.Width, BShowProgressPicBox.Height);
            //BShowProgressPicBox.Image = Bbmp;
            ////畫出該區示意圖
            ////3.畫動畫
            //Bbmp = DrawItems.DrawBlock(Bbmp,
            //      Bw * (BIniAreaNumXList[RepairAreaIdx] - 1) + BOffsetW, Bh * (BIniAreaNumYList[RepairAreaIdx] - 1) + BOffsetH,
            //      Bw * BIniAreaNumXList[RepairAreaIdx] + BOffsetW, Bh * (BIniAreaNumYList[RepairAreaIdx] - 1) + BOffsetH,
            //      Bw * (BIniAreaNumXList[RepairAreaIdx] - 1) + BOffsetW, Bh * BIniAreaNumYList[RepairAreaIdx] + BOffsetH,
            //      BIniPicFileList[RepairAreaIdx]);

            ////補畫方框
            //Bbmp = DrawBEmpthRect(Bbmp, BIniAreaCnt);
            //BShowProgressPicBox.Image = Bbmp;
            //GC.Collect();


            ////B如果還沒進料 先幫忙進料 有進就不用動
            //string RepairSelectPort = RepairABcomboBox.SelectedItem.ToString();
            //double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1135", 1);//是否推入到位
            //if (_ReadVal[0] == 0)//還沒推入到位
            //{
            //    _ReadVal = new double[1];
            //    _ReadVal = PLCAction.ReadPLCDataRandom("M1134", 1);//是否推出到位
            //    if (_ReadVal[0] == 1) //推出到位就直接推入
            //    {
            //        PLCAction.TrackIn(RepairSelectPort);
            //    }
            //    else
            //        Console.WriteLine(RepairSelectPort + "Port尚未到達上料位置!請先移動至上料位置");//沒推入 手動處理

            //}
            //else
            //{
            //    Console.WriteLine(RepairSelectPort + "Port已經推入到位!");

            //}

            //PLCAction.ManualSet(0.0, 3000, 3000, 1500, 100);
            //PLCAction.AutoStageMove(BIniXPointList[RepairAreaIdx], BIniYPointList[RepairAreaIdx], BIniZPointList[RepairAreaIdx], 100);
            //axMMMark1.LoadFile(BIniMMFileList[RepairAreaIdx]);

            //}

            EnableRepairBtn(1);
            RedLightOff();
            //MessageBox.Show("已移至該區位置!");

        }


        private int FindRepairIdx()
        {
            int Idx = 0;
            int XAreaNum = Convert.ToInt32(ManualRepairXcomboBox.SelectedItem.ToString());
            int YAreaNum = Convert.ToInt32(ManualRepairYcomboBox.SelectedItem.ToString());

            //if (RepairABcomboBox.Text == "A")
            //{
            for (int i = 0; i < AIniAreaCnt; i++)
            {
                if (AIniAreaNumXList[i] == XAreaNum && AIniAreaNumYList[i] == YAreaNum)
                {
                    Idx = i;
                    break;
                }
            }
            //}
            //else
            //{
            //    for (int i = 0; i < BIniAreaCnt; i++)
            //    {
            //        if (BIniAreaNumXList[i] == XAreaNum && BIniAreaNumYList[i] == YAreaNum)
            //        {
            //            Idx = i;
            //            break;
            //        }
            //    }


            //}
            return Idx;

        }

        private void WeldSystem_Shown(object sender, EventArgs e)
        {

        }

        private void RepairRedLightBtn_Click(object sender, EventArgs e)
        {
            //用紅光要比設定檔下降13mm
            int RepairAreaIdx = FindRepairIdx();
            //if (RepairABcomboBox.SelectedItem.ToString() == "A")
            //{
            PLCAction.AutoStageMove(AIniXPointList[RepairAreaIdx], AIniYPointList[RepairAreaIdx], AIniZPointList[RepairAreaIdx] - 13, 100);
            //}
            //else if (RepairABcomboBox.SelectedItem.ToString() == "B")
            //{
            //    PLCAction.AutoStageMove(BIniXPointList[RepairAreaIdx], BIniYPointList[RepairAreaIdx], BIniZPointList[RepairAreaIdx] - 13, 100);
            //}
            int RedLightStatus;
            PLCAction.axActUtlType1.GetDevice("M1206", out RedLightStatus);

            if (RedLightStatus == 0)//開啟紅光
            {

                PLCAction.axActUtlType1.SetDevice("M1206", 1);
                axMMMark1.StopMarking();
                axMMMark1.SetMarkSelect(1);
                axMMMark1.StartMarking(3);

                RepairRedLightBtn.BackColor = Color.Green;

            }
            else //關閉紅光
            {
                RedLightOff();
            }



        }
        private void RedLightOff()
        {
            //防呆停止紅光預覽
            RepairRedLightBtn.BackColor = Color.Thistle;
            PLCAction.axActUtlType1.SetDevice("M1018", 0);
            axMMMark1.StopMarking();
            Thread.Sleep(100);

        }
        private void RepairLaserBtn_Click(object sender, EventArgs e)
        {
            //改成用mm file不用圖層故全部mark掉
            //DialogResult AutoLaserChkResult = MessageBox.Show(this, "確定要激發雷射做補銲嗎?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (AutoLaserChkResult == DialogResult.No)
            //    return;

            //axMMMark1.SelectClearObjects();
            //RedLightOff();

            //string SettingFilePath = @"D:\ITRI\TestCase\流水線測試檔\PLCRepairInfo.txt";
            //string line;
            //System.IO.StreamReader sr = new System.IO.StreamReader(SettingFilePath, Encoding.GetEncoding(950));
            //sr = System.IO.File.OpenText(SettingFilePath);


            //List<DefectRepairInfo> DefectRepairList = new List<DefectRepairInfo>();
            //DefectRepairInfo PT = new DefectRepairInfo();

            ////讀取DEFECT資訊
            //while ((line = sr.ReadLine()) != null)
            //{
            //    PT = new DefectRepairInfo();
            //    if (line.IndexOf("Defect") < 0)//不是表頭
            //    {
            //        string[] words = line.Split(',');
            //        foreach (var word in words)
            //        {
            //            PT.RepairAreaIdx = words[0];
            //            PT.RepairLayerName = words[1];
            //            PT.RepairObjName = words[2];
            //        }
            //        DefectRepairList.Add(PT);
            //    }

            //}



            ////要改成 同一個檔案裡的要一起做不能被打散
            //string RepairObjName = "";
            //List<string> UsedMM = new List<string>();

            //foreach (var DefectPT in DefectRepairList)
            //{
            //    //列入已處理的mm file
            //    UsedMM.Add(AIniMMFileList[Convert.ToInt32(DefectPT.RepairAreaIdx) - 1]);

            //    //讀取對應MM FILE
            //    axMMMark1.LoadFile(AIniMMFileList[Convert.ToInt32(DefectPT.RepairAreaIdx) - 1]);
            //    MMfileNameLbl.Text = AIniMMFileList[Convert.ToInt32(DefectPT.RepairAreaIdx) - 1];


            //    //抓取該mm檔的物件
            //    CatchEZMObjInfo();

            //    //
            //    foreach (var DefectPT2 in DefectRepairList)
            //    {
            //        //
            //        foreach (var ezmPT in AllEZMLayerObjList)
            //        {

            //            if (ezmPT.LayerName.IndexOf(DefectPT.RepairLayerName) > 0 && ezmPT.ObjName != "")
            //            {

            //                if (ezmPT.ObjName.Substring(ezmPT.ObjName.LastIndexOf("-") + 1, ezmPT.ObjName.Length - ezmPT.ObjName.LastIndexOf("-") - 1)
            //                       == DefectPT.RepairObjName)
            //                {
            //                    Console.WriteLine("ezm=" + ezmPT.ObjName);
            //                    RepairObjName = ezmPT.ObjName;
            //                    axMMMark1.SelectAddObject(RepairObjName);
            //                    string objname = "";
            //                    for (int i = 0; i < axMMMark1.SelectGetCount() + 1; i++)
            //                    {
            //                        axMMMark1.SelectEnum(i, ref objname);
            //                        Console.WriteLine("=>select items[" + i + "]= " + objname);
            //                    }
            //                    Thread.Sleep(800);
            //                    //ActionMode = 2; //自動補銲模式只需要開頭問一次就好
            //                    //ManualLaserStart();
            //                    //ActionMode = 0; //恢復

            //                    //axMMMark1.StopMarking();
            //                    //axMMMark1.SetMarkSelect(1);
            //                    //axMMMark1.StartMarking(1);
            //                    Console.WriteLine("補銲了");

            //                    Thread.Sleep(200);

            //                }
            //            }
            //        }// foreach
            //    }//foreach defect


            //}

            ////自動補焊,執行前問一次就好
            //MessageBox.Show(this, "已完本次流水線補銲!");
            /***************************自動流水線**************************************/


        }
        void CatchEZMObjInfo()
        {
            RepairPListBox.Items.Clear();
            RepairAreaListBox.Items.Clear();
            //AutoLayerLbl.Text = "";

            AllEZMLayerObjList = new List<EZMLayerObjInfo>();
            string LayerName = "";
            string ObjName = "";
            EZMLayerObjInfo NewObj = new EZMLayerObjInfo();

            for (int i = 0; i <= axMMEdit1.GetLayerCount(); i++)
            {
                LayerName = "";
                axMMEdit1.GetLayerName(i, ref LayerName);

                for (int j = 0; j <= axMMEdit1.GetChildObjectCount(LayerName); j++)
                {
                    NewObj = new EZMLayerObjInfo();
                    ObjName = "";
                    axMMEdit1.GetChildObjectName(LayerName, j, ref ObjName);
                    NewObj.LayerName = LayerName;
                    NewObj.LayerNum = i;
                    NewObj.ObjName = ObjName;
                    NewObj.ObjNum = j;
                    AllEZMLayerObjList.Add(NewObj);
                }

            }

            RepairAreaListBox.Items.Clear();
            List<string> TempUsedList = new List<string>();
            int TempCnt = 0;
            for (int i = 0; i < AllEZMLayerObjList.Count(); i++)
            {
                TempCnt = 0;
                for (int j = 0; j < TempUsedList.Count(); j++)
                {
                    if (TempUsedList[j] == AllEZMLayerObjList[i].LayerName)
                        TempCnt++;
                }
                if (TempCnt == 0)
                {
                    if (AllEZMLayerObjList[i].LayerName != "" && AllEZMLayerObjList[i].LayerName.IndexOf("-") > 0)
                    {
                        RepairAreaListBox.Items.Add(AllEZMLayerObjList[i].LayerName);
                        TempUsedList.Add(AllEZMLayerObjList[i].LayerName);
                    }
                }
                //if (AllEZMLayerObjList[i].LayerName.IndexOf("weld") > 0)
                //AutoLayerLbl.Text = AllEZMLayerObjList[i].LayerName;
            }

            RepairAreaListBox.SelectedIndex = 1;



        }
        private void EngRepairChkBox_Click(object sender, EventArgs e)
        {
            //if (EngRepairChkBox.Checked == true)
            //{

            //    //if ((ARecipeName == "" && ModeARadBtn.Checked == true) || (BRecipeName == "" && ModeBRadBtn.Checked == true)
            //    //    || ((ARecipeName == "" || BRecipeName == "") && ModeABRadBtn.Checked == true))
            //    if (ARecipeName == "")
            //    {
            //        MessageBox.Show("未指定完整 RECIPE!");
            //        return;
            //    }
            //    else
            //        EnableRepairGrpBox(1);//補銲打開


            //}
            //else
            //    EnableRepairGrpBox(0);
        }

        private void KeyUIRadBtn_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RepairTrackOutBtn_Click(object sender, EventArgs e)
        {
            //if (RepairABcomboBox.SelectedItem.ToString() == "A")
            //{
            //    NowProcessPort = 0;
            //    EngOutBtn_Click(sender, e);
            //}
            //else if (RepairABcomboBox.SelectedItem.ToString() == "B")
            //{
            //    NowProcessPort = 1;
            EngOutBtn_Click(sender, e);
            //}
        }

        private void RepairABcomboBox_DropDown(object sender, EventArgs e)
        {
            EnableRepairBtn(0);
        }

        private void RepairXcomboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void RepairYcomboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void RepairXcomboBox_DropDown(object sender, EventArgs e)
        {
            EnableRepairBtn(0);
        }

        private void RepairYcomboBox_DropDown(object sender, EventArgs e)
        {
            EnableRepairBtn(0);
        }

        private void GoHomeBtn_Click(object sender, EventArgs e)
        {

        }

        private void GoHomeBtn_MouseDown(object sender, MouseEventArgs e)
        {
            PLCAction.axActUtlType1.SetDevice("M1023", 1); //z
            PLCAction.axActUtlType1.SetDevice("M1003", 1); //x
            PLCAction.axActUtlType1.SetDevice("M1013", 1); //y

            // StopFrozenUI(1);


        }

        private void GoHomeBtn_MouseUp(object sender, MouseEventArgs e)
        {
            PLCAction.axActUtlType1.SetDevice("M1023", 0); //z
            PLCAction.axActUtlType1.SetDevice("M1003", 0); //x
            PLCAction.axActUtlType1.SetDevice("M1013", 0); //y
        }

        private void EngM1205Btn_Click(object sender, EventArgs e)
        {
            OpenM1205();
        }

        private void RepairRlsBtn_Click(object sender, EventArgs e)
        {
            //if (RepairABcomboBox.SelectedItem.ToString() == "A")
            //{
            //    NowProcessPort = 0;
            //    ReleaseBtn_Click(sender, e);
            //}
            //else if (RepairABcomboBox.SelectedItem.ToString() == "B")
            //{
            //    NowProcessPort = 1;
            //    ReleaseBtn_Click(sender, e);
            //}
        }

        private void AirBtn_Click(object sender, EventArgs e)
        {

            double[] _AirData = PLCAction.ReadPLCDataRandom("M1204", 1);
            if (_AirData[0] == 0)
            {
                PLCAction.axActUtlType1.SetDevice("M1204", 1); //z
                AirBtn.BackColor = Color.Green;
            }
            else if (_AirData[0] == 1)
            {
                PLCAction.axActUtlType1.SetDevice("M1204", 0); //z
                AirBtn.BackColor = Color.White;
            }



        }

        private void AShowProgressPicBox_Click(object sender, EventArgs e)
        {

        }

        private void AutoRepairRadBtn_Click(object sender, EventArgs e)
        {
            ManualRepairAreaGrpBox.Enabled = false;
            ManualRepairLayerRadBtn.Enabled = false;
        }

        private void ManualRepairRadBtn_Click(object sender, EventArgs e)
        {
            ManualRepairAreaGrpBox.Enabled = true;
            ManualRepairLayerRadBtn.Enabled = true;
        }

        private void ManualRepairMoveBtn_Click(object sender, EventArgs e)
        {
            RepairStageMoveBtn_Click(sender, e);
        }
        List<RepairDictionary> RepairDataProcess(double[] RepairData)
        {
            List<RepairDictionary> RepairDicList = new List<RepairDictionary>();
            RepairDictionary RepairP;
            for (int i = 0; i < RepairData.Count(); i++)
            {
                RepairP = new RepairDictionary();
                double temp = Math.Floor(Convert.ToDouble(RepairData[i]) / 100.0);
                if (temp == 0)
                {
                    RepairP.DicVisionAreaID = (Math.Floor(RepairData[i] / 10.0)).ToString();
                    RepairP.DicVisionWeldID = (RepairData[i] - (Math.Floor(Convert.ToDouble(RepairP.DicVisionAreaID)) * 10)).ToString();
                }
                else
                {
                    RepairP.DicVisionAreaID = (Math.Floor(RepairData[i] / 100.0)).ToString();
                    RepairP.DicVisionWeldID = (RepairData[i] - (Math.Floor(Convert.ToDouble(RepairP.DicVisionAreaID)) * 100)).ToString();
                }

                RepairDicList.Add(RepairP);

            }
            return RepairDicList;

        }
        private void FlowRepairMarkBtn_Click(object sender, EventArgs e)
        {

            //3.第幾區
            //4.第幾點
            double[] RepairData = PLCAction.ReadPLCDataRandom16("D2260\nD2261\nD2262\nD2263\nD2264\nD2265\nD2266\nD2267\nD2268\nD2269\nD2270\nD2271", 12);
            List<RepairDictionary> RepairDicList = new List<RepairDictionary>();
            RepairDicList = RepairDataProcess(RepairData);
            string[] row;
            RepairDataGridView.Rows.Clear();
            for (int i = 0; i < RepairDicList.Count(); i++)
            {
                row = new string[] { "", "" };
                row = new string[] { RepairDicList[i].DicVisionAreaID, RepairDicList[i].DicVisionWeldID };
                RepairDataGridView.Rows.Add(row);

            }
            //1.A還是B
            string ModuleSide = "A";
            //2.第幾次銲
            string WeldCnt = "1";
            for (int i = 0; i < RepairDicList.Count(); i++)
            {
                RepairDicList[i].DicSide = ModuleSide;
                RepairDicList[i].DicWeldCnt = WeldCnt;
                RepairDicList[i].DicRepairOK1 = "0";
                RepairDicList[i].DicRepairOK2 = "0";
            }
            //5.查表:
            //(1)MM FILE
            //(2)第幾個物件
            List<RepairDictionary> ConnectDict = new List<RepairDictionary>();
            ConnectDict = FileIO.ReadWeldDictionay(ConnectDict);
            string RepairMMFilePath;

            for (int i = 0; i < RepairDicList.Count(); i++)
            {
                for (int j = 0; j < ConnectDict.Count(); j++)
                {
                    if (ConnectDict[j].DicVisionAreaID == RepairDicList[i].DicVisionAreaID && ConnectDict[j].DicVisionWeldID == RepairDicList[i].DicVisionWeldID)
                    {
                        RepairMMFilePath = @"D:\18650\HH_Repair";
                        RepairDicList[i].DicMMName = ConnectDict[j].DicMMName;
                        RepairDicList[i].DicWeldObjID = ConnectDict[j].DicWeldObjID;

                        //每一次補銲都對應到不同的 MM FILE 檔名
                        RepairMMFilePath = RepairMMFilePath + "\\Repair" + WeldCnt + "\\" + RepairDicList[i].DicMMName + "_r" + WeldCnt + ".ezm";
                        RepairDicList[i].DicMMName = RepairMMFilePath;
                        RepairDicList[i].DicMMRank = ConnectDict[j].DicMMRank;
                        break;
                    }
                }

            }
            for (int i = 0; i < RepairDicList.Count(); i++)
                Console.WriteLine(RepairDicList[i].DicSide + "," + RepairDicList[i].DicVisionAreaID + "," + RepairDicList[i].DicVisionWeldID + "," + RepairDicList[i].DicMMRank
                                 + "," + RepairDicList[i].DicMMName + "," + RepairDicList[i].DicWeldObjID);


            //6.依次LOAD MM FILE 並圈出物件
            string tempMMRank = FlowRepairAreaTxtBox.Text;
            //string tempMMRank = "";
            for (int i = 0; i < RepairDicList.Count(); i++)
            {
               
                if (RepairDicList[i].DicMMRank == tempMMRank && RepairDicList[i].DicRepairOK1 == "0")
                {

                    ///-------------
                    //抓同一區一次load
                    //tempMMRank =RepairDicList[i].DicMMRank;
                    axMMMark1.LoadFile(RepairDicList[i].DicMMName);
                    axMMMark1.SelectClearObjects();
                    //axMMMark1.SelectAddObject(RepairDicList[i].DicWeldObjID);
                    //Console.WriteLine("MMRank=" + RepairDicList[i].DicMMRank + ", ObjID=" + RepairDicList[j].DicWeldObjID);
                    //RepairDicList[i].DicRepairOK1 = "1";

                    //找其他有沒有同一區的
                    for (int j = 0; j < RepairDicList.Count(); j++)
                    {
                        if ((RepairDicList[j].DicMMRank == tempMMRank))
                        {

                            axMMMark1.SelectAddObject(RepairDicList[j].DicWeldObjID);
                            Console.WriteLine("MMRank=" + RepairDicList[j].DicMMRank + ", ObjID=" + RepairDicList[j].DicWeldObjID);
                            RepairDicList[j].DicRepairOK1 = "1";
                            
                        }

                    }
                    ///-------------
                    //--移動至該區
                    int IntTempMMRank = Convert.ToInt32(tempMMRank);

                    PLCAction.AutoStageMove(AIniXPointList[IntTempMMRank], AIniYPointList[IntTempMMRank], AIniZPointList[IntTempMMRank], 100);
                    //7.出光

                }
            }

            
        }

        private void FlowRepairRedLightBtn_Click(object sender, EventArgs e)
        {

        }

        private void ManualRepairBtn_Click(object sender, EventArgs e)
        {

            DialogResult AutoLaserChkResult = MessageBox.Show(this, "確定要激發雷射做補銲嗎?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (AutoLaserChkResult == DialogResult.No)
                return;
            int RedLightFlag, SWILaserReqFlag;

            PLCAction.axActUtlType1.GetDevice("M1205", out SWILaserReqFlag);

            if (SWILaserReqFlag == 0)
            {

                PLCAction.axActUtlType1.SetDevice("M1017", 1);

                PLCAction.axActUtlType1.GetDevice("M1018", out RedLightFlag);
                if (RedLightFlag == 1)
                {
                    PLCAction.axActUtlType1.SetDevice("M1018", 0);
                    ManualRedLightBtn.BackColor = Color.Thistle;

                }


                while (true)
                {
                    double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1017", 1);
                    {
                        PLCAction.axActUtlType1.SetDevice("M1048", 1); //啟動雷射輸出命令 EMISSION ENABLE
                        break;
                    }
                }

                //-----前面都在COVER PLC,這段才開始正式MM------------------------
                Thread.Sleep(1000);
                axMMMark1.StopMarking();
                axMMMark1.SetMarkSelect(1);
                axMMMark1.StartMarking(1);

                MessageBox.Show("已完成銲接!!");
                //--------------------------------------------------------------

            }
            else
            {
                PLCAction.axActUtlType1.SetDevice("M1017", 0);
                PLCAction.axActUtlType1.SetDevice("M1048", 0);

            }


            //避免預覽完紅光忘記還原位置
            //RepairMove();



            //20220113 FOR A+ 先MARK掉
            //DialogResult AutoLaserChkResult = MessageBox.Show(this, "確定要激發雷射做補銲嗎?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (AutoLaserChkResult == DialogResult.No)
            //    return;

            ///***************************新盛力版本**************************************/
            ////避免預覽完紅光忘記還原位置
            //RepairMove();

            //RedLightOff();
            //Thread.Sleep(200);

            ////鏡片吹氣保護on
            //PLCAction.axActUtlType1.SetDevice("M1204", 1); //z
            //Thread.Sleep(1000);
            //axMMMark1.StopMarking();
            //axMMMark1.SetMarkSelect(1);
            //axMMMark1.StartMarking(1);

            //RepairRedLightBtn.BackColor = Color.Thistle;

            //MessageBox.Show("已完成銲接!!");

            ////鏡片吹氣保護off
            //PLCAction.axActUtlType1.SetDevice("M1204", 0); //z
        }

        private void ManualRedLight_Click(object sender, EventArgs e)
        {


            //20220113 幫國陽PLC cover
            int LaserGoFlag, SWIRedLightReqFlag;

            //1.先判斷紅光有沒有開(M1206國揚好像PLC沒改到不管了)
            PLCAction.axActUtlType1.GetDevice("M1018", out SWIRedLightReqFlag);

            //2.紅光沒開: 檢查1017雷射輸出有沒有開
            if (SWIRedLightReqFlag == 0)
            {
                //20220112 如果m1017 keep
                PLCAction.axActUtlType1.GetDevice("M1017", out LaserGoFlag);

                //20220112 幫國陽直接把emmision off
                //2.1 如果1017有開則EMISSIO OFF
                if (LaserGoFlag == 1)
                    PLCAction.axActUtlType1.SetDevice("M1048", 0);

                //2.2 開啟紅光
                PLCAction.axActUtlType1.SetDevice("M1018", 1);

                //2.3開MM紅光
                axMMMark1.StopMarking();
                axMMMark1.SetMarkSelect(1);
                axMMMark1.StartMarking(3);


                ManualRedLightBtn.BackColor = Color.Green;
            }

            //3.紅光有開:
            else
            {
                PLCAction.axActUtlType1.SetDevice("M1018", 0);


                //20220112 如果m1017 keep
                PLCAction.axActUtlType1.GetDevice("M1017", out LaserGoFlag);
                //20220112 幫國陽直接把emmision ON 還原
                if (LaserGoFlag == 1)
                    PLCAction.axActUtlType1.SetDevice("M1048", 1);

                RedLightOff();//MM紅光關閉
                ManualRedLightBtn.BackColor = Color.Thistle;
            }


            //    //用紅光要比設定檔下降13mm
            //    int RepairAreaIdx = FindRepairIdx();

            //    int RedLightStatus, LaserGoFlag;
            //    PLCAction.axActUtlType1.GetDevice("M1018", out RedLightStatus);



            //    if (RedLightStatus == 0)//開啟紅光
            //    {
            //        PLCAction.axActUtlType1.GetDevice("M1017", out LaserGoFlag);
            //        if (LaserGoFlag == 1)
            //        {
            //            PLCAction.axActUtlType1.SetDevice("M1048", 0);
            //            Thread.Sleep(300);
            //        }


            //        PLCAction.axActUtlType1.SetDevice("M1018", 1);
            //        axMMMark1.StopMarking();
            //        axMMMark1.SetMarkSelect(1);
            //        axMMMark1.StartMarking(3);

            //        ManualRedLightBtn.BackColor = Color.Green;

            //        if (LaserGoFlag == 1)
            //        {
            //            PLCAction.axActUtlType1.SetDevice("M1048", 1);
            //        }

            //    }
            //    else //關閉紅光
            //    {
            //        RedLightOff();
            //        ManualRedLightBtn.BackColor = Color.Thistle;
            //    }

            //}


        }

        private void InputReadyBtn_Click(object sender, EventArgs e)
        {
            PLCAction.AutoStageMove(AIniXPointList[0], AIniYPointList[0], AIniZPointList[0], 100);
        }

        private void OutputReadyBtn_Click(object sender, EventArgs e)
        {

        }

        private void OutputReadyBtn_Click_1(object sender, EventArgs e)
        {
            PLCAction.AutoStageMove(AIniXPointList[0], 0, AIniZPointList[0], 100);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PLCAction.FlowWeldModuleIn();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PLCAction.FlowWeldModuleOut();
        }

        private void ManualRepairXcomboBox_DropDown(object sender, EventArgs e)
        {
            ManualRepairXcomboBox.Items.Clear();
            for (int i = 0; i < AIniAreaNumXList.Max(); i++)
                ManualRepairXcomboBox.Items.Add(i + 1);


        }

        private void ManualRepairYcomboBox_DropDown(object sender, EventArgs e)
        {
            ManualRepairYcomboBox.Items.Clear();
            for (int i = 0; i < AIniAreaNumYList.Max(); i++)
                ManualRepairYcomboBox.Items.Add(i + 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double[] _ReadVal = PLCAction.ReadPLCDataRandom("D2260\nD2261", 2);

            Console.WriteLine("補銲資料2=" + _ReadVal[0]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PLCAction.WriteD16Val(3);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            double[] test = PLCAction.ReadPLCDataRandom16("D2260\nD2261\nD2262", 3);

            for (int i = 0; i < test.Count(); i++)
                Console.WriteLine("test[" + i + "]=" + test[i]);
        }

        private void RepairChkBtn_Click(object sender, EventArgs e)
        {

        }



    }


    class EZMLayerObjInfo
    {
        public string LayerName;
        public int LayerNum;
        public string ObjName;
        public int ObjNum;
    }




}
