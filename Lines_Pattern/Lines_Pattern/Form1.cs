using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lines_Pattern
{
    public partial class Form1 : Form
    {

        Pen myPen = new Pen(Color.Black);
        Graphics g;

        static int center_X, center_Y;
        static int start_X, start_Y;
        static int end_X, end_Y;

        static int my_angle = 0;
        static int my_length = 0;
        static int my_increment = 0;



        static int num_lines = 0;

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            myPen.Width = 1;
            my_length = Int32.Parse(length.Text);
            g = canvas.CreateGraphics();
            for(int i = 0; i < Int32.Parse(lines.Text); i++)
                drawLine();
            /*
            Point[] points =
            {
                new Point(start_X, start_Y),
                new Point(start_X + 100, start_Y + 100)
            };
            g = e.Graphics;
            g.DrawLines(myPen, points);
            */
        }

        public Form1()
        {
            InitializeComponent();
            start_X = canvas.Width / 2;
            start_Y = canvas.Height / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            my_length = Int32.Parse(length.Text);
            my_increment = Int32.Parse(increment.Text);
            my_angle = Int32.Parse(angle.Text);

            start_X = canvas.Width / 2;
            start_Y = canvas.Height / 2;

            canvas.Refresh();
        }

        private void drawLine()
        {
            Random randomGen = new Random();
            myPen.Color = Color.FromArgb(randomGen.Next(255), randomGen.Next(255), randomGen.Next(255));
            my_angle = my_angle + Int32.Parse(angle.Text);
            my_length = my_length + Int32.Parse(increment.Text);

            end_X = (int)(start_X + Math.Cos(my_angle * .017453292519) * my_length);
            end_Y = (int)(start_Y + Math.Sin(my_angle * .017453292519) * my_length);

            Point[] points =
            {
                new Point(start_X, start_Y),
                new Point(end_X, end_Y)
            };

            start_X = end_X;
            start_Y = end_Y;

            g.DrawLines(myPen, points);
        }
    }
}
