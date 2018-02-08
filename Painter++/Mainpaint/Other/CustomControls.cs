using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Mainpaint
{
    namespace Controls
    {
        public class CheckButton : PictureBox
        {
            private bool __checked = false;

            protected override void OnPaint(PaintEventArgs pe)
            {
                base.OnPaint(pe);
            }

            protected override void OnPaintBackground(PaintEventArgs pevent)
            {
                base.OnPaintBackground(pevent);
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(new Rectangle(0, 0, this.Width, this.Height));
                Region rg = new System.Drawing.Region(gp);
                this.Region = rg;
                Graphics g = pevent.Graphics;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                if (!this.Checked)
                    g.FillRectangle(new LinearGradientBrush(new Point(this.Size.Width / 2, 0), new Point(this.Size.Width / 2, this.Size.Height), Color.FromArgb(255, 131, 217, 255), Color.FromArgb(255, 89, 120, 255)), this.ClientRectangle);
                else
                    g.FillRectangle(new LinearGradientBrush(new Point(this.Size.Width / 2, this.Size.Height), new Point(this.Size.Width / 2, 0), Color.FromArgb(255, 131, 217, 255), Color.FromArgb(255, 89, 120, 255)), this.ClientRectangle);

            }

            public bool Checked
            {
                get
                {
                    return __checked;
                }
                set
                {
                    __checked = value;
                    this.Invalidate();
                }
            }

            protected override void OnMouseClick(MouseEventArgs e)
            {
                base.OnMouseClick(e);
                this.Checked = (!__checked) ? true : false;
            }
        }

        public class TextInputBox : RichTextBox
        {
            private Bitmap _back;

            public TextInputBox()
            {
                this.BackColor = Color.White;
                _back = new Bitmap(this.Width, this.Height);
                using (Graphics g = Graphics.FromImage(_back))
                {
                    g.Clear(this.BackColor);
                }
            }

            public Bitmap _Render
            {
                get
                {
                    return _back;
                }
                set
                {
                    _back = value;
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                Graphics g = e.Graphics;
                g.DrawImage(_back, new Point(0, 0));
            }

            private Bitmap Result()
            {
                Bitmap bmp = new Bitmap(this.Width, this.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(this.PointToScreen(Point.Empty), Point.Empty, this.Size);
                }
                return bmp;
            }

            public Bitmap RenderedResult
            {
                get
                {
                    return Result();
                }
            }
        }

        public class DoubleBufferedPictureBox : PictureBox
        {
            private double _increment = 1;
            public DoubleBufferedPictureBox()
            {
                SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            }

            protected override void OnInvalidated(InvalidateEventArgs e)
            {
                base.OnInvalidated(e);
            }

            protected override void OnMouseWheel(MouseEventArgs e)
            {
                base.OnMouseWheel(e);
                Form1 fr = (Form1)this.FindForm();
                if (e.Delta > 0)
                    _increment += 0.1;
                else
                    _increment -= 0.1;
                fr.Rescale(_increment);
            }

            protected override void OnKeyDown(KeyEventArgs e)
            {
                base.OnKeyDown(e);
                Form1 fr = (Form1)this.FindForm();
                fr.KeyDownCall(Form1.KeyEvent.key_down, e);
            }

            protected override void OnKeyUp(KeyEventArgs e)
            {
                base.OnKeyUp(e);
                Form1 fr = (Form1)this.FindForm();
                fr.KeyDownCall(Form1.KeyEvent.key_up, e);
            }
        }
    }
}
