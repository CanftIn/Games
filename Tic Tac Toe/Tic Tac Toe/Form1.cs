using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Form1 : Form
    {
        bool turn = false;
        int turn_count = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Designed by CanftIn", "Tic Tac Toe About");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (turn)
            {
                b.Text = "X";
            }
            else
            {
                b.Text = "O";
            }
                
            turn = !turn;
            b.Enabled = false;
            turn_count++;
            CheckForWinner();
        }

        private void CheckForWinner(/*Button btn*/)
        {
            //string str = btn.Name;
            //char ctmp = str[0];
            bool isWinner = false;
            /*
            if((A1.Text == A2.Text) && (A2.Text == A3.Text) && (!A1.Enabled))
            {
                isWinner = true;
            }
            if ((B1.Text == B2.Text) && (B2.Text == B3.Text) && (!B1.Enabled))
            {
                isWinner = true;
            }
            if ((C1.Text == C2.Text) && (C2.Text == C3.Text) && (!C1.Enabled))
            {
                isWinner = true;
            }

            if ((A1.Text == B1.Text) && (B1.Text == C1.Text) && (!A1.Enabled))
            {
                isWinner = true;
            }
            if ((A2.Text == B2.Text) && (B2.Text == C2.Text) && (!A2.Enabled))
            {
                isWinner = true;
            }
            if ((A3.Text == B3.Text) && (B3.Text == C3.Text) && (!A3.Enabled))
            {
                isWinner = true;
            }

            if ((A1.Text == B2.Text) && (B2.Text == C3.Text) && (!A1.Enabled))
            {
                isWinner = true;
            }
            if ((A3.Text == B2.Text) && (B2.Text == C1.Text) && (!A3.Enabled))
            {
                isWinner = true;
            }
            */

            if (((A1.Text == A2.Text) && (A2.Text == A3.Text) && (!A1.Enabled)) ||
                ((B1.Text == B2.Text) && (B2.Text == B3.Text) && (!B1.Enabled)) ||
                ((C1.Text == C2.Text) && (C2.Text == C3.Text) && (!C1.Enabled)) ||
                ((A1.Text == B1.Text) && (B1.Text == C1.Text) && (!A1.Enabled)) ||
                ((A2.Text == B2.Text) && (B2.Text == C2.Text) && (!A2.Enabled)) ||
                ((A3.Text == B3.Text) && (B3.Text == C3.Text) && (!A3.Enabled)) ||
                ((A1.Text == B2.Text) && (B2.Text == C3.Text) && (!A1.Enabled)) ||
                ((A3.Text == B2.Text) && (B2.Text == C1.Text) && (!A3.Enabled)))
            {
                isWinner = true;
            }

            if (isWinner)
            {
                disableButtons();
                string winner = "";
                if(turn)
                {
                    winner = "O";
                }
                else
                {
                    winner = "X";
                }

                MessageBox.Show(winner + " Wins!!!", "Thanks for play");
            }
            else
            {
                if(turn_count == 9)
                    MessageBox.Show("All click down. Game Over", "Thanks for play");
            }
        }

        private void disableButtons()
        {
            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;
                    b.Enabled = false;
                }
            }
            catch { }
            
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            turn = true;
            turn_count = 0;

            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;
                    b.Enabled = true;
                    b.Text = "";
                }
            }
            catch { }
        }
    }
}
