using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCRTimeline
{
    public partial class MainForm : Form
    {
        const int IconSize = 48;

        List<Avatar> avatarlist = new List<Avatar>();
        System.Windows.Forms.ImageList imageList = new System.Windows.Forms.ImageList();

        List<Battler> battlerlist = new List<Battler>();

        TimelineForm timeline;
        CharaForm chara;

        int scope = 2;

        public MainForm()
        {
            InitializeComponent();
        }

        void AvatarLoad()
        {
            const string path = @"Data\Icon\";
            string[] files = System.IO.Directory.GetFiles(path, "*.png", System.IO.SearchOption.AllDirectories);

            foreach (var file in files)
            {
                avatarlist.Add(new Avatar()
                {
                    name = file,
                    image = (new Bitmap(Image.FromFile(file), new Size(IconSize, IconSize)))
                });
                avatarlist.Last().SetSkill();
            }

            for (int i = 0; i < 5; i++)
            {
                battlerlist.Add(new Battler() { avatar = avatarlist[i] });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            AvatarLoad();

            OpenTimeline();

            OpenChara();

        }

        private void OpenChara()
        {
            chara = new CharaForm();
            chara.avatarlist = avatarlist;
            chara.Show(dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.DockBottom);

            chara.FormClosed += (s, e) => charaToolStripMenuItem.Checked = false;
        }

        private void OpenTimeline()
        {
            timeline = new TimelineForm();
            timeline.battlerlist = battlerlist;
            timeline.Show(dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            timeline.scope = scope;

            timeline.FormClosed += (s, e) => timelineToolStripMenuItem.Checked = false;
        }

        Point mouseDownPoint = Point.Empty;

        private void splitContainer1_Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownPoint != Point.Empty)
            {
                int x = Cursor.Position.X + mouseDownPoint.X;
                int y = Cursor.Position.Y + mouseDownPoint.Y;
                Win32ImageList.ImageList_DragMove(x, y);
                /*
                Rectangle dragRegion = new Rectangle(
                    mouseDownPoint.X - SystemInformation.DragSize.Width / 2,
                    mouseDownPoint.Y - SystemInformation.DragSize.Height / 2,
                    SystemInformation.DragSize.Width,
                    SystemInformation.DragSize.Height);
                if (!dragRegion.Contains(e.X, e.Y))
                {
                    
                    Win32ImageList.ImageList_DragMove(e.X, e.Y);
                    mouseDownPoint = Point.Empty;
                }
                */

            }
        }

        private void splitContainer1_Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var image = avatarlist[0].image;
                mouseDownPoint = new Point(e.X - this.Bounds.X - image.Width / 2, e.Y - this.Bounds.Y - image.Height / 2);

                // Imageの初期化 
                imageList.Images.Clear();
                imageList.ImageSize = new Size(image.Width, image.Height);

                // 半透明イメージの元画像を作成、ImageListに追加 
                imageList.Images.Add(image);

                // ImageList_BeginDragにはドラッグする
                // イメージの中における相対座標を指定する 
                Win32ImageList.ImageList_BeginDrag(imageList.Handle, 0, e.X, e.Y);
                int x = Cursor.Position.X + mouseDownPoint.X;
                int y = Cursor.Position.Y + mouseDownPoint.Y;
                Win32ImageList.ImageList_DragEnter(this.Handle, x, y);
            }
        }

        private void splitContainer1_Panel2_DragEnter(object sender, DragEventArgs e)
        {
            // ImageList_DragEnterにはクライアント領域における相対座標ではなく 
            // タイトルバーなどの非クライアント領域を含むWindowにおける相対座標を指定する 
            Point p = this.PointToClient(Cursor.Position);
            int x = Cursor.Position.X - this.Left;
            int y = Cursor.Position.Y - this.Top;
            // ドラッグ中は半透明イメージを表示し続けたいのでImageList_DragEnterには 
            // ListBoxのHandleを渡すのでなく、FormのHandleを渡す 
            Win32ImageList.ImageList_DragEnter(this.Handle, x, y);
        }

        private void splitContainer1_Panel2_DragLeave(object sender, EventArgs e)
        {
            Win32ImageList.ImageList_DragLeave(this.Handle);

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Win32ImageList.ImageList_EndDrag();
                mouseDownPoint = Point.Empty;
            }
        }

        void ScopeChange(int scope)
        {
            ToolStripMenuItem [] menu = {
                x14ToolStripMenuItem,
                x12ToolStripMenuItem,
                x1ToolStripMenuItem,
                x2ToolStripMenuItem,
                x4ToolStripMenuItem,
            };

            for (int i = 0; i < menu.Length; i++)
            {
                menu[i].Checked = i == scope;
                if (i == scope)
                {
                    menu[i].CheckState = System.Windows.Forms.CheckState.Checked;
                }
            }

            if (timeline != null)
            {
                this.scope = scope;
                timeline.scope = scope;
                timeline.Invalidate();
            }
        }

        private void x14ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScopeChange(0);
        }

        private void x12ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScopeChange(1);
        }

        private void x1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScopeChange(2);
        }

        private void x2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScopeChange(3);
        }

        private void x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScopeChange(4);
        }

        private void timelineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timelineToolStripMenuItem.Checked)
            {
                timeline.Close();
                timeline = null;
                timelineToolStripMenuItem.Checked = false;
            }
            else
            {
                OpenTimeline();
                timelineToolStripMenuItem.Checked = true;
            }
        }

        private void charaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (charaToolStripMenuItem.Checked)
            {
                chara.Close();
                chara = null;
                charaToolStripMenuItem.Checked = false;
            }
            else
            {
                OpenChara();
                charaToolStripMenuItem.Checked = true;
            }

        }
    }
}

