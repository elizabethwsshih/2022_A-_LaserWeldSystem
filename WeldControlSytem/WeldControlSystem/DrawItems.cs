using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WeldControlSystem
{
    class DrawItems
    {

        public Bitmap DrawRect(Bitmap bmp,double _x,double _y,double _w,double _h)
        { 
            Graphics gr = Graphics.FromImage(bmp);
            Pen BlackPen = new Pen(Color.Black, 3);
            Rectangle PicRect = new Rectangle(Convert.ToInt32(_x),Convert.ToInt32(_y),Convert.ToInt32(_w), Convert.ToInt32(_h));
            gr.DrawRectangle(BlackPen, PicRect);
            return bmp;
        }
        public Bitmap DrawBlock(Bitmap bmp, double x1, double y1, double x2, double y2, double x3, double y3,
                               string str)//有著色, 1:綠色, 0:白色, 2: 黃色
        {
            Graphics gr = Graphics.FromImage(bmp);
            Bitmap bmp2 = new Bitmap(str);
            //將圖案繪製於固定大小之畫布
            gr.DrawImage(bmp2, new Point[] { new Point((int)x1, (int)y1), new Point((int)x2, (int)y2), new Point((int)x3, (int)y3) });
            Console.WriteLine(str);
            return bmp;
        }
        
        public Bitmap FillRectsBySteps(Bitmap _Bitmap,int x_cnt, int y_cnt, int XAreaIdx, int YAreaIdx, int color_mode, string action_mode)//有著色, 1:綠色, 0:白色, 2: 黃色
        {

            double x_width_pic = 0.0, y_height_pic = 0.0;
          
            Bitmap DemoBitmap = _Bitmap;

          
            Graphics g = Graphics.FromImage(DemoBitmap);
            System.Drawing.SolidBrush FillBrushGreen = new System.Drawing.SolidBrush(Color.Green);
            System.Drawing.SolidBrush FillBrushWhite = new System.Drawing.SolidBrush(Color.White);
            System.Drawing.SolidBrush FillBrushYellow = new System.Drawing.SolidBrush(Color.Yellow);
            Pen BlackPen = new Pen(Color.Black, 3);
            
            x_width_pic = Convert.ToDouble(_Bitmap.Width-5) / Convert.ToDouble(x_cnt);
            y_height_pic = Convert.ToDouble(_Bitmap.Height-5) / Convert.ToDouble(y_cnt);
       
            Rectangle rect;
            rect = new Rectangle(Convert.ToInt32(x_width_pic * XAreaIdx), Convert.ToInt32(y_height_pic * YAreaIdx), Convert.ToInt32(x_width_pic), Convert.ToInt32(y_height_pic));


            if (color_mode == 0) g.FillRectangle(FillBrushWhite, rect);
            else if (color_mode == 1) g.FillRectangle(FillBrushGreen, rect);
            else if (color_mode == 2) g.FillRectangle(FillBrushYellow, rect);

            g.DrawRectangle(BlackPen, rect);

            return DemoBitmap;

        }
    }
}
