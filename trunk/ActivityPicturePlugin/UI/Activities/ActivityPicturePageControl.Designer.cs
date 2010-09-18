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

namespace ActivityPicturePlugin.UI.Activities
    {
    partial class ActivityPicturePageControl
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
                this.components = new System.ComponentModel.Container();
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActivityPicturePageControl));
                this.actionBannerViews = new ZoneFiveSoftware.Common.Visuals.ActionBanner();
                this.panelViews = new ZoneFiveSoftware.Common.Visuals.Panel();
                this.dataGridViewImages = new System.Windows.Forms.DataGridView();
                this.TypeImage = new System.Windows.Forms.DataGridViewImageColumn();
                this.ExifGPS = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.Altitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.commentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.thumbnailDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
                this.dateTimeOriginalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.equipmentModelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.photoSourceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.referenceIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.waypointDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.bindingSourceImageList = new System.Windows.Forms.BindingSource(this.components);
                this.pictureAlbumView = new ActivityPicturePlugin.Helper.PictureAlbum();
                this.importControl1 = new ActivityPicturePlugin.UI.ImportControl();
                this.groupBoxListOptions = new System.Windows.Forms.GroupBox();
                this.btnTimeOffset = new ZoneFiveSoftware.Common.Visuals.Button();
                this.btnKML = new ZoneFiveSoftware.Common.Visuals.Button();
                this.btnGeoTag = new ZoneFiveSoftware.Common.Visuals.Button();
                this.groupBoxVideo = new System.Windows.Forms.GroupBox();
                this.toolStripVideo = new System.Windows.Forms.ToolStrip();
                this.toolStripButtonPlay = new System.Windows.Forms.ToolStripButton();
                this.toolStripButtonPause = new System.Windows.Forms.ToolStripButton();
                this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
                this.volumeSlider2 = new ActivityPicturePlugin.Helper.VolumeSlider();
                this.trackBarVideo = new System.Windows.Forms.TrackBar();
                this.groupBoxImage = new System.Windows.Forms.GroupBox();
                this.labelImageSize = new System.Windows.Forms.Label();
                this.trackBarImageSize = new System.Windows.Forms.TrackBar();
                this.contextMenuStripView = new System.Windows.Forms.ContextMenuStrip(this.components);
                this.pictureAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.pictureListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.folderBrowserDialogImport = new System.Windows.Forms.FolderBrowserDialog();
                this.openFileDialogImport = new System.Windows.Forms.OpenFileDialog();
                this.timerVideo = new System.Windows.Forms.Timer(this.components);
                this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
                this.panelViews.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImages)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImageList)).BeginInit();
                this.groupBoxListOptions.SuspendLayout();
                this.groupBoxVideo.SuspendLayout();
                this.toolStripVideo.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.trackBarVideo)).BeginInit();
                this.groupBoxImage.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.trackBarImageSize)).BeginInit();
                this.contextMenuStripView.SuspendLayout();
                this.SuspendLayout();
                // 
                // actionBannerViews
                // 
                this.actionBannerViews.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.actionBannerViews.BackColor = System.Drawing.Color.Black;
                this.actionBannerViews.HasMenuButton = true;
                this.actionBannerViews.Location = new System.Drawing.Point(0, 3);
                this.actionBannerViews.Name = "actionBannerViews";
                this.actionBannerViews.Size = new System.Drawing.Size(451, 22);
                this.actionBannerViews.Style = ZoneFiveSoftware.Common.Visuals.ActionBanner.BannerStyle.Header1;
                this.actionBannerViews.TabIndex = 0;
                this.actionBannerViews.Text = "List";
                this.actionBannerViews.UseStyleFont = true;
                this.actionBannerViews.MenuClicked += new System.EventHandler(this.actionBanner1_MenuClicked);
                // 
                // panelViews
                // 
                this.panelViews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.panelViews.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.panelViews.BackColor = System.Drawing.Color.Transparent;
                this.panelViews.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.SquareShadow;
                this.panelViews.BorderColor = System.Drawing.Color.Transparent;
                this.panelViews.BorderShadowColor = System.Drawing.Color.Transparent;
                this.panelViews.Controls.Add(this.dataGridViewImages);
                this.panelViews.Controls.Add(this.pictureAlbumView);
                this.panelViews.Controls.Add(this.importControl1);
                this.panelViews.Controls.Add(this.groupBoxListOptions);
                this.panelViews.Controls.Add(this.groupBoxVideo);
                this.panelViews.Controls.Add(this.groupBoxImage);
                this.panelViews.HeadingBackColor = System.Drawing.Color.Transparent;
                this.panelViews.HeadingFont = null;
                this.panelViews.HeadingLeftMargin = 0;
                this.panelViews.HeadingText = "";
                this.panelViews.HeadingTextColor = System.Drawing.Color.Black;
                this.panelViews.HeadingTopMargin = 3;
                this.panelViews.Location = new System.Drawing.Point(0, 25);
                this.panelViews.Name = "panelViews";
                this.panelViews.Size = new System.Drawing.Size(448, 540);
                this.panelViews.TabIndex = 1;
                this.panelViews.Resize += new System.EventHandler(this.panel1_Resize);
                // 
                // dataGridViewImages
                // 
                this.dataGridViewImages.AllowUserToAddRows = false;
                this.dataGridViewImages.AllowUserToResizeRows = false;
                this.dataGridViewImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.dataGridViewImages.AutoGenerateColumns = false;
                this.dataGridViewImages.BorderStyle = System.Windows.Forms.BorderStyle.None;
                this.dataGridViewImages.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
                this.dataGridViewImages.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                this.dataGridViewImages.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                this.dataGridViewImages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                this.dataGridViewImages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TypeImage,
            this.ExifGPS,
            this.Altitude,
            this.commentDataGridViewTextBoxColumn,
            this.thumbnailDataGridViewImageColumn,
            this.dateTimeOriginalDataGridViewTextBoxColumn,
            this.titleDataGridViewTextBoxColumn,
            this.equipmentModelDataGridViewTextBoxColumn,
            this.photoSourceDataGridViewTextBoxColumn,
            this.referenceIDDataGridViewTextBoxColumn,
            this.waypointDataGridViewTextBoxColumn});
                this.dataGridViewImages.DataSource = this.bindingSourceImageList;
                this.dataGridViewImages.EnableHeadersVisualStyles = false;
                this.dataGridViewImages.Location = new System.Drawing.Point(5, 60);
                this.dataGridViewImages.Name = "dataGridViewImages";
                this.dataGridViewImages.RowHeadersVisible = false;
                this.dataGridViewImages.RowTemplate.Height = 50;
                this.dataGridViewImages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                this.dataGridViewImages.Size = new System.Drawing.Size(438, 480);
                this.dataGridViewImages.TabIndex = 1;
                this.dataGridViewImages.Visible = false;
                this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellValueChanged);
                this.dataGridViewImages.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellDoubleClick);
                this.dataGridViewImages.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewImages_ColumnHeaderMouseClick);
                this.dataGridViewImages.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewImages_DataError);
                this.dataGridViewImages.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellEnter);
                this.dataGridViewImages.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewImages_RowsRemoved);
                this.dataGridViewImages.SelectionChanged += new System.EventHandler(this.dataGridViewImages_SelectionChanged);
                // 
                // TypeImage
                // 
                this.TypeImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                this.TypeImage.DataPropertyName = "TypeImage";
                this.TypeImage.HeaderText = "TypeImage";
                this.TypeImage.Name = "TypeImage";
                this.TypeImage.ReadOnly = true;
                this.TypeImage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                this.TypeImage.Width = 40;
                // 
                // ExifGPS
                // 
                this.ExifGPS.DataPropertyName = "ExifGPS";
                dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                this.ExifGPS.DefaultCellStyle = dataGridViewCellStyle2;
                this.ExifGPS.HeaderText = "ExifGPS";
                this.ExifGPS.Name = "ExifGPS";
                this.ExifGPS.ReadOnly = true;
                this.ExifGPS.Width = 80;
                // 
                // Altitude
                // 
                this.Altitude.DataPropertyName = "Altitude";
                this.Altitude.HeaderText = "Altitude";
                this.Altitude.Name = "Altitude";
                this.Altitude.ReadOnly = true;
                this.Altitude.Width = 60;
                // 
                // commentDataGridViewTextBoxColumn
                // 
                this.commentDataGridViewTextBoxColumn.DataPropertyName = "Comments";
                this.commentDataGridViewTextBoxColumn.HeaderText = "Comment";
                this.commentDataGridViewTextBoxColumn.Name = "commentDataGridViewTextBoxColumn";
                this.commentDataGridViewTextBoxColumn.Width = 80;
                // 
                // thumbnailDataGridViewImageColumn
                // 
                this.thumbnailDataGridViewImageColumn.DataPropertyName = "Thumbnail";
                this.thumbnailDataGridViewImageColumn.HeaderText = "Thumbnail";
                this.thumbnailDataGridViewImageColumn.Name = "thumbnailDataGridViewImageColumn";
                this.thumbnailDataGridViewImageColumn.ReadOnly = true;
                this.thumbnailDataGridViewImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                this.thumbnailDataGridViewImageColumn.Width = 67;
                // 
                // dateTimeOriginalDataGridViewTextBoxColumn
                // 
                this.dateTimeOriginalDataGridViewTextBoxColumn.DataPropertyName = "DateTimeOriginal";
                dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                this.dateTimeOriginalDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
                this.dateTimeOriginalDataGridViewTextBoxColumn.HeaderText = "Date & Time";
                this.dateTimeOriginalDataGridViewTextBoxColumn.Name = "dateTimeOriginalDataGridViewTextBoxColumn";
                this.dateTimeOriginalDataGridViewTextBoxColumn.ReadOnly = true;
                this.dateTimeOriginalDataGridViewTextBoxColumn.Width = 120;
                // 
                // titleDataGridViewTextBoxColumn
                // 
                this.titleDataGridViewTextBoxColumn.DataPropertyName = "Title";
                this.titleDataGridViewTextBoxColumn.HeaderText = "Photo Title";
                this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
                this.titleDataGridViewTextBoxColumn.Width = 80;
                // 
                // equipmentModelDataGridViewTextBoxColumn
                // 
                this.equipmentModelDataGridViewTextBoxColumn.DataPropertyName = "EquipmentModel";
                this.equipmentModelDataGridViewTextBoxColumn.HeaderText = "Camera Model";
                this.equipmentModelDataGridViewTextBoxColumn.Name = "equipmentModelDataGridViewTextBoxColumn";
                this.equipmentModelDataGridViewTextBoxColumn.ReadOnly = true;
                this.equipmentModelDataGridViewTextBoxColumn.Width = 80;
                // 
                // photoSourceDataGridViewTextBoxColumn
                // 
                this.photoSourceDataGridViewTextBoxColumn.DataPropertyName = "PhotoSource";
                dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                this.photoSourceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
                this.photoSourceDataGridViewTextBoxColumn.HeaderText = "PhotoSource";
                this.photoSourceDataGridViewTextBoxColumn.Name = "photoSourceDataGridViewTextBoxColumn";
                this.photoSourceDataGridViewTextBoxColumn.ReadOnly = true;
                this.photoSourceDataGridViewTextBoxColumn.Width = 80;
                // 
                // referenceIDDataGridViewTextBoxColumn
                // 
                this.referenceIDDataGridViewTextBoxColumn.DataPropertyName = "ReferenceID";
                this.referenceIDDataGridViewTextBoxColumn.HeaderText = "ReferenceID";
                this.referenceIDDataGridViewTextBoxColumn.Name = "referenceIDDataGridViewTextBoxColumn";
                this.referenceIDDataGridViewTextBoxColumn.ReadOnly = true;
                this.referenceIDDataGridViewTextBoxColumn.Width = 80;
                // 
                // waypointDataGridViewTextBoxColumn
                // 
                this.waypointDataGridViewTextBoxColumn.DataPropertyName = "Waypoint";
                this.waypointDataGridViewTextBoxColumn.HeaderText = "Waypoint";
                this.waypointDataGridViewTextBoxColumn.Name = "waypointDataGridViewTextBoxColumn";
                this.waypointDataGridViewTextBoxColumn.ReadOnly = true;
                this.waypointDataGridViewTextBoxColumn.Visible = false;
                // 
                // bindingSourceImageList
                // 
                this.bindingSourceImageList.DataSource = typeof(ActivityPicturePlugin.Helper.ImageData);
                // 
                // pictureAlbumView
                // 
                this.pictureAlbumView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.pictureAlbumView.AutoScroll = true;
                this.pictureAlbumView.ImageList = null;
                this.pictureAlbumView.Location = new System.Drawing.Point(0, 60);
                this.pictureAlbumView.Name = "pictureAlbumView";
                this.pictureAlbumView.NoThumbNails = false;
                this.pictureAlbumView.Size = new System.Drawing.Size(445, 480);
                this.pictureAlbumView.TabIndex = 5;
                this.pictureAlbumView.Visible = false;
                this.pictureAlbumView.Zoom = 0;
                this.pictureAlbumView.Load += new System.EventHandler(this.pictureAlbumView_Load);
                this.pictureAlbumView.ShowVideoOptions += new ActivityPicturePlugin.Helper.PictureAlbum.ShowVideoOptionsEventHandler(this.pictureAlbumView_ShowVideoOptions);
                this.pictureAlbumView.ZoomChange += new ActivityPicturePlugin.Helper.PictureAlbum.ZoomChangeEventHandler(this.pictureAlbumView_ZoomChange);
                this.pictureAlbumView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureAlbum1_MouseClick);
                this.pictureAlbumView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureAlbum1_MouseDown);
                this.pictureAlbumView.UpdateVideoToolBar += new ActivityPicturePlugin.Helper.PictureAlbum.UpdateVideoToolBarEventHandler(this.pictureAlbumView_UpdateVideoToolBar);
                this.pictureAlbumView.ActivityChanged += new ActivityPicturePlugin.Helper.PictureAlbum.ActivityChangedEventHandler(this.pictureAlbumView_ActivityChanged);
                // 
                // importControl1
                // 
                this.importControl1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.importControl1.Location = new System.Drawing.Point(0, 0);
                this.importControl1.Name = "importControl1";
                this.importControl1.ShowAllActivities = false;
                this.importControl1.Size = new System.Drawing.Size(448, 540);
                this.importControl1.TabIndex = 6;
                this.importControl1.Visible = false;
                this.importControl1.Load += new System.EventHandler(this.importControl1_Load_1);
                // 
                // groupBoxListOptions
                // 
                this.groupBoxListOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.groupBoxListOptions.Controls.Add(this.btnTimeOffset);
                this.groupBoxListOptions.Controls.Add(this.btnKML);
                this.groupBoxListOptions.Controls.Add(this.btnGeoTag);
                this.groupBoxListOptions.Location = new System.Drawing.Point(3, 6);
                this.groupBoxListOptions.Name = "groupBoxListOptions";
                this.groupBoxListOptions.Size = new System.Drawing.Size(436, 48);
                this.groupBoxListOptions.TabIndex = 6;
                this.groupBoxListOptions.TabStop = false;
                this.groupBoxListOptions.Text = "Image Options";
                // 
                // btnTimeOffset
                // 
                this.btnTimeOffset.BackColor = System.Drawing.Color.Transparent;
                this.btnTimeOffset.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
                this.btnTimeOffset.CenterImage = null;
                this.btnTimeOffset.DialogResult = System.Windows.Forms.DialogResult.None;
                this.btnTimeOffset.HyperlinkStyle = false;
                this.btnTimeOffset.ImageMargin = 2;
                this.btnTimeOffset.LeftImage = null;
                this.btnTimeOffset.Location = new System.Drawing.Point(79, 19);
                this.btnTimeOffset.Name = "btnTimeOffset";
                this.btnTimeOffset.PushStyle = true;
                this.btnTimeOffset.RightImage = null;
                this.btnTimeOffset.Size = new System.Drawing.Size(125, 23);
                this.btnTimeOffset.TabIndex = 9;
                this.btnTimeOffset.Text = "Time Offset";
                this.btnTimeOffset.TextAlign = System.Drawing.StringAlignment.Center;
                this.btnTimeOffset.TextLeftMargin = 2;
                this.btnTimeOffset.TextRightMargin = 2;
                this.btnTimeOffset.Click += new System.EventHandler(this.btnTimeOffset_Click);
                // 
                // btnKML
                // 
                this.btnKML.BackColor = System.Drawing.Color.Transparent;
                this.btnKML.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
                this.btnKML.CenterImage = null;
                this.btnKML.DialogResult = System.Windows.Forms.DialogResult.None;
                this.btnKML.HyperlinkStyle = false;
                this.btnKML.ImageMargin = 2;
                this.btnKML.LeftImage = null;
                this.btnKML.Location = new System.Drawing.Point(204, 19);
                this.btnKML.Name = "btnKML";
                this.btnKML.PushStyle = true;
                this.btnKML.RightImage = null;
                this.btnKML.Size = new System.Drawing.Size(182, 23);
                this.btnKML.TabIndex = 8;
                this.btnKML.Text = "Export to Google Earth";
                this.btnKML.TextAlign = System.Drawing.StringAlignment.Center;
                this.btnKML.TextLeftMargin = 2;
                this.btnKML.TextRightMargin = 2;
                this.btnKML.Click += new System.EventHandler(this.btnKML_Click);
                // 
                // btnGeoTag
                // 
                this.btnGeoTag.BackColor = System.Drawing.Color.Transparent;
                this.btnGeoTag.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
                this.btnGeoTag.CenterImage = null;
                this.btnGeoTag.DialogResult = System.Windows.Forms.DialogResult.None;
                this.btnGeoTag.HyperlinkStyle = false;
                this.btnGeoTag.ImageMargin = 2;
                this.btnGeoTag.LeftImage = null;
                this.btnGeoTag.Location = new System.Drawing.Point(6, 19);
                this.btnGeoTag.Name = "btnGeoTag";
                this.btnGeoTag.PushStyle = true;
                this.btnGeoTag.RightImage = null;
                this.btnGeoTag.Size = new System.Drawing.Size(73, 23);
                this.btnGeoTag.TabIndex = 7;
                this.btnGeoTag.Text = "GeoTag";
                this.btnGeoTag.TextAlign = System.Drawing.StringAlignment.Center;
                this.btnGeoTag.TextLeftMargin = 2;
                this.btnGeoTag.TextRightMargin = 2;
                this.btnGeoTag.Click += new System.EventHandler(this.btnGeoTag_Click);
                // 
                // groupBoxVideo
                // 
                this.groupBoxVideo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.groupBoxVideo.Controls.Add(this.toolStripVideo);
                this.groupBoxVideo.Controls.Add(this.volumeSlider2);
                this.groupBoxVideo.Controls.Add(this.trackBarVideo);
                this.groupBoxVideo.Location = new System.Drawing.Point(219, 6);
                this.groupBoxVideo.Name = "groupBoxVideo";
                this.groupBoxVideo.Size = new System.Drawing.Size(221, 48);
                this.groupBoxVideo.TabIndex = 8;
                this.groupBoxVideo.TabStop = false;
                this.groupBoxVideo.Text = "Video Options";
                // 
                // toolStripVideo
                // 
                this.toolStripVideo.Dock = System.Windows.Forms.DockStyle.None;
                this.toolStripVideo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPlay,
            this.toolStripButtonPause,
            this.toolStripButtonStop});
                this.toolStripVideo.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
                this.toolStripVideo.Location = new System.Drawing.Point(3, 19);
                this.toolStripVideo.Name = "toolStripVideo";
                this.toolStripVideo.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
                this.toolStripVideo.Size = new System.Drawing.Size(70, 23);
                this.toolStripVideo.TabIndex = 0;
                this.toolStripVideo.Text = "toolStrip2";
                // 
                // toolStripButtonPlay
                // 
                this.toolStripButtonPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonPlay.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPlay.Image")));
                this.toolStripButtonPlay.ImageTransparentColor = System.Drawing.Color.Red;
                this.toolStripButtonPlay.Name = "toolStripButtonPlay";
                this.toolStripButtonPlay.Size = new System.Drawing.Size(23, 20);
                this.toolStripButtonPlay.Text = "toolStripButtonPlay";
                this.toolStripButtonPlay.Click += new System.EventHandler(this.toolStripButtonPlay_Click);
                // 
                // toolStripButtonPause
                // 
                this.toolStripButtonPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonPause.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPause.Image")));
                this.toolStripButtonPause.ImageTransparentColor = System.Drawing.Color.Red;
                this.toolStripButtonPause.Name = "toolStripButtonPause";
                this.toolStripButtonPause.Size = new System.Drawing.Size(23, 20);
                this.toolStripButtonPause.Text = "toolStripButtonPause";
                this.toolStripButtonPause.Click += new System.EventHandler(this.toolStripButtonPause_Click);
                // 
                // toolStripButtonStop
                // 
                this.toolStripButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStop.Image")));
                this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Red;
                this.toolStripButtonStop.Name = "toolStripButtonStop";
                this.toolStripButtonStop.Size = new System.Drawing.Size(23, 20);
                this.toolStripButtonStop.Text = "toolStripButtonStop";
                this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
                // 
                // volumeSlider2
                // 
                this.volumeSlider2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                this.volumeSlider2.Location = new System.Drawing.Point(145, 16);
                this.volumeSlider2.MaximumSize = new System.Drawing.Size(100, 23);
                this.volumeSlider2.Name = "volumeSlider2";
                this.volumeSlider2.Size = new System.Drawing.Size(70, 23);
                this.volumeSlider2.TabIndex = 6;
                this.volumeSlider2.Volume = ((uint)(100u));
                // 
                // trackBarVideo
                // 
                this.trackBarVideo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.trackBarVideo.AutoSize = false;
                this.trackBarVideo.Location = new System.Drawing.Point(76, 16);
                this.trackBarVideo.Maximum = 1000;
                this.trackBarVideo.Name = "trackBarVideo";
                this.trackBarVideo.Size = new System.Drawing.Size(63, 23);
                this.trackBarVideo.TabIndex = 1;
                this.trackBarVideo.TickStyle = System.Windows.Forms.TickStyle.None;
                this.trackBarVideo.Scroll += new System.EventHandler(this.trackBarVideo_Scroll);
                // 
                // groupBoxImage
                // 
                this.groupBoxImage.Controls.Add(this.labelImageSize);
                this.groupBoxImage.Controls.Add(this.trackBarImageSize);
                this.groupBoxImage.Location = new System.Drawing.Point(4, 6);
                this.groupBoxImage.Name = "groupBoxImage";
                this.groupBoxImage.Size = new System.Drawing.Size(209, 48);
                this.groupBoxImage.TabIndex = 7;
                this.groupBoxImage.TabStop = false;
                this.groupBoxImage.Text = "Image Options";
                // 
                // labelImageSize
                // 
                this.labelImageSize.AutoSize = true;
                this.labelImageSize.ForeColor = System.Drawing.SystemColors.ControlText;
                this.labelImageSize.Location = new System.Drawing.Point(6, 20);
                this.labelImageSize.Name = "labelImageSize";
                this.labelImageSize.Size = new System.Drawing.Size(62, 13);
                this.labelImageSize.TabIndex = 2;
                this.labelImageSize.Text = "Image Size:";
                // 
                // trackBarImageSize
                // 
                this.trackBarImageSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.trackBarImageSize.AutoSize = false;
                this.trackBarImageSize.Location = new System.Drawing.Point(102, 16);
                this.trackBarImageSize.Maximum = 100;
                this.trackBarImageSize.Name = "trackBarImageSize";
                this.trackBarImageSize.Size = new System.Drawing.Size(101, 26);
                this.trackBarImageSize.TabIndex = 6;
                this.trackBarImageSize.TickFrequency = 10;
                this.trackBarImageSize.TickStyle = System.Windows.Forms.TickStyle.None;
                this.trackBarImageSize.Value = 20;
                this.trackBarImageSize.Scroll += new System.EventHandler(this.trackBarImageSize_ValueChanged);
                // 
                // contextMenuStripView
                // 
                this.contextMenuStripView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pictureAlbumToolStripMenuItem,
            this.pictureListToolStripMenuItem,
            this.importToolStripMenuItem});
                this.contextMenuStripView.Name = "contextMenuStrip1";
                this.contextMenuStripView.ShowImageMargin = false;
                this.contextMenuStripView.ShowItemToolTips = false;
                this.contextMenuStripView.Size = new System.Drawing.Size(93, 70);
                // 
                // pictureAlbumToolStripMenuItem
                // 
                this.pictureAlbumToolStripMenuItem.Name = "pictureAlbumToolStripMenuItem";
                this.pictureAlbumToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
                this.pictureAlbumToolStripMenuItem.Text = "Album";
                this.pictureAlbumToolStripMenuItem.Click += new System.EventHandler(this.pictureAlbumToolStripMenuItem_Click);
                // 
                // pictureListToolStripMenuItem
                // 
                this.pictureListToolStripMenuItem.Name = "pictureListToolStripMenuItem";
                this.pictureListToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
                this.pictureListToolStripMenuItem.Text = "List";
                this.pictureListToolStripMenuItem.Click += new System.EventHandler(this.pictureListToolStripMenuItem_Click);
                // 
                // importToolStripMenuItem
                // 
                this.importToolStripMenuItem.Name = "importToolStripMenuItem";
                this.importToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
                this.importToolStripMenuItem.Text = "Import";
                this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
                // 
                // openFileDialogImport
                // 
                this.openFileDialogImport.FileName = "openFileDialog1";
                // 
                // timerVideo
                // 
                this.timerVideo.Enabled = true;
                this.timerVideo.Tick += new System.EventHandler(this.timerVideo_Tick);
                // 
                // ActivityPicturePageControl
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.AutoScroll = true;
                this.Controls.Add(this.panelViews);
                this.Controls.Add(this.actionBannerViews);
                this.Name = "ActivityPicturePageControl";
                this.Size = new System.Drawing.Size(451, 568);
                this.Load += new System.EventHandler(this.ActivityPicturePageControl_Load);
                this.panelViews.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImages)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImageList)).EndInit();
                this.groupBoxListOptions.ResumeLayout(false);
                this.groupBoxVideo.ResumeLayout(false);
                this.groupBoxVideo.PerformLayout();
                this.toolStripVideo.ResumeLayout(false);
                this.toolStripVideo.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.trackBarVideo)).EndInit();
                this.groupBoxImage.ResumeLayout(false);
                this.groupBoxImage.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.trackBarImageSize)).EndInit();
                this.contextMenuStripView.ResumeLayout(false);
                this.ResumeLayout(false);

            }

        #endregion

        private ZoneFiveSoftware.Common.Visuals.ActionBanner actionBannerViews;
        private ZoneFiveSoftware.Common.Visuals.Panel panelViews;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripView;
        private System.Windows.Forms.ToolStripMenuItem pictureAlbumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pictureListToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSourceImageList;
        private System.Windows.Forms.DataGridView dataGridViewImages;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogImport;
        private ActivityPicturePlugin.Helper.PictureAlbum pictureAlbumView;
        private System.Windows.Forms.OpenFileDialog openFileDialogImport;
        private System.Windows.Forms.GroupBox groupBoxListOptions;
        private System.Windows.Forms.Timer timerVideo;
        private System.Windows.Forms.DataGridViewImageColumn TypeImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExifGPS;
        private System.Windows.Forms.DataGridViewTextBoxColumn Altitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn thumbnailDataGridViewImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateTimeOriginalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn equipmentModelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photoSourceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn referenceIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waypointDataGridViewTextBoxColumn;
        private ImportControl importControl1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private ZoneFiveSoftware.Common.Visuals.Button btnGeoTag;
        private ZoneFiveSoftware.Common.Visuals.Button btnKML;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private ZoneFiveSoftware.Common.Visuals.Button btnTimeOffset;
        private System.Windows.Forms.GroupBox groupBoxImage;
        private System.Windows.Forms.Label labelImageSize;
        private System.Windows.Forms.TrackBar trackBarImageSize;
        private System.Windows.Forms.GroupBox groupBoxVideo;
        private ActivityPicturePlugin.Helper.VolumeSlider volumeSlider2;
        private System.Windows.Forms.TrackBar trackBarVideo;
        private System.Windows.Forms.ToolStrip toolStripVideo;
        private System.Windows.Forms.ToolStripButton toolStripButtonPlay;
        private System.Windows.Forms.ToolStripButton toolStripButtonPause;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.ToolTip toolTip1;
        }
    }
