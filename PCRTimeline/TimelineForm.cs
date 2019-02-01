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
        internal List<Avatar> avatarlist;
        internal List<Battler> battlerlist;

        public TimelineForm()
        {
            InitializeComponent();
        }

        private void TimelineForm_Paint(object sender, PaintEventArgs e)
        {
            const int IconSize = 48;
            const int secondsize = 16;
            const int timelinesize = 16;

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
                    g.FillRectangle(Brushes.LightBlue, time * secondsize + image.Width, y + 4, item.acttime * secondsize, image.Height - 16);
                    g.DrawRectangle(Pens.Black, time * secondsize + image.Width, y + 4, item.acttime * secondsize, image.Height - 16);
                    time += item.AllTime;
                }

                y += image.Height;
            }

        }
    }
}
