namespace Calc
{
    partial class Calc
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
            this.btnOp = new System.Windows.Forms.Button();
            this.InputExp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOp
            // 
            this.btnOp.Location = new System.Drawing.Point(63, 64);
            this.btnOp.Name = "btnOp";
            this.btnOp.Size = new System.Drawing.Size(120, 23);
            this.btnOp.TabIndex = 1;
            this.btnOp.Text = "运算";
            this.btnOp.UseVisualStyleBackColor = true;
            this.btnOp.Click += new System.EventHandler(this.btnOp_Click);
            // 
            // InputExp
            // 
            this.InputExp.Location = new System.Drawing.Point(63, 28);
            this.InputExp.Name = "InputExp";
            this.InputExp.Size = new System.Drawing.Size(120, 20);
            this.InputExp.TabIndex = 2;
            // 
            // Calc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 130);
            this.Controls.Add(this.InputExp);
            this.Controls.Add(this.btnOp);
            this.Name = "Calc";
            this.Text = "Calc";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Diagnostics.Process process1;
        private System.Windows.Forms.Button btnOp;
        private System.Windows.Forms.TextBox InputExp;
    }
}

