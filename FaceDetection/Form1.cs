using Alturos.Yolo;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace FaceDetection
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "PNG|*.png|JPEG|*.jpeg" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void btnDetect_Click(object sender, EventArgs e)
        {
            var configurationDetector = new ConfigurationDetector();
            var config = configurationDetector.Detect();
            using (var yoloWrapper = new YoloWrapper(config))
            {
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, ImageFormat.Png);
                var items = yoloWrapper.Detect(ms.ToArray());
                foreach (var item in items)
                {
                    var x = item.X;
                    var y = item.Y;
                    var width = item.Width;
                    var height = item.Height;
                    var type = item.Type; 

                    Pen pen = new Pen(Color.Red);
                    Graphics g = pictureBox1.CreateGraphics();
                    g.DrawRectangle(pen, x, y, width, height);
                }

                dataGridView1.DataSource = items;
            }
        }
    }
}
