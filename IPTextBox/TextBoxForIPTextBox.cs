using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPTextBox
{
    internal class TextBoxForIPTextBox : TextBox
    {
        public TextBoxForIPTextBox()
        {
            Font = new System.Drawing.Font("微軟正黑體", 9);
            BorderStyle = BorderStyle.None;
            TextAlign = HorizontalAlignment.Center;
        }
        private string _previousText;
        private bool _isPastedText = false;
        public bool IsPastedText { get => _isPastedText; }

        [Description("字串長度已達上限時引發的事件。")]
        public event EventHandler MaxTextLengthReached;
        private void OnMaxTextLengthReached(EventArgs e)
        {
            EventHandler handler = MaxTextLengthReached;
            handler?.Invoke(this, e);
        }
        protected override void WndProc(ref Message m)
        {
            // 參考"windows.h" (https://goma.googlesource.com/wine/+/wine-961215/include/windows.h)
            if (m.Msg == 0x0302)
            {
                _isPastedText = true;
                _previousText = Text;
            }
            base.WndProc(ref m);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            Regex ipExpression = new Regex(@"^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$");
            // 驗證一般輸入字串格式
            if (!_isPastedText)
            {
                bool isMatch = ipExpression.IsMatch(Text);
                if (isMatch && Text.Length == 3)
                {
                    OnMaxTextLengthReached(EventArgs.Empty);
                }
                if (Text.Length > 0 && !isMatch)
                {
                    Text = "255";
                }
            }
            else
            {
                // 給iptextBox嘗試驗證貼上字串格式
                base.OnTextChanged(EventArgs.Empty);
                _isPastedText = false;
                // 若驗證失敗，text會維持貼上後的字串
                // 但應該要恢復成貼上前的字串
                if (!ipExpression.IsMatch(Text))
                {
                    Text = _previousText;
                }
            }
        }
    }
}
