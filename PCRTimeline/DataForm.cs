using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PCRTimeline
{
    public partial class DataForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public string text = "";
        public DataForm()
        {
            InitializeComponent();
        }

        private void DataForm_Load(object sender, EventArgs e)
        {
        }

        private void DataForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = ClientRectangle;

            g.DrawString(text, Font, Brushes.Black, rect.Location);
        }
    }
}
