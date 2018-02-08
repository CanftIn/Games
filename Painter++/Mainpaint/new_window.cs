using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mainpaint
{
    public partial class new_window : Form
    {

        MeasurementUnit current = MeasurementUnit.px;

        public new_window()
        {
            InitializeComponent();
        }

        public string ImageName
        {
            get
            {
                return textBox1.Text;
            }
        }

        public Size ImageSize
        {
            get
            {
                return new Size((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            }
        }

        public MeasurementUnit unit
        {
            get
            {
                return current;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void new_window_Load(object sender, EventArgs e)
        {
            
        }

        public enum MeasurementUnit
        {
            cm, inch, mm, px
        }

        public int _cm_to_px_X(int value)
        {
            double px = -1;
            using (Graphics g = this.CreateGraphics())
            {
                px = value * g.DpiX / 2.54d;
            }
            return (int)px;
        }

        public int _cm_to_px_Y(int value)
        {
            double px = -1;
            using (Graphics g = this.CreateGraphics())
            {
                px = value * g.DpiY / 2.54d;
            }
            return (int)px;
        }

        public int _px_to_cm_X(int value)
        {
            double px = -1;
            using (Graphics g = this.CreateGraphics())
            {
                px = value / g.DpiX * 2.54d;
            }
            return (int)px;
        }

        public int _px_to_cm_Y(int value)
        {
            double px = -1;
            using (Graphics g = this.CreateGraphics())
            {
                px = value / g.DpiY * 2.54d;
            }
            return (int)px;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "cm" && current == MeasurementUnit.px)
            {
                numericUpDown1.Value = _px_to_cm_X((int)numericUpDown1.Value);
                numericUpDown2.Value = _px_to_cm_X((int)numericUpDown2.Value);
                current = MeasurementUnit.cm;
            }
            else if (comboBox1.Text == "cm" && current == MeasurementUnit.inch)
            {
                numericUpDown1.Value = Convert.ToInt32((int)numericUpDown1.Value * 2.54d);
                numericUpDown2.Value = Convert.ToInt32((int)numericUpDown2.Value * 2.54d);
                current = MeasurementUnit.cm;
            }
            else if (comboBox1.Text == "cm" && current == MeasurementUnit.mm)
            {
                numericUpDown1.Value = (numericUpDown1.Value) / 1000;
                numericUpDown2.Value = (numericUpDown2.Value) / 1000;
                current = MeasurementUnit.cm;
            }

            if (comboBox1.Text == "inch" && current == MeasurementUnit.px)
            {
                numericUpDown1.Value = Convert.ToInt32(_px_to_cm_X((int)numericUpDown1.Value) / 2.54d);
                numericUpDown2.Value = Convert.ToInt32(_px_to_cm_X((int)numericUpDown2.Value) / 2.54d);
                current = MeasurementUnit.inch;
            }
            else if (comboBox1.Text == "inch" && current == MeasurementUnit.cm)
            {
                numericUpDown1.Value = Convert.ToInt32((int)numericUpDown1.Value / 2.54d);
                numericUpDown2.Value = Convert.ToInt32((int)numericUpDown2.Value / 2.54d);
                current = MeasurementUnit.inch;
            }
            else if (comboBox1.Text == "inch" && current == MeasurementUnit.mm)
            {
                numericUpDown1.Value = Convert.ToInt32((int)(numericUpDown1.Value / 1000) / 2.54d);
                numericUpDown2.Value = Convert.ToInt32((int)(numericUpDown2.Value / 1000) / 2.54d);
                current = MeasurementUnit.inch;
            }

            if (comboBox1.Text == "px" && current == MeasurementUnit.inch)
            {
                numericUpDown1.Value = Convert.ToInt32(_cm_to_px_X((int)((int)numericUpDown1.Value * 2.54d)));
                numericUpDown2.Value = Convert.ToInt32(_cm_to_px_X((int)((int)numericUpDown2.Value * 2.54d)));
                current = MeasurementUnit.px;
            }
            else if (comboBox1.Text == "px" && current == MeasurementUnit.cm)
            {
                numericUpDown1.Value = Convert.ToInt32(_cm_to_px_X((int)numericUpDown1.Value));
                numericUpDown2.Value = Convert.ToInt32(_cm_to_px_X((int)numericUpDown2.Value));
                current = MeasurementUnit.px;
            }
            else if (comboBox1.Text == "px" && current == MeasurementUnit.mm)
            {
                numericUpDown1.Value = Convert.ToInt32(_cm_to_px_X((int)(numericUpDown1.Value / 1000)));
                numericUpDown2.Value = Convert.ToInt32(_cm_to_px_X((int)(numericUpDown2.Value / 1000)));
                current = MeasurementUnit.px;
            }

            if (comboBox1.Text == "mm" && current == MeasurementUnit.inch)
            {
                numericUpDown1.Value = Convert.ToInt32(((int)numericUpDown1.Value * 2.54d) * 1000);
                numericUpDown2.Value = Convert.ToInt32(((int)numericUpDown2.Value * 2.54d) * 1000);
                current = MeasurementUnit.mm;
            }
            else if (comboBox1.Text == "mm" && current == MeasurementUnit.cm)
            {
                numericUpDown1.Value = Convert.ToInt32(numericUpDown1.Value * 1000);
                numericUpDown2.Value = Convert.ToInt32(numericUpDown2.Value * 1000);
                current = MeasurementUnit.mm;
            }
            else if (comboBox1.Text == "mm" && current == MeasurementUnit.px)
            {
                numericUpDown1.Value = Convert.ToInt32(_px_to_cm_X((int)(numericUpDown1.Value)) * 1000);
                numericUpDown2.Value = Convert.ToInt32(_px_to_cm_X((int)(numericUpDown2.Value)) * 1000);
                current = MeasurementUnit.mm;
            }
        }
    }
}
