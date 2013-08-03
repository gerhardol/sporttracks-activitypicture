/*
Copyright (C) 2008 Dominik Laufer

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 3 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library. If not, see <http://www.gnu.org/licenses/>.
 */

namespace ActivityPicturePlugin.UI
{
    partial class ImportControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.btnChangeFolderView = new ZoneFiveSoftware.Common.Visuals.Button();
            this.treeViewImages = new System.Windows.Forms.TreeView();
            this.contextMenuTreeViewImages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.btnScan = new ZoneFiveSoftware.Common.Visuals.Button();
            this.listViewDrive = new ActivityPicturePlugin.UI.ListViewEx();
            this.colDImage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDDateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDGPS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuListViewDrive = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnChangeActivityView = new ZoneFiveSoftware.Common.Visuals.Button();
            this.btnExpandAll = new ZoneFiveSoftware.Common.Visuals.Button();
            this.btnCollapseAll = new ZoneFiveSoftware.Common.Visuals.Button();
            this.treeViewActivities = new System.Windows.Forms.TreeView();
            this.contextMenuTreeViewActivities = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuCopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuMigratePaths = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewAct = new ActivityPicturePlugin.UI.ListViewEx();
            this.colImage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGPS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuListViewAct = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.timerProgressBar = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.contextMenuTreeViewImages.SuspendLayout();
            this.contextMenuListViewDrive.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuTreeViewActivities.SuspendLayout();
            this.contextMenuListViewAct.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(4, 404);
            this.lblProgress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(65, 17);
            this.lblProgress.TabIndex = 10;
            this.lblProgress.Text = "Progress";
            // 
            // progressBar2
            // 
            this.progressBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar2.Location = new System.Drawing.Point(4, 403);
            this.progressBar2.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar2.MarqueeAnimationSpeed = 1000;
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(700, 23);
            this.progressBar2.Step = 1;
            this.progressBar2.TabIndex = 9;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(700, 421);
            this.splitContainer1.SplitterDistance = 224;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 11;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.btnChangeFolderView);
            this.splitContainer3.Panel1.Controls.Add(this.treeViewImages);
            this.splitContainer3.Panel1.Controls.Add(this.btnScan);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.listViewDrive);
            this.splitContainer3.Size = new System.Drawing.Size(224, 421);
            this.splitContainer3.SplitterDistance = 202;
            this.splitContainer3.SplitterWidth = 2;
            this.splitContainer3.TabIndex = 13;
            this.splitContainer3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.splitContainer3_MouseDoubleClick);
            // 
            // btnChangeFolderView
            // 
            this.btnChangeFolderView.BackColor = System.Drawing.Color.Transparent;
            this.btnChangeFolderView.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.btnChangeFolderView.CenterImage = null;
            this.btnChangeFolderView.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnChangeFolderView.HyperlinkStyle = false;
            this.btnChangeFolderView.ImageMargin = 2;
            this.btnChangeFolderView.LeftImage = null;
            this.btnChangeFolderView.Location = new System.Drawing.Point(0, 4);
            this.btnChangeFolderView.Margin = new System.Windows.Forms.Padding(5);
            this.btnChangeFolderView.Name = "btnChangeFolderView";
            this.btnChangeFolderView.PushStyle = true;
            this.btnChangeFolderView.RightImage = null;
            this.btnChangeFolderView.Size = new System.Drawing.Size(100, 28);
            this.btnChangeFolderView.TabIndex = 0;
            this.btnChangeFolderView.Text = "Change View";
            this.btnChangeFolderView.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnChangeFolderView.TextLeftMargin = 2;
            this.btnChangeFolderView.TextRightMargin = 2;
            this.btnChangeFolderView.Click += new System.EventHandler(this.btnChangeFolderView_Click);
            // 
            // treeViewImages
            // 
            this.treeViewImages.AllowDrop = true;
            this.treeViewImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewImages.CheckBoxes = true;
            this.treeViewImages.ContextMenuStrip = this.contextMenuTreeViewImages;
            this.treeViewImages.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewImages.HideSelection = false;
            this.treeViewImages.Location = new System.Drawing.Point(0, 38);
            this.treeViewImages.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewImages.Name = "treeViewImages";
            this.treeViewImages.Size = new System.Drawing.Size(224, 164);
            this.treeViewImages.TabIndex = 5;
            this.treeViewImages.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewImages_AfterCheck);
            this.treeViewImages.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewImages_BeforeExpand);
            this.treeViewImages.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewImages_DrawNode);
            this.treeViewImages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewImages_AfterSelect);
            this.treeViewImages.EnabledChanged += new System.EventHandler(this.treeViewImages_EnabledChanged);
            this.treeViewImages.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewImages_MouseClick);
            // 
            // contextMenuTreeViewImages
            // 
            this.contextMenuTreeViewImages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuRefresh});
            this.contextMenuTreeViewImages.Name = "contextMenuTreeViewImages";
            this.contextMenuTreeViewImages.Size = new System.Drawing.Size(127, 26);
            // 
            // toolStripMenuRefresh
            // 
            this.toolStripMenuRefresh.Name = "toolStripMenuRefresh";
            this.toolStripMenuRefresh.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuRefresh.Text = "Refresh";
            this.toolStripMenuRefresh.Click += new System.EventHandler(this.toolStripMenuRefresh_Click);
            // 
            // btnScan
            // 
            this.btnScan.BackColor = System.Drawing.Color.Transparent;
            this.btnScan.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.btnScan.CenterImage = null;
            this.btnScan.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnScan.HyperlinkStyle = false;
            this.btnScan.ImageMargin = 2;
            this.btnScan.LeftImage = null;
            this.btnScan.Location = new System.Drawing.Point(101, 4);
            this.btnScan.Margin = new System.Windows.Forms.Padding(5);
            this.btnScan.Name = "btnScan";
            this.btnScan.PushStyle = true;
            this.btnScan.RightImage = null;
            this.btnScan.Size = new System.Drawing.Size(100, 28);
            this.btnScan.TabIndex = 1;
            this.btnScan.Text = "Auto Scan";
            this.btnScan.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnScan.TextLeftMargin = 2;
            this.btnScan.TextRightMargin = 2;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // listViewDrive
            // 
            this.listViewDrive.AllowDrop = true;
            this.listViewDrive.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDImage,
            this.colDDateTime,
            this.colDGPS,
            this.colDTitle,
            this.colDDescription});
            this.listViewDrive.ContextMenuStrip = this.contextMenuListViewDrive;
            this.listViewDrive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDrive.DoubleBufferedEnabled = true;
            this.listViewDrive.FullRowSelect = true;
            this.listViewDrive.Location = new System.Drawing.Point(0, 0);
            this.listViewDrive.Margin = new System.Windows.Forms.Padding(4);
            this.listViewDrive.Name = "listViewDrive";
            this.listViewDrive.OwnerDraw = true;
            this.listViewDrive.Size = new System.Drawing.Size(224, 217);
            this.listViewDrive.TabIndex = 7;
            this.listViewDrive.TileSize = new System.Drawing.Size(328, 104);
            this.listViewDrive.UseCompatibleStateImageBehavior = false;
            this.listViewDrive.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewDrive_ColumnClick);
            this.listViewDrive.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listViewDrive_DrawColumnHeader);
            this.listViewDrive.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewDrive_DrawItem);
            this.listViewDrive.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewDrive_ItemDrag);
            this.listViewDrive.DoubleClick += new System.EventHandler(this.listViewDrive_DoubleClick);
            this.listViewDrive.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewDrive_KeyDown);
            // 
            // colDImage
            // 
            this.colDImage.Tag = "colImage";
            this.colDImage.Text = "Image";
            this.colDImage.Width = 150;
            // 
            // colDDateTime
            // 
            this.colDDateTime.Tag = "colDateTime";
            this.colDDateTime.Text = "DateTime";
            this.colDDateTime.Width = 100;
            // 
            // colDGPS
            // 
            this.colDGPS.Tag = "colGPS";
            this.colDGPS.Text = "GPS coord.";
            this.colDGPS.Width = 120;
            // 
            // colDTitle
            // 
            this.colDTitle.Tag = "colTitle";
            this.colDTitle.Text = "Title";
            this.colDTitle.Width = 100;
            // 
            // colDDescription
            // 
            this.colDDescription.Tag = "colDescription";
            this.colDDescription.Text = "Description";
            this.colDDescription.Width = 100;
            // 
            // contextMenuListViewDrive
            // 
            this.contextMenuListViewDrive.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuAdd});
            this.contextMenuListViewDrive.Name = "contextMenuListViewDrive";
            this.contextMenuListViewDrive.Size = new System.Drawing.Size(102, 26);
            this.contextMenuListViewDrive.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuListViewDrive_Opening);
            // 
            // toolStripMenuAdd
            // 
            this.toolStripMenuAdd.Name = "toolStripMenuAdd";
            this.toolStripMenuAdd.Size = new System.Drawing.Size(101, 22);
            this.toolStripMenuAdd.Text = "Add";
            this.toolStripMenuAdd.Click += new System.EventHandler(this.toolStripMenuAdd_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnChangeActivityView);
            this.splitContainer2.Panel1.Controls.Add(this.btnExpandAll);
            this.splitContainer2.Panel1.Controls.Add(this.btnCollapseAll);
            this.splitContainer2.Panel1.Controls.Add(this.treeViewActivities);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listViewAct);
            this.splitContainer2.Size = new System.Drawing.Size(473, 421);
            this.splitContainer2.SplitterDistance = 202;
            this.splitContainer2.SplitterWidth = 2;
            this.splitContainer2.TabIndex = 12;
            this.splitContainer2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.splitContainer2_MouseDoubleClick);
            // 
            // btnChangeActivityView
            // 
            this.btnChangeActivityView.BackColor = System.Drawing.Color.Transparent;
            this.btnChangeActivityView.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.btnChangeActivityView.CenterImage = null;
            this.btnChangeActivityView.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnChangeActivityView.HyperlinkStyle = false;
            this.btnChangeActivityView.ImageMargin = 2;
            this.btnChangeActivityView.LeftImage = null;
            this.btnChangeActivityView.Location = new System.Drawing.Point(0, 4);
            this.btnChangeActivityView.Margin = new System.Windows.Forms.Padding(5);
            this.btnChangeActivityView.Name = "btnChangeActivityView";
            this.btnChangeActivityView.PushStyle = true;
            this.btnChangeActivityView.RightImage = null;
            this.btnChangeActivityView.Size = new System.Drawing.Size(100, 28);
            this.btnChangeActivityView.TabIndex = 2;
            this.btnChangeActivityView.Text = "Change View";
            this.btnChangeActivityView.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnChangeActivityView.TextLeftMargin = 2;
            this.btnChangeActivityView.TextRightMargin = 2;
            this.btnChangeActivityView.Click += new System.EventHandler(this.btnChangeActivityView_Click);
            // 
            // btnExpandAll
            // 
            this.btnExpandAll.BackColor = System.Drawing.Color.Transparent;
            this.btnExpandAll.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.btnExpandAll.CenterImage = null;
            this.btnExpandAll.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnExpandAll.HyperlinkStyle = false;
            this.btnExpandAll.ImageMargin = 2;
            this.btnExpandAll.LeftImage = null;
            this.btnExpandAll.Location = new System.Drawing.Point(100, 4);
            this.btnExpandAll.Margin = new System.Windows.Forms.Padding(5);
            this.btnExpandAll.Name = "btnExpandAll";
            this.btnExpandAll.PushStyle = true;
            this.btnExpandAll.RightImage = null;
            this.btnExpandAll.Size = new System.Drawing.Size(100, 28);
            this.btnExpandAll.TabIndex = 3;
            this.btnExpandAll.Text = "Expand All";
            this.btnExpandAll.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnExpandAll.TextLeftMargin = 2;
            this.btnExpandAll.TextRightMargin = 2;
            this.toolTip1.SetToolTip(this.btnExpandAll, "Expand All");
            this.btnExpandAll.Click += new System.EventHandler(this.btnExpandAll_Click);
            // 
            // btnCollapseAll
            // 
            this.btnCollapseAll.BackColor = System.Drawing.Color.Transparent;
            this.btnCollapseAll.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.btnCollapseAll.CenterImage = null;
            this.btnCollapseAll.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnCollapseAll.HyperlinkStyle = false;
            this.btnCollapseAll.ImageMargin = 2;
            this.btnCollapseAll.LeftImage = null;
            this.btnCollapseAll.Location = new System.Drawing.Point(231, 4);
            this.btnCollapseAll.Margin = new System.Windows.Forms.Padding(5);
            this.btnCollapseAll.Name = "btnCollapseAll";
            this.btnCollapseAll.PushStyle = true;
            this.btnCollapseAll.RightImage = null;
            this.btnCollapseAll.Size = new System.Drawing.Size(100, 28);
            this.btnCollapseAll.TabIndex = 4;
            this.btnCollapseAll.Text = "Collapse All";
            this.btnCollapseAll.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnCollapseAll.TextLeftMargin = 2;
            this.btnCollapseAll.TextRightMargin = 2;
            this.btnCollapseAll.Click += new System.EventHandler(this.btnCollapsAll_Click);
            // 
            // treeViewActivities
            // 
            this.treeViewActivities.AllowDrop = true;
            this.treeViewActivities.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewActivities.ContextMenuStrip = this.contextMenuTreeViewActivities;
            this.treeViewActivities.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewActivities.FullRowSelect = true;
            this.treeViewActivities.HotTracking = true;
            this.treeViewActivities.ItemHeight = 20;
            this.treeViewActivities.Location = new System.Drawing.Point(0, 38);
            this.treeViewActivities.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewActivities.Name = "treeViewActivities";
            this.treeViewActivities.Size = new System.Drawing.Size(472, 163);
            this.treeViewActivities.TabIndex = 6;
            this.treeViewActivities.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewActivities_DrawNode);
            this.treeViewActivities.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewActivities_AfterSelect);
            this.treeViewActivities.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewActivities_NodeMouseClick);
            this.treeViewActivities.NodeMouseDoubleClick += treeViewActivities_NodeMouseDoubleClick;
            this.treeViewActivities.EnabledChanged += new System.EventHandler(this.treeViewActivities_EnabledChanged);
            // 
            // contextMenuTreeViewActivities
            // 
            this.contextMenuTreeViewActivities.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuCopyToClipboard, this.toolStripMenuMigratePaths});
            this.contextMenuTreeViewActivities.Name = "contextMenuTreeViewActivities";
            this.contextMenuTreeViewActivities.Size = new System.Drawing.Size(110, 26);
            // 
            // toolStripMenuCopyToClipboard
            // 
            this.toolStripMenuCopyToClipboard.Name = "toolStripMenuCopyToClipboard";
            this.toolStripMenuCopyToClipboard.Size = new System.Drawing.Size(109, 22);
            this.toolStripMenuCopyToClipboard.Text = "Copy";
            this.toolStripMenuCopyToClipboard.Click += new System.EventHandler(this.toolStripMenuCopyToClipboard_Click);
            // 
            // toolStripMenuMigratePaths
            // 
            this.toolStripMenuMigratePaths.Name = "toolStripMenuMigratePaths";
            this.toolStripMenuMigratePaths.Size = new System.Drawing.Size(109, 22);
            this.toolStripMenuMigratePaths.Text = "<Migrate Paths...";
            this.toolStripMenuMigratePaths.Click += new System.EventHandler(this.toolStripMenuMigratePaths_Click);
            // 
            // listViewAct
            // 
            this.listViewAct.AllowDrop = true;
            this.listViewAct.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colImage,
            this.colDateTime,
            this.colGPS,
            this.colTitle,
            this.colDescription});
            this.listViewAct.ContextMenuStrip = this.contextMenuListViewAct;
            this.listViewAct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewAct.DoubleBufferedEnabled = true;
            this.listViewAct.FullRowSelect = true;
            this.listViewAct.Location = new System.Drawing.Point(0, 0);
            this.listViewAct.Margin = new System.Windows.Forms.Padding(4);
            this.listViewAct.Name = "listViewAct";
            this.listViewAct.OwnerDraw = true;
            this.listViewAct.Size = new System.Drawing.Size(473, 217);
            this.listViewAct.TabIndex = 8;
            this.listViewAct.TileSize = new System.Drawing.Size(328, 104);
            this.listViewAct.UseCompatibleStateImageBehavior = false;
            this.listViewAct.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewAct_ColumnClick);
            this.listViewAct.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listViewAct_DrawColumnHeader);
            this.listViewAct.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewAct_DrawItem);
            this.listViewAct.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewAct_DragDrop);
            this.listViewAct.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewAct_DragEnter);
            this.listViewAct.DoubleClick += new System.EventHandler(this.listViewAct_DoubleClick);
            this.listViewAct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewAct_KeyDown);
            // 
            // colImage
            // 
            this.colImage.Tag = "colImage";
            this.colImage.Text = "Image";
            this.colImage.Width = 150;
            // 
            // colDateTime
            // 
            this.colDateTime.Tag = "colDateTime";
            this.colDateTime.Text = "DateTime";
            this.colDateTime.Width = 100;
            // 
            // colGPS
            // 
            this.colGPS.Tag = "colGPS";
            this.colGPS.Text = "GPS coord.";
            this.colGPS.Width = 120;
            // 
            // colTitle
            // 
            this.colTitle.Tag = "colTitle";
            this.colTitle.Text = "Title";
            this.colTitle.Width = 100;
            // 
            // colDescription
            // 
            this.colDescription.Tag = "colDescription";
            this.colDescription.Text = "Description";
            this.colDescription.Width = 100;
            // 
            // contextMenuListViewAct
            // 
            this.contextMenuListViewAct.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuOpenFolder,
            this.toolStripMenuRemove});
            this.contextMenuListViewAct.Name = "contextMenuListViewAct";
            this.contextMenuListViewAct.Size = new System.Drawing.Size(227, 48);
            this.contextMenuListViewAct.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuListViewAct_Opening);
            // 
            // toolStripMenuOpenFolder
            // 
            this.toolStripMenuOpenFolder.Name = "toolStripMenuOpenFolder";
            this.toolStripMenuOpenFolder.Size = new System.Drawing.Size(226, 22);
            this.toolStripMenuOpenFolder.Text = "Open Containing Folder";
            this.toolStripMenuOpenFolder.Click += new System.EventHandler(this.toolStripMenuOpenFolder_Click);
            // 
            // toolStripMenuRemove
            // 
            this.toolStripMenuRemove.Name = "toolStripMenuRemove";
            this.toolStripMenuRemove.Size = new System.Drawing.Size(226, 22);
            this.toolStripMenuRemove.Text = "Remove";
            this.toolStripMenuRemove.Click += new System.EventHandler(this.toolStripMenuRemove_Click);
            // 
            // timerProgressBar
            // 
            this.timerProgressBar.Interval = 5000;
            this.timerProgressBar.Tick += new System.EventHandler(this.timerProgressBar_Tick);
            // 
            // ImportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ImportControl";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(708, 429);
            this.Load += new System.EventHandler(this.ImportControl_Load);
            this.Resize += new System.EventHandler(this.ImportControl_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.contextMenuTreeViewImages.ResumeLayout(false);
            this.contextMenuListViewDrive.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuTreeViewActivities.ResumeLayout(false);
            this.contextMenuListViewAct.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewImages;
        private System.Windows.Forms.TreeView treeViewActivities;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ZoneFiveSoftware.Common.Visuals.Button btnCollapseAll;
        private ZoneFiveSoftware.Common.Visuals.Button btnExpandAll;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar2;
        private ZoneFiveSoftware.Common.Visuals.Button btnScan;
        private ListViewEx listViewAct;
        private System.Windows.Forms.ColumnHeader colImage;
        private System.Windows.Forms.ColumnHeader colDateTime;
        private System.Windows.Forms.ColumnHeader colTitle;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.ColumnHeader colGPS;
        private System.Windows.Forms.ColumnHeader colDImage;
        private System.Windows.Forms.ColumnHeader colDDateTime;
        private System.Windows.Forms.ColumnHeader colDTitle;
        private System.Windows.Forms.ColumnHeader colDDescription;
        private System.Windows.Forms.ColumnHeader colDGPS;
        private ListViewEx listViewDrive;
        private ZoneFiveSoftware.Common.Visuals.Button btnChangeFolderView;
        private ZoneFiveSoftware.Common.Visuals.Button btnChangeActivityView;
        private System.Windows.Forms.Timer timerProgressBar;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuListViewDrive;
        private System.Windows.Forms.ContextMenuStrip contextMenuListViewAct;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuAdd;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuRemove;
        private System.Windows.Forms.ContextMenuStrip contextMenuTreeViewImages;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuRefresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuTreeViewActivities;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuCopyToClipboard;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuMigratePaths;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuOpenFolder;
    }
}