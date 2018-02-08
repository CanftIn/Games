using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mainpaint.Diagnostics;

namespace Mainpaint.EffectWindows
{
    public partial class GradientDetect : Form
    {
        Form1 parent;
        Bitmap original;
        public GradientDetect(Form1 pt)
        {
            parent = pt;
            InitializeComponent();
        }

        private void UpdateImage()
        {
            Bitmap bmp = new Bitmap(original.Width, original.Height);
            EdgeFilterMode EFM = EdgeFilterMode.Sharpen;
            switch (comboBox1.Text)
            {
                case "Sharpen":
                    EFM = EdgeFilterMode.Sharpen;
                    break;
                case "Mono":
                    EFM = EdgeFilterMode.EdgeDetectMono;
                    break;
                case "Gradient":
                    EFM = EdgeFilterMode.EdgeDetectionGradient;
                    break;
                case "Gradient sharpen":
                    EFM = EdgeFilterMode.SharpenGradient;
                    break;
                default:
                    EFM = EdgeFilterMode.Sharpen;
                    break;
            }
            bmp = EdgeDetection.GradientEdgeDetection(original, EFM, ((radioButton1.Checked) ? DeriviationLevel.First : DeriviationLevel.Second), new float[] { (float)trackBar1.Value / 10, (float)trackBar2.Value / 10, (float)trackBar3.Value / 10 }, (byte)trackBar4.Value);
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(bmp, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void GradientDetect_Load(object sender, EventArgs e)
        {
            original = (Bitmap)parent.Space.SelectedLayerW.LayerImage.Clone();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(original, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }
    }
}
