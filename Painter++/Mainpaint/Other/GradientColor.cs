using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mainpaint.Other
{
    public enum GradientType
    {
        Rectangular, Circular, Linear, Other
    }

    public struct GradientInformation
    {
        public Brush brush;
        public GradientType type;
        public List<Color> colors;
        public float[] positions;
    }

    public partial class GradientColor : UserControl
    {
        public GradientColor()
        {
            InitializeComponent();
        }

        public override string Text
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        public Color _GradientColor
        {
            get
            {
                return pictureBox1.BackColor;
            }
            set
            {
                pictureBox1.BackColor = value;
            }
        }

        private void GradientColor_Layout(object sender, LayoutEventArgs e)
        {
            base.Dock = DockStyle.Top;
        }

        private void GradientColor_Click(object sender, EventArgs e)
        {
            GradientColorHolder gch = (GradientColorHolder)this.Parent.Parent;
            gch.Select(this);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            GradientColor_Click(this, e);
        }
    }
}
