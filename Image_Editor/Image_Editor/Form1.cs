using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_Editor
{
    public partial class Form1 : Form
    {
        Image file;
        Boolean opened = false;

        public Form1()
        {
            InitializeComponent();
        }

        //--------------------------------------------------
        // file control
        //--------------------------------------------------

        private void openImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "jpg (*.jpg)|*.jpg|bmp (*.bmp)|*.bmp|png (*.png)|*png|all (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName.Length > 0)
            {
                pictureBoxMain.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBoxMain.Image = Image.FromFile(ofd.FileName);
                file = Image.FromFile(ofd.FileName);
                //pictureBoxMain.Size = pictureBoxMain.Image.Size;
                opened = true;
            }
        }

        private void saveImage()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "jpg (*.jpg)|*.jpg|bmp (*.bmp)|*.bmp|png (*.png)|*.png";

            if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName.Length > 0)
            {
                pictureBoxMain.Image.Save(sfd.FileName);
                file = Image.FromFile(sfd.FileName);
            }
        }

        private void reload()
        {
            if (!opened)
            {
                // MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                if (opened)
                {
                    pictureBoxMain.Image = file;
                    opened = true;
                }
            }
        }


        /* 
           -----------------------------------------------------------Color Matrix Combinations---------------------------------------------------- 

            R G B A W            R G B A W             R   G   B   A   W

        R  [1 0 0 0 0]       R  [c 0 0 0 0]       R  [sr+s sr  sr  0   0]
        G  [0 1 0 0 0]       G  [0 c 0 0 0]       G  [ sg sg+s sg  0   0]
        B  [0 0 1 0 0]    X  B  [0 0 c 0 0]    X  B  [ sb  sb sb+s 0   0]
        A  [0 0 0 1 0]       A  [0 0 0 1 0]       A  [ 0   0   0   1   0]
        W  [b b b 0 1]       W  [t t t 0 1]       W  [ 0   0   0   0   1]

        Brightness Matrix     Contrast Matrix          Saturation Matrix


                        R      G      B      A      W

                 R  [c(sr+s) c(sr)  c(sr)    0      0   ]
                 G  [ c(sg) c(sg+s) c(sg)    0      0   ]
         ===>    B  [ c(sb)  c(sb) c(sb+s)   0      0   ]
                 A  [   0      0      0      1      0   ]
                 W  [  t+b    t+b    t+b     0      1   ]

                           Transformation Matrix

         */

        //----------------------------------------------
        // buttons function
        //----------------------------------------------

        void Basefunc(ColorMatrix pic)
        {
            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {

                Image img = pictureBoxMain.Image;                             // storing image into img variable of image type from pictureBoxMain
                Bitmap bmpInverted = new Bitmap(img.Width, img.Height);   /* creating a bitmap of the height of imported picture in picturebox which consists of the pixel data for a graphics image
                                                                        and its attributes. A Bitmap is an object used to work with images defined by pixel data.*/

                ImageAttributes ia = new ImageAttributes();                 //creating an object of imageattribute ia to change the attribute of images
                ColorMatrix cmPicture = pic;
                ia.SetColorMatrix(cmPicture);           //pass the color matrix to imageattribute object ia
                Graphics g = Graphics.FromImage(bmpInverted);   /*create a new object of graphics named g, ; Create graphics object for alteration.
                                                            Graphics newGraphics = Graphics.FromImage(imageFile); is the format of loading image into graphics for alteration*/

                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);


                /*   g.drawimage(image, new rectangle(location of rectangle axix-x, location axis-y, width of rectangle, height of rectangle),
               location of image in rectangle x-axis, location of image in rectangle y-axis, width of image, height of image,
               format of graphics unit,provide the image attributes   */


                g.Dispose();                            //Releases all resources used by this Graphics.
                pictureBoxMain.Image = bmpInverted;

            }
        }

        void grayscale()
        {
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{0.299f, 0.299f, 0.299f, 0, 0},
                new float[]{0.587f, 0.587f, 0.587f, 0, 0},
                new float[]{0.114f, 0.114f, 0.114f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 0}
                });
            Basefunc(cmPicture);
        }

        void redscale()
        {
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{.393f, .349f, .272f, 0, 0},
                new float[]{.769f, .686f, .534f, 0, 0},
                new float[]{.189f, .168f, .131f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            Basefunc(cmPicture);
        }

        void Winter()
        {
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{1,0,0,0,0},
                new float[]{0,1,0,0,0},
                new float[]{0,0,1,0,0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 1, 0, 1}
            });
            Basefunc(cmPicture);
        }

        void fog()
        {
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{1+0.3f, 0, 0, 0, 0},
                new float[]{0, 1+0.7f, 0, 0, 0},
                new float[]{0, 0, 1+1.3f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            Basefunc(cmPicture);
        }

        void flash()
        {
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{1+0.9f, 0, 0, 0, 0},
                new float[]{0, 1+1.5f, 0, 0, 0},
                new float[]{0, 0, 1+1.3f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            Basefunc(cmPicture);
        }

        void frozen()
        {
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{1+0.3f, 0, 0, 0, 0},
                new float[]{0, 1+0f, 0, 0, 0},
                new float[]{0, 0, 1+5f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            Basefunc(cmPicture);
        }

        void filter1()
        {
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{.393f, .349f, .272f+1.3f, 0, 0},
                new float[]{.769f, .686f+0.5f, .534f, 0, 0},
                new float[]{.189f+2.3f, .168f, .131f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            Basefunc(cmPicture);
        }

        void filter2()
        {
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{.393f, .349f+0.5f, .272f, 0, 0},
                new float[]{.769f+0.3f, .686f, .534f, 0, 0},
                new float[]{.189f, .168f, .131f+0.5f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            Basefunc(cmPicture);
        }

        void filter3()
        {
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{.393f+0.3f, .349f, .272f, 0, 0},
                new float[]{.769f, .686f+0.2f, .534f, 0, 0},
                new float[]{.189f, .168f, .131f+0.9f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            Basefunc(cmPicture);
        }

        void hue()
        {
            float changered = redbar.Value * 0.1f;
            float changegreen = greenbar.Value * 0.1f;
            float changeblue = bluebar.Value * 0.1f;

            reload();
            ColorMatrix cmPicture = new ColorMatrix(new float[][]
            {
                new float[]{1+changered, 0, 0, 0, 0},
                new float[]{0, 1+changegreen, 0, 0, 0},
                new float[]{0, 0, 1+changeblue, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            });
            Basefunc(cmPicture);
        }

        private void pictureBoxMain_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            openImage();
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            saveImage();
        }

        private void gray_Click(object sender, EventArgs e)
        {
            reload();
            grayscale();
        }

        private void Sepia_Click(object sender, EventArgs e)
        {
            reload();
            redscale();
        }

        private void Artistic_Click(object sender, EventArgs e)
        {
            reload();
            Winter();
        }

        private void Spike_Click(object sender, EventArgs e)
        {
            reload();
            fog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            reload();
            flash();
        }

        private void Frozen_Click(object sender, EventArgs e)
        {
            reload();
            frozen();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            reload();
            filter1();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            reload();
            filter2();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            reload();
            filter3();
        }

        private void redbar_Scroll(object sender, EventArgs e)
        {

        }

        private void redbar_ValueChanged(object sender, EventArgs e)
        {
            hue();
        }

        private void greenbar_ValueChanged(object sender, EventArgs e)
        {
            hue();
        }

        private void bluebar_ValueChanged(object sender, EventArgs e)
        {
            hue();
        }
    }
}
