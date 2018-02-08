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
    public partial class Pixelate : Form
    {
        Form1 target;
        Bitmap original;

        public Pixelate(Form1 parent)
        {
            InitializeComponent();
            target = parent;
            original = (Bitmap)target.Space.SelectedLayerW.LayerImage.Clone();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = Effects.ColorEffects.Pixelate(original, (int)(numericUpDown1.Value));
            using (Graphics gn = Graphics.FromImage(target.Space.SelectedLayerW.LayerImage))
            {
                gn.DrawImageUnscaled(bmp, new Point(0, 0));
            }
            target.DrawUpdater(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Graphics gn = Graphics.FromImage(target.Space.SelectedLayerW.LayerImage))
            {
                gn.DrawImageUnscaled(original, new Point(0, 0));
            }
            target.DrawUpdater(true);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Bitmap bmp = Effects.ColorEffects.Pixelate(original, (int)(numericUpDown1.Value));
            using (Graphics gn = Graphics.FromImage(target.Space.SelectedLayerW.LayerImage))
            {
                gn.DrawImageUnscaled(bmp, new Point(0, 0));
            }
            target.DrawUpdater(true);
        }

        private void Pixelate_Load(object sender, EventArgs e)
        {
            Bitmap bmp = Effects.ColorEffects.Pixelate(original, (int)(numericUpDown1.Value));
            using (Graphics gn = Graphics.FromImage(target.Space.SelectedLayerW.LayerImage))
            {
                gn.DrawImageUnscaled(bmp, new Point(0, 0));
            }
            target.DrawUpdater(true);
        }
    }
}
