using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
namespace WeldControlSystem
{
    public class DefectRepairInfo
    {
        public string VisionDefectAreaIdx;
        public string VisionWeldID;
    }
    public class RepairDictionary
    {
        public string DicSide;
        public string DicWeldCnt;
        public string DicVisionAreaID;
        public string DicVisionWeldID;
        public string DicMMRank;
        public string DicMMName;
        public string DicWeldObjID;
        public string DicRepairOK1;
        public string DicRepairOK2;
    }
    public class ProdMgrFile
    {
        public string FileHead = "ACCOUNT,"
                        + "EQP_ID,"
                        + "ORDER,"
                        + "PROGRAM,"
                        + "MODULE,"
                        + "STEP,"
                        + "RECIPE,"
                        + "STARTEND,"
                        + "START_TIME,"
                        + "END_TIME,"
                        + "RUN_TIME,"
                        + "STATUS,"
                        + "REMARK";

        public string ACCOUNT;  //人員工號
        public string EQP_ID; //機台名稱
        public string ORDER; //製令單號(掃描)
        public string PROGRAM; //系統執行檔名
        public string MODULE; //模組條碼(掃描)
        public string STEP; //執行步驟
        public string RECIPE; //製程檔名
        public string STARTEND; //開始/結束
        public string START_TIME; //開始時間
        public string END_TIME; //結束時間
        public string RUN_TIME; //執行時間
        public string STATUS; //執行結果 Y成功,E失敗
        public string REMARK; //ALARM 訊息
        public string ProdFileName;
    }
    class FileIO
    {
        ProdMgrFile MgrFile;
        [DllImport("kernel32")]
        private static extern uint GetPrivateProfileString(string section, string key, string defaultvalue, byte[] buf, uint size, string FilePath);
        public void IniProdMgrFile() //完全初始化
        {
            MgrFile = new ProdMgrFile();
            MgrFile.ACCOUNT = null;  //人員工號
            MgrFile.EQP_ID = null; //機台名稱
            MgrFile.ORDER = null; //製令單號(掃描)
            MgrFile.PROGRAM = null; //系統執行檔名
            MgrFile.MODULE = null; //模組條碼(掃描)
            MgrFile.STEP = null; //執行步驟
            MgrFile.RECIPE = null; //執行步驟
            MgrFile.START_TIME = null; //開始時間
            MgrFile.END_TIME = null; //結束時間
            MgrFile.RUN_TIME = null; //執行時間
            MgrFile.STATUS = null; //執行結果 Y成功,E失敗
            MgrFile.REMARK = null; //ALARM 訊息
            MgrFile.STARTEND = null; //開始/結束
        }
        public void IniProdMgrFile2()//部分初始化,保留人員機台名稱等,產生新檔案前必加
        {
            //MgrFile.ACCOUNT = null;  //人員工號
            //MgrFile.EQP_ID = null; //機台名稱
            //MgrFile.ORDER = null; //製令單號(掃描)
            //MgrFile.PROGRAM = null; //系統執行檔名
            //MgrFile.MODULE = null; //模組條碼(掃描)
            MgrFile.STEP = null; //執行步驟
            //MgrFile.RECIPE = null; //執行步驟
            MgrFile.START_TIME = null; //開始時間
            MgrFile.END_TIME = null; //結束時間
            MgrFile.RUN_TIME = null; //執行時間
            MgrFile.STATUS = null; //執行結果 Y成功,E失敗
            MgrFile.REMARK = null; //ALARM 訊息
            MgrFile.STARTEND = null; //ALARM 訊息
        }
        public void SetProdMgrFile(string FileItem, string ItemText)
        {
            if (FileItem == "ACCOUNT")
                MgrFile.ACCOUNT = ItemText;  //人員工號

            else if (FileItem == "EQP_ID")
                MgrFile.EQP_ID = ItemText; //機台名稱

            else if (FileItem == "ORDER")
                MgrFile.ORDER = ItemText; //製令單號(掃描)

            else if (FileItem == "PROGRAM")
                MgrFile.PROGRAM = ItemText; //系統執行檔名

            else if (FileItem == "MODULE")
                MgrFile.MODULE = ItemText; //模組條碼(掃描)

            else if (FileItem == "STEP")
                MgrFile.STEP = ItemText; //執行步驟

            else if (FileItem == "RECIPE")
                MgrFile.RECIPE = ItemText; //製成檔名

            else if (FileItem == "START_TIME")
                MgrFile.START_TIME = ItemText; //開始時間

            else if (FileItem == "END_TIME")
                MgrFile.END_TIME = ItemText; //結束時間

            else if (FileItem == "RUN_TIME")
                MgrFile.RUN_TIME = ItemText; //執行時間

            else if (FileItem == "STATUS")
                MgrFile.STATUS = ItemText; //執行結果 Y成功,E失敗

            else if (FileItem == "REMARK")
                MgrFile.REMARK = ItemText; //ALARM 訊息

            else if (FileItem == "STARTEND")
                MgrFile.STARTEND = ItemText;
        }
        public void LogProdMgrFile(string ActStr, string FilePath)
        {

            FilePath = FilePath + "\\" + DateTime.Now.ToString("yyyyMMddhhmmss")
                //+ "_" + MgrFile.MODULE
                //+ "_" + ActStr  
                                + ".csv";
            StreamWriter sw;
            //if (File.Exists(FilePath))
            //{

            //    sw = File.AppendText(FilePath);
            //    //--------------
            //    sw.Write(MgrFile.FileHead + "\r\n");  //檔頭
            //    sw.Write(MgrFile.ACCOUNT + ",");  //人員工號
            //    sw.Write(MgrFile.EQP_ID + ","); //機台名稱
            //    sw.Write(MgrFile.ORDER + ","); //製令單號(掃描)
            //    sw.Write(MgrFile.PROGRAM + ","); //系統執行檔名
            //    sw.Write(MgrFile.MODULE + ","); //模組條碼(掃描)
            //    sw.Write(MgrFile.STEP + ","); //執行步驟
            //    sw.Write(MgrFile.RECIPE + ","); //製程檔名
            //    sw.Write(MgrFile.STARTEND + ","); //開始/結束
            //    sw.Write(MgrFile.START_TIME + ","); //開始時間
            //    sw.Write(MgrFile.END_TIME + ","); //結束時間
            //    sw.Write(MgrFile.RUN_TIME + ","); //執行時間
            //    sw.Write(MgrFile.STATUS + ","); //執行結果 Y成功,E失敗
            //    sw.Write(MgrFile.REMARK); //ALARM 訊息
            //    //--------------
            //    sw.Flush();
            //    sw.Close();
            //}
            //else
            //{
            FileStream fs = File.Create(FilePath);
            sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            //--------------
            sw.Write(MgrFile.FileHead + "\r\n");  //檔頭
            sw.Write(MgrFile.ACCOUNT + ",");  //人員工號
            sw.Write(MgrFile.EQP_ID + ","); //機台名稱
            sw.Write(MgrFile.ORDER + ","); //製令單號(掃描)
            sw.Write(MgrFile.PROGRAM + ","); //系統執行檔名
            sw.Write(MgrFile.MODULE + ","); //模組條碼(掃描)
            sw.Write(MgrFile.STEP + ","); //執行步驟
            sw.Write(MgrFile.RECIPE + ","); //製程檔名
            sw.Write(MgrFile.STARTEND + ","); //開始/結束
            sw.Write(MgrFile.START_TIME + ","); //開始時間
            sw.Write(MgrFile.END_TIME + ","); //結束時間
            sw.Write(MgrFile.RUN_TIME + ","); //執行時間
            sw.Write(MgrFile.STATUS + ","); //執行結果 Y成功,E失敗
            sw.Write(MgrFile.REMARK); //ALARM 訊息
            //--------------
            sw.Flush();
            sw.Close();
            fs.Close();
            //}
        }
        public List<RepairDictionary> ReadRepairConfig(string ConfigFileName)
        {
            List<RepairDictionary> _RepairDictionary = new List<RepairDictionary>();
            RepairDictionary DicUnit = new RepairDictionary();
            try
            {
                string line;
                System.IO.StreamReader sr = new System.IO.StreamReader(ConfigFileName, Encoding.GetEncoding(950));
                sr = System.IO.File.OpenText(ConfigFileName);
                string substr;

                while ((line = sr.ReadLine()) != null)
                {
                    DicUnit = new RepairDictionary();
                    DicUnit.DicSide = line.Substring(0, line.IndexOf(","));
                    // Console.WriteLine("DicUnit.DicSide="+DicUnit.DicSide);

                    substr = line.Substring(line.IndexOf(",") + 1, line.Length - (line.IndexOf(",") + 1));
                    DicUnit.DicVisionAreaID = substr.Substring(0, substr.IndexOf(","));
                    //Console.WriteLine("DicUnit.DicVisionAreaID="+DicUnit.DicVisionAreaID);



                    substr = substr.Substring(substr.IndexOf(",") + 1, substr.Length - (substr.IndexOf(",") + 1));
                    DicUnit.DicVisionWeldID = substr.Substring(0, substr.IndexOf(","));
                    //  Console.WriteLine(" DicUnit.DicVisionWeldID="+ DicUnit.DicVisionWeldID);

                    substr = substr.Substring(substr.IndexOf(",") + 1, substr.Length - (substr.IndexOf(",") + 1));
                    DicUnit.DicMMName = substr.Substring(0, substr.IndexOf(","));
                    //  Console.WriteLine("DicUnit.DicMMName="+DicUnit.DicMMName);

                    substr = substr.Substring(substr.IndexOf(",") + 1, substr.Length - (substr.IndexOf(",") + 1));
                    DicUnit.DicWeldObjID = substr.Substring(0, substr.Length);
                    // Console.WriteLine("DicUnit.DicWeldObjID="+DicUnit.DicWeldObjID);

                    _RepairDictionary.Add(DicUnit);
                }
            }
            catch
            {

            }
            return _RepairDictionary;
        }
        public void LogAction(string BtnName)
        {
            string FilePath = @"D://MM_PLC_Setting//LogFiles//" + DateTime.Now.ToString("yyyyMMdd_HH") + "LogFile.txt";
            StreamWriter sw;
            if (File.Exists(FilePath))
            {
                sw = File.AppendText(FilePath);
                sw.Write("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + BtnName + "\r\n");
                sw.Flush();
                sw.Close();
            }
            else
            {
                FileStream fs = File.Create(FilePath);//new FileStream(FilePath, FileMode.Create);
                sw = new StreamWriter(fs);
                sw.Write("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + BtnName + "\r\n");
                sw.Flush();
                sw.Close();
                fs.Close();
            }

        }
        public void LogMotion(string MotionStr)
        {
            string FilePath = @"D://MM_PLC_Setting//LogFiles//MotionLog//" + DateTime.Now.ToString("yyyyMMdd_HH") + "MotionLog.txt";
            StreamWriter sw;
            if (File.Exists(FilePath))
            {
                sw = File.AppendText(FilePath);
                sw.Write("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + MotionStr + "\r\n");
                sw.Flush();
                sw.Close();
            }
            else
            {
                FileStream fs = File.Create(FilePath);
                sw = new StreamWriter(fs);
                sw.Write("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + MotionStr + "\r\n");
                sw.Flush();
                sw.Close();
                fs.Close();
            }

        }

        public string GetFileName(string FilePath)
        {
            string FileName = null;
            FileName = FilePath.Substring(FilePath.LastIndexOf("\\") + 1, FilePath.Length - (FilePath.LastIndexOf("\\") + 1));
            return (FileName);
        }
        public int GetMemberPW(String SettingFilePath, String _User, String _PW, out String _Category)
        {
            int result = -1;
            _Category = "";
            string line, user_s, pw_s, category_s;
            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(SettingFilePath, Encoding.GetEncoding(950));
                sr = System.IO.File.OpenText(SettingFilePath);

                while ((line = sr.ReadLine()) != null)
                {
                    user_s = line.Substring(0, line.IndexOf(",")); ;
                    pw_s = line.Substring(line.IndexOf(",") + 1, line.LastIndexOf(",") - (line.IndexOf(",") + 1));
                    category_s = line.Substring(line.LastIndexOf(",") + 1, line.Length - (line.LastIndexOf(",") + 1));
                    if (_User == user_s && _PW == pw_s)
                    {
                        result = 1;
                        _Category = category_s;
                        break;
                    }
                    else result = 0;
                }
            }
            catch
            {
                result = 2;
            }
            return result;
        }
        public string GetLensFile()
        {
            string SettingFilePath = @"D://MM_PLC_Setting//Setting.ini";
            string line, LensFileStr = "";
            System.IO.StreamReader sr = new System.IO.StreamReader(SettingFilePath, Encoding.GetEncoding(950));
            sr = System.IO.File.OpenText(SettingFilePath);

            while ((line = sr.ReadLine()) != null)
            {
                //substr = line.Substring(line.IndexOf("]") + 1, line.Length - (line.IndexOf("]") + 1));

                if (line.IndexOf("[LensFile]") >= 0)
                {
                    LensFileStr = line.Substring(line.IndexOf("]") + 1, line.Length - (line.IndexOf("]") + 1));
                    break;
                }
            }
            return LensFileStr;
        }

        public int CycleCnt()
        {
            string SettingFilePath = @"D://MM_PLC_Setting//Setting.ini";
            //string line,  CycleStr = "";
            //System.IO.StreamReader sr = new System.IO.StreamReader(SettingFilePath, Encoding.GetEncoding(950));
            //sr = System.IO.File.OpenText(SettingFilePath);
            int CycleNum = 0;
            string CycleStr;


            //while ((line = sr.ReadLine()) != null)
            //{

            //    if (line.IndexOf("[CycleCnt]") >= 0)
            //    {
            //        CycleStr = line.Substring(line.IndexOf("]") + 1, line.Length - (line.IndexOf("]") + 1));
            //        CycleNum = Convert.ToInt32(CycleStr);
            //        break;
            //    }
            //}
            //sr.Close();
            string[] txt = File.ReadAllLines(SettingFilePath);
            for (int i = 0; i < txt.Length; i++)
            {
                if (txt[i].IndexOf("[CycleCnt]") >= 0)
                {
                    CycleStr = txt[i].Substring(txt[i].IndexOf("]") + 1, txt[i].Length - (txt[i].IndexOf("]") + 1));
                    CycleNum = Convert.ToInt32(CycleStr);
                    txt[i] = "[CycleCnt]" + (CycleNum + 1).ToString();
                    break;
                }
            }
            File.WriteAllLines(SettingFilePath, txt);

            return CycleNum;
        }
        public List<string> IniDoubleReadSec(String FileName)
        {
            List<string> RetKeyVal = new List<string>();
            byte[] buf = new byte[65536];

            uint len = GetPrivateProfileString(null, null, null, buf, (uint)buf.Length, FileName);

            int j = 0;
            for (int i = 0; i < len; i++)
            {
                if (buf[i] == 0)
                {
                    RetKeyVal.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            }

            return (RetKeyVal);
        }
        public List<double> IniDoubleReadKey(List<String> SecList, String Key, String FileName)
        {
            List<double> RetKeyVal = new List<double>();
            byte[] buf2 = new byte[65536];

            foreach (string Sec in SecList)
            {
                uint len = GetPrivateProfileString(Sec, Key, "", buf2, (uint)buf2.Length, FileName);

                int j = 0;
                for (int i = 0; i <= len; i++)
                {
                    if (buf2[i] == 0)
                    {
                        RetKeyVal.Add(Convert.ToDouble(Encoding.Default.GetString(buf2, j, i - j)));
                        j = i + 1;
                    }
                }
            }
            return (RetKeyVal);
        }
        public List<Int32> IniIntReadKey(List<String> SecList, String Key, String FileName)
        {
            List<Int32> RetKeyVal = new List<Int32>();
            byte[] buf2 = new byte[65536];

            foreach (string Sec in SecList)
            {
                uint len = GetPrivateProfileString(Sec, Key, "", buf2, (uint)buf2.Length, FileName);

                int j = 0;
                for (int i = 0; i <= len; i++)
                {
                    if (buf2[i] == 0)
                    {
                        RetKeyVal.Add(Convert.ToInt32(Encoding.Default.GetString(buf2, j, i - j)));
                        j = i + 1;
                    }
                }
            }
            return (RetKeyVal);
        }
        public List<string> IniStrReadKey(List<String> SecList, String Key, String FileName)//適用多個重複key,會回傳多個值
        {
            List<string> RetKeyVal = new List<string>();
            byte[] buf2 = new byte[65536];//System.Text.UnicodeEncoding.Unicode.GetBytes();//new byte[65536];

            foreach (string Sec in SecList)
            {
                uint len = GetPrivateProfileString(Sec, Key, "", buf2, (uint)buf2.Length, FileName);

                int j = 0;
                for (int i = 0; i <= len; i++)
                {
                    if (buf2[i] == 0)
                    {

                        RetKeyVal.Add(Encoding.UTF8.GetString(buf2, j, i - j));//utf處理中文亂碼
                        j = i + 1;
                    }
                }
            }
            return (RetKeyVal);
        }
        public string IniStrReadKeySingle(List<String> SecList, String Key, String FileName)//適用只有單一無重複 key,只會回傳單一值
        {
            string RetKeyVal = null;
            byte[] buf2 = new byte[65536];

            foreach (string Sec in SecList)
            {
                uint len = GetPrivateProfileString(Sec, Key, "", buf2, (uint)buf2.Length, FileName);

                if (buf2[0] != 0) // 表示有找到 key
                {
                    int j = 0;
                    for (int i = 0; i <= len; i++)
                    {
                        if (buf2[i] == 0)
                        {

                            // RetKeyVal.Add(Encoding.UTF8.GetString(buf2, j, i - j));//utf處理中文亂碼
                            //  j = i + 1;
                            RetKeyVal = Encoding.UTF8.GetString(buf2, j, i - j);
                            break;
                        }
                    }

                }
            }//for each
            return (RetKeyVal);
        }
        public List<int> IniAreaNumGet(List<String> AreaNumList, string dir)
        {
            List<int> RetKeyVal = new List<int>();
            string substr;
            foreach (string AreaNum in AreaNumList)
            {

                if (AreaNum.IndexOf(",") > 0 && dir == "x")
                {
                    substr = AreaNum.Substring(0, AreaNum.IndexOf(","));
                    RetKeyVal.Add(Convert.ToInt32(substr));
                }
                else if (AreaNum.IndexOf(",") > 0 && dir == "y")
                {
                    substr = AreaNum.Substring(AreaNum.IndexOf(",") + 1, AreaNum.Length - AreaNum.IndexOf(",") - 1);
                    RetKeyVal.Add(Convert.ToInt32(substr));
                }


            }


            return (RetKeyVal);
        }
        public void DeleteFile(String DelFilePath, String FileType, int Days)
        {
            string[] files = Directory.GetFiles(DelFilePath, FileType);// "*.csv");
            foreach (string file in files)
            {
                FileInfo f = new FileInfo(file);
                TimeSpan ts = DateTime.Now - f.CreationTime;
                double TimeDiff = Convert.ToDouble(ts.TotalDays.ToString());
                if (TimeDiff > Days)
                    File.Delete(file);
            }
        }
        public List<RepairDictionary> ReadWeldDictionay(List<RepairDictionary> _ConnectDict)
        {

            string DictPath = @"D:\ITRI\WeldSystemSetting\WeldMeasMapping.csv";

            string line, LensFileStr = "";
            System.IO.StreamReader sr = new System.IO.StreamReader(DictPath, Encoding.GetEncoding(950));
            sr = System.IO.File.OpenText(DictPath);
            RepairDictionary dicP ;
            string[] _SubLine;
            while ((line = sr.ReadLine()) != null)
            {
                dicP = new RepairDictionary();
                _SubLine = new string[7];
                _SubLine = ParsingCSV(line);
                dicP.DicSide = _SubLine[0];
                dicP.DicVisionAreaID = _SubLine[1];
                dicP.DicVisionWeldID = _SubLine[2];
                dicP.DicMMRank = _SubLine[3];
                dicP.DicMMName = _SubLine[4];
                dicP.DicWeldObjID = _SubLine[5];
                _ConnectDict.Add(dicP);
            }
           
            return _ConnectDict;
        }
        public string[] ParsingCSV(string _line)
        {
            Char delimiter = ',';
            string[] _SubLine = _line.Split(delimiter);
            return _SubLine;
        }

    }
}
