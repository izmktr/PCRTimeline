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
        private Point prevPosition;
        int toolTipTime = 0;

        class ClickableObject{
            public Rectangle rect;
            public Avatar avatar;
        }

        List<ClickableObject> clicklist = new List<ClickableObject>();
        
        int beforetab = 0;

        public CharaForm()
        {
            InitializeComponent();
        }

        private void CharaForm_Load(object sender, EventArgs e)
        {

        }

        void DrawCharactor(TabPage c, PaintEventArgs e)
        {
            int width = c.Width - vScrollBar1.Width;
            int height = c.Height;

            clicklist.Clear();

            int x = 0, y = 0, maxheight = 0;
            foreach (var avatar in avatarlist)
            {
                Image image = avatar.image;

                if (0 < x && width <= x + image.Width)
                {
                    x = 0;
                    y += maxheight;
                    maxheight = 0;
                }

                //DrawImageメソッドで画像を座標(0, 0)の位置に表示する
                e.Graphics.DrawImage(image, x, y - vScrollBar1.Value, image.Width, image.Height);

                clicklist.Add(
                    new ClickableObject()
                    {
                        rect = new Rectangle(x, y - vScrollBar1.Value, image.Width, image.Height),
                        avatar = avatar
                    });

                x += image.Width;
                maxheight = Math.Max(maxheight, image.Height);
            }

            vScrollBar1.Maximum = y + maxheight;
            vScrollBar1.LargeChange = height;
        }


        private void CharaForm_Paint(object sender, PaintEventArgs e)
        {
            
            DrawCharactor((TabPage)sender, e);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tab = (TabControl)sender;

            if (beforetab != tab.SelectedIndex) {
                tab.TabPages[beforetab].Controls.Remove(vScrollBar1);
                tab.TabPages[tab.SelectedIndex].Controls.Add(vScrollBar1);
                beforetab = tab.SelectedIndex;
            }
            
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            tabControl.Invalidate();
        }

        private void tabPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (prevPosition != e.Location)
            {
                prevPosition = e.Location;
                toolTipTime = 0;
                nameToolTip.Hide(this);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (0 <= toolTipTime)
            {
                toolTipTime += timer1.Interval;

                if (nameToolTip.AutomaticDelay < toolTipTime)
                {
                    var mouse = this.PointToClient(System.Windows.Forms.Cursor.Position);
//                    nameToolTip.Show($"({mouse.X},{mouse.Y})", this, mouse.X, mouse.Y);
                    toolTipTime = -1;
                }
            }
        }
    }
}
