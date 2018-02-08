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
    public partial class BalanceWindow : Form
    {
        Bitmap original;
        Form1 parent;
        public BalanceWindow(Form1 pr)
        {
            parent = pr;
            InitializeComponent();
        }

        private void BalanceWindow_Load(object sender, EventArgs e)
        {
            original = (Bitmap)parent.Space.SelectedLayerW.LayerImage.Clone(); //backup, just in case :D
        }

        private void UpdateImage()
        {
            Bitmap newBmp = new Bitmap(original.Width, original.Height);
            newBmp = Effects.ColorEffects.ColorBalance(original, new float[] { (float)trackBar1.Value, (float)trackBar2.Value, (float)trackBar3.Value });
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(newBmp, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)  //green
        {
            UpdateImage();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)  //red
        {
            UpdateImage();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)  //blue
        {
            UpdateImage();
        }

        private void button2_Click(object sender, EventArgs e)
        {

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
