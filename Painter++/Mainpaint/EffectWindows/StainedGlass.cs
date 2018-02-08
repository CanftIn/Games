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
    public partial class StainedGlass : Form
    {
        Form1 target;
        Bitmap original;

        public StainedGlass(Form1 parent)
        {
            InitializeComponent();
            target = parent;
            original = (Bitmap)target.Space.SelectedLayerW.LayerImage.Clone();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap temp  = new Bitmap(original.Width, original.Height);
            if(comboBox1.Text == "Euclidean")
                temp = Effects.ColorEffects.PerformStainedGlass(original, (int)numericUpDown1.Value, Convert.ToDouble(numericUpDown2.Value), Effects.ColorEffects.DistanceFormulaType.Euclidean, false, 0, Color.Black);
            if (comboBox1.Text == "Manhattan")
                temp = Effects.ColorEffects.PerformStainedGlass(original, (int)numericUpDown1.Value, Convert.ToDouble(numericUpDown2.Value), Effects.ColorEffects.DistanceFormulaType.Manhattan, false, 0, Color.Black);
            if (comboBox1.Text == "Chebyshev")
                temp = Effects.ColorEffects.PerformStainedGlass(original, (int)numericUpDown1.Value, Convert.ToDouble(numericUpDown2.Value), Effects.ColorEffects.DistanceFormulaType.Chebyshev, false, 0, Color.Black);

            using (Graphics gn = Graphics.FromImage(target.Space.SelectedLayerW.LayerImage))
            {
                gn.DrawImageUnscaled(temp, new Point(0, 0));
            }
            target.DrawUpdater(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(original.Width, original.Height);
            if (comboBox1.Text == "Euclidean")
                temp = Effects.ColorEffects.PerformStainedGlass(original, (int)numericUpDown1.Value, Convert.ToDouble(numericUpDown2.Value), Effects.ColorEffects.DistanceFormulaType.Euclidean, false, 0, Color.Black);
            if (comboBox1.Text == "Manhattan")
                temp = Effects.ColorEffects.PerformStainedGlass(original, (int)numericUpDown1.Value, Convert.ToDouble(numericUpDown2.Value), Effects.ColorEffects.DistanceFormulaType.Manhattan, false, 0, Color.Black);
            if (comboBox1.Text == "Chebyshev")
                temp = Effects.ColorEffects.PerformStainedGlass(original, (int)numericUpDown1.Value, Convert.ToDouble(numericUpDown2.Value), Effects.ColorEffects.DistanceFormulaType.Chebyshev, false, 0, Color.Black);

            using (Graphics gn = Graphics.FromImage(target.Space.SelectedLayerW.LayerImage))
            {
                gn.DrawImageUnscaled(temp, new Point(0, 0));
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
    }
}
