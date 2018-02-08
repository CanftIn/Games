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
    public partial class layerRename : Form
    {
        public layerRename(string oldName)
        {
            InitializeComponent();
            textBox1.Text = oldName;
        }

        public string LayerText
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        private void layerRename_Load(object sender, EventArgs e)
        {

        }
    }
}
