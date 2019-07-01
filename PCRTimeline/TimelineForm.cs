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
        const int TimeHeader = 16;
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
            public float start;
            public Rectangle rect;
            public TimelineType dragpoint;
            public CustomSkill skill;
            public AttackResult attackresult;

            public DragPoint(float start, Rectangle rect, TimelineType dragpoint, CustomSkill skill)
            {
                this.start = start;
                this.rect = rect;
                this.dragpoint = dragpoint;
                this.skill = skill;
                this.attackresult = null;
            }

            public string Infomation()
            {
                float time = 90f - start;
                int minute = (int) time / 60;
                int second = (int) time % 60;

                return $"[{minute}:{second.ToString("D2")}]{skill.name}";
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
        DragPoint tooltip = null;
        private float dragvalue;
        private Point draglocation;
        private ClickPoint clickpoint;
        private int toolTipTime;
        private Point prevPosition;

        public TimelineForm()
        {
            InitializeComponent();
        }

        static IEnumerable<(CustomSkill skill, Rectangle rect)> SkillRectangle(IEnumerable<CustomSkill> timeline, int secondsize)
        {
            float time = 0f;
            foreach (var cskill in timeline)
            {
                int x = (int)(time * secondsize + IconSize);
                int width = (int)(cskill.interval * secondsize);

                Rectangle skillrect;
                int minwidth = 4;
                if (width < minwidth)
                {
                    skillrect = new Rectangle(x, 0, minwidth, IconSize - 8);
                }
                else
                {
                    skillrect = new Rectangle(x, 4, width, IconSize - 16);
                }

                yield return (cskill, skillrect);

                time += cskill.interval;

                if (limittime < time) break;
                if (cskill.Type == SkillType.Dead) break;
            }
        }

        void SetWindowSize()
        {
            int width = IconSize + righttime * secondsize;
            int height = TimeHeader + IconSize * battlerlist.Count;
            floatTimeline.AutoScrollMinSize = new Size(width, height);
        }

        public void CreatePoint()
        {
            SetWindowSize();
            dragablepoint.Clear();
            clickablepoint.Clear();

            CalcDamage();

            int bindex = 0;
            foreach (var battler in battlerlist)
            {
                CustomSkill before = null;
                float time = 0f;

                foreach (var (skill, srect) in SkillRectangle(battler.timeline, secondsize))
                {
                    var rect = new Rectangle(srect.X, srect.Y + TimeHeader + IconSize * bindex, srect.Width, srect.Height);

                    AddDragPoint(time, skill, rect, before);
                    AddClickPoint(battler, skill, rect, before);
                    before = skill;
                    time += skill.interval;
                }
                bindex++;
            }

            var size = new Size(IconSize + righttime * secondsize, TimeHeader + IconSize * battlerlist.Count);

            var image = new Bitmap(size.Width, size.Height);

            CreatePicture(Graphics.FromImage(image), new Rectangle(Point.Empty, image.Size));
            pictureBox1.Image = image;

            Invalidate();
        }

        private void CalcDamage()
        {
            var enemy = battlerlist.Find(n => n.avatar.position == 0)?.avatar;
            if (enemy == null)
            {
                enemy = new Avatar();
            }

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

        private void AddDragPoint(float time, CustomSkill current, Rectangle skillrect, CustomSkill before)
        {
            if (0 < skillrect.Width)
            {
                dragablepoint.Add(new DragPoint(
                    time, skillrect, TimelineType.Interval, current)
                    );
            }
        }

        private void DrawSkill(Graphics g, Battler battler, CustomSkill item, Rectangle skillrect, Point offset)
        {
            if (0 < skillrect.Width)
            {
                var drawrect = new Rectangle(skillrect.X + offset.X, skillrect.Y + offset.Y, skillrect.Width, skillrect.Height);

                var color = GetColor(item);
                LinearGradientBrush gb = new LinearGradientBrush(
                    drawrect, color, Color.White, LinearGradientMode.Horizontal);

                g.FillRectangle(gb, drawrect);
                g.DrawRectangle(Pens.DarkGray, drawrect);
                if (item.darty)
                {
                    var flute = drawrect;
                    flute.Inflate(1, 1);
                    g.DrawRectangle(Pens.DarkGray, flute);
                }

                if (item.name != null && item.Type != SkillType.Attack)
                {
                    var clip = g.ClipBounds;
                    g.SetClip(drawrect);
                    g.DrawString(item.name, this.Font, Brushes.Black, drawrect.X + 1, drawrect.Y + 1);
                    g.SetClip(clip);
                }
            }
        }

        public void DrawTimeline(Graphics g, int secondsize, Size size, Point offset)
        {
            for (int i = 0; i <= 90; i++)
            {
                int x = IconSize + i * secondsize + offset.X;
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
            if (prevPosition != e.Location)
            {
                if (skillToolTip.Active)
                {
                    prevPosition = e.Location;

                    if (tooltip == null || !tooltip.rect.Contains(e.Location))
                    {
                        toolTipTime = 0;
                        skillToolTip.Hide(this);
                    }
                }
            }

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

                CreatePoint();
            }
        }

        internal Bitmap ExportImage()
        {
            Bitmap image = new Bitmap(IconSize + righttime * secondsize, IconSize * battlerlist.Count + TimeHeader);

            using (var g = Graphics.FromImage(image))
            {
                int y = 0;
                var offset = new Point();

                g.FillRectangle(Brushes.White, new Rectangle(0, 0, image.Width, image.Height));

                DrawTimeline(g, secondsize, image.Size, offset);
                y += TimeHeader;

                foreach (var battler in battlerlist)
                {
                    foreach (var data in dragablepoint)
                    {
                        DrawSkill(g, battler, data.skill, data.rect, offset);
                    }

                    var time = 0f;
                    var shiftHeight = new int[10];
                    foreach (var item in battler.timeline)
                    {
                        int x = (int)(time * secondsize + IconSize);
                        if (item.effect != null && 0 < item.effect.duration)
                        {
                            int px = (int)(x + item.effect.delay * secondsize);
                            int py = y + TimeHeader + item.skillNo * 6 + shiftHeight[item.skillNo] * 3;
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
                    g.DrawImage(avatarimage, 0, offset.Y + y, avatarimage.Width, avatarimage.Height);
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

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clickpoint != null)
            {
                clickpoint.skill.Reset();
            }

            CreatePoint();
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

            CreatePoint();
        }

        private void bindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bind = clickpoint.battler.avatar.GetSkill(Data.SkillType.Bind);
            var customskill = new CustomSkill(bind);
            AddCustomerSkill(clickpoint, customskill);

            CreatePoint();
        }

        private void deadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bind = clickpoint.battler.avatar.GetSkill(Data.SkillType.Dead);
            var customskill = new CustomSkill(bind);
            AddCustomerSkill(clickpoint, customskill);

            CreatePoint();
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

                CreatePoint();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (0 <= toolTipTime && drag == null)
            {
                toolTipTime += timer1.Interval;

                if (skillToolTip.AutomaticDelay < toolTipTime)
                {
                    var position = prevPosition;
                    tooltip = dragablepoint.Find(n => n.rect.Contains(position));

                    //                    var mouse = this.PointToClient(System.Windows.Forms.Cursor.Position);
                    if (tooltip != null)
                    {
                        skillToolTip.Show(tooltip.Infomation(), this, position);
                    }
                    toolTipTime = -1;
                }
            }

        }

        private void floatTimeline_Paint(object sender, PaintEventArgs e)
        {
            var offset = AutoScrollPosition;

            int y = 0;
            Graphics g = e.Graphics;

            g.FillRectangle(Brushes.White, new Rectangle(0, 0, Width, Height));

            DrawTimeline(g, secondsize, this.Size, offset);
            y += TimeHeader;

            foreach (var battler in battlerlist)
            {
                foreach (var data in dragablepoint)
                {
                    DrawSkill(g, battler, data.skill, data.rect, offset);
                }

                var time = 0f;
                var shiftHeight = new int[10];
                foreach (var item in battler.timeline)
                {
                    int x = (int)(time * secondsize + IconSize);
                    if (item.effect != null && 0 < item.effect.duration)
                    {
                        int px = (int)(x + item.effect.delay * secondsize);
                        int py = y + TimeHeader + item.skillNo * 6 + shiftHeight[item.skillNo] * 3;
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

//                 Image image = battler.avatar.image;
//                 g.DrawImage(image, 0, offset.Y + y, image.Width, image.Height);
//                 y += IconSize;
            }

        }

        void CreatePicture(Graphics g, Rectangle rect)
        {
            g.FillRectangle(Brushes.White, rect);

            Point offset = new Point();

            int y = 0;
            //DrawTimeline(g, secondsize, this.Size);
            y += TimeHeader;

            foreach (var battler in battlerlist)
            {
                foreach (var data in dragablepoint)
                {
                    DrawSkill(g, battler, data.skill, data.rect, offset);
                }

                var time = 0f;
                var shiftHeight = new int[10];
                foreach (var item in battler.timeline)
                {
                    int x = (int)((time + item.interval) * secondsize + IconSize);
                    if (item.effect != null && 0 < item.effect.duration)
                    {
                        int px = (int)(x + item.effect.delay * secondsize);
                        int py = y + TimeHeader + item.skillNo * 6 + shiftHeight[item.skillNo] * 3;
                        var skillrect = new Rectangle(px, py, (int)(item.effect.duration * secondsize), IconSize - 32);
                        g.FillRectangle(Brushes.LightYellow, skillrect);
                        g.DrawRectangle(Pens.DarkGray, skillrect);

                        shiftHeight[item.skillNo] = (shiftHeight[item.skillNo] + 1) % 2;
                    }
                    time += item.interval;
                    if (limittime < time) break;
                    if (item.Type == SkillType.Dead) break;
                }

                //DrawImageメソッドで画像を座標(0, 0)の位置に表示する

                //                 Image image = battler.avatar.image;
                //                 g.DrawImage(image, 0, offset.Y + y, image.Width, image.Height);
                //                 y += IconSize;
            }

        }

        private void TimelineForm_Load(object sender, EventArgs e)
        {
            var timelinebar = new Bitmap(secondsize * 95, 16);
            var g = Graphics.FromImage(timelinebar);
            DrawTimeline(g, secondsize, timelinebar.Size, new Point());

            timelinePicture.Image = timelinebar;
            timelinePicture.Size = timelinebar.Size;
        }

        private void TimelineForm_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                timelinePicture.Location = new Point(timelinePicture.Location.X, 0);
            }
        }
    }
}
