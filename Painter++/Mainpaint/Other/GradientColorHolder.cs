using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mainpaint;

namespace Mainpaint.Other
{
    public partial class GradientColorHolder : UserControl
    {
        public GradientColor Selected;
        private List<Color> _colors = new List<Color>();

        public GradientColorHolder()
        {
            InitializeComponent();
        }

        public void AddColor(Color gcl, string text)
        {
            GradientColor gc = new GradientColor();
            gc.Text = text;
            gc._GradientColor = gcl;
            panel1.Controls.Add(gc);
        }

        private void GeneratePalette()
        {
            _colors = new List<Color>();
            foreach (GradientColor gc in panel1.Controls)
            {
                _colors.Add(gc._GradientColor);
            }
        }

        public List<Color> Colors
        {
            get
            {
                GeneratePalette();
                return _colors;
            }
        }

        public GradientColor getColorByText(string text)
        {
            foreach (GradientColor gc in panel1.Controls)
            {
                if (gc.Text == text) return gc;
            }
            return null;
        }

        public void Move(string text, MoveDirection direction)
        {
            GradientColor gc = getColorByText(text);
            switch (direction)
            {
                case MoveDirection.Up:

                    if (panel1.Controls.IndexOf(gc) < panel1.Controls.Count - 1)
                    {
                        int cp = panel1.Controls.IndexOf(gc);
                        panel1.Controls.SetChildIndex(gc, cp + 1);
                    }
                    break;
                case MoveDirection.Down:
                    if (panel1.Controls.IndexOf(gc) > 0)
                    {
                        int cp = panel1.Controls.IndexOf(gc);
                        panel1.Controls.SetChildIndex(gc, cp - 1);
                    }
                    break;
            }
        }

        public void RemoveColor(string text)
        {
            if (getColorByText(text) != null)
            {
                GradientColor gc = getColorByText(text);
                panel1.Controls.Remove(gc);
            }
        }

        public void Select(GradientColor gc)
        {
            Selected = gc;
            foreach (GradientColor gcc in panel1.Controls)
            {
                gcc.BackColor = SystemColors.Control;
            }
            gc.BackColor = Color.FromArgb(255, 0, 100, 255);
        }
    }
}
