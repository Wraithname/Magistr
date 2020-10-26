using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Magistr
{
    class Box
    {
        private Point[] points = new Point[4];
        private Point center;
        private Point lefttop;
        private Point leftbottom;
        private Point righttop;
        private Point rightbottom;
        public Image img;
        Bitmap lNewBitmap;
        public Box(Image img,Point lefttop, Point leftbottom, Point righttop, Point rightbottom,Point ct)
        {
            this.img = img;
            this.lefttop = lefttop;
            this.leftbottom = leftbottom;
            this.righttop = righttop;
            this.rightbottom = rightbottom;
            this.center = ct;
            this.lNewBitmap= new Bitmap(img);
            points[0]= lefttop;
            points[1] = leftbottom;
            points[2] = righttop;
            points[3] = rightbottom;
        }
        
        public void RotatePoint(Single angle,PictureBox pic)
        {
            double unangle = (-Math.PI* angle) / 180.0;
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Point((int)((points[i].X - center.X) * Math.Cos(unangle) - (points[i].Y - center.Y) * Math.Sin(unangle)+center.X),
                    (int)((points[i].X - center.X) * Math.Sin(unangle) + (points[i].Y - center.Y) * Math.Cos(unangle)+center.Y));
            }
            Image pirt = pic.Image;
            lefttop = points[0];
            leftbottom = points[1];
            righttop = points[2];
            rightbottom = points[3];
            using (Graphics g = Graphics.FromImage(pirt))
            {
                Pen red = new Pen(Color.Red, 2);
                Pen blue = new Pen(Color.Blue, 2);
                g.DrawLine(blue, leftbottom, lefttop);
                g.DrawLine(red, lefttop, righttop);
                g.DrawLine(red, righttop, rightbottom);
                g.DrawLine(blue, leftbottom, rightbottom);
            }
            pic.Image = pirt;
        }
    }
}
