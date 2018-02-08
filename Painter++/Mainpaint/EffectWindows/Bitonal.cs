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
    public partial class Bitonal : Form
    {
        Bitmap original;
        Form1 parent;
        public Bitonal(Form1 pt)
        {
            parent = pt;
            InitializeComponent();
        }

        private void UpdateImage()
        {
            Bitmap bmp = new Bitmap(original.Width, original.Height);
            bmp = Effects.ColorEffects.Bitonal(original, pictureBox1.BackColor, pictureBox2.BackColor, trackBar1.Value * 10);
            using (Graphics gdx = Graphics.FromImage(parent.Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(bmp, new Point(0, 0));
            }
            parent.DrawUpdater(true);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void Bitonal_Load(object sender, EventArgs e)
        {
            original = (Bitmap)parent.Space.SelectedLayerW.LayerImage.Clone();
        }

        private void pictureBox1_BackColorChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void pictureBox2_BackColorChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.BackColor = cd.Color;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox2.BackColor = cd.Color;
            }
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
