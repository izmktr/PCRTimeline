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

        DragPoint drag = null;

        class DragPoint
        {
            public Rectangle rect;
            public bool start;

            public DragPoint(Rectangle rect, bool start)
            {
                this.rect = rect;
                this.start = start;
            }
        }
        List<DragPoint> clickablepoint = new List<DragPoint>();

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
            float limittime = 90f;

            clickablepoint.Clear();

            int y = 0;
            Graphics g = e.Graphics;

            g.FillRectangle(Brushes.White, new Rectangle(0, y, Width, timelinesize));
            for (int i = 0; i < 90; i++)
            {
                int x = IconSize + i * secondsize - this.hScrollBar1.Value;
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

                float time = 0f;
                foreach (var item in battler.timeline)
                {
                    int x = (int) (time * secondsize + IconSize) - this.hScrollBar1.Value;
                    int width = (int) (item.acttime * secondsize);


                    var brush = GetBrush(item);
                    g.FillRectangle(brush, x, y + 4, width, image.Height - 16);
                    g.DrawRectangle(Pens.Black, x, y + 4, width, image.Height - 16);

                    if (item.darty)
                    {
                        g.DrawRectangle(Pens.Black, x - 1, y + 4 - 1, width + 2, image.Height - 16 + 2);
                    }

                    int intervalwidth = (int)(item.interval * secondsize);
                    clickablepoint.Add(new DragPoint(new Rectangle(x - clickband / 2 + width, y + 4, clickband, image.Height - 16), true));
                    clickablepoint.Add(new DragPoint(new Rectangle(x - clickband / 2 + width + intervalwidth, y + 4, clickband, image.Height - 16), false));

                    time += item.acttime + item.interval;

                    if (limittime < time) break;
                }

                //DrawImageメソッドで画像を座標(0, 0)の位置に表示する
                g.DrawImage(image, 0, y, image.Width, image.Height);

                y += image.Height;
            }


        }

        private Brush GetBrush(ISkill skill)
        {
            switch (skill.Type)
            {
                case SkillType.Opening:
                    return Brushes.White;
                case SkillType.Attack:
                    return Brushes.LightBlue;
                case SkillType.Skill1:
                    return Brushes.Orange;
                case SkillType.Skill2:
                    return Brushes.LightPink;
                case SkillType.UnionBurst:
                    return Brushes.Red;
                case SkillType.Bind:
                    return Brushes.DarkGray;
                default:
                    return Brushes.White;
            }
        }

        private void TimelineForm_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (var click in clickablepoint)
            {
                if (click.rect.Contains(e.X, e.Y))
                {
                    this.Cursor = Cursors.SizeWE;
                    return;
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void TimelineForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
            }
        }

        private void TimelineForm_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void TimelineForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                System.Drawing.Point p = System.Windows.Forms.Cursor.Position;

                //指定した画面上の座標位置にコンテキストメニューを表示する
                this.contextMenuStrip1.Show(p);
            }

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }
    }
}
