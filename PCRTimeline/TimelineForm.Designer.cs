namespace PCRTimeline
{
    partial class TimelineForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unionburstToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.skillToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.floatTimeline = new System.Windows.Forms.Panel();
            this.timelinePicture = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            this.floatTimeline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timelinePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem,
            this.addDataToolStripMenuItem,
            this.deleteDataToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(141, 88);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(140, 28);
            this.resetToolStripMenuItem.Text = "リセット";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // addDataToolStripMenuItem
            // 
            this.addDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unionburstToolStripMenuItem,
            this.bindToolStripMenuItem,
            this.deadToolStripMenuItem});
            this.addDataToolStripMenuItem.Name = "addDataToolStripMenuItem";
            this.addDataToolStripMenuItem.Size = new System.Drawing.Size(140, 28);
            this.addDataToolStripMenuItem.Text = "追加";
            // 
            // unionburstToolStripMenuItem
            // 
            this.unionburstToolStripMenuItem.Name = "unionburstToolStripMenuItem";
            this.unionburstToolStripMenuItem.Size = new System.Drawing.Size(214, 28);
            this.unionburstToolStripMenuItem.Text = "ユニオンバースト";
            this.unionburstToolStripMenuItem.Click += new System.EventHandler(this.unionburstToolStripMenuItem_Click);
            // 
            // bindToolStripMenuItem
            // 
            this.bindToolStripMenuItem.Name = "bindToolStripMenuItem";
            this.bindToolStripMenuItem.Size = new System.Drawing.Size(214, 28);
            this.bindToolStripMenuItem.Text = "行動不能";
            this.bindToolStripMenuItem.Click += new System.EventHandler(this.bindToolStripMenuItem_Click);
            // 
            // deadToolStripMenuItem
            // 
            this.deadToolStripMenuItem.Name = "deadToolStripMenuItem";
            this.deadToolStripMenuItem.Size = new System.Drawing.Size(214, 28);
            this.deadToolStripMenuItem.Text = "戦闘不能";
            this.deadToolStripMenuItem.Click += new System.EventHandler(this.deadToolStripMenuItem_Click);
            // 
            // deleteDataToolStripMenuItem
            // 
            this.deleteDataToolStripMenuItem.Name = "deleteDataToolStripMenuItem";
            this.deleteDataToolStripMenuItem.Size = new System.Drawing.Size(140, 28);
            this.deleteDataToolStripMenuItem.Text = "削除";
            this.deleteDataToolStripMenuItem.Click += new System.EventHandler(this.deleteDataToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // skillToolTip
            // 
            this.skillToolTip.AutoPopDelay = 3000;
            this.skillToolTip.InitialDelay = 500;
            this.skillToolTip.ReshowDelay = 100;
            // 
            // floatTimeline
            // 
            this.floatTimeline.AutoScroll = true;
            this.floatTimeline.Controls.Add(this.timelinePicture);
            this.floatTimeline.Controls.Add(this.pictureBox1);
            this.floatTimeline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.floatTimeline.Location = new System.Drawing.Point(0, 0);
            this.floatTimeline.Name = "floatTimeline";
            this.floatTimeline.Size = new System.Drawing.Size(284, 261);
            this.floatTimeline.TabIndex = 1;
            this.floatTimeline.Scroll += new System.Windows.Forms.ScrollEventHandler(this.TimelineForm_Scroll);
            // 
            // timelinePicture
            // 
            this.timelinePicture.Location = new System.Drawing.Point(0, 0);
            this.timelinePicture.Name = "timelinePicture";
            this.timelinePicture.Size = new System.Drawing.Size(16, 16);
            this.timelinePicture.TabIndex = 1;
            this.timelinePicture.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(284, 261);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TimelineForm_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimelineForm_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TimelineForm_MouseUp);
            // 
            // TimelineForm
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.floatTimeline);
            this.DoubleBuffered = true;
            this.HideOnClose = true;
            this.Name = "TimelineForm";
            this.Text = "Timeline";
            this.Load += new System.EventHandler(this.TimelineForm_Load);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.TimelineForm_Scroll);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TimelineForm_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimelineForm_MouseMove);
            this.contextMenuStrip1.ResumeLayout(false);
            this.floatTimeline.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.timelinePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unionburstToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bindToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deadToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolTip skillToolTip;
        private System.Windows.Forms.Panel floatTimeline;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox timelinePicture;
    }
}
