using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Mainpaint
{

    namespace Diagnostics
    {
        public enum EdgeFilterMode
        {
            EdgeDetectMono, EdgeDetectionGradient, Sharpen, SharpenGradient
        }

        public enum DeriviationLevel
        {
            First = 1, Second = 2
        }

        public class EdgeDetection
        {
            public static Bitmap GradientEdgeDetection(Bitmap original, EdgeFilterMode filter, DeriviationLevel level, float[] rgbFactors, byte threshold)
            {
                BitmapData bmpd = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelBuffer = new byte[bmpd.Stride * original.Height];
                byte[] buffer = new byte[bmpd.Stride * original.Height];

                Marshal.Copy(bmpd.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                original.UnlockBits(bmpd);

                int rG = 0, gG = 0, bG = 0, der = (int)level, byteOffset = 0;
                double r = 0, g = 0, b = 0;
                bool IsOff = false;

                for (int y = 1; y < original.Height - 1; y++)
                {
                    for (int x = 1; x < original.Width - 1; x++)
                    {
                        byteOffset = y * bmpd.Stride + x * 4;

                        bG = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]) / der;
                        bG += Math.Abs(pixelBuffer[byteOffset - bmpd.Stride] - pixelBuffer[byteOffset + bmpd.Stride]) / der;
                        byteOffset++;

                        gG = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]) / der;
                        gG += Math.Abs(pixelBuffer[byteOffset - bmpd.Stride] - pixelBuffer[byteOffset + bmpd.Stride]) / der;
                        byteOffset++;

                        rG = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]) / der;
                        rG += Math.Abs(pixelBuffer[byteOffset - bmpd.Stride] - pixelBuffer[byteOffset + bmpd.Stride]) / der;

                        if (bG + gG + rG > threshold)
                        {
                            IsOff = true;
                        }
                        else
                        {
                            byteOffset -= 2;

                            bG = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                            byteOffset++;

                            gG = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                            byteOffset++;

                            rG = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);

                            if (bG + gG + rG > threshold)
                            {
                                IsOff = true;
                            }
                            else
                            {
                                byteOffset -= 2;

                                bG = Math.Abs(pixelBuffer[byteOffset - bmpd.Stride] - pixelBuffer[byteOffset + bmpd.Stride]);
                                byteOffset++;

                                gG = Math.Abs(pixelBuffer[byteOffset - bmpd.Stride] - pixelBuffer[byteOffset + bmpd.Stride]);
                                byteOffset++;

                                rG = Math.Abs(pixelBuffer[byteOffset - bmpd.Stride] - pixelBuffer[byteOffset + bmpd.Stride]);

                                if (bG + gG + rG > threshold)
                                {
                                    IsOff = true;
                                }
                                else
                                {
                                    byteOffset -= 2;

                                    bG = Math.Abs(pixelBuffer[byteOffset - 4 - bmpd.Stride] - pixelBuffer[byteOffset + 4 + bmpd.Stride]) / der;
                                    bG += Math.Abs(pixelBuffer[byteOffset - bmpd.Stride + 4] - pixelBuffer[byteOffset + bmpd.Stride - 4]) / der;
                                    byteOffset++;

                                    gG = Math.Abs(pixelBuffer[byteOffset - 4 - bmpd.Stride] - pixelBuffer[byteOffset + 4 + bmpd.Stride]) / der;
                                    gG += Math.Abs(pixelBuffer[byteOffset - bmpd.Stride + 4] - pixelBuffer[byteOffset + bmpd.Stride - 4]) / der;
                                    byteOffset++;

                                    rG = Math.Abs(pixelBuffer[byteOffset - 4 - bmpd.Stride] - pixelBuffer[byteOffset + 4 + bmpd.Stride]) / der;
                                    rG += Math.Abs(pixelBuffer[byteOffset - bmpd.Stride + 4] - pixelBuffer[byteOffset + bmpd.Stride - 4]) / der;

                                    if (bG + gG + rG > threshold)
                                    {
                                        IsOff = true;
                                    }
                                    else
                                    {
                                        IsOff = false;
                                    }
                                }
                            }
                        }

                        byteOffset -= 2;

                        if (IsOff)
                        {
                            if (filter == EdgeFilterMode.EdgeDetectMono)
                            {
                                b = g = r = 255;
                            }
                            else if (filter == EdgeFilterMode.EdgeDetectionGradient)
                            {
                                b = bG * rgbFactors[2];
                                g = gG * rgbFactors[1];
                                r = rG * rgbFactors[0];
                            }
                            else if (filter == EdgeFilterMode.Sharpen)
                            {
                                b = pixelBuffer[byteOffset] * rgbFactors[2];
                                g = pixelBuffer[byteOffset + 1] * rgbFactors[1];
                                r = pixelBuffer[byteOffset + 2] * rgbFactors[0];
                            }
                            else if (filter == EdgeFilterMode.SharpenGradient)
                            {
                                b = pixelBuffer[byteOffset] + bG * rgbFactors[2];
                                g = pixelBuffer[byteOffset + 1] + gG * rgbFactors[1];
                                r = pixelBuffer[byteOffset + 2] + rG * rgbFactors[0];
                            }
                        }
                        else
                        {
                            if (filter == EdgeFilterMode.EdgeDetectMono || filter == EdgeFilterMode.EdgeDetectionGradient)
                            {
                                b = g = r = 0;
                            }
                            else if (filter == EdgeFilterMode.Sharpen || filter == EdgeFilterMode.SharpenGradient)
                            {
                                b = pixelBuffer[byteOffset];
                                g = pixelBuffer[byteOffset + 1];
                                r = pixelBuffer[byteOffset + 2];
                            }
                        }

                        b = (b > 255) ? 255 : (b < 0 ? 0 : b);
                        g = (g > 255) ? 255 : (g < 0 ? 0 : g);
                        r = (r > 255) ? 255 : (r < 0 ? 0 : r);

                        buffer[byteOffset] = (byte)b;
                        buffer[byteOffset + 1] = (byte)g;
                        buffer[byteOffset + 2] = (byte)r;
                        buffer[byteOffset + 3] = 255;
                    }
                }

                Bitmap ret = new Bitmap(original.Width, original.Height);
                BitmapData rbmpd = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(buffer, 0, rbmpd.Scan0, buffer.Length);
                ret.UnlockBits(rbmpd);
                return ret;
            }
        }
    }

    namespace Blur
    {
        public class Gaussian
        {
            public static double[,] _1DKernel(double dev, int size)
            {
                double[,] ret = new double[size, 1];

                double sum = 0;
                int pol = size / 2;
                for (int i = 0; i < size; i++)
                {
                    ret[i, 0] = 1 / (Math.Sqrt(2 * Math.PI) * dev) * Math.Exp(-(i - pol) * (i - pol) / (2 * dev * dev));
                    sum += ret[i, 0];
                }
                return ret;
            }

            public static double[,] _1DKernel(double dev)
            {
                int size = (int)Math.Ceiling(dev * 3) * 2 + 1;
                return _1DKernel(dev, size);
            }

            public static double[,] NormMatrix(double[,] matrix)
            {
                double[,] ret = new double[matrix.GetLength(0), matrix.GetLength(1)];

                double sum = 0;
                for (int x = 0; x < ret.GetLength(0); x++)
                {
                    for (int y = 0; y < ret.GetLength(1); y++)
                    {
                        sum += matrix[x, y];
                    }
                }
                if (sum != 0)
                {
                    for (int x = 0; x < ret.GetLength(0); x++)
                    {
                        for (int y = 0; y < ret.GetLength(1); y++)
                        {
                            ret[x, y] = matrix[x, y] / sum;
                        }
                    }
                }
                return ret;
            }

            public static double[,] _1DNormalizedMatrix(double dev)
            {
                return NormMatrix(_1DKernel(dev));
            }

            public static double PProcessor(double[,] matrix, int x, int y, double[,] kernel, int dir)
            {
                double res = 0;
                double pol = kernel.GetLength(0) / 2;
                for (int z = 0; z < kernel.GetLength(0); z++)
                {
                    int cx = (dir == 0) ? (int)(x + z - pol) : x;
                    int cy = (dir == 1) ? (int)(y + z - pol) : y;
                    if (cx >= 0 && cx < matrix.GetLength(0) && cy >= 0 && cy < matrix.GetLength(1))
                    {
                        res += matrix[cx, cy] * kernel[z, 0];
                    }
                }
                return res;
            }

            public static double[,] GConvolution(double[,] matrix, double dev)
            {
                double[,] kernel = _1DNormalizedMatrix(dev);
                double[,] res1 = new double[matrix.GetLength(0), matrix.GetLength(1)];
                double[,] res2 = new double[matrix.GetLength(0), matrix.GetLength(1)];

                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    for (int y = 0; y < matrix.GetLength(1); y++)
                    {
                        res1[x, y] = PProcessor(matrix, x, y, kernel, 0);
                    }
                }

                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    for (int y = 0; y < matrix.GetLength(1); y++)
                    {
                        res2[x, y] = PProcessor(res1, x, y, kernel, 1);
                    }
                }
                return res2;
            }

            public Bitmap ProcessImage(double d, Bitmap original)
            {
                Bitmap ret = new Bitmap(original.Width, original.Height);
                double[,] matrixa = new double[original.Width, original.Height];
                double[,] matrixr = new double[original.Width, original.Height];
                double[,] matrixg = new double[original.Width, original.Height];
                double[,] matrixb = new double[original.Width, original.Height];
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        matrixa[x, y] = original.GetPixel(x, y).A;
                        matrixr[x, y] = original.GetPixel(x, y).R;
                        matrixg[x, y] = original.GetPixel(x, y).G;
                        matrixb[x, y] = original.GetPixel(x, y).B;
                    }
                }
                matrixa = Gaussian.GConvolution(matrixa, d);
                matrixr = Gaussian.GConvolution(matrixr, d);
                matrixg = Gaussian.GConvolution(matrixg, d);
                matrixb = Gaussian.GConvolution(matrixb, d);
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        int vala = (int)Math.Min(255, matrixa[x, y]);
                        int valr = (int)Math.Min(255, matrixr[x, y]);
                        int valg = (int)Math.Min(255, matrixg[x, y]);
                        int valb = (int)Math.Min(255, matrixb[x, y]);
                        ret.SetPixel(x, y, Color.FromArgb(vala, valr, valg, valb));
                    }
                }
                return ret;
            }

            public Bitmap ProcessImage(double d, Bitmap original, Rectangle rect)
            {
                Bitmap ret = new Bitmap(rect.Width, rect.Height);
                ret.MakeTransparent();
                double[,] matrixa = new double[rect.Width, rect.Height];
                double[,] matrixr = new double[rect.Width, rect.Height];
                double[,] matrixg = new double[rect.Width, rect.Height];
                double[,] matrixb = new double[rect.Width, rect.Height];
                for (int x = 0; x < rect.Width; x++)
                {
                    for (int y = 0; y < rect.Height; y++)
                    {
                        matrixa[x, y] = Convert.ToInt16(original.GetPixel(rect.X + x, rect.Y + y).A);
                        matrixr[x, y] = original.GetPixel(rect.X + x, rect.Y + y).R;
                        matrixg[x, y] = original.GetPixel(rect.X + x, rect.Y + y).G;
                        matrixb[x, y] = original.GetPixel(rect.X + x, rect.Y + y).B;
                    }
                }
                matrixr = Gaussian.GConvolution(matrixr, d);
                matrixg = Gaussian.GConvolution(matrixg, d);
                matrixb = Gaussian.GConvolution(matrixb, d);

                for (int x = 0; x < rect.Width; x++)
                {
                    for (int y = 0; y < rect.Height; y++)
                    {
                        int vala = (int)matrixa[x, y];
                        int valr = (int)Math.Min(255, matrixr[x, y]);
                        int valg = (int)Math.Min(255, matrixg[x, y]);
                        int valb = (int)Math.Min(255, matrixb[x, y]);
                        ret.SetPixel(x, y, Color.FromArgb(vala, valr, valg, valb));
                    }
                }
                return ret;
            }
        }
    }

    class Effects
    {
        public enum Morphology
        {
            Erode, Dilate
        }

        public enum ColorFilterType
        {
            Red, Green, Blue
        }

        public class ColorEffects
        {
            private static int Clamp(int x, int min, int max)
            {
                if (x < min)
                    x = min;
                else if (x > max)
                    x = max;

                return x;
            }

            private static Color Gray(Color cr)
            {
                return Color.FromArgb(cr.A, (int)(cr.R * 0.3 + cr.G * 0.59 + cr.B * 0.11), (int)(cr.R * 0.3 + cr.G * 0.59 + cr.B * 0.11), (int)(cr.R * 0.3 + cr.G * 0.59 + cr.B * 0.11));
            }

            public static Bitmap Grayscale(Bitmap original)
            {
                Bitmap retBmp = new Bitmap(original.Width, original.Height);
                retBmp.MakeTransparent();
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        retBmp.SetPixel(x, y, Gray(original.GetPixel(x, y)));
                    }
                }
                return retBmp;
            }

            public static Bitmap SepiaImage(Bitmap original)
            {
                Bitmap ret_bmp = (Bitmap)original.Clone();
                BitmapData bmpd = ret_bmp.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                IntPtr ptr = bmpd.Scan0;
                byte[] buffer = new byte[bmpd.Stride * ret_bmp.Height];
                Marshal.Copy(ptr, buffer, 0, buffer.Length);

                byte max = 255;
                float r = 0, g = 0, b = 0;
                for (int x = 0; x < buffer.Length; x += 4)
                {
                    r = buffer[x] * 0.189f + buffer[x + 1] * 0.769f + buffer[x + 2] * 0.393f;
                    g = buffer[x] * 0.168f + buffer[x + 1] * 0.686f + buffer[x + 2] * 0.349f;
                    b = buffer[x] * 0.131f + buffer[x + 1] * 0.534f + buffer[x + 2] * 0.272f;

                    buffer[x + 2] = (r > max ? max : (byte)r);
                    buffer[x + 1] = (g > max ? max : (byte)g);
                    buffer[x] = (b > max ? max : (byte)b);
                }

                Marshal.Copy(buffer, 0, ptr, buffer.Length);
                ret_bmp.UnlockBits(bmpd);
                bmpd = null;
                buffer = null;
                return ret_bmp;
            }

            public static Bitmap Negative(Bitmap original)
            {
                Bitmap ret_bmp = (Bitmap)original.Clone();
                BitmapData bmpd = ret_bmp.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                IntPtr ptr = bmpd.Scan0;
                byte[] buffer = new byte[bmpd.Stride * ret_bmp.Height];
                Marshal.Copy(ptr, buffer, 0, buffer.Length);
                byte[] pixelBuffer = null;

                int pixel = 0;

                for (int x = 0; x < buffer.Length; x += 4)
                {
                    pixel = ~BitConverter.ToInt32(buffer, x);
                    pixelBuffer = BitConverter.GetBytes(pixel);

                    buffer[x] = pixelBuffer[0];
                    buffer[x + 1] = pixelBuffer[1];
                    buffer[x + 2] = pixelBuffer[2];
                }

                Marshal.Copy(buffer, 0, ptr, buffer.Length);
                ret_bmp.UnlockBits(bmpd);
                bmpd = null;
                buffer = null;
                return ret_bmp;
            }

            public static Bitmap ColorBalance(Bitmap original, float[] rgb)
            {
                BitmapData bmpd = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelbuffer = new byte[bmpd.Stride * original.Height];

                Marshal.Copy(bmpd.Scan0, pixelbuffer, 0, pixelbuffer.Length);
                original.UnlockBits(bmpd);

                float r = 0, g = 0, b = 0;
                float rl = rgb[0], gl = rgb[1], bl = rgb[2];

                for (int x = 0; x + 4 < pixelbuffer.Length; x += 4)
                {
                    b = 255.0f / bl * (float)pixelbuffer[x];
                    g = 255.0f / gl * (float)pixelbuffer[x + 1];
                    r = 255.0f / rl * (float)pixelbuffer[x + 2];

                    if (b > 255) { b = 255; }
                    else if (b < 0) { b = 0; }
                    if (g > 255) { g = 255; }
                    else if (g < 0) { g = 0; }
                    if (r > 255) { r = 255; }
                    else if (r < 0) { r = 0; }

                    pixelbuffer[x] = (byte)b;
                    pixelbuffer[x + 1] = (byte)g;
                    pixelbuffer[x + 2] = (byte)r;
                }

                Bitmap ret = new Bitmap(original.Width, original.Height);

                BitmapData rbmpd = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(pixelbuffer, 0, rbmpd.Scan0, pixelbuffer.Length);

                ret.UnlockBits(rbmpd);
                return ret;
            }

            public static Bitmap ErodeDilateFilter(Bitmap original, int matrixSize, bool[] appliedColors, Morphology morph)
            {
                BitmapData bmpd = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelBuffer = new byte[bmpd.Stride * original.Height];
                byte[] buffer = new byte[bmpd.Stride * original.Height];

                Marshal.Copy(bmpd.Scan0, pixelBuffer, 0, pixelBuffer.Length);

                original.UnlockBits(bmpd);

                int filterOffset = (matrixSize - 1) / 2;
                int calcOffset = 0, byteOffset = 0;
                byte r = 0, g = 0, b = 0, morphReset = 0;

                if (morph == Morphology.Erode)
                {
                    morphReset = 255;
                }

                for (int y = filterOffset; y < original.Height - filterOffset; y++)
                {
                    for (int x = filterOffset; x < original.Width - filterOffset; x++)
                    {
                        byteOffset = y * bmpd.Stride + x * 4;
                        b = morphReset;
                        g = morphReset;
                        r = morphReset;
                        if (morph == Morphology.Dilate)
                        {
                            for (int fy = -filterOffset; fy <= filterOffset; fy++)
                            {
                                for (int fx = -filterOffset; fx < filterOffset; fx++)
                                {
                                    calcOffset = byteOffset + (fx * 4) + (fy * bmpd.Stride);

                                    if (pixelBuffer[calcOffset] > b)
                                    {
                                        b = pixelBuffer[calcOffset];
                                    }
                                    if (pixelBuffer[calcOffset + 1] > g)
                                    {
                                        g = pixelBuffer[calcOffset + 1];
                                    }
                                    if (pixelBuffer[calcOffset + 2] > r)
                                    {
                                        r = pixelBuffer[calcOffset + 2];
                                    }
                                }
                            }
                        }
                        else if (morph == Morphology.Erode)
                        {
                            for (int fy = -filterOffset; fy <= filterOffset; fy++)
                            {
                                for (int fx = -filterOffset; fx < filterOffset; fx++)
                                {
                                    calcOffset = byteOffset + (fx * 4) + (fy * bmpd.Stride);

                                    if (pixelBuffer[calcOffset] < b)
                                    {
                                        b = pixelBuffer[calcOffset];
                                    }
                                    if (pixelBuffer[calcOffset + 1] < g)
                                    {
                                        g = pixelBuffer[calcOffset + 1];
                                    }
                                    if (pixelBuffer[calcOffset + 2] < r)
                                    {
                                        r = pixelBuffer[calcOffset + 2];
                                    }
                                }
                            }
                        }

                        if (!appliedColors[0])
                        {
                            r = pixelBuffer[byteOffset + 2];
                        }
                        if (!appliedColors[1])
                        {
                            g = pixelBuffer[byteOffset + 1];
                        }
                        if (!appliedColors[2])
                        {
                            b = pixelBuffer[byteOffset];
                        }


                        buffer[byteOffset] = b;
                        buffer[byteOffset + 1] = g;
                        buffer[byteOffset + 2] = r;
                        buffer[byteOffset + 3] = 255;
                    }
                }

                Bitmap ret = new Bitmap(original.Width, original.Height);
                BitmapData rbmpd = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(buffer, 0, rbmpd.Scan0, buffer.Length);
                ret.UnlockBits(rbmpd);
                return ret;
            }

            public static Bitmap PixelDistort(Bitmap original, int pixelSize)
            {
                BitmapData oldbmpData = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelBuffer = new byte[oldbmpData.Stride * original.Height];
                Marshal.Copy(oldbmpData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                original.UnlockBits(oldbmpData);

                for (int x = 0; x + 4 < pixelBuffer.Length; x += pixelSize * 4)
                {
                    byte R = pixelBuffer[x + 3], G = pixelBuffer[x + 2], B = pixelBuffer[x + 1], A = pixelBuffer[x];

                    for (int x1 = x; x1 + 4 < x + (pixelSize * 4) && x1 + 4 < pixelBuffer.Length; x1++)
                    {
                        pixelBuffer[x1 + 3] = R;
                        pixelBuffer[x1 + 2] = G;
                        pixelBuffer[x1 + 1] = B;
                        pixelBuffer[x1] = A;
                    }
                }

                Bitmap retBmp = new Bitmap(original.Width, original.Height);
                BitmapData bmpd = retBmp.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(pixelBuffer, 0, bmpd.Scan0, pixelBuffer.Length);
                retBmp.UnlockBits(bmpd);
                return retBmp;
            }

            public static Bitmap Pixelate(Bitmap original, int pixelSize)
            {
                BitmapData oldbmpData = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelBuffer = new byte[oldbmpData.Stride * original.Height];
                Marshal.Copy(oldbmpData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                original.UnlockBits(oldbmpData);

                for (int x = 0; x + 4 < pixelBuffer.Length; x += pixelSize * 4)
                {
                    byte R = pixelBuffer[x + 2], G = pixelBuffer[x + 1], B = pixelBuffer[x], A = pixelBuffer[x + 3]; // to pobiera kod z pierwszego pixela proste ARGB

                    for (int x1 = x; x1 + 4 <= x + pixelSize * 4 && x1 + 4 < pixelBuffer.Length; x1 += 4)
                    { // ta petla wypelnia dany blok
                        pixelBuffer[x1 + 3] = A;
                        pixelBuffer[x1 + 2] = R;
                        pixelBuffer[x1 + 1] = G;
                        pixelBuffer[x1] = B;
                    }
                }

                Bitmap retBmp = new Bitmap(original.Width, original.Height);
                BitmapData bmpd = retBmp.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(pixelBuffer, 0, bmpd.Scan0, pixelBuffer.Length);
                retBmp.UnlockBits(bmpd);
                return retBmp;
            }

            public enum DistanceFormulaType
            {
                Euclidean, Manhattan, Chebyshev
            }


            public class Pixel
            {
                private int xOffset = 0;
                public int XOffset
                {
                    get { return xOffset; }
                    set { xOffset = value; }
                }


                private int yOffset = 0;
                public int YOffset
                {
                    get { return yOffset; }
                    set { yOffset = value; }
                }


                private byte blue = 0;
                public byte Blue
                {
                    get { return blue; }
                    set { blue = value; }
                }


                private byte green = 0;
                public byte Green
                {
                    get { return green; }
                    set { green = value; }
                }


                private byte red = 0;
                public byte Red
                {
                    get { return red; }
                    set { red = value; }
                }
            }

            public class VoronoiPoint
            {
                private int xOffset = 0;
                public int XOffset
                {
                    get { return xOffset; }
                    set { xOffset = value; }
                }


                private int yOffset = 0;
                public int YOffset
                {
                    get { return yOffset; }
                    set { yOffset = value; }
                }


                private int blueTotal = 0;
                public int BlueTotal
                {
                    get { return blueTotal; }
                    set { blueTotal = value; }
                }


                private int greenTotal = 0;
                public int GreenTotal
                {
                    get { return greenTotal; }
                    set { greenTotal = value; }
                }


                private int redTotal = 0;
                public int RedTotal
                {
                    get { return redTotal; }
                    set { redTotal = value; }
                }


                public void CalculateAverages()
                {
                    if (pixelCollection.Count > 0)
                    {
                        blueAverage = blueTotal / pixelCollection.Count;
                        greenAverage = greenTotal / pixelCollection.Count;
                        redAverage = redTotal / pixelCollection.Count;
                    }
                }


                private int blueAverage = 0;
                public int BlueAverage
                {
                    get { return blueAverage; }
                }


                private int greenAverage = 0;
                public int GreenAverage
                {
                    get { return greenAverage; }
                }


                private int redAverage = 0;
                public int RedAverage
                {
                    get { return redAverage; }
                }


                private List<Pixel> pixelCollection = new List<Pixel>();
                public List<Pixel> PixelCollection
                {
                    get { return pixelCollection; }
                }


                public void AddPixel(Pixel pixel)
                {
                    blueTotal += pixel.Blue;
                    greenTotal += pixel.Green;
                    redTotal += pixel.Red;


                    pixelCollection.Add(pixel);
                }
            }

            private static Dictionary<int, int> squareRoots = new Dictionary<int, int>();


            private static int CalculateDistanceEuclidean(int x1, int x2, int y1, int y2)
            {
                int square = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);


                if (squareRoots.ContainsKey(square) == false)
                {
                    squareRoots.Add(square, (int)Math.Sqrt(square));
                }


                return squareRoots[square];
            }

            private static int CalculateDistanceManhattan(int x1, int x2, int y1, int y2)
            {
                return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
            }


            private static int CalculateDistanceChebyshev(int x1, int x2, int y1, int y2)
            {
                return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
            }

            public static Bitmap PerformStainedGlass(Bitmap sourceBitmap, int blockSize, double blockFactor, DistanceFormulaType distanceType, bool highlightEdges, byte edgeThreshold, Color edgeColor)
            {
                BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
                byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
                Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                sourceBitmap.UnlockBits(sourceData);

                int neighbourHoodTotal = 0;
                int sourceOffset = 0;
                int resultOffset = 0;
                int currentPixelDistance = 0;
                int nearestPixelDistance = 0;
                int nearesttPointIndex = 0;
                Random randomizer = new Random();
                List<VoronoiPoint> randomPointList = new List<VoronoiPoint>();


                for (int row = 0; row < sourceBitmap.Height - blockSize; row += blockSize)
                {
                    for (int col = 0; col < sourceBitmap.Width - blockSize; col += blockSize)
                    {
                        sourceOffset = row * sourceData.Stride + col * 4;
                        neighbourHoodTotal = 0;

                        for (int y = 0; y < blockSize; y++)
                        {
                            for (int x = 0; x < blockSize; x++)
                            {
                                resultOffset = sourceOffset + y * sourceData.Stride + x * 4;
                                neighbourHoodTotal += pixelBuffer[resultOffset];
                                neighbourHoodTotal += pixelBuffer[resultOffset + 1];
                                neighbourHoodTotal += pixelBuffer[resultOffset + 2];
                            }
                        }

                        randomizer = new Random(neighbourHoodTotal);
                        VoronoiPoint randomPoint = new VoronoiPoint();
                        randomPoint.XOffset = randomizer.Next(0, blockSize) + col;
                        randomPoint.YOffset = randomizer.Next(0, blockSize) + row;
                        randomPointList.Add(randomPoint);
                    }
                }

                int rowOffset = 0;
                int colOffset = 0;

                for (int bufferOffset = 0; bufferOffset < pixelBuffer.Length - 4; bufferOffset += 4)
                {
                    rowOffset = bufferOffset / sourceData.Stride;
                    colOffset = (bufferOffset % sourceData.Stride) / 4;
                    currentPixelDistance = 0;
                    nearestPixelDistance = blockSize * 4;
                    nearesttPointIndex = 0;
                    List<VoronoiPoint> pointSubset = new List<VoronoiPoint>();

                    pointSubset.AddRange(from t in randomPointList
                                         where
                                         rowOffset >= t.YOffset - blockSize * 2 &&
                                         rowOffset <= t.YOffset + blockSize * 2
                                         select t);

                    for (int k = 0; k < pointSubset.Count; k++)
                    {
                        if (distanceType == DistanceFormulaType.Euclidean)
                        {
                            currentPixelDistance = CalculateDistanceEuclidean(pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);
                        }
                        else if (distanceType == DistanceFormulaType.Manhattan)
                        {
                            currentPixelDistance = CalculateDistanceManhattan(pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);
                        }
                        else if (distanceType == DistanceFormulaType.Chebyshev)
                        {
                            currentPixelDistance = CalculateDistanceChebyshev(pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);
                        }

                        if (currentPixelDistance <= nearestPixelDistance)
                        {
                            nearestPixelDistance = currentPixelDistance;
                            nearesttPointIndex = k;

                            if (nearestPixelDistance <= blockSize / blockFactor)
                            {
                                break;
                            }
                        }
                    }


                    Pixel tmpPixel = new Pixel();
                    tmpPixel.XOffset = colOffset;
                    tmpPixel.YOffset = rowOffset;
                    tmpPixel.Blue = pixelBuffer[bufferOffset];
                    tmpPixel.Green = pixelBuffer[bufferOffset + 1];
                    tmpPixel.Red = pixelBuffer[bufferOffset + 2];
                    pointSubset[nearesttPointIndex].AddPixel(tmpPixel);
                }

                for (int k = 0; k < randomPointList.Count; k++)
                {
                    randomPointList[k].CalculateAverages();


                    for (int i = 0; i < randomPointList[k].PixelCollection.Count; i++)
                    {
                        resultOffset = randomPointList[k].PixelCollection[i].YOffset * sourceData.Stride + randomPointList[k].PixelCollection[i].XOffset * 4;


                        resultBuffer[resultOffset] = (byte)randomPointList[k].BlueAverage;
                        resultBuffer[resultOffset + 1] = (byte)randomPointList[k].GreenAverage;
                        resultBuffer[resultOffset + 2] = (byte)randomPointList[k].RedAverage;
                        resultBuffer[resultOffset + 3] = 255;
                    }
                }


                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
                BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
                resultBitmap.UnlockBits(resultData);

                if (highlightEdges == true)
                {
                    resultBitmap = Mainpaint.Diagnostics.EdgeDetection.GradientEdgeDetection(resultBitmap, Diagnostics.EdgeFilterMode.EdgeDetectionGradient, Diagnostics.DeriviationLevel.First, new float[] { 0.0f, 0.0f, 0.0f }, 0);
                }

                return resultBitmap;
            }

            public static Bitmap AddNoise(Bitmap Original, int amount)
            {
                Bitmap nbmp = new Bitmap(Original.Width, Original.Height);
                BitmapData bmpd = Original.LockBits(new Rectangle(0, 0, Original.Width, Original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelData = new byte[bmpd.Stride * Original.Height];
                Marshal.Copy(bmpd.Scan0, pixelData, 0, pixelData.Length);
                Original.UnlockBits(bmpd);

                Random trnd = new Random();

                for (int x = 0; x + 4 < pixelData.Length; x += 4)
                {
                    int B = pixelData[x + 1] + trnd.Next(-amount, amount + 1);  // Blue
                    int G = pixelData[x + 2] + trnd.Next(-amount, amount + 1);  // Green
                    int R = pixelData[x + 3] + trnd.Next(-amount, amount + 1);  // Red

                    if (B > 255) B = 255;
                    if (G > 255) G = 255;
                    if (R > 255) R = 255;

                    if (B < 0) B = 0;
                    if (G < 0) G = 0;
                    if (R < 0) R = 0;

                    pixelData[x + 1] = (byte)B;
                    pixelData[x + 2] = (byte)G;
                    pixelData[x + 3] = (byte)R;
                }

                BitmapData nbmpd = nbmp.LockBits(new Rectangle(0, 0, nbmp.Width, nbmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(pixelData, 0, nbmpd.Scan0, pixelData.Length);
                nbmp.UnlockBits(nbmpd);
                return nbmp;
            }

            public static Bitmap AddJitter(Bitmap original, int max)
            {
                Bitmap nbmp = new Bitmap(original.Width, original.Height);
                BitmapData bmpd = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelBuffer = new byte[bmpd.Stride * original.Height];
                Marshal.Copy(bmpd.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                original.UnlockBits(bmpd);

                Random trnd = new Random();

                for (int x = 0; x + 4 < pixelBuffer.Length; x += 4)
                {
                    int newx = trnd.Next(-max, max);
                    newx += x;
                    newx = Clamp(newx, 0, pixelBuffer.Length - 4);

                    pixelBuffer[x + 1] = pixelBuffer[newx + 1];
                    pixelBuffer[x + 2] = pixelBuffer[newx + 2];
                    pixelBuffer[x + 3] = pixelBuffer[newx + 3];
                }

                BitmapData nbmpd = nbmp.LockBits(new Rectangle(0, 0, nbmp.Width, nbmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(pixelBuffer, 0, nbmpd.Scan0, pixelBuffer.Length);
                nbmp.UnlockBits(nbmpd);
                return nbmp;
            }

            public static Bitmap MedianFilter(Bitmap original, int size)
            {
                BitmapData bmpd = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelBuffer = new byte[bmpd.Stride * original.Height];
                byte[] buffer = new byte[bmpd.Stride * original.Height];
                Marshal.Copy(bmpd.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                original.UnlockBits(bmpd);

                int filterOffset = (size - 1) / 2, byteOffset = 0, calcOffset = 0;
                List<int> pixels = new List<int>();
                byte[] centerPixel;

                for (int y = filterOffset; y < original.Height - filterOffset; y++)
                {
                    for (int x = filterOffset; x < original.Width - filterOffset; x++)
                    {
                        byteOffset = y * bmpd.Stride + x * 4;
                        pixels.Clear();

                        for (int fy = -filterOffset; fy <= filterOffset; fy++)
                        {
                            for (int fx = -filterOffset; fx < filterOffset; fx++)
                            {
                                calcOffset = byteOffset + (fx * 4) + (fy * bmpd.Stride);
                                pixels.Add(BitConverter.ToInt32(pixelBuffer, calcOffset));
                            }
                        }

                        pixels.Sort();
                        centerPixel = BitConverter.GetBytes(pixels[filterOffset]);

                        buffer[byteOffset] = centerPixel[0];
                        buffer[byteOffset + 1] = centerPixel[1];
                        buffer[byteOffset + 2] = centerPixel[2];
                        buffer[byteOffset + 3] = centerPixel[3];
                    }
                }

                Bitmap ret = new Bitmap(original.Width, original.Height);
                BitmapData rbmpd = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(buffer, 0, rbmpd.Scan0, buffer.Length);
                ret.UnlockBits(rbmpd);
                return ret;
            }

            public static Bitmap Bitonal(Bitmap original, Color primary, Color secondary, int threshold)
            {
                BitmapData bmpd = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[bmpd.Stride * original.Height];
                Marshal.Copy(bmpd.Scan0, pixelBuffer, 0, pixelBuffer.Length);

                original.UnlockBits(bmpd);

                for (int x = 0; x + 4 < pixelBuffer.Length; x += 4)
                {
                    if (pixelBuffer[x] + pixelBuffer[x + 1] + pixelBuffer[x + 2] <= threshold)
                    {
                        pixelBuffer[x] = primary.B;
                        pixelBuffer[x + 1] = primary.G;
                        pixelBuffer[x + 2] = primary.R;
                    }
                    else
                    {
                        pixelBuffer[x] = secondary.B;
                        pixelBuffer[x + 1] = secondary.G;
                        pixelBuffer[x + 2] = secondary.R;
                    }
                }

                Bitmap ret = new Bitmap(original.Width, original.Height);
                BitmapData rbmpd = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(pixelBuffer, 0, rbmpd.Scan0, pixelBuffer.Length);
                ret.UnlockBits(rbmpd);
                return ret;
            }

            public static Bitmap ColorFilter(Bitmap original, ColorFilterType type)
            {
                Bitmap bmp = (Bitmap)original.Clone();
                Color c = Color.Empty;
                int[] rgbm = new int[] { };

                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        c = bmp.GetPixel(x, y);
                        rgbm = new int[] { 0, 0, 0 };

                        if (type == ColorFilterType.Red)
                        {
                            rgbm = new int[] { c.R, c.G - 255, c.B - 255 };
                        }
                        else if (type == ColorFilterType.Green)
                        {
                            rgbm = new int[] { c.R - 255, c.G, c.B - 255 };
                        }
                        else if (type == ColorFilterType.Blue)
                        {
                            rgbm = new int[] { c.R - 255, c.G - 255, c.B };
                        }

                        rgbm[0] = Math.Max(rgbm[0], 0);
                        rgbm[0] = Math.Min(255, rgbm[0]);

                        rgbm[1] = Math.Max(rgbm[1], 0);
                        rgbm[1] = Math.Min(255, rgbm[1]);

                        rgbm[2] = Math.Max(rgbm[2], 0);
                        rgbm[2] = Math.Min(255, rgbm[2]);

                        bmp.SetPixel(x, y, Color.FromArgb(c.A, rgbm[0], rgbm[1], rgbm[2]));
                    }
                }
                return bmp;
            }

            private static byte[] GammaArray(double color)
            {
                byte[] gamma = new byte[256];
                for (int i = 0; i < 256; ++i)
                {
                    gamma[i] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / color)) + 0.5));
                }
                return gamma;
            }

            public static Bitmap GammaAdjustment(Bitmap original, double[] rgb)
            {
                Bitmap bmp = (Bitmap)original.Clone();
                BitmapData bmpd = bmp.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelBuffer = new byte[bmpd.Stride * original.Height];
                Marshal.Copy(bmpd.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                bmp.UnlockBits(bmpd);

                byte[] redG = GammaArray(rgb[0]);
                byte[] greenG = GammaArray(rgb[1]);
                byte[] blueG = GammaArray(rgb[2]);

                double[] crgb = new double[] { 0, 0, 0 };

                for (int x = 0; x + 4 < pixelBuffer.Length; x += 4)
                {
                    crgb[0] = redG[pixelBuffer[x + 2]];
                    crgb[1] = greenG[pixelBuffer[x + 1]];
                    crgb[2] = blueG[pixelBuffer[x]];

                    pixelBuffer[x + 2] = (byte)crgb[0];
                    pixelBuffer[x + 1] = (byte)crgb[1];
                    pixelBuffer[x] = (byte)crgb[2];
                }

                Bitmap ret = new Bitmap(original.Width, original.Height);
                BitmapData rbmpd = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(pixelBuffer, 0, rbmpd.Scan0, pixelBuffer.Length);
                ret.UnlockBits(rbmpd);
                return ret;
            }

            public static Bitmap Brightness(Bitmap original, int factor)
            {
                Bitmap bmp = (Bitmap)original.Clone();
                if (factor < -255) factor = -255;
                if (factor > 255) factor = 255;

                BitmapData bmpd = bmp.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                byte[] pixelData = new byte[bmpd.Stride * original.Height];

                Marshal.Copy(bmpd.Scan0, pixelData, 0, pixelData.Length);
                bmp.UnlockBits(bmpd);

                double[] rgb = new double[] { 0, 0, 0 };

                for (int x = 0; x + 4 < pixelData.Length; x += 4)
                {
                    rgb[0] = pixelData[x + 2] + factor;
                    rgb[1] = pixelData[x + 1] + factor;
                    rgb[2] = pixelData[x] + factor;

                    if (rgb[0] < 0) rgb[0] = 1;
                    else if (rgb[0] > 255) rgb[0] = 255;

                    if (rgb[1] < 0) rgb[1] = 1;
                    else if (rgb[1] > 255) rgb[1] = 255;

                    if (rgb[2] < 0) rgb[2] = 1;
                    else if (rgb[2] > 255) rgb[2] = 255;

                    pixelData[x + 2] = (byte)rgb[0];
                    pixelData[x + 1] = (byte)rgb[1];
                    pixelData[x] = (byte)rgb[2];
                }

                BitmapData rbmpd = bmp.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(pixelData, 0, rbmpd.Scan0, pixelData.Length);
                bmp.UnlockBits(rbmpd);

                return bmp;
            }

            public static Bitmap Contrast(Bitmap original, double factor, out Bitmap ret)
            {
                Bitmap bmp = (Bitmap)original.Clone();
                BitmapData bmpd = bmp.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[bmpd.Stride * original.Height];

                Marshal.Copy(bmpd.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                bmp.UnlockBits(bmpd);

                if (factor < -100) factor = -100;
                if (factor > 100) factor = 100;

                double clevel = Math.Pow((100.0 + factor) / 100.0, 2);
                double[] rgb = new double[] { 0, 0, 0 };

                for (int x = 0; x + 4 < pixelBuffer.Length; x += 4)
                {
                    rgb[0] = pixelBuffer[x + 2];
                    rgb[1] = pixelBuffer[x + 1];
                    rgb[2] = pixelBuffer[x];

                    double R = ((((rgb[0] / 255.0) - 0.5) * clevel) + 0.5) * 255.0;
                    if (R < 0) R = 0;
                    if (R > 255) R = 255;

                    double G = ((((rgb[1] / 255.0) - 0.5) * clevel) + 0.5) * 255.0;
                    if (G < 0) G = 0;
                    if (G > 255) G = 255;

                    double B = ((((rgb[2] / 255.0) - 0.5) * clevel) + 0.5) * 255.0;
                    if (B < 0) B = 0;
                    if (B > 255) B = 255;

                    pixelBuffer[x + 2] = (byte)R;
                    pixelBuffer[x + 1] = (byte)G;
                    pixelBuffer[x] = (byte)B;
                }

                Bitmap rbmp = new Bitmap(original.Width, original.Height);
                BitmapData rbmpd = rbmp.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(pixelBuffer, 0, rbmpd.Scan0, pixelBuffer.Length);
                rbmp.UnlockBits(rbmpd);
                ret = rbmp;
                return rbmp;
            }

            public static Bitmap OilPaintFilter(Bitmap original, int levels, int filterSize)
            {
                BitmapData bmpd = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelBuffer = new byte[bmpd.Stride * bmpd.Height];
                byte[] resultBuffer = new byte[bmpd.Stride * bmpd.Height];
                Marshal.Copy(bmpd.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                original.UnlockBits(bmpd);


                int[] intensityBin = new int[levels];
                int[] blueBin = new int[levels];
                int[] greenBin = new int[levels];
                int[] redBin = new int[levels];


                levels -= 1;


                int filterOffset = (filterSize - 1) / 2;
                int byteOffset = 0;
                int calcOffset = 0;
                int currentIntensity = 0;
                int maxIntensity = 0;
                int maxIndex = 0;


                double blue = 0;
                double green = 0;
                double red = 0;


                for (int offsetY = filterOffset; offsetY < original.Height - filterOffset; offsetY++)
                {
                    for (int offsetX = filterOffset; offsetX < original.Width - filterOffset; offsetX++)
                    {
                        blue = green = red = 0;

                        currentIntensity = maxIntensity = maxIndex = 0;

                        intensityBin = new int[levels + 1];
                        blueBin = new int[levels + 1];
                        greenBin = new int[levels + 1];
                        redBin = new int[levels + 1];

                        byteOffset = offsetY * bmpd.Stride + offsetX * 4;


                        for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                            {
                                calcOffset = byteOffset + (filterX * 4) + (filterY * bmpd.Stride);

                                currentIntensity = (int)Math.Round(((double)(pixelBuffer[calcOffset] + pixelBuffer[calcOffset + 1] + pixelBuffer[calcOffset + 2]) / 3.0 * (levels)) / 255.0);


                                intensityBin[currentIntensity] += 1;
                                blueBin[currentIntensity] += pixelBuffer[calcOffset];
                                greenBin[currentIntensity] += pixelBuffer[calcOffset + 1];
                                redBin[currentIntensity] += pixelBuffer[calcOffset + 2];

                                if (intensityBin[currentIntensity] > maxIntensity)
                                {
                                    maxIntensity = intensityBin[currentIntensity];
                                    maxIndex = currentIntensity;
                                }
                            }
                        }


                        blue = blueBin[maxIndex] / maxIntensity;
                        green = greenBin[maxIndex] / maxIntensity;
                        red = redBin[maxIndex] / maxIntensity;


                        resultBuffer[byteOffset] = ClipByte(blue);
                        resultBuffer[byteOffset + 1] = ClipByte(green);
                        resultBuffer[byteOffset + 2] = ClipByte(red);
                        resultBuffer[byteOffset + 3] = 255;

                    }
                }


                Bitmap nbmp = new Bitmap(original.Width, original.Height);
                BitmapData nbmpd = nbmp.LockBits(new Rectangle(0, 0, nbmp.Width, nbmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(resultBuffer, 0, nbmpd.Scan0, resultBuffer.Length);
                nbmp.UnlockBits(nbmpd);
                return nbmp;
            }

            private static byte ClipByte(double color)
            {
                return (byte)(color > 255 ? 255 :
                       (color < 0 ? 0 : color));
            }

            private static bool CheckThreshold(byte[] pixelBuffer, int offset1, int offset2, ref int gradientValue, byte threshold, int divideBy = 1)
            {
                gradientValue += Math.Abs(pixelBuffer[offset1] - pixelBuffer[offset2]) / divideBy;
                gradientValue += Math.Abs(pixelBuffer[offset1 + 1] - pixelBuffer[offset2 + 1]) / divideBy;
                gradientValue += Math.Abs(pixelBuffer[offset1 + 2] - pixelBuffer[offset2 + 2]) / divideBy;
                return (gradientValue >= threshold);
            }

            public static Bitmap GradientBasedEdgeDetectionFilter(Bitmap sourceBitmap, byte threshold = 0)
            {
                BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
                byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
                Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length); sourceBitmap.UnlockBits(sourceData);

                int sourceOffset = 0, gradientValue = 0;
                bool exceedsThreshold = false;

                for (int offsetY = 1; offsetY < sourceBitmap.Height - 1; offsetY++)
                {
                    for (int offsetX = 1; offsetX < sourceBitmap.Width - 1; offsetX++)
                    {
                        sourceOffset = offsetY * sourceData.Stride + offsetX * 4;
                        gradientValue = 0;
                        exceedsThreshold = true;

                        CheckThreshold(pixelBuffer, sourceOffset - 4, sourceOffset + 4, ref gradientValue, threshold, 2);

                        exceedsThreshold = CheckThreshold(pixelBuffer, sourceOffset - sourceData.Stride, sourceOffset + sourceData.Stride, ref gradientValue, threshold, 2);
                        if (exceedsThreshold == false)
                        {
                            gradientValue = 0;
                            exceedsThreshold = CheckThreshold(pixelBuffer, sourceOffset - 4, sourceOffset + 4, ref gradientValue, threshold);
                            if (exceedsThreshold == false)
                            {
                                gradientValue = 0;
                                exceedsThreshold = CheckThreshold(pixelBuffer, sourceOffset - sourceData.Stride, sourceOffset + sourceData.Stride, ref gradientValue, threshold);
                                if (exceedsThreshold == false)
                                {
                                    gradientValue = 0;
                                    CheckThreshold(pixelBuffer, sourceOffset - 4 - sourceData.Stride, sourceOffset + 4 + sourceData.Stride, ref gradientValue, threshold, 2);
                                    exceedsThreshold = CheckThreshold(pixelBuffer, sourceOffset - sourceData.Stride + 4, sourceOffset - 4 + sourceData.Stride, ref gradientValue, threshold, 2);
                                    if (exceedsThreshold == false)
                                    {
                                        gradientValue = 0;
                                        exceedsThreshold = CheckThreshold(pixelBuffer, sourceOffset - 4 - sourceData.Stride, sourceOffset + 4 + sourceData.Stride, ref gradientValue, threshold);
                                        if (exceedsThreshold == false)
                                        {
                                            gradientValue = 0;
                                            exceedsThreshold = CheckThreshold(pixelBuffer, sourceOffset - sourceData.Stride + 4, sourceOffset + sourceData.Stride - 4, ref gradientValue, threshold);
                                        }
                                    }
                                }
                            }
                        }
                        resultBuffer[sourceOffset] = (byte)(exceedsThreshold ? 255 : 0);
                        resultBuffer[sourceOffset + 1] = resultBuffer[sourceOffset];
                        resultBuffer[sourceOffset + 2] = resultBuffer[sourceOffset];
                        resultBuffer[sourceOffset + 3] = 255;
                    }
                }

                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
                BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length); resultBitmap.UnlockBits(resultData);
                return resultBitmap;
            }

            public static Bitmap CartoonFilter(Bitmap original, int levels, int filterSize, byte threshold)
            {
                Bitmap paintFilterImage = OilPaintFilter(original, levels, filterSize);

                Bitmap edgeDetectImage = GradientBasedEdgeDetectionFilter(original, threshold);
                BitmapData paintData = paintFilterImage.LockBits(new Rectangle(0, 0, paintFilterImage.Width, paintFilterImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] paintPixelBuffer = new byte[paintData.Stride * paintData.Height];
                Marshal.Copy(paintData.Scan0, paintPixelBuffer, 0, paintPixelBuffer.Length);
                paintFilterImage.UnlockBits(paintData);

                BitmapData edgeData = edgeDetectImage.LockBits(new Rectangle(0, 0, edgeDetectImage.Width, edgeDetectImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] edgePixelBuffer = new byte[edgeData.Stride * edgeData.Height];
                Marshal.Copy(edgeData.Scan0, edgePixelBuffer, 0, edgePixelBuffer.Length);
                edgeDetectImage.UnlockBits(edgeData);

                byte[] resultBuffer = new byte[edgeData.Stride * edgeData.Height];


                for (int k = 0; k + 4 < paintPixelBuffer.Length; k += 4)
                {
                    if (edgePixelBuffer[k] == 255 || edgePixelBuffer[k + 1] == 255 || edgePixelBuffer[k + 2] == 255)
                    {
                        resultBuffer[k] = 0;
                        resultBuffer[k + 1] = 0;
                        resultBuffer[k + 2] = 0;
                        resultBuffer[k + 3] = 255;
                    }
                    else
                    {
                        resultBuffer[k] = paintPixelBuffer[k];
                        resultBuffer[k + 1] = paintPixelBuffer[k + 1];
                        resultBuffer[k + 2] = paintPixelBuffer[k + 2];
                        resultBuffer[k + 3] = 255;
                    }
                }


                Bitmap nbmp = new Bitmap(original.Width, original.Height);
                BitmapData nbmpd = nbmp.LockBits(new Rectangle(0, 0, nbmp.Width, nbmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(resultBuffer, 0, nbmpd.Scan0, resultBuffer.Length);
                nbmp.UnlockBits(nbmpd);
                return nbmp;
            }

        }
    }
}