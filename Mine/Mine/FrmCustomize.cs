using System;
using System.Windows.Forms;

namespace Mine
{
    public partial class FrmCustomize : Form
    {
        public FrmCustomize()
        {
            InitializeComponent();
        }

        private void FrmCustomize_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            txtWidth.Text = Properties.Settings.Default.Width.ToString();
            txtHeight.Text = Properties.Settings.Default.Height.ToString();
            txtMinesCount.Text = Properties.Settings.Default.MinesCount.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int width, height,minesCount;

            if (int.TryParse(txtWidth.Text, out width))
            {
                if (width > Game.MaxWidth)
                    width = Game.MaxWidth;
                else if (width < Game.MinWidth)
                    width = Game.MinWidth;

                Properties.Settings.Default.Width = width;
            }

            if (int.TryParse(txtHeight.Text, out height))
            {
                if (height > Game.MaxHeight)
                    height = Game.MaxHeight;
                else if (height < Game.MinHeight)
                    height = Game.MinHeight;

                Properties.Settings.Default.Height = height;
            }

            if (int.TryParse(txtMinesCount.Text, out minesCount))
            {
                if (minesCount < Game.MinMinesCount)
                    minesCount = Game.MinMinesCount;
                else if (minesCount > (width - 1) * (height - 1))
                    minesCount = (width - 1) * (height - 1);

                Properties.Settings.Default.MinesCount = minesCount;
            }

            Properties.Settings.Default.Save();

            DialogResult = DialogResult.OK;
        }
    }
}
