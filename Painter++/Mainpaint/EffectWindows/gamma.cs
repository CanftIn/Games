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
    public partial class gamma : Form
    {
        Form1 parent;
        Bitmap original, affected;

        public gamma(Form1 p)
        {
            parent = p;
            InitializeComponent();
        }

        private void UpdateIM()
        {
            affected = Effects.ColorEffects.GammaAdjustment(original, new double[] { (double)trackBar1.Value, (double)trackBar1.Value, (double)trackBar1.Value });
            using (Graphics gdx = pictureBox1.CreateGraphics())
            {
                gdx.DrawImage(affected, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void gamma_Load(object sender, EventArgs e)
        {
            original = (Bitmap)parent.Space.SelectedLayerW.LayerImage.Clone();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateIM();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.Space.SelectedLayerW.Clean();
            using (Graphics gd = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gd.DrawImageUnscaled(affected, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }
    }
}
