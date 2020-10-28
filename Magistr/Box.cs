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
        private Point lefttopNotRotate;
        private Point leftbottomNotRotate;
        private Point righttopNotRotate;
        private Point rightbottomNotRotate;
        public Image img;
        Bitmap lNewBitmap;
        public Box(Image img,Point lefttop, Point leftbottom, Point righttop, Point rightbottom,Point ct)
        {
            this.img = img;
            this.center = ct;
            this.lNewBitmap= new Bitmap(img);
            this.leftbottomNotRotate = leftbottom;
            this.lefttopNotRotate = lefttop;
            this.rightbottomNotRotate = rightbottom;
            this.righttopNotRotate = righttop;
            points[0]= lefttop;
            points[1] = leftbottom;
            points[2] = righttop;
            points[3] = rightbottom;
        }
        public Point CalculateLocalePoint(Point checkedPoint,Single angle)
        {
            Point localePoint,checkPointRotate,localePointRotated;
            checkPointRotate = new Point((int)((checkedPoint.X - center.X) * Math.Cos(angle) + (checkedPoint.Y - center.Y) * Math.Sin(angle) + center.X),
                    (int)((checkedPoint.X - center.X) * Math.Sin(angle) - (checkedPoint.Y - center.Y) * Math.Cos(angle) + center.Y));
            localePointRotated = new Point(checkPointRotate.X-lefttopNotRotate.X,checkPointRotate.Y-lefttopNotRotate.Y);
            double unangle = -angle;
            localePoint = new Point((int)((localePointRotated.X - center.X) * Math.Cos(unangle) - (localePointRotated.Y - center.Y) * Math.Sin(unangle) + center.X),
                    (int)((localePointRotated.X - center.X) * Math.Sin(unangle) + (localePointRotated.Y - center.Y) * Math.Cos(unangle) + center.Y));
            return localePoint;
        }
        public double[] PercentPoint()
        {
            double[] percent = new double[2];
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
            float offset = 25;
            Point p3 = TranslatePoint(righttop, offset, v);
            PointF v1 = new Point()
            {
                X = leftbottom.X-lefttop.X,
                Y = leftbottom.Y- lefttop.Y
            };
            Point p4 = TranslatePoint(leftbottom, offset, v1);
            using (Graphics g = Graphics.FromImage(pirt))
            {
                Pen red = new Pen(Color.Red, 2);
                Pen blue = new Pen(Color.Blue, 8);
                blue.EndCap= LineCap.ArrowAnchor;
                blue.StartCap = LineCap.Round;
                g.DrawLine(red, righttop, rightbottom);
                //g.DrawLine(blue, righttop, p3);
                //g.DrawLine(blue, righttop, p4);
                g.DrawLine(red, leftbottom, rightbottom);
                g.DrawLine(blue, lefttop, p4);
                g.DrawLine(blue, lefttop, p3);
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
