using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
            _BackColor = pictureBox2.BackColor;
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
        }
        private Color _BackColor;
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
        private void UpdateZoomedImage(MouseEventArgs e)
        {
            int zoomWidth = pictureBox2.Width/2;
            int zoomHeight = pictureBox2.Height/2;
            int halfWidth = zoomWidth / 2;
            int halfHeight = zoomHeight / 2;
            Bitmap tempBitmap = new Bitmap(zoomWidth, zoomHeight, PixelFormat.Format24bppRgb);
            Graphics bmGraphics = Graphics.FromImage(tempBitmap);
            bmGraphics.Clear(_BackColor);
            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            bmGraphics.DrawImage(pictureBox2.Image,
                                 new Rectangle(0, 0, zoomWidth, zoomHeight),
                                 new Rectangle(e.X *3 -halfWidth, e.Y *3-halfHeight, zoomWidth, zoomHeight),
                                 GraphicsUnit.Pixel);
            pictureBox3.Image = tempBitmap;
            bmGraphics.DrawLine(Pens.Black, halfWidth + 1, halfHeight - 4, halfWidth + 1, halfHeight - 1);
            bmGraphics.DrawLine(Pens.Black, halfWidth + 1, halfHeight + 6, halfWidth + 1, halfHeight + 3);
            bmGraphics.DrawLine(Pens.Black, halfWidth - 4, halfHeight + 1, halfWidth - 1, halfHeight + 1);
            bmGraphics.DrawLine(Pens.Black, halfWidth + 6, halfHeight + 1, halfWidth + 3, halfHeight + 1);
            bmGraphics.Dispose();
            pictureBox3.Refresh();
        }
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBox2.Image == null)
                return;
            UpdateZoomedImage(e);
        }
    }
}
