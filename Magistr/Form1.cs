using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Magistr
{
    public partial class Form1 : Form
    {
        Moment matr1;
        Moment matr2;
        Moment matr3;
        double[] res1;
        double[] res2;
        double[] res3;
        int thcol1;
        int thcol2;
        int thcol3;
        string[] count;
        double angle;
        string rpath;
        string sipath;
        string fpath ;
        string fpath1 ;
        string fpath2 ;
        string fpath3 ;
        double[] center;
        int step;
        Stopwatch stopwatch = new Stopwatch();
        Stopwatch rec1 = new Stopwatch();
        Stopwatch rec2 = new Stopwatch();
        Stopwatch rec3 = new Stopwatch();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] lines = null;
            stopwatch.Start();
            button1.Enabled = false;
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
            if (thcol1 != 0)
            {
                lines = File.ReadAllLines(fpath1);
                File.AppendAllLines(fpath, lines);
                File.Delete(fpath1);
            }
            if (thcol2 != 0)
            {
                lines = File.ReadAllLines(fpath2);
                File.AppendAllLines(fpath, lines);
                File.Delete(fpath2);
            }
            if (thcol3 != 0)
            {
                lines = File.ReadAllLines(fpath3);
                File.AppendAllLines(fpath, lines);
                File.Delete(fpath3);
            }
            File.AppendAllText(fpath, "Время работы программы: " + (stopwatch.ElapsedMilliseconds / 1000) + " сек" + Environment.NewLine);
            MessageBox.Show("Обработка завершена. Программа закроется автоматически. Результаты обработки хранятся по пути: " + sipath, "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }
        private void Schet1()
        {
            for (int i = 0; i < thcol1; i++)
            {
                rec1.Start();
                    Image img = Image.FromFile(count[i]);
                    Invalidate();
                    Bitmap image1 = new Bitmap(img);
                    matr1 = new Moment(image1);
                    matr1.GetRes();
                    res1 = matr1.cResult;
                center = matr1.center;
                    angle = matr1.gradus;
                img = WriteToFile1(img, (float)angle, i);
                PaintReactangel(img, (float)angle);
                rec1.Stop();
                    File.AppendAllText(fpath1, "Время обработки: " + (rec1.ElapsedMilliseconds / 1000) + " сек" + Environment.NewLine);
                    File.AppendAllText(fpath1, "---------------------------------------------------------" + Environment.NewLine);
                    rec1.Reset();
            }
        }
        private void Schet2()
        {
            for (int i = thcol1; i < thcol2; i++)
            {
                rec2.Start();
                    Image img = Image.FromFile(count[i]);
                    Invalidate();
                    Bitmap image2 = new Bitmap(img);
                    matr2 = new Moment(image2);
                    matr2.GetRes();
                    res2 = matr2.cResult;
                    angle = matr2.gradus;
                img = WriteToFile2(img, (float)angle, i);
                PaintReactangel(img, (float)angle);
                rec2.Stop();
                    File.AppendAllText(fpath2, "Время обработки: " + (rec2.ElapsedMilliseconds / 1000) + " сек" + Environment.NewLine);
                    File.AppendAllText(fpath2, "---------------------------------------------------------" + Environment.NewLine);
                    rec2.Reset();
            }
        }
        private void Schet3()
        {
            for (int i = thcol2; i < thcol3; i++)
            {
                rec3.Start();
                    Image img = Image.FromFile(count[i]);
                    Invalidate();
                    Bitmap image3 = new Bitmap(img);
                    matr3 = new Moment(image3);
                    matr3.GetRes();
                    res3 = matr3.cResult;
                    angle = matr3.gradus;
                img = WriteToFile3(img,(float)angle,i);
                PaintReactangel(img, (float)angle);
                rec3.Stop();
                    File.AppendAllText(fpath3, "Время обработки: " + (rec3.ElapsedMilliseconds / 1000) + " сек" + Environment.NewLine);
                    File.AppendAllText(fpath3, "---------------------------------------------------------" + Environment.NewLine);
                    rec3.Reset();
            }
        }
        private Image WriteToFile1(Image img,Single angle,int num)
        {
            File.AppendAllText(fpath1, "---------------------------------------------------------" + Environment.NewLine);
            File.AppendAllText(fpath1, "Результат обработки изображения " + count[num].Split('\\').Last() + Environment.NewLine);
            File.AppendAllText(fpath1, "Координаты центра: " + matr1.center[0].ToString() + " : " + matr1.center[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath1, "M 1:1 = " + res1[0].ToString() + Environment.NewLine);
            File.AppendAllText(fpath1, "M 2:0 = " + res1[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath1, "M 0:2 = " + res1[2].ToString() + Environment.NewLine);
            File.AppendAllText(fpath1, "Угол поворота: " + matr1.gradus.ToString() + Environment.NewLine);
            img = RotateImage(img, (float)angle);
            img.Save(sipath + "\\" + count[num].Split('\\').Last());
            return img;
        }
        private Image WriteToFile2(Image img, Single angle, int num)
        {
            File.AppendAllText(fpath2, "---------------------------------------------------------" + Environment.NewLine);
            File.AppendAllText(fpath2, "Результат обработки изображения " + count[num].Split('\\').Last() + Environment.NewLine);
            File.AppendAllText(fpath2, "Координаты центра: " + matr2.center[0].ToString() + " : " + matr2.center[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath2, "M 1:1 = " + res2[0].ToString() + Environment.NewLine);
            File.AppendAllText(fpath2, "M 2:0 = " + res2[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath2, "M 0:2 = " + res2[2].ToString() + Environment.NewLine);
            File.AppendAllText(fpath2, "Угол поворота: " + matr2.gradus.ToString() + Environment.NewLine);
            img = RotateImage(img, (float)angle);
            img.Save(sipath + "\\" + count[num].Split('\\').Last());
            return img;
        }
        private Image WriteToFile3(Image img, Single angle, int num)
        {
            File.AppendAllText(fpath3, "---------------------------------------------------------" + Environment.NewLine);
            File.AppendAllText(fpath3, "Результат обработки изображения " + count[num].Split('\\').Last() + Environment.NewLine);
            File.AppendAllText(fpath3, "Координаты центра: " + matr3.center[0].ToString() + " : " + matr3.center[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath3, "M 1:1 = " + res3[0].ToString() + Environment.NewLine);
            File.AppendAllText(fpath3, "M 2:0 = " + res3[1].ToString() + Environment.NewLine);
            File.AppendAllText(fpath3, "M 0:2 = " + res3[2].ToString() + Environment.NewLine);
            File.AppendAllText(fpath3, "Угол поворота: " + matr3.gradus.ToString() + Environment.NewLine);
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
        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label5.Text = folderBrowserDialog1.SelectedPath;
                rpath= folderBrowserDialog1.SelectedPath;
            }
            count = Directory.GetFiles(rpath);
            string[] result = (from s in count
                               let dt = s.Split('\\').Last()
                               let dt2 = Int32.Parse(dt.Split('.').First())
                               orderby dt2
                               select s)
         .ToArray();
            count = result;
            int col = count.Length;
            int steplast= col % 3;
            step = col / 3;
            thcol1 = step;
            thcol2 = 2*step;
            thcol3 = 3*step+steplast;
            button2.Enabled = false;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label6.Text = folderBrowserDialog1.SelectedPath;
                sipath = folderBrowserDialog1.SelectedPath;
                fpath = folderBrowserDialog1.SelectedPath + "\\result.txt";
                if(thcol1!=0)
                fpath1 = folderBrowserDialog1.SelectedPath + "\\result1.txt";
                if (thcol2 != 0)
                    fpath2 = folderBrowserDialog1.SelectedPath + "\\result2.txt";
                if (thcol3 != 0)
                    fpath3 = folderBrowserDialog1.SelectedPath + "\\result3.txt";
            }
            button3.Enabled = false;
            button1.Enabled = true;
        }
        private void PaintReactangel(Image img, Single angle)
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
            using (Graphics g = Graphics.FromImage(img))
            {
                Pen red = new Pen(Color.Red,2);
                g.DrawRectangle(red, minX,minY,maxX-minX,maxY-minY);
            }
            img.Save("c:\\r\\test.png");
            angle *= -1;
            img = RotateImage(img, (float)angle);
            img.Save("c:\\r\\test1.png");
        }
    }
}
