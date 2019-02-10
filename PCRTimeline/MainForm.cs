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

        string currentFilename = "";

        int scope = 2;

        public MainForm()
        {
            InitializeComponent();
        }

        void AvatarLoad()
        {
            {
                const string path = @"Data\";
                string[] files = System.IO.Directory.GetFiles(path, "*.xml", System.IO.SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    //XMLファイルから読み込む
                    System.Xml.Serialization.XmlSerializer serializer1 =
                        new System.Xml.Serialization.XmlSerializer(typeof(Avatar));
                    System.IO.StreamReader sr = new System.IO.StreamReader(
                        file, new System.Text.UTF8Encoding(false));
                    Avatar avatar = (Avatar)serializer1.Deserialize(sr);
                    sr.Close();

                    string iconfile = path + avatar.icon;
                    if (avatar.icon != null && System.IO.File.Exists(iconfile))
                    {
                        avatar.image = new Bitmap(Image.FromFile(iconfile), new Size(IconSize, IconSize));
                    }
                    else
                    {
                        avatar.image = CreateIcon(avatar);
                    }

                    avatarlist.Add(avatar);
                }
            }

        }

        private Image CreateIcon(Avatar avatar)
        {
            Bitmap bitmap = new Bitmap(IconSize, IconSize);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                g.DrawRectangle(Pens.LightGray, new Rectangle(0, 0, bitmap.Width - 1, bitmap.Height - 1));
                g.DrawString(avatar.name, this.Font, Brushes.Black, 2, 2);
            }

            return bitmap;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XmlWrite.SampleWrite();

            AvatarLoad();

            OpenTimeline();

            OpenChara();

        }

        private void OpenChara()
        {
            chara = new CharaForm();
            chara.avatarlist = avatarlist;
            chara.battlelist = battlerlist;
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

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exportImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = "export.png";
            int secondsize = 8;

            Bitmap bitmap = new Bitmap(IconSize + 95 * secondsize, IconSize * battlerlist.Count);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                timeline.DrawTimeline(g, IconSize, secondsize, 0, bitmap.Size);
            }

            bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Png);

        }

        private void fileOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "tml files (*.tml)|*.tml|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    var filePath = openFileDialog.FileName;

                    var sketch = TimelineSketch.Load(filePath);
                    sketch.avatarlist = avatarlist;
                    battlerlist.Clear();
                    var deSerialize = sketch.DeSerialize();
                    battlerlist.AddRange(deSerialize);
                    currentFilename = filePath;

                    timeline.Invalidate();
                }
            }
        }

        private void insertOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "tml files (*.tml)|*.tml|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    var filePath = openFileDialog.FileName;

                    var sketch = TimelineSketch.Load(filePath);
                    sketch.avatarlist = avatarlist;

                    var deSerialize = sketch.DeSerialize();
                    battlerlist.AddRange(deSerialize);
                    currentFilename = filePath;

                    timeline.battlerlist = battlerlist;
                    timeline.Invalidate();
                }
            }
        }

        private void fileSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFilename.Length == 0)
            {
                fileSaveAsToolStripMenuItem_Click(sender, e);
                return;
            }

            TimelineSketch sketch = new TimelineSketch();
            sketch.Serialize(battlerlist);

            TimelineSketch.Save(sketch, currentFilename);
        }

        private void fileSaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "tml files (*.tml)|*.tml|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    currentFilename = saveFileDialog.FileName;

                    TimelineSketch sketch = new TimelineSketch();
                    sketch.Serialize(battlerlist);

                    TimelineSketch.Save(sketch, currentFilename);
                }
            }
        }

    }
}

