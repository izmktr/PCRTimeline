using PCRTimeline.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        const int limittime = 90;
        const int righttime = limittime + 5;

        int secondsize { get { return secondsizearray[scope]; } }

        enum TimelineType
        {
            None,
            Interval,
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
            public CustomSkill before;
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
            foreach (var cskill in timeline)
            {
                int x = (int)(time * secondsize + IconSize) - hoffset;
                int width = (int)(cskill.interval * secondsize);

                Rectangle skillrect;
                int minwidth = 4;
                if (width < minwidth)
                {
                    skillrect = new Rectangle(x, y + 0, minwidth, IconSize - 8);
                }
                else
                {
                    skillrect = new Rectangle(x, y + 4, width, IconSize - 16);
                }

                yield return (cskill, skillrect);

                time += cskill.interval;

                if (limittime < time) break;
                if (cskill.Type == SkillType.Dead) break;
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

                    AddDragPoint(skill, rect, before);
                    AddClickPoint(battler, skill, rect, before);
                    before = skill;
                }

                time = 0f;
                var shiftHeight = new int[10];                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
                foreach (var item in battler.timeline)
                {
                    int x = (int)((time + item.interval) * secondsize + IconSize) - this.hScrollBar1.Value;
                    if (item.effect != null && 0 < item.effect.duration)
                    {
                        int px = (int)(x + item.effect.delay * secondsize);
                        int py = y + 16 + item.skillNo * 6 + shiftHeight[item.skillNo] * 3;
                        var rect = new Rectangle(px, py, (int)(item.effect.duration * secondsize), IconSize - 32);
                        g.FillRectangle(Brushes.LightYellow, rect);
                        g.DrawRectangle(Pens.DarkGray, rect);

                        shiftHeight[item.skillNo] = (shiftHeight[item.skillNo] + 1) % 2;
                    }
                    time += item.interval;
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

        private void AddClickPoint(Battler battler, CustomSkill skill, Rectangle skillrect, CustomSkill before)
        {
            if (0 < skillrect.Width)
            {
                clickablepoint.Add(new ClickPoint()
                {
                    battler = battler,
                    rect = skillrect,
                    skill = skill,
                    before = before
                });
            }
        }

        private void AddDragPoint(CustomSkill current, Rectangle skillrect, CustomSkill before)
        {
            if (0 < current.interval && before != null)
            {
                dragablepoint.Add(new DragPoint(
                    skillrect, TimelineType.Interval, current)
                    );
            }
        }

        private void DrawSkill(Graphics g, Battler battler, CustomSkill item, Rectangle skillrect)
        {
            if (0 < skillrect.Width)
            {
                Image image = battler.avatar.image;

                var color = GetColor(item);
                LinearGradientBrush gb = new LinearGradientBrush(
                    skillrect, color, Color.White, LinearGradientMode.Horizontal);

                g.FillRectangle(gb, skillrect);
                g.DrawRectangle(Pens.DarkGray, skillrect);
                if (item.darty)
                {
                    var flute = skillrect;
                    flute.Inflate(1, 1);
                    g.DrawRectangle(Pens.DarkGray, flute);
                }

                if (item.name != null && item.Type != SkillType.Attack)
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

        public void DrawBuffGraph(Graphics g, int secondsize, int offset, Rectangle rect)
        {
            if (battlerlist.Count == 0) return;
            var buff = battlerlist[0].buff;

            List<BuffValuePair> line;
            if (!buff.TryGetValue(BuffEffectType.DefEnemy, out line)) return;

            int maxvalue = 300;
            var pen = Pens.Blue;
            BuffValuePair before = new BuffValuePair(0, 0);
            foreach (var item in line)
            {
                var left = new Point((int)(before.time * secondsize - offset) + rect.X,  - before.totalValue * rect.Height / maxvalue + rect.Bottom);
                var rightLow = new Point((int)(item.time * secondsize - offset) + rect.X, before.totalValue * rect.Height / maxvalue + rect.Bottom);
                var rightHigh= new Point((int)(item.time * secondsize - offset) + rect.X, item.totalValue * rect.Height / maxvalue + rect.Bottom);

                g.DrawLine(pen, left, rightLow);
                g.DrawLine(pen, rightLow, rightHigh);

                before = item;
            }
        }

        private Color GetColor(CustomSkill skill)
        {
            switch (skill.Type)
            {
                case SkillType.Opening:
                    return Color.White;
                case SkillType.Attack:
                    return Color.LightBlue;
                case SkillType.Skill1:
                    return Color.Orange;
                case SkillType.Skill2:
                    return Color.LightPink;
                case SkillType.UnionBurst:
                    return Color.Red;
                case SkillType.Bind:
                    return Color.LightYellow;
                case SkillType.Dead:
                    return Color.DarkGray;
                default:
                    return Color.White;
            }
        }

        private void TimelineForm_MouseMove(object sender, MouseEventArgs e)
        {
//            ChangeCursor(e);

            if (drag != null)
            {
                switch (drag.dragpoint)
                {
                    case TimelineType.Interval:
                        {
                            float value = dragvalue + (e.Location.X - draglocation.X) / (float)secondsize;
                            drag.skill.adjustment = Math.Max(value, -drag.skill.orginterval);
                        }
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
                        int x = (int)((time + item.interval) * secondsize + IconSize);
                        if (item.effect != null && 0 < item.effect.duration)
                        {
                            int px = (int)(x + item.effect.delay * secondsize);
                            int py = y + 16 + item.skillNo * 6 + shiftHeight[item.skillNo] * 3;
                            var rect = new Rectangle(px, py, (int)(item.effect.duration * secondsize), IconSize - 32);
                            g.FillRectangle(Brushes.LightYellow, rect);
                            g.DrawRectangle(Pens.DarkGray, rect);

                            shiftHeight[item.skillNo] = (shiftHeight[item.skillNo] + 1) % 2;
                        }
                        time += item.interval;
                        if (limittime < time) break;
                        if (item.Type == SkillType.Dead) break;
                    }

                    //アイコンの表示
                    Image avatarimage = battler.avatar.image;
                    g.DrawImage(avatarimage, 0, y, avatarimage.Width, avatarimage.Height);

                    y += IconSize;
                }
            }

            return image;
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
                        case TimelineType.Interval:
                            dragvalue = drag.skill.adjustment;
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
                    resetToolStripMenuItem.Enabled = clickpoint.skill.darty;
                    deleteDataToolStripMenuItem.Enabled = Deleteable(clickpoint.skill);

                    //指定した画面上の座標位置にコンテキストメニューを表示する
                    this.contextMenuStrip1.Show(p);
                }
            }

        }

        private bool Deleteable(CustomSkill skill)
        {
            switch (skill.Type)
            {
                case SkillType.UnionBurst:
                case SkillType.Bind:
                case SkillType.Dead:
                    return true;
                default:
                    return false;
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
            var ub = clickpoint.battler.avatar.GetSkill(Data.SkillType.UnionBurst);
            var customskill = new CustomSkill(ub);
            AddCustomerSkill(clickpoint, customskill);
            this.Invalidate();
        }

        private void bindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bind = clickpoint.battler.avatar.GetSkill(Data.SkillType.Bind);
            var customskill = new CustomSkill(bind);
            AddCustomerSkill(clickpoint, customskill);
            this.Invalidate();
        }

        private void deadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bind = clickpoint.battler.avatar.GetSkill(Data.SkillType.Dead);
            var customskill = new CustomSkill(bind);
            AddCustomerSkill(clickpoint, customskill);
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
