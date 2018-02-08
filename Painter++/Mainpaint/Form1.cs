using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

using Mainpaint.Other;
using Mainpaint.misc;
using Mainpaint.EffectWindows;
using System.Collections.Generic;

namespace Mainpaint
{
    public partial class Form1 : Form
    {

        #region init_stuff

        DrawingSpace ds;
        Color _ForeColor, _BackColor;
        Point SLoc = Point.Empty, CPoint = Point.Empty, PCloc = new Point(128, 128), SCloc = new Point(128, 128), rulerLoc, Cpos = new Point(43, 41);
        Size ImageSize, resizedImageSize;
        DrawingSpace.Tool currentTool = DrawingSpace.Tool.Rectangle;
        DrawOperationType drawMode = DrawOperationType.Fill;
        Brush mainBrush = new SolidBrush(Color.White);
        Rectangle WorkingSpace = Rectangle.Empty, AllocatedTextSpace = Rectangle.Empty, RescaledWorkingSpace = Rectangle.Empty;
        bool DrawOp = false, AllowedColorChange = true, Control = false, ChooseState = false, Shift = false;
        ColorMode MainDrawMode, PickerMode = ColorMode.ForeColor;
        Pen pen1 = new Pen(Color.Black), pen1_text = new Pen(Color.Orange);
        TextInputBox TIB;
        double ScaleFactor = 1;
        FileDrag fd = new FileDrag();
        BrushType b_mode = BrushType.Circle_smooth;
        Bitmap color_bmp;
        GradientColorPicker GCP;
        GradientInformation GI;
        decimal difference = 1;

        public enum BrushType
        {
            Rect_smooth, Rect_sharp, Circle_smooth, Circle_sharp
        }

        public delegate void DelegateCaller();

        public Form1()
        {
            InitializeComponent();
            layerContainer1.MasterWindow = this;
            ImageSize = new Size(800, 600);
            ds = new DrawingSpace(new Size(800, 600));
            pen1.DashCap = DashCap.Round;
            pen1.DashPattern = new float[] { 4f, 2f, 2f, 3f };
            pen1_text.DashCap = DashCap.Round;
            pen1_text.DashPattern = new float[] { 4f, 2f, 3f, 4f };
            textBox1.Parent = this;
            TIB = new TextInputBox(this);
            layerContainer1.AddLayer("Background", true);
            using (Graphics g = Graphics.FromImage(Space.Layers[1].LayerImage))
            {
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, Space.Layers[1].LayerImage.Width, Space.Layers[1].LayerImage.Height);
            }
            GCP = new GradientColorPicker();
        }

        #endregion

        #region color_picker_and_message_system

        public Color foreColor
        {
            get
            {
                return _ForeColor;
            }
            set
            {
                _ForeColor = value;
            }
        }

        public Size SpaceSize
        {
            get
            {
                return ImageSize;
            }
        }

        public DrawingSpace Space
        {
            get
            {
                return ds;
            }
        }

        public Color backColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                _BackColor = value;
            }
        }

        public void ForeColorUpdate(Color c)
        {
            _ForeColor = c;
            pictureBox3.BackColor = c;
            HexVal.Text = c.Name.ToString();
            RVal.Value = c.R;
            GVal.Value = c.G;
            BVal.Value = c.B;
            AVal.Value = c.A;
            if (mainBrush.GetType() == typeof(HatchBrush))
            {
                HatchBrush hb = (HatchBrush)mainBrush;
                mainBrush = new HatchBrush(hb.HatchStyle, _ForeColor, Color.Transparent);
            }
            else if (mainBrush.GetType() == typeof(SolidBrush))
            {
                mainBrush = new SolidBrush(_ForeColor);
            }
            DrawUpdater();
        }

        public void BackColorUpdate(Color c)
        {
            _BackColor = c;
            pictureBox2.BackColor = c;
            HexVal.Text = c.Name.ToString();
            RVal.Value = c.R;
            GVal.Value = c.G;
            BVal.Value = c.B;
            AVal.Value = c.A;
            if (mainBrush.GetType() == typeof(HatchBrush))
            {
                HatchBrush hb = (HatchBrush)mainBrush;
                mainBrush = new HatchBrush(hb.HatchStyle, _BackColor, Color.Transparent);
            }
            else if (mainBrush.GetType() == typeof(SolidBrush))
            {
                mainBrush = new SolidBrush(_BackColor);
            }
            DrawUpdater();
        }

        private void ForeColorUpdate(Color c, bool IsColorChangeAllowed)
        {
            if (!IsColorChangeAllowed)
                AllowedColorChange = false;
            pictureBox3.BackColor = _ForeColor;
            _ForeColor = c;
            HexVal.Text = c.Name.ToString();
            RVal.Value = c.R;
            GVal.Value = c.G;
            BVal.Value = c.B;
            AVal.Value = c.A;
            if (mainBrush.GetType() == typeof(HatchBrush))
            {
                HatchBrush hb = (HatchBrush)mainBrush;
                mainBrush = new HatchBrush(hb.HatchStyle, _ForeColor, Color.Transparent);
            }
            else if (mainBrush.GetType() == typeof(SolidBrush))
            {
                mainBrush = new SolidBrush(_ForeColor);
            }
            AllowedColorChange = true;
            DrawUpdater();
        }

        private void BackColorUpdate(Color c, bool IsColorChangeAllowed)
        {
            if (!IsColorChangeAllowed)
                AllowedColorChange = false;
            pictureBox2.BackColor = _BackColor;
            _BackColor = c;
            HexVal.Text = _BackColor.Name.ToString();
            RVal.Value = _BackColor.R;
            GVal.Value = _BackColor.G;
            BVal.Value = _BackColor.B;
            AVal.Value = _BackColor.A;
            if (mainBrush.GetType() == typeof(HatchBrush))
            {
                HatchBrush hb = (HatchBrush)mainBrush;
                mainBrush = new HatchBrush(hb.HatchStyle, _BackColor, Color.Transparent);
            }
            else if (mainBrush.GetType() == typeof(SolidBrush))
            {
                mainBrush = new SolidBrush(_BackColor);
            }
            AllowedColorChange = true;
            DrawUpdater();
        }

        public enum DrawOperationType
        {
            Draw, Fill
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string style in Enum.GetNames(typeof(HatchStyle)))
            {
                listBox1.Items.Add(style);
            }
            listBox1.Items.Add("Solid");
            color_bmp = new Bitmap(pictureBox5.Image, new Size(260, 260));

            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, 260, 260);
            Region rg = new Region(gp);
            pictureBox5.Region = rg;

            resizedImageSize = new Size(800, 600);
            panel3.AutoScroll = true;
            panel3.HorizontalScroll.Enabled = true;
            panel3.VerticalScroll.Enabled = true;
        }

        public enum MessageType
        {
            Warning, Error
        }

        public void Message(string text, MessageType type, bool constant)
        {
            switch (type)
            {
                case MessageType.Warning:
                    toolStripStatusLabel1.Image = Mainpaint.Properties.Resources.warning;
                    break;
                case MessageType.Error:
                    toolStripStatusLabel1.Image = Mainpaint.Properties.Resources.error;
                    break;
            }
            toolStripStatusLabel1.Text = text;
            toolStripStatusLabel1.Visible = true;
            if(!constant)
                timer1.Enabled = true;
        }
        public void Message(string text, MessageType type)
        {
            switch (type)
            {
                case MessageType.Warning:
                    toolStripStatusLabel1.Image = Mainpaint.Properties.Resources.warning;
                    break;
                case MessageType.Error:
                    toolStripStatusLabel1.Image = Mainpaint.Properties.Resources.error;
                    break;
            }
            toolStripStatusLabel1.Text = text;
            toolStripStatusLabel1.Visible = true;
            timer1.Enabled = true;
        }

        #endregion

        #region DrawingWorkspace

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Middle)
            {
                CPoint = new Point(e.X, e.Y);
                rulerLoc = new Point(e.X + pictureBox1.Left, e.Y + (pictureBox1.Top - 30));
                pictureBox7.Invalidate(); pictureBox8.Invalidate();
                DrawUpdater();
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                pictureBox1.Left = MousePosition.X - this.PointToScreen(SLoc).X;
                pictureBox1.Top = MousePosition.Y - this.PointToScreen(SLoc).Y - 74;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            DrawOp = false;
            if (e.Button != System.Windows.Forms.MouseButtons.Middle)
            {
                SLoc = new Point(Convert.ToInt32(SLoc.X / ScaleFactor), Convert.ToInt32(SLoc.Y / ScaleFactor));
                Point endP = new Point(Convert.ToInt32(e.X / ScaleFactor), Convert.ToInt32(e.Y / ScaleFactor));
                using (Graphics g = Graphics.FromImage(ds.SelectedLayerW.LayerImage))
                {
                    g.InterpolationMode = InterpolationMode.High;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    if (WorkingSpace != Rectangle.Empty)
                        g.SetClip(WorkingSpace);

                    switch (currentTool)
                    {
                        case DrawingSpace.Tool.Rectangle:
                            if (drawMode == DrawOperationType.Fill)
                                g.FillRectangle(mainBrush, new Rectangle(SLoc.X, SLoc.Y, endP.X - SLoc.X, endP.Y - SLoc.Y));
                            else if (drawMode == DrawOperationType.Draw)
                                g.DrawRectangle(new Pen(mainBrush, (float)BrushSize.Value), new Rectangle(SLoc.X, SLoc.Y, endP.X - SLoc.X, endP.Y - SLoc.Y));
                            break;
                        case DrawingSpace.Tool.Ellipse:
                            if (drawMode == DrawOperationType.Fill)
                                g.FillEllipse(mainBrush, new Rectangle(SLoc.X, SLoc.Y, endP.X - SLoc.X, endP.Y - SLoc.Y));
                            else if (drawMode == DrawOperationType.Draw)
                                g.DrawEllipse(new Pen(mainBrush, (float)BrushSize.Value), new Rectangle(SLoc.X, SLoc.Y, endP.X - SLoc.X, endP.Y - SLoc.Y));
                            break;
                        case DrawingSpace.Tool.Line:
                            g.DrawLine(new Pen(mainBrush, (float)BrushSize.Value), SLoc, new Point(endP.X, endP.Y));
                            break;
                        case DrawingSpace.Tool.Bucket:
                            if (WorkingSpace == Rectangle.Empty)
                            {
                                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                                    ds.FloodFill(new Point(endP.X, endP.Y), this.Space.SelectedLayerW.LayerImage.GetPixel(endP.X, endP.Y), _ForeColor, trackBar2.Value);
                                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                                    ds.FloodFill(new Point(endP.X, endP.Y), this.Space.SelectedLayerW.LayerImage.GetPixel(endP.X, endP.Y), _BackColor, trackBar2.Value);
                            }
                            else
                            {
                                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                                    ds.FloodFill(new Point(endP.X, endP.Y), this.Space.SelectedLayerW.LayerImage.GetPixel(endP.X, endP.Y), _ForeColor, trackBar2.Value, WorkingSpace);
                                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                                    ds.FloodFill(new Point(endP.X, endP.Y), this.Space.SelectedLayerW.LayerImage.GetPixel(endP.X, endP.Y), _BackColor, trackBar2.Value, WorkingSpace);
                            }
                            Thread.Sleep(10);
                            break;
                        case DrawingSpace.Tool.Rotate:
                            PointF centerOffset = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
                            double anglex = Math.Atan2((double)endP.Y - (double)centerOffset.Y, (double)endP.X - (double)centerOffset.X);
                            Bitmap bmp = new Bitmap(ds.SelectedLayerW.LayerImage.Width, ds.SelectedLayerW.LayerImage.Height);
                            bmp.MakeTransparent();
                            using (Graphics gx = Graphics.FromImage(bmp))
                            {
                                gx.DrawImage(DrawingSpace.Rotate(ds.SelectedLayerW.LayerImage, anglex, centerOffset), new Point(0, 0));
                            }
                            DrawingSpace.Layer lx = new DrawingSpace.Layer();
                            lx.LayerName = ds.SelectedLayerW.LayerName;
                            lx.Visible = ds.SelectedLayerW.Visible;
                            lx.LayerImage = bmp;
                            ds.Layers[ds.Layers.IndexOf(ds.SelectedLayerW)] = lx;
                            ds.SelectedLayerW = ds.Layers[ds.Layers.IndexOf(lx)];
                            break;
                        case DrawingSpace.Tool.Text:
                            if (endP.X - SLoc.X <= 3 && endP.Y - SLoc.Y <= 3)
                            {
                                AllocatedTextSpace = Rectangle.Empty;
                                using (Graphics gx = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
                                {
                                    gx.DrawString(textBox1.Text, TIB.ChoosenFont, mainBrush, SLoc);
                                }
                                textBox1.Text = "";
                                TIB.Hide();
                            }
                            else
                            {
                                TIB.Location = new Point(this.Location.X + SLoc.X, this.Location.Y + SLoc.Y); TIB.Show();
                                AllocatedTextSpace = new Rectangle(SLoc.X, SLoc.Y, endP.X - SLoc.X, endP.Y - SLoc.Y);
                                textBox1.Focus();
                            }
                            break;
                        case DrawingSpace.Tool.Gradient:

                            try
                            {
                                if (GI.type == GradientType.Linear)
                                {
                                    float angle = (float)Math.Round(Math.Atan2(endP.Y - SLoc.Y, endP.X - SLoc.X) * 1000) / 1000;
                                    LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(SLoc.X, SLoc.Y, endP.X - SLoc.X, endP.Y - SLoc.Y), Color.Black, Color.Black, angle);
                                    ColorBlend cb = new ColorBlend();
                                    cb.Positions = GI.positions;
                                    cb.Colors = GI.colors.ToArray();
                                    lgb.InterpolationColors = cb;

                                    using (Graphics gp = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
                                    {
                                        gp.FillRectangle(lgb, new Rectangle(SLoc.X, SLoc.Y, endP.X - SLoc.X, endP.Y - SLoc.Y));
                                    }
                                    lgb.Dispose();

                                }
                                else if (GI.type == GradientType.Rectangular)
                                {
                                    GraphicsPath gp = new GraphicsPath();
                                    gp.AddRectangle(new Rectangle(SLoc.X, SLoc.Y, endP.X - SLoc.X, endP.Y - SLoc.Y));

                                    PathGradientBrush pgb = new PathGradientBrush(gp);
                                    ColorBlend cb = new ColorBlend();
                                    cb.Positions = GI.positions;
                                    cb.Colors = GI.colors.ToArray();
                                    pgb.InterpolationColors = cb;

                                    using (Graphics gp1 = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
                                    {
                                        gp1.FillRectangle(pgb, new Rectangle(SLoc.X, SLoc.Y, endP.X - SLoc.X, endP.Y - SLoc.Y));
                                    }

                                    pgb.Dispose();
                                }
                                else if(GI.type == GradientType.Circular)
                                {

                                    GraphicsPath gp = new GraphicsPath();
                                    gp.AddEllipse(new Rectangle(SLoc.X, SLoc.Y, endP.X - SLoc.X, endP.Y - SLoc.Y));
                                    PathGradientBrush pgb = new PathGradientBrush(gp);
                                    ColorBlend cb = new ColorBlend();
                                    cb.Positions = GI.positions;
                                    cb.Colors = GI.colors.ToArray();
                                    pgb.InterpolationColors = cb;

                                    using (Graphics gp1 = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
                                    {
                                        gp1.FillEllipse(pgb, new Rectangle(SLoc.X, SLoc.Y, endP.X - SLoc.X, endP.Y - SLoc.Y));
                                    }

                                    pgb.Dispose();

                                }
                            }
                            catch (Exception ex)
                            {
                                Message(ex.Message, MessageType.Error);
                            }
                            break;
                        case DrawingSpace.Tool.RectSelect:
                            if (e.X - SLoc.X <= 3 && e.Y - SLoc.Y <= 3)
                            {
                                WorkingSpace = Rectangle.Empty; RescaledWorkingSpace = Rectangle.Empty;
                            }
                            else
                            {
                                WorkingSpace = new Rectangle(SLoc.X, SLoc.Y, e.X - SLoc.X, e.Y - SLoc.Y);
                                RescaledWorkingSpace = WorkingSpace;
                            }
                            break;
                    }
                    ds.DrawnData();
                }
                DrawUpdater();
            }
            else
                Cpos = new Point(pictureBox1.Left, pictureBox1.Top);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Middle)
            {
                switch (e.Button)
                {
                    case System.Windows.Forms.MouseButtons.Left:
                        MainDrawMode = ColorMode.ForeColor;
                        if (mainBrush.GetType() == typeof(HatchBrush))
                        {
                            HatchBrush hb = (HatchBrush)mainBrush;
                            mainBrush = new HatchBrush(hb.HatchStyle, _ForeColor, _BackColor);
                        }
                        else if (mainBrush.GetType() == typeof(SolidBrush))
                        {
                            mainBrush = new SolidBrush(_ForeColor);
                        }
                        break;
                    case System.Windows.Forms.MouseButtons.Right:
                        MainDrawMode = ColorMode.BackColor;
                        if (mainBrush.GetType() == typeof(HatchBrush))
                        {
                            HatchBrush hb = (HatchBrush)mainBrush;
                            mainBrush = new HatchBrush(hb.HatchStyle, _BackColor, _ForeColor);
                        }
                        else if (mainBrush.GetType() == typeof(SolidBrush))
                        {
                            mainBrush = new SolidBrush(_BackColor);
                        }
                        break;
                }
                if (currentTool == DrawingSpace.Tool.ColorPicker)
                {
                    if (MainDrawMode == ColorMode.ForeColor)
                    {
                        ForeColorUpdate(ds.Final.GetPixel(e.X, e.Y), false);
                    }
                    else if (MainDrawMode == ColorMode.BackColor)
                    {
                        BackColorUpdate(ds.Final.GetPixel(e.X, e.Y), false);
                    }
                }
                if (currentTool == DrawingSpace.Tool.RectSelect)
                {
                    WorkingSpace = Rectangle.Empty;
                    using (Graphics g = pictureBox1.CreateGraphics())
                    {
                        g.Clear(pictureBox1.BackColor);
                        g.DrawImage(ds.Final, new Point(0, 0));
                    }
                }
            }

            SLoc = new Point(e.X, e.Y);

            if(e.Button != System.Windows.Forms.MouseButtons.Middle)
                DrawOp = true;

            DrawUpdater();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs ex)
        {
            Graphics g = ex.Graphics;
            g.DrawImage(ds.Final, 0, 0, resizedImageSize.Width, resizedImageSize.Height);

            if (WorkingSpace != Rectangle.Empty)
            {
                g.DrawRectangle(new Pen(new HatchBrush(HatchStyle.SmallCheckerBoard, Color.Blue, Color.Transparent)), RescaledWorkingSpace);
                if (currentTool != DrawingSpace.Tool.RectSelect)
                    g.SetClip(RescaledWorkingSpace);
            }

            if (!DrawOp)
            {
                if (AllocatedTextSpace != Rectangle.Empty)
                {
                    g.DrawRectangle(pen1_text, AllocatedTextSpace);
                    g.DrawString(textBox1.Text, TIB.ChoosenFont, mainBrush, new PointF(AllocatedTextSpace.X, AllocatedTextSpace.Y));
                }

                switch (currentTool)
                {
                    case DrawingSpace.Tool.Brush:
                    case DrawingSpace.Tool.Pencil:
                        switch (b_mode)
                        {
                            case BrushType.Circle_sharp:
                            case BrushType.Circle_smooth:
                                g.DrawEllipse(new Pen(new SolidBrush(Color.Black)), new Rectangle(CPoint.X - (int)(Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor) / 2), CPoint.Y - (int)(Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor) / 2), (int)Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor), (int)Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor)));
                                break;
                            case BrushType.Rect_sharp:
                            case BrushType.Rect_smooth:
                                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(CPoint.X - (int)(Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor) / 2), CPoint.Y - (int)(Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor) / 2), (int)Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor), (int)Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor)));
                                break;
                        }
                        break;
                    default:
                        if (currentTool != DrawingSpace.Tool.Eraser)
                        {
                            g.DrawLine(new Pen(new SolidBrush(Color.Black)), new Point(CPoint.X - 4, CPoint.Y), new Point(CPoint.X + 4, CPoint.Y));
                            g.DrawLine(new Pen(new SolidBrush(Color.Black)), new Point(CPoint.X, CPoint.Y - 4), new Point(CPoint.X, CPoint.Y + 4));
                        }
                        else
                        {
                            g.DrawEllipse(new Pen(new SolidBrush(Color.Black)), new Rectangle(CPoint.X - (int)(Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor) / 2), CPoint.Y - (int)(Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor) / 2), (int)Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor), (int)Convert.ToInt32(Convert.ToDouble(BrushSize.Value) * ScaleFactor)));
                        }
                        break;
                }
            }
            else
            {
                DrawUpdater();
                switch (currentTool)
                {
                    case DrawingSpace.Tool.Brush:
                    case DrawingSpace.Tool.Pencil:
                        Point tempC = new Point(Convert.ToInt32(CPoint.X / difference), Convert.ToInt32(CPoint.Y / difference));
                        using (Graphics gn = Graphics.FromImage(ds.SelectedLayerW.LayerImage))
                        {
                            if (WorkingSpace != Rectangle.Empty)
                                gn.SetClip(WorkingSpace);

                            switch (b_mode)
                            {
                                case BrushType.Circle_sharp:
                                    if (ToolDrawMode.Text == "Filled") gn.FillEllipse(mainBrush, new Rectangle(tempC.X - (int)(BrushSize.Value / 2), tempC.Y - (int)(BrushSize.Value / 2), (int)BrushSize.Value, (int)BrushSize.Value));
                                    else gn.DrawEllipse(new Pen(mainBrush), new Rectangle(tempC.X - (int)(BrushSize.Value / 2), tempC.Y - (int)(BrushSize.Value / 2), (int)BrushSize.Value, (int)BrushSize.Value));
                                    break;
                                case BrushType.Circle_smooth:
                                    gn.SmoothingMode = SmoothingMode.HighQuality;
                                    if (ToolDrawMode.Text == "Filled") gn.FillEllipse(mainBrush, new Rectangle(tempC.X - (int)(BrushSize.Value / 2), tempC.Y - (int)(BrushSize.Value / 2), (int)BrushSize.Value, (int)BrushSize.Value));
                                    else gn.DrawEllipse(new Pen(mainBrush), new Rectangle(tempC.X - (int)(BrushSize.Value / 2), tempC.Y - (int)(BrushSize.Value / 2), (int)BrushSize.Value, (int)BrushSize.Value));
                                    break;
                                case BrushType.Rect_sharp:
                                    if (ToolDrawMode.Text == "Filled") gn.FillRectangle(mainBrush, new Rectangle(tempC.X - (int)(BrushSize.Value / 2), tempC.Y - (int)(BrushSize.Value / 2), (int)BrushSize.Value, (int)BrushSize.Value));
                                    else gn.DrawRectangle(new Pen(mainBrush), new Rectangle(tempC.X - (int)(BrushSize.Value / 2), tempC.Y - (int)(BrushSize.Value / 2), (int)BrushSize.Value, (int)BrushSize.Value));
                                    break;
                                case BrushType.Rect_smooth:
                                    gn.SmoothingMode = SmoothingMode.HighQuality;
                                    if (ToolDrawMode.Text == "Filled") gn.FillRectangle(mainBrush, new Rectangle(tempC.X - (int)(BrushSize.Value / 2), tempC.Y - (int)(BrushSize.Value / 2), (int)BrushSize.Value, (int)BrushSize.Value));
                                    else gn.DrawRectangle(new Pen(mainBrush), new Rectangle(tempC.X - (int)(BrushSize.Value / 2), tempC.Y - (int)(BrushSize.Value / 2), (int)BrushSize.Value, (int)BrushSize.Value));
                                    break;
                            }
                        }
                        ds.DrawnData();
                        break;
                    case DrawingSpace.Tool.Eraser:
                        using (Graphics gn = Graphics.FromImage(ds.SelectedLayerW.LayerImage))
                        {
                            if (WorkingSpace != Rectangle.Empty)
                                gn.SetClip(WorkingSpace);

                            gn.FillEllipse(new SolidBrush(Color.FromArgb(255, 254, 254, 254)), new Rectangle(CPoint.X - (int)(BrushSize.Value / 2), CPoint.Y - (int)(BrushSize.Value / 2), (int)BrushSize.Value, (int)BrushSize.Value));
                            ds.SelectedLayerW.LayerImage.MakeTransparent(Color.FromArgb(255, 254, 254, 254));
                        }
                        ds.DrawnData();
                        break;
                    case DrawingSpace.Tool.Rectangle:
                        if (drawMode == DrawOperationType.Fill) g.FillRectangle(mainBrush, new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y));
                        if (drawMode == DrawOperationType.Draw) g.DrawRectangle(new Pen(mainBrush, (float)BrushSize.Value), new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y));
                        break;
                    case DrawingSpace.Tool.Ellipse:
                        if (drawMode == DrawOperationType.Draw) g.DrawEllipse(new Pen(mainBrush, (float)BrushSize.Value), new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y));
                        if (drawMode == DrawOperationType.Fill) g.FillEllipse(mainBrush, new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y));
                        break;
                    case DrawingSpace.Tool.Rotate:
                        PointF centerOffset = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
                        double anglex = Math.Atan2((double)CPoint.Y - (double)centerOffset.Y, (double)CPoint.X - (double)centerOffset.X);
                        g.DrawString(anglex.ToString(), new Font("Segoe UI", 12.0f, FontStyle.Regular), new SolidBrush(Color.Black), new PointF(10, 10));
                        g.DrawImage(DrawingSpace.Rotate(ds.SelectedLayerW.LayerImage, anglex, centerOffset), new Point(0, 0));
                        break;
                    case DrawingSpace.Tool.Line:
                        g.DrawLine(new Pen(mainBrush, (float)(Convert.ToDouble(BrushSize.Value) * ScaleFactor)), SLoc, CPoint);
                        break;
                    case DrawingSpace.Tool.Text:
                        g.DrawRectangle(new Pen(new HatchBrush(HatchStyle.DashedHorizontal, _ForeColor)), new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y));
                        break;
                    case DrawingSpace.Tool.Measure:
                        double lgth = Math.Floor(Math.Sqrt(Math.Pow(CPoint.X - SLoc.X, 2) + Math.Pow(CPoint.Y - SLoc.Y, 2)));
                        g.DrawString("Length: " + lgth.ToString() + "px", new Font("Tahoma", 12.0f, FontStyle.Regular), new SolidBrush(Color.Red), new PointF(10, 10));
                        g.DrawLine(new Pen(new SolidBrush(Color.Blue)), SLoc, CPoint);
                        break;
                    case DrawingSpace.Tool.Gradient:
                        try
                        {
                            if (GI.type == GradientType.Linear)
                            {
                                float angle = (float)Math.Round(Math.Atan2(CPoint.Y - SLoc.Y, CPoint.X - SLoc.X) * 1000) / 1000;
                                LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y), Color.Black, Color.Black, angle);
                                ColorBlend cb = new ColorBlend();
                                cb.Positions = GI.positions;
                                cb.Colors = GI.colors.ToArray();
                                lgb.InterpolationColors = cb;

                                g.FillRectangle(lgb, new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y));

                            }
                            else if (GI.type == GradientType.Rectangular)
                            {
                                GraphicsPath gp = new GraphicsPath();
                                gp.AddRectangle(new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y));

                                PathGradientBrush pgb = new PathGradientBrush(gp);
                                ColorBlend cb = new ColorBlend();
                                cb.Positions = GI.positions;
                                cb.Colors = GI.colors.ToArray();
                                pgb.InterpolationColors = cb;

                                g.FillRectangle(pgb, new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y));

                                pgb.Dispose();
                            }
                            else if (GI.type == GradientType.Circular)
                            {

                                GraphicsPath gp = new GraphicsPath();
                                gp.AddEllipse(new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y));
                                PathGradientBrush pgb = new PathGradientBrush(gp);
                                ColorBlend cb = new ColorBlend();
                                cb.Positions = GI.positions;
                                cb.Colors = GI.colors.ToArray();
                                pgb.InterpolationColors = cb;

                                g.FillRectangle(pgb, new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y));

                                pgb.Dispose();

                            }
                        }
                        catch (Exception exs)
                        {
                            Message(exs.Message, MessageType.Error);
                        }
                        break;
                    case DrawingSpace.Tool.RectSelect:
                        g.DrawRectangle(new Pen(new HatchBrush(HatchStyle.SmallCheckerBoard, Color.Blue, Color.Transparent)), new Rectangle(SLoc.X, SLoc.Y, CPoint.X - SLoc.X, CPoint.Y - SLoc.Y));
                        break;

                }
            }
        }

        public void DrawUpdater()
        {
            pictureBox1.Invalidate();
        }

        public void DrawUpdater(bool force_redraw)
        {
            if (force_redraw) ds.DrawnData();
            pictureBox1.Invalidate();
        }

        public void Rescale(double d)
        {
            if (Control)
            {
                ScaleFactor = (d > 0) ? d : 0.1;

                toolStripStatusLabel2.Text = "Zoom: " + (d * 100) + "%";

                int newW = (int)(ImageSize.Width * ScaleFactor);
                int newH = (int)(ImageSize.Height * ScaleFactor);

                int originalWidth = ImageSize.Width;
                int originalHeight = ImageSize.Height;
                float percentWidth = (float)newW / (float)originalWidth;
                float percentHeight = (float)newH / (float)originalHeight;
                float percent = (percentHeight < percentWidth) ? percentHeight : percentWidth;

                difference = Convert.ToDecimal(percent);

                newW = (int)(originalWidth * percent);
                newH = (int)(originalHeight * percent);

                if(WorkingSpace != Rectangle.Empty)
                    RescaledWorkingSpace = new Rectangle(Convert.ToInt32(WorkingSpace.X * difference), Convert.ToInt32(WorkingSpace.Y * difference), Convert.ToInt32(WorkingSpace.Width * difference), Convert.ToInt32(WorkingSpace.Height * difference));

                resizedImageSize = new Size(newW, newH);
                pictureBox1.Size = new Size(newW, newH);
                DrawUpdater(true);
            }
        }

        #endregion

        #region form_controls_1

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            Graphics g = panel2.CreateGraphics();
            g.Clear(Color.White);
            HatchStyle style;
            if (Enum.TryParse<HatchStyle>(listBox1.SelectedItem.ToString(), out style))
            {
                HatchBrush hb = new HatchBrush(style, Color.Red, Color.White);
                g.FillRectangle(hb, panel2.ClientRectangle);
                mainBrush = new HatchBrush(style, _ForeColor, _BackColor);
            }
            else if (listBox1.SelectedItem.ToString() == "Solid")
            {
                SolidBrush sb = new SolidBrush(Color.Red);
                g.FillRectangle(sb, panel2.ClientRectangle);
                mainBrush = new SolidBrush(_ForeColor);
            }
            g.Dispose();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Color temp = _ForeColor;
            _ForeColor = _BackColor;
            _BackColor = temp;
            pictureBox3.BackColor = _ForeColor;
            pictureBox2.BackColor = _BackColor;
        }

        private void toolStripButton25_Click(object sender, EventArgs e)
        {
            new_layer nl = new new_layer();
            if (nl.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                layerContainer1.AddLayer(nl.LayerText, nl.Visibility);
                Space.SelectedLayerW = Space.LayerLocate(nl.LayerText);
            }
        }

        private void toolStripButton27_Click(object sender, EventArgs e)
        {
            layerContainer1.MoveLayer(layerContainer1.SelectedLayer, MoveDirection.Up);
        }

        private void toolStripButton28_Click(object sender, EventArgs e)
        {
            layerContainer1.MoveLayer(layerContainer1.SelectedLayer, MoveDirection.Down);
        }

        private void toolStripButton29_Click(object sender, EventArgs e)
        {
            layerContainer1.RemoveLayer(layerContainer1.SelectedLayer);
        }

        private void toolStripButton26_Click(object sender, EventArgs e)
        {
            layerRename lr = new layerRename(layerContainer1.SelectedLayer.Text);
            if (lr.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                layerContainer1.RenameLayer(layerContainer1.SelectedLayer, lr.LayerText);
            }
        }

        private void DeselectAll()
        {
            foreach (ToolStripItem tsb in toolStrip2.Items)
            {
                if (tsb.GetType() == typeof(ToolStripButton))
                {
                    ToolStripButton ts = (ToolStripButton)tsb;
                    ts.Checked = false;
                }
            }
            if (currentTool == DrawingSpace.Tool.Text)
            {
                AllocatedTextSpace = Rectangle.Empty;
                using (Graphics gx = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
                {
                    gx.DrawString(textBox1.Text, TIB.ChoosenFont, mainBrush, SLoc);
                }
                TIB.Hide();
                textBox1.Text = "";
                DrawUpdater(true);
            }
        }

        private void DrawRect_Click(object sender, EventArgs e)
        {
            DeselectAll();
            DrawRect.Checked = true;
            currentTool = DrawingSpace.Tool.Rectangle;
        }

        private void DrawEllipse_Click(object sender, EventArgs e)
        {
            DeselectAll();
            DrawEllipse.Checked = true;
            currentTool = DrawingSpace.Tool.Ellipse;
        }

        private void Brush_Click(object sender, EventArgs e)
        {
            DeselectAll();
            Brush.Checked = true;
            currentTool = DrawingSpace.Tool.Brush;
            DrawUpdater();
        }

        private void Pencil_Click(object sender, EventArgs e)
        {
            DeselectAll();
            Pencil.Checked = true;
            currentTool = DrawingSpace.Tool.Pencil;
        }

        private void Rect_Select_Click(object sender, EventArgs e)
        {
            DeselectAll();
            Rect_Select.Checked = true;
            currentTool = DrawingSpace.Tool.RectSelect;
        }

        private void Move_Click(object sender, EventArgs e)
        {
            DeselectAll();
            Move.Checked = true;
            currentTool = DrawingSpace.Tool.MoveObj;
        }

        private void Rubber_Click(object sender, EventArgs e)
        {
            DeselectAll();
            Rubber.Checked = true;
            currentTool = DrawingSpace.Tool.Eraser;
            DrawUpdater();
        }

        private void Bucket_Click(object sender, EventArgs e)
        {
            DeselectAll();
            Bucket.Checked = true;
            currentTool = DrawingSpace.Tool.Bucket;
        }

        private void Line_Click(object sender, EventArgs e)
        {
            DeselectAll();
            Line.Checked = true;
            currentTool = DrawingSpace.Tool.Line;
        }

        private void ColorPicker_Click(object sender, EventArgs e)
        {
            DeselectAll();
            ColorPicker.Checked = true;
            currentTool = DrawingSpace.Tool.ColorPicker;
        }

        private void Rotate_Click(object sender, EventArgs e)
        {
            DeselectAll();
            Rotate.Checked = true;
            currentTool = DrawingSpace.Tool.Rotate;
        }

        private void Text_B_Click(object sender, EventArgs e)
        {
            DeselectAll();
            Text_B.Checked = true;
            currentTool = DrawingSpace.Tool.Text;
        }

        private void Measure_Click(object sender, EventArgs e)
        {
            DeselectAll();
            Measure.Checked = true;
            currentTool = DrawingSpace.Tool.Measure;
        }

        private void ToolDrawMode_TextChanged(object sender, EventArgs e)
        {
            if (ToolDrawMode.Text.ToLower() == "filled")
            {
                drawMode = DrawOperationType.Fill;
            }
            else if (ToolDrawMode.Text.ToLower() == "border only")
            {
                drawMode = DrawOperationType.Draw;
            }
        }

        private void RVal_ValueChanged(object sender, EventArgs e)
        {
            if (PickerMode == ColorMode.ForeColor && AllowedColorChange)
            {
                ForeColorUpdate(Color.FromArgb((int)AVal.Value, (int)RVal.Value, (int)GVal.Value, (int)BVal.Value));
            }
            else if (PickerMode == ColorMode.BackColor && AllowedColorChange)
            {
                BackColorUpdate(Color.FromArgb((int)AVal.Value, (int)RVal.Value, (int)GVal.Value, (int)BVal.Value));
            }
        }

        private void GVal_ValueChanged(object sender, EventArgs e)
        {
            if (PickerMode == ColorMode.ForeColor && AllowedColorChange)
            {
                ForeColorUpdate(Color.FromArgb((int)AVal.Value, (int)RVal.Value, (int)GVal.Value, (int)BVal.Value));
            }
            else if (PickerMode == ColorMode.BackColor && AllowedColorChange)
            {
                BackColorUpdate(Color.FromArgb((int)AVal.Value, (int)RVal.Value, (int)GVal.Value, (int)BVal.Value));
            }
        }

        private void BVal_ValueChanged(object sender, EventArgs e)
        {
            if (PickerMode == ColorMode.ForeColor && AllowedColorChange)
            {
                ForeColorUpdate(Color.FromArgb((int)AVal.Value, (int)RVal.Value, (int)GVal.Value, (int)BVal.Value));
            }
            else if (PickerMode == ColorMode.BackColor && AllowedColorChange)
            {
                BackColorUpdate(Color.FromArgb((int)AVal.Value, (int)RVal.Value, (int)GVal.Value, (int)BVal.Value));
            }
        }

        private void AVal_ValueChanged(object sender, EventArgs e)
        {
            if (PickerMode == ColorMode.ForeColor && AllowedColorChange)
            {
                ForeColorUpdate(Color.FromArgb((int)AVal.Value, (int)RVal.Value, (int)GVal.Value, (int)BVal.Value));
            }
            else if (PickerMode == ColorMode.BackColor && AllowedColorChange)
            {
                BackColorUpdate(Color.FromArgb((int)AVal.Value, (int)RVal.Value, (int)GVal.Value, (int)BVal.Value));
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            PickerMode = ColorMode.ForeColor;
            AllowedColorChange = false;
            ForeColorUpdate(pictureBox3.BackColor);
            AllowedColorChange = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            PickerMode = ColorMode.BackColor;
            AllowedColorChange = false;
            BackColorUpdate(pictureBox2.BackColor);
            AllowedColorChange = true;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (WorkingSpace != Rectangle.Empty)
                DrawUpdater();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Visible = false;
            timer1.Enabled = false;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
            if (AllocatedTextSpace != Rectangle.Empty && currentTool == DrawingSpace.Tool.Text)
                textBox1.Focus();
            else
                pictureBox1.Focus();
        }

        #endregion

        #region form_controls_2_and_key_control

        public enum KeyEvent
        {
            key_down, key_up
        }

        public void KeyDownCall(KeyEvent key, KeyEventArgs e)
        {
            switch (key)
            {
                case KeyEvent.key_down:
                    Form1_KeyDown(this, e);
                    break;
                case KeyEvent.key_up:
                    Form1_KeyUp(this, e);
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control)
                Control = true;

            if (e.Shift)
                Shift = true;

            if (e.KeyCode == Keys.Add)
            {
                BrushSize.Value += ((BrushSize.Value <= (BrushSize.Maximum - 5)) ? 5 : 0); DrawUpdater();
            }
            if (e.KeyCode == Keys.Subtract)
            {
                BrushSize.Value -= ((BrushSize.Value > 5) ? 5 : 0); DrawUpdater();
            }
            if (Control && Shift && e.KeyCode == Keys.N)
            {
                toolStripButton25_Click(this, new EventArgs());
            }
            if (Control && e.KeyCode == Keys.F)
            {
                flattenImageToolStripMenuItem_Click(this, new EventArgs());
            }
            if (Shift && e.KeyCode == Keys.R)
            {
                if (currentTool == DrawingSpace.Tool.Rectangle)
                    Rect_Select_Click(this, new EventArgs());
                else
                    DrawRect_Click(this, new EventArgs());
            }
            if (Shift && e.KeyCode == Keys.E)
            {
                DrawEllipse_Click(this, new EventArgs());
            }
            if (Shift && e.KeyCode == Keys.B)
            {
                if (currentTool == DrawingSpace.Tool.Bucket)
                    Bucket_Click(this, new EventArgs());
                else
                    Brush_Click(this, new EventArgs());
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DrawUpdater();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
                Control = false;
                Shift = false;
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            infoForm ifr = new infoForm();
            ifr.ShowDialog();
        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GaussianWindow gw = new GaussianWindow(this);
            gw.ShowDialog();
        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(Space.SelectedLayerW.LayerImage.Width, Space.SelectedLayerW.LayerImage.Height);
            bmp.MakeTransparent();
            using (Graphics gdx = Graphics.FromImage(bmp))
            {
                gdx.DrawImage(Space.SelectedLayerW.LayerImage, new Point(0, 0));
            }
            bmp = Effects.ColorEffects.Grayscale(bmp);
            using (Graphics gdx = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(bmp, new Point(0, 0));
            }
            DrawUpdater(true);
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(Space.SelectedLayerW.LayerImage.Width, Space.SelectedLayerW.LayerImage.Height);
            bmp.MakeTransparent();
            using (Graphics gdx = Graphics.FromImage(bmp))
            {
                gdx.DrawImage(Space.SelectedLayerW.LayerImage, new Point(0, 0));
            }
            bmp = Effects.ColorEffects.SepiaImage(bmp);
            using (Graphics gdx = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(bmp, new Point(0, 0));
            }
            DrawUpdater(true);
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            fd.DragBegin(sender, e);
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            fd.ExecuteDrag(sender, e);
            Bitmap bmp = (Bitmap)fd.Image;
            using (Graphics gdx = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(bmp, new Point(0, 0));
            }
            DrawUpdater(true);
        }

        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(Space.SelectedLayerW.LayerImage.Width, Space.SelectedLayerW.LayerImage.Height);
            bmp.MakeTransparent();
            using (Graphics gdx = Graphics.FromImage(bmp))
            {
                gdx.DrawImage(Space.SelectedLayerW.LayerImage, new Point(0, 0));
            }
            bmp = Effects.ColorEffects.Negative(bmp);
            using (Graphics gdx = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                gdx.DrawImage(bmp, new Point(0, 0));
            }
            DrawUpdater(true);
        }

        private void colorBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BalanceWindow bw = new BalanceWindow(this);
            bw.ShowDialog();
        }

        private void erosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErodeFilter ef = new ErodeFilter(this);
            ef.ShowDialog();
        }

        private void medianFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Median md = new Median(this);
            md.ShowDialog();
        }

        private void gradientDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GradientDetect gd = new GradientDetect(this);
            gd.ShowDialog();
        }

        private void makeBitonalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitonal bt = new Bitonal(this);
            bt.ShowDialog();
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)ds.SelectedLayerW.LayerImage.Clone();
            bmp = Effects.ColorEffects.ColorFilter(bmp, Effects.ColorFilterType.Red);
            using(Graphics g = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                g.DrawImage(bmp, new Point(0, 0));
                g.Flush();
            }
            DrawUpdater(true);
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)ds.SelectedLayerW.LayerImage.Clone();
            bmp = Effects.ColorEffects.ColorFilter(bmp, Effects.ColorFilterType.Green);
            using (Graphics g = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                g.DrawImage(bmp, new Point(0, 0));
                g.Flush();
            }
            DrawUpdater(true);
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)ds.SelectedLayerW.LayerImage.Clone();
            bmp = Effects.ColorEffects.ColorFilter(bmp, Effects.ColorFilterType.Blue);
            using (Graphics g = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                g.DrawImage(bmp, new Point(0, 0));
                g.Flush();
            }
            DrawUpdater(true);
        }

        private void contrastBrightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brightness_Contrast bc = new Brightness_Contrast(this);
            bc.ShowDialog();
        }

        private void adjustGammaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gamma gm = new gamma(this);
            gm.ShowDialog();
        }

        #endregion

        #region combo_palette

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "Smooth")
            {
                if (comboBox1.Text == "Circular") b_mode = BrushType.Circle_smooth;
                else if (comboBox1.Text == "Rectangular") b_mode = BrushType.Rect_smooth;
            }
            else if (comboBox2.Text == "Sharp")
            {
                if (comboBox1.Text == "Circular") b_mode = BrushType.Circle_sharp;
                if (comboBox1.Text == "Rectangular") b_mode = BrushType.Rect_sharp;
            }
            DrawUpdater();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1_SelectedIndexChanged(this, e);
            DrawUpdater();
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            ChooseState = true;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                PickerMode = ColorMode.ForeColor;
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                PickerMode = ColorMode.BackColor;
        }

        private void pictureBox5_MouseUp(object sender, MouseEventArgs e)
        {
            ChooseState = false;
        }

        public enum ColorMode
        {
            ForeColor, BackColor, Empty
        }

        private void pictureBox5_MouseMove(object sender, MouseEventArgs e)
        {
            if (ChooseState)
            {
                try
                {
                    Color pickedColor = color_bmp.GetPixel(e.X, e.Y);
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        ForeColorUpdate(pickedColor);
                        PCloc.X = e.X - 2; PCloc.Y = e.Y - 2;
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        BackColorUpdate(pickedColor);
                        SCloc.X = e.X - 2; SCloc.Y = e.Y - 2;
                    }
                }
                catch (Exception ex)
                {
                     Message(ex.Message, Form1.MessageType.Error);
                }
                pictureBox5.Invalidate();
            }
        }

        private void pictureBox5_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawEllipse(new Pen(new SolidBrush(Color.Black)), new Rectangle(PCloc.X, PCloc.Y, 4, 4));
            g.DrawEllipse(new Pen(new SolidBrush(Color.Gray)), new Rectangle(SCloc.X, SCloc.Y, 4, 4));
        }

        #endregion

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (AllocatedTextSpace != Rectangle.Empty && AllocatedTextSpace.Width <= 0 || AllocatedTextSpace.Height <= 0)
            {
                using (Graphics gx = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
                {
                    gx.DrawString(textBox1.Text, TIB.ChoosenFont, mainBrush, SLoc);
                }
                textBox1.Text = "";
                TIB.Hide();
                DrawUpdater(true);
                AllocatedTextSpace = Rectangle.Empty;
            }
        }

        private void toolStrip2_MouseDown(object sender, MouseEventArgs e)
        {
            Form1_MouseDown(this, e);
        }

        private void toolStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            Form1_MouseDown(this, e);
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            Form1_MouseDown(this, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GCP.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Graphics gsp = pictureBox6.CreateGraphics();
                gsp.FillRectangle(GCP.GradientBrush, pictureBox6.ClientRectangle);
                gsp.Dispose();

                GI = new GradientInformation();
                GI.positions = GCP.Positions;
                GI.type = GCP.GType;
                GI.colors = GCP.Colors;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DeselectAll();
            toolStripButton1.Checked = true;
            currentTool = DrawingSpace.Tool.Gradient;
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.png;*.tiff;*.bmp";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (Graphics gp = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
                {
                    gp.DrawImageUnscaled(new Bitmap(ofd.FileName), new Point(0, 0));
                }
                DrawUpdater(true);
            }
        }

        
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            layerContainer1.DuplicateLayer();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Show();
        }

        private void newLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton25_Click(this, e);
        }

        private void rotate90ClockwiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PointF centerOffset = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            Bitmap tempbmp = DrawingSpace.Rotate(Space.SelectedLayerW.LayerImage, 0.5, centerOffset);

            using (Graphics gp = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                gp.Clear(Color.White);
                Space.SelectedLayerW.LayerImage.MakeTransparent(Color.White);
            }
            using (Graphics gp = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                gp.DrawImageUnscaled(tempbmp, new Point(0, 0));
            }
            DrawUpdater(true);
        }

        private void rotate90CounterclockwiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PointF centerOffset = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            Bitmap tempbmp = DrawingSpace.Rotate(Space.SelectedLayerW.LayerImage, -0.5, centerOffset);

            using (Graphics gp = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                gp.Clear(Color.White);
                Space.SelectedLayerW.LayerImage.MakeTransparent(Color.White);
            }
            using (Graphics gp = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                gp.DrawImageUnscaled(tempbmp, new Point(0, 0));
            }
            DrawUpdater(true);
        }

        private void rotate180ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PointF centerOffset = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            Bitmap tempbmp = DrawingSpace.Rotate(Space.SelectedLayerW.LayerImage, 1, centerOffset);

            using (Graphics gp = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                gp.Clear(Color.White);
                Space.SelectedLayerW.LayerImage.MakeTransparent(Color.White);
            }
            using (Graphics gp = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                gp.DrawImageUnscaled(tempbmp, new Point(0, 0));
            }
            DrawUpdater(true);
        }

        private void flipVerticallyToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            DrawUpdater(true);
        }

        private void flipHorizontallyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            DrawUpdater(true);
        }

        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(this, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton29_Click(this, e);
        }

        private void brushToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brush_Click(this, e);
        }

        private void colorPickerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorPicker_Click(this, e);
        }

        private void drawLinecurveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Line_Click(this, e);
        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawEllipse_Click(this, e);
        }

        private void eraserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rubber_Click(this, e);
        }

        private void fillBucketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bucket_Click(this, e);
        }

        private void measureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Measure_Click(this, e);
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Move_Click(this, e);
        }

        private void pencilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pencil_Click(this, e);
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawRect_Click(this, e);
        }

        private void rotateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Rotate_Click(this, e);
        }

        private void rectangularSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rect_Select_Click(this, e);
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Text_B_Click(this, e);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(ds.FinalTransparent);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                using (Graphics g = Graphics.FromImage(ds.SelectedLayerW.LayerImage))
                {
                    g.DrawImageUnscaled(Clipboard.GetImage(), new Point(0, 0));
                }
            }
            DrawUpdater(true);
        }

        private void rotate90ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size tmps = new Size(pictureBox1.Height, pictureBox1.Width);
            ImageSize = tmps;
            Space.ResizeCanvas(tmps, false);
            int tmpW = resizedImageSize.Width;
            resizedImageSize.Width = resizedImageSize.Height;
            resizedImageSize.Height = tmpW;
            pictureBox1.Size = tmps;

            foreach (DrawingSpace.Layer lrs in Space.Layers)
            {
                if (Space.Layers.IndexOf(lrs) == 0) continue;

                Bitmap bmp = (Bitmap)lrs.LayerImage.Clone();
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);

                using (Graphics g = Graphics.FromImage(lrs.LayerImage))
                {
                    g.DrawImage(bmp, new Point(0, 0));
                }
            }

            DrawUpdater(true);
        }

        private void pixelateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Pixelate pxl = new Pixelate(this);
            pxl.ShowDialog();
        }

        private void pixelDistortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PixelDistort pxl = new PixelDistort(this);
            pxl.ShowDialog();
        }

        private void stainedGlassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StainedGlass sg = new StainedGlass(this);
            sg.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            infoForm ifr = new infoForm();
            ifr.ShowDialog();
        }

        private void rotate90CounterclockwisrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size tmps = new Size(pictureBox1.Height, pictureBox1.Width);
            ImageSize = tmps;
            Space.ResizeCanvas(tmps, false);
            int tmpW = resizedImageSize.Width;
            resizedImageSize.Width = resizedImageSize.Height;
            resizedImageSize.Height = tmpW;
            pictureBox1.Size = tmps;

            foreach (DrawingSpace.Layer lrs in Space.Layers)
            {
                if (Space.Layers.IndexOf(lrs) == 0) continue;

                Bitmap bmp = (Bitmap)lrs.LayerImage.Clone();
                bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);

                using (Graphics g = Graphics.FromImage(lrs.LayerImage))
                {
                    g.DrawImage(bmp, new Point(0, 0));
                }
            }

            DrawUpdater(true);
        }

        private void rotate180ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DrawingSpace.Layer lrs in Space.Layers)
            {
                if (Space.Layers.IndexOf(lrs) == 0) continue;

                Bitmap bmp = (Bitmap)lrs.LayerImage.Clone();
                bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);

                using (Graphics g = Graphics.FromImage(lrs.LayerImage))
                {
                    g.DrawImage(bmp, new Point(0, 0));
                }
            }

            DrawUpdater(true);
        }

        private void pictureBox7_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int x = 30; x < pictureBox7.Width; x += 100)
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Gray), 3.0f), new Point(x, 0), new Point(x, 20));
            }
            for (int x = 30; x < pictureBox7.Width; x += 50)
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Gray), 2.0f), new Point(x, 0), new Point(x, 10));
            }
            for (int x = 30; x < pictureBox7.Width; x += 10)
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Gray), 1.0f), new Point(x, 0), new Point(x, 5));
            }

            g.DrawLine(new Pen(new SolidBrush(Color.Red), 2.0f), new Point(rulerLoc.X, 0), new Point(rulerLoc.X, 10));
        }

        private void pictureBox8_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int x = 1; x < pictureBox8.Height; x += 100)
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Gray), 3.0f), new Point(0, x), new Point(20, x));
            }
            for (int x = 1; x < pictureBox8.Height; x += 50)
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Gray), 2.0f), new Point(0, x), new Point(10, x));
            }
            for (int x = 1; x < pictureBox8.Height; x += 10)
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Gray), 1.0f), new Point(0, x), new Point(5, x));
            }

            g.DrawLine(new Pen(new SolidBrush(Color.Blue), 2.0f), new Point(0, rulerLoc.Y), new Point(10, rulerLoc.Y));
        }

        private void addNoiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Noise ns = new Noise(this);
            ns.ShowDialog();
        }

        private void addJitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jitter jt = new jitter(this);
            jt.ShowDialog();
        }

        private void redEyeRemovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WorkingSpace != Rectangle.Empty)
            {
                for (int x = WorkingSpace.X; x < WorkingSpace.X + WorkingSpace.Width; x++)
                {
                    for (int y = WorkingSpace.Y; y < WorkingSpace.Y + WorkingSpace.Height; y++)
                    {
                        Color clr = Space.SelectedLayerW.LayerImage.GetPixel(x, y);

                        float intensity = ((float)clr.R / ((clr.G + clr.B) / 2));
                        if (intensity > 1.5f)
                        {
                            Space.SelectedLayerW.LayerImage.SetPixel(x, y, Color.FromArgb(255, (clr.B + clr.G) / 2, clr.G, clr.B));
                        }
                    }
                }
                DrawUpdater(true);
            }
            else
            {
                Message("Select the eyes first", MessageType.Error);
            }
        }

        private void flipVerticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DrawingSpace.Layer lrs in Space.Layers)
            {
                if (Space.Layers.IndexOf(lrs) == 0) continue;

                Bitmap bmp = (Bitmap)lrs.LayerImage.Clone();
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

                using (Graphics g = Graphics.FromImage(lrs.LayerImage))
                {
                    g.DrawImage(bmp, new Point(0, 0));
                }
            }

            DrawUpdater(true);
        }

        private void flipHorizontallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DrawingSpace.Layer lrs in Space.Layers)
            {
                if (Space.Layers.IndexOf(lrs) == 0) continue;

                Bitmap bmp = (Bitmap)lrs.LayerImage.Clone();
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);

                using (Graphics g = Graphics.FromImage(lrs.LayerImage))
                {
                    g.DrawImage(bmp, new Point(0, 0));
                }
            }

            DrawUpdater(true);
        }

        private void flattenImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmps = new Bitmap(Space.Final.Width, Space.Final.Height);
            bmps.MakeTransparent();

            foreach (DrawingSpace.Layer lrs in Space.Layers)
            {
                if (Space.Layers.IndexOf(lrs) == 0) continue;
                if (!lrs.Visible) continue;

                using (Graphics g = Graphics.FromImage(bmps))
                {
                    g.DrawImageUnscaled(lrs.LayerImage, new Point(0, 0));
                }
            }

            layerContainer1.RemoveAllLayers();

            for (int x = Space.Layers.Count - 1; x >= 1; x--)
            {
                Space.LayerRemove(Space.Layers[x].LayerName);
            }

            if (Space.LayerLocate("Flattened").LayerName == null)
            {
                Space.CreateLayer("Flattened", Space.Final.Size, true);
                layerContainer1.AddLayer("Flattened", true);
            }
            else
            {
                Random rnd = new Random();
                string name = "#" + rnd.Next(0, 1000000).ToString();
                Space.CreateLayer(name, Space.Final.Size, true);
                layerContainer1.AddLayer(name, true);
            }

            Space.SelectedLayerW = Space.LayerLocate("Flattened");

            using (Graphics g = Graphics.FromImage(Space.SelectedLayerW.LayerImage))
            {
                g.DrawImageUnscaled(bmps, new Point(0, 0));
            }

            DrawUpdater(true);
        }

        private void oilPaintingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OilPaint op = new OilPaint(this);
            op.ShowDialog();
        }

        private void pencilDrawingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cartoonEffectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CartoonFilter cf = new CartoonFilter(this);
            cf.ShowDialog();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenButton_Click(this, e);
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            new_window nw = new new_window();
            if (nw.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Text = "Mainpaint | " + nw.ImageName;
                ImageSize = nw.ImageSize;
                resizedImageSize = nw.ImageSize;
                pictureBox1.Size = nw.ImageSize;
                ds = new DrawingSpace(nw.ImageSize);
                layerContainer1.RemoveAllLayers();
                Space.CreateLayer("Background", nw.ImageSize, true);
                layerContainer1.AddLayer("Background", true);
                Space.SelectedLayerW = Space.Layers[1];
                using (Graphics g = Graphics.FromImage(Space.Layers[1].LayerImage))
                {
                    g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, nw.ImageSize.Width, nw.ImageSize.Height));
                }
            }
        }
    }
}
