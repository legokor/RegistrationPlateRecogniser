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
        Mat canny;

        private void recognize_Click(object sender, EventArgs e)
        {
            Cv2.NamedWindow("kép");
            Cv2.ResizeWindow("kép", 1280, 720);

            Mat image = new Mat(@"D:\Visual_studio\RegistrationPlateRecognizer\car.jpg");
            Mat grayImage = new Mat();
            Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);
            WhiteImage = new Mat();
            //Cv2.InRange(image, new Scalar(160, 160, 160), new Scalar(255, 255, 255), WhiteImage);

            Cv2.Threshold(grayImage, WhiteImage, 190, 255, ThresholdTypes.Binary);
            Dilate(WhiteImage, 2);

            //kontúrok megtalálsa
            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(WhiteImage, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

             
            /// Draw contours and find biggest contour (if there are other contours in the image, we assume the biggest one is the desired rect)
            // drawing here is only for demonstration!
            int biggestContourIdx = -1;
            float biggestContourArea = 0;
            Mat drawing = Mat.Zeros(WhiteImage.Size(), MatType.CV_8UC3);
            List<OpenCvSharp.Point[]> contoursList = contours.ToList();
            for (int i = 0; i < contours; i++)
            {
                cv::Scalar color = cv::Scalar(0, 100, 0);
                drawContours(drawing, contours, i, color, 1, 8, hierarchy, 0, cv::Point());
                Cv2.DrawContours(drawing, )

                float ctArea = cv::contourArea(contours[i]);
                if (ctArea > biggestContourArea)
                {
                    biggestContourArea = ctArea;
                    biggestContourIdx = i;
                }
            }

            // if no contour found
            if (biggestContourIdx < 0)
            {
                std::cout << "no contour found" << std::endl;
                return 1;
            }

            // compute the rotated bounding rect of the biggest contour! (this is the part that does what you want/need)
            cv::RotatedRect boundingBox = cv::minAreaRect(contours[biggestContourIdx]);
            // one thing to remark: this will compute the OUTER boundary box, so maybe you have to erode/dilate if you want something between the ragged lines



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
            //OpenCvSharp.Point[][] contours = Cv2.FindContoursAsArray(WhiteImage, RetrievalModes.External, ContourApproximationModes.ApproxSimple);


            Cv2.ImShow("kép", WhiteImage);
        }

        void Dilate(Mat image, int number)
        {
            for (int i = 0; i < number; i++)
            {
                Cv2.Dilate(image, image, new Mat());
            }
        }

        private void cannyButton_Click(object sender, EventArgs e)
        {
            Cv2.NamedWindow("canny");
            Cv2.ResizeWindow("canny", 1280, 720);
            canny = new Mat();
            Cv2.Canny(WhiteImage, canny, 1000, 500);
            Dilate(canny, 1);
            Cv2.ImShow("canny", canny);
        }
    }
}
