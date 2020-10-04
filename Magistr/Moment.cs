using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Magistr
{
    public class Moment
    {
        private Bitmap matrix1;
        private Bitmap matrix2;
        private Bitmap matrix3;
        private int maxpoint = 0;
        public double[] result=new double[6];
        public double[] cResult = new double[3];
        public double[] center;
        public double gradus;
        public Moment(Bitmap img)
        {
            this.matrix1 = img;
            this.matrix2 = img;
            this.matrix3 = img;
            this.center = new double[2];
        }
        private void CMoment()
        {
            double x = 0;
            double y = 0;
            double sp;
            for (int i = 0; i < matrix1.Height; i++)
            {
                for (int j = 0; j < matrix1.Width; j++)
                {
                    sp = 0.3 * matrix1.GetPixel(i, j).R + 0.59 * matrix1.GetPixel(i, j).G + 0.11 * matrix1.GetPixel(i, j).B;
                    if (sp != 255)
                        maxpoint++;
                }
            }
            for (int i = 0; i < matrix1.Height; i++)
            {
                for (int j = 0; j < matrix1.Width; j++)
                {
                    sp = 0.3 * matrix1.GetPixel(i, j).R + 0.59 * matrix1.GetPixel(i, j).G + 0.11 * matrix1.GetPixel(i, j).B;
                    if (sp != 255)
                        x+=i;
                }
            }
            x = x / maxpoint;
            for (int i = 0; i < matrix1.Height; i++)
            {
                for (int j = 0; j < matrix1.Width; j++)
                {
                    sp = 0.3 * matrix1.GetPixel(i, j).R + 0.59 * matrix1.GetPixel(i, j).G + 0.11 * matrix1.GetPixel(i, j).B;
                    if (sp != 255)
                        y+=j;
                }
            }
            y = y / maxpoint;
            x = Math.Round(x, mode: MidpointRounding.AwayFromZero);
            y = Math.Round(y, mode: MidpointRounding.AwayFromZero);
            center[0] = x;
            center[1] = y;
            double n1 = 0;
            for (int i = 0; i < matrix1.Height; i++)
            {
                for (int j = 0; j < matrix1.Width; j++)
                {
                    sp = 0.3 * matrix1.GetPixel(i, j).R + 0.59 * matrix1.GetPixel(i, j).G + 0.11 * matrix1.GetPixel(i, j).B;
                    if (sp != 255)
                        n1 += (i - x) * (j - y) * sp;
                }
            }
            cResult[0]=n1;
            double n3 = 0;
            for (int i = 0; i < matrix1.Height; i++)
            {
                for (int j = 0; j < matrix1.Width; j++)
                {
                    sp = 0.3 * matrix1.GetPixel(i, j).R + 0.59 * matrix1.GetPixel(i, j).G + 0.11 * matrix1.GetPixel(i, j).B;
                    if (sp != 255)
                        n3 += (i - x)* (i - x) * 1 * sp;
                }
            }
            cResult[1]=n3;
            double n2 = 0;
            for (int i = 0; i < matrix1.Height; i++)
            {
                for (int j = 0; j < matrix1.Width; j++)
                {
                    sp = 0.3 * matrix1.GetPixel(i, j).R + 0.59 * matrix1.GetPixel(i, j).G + 0.11 * matrix1.GetPixel(i, j).B;
                    if (sp != 255)
                        n2 += 1 * (j - y) * (j - y) * sp;
                }
            }
            cResult[2]=n2;
            GradusRes();
        }

        private void GradusRes()
        {
            gradus = Math.Round(0.5 * Math.Atan((2 * (cResult[0])) / (cResult[1] - cResult[2])) * -(180.0 / Math.PI), 0, mode: MidpointRounding.AwayFromZero);
        }
        public void GetRes()
        {
            Task<double> res0 = new Task<double>(UnM0);
            Task<double[]> res1 = new Task<double[]>(UnM1);
            Task<double[]> res2 = new Task<double[]>(UnM2);
            res0.RunSynchronously();
            res1.RunSynchronously();
            res2.RunSynchronously();
            res0.Wait();
            res1.Wait();
            res2.Wait();
            int j = 0;
            result[j]=res0.Result;
            j++;
            res0.Dispose();
            for (int i = 0; i < res1.Result.Length; i++)
            {
                result[j]=res1.Result[i];
                j++;
            }
            res1.Dispose();
            for (int i = 0; i < res2.Result.Length; i++)
            {
                result[j] = res2.Result[i];
                j++;
            }
            res2.Dispose();
            CMoment();
        }
        private double UnM0()
        {
            double h = 0;
            double sp;
            //Порядок 0
            for (int x = 0; x < matrix1.Height; x++)
            {
                for (int y = 0; y < matrix1.Width; y++)
                {
                    sp = 0.3 * matrix1.GetPixel(x, y).R + 0.59 * matrix1.GetPixel(x, y).G + 0.11 * matrix1.GetPixel(x, y).B;
                    if (sp != 255)
                        h += 1 * 1 * sp;
                }
            }
            return h;
        }
        private double[] UnM1()
        {
            double[] rec = new double[3];
            double h = 0;
            double sp;
            for (int x = 0; x < matrix2.Height; x++)
            {
                for (int y = 0; y < matrix2.Width; y++)
                {
                    sp = 0.3 * matrix1.GetPixel(x, y).R + 0.59 * matrix1.GetPixel(x, y).G + 0.11 * matrix1.GetPixel(x, y).B;
                    if (sp != 255)
                        h += 1 * y * sp;
                }
            }
            rec[0]=h;
            h = 0;
            for (int x = 0; x < matrix2.Height; x++)
            {
                for (int y = 0; y < matrix2.Width; y++)
                {
                    sp = 0.3 * matrix1.GetPixel(x, y).R + 0.59 * matrix1.GetPixel(x, y).G + 0.11 * matrix1.GetPixel(x, y).B;
                    if (sp != 255)
                        h += x * 1 * sp;
                }
            }
            rec[1]=h;
            h = 0;
            for (int x = 0; x < matrix2.Height; x++)
            {
                for (int y = 0; y < matrix2.Width; y++)
                {
                    sp = 0.3 * matrix1.GetPixel(x, y).R + 0.59 * matrix1.GetPixel(x, y).G + 0.11 * matrix1.GetPixel(x, y).B;
                    if (sp != 255)
                        h += x * y * sp;
                }
            }
            rec[2]=h;
            return rec;
        }
        private double[] UnM2()
        {
            double[] rec = new double[2];
            double h = 0;
            double sp;
            for (int x = 0; x < matrix3.Height; x++)
            {
                for (int y = 0; y < matrix3.Width; y++)
                {
                    sp = 0.3 * matrix1.GetPixel(x, y).R + 0.59 * matrix1.GetPixel(x, y).G + 0.11 * matrix1.GetPixel(x, y).B;
                    if (sp != 255)
                        h += 1 *y*y * sp;
                }
            }
            rec[0]=h;
            h = 0;
            for (int x = 0; x < matrix3.Height; x++)
            {
                for (int y = 0; y < matrix3.Width; y++)
                {
                    sp = 0.3 * matrix1.GetPixel(x, y).R + 0.59 * matrix1.GetPixel(x, y).G + 0.11 * matrix1.GetPixel(x, y).B;
                    if (sp != 255)
                        h += x*x * 1 * sp;
                }
            }
            rec[1]=h;
            return rec;
        }
    }
}
