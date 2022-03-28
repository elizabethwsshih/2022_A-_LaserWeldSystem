using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WeldControlSystem
{
    public partial class PLCAction : Form
    {
        //  FileIO FileIO = new FileIO();
        public PLCAction()
        {
            InitializeComponent();
        }
        public int PLC_Connect(int iLogicalStationNumber)//LogicalStationNumber for ActUtlType
        {
            int iReturnCode = 0;				//Return code

            try
            {
                //Set the value of 'LogicalStationNumber' to the property.
                axActUtlType1.ActLogicalStationNumber = iLogicalStationNumber;

                //The Open method is executed.
                iReturnCode = axActUtlType1.Open();

                //When the Open method is succeeded, make the EventHandler of ActProgType Controle.
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message,
                            Name, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return iReturnCode;

        }
        //public double[] ReadPLCDataRandom(string DeviceName, int DataSize)//一次讀多個位置
        //{

        //    short[] DataRead = new short[DataSize];
        //    int ReadValINT32;
        //    double ReadValSingle = 0.0;
        //    try
        //    {
        //        axActUtlType1.ReadDeviceRandom2(DeviceName, DataSize, out DataRead[0]);

        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show(exception.Message, Name,
        //                         MessageBoxButtons.OK, MessageBoxIcon.Error);


        //    }
        //    //解析 DeviceName 有幾個D,幾個R, 幾個M
        //    Regex rD = new Regex("D");
        //    MatchCollection MCD;
        //    MCD = rD.Matches(DeviceName);

        //    Regex rR = new Regex("R");
        //    MatchCollection MCR;
        //    MCR = rR.Matches(DeviceName);

        //    double[] ReadVal = new double[((MCD.Count + MCR.Count) / 2) + (DataRead.Length - (MCD.Count + MCR.Count))];

        //    //前幾組D/R要一次讀兩個
        //    int k = 0;
        //    for (int i = 0; i < (MCD.Count + MCR.Count); i = i + 2)
        //    {
        //        ReadValINT32 = Short2Int32(DataRead[i], DataRead[i + 1]);
        //        ReadValSingle = Convert.ToDouble(ReadValINT32) / Convert.ToDouble(10000);
        //        ReadVal[k] = ReadValSingle;
        //        k++;
        //    }
        //    //M一次只要讀一個
        //    for (int i = (MCD.Count + MCR.Count); i < DataRead.Length; i++)
        //    {
        //        ReadValSingle = DataRead[i];
        //        ReadVal[k] = ReadValSingle;
        //        k++;
        //    }

        //    return ReadVal;

        //}
        public double[] ReadPLCDataRandom(string DeviceName, int DataSize)//一次讀多個位置
        {

            short[] DataRead = new short[DataSize];
            int ReadValINT32;
            double ReadValSingle = 0.0;
            try
            {
               // Console.WriteLine("ax Random2 start");
                axActUtlType1.ReadDeviceRandom2(DeviceName, DataSize, out DataRead[0]);
               //  Console.WriteLine("ax Random2 end");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, Name,
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //解析 DeviceName 有幾個D,幾個R, 幾個M
            Regex rD = new Regex("D");
            MatchCollection MCD;
            MCD = rD.Matches(DeviceName);

            Regex rR = new Regex("R");
            MatchCollection MCR;
            MCR = rR.Matches(DeviceName);

            double[] ReadVal = new double[((MCD.Count + MCR.Count) / 2) + (DataRead.Length - (MCD.Count + MCR.Count))];

            //前幾組D/R要一次讀兩個

            //for (int i=0; i< ;i++)
            //{
            //    if()

            //}
            string[] SubDevices;
            SubDevices = ParsingDevice(DeviceName);
            int j = 0, k = 0;
            for (int i = 0; i < SubDevices.Length; i++)
            {
                // D or R 要一次讀兩個,兩個一組
                if (SubDevices[i].Substring(0, 1) == "D" || SubDevices[i].Substring(0, 1) == "R")
                {
                    ReadValINT32 = Short2Int32(DataRead[j], DataRead[j + 1]);
                    ReadValSingle = Convert.ToDouble(ReadValINT32) / Convert.ToDouble(10000);
                    ReadVal[k] = ReadValSingle;
                    i++;  // 因為一次讀兩個,i也要跳過增加1
                    k++;
                    j += 2;
                }
                // M 一次讀一個
                else if (SubDevices[i].Substring(0, 1) == "M")
                {
                    ReadValSingle = DataRead[j];
                    ReadVal[k] = ReadValSingle;
                    k++;
                    j++;
                }
            }
            ////前幾組D/R要一次讀兩個
            //for (int i = 0; i < (MCD.Count + MCR.Count); i = i + 2)
            //{
            //    ReadValINT32 = Short2Int32(DataRead[i], DataRead[i + 1]);
            //    ReadValSingle = Convert.ToDouble(ReadValINT32) / Convert.ToDouble(10000);
            //    ReadVal[k] = ReadValSingle;
            //    k++;
            //}
            ////M一次只要讀一個
            //for (int i = (MCD.Count + MCR.Count); i < DataRead.Length; i++)
            //{
            //    ReadValSingle = DataRead[i];
            //    ReadVal[k] = ReadValSingle;
            //    k++;
            //}

            return ReadVal;

        }

        //ONLY FOR 1個D解碼成 INT16
        public double[] ReadPLCDataRandom16(string DeviceName, int DataSize)//一次讀多個位置
        {

            int[] DataRead16 = new int[DataSize];
            int ReadValINT16;
            double ReadValSingle = 0.0;
            try
            {
                axActUtlType1.ReadDeviceRandom(DeviceName, DataSize, out DataRead16[0]);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, Name,
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //解析 DeviceName 有幾個D,幾個R, 幾個M
            Regex rD = new Regex("D");
            MatchCollection MCD;
            MCD = rD.Matches(DeviceName);


            double[] ReadVal = new double[MCD.Count];


            string[] SubDevices;
            SubDevices = ParsingDevice(DeviceName);

            int j = 0, k = 0;
            for (int i = 0; i < SubDevices.Length; i++)
            {

                if (SubDevices[0].Substring(0, 1) == "D")
                {
                    ReadValINT16 = DataRead16[j];// Short2Int32(DataRead[j], DataRead[j + 1]);
                    ReadValSingle = Convert.ToDouble(ReadValINT16);/// Convert.ToDouble(10000);
                    ReadVal[k] = ReadValSingle;
                    k++;
                    j++;

                }

            }

            return ReadVal;

        }




        public string[] ParsingDevice(string _DeviceName)
        {
            Char delimiter = '\n';
            string[] _SubDevices = _DeviceName.Split(delimiter);
            //foreach (var SubDevice in SubDevices)
            //    Console.WriteLine(SubDevice);
            return _SubDevices;
        }
        public int Short2Int32(short argument1, short argument2)
        {
            int ValueInt32;
            byte[] byteArray1 = BitConverter.GetBytes(argument1);
            byte[] byteArray2 = BitConverter.GetBytes(argument2);
            byte[] byteArray3 = new byte[byteArray1.Length + byteArray2.Length];
            Array.Copy(byteArray1, 0, byteArray3, 0, byteArray1.Length);//合併 2 bytes+ 2 bytes
            Array.Copy(byteArray2, 0, byteArray3, byteArray1.Length, byteArray2.Length);

            ValueInt32 = BitConverter.ToInt32(byteArray3, 0);
            return ValueInt32;
        }
        public int Short2Int16(short argument1)
        {
            int ValueInt16;
            byte[] byteArray1 = BitConverter.GetBytes(argument1);
            //byte[] byteArray3 = new byte[byteArray1.Length];
            //Array.Copy(byteArray1, 0, byteArray3, 0, byteArray1.Length);//合併 2 bytes+ 2 bytes
            //Array.Copy(byteArray2, 0, byteArray3, byteArray1.Length, byteArray2.Length);

            ValueInt16 = BitConverter.ToInt16(byteArray1, 0);
            return ValueInt16;

        }
        public short[] Int16Short(double argument1, int ratio)
        {
            // input double=>32bits=>4 bytes=>2 short
            int arg1;
            arg1 = Convert.ToInt32(argument1 * ratio);
            short[] ValueInt16 = new short[1];
            byte[] byteArray1 = BitConverter.GetBytes(arg1);
            //byte[] byteArray2 = new byte[2];
            //byte[] byteArray3 = new byte[2];
            //for (int i = 0; i <= 1; i++)
            //{
            //    byteArray2[i] = byteArray1[i];
            //}
            //for (int i = 0; i <= 1; i++)
            //{
            //    byteArray3[i] = byteArray1[i + 2];
            //}
            //ValueInt16[0] = BitConverter.ToInt16(byteArray1, 0);
            //ValueInt16[1] = BitConverter.ToInt16(byteArray3, 0);
            return ValueInt16;
        }
        public short[] Int32Short(double argument1, int ratio)
        {
            // input double=>32bits=>4 bytes=>2 short
            int arg1;
            arg1 = Convert.ToInt32(argument1 * ratio);
            short[] ValueInt16 = new short[2];
            byte[] byteArray1 = BitConverter.GetBytes(arg1);
            byte[] byteArray2 = new byte[2];
            byte[] byteArray3 = new byte[2];
            for (int i = 0; i <= 1; i++)
            {
                byteArray2[i] = byteArray1[i];
            }
            for (int i = 0; i <= 1; i++)
            {
                byteArray3[i] = byteArray1[i + 2];
            }
            ValueInt16[0] = BitConverter.ToInt16(byteArray2, 0);
            ValueInt16[1] = BitConverter.ToInt16(byteArray3, 0);
            return ValueInt16;
        }
        public short[] Int32ShortSpeed(double argument1)
        {
            // input double=>32bits=>4 bytes=>2 short
            int arg1;
            arg1 = Convert.ToInt32(argument1 * 100);
            short[] ValueInt16 = new short[2];
            byte[] byteArray1 = BitConverter.GetBytes(arg1);
            byte[] byteArray2 = new byte[2];
            byte[] byteArray3 = new byte[2];
            for (int i = 0; i <= 1; i++)
            {
                byteArray2[i] = byteArray1[i];
            }
            for (int i = 0; i <= 1; i++)
            {
                byteArray3[i] = byteArray1[i + 2];
            }
            ValueInt16[0] = BitConverter.ToInt16(byteArray2, 0);
            ValueInt16[1] = BitConverter.ToInt16(byteArray3, 0);
            return ValueInt16;
        }
        public double ReadPLCData(string DeviceName)
        {
            short[] DataRead = new short[2];
            int ReadValINT32;
            double ReadVal = 0.0;
            try
            {
                axActUtlType1.ReadDeviceBlock2(DeviceName, 2, out DataRead[0]);
                ReadValINT32 = Short2Int32(DataRead[0], DataRead[1]);
                ReadVal = Convert.ToDouble(ReadValINT32) / Convert.ToDouble(10000);
                // ManualMsgTxtBox.Text += Convert.ToString(StepDistVal) + "\r\n";

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, Name,
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
            return ReadVal;
        }
        public void GoHome()//(double OriX, double OriY, double OriZ)
        {

            axActUtlType1.SetDevice("M1023", 1); //z
            axActUtlType1.SetDevice("M1003", 1); //x
            axActUtlType1.SetDevice("M1013", 1); //y

            while (true)
            {
                double[] _ReadVal = ReadPLCDataRandom("M1107\nM1117\nM1127",//4-5-6
                                                        3);
                if (_ReadVal[0] == 1 && _ReadVal[1] == 1 && _ReadVal[2] == 1)
                {
                  
                    break;
                }
            }
            MessageBox.Show("已完成歸 home!");
            

        }
        public void SetManualContinous(int OnOff)
        {
            //axActUtlType1.SetDevice("M1201", OnOff);//連續移動
            //A+
            axActUtlType1.SetDevice("M1004", OnOff);//連續移動
        }
        public void ManualContinous(string ActionPos)
        {
           // axActUtlType1.SetDevice("M1201", 0);//連續移動(滑鼠一值按著)
            //A+
            axActUtlType1.SetDevice("M1004", 0);//連續移動

            if (ActionPos == "XPos")
            {
                axActUtlType1.SetDevice("M1001", 1);//X正向
                //     FileIO.LogMotion("ManualContinous M1001=1");
            }
            else if (ActionPos == "XNag")
            {
                axActUtlType1.SetDevice("M1000", 1);//X反向
                //       FileIO.LogMotion("ManualContinous M1000=1");
            }
            else if (ActionPos == "YPos")
            {
                axActUtlType1.SetDevice("M1010", 1);//Y正向
                //      FileIO.LogMotion("ManualContinous M1010=1");
            }
            else if (ActionPos == "YNag")
            {
                axActUtlType1.SetDevice("M1011", 1);//Y反向
                //     FileIO.LogMotion("ManualContinous M1011=1");
            }
            else if (ActionPos == "ZPos")
            {
                axActUtlType1.SetDevice("M1020", 1);//Z反向
                //     FileIO.LogMotion("ManualContinous M1020=1");
            }
            else if (ActionPos == "ZNeg")
            {
                axActUtlType1.SetDevice("M1021", 1);//Z反向
                //      FileIO.LogMotion("ManualContinous M1021=1");
            }
        }

        public void ManualContinousPause()
        {
            //停止
            axActUtlType1.SetDevice("M1001", 0);//X正向
            //FileIO.LogMotion("ManualContinousPause M1001=1");
            axActUtlType1.SetDevice("M1000", 0);//X反向
            //FileIO.LogMotion("ManualContinousPause M1000=1");
            axActUtlType1.SetDevice("M1010", 0);//Y正向
            //FileIO.LogMotion("ManualContinousPause M1010=1");
            axActUtlType1.SetDevice("M1011", 0);//Y反向
            //FileIO.LogMotion("ManualContinousPause M1011=1");
            axActUtlType1.SetDevice("M1020", 0);//Z正向
            //FileIO.LogMotion("ManualContinousPause M1020=1");
            axActUtlType1.SetDevice("M1021", 0);//Z反向
            //FileIO.LogMotion("ManualContinousPause M1021=1");

        }
        public void ManualStep(string ActionPos)
        {
            //axActUtlType1.SetDevice("M1201", 1);//連續移動(滑鼠按一次只移動一定距離)
            //A+
            axActUtlType1.SetDevice("M1004", 1);//連續移動

            //FileIO.LogMotion("ManualStep M1201=1");
            if (ActionPos == "XPos")
            {
                axActUtlType1.SetDevice("M1001", 1);//X正向
                //  FileIO.LogMotion("ManualStep M1001=1");
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1101\nM1105", 2);
                    if (_ReadVal[0] == 1 && _ReadVal[1] == 0)
                        break;
                }

                axActUtlType1.SetDevice("M1001", 0);
                //FileIO.LogMotion("ManualStep M1001=0");
            }
            else if (ActionPos == "XNag")
            {
                axActUtlType1.SetDevice("M1000", 1);//X反向
                //FileIO.LogMotion("ManualStep M1000=1");
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1100\nM1105", 2);
                    if (_ReadVal[0] == 1 && _ReadVal[1] == 0)
                        break;
                }
                axActUtlType1.SetDevice("M1000", 0);
                //FileIO.LogMotion("ManualStep M1000=0");
            }
            else if (ActionPos == "YPos")
            {
                axActUtlType1.SetDevice("M1010", 1);//Y正向
                //FileIO.LogMotion("ManualStep M1010=1");
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1110\nM1115", 2);
                    if (_ReadVal[0] == 1 && _ReadVal[1] == 0)
                        break;
                }
                axActUtlType1.SetDevice("M1010", 0);
                //FileIO.LogMotion("ManualStep M1010=0");
            }
            else if (ActionPos == "YNag")
            {
                axActUtlType1.SetDevice("M1011", 1);//Y反向
                // FileIO.LogMotion("ManualStep M1011=1");
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1111\nM1115", 2);
                    if (_ReadVal[0] == 1 && _ReadVal[1] == 0)
                        break;
                }
                axActUtlType1.SetDevice("M1011", 0);
                // FileIO.LogMotion("ManualStep M1011=1");
            }
            else if (ActionPos == "ZPos")
            {
                axActUtlType1.SetDevice("M1020", 1);//Z正向
                //FileIO.LogMotion("ManualStep M1020=1");
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1120\nM1125", 2);
                    if (_ReadVal[0] == 1 && _ReadVal[1] == 0)
                        break;
                }
                axActUtlType1.SetDevice("M1020", 0);
                //FileIO.LogMotion("ManualStep M1020=0");
            }
            else if (ActionPos == "ZNeg")
            {
                axActUtlType1.SetDevice("M1021", 1);//Z正向
                // FileIO.LogMotion("ManualStep M1021=1");
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1121\nM1125", 2);
                    if (_ReadVal[0] == 1 && _ReadVal[1] == 0)
                        break;
                }
                axActUtlType1.SetDevice("M1021", 0);
                // FileIO.LogMotion("ManualStep M1021=0");
            }
        }
        public int FindMaxNum(List<int> AreaNumList)
        {
            int MaxNum = 0;
            for (int i = 0; i < AreaNumList.Count(); i++)
            {
                if (AreaNumList[i] > MaxNum) MaxNum = AreaNumList[i];
            }
            return (MaxNum);
        }

        //寫入D(16)值 
        public void WriteD16Val(double InsertVal)
        {

            int[] Int16Val = new int[3];
            Int16Val[0] = 1;
            Int16Val[1] = 3;
            Int16Val[2] = 4;
            axActUtlType1.WriteDeviceBlock("D2257", 1, ref Int16Val[0]);
            axActUtlType1.WriteDeviceBlock("D2258", 1, ref Int16Val[1]);
            axActUtlType1.WriteDeviceBlock("D2259", 1, ref Int16Val[2]);
            double[] test = ReadPLCDataRandom16("D2257\nD2258\nD2259", 3);

            for (int i = 0; i < test.Count(); i++)
                Console.WriteLine("test[" + i + "]=" + test[i]);
        }

        public void ManualSet(double StepDistVal, double XSpeedVal, double YSpeedVal, double ZSpeedVal, double SpeedRateVal)
        {

            short[] StepDist = new short[2];
            short[] XSpeed = new short[2];
            short[] YSpeed = new short[2];
            short[] ZSpeed = new short[2];
            short[] SpeedRate = new short[2];
            short[] XTarget = new short[2];
            short[] YTarget = new short[2];
            short[] ZTarget = new short[2];


            //if (ManualStepDistComboBox.Text.Length > 0)
            StepDist = Int32Short(StepDistVal, 10000);

            //if (ManualXSpeedTxtBox.Text.Length > 0)
            XSpeed = Int32ShortSpeed(XSpeedVal);

            //if (ManualYSpeedTxtBox.Text.Length > 0)
            YSpeed = Int32ShortSpeed(YSpeedVal);

            ZSpeed = Int32ShortSpeed(ZSpeedVal);

            SpeedRate = Int32Short(SpeedRateVal, 10);

            //寫入吋動距離
            try
            { 
                axActUtlType1.WriteDeviceBlock2("R1202", 2, ref StepDist[0]); 
            }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }


            //寫入X軸速度
            try
            { 
                axActUtlType1.WriteDeviceBlock2("R1002", 2, ref XSpeed[0]); 
            }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }



            //寫入Y軸速度
            try
            { 
                axActUtlType1.WriteDeviceBlock2("R1012", 2, ref YSpeed[0]);
            }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            //寫入Z軸速度
            try
            {
                axActUtlType1.WriteDeviceBlock2("R1022", 2, ref ZSpeed[0]); 
            }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            //寫入速度比例
            try
            { 
                axActUtlType1.WriteDeviceBlock2("R1200", 2, ref SpeedRate[0]); 
            
            }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }


        }
        public void AutoZAxisMove(int i, int j, double IniZPt, string[][] AutoSpeedArray)
        {
            double ZTgtPos1, ZTgtPos2;
            short[] ZTarget = new short[2];
            short[] ZSpeed = new short[2];
            ZTgtPos1 = IniZPt;

            //寫入Z 軸目標位置
            ZTarget = Int32Short(ZTgtPos1, 10000);
            try
            { axActUtlType1.WriteDeviceBlock2("R1020", 2, ref ZTarget[0]); }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            ZTgtPos2 = ReadPLCData("R1020");

            //寫入Z軸速度
            for (int k = 0; k < AutoSpeedArray.Length; k++)
            {
                if (AutoSpeedArray[k][0] == "(" + (i + 1) + ',' + (j + 1) + ")") //user介面比實際陣列編號多1
                {
                    ZSpeed = Int32Short(Convert.ToDouble(AutoSpeedArray[k][3]), 100);
                    break;
                }
            }
            //--------------Z移動動作--------------
            axActUtlType1.SetDevice("M1122", 0);//自動模式要用一速移,先設為0避免卡住
            axActUtlType1.SetDevice("M1122", 1);//自動模式要用一速移動


            //Console.WriteLine("Z一速移動開始 M1122=1");
            //FileIO.LogMotion("Z一速移動開始 M1122=1");
            while (true)
            {
                double[] _ReadVal = ReadPLCDataRandom("D1020\nD1021\nM1122\nM1125", 4);
                if (_ReadVal[1] == 1 && _ReadVal[2] == 0 && _ReadVal[0] == ZTgtPos2)
                    break;

            }

            axActUtlType1.SetDevice("M1122", 0);//自動模式要用一速移動
            //Console.WriteLine("X結束一速接收命令狀態 M1122=0");
            //FileIO.LogMotion("X結束一速接收命令狀態 M1122=0");


        }
        public void AutoStageMove(double IniXPt, double IniYPt, double IniZPt, double SpeedRate)
        {
            double XTgtPos1, YTgtPos1 = 0;
            short[] XTarget = new short[2];
            short[] YTarget = new short[2];
            short[] XSpeed = new short[2];
            short[] YSpeed = new short[2];

            double ZTgtPos1;
            short[] ZTarget = new short[2];
            short[] ZSpeed = new short[2];

            //================ X ======================
            XTgtPos1 = IniXPt;
            //寫入X 軸目標位置
            XTarget = Int32Short(XTgtPos1, 10000);
            try
            { axActUtlType1.WriteDeviceBlock2("R1000", 2, ref XTarget[0]); }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            // XTgtPos2 = ReadPLCData("R1000");
            //寫入X軸速度-----------------------------------
            XSpeed = Int32Short(3000 * SpeedRate / 100, 100);
            try
            { axActUtlType1.WriteDeviceBlock2("R1002", 2, ref XSpeed[0]); }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            //--------------X移動動作--------------
            axActUtlType1.SetDevice("M1002", 0);//自動模式要用一速移,先設為0避免卡住
            axActUtlType1.SetDevice("M1002", 1);//自動模式要用一速移動
            //FileIO.LogMotion("X一速移動開始 M1002=1");
            //----確認是否接受指令
            while (true)
            {
                double[] _ReadVal = ReadPLCDataRandom("M1102", 1);
                if (_ReadVal[0] == 1)
                {
                    axActUtlType1.SetDevice("M1002", 0);
                    break;
                }
             
            }
           
            ////-----------------判斷是否移動至目標位置完成--------------------

            while (true)
            {
                double[] _ReadVal = ReadPLCDataRandom("D1048\nD1049\nM1102\nM1105", 4);
                if (//_ReadVal[1] == 1 && _ReadVal[2] == 0 && 
                    Math.Abs(_ReadVal[0] - XTgtPos1) < 0.001)
                    break;

            }
            //axActUtlType1.SetDevice("M1002", 0);//自動模式要用一速移動
            // FileIO.LogMotion("X結束一速接收命令狀態 M1002=0");
            //-----------------------------------------------------

            //================ Y ======================
            YTgtPos1 = IniYPt;
            //寫入Y 軸目標位置
            YTarget = Int32Short(YTgtPos1, 10000);
            try
            { axActUtlType1.WriteDeviceBlock2("R1010", 2, ref YTarget[0]); }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            //Console.WriteLine("寫入Y 軸目標位置 M1010" + "YTgtPos1=" + YTgtPos1);
            //寫入Y軸速度
            YSpeed = Int32Short(2000 * SpeedRate / 100, 100);
            try
            { axActUtlType1.WriteDeviceBlock2("R1012", 2, ref YSpeed[0]); }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            //--------------Y移動動作--------------
            axActUtlType1.SetDevice("M1012", 0);//自動模式要用一速移,先設為0避免卡住
            axActUtlType1.SetDevice("M1012", 1);//自動模式要用一速移動
            //FileIO.LogMotion("Y一速移動開始 M1012=1");
            //----確認是否接受指令
            while (true)
            {
                double[] _ReadVal = ReadPLCDataRandom("M1112", 1);
                if (_ReadVal[0] == 1)
                {
                    axActUtlType1.SetDevice("M1012", 0);
                    break;
                }

            }
            //-----------------判斷是否移動至目標位置完成--------------------
            while (true)
            {
                double[] _ReadVal = ReadPLCDataRandom("D1052\nD1053\nM1112\nM1115", 4);
                if (//_ReadVal[1] == 1 && _ReadVal[2] == 0 && 
                    Math.Abs(_ReadVal[0] - YTgtPos1) < 0.001)
                    break;
                //   Console.WriteLine("Y一速移動,YTgtPos1=" + YTgtPos1 + ", _ReadVal[0]=" + _ReadVal[0]);
            }
            //axActUtlType1.SetDevice("M1012", 0);//自動模式要用一速移動
            //  FileIO.LogMotion("Y結束一速接收命令狀態 M1012=0");

            //-----------------------------------------------------
            //201903 復歸MARK
            //================ Z ======================
            ZTgtPos1 = IniZPt;
            //寫入Z 軸目標位置
            ZTarget = Int32Short(ZTgtPos1, 10000);
            ZSpeed = Int32Short(1000 * SpeedRate / 100, 100);
            try
            { axActUtlType1.WriteDeviceBlock2("R1020", 2, ref ZTarget[0]); }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }


            //寫入z軸速度
            try
            { axActUtlType1.WriteDeviceBlock2("R1022", 2, ref ZSpeed[0]); }
            catch (Exception exception)
            { MessageBox.Show(exception.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            //--------------Z移動動作--------------
            axActUtlType1.SetDevice("M1022", 0);//自動模式要用一速移,先設為0避免卡住
            axActUtlType1.SetDevice("M1022", 1);//自動模式要用一速移動
            //----確認是否接受指令
            while (true)
            {
                
                double[] _ReadVal = ReadPLCDataRandom("M1122", 1);
                if (_ReadVal[0] == 1)
                {
                    axActUtlType1.SetDevice("M1022", 0);
                    break;
                }

            }
            //Console.WriteLine("Z一速移動開始 M1022=1");
            //FileIO.LogMotion("Z一速移動開始 M1022=1");
            while (true)
            {
                double[] _ReadVal = ReadPLCDataRandom("D1060\nD1061\nM1122\nM1125", 4);
                if (//_ReadVal[1] == 1 && _ReadVal[2] == 0 && 
                    Math.Abs(_ReadVal[0] - ZTgtPos1) < 0.001)
                    break;

            }

            //axActUtlType1.SetDevice("M1022", 0);//自動模式要用一速移動
            //FileIO.LogMotion("Z 結束一速接收命令狀態 M1122=0");




        }
        public void LaserReq()
        {
            axActUtlType1.SetDevice("X36", 0);
            axActUtlType1.SetDevice("Y70", 0);

        }
        public int PLC_HandShakingRead()
        {
            //系統交握訊號讀回
            //double[] _ReadVal1 = ReadPLCDataRandom("M1209", 1);
            //int M1209_ = Convert.ToInt32(_ReadVal1[0]);
            //return M1209_;

            //A+
            double[] _ReadVal1 = ReadPLCDataRandom("M1007", 1);
            int M1007_ = Convert.ToInt32(_ReadVal1[0]);
            return M1007_;
        }

        public void PLC_AllInitial()
        {
            //1.全部清0
            PLC_AllClear();


            //2. ERROR RESET
            //axActUtlType1.SetDevice("M1200", 1);
            //while (true)
            //{

            //    double[] _ReadVal = ReadPLCDataRandom("M1210", 1);
            //    if (_ReadVal[0] == 1)
            //    {
            //        axActUtlType1.SetDevice("M1200", 0);
            //        // axActUtlType1.SetDevice("M1202", 0);
            //        break;
            //    }
            //}

            //3.伺服馬達重啟
         //   PLC_StageMotor(1);




        }

        private void PLCAction_Load(object sender, EventArgs e)
        {
            ////1. 通訊
            PLC_Connect(0);

            ////2.初始化:清0 + err reset + 重啟伺服馬達
            PLC_AllInitial();

            ////3.主控權
            MainControl(1);//1:pc,0:plc

            //4.聽訊號
            //TimerCallback callback1 = new TimerCallback(_listen);
            //timer1 = new System.Threading.Timer(callback1, null, 0, 250);//250豪秒起來一次
        }
        public void MainControl(int PC_ControlVal)
        {
            //axActUtlType1.SetDevice("M1203", PC_ControlVal);//plc主控權給PC
            axActUtlType1.SetDevice("M1005", PC_ControlVal);//plc主控權給PC

        }
        public void PLC_HandShaking(int OnOff)//1:開啟交握,0:關閉
        {
            //axActUtlType1.SetDevice("M1230", OnOff);//handshaking啟動關閉
            //A+
            axActUtlType1.SetDevice("M1230", OnOff);
        }
        public void PLC_StageMotor(int OnOff)
        {
            if (OnOff == 1)
            {
                //axActUtlType1.SetDevice("M1208", 0); //防呆先關
                //axActUtlType1.SetDevice("M1207", 1); //馬達on要等三秒.   
                
                //A+      
                axActUtlType1.SetDevice("M1043", 0); //防呆先關
                axActUtlType1.SetDevice("M1042", 1); //馬達on要等三秒. 
                while (true)
                {
                   //double[] _ReadVal = ReadPLCDataRandom("M1211", 1); //伺服馬達要等ready才能繼續進行下步驟
                    //A+
                    double[] _ReadVal = ReadPLCDataRandom("M1142", 1); //伺服馬達要等ready才能繼續進行下步驟

                    if (_ReadVal[0] == 1)
                    {
                        break;
                    }

                }
            }
            else
            {
                
                //axActUtlType1.SetDevice("M1202", 1);//20211019 自己加的伺服馬達關閉
                //axActUtlType1.SetDevice("M1207", 0);//20211019 自己加的伺服馬達關閉
                //axActUtlType1.SetDevice("M1208", 1);//伺服馬達關閉
                //axActUtlType1.SetDevice("M1208", 0);
                //A+  
                axActUtlType1.SetDevice("M1041", 1);//20211019 自己加的伺服馬達關閉
                axActUtlType1.SetDevice("M1042", 0);//20211019 自己加的伺服馬達關閉
                axActUtlType1.SetDevice("M1043", 1);//伺服馬達關閉
                axActUtlType1.SetDevice("M1043", 0);
                axActUtlType1.SetDevice("M1041", 0);
                
            }

        }
        public void PLC_Close()
        {
            axActUtlType1.Close();
        }
        public void PLC_AllClear()
        {
            //初始化所有平台動作指令

            axActUtlType1.SetDevice("M1000", 0);
            axActUtlType1.SetDevice("M1001", 0);
            axActUtlType1.SetDevice("M1002", 0);
            axActUtlType1.SetDevice("M1003", 0);
            axActUtlType1.SetDevice("M1003", 0);
            axActUtlType1.SetDevice("M1005", 0);
            axActUtlType1.SetDevice("M1006", 0);
            axActUtlType1.SetDevice("M1007", 0);
            axActUtlType1.SetDevice("M1008", 0);
            axActUtlType1.SetDevice("M1010", 0);
            axActUtlType1.SetDevice("M1011", 0);
            axActUtlType1.SetDevice("M1012", 0);
            axActUtlType1.SetDevice("M1013", 0);
            axActUtlType1.SetDevice("M1014", 0);
            axActUtlType1.SetDevice("M1015", 0);
            axActUtlType1.SetDevice("M1016", 0);
            axActUtlType1.SetDevice("M1017", 0);
            axActUtlType1.SetDevice("M1018", 0);
            axActUtlType1.SetDevice("M1019", 0);
            axActUtlType1.SetDevice("M1020", 0);
            axActUtlType1.SetDevice("M1021", 0);
            axActUtlType1.SetDevice("M1022", 0);
            axActUtlType1.SetDevice("M1023", 0);
            axActUtlType1.SetDevice("M1024", 0);
            axActUtlType1.SetDevice("M1025", 0);
            axActUtlType1.SetDevice("M1026", 0);
            axActUtlType1.SetDevice("M1027", 0);
            axActUtlType1.SetDevice("M1028", 0);
            axActUtlType1.SetDevice("M1029", 0);
            axActUtlType1.SetDevice("M1030", 0);
            axActUtlType1.SetDevice("M1031", 0);
            axActUtlType1.SetDevice("M1032", 0);
            axActUtlType1.SetDevice("M1033", 0);
            axActUtlType1.SetDevice("M1034", 0);
            axActUtlType1.SetDevice("M1035", 0);
            axActUtlType1.SetDevice("M1036", 0);
            axActUtlType1.SetDevice("M1037", 0);
            axActUtlType1.SetDevice("M1038", 0);
            axActUtlType1.SetDevice("M1039", 0);
            axActUtlType1.SetDevice("M1040", 0);
            axActUtlType1.SetDevice("M1041", 0);

            //1222
            axActUtlType1.SetDevice("M1044", 0);


            axActUtlType1.SetDevice("M1100", 0);
            axActUtlType1.SetDevice("M1101", 0);
            axActUtlType1.SetDevice("M1102", 0);
            axActUtlType1.SetDevice("M1103", 0);
            axActUtlType1.SetDevice("M1104", 0);
            axActUtlType1.SetDevice("M1105", 0);
            axActUtlType1.SetDevice("M1106", 0);
            axActUtlType1.SetDevice("M1107", 0);
            axActUtlType1.SetDevice("M1108", 0);
            axActUtlType1.SetDevice("M1109", 0);
            axActUtlType1.SetDevice("M1110", 0);
            axActUtlType1.SetDevice("M1111", 0);
            axActUtlType1.SetDevice("M1112", 0);
            axActUtlType1.SetDevice("M1113", 0);
            axActUtlType1.SetDevice("M1114", 0);
            axActUtlType1.SetDevice("M1115", 0);
            axActUtlType1.SetDevice("M1116", 0);
            axActUtlType1.SetDevice("M1117", 0);
            axActUtlType1.SetDevice("M1118", 0);
            axActUtlType1.SetDevice("M1119", 0);
            axActUtlType1.SetDevice("M1120", 0);
            axActUtlType1.SetDevice("M1121", 0);
            axActUtlType1.SetDevice("M1122", 0);
            axActUtlType1.SetDevice("M1123", 0);
            axActUtlType1.SetDevice("M1124", 0);
            axActUtlType1.SetDevice("M1125", 0);
            axActUtlType1.SetDevice("M1126", 0);
            axActUtlType1.SetDevice("M1127", 0);
            axActUtlType1.SetDevice("M1128", 0);
            axActUtlType1.SetDevice("M1129", 0);
            axActUtlType1.SetDevice("M1130", 0);
            axActUtlType1.SetDevice("M1131", 0);
            axActUtlType1.SetDevice("M1132", 0);
            axActUtlType1.SetDevice("M1133", 0);
            axActUtlType1.SetDevice("M1134", 0);
            axActUtlType1.SetDevice("M1135", 0);
            axActUtlType1.SetDevice("M1136", 0);
            axActUtlType1.SetDevice("M1137", 0);
            axActUtlType1.SetDevice("M1138", 0);
            axActUtlType1.SetDevice("M1139", 0);
            axActUtlType1.SetDevice("M1140", 0);

            axActUtlType1.SetDevice("M1200", 0);
            axActUtlType1.SetDevice("M1201", 0);
            axActUtlType1.SetDevice("M1202", 0);
            axActUtlType1.SetDevice("M1203", 0);
            axActUtlType1.SetDevice("M1204", 0);
            axActUtlType1.SetDevice("M1205", 0);
            axActUtlType1.SetDevice("M1206", 0);
            axActUtlType1.SetDevice("M1207", 0);
            axActUtlType1.SetDevice("M1208", 0);
            axActUtlType1.SetDevice("M1209", 0);
            axActUtlType1.SetDevice("M1210", 0);
            axActUtlType1.SetDevice("M1211", 0);
            axActUtlType1.SetDevice("M1212", 0);
            axActUtlType1.SetDevice("M1213", 0);
            axActUtlType1.SetDevice("M1214", 0);
            axActUtlType1.SetDevice("M1215", 0);
            axActUtlType1.SetDevice("M1216", 0);
            axActUtlType1.SetDevice("M1217", 0);
            axActUtlType1.SetDevice("M1218", 0);
            axActUtlType1.SetDevice("M1219", 0);
            axActUtlType1.SetDevice("M1220", 0);
            axActUtlType1.SetDevice("M1221", 0);
            axActUtlType1.SetDevice("M1222", 0);
            axActUtlType1.SetDevice("M1223", 0);
            axActUtlType1.SetDevice("M1224", 0);
            axActUtlType1.SetDevice("M1225", 0);
            axActUtlType1.SetDevice("M1226", 0);
            axActUtlType1.SetDevice("M1227", 0);
            axActUtlType1.SetDevice("M1228", 0);
            axActUtlType1.SetDevice("M1229", 0);
            axActUtlType1.SetDevice("M1230", 0);

            //for A+改掉
            axActUtlType1.SetDevice("M1040", 0);
            axActUtlType1.SetDevice("M1004", 0);
            axActUtlType1.SetDevice("M1041", 0);
            axActUtlType1.SetDevice("M1005", 0);
            axActUtlType1.SetDevice("M1006", 0);
            axActUtlType1.SetDevice("M1017", 0);
            axActUtlType1.SetDevice("M1018", 0);
            axActUtlType1.SetDevice("M1042", 0);
            axActUtlType1.SetDevice("M1043", 0);
            axActUtlType1.SetDevice("M1007", 0);
            axActUtlType1.SetDevice("M1140", 0);
            axActUtlType1.SetDevice("M1142", 0);
            axActUtlType1.SetDevice("M1141", 0);

            axActUtlType1.SetDevice("M1043", 0);

            axActUtlType1.SetDevice("M1044", 0);
            axActUtlType1.SetDevice("M1045", 0);
            //axActUtlType1.SetDevice("M1215", 0);
            axActUtlType1.SetDevice("M1156", 0);
            //axActUtlType1.SetDevice("M1217", 0);
            //axActUtlType1.SetDevice("M1218", 0);
            //axActUtlType1.SetDevice("M1219", 0);
            axActUtlType1.SetDevice("M1046", 0);
            //axActUtlType1.SetDevice("M1221", 0);
            //axActUtlType1.SetDevice("M1222", 0);
            //axActUtlType1.SetDevice("M1223", 0);
            axActUtlType1.SetDevice("M1049", 0);
            //axActUtlType1.SetDevice("M1225", 0);
            axActUtlType1.SetDevice("M1048", 0);
            axActUtlType1.SetDevice("M1016", 0);
            //axActUtlType1.SetDevice("M1228", 0);
            //axActUtlType1.SetDevice("M1229", 0);
            //axActUtlType1.SetDevice("M1230", 0);

        }

        public void PLCAction_FormClosed(object sender, FormClosedEventArgs e)
        {
            //全部清0
            PLC_AllClear();

            //關閉伺服馬達
            PLC_StageMotor(0);

            //停止交握
            PLC_HandShaking(0);

            //交還主控權給PLC
            MainControl(0);

            //關閉PLC元件
            PLC_Close();
        }

        private void PLCAction_FormClosing(object sender, FormClosingEventArgs e)
        {
            //全部清0
            PLC_AllClear();

            //關閉伺服馬達
            PLC_StageMotor(0);

            //停止交握
            PLC_HandShaking(0);

            //交還主控權給PLC
            MainControl(0);

            //關閉PLC元件
            PLC_Close();
        }
        public void ErrorReset()
        {


            ////1.取消馬達停止
            //axActUtlType1.SetDevice("M1202", 0);

            ////1. ERROR RESET
            //axActUtlType1.SetDevice("M1200", 1);
            //while (true)
            //{

            //    double[] _ReadVal = ReadPLCDataRandom("M1210", 1);
            //    if (_ReadVal[0] == 1)
            //    {
            //        axActUtlType1.SetDevice("M1200", 0);
            //        //// axActUtlType1.SetDevice("M1202", 0);
            //        break;
            //    }
            //}



            //A+
            axActUtlType1.SetDevice("M1041", 0);

            //1. ERROR RESET
            axActUtlType1.SetDevice("M1040", 1);
            while (true)
            {

                double[] _ReadVal = ReadPLCDataRandom("M1140", 1);
                if (_ReadVal[0] == 1)
                {
                    axActUtlType1.SetDevice("M1040", 0);
                    //// axActUtlType1.SetDevice("M1202", 0);
                    break;
                }
            }
            //4.伺服馬達重啟
            PLC_StageMotor(1);
           

            PLC_AllClear();

            //202109 主控權
            //axActUtlType1.SetDevice("M1203", 1);//0: PLC , 1: PC
            //A+
            axActUtlType1.SetDevice("M1005", 1);//0: PLC , 1: PC


        }
        public void ErrorResetY()
        {


           

            //1. ERROR RESET
            //axActUtlType1.SetDevice("M1200", 1);
            //while (true)
            //{

            //    double[] _ReadVal = ReadPLCDataRandom("M1210", 1);
            //    if (_ReadVal[0] == 1)
            //    {
            //        axActUtlType1.SetDevice("M1200", 0);
            //        //// axActUtlType1.SetDevice("M1202", 0);
            //        break;
            //    }
            //}

            //A+
            axActUtlType1.SetDevice("M1040", 1);
            while (true)
            {

                double[] _ReadVal = ReadPLCDataRandom("M1140", 1);
                if (_ReadVal[0] == 1)
                {
                    axActUtlType1.SetDevice("M1040", 0);
                    break;
                }
            }

        }
        public void ManualStepHH(string ActionPos, double StepDistVal)
        {

            double[] _ReadVal = ReadPLCDataRandom("D1000\nD1001\nD1010\nD1011\nD1020\nD1021", 6);

            double StepTgtX, StepTgtY, StepTgtZ;


            StepTgtX = Math.Round(_ReadVal[0], 3);
            StepTgtY = Math.Round(_ReadVal[1], 3);
            StepTgtZ = Math.Round(_ReadVal[2], 3);

            if (ActionPos == "XPos")
            {
                //計算目標位置:向右增一單位
                StepTgtX = Math.Round(_ReadVal[0], 3) + StepDistVal;
                AutoStageMove(StepTgtX, StepTgtY, StepTgtZ, 100);//預設手動走3000,100%
            }
            else if (ActionPos == "XNag")
            {
                //計算目標位置:向左一單位
                StepTgtX = Math.Round(_ReadVal[0], 3) - StepDistVal;
                AutoStageMove(StepTgtX, StepTgtY, StepTgtZ, 100);//預設手動走3000,100%
            }


            else if (ActionPos == "YPos")
            {
                //計算目標位置:向左一單位
                StepTgtY = Math.Round(_ReadVal[1], 3) + StepDistVal;
                AutoStageMove(StepTgtX, StepTgtY, StepTgtZ, 100);//預設手動走3000,100%
            }
            else if (ActionPos == "YNag")
            {
                //計算目標位置:向左一單位
                StepTgtY = Math.Round(_ReadVal[1], 3) - StepDistVal;
                AutoStageMove(StepTgtX, StepTgtY, StepTgtZ, 100);//預設手動走3000,100%
            }


            else if (ActionPos == "ZPos")
            {
                //計算目標位置:向左一單位
                StepTgtZ = Math.Round(_ReadVal[2], 3) + StepDistVal;
                AutoStageMove(StepTgtX, StepTgtY, StepTgtZ, 100);//預設手動走3000,100%
            }
            else if (ActionPos == "ZNag")
            {
                //計算目標位置:向左一單位
                StepTgtZ = Math.Round(_ReadVal[2], 3) - StepDistVal;
                AutoStageMove(StepTgtX, StepTgtY, StepTgtZ, 100);//預設手動走3000,100%
            }



        }
        public void TrackIn(string PortName)
        {
            if (PortName == "A")
            {
                axActUtlType1.SetDevice("M1031", 1);
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1131", 1);
                    if (_ReadVal[0] == 1)
                    {
                        axActUtlType1.SetDevice("M1031", 0);
                        break;
                    }
                }

            }
            else
            {
                axActUtlType1.SetDevice("M1035", 1);
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1135", 1);
                    if (_ReadVal[0] == 1)
                    {
                        axActUtlType1.SetDevice("M1035", 0);
                        break;
                    }
                }

            }



        }
        public void TrackOut(string PortName)
        {
            if (PortName == "A")
            {
                axActUtlType1.SetDevice("M1030", 1);
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1130", 1);
                    if (_ReadVal[0] == 1)
                    {
                        axActUtlType1.SetDevice("M1030", 0);
                        break;
                    }
                }

            }
            else
            {
                axActUtlType1.SetDevice("M1034", 1);
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1134", 1);
                    if (_ReadVal[0] == 1)
                    {
                        axActUtlType1.SetDevice("M1034", 0);
                        break;
                    }
                }

            }


        }
        public void Bounding(string PortName)
        {

            //if (PortName == "A")
            //{
                axActUtlType1.SetDevice("M1038", 1);
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1138", 1);
                    if (_ReadVal[0] == 1)
                    {
                        axActUtlType1.SetDevice("M1038", 0);
                        break;
                    }
                }
            //}
            //else
            //{
            //    axActUtlType1.SetDevice("M1036", 0);
            //    axActUtlType1.SetDevice("M1036", 1);
            //    while (true)
            //    {
            //        double[] _ReadVal = ReadPLCDataRandom("M1136", 1);
            //        if (_ReadVal[0] == 1)
            //        {
            //            axActUtlType1.SetDevice("M1036", 0);
            //            break;
            //        }
            //    }

            //}

        }
        public void Release(string PortName)
        {

            //if (PortName == "A")
            //{
            axActUtlType1.SetDevice("M1039", 0);
                axActUtlType1.SetDevice("M1039", 1);
                while (true)
                {
                    double[] _ReadVal = ReadPLCDataRandom("M1139", 1);
                    if (_ReadVal[0] == 1)
                    {
                        axActUtlType1.SetDevice("M1039", 0);
                        break;
                    }
                }
            //}
            //else
            //{
            //    axActUtlType1.SetDevice("M1037", 1);
            //    while (true)
            //    {
            //        double[] _ReadVal = ReadPLCDataRandom("M1137", 1);
            //        if (_ReadVal[0] == 1)
            //        {
            //            axActUtlType1.SetDevice("M1037", 0);
            //            break;
            //        }
            //    }

            //}

        }
        public void OnLaserReq()
        {
           // axActUtlType1.SetDevice("M1220", 1);
            //A+
            axActUtlType1.SetDevice("M1046", 1);
        }
        public void OffLaserReq()
        {
            //axActUtlType1.SetDevice("M1220", 0);
            //A+
            axActUtlType1.SetDevice("M1046", 0);
        }
        public void OnLaserInterCntl()
        {
            //axActUtlType1.SetDevice("M1222", 1);
            //A+
            axActUtlType1.SetDevice("M1047", 1);
        }
        public void OffLaserInterCntl()
        {
            //axActUtlType1.SetDevice("M1222", 0);
            //A+
            axActUtlType1.SetDevice("M1047", 0);
        }
        public void OnLaserGuide()
        {
            //axActUtlType1.SetDevice("M1224", 1);
            //A+
            axActUtlType1.SetDevice("M1049", 1);
        }
        public void OffLaserGuide()
        {
            //axActUtlType1.SetDevice("M1224", 0);
            //A+
            axActUtlType1.SetDevice("M1049", 0);
        }
        public void OnLaserOn()
        {
            //axActUtlType1.SetDevice("M1226", 1);
            //A+
            axActUtlType1.SetDevice("M1048", 1);
        }
        public void OffLaserOn()
        {
            //axActUtlType1.SetDevice("M1226", 0);
            //A+
            axActUtlType1.SetDevice("M1048", 0);
        }
        public void OnLaserProStart()
        {
            axActUtlType1.SetDevice("M1221", 1);
        }
        public void OffLaserProStart()
        {
            axActUtlType1.SetDevice("M1221", 0);
        }
        public void OnLaserResetErr()
        {
            axActUtlType1.SetDevice("M1223", 1);
        }
        public void OffLaserResetErr()
        {
            axActUtlType1.SetDevice("M1223", 0);
        }
        public void OnLaserAnalogCntl()
        {
            axActUtlType1.SetDevice("M1225", 1);
        }
        public void OffLaserAnalogCntl()
        {
            axActUtlType1.SetDevice("M1225", 0);
        }
        public void OnLaserSCReset()
        {
            //axActUtlType1.SetDevice("M1227", 1);
            //A+
            axActUtlType1.SetDevice("M1016", 1);
        }
        public void OffLaserSCReset()
        {
            //axActUtlType1.SetDevice("M1227", 0);
            //A+
            axActUtlType1.SetDevice("M1016", 0);
        }

        public void FlowWeldModuleIn()
        {
            //先聽M1159確定=1再發M1025
            while (true)
            {
                double[] _ReadVal1 = ReadPLCDataRandom("M1157", 1);
                if (_ReadVal1[0] == 1)
                {
                    Console.WriteLine("分料機1: 可以準備入料了! M1157=1");
                    axActUtlType1.SetDevice("M1024", 1);
                    double[] _ReadVal = ReadPLCDataRandom("M1024", 1);
                    if (_ReadVal[0] == 1)
                    {
                        Console.WriteLine("已經通知分料機1可以入料了!");
                    }
                    break;
                }
            }

            //確認1160=1完成出料
            while (true)
            {
                double[] _ReadVal2 = ReadPLCDataRandom("M1158", 1);
                if (_ReadVal2[0] == 1)
                {
                    Console.WriteLine("分料機1: 完成出料了! M1158=1");
                    break;
                }
            }


            //axActUtlType1.SetDevice("M1024", 1);

            //double[] _ReadVal = ReadPLCDataRandom("M1024", 1);
            //if (_ReadVal[0] == 1 )
            //{
            //    Console.WriteLine("已經通知分料機1可以入料了!");
            //}
            //while (true)
            //{
            //    double[] _ReadVal1 = ReadPLCDataRandom("M1157", 1);
            //    if (_ReadVal1[0] == 1)
            //    {
            //        Console.WriteLine("分料機1: 可以準備入料了! M1157=1");
            //        break;
            //    }
            //}
            //while (true)
            //{
            //    double[] _ReadVal2 = ReadPLCDataRandom("M1158", 1);
            //    if (_ReadVal2[0] == 1)
            //    {
            //        Console.WriteLine("分料機1: 完成入料了! M1158=1");
            //        break;
            //    }
            //}
        }
        public void FlowWeldModuleOut()
        {
            //先聽M1159確定=1再發M1025
            while (true)
            {
                double[] _ReadVal1 = ReadPLCDataRandom("M1159", 1);
                if (_ReadVal1[0] == 1)
                {
                    Console.WriteLine("分料機2: 可以準備出料了! M1159=1");
                    axActUtlType1.SetDevice("M1025", 1);
                    double[] _ReadVal = ReadPLCDataRandom("M1025", 1);
                    if (_ReadVal[0] == 1)
                    {
                        Console.WriteLine("已經通知分料機2可以出料了!");
                    }
                    break;
                }
            }

            //確認1160=1完成出料
            while (true)
            {
                double[] _ReadVal2 = ReadPLCDataRandom("M1160", 1);
                if (_ReadVal2[0] == 1)
                {
                    Console.WriteLine("分料機2: 完成出料了! M1160=1");
                    break;
                }
            }
           
        
        }

        //public ReadRepairData()
        //{
        //    double[] _ReadVal2 = ReadDeviceRandom("D2257", 1);
        //    Console.WriteLine("_ReadVal2=" + _ReadVal2[0]);
        
        //}
        //////////////////////


    }
}
