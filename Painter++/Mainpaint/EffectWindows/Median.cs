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
    public partial class Median : Form
    {
        Bitmap original;
        Form1 parent;
        public Median(Form1 pt)
        {
            parent = pt;
            InitializeComponent();
        }

        private void UpdateImage()
        {
            Bitmap newBmp = new Bitmap(original.Width, original.Height);
            newBmp = Effects.ColorEffects.MedianFilter(original, trackBar1.Value);
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(newBmp, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Median_Load(object sender, EventArgs e)
        {
            original = (Bitmap)parent.Space.SelectedLayerW.LayerImage.Clone();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
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
