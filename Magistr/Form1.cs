using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Magistr
{
    public partial class Form1 : Form
    {
        double[,] matrix;
        Moment matr;
        List<double> res;
        double angle;
        Bitmap image;
        public Form1()
        {
            matrix = new double[3, 3];
            res = new List<double>();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            image = new Bitmap(pictureBox1.Image);
            matr = new Moment(image);
            matr.GetRes();
            res = matr.cResult;
            angle = matr.gradus;
            showMatrix();
        }
        private void showMatrix()
        {
            txt1.Text = matr.center[0].ToString()+" : "+ matr.center[1].ToString();
            txt4.Text = res[0].ToString();
            txt5.Text = res[1].ToString();
            txt6.Text = res[2].ToString();
            textBox1.Text = matr.gradus.ToString();
        }
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
    }
}
