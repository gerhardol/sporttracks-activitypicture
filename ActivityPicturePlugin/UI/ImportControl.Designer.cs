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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        this.treeViewImages = new System.Windows.Forms.TreeView();
        this.treeViewActivities = new System.Windows.Forms.TreeView();
        this.splitContainer1 = new System.Windows.Forms.SplitContainer();
        this.splitContainer3 = new System.Windows.Forms.SplitContainer();
        this.btnViewFolder = new ZoneFiveSoftware.Common.Visuals.Button();
        this.btnScan = new ZoneFiveSoftware.Common.Visuals.Button();
        this.listViewDrive = new System.Windows.Forms.ListView();
        this.colDImage = new System.Windows.Forms.ColumnHeader();
        this.colDDateTime = new System.Windows.Forms.ColumnHeader();
        this.colDGPS = new System.Windows.Forms.ColumnHeader();
        this.colDTitle = new System.Windows.Forms.ColumnHeader();
        this.colDDescription = new System.Windows.Forms.ColumnHeader();
        this.splitContainer2 = new System.Windows.Forms.SplitContainer();
        this.btnViewAct = new ZoneFiveSoftware.Common.Visuals.Button();
        this.btnExpandAll = new ZoneFiveSoftware.Common.Visuals.Button();
        this.btnCollapseAll = new ZoneFiveSoftware.Common.Visuals.Button();
        this.listViewAct = new System.Windows.Forms.ListView();
        this.colImage = new System.Windows.Forms.ColumnHeader();
        this.colDateTime = new System.Windows.Forms.ColumnHeader();
        this.colGPS = new System.Windows.Forms.ColumnHeader();
        this.colTitle = new System.Windows.Forms.ColumnHeader();
        this.colDescription = new System.Windows.Forms.ColumnHeader();
        this.lblProgress = new System.Windows.Forms.Label();
        this.progressBar2 = new System.Windows.Forms.ProgressBar();
        this.splitContainer1.Panel1.SuspendLayout();
        this.splitContainer1.Panel2.SuspendLayout();
        this.splitContainer1.SuspendLayout();
        this.splitContainer3.Panel1.SuspendLayout();
        this.splitContainer3.Panel2.SuspendLayout();
        this.splitContainer3.SuspendLayout();
        this.splitContainer2.Panel1.SuspendLayout();
        this.splitContainer2.Panel2.SuspendLayout();
        this.splitContainer2.SuspendLayout();
        this.SuspendLayout();
        // 
        // treeViewImages
        // 
        this.treeViewImages.AllowDrop = true;
        this.treeViewImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.treeViewImages.CheckBoxes = true;
        this.treeViewImages.Location = new System.Drawing.Point(3, 32);
        this.treeViewImages.Name = "treeViewImages";
        this.treeViewImages.Size = new System.Drawing.Size(165, 189);
        this.treeViewImages.TabIndex = 1;
        this.treeViewImages.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewImages_AfterCheck);
        this.treeViewImages.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewImages_BeforeExpand);
        this.treeViewImages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewImages_AfterSelect);
        // 
        // treeViewActivities
        // 
        this.treeViewActivities.AllowDrop = true;
        this.treeViewActivities.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.treeViewActivities.FullRowSelect = true;
        this.treeViewActivities.HotTracking = true;
        this.treeViewActivities.ItemHeight = 20;
        this.treeViewActivities.Location = new System.Drawing.Point(3, 31);
        this.treeViewActivities.Name = "treeViewActivities";
        this.treeViewActivities.Size = new System.Drawing.Size(341, 191);
        this.treeViewActivities.TabIndex = 6;
        this.treeViewActivities.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewActivities_AfterSelect);
        // 
        // splitContainer1
        // 
        this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.splitContainer1.Location = new System.Drawing.Point(0, 0);
        this.splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
        // 
        // splitContainer1.Panel2
        // 
        this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
        this.splitContainer1.Size = new System.Drawing.Size(526, 452);
        this.splitContainer1.SplitterDistance = 172;
        this.splitContainer1.TabIndex = 8;
        // 
        // splitContainer3
        // 
        this.splitContainer3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.splitContainer3.Location = new System.Drawing.Point(0, 0);
        this.splitContainer3.Name = "splitContainer3";
        this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
        // 
        // splitContainer3.Panel1
        // 
        this.splitContainer3.Panel1.Controls.Add(this.btnViewFolder);
        this.splitContainer3.Panel1.Controls.Add(this.treeViewImages);
        this.splitContainer3.Panel1.Controls.Add(this.btnScan);
        // 
        // splitContainer3.Panel2
        // 
        this.splitContainer3.Panel2.Controls.Add(this.listViewDrive);
        this.splitContainer3.Size = new System.Drawing.Size(170, 451);
        this.splitContainer3.SplitterDistance = 224;
        this.splitContainer3.TabIndex = 12;
        // 
        // btnViewFolder
        // 
        this.btnViewFolder.BackColor = System.Drawing.Color.Transparent;
        this.btnViewFolder.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
        this.btnViewFolder.CenterImage = null;
        this.btnViewFolder.DialogResult = System.Windows.Forms.DialogResult.None;
        this.btnViewFolder.HyperlinkStyle = false;
        this.btnViewFolder.ImageMargin = 2;
        this.btnViewFolder.LeftImage = null;
        this.btnViewFolder.Location = new System.Drawing.Point(3, 3);
        this.btnViewFolder.Name = "btnViewFolder";
        this.btnViewFolder.PushStyle = true;
        this.btnViewFolder.RightImage = null;
        this.btnViewFolder.Size = new System.Drawing.Size(60, 23);
        this.btnViewFolder.TabIndex = 13;
        this.btnViewFolder.Text = "View";
        this.btnViewFolder.TextAlign = System.Drawing.StringAlignment.Center;
        this.btnViewFolder.TextLeftMargin = 2;
        this.btnViewFolder.TextRightMargin = 2;
        this.btnViewFolder.Click += new System.EventHandler(this.btnViewFolder_Click);
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
        this.btnScan.Location = new System.Drawing.Point(69, 3);
        this.btnScan.Name = "btnScan";
        this.btnScan.PushStyle = true;
        this.btnScan.RightImage = null;
        this.btnScan.Size = new System.Drawing.Size(80, 23);
        this.btnScan.TabIndex = 12;
        this.btnScan.Text = "Scan";
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
        this.listViewDrive.Dock = System.Windows.Forms.DockStyle.Fill;
        this.listViewDrive.Location = new System.Drawing.Point(0, 0);
        this.listViewDrive.Name = "listViewDrive";
        this.listViewDrive.Size = new System.Drawing.Size(170, 223);
        this.listViewDrive.TabIndex = 12;
        this.listViewDrive.UseCompatibleStateImageBehavior = false;
        this.listViewDrive.DoubleClick += new System.EventHandler(this.listViewDrive_DoubleClick);
        this.listViewDrive.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewDrive_KeyDown);
        this.listViewDrive.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewDrive_ItemDrag);
        // 
        // colDImage
        // 
        this.colDImage.Text = "Image";
        this.colDImage.Width = 150;
        // 
        // colDDateTime
        // 
        this.colDDateTime.Text = "DateTime";
        this.colDDateTime.Width = 100;
        // 
        // colDGPS
        // 
        this.colDGPS.Text = "GPS coord.";
        this.colDGPS.Width = 120;
        // 
        // colDTitle
        // 
        this.colDTitle.Text = "Title";
        this.colDTitle.Width = 100;
        // 
        // colDDescription
        // 
        this.colDDescription.Text = "Description";
        this.colDDescription.Width = 100;
        // 
        // splitContainer2
        // 
        this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.splitContainer2.Location = new System.Drawing.Point(3, 0);
        this.splitContainer2.Name = "splitContainer2";
        this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
        // 
        // splitContainer2.Panel1
        // 
        this.splitContainer2.Panel1.Controls.Add(this.btnViewAct);
        this.splitContainer2.Panel1.Controls.Add(this.treeViewActivities);
        this.splitContainer2.Panel1.Controls.Add(this.btnExpandAll);
        this.splitContainer2.Panel1.Controls.Add(this.btnCollapseAll);
        // 
        // splitContainer2.Panel2
        // 
        this.splitContainer2.Panel2.Controls.Add(this.listViewAct);
        this.splitContainer2.Size = new System.Drawing.Size(345, 451);
        this.splitContainer2.SplitterDistance = 224;
        this.splitContainer2.TabIndex = 13;
        // 
        // btnViewAct
        // 
        this.btnViewAct.BackColor = System.Drawing.Color.Transparent;
        this.btnViewAct.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
        this.btnViewAct.CenterImage = null;
        this.btnViewAct.DialogResult = System.Windows.Forms.DialogResult.None;
        this.btnViewAct.HyperlinkStyle = false;
        this.btnViewAct.ImageMargin = 2;
        this.btnViewAct.LeftImage = null;
        this.btnViewAct.Location = new System.Drawing.Point(3, 3);
        this.btnViewAct.Name = "btnViewAct";
        this.btnViewAct.PushStyle = true;
        this.btnViewAct.RightImage = null;
        this.btnViewAct.Size = new System.Drawing.Size(58, 23);
        this.btnViewAct.TabIndex = 14;
        this.btnViewAct.Text = "View";
        this.btnViewAct.TextAlign = System.Drawing.StringAlignment.Center;
        this.btnViewAct.TextLeftMargin = 2;
        this.btnViewAct.TextRightMargin = 2;
        this.btnViewAct.Click += new System.EventHandler(this.btnViewAct_Click);
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
        this.btnExpandAll.Location = new System.Drawing.Point(67, 3);
        this.btnExpandAll.Name = "btnExpandAll";
        this.btnExpandAll.PushStyle = true;
        this.btnExpandAll.RightImage = null;
        this.btnExpandAll.Size = new System.Drawing.Size(100, 23);
        this.btnExpandAll.TabIndex = 11;
        this.btnExpandAll.Text = "Expand All";
        this.btnExpandAll.TextAlign = System.Drawing.StringAlignment.Center;
        this.btnExpandAll.TextLeftMargin = 2;
        this.btnExpandAll.TextRightMargin = 2;
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
        this.btnCollapseAll.Location = new System.Drawing.Point(173, 3);
        this.btnCollapseAll.Name = "btnCollapseAll";
        this.btnCollapseAll.PushStyle = true;
        this.btnCollapseAll.RightImage = null;
        this.btnCollapseAll.Size = new System.Drawing.Size(100, 23);
        this.btnCollapseAll.TabIndex = 12;
        this.btnCollapseAll.Text = "Collapse All";
        this.btnCollapseAll.TextAlign = System.Drawing.StringAlignment.Center;
        this.btnCollapseAll.TextLeftMargin = 2;
        this.btnCollapseAll.TextRightMargin = 2;
        this.btnCollapseAll.Click += new System.EventHandler(this.btnCollapsAll_Click);
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
        this.listViewAct.Dock = System.Windows.Forms.DockStyle.Fill;
        this.listViewAct.Location = new System.Drawing.Point(0, 0);
        this.listViewAct.Name = "listViewAct";
        this.listViewAct.Size = new System.Drawing.Size(345, 223);
        this.listViewAct.TabIndex = 11;
        this.listViewAct.UseCompatibleStateImageBehavior = false;
        this.listViewAct.DoubleClick += new System.EventHandler(this.listViewAct_DoubleClick);
        this.listViewAct.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewAct_DragDrop);
        this.listViewAct.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewAct_DragEnter);
        this.listViewAct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewAct_KeyDown);
        // 
        // colImage
        // 
        this.colImage.Text = "Image";
        this.colImage.Width = 150;
        // 
        // colDateTime
        // 
        this.colDateTime.Text = "DateTime";
        this.colDateTime.Width = 100;
        // 
        // colGPS
        // 
        this.colGPS.Text = "GPS coord.";
        this.colGPS.Width = 120;
        // 
        // colTitle
        // 
        this.colTitle.Text = "Title";
        this.colTitle.Width = 100;
        // 
        // colDescription
        // 
        this.colDescription.Text = "Description";
        this.colDescription.Width = 100;
        // 
        // lblProgress
        // 
        this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.lblProgress.AutoSize = true;
        this.lblProgress.Location = new System.Drawing.Point(3, 484);
        this.lblProgress.Name = "lblProgress";
        this.lblProgress.Size = new System.Drawing.Size(48, 13);
        this.lblProgress.TabIndex = 10;
        this.lblProgress.Text = "Progress";
        // 
        // progressBar2
        // 
        this.progressBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.progressBar2.Location = new System.Drawing.Point(6, 458);
        this.progressBar2.Name = "progressBar2";
        this.progressBar2.Size = new System.Drawing.Size(515, 23);
        this.progressBar2.TabIndex = 11;
        // 
        // ImportControl
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.progressBar2);
        this.Controls.Add(this.lblProgress);
        this.Controls.Add(this.splitContainer1);
        this.DoubleBuffered = true;
        this.Name = "ImportControl";
        this.Size = new System.Drawing.Size(529, 508);
        this.Load += new System.EventHandler(this.ImportControl_Load);
        this.splitContainer1.Panel1.ResumeLayout(false);
        this.splitContainer1.Panel2.ResumeLayout(false);
        this.splitContainer1.ResumeLayout(false);
        this.splitContainer3.Panel1.ResumeLayout(false);
        this.splitContainer3.Panel2.ResumeLayout(false);
        this.splitContainer3.ResumeLayout(false);
        this.splitContainer2.Panel1.ResumeLayout(false);
        this.splitContainer2.Panel2.ResumeLayout(false);
        this.splitContainer2.ResumeLayout(false);
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
        private System.Windows.Forms.ListView listViewAct;
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
        private System.Windows.Forms.ListView listViewDrive;
        private ZoneFiveSoftware.Common.Visuals.Button btnViewFolder;
        private ZoneFiveSoftware.Common.Visuals.Button btnViewAct;
    }
}
