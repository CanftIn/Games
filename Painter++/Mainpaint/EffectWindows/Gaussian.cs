using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mainpaint.EffectWindows
{
    public partial class GaussianWindow : Form
    {
        Bitmap original;
        Form1 parent;
        public GaussianWindow(Form1 pt)
        {
            parent = pt;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Blur.Gaussian gx = new Blur.Gaussian();
            Bitmap bmp = gx.ProcessImage(trackBar1.Value, original);
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(bmp, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void Gaussian_Load(object sender, EventArgs e)
        {
            original = (Bitmap)parent.Space.SelectedLayerW.LayerImage.Clone();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Blur.Gaussian gx = new Blur.Gaussian();
            Bitmap bmp = gx.ProcessImage(trackBar1.Value, original);
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(bmp, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(original, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }
    }
}
