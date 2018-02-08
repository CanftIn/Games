using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mainpaint
{
    public partial class LayerRow : UserControl
    {
        private bool LayerVisible = true;
        private bool Selected_layer = false;
        public LayerRow()
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

        public bool layerVisible
        {
            get
            {
                return LayerVisible;
            }
            set
            {
                LayerVisible = value;
                UpdateVisibility();
            }
        }

        private void UpdateVisibility()
        {
            LayerContainer lc = (this.Parent != null) ? (LayerContainer)this.Parent.Parent : null;
            if (LayerVisible)
            {
                pictureBox1.Image = Mainpaint.Properties.Resources.layer_visibility;
                if (lc != null)
                {
                    lc.MasterWindow.Space.LayerChangeVisibility(this.Text, true);
                    lc.MasterWindow.DrawUpdater(true);
                }
            }
            else
            {
                if (lc != null)
                {
                    lc.MasterWindow.Space.LayerChangeVisibility(this.Text, false);
                    lc.MasterWindow.DrawUpdater(true);
                }
                pictureBox1.Image = null;
            }
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            this.Dock = DockStyle.Top;
        }

        public bool Selected
        {
            get
            {
                return Selected_layer;
            }
            set
            {
                Selected_layer = value;
                LayerSelect();
            }
        }

        public void LayerSelect()
        {
            if (!this.Selected)
                this.BackColor = Color.White;
            else
                this.BackColor = Color.FromArgb(255, 0, 100, 255);
        }

        private void LayerRow_Click(object sender, EventArgs e)
        {
            LayerContainer lc = (LayerContainer)this.Parent.Parent;
            lc.UnselectAll();
            lc.SelectedLayer = this;
            this.Selected = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!this.layerVisible)
            {
                this.layerVisible = true;
            }
            else
            {
                this.layerVisible = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            LayerRow_Click(this, e);
        }
    }
}
