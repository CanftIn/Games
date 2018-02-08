using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using Mainpaint.Other;

namespace Mainpaint
{
    public partial class GradientColorPicker : Form
    {
        int colorNumber = 1;

        Point P1_init = new Point(128, 128), CP1_init = new Point(79, 79);
        bool selectorActive = false, centerPointModifier = false;
        private Brush finished;
        private float[] positions;
        GradientType gtype;

        public GradientColorPicker()
        {
            InitializeComponent();
        }

        private void GradientColorPicker_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            gradientColorHolder1.AddColor(Color.Blue, "Color " + colorNumber);
            colorNumber++;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            gradientColorHolder1.Move(gradientColorHolder1.Selected.Text, MoveDirection.Up);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            gradientColorHolder1.Move(gradientColorHolder1.Selected.Text, MoveDirection.Down);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            gradientColorHolder1.RemoveColor(gradientColorHolder1.Selected.Text);
        }

        private void pictureBox5_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawEllipse(new Pen(new SolidBrush(Color.Black)), new Rectangle(P1_init.X, P1_init.Y, 4, 4));
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            selectorActive = true;
            P1_init = new Point(e.X - 2, e.Y - 2); 
        }

        private void pictureBox5_MouseUp(object sender, MouseEventArgs e)
        {
            selectorActive = false;
        }

        private void pictureBox5_MouseMove(object sender, MouseEventArgs e)
        {
            P1_init.X = e.X - 2; P1_init.Y = e.Y - 2;
            if (selectorActive)
            {
                Bitmap temp = (Bitmap)pictureBox5.Image.Clone();
                if (gradientColorHolder1.Selected != null) gradientColorHolder1.Selected._GradientColor = temp.GetPixel(e.X, e.Y);
                pictureBox5.Invalidate();
            }
        }

        private void GenerateGradient()
        {
            if (radioButton1.Checked)
            {
                LinearGradientBrush lgb = new LinearGradientBrush(pictureBox1.ClientRectangle, Color.Black, Color.Black, 0f);
                ColorBlend cb = new ColorBlend();
                float max = gradientColorHolder1.Colors.Count;
                cb.Positions = new float[(int)max];
                cb.Colors = new Color[(int)max];
                for (int x = 0; x < max; x++)
                {
                    cb.Positions[x] = (float)x / max;
                    cb.Colors[x] = gradientColorHolder1.Colors[x];
                }
                cb.Positions[(int)max - 1] = 1.0f;
                lgb.InterpolationColors = cb;

                positions = cb.Positions;

                Graphics gp = pictureBox1.CreateGraphics();
                gp.FillRectangle(lgb, pictureBox1.ClientRectangle);
                gp.Dispose();

                finished = (LinearGradientBrush)lgb.Clone();

                lgb.Dispose();

                gtype = GradientType.Linear;
            }
            if (radioButton2.Checked)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(pictureBox2.ClientRectangle);
                PathGradientBrush pgb = new PathGradientBrush(gp);
                pgb.CenterColor = gradientColorHolder1.Colors[0];

                ColorBlend cb = new ColorBlend();
                float max = gradientColorHolder1.Colors.Count;
                cb.Positions = new float[(int)max];
                cb.Colors = new Color[(int)max];
                for (int x = 0; x < max; x++)
                {
                    cb.Positions[x] = (float)x / max;
                    cb.Colors[x] = gradientColorHolder1.Colors[x];
                }
                cb.Positions[(int)max - 1] = 1.0f;

                pgb.CenterPoint = CP1_init;

                positions = cb.Positions;

                pgb.InterpolationColors = cb;

                Graphics gn = pictureBox2.CreateGraphics();
                gn.Clear(SystemColors.Control);
                gn.FillRectangle(pgb, pictureBox2.ClientRectangle);

                finished = (PathGradientBrush)pgb.Clone();

                gn.Dispose();
                gp.Dispose();
                pgb.Dispose();
                gtype = GradientType.Circular;
            }
            if (radioButton3.Checked)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddRectangle(pictureBox2.ClientRectangle);
                PathGradientBrush pgb = new PathGradientBrush(gp);
                pgb.CenterColor = gradientColorHolder1.Colors[0];

                ColorBlend cb = new ColorBlend();
                float max = gradientColorHolder1.Colors.Count;
                cb.Positions = new float[(int)max];
                cb.Colors = new Color[(int)max];
                for (int x = 0; x < max; x++)
                {
                    cb.Positions[x] = (float)x / max;
                    cb.Colors[x] = gradientColorHolder1.Colors[x];
                }
                cb.Positions[(int)max - 1] = 1.0f;

                pgb.CenterPoint = CP1_init;

                positions = cb.Positions;

                pgb.InterpolationColors = cb;

                Graphics gn = pictureBox2.CreateGraphics();
                gn.Clear(SystemColors.Control);
                gn.FillRectangle(pgb, pictureBox2.ClientRectangle);

                finished = (PathGradientBrush)pgb.Clone();

                gn.Dispose();
                gp.Dispose();
                pgb.Dispose();
                gtype = GradientType.Rectangular;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateGradient();
        }

        //center point PGB modifications

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            centerPointModifier = false;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {

            if (centerPointModifier)
            {
                CP1_init.X = e.X - 2; CP1_init.Y = e.Y - 2;
                pictureBox2.Invalidate();
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            centerPointModifier = true;
        }

        public Brush GradientBrush
        {
            get
            {
                return finished;
            }
        }

        public float[] Positions
        {
            get
            {
                return positions;
            }
        }

        public List<Color> Colors
        {
            get
            {
                return gradientColorHolder1.Colors;
            }
        }

        public GradientType GType
        {
            get
            {
                return gtype;
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Graphics gnp = e.Graphics;
            
            if (radioButton2.Checked)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(pictureBox2.ClientRectangle);
                PathGradientBrush pgb = new PathGradientBrush(gp);

                ColorBlend cb = new ColorBlend();
                float max = gradientColorHolder1.Colors.Count;
                cb.Positions = new float[(int)max];
                cb.Colors = new Color[(int)max];
                for (int x = 0; x < max; x++)
                {
                    cb.Positions[x] = (float)x / max;
                    cb.Colors[x] = gradientColorHolder1.Colors[x];
                }
                cb.Positions[(int)max - 1] = 1.0f;

                positions = cb.Positions;

                pgb.CenterPoint = new PointF(CP1_init.X, CP1_init.Y);

                pgb.InterpolationColors = cb;

                gnp.FillRectangle(pgb, pictureBox2.ClientRectangle);

                finished = (PathGradientBrush)pgb;
            }
            if (radioButton3.Checked)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddRectangle(pictureBox2.ClientRectangle);
                PathGradientBrush pgb = new PathGradientBrush(gp);
                pgb.CenterColor = gradientColorHolder1.Colors[0];

                ColorBlend cb = new ColorBlend();
                float max = gradientColorHolder1.Colors.Count;
                cb.Positions = new float[(int)max];
                cb.Colors = new Color[(int)max];
                for (int x = 0; x < max; x++)
                {
                    cb.Positions[x] = (float)x / max;
                    cb.Colors[x] = gradientColorHolder1.Colors[x];
                }
                cb.Positions[(int)max - 1] = 1.0f;

                positions = cb.Positions;

                pgb.CenterPoint = new PointF(CP1_init.X, CP1_init.Y);

                pgb.InterpolationColors = cb;

                gnp.FillRectangle(pgb, pictureBox2.ClientRectangle);

                finished = (PathGradientBrush)pgb;
            }

            gnp.DrawEllipse(new Pen(new SolidBrush(Color.Black)), new Rectangle(CP1_init.X, CP1_init.Y, 4, 4));
        }
    }
}
