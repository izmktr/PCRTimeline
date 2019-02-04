using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PCRTimeline
{
    public partial class TimelineForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        internal List<Battler> battlerlist;
        internal int scope = 2;
        int[] secondsizearray = { 4, 8, 16, 32, 64 };


        class DragPoint
        {
            public Rectangle rect;

        }
        List<Rectangle> clickablepoint = new List<Rectangle>();

        public TimelineForm()
        {
            InitializeComponent();
        }

        private void TimelineForm_Paint(object sender, PaintEventArgs e)
        {
            const int IconSize = 48;
            int secondsize = secondsizearray[scope];
            const int timelinesize = 16;
            const int clickband = 6;

            clickablepoint.Clear();

            int y = 0;
            Graphics g = e.Graphics;

            g.FillRectangle(Brushes.White, new Rectangle(0, y, Width, timelinesize));
            for (int i = 0; i < 90; i++)
            {
                int x = IconSize + i * secondsize;
                var pen = i % 10 == 0 ? Pens.Black : i % 5 == 0 ? Pens.Gray : Pens.LightGray;
                g.DrawLine(pen, new Point(x, y + timelinesize), new Point(x, Height));

                if (i % 10 == 0)
                {
                    g.DrawString(string.Format("{0}:{1}0", i <= 30 ? 1 : 0, (9 - i / 10) % 6), Font, Brushes.Black, x - 10, y);
                }
            }
            y += 16;

            foreach (var battler in battlerlist)
            {
                Image image = battler.avatar.image;

                //DrawImageメソッドで画像を座標(0, 0)の位置に表示する
                g.DrawImage(image, 0, y, image.Width, image.Height);

                float time = 0f;
                foreach (var item in battler.avatar.timeline)
                {
                    int x = (int) time * secondsize + IconSize;
                    int width = (int) item.acttime * secondsize;
                    g.FillRectangle(Brushes.LightBlue, x, y + 4, width, image.Height - 16);
                    g.DrawRectangle(Pens.Black, x, y + 4, width, image.Height - 16);
                    time += item.AllTime;

                    clickablepoint.Add(new Rectangle(x - clickband / 2, y + 4, clickband, image.Height - 16));
                    clickablepoint.Add(new Rectangle(x - clickband / 2 + width, y + 4, clickband, image.Height - 16));

                }

                y += image.Height;
            }

        }

        private void TimelineForm_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (var rect in clickablepoint)
            {
                if (rect.Contains(e.X, e.Y))
                {
                    this.Cursor = Cursors.SizeWE;
                    return;
                }
            }
            this.Cursor = Cursors.Default;
        }
    }
}
