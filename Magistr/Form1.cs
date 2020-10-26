using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magistr
{
    public partial class Form1 : Form
    {
        Point coordinateplace;
        Engine runtet;
        public Form1()
        {
            InitializeComponent();
            runtet = new Engine(richTextBox1, pictureBox1, pictureBox2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                    runtet.Start();
                }
            }
            button1.Enabled = true;
        }

        private void OpenImg2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                { 
                    pictureBox2.Image = new Bitmap(open.FileName);
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            coordinateplace.X = (int)(((double)pictureBox1.Image.Width) / ((double)pictureBox1.Width) * (double)e.X);
            coordinateplace.Y = (int)(((double)pictureBox1.Image.Height) / ((double)pictureBox1.Height) * (double)e.Y);
            richTextBox1.Text += "Глобалные координаты точки "+ coordinateplace.X + ":"+ coordinateplace.Y + Environment.NewLine;
        }
    }
}
