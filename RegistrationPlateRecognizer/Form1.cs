using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using System.Threading;

namespace RegistrationPlateRecognizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            show.Enabled = false;
            hide.Enabled = false;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        enum RecognizationModes { camera, image };
        string path = @"D:\Visual_studio\RegistrationPlateRecognizer\Pictures\car2.jpg";
        Mat image;
        VideoCapture capture;
        Thread thread;
        bool stop;

        void Recognize(object param)
        {
            stop = false;
            var mode = (RecognizationModes)param;
            List<Rect> boundRect = new List<Rect>();
            int sleepTime = 0;
            int frameCount = 0;

            if (mode == RecognizationModes.image)
                image = new Mat(path);
            else
            {
                image = new Mat();
                capture = new VideoCapture(0);
                double fps = capture.Fps;
                if (fps == 0) fps = 60;
                sleepTime = (int)Math.Round(1000 / fps);
            }
            using (Mat grayImage = new Mat())
            using (Mat sobelImage = new Mat())
            using (Mat tresholdImage = new Mat())
            {
                while (true)
                {
                    if (frameCount % 6 == 0)
                        boundRect.Clear();
                    if (mode == RecognizationModes.camera)
                        capture.Read(image);
                    //make gray image
                    Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);

                    //sobel filter to detect vertical edges
                    Cv2.Sobel(grayImage, sobelImage, MatType.CV_8U, 1, 0);
                    Cv2.Threshold(sobelImage, tresholdImage, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);

                    using (Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(10, 15)))
                    {
                        Cv2.MorphologyEx(tresholdImage, tresholdImage, MorphTypes.Close, element);
                        OpenCvSharp.Point[][] edgesArray = tresholdImage.Clone().FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxNone);
                        foreach (OpenCvSharp.Point[] edges in edgesArray)
                        {
                            OpenCvSharp.Point[] normalizedEdges = Cv2.ApproxPolyDP(edges, 17, true);
                            Rect appRect = Cv2.BoundingRect(normalizedEdges);
                            if (appRect.Height > 10 && appRect.Width > 20 && appRect.Height / (double)appRect.Width < 0.45)
                                boundRect.Add(appRect);
                        }
                    }

                    foreach (Rect r in boundRect)
                    {
                        Mat cut = new Mat(image, r);
                        OpenCvSharp.Size size = new OpenCvSharp.Size(25, 25);
                        Cv2.GaussianBlur(cut, cut, size, 10);
                        image.CopyTo(cut);
                    }

                    frameCount++;
                    pictureBox1.Image = new Bitmap(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image));
                    //Cv2.ImWrite(@"D:\Visual_studio\RegistrationPlateRecognizer\Pictures\hidePlate.jpg", image);
                    Cv2.WaitKey(sleepTime);
                    if (mode == RecognizationModes.image)
                        return;

                    if (stop)
                    {
                        pictureBox1.Image = null;
                        return;
                    }
                }
            }
        }


        private void recognize_Click(object sender, EventArgs e)
        {
            thread = new Thread(new ParameterizedThreadStart(Recognize));
            thread.Start(RecognizationModes.image);
        }

        void Dilate(Mat image, int number)
        {
            for (int i = 0; i < number; i++)
            {
                Cv2.Dilate(image, image, new Mat());
            }
        }

        void Erode(Mat image, int number)
        {
            for (int i = 0; i < number; i++)
            {
                Cv2.Erode(image, image, new Mat());
            }
        }

        private void show_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(path);
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {
            thread = new Thread(new ParameterizedThreadStart(Recognize));
            thread.Start(RecognizationModes.camera);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;
                show.Enabled = true;
                hide.Enabled = true;
                pictureBox1.Image = new Bitmap(path);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stop = true;
            thread.Join();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            stop = true;
        }
    }
}
