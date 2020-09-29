using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Magistr
{
    public class Moment
    {
        private Bitmap matrix1;
        private Bitmap matrix2;
        private Bitmap matrix3;
        public List<double> result;
        public List<double> cResult;
        public double[] center;
        public double gradus;
        public Moment(Bitmap img)
        {
            this.matrix1 = img;
            this.matrix2 = img;
            this.matrix3 = img;
            this.result = new List<double>();
            this.cResult = new List<double>();
            this.center = new double[2];
        }
        private void CMoment()
        {
            double x = Math.Round(result[2] / result[0], mode: MidpointRounding.AwayFromZero);
            double y = Math.Round(result[1] / result[0], mode: MidpointRounding.AwayFromZero);
            center[0] = x;
            center[1] = y;
            double n1 = result[3]/result[0]-x*y;
            cResult.Add(n1);
            double n2 = result[4] / result[0] - y * y;
            cResult.Add(n2);
            double n3 =result[5] / result[0] - x * x;
            cResult.Add(n3);
            GradusRes();
        }
       
        private void GradusRes()
        {
            gradus = Math.Round(0.5 * Math.Atan((2 * (cResult[0])) / (cResult[2] - cResult[1]))*(180.0/Math.PI),1,mode: MidpointRounding.AwayFromZero);
        }
        public void GetRes()
        {
            Task<double> res0 = new Task<double>(UnM0);
            Task<List<double>> res1 = new Task<List<double>>(UnM1);
            Task<List<double>> res2 = new Task<List<double>>(UnM2);
            res0.RunSynchronously();
            res1.RunSynchronously();
            res2.RunSynchronously();
            res0.Wait(); 
            res1.Wait();
            res2.Wait();
            result.Add(res0.Result);
            res0.Dispose();
            for (int i = 0; i < res1.Result.Count; i++)
                result.Add(res1.Result[i]);
            res1.Dispose();
            for (int i = 0; i < res2.Result.Count; i++)
                result.Add(res2.Result[i]);
            res2.Dispose();
            CMoment();
        }
        private double UnM0()
        {
            double h = 0;
            //Порядок 0
            for (int x = 0; x < matrix1.Height; x++)
            {
                for (int y = 0; y < matrix1.Width; y++)
                {
                    if (matrix1.GetPixel(x, y).R != 255 && matrix1.GetPixel(x, y).G != 255 && matrix1.GetPixel(x, y).B != 255)
                        h += Math.Pow(x, 0) * Math.Pow(y, 0) * ((0.3 * matrix1.GetPixel(x, y).R + 0.59 * matrix1.GetPixel(x, y).G + 0.11 * matrix1.GetPixel(x, y).B));
                }
            }
            return h;
        }
        private List<double> UnM1()
        {
            List<double> rec = new List<double>();
            double h = 0;
            for (int x = 0; x < matrix2.Height; x++)
            {
                for (int y = 0; y < matrix2.Width; y++)
                {
                    if (matrix2.GetPixel(x, y).R != 255 && matrix2.GetPixel(x, y).G != 255 && matrix2.GetPixel(x, y).B != 255)
                        h += Math.Pow(x, 0) * Math.Pow(y, 1) * ((0.3 * matrix2.GetPixel(x, y).R + 0.59 * matrix2.GetPixel(x, y).G + 0.11 * matrix2.GetPixel(x, y).B));
                }
            }
            rec.Add(h);
            h = 0;
            for (int x = 0; x < matrix2.Height; x++)
            {
                for (int y = 0; y < matrix2.Width; y++)
                {
                    if (matrix2.GetPixel(x, y).R != 255 && matrix2.GetPixel(x, y).G != 255 && matrix2.GetPixel(x, y).B != 255)
                        h += Math.Pow(x, 1) * Math.Pow(y, 0) * ((0.3 * matrix2.GetPixel(x, y).R + 0.59 * matrix2.GetPixel(x, y).G + 0.11 * matrix2.GetPixel(x, y).B));
                }
            }
            rec.Add(h);
            h = 0;
            for (int x = 0; x < matrix2.Height; x++)
            {
                for (int y = 0; y < matrix2.Width; y++)
                {
                    if (matrix2.GetPixel(x, y).R != 255 && matrix2.GetPixel(x, y).G != 255 && matrix2.GetPixel(x, y).B != 255)
                        h += Math.Pow(x, 1) * Math.Pow(y, 1) * ((0.3 * matrix2.GetPixel(x, y).R + 0.59 * matrix2.GetPixel(x, y).G + 0.11 * matrix2.GetPixel(x, y).B));
                }
            }
            rec.Add(h);
            return rec;
        }
        private List<double> UnM2()
        {
            List<double> rec = new List<double>();
            double h = 0;
            for (int x = 0; x < matrix3.Height; x++)
            {
                for (int y = 0; y < matrix3.Width; y++)
                {
                    if (matrix3.GetPixel(x, y).R != 255 && matrix3.GetPixel(x, y).G != 255 && matrix3.GetPixel(x, y).B != 255)
                        h += Math.Pow(x, 0) * Math.Pow(y, 2) * ((0.3 * matrix3.GetPixel(x, y).R + 0.59 * matrix3.GetPixel(x, y).G + 0.11 * matrix3.GetPixel(x, y).B));
                }
            }
            rec.Add(h);
            h = 0;
            for (int x = 0; x < matrix3.Height; x++)
            {
                for (int y = 0; y < matrix3.Width; y++)
                {
                    if (matrix3.GetPixel(x, y).R != 255 && matrix3.GetPixel(x, y).G != 255 && matrix3.GetPixel(x, y).B != 255)
                        h += Math.Pow(x, 2) * Math.Pow(y, 0) * ((0.3 * matrix3.GetPixel(x, y).R + 0.59 * matrix3.GetPixel(x, y).G + 0.11 * matrix3.GetPixel(x, y).B));
                }
            }
            rec.Add(h);
            return rec;
        }
    }
}
