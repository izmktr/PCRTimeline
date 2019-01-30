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
    public partial class Form1 : Form
    {
        List<Avatar> avatarlist = new List<Avatar>();
        System.Windows.Forms.ImageList imageList;

        public Form1()
        {
            InitializeComponent();
            imageList = new System.Windows.Forms.ImageList();
        }

        void AvatarLoad()
        {
            const string path = @"Data\Icon\";
            string[] files = System.IO.Directory.GetFiles(path, "*.png", System.IO.SearchOption.AllDirectories);

            foreach (var file in files)
            {
                avatarlist.Add(new Avatar()
                {
                    Name = file,
                    image = Image.FromFile(file)
                });
            }

            for (int i = 0; i < 10; i++)
            {
                avatarlist.Add(avatarlist[0]);
                avatarlist.Add(avatarlist[1]);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AvatarLoad();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            Control c = (Control)sender;
            int width = c.Width - iconScrollBar.Width;
            int height = c.Height;

            int x = 0, y = 0, maxheight = 0;
            foreach (var avatar in avatarlist)
            {
                Image image = avatar.image;

                //DrawImageメソッドで画像を座標(0, 0)の位置に表示する
                e.Graphics.DrawImage(image, x, y - iconScrollBar.Value, image.Width, image.Height);
                x += image.Width;

                maxheight = Math.Max(maxheight, image.Height);
                if (0 < x && width - image.Width <= x)
                {
                    x = 0;
                    y += maxheight;
                    maxheight = 0;
                }
            }

            iconScrollBar.Maximum = y + maxheight;
            iconScrollBar.LargeChange = height;

        }

        private void iconScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            splitContainer1.Panel2.Invalidate();
        }


        Point mouseDownPoint = Point.Empty;

        private void splitContainer1_Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownPoint != Point.Empty)
            {
                Rectangle dragRegion = new Rectangle(
                    mouseDownPoint.X - SystemInformation.DragSize.Width / 2,
                    mouseDownPoint.Y - SystemInformation.DragSize.Height / 2,
                    SystemInformation.DragSize.Width,
                    SystemInformation.DragSize.Height);
                if (!dragRegion.Contains(e.X, e.Y))
                {
                    var image = avatarlist[0].image;


                    // Imageの初期化 
                    imageList.Images.Clear();
                    imageList.ImageSize = new Size(image.Width, image.Height);

                    // 半透明イメージの元画像を作成、ImageListに追加 
                    imageList.Images.Add(image);

                    // ImageList_BeginDragにはドラッグする
                    // イメージの中における相対座標を指定する 
                    if (Win32ImageList.ImageList_BeginDrag(imageList.Handle, 0,
                                                     e.X,
                                                     e.Y))
                    {
                        Win32ImageList.ImageList_EndDrag();
                    }
                    mouseDownPoint = Point.Empty;
                }

            }
        }

        private void splitContainer1_Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownPoint = new Point(e.X, e.Y);
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
    }
}

