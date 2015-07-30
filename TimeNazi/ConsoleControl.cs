using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeNazi
{
    public class ConsoleControl : RichTextBox
    {
        private int _iMaxLineCount = 1000;
        public int MaxLineCount
        {
            get { return _iMaxLineCount; }
            set { _iMaxLineCount = value; }
        }

        public ConsoleControl()
        {
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "rtbConsoleTextBox";
            this.ReadOnly = true;
            this.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.Size = new System.Drawing.Size(734, 381);
            this.TabIndex = 0;
            this.Text = "";
            this.TextChanged += new System.EventHandler(this._textChanged);
        }

        private void _textChanged(object sender, EventArgs e)
        {
            if (this.Lines.Length > MaxLineCount)
            {
                this.Lines = this.Lines.Skip(Math.Abs(MaxLineCount - this.Lines.Length)).ToArray();
            }
            // autoscroll
            this.SelectionStart = this.Text.Length;
            this.ScrollToCaret();
        }

        public void AddLine(string line)
        {
            List<string> lsTMP = new List<string>(this.Lines);
            lsTMP.Add(line);
            this.Lines = lsTMP.ToArray();
        }

    }
}
