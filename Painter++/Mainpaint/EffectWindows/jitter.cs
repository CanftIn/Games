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
    public partial class jitter : Form
    {
        Form1 parent;
        Bitmap original;
        public jitter(Form1 Parent)
        {
            InitializeComponent();
            parent = Parent;
            original = (Bitmap)parent.Space.SelectedLayerW.LayerImage.Clone();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Bitmap temp = Effects.ColorEffects.AddJitter(original, (int)numericUpDown1.Value);

            parent.Space.SelectedLayerW.Clean();
            using (Graphics g = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                g.DrawImage(temp, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.Space.SelectedLayerW.Clean();
            using (Graphics g = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                g.DrawImage(original, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap temp = Effects.ColorEffects.AddJitter(original, (int)numericUpDown1.Value);

            parent.Space.SelectedLayerW.Clean();
            using (Graphics g = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                g.DrawImage(temp, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }
    }
}
