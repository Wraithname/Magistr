using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magistr
{
    public partial class Form1 : Form
    {
        Point coordinateplace;
        Engine runtet;
        bool doClick;
        public Form1()
        {
            InitializeComponent();
            runtet = new Engine(richTextBox1, pictureBox1, pictureBox2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            runtet.CalculationLocalPointStart();
            calculation.Enabled = false;
            OpenImg2.Enabled = true;
        }

        private void OpenImg_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                { 
                    pictureBox1.Image = new Bitmap(open.FileName);
                    runtet.CalculationStart();
                }
            }
            pictureBox2.Image = null;
            calculation.Enabled = true;
            doClick = true;
        }

        private void OpenImg2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                { 
                    pictureBox2.Image = new Bitmap(open.FileName);
                    runtet.CalculationSecStart();
                    runtet.CalculationGlobalPointStart();
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (doClick)
            {
                //coordinateplace.X = (int)(((double)pictureBox1.Image.Width) / ((double)pictureBox1.Width) * (double)e.X);
                //coordinateplace.Y = (int)(((double)pictureBox1.Image.Height) / ((double)pictureBox1.Height) * (double)e.Y);
                coordinateplace.X = 478;
                coordinateplace.Y = 501;
                richTextBox1.Text += "Глобалные координаты точки " + coordinateplace.X + ":" + coordinateplace.Y + Environment.NewLine;
                runtet.checkPoint = coordinateplace;
                Point i1, i2, i3, i4;
                i1 = new Point(coordinateplace.X+10, coordinateplace.Y);
                i2 = new Point(coordinateplace.X - 10, coordinateplace.Y);
                i3 = new Point(coordinateplace.X , coordinateplace.Y + 10);
                i4 = new Point(coordinateplace.X, coordinateplace.Y - 10);
                Image itg = pictureBox1.Image;
                using (Graphics g=Graphics.FromImage(itg))
                {
                    g.DrawLine(new Pen(Color.Red, 3),i2, i1);
                    g.DrawLine(new Pen(Color.Red, 3), i3, i4);
                }
                pictureBox1.Image = itg;
                doClick = false;
            }
        }
    }
}
