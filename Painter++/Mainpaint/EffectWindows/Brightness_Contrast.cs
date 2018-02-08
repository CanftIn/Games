using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Mainpaint.EffectWindows
{
    public partial class Brightness_Contrast : Form
    {
        Bitmap original;
        Bitmap appliedBr, appliedCtr;
        Form1 parent;

        public Brightness_Contrast(Form1 p)
        {
            parent = p;
            InitializeComponent();
        }

        private void UpdateBrightness()
        {
            if(trackBar2.Value != 0)
                appliedBr = Effects.ColorEffects.Brightness(appliedCtr, trackBar1.Value);
            else
                appliedBr = Effects.ColorEffects.Brightness(original, trackBar1.Value);

            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(appliedBr, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void UpdateContrast()
        {
            if(trackBar1.Value != 0)
                Effects.ColorEffects.Contrast(appliedBr, trackBar2.Value, out appliedCtr);
            else
                Effects.ColorEffects.Contrast(original, trackBar2.Value, out appliedCtr);
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(appliedCtr, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void Brightness_Contrast_Load(object sender, EventArgs e)
        {
            original = (Bitmap)parent.Space.SelectedLayerW.LayerImage.Clone();
            appliedBr = (Bitmap)original.Clone();
            appliedCtr = (Bitmap)original.Clone();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(original, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateBrightness();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            UpdateContrast();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap final = new Bitmap(original.Width, original.Height);

            if (trackBar1.Value == 0 && trackBar2.Value != 0)
                final = appliedCtr;
            else if (trackBar1.Value != 0 && trackBar2.Value == 0)
                final = appliedBr;
            else
                final = Effects.ColorEffects.Brightness(original, trackBar1.Value); Effects.ColorEffects.Contrast(final, trackBar2.Value, out final);

            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(final, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }
    }
}
