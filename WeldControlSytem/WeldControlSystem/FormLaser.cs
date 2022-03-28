using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WeldControlSystem
{
    public partial class FormLaser : Form
    {

        // new CLASS
        PLCAction PLCAction = new PLCAction();

        System.Threading.Timer timer4; //手動雷射源專用 timer
        System.Threading.Timer timer3; //手動雷射初始訊號重置專用 timer
        int LaserReq, LaserInterCntl, LaserGuide, LaserOn, LaserProStart, LaserResetErr, LaserAnalogCntl, LaserSCReset;
        int M1209 = -1;
        public int LaserReset = -1;//安全機制:系統交握
        int SWIRedLightReqFlag = -1;
        int SWILaserReqFlag = -1;
        int SWILaserCtrlReqFlag = -1;
        int HandShakeFlag = -1;


        //------------------------------MDI傳值區-------------------------------

        private int X00, X01, X02, X03, X04, X05, X06, X07, X08, X0D, X0E;
        private int Y04, Y05, Y08, Y09, Y0A, Y0B, Y0C, Y0E, Y0D;
        private int M1205, M1206, M1215, M1216;
        private int M1017, M1018;

        public int ValM1205
        {
            set { M1205 = value; }
        }
        public int ValM1215
        {
            set { M1215 = value; }
        }
        public int ValM1206
        {
            set { M1206 = value; }
        }
        public int ValM1216
        {
            set { M1216 = value; }
        }
        public int ValX00
        {
            set { X00 = value; }
        }
        public int ValX01
        {
            set { X01 = value; }
        }
        public int ValX02
        {
            set { X02 = value; }
        }
        public int ValX03
        {
            set { X03 = value; }
        }
        public int ValX04
        {
            set { X04 = value; }
        }
        public int ValX05
        {
            set { X05 = value; }
        }
        public int ValX06
        {
            set { X06 = value; }
        }
        public int ValX07
        {
            set { X07 = value; }
        }
        public int ValX08
        {
            set { X08 = value; }
        }
        public int ValX0D
        {
            set { X0D = value; }
        }
        public int ValX0E
        {
            set { X0E = value; }
        }
        public int ValY04
        {
            set { Y04 = value; }
        }
        public int ValY05
        {
            set { Y05 = value; }
        }
        public int ValY08
        {
            set { Y08 = value; }
        }
        public int ValY09
        {
            set { Y09 = value; }
        }
        public int ValY0A
        {
            set { Y0A = value; }
        }
        public int ValY0B
        {
            set { Y0B = value; }
        }
        public int ValY0C
        {
            set { Y0C = value; }
        }
        public int ValY0E
        {
            set { Y0E = value; }
        }
        public int ValY0D
        {
            set { Y0D = value; }
        }

        public void setValue()
        {
            //textBox1.Text = StageXCurPos.ToString();
            //textBox2.Text = StageYCurPos.ToString();
            //textBox3.Text = StageZCurPos.ToString();
        }




        private string _args;
        public FormLaser()
        {
            InitializeComponent();
        }
        public FormLaser(string value)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(value))
            {
                _args = value;  //1為工研院版本,2為利通版本,3:swiroc
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PLCAction.PLC_Connect(0);

            //PLCAction.PLC_AllInitial();

            PLCAction.axActUtlType1.SetDevice("M1005", 1);//plc主控權給PC

            LaserReset = 1; // 交握訊號重置

            //雷射源手動頁面專用 timer
            TimerCallback callback3 = new TimerCallback(_do3);
            timer4 = new System.Threading.Timer(callback3, null, 0, 500);

            //每五百毫秒起來一次
            //TimerCallback callback2 = new TimerCallback(PLC_HandShakingSet);
            //timer3 = new System.Threading.Timer(callback2, null, 0, 500);


            //UI 處理
            PCCtrlRadBtn_Click(sender, e); //m1227 = 1

            if (_args == "1")//工研院版本
            {
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
                this.tabPage5.Parent = null;
            }
            else if (_args == "2")//利通版本
            {
                this.tabPage1.Parent = null;
                this.tabPage3.Parent = null;
                this.tabPage5.Parent = null;
            }
            else if (_args == "3")//雷捷版本
            {
                this.tabPage1.Parent = null;
                this.tabPage2.Parent = null;
                this.tabPage4.Parent = null;
            }

            //20211222 安全1&2
            PLCAction.axActUtlType1.SetDevice("M1016", 1);

            //20211222 自動模式啟動/關閉
            PLCAction.axActUtlType1.SetDevice("M1044", 1);

        }
        private void PLC_HandShakingSet(object state)
        {
            //-================系統交握===============================
            //** 雷射重置機制,回傳值給PLC表示我還活著
            //** M1200 重置, M1209 傳給 PLC 訊號
            //** M1200 在程式起來時只需要做一次, by LaserReset flag 判段只須做一次
            //** PLC 將 M1209 = 0, PC端將 M1209 =1, 不斷循環

            if (LaserReset == 0)
            {
                PLCAction.axActUtlType1.SetDevice("M1200", 0);
            }
            else if (LaserReset == 1)
            {
                PLCAction.axActUtlType1.SetDevice("M1200", 1);
                LaserReset = 0;
            }
            if (M1209 == 0)
            {
                PLCAction.axActUtlType1.SetDevice("M1209", 1);
            }



        }

        private void _do3(object state)
        {

            this.BeginInvoke(new ManualLaserTag1(ManualLaserTag2));//委派
        }
        delegate void ManualLaserTag1();
        private void ManualLaserTag2()
        {

            //讀回系統M1209交握值
            //M1209 = PLCAction.PLC_HandShakingRead();

            //double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1220\nM1222\nM1224\nM1226\nM1221\nM1223\nM1225\nM1227\nM1205\nM1206\nM1230", 11);
            // int X30,X34,X38,X3C,X31,X35,X39,X3D,X32,X36,X3A,X3E,X33,X37,X3B,X3F;

            //IPG 24 雷捷 23
            //int[] ReadXYval = new int[47];


            ////IPG: 0-23
            //PLCAction.axActUtlType1.ReadDeviceRandom("X30\nX34\nX38\nX3C\nX31\nX35\nX39\nX3D\nX32\nX36\n" +
            //                                         "X3A\nX3E\nX33\nX37\nX3B\nX3F\n" +
            //                                         "Y70\nY71\nY72\nY73\nY74\nY75\nY76\nY77\n" +
            //    //雷捷 24-46
            //                                         "X20\nX21\nX22\nX23\nX24\nX25\nX26\nX27\nX28\nX29\n" +
            //                                         "X2A\nX2B\nX2C\nX2D\nX2E\nX2F\n" +
            //                                         "Y68\nY69\nY6A\nY6B\nY6C\nY6D\nY6E"
            //                                         , 47, out  ReadXYval[0]);

            //Botton status
            //if (_ReadVal[0] == 1)
            //{
            //    ManLaserReqBtn.BackColor = Color.Crimson;

            //    //台勵福雷捷
            //    LaserCtrlReqBtn.BackColor = Color.Green;
            //    SWILaserCtrlReqFlag = 1;
            //}
            //else
            //{
            //    //台勵福雷捷
            //    LaserCtrlReqBtn.BackColor = Color.White;
            //    //SWIRedLightReqFlag = 0;
            //    SWILaserCtrlReqFlag = 0;
            //}
            //if (_ReadVal[1] == 1)
            //    ManLaserInterCntlBtn.BackColor = Color.Crimson;
            //else
            //    ManLaserInterCntlBtn.BackColor = Color.White;
            //if (_ReadVal[2] == 1)
            //    ManLaserGuideBtn.BackColor = Color.Crimson;
            //else
            //    ManLaserGuideBtn.BackColor = Color.White;

            //if (_ReadVal[3] == 1)
            //    ManLaserOnBtn.BackColor = Color.Crimson;
            //else
            //    ManLaserOnBtn.BackColor = Color.White;
            //if (_ReadVal[4] == 1)
            //    ManLaserProStartBtn.BackColor = Color.Crimson;
            //else
            //    ManLaserProStartBtn.BackColor = Color.White;
            //if (_ReadVal[5] == 1)
            //    ManLaserResetErrBtn.BackColor = Color.Crimson;
            //else
            //    ManLaserResetErrBtn.BackColor = Color.White;

            //if (_ReadVal[6] == 1)
            //    ManLaserAnalogCntlBtn.BackColor = Color.Crimson;
            //else
            //    ManLaserAnalogCntlBtn.BackColor = Color.White;
            //if (_ReadVal[7] == 1)
            //    ManLaserSCResetBtn.BackColor = Color.Crimson;
            //else
            //    ManLaserSCResetBtn.BackColor = Color.White;

            ////雷捷測試
            //if (_ReadVal[8] == 1)//1205 出光
            //{
            //    ManLaserReqBtn3.BackColor = Color.Green;
            //    SWILaserReqFlag = 1;
            //}
            //else
            //{
            //    ManLaserReqBtn3.BackColor = Color.White;
            //    SWILaserReqFlag = 0;
            //}


            //if (_ReadVal[9] == 1)//1206 紅光
            //{
            //    ManLaserGuideBtn3.BackColor = Color.Green;
            //    SWIRedLightReqFlag = 1;
            //}
            //else
            //{
            //    ManLaserGuideBtn3.BackColor = Color.White;
            //    SWIRedLightReqFlag = 0;
            //}
            //if (_ReadVal[10] == 1)//1230 交握
            //{
            //    HandShakeBtn.BackColor = Color.Green;
            //    HandShakeFlag = 1;
            //}
            //else
            //{
            //    HandShakeBtn.BackColor = Color.White;
            //    HandShakeFlag = 0;
            //}
            //int[] ReadXYval2 = new int[1];

            //PLCAction.axActUtlType1.ReadDeviceRandom("X05", 1, out  ReadXYval[0]);
            ////   PLCAction.axActUtlType1.ReadDeviceRandom("X01", 1, out  ReadXYval2[0]);


            //if (ReadXYval[0] == 1)
            //    X05Lbl.BackColor = Color.Green;

            //else
            //    X05Lbl.BackColor = Color.Bisque;



            //if (ReadXYval2[0] == 1)
            //    X01Lbl.BackColor = Color.Green;

            //else
            //    X01Lbl.BackColor = Color.Bisque;



            //if (ReadXYval[3] == 1)
            //    Y0ALbl.BackColor = Color.Green;

            //else
            //    Y0ALbl.BackColor = Color.Tan;





            //I/O Status
            if (M1206 == 1)
            //if (M1018 == 1)//20220113很奇怪問國陽新版反而不行
            {
                SWIRedLightReqFlag = 1;
                ManLaserGuideBtn3.BackColor = Color.Green;
            }
            else
            {
                SWIRedLightReqFlag = 0;
                ManLaserGuideBtn3.BackColor = Color.White;
            }
            if(M1205 == 1)
            //if (M1017 == 1)//20220113很奇怪問國陽新版反而不行
            {
                SWILaserReqFlag = 1;
                ManLaserReqBtn3.BackColor = Color.Green;
            }
            else
            {
                SWILaserReqFlag = 0;
                ManLaserReqBtn3.BackColor = Color.White;

            }
            if (X00 == 1)
                X00Lbl.BackColor = Color.Green;
            else
                X00Lbl.BackColor = Color.Bisque;


            if (X01 == 1)
                X01Lbl.BackColor = Color.Green;

            else
                X01Lbl.BackColor = Color.Bisque;


            if (X02 == 1)
                X02Lbl.BackColor = Color.Green;
            else
                X02Lbl.BackColor = Color.Bisque;


            if (X03 == 1)
                X03Lbl.BackColor = Color.Green;
            else
                X03Lbl.BackColor = Color.Bisque;


            if (X04 == 1)
                X04Lbl.BackColor = Color.Green;
            else
                X04Lbl.BackColor = Color.Bisque;


            if (X05 == 1)
                X05Lbl.BackColor = Color.Green;
            else
                X05Lbl.BackColor = Color.Bisque;


            if (X06 == 1)
                X06Lbl.BackColor = Color.Green;
            else
                X06Lbl.BackColor = Color.Bisque;


            if (X07 == 1)
                X07Lbl.BackColor = Color.Green;
            else
                X07Lbl.BackColor = Color.Bisque;


            if (X08 == 1)
                X08Lbl.BackColor = Color.Green;
            else
                X08Lbl.BackColor = Color.Bisque;



            if (X0D == 1)
                X0DLbl.BackColor = Color.Green;
            else
                X0DLbl.BackColor = Color.Bisque;



            if (X0E == 1)
                X0ELbl.BackColor = Color.Green;
            else
                X0ELbl.BackColor = Color.Bisque;

            //-----------------------------------------


            if (Y04 == 1)
                Y04Lbl.BackColor = Color.Green;
            else
                Y04Lbl.BackColor = Color.Bisque;


            if (Y05 == 1)
                Y05Lbl.BackColor = Color.Green;
            else
                Y05Lbl.BackColor = Color.Bisque;


            if (Y08 == 1)
                Y08Lbl.BackColor = Color.Green;
            else
                Y08Lbl.BackColor = Color.Bisque;



            if (Y09 == 1)
                Y09Lbl.BackColor = Color.Green;

            else
                Y09Lbl.BackColor = Color.Bisque;


            if (Y0A == 1)
                Y0ALbl.BackColor = Color.Green;
            else
                Y0ALbl.BackColor = Color.Bisque;


            if (Y0B == 1)
                Y0BLbl.BackColor = Color.Green;
            else
                Y0BLbl.BackColor = Color.Bisque;



            if (Y0C == 1)
                Y0CLbl.BackColor = Color.Green;
            else
                Y0CLbl.BackColor = Color.Bisque;


            if (Y0E == 1)
                Y0ELbl.BackColor = Color.Green;
            else
                Y0ELbl.BackColor = Color.Bisque;



            if (Y0D == 1)
                Y0DLbl.BackColor = Color.Green;
            else
                Y0DLbl.BackColor = Color.Bisque;




            X3ELbl.BackColor = Color.LimeGreen;
            X3FLbl.BackColor = Color.LimeGreen;

            ////雷捷

            //// "X20\nX21\nX22\nX23\nX24\nX25\nX26\nX27\nX28\nX29\n"+
            //// "X2A\nX2B\nX2C\nX2D\nX2E\nX2F\n"+
            ////"Y68\nY69\nY6A\nY6B\nY6C\nY6D\nY6E"

            //if (ReadXYval[24] == 1)
            //    X20Lbl.BackColor = Color.Green;
            //else
            //    X20Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[25] == 1)
            //    X00Lbl.BackColor = Color.Green;
            //else
            //    X00Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[26] == 1)
            //    X01Lbl.BackColor = Color.Green;
            //else
            //    X01Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[27] == 1)
            //    X02Lbl.BackColor = Color.Green;
            //else
            //    X02Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[28] == 1)
            //    X03Lbl.BackColor = Color.Green;
            //else
            //    X03Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[29] == 1)
            //    X04Lbl.BackColor = Color.Green;
            //else
            //    X04Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[30] == 1)
            //    X05Lbl.BackColor = Color.Green;
            //else
            //    X05Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[31] == 1)
            //    X07Lbl.BackColor = Color.Green;
            //else
            //    X07Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[32] == 1)
            //    X08Lbl.BackColor = Color.Green;
            //else
            //    X08Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[33] == 1)
            //    X06Lbl.BackColor = Color.Green;
            //else
            //    X06Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[34] == 1)
            //    X0DLbl.BackColor = Color.Green;
            //else
            //    X0DLbl.BackColor = Color.Bisque;

            //if (ReadXYval[35] == 1)
            //    X2BLbl.BackColor = Color.Green;
            //else
            //    X2BLbl.BackColor = Color.Bisque;

            //if (ReadXYval[36] == 1)
            //    X2CLbl.BackColor = Color.Green;
            //else
            //    X2CLbl.BackColor = Color.Bisque;

            //if (ReadXYval[37] == 1)
            //    X2DLbl.BackColor = Color.Green;
            //else
            //    X2DLbl.BackColor = Color.Bisque;

            //if (ReadXYval[38] == 1)
            //    X0ELbl.BackColor = Color.Green;
            //else
            //    X0ELbl.BackColor = Color.Bisque;

            //if (ReadXYval[39] == 1)
            //    X2FLbl.BackColor = Color.Green;
            //else
            //    X2FLbl.BackColor = Color.Bisque;


            //if (ReadXYval[40] == 1)
            //    Y04Lbl.BackColor = Color.Green;
            //else
            //    Y04Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[41] == 1)
            //    Y05Lbl.BackColor = Color.Green;
            //else
            //    Y05Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[42] == 1)
            //    Y08Lbl.BackColor = Color.Green;
            //else
            //    Y08Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[43] == 1)
            //    Y09Lbl.BackColor = Color.Green;
            //else
            //    Y09Lbl.BackColor = Color.Bisque;

            //if (ReadXYval[44] == 1)
            //    Y0ALbl.BackColor = Color.Green;
            //else
            //    Y0ALbl.BackColor = Color.Bisque;

            //if (ReadXYval[45] == 1)
            //    Y0BLbl.BackColor = Color.Green;
            //else
            //    Y0BLbl.BackColor = Color.Bisque;

            //if (ReadXYval[46] == 1)
            //    Y0CLbl.BackColor = Color.Green;
            //else
            //    Y0CLbl.BackColor = Color.Bisque;



            X0ELbl.BackColor = Color.LimeGreen;
            // X2FLbl.BackColor = Color.LimeGreen;

        }
        private void ManLaserReqBtn_Click(object sender, EventArgs e)
        {
            if (LaserReq == 0)
            {
                PLCAction.OnLaserReq();
                LaserReq = 1;
            }
            else
            {
                PLCAction.OffLaserReq();
                LaserReq = 0;
            }
        }

        private void ManLaserInterCntlBtn_Click(object sender, EventArgs e)
        {
            if (LaserInterCntl == 0)
            {
                PLCAction.OnLaserInterCntl();
                LaserInterCntl = 1;
            }
            else
            {
                PLCAction.OffLaserInterCntl();
                LaserInterCntl = 0;
            }
        }

        private void ManLaserGuideBtn_Click(object sender, EventArgs e)
        {
            if (LaserGuide == 0)
            {
                PLCAction.OnLaserGuide();
                LaserGuide = 1;
            }
            else
            {
                PLCAction.OffLaserGuide();
                LaserGuide = 0;
            }
        }

        private void ManLaserOnBtn_Click(object sender, EventArgs e)
        {
            if (LaserOn == 0)
            {
                PLCAction.OnLaserOn();
                LaserOn = 1;
            }
            else
            {
                PLCAction.OffLaserOn();
                LaserOn = 0;
            }
        }

        private void ManLaserProStartBtn_Click(object sender, EventArgs e)
        {
            if (LaserProStart == 0)
            {
                PLCAction.OnLaserProStart();
                LaserProStart = 1;
            }
            else
            {
                PLCAction.OffLaserProStart();
                LaserProStart = 0;
            }
        }

        private void ManLaserResetErrBtn_Click(object sender, EventArgs e)
        {
            if (LaserResetErr == 0)
            {
                PLCAction.OnLaserResetErr();
                LaserResetErr = 1;
            }
            else
            {
                PLCAction.OffLaserResetErr();
                LaserResetErr = 0;
            }
        }

        private void ManLaserAnalogCntlBtn_Click(object sender, EventArgs e)
        {
            if (LaserAnalogCntl == 0)
            {
                PLCAction.OnLaserAnalogCntl();
                LaserAnalogCntl = 1;
            }
            else
            {
                PLCAction.OffLaserAnalogCntl();
                LaserAnalogCntl = 0;
            }
        }

        private void ManLaserSCResetBtn_Click(object sender, EventArgs e)
        {
            if (LaserSCReset == 0)
            {
                PLCAction.OnLaserSCReset();
                LaserSCReset = 1;
            }
            else
            {
                PLCAction.OffLaserSCReset();
                LaserSCReset = 0;
            }
        }

        private void ManLaserReqBtn2_Click(object sender, EventArgs e)
        {
            ManLaserReqBtn_Click(null, null);
        }

        private void ManLaserOnBtn2_Click(object sender, EventArgs e)
        {
            ManLaserOnBtn_Click(null, null);
        }

        private void ManLaserAnalogCntlBtn2_Click(object sender, EventArgs e)
        {
            ManLaserAnalogCntlBtn_Click(null, null);
        }

        private void ManLaserProStartBtn2_Click(object sender, EventArgs e)
        {
            ManLaserProStartBtn_Click(null, null);
        }

        private void ManLaserGuideBtn2_Click(object sender, EventArgs e)
        {
            ManLaserGuideBtn_Click(null, null);
        }

        private void ManLaserGuideBtn3_Click(object sender, EventArgs e)
        {
            int LaserGoFlag;
            if (SWIRedLightReqFlag == 0)
            {
                //20220112 如果m1017 keep
                PLCAction.axActUtlType1.GetDevice("M1017", out LaserGoFlag);

                //20220112 幫國陽直接把emmision off
                if (LaserGoFlag == 1)
                    PLCAction.axActUtlType1.SetDevice("M1048", 0);

                PLCAction.axActUtlType1.SetDevice("M1018", 1);
            }
            else
            {
                PLCAction.axActUtlType1.SetDevice("M1018", 0);

                //20220112 如果m1017 keep
                PLCAction.axActUtlType1.GetDevice("M1017", out LaserGoFlag);
                //20220112 幫國陽直接把emmision off
                if (LaserGoFlag == 1)
                    PLCAction.axActUtlType1.SetDevice("M1048", 1);

            }
        }

        private void ManLaserReqBtn3_Click(object sender, EventArgs e)
        {
            int RedLightFlag = 0;
            if (SWILaserReqFlag == 0)
            {
                
                PLCAction.axActUtlType1.SetDevice("M1017", 1);
                
                PLCAction.axActUtlType1.GetDevice("M1018", out RedLightFlag);
                if (RedLightFlag==1)
                {
                    PLCAction.axActUtlType1.SetDevice("M1018", 0);

                }


                while (true)
                {
                    double[] _ReadVal = PLCAction.ReadPLCDataRandom("M1017", 1);
                    {
                        PLCAction.axActUtlType1.SetDevice("M1048", 1); //啟動雷射輸出命令 EMISSION ENABLE
                        break;
                    }
                }


            }
            else
            {
                PLCAction.axActUtlType1.SetDevice("M1017", 0);
                PLCAction.axActUtlType1.SetDevice("M1048", 0);

            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void PlCCtrlRadBtn_Click(object sender, EventArgs e)
        {
            PLCCtrlRadBtn.Checked = true;
            PCCtrlRadBtn.Checked = false;
            PLCAction.axActUtlType1.SetDevice("M1213", 0); //PLC
            PLCAction.axActUtlType1.SetDevice("M1214", 1); //PLC
            PLCAction.axActUtlType1.SetDevice("M1227", 0); //PLC
            PLCAction.axActUtlType1.SetDevice("M1214", 0); //PLC
        }

        private void PCCtrlRadBtn_Click(object sender, EventArgs e)
        {
            PCCtrlRadBtn.Checked = true;
            PLCCtrlRadBtn.Checked = false;
            PLCAction.axActUtlType1.SetDevice("M1213", 1); //PC
            PLCAction.axActUtlType1.SetDevice("M1227", 1); //PC
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }



        private void HandShakeBtn_Click(object sender, EventArgs e)
        {
            if (HandShakeFlag == 0)
            {

                PLCAction.axActUtlType1.SetDevice("M1230", 1);

            }
            else
            {
                PLCAction.axActUtlType1.SetDevice("M1230", 0);

            }
        }

        private void LaserCtrlReqBtn_Click(object sender, EventArgs e)
        {
            if (SWILaserCtrlReqFlag == 0)
            {

                PLCAction.axActUtlType1.SetDevice("M1220", 1);

            }
            else
            {
                PLCAction.axActUtlType1.SetDevice("M1220", 0);

            }
        }

        public void FormLaser_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer4.Dispose();
        }

        private void FormLaser_LocationChanged(object sender, EventArgs e)
        {

        }





    }
}
