﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Magistr
{
    class Engine
    {
        Moment matr1;
        double[] res1;
        double angle;
        double[] center;
        RichTextBox resultes;
        PictureBox picture1;
        PictureBox picture2;
        public Point checkPoint;
        public Point localPoint;
        private Point calculatedGlobalPointbyWh;
        Point lefttop;
        Box rect;
        double[] wh1;
        double[] widthHeight = new double[2];
        public Engine(RichTextBox rtb, PictureBox img, PictureBox img2)
        {
            this.resultes = rtb;
            this.picture1 = img;
            this.picture2 = img2;
        }
        public void CalculationLocalPointStart()
        {
            wh1 = rect.PercentPoint();
            localPoint =rect.CalculateLocalePoint(checkPoint, (float)angle);
            widthHeight[0] = localPoint.X / wh1[0];
            widthHeight[1] = localPoint.Y / wh1[1];
            resultes.Text += "Локальные координаты: " + localPoint.X + " : " + localPoint.Y + Environment.NewLine;
            resultes.Text += "Параметры объектной координатной плоскости: Длина=" + Math.Round(wh1[0],MidpointRounding.AwayFromZero) + " Ширина= " + Math.Round(wh1[1],MidpointRounding.AwayFromZero) + Environment.NewLine;
            resultes.Text += "Относительные координаты: " + localPoint.X +"/"+Math.Round(wh1[0],MidpointRounding.AwayFromZero)+ " : " + localPoint.Y +"/"+Math.Round(wh1[1],MidpointRounding.AwayFromZero)+ Environment.NewLine;
            resultes.Text += "Коэффициенты отношения: X:" + (localPoint.X/ Math.Round(wh1[0], MidpointRounding.AwayFromZero)) + " Y: " + (localPoint.Y/ Math.Round(wh1[1], MidpointRounding.AwayFromZero)) + Environment.NewLine;
        }
        public void CalculationGlobalPointStart()
        {
            double[] wh = rect.PercentPoint();
            calculatedGlobalPointbyWh = rect.CalculatePointForImageByWH(localPoint, wh, widthHeight, (float)angle);
            resultes.Text += "Найденные глобальные координаты через коэффициенты: " + calculatedGlobalPointbyWh.X + " : " + calculatedGlobalPointbyWh.Y + Environment.NewLine;
            Point i1, i2, i3, i4;
            i1 = new Point(calculatedGlobalPointbyWh.X + 10, calculatedGlobalPointbyWh.Y);
            i2 = new Point(calculatedGlobalPointbyWh.X - 10, calculatedGlobalPointbyWh.Y);
            i3 = new Point(calculatedGlobalPointbyWh.X, calculatedGlobalPointbyWh.Y + 10);
            i4 = new Point(calculatedGlobalPointbyWh.X, calculatedGlobalPointbyWh.Y - 10);
            Image itg = picture2.Image;
            using (Graphics g = Graphics.FromImage(itg))
            {
                g.DrawLine(new Pen(Color.Red, 3), i2, i1);
                g.DrawLine(new Pen(Color.Red, 3), i3, i4);
            }
            picture2.Image = itg;
            /*
            calculatedGlobalPoint = rect.CalculateGlobalPointForImage(localPoint, (float)angle);
            resultes.Text += "Найденные глобальные координаты: " + calculatedGlobalPoint.X + " : " + calculatedGlobalPoint.Y + Environment.NewLine;
            Point i1, i2, i3, i4;
            i1 = new Point(calculatedGlobalPoint.X + 10, calculatedGlobalPoint.Y);
            i2 = new Point(calculatedGlobalPoint.X - 10, calculatedGlobalPoint.Y);
            i3 = new Point(calculatedGlobalPoint.X, calculatedGlobalPoint.Y + 10);
            i4 = new Point(calculatedGlobalPoint.X, calculatedGlobalPoint.Y - 10);
            Image itg = picture2.Image;
            using (Graphics g = Graphics.FromImage(itg))
            {
                g.DrawLine(new Pen(Color.Red, 3), i2, i1);
                g.DrawLine(new Pen(Color.Red, 3), i3, i4);
            }
            picture2.Image = itg;
            */
        }
        public void CalculationSecStart()
        {
            Schet2();
        }
        public void CalculationStart()
        {
            Schet1();
        }
        private void Schet1()
        {
            Image img = picture1.Image;
            Bitmap image1 = new Bitmap(picture1.Image);
            matr1 = new Moment(image1);
            matr1.GetRes();
            center = matr1.center;
            res1 = matr1.cResult;
            angle = matr1.gradus;
            img = WriteToShow(img, (float)angle);
            GetReactangel(img, (float)angle,true);
        }
        private void Schet2()
        {
            Image img = picture2.Image;
            Bitmap image1 = new Bitmap(picture2.Image);
            matr1 = new Moment(image1);
            matr1.GetRes();
            center = matr1.center;
            res1 = matr1.cResult;
            angle = matr1.gradus;
            img = RotateImage(img, (float)angle, matr1.center);
            GetReactangel(img, (float)angle,false);
        }

        private Image WriteToShow(Image img, float angle)
        {
            resultes.Text+="Координаты центра: " + matr1.center[0].ToString() + " : " + matr1.center[1].ToString() + Environment.NewLine;
            resultes.Text += "M 1:1 = " + res1[0].ToString() + Environment.NewLine;
            resultes.Text += "M 2:0 = " + res1[1].ToString() + Environment.NewLine;
            resultes.Text += "M 0:2 = " + res1[2].ToString() + Environment.NewLine;
            resultes.Text += "Угол поворота: " + matr1.gradus.ToString() + Environment.NewLine;
            img = RotateImage(img, (float)angle,matr1.center);
            return img;
        }

        private Image RotateImage(
            Image pImage, float pAngle,double[] center)
        {
            Matrix lMatrix = new Matrix();
            lMatrix.RotateAt(pAngle, new Point((int)center[0], (int)center[1]));
            Bitmap lNewBitmap = new Bitmap(pImage.Width, pImage.Height);
            lNewBitmap.SetResolution(pImage.HorizontalResolution, pImage.VerticalResolution);
            Graphics lGraphics = Graphics.FromImage(lNewBitmap);
            lGraphics.Clear(Color.White);
            lGraphics.Transform = lMatrix;
            lGraphics.DrawImage(pImage, 0, 0);
            lGraphics.Dispose();
            lMatrix.Dispose();
            return lNewBitmap;
        }
        private void GetReactangel(Image img, Single angle,bool flag)
        {
            int minX, minY;
            int maxX, maxY;
            minX = minY = int.MaxValue;
            maxX = maxY = int.MinValue;
            Bitmap ig = new Bitmap(img);
            for (int x = 0; x < ig.Height; x++)
            {
                for (int y = 0; y < ig.Width; y++)
                {
                    if (ig.GetPixel(x, y).R != 255)
                    {
                        if (x > maxX) maxX = x;
                        if (x < minX) minX = x;
                        if (y > maxY) maxY = y;
                        if (y < minY) minY = y;
                    }
                }
            }
            Point leftbottom = new Point(minX, maxY), righttop = new Point(maxX, minY), rightbottom = new Point(maxX, maxY);
            lefttop = new Point(minX, minY);
            Point ct=new Point(Convert.ToInt32(center[0]), Convert.ToInt32(center[1]));
            rect = new Box(img, lefttop, leftbottom, righttop, rightbottom,ct);
            if(flag)
            rect.RotatePoint(angle,picture1);
            else rect.RotatePoint(angle, picture2);
            //picture1.Image=rect.img;
        }
    }
}
