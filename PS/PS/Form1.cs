using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Windows.Forms;

namespace PS
{
    public partial class Form1 : Form
    {
        //Sobel operator kernel for horizontal pixel changes
        int imageWidth = 0;
        int imageHeight = 0;
        private static double[,] xSobel
        {
            get
            {
                return new double[,]
                {
                    //{ 1, 0, -1 },
                    //{ 2, 0, -2 },
                    //{ 1, 0, -1 }
                    { -1, 0, 1 },
                    { -2, 0, 2 },
                    { -1, 0, 1 }
                };
            }
        }

        //Sobel operator kernel for vertical pixel changes
        private static double[,] ySobel
        {
            get
            {
                return new double[,]
                {
                    //{ 1, 2, 1 },
                    //{ 0, 0, 0 },
                    //{ -1, -2, -1 }
                    { -1, -2, -1 },
                    {  0,  0,  0 },
                    { 1, 2, 1 }
                };
            }
        }

        private static double[,] gauss3x3
        {
            get
            {
                return new double[,]
                {
                    {  1,  2,  1 },
                    {  2,  4,  2 },
                    {  1,  2,  1 }
                };
            }
        }

        public static double[,] Laplacian3x3
        {
            get
            {
                return new double[,]
                { { -1, -1, -1, },
         { -1,  8, -1, },
         { -1, -1, -1, }, };
            }
        }

        ConvMatrix m;

        public Form1()
        {
            m = new ConvMatrix();
            
            InitializeComponent();
        }

        private void bttn_Tallozas_Click(object sender, EventArgs e)
        {
            Tallozas();
        }

        private void bttn_Negalas_Click(object sender, EventArgs e)
        {
            newImage.Image = Negalas(new Bitmap(originalImage.Image));
            SaveToFile(newImage.Image, "Negalas.jpeg");
        }

        private void bttn_GammaTranszformacio_Click(object sender, EventArgs e)
        {
            newImage.Image = GammaTransz(new Bitmap(originalImage.Image), 0.4, 1);
            SaveToFile(newImage.Image, "GammaTransz.jpg");
        }

        private void bttn_LogaritmikusTranszformacio_Click(object sender, EventArgs e)
        {
            newImage.Image = LogTransform(new Bitmap(originalImage.Image), 800);
            SaveToFile(newImage.Image, "LogaritmikusTransz.jpg");
        }

        private void bttn_Szurkites_Click(object sender, EventArgs e)
        {
            newImage.Image = Szurkites(new Bitmap(originalImage.Image));
            SaveToFile(newImage.Image, "Szurkites.jpg");
        }

        private void bttn_HisztorgramKeszites_Click(object sender, EventArgs e)
        {
            newImage.Image = HisztogramKeszites(new Bitmap(originalImage.Image));
            SaveToFile(newImage.Image, "Hisztogramkeszites.jpg");
        }

        private void bttn_HisztogramKiegyenlites_Click(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(originalImage.Image);
            int w = temp.Width;
            int h = temp.Height;

            BitmapData sd = temp.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = sd.Stride * sd.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(sd.Scan0, buffer, 0, bytes);
            temp.UnlockBits(sd);
            int current = 0;
            double[] pn = new double[256];

            for (int p = 0; p < bytes; p += 4)
            {
                pn[buffer[p]]++;
            }
            for (int prob = 0; prob < pn.Length; prob++)
            {
                pn[prob] /= (w * h);
            }
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    current = y * sd.Stride + x * 4;
                    double sum = 0;
                    for (int i = 0; i < buffer[current]; i++)
                    {
                        sum += pn[i];
                    }
                    for (int c = 0; c < 3; c++)
                    {
                        result[current + c] = (byte)Math.Floor(255 * sum);
                    }
                    result[current + 3] = 255;
                }
            }

            Bitmap res = new Bitmap(w, h);
            BitmapData rd = res.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, rd.Scan0, bytes);
            res.UnlockBits(rd);

            newImage.Image = res;
            SaveToFile(newImage.Image, "Hisztogramkiegyenlítés.jpg");
        }

        private void bttn_AtlagoloSzuro_Click(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(originalImage.Image);

            List<byte> termsList = new List<byte>();

            byte[,] image = new byte[temp.Width, temp.Height];

            //Convert to Grayscale 
            for (int i = 0; i < temp.Width; i++)
            {
                for (int j = 0; j < temp.Height; j++)
                {
                    var c = temp.GetPixel(i, j);
                    byte gray = (byte)(.333 * c.R + .333 * c.G + .333 * c.B);
                    image[i, j] = gray;
                }
            }

            //applying Median Filtering 
            for (int i = 0; i <= temp.Width - 3; i++)
            {
                for (int j = 0; j <= temp.Height - 3; j++)
                {
                    for (int x = i; x <= i + 2; x++)
                        for (int y = j; y <= j + 2; y++)
                        {
                            termsList.Add(image[x, y]);
                        }
                    byte[] terms = termsList.ToArray();
                    termsList.Clear();
                    Array.Sort<byte>(terms);
                    Array.Reverse(terms);
                    byte color = terms[4];
                    temp.SetPixel(i + 1, j + 1, Color.FromArgb(color, color, color));
                }
            }

            newImage.Image = temp;
            SaveToFile(newImage.Image, "AtlagoloSzures.jpg");
        }

        private void bttn_GaussSzuro_Click(object sender, EventArgs e)
        {
            //newImage.Image = ConvolutionFilter(new Bitmap(originalImage.Image), gauss3x3, 1.0, 0, false);
            newImage.Image = Conv3x3(new Bitmap(originalImage.Image), m.Gauss());
            SaveToFile(newImage.Image, "Gauss.jpg");
        }

        private void bttn_SobelEldetektor_Click(object sender, EventArgs e)
        {
            newImage.Image = Sobel(new Bitmap(originalImage.Image), xSobel, ySobel, 1.0, 0, false);
            SaveToFile(newImage.Image, "Sobel.jpg");
        }

        private void bttn_LaplaceEldetektor_Click(object sender, EventArgs e)
        {
            newImage.Image = ConvolutionFilter(new Bitmap(originalImage.Image), Laplacian3x3, 1.0, 0, false);
        }

        private void bttn_JellemzoPontok_Click(object sender, EventArgs e)
        {
           newImage.Image = Harris(new Bitmap(originalImage.Image), xSobel, ySobel, true);
           SaveToFile(newImage.Image, "JellemzoPontok.jpg");
        }

        public static Bitmap Conv3x3(Bitmap b, ConvMatrix m)
        {
            // GDI+-ra figyelni kell, mert nem RGB a kimenetel, hanem BGR!
            Bitmap bSrc = (Bitmap)b.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                                ImageLockMode.ReadWrite,
                                PixelFormat.Format24bppRgb);

            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height),
                               ImageLockMode.ReadWrite,
                               PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;

            IntPtr Scan0 = bmData.Scan0;
            IntPtr SrcScan0 = bmSrc.Scan0;
            
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;
                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = ((((pSrc[2] * m.TopLeft) +
                            (pSrc[5] * m.TopMid) +
                            (pSrc[8] * m.TopRight) +
                            (pSrc[2 + stride] * m.MidLeft) +
                            (pSrc[5 + stride] * m.Pixel) +
                            (pSrc[8 + stride] * m.MidRight) +
                            (pSrc[2 + stride2] * m.BottomLeft) +
                            (pSrc[5 + stride2] * m.BottomMid) +
                            (pSrc[8 + stride2] * m.BottomRight))
                            / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * m.TopLeft) +
                            (pSrc[4] * m.TopMid) +
                            (pSrc[7] * m.TopRight) +
                            (pSrc[1 + stride] * m.MidLeft) +
                            (pSrc[4 + stride] * m.Pixel) +
                            (pSrc[7 + stride] * m.MidRight) +
                            (pSrc[1 + stride2] * m.BottomLeft) +
                            (pSrc[4 + stride2] * m.BottomMid) +
                            (pSrc[7 + stride2] * m.BottomRight))
                            / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * m.TopLeft) +
                                       (pSrc[3] * m.TopMid) +
                                       (pSrc[6] * m.TopRight) +
                                       (pSrc[0 + stride] * m.MidLeft) +
                                       (pSrc[3 + stride] * m.Pixel) +
                                       (pSrc[6 + stride] * m.MidRight) +
                                       (pSrc[0 + stride2] * m.BottomLeft) +
                                       (pSrc[3 + stride2] * m.BottomMid) +
                                       (pSrc[6 + stride2] * m.BottomRight))
                            / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[3 + stride] = (byte)nPixel;

                        p += 3;
                        pSrc += 3;
                    }

                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);
            return b;
        }

        public static Bitmap Laplacian3x3Filter(Bitmap sourceBitmap, bool grayscale = true)
        {
            Bitmap resultBitmap = ConvolutionFilter(sourceBitmap, Laplacian3x3, 1.0, 0, grayscale);
            return resultBitmap;

        }

        private static Bitmap ConvolutionFilter(Bitmap sourceBitmap, double[,] filterMatrix, double factor = 1, int bias = 0, bool grayscale = false)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);


            if (grayscale)
            {
                float rgb = 0;

                for (int k = 0; k < pixelBuffer.Length; k += 4)
                {
                    rgb = pixelBuffer[k] * 0.11f;
                    rgb += pixelBuffer[k + 1] * 0.59f;
                    rgb += pixelBuffer[k + 2] * 0.3f;

                    pixelBuffer[k] = (byte)rgb;
                    pixelBuffer[k + 1] = pixelBuffer[k];
                    pixelBuffer[k + 2] = pixelBuffer[k];
                    pixelBuffer[k + 3] = 255;
                }
            }

            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;

            int filterWidth = filterMatrix.GetLength(1);
            int filterHeight = filterMatrix.GetLength(0);

            int filterOffset = (filterWidth - 1) / 2;
            int calcOffset = 0;

            int byteOffset = 0;

            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;

                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;


                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);

                            blue += (double)(pixelBuffer[calcOffset]) * filterMatrix[filterY + filterOffset, filterX + filterOffset];

                            green += (double)(pixelBuffer[calcOffset + 1]) * filterMatrix[filterY + filterOffset, filterX + filterOffset];

                            red += (double)(pixelBuffer[calcOffset + 2]) * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                        }
                    }

                    blue = factor * blue + bias;
                    green = factor * green + bias;
                    red = factor * red + bias;

                    if (blue > 255)
                    { blue = 255; }
                    else if (blue < 0)
                    { blue = 0; }

                    if (green > 255)
                    { green = 255; }
                    else if (green < 0)
                    { green = 0; }

                    if (red > 255)
                    { red = 255; }
                    else if (red < 0)
                    { red = 0; }

                    resultBuffer[byteOffset] = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        private Bitmap GammaTransz(Bitmap img, double gamma, double c = 1d)
        {
            int width = img.Width;
            int height = img.Height;
            BitmapData srcData = img.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            img.UnlockBits(srcData);
            int current = 0;
            int cChannels = 3;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    current = y * srcData.Stride + x * 4;
                    for (int i = 0; i < cChannels; i++)
                    {
                        double range = (double)buffer[current + i] / 255;
                        double correction = c * Math.Pow(range, gamma);
                        result[current + i] = (byte)(correction * 255);
                    }
                    result[current + 3] = 255;
                }
            }
            Bitmap resImg = new Bitmap(width, height);
            BitmapData resData = resImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, resData.Scan0, bytes);
            resImg.UnlockBits(resData);
            return resImg;
        }

        public Bitmap Negalas(Bitmap originalImage)
        {
            Bitmap newImage = originalImage;

            for (int y = 0; (y <= (newImage.Height - 1)); y++)
            {
                for (int x = 0; (x <= (newImage.Width - 1)); x++)
                {
                    Color inv = newImage.GetPixel(x, y);
                    inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                    newImage.SetPixel(x, y, inv);
                }
            }

            return newImage;
        }

        public Bitmap Szurkites(Bitmap originalImage)
        {
            Bitmap newImage = new Bitmap(originalImage);
            Color p;

            int width = newImage.Width;
            int height = newImage.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //get pixel value
                    p = newImage.GetPixel(x, y);

                    //extract pixel component ARGB
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    //find average
                    int avg = (r + g + b) / 3;

                    //set new pixel value
                    newImage.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));
                }
            }
            return newImage;
        }

        public Bitmap HisztogramKeszites(Bitmap originalImage)
        {
            Bitmap bmp = new Bitmap(originalImage);
            int[] histogram_r = new int[256];
            float max = 0;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int redValue = bmp.GetPixel(i, j).R;
                    histogram_r[redValue]++;
                    if (max < histogram_r[redValue])
                        max = histogram_r[redValue];
                }
            }

            Bitmap img = new Bitmap(400, 400);
            using (Graphics g = Graphics.FromImage(img))
            {
                for (int i = 0; i < histogram_r.Length; i++)
                {
                    float pct = histogram_r[i] / max;   // What percentage of the max is this value?
                    g.DrawLine(Pens.Black,
                        new Point(i, img.Height - 5),
                        new Point(i, img.Height - 5 - (int)(pct * 400))  // Use that percentage of the height
                        );
                }
            }
            return img;
        }

        public Bitmap LogTransform(Bitmap img, int constant)
        {
            int w = img.Width;
            int h = img.Height;

            img = Szurkites(img);

            BitmapData sd = img.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = sd.Stride * sd.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(sd.Scan0, buffer, 0, bytes);
            img.UnlockBits(sd);
            int current = 0;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    current = y * sd.Stride + x * 4;
                    for (int i = 0; i < 3; i++)
                    {
                        result[current + i] = (byte)(constant * Math.Log10(buffer[current + i] + 1));
                    }
                    result[current + 3] = 255;
                }
            }

            Bitmap resimg = new Bitmap(w, h);
            BitmapData rd = resimg.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, rd.Scan0, bytes);
            resimg.UnlockBits(rd);
            return resimg;
        }

        public Bitmap Harris(Bitmap sourceImage, double[,] xkernel, double[,] ykernel, bool grayscale)
        {
            //Image dimensions stored in variables for convenience
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            //Lock source image bits into system memory
            BitmapData srcData = sourceImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            //Get the total number of bytes in your image - 32 bytes per pixel x image width x image height -> for 32bpp images
            int bytes = srcData.Stride * srcData.Height;

            //Create byte arrays to hold pixel information of your image
            byte[] pixelBuffer = new byte[bytes];
            byte[] resultBuffer = new byte[bytes];

            //Get the address of the first pixel data
            IntPtr srcScan0 = srcData.Scan0;

            //Copy image data to one of the byte arrays
            Marshal.Copy(srcScan0, pixelBuffer, 0, bytes);

            //Unlock bits from system memory -> we have all our needed info in the array
            sourceImage.UnlockBits(srcData);

            //Convert your image to grayscale if necessary
            if (grayscale == true)
            {
                float rgb = 0;
                for (int c = 0; c < pixelBuffer.Length; c += 4)
                {
                    rgb = pixelBuffer[c] * .21f;
                    rgb += pixelBuffer[c + 1] * .71f;
                    rgb += pixelBuffer[c + 2] * .071f;
                    pixelBuffer[c] = (byte)rgb;
                    pixelBuffer[c + 1] = pixelBuffer[c];
                    pixelBuffer[c + 2] = pixelBuffer[c];
                    pixelBuffer[c + 3] = 255;
                }
            }

            //Create variable for pixel data for each kernel
            double xr = 0.0;
            double xg = 0.0;
            double xb = 0.0;
            double yr = 0.0;
            double yg = 0.0;
            double yb = 0.0;
            double rt = 0.0;
            double gt = 0.0;
            double bt = 0.0;

            //This is how much your center pixel is offset from the border of your kernel
            //Sobel is 3x3, so center is 1 pixel from the kernel border
            int filterOffset = 1;
            int calcOffset = 0;
            int byteOffset = 0;

            byte[] hX = new byte[bytes];
            byte[] hY = new byte[bytes];
            int[] xY = new int[bytes];
            int[] xX = new int[bytes];
            int[] yY = new int[bytes];
            int[] sumXx = new int[bytes];
            int[] sumYy = new int[bytes];
            int[] sumXY = new int[bytes];
            int[] sumXxPlusSumYyDivTwo = new int[bytes];
            int[] sumXxPlusSumYyRootTwo = new int[bytes];
            int[] sumXxMultimplySumYyMinusSumXY = new int[bytes];
            int[] eigenValue = new int[bytes];
            int[] OtherEigenValue = new int[bytes];

            //Start with the pixel that is offset 1 from top and 1 from the left side
            //this is so entire kernel is on your image
            for (int OffsetY = filterOffset; OffsetY < height - filterOffset; OffsetY++)
            {
                for (int OffsetX = filterOffset; OffsetX < width - filterOffset; OffsetX++)
                {
                    //reset rgb values to 0
                    xr = xg = xb = yr = yg = yb = 0;
                    rt = gt = bt = 0.0;

                    //position of the kernel center pixel
                    byteOffset = OffsetY * srcData.Stride + OffsetX * 4;

                    //kernel calculations
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + filterX * 4 + filterY * srcData.Stride;
                            xb += (double)(pixelBuffer[calcOffset]) * xkernel[filterY + filterOffset, filterX + filterOffset];
                            xg += (double)(pixelBuffer[calcOffset + 1]) * xkernel[filterY + filterOffset, filterX + filterOffset];
                            xr += (double)(pixelBuffer[calcOffset + 2]) * xkernel[filterY + filterOffset, filterX + filterOffset];

                            yb += (double)(pixelBuffer[calcOffset]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                            yg += (double)(pixelBuffer[calcOffset + 1]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                            yr += (double)(pixelBuffer[calcOffset + 2]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                        }
                    }

                    //total rgb values for this pixel
                    bt = Math.Sqrt((xb * xb) + (yb * yb));
                    gt = Math.Sqrt((xg * xg) + (yg * yg));
                    rt = Math.Sqrt((xr * xr) + (yr * yr));

                    //set limits, bytes can hold values from 0 up to 255;
                    if (bt > 255) bt = 255;
                    else if (bt < 0) bt = 0;
                    if (gt > 255) gt = 255;
                    else if (gt < 0) gt = 0;
                    if (rt > 255) rt = 255;
                    else if (rt < 0) rt = 0;

                    // X
                    hX[byteOffset] = (byte)(xb);
                    hX[byteOffset + 1] = (byte)(xg);
                    hX[byteOffset + 2] = (byte)(xr);
                    hX[byteOffset + 3] = 255;

                    // Y
                    hY[byteOffset] = (byte)(yb);
                    hY[byteOffset + 1] = (byte)(yg);
                    hY[byteOffset + 2] = (byte)(yr);
                    hY[byteOffset + 3] = 255;

                    // X * Y
                    xY[byteOffset] = (int)(xb * yb);
                    xY[byteOffset + 1] = (int)(xg * yg);
                    xY[byteOffset + 2] = (int)(xg * yr);
                    xY[byteOffset + 3] = 255;

                    // X ^ 2
                    xX[byteOffset] = (int)(xb * xb);
                    xX[byteOffset + 1] = (int)(xg * xg);
                    xX[byteOffset + 2] = (int)(xg * xr);
                    xX[byteOffset + 3] = 255;

                    // Y ^ 2
                    yY[byteOffset] = (int)(yb * yb);
                    yY[byteOffset + 1] = (int)(yg * yg);
                    yY[byteOffset + 2] = (int)(yg * yr);
                    yY[byteOffset + 3] = 255;

                    //set new data in the other byte array for your image data
                    resultBuffer[byteOffset] = (byte)(bt);
                    resultBuffer[byteOffset + 1] = (byte)(gt);
                    resultBuffer[byteOffset + 2] = (byte)(rt);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }

            /*for (int i = (width * 4) + 4; i < xX.Length - (width * 4) - 4; i += 4)
            {             
                        // mid    // left      // right                   
                sumXx[i] = xX[i] + xX[i - 4] + xX[i + 4] +
                    // topleft                // topright                // top mid
                    xX[i - (width * 4) - 4] + xX[i - (width * 4) + 4] + xX[i - (width * 4)] +
                    // botleft                 // botright                 // bot mid
                    xX[i + (width * 4) - 4] + xX[i + (width * 4) + 4] + xX[i + (width * 4)];

                        // mid    // left      // right
                sumYy[i] = yY[i] + yY[i - 4] + yY[i + 4] +
                    // topleft                // topright                // top mid
                    yY[i - (width * 4) - 4] + yY[i - (width * 4) + 4] + yY[i - (width * 4)] +
                    // botleft                 // botright                 // bot mid
                    yY[i + (width * 4) - 4] + yY[i + (width * 4) + 4] + yY[i + (width * 4)];

                        // mid              // left                      // right
                sumXY[i] = hY[i] * hX[i] + hY[i - 4] * hX[i - 4] + hY[i + 4] * hX[i + 4] +
                    // topleft                                          // topright                                         // top mid
                    yY[i - (width * 4) - 4] * hX[i - (width * 4) - 4] + hY[i - (width * 4) + 4] * hX[i - (width * 4) + 4] + hY[i - (width * 4)] * hX[i - (width * 4)] +
                    // botleft                                          // botright                                         // bot mid
                    hX[i + (width * 4) - 4] * hX[i + (width * 4) - 4] + hX[i + (width * 4) + 4] * hY[i + (width * 4) + 4] + hY[i + (width * 4)] * hY[i + (width * 4)];

                // (a+d)/2
                sumXxPlusSumYyDivTwo[i] = ((sumXx[i] + sumYy[i]) / 2) / 10000;
                
                // (a+d)^2
                sumXxPlusSumYyRootTwo[i] = ((sumXx[i] + sumYy[i]) * (sumXx[i] + sumYy[i])) / 10000;

                // ad - bc
                sumXxMultimplySumYyMinusSumXY[i] = ((sumXx[i] * sumYy[i]) - (sumXY[i] * sumXY[i])) / 10000;

                eigenValue[i] = sumXxPlusSumYyDivTwo[i] + (int)Math.Sqrt(((sumXxPlusSumYyRootTwo[i] / 4) - sumXxMultimplySumYyMinusSumXY[i]));
                OtherEigenValue[i] = sumXxPlusSumYyDivTwo[i] - (int)Math.Sqrt(((sumXxPlusSumYyRootTwo[i] / 4) - sumXxMultimplySumYyMinusSumXY[i]));

                if (i % (width * 4 - 4) == 0)
                {
                    i += 4;
                }
            }*/

            int i = (width * 4) + 4;

            while (i < xX.Length - (width * 4) - 4)
            {

                // mid    // left      // right                   
                sumXx[i] = xX[i] + xX[i - 4] + xX[i + 4] +
                    // topleft                // topright                // top mid
                    xX[i - (width * 4) - 4] + xX[i - (width * 4) + 4] + xX[i - (width * 4)] +
                    // botleft                 // botright                 // bot mid
                    xX[i + (width * 4) - 4] + xX[i + (width * 4) + 4] + xX[i + (width * 4)];

                // mid    // left      // right
                sumYy[i] = yY[i] + yY[i - 4] + yY[i + 4] +
                    // topleft                // topright                // top mid
                    yY[i - (width * 4) - 4] + yY[i - (width * 4) + 4] + yY[i - (width * 4)] +
                    // botleft                 // botright                 // bot mid
                    yY[i + (width * 4) - 4] + yY[i + (width * 4) + 4] + yY[i + (width * 4)];

                // mid              // left                      // right
                sumXY[i] = hY[i] * hX[i] + hY[i - 4] * hX[i - 4] + hY[i + 4] * hX[i + 4] +
                    // topleft                                          // topright                                         // top mid
                    yY[i - (width * 4) - 4] * hX[i - (width * 4) - 4] + hY[i - (width * 4) + 4] * hX[i - (width * 4) + 4] + hY[i - (width * 4)] * hX[i - (width * 4)] +
                    // botleft                                          // botright                                         // bot mid
                    hX[i + (width * 4) - 4] * hX[i + (width * 4) - 4] + hX[i + (width * 4) + 4] * hY[i + (width * 4) + 4] + hY[i + (width * 4)] * hY[i + (width * 4)];

                // (a+d)/2
                sumXxPlusSumYyDivTwo[i] = ((sumXx[i] + sumYy[i]) / 2) / 10000;

                // (a+d)^2
                sumXxPlusSumYyRootTwo[i] = ((sumXx[i] + sumYy[i]) * (sumXx[i] + sumYy[i])) / 10000;

                // ad - bc
                sumXxMultimplySumYyMinusSumXY[i] = ((sumXx[i] * sumYy[i]) - (sumXY[i] * sumXY[i])) / 10000;

                eigenValue[i] = sumXxPlusSumYyDivTwo[i] + (int)Math.Sqrt(((sumXxPlusSumYyRootTwo[i] / 4) - sumXxMultimplySumYyMinusSumXY[i]));
                OtherEigenValue[i] = sumXxPlusSumYyDivTwo[i] - (int)Math.Sqrt(((sumXxPlusSumYyRootTwo[i] / 4) - sumXxMultimplySumYyMinusSumXY[i]));

                //if (i % (width * 4 - 4) == 0)
                //{
                //    i += 4;
                //}
                i += 4;
            }

            int j = (width * 4) + 4;

            while (j < xX.Length - (width * 4) - 4)
            {
                                          // left                                          // right
                if (OtherEigenValue[j] > OtherEigenValue[j - 4] && OtherEigenValue[j] > OtherEigenValue[j + 4] &&
                    // top                                             // bot
                    OtherEigenValue[j] > OtherEigenValue[j - (width * 4)] && OtherEigenValue[j] > OtherEigenValue[j + (width * 4)] &&
                    // top left                                                     // top right
                    OtherEigenValue[j] > OtherEigenValue[j - (width * 4) - 4] && OtherEigenValue[j] > OtherEigenValue[j - (width * 4) + 4] &&
                    // bot left                                                     // bot right
                    OtherEigenValue[j] > OtherEigenValue[j + (width * 4) - 4] && OtherEigenValue[j] > OtherEigenValue[j + (width * 4) + 4])
                {
                    pixelBuffer[j] = 0;
                    pixelBuffer[j + 1] = 0;
                    pixelBuffer[j + 2] = 255;
                    pixelBuffer[j + 3] = 255;
                }

                /*if (eigenValue[i] > eigenValue[i - 4] && eigenValue[i] > eigenValue[i + 4] &&
                    // top                                             // bot
                    eigenValue[i] > eigenValue[i - (width * 4)] && eigenValue[i] > eigenValue[i + (width * 4)] &&
                    // top left                                                     // top right
                    eigenValue[i] > eigenValue[i - (width * 4) - 4] && eigenValue[i] > eigenValue[i - (width * 4) + 4] &&
                    // bot left                                                     // bot right
                    eigenValue[i] > eigenValue[i + (width * 4) - 4] && eigenValue[i] > eigenValue[i + (width * 4) + 4])
                {
                    pixelBuffer[i] = 0;
                    pixelBuffer[i + 1] = 0;
                    pixelBuffer[i + 2] = 255;
                    pixelBuffer[i + 3] = 255;
                } 
                else
                {
                    int t = (pixelBuffer[i] + pixelBuffer[i + 1] + pixelBuffer[i + 2]) / 3;
                    pixelBuffer[i] = (byte)t;
                    pixelBuffer[i + 1] = (byte)t;
                    pixelBuffer[i + 2] = (byte)t;
                    pixelBuffer[i + 3] = 255;
                }*/

                //if (j % (width * 4 - 4) == 0)
                //{
                //    j += 4;
                //}

                j += 4;
            }

            //Create new bitmap which will hold the processed data
            Bitmap resultImage = new Bitmap(width, height);

            //Lock bits into system memory
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            //Copy from byte array that holds processed data to bitmap
            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);

            //Unlock bits from system memory
            resultImage.UnlockBits(resultData);

            //Return processed image
            return resultImage;
        }

        private static Bitmap Sobel(Bitmap sourceImage, double[,] xkernel, double[,] ykernel, double factor = 1, int bias = 0, bool grayscale = false)
        {
            //Image dimensions stored in variables for convenience
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            //Lock source image bits into system memory
            BitmapData srcData = sourceImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            //Get the total number of bytes in your image - 32 bytes per pixel x image width x image height -> for 32bpp images
            int bytes = srcData.Stride * srcData.Height;

            //Create byte arrays to hold pixel information of your image
            byte[] pixelBuffer = new byte[bytes];
            byte[] resultBuffer = new byte[bytes];

            //Get the address of the first pixel data
            IntPtr srcScan0 = srcData.Scan0;

            //Copy image data to one of the byte arrays
            Marshal.Copy(srcScan0, pixelBuffer, 0, bytes);

            //Unlock bits from system memory -> we have all our needed info in the array
            sourceImage.UnlockBits(srcData);

            //Convert your image to grayscale if necessary
            if (grayscale == true)
            {
                float rgb = 0;
                for (int i = 0; i < pixelBuffer.Length; i += 4)
                {
                    rgb = pixelBuffer[i] * .21f;
                    rgb += pixelBuffer[i + 1] * .71f;
                    rgb += pixelBuffer[i + 2] * .071f;
                    pixelBuffer[i] = (byte)rgb;
                    pixelBuffer[i + 1] = pixelBuffer[i];
                    pixelBuffer[i + 2] = pixelBuffer[i];
                    pixelBuffer[i + 3] = 255;
                }
            }

            //Create variable for pixel data for each kernel
            double xr = 0.0;
            double xg = 0.0;
            double xb = 0.0;
            double yr = 0.0;
            double yg = 0.0;
            double yb = 0.0;
            double rt = 0.0;
            double gt = 0.0;
            double bt = 0.0;

            //This is how much your center pixel is offset from the border of your kernel
            //Sobel is 3x3, so center is 1 pixel from the kernel border
            int filterOffset = 1;
            int calcOffset = 0;
            int byteOffset = 0;

            //Start with the pixel that is offset 1 from top and 1 from the left side
            //this is so entire kernel is on your image
            for (int OffsetY = filterOffset; OffsetY < height - filterOffset; OffsetY++)
            {
                for (int OffsetX = filterOffset; OffsetX < width - filterOffset; OffsetX++)
                {
                    //reset rgb values to 0
                    xr = xg = xb = yr = yg = yb = 0;
                    rt = gt = bt = 0.0;

                    //position of the kernel center pixel
                    byteOffset = OffsetY * srcData.Stride + OffsetX * 4;

                    //kernel calculations
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + filterX * 4 + filterY * srcData.Stride;
                            xb += (double)(pixelBuffer[calcOffset]) * xkernel[filterY + filterOffset, filterX + filterOffset];
                            xg += (double)(pixelBuffer[calcOffset + 1]) * xkernel[filterY + filterOffset, filterX + filterOffset];
                            xr += (double)(pixelBuffer[calcOffset + 2]) * xkernel[filterY + filterOffset, filterX + filterOffset];
                            yb += (double)(pixelBuffer[calcOffset]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                            yg += (double)(pixelBuffer[calcOffset + 1]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                            yr += (double)(pixelBuffer[calcOffset + 2]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                        }
                    }

                    //total rgb values for this pixel
                    bt = Math.Sqrt((xb * xb) + (yb * yb));
                    gt = Math.Sqrt((xg * xg) + (yg * yg));
                    rt = Math.Sqrt((xr * xr) + (yr * yr));

                    //set limits, bytes can hold values from 0 up to 255;
                    if (bt > 255) bt = 255;
                    else if (bt < 0) bt = 0;
                    if (gt > 255) gt = 255;
                    else if (gt < 0) gt = 0;
                    if (rt > 255) rt = 255;
                    else if (rt < 0) rt = 0;

                    //set new data in the other byte array for your image data
                    resultBuffer[byteOffset] = (byte)(bt);
                    resultBuffer[byteOffset + 1] = (byte)(gt);
                    resultBuffer[byteOffset + 2] = (byte)(rt);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }

            //Create new bitmap which will hold the processed data
            Bitmap resultImage = new Bitmap(width, height);

            //Lock bits into system memory
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            //Copy from byte array that holds processed data to bitmap
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);

            //Unlock bits from system memory
            resultImage.UnlockBits(resultData);

            //Return processed image
            return resultImage;
        }
                
        public void Tallozas()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                originalImage.Image = new Bitmap(open.FileName);
                imageWidth = originalImage.Width;
                imageHeight = originalImage.Height;
            }
        }

        public void SaveToFile(Image img, string name)
        {
            Bitmap bmp = new Bitmap(imageWidth, imageHeight);
            newImage.DrawToBitmap(bmp, new Rectangle(0, 0, imageWidth, imageHeight));
            bmp.Save(name, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}

public class ConvMatrix
{
    public int TopLeft = 1, TopMid = 2, TopRight = 1;
    public int MidLeft = 2, Pixel = 4, MidRight = 2;
    public int BottomLeft = 1, BottomMid = 2, BottomRight = 1;
    public int Factor = 16;
    public int Offset = 0;

    public ConvMatrix Box()
    {
        ConvMatrix m = new ConvMatrix();

        TopLeft = 1;
        TopMid = 1;
        TopRight = 1;

        MidLeft = 1;
        Pixel = 1;
        MidRight = 1;

        BottomLeft = 1;
        BottomMid = 1;
        BottomRight = 1;

        Factor = 9;
        Offset = 0;

        return m;
    }

    public ConvMatrix Gauss()
    {
        ConvMatrix m = new ConvMatrix();

        TopLeft = 1;
        TopMid = 2;
        TopRight = 1;

        MidLeft = 2;
        Pixel = 4;
        MidRight = 2;

        BottomLeft = 1;
        BottomMid = 2;
        BottomRight = 1;

        Factor = 16;
        Offset = 0;

        return m;
    }

    public void SetAll(int nVal)
    {
        TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight =
                  BottomLeft = BottomMid = BottomRight = nVal;
    }
}