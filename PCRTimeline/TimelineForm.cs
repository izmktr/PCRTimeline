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
        const int timelineheight = 16;

        int secondsize { get { return secondsizearray[scope]; } }

        class DragPoint
        {
            public Rectangle rect;
            public bool start;
            public ISkill skill;

            public DragPoint(Rectangle rect, bool start, ISkill skill)
            {
                this.rect = rect;
                this.start = start;
                this.skill = skill;
            }
        }
        List<DragPoint> clickablepoint = new List<DragPoint>();

        DragPoint drag = null;
        private float dragvalue;
        private Point draglocation;

        public TimelineForm()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        private void TimelineForm_Paint(object sender, PaintEventArgs e)
        {
            const int IconSize = 48;
            const int clickband = 6;
            float limittime = 90f;

            clickablepoint.Clear();

            int maxvalue = 100 * secondsize - (this.Width - IconSize);
            if (maxvalue < hScrollBar1.Value)
            {
                hScrollBar1.Value = Math.Max(0, maxvalue);
            }

            int y = 0;
            Graphics g = e.Graphics;

            g.FillRectangle(Brushes.White, new Rectangle(0, 0, Width, Height));
            DrawTimeline(g, IconSize, secondsize, hScrollBar1.Value, this.Size);
            y += 16;

            foreach (var battler in battlerlist)
            {
                Image image = battler.avatar.image;

                float time = 0f;
                foreach (var item in battler.timeline)
                {
                    int x = (int)(time * secondsize + IconSize) - this.hScrollBar1.Value;
                    int width = (int)(item.acttime * secondsize);


                    var brush = GetBrush(item);
                    g.FillRectangle(brush, x, y + 4, width, image.Height - 16);
                    g.DrawRectangle(Pens.Black, x, y + 4, width, image.Height - 16);

                    if (item.darty)
                    {
                        g.DrawRectangle(Pens.Black, x - 1, y + 4 - 1, width + 2, image.Height - 16 + 2);
                    }

                    int intervalwidth = (int)(item.interval * secondsize);
                    clickablepoint.Add(new DragPoint(new Rectangle(x - clickband / 2 + width, y + 4, clickband, image.Height - 16), true, item));
                    clickablepoint.Add(new DragPoint(new Rectangle(x - clickband / 2 + width + intervalwidth, y + 4, clickband, image.Height - 16), false, item));

                    time += item.acttime + item.interval;

                    if (limittime < time) break;
                }

                //DrawImageメソッドで画像を座標(0, 0)の位置に表示する
                g.DrawImage(image, 0, y, image.Width, image.Height);

                y += image.Height;
            }

            hScrollBar1.Maximum = 95 * secondsize;
            hScrollBar1.LargeChange = this.Width;
        }

        public void DrawTimeline(Graphics g, int IconSize, int secondsize, int offset, Size size)
        {
            for (int i = 0; i <= 90; i++)
            {
                int x = IconSize + i * secondsize - offset;
                var pen = i % 10 == 0 ? Pens.Black : i % 5 == 0 ? Pens.Gray : Pens.LightGray;
                g.DrawLine(pen, new Point(x, timelineheight), new Point(x, size.Height));

                if (i % 10 == 0)
                {
                    g.DrawString(string.Format("{0}:{1}0", i <= 30 ? 1 : 0, (9 - i / 10) % 6), Font, Brushes.Black, x - 10, 0);
                }
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
            ChangeCursor(e);

            if (drag != null)
            {
                float value = dragvalue + (e.Location.X - draglocation.X) / (float) secondsize;
                if (value < 0.25f) value = 0.25f;
                if (drag.start)
                {
                    drag.skill.acttime = value;
                }
                else
                {
                    drag.skill.interval = value;
                }
                drag.skill.darty = true;

                Invalidate();
            }
        }

        private void ChangeCursor(MouseEventArgs e)
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
                drag = clickablepoint.Find(n => n.rect.Contains(e.Location));
                if (drag != null)
                {
                    dragvalue = drag.start ? drag.skill.acttime : drag.skill.interval;
                    draglocation = e.Location;
                }
            }
        }

        private void TimelineForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drag = null;
            }
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
