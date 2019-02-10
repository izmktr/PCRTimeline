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

        enum TimelineType
        {
            None,
            ActStart,
            ActEnd,
            EffectStart,
            EffectEnd,
        }

        class DragPoint
        {
            public Rectangle rect;
            public TimelineType dragpoint;
            public CustomSkill skill;

            public DragPoint(Rectangle rect, TimelineType dragpoint, CustomSkill skill)
            {
                this.rect = rect;
                this.dragpoint = dragpoint;
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
                    if (0 < width)
                    {
                        g.FillRectangle(brush, x, y + 4, width, image.Height - 16);
                        g.DrawRectangle(Pens.DarkGray, x, y + 4, width, image.Height - 16);
                        if (item.darty)
                        {
                            g.DrawRectangle(Pens.DarkGray, x - 1, y + 4 - 1, width + 2, image.Height - 16 + 2);
                        }

                        if (item.IsSkill)
                        {
                            var clip = g.ClipBounds;
                            g.SetClip(new Rectangle(x, y + 4, width, image.Height - 16));
                            g.DrawString(item.name, this.Font, Brushes.Black, x + 1, y + 4 + 1);
                            g.SetClip(clip);
                        }
                    }

                    int intervalwidth = (int)(item.interval * secondsize);
                    if (0 < width)
                    {
                        clickablepoint.Add(new DragPoint(
                            new Rectangle(x - clickband / 2 + width, y + 4, clickband, image.Height - 16), TimelineType.ActStart, item
                            ));
                    }
                    clickablepoint.Add(new DragPoint(
                        new Rectangle(x - clickband / 2 + width + intervalwidth, y + 4, clickband, image.Height - 16), TimelineType.ActEnd, item)
                        );

                    time += item.acttime + item.interval;

                    if (limittime < time) break;
                }


                time = 0f;
                var shiftHeight = new int[10];
                foreach (var item in battler.timeline)
                {
                    int x = (int)(time * secondsize + IconSize) - this.hScrollBar1.Value;
                    if (item.effect != null && 0 < item.effect.duration)
                    {
                        int px = (int)(x + item.effect.delay * secondsize);
                        int py = y + 16 + item.skillNo * 6 + shiftHeight[item.skillNo] * 3;
                        var rect = new Rectangle(px, py, (int)(item.effect.duration * secondsize), image.Height - 32);
                        g.FillRectangle(Brushes.LightYellow, rect);
                        g.DrawRectangle(Pens.DarkGray, rect);

                        shiftHeight[item.skillNo] = (shiftHeight[item.skillNo] + 1) % 2;
                    }
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

        private Brush GetBrush(CustomSkill skill)
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
                drag.skill.CreateModify();

                switch (drag.dragpoint)
                {
                    case TimelineType.ActStart:
                        drag.skill.modify.acttime = value;
                        break;
                    case TimelineType.ActEnd:
                        drag.skill.modify.interval = value;
                        break;
                    case TimelineType.EffectStart:
                        drag.skill.effect.delay = value;
                        break;
                    case TimelineType.EffectEnd:
                        drag.skill.effect.duration = value;
                        break;
                    default:
                        break;
                }

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
                    draglocation = e.Location;
                    switch (drag.dragpoint)
                    {
                        case TimelineType.ActStart:
                            dragvalue = drag.skill.acttime;
                            break;
                        case TimelineType.ActEnd:
                            dragvalue = drag.skill.interval;
                            break;
                        case TimelineType.EffectStart:
                            dragvalue = drag.skill.effect.delay;
                            break;
                        case TimelineType.EffectEnd:
                            dragvalue = drag.skill.effect.duration;
                            break;
                        default:
                            drag = null;
                            break;
                    }
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
