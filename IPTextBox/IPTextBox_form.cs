using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPTextBox
{
    public partial class IPTextBox_form : UserControl
    {
        [Browsable(true)]
        [Description("IP位址。")]
        public override string Text
        {
            get
            {
                if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox3.Text) && string.IsNullOrEmpty(textBox4.Text))
                {
                    return "";
                }
                return textBox1.Text + "." + textBox2.Text + "." + textBox3.Text + "." + textBox4.Text;
            }
            set
            {
                Regex ipExpression = new Regex(@"^(([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\.){3}([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$");
                if (string.IsNullOrEmpty(value))
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                }
                else if (ipExpression.IsMatch(value))
                {
                    string[] texts = value.Split('.');
                    textBox1.Text = texts[0];
                    textBox2.Text = texts[1];
                    textBox3.Text = texts[2];
                    textBox4.Text = texts[3];
                }
            }
        }
        public IPTextBox_form()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.SelectionStart == tb.TextLength && e.KeyCode == Keys.Right)
            {
                MoveFocus(tb, true);
            }
            else if (tb.SelectionStart == 0 && e.KeyCode == Keys.Left || tb.TextLength == 0 && e.KeyCode == Keys.Back)
            {
                MoveFocus(tb, false);
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.TextLength > 0 && e.KeyChar == '.')
            {
                MoveFocus(tb, true);
            }
            if (e.KeyChar != '\b')
            {
                e.Handled = !char.IsDigit(Convert.ToChar(e.KeyChar));
            }
        }
        /// <summary>
        /// 移動焦點
        /// </summary>
        /// <param name="IsMoveFoward">true為將焦點往tabIndex下一個順序的項目移動，false則反向</param>
        private void MoveFocus(TextBox textBox, bool IsMoveFoward)
        {
            if (!IsMoveFoward && !textBox.Equals(textBox1) || IsMoveFoward && !textBox.Equals(textBox4))
            {
                flowLayoutPanel.SelectNextControl(ActiveControl, IsMoveFoward, true, false, false);               
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            Text = Clipboard.GetText();
        }

        private void TextBox_MaxTextLengthReached(object sender, EventArgs e)
        {
            MoveFocus(sender as TextBox, true);
        }
    }
}
