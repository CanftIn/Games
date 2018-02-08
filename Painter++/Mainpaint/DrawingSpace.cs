using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Mainpaint
{
    public class DrawingSpace
    {
        private List<Layer> layers = new List<Layer>();
        Layer WM_layer;
        Bitmap processed;
        Bitmap trsMap;

        public DrawingSpace(Size sx)
        {
            processed = new Bitmap(sx.Width, sx.Height);
            trsMap = TransparentMap(sx);
            Layer ly = new Layer();
            ly.LayerImage = trsMap;
            ly.LayerName = "Transparent Map";
            ly.Visible = true;
            layers.Add(ly);
        }

        public Layer SelectedLayerW
        {
            get
            {
                return WM_layer;
            }
            set
            {
                WM_layer = value;
            }
        }

        public struct Layer
        {
            public Bitmap LayerImage;
            public string LayerName;
            public bool Visible;
            public void Update(bool visibility)
            {
                this.Visible = visibility;
            }
            public void Update(Bitmap Image)
            {
                this.LayerImage = Image;
            }
            public void Clean()
            {
                this.LayerImage = new Bitmap(this.LayerImage.Size.Width, this.LayerImage.Size.Height);
                this.LayerImage.MakeTransparent();
            }
            public void Resize(Size sx)
            {
                this.LayerImage = new Bitmap(sx.Width, sx.Height);
                this.LayerImage.MakeTransparent();
            }
        }

        public void CreateLayer(string name, Size size, bool visible)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            Layer l = new Layer();
            bmp.MakeTransparent();
            l.LayerImage = bmp;
            l.LayerName = name;
            l.Visible = visible;
            layers.Add(l);
        }

        public enum Tool
        {
            Pencil, Brush, Rectangle, Ellipse, ColorPicker, Line, Eraser, Text, RectSelect, Gradient, Bucket, ColorSelect, MoveObj, Rotate, Measure
        }

        public Layer LayerLocate(string name)
        {
            Layer ln = new Layer();
            foreach (Layer ls in layers)
            {
                if (ls.LayerName == name)
                {
                    ln = ls;
                }
            }
            return ln;
        }

        public Bitmap FinalTransparent
        {
            get
            {
                Bitmap bmp = new Bitmap(processed.Width, processed.Height);
                bmp.MakeTransparent();
                foreach (Layer l in layers)
                {
                    if (l.LayerName == "TrsMap") continue;
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.DrawImageUnscaled(l.LayerImage, new Point(0, 0));
                    }
                }
                return bmp;
            }
        }

        public void LayerChangeVisibility(Layer l, bool visible)
        {
            if (layers.Contains(l))
            {
                layers[layers.IndexOf(l)].Update(visible);
            }
        }

        public void LayerChangeVisibility(string name, bool visible)
        {
            Layer l = LayerLocate(name);
            if (layers.Contains(l))
            {
                Layer lc = new Layer();
                lc.LayerImage = layers[layers.IndexOf(l)].LayerImage;
                lc.LayerName = layers[layers.IndexOf(l)].LayerName;
                lc.Visible = visible;
                int ind = layers.IndexOf(l);
                layers.Remove(l);
                layers.Insert(ind, lc);
            }
        }

        public void LayerRemove(string name)
        {
            Layer lr = LayerLocate(name);
            if (layers.Contains(lr))
            {
                layers.Remove(lr);
            }
        }

        public void ResizeCanvas(Size sx, bool ResizeLayers)
        {
            processed = new Bitmap(sx.Width, sx.Height);
            UpdateTransparencyMap(sx);
            if (ResizeLayers)
            {
                foreach (Layer lrs in layers)
                {
                    if (layers.IndexOf(lrs) == 0) continue;
                    lrs.Resize(sx);
                }
            }
        }

        public struct FloodData
        {
            public Point p1;
            public Color color1;
            public Color color2;
        }

        public List<Layer> Layers
        {
            get
            {
                return layers;
            }
        }

        public Bitmap Final
        {
            get
            {
                return processed;
            }
        }

        public Bitmap TransparentMap(Size dim)
        {
            Bitmap bmp = new Bitmap(dim.Width, dim.Height);
            bmp.MakeTransparent();
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(new HatchBrush(HatchStyle.SmallCheckerBoard, Color.FromArgb(255, 50, 50, 50), Color.FromArgb(255, 150, 150, 150)), new Rectangle(0, 0, dim.Width, dim.Height));
            }
            Bitmap dest = new Bitmap(dim.Width * 5, dim.Height * 5);
            using (Graphics gx = Graphics.FromImage(dest))
            {
                gx.InterpolationMode = InterpolationMode.NearestNeighbor;
                gx.DrawImage(bmp, 0, 0, dest.Width, dest.Height);
            }
            return dest;
        }

        public void UpdateTransparencyMap(Size newSize)
        {
            Layer ls = new Layer();
            ls.LayerImage = TransparentMap(newSize);
            ls.LayerName = "Transparent Map";
            layers[0] = ls;
        }

        public void DrawnData()
        {
            processed = new Bitmap(processed.Width, processed.Height);
            processed.MakeTransparent();

            foreach (Layer l in layers)
            {
                if (l.Visible)
                {
                    using (Graphics g = Graphics.FromImage(processed))
                    {
                        g.DrawImage(l.LayerImage, new Point(0, 0));
                    }
                }
                else
                {

                }
            }

        }

        private static bool MatchColor(Color a, Color b, int tolerance)
        {
            bool isAlike = false;
            if (b.R >= (a.R - tolerance) && b.R <= (a.R + tolerance))
            {
                if (b.G >= (a.G - tolerance) && b.G <= (a.G + tolerance))
                {
                    if (b.B >= (a.B - tolerance) && b.B <= (a.B + tolerance))
                    {
                        isAlike = true;
                    }
                }
            }
            return isAlike;
        }

        public void FloodFill(Point p1, Color color1, Color color2, int tolerace)
        {
            Queue<Point> q = new Queue<Point>();
            q.Enqueue(p1);

            while (q.Count > 0)
            {
                Point p2 = q.Dequeue();

                if (!MatchColor(this.SelectedLayerW.LayerImage.GetPixel(p2.X, p2.Y), color1, tolerace)) continue;
                if (MatchColor(this.SelectedLayerW.LayerImage.GetPixel(p2.X, p2.Y), color2, 0)) continue;

                Point p3 = p2, p4 = new Point(p2.X + 1, p2.Y);

                while ((p3.X > 0) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p3.X, p3.Y), color1, tolerace))
                {
                    SelectedLayerW.LayerImage.SetPixel(p3.X, p3.Y, color2);

                    if ((p3.Y > 0) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p3.X, p3.Y - 1), color1, tolerace))
                        q.Enqueue(new Point(p3.X, p3.Y - 1));

                    if ((p3.Y < SelectedLayerW.LayerImage.Height - 1) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p3.X, p3.Y + 1), color1, tolerace))
                        q.Enqueue(new Point(p3.X, p3.Y + 1));

                    p3.X--;
                }

                while ((p4.X < SelectedLayerW.LayerImage.Width - 1) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p4.X, p4.Y), color1, tolerace))
                {
                    SelectedLayerW.LayerImage.SetPixel(p4.X, p4.Y, color2);

                    if ((p4.Y > 0) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p4.X, p4.Y - 1), color1, tolerace))
                        q.Enqueue(new Point(p4.X, p4.Y - 1));

                    if ((p4.Y < SelectedLayerW.LayerImage.Height - 1) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p4.X, p4.Y + 1), color1, tolerace))
                        q.Enqueue(new Point(p4.X, p4.Y + 1));

                    p4.X++;
                }
            }
        }


        public void FloodFill(Point p1, Color color1, Color color2, int tolerace, Rectangle rect)
        {
            Queue<Point> q = new Queue<Point>();
            Point p1n = new Point(p1.X - rect.X, p1.Y - rect.Y);
            
            q.Enqueue(p1);

            while (q.Count > 0)
            {
                Point p2 = q.Dequeue();

                if (!MatchColor(SelectedLayerW.LayerImage.GetPixel(p2.X, p2.Y), color1, tolerace)) continue;
                if (MatchColor(SelectedLayerW.LayerImage.GetPixel(p2.X, p2.Y), color2, 0)) continue;

                Point p3 = p2, p4 = new Point(p2.X + 1, p2.Y);

                while ((p3.X > rect.X) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p3.X, p3.Y), color1, tolerace))
                {
                    SelectedLayerW.LayerImage.SetPixel(p3.X, p3.Y, color2);

                    if ((p3.Y > rect.Y) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p3.X, p3.Y - 1), color1, tolerace))
                        q.Enqueue(new Point(p3.X, p3.Y - 1));

                    if ((p3.Y < (rect.Height + rect.Y) - 1) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p3.X, p3.Y + 1), color1, tolerace))
                        q.Enqueue(new Point(p3.X, p3.Y + 1));

                    p3.X--;
                }

                while ((p4.X < (rect.Width + rect.X) - 1) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p4.X, p4.Y), color1, tolerace))
                {
                    SelectedLayerW.LayerImage.SetPixel(p4.X, p4.Y, color2);

                    if ((p4.Y > rect.Y) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p4.X, p4.Y - 1), color1, tolerace))
                        q.Enqueue(new Point(p4.X, p4.Y - 1));

                    if ((p4.Y < (rect.Height + rect.Y) - 1) && MatchColor(SelectedLayerW.LayerImage.GetPixel(p4.X, p4.Y + 1), color1, tolerace))
                        q.Enqueue(new Point(p4.X, p4.Y + 1));

                    p4.X++;
                }
            }
        }

        public Bitmap SelectRegion(Rectangle rect)
        {
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImageUnscaledAndClipped(SelectedLayerW.LayerImage, rect);
            }
            return bmp;
        }

        public static Bitmap Rotate(Bitmap img, double angle, PointF offset)
        {
            Bitmap bmp_trf = new Bitmap(img.Width, img.Height);
            bmp_trf.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            using (Graphics g = Graphics.FromImage(bmp_trf))
            {
                g.TranslateTransform(offset.X, offset.Y);
                g.RotateTransform((float)angle * 180);
                g.TranslateTransform(-offset.X, -offset.Y);
                g.DrawImage(img, new PointF(0, 0));
            }

            return bmp_trf;
        }
    }
}
