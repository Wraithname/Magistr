using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Magistr
{
    class Engine
    {
        Moment matr1;
        Moment matr2;
        Moment matr3;
        double[] res1;
        double[] res2;
        double[] res3;
        double angle;
        string sipath;
        int[] thcol;
        string[] count;
        string[] fpath;
        double[] center;
        Stopwatch stopwatch = new Stopwatch();
        Stopwatch rec1 = new Stopwatch();
        Stopwatch rec2 = new Stopwatch();
        Stopwatch rec3 = new Stopwatch();

        public Engine( string sipath, int[] thcol, string[] count, string[] fpath)
        {
            this.sipath = sipath;
            this.thcol = thcol;
            this.count = count;
            this.fpath = fpath;
        }

        public void Start()
        {
            string[] lines = null;
            stopwatch.Start();
            var threads = new Thread[3];
            threads[0] = new Thread(Schet1);
            threads[1] = new Thread(Schet2);
            threads[2] = new Thread(Schet3);
            threads[0].Start();
            threads[1].Start();
            threads[2].Start();
            for (int i = 0; i < 3; i++)
            {
                threads[i].Join();
            }
            stopwatch.Stop();
            if (thcol[0] != 0)
            {
                lines = File.ReadAllLines(fpath[1]);
                File.AppendAllLines(fpath[0], lines);
                File.Delete(fpath[1]);
            }
            if (thcol[1] != 0)
            {
                lines = File.ReadAllLines(fpath[2]);
                File.AppendAllLines(fpath[0], lines);
                File.Delete(fpath[2]);
            }
            if (thcol[2] != 0)
            {
                lines = File.ReadAllLines(fpath[3]);
                File.AppendAllLines(fpath[0], lines);
                File.Delete(fpath[3]);
            }
            File.AppendAllText(fpath[0], "Время работы программы: " + (stopwatch.ElapsedMilliseconds / 1000) + " сек" + Environment.NewLine);
        }
        private void Schet1()
        {
            for (int i = 0; i < thcol[0]; i++)
            {
                rec1.Start();
                Image img = Image.FromFile(count[i]);
                Bitmap image1 = new Bitmap(img);
                matr1 = new Moment(image1);
                matr1.GetRes();
                center = matr1.center;
                res1 = matr1.cResult;
                angle = matr1.gradus;
                img = WriteToFile1(img, (float)angle, i);
                GetReactangel(img, (float)angle);
                rec1.Stop();
                File.AppendAllText(fpath[1], "Время обработки: " + (rec1.ElapsedMilliseconds / 1000) + " сек" + Environment.NewLine);
                File.AppendAllText(fpath[1], "---------------------------------------------------------" + Environment.NewLine);
                rec1.Reset();
            }
        }
        private void Schet2()
        {
            for (int i = thcol[0]; i < thcol[1]; i++)
            {
                rec2.Start();
                Image img = Image.FromFile(count[i]);
                Bitmap image2 = new Bitmap(img);
                matr2 = new Moment(image2);
                matr2.GetRes();
                res2 = matr2.cResult;
                angle = matr2.gradus;
                img = WriteToFile2(img, (float)angle, i);
                GetReactangel(img, (float)angle);
                rec2.Stop();
                File.AppendAllText(fpath[2], "Время обработки: " + (rec2.ElapsedMilliseconds / 1000) + " сек" + Environment.NewLine);
                File.AppendAllText(fpath[2], "---------------------------------------------------------" + Environment.NewLine);
                rec2.Reset();
            }
        }
        private void Schet3()
        {
            for (int i = thcol[1]; i < thcol[2]; i++)
            {
                rec3.Start();
                Image img = Image.FromFile(count[i]);
                Bitmap image3 = new Bitmap(img);
                matr3 = new Moment(image3);
                matr3.GetRes();
                res3 = matr3.cResult;
                angle = matr3.gradus;
                img = WriteToFile3(img, (float)angle, i);
                GetReactangel(img, (float)angle);
                rec3.Stop();
                File.AppendAllText(fpath[3], "Время обработки: " + (rec3.ElapsedMilliseconds / 1000) + " сек" + Environment.NewLine);
                File.AppendAllText(fpath[3], "---------------------------------------------------------" + Environment.NewLine);
                rec3.Reset();
            }
        }
        private Image WriteToFile1(Image img, Single angle, int num)
        {
            File.AppendAllText(fpath[1], "---------------------------------------------------------" + Environment.NewLine);
            File.AppendAllText(fpath[1], "Результат обработки изображения " + count[num].Split('\\').Last() + Environment.NewLine);
            File.AppendAllText(fpath[1], "Координаты центра: " + matr1.center[0].ToString() + " : " + matr1.center[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[1], "M 1:1 = " + res1[0].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[1], "M 2:0 = " + res1[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[1], "M 0:2 = " + res1[2].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[1], "Угол поворота: " + matr1.gradus.ToString() + Environment.NewLine);
            img = RotateImage(img, (float)angle);
            img.Save(sipath + "\\" + count[num].Split('\\').Last());
            return img;
        }
        private Image WriteToFile2(Image img, Single angle, int num)
        {
            File.AppendAllText(fpath[2], "---------------------------------------------------------" + Environment.NewLine);
            File.AppendAllText(fpath[2], "Результат обработки изображения " + count[num].Split('\\').Last() + Environment.NewLine);
            File.AppendAllText(fpath[2], "Координаты центра: " + matr2.center[0].ToString() + " : " + matr2.center[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[2], "M 1:1 = " + res2[0].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[2], "M 2:0 = " + res2[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[2], "M 0:2 = " + res2[2].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[2], "Угол поворота: " + matr2.gradus.ToString() + Environment.NewLine);
            img = RotateImage(img, (float)angle);
            img.Save(sipath + "\\" + count[num].Split('\\').Last());
            return img;
        }
        private Image WriteToFile3(Image img, Single angle, int num)
        {
            File.AppendAllText(fpath[3], "---------------------------------------------------------" + Environment.NewLine);
            File.AppendAllText(fpath[3], "Результат обработки изображения " + count[num].Split('\\').Last() + Environment.NewLine);
            File.AppendAllText(fpath[3], "Координаты центра: " + matr3.center[0].ToString() + " : " + matr3.center[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[3], "M 1:1 = " + res3[0].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[3], "M 2:0 = " + res3[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[3], "M 0:2 = " + res3[2].ToString() + Environment.NewLine);
            File.AppendAllText(fpath[3], "Угол поворота: " + matr3.gradus.ToString() + Environment.NewLine);
            img = RotateImage(img, (float)angle);
            img.Save(sipath + "\\" + count[num].Split('\\').Last());
            return img;
        }
        protected static Image RotateImage(Image pImage, Single pAngle)
        {

            Matrix lMatrix = new Matrix();
            lMatrix.RotateAt(pAngle, new PointF(pImage.Width / 2, pImage.Height / 2));
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
        private void GetReactangel(Image img, Single angle)
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
            Point lefttop=new Point(minX,minY), leftbottom=new Point(maxX,minY), righttop=new Point(minX,maxY), rightbottom=new Point(maxX,maxY);
            Box rect = new Box(img,lefttop, leftbottom, righttop, rightbottom);
            rect.RotatePoint((int) center[0], (int) center[1], angle);
        }
    }
}
