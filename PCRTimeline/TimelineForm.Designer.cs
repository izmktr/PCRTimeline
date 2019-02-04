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
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.リセットToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.データを追加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.データを削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(0, 244);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(284, 17);
            this.hScrollBar1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.リセットToolStripMenuItem,
            this.データを追加ToolStripMenuItem,
            this.データを削除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 116);
            // 
            // リセットToolStripMenuItem
            // 
            this.リセットToolStripMenuItem.Name = "リセットToolStripMenuItem";
            this.リセットToolStripMenuItem.Size = new System.Drawing.Size(210, 28);
            this.リセットToolStripMenuItem.Text = "リセット";
            // 
            // データを追加ToolStripMenuItem
            // 
            this.データを追加ToolStripMenuItem.Name = "データを追加ToolStripMenuItem";
            this.データを追加ToolStripMenuItem.Size = new System.Drawing.Size(210, 28);
            this.データを追加ToolStripMenuItem.Text = "データを追加";
            // 
            // データを削除ToolStripMenuItem
            // 
            this.データを削除ToolStripMenuItem.Name = "データを削除ToolStripMenuItem";
            this.データを削除ToolStripMenuItem.Size = new System.Drawing.Size(210, 28);
            this.データを削除ToolStripMenuItem.Text = "データを削除";
            // 
            // TimelineForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.hScrollBar1);
            this.Name = "TimelineForm";
            this.Text = "Timeline";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TimelineForm_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TimelineForm_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TimelineForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimelineForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TimelineForm_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem リセットToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem データを追加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem データを削除ToolStripMenuItem;
    }
}
