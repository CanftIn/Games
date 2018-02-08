namespace Mainpaint
{
    partial class TextInputBox
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.checkButton4 = new Mainpaint.Controls.CheckButton();
            this.checkButton3 = new Mainpaint.Controls.CheckButton();
            this.checkButton2 = new Mainpaint.Controls.CheckButton();
            this.checkButton1 = new Mainpaint.Controls.CheckButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkButton4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkButton1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(8, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Font:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(50, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(273, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(8, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Font size:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(76, 31);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(72, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // checkButton4
            // 
            this.checkButton4.Checked = false;
            this.checkButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkButton4.Image = global::Mainpaint.Properties.Resources.underline1;
            this.checkButton4.Location = new System.Drawing.Point(244, 29);
            this.checkButton4.Name = "checkButton4";
            this.checkButton4.Size = new System.Drawing.Size(24, 24);
            this.checkButton4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.checkButton4.TabIndex = 7;
            this.checkButton4.TabStop = false;
            this.checkButton4.Click += new System.EventHandler(this.checkButton4_Click);
            // 
            // checkButton3
            // 
            this.checkButton3.Checked = false;
            this.checkButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkButton3.Image = global::Mainpaint.Properties.Resources.strike1;
            this.checkButton3.Location = new System.Drawing.Point(214, 29);
            this.checkButton3.Name = "checkButton3";
            this.checkButton3.Size = new System.Drawing.Size(24, 24);
            this.checkButton3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.checkButton3.TabIndex = 6;
            this.checkButton3.TabStop = false;
            this.checkButton3.Click += new System.EventHandler(this.checkButton3_Click);
            // 
            // checkButton2
            // 
            this.checkButton2.Checked = false;
            this.checkButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkButton2.Image = global::Mainpaint.Properties.Resources.italic2;
            this.checkButton2.Location = new System.Drawing.Point(184, 29);
            this.checkButton2.Name = "checkButton2";
            this.checkButton2.Size = new System.Drawing.Size(24, 24);
            this.checkButton2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.checkButton2.TabIndex = 5;
            this.checkButton2.TabStop = false;
            this.checkButton2.Click += new System.EventHandler(this.checkButton2_Click);
            // 
            // checkButton1
            // 
            this.checkButton1.Checked = false;
            this.checkButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkButton1.Image = global::Mainpaint.Properties.Resources.bold3;
            this.checkButton1.ImageLocation = "";
            this.checkButton1.Location = new System.Drawing.Point(154, 29);
            this.checkButton1.Name = "checkButton1";
            this.checkButton1.Size = new System.Drawing.Size(24, 24);
            this.checkButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.checkButton1.TabIndex = 4;
            this.checkButton1.TabStop = false;
            this.checkButton1.Click += new System.EventHandler(this.checkButton1_Click);
            // 
            // TextInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 64);
            this.Controls.Add(this.checkButton4);
            this.Controls.Add(this.checkButton3);
            this.Controls.Add(this.checkButton2);
            this.Controls.Add(this.checkButton1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TextInputBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TextInputBox";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.TextInputBox_Load);
            this.MouseEnter += new System.EventHandler(this.TextInputBox_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.TextInputBox_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkButton4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkButton1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private Controls.CheckButton checkButton1;
        private Controls.CheckButton checkButton2;
        private Controls.CheckButton checkButton3;
        private Controls.CheckButton checkButton4;
    }
}