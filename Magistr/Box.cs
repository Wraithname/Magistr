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
        private Point lefttopLocal;
        private Point leftbottomLocal;
        private Point righttopLocal;
        private Point rightbottomLocal;
        private Point lefttopNotRotate;
        public Image img;
        double[] percent = new double[2];
        public Box(Image img,Point lefttop, Point leftbottom, Point righttop, Point rightbottom,Point ct)
        {
            this.img = img;
            this.center = ct;
            this.lefttopNotRotate = lefttop;
            points[0]= lefttop;
            points[1] = leftbottom;
            points[2] = righttop;
            points[3] = rightbottom;
        }
        public Point CalculateGlobalPointForImage(Point localePoint,Single angle)
        {
            Point globalPoint;
            double unangle = (-Math.PI * angle) / 180.0;
            Point loc = new Point(localePoint.X + lefttopNotRotate.X, localePoint.Y + lefttopNotRotate.Y);
            globalPoint = new Point((int)((loc.X - center.X) * Math.Cos(unangle) - (loc.Y - center.Y) * Math.Sin(unangle) + center.X),
                    (int)((loc.X - center.X) * Math.Sin(unangle) + (loc.Y - center.Y) * Math.Cos(unangle) + center.Y));
            return globalPoint;
        }
        public Point CalculateLocalePoint(Point checkedPoint,Single angle)
        {
            Point localePoint;
            double unangle = (Math.PI * angle) / 180.0;
            Point loc= new Point((int)((checkedPoint.X - center.X) * Math.Cos(unangle) - (checkedPoint.Y - center.Y) * Math.Sin(unangle) + center.X),
                    (int)((checkedPoint.X - center.X) * Math.Sin(unangle) + (checkedPoint.Y - center.Y) * Math.Cos(unangle) + center.Y));
            localePoint = new Point(loc.X - lefttopNotRotate.X, loc.Y - lefttopNotRotate.Y);
            return localePoint;
        }
        public double[] PercentPoint()
        {
            double w=Math.Sqrt((lefttop.X - righttop.X) * (lefttop.X - righttop.X) + (lefttop.Y - righttop.Y) * (lefttop.Y - righttop.Y));
            double h= Math.Sqrt((leftbottom.X - lefttop.X) * (leftbottom.X - lefttop.X) + (leftbottom.Y - lefttop.Y) * (leftbottom.Y - lefttop.Y));
            percent[0] = w;
            percent[1] = h;
            return percent;
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
            PointF v = new Point()
            {
                X = righttop.X-lefttop.X,
                Y = righttop.Y -lefttop.Y
            };
            float offset = 30;
            Point p3 = TranslatePoint(righttop, offset, v);
            PointF v1 = new Point()
            {
                X = leftbottom.X-lefttop.X,
                Y = leftbottom.Y- lefttop.Y
            };
            Point p4 = TranslatePoint(leftbottom, offset, v1);
            using (Graphics g = Graphics.FromImage(pirt))
            {
                Pen red = new Pen(Color.Red, 2f);
                Pen blue = new Pen(Color.Blue, 2f);
                //Расчёт стрелок
                double x = p4.X - leftbottom.X;
                double y = p4.Y - leftbottom.Y;
                double d = Math.Sqrt(Math.Pow(p4.X - leftbottom.X, 2) + Math.Pow(p4.Y - leftbottom.Y, 2));
                double X4 = p4.X - (x / d)*7;
                double Y4 = p4.Y - (y / d)*7;
                double Xp = p4.Y - leftbottom.Y;
                double Yp = leftbottom.X - p4.X;
                // координаты перпендикуляров, удалённой от точки X4;Y4 на 10px в разные стороны
                double X5 = X4 + (Xp / d) * 10;
                double Y5 = Y4 + (Yp / d) * 10;
                double X6 = X4 - (Xp / d) * 10;
                double Y6 = Y4 - (Yp / d) * 10;
                //Расчёт стрелок
                double x1 = p3.X - righttop.X;
                double y1 = p3.Y - righttop.Y;
                double d1 = Math.Sqrt(Math.Pow(p3.X - righttop.X, 2) + Math.Pow(p3.Y - righttop.Y, 2));
                double X41 = p3.X - (x1 / d1) * 10;
                double Y41 = p3.Y - (y1 / d1) * 10;
                double Xp1 = p3.Y - righttop.Y;
                double Yp1 = righttop.X - p3.X;
                // координаты перпендикуляров, удалённой от точки X4;Y4 на 10px в разные стороны
                double X51 = X41 + (Xp1 / d) * 10;
                double Y51 = Y41 + (Yp1 / d) * 10;
                double X61 = X41 - (Xp1 / d) * 10;
                double Y61 = Y41 - (Yp1 / d) * 10;
                // построение линий
                g.DrawLine(red, righttop, rightbottom);
                g.DrawLine(red, leftbottom, rightbottom);
                g.DrawLine(blue, lefttop, p4);
                g.DrawLine(blue, lefttop, p3);
                g.DrawLine(blue, p4, new Point((int)X5, (int)Y5));
                g.DrawLine(blue, p4, new Point((int)X6, (int)Y6));
                g.DrawLine(blue, p3, new Point((int)X51, (int)Y51));
                g.DrawLine(blue, p3, new Point((int)X61, (int)Y61));
            }
            pic.Image = pirt;
        }
        static Point TranslatePoint(Point point, float offset, PointF vector)
        {
            float magnitude = (float)Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y)); // = length
            vector.X /= magnitude;
            vector.Y /= magnitude;
            PointF translation = new PointF()
            {
                X = offset * vector.X,
                Y = offset * vector.Y
            };
            using (Matrix m = new Matrix())
            {
                m.Translate(translation.X, translation.Y);
                Point[] pts = new Point[] { point };
                m.TransformPoints(pts);
                return pts[0];
            }
        }
    }
}
