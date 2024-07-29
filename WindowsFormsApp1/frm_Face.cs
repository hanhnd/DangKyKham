using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;

using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;
using System.Runtime;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace DangKyKhamTuDong
{
    public partial class frm_Face : Form
    {
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;

       

        private void btn_Start_Click(object sender, EventArgs e)
        {
            //Initialize the capture device
            grabber = new Capture();
            grabber.QueryFrame();
            //Initialize the FrameGraber event
            //System.Threading.Thread.Sleep(3000);
            Application.Idle += new EventHandler(FrameGrabber);
            btn_Start.Enabled = false;
        }

        void FrameGrabber(object sender, EventArgs e)
        {
            try
            {
               
                label3.Text = "0";
                //label4.Text = "";
                NamePersons.Add("");


                //Get the current frame form capture device
                currentFrame = grabber.QueryFrame().Resize(350, 300, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Convert it to Grayscale
                gray = currentFrame.Convert<Gray, Byte>();

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
              face,
              1.2,
              1,
              Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
              new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {

                    t = t + 1;
                    if (f.rect.Width >= 200)
                    {
                        result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                        //draw the face detected in the 0th (gray) channel with blue color
                        currentFrame.Draw(f.rect, new Bgr(Color.Green), 2);


                        //currentFrame.Draw("",ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));
                        ib_face.Image = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); 

                        //if (trainingImages.ToArray().Length != 0)
                        //{
                        //    //TermCriteria for face recognition with numbers of trained images like maxIteration
                        //    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                        //    //Eigen face recognizer
                        //    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                        //       trainingImages.ToArray(),
                        //       labels.ToArray(),
                        //       3000,
                        //       ref termCrit);

                        //    name = recognizer.Recognize(result);

                        //    //Draw the label for each face detected and recognized
                        //    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));

                        //}

                        //NamePersons[t - 1] = name;
                        //NamePersons.Add("");


                        //Set the number of faces detected on the scene
                        //label3.Text = facesDetected[0].Length.ToString();
                        // Cap_face();
                    }

                    //button2_Click(null, null);
                    
                    //Set the region of interest on the faces

                    //gray.ROI = f.rect;
                    //MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                    //   eye,
                    //   1.1,
                    //   10,
                    //   Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                    //   new Size(20, 20));
                    //gray.ROI = Rectangle.Empty;

                    //foreach (MCvAvgComp ey in eyesDetected[0])
                    //{
                    //    Rectangle eyeRect = ey.rect;
                    //    eyeRect.Offset(f.rect.X, f.rect.Y);
                    //    currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                    //}
                     

                }
                t = 0;

                //Names concatenation of persons recognized
                //for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                //{
                //    names = names + NamePersons[nnn] + ", ";
                //}
                //Show the faces procesed and recognized
                imageBoxFrameGrabber.Image = currentFrame;
                //label4.Text = names;
                names = "";
                //Clear the list(vector) of names
                NamePersons.Clear();
            }
            catch(Exception ex)
            {

            }
            
        }

        string name, names = null;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            ib_face.Image = null;
            btn_Start.Enabled = true;
        }

        public frm_Face()
        {
            InitializeComponent();
            //Load haarcascades for face detection
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade(Application.StartupPath + "/haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show("Nothing in binary database, please add face.", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, imageBoxFrameGrabber.Width - 80, imageBoxFrameGrabber.Height);
            Region rg = new Region(gp);
            imageBoxFrameGrabber.Region = rg;
            imageBoxFrameGrabber.Left = imageBoxFrameGrabber.Left + 40;
        }
        private void Cap_face()
        {
            try
            {
                //textBox1.Text = string.Format("{0:YYMMddHHmmss}", DateTime.Now);
                string strName_tmp = string.Format("{0:YYMMddHHmmss}", DateTime.Now);
                //if (string.IsNullOrEmpty(textBox1.Text))
                //{
                //    MessageBox.Show("Please input your name to add face");
                //}
                //else
                //{
                //Trained face counter
                ContTrain = ContTrain + 1;

                    //Get a gray frame from capture device
                    gray = grabber.QueryGrayFrame().Resize(350, 300, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    //Face Detector
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                    face,
                    1.2,
                    10,
                    Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                    new Size(20, 20));

                    //Action for each element detected
                    foreach (MCvAvgComp f in facesDetected[0])
                    {
                        TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                        break;
                    }

                    //resize face detected image for force to compare the same size with the 
                    //test image with cubic interpolation type method
                    TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    trainingImages.Add(TrainedFace);
                    labels.Add(strName_tmp);

                    //Show face added in gray scale
                    ib_face.Image = TrainedFace;

                    //Write the number of triained faces in a file text for further load
                    File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                    //Write the labels of triained faces in a file text for further load
                    for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                    {
                        trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                        File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                    }

                    //MessageBox.Show(strName_tmp + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Idle -= new EventHandler(FrameGrabber);
                //}
            }
            catch
            {
                MessageBox.Show("No face detected. Please check your camera or stand closer.", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
      

    }

   
}
