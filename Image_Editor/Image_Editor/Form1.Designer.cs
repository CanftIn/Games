namespace Image_Editor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxMain = new System.Windows.Forms.PictureBox();
            this.none = new System.Windows.Forms.Button();
            this.gray = new System.Windows.Forms.Button();
            this.Sepia = new System.Windows.Forms.Button();
            this.Artistic = new System.Windows.Forms.Button();
            this.Spike = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.Frozen = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.redbar = new System.Windows.Forms.TrackBar();
            this.greenbar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.bluebar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.btnOpenImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bluebar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxMain
            // 
            this.pictureBoxMain.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxMain.Name = "pictureBoxMain";
            this.pictureBoxMain.Size = new System.Drawing.Size(790, 608);
            this.pictureBoxMain.TabIndex = 0;
            this.pictureBoxMain.TabStop = false;
            this.pictureBoxMain.Click += new System.EventHandler(this.pictureBoxMain_Click);
            // 
            // none
            // 
            this.none.Location = new System.Drawing.Point(794, 12);
            this.none.Name = "none";
            this.none.Size = new System.Drawing.Size(102, 36);
            this.none.TabIndex = 1;
            this.none.Text = "none";
            this.none.UseVisualStyleBackColor = true;
            // 
            // gray
            // 
            this.gray.Location = new System.Drawing.Point(794, 54);
            this.gray.Name = "gray";
            this.gray.Size = new System.Drawing.Size(102, 36);
            this.gray.TabIndex = 2;
            this.gray.Text = "gray";
            this.gray.UseVisualStyleBackColor = true;
            this.gray.Click += new System.EventHandler(this.gray_Click);
            // 
            // Sepia
            // 
            this.Sepia.Location = new System.Drawing.Point(794, 96);
            this.Sepia.Name = "Sepia";
            this.Sepia.Size = new System.Drawing.Size(102, 36);
            this.Sepia.TabIndex = 3;
            this.Sepia.Text = "Sepia";
            this.Sepia.UseVisualStyleBackColor = true;
            this.Sepia.Click += new System.EventHandler(this.Sepia_Click);
            // 
            // Artistic
            // 
            this.Artistic.Location = new System.Drawing.Point(794, 138);
            this.Artistic.Name = "Artistic";
            this.Artistic.Size = new System.Drawing.Size(102, 36);
            this.Artistic.TabIndex = 4;
            this.Artistic.Text = "Artistic";
            this.Artistic.UseVisualStyleBackColor = true;
            this.Artistic.Click += new System.EventHandler(this.Artistic_Click);
            // 
            // Spike
            // 
            this.Spike.Location = new System.Drawing.Point(794, 180);
            this.Spike.Name = "Spike";
            this.Spike.Size = new System.Drawing.Size(102, 36);
            this.Spike.TabIndex = 5;
            this.Spike.Text = "Spike";
            this.Spike.UseVisualStyleBackColor = true;
            this.Spike.Click += new System.EventHandler(this.Spike_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(794, 222);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(102, 36);
            this.button6.TabIndex = 6;
            this.button6.Text = "flash";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // Frozen
            // 
            this.Frozen.Location = new System.Drawing.Point(794, 264);
            this.Frozen.Name = "Frozen";
            this.Frozen.Size = new System.Drawing.Size(102, 36);
            this.Frozen.TabIndex = 7;
            this.Frozen.Text = "Frozen";
            this.Frozen.UseVisualStyleBackColor = true;
            this.Frozen.Click += new System.EventHandler(this.Frozen_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(794, 306);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(102, 36);
            this.button8.TabIndex = 8;
            this.button8.Text = "filter1";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(794, 348);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(102, 36);
            this.button9.TabIndex = 9;
            this.button9.Text = "filter2";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(794, 390);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(102, 36);
            this.button10.TabIndex = 10;
            this.button10.Text = "filter3";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Mistral", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 627);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 29);
            this.label1.TabIndex = 11;
            this.label1.Text = "Red";
            // 
            // redbar
            // 
            this.redbar.Location = new System.Drawing.Point(89, 627);
            this.redbar.Name = "redbar";
            this.redbar.Size = new System.Drawing.Size(701, 45);
            this.redbar.TabIndex = 12;
            this.redbar.Scroll += new System.EventHandler(this.redbar_Scroll);
            this.redbar.ValueChanged += new System.EventHandler(this.redbar_ValueChanged);
            // 
            // greenbar
            // 
            this.greenbar.Location = new System.Drawing.Point(89, 678);
            this.greenbar.Name = "greenbar";
            this.greenbar.Size = new System.Drawing.Size(701, 45);
            this.greenbar.TabIndex = 14;
            this.greenbar.ValueChanged += new System.EventHandler(this.greenbar_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Mistral", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 678);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 29);
            this.label2.TabIndex = 13;
            this.label2.Text = "Green";
            // 
            // bluebar
            // 
            this.bluebar.Location = new System.Drawing.Point(89, 729);
            this.bluebar.Name = "bluebar";
            this.bluebar.Size = new System.Drawing.Size(701, 45);
            this.bluebar.TabIndex = 16;
            this.bluebar.ValueChanged += new System.EventHandler(this.bluebar_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Mistral", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 729);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 29);
            this.label3.TabIndex = 15;
            this.label3.Text = "Blue";
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Location = new System.Drawing.Point(797, 627);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(99, 53);
            this.btnSaveImage.TabIndex = 17;
            this.btnSaveImage.Text = "Save Image";
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // btnOpenImage
            // 
            this.btnOpenImage.Location = new System.Drawing.Point(797, 699);
            this.btnOpenImage.Name = "btnOpenImage";
            this.btnOpenImage.Size = new System.Drawing.Size(99, 48);
            this.btnOpenImage.TabIndex = 18;
            this.btnOpenImage.Text = "Open Image";
            this.btnOpenImage.UseVisualStyleBackColor = true;
            this.btnOpenImage.Click += new System.EventHandler(this.btnOpenImage_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 783);
            this.Controls.Add(this.btnOpenImage);
            this.Controls.Add(this.btnSaveImage);
            this.Controls.Add(this.bluebar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.greenbar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.redbar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.Frozen);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.Spike);
            this.Controls.Add(this.Artistic);
            this.Controls.Add(this.Sepia);
            this.Controls.Add(this.gray);
            this.Controls.Add(this.none);
            this.Controls.Add(this.pictureBoxMain);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Image Editor";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bluebar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxMain;
        private System.Windows.Forms.Button none;
        private System.Windows.Forms.Button gray;
        private System.Windows.Forms.Button Sepia;
        private System.Windows.Forms.Button Artistic;
        private System.Windows.Forms.Button Spike;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button Frozen;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar redbar;
        private System.Windows.Forms.TrackBar greenbar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar bluebar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSaveImage;
        private System.Windows.Forms.Button btnOpenImage;
    }
}

