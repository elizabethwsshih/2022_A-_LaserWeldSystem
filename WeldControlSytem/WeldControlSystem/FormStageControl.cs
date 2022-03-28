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
using System.Security.Permissions;

namespace WeldControlSystem
{
    public partial class FormStageControl : Form
    {

        //移動模式//0:連續移動,1:吋動,2:定位移動
        int StageMoveMode = -1;

        //double XCurPos, YCurPos, ZCurPos;

        //移動thread
        Thread AsnPosMovethread;

        //吋動&連續移動
        Thread XPosJogThread, XNagJogThread, YNagJogThread, YPosJogThread, ZNagJogThread, ZPosJogThread;
        Thread XPosStepThread, XNagStepThread, YNagStepThread, YPosStepThread, ZNagStepThread, ZPosStepThread;


        // int XPosStepStartFlag = -1, XNagStepStartFlag = -1, YNagStepStartFlag = -1, YPosStepStartFlag = -1, ZNagStepStartFlag = -1, ZPosStepStartFlag = -1;
        // 各軸座標上下限
        double XUpBound = 1040;//1000;
        double XLowBound = -150;// 0;
        double YUpBound = 460;
        double YLowBound = -1;
        double ZUpBound = 1;
        double ZLowBound = -200;

        double XJogUpSpeedBound = 3000;
        double YJogUpSpeedBound = 2000;
        double ZJogUpSpeedBound = 1000;
        double XAsnUpSpeedBound = 8000;
        double YAsnUpSpeedBound = 6000;
        double ZAsnUpSpeedBound = 5000;

        int XHomeLockBound = 0; //歸home後不用受上下限控制,初始值設-1才不會一打開程式就啟動
        int YHomeLockBound = 0;
        int ZHomeLockBound = 0;

        //速度上下限
        int ClickLock = 0;

        Thread GoHomeThread;
        System.Threading.Timer timer1; //聽 極限
        System.Threading.Timer timer2; //防連點

        //------------------------------MDI傳值區-------------------------------

        private double StageXCurPos;
        private double StageYCurPos;
        private double StageZCurPos;


        public double ValXCurPos
        {
            set { StageXCurPos = value; }
        }
        public double ValYCurPos
        {
            set { StageYCurPos = value; }
        }
        public double ValZCurPos
        {
            set { StageZCurPos = value; }
        }
        public void setValue()
        {
            //textBox1.Text = StageXCurPos.ToString();
            //textBox2.Text = StageYCurPos.ToString();
            //textBox3.Text = StageZCurPos.ToString();
        }



        PLCAction PLCAction = new PLCAction();

        public FormStageControl()
        {
            InitializeComponent();
        }

        private void FormStageControl_Load(object sender, EventArgs e)
        {
            ////UI
            PCModeRdioBox_Click(sender, e);
            ManualNoStopRadioBtn_Click(sender, e);


            //1. 通訊
            PLCAction.PLC_Connect(0);



            //2.初始化:清0 + err reset + 重啟伺服馬達
            // PLCAction.PLC_AllInitial();

            //先停止交握,出機再開
            PLCAction.PLC_HandShaking(0);

            //3.主控權
            PLCAction.MainControl(1);//1:pc,0:plc

            //4.聽訊號
            //TimerCallback callback1 = new TimerCallback(_ListenBound);
            //timer1 = new System.Threading.Timer(callback1, null, 0, 400);

            //5.保護不連續按
            //TimerCallback callback2 = new TimerCallback(_ListenAvoidCLick);
            //timer2 = new System.Threading.Timer(callback2, null, 0, 300);

        }
        private void _ListenAvoidCLick(object state)
        {
            this.BeginInvoke(new Listen2(AvoidClick));//委派
        }
        delegate void Listen2();
        private void AvoidClick()
        {
            if (ClickLock == 1) ClickLock = 0;
        }

        private void _ListenBound(object state)
        {
            this.BeginInvoke(new Listen1(ListenBound));//委派
        }
        delegate void Listen1();
        private void ListenBound()
        {

            if (XHomeLockBound == 0)//按了jog解掉
            {
                //--------------------------------
                if (StageXCurPos < XLowBound)//-70
                {
                    PLCAction.ManualContinousPause();
                    XHomeLockBound = 1;

                    ManualXPosBtn.Enabled = false;
                    ManualXPosBtn.BackColor = Color.Gray;

                }

                else if (StageXCurPos > XUpBound)//1070
                {
                    PLCAction.ManualContinousPause();
                    XHomeLockBound = 1;

                    ManualXNegBtn.Enabled = false;
                    ManualXNegBtn.BackColor = Color.Gray;

                }

            }
            if (YHomeLockBound == 0)
            {
                //--------------------------------0
                if (StageYCurPos < YLowBound)//0
                {
                    PLCAction.ManualContinousPause();
                    YHomeLockBound = 1;

                    ManualYPosBtn.Enabled = false;
                    ManualYPosBtn.BackColor = Color.Gray;
                }
                else if (StageYCurPos > YUpBound)//500
                {
                    PLCAction.ManualContinousPause();
                    YHomeLockBound = 1;

                    ManualYNegBtn.Enabled = false;
                    ManualYNegBtn.BackColor = Color.Gray;

                }

            }

        }

        private void PlcModeRdioBox_Click(object sender, EventArgs e)
        {
            PlcModeRdioBox.Checked = true;
            PCModeRdioBox.Checked = false;
            PLCAction.MainControl(0);
            //封鎖整個ui
            ManualMoveGrpBox.Enabled = false;
            ManualAsnPosGrpBox.Enabled = false;

        }

        private void PCModeRdioBox_Click(object sender, EventArgs e)
        {
            PlcModeRdioBox.Checked = false;
            PCModeRdioBox.Checked = true;
            PLCAction.MainControl(1);
            //開啟整個ui
            ManualMoveGrpBox.Enabled = true;
            ManualAsnPosGrpBox.Enabled = true;
        }

        private void ManualXPosBtn_Click(object sender, EventArgs e)
        {
            //if (ClickLock == 0)
            {
                if (StageMoveMode == 1)
                {

                    XNagStepThread = new Thread(new ThreadStart(XNagStep));
                    Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                    XNagStepThread.Start();
                    //ManualSet();
                    //double StepDistVal = Convert.ToDouble(ManualStepDistComboBox.Text);
                    //PLCAction.ManualStepHH("XNag", StepDistVal);
                }
            }
        }
        void XNagStep()
        {
            //ClickLock = 1;
            XHomeLockBound = 0;
            int _rst = ManualSet();
            if (_rst == 0)
            {
                double StepDistVal = Convert.ToDouble(ManualStepDistComboBox.Text);

                //double[] _ReadVal = PLCAction.ReadPLCDataRandom("D1000\nD1001", 2);
                double StepTgtX = 0;

                StepTgtX = StageXCurPos + StepDistVal;

                if (StepTgtX < XLowBound)
                {
                    //StepTgtX = XLowBound;
                    ManualXPosBtn.Enabled = false;
                    ManualXPosBtn.BackColor = Color.Gray;
                }
                //else if (StepTgtX > XUpBound) StepTgtX = XUpBound;
                PLCAction.AutoStageMove(StepTgtX, StageYCurPos, StageZCurPos, 100);//預設手動走3000,100%
                XOKColorChange();
            }
            else if (_rst == 1)
                MessageBox.Show("速度超過上限!請重新調整\nX上限:8000\nY上限:6000\nZ上限:5000");

        }
        private void ManualXPosBtn_MouseDown(object sender, MouseEventArgs e)
        {

            //if (StageMoveMode == 0)
            //{
            //    //XPosJogThread = new Thread(new ThreadStart(XPosJog));
            //    //Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
            //    //XPosJogThread.Start();
            //    XHomeLockBound = 0;
            //    int _rst = ManualSet();
            //    if (_rst == 0)
            //    {
            //        ManualSet();
            //        PLCAction.ManualContinous("XPos");
            //    }

            //    else if (_rst == 1)
            //        MessageBox.Show("速度超過上限!請重新調整\nX上限:3000\nY上限:2000\nZ上限:1000");
            //}


            if (StageMoveMode == 0)
            {
                XHomeLockBound = 0;

                int _rst = ManualSet();
                if (_rst == 0)
                    PLCAction.ManualContinous("XNag");
                else if (_rst == 1)
                    MessageBox.Show("速度超過上限!請重新調整\nX上限:3000\nY上限:2000\nZ上限:1000");
                //XNagJogThread = new Thread(new ThreadStart(XNagJog));
                //Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                //XNagJogThread.Start();
            }
            
        }
        void XPosJog()
        {
            //ClickLock = 1;
            XHomeLockBound = 0;

            int _rst = ManualSet();
            if (_rst == 0)
            {
                PLCAction.ManualContinous("XPos");
            }
            else if (_rst == 1)
                MessageBox.Show("速度超過上限!請重新調整\nX上限:8000\nY上限:6000\nZ上限:5000");

        }


        private int ManualSet() //0: ok , 1:fail
        {
            int rst = -1;
            if (StageMoveMode == 0) //連續移動 吃設定速度
            {
                double _StepDistVal = 0.0;//沒有用了
                double _XSpeedVal = double.Parse(ManualXSpeedTxtBox.Text);
                double _YSpeedVal = double.Parse(ManualYSpeedTxtBox.Text);
                double _ZSpeedVal = double.Parse(ManualZSpeedTxtBox.Text);
                int _SpeedRateVal = 100;

                if (_XSpeedVal > XJogUpSpeedBound || _YSpeedVal > YJogUpSpeedBound || _ZSpeedVal > ZJogUpSpeedBound)
                    rst = 1;
                else
                {
                    PLCAction.ManualSet(_StepDistVal, _XSpeedVal, _YSpeedVal, _ZSpeedVal, _SpeedRateVal);// for 吋動需要修改步階距離
                    rst = 0;
                }


            }
            else if (StageMoveMode == 1) //吋動 直接寫死速度
            {
                double _StepDistVal = double.Parse(ManualStepDistComboBox.Text);
                double _XSpeedVal = 1000.0;
                double _YSpeedVal = 1000.0;
                double _ZSpeedVal = 1000.0;
                int _SpeedRateVal = 100;

                PLCAction.ManualSet(_StepDistVal, _XSpeedVal, _YSpeedVal, _ZSpeedVal, _SpeedRateVal);// for 吋動需要修改步階距離
                rst = 0;
            }
            else if (StageMoveMode == 2)//連續移動 吃設定速度
            {
                double _StepDistVal = 0.0;// double.Parse(ManualStepDistComboBox.Text);
                double _XSpeedVal = double.Parse(XAsnSpeedTxtBox.Text);
                double _YSpeedVal = double.Parse(YAsnSpeedTxtBox.Text);
                double _ZSpeedVal = double.Parse(ZAsnSpeedTxtBox.Text);
                int _SpeedRateVal = 100;
                if (_XSpeedVal > XAsnUpSpeedBound || _YSpeedVal > YAsnUpSpeedBound || _ZSpeedVal > ZAsnUpSpeedBound)
                    rst = 1;
                else
                {
                    PLCAction.ManualSet(_StepDistVal, _XSpeedVal, _YSpeedVal, _ZSpeedVal, _SpeedRateVal);// for 吋動需要修改步階距離
                    rst = 0;
                }
            }
            return rst;


        }
        void UanbleUIMoveModeFunc()
        {
            ManualNoStopRadioBtn.Checked = false;
            ManualStepRadioBox.Checked = false;
            ManualAsnPosRadioBtn.Checked = false;
            ManualAsnPosMoveBtn.Enabled = false;
        }
        private void ManualNoStopRadioBtn_Click(object sender, EventArgs e)
        {
            StageMoveMode = 0;//連續移動
            int rst = ManualSet();
            if (rst == 0)
            {
                UanbleUIMoveModeFunc();
                ManualNoStopRadioBtn.Checked = true;
            }
            else if (rst == 1)
                MessageBox.Show("");
            // PLCAction.axActUtlType1.SetDevice("M1201", 0);//連續移動
            //PLCAction.SetManualContinous(0);//連續移動


        }

        private void ManualStepRadioBox_Click(object sender, EventArgs e)
        {
            StageMoveMode = 1;//步徑
            UanbleUIMoveModeFunc();
            ManualStepRadioBox.Checked = true;

        }

        private void ManualAsnPosRadioBtn_Click(object sender, EventArgs e)
        {
            StageMoveMode = 2;//指定位置移動
            UanbleUIMoveModeFunc();
            ManualAsnPosRadioBtn.Checked = true;
            ManualAsnPosMoveBtn.Enabled = true;
        }

        private void ManualXPosBtn_MouseUp(object sender, MouseEventArgs e)
        {
            if (StageMoveMode == 0)
            {
                ManualSet();
                PLCAction.ManualContinousPause();
                XOKColorChange();


            }
        }

        private void ManualXNegBtn_MouseDown(object sender, MouseEventArgs e)
        {


            //if (StageMoveMode == 0)
            //{
            //    XHomeLockBound = 0;

            //    int _rst = ManualSet();
            //    if (_rst == 0)
            //        PLCAction.ManualContinous("XNag");
            //    else if (_rst == 1)
            //        MessageBox.Show("速度超過上限!請重新調整\nX上限:3000\nY上限:2000\nZ上限:1000");
            //    //XNagJogThread = new Thread(new ThreadStart(XNagJog));
            //    //Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
            //    //XNagJogThread.Start();


            //}
            if (StageMoveMode == 0)
            {
                //XPosJogThread = new Thread(new ThreadStart(XPosJog));
                //Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                //XPosJogThread.Start();
                XHomeLockBound = 0;
                int _rst = ManualSet();
                if (_rst == 0)
                {
                    ManualSet();
                    PLCAction.ManualContinous("XPos");
                }

                else if (_rst == 1)
                    MessageBox.Show("速度超過上限!請重新調整\nX上限:3000\nY上限:2000\nZ上限:1000");
            }
            
        }
        void XNagJog()
        {
            // ClickLock = 1;
            XHomeLockBound = 0;
            int _rst = ManualSet();

            if (_rst == 0)
            {
                PLCAction.ManualContinous("XNag");
            }
            else if (_rst == 1)
            {
                MessageBox.Show("速度超過上限!請重新調整\nX上限:3000\nY上限:2000\nZ上限:1000");

            }
            XHomeLockBound = 0;
        }


        private void ManualXNegBtn_MouseUp(object sender, MouseEventArgs e)
        {
            if (StageMoveMode == 0)
            {

                ManualSet();

                PLCAction.ManualContinousPause();
                XOKColorChange();
            }
        }

        private void ManualXNegBtn_Click(object sender, EventArgs e)
        {
            //if (ClickLock == 0)
            {
                if (StageMoveMode == 1)
                {

                    XPosStepThread = new Thread(new ThreadStart(XPosStep));
                    Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                    XPosStepThread.Start();


                }
            }
        }
        void XPosStep()
        {

            //ClickLock = 1;
            XHomeLockBound = 0;

            ManualSet();
            double StepDistVal = Convert.ToDouble(ManualStepDistComboBox.Text);

            // double[] _ReadVal = PLCAction.ReadPLCDataRandom("D1000\nD1001", 2);

            //double StepTgtX = Math.Round(_ReadVal[0], 3) + StepDistVal;
            double StepTgtX = StageXCurPos - StepDistVal;

            if (StepTgtX < XLowBound)
            {
                // StepTgtX = XLowBound;
            }

            else if (StepTgtX > XUpBound)
            {
                //StepTgtX = XUpBound;
                ManualXNegBtn.Enabled = false;
                ManualXNegBtn.BackColor = Color.Gray;
            }
            PLCAction.AutoStageMove(StepTgtX, StageYCurPos, StageZCurPos, 100);//預設手動走3000,100%
            XOKColorChange();

        }

        private void GoHomeBtn_Click(object sender, EventArgs e)
        {
            XHomeLockBound = 1;
            YHomeLockBound = 1;
            ZHomeLockBound = 1;

        }
        void GoHome()
        {
            PLCAction.GoHome();

        }

        private void GoHomeBtn_MouseDown(object sender, MouseEventArgs e)
        {
            XHomeLockBound = 1;
            YHomeLockBound = 1;
            ZHomeLockBound = 1;
            PLCAction.axActUtlType1.SetDevice("M1023", 1); //z
            PLCAction.axActUtlType1.SetDevice("M1003", 1); //x
            PLCAction.axActUtlType1.SetDevice("M1013", 1); //y
            XHomeLockBound = 0;
            YHomeLockBound = 0;
            ZHomeLockBound = 0;

        }

        private void GoHomeBtn_MouseUp(object sender, MouseEventArgs e)
        {
            PLCAction.axActUtlType1.SetDevice("M1023", 0); //z
            PLCAction.axActUtlType1.SetDevice("M1003", 0); //x
            PLCAction.axActUtlType1.SetDevice("M1013", 0); //y

        }

        private void ManualYPosBtn_MouseDown(object sender, MouseEventArgs e)
        {
            //if (ClickLock == 0)
            {

                //要記得先連通PLC才能執行
                if (StageMoveMode == 0)
                {
                    YHomeLockBound = 0;
                    int _rst = ManualSet();

                    if (_rst == 0)
                    {
                        PLCAction.ManualContinous("YPos");
                    }
                    else if (_rst == 1)
                        MessageBox.Show("速度超過上限!請重新調整\nX上限:3000\nY上限:2000\nZ上限:1000");
                    //YPosJogThread = new Thread(new ThreadStart(YPosJog));
                    //Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                    //YPosJogThread.Start();
                }
            }
        }

        void YPosJog()
        {
            //ClickLock = 1;
            YHomeLockBound = 0;
            ManualSet();
            PLCAction.ManualContinous("YPos");


        }
        private void ManualYPosBtn_MouseUp(object sender, MouseEventArgs e)
        {
            if (StageMoveMode == 0)
            {
                ManualSet();
                PLCAction.ManualContinousPause();
                //Thread.Sleep(10);
                //if (YPosJogThread.IsAlive)
                //{
                //    YPosJogThread.Abort();
                //    YPosJogThread.Join();
                //}
                //Thread.Sleep(10);
                YOKColorChange();
            }
        }

        private void ManualZNegBtn_MouseDown(object sender, MouseEventArgs e)
        {

          
                //if (StageMoveMode == 0)
                //{
                //    ZHomeLockBound = 0;
                //    int _rst = ManualSet();
                //    if (_rst == 0)
                //    {
                //        PLCAction.ManualContinous("ZNeg");
                //    }
                //    else if (_rst == 1)
                //        MessageBox.Show("速度超過上限!請重新調整\nX上限:3000\nY上限:2000\nZ上限:1000");
                //    //ZNagJogThread = new Thread(new ThreadStart(ZNagJog));
                //    //Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                //    //ZNagJogThread.Start();

                //}
            if (StageMoveMode == 0)
            {
                ZHomeLockBound = 0;
                int _rst = ManualSet();
                if (_rst == 0)
                {
                    PLCAction.ManualContinous("ZPos");
                }

                else if (_rst == 1)
                    MessageBox.Show("速度超過上限!請重新調整\nX上限:3000\nY上限:2000\nZ上限:1000");

                //ZPosJogThread = new Thread(new ThreadStart(ZPosJog));
                //Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                //ZPosJogThread.Start();

            }
            
        }
        void ZNagJog()
        {
            //ClickLock = 1;
            ZHomeLockBound = 0;
            ManualSet();
            PLCAction.ManualContinous("ZNeg");

        }

        private void ManualZNegBtn_MouseUp(object sender, MouseEventArgs e)
        {
            if (StageMoveMode == 0)
            {
                ManualSet();
                //if (ZNagJogThread.IsAlive)
                //{
                //    ZNagJogThread.Abort();
                //    ZNagJogThread.Join();
                //}
                PLCAction.ManualContinousPause();
                ZOKColorChange();
            }

        }

        private void ManualZPosBtn_MouseDown(object sender, MouseEventArgs e)
        {
                ////要記得先連通PLC才能執行
                //if (StageMoveMode == 0)
                //{
                //    ZHomeLockBound = 0;
                //    int _rst = ManualSet();
                //    if (_rst == 0)
                //    {
                //        PLCAction.ManualContinous("ZPos");
                //    }

                //    else if (_rst == 1)
                //        MessageBox.Show("速度超過上限!請重新調整\nX上限:3000\nY上限:2000\nZ上限:1000");

                //    //ZPosJogThread = new Thread(new ThreadStart(ZPosJog));
                //    //Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                //    //ZPosJogThread.Start();

                //}
            if (StageMoveMode == 0)
            {
                ZHomeLockBound = 0;
                int _rst = ManualSet();
                if (_rst == 0)
                {
                    PLCAction.ManualContinous("ZNeg");
                }
                else if (_rst == 1)
                    MessageBox.Show("速度超過上限!請重新調整\nX上限:3000\nY上限:2000\nZ上限:1000");
                //ZNagJogThread = new Thread(new ThreadStart(ZNagJog));
                //Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                //ZNagJogThread.Start();

            }

            
        }
        void ZPosJog()
        {
            //ClickLock = 1;
            ZHomeLockBound = 0;
            ManualSet();
            PLCAction.ManualContinous("ZPos");

        }
        private void ManualZPosBtn_MouseUp(object sender, MouseEventArgs e)
        {
            if (StageMoveMode == 0)
            {
                ManualSet();
                //if (ZPosJogThread.IsAlive)
                //{
                //    ZPosJogThread.Abort();
                //    ZPosJogThread.Join();
                //}
                PLCAction.ManualContinousPause();
                ZOKColorChange();
            }

        }

        private void ErrResetBtn_Click(object sender, EventArgs e)
        {
            PLCAction.ErrorReset();
            PLCAction.MainControl(1);
        }

        private void ManualYNegBtn_MouseDown(object sender, MouseEventArgs e)
        {
            //if (ClickLock == 0)
            {

                //要記得先連通PLC才能執行
                if (StageMoveMode == 0)
                {
                    YHomeLockBound = 0;
                    int _rst = ManualSet();
                    if (_rst == 0)
                    {
                        PLCAction.ManualContinous("YNag");
                    }
                    else if (_rst == 1)
                        MessageBox.Show("速度超過上限!請重新調整\nX上限:3000\nY上限:2000\nZ上限:1000");

                    //YNagJogThread = new Thread(new ThreadStart(YNagJog));
                    //Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                    //YNagJogThread.Start();

                }

            }
        }
        void YNagJog()
        {
            //ClickLock = 0;
            YHomeLockBound = 0;
            ManualSet();
            PLCAction.ManualContinous("YNag");

        }
        void YOKColorChange()
        {
            //double[] _ReadVal = PLCAction.ReadPLCDataRandom("D1010\nD1011",2);

            //if (_ReadVal[0] > YLowBound && _ReadVal[0] < YUpBound)
            if (StageYCurPos > YLowBound && StageYCurPos < YUpBound)
            {

                ManualYPosBtn.Enabled = true;
                ManualYPosBtn.BackColor = Color.WhiteSmoke;
                ManualYNegBtn.Enabled = true;
                ManualYNegBtn.BackColor = Color.WhiteSmoke;

            }

        }
        void ZOKColorChange()
        {
            // double[] _ReadVal = PLCAction.ReadPLCDataRandom("D1020\nD1021", 2);

            //if (_ReadVal[0] > ZLowBound && _ReadVal[0] < ZUpBound)
            if (StageZCurPos > ZLowBound && StageZCurPos < ZUpBound)
            {

                ManualZPosBtn.Enabled = true;
                ManualZPosBtn.BackColor = Color.WhiteSmoke;
                ManualZNegBtn.Enabled = true;
                ManualZNegBtn.BackColor = Color.WhiteSmoke;

            }

        }
        void XOKColorChange()
        {
            //double[] _ReadVal = PLCAction.ReadPLCDataRandom("D1000\nD1001", 2);

            // if (_ReadVal[0] > XLowBound && _ReadVal[0] < XUpBound)
            if (StageXCurPos > XLowBound && StageXCurPos < XUpBound)
            {

                ManualXPosBtn.Enabled = true;
                ManualXPosBtn.BackColor = Color.WhiteSmoke;
                ManualXNegBtn.Enabled = true;
                ManualXNegBtn.BackColor = Color.WhiteSmoke;

            }

        }
        private void ManualYNegBtn_MouseUp(object sender, MouseEventArgs e)
        {
            if (StageMoveMode == 0)
            {
                ManualSet();
                //if (YNagJogThread.IsAlive)
                //{
                //    YNagJogThread.Abort();
                //    YNagJogThread.Join();
                //}


                PLCAction.ManualContinousPause();
                YOKColorChange();
            }

        }

        private void ManualAsnPosMoveBtn_Click(object sender, EventArgs e)
        {

            ManualAsnPosRadioBtn_Click(sender, e);
            int _rst = ManualSet();//移動速度也要設定速度
            if (_rst == 0)
            {
                AsnPosMovethread = new Thread(new ThreadStart(AsnPosMove));
                Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                AsnPosMovethread.Start();
            }
            else if (_rst == 1)
                MessageBox.Show("速度超過上限!請重新調整\nX上限:8000\nY上限:6000\nZ上限:5000");

        }

        private void AsnPosMove()
        {

            //平台走位
            double ManualXPos = Convert.ToDouble(ManualXPosTxtBox.Text);
            double ManualYPos = Convert.ToDouble(ManualYPosTxtBox.Text);
            double ManualZPos = Convert.ToDouble(ManualZPosTxtBox.Text);

            //卡上下限
            if (ManualXPos < XLowBound || ManualXPos > XUpBound || ManualYPos < YLowBound || ManualYPos > YUpBound || ManualZPos < ZLowBound || ManualZPos > ZUpBound)
            {

                MessageBox.Show(this, "超過極限!請重新輸入座標! \n"
                                      + " X範圍:" + XLowBound + "~" + XUpBound + "\n"
                                      + " Y範圍:" + YLowBound + "~" + YUpBound + "\n"
                                      + " Z範圍:" + ZLowBound + "~" + ZUpBound);

                return;
            }

            else
            {
                PLCAction.AutoStageMove(ManualXPos, ManualYPos, ManualZPos, 100);//預設手動走3000,100%
                MessageBox.Show(this, "已移動至指定位置!");
            }

        }

        private void ManualYPosBtn_Click(object sender, EventArgs e)
        {

            //if (ClickLock == 0)
            {
                if (StageMoveMode == 1)
                {
                    //HomeLockBound = 0;
                    //ManualSet();
                    //double StepDistVal = Convert.ToDouble(ManualStepDistComboBox.Text);
                    //PLCAction.ManualStepHH("YNag", StepDistVal);
                    YNagStepThread = new Thread(new ThreadStart(YNagStep));
                    Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                    YNagStepThread.Start();
                }
            }
        }
        void YNagStep()
        {
            // ClickLock = 1;
            XHomeLockBound = 0;
            ManualSet();
            double StepDistVal = Convert.ToDouble(ManualStepDistComboBox.Text);

            //double[] _ReadVal = PLCAction.ReadPLCDataRandom("D1010\nD1011", 2);
            double StepTgtY;
            //StepTgtY = Math.Round(_ReadVal[0], 3) - StepDistVal;
            StepTgtY = Math.Round(StageYCurPos, 3) + StepDistVal;

            if (StepTgtY < YLowBound)
            {
                //StepTgtY = YLowBound;
                ManualYPosBtn.Enabled = false;
                ManualYPosBtn.BackColor = Color.Gray;
            }
            //else if (StepTgtY > YUpBound) StepTgtY = YUpBound;
            PLCAction.AutoStageMove(StageXCurPos, StepTgtY, StageZCurPos, 100);//預設手動走3000,100%
            YOKColorChange();
        }

        private void ManualYNegBtn_Click(object sender, EventArgs e)
        {
            // if (ClickLock == 0)
            {
                if (StageMoveMode == 1)
                {

                    YPosStepThread = new Thread(new ThreadStart(YPosStep));
                    Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                    YPosStepThread.Start();
                    //ManualSet();
                    //double StepDistVal = Convert.ToDouble(ManualStepDistComboBox.Text);
                    //PLCAction.ManualStepHH("YPos", StepDistVal);
                }
            }
        }
        void YPosStep()
        {
            // ClickLock = 1;
            XHomeLockBound = 0;

            ManualSet();
            double StepDistVal = Convert.ToDouble(ManualStepDistComboBox.Text);

            //double[] _ReadVal = PLCAction.ReadPLCDataRandom("D1010\nD1011", 2);
            double StepTgtY;
            //StepTgtY = Math.Round(_ReadVal[0], 3) + StepDistVal;
            StepTgtY = Math.Round(StageYCurPos, 3) - StepDistVal;

            if (StepTgtY < YLowBound)
            {
                //StepTgtY = YLowBound;
                ManualYPosBtn.Enabled = false;
                ManualYPosBtn.BackColor = Color.Gray;
            }
            // else if (StepTgtY > YUpBound) StepTgtY = YUpBound;
            PLCAction.AutoStageMove(StageXCurPos, StepTgtY, StageZCurPos, 100);//預設手動走3000,100%
            YOKColorChange();

        }


        private void ManualZNegBtn_Click(object sender, EventArgs e)
        {
            // if (ClickLock == 0)
            {
                if (StageMoveMode == 1)
                {

                    ZNagStepThread = new Thread(new ThreadStart(ZNagStep));
                    Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                    ZNagStepThread.Start();
                }
            }

        }
        void ZNagStep()
        {

            // ClickLock = 1;
            ZHomeLockBound = 0;

            ManualSet();
            double StepDistVal = Convert.ToDouble(ManualStepDistComboBox.Text);

            //double[] _ReadVal = PLCAction.ReadPLCDataRandom("D1020\nD1021", 2);
            double StepTgtZ;
            //StepTgtZ = Math.Round(_ReadVal[0], 3) + StepDistVal;
            StepTgtZ = StageZCurPos + StepDistVal;

            if (StepTgtZ < ZLowBound)
            {
                //StepTgtZ = ZLowBound;
                ManualZNegBtn.Enabled = false;
                ManualZNegBtn.BackColor = Color.Gray;
            }
            //else if (StepTgtZ > ZUpBound) StepTgtZ = ZUpBound;
            PLCAction.AutoStageMove(StageXCurPos, StageYCurPos, StepTgtZ, 100);//預設手動走3000,100%
            ZOKColorChange();

        }
        private void ManualZPosBtn_Click(object sender, EventArgs e)
        {

            // if (ClickLock == 0)
            {
                if (StageMoveMode == 1)
                {

                    ZPosStepThread = new Thread(new ThreadStart(ZPosStep));
                    Form.CheckForIllegalCrossThreadCalls = false; // 存取 UI 時需要用,較不安全的寫法,改用委派較佳(EX: UPDATE TXTBOX)
                    ZPosStepThread.Start();
                }
            }
        }
        void ZPosStep()
        {
            //  ClickLock = 1;
            ZHomeLockBound = 0;

            ManualSet();
            double StepDistVal = Convert.ToDouble(ManualStepDistComboBox.Text);

            //double[] _ReadVal = PLCAction.ReadPLCDataRandom("D1020\nD1021", 2);
            double StepTgtZ;
            //StepTgtZ = Math.Round(_ReadVal[0], 3) - StepDistVal;
            StepTgtZ = StageZCurPos - StepDistVal;

            if (StepTgtZ < ZLowBound)
            {
                //StepTgtZ = ZLowBound;
                ManualZPosBtn.Enabled = false;
                ManualZPosBtn.BackColor = Color.Gray;
            }
            //else if (StepTgtZ > ZUpBound) StepTgtZ = ZUpBound;
            PLCAction.AutoStageMove(StageXCurPos, StageYCurPos, StepTgtZ, 100);//預設手動走3000,100%
            ZOKColorChange();

        }
        private void StageStopBtn_Click(object sender, EventArgs e)
        {
            //// 1.急停
            //PLCAction.axActUtlType1.SetDevice("M1202", 1);

            ////2.伺服馬達 off
            //PLCAction.axActUtlType1.SetDevice("M1207", 0);

            //A+
            // 1.急停
            PLCAction.axActUtlType1.SetDevice("M1041", 1);

            //2.伺服馬達 off
            PLCAction.axActUtlType1.SetDevice("M1042", 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PLCAction.ErrorResetY();
        }

        public void FormStageControl_FormClosed(object sender, FormClosedEventArgs e)
        {
          //  timer1.Dispose();

        }
















    }
}
