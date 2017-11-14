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

namespace RegistrationPlateRecognizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Mat WhiteImage;
        string path = @"D:\Visual_studio\RegistrationPlateRecognizer\Pictures\car2.jpg";

        private void recognize_Click(object sender, EventArgs e)
        {

            //Mat image = new Mat(path);
            //Mat grayImage = new Mat();
            //Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);
            //WhiteImage = new Mat();

            //Cv2.Threshold(grayImage, WhiteImage, 180, 255, ThresholdTypes.Binary);
            //Erode(WhiteImage, 1);

            ////kontúrok megtalálsa
            //OpenCvSharp.Point[][] contours;
            //HierarchyIndex[] hierarchy;
            //Cv2.FindContours(WhiteImage, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);


            //var componentCount = 0;
            //var contourIndex = 0;
            //Mat drawing = Mat.Zeros(WhiteImage.Size(), MatType.CV_8UC1);
            //while (contourIndex >= 0)
            //{
            //    Cv2.DrawContours(drawing, contours, contourIndex, Scalar.All(componentCount + 15), -1, LineTypes.Link8, hierarchy, int.MaxValue);
            //    componentCount++;
            //    contourIndex = hierarchy[contourIndex].Next;
            //}
            //Erode(drawing, 10);
            //Dilate(drawing, 10);

            //Cv2.ImWrite(@"D:\Visual_studio\RegistrationPlateRecognizer\Pictures\car_Drawing.jpg", drawing);
            //Rect plate = new Rect();
            //bool find = false;

            //for (int y = image.Height - 1; y >= 0; y--)
            //{
            //    for (int x = image.Width - 1; x >= 0; x--)
            //    {
            //        if(drawing.At<byte>(y, x) != 0)
            //        {
            //            Cv2.FloodFill(drawing, new OpenCvSharp.Point(x, y), new Scalar(255), out plate, null, null, 4);
            //            find = true;
            //            break;
            //        }
            //    }
            //    if (find) break;
            //}

            //Cv2.Rectangle(image, plate, new Scalar(255, 255, 255), -1);
            //pictureBox1.Image = new Bitmap (OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image));
            //Cv2.ImWrite(@"D:\Visual_studio\RegistrationPlateRecognizer\Pictures\hidePlate.jpg", image);



            
            List<Rect> boundRect = new List<Rect>();
            using (Mat image = new Mat(path))
            using (Mat grayImage = new Mat())
            using (Mat sobelImage = new Mat())
            using (Mat tresholdImage = new Mat())
            {
                //make gray image
                Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);

                //gaussian blur
                //Cv2.GaussianBlur(grayImage, grayImage, new OpenCvSharp.Size(5, 5), 0);

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
                        if(appRect.Height > 20 && appRect.Width > 50 && appRect.Height/(double)appRect.Width < 0.5)
                            boundRect.Add(appRect);
                    }
                }

                foreach(Rect r in boundRect)
                {
                    Cv2.Rectangle(image, r, new Scalar(0, 0, 255), 2);
                }

                pictureBox1.Image = new Bitmap(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(sobelImage));
            }
          

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
    }
}
