using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Mainpaint
{
    namespace misc
    {
        public class FileDrag
        {
            protected Thread loader;
            protected bool validData;
            string path;
            protected Bitmap image;

            private bool GetFile(out string filename, DragEventArgs e)
            {
                bool ret = false;
                filename = String.Empty;
                if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
                {
                    Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                    if (data != null)
                    {
                        if ((data.Length == 1) && (data.GetValue(0) is String))
                        {
                            filename = ((string[])data)[0];
                            string ext = Path.GetExtension(filename).ToLower();
                            if (ext == ".jpg" || ext == ".png" || ext == ".bmp")
                            {
                                ret = true;
                            }
                        }
                    }
                }

                return ret;
            }

            public void DragBegin(object sender, DragEventArgs e)
            {
                string file;
                validData = GetFile(out file, e);
                if (validData)
                {
                    path = file;
                    loader = new Thread(new ThreadStart(LoadImage));
                    loader.Start();
                    e.Effect = DragDropEffects.Copy;
                }
                else
                    e.Effect = DragDropEffects.None;
            }

            protected void LoadImage()
            {
                image = new Bitmap(path);
            }

            public Bitmap Image
            {
                get
                {
                    return image;
                }
            }

            public void ExecuteDrag(object sender, DragEventArgs e)
            {
                if (validData)
                {
                    while (loader.IsAlive)
                    {
                        Application.DoEvents();
                        Thread.Sleep(0);
                    }
                }
            }
        }
    }
}
