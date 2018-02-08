using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mainpaint
{
    public partial class ErodeFilter : Form
    {
        Bitmap original;
        Form1 parent;
        public ErodeFilter(Form1 pt)
        {
            parent = pt;
            InitializeComponent();
        }

        private void UpdateImage()
        {
            Bitmap newBmp = new Bitmap(original.Width, original.Height);
            newBmp = Effects.ColorEffects.ErodeDilateFilter(original, trackBar1.Value, new bool[] { checkBox1.Checked, checkBox2.Checked, checkBox3.Checked }, ((radioButton1.Checked) ? Effects.Morphology.Erode : Effects.Morphology.Dilate));
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(newBmp, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void ErodeFilter_Load(object sender, EventArgs e)
        {
            original = (Bitmap)parent.Space.SelectedLayerW.LayerImage.Clone();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(original, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
        }
    }
}
