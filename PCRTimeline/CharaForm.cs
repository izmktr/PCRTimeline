using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PCRTimeline
{
    public partial class CharaForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        internal List<Avatar> avatarlist;

        ScrollBar charascrollbar = new HScrollBar();
        
        public CharaForm()
        {
            InitializeComponent();
        }

        private void CharaForm_Load(object sender, EventArgs e)
        {

        }

        void DrawCharactor(Control c, PaintEventArgs e, ScrollBar scrollBar)
        {
            int width = c.Width - scrollBar.Width + 10;
            int height = c.Height;

            int x = 0, y = 0, maxheight = 0;
            foreach (var avatar in avatarlist)
            {
                Image image = avatar.image;

                //DrawImageメソッドで画像を座標(0, 0)の位置に表示する
                e.Graphics.DrawImage(image, x, y - scrollBar.Value, image.Width, image.Height);
                x += image.Width;

                maxheight = Math.Max(maxheight, image.Height);
                if (0 < x && width - image.Width <= x)
                {
                    x = 0;
                    y += maxheight;
                    maxheight = 0;
                }
            }

            scrollBar.Maximum = y + maxheight;
            scrollBar.LargeChange = height;
        }


        private void CharaForm_Paint(object sender, PaintEventArgs e)
        {
            DrawCharactor((Control)sender, e, charascrollbar);
        }
    }
}
