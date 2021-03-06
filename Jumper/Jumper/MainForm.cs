﻿using Jumper.Screens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jumper
{
    public partial class MainForm : Form, IGameScreenController
    {
        private Bitmap bitmap = null;
        private Graphics graphics = null;
        private GameScreen currentScreen = null;
        private GameSettings settings = new GameSettings();

        public MainForm()
        {
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            this.ClientSize = new Size(400, 600);

            this.bitmap = new Bitmap(400, 600);
            this.graphics = Graphics.FromImage(this.bitmap);
            this.graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            (this as IGameScreenController).OpenScreen(typeof(StartScreen));
        }
        #region IGameScreenController Members

        Graphics IGameScreenController.BufferGraphics
        {
            get
            {
                return this.graphics;
            }
        }

        Size IGameScreenController.BufferSize
        {
            get
            {
                return this.bitmap.Size;
            }
        }

        Form IGameScreenController.GameForm
        {
            get
            {
                return this;
            }
        }

        GameSettings IGameScreenController.Settings
        {
            get
            {
                return this.settings;
            }
        }

        void IGameScreenController.Flush()
        {
            this.Refresh();
        }

        void IGameScreenController.OpenScreen(Type screenType, params object[] arguments)
        {
            if (this.currentScreen != null)
            {
                this.currentScreen.Dispose();
            }
            this.currentScreen = (GameScreen)screenType.GetConstructor(arguments.Select(o => o.GetType()).ToArray()).Invoke(arguments);
            this.currentScreen.AttachController(this);
        }

        #endregion

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(this.bitmap, 0, 0);
            // Create pen.
            Pen blackPen = new Pen(Color.Red, 3);

            // Create location and size of ellipse.
            float x = 0.0F;
            float y = 0.0F;
            float width = 400.0F;
            float height = 500.0F;

            // Draw ellipse to screen.
            e.Graphics.DrawEllipse(blackPen, x, y, width, height);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.currentScreen != null)
            {
                this.currentScreen.Dispose();
            }
            this.currentScreen = null;
        }
    }
}
