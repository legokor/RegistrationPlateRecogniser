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

            Mat image = new Mat(path);
            Mat grayImage = new Mat();
            Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);
            WhiteImage = new Mat();

            Cv2.Threshold(grayImage, WhiteImage, 180, 255, ThresholdTypes.Binary);
            Erode(WhiteImage, 1);

            //kontúrok megtalálsa
            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(WhiteImage, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            

            var componentCount = 0;
            var contourIndex = 0;
            Mat drawing = Mat.Zeros(WhiteImage.Size(), MatType.CV_8UC1);
            while (contourIndex >= 0)
            {
                Cv2.DrawContours(drawing, contours, contourIndex, Scalar.All(componentCount + 15), -1, LineTypes.Link8, hierarchy, int.MaxValue);
                componentCount++;
                contourIndex = hierarchy[contourIndex].Next;
            }
            Erode(drawing, 10);
            Dilate(drawing, 10);

            Cv2.ImWrite(@"D:\Visual_studio\RegistrationPlateRecognizer\Pictures\car_Drawing.jpg", drawing);
            Rect plate = new Rect();
            bool find = false;

            for (int y = image.Height - 1; y >= 0; y--)
            {
                for (int x = image.Width - 1; x >= 0; x--)
                {
                    if(drawing.At<byte>(y, x) != 0)
                    {
                        Cv2.FloodFill(drawing, new OpenCvSharp.Point(x, y), new Scalar(255), out plate, null, null, 4);
                        find = true;
                        break;
                    }
                }
                if (find) break;
            }

            Cv2.Rectangle(image, plate, new Scalar(255, 255, 255), -1);
            pictureBox1.Image = new Bitmap (OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image));
            Cv2.ImWrite(@"D:\Visual_studio\RegistrationPlateRecognizer\Pictures\hidePlate.jpg", image);

            /*
            

             // draw the rotated rect
             cv::Point2f corners[4];
             boundingBox.points(corners);
             cv::line(drawing, corners[0], corners[1], cv::Scalar(255, 255, 255));
             cv::line(drawing, corners[1], corners[2], cv::Scalar(255, 255, 255));
             cv::line(drawing, corners[2], corners[3], cv::Scalar(255, 255, 255));
             cv::line(drawing, corners[3], corners[0], cv::Scalar(255, 255, 255));

             // display
             cv::imshow("input", input);
             cv::imshow("drawing", drawing);
             cv::waitKey(0);

             cv::imwrite("rotatedRect.png", drawing);

     */

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
