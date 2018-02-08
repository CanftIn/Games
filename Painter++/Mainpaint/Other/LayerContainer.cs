using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Mainpaint
{
    public enum MoveDirection
    {
        Up, Down
    }

    public partial class LayerContainer : UserControl
    {
        List<LayerRow> layers = new List<LayerRow>();
        Form1 mWindow;
        private LayerRow Selected = null;

        public LayerContainer()
        {
            InitializeComponent();
            LayerRow lr = new LayerRow();
            lr.Text = "Transparent Map";
            lr.Visible = false;
            layers.Add(lr);
        }

        public void UnselectAll()
        {
            foreach (LayerRow row in layers)
            {
                row.Selected = false;
            }
        }

        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                base.Dock = value;
            }
        }

        public Form1 MasterWindow
        {
            get
            {
                return mWindow;
            }
            set
            {
                mWindow = value;
            }
        }

        public void AddLayer(string name, bool visible)
        {
            LayerRow lr = new LayerRow();
            lr.Text = name;
            lr.layerVisible = visible;
            panel1.Controls.Add(lr);
            layers.Add(lr);
            MasterWindow.Space.CreateLayer(name, MasterWindow.SpaceSize, visible);
            this.SelectedLayer = layers[layers.Count - 1];
            this.SelectedLayer.Selected = true;
            this.SelectedLayer.LayerSelect();
        }

        public void DuplicateLayer()
        {
            int cli = MasterWindow.Space.Layers.IndexOf(MasterWindow.Space.SelectedLayerW);

            LayerRow lr = new LayerRow();
            lr.Text = MasterWindow.Space.SelectedLayerW.LayerName + "(copy)";
            lr.layerVisible = MasterWindow.Space.SelectedLayerW.Visible;
            panel1.Controls.Add(lr);
            panel1.Controls.SetChildIndex(lr, cli);
            layers.Insert(cli+1, lr);

            DrawingSpace.Layer ld = new DrawingSpace.Layer();
            ld.LayerImage = (Bitmap)MasterWindow.Space.SelectedLayerW.LayerImage.Clone();
            ld.LayerName = lr.Text;
            ld.Visible = lr.layerVisible;

            MasterWindow.Space.Layers.Insert(cli + 1, ld);
        }

        public LayerRow SelectedLayer
        {
            get
            {
                return Selected;
            }
            set
            {
                UnselectAll();
                Selected = value;
                if(Selected != null)
                    MasterWindow.Space.SelectedLayerW = MasterWindow.Space.Layers[layers.IndexOf(Selected)];
            }
        }

        public void RemoveLayer(string name)
        {
            for (int x = layers.Count - 1; x >= 0; x--)
            {
                if (layers[x].Text == name)
                {
                    layers.Remove(layers[x]);
                    panel1.Controls.Remove(layers[x]);
                    MasterWindow.Space.LayerRemove(layers[x].Text);
                    MasterWindow.DrawUpdater(true);
                }
            }
        }

        public void RemoveLayer(LayerRow lrn)
        {
            if(layers.Contains(lrn))
            {
                 layers.Remove(lrn);
                 panel1.Controls.Remove(lrn);
                 MasterWindow.Space.LayerRemove(lrn.Text);
                 MasterWindow.DrawUpdater(true);
            }
           
        }

        public void RemoveAllLayers()
        {
            layers = new List<LayerRow>();
            panel1.Controls.Clear();
        }

        public LayerRow FindLayer(string name)
        {
            LayerRow lr_r = null;
            foreach(LayerRow lr in layers)
            {
                if (lr.Text == name)
                {
                    lr_r = lr;
                }
            }
            return lr_r;
        }

        public void MoveLayer(string name, MoveDirection direction)
        {
            LayerRow lrn = FindLayer(name);
            switch (direction)
            {
                case MoveDirection.Up:
                    if (layers.IndexOf(lrn) < layers.Count - 1)
                    {
                        LayerRow ln = null;
                        ln = layers[layers.IndexOf(lrn) + 1];
                        layers[layers.IndexOf(lrn)] = ln;
                        layers[layers.IndexOf(ln) + 1] = lrn;

                        DrawingSpace.Layer lp;
                        lp = MasterWindow.Space.Layers[layers.IndexOf(lrn) + 1];
                        MasterWindow.Space.Layers[layers.IndexOf(lrn) + 1] = MasterWindow.Space.Layers[layers.IndexOf(lrn)];
                        MasterWindow.Space.Layers[layers.IndexOf(lrn)] = lp;

                        panel1.Controls.SetChildIndex(lrn, panel1.Controls.IndexOf(lrn) + 1);
                    }
                    break;
                case MoveDirection.Down:
                    if (layers.IndexOf(lrn) > 0)
                    {
                        LayerRow ls = null;
                        ls = layers[layers.IndexOf(lrn) - 1];
                        layers[layers.IndexOf(lrn)] = ls;
                        layers[layers.IndexOf(ls)] = lrn;

                        DrawingSpace.Layer lp;
                        lp = MasterWindow.Space.Layers[layers.IndexOf(lrn) - 1];
                        MasterWindow.Space.Layers[layers.IndexOf(lrn) - 1] = MasterWindow.Space.Layers[layers.IndexOf(lrn)];
                        MasterWindow.Space.Layers[layers.IndexOf(lrn)] = lp;

                        panel1.Controls.SetChildIndex(lrn, panel1.Controls.IndexOf(lrn) - 1);
                    }
                    break;
            }
            MasterWindow.Space.DrawnData();
            MasterWindow.DrawUpdater();
        }
       
        public void MoveLayer(LayerRow lrn, MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Up:
                    if (layers.IndexOf(lrn) < layers.Count - 1)
                    {
                        LayerRow ln = null;
                        ln = layers[layers.IndexOf(lrn) + 1];
                        layers[layers.IndexOf(lrn)] = ln;
                        layers[layers.IndexOf(ln) + 1] = lrn;

                        DrawingSpace.Layer lp;
                        lp = MasterWindow.Space.Layers[layers.IndexOf(lrn)];
                        MasterWindow.Space.Layers[layers.IndexOf(lrn)] = MasterWindow.Space.Layers[layers.IndexOf(lrn) - 1];
                        MasterWindow.Space.Layers[layers.IndexOf(lrn) - 1] = lp;

                        panel1.Controls.SetChildIndex(lrn, panel1.Controls.IndexOf(lrn) + 1);
                    }
                    break;
                case MoveDirection.Down:
                    if (layers.IndexOf(lrn) > 0)
                    {
                        LayerRow ls = null;
                        ls = layers[layers.IndexOf(lrn) - 1];
                        layers[layers.IndexOf(lrn)] = ls;
                        layers[layers.IndexOf(ls)] = lrn;

                        DrawingSpace.Layer lp;
                        lp = MasterWindow.Space.Layers[layers.IndexOf(lrn)];
                        MasterWindow.Space.Layers[layers.IndexOf(lrn)] = MasterWindow.Space.Layers[layers.IndexOf(lrn) + 1];
                        MasterWindow.Space.Layers[layers.IndexOf(lrn) + 1] = lp;

                        panel1.Controls.SetChildIndex(lrn, panel1.Controls.IndexOf(lrn) - 1);
                    }
                    break;
            }
            MasterWindow.Space.DrawnData();
            MasterWindow.DrawUpdater();
        }

        public void RenameLayer(string name, string newName)
        {
            LayerRow lr = FindLayer(name);
            lr.Text = newName;
        }

        public void RenameLayer(LayerRow lrn, string newName)
        {
            if(layers.Contains(lrn))
                lrn.Text = newName;
        }
    }
}
