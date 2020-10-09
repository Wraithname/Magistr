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

        int[] thcol;
        int step;
        string[] count;
        string rpath;
        string sipath;
        string[] fpath;
        public Form1()
        {
            this.fpath = new string[4];
            this.thcol = new int[3];
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Engine runtet = new Engine(sipath, thcol, count, fpath);
            runtet.Start();
            MessageBox.Show("Обработка завершена. Программа закроется автоматически. Результаты обработки хранятся по пути: " + sipath, "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
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
            thcol[0] = step;
            thcol[1] = 2*step;
            thcol[2] = 3*step+steplast;
            button2.Enabled = false;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label6.Text = folderBrowserDialog1.SelectedPath;
                sipath = folderBrowserDialog1.SelectedPath;
                fpath[0] = folderBrowserDialog1.SelectedPath + "\\result.txt";
                if(thcol[0] != 0)
                    fpath[1] = folderBrowserDialog1.SelectedPath + "\\result1.txt";
                if (thcol[1] != 0)
                    fpath[2] = folderBrowserDialog1.SelectedPath + "\\result2.txt";
                if (thcol[2] != 0)
                    fpath[3] = folderBrowserDialog1.SelectedPath + "\\result3.txt";
            }
            button3.Enabled = false;
            button1.Enabled = true;
        }
        
    }
}
