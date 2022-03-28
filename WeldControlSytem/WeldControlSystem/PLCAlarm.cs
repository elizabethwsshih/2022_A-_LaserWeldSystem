using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeldControlSystem
{
    class PLCAlarm
    {
        public string AlarmFind(string AlarmCode)//回傳訊息
        {
            string AlarmString = "";

            if (AlarmCode == "F1")
                AlarmString = "_EMO1 雷銲機_加工區_緊急停止";
            if (AlarmCode == "F2")
                AlarmString = "_EMO2 雷焊機_A區_緊急停止";
            if (AlarmCode == "F3")
                AlarmString = "_EMO3 雷焊機_B區_緊急停止";
            if (AlarmCode == "F23")
                AlarmString = "_雷銲機X軸異常";
            if (AlarmCode == "F24")
                AlarmString = "_雷銲機Y1軸異常";
            if (AlarmCode == "F25")
                AlarmString = "_MS4-3 雷銲機Y2軸異常";
            if (AlarmCode == "F26")
                AlarmString = "_MS4-4 雷銲機Z軸異常";
            if (AlarmCode == "F31")
                AlarmString = "_冷水機異常";
            if (AlarmCode == "F32")
                AlarmString = "_Y2尚未同步運行";
            if (AlarmCode == " F33")
                AlarmString = "_電腦交握逾時";


            if (AlarmCode == "F71")
                AlarmString = "_0071雷射輸出功率異常";
            if (AlarmCode == "F72")
                AlarmString = "_0072雷射反射光過高異常";
            if (AlarmCode == "F73")
                AlarmString = "_0073雷射模組光訊號異常";
            if (AlarmCode == "F74")
                AlarmString = "_0074雷射QBH異常";
            if (AlarmCode == "F76")
                AlarmString = "_0076雷射緊急按鈕觸發異常";
            if (AlarmCode == "F77")
                AlarmString = "_0077雷射外部安全迴路異常";
            if (AlarmCode == "F78")
                AlarmString = "_0078雷射機箱門迴路異常";
            if (AlarmCode == "F79")
                AlarmString = "_0079雷射動力源異常";
            if (AlarmCode == "F80")
                AlarmString = "_0080雷射溫度或流量異常";
            if (AlarmCode == "F81")
                AlarmString = "_0081雷射模組溫度異常";
            if (AlarmCode == "F82")
                AlarmString = "_0082雷射機箱濕度過高異常";
            if (AlarmCode == "F83")
                AlarmString = "_0083雷射機箱漏水異常";
            if (AlarmCode == "F84")
                AlarmString = "_0084雷射電流輸出異常";
            if (AlarmCode == "F85")
                AlarmString = "_0085雷射其他異常";



            if (AlarmCode == "F400")
                AlarmString = "_雷銲機X軸重大異常";
            if (AlarmCode == "F401")
                AlarmString = "_雷銲機X軸重大異常";
            if (AlarmCode == "F402")
                AlarmString = "_雷銲機X軸原點復歸位置異常";
            if (AlarmCode == "F403")
                AlarmString = "_雷銲機X軸JOG微動異常";
            if (AlarmCode == "F404")
                AlarmString = "_雷銲機X軸定位執行異常";
            if (AlarmCode == "F405")
                AlarmString = "_雷銲機X軸同期控制輸入軸異常";
            if (AlarmCode == "F406")
                AlarmString = "_雷銲機X軸同期控制輸出軸異常";
            if (AlarmCode == "F407")
                AlarmString = "_雷銲機X軸I/F介面異常";
            if (AlarmCode == "F408")
                AlarmString = "_雷銲機X軸參數設置異常";
            if (AlarmCode == "F409")
                AlarmString = "_雷銲機X軸編碼器異常";
            if (AlarmCode == "F410")
                AlarmString = "_雷銲機X軸伺服異常";
            if (AlarmCode == "F411")
                AlarmString = "_雷銲機X軸正極限被觸發";
            if (AlarmCode == "F412")
                AlarmString = "_雷銲機X軸負極限被觸發";
            if (AlarmCode == "F420")
                AlarmString = "_雷銲機Y1軸重大異常";
            if (AlarmCode == "F421")
                AlarmString = "_雷銲機Y1軸通用異常";
            if (AlarmCode == "F422")
                AlarmString = "_雷銲機Y1軸原點復歸位置異常";
            if (AlarmCode == "F423")
                AlarmString = "_雷銲機Y1軸JOG微動異常";
            if (AlarmCode == "F424")
                AlarmString = "_雷銲機Y1軸定位執行異常";
            if (AlarmCode == "F425")
                AlarmString = "_雷銲機Y1軸同期控制輸入軸異常";
            if (AlarmCode == "F426")
                AlarmString = "_雷銲機Y1軸同期控制輸出軸異常";
            if (AlarmCode == "F427")
                AlarmString = "_雷銲機Y1軸I/F介面異常";
            if (AlarmCode == "F428")
                AlarmString = "_雷銲機Y1軸參數設置異常";
            if (AlarmCode == "F429")
                AlarmString = "_雷銲機Y1軸編碼器異常";
            if (AlarmCode == "F430")
                AlarmString = "_雷銲機Y1軸伺服異常";
            if (AlarmCode == "F431")
                AlarmString = "_雷銲機Y1軸正極限被觸發";
            if (AlarmCode == "F432")
                AlarmString = "_雷銲機Y1軸負極限被觸發";
            if (AlarmCode == "F440")
                AlarmString = "_雷銲機Y2軸重大異常";
            if (AlarmCode == "F441")
                AlarmString = "_雷銲機Y2軸通用異常";
            if (AlarmCode == "F442")
                AlarmString = "_雷銲機Y2軸原點復歸位置異常";
            if (AlarmCode == "F443")
                AlarmString = "_雷銲機Y2軸JOG微動異常";
            if (AlarmCode == "F444")
                AlarmString = "_雷銲機Y2軸JOG微動異常";
            if (AlarmCode == "F445")
                AlarmString = "_雷銲機Y2軸同期控制輸入軸異常";
            if (AlarmCode == "F446")
                AlarmString = "_雷銲機Y2軸同期控制輸出軸異常";
            if (AlarmCode == "F447")
                AlarmString = "_雷銲機Y2軸I/F介面異常";
            if (AlarmCode == "F448")
                AlarmString = "_雷銲機Y2軸參數設置異常";
            if (AlarmCode == "F449")
                AlarmString = "_雷銲機Y2軸編碼器異常";
            if (AlarmCode == "F450")
                AlarmString = "_雷銲機Y2軸伺服異常";
            if (AlarmCode == "F451")
                AlarmString = "_雷銲機Y2軸正極限被觸發";
            if (AlarmCode == "F452")
                AlarmString = "_雷銲機Y2軸負極限被觸發";
            if (AlarmCode == "F460")
                AlarmString = "_雷銲機Z軸重大異常";
            if (AlarmCode == "F461")
                AlarmString = "_雷銲機Z軸通用異常";
            if (AlarmCode == "F462")
                AlarmString = "_雷銲機Z軸原點復歸位置異常";
            if (AlarmCode == "F463")
                AlarmString = "_雷銲機Z軸JOG微動異常";
            if (AlarmCode == "F464")
                AlarmString = "_雷銲機Z軸定位執行異常";
            if (AlarmCode == "F465")
                AlarmString = "_雷銲機Z軸同期控制輸入軸異常";
            if (AlarmCode == "F466")
                AlarmString = "_雷銲機Z軸同期控制輸出軸異常";
            if (AlarmCode == "F467")
                AlarmString = "_雷銲機Z軸I/F介面異常";
            if (AlarmCode == "F468")
                AlarmString = "_雷銲機Z軸參數設置異常";
            if (AlarmCode == "F469")
                AlarmString = "_雷銲機Z軸編碼器異常";
            if (AlarmCode == "F470")
                AlarmString = "_雷銲機Z軸伺服異常";
            if (AlarmCode == "F471")
                AlarmString = "_雷銲機Z軸正極限被觸發";
            if (AlarmCode == "F472")
                AlarmString = "_雷銲機Z軸負極限被觸發";
            if (AlarmCode == "F9")
                AlarmString = "_安全門未關好";







            return AlarmString;
        }
        /*
        F1	EMO1 雷銲機_加工區_緊急停止
        F2	EMO2 雷焊機_A區_緊急停止
        F3	EMO3 雷焊機_B區_緊急停止
        F4	EMO4
        F5	EMO5 ROBOT1
        F6	EMO6 ROBOT2
        F7	EMO7 ROBOT3
        F8	EMO8 檢查機
        F9	安全門未關好
        F10	入氣端壓力低下
      
        F23	MS4-1 雷銲機X軸異常
        F24	MS4-2 雷銲機Y1軸異常
        F25	MS4-3 雷銲機Y2軸異常
        F26	MS4-4 雷銲機Z軸異常
        F27	MS4-1 檢查機X軸異常
        F28	MS4-2 檢查機Y軸異常
        F29	MS4-3 回流輸送軸異常
        F30	MS4-4 翻轉軸異常
        F31	冷水機異常
        F32	Y2尚未同步運行
        F33	電腦交握逾時
      
        F71	_0071雷射輸出功率異常
        F72	_0072雷射反射光過高異常
        F73	_0073雷射模組光訊號異常
        F74	_0074雷射QBH異常
        F75	_0075雷射光纖接線箱異常
        F76	_0076雷射緊急按鈕觸發異常
        F77	_0077雷射外部安全迴路異常
        F78	_0078雷射機箱門迴路異常
        F79	_0079雷射動力源異常
        F80	_0080雷射溫度或流量異常
        F81	_0081雷射模組溫度異常
        F82	_0082雷射機箱濕度過高異常
        F83	_0083雷射機箱漏水異常
        F84	_0084雷射電流輸出異常
        F85	_0085雷射其他異常

        F400	雷銲機X軸重大異常
        F401	雷銲機X軸通用異常
        F402	雷銲機X軸原點復歸位置異常
        F403	雷銲機X軸JOG微動異常
        F404	雷銲機X軸定位執行異常
        F405	雷銲機X軸同期控制輸入軸異常
        F406	雷銲機X軸同期控制輸出軸異常
        F407	雷銲機X軸I/F介面異常
        F408	雷銲機X軸參數設置異常
        F409	雷銲機X軸編碼器異常
        F410	雷銲機X軸伺服異常
        F411	雷銲機X軸正極限被觸發
        F412	雷銲機X軸負極限被觸發
        

        F420	雷銲機Y1軸重大異常
        F421	雷銲機Y1軸通用異常
        F422	雷銲機Y1軸原點復歸位置異常
        F423	雷銲機Y1軸JOG微動異常
        F424	雷銲機Y1軸定位執行異常
        F425	雷銲機Y1軸同期控制輸入軸異常
        F426	雷銲機Y1軸同期控制輸出軸異常
        F427	雷銲機Y1軸I/F介面異常
        F428	雷銲機Y1軸參數設置異常
        F429	雷銲機Y1軸編碼器異常
        F430	雷銲機Y1軸伺服異常
        F431	雷銲機Y1軸正極限被觸發
        F432	雷銲機Y1軸負極限被觸發

        F440	雷銲機Y2軸重大異常
        F441	雷銲機Y2軸通用異常
        F442	雷銲機Y2軸原點復歸位置異常
        F443	雷銲機Y2軸JOG微動異常
        F444	雷銲機Y2軸定位執行異常
        F445	雷銲機Y2軸同期控制輸入軸異常
        F446	雷銲機Y2軸同期控制輸出軸異常
        F447	雷銲機Y2軸I/F介面異常
        F448	雷銲機Y2軸參數設置異常
        F449	雷銲機Y2軸編碼器異常
        F450	雷銲機Y2軸伺服異常
        F451	雷銲機Y2軸正極限被觸發
        F452	雷銲機Y2軸負極限被觸發
        
        F460	雷銲機Z軸重大異常
        F461	雷銲機Z軸通用異常
        F462	雷銲機Z軸原點復歸位置異常
        F463	雷銲機Z軸JOG微動異常
        F464	雷銲機Z軸定位執行異常
        F465	雷銲機Z軸同期控制輸入軸異常
        F466	雷銲機Z軸同期控制輸出軸異常
        F467	雷銲機Z軸I/F介面異常
        F468	雷銲機Z軸參數設置異常
        F469	雷銲機Z軸編碼器異常
        F470	雷銲機Z軸伺服異常
        F471	雷銲機Z軸正極限被觸發
        F472	雷銲機Z軸負極限被觸發

 
         * */
    }



}
