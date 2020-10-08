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
        public double[,] matrix;
        public Moment(Bitmap img)
        {
            this.matrix1 = img;
            this.matrix2 = img;
            this.matrix3 = img;
            this.center = new double[2];
            this.matrix = new double[img.Height, img.Width];
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
                    if (matrix[i, j] != 255)
                        maxpoint++;
                }
            }
            for (int i = 0; i < matrix1.Height; i++)
            {
                for (int j = 0; j < matrix1.Width; j++)
                {
                    if (matrix[i, j] != 255)
                        x+=i;
                }
            }
            x = x / maxpoint;
            for (int i = 0; i < matrix1.Height; i++)
            {
                for (int j = 0; j < matrix1.Width; j++)
                {
                    if (matrix[i, j] != 255)
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
                    if (matrix[i, j] != 255)
                        n1 += (i - x) * (j - y) * matrix[i, j];
                }
            }
            cResult[0]=n1;
            double n3 = 0;
            for (int i = 0; i < matrix1.Height; i++)
            {
                for (int j = 0; j < matrix1.Width; j++)
                {
                    if (matrix[i, j] != 255)
                        n3 += (i - x)* (i - x) * 1 * matrix[i, j];
                }
            }
            cResult[1]=n3;
            double n2 = 0;
            for (int i = 0; i < matrix1.Height; i++)
            {
                for (int j = 0; j < matrix1.Width; j++)
                {
                    if (matrix[i, j] != 255)
                        n2 += 1 * (j - y) * (j - y) * matrix[i, j];
                }
            }
            cResult[2]=n2;
            GradusRes();
        }

        private void GradusRes()
        {
            gradus = Math.Round(0.5 * Math.Atan((2 * (cResult[0])) / (cResult[1] - cResult[2])) * -(180.0 / Math.PI), 0, mode: MidpointRounding.AwayFromZero);
            if (cResult[2] > cResult[1])
                gradus += 90;
        }
        public void GetRes()
        {
            double res0 = UnM0();
            double[] res1 = UnM1();
            double[] res2 = UnM2();
            int j = 0;
            result[j]=res0;
            j++;
            for (int i = 0; i < res1.Length; i++)
            {
                result[j]=res1[i];
                j++;
            }
            for (int i = 0; i < res2.Length; i++)
            {
                result[j] = res2[i];
                j++;
            }
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
                    matrix[x, y] = matrix1.GetPixel(x, y).R;
                    if (matrix[x, y] != 255)
                        h += 1 * 1 * matrix[x, y];
                }
            }
            return h;
        }
        private double[] UnM1()
        {
            double[] rec = new double[3];
            double h = 0;
            for (int x = 0; x < matrix2.Height; x++)
            {
                for (int y = 0; y < matrix2.Width; y++)
                {
                    if (matrix[x, y] != 255)
                        h += 1 * y * matrix[x, y];
                }
            }
            rec[0]=h;
            h = 0;
            for (int x = 0; x < matrix2.Height; x++)
            {
                for (int y = 0; y < matrix2.Width; y++)
                {   
                    if (matrix[x, y]  != 255)
                        h += x * 1 * matrix[x, y];
                }
            }
            rec[1]=h;
            h = 0;
            for (int x = 0; x < matrix2.Height; x++)
            {
                for (int y = 0; y < matrix2.Width; y++)
                {
                    if (matrix[x, y] != 255)
                        h += x * y * matrix[x, y];
                }
            }
            rec[2]=h;
            return rec;
        }
        private double[] UnM2()
        {
            double[] rec = new double[2];
            double h = 0;
            for (int x = 0; x < matrix3.Height; x++)
            {
                for (int y = 0; y < matrix3.Width; y++)
                {
                    if (matrix[x, y] != 255)
                        h += 1 *y*y * matrix[x, y];
                }
            }
            rec[0]=h;
            h = 0;
            for (int x = 0; x < matrix3.Height; x++)
            {
                for (int y = 0; y < matrix3.Width; y++)
                {
                    if (matrix[x, y] != 255)
                        h += x*x * 1 * matrix[x, y];
                }
            }
            rec[1]=h;
            return rec;
        }
    }
}
