using System;
using System.Drawing;
using System.Linq;

namespace Magistr
{
    class Box
    {
        private Point[] points = new Point[4];
        private string[] angels = new string[4];
        enum angelname{LeftTop,LeftBottom,RightTop,RightBottom};
        public Box(Image img,Point lefttop, Point leftbottom, Point righttop, Point rightbottom)
        {
            this.img = img;
            this.lefttop = lefttop;
            this.leftbottom = leftbottom;
            this.righttop = righttop;
            this.rightbottom = rightbottom;
            points[0]= lefttop;
            points[1] = leftbottom;
            points[2] = righttop;
            points[3] = rightbottom;
            angels[0] = angelname.LeftTop.ToString();
            angels[1] = angelname.LeftBottom.ToString();
            angels[2] = angelname.RightTop.ToString();
            angels[3] = angelname.RightBottom.ToString();
        }
        
        public void RotatePoint(int x0,int y0,Single angle)
        {
            double unangle = (Math.PI* angle*(-1)) / 180.0;
            for (int i = 0; i < points.Length; i++)
            {
                int x = (int)((points[i].X - x0) * Math.Cos(unangle) - (points[i].Y - y0) * Math.Sin(unangle) + x0);
                int y = (int)((points[i].X - x0) * Math.Sin(unangle) + (points[i].Y - y0) * Math.Cos(unangle) + y0);
                points[i] = new Point(x, y);
            }
            lefttop = points[0];
            leftbottom = points[1];
            righttop = points[2];
            rightbottom = points[3];
            PaintRotatedBox();
        }
        private void PaintRotatedBox()
        {
            int minX= points.Min(p => p.X), minY = points.Min(p => p.Y), maxX= points.Max(p => p.X), maxY= points.Max(p => p.Y);
            using (Graphics g = Graphics.FromImage(img))
            {
                Pen red = new Pen(Color.Red, 2);
                g.DrawRectangle(red, minX, minY, maxX - minX, maxY - minY);
            }
            SaveImage();
        }
        public void SaveImage()
        {
            img.Save("c:\\r\\test.png");
        }
        private Point lefttop { get; set; }
        private Point leftbottom { get; set; }
        private Point righttop { get; set; }
        private Point rightbottom { get; set; }
        private Image img { get; set; }
    }
}
