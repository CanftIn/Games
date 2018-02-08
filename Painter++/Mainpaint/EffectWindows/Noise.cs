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
    public partial class Noise : Form
    {
        Bitmap Original;
        Form1 parent;

        public Noise(Form1 Parent)
        {
            InitializeComponent();
            parent = Parent;
            Original = (Bitmap)parent.Space.SelectedLayerW.LayerImage.Clone();
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.Space.SelectedLayerW.Clean();
            using (Graphics g = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                g.DrawImage(Original, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Bitmap temp = Effects.ColorEffects.AddNoise(Original, (int)numericUpDown1.Value);
            using (Graphics g = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                g.DrawImage(temp, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap temp = Effects.ColorEffects.AddNoise(Original, (int)numericUpDown1.Value);

            parent.Space.SelectedLayerW.Clean();
            using (Graphics g = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                g.DrawImage(temp, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }
    }
}
