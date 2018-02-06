using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calc
{
    public partial class Calc : Form
    {
        public Calc()
        {
            InitializeComponent();
        }

        private void btnOp_Click(object sender, EventArgs e)
        {
            string str = InputExp.Text;
            int result = 0;
            while(true)
            {
                if(calc(ref str, ref result))
                {
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine("error");
                }
            }
        }

        public int getExpLength()
        {
            string str = InputExp.Text;
            int length = str.Length;
            return length - 1;
        }

        char getStrIndexOf(int i)
        {
            string str = InputExp.Text;
            return str[i];
        }

        public char test_char(ref int i, int end, string candidates)
        {
            if (i == end) return (char)0;
            var result = candidates.IndexOf(getStrIndexOf(i));
            if (result == 0) return (char)0;
            i++;
            return getStrIndexOf(i);
        }

        bool number(ref int i, int end, ref int result)
        {
            result = 0;
            int j = i;
            while (j != end && '0' <= getStrIndexOf(j) && getStrIndexOf(j) <= '9')
            {
                result = result * 10 + (getStrIndexOf(j) - '0');
                j++;
            }
            if (i == j) return false;
            i = j;
            return true;
        }

        bool factor(ref int i, int end, ref int result)
        {
            if (number(ref i, end, ref result)) return true;
            var j = i;
            if (test_char(ref j, end, "(") == 0) return false;
            if (!exp(ref j, end, ref result)) return false;
            if (test_char(ref j, end, ")") == 0) return false;
            i = j;
            return true;
        }

        public bool term(ref int i, int end, ref int result)
        {
            if (!factor(ref i, end, ref result)) return false;
            while(true)
            {
                var j = i;
                var op = test_char(ref j, end, "*/");
                if (op != '\0')
                {
                    int next = 0;
                    if (!factor(ref j, end, ref next)) return true;
                    result = op == '*'
                        ? result * next
                        : result / next;
                    i = j;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool exp(ref int i, int end, ref int result)
        {
            if (!term(ref i, end, ref result)) return false;
            while(true)
            {
                var j = i;
                var op = test_char(ref j, end, "+-");
                if (op != '\0')
                {
                    int next = 0;
                    if (!factor(ref j, end, ref next)) return true;
                    result = op == '+'
                        ? result + next
                        : result - next;
                    i = j;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool calc(ref string str, ref int result)
        {
            int begin = 0;
            int end = str.Length - 1;
            return exp(ref begin, end, ref result);
        }
    }
}
