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
    public partial class new_layer : Form
    {
        public new_layer()
        {
            InitializeComponent();
        }

        public string LayerText
        {
            get
            {
                return textBox1.Text;
            }
        }

        public bool Visibility
        {
            get
            {
                return checkBox1.Checked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void new_layer_Load(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
           
        }
    }
}
