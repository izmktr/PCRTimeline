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
        const int IconSize = 48;
        const int clickband = 6;
        const int limittime = 90;
        const int righttime = limittime + 5;

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
        List<DragPoint> dragablepoint = new List<DragPoint>();


        class ClickPoint
        {
            public Rectangle rect;
            public Battler battler;
            public CustomSkill skill;
        }
        List<ClickPoint> clickablepoint = new List<ClickPoint>();

        DragPoint drag = null;
        private float dragvalue;
        private Point draglocation;
        private ClickPoint clickpoint;

        public TimelineForm()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        static IEnumerable<(CustomSkill skill, Rectangle rect)> SkillRectangle(IEnumerable<CustomSkill> timeline, int y, int secondsize, int hoffset)
        {
            float time = 0f;
            foreach (var item in timeline)
            {
                int x = (int)(time * secondsize + IconSize) - hoffset;
                int width = (int)(item.acttime * secondsize);
                if (0 < item.acttime && width < 8) width = 8;

                Rectangle skillrect = new Rectangle(x, y + 4, width, IconSize - 16);

                yield return (item, skillrect);

                time += item.acttime + item.interval;

                if (limittime < time) break;
                if (item.Type == SkillType.Dead) break;
            }
        }

        private void TimelineForm_Paint(object sender, PaintEventArgs e)
        {

            dragablepoint.Clear();
            clickablepoint.Clear();

            int maxvalue = righttime * secondsize - (this.Width - IconSize);
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
                float time = 0f;
                CustomSkill before = null;
                foreach (var (skill, rect) in SkillRectangle(battler.timeline, y, secondsize, hScrollBar1.Value))
                {
                    DrawSkill(g, battler, skill, rect);

                    AddDragPoint(skill, before, rect);
                    AddClickPoint(battler, skill, rect);

                    before = skill;
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
                        var rect = new Rectangle(px, py, (int)(item.effect.duration * secondsize), IconSize - 32);
                        g.FillRectangle(Brushes.LightYellow, rect);
                        g.DrawRectangle(Pens.DarkGray, rect);

                        shiftHeight[item.skillNo] = (shiftHeight[item.skillNo] + 1) % 2;
                    }
                    time += item.acttime + item.interval;
                    if (limittime < time) break;
                    if (item.Type == SkillType.Dead) break;
                }

                //DrawImageメソッドで画像を座標(0, 0)の位置に表示する

                Image image = battler.avatar.image;
                g.DrawImage(image, 0, y, image.Width, image.Height);

                y += IconSize;
            }

            hScrollBar1.Maximum = righttime * secondsize;
            hScrollBar1.LargeChange = this.Width;
        }

        private void AddClickPoint(Battler battler, CustomSkill item, Rectangle skillrect)
        {
            if (0 < skillrect.Width)
            {
                clickablepoint.Add(new ClickPoint()
                {
                    battler = battler,
                    rect = skillrect,
                    skill = item
                });
            }
        }

        private void AddDragPoint(CustomSkill current, CustomSkill before, Rectangle skillrect)
        {
            if (0 < current.acttime)
            {
                dragablepoint.Add(new DragPoint(
                    new Rectangle(skillrect.Right - clickband / 2, skillrect.Y, clickband, skillrect.Height), TimelineType.ActStart, current
                    ));

                int actwidth = (int)(current.acttime * secondsize);
                dragablepoint.Add(new DragPoint(
                    new Rectangle(skillrect.X, skillrect.Y, actwidth - clickband / 2, skillrect.Height), TimelineType.ActEnd, before)
                    );
            }
        }

        private void DrawSkill(Graphics g, Battler battler, CustomSkill item, Rectangle skillrect)
        {
            Image image = battler.avatar.image;

            var brush = GetBrush(item);
            if (0 < skillrect.Width)
            {
                g.FillRectangle(brush, skillrect);
                g.DrawRectangle(Pens.DarkGray, skillrect);
                if (item.darty)
                {
                    var flute = skillrect;
                    flute.Inflate(1, 1);
                    g.DrawRectangle(Pens.DarkGray, flute);
                }

                if (item.IsSkill)
                {
                    var clip = g.ClipBounds;
                    g.SetClip(skillrect);
                    g.DrawString(item.name, this.Font, Brushes.Black, skillrect.X + 1, skillrect.Y + 1);
                    g.SetClip(clip);
                }
            }
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
                case SkillType.Dead:
                    return Brushes.Red;
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

        internal Bitmap ExportImage()
        {
            Bitmap image = new Bitmap(IconSize + righttime * secondsize, IconSize * battlerlist.Count + 16);

            using (var g = Graphics.FromImage(image))
            {
                int y = 0;

                g.FillRectangle(Brushes.White, new Rectangle(0, 0, image.Width, image.Height));

                DrawTimeline(g, IconSize, secondsize, 0, image.Size);
                y += 16;

                foreach (var battler in battlerlist)
                {
                    float time = 0f;
                    foreach (var (skill, rect) in SkillRectangle(battler.timeline, y, secondsize, hScrollBar1.Value))
                    {
                        DrawSkill(g, battler, skill, rect);
                    }

                    time = 0f;
                    var shiftHeight = new int[10];
                    foreach (var item in battler.timeline)
                    {
                        int x = (int)(time * secondsize + IconSize);
                        if (item.effect != null && 0 < item.effect.duration)
                        {
                            int px = (int)(x + item.effect.delay * secondsize);
                            int py = y + 16 + item.skillNo * 6 + shiftHeight[item.skillNo] * 3;
                            var rect = new Rectangle(px, py, (int)(item.effect.duration * secondsize), IconSize - 32);
                            g.FillRectangle(Brushes.LightYellow, rect);
                            g.DrawRectangle(Pens.DarkGray, rect);

                            shiftHeight[item.skillNo] = (shiftHeight[item.skillNo] + 1) % 2;
                        }
                        time += item.acttime + item.interval;
                        if (limittime < time) break;
                        if (item.Type == SkillType.Dead) break;
                    }

                    //DrawImageメソッドで画像を座標(0, 0)の位置に表示する

                    Image avatarimage = battler.avatar.image;
                    g.DrawImage(avatarimage, 0, y, avatarimage.Width, avatarimage.Height);

                    y += IconSize;
                }
            }

            return image;
        }

        private void ChangeCursor(MouseEventArgs e)
        {
            foreach (var click in dragablepoint)
            {
                if (click.rect.Contains(e.X, e.Y))
                {
                    if (click.dragpoint == TimelineType.ActStart)
                    {
                        this.Cursor = Cursors.SizeWE;
                        return;
                    }
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void TimelineForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drag = dragablepoint.Find(n => n.rect.Contains(e.Location));
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

                clickpoint = clickablepoint.Find(n => n.rect.Contains(e.Location));

                if (clickpoint != null)
                {
                    resetToolStripMenuItem.Enabled = clickpoint.skill.basic && clickpoint.skill.darty;
                    deleteDataToolStripMenuItem.Enabled  = !clickpoint.skill.basic || clickpoint.skill.Type == SkillType.UnionBurst;

                    //指定した画面上の座標位置にコンテキストメニューを表示する
                    this.contextMenuStrip1.Show(p);
                }
            }

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clickpoint != null)
            {
                clickpoint.skill.Reset();
                if (clickpoint.skill.Type == SkillType.UnionBurst)
                {
                    clickpoint.skill.CreateModify();
                }
            }
            this.Invalidate();
        }

        private void AddCustomerSkill(ClickPoint cb, CustomSkill skill)
        {
            if (cb != null)
            {
                var addindex = cb.battler.timeline.FindIndex(n => n == cb.skill);
                if (0 <= addindex)
                {
                    cb.battler.timeline.Insert(addindex + 1, skill);
                }

            }
        }

        private void unionburstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ub = clickpoint.battler.avatar.skill.Find(n => n.type == SkillType.UnionBurst);
            var customskill = new CustomSkill(ub);
            customskill.CreateModify();
            AddCustomerSkill(clickpoint, customskill);
            this.Invalidate();
        }

        private void bindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bind = new CustomSkill(null);
            bind.modify = new Skill();
            bind.modify.acttime = 1.0f;
            bind.modify.type = SkillType.Bind;

            clickpoint.skill.CreateModify();

            AddCustomerSkill(clickpoint, bind);
            this.Invalidate();
        }

        private void deadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bind = new CustomSkill(null);
            bind.modify = new Skill();
            bind.modify.acttime = Math.Max(clickpoint.skill.interval, 3.0f);
            bind.modify.type = SkillType.Dead;

            clickpoint.skill.CreateModify();
            clickpoint.skill.modify.interval = 0;

            AddCustomerSkill(clickpoint, bind);
            this.Invalidate();
        }

        private void deleteDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clickpoint != null)
            {
                var addindex = clickpoint.battler.timeline.FindIndex(n => n == clickpoint.skill);
                if (0 <= addindex)
                {
                    clickpoint.battler.timeline.RemoveAt(addindex);
                }
                this.Invalidate();
            }
        }
    }
}
