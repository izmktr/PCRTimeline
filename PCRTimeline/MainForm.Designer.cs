﻿namespace PCRTimeline
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scopeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timelineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.charaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.x14ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x12ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.scopeToolStripMenuItem,
            this.windowToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(819, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.fileToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // scopeToolStripMenuItem
            // 
            this.scopeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x14ToolStripMenuItem,
            this.x12ToolStripMenuItem,
            this.x1ToolStripMenuItem,
            this.x2ToolStripMenuItem,
            this.x4ToolStripMenuItem});
            this.scopeToolStripMenuItem.Name = "scopeToolStripMenuItem";
            this.scopeToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.scopeToolStripMenuItem.Text = "倍率(&X)";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timelineToolStripMenuItem,
            this.charaToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.windowToolStripMenuItem.Text = "ウィンドウ(&W)";
            // 
            // timelineToolStripMenuItem
            // 
            this.timelineToolStripMenuItem.Checked = true;
            this.timelineToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.timelineToolStripMenuItem.Name = "timelineToolStripMenuItem";
            this.timelineToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.timelineToolStripMenuItem.Text = "タイムライン(&T)";
            this.timelineToolStripMenuItem.Click += new System.EventHandler(this.timelineToolStripMenuItem_Click);
            // 
            // charaToolStripMenuItem
            // 
            this.charaToolStripMenuItem.Checked = true;
            this.charaToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.charaToolStripMenuItem.Name = "charaToolStripMenuItem";
            this.charaToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.charaToolStripMenuItem.Text = "キャラクター(&C)";
            this.charaToolStripMenuItem.Click += new System.EventHandler(this.charaToolStripMenuItem_Click);
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockPanel1.Location = new System.Drawing.Point(0, 24);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(819, 441);
            this.dockPanel1.TabIndex = 1;
            // 
            // x14ToolStripMenuItem
            // 
            this.x14ToolStripMenuItem.Name = "x14ToolStripMenuItem";
            this.x14ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.x14ToolStripMenuItem.Text = "x1/4";
            this.x14ToolStripMenuItem.Click += new System.EventHandler(this.x14ToolStripMenuItem_Click);
            // 
            // x12ToolStripMenuItem
            // 
            this.x12ToolStripMenuItem.Name = "x12ToolStripMenuItem";
            this.x12ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.x12ToolStripMenuItem.Text = "x1/2";
            this.x12ToolStripMenuItem.Click += new System.EventHandler(this.x12ToolStripMenuItem_Click);
            // 
            // x1ToolStripMenuItem
            // 
            this.x1ToolStripMenuItem.Checked = true;
            this.x1ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x1ToolStripMenuItem.Name = "x1ToolStripMenuItem";
            this.x1ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.x1ToolStripMenuItem.Text = "x1";
            this.x1ToolStripMenuItem.Click += new System.EventHandler(this.x1ToolStripMenuItem_Click);
            // 
            // x2ToolStripMenuItem
            // 
            this.x2ToolStripMenuItem.Name = "x2ToolStripMenuItem";
            this.x2ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.x2ToolStripMenuItem.Text = "x2";
            this.x2ToolStripMenuItem.Click += new System.EventHandler(this.x2ToolStripMenuItem_Click);
            // 
            // x4ToolStripMenuItem
            // 
            this.x4ToolStripMenuItem.Name = "x4ToolStripMenuItem";
            this.x4ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.x4ToolStripMenuItem.Text = "x4";
            this.x4ToolStripMenuItem.Click += new System.EventHandler(this.x4ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 465);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "PCR Timeline";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scopeToolStripMenuItem;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timelineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem charaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x14ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x12ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x4ToolStripMenuItem;
    }
}

