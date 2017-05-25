using Emgu.CV;
using Emgu.CV.Structure;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.Util;
using Emgu.CV.CvEnum;
using System.Diagnostics;
using Emgu.CV.Util;
using System.Net.Http;

namespace FormMaker
{

    public partial class FormMaker : Form
    {
        int iLabelCount = 0;
        int iQuestions = 0;
        int iAnswers = 4;
        public FormMaker()
        {
            InitializeComponent();
        }

        public async Task PostConfigAsString(string data)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            response = await client.PostAsync("http://217.182.64.159:8080/api/config", new StringContent(data));

            if (!response.IsSuccessStatusCode)
            {
                lResultLabel.Text = "Config send...";
            }

        }


        private async void GenerateConfig()
        {
            string sConfigString = "";
            int counter = 0;
            var answers = pQuestions.Controls.OfType<ComboBox>().ToList();
            foreach (var answer in answers)
            {

                sConfigString += counter.ToString() + ":" + (Convert.ToInt16(answer.Text)-1).ToString() + "#";
                counter++;
            }
            await PostConfigAsString(sConfigString);

        }

        private void GenerateTest()
        {
            string sTestName = "arkusz.jpg";
            string sTestGraderPath = "";
            Image<Bgr, Byte> img = new Image<Bgr, byte>(612, 792, new Bgr(255, 255, 255));
            int iHalfWidth = img.Width / 2;
            int iHalfHeight = img.Height / 2;
            int iPointX = (iHalfWidth / 2) - 60;
            int iPointY = (iHalfHeight / 2) - 65;
            Cross2DF p = new Cross2DF(new PointF(iPointX, iPointY), 30, 30);

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    sTestGraderPath = fbd.SelectedPath + "\\"+ sTestName;
                }
            }


            for (int i = 0; i < iQuestions; i++)
            {
                int iWidth = 30;
                int iHeight = 20;
                int iX = 0;
                int iY = (iHalfHeight / 2) + (i * (iHeight + 7));

                //Create the font
                System.Drawing.Font font = new System.Drawing.Font("Arial", 3, FontStyle.Bold); //creates new font
                for (int j = 0; j < iAnswers; j++)
                {
                    iX = (iHalfWidth / 2) + (j * (iWidth + 7));
                    if (j == 0)
                    {
                        img.Draw((i + 1).ToString(), new Point(iX - 50, iY), new FontFace(), 0.5, new Bgr(90, 90, 90), 1);
                    }
                    RotatedRect rBox = new RotatedRect(new PointF(iX, iY), new SizeF(iWidth, iHeight), 0);

                    img.Draw(rBox, new Bgr(100, 100, 100), 2);

                }
            }

            img.Draw(p, new Bgr(Color.Black), 2);
            img.Save(sTestGraderPath);


        }


        private void bNew_Click(object sender, EventArgs e)
        {
            iQuestions = Decimal.ToInt32(questions.Value);
            pQuestions.Controls.Clear();

            for (int i = 0; i < iQuestions; ++i)
            {
                Label label = new Label();
                int count = pQuestions.Controls.OfType<Label>().ToList().Count;
                iLabelCount = count;
                label.Location = new Point(10, (25 * count) + 2);
                label.Size = new Size(70, 20);
                label.Name = "lQuestion_" + (count + 1);
                label.Text = "Pytanie: " + (count + 1);
                pQuestions.Controls.Add(label);

                ComboBox combobox = new ComboBox();
                count = pQuestions.Controls.OfType<ComboBox>().ToList().Count;
                combobox.Location = new System.Drawing.Point(90, 25 * count);
                combobox.Size = new System.Drawing.Size(80, 20);
                combobox.Name = "combobox_" + (count + 1);
                for (int j = 1; j <= iAnswers; ++j)
                {
                    combobox.Items.Add(j);
                }
                pQuestions.Controls.Add(combobox);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bGen_Click(object sender, EventArgs e)
        {

            GenerateTest();
            GenerateConfig();
            lResultLabel.Text = "Done!";
        }
    }
}
