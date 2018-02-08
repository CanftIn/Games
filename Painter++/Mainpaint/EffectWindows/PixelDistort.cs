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
    public partial class PixelDistort : Form
    {
        Form1 target;
        Bitmap original;

        public PixelDistort(Form1 parent)
        {
            InitializeComponent();
            target = parent;
            original = (Bitmap)target.Space.SelectedLayerW.LayerImage.Clone();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap tempBmp = Mainpaint.Effects.ColorEffects.PixelDistort(original, (int)numericUpDown1.Value);

            using (Graphics gp = Graphics.FromImage(target.Space.SelectedLayerW.LayerImage))
            {
                gp.DrawImageUnscaled(tempBmp, new Point(0, 0));
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Bitmap tempBmp = Mainpaint.Effects.ColorEffects.PixelDistort(original, (int)numericUpDown1.Value);

           using (Graphics gp = Graphics.FromImage(target.Space.SelectedLayerW.LayerImage))
           {
               gp.DrawImageUnscaled(tempBmp, new Point(0, 0));
           }
           target.DrawUpdater(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Graphics gp = Graphics.FromImage(target.Space.SelectedLayerW.LayerImage))
            {
                gp.DrawImageUnscaled(original, new Point(0, 0));
            }
            target.DrawUpdater(true);
        }
    }
}
