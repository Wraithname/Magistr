using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Magistr
{
    public partial class Form1 : Form
    {
        Moment matr;
        List<double> res;
        double angle;
        Bitmap image;
        string rpath = "C:\\3";
        string sipath = "C:\\r";
        int ri = 1;
        StreamWriter sw;
        Stopwatch stopwatch = new Stopwatch();
        Stopwatch rec = new Stopwatch();
        public Form1()
        {
            res = new List<double>();
            sw = new StreamWriter(sipath + "\\result.txt");
            InitializeComponent();
            notifyIcon1.Visible = false;
            this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
            this.Resize += new System.EventHandler(this.Form1_Resize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stopwatch.Start();
            button1.Enabled = false;
            for (int i = 1; i < 360; i++)
            {
                rec.Start();
                try
                {
                    image = new Bitmap(pictureBox1.Image);
                    matr = new Moment(image);
                    matr.GetRes();
                    res = matr.cResult;
                    angle = matr.gradus;
                    WriteToFile();
                    rec.Stop();
                    sw.WriteLine("Время обработки: " + (rec.ElapsedMilliseconds / 1000) + " сек");
                    rec.Reset();
                }
                catch {
                    rec.Stop();
                    rec.Reset();
                    sw.WriteLine("Изображение " + ri + ".png отсутствует");
                }
            }
            stopwatch.Stop();
            sw.WriteLine("Время работы программы: "+ (stopwatch.ElapsedMilliseconds/1000)+" сек");
            sw.WriteLine("Среднее время на обработку 1 изображения: "+ ((stopwatch.ElapsedMilliseconds / 1000)/360) + " сек");
            sw.Flush();
            sw.Close();
            MessageBox.Show("Обработка завершена. Программа закроется автоматически. Результаты обработки хранятся по пути: "+sipath, "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
            //showMatrix();
        }
        /*
        private void showMatrix()
        {
            txt1.Text = matr.center[0].ToString()+" : "+ matr.center[1].ToString();
            txt4.Text = res[0].ToString();
            txt5.Text = res[1].ToString();
            txt6.Text = res[2].ToString();
            textBox1.Text = matr.gradus.ToString();
        }
        */
        private void WriteToFile()
        {
            sw.WriteLine("Результат обработки изображения " + ri + ".png");
                sw.WriteLine("Координаты центра: "+ matr.center[0].ToString() + " : " + matr.center[1].ToString());
                sw.WriteLine("M 1:1 = " + res[0].ToString());
                sw.WriteLine("M 2:0 = " + res[1].ToString());
                sw.WriteLine("M 0:2 = " + res[2].ToString());
                sw.WriteLine("Угол поворота: " + matr.gradus.ToString());
            using (Bitmap clone = (Bitmap)pictureBox1.Image.Clone())
            {
                using (Graphics gfx = Graphics.FromImage(clone))
                {
                    gfx.Clear(Color.Transparent);
                    gfx.TranslateTransform(pictureBox1.Image.Width / 2, pictureBox1.Image.Height / 2);
                    gfx.RotateTransform((float)angle);
                    gfx.DrawImage(pictureBox1.Image, -pictureBox1.Image.Width / 2, -pictureBox1.Image.Height / 2);
                }
                clone.Save(sipath + "\\" + ri + ".png");
                ri++;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }
        /*
protected override void OnPaint(PaintEventArgs e)
{
using (Bitmap clone = (Bitmap)pictureBox1.Image.Clone())
{
using (Graphics gfx = Graphics.FromImage(clone))
{
gfx.Clear(Color.Transparent);
gfx.TranslateTransform(pictureBox1.Image.Width / 2, pictureBox1.Image.Height / 2);
gfx.RotateTransform((float)angle);
gfx.DrawImage(pictureBox1.Image, -pictureBox1.Image.Width / 2, -pictureBox1.Image.Height / 2);
}
pictureBox1.Image = (Bitmap)clone.Clone();
}
}
*/
    }
}
