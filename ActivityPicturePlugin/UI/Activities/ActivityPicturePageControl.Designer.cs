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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActivityPicturePageControl));
            this.actionBannerViews = new ZoneFiveSoftware.Common.Visuals.ActionBanner();
            this.panelViews = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.dataGridViewImages = new System.Windows.Forms.DataGridView();
            this.cThumbnail = new System.Windows.Forms.DataGridViewImageColumn();
            this.cTypeImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.cExifGPS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAltitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDateTimeOriginal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPhotoTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCamera = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPhotoSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cReferenceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuListImages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuNone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuTypeImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuExifGPS = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuAltitude = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuDateTime = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuComment = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuCamera = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuPhotoSource = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuReferenceID = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSourceImageList = new System.Windows.Forms.BindingSource(this.components);
            this.panelPictureAlbumView = new ActivityPicturePlugin.UI.Activities.PanelEx();
            this.pictureAlbumView = new ActivityPicturePlugin.Helper.PictureAlbum();
            this.importControl1 = new ActivityPicturePlugin.UI.ImportControl();
            this.groupBoxListOptions = new System.Windows.Forms.GroupBox();
            this.btnGEList = new ZoneFiveSoftware.Common.Visuals.Button();
            this.toolstripListOptions = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonGE = new System.Windows.Forms.ToolStripButton();
            this.btnTimeOffset = new ZoneFiveSoftware.Common.Visuals.Button();
            this.btnKML = new ZoneFiveSoftware.Common.Visuals.Button();
            this.btnGeoTag = new ZoneFiveSoftware.Common.Visuals.Button();
            this.groupBoxVideo = new System.Windows.Forms.GroupBox();
            this.toolStripVideo = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSnapshot = new System.Windows.Forms.ToolStripButton();
            this.volumeSlider2 = new ActivityPicturePlugin.Helper.VolumeSlider();
            this.sliderVideo = new MB.Controls.ColorSlider();
            this.groupBoxImage = new System.Windows.Forms.GroupBox();
            this.labelImageSize = new System.Windows.Forms.Label();
            this.sliderImageSize = new MB.Controls.ColorSlider();
            this.contextMenuSliderImageSize = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuFitToWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.waypointDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pictureAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialogImport = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialogImport = new System.Windows.Forms.OpenFileDialog();
            this.timerVideo = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuListGE = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuPictureAlbum = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuResetSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.panelViews.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImages)).BeginInit();
            this.contextMenuListImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImageList)).BeginInit();
            this.panelPictureAlbumView.SuspendLayout();
            this.groupBoxListOptions.SuspendLayout();
            this.toolstripListOptions.SuspendLayout();
            this.groupBoxVideo.SuspendLayout();
            this.toolStripVideo.SuspendLayout();
            this.groupBoxImage.SuspendLayout();
            this.contextMenuSliderImageSize.SuspendLayout();
            this.contextMenuStripView.SuspendLayout();
            this.contextMenuPictureAlbum.SuspendLayout();
            this.SuspendLayout();
            // 
            // actionBannerViews
            // 
            this.actionBannerViews.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.actionBannerViews.BackColor = System.Drawing.Color.Black;
            this.actionBannerViews.HasMenuButton = true;
            this.actionBannerViews.Location = new System.Drawing.Point(0, 4);
            this.actionBannerViews.Margin = new System.Windows.Forms.Padding(4);
            this.actionBannerViews.Name = "actionBannerViews";
            this.actionBannerViews.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.actionBannerViews.Size = new System.Drawing.Size(600, 24);
            this.actionBannerViews.Style = ZoneFiveSoftware.Common.Visuals.ActionBanner.BannerStyle.Header2;
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
            this.panelViews.BorderColor = System.Drawing.Color.Gray;
            this.panelViews.Controls.Add(this.dataGridViewImages);
            this.panelViews.Controls.Add(this.panelPictureAlbumView);
            this.panelViews.Controls.Add(this.importControl1);
            this.panelViews.Controls.Add(this.groupBoxListOptions);
            this.panelViews.Controls.Add(this.groupBoxVideo);
            this.panelViews.Controls.Add(this.groupBoxImage);
            this.panelViews.Controls.Add(this.progressBar1);
            this.panelViews.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.panelViews.HeadingFont = null;
            this.panelViews.HeadingLeftMargin = 0;
            this.panelViews.HeadingText = null;
            this.panelViews.HeadingTextColor = System.Drawing.Color.Black;
            this.panelViews.HeadingTopMargin = 3;
            this.panelViews.Location = new System.Drawing.Point(0, 33);
            this.panelViews.Margin = new System.Windows.Forms.Padding(4);
            this.panelViews.Name = "panelViews";
            this.panelViews.Padding = new System.Windows.Forms.Padding(6);
            this.panelViews.Size = new System.Drawing.Size(596, 365);
            this.panelViews.TabIndex = 1;
            this.panelViews.Resize += new System.EventHandler(this.panelViews_Resize);
            // 
            // dataGridViewImages
            // 
            this.dataGridViewImages.AllowUserToAddRows = false;
            this.dataGridViewImages.AllowUserToDeleteRows = false;
            this.dataGridViewImages.AllowUserToResizeRows = false;
            this.dataGridViewImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewImages.AutoGenerateColumns = false;
            this.dataGridViewImages.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewImages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewImages.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedHorizontal;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImages.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewImages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewImages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cThumbnail,
            this.cTypeImage,
            this.cExifGPS,
            this.cAltitude,
            this.cDateTimeOriginal,
            this.cComment,
            this.cPhotoTitle,
            this.cCamera,
            this.cPhotoSource,
            this.cReferenceID});
            this.dataGridViewImages.ContextMenuStrip = this.contextMenuListImages;
            this.dataGridViewImages.DataSource = this.bindingSourceImageList;
            this.dataGridViewImages.EnableHeadersVisualStyles = false;
            this.dataGridViewImages.Location = new System.Drawing.Point(10, 80);
            this.dataGridViewImages.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewImages.Name = "dataGridViewImages";
            this.dataGridViewImages.RowHeadersVisible = false;
            this.dataGridViewImages.RowTemplate.Height = 50;
            this.dataGridViewImages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewImages.Size = new System.Drawing.Size(576, 273);
            this.dataGridViewImages.StandardTab = true;
            this.dataGridViewImages.TabIndex = 1;
            this.dataGridViewImages.Visible = false;
            this.dataGridViewImages.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellDoubleClick);
            this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellValueChanged);
            this.dataGridViewImages.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewImages_ColumnHeaderMouseClick);
            this.dataGridViewImages.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewImages_DataError);
            this.dataGridViewImages.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewImages_RowsRemoved);
            this.dataGridViewImages.SelectionChanged += new System.EventHandler(this.dataGridViewImages_SelectionChanged);
            this.dataGridViewImages.BindingContextChanged += new System.EventHandler(this.dataGridViewImages_BindingContextChanged);
            this.dataGridViewImages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewImages_KeyDown);
            // 
            // cThumbnail
            // 
            this.cThumbnail.DataPropertyName = "Thumbnail";
            this.cThumbnail.FillWeight = 1F;
            this.cThumbnail.HeaderText = "Thumbnail";
            this.cThumbnail.MinimumWidth = 85;
            this.cThumbnail.Name = "cThumbnail";
            this.cThumbnail.ReadOnly = true;
            this.cThumbnail.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cTypeImage
            // 
            this.cTypeImage.DataPropertyName = "TypeImage";
            this.cTypeImage.FillWeight = 1F;
            this.cTypeImage.HeaderText = "TypeImage";
            this.cTypeImage.MinimumWidth = 50;
            this.cTypeImage.Name = "cTypeImage";
            this.cTypeImage.ReadOnly = true;
            this.cTypeImage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cExifGPS
            // 
            this.cExifGPS.DataPropertyName = "ExifGPS";
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cExifGPS.DefaultCellStyle = dataGridViewCellStyle2;
            this.cExifGPS.FillWeight = 1F;
            this.cExifGPS.HeaderText = "ExifGPS";
            this.cExifGPS.MinimumWidth = 85;
            this.cExifGPS.Name = "cExifGPS";
            this.cExifGPS.ReadOnly = true;
            // 
            // cAltitude
            // 
            this.cAltitude.DataPropertyName = "Altitude";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cAltitude.DefaultCellStyle = dataGridViewCellStyle3;
            this.cAltitude.FillWeight = 1F;
            this.cAltitude.HeaderText = "Altitude";
            this.cAltitude.MinimumWidth = 95;
            this.cAltitude.Name = "cAltitude";
            this.cAltitude.ReadOnly = true;
            // 
            // cDateTimeOriginal
            // 
            this.cDateTimeOriginal.DataPropertyName = "DateTimeOriginal";
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cDateTimeOriginal.DefaultCellStyle = dataGridViewCellStyle4;
            this.cDateTimeOriginal.FillWeight = 1F;
            this.cDateTimeOriginal.HeaderText = "Date & Time";
            this.cDateTimeOriginal.MinimumWidth = 90;
            this.cDateTimeOriginal.Name = "cDateTimeOriginal";
            this.cDateTimeOriginal.ReadOnly = true;
            // 
            // cComment
            // 
            this.cComment.DataPropertyName = "Comments";
            this.cComment.HeaderText = "Comment";
            this.cComment.MinimumWidth = 95;
            this.cComment.Name = "cComment";
            // 
            // cPhotoTitle
            // 
            this.cPhotoTitle.DataPropertyName = "Title";
            this.cPhotoTitle.HeaderText = "Photo Title";
            this.cPhotoTitle.MinimumWidth = 80;
            this.cPhotoTitle.Name = "cPhotoTitle";
            // 
            // cCamera
            // 
            this.cCamera.DataPropertyName = "EquipmentModel";
            this.cCamera.HeaderText = "Camera Model";
            this.cCamera.MinimumWidth = 110;
            this.cCamera.Name = "cCamera";
            this.cCamera.ReadOnly = true;
            // 
            // cPhotoSource
            // 
            this.cPhotoSource.DataPropertyName = "PhotoSource";
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cPhotoSource.DefaultCellStyle = dataGridViewCellStyle5;
            this.cPhotoSource.HeaderText = "PhotoSource";
            this.cPhotoSource.MinimumWidth = 110;
            this.cPhotoSource.Name = "cPhotoSource";
            this.cPhotoSource.ReadOnly = true;
            // 
            // cReferenceID
            // 
            this.cReferenceID.DataPropertyName = "ReferenceID";
            this.cReferenceID.HeaderText = "ReferenceID";
            this.cReferenceID.MinimumWidth = 90;
            this.cReferenceID.Name = "cReferenceID";
            this.cReferenceID.ReadOnly = true;
            // 
            // contextMenuListImages
            // 
            this.contextMenuListImages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuCopy,
            this.toolStripSeparator2,
            this.toolStripMenuNone,
            this.toolStripMenuAll,
            this.toolStripSeparator1,
            this.toolStripMenuTypeImage,
            this.toolStripMenuExifGPS,
            this.toolStripMenuAltitude,
            this.toolStripMenuDateTime,
            this.toolStripMenuTitle,
            this.toolStripMenuComment,
            this.toolStripMenuCamera,
            this.toolStripMenuPhotoSource,
            this.toolStripMenuReferenceID});
            this.contextMenuListImages.Name = "contextMenuListImages";
            this.contextMenuListImages.Size = new System.Drawing.Size(189, 280);
            // 
            // toolStripMenuCopy
            // 
            this.toolStripMenuCopy.Name = "toolStripMenuCopy";
            this.toolStripMenuCopy.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuCopy.Text = "Copy";
            this.toolStripMenuCopy.Click += new System.EventHandler(this.toolStripMenuCopy_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
            // 
            // toolStripMenuNone
            // 
            this.toolStripMenuNone.Name = "toolStripMenuNone";
            this.toolStripMenuNone.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuNone.Text = "Hide All Columns";
            this.toolStripMenuNone.Click += new System.EventHandler(this.toolStripMenuNone_Click);
            // 
            // toolStripMenuAll
            // 
            this.toolStripMenuAll.Name = "toolStripMenuAll";
            this.toolStripMenuAll.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuAll.Text = "Show All Columns";
            this.toolStripMenuAll.Click += new System.EventHandler(this.toolStripMenuAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // toolStripMenuTypeImage
            // 
            this.toolStripMenuTypeImage.Checked = true;
            this.toolStripMenuTypeImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuTypeImage.Name = "toolStripMenuTypeImage";
            this.toolStripMenuTypeImage.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuTypeImage.Text = "Type Image";
            this.toolStripMenuTypeImage.CheckStateChanged += new System.EventHandler(this.toolStripMenuTypeImage_CheckStateChanged);
            this.toolStripMenuTypeImage.Click += new System.EventHandler(this.toolStripMenuTypeImage_Click);
            // 
            // toolStripMenuExifGPS
            // 
            this.toolStripMenuExifGPS.Checked = true;
            this.toolStripMenuExifGPS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuExifGPS.Name = "toolStripMenuExifGPS";
            this.toolStripMenuExifGPS.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuExifGPS.Text = "ExifGPS";
            this.toolStripMenuExifGPS.CheckStateChanged += new System.EventHandler(this.toolStripMenuExifGPS_CheckStateChanged);
            this.toolStripMenuExifGPS.Click += new System.EventHandler(this.toolStripMenuExifGPS_Click);
            // 
            // toolStripMenuAltitude
            // 
            this.toolStripMenuAltitude.Checked = true;
            this.toolStripMenuAltitude.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuAltitude.Name = "toolStripMenuAltitude";
            this.toolStripMenuAltitude.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuAltitude.Text = "Altitude";
            this.toolStripMenuAltitude.CheckStateChanged += new System.EventHandler(this.toolStripMenuAltitude_CheckStateChanged);
            this.toolStripMenuAltitude.Click += new System.EventHandler(this.toolStripMenuAltitude_Click);
            // 
            // toolStripMenuDateTime
            // 
            this.toolStripMenuDateTime.Checked = true;
            this.toolStripMenuDateTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuDateTime.Name = "toolStripMenuDateTime";
            this.toolStripMenuDateTime.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuDateTime.Text = "Date && Time";
            this.toolStripMenuDateTime.CheckStateChanged += new System.EventHandler(this.toolStripMenuDateTime_CheckStateChanged);
            this.toolStripMenuDateTime.Click += new System.EventHandler(this.toolStripMenuDateTime_Click);
            // 
            // toolStripMenuTitle
            // 
            this.toolStripMenuTitle.Checked = true;
            this.toolStripMenuTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuTitle.Name = "toolStripMenuTitle";
            this.toolStripMenuTitle.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuTitle.Text = "Photo Title";
            this.toolStripMenuTitle.CheckStateChanged += new System.EventHandler(this.toolStripMenuTitle_CheckStateChanged);
            this.toolStripMenuTitle.Click += new System.EventHandler(this.toolStripMenuTitle_Click);
            // 
            // toolStripMenuComment
            // 
            this.toolStripMenuComment.Checked = true;
            this.toolStripMenuComment.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuComment.Name = "toolStripMenuComment";
            this.toolStripMenuComment.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuComment.Text = "Comment";
            this.toolStripMenuComment.CheckStateChanged += new System.EventHandler(this.toolStripMenuComment_CheckStateChanged);
            this.toolStripMenuComment.Click += new System.EventHandler(this.toolStripMenuComment_Click);
            // 
            // toolStripMenuCamera
            // 
            this.toolStripMenuCamera.Checked = true;
            this.toolStripMenuCamera.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuCamera.Name = "toolStripMenuCamera";
            this.toolStripMenuCamera.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuCamera.Text = "Camera Model";
            this.toolStripMenuCamera.CheckStateChanged += new System.EventHandler(this.toolStripMenuCamera_CheckStateChanged);
            this.toolStripMenuCamera.Click += new System.EventHandler(this.toolStripMenuCamera_Click);
            // 
            // toolStripMenuPhotoSource
            // 
            this.toolStripMenuPhotoSource.Checked = true;
            this.toolStripMenuPhotoSource.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuPhotoSource.Name = "toolStripMenuPhotoSource";
            this.toolStripMenuPhotoSource.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuPhotoSource.Text = "Photo Source";
            this.toolStripMenuPhotoSource.CheckStateChanged += new System.EventHandler(this.toolStripMenuPhotoSource_CheckStateChanged);
            this.toolStripMenuPhotoSource.Click += new System.EventHandler(this.toolStripMenuPhotoSource_Click);
            // 
            // toolStripMenuReferenceID
            // 
            this.toolStripMenuReferenceID.Checked = true;
            this.toolStripMenuReferenceID.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuReferenceID.Name = "toolStripMenuReferenceID";
            this.toolStripMenuReferenceID.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuReferenceID.Text = "ReferenceID";
            this.toolStripMenuReferenceID.CheckStateChanged += new System.EventHandler(this.toolStripMenuReferenceID_CheckStateChanged);
            this.toolStripMenuReferenceID.Click += new System.EventHandler(this.toolStripMenuReferenceID_Click);
            // 
            // bindingSourceImageList
            // 
            this.bindingSourceImageList.DataSource = typeof(ActivityPicturePlugin.Helper.ImageData);
            // 
            // panelPictureAlbumView
            // 
            this.panelPictureAlbumView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPictureAlbumView.AutoScroll = true;
            this.panelPictureAlbumView.AutoScrollMinSize = new System.Drawing.Size(50, 50);
            this.panelPictureAlbumView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelPictureAlbumView.BackColor = System.Drawing.Color.Transparent;
            this.panelPictureAlbumView.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.panelPictureAlbumView.BorderColor = System.Drawing.Color.Transparent;
            this.panelPictureAlbumView.BorderShadowColor = System.Drawing.Color.Transparent;
            this.panelPictureAlbumView.Controls.Add(this.pictureAlbumView);
            this.panelPictureAlbumView.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.panelPictureAlbumView.HeadingFont = null;
            this.panelPictureAlbumView.HeadingLeftMargin = 0;
            this.panelPictureAlbumView.HeadingText = null;
            this.panelPictureAlbumView.HeadingTextColor = System.Drawing.Color.Black;
            this.panelPictureAlbumView.HeadingTopMargin = 3;
            this.panelPictureAlbumView.Location = new System.Drawing.Point(10, 80);
            this.panelPictureAlbumView.Margin = new System.Windows.Forms.Padding(4);
            this.panelPictureAlbumView.MinimumSize = new System.Drawing.Size(4, 200);
            this.panelPictureAlbumView.Name = "panelPictureAlbumView";
            this.panelPictureAlbumView.Size = new System.Drawing.Size(576, 273);
            this.panelPictureAlbumView.TabIndex = 1;
            this.panelPictureAlbumView.Click += new System.EventHandler(this.panelPictureAlbumView_Click);
            // 
            // pictureAlbumView
            // 
            this.pictureAlbumView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureAlbumView.AutoScroll = true;
            this.pictureAlbumView.AutoScrollMinSize = new System.Drawing.Size(50, 50);
            this.pictureAlbumView.AutoSize = true;
            this.pictureAlbumView.ImageList = null;
            this.pictureAlbumView.Location = new System.Drawing.Point(0, 0);
            this.pictureAlbumView.Margin = new System.Windows.Forms.Padding(4);
            this.pictureAlbumView.MaximumImageSize = ActivityPicturePlugin.Helper.PictureAlbum.MaxImageSize.NoLimit;
            this.pictureAlbumView.MinimumSize = new System.Drawing.Size(0, 50);
            this.pictureAlbumView.Name = "pictureAlbumView";
            this.pictureAlbumView.NoThumbNails = false;
            this.pictureAlbumView.SelectedIndex = -1;
            this.pictureAlbumView.Size = new System.Drawing.Size(576, 50);
            this.pictureAlbumView.TabIndex = 5;
            this.pictureAlbumView.Visible = false;
            this.pictureAlbumView.Zoom = 0;
            this.pictureAlbumView.SelectedChanged += new ActivityPicturePlugin.Helper.PictureAlbum.SelectedChangedEventHandler(this.pictureAlbumView_SelectedChanged);
            this.pictureAlbumView.ZoomChange += new ActivityPicturePlugin.Helper.PictureAlbum.ZoomChangeEventHandler(this.pictureAlbumView_ZoomChange);
            this.pictureAlbumView.UpdateVideoToolBar += new ActivityPicturePlugin.Helper.PictureAlbum.UpdateVideoToolBarEventHandler(this.pictureAlbumView_UpdateVideoToolBar);
            this.pictureAlbumView.ShowVideoOptions += new ActivityPicturePlugin.Helper.PictureAlbum.ShowVideoOptionsEventHandler(this.pictureAlbumView_ShowVideoOptions);
            this.pictureAlbumView.VideoChanged += new ActivityPicturePlugin.Helper.PictureAlbum.CurrentVideoIndexChangedEventHandler(this.pictureAlbumView_VideoChanged);
            this.pictureAlbumView.Load += new System.EventHandler(this.pictureAlbumView_Load);
            this.pictureAlbumView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureAlbumView_MouseClick);
            // 
            // importControl1
            // 
            this.importControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importControl1.Location = new System.Drawing.Point(5, 4);
            this.importControl1.Margin = new System.Windows.Forms.Padding(5);
            this.importControl1.Name = "importControl1";
            this.importControl1.Padding = new System.Windows.Forms.Padding(4);
            this.importControl1.ShowAllActivities = false;
            this.importControl1.Size = new System.Drawing.Size(596, 365);
            this.importControl1.TabIndex = 6;
            this.importControl1.Visible = false;
            this.importControl1.ActivityImagesChanged += new ActivityPicturePlugin.UI.ImportControl.ActivityImagesChangedEventHandler(this.importControl1_ActivityImagesChanged);
            // 
            // groupBoxListOptions
            // 
            this.groupBoxListOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxListOptions.Controls.Add(this.btnGEList);
            this.groupBoxListOptions.Controls.Add(this.toolstripListOptions);
            this.groupBoxListOptions.Controls.Add(this.btnTimeOffset);
            this.groupBoxListOptions.Controls.Add(this.btnKML);
            this.groupBoxListOptions.Controls.Add(this.btnGeoTag);
            this.groupBoxListOptions.Location = new System.Drawing.Point(10, 10);
            this.groupBoxListOptions.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxListOptions.Name = "groupBoxListOptions";
            this.groupBoxListOptions.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxListOptions.Size = new System.Drawing.Size(576, 60);
            this.groupBoxListOptions.TabIndex = 6;
            this.groupBoxListOptions.TabStop = false;
            this.groupBoxListOptions.Text = "Image Options";
            // 
            // btnGEList
            // 
            this.btnGEList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGEList.BackColor = System.Drawing.Color.Transparent;
            this.btnGEList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.btnGEList.CenterImage = global::ActivityPicturePlugin.Properties.Resources.GE2;
            this.btnGEList.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnGEList.HyperlinkStyle = false;
            this.btnGEList.ImageMargin = 2;
            this.btnGEList.LeftImage = null;
            this.btnGEList.Location = new System.Drawing.Point(539, 23);
            this.btnGEList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGEList.Name = "btnGEList";
            this.btnGEList.PushStyle = true;
            this.btnGEList.RightImage = null;
            this.btnGEList.Size = new System.Drawing.Size(23, 23);
            this.btnGEList.TabIndex = 11;
            this.btnGEList.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnGEList.TextLeftMargin = 2;
            this.btnGEList.TextRightMargin = 2;
            this.btnGEList.Click += new System.EventHandler(this.btnGEList_Click);
            // 
            // toolstripListOptions
            // 
            this.toolstripListOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolstripListOptions.Dock = System.Windows.Forms.DockStyle.None;
            this.toolstripListOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolstripListOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonGE});
            this.toolstripListOptions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolstripListOptions.Location = new System.Drawing.Point(547, 23);
            this.toolstripListOptions.Name = "toolstripListOptions";
            this.toolstripListOptions.Size = new System.Drawing.Size(24, 7);
            this.toolstripListOptions.TabIndex = 10;
            this.toolstripListOptions.Text = "toolstripListOptions";
            this.toolstripListOptions.Visible = false;
            // 
            // toolStripButtonGE
            // 
            this.toolStripButtonGE.AutoToolTip = false;
            this.toolStripButtonGE.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGE.Name = "toolStripButtonGE";
            this.toolStripButtonGE.Size = new System.Drawing.Size(23, 4);
            this.toolStripButtonGE.Text = "toolStripButtonGE";
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
            this.btnTimeOffset.Location = new System.Drawing.Point(105, 23);
            this.btnTimeOffset.Margin = new System.Windows.Forms.Padding(5);
            this.btnTimeOffset.Name = "btnTimeOffset";
            this.btnTimeOffset.PushStyle = true;
            this.btnTimeOffset.RightImage = null;
            this.btnTimeOffset.Size = new System.Drawing.Size(100, 28);
            this.btnTimeOffset.TabIndex = 8;
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
            this.btnKML.Location = new System.Drawing.Point(272, 23);
            this.btnKML.Margin = new System.Windows.Forms.Padding(5);
            this.btnKML.Name = "btnKML";
            this.btnKML.PushStyle = true;
            this.btnKML.RightImage = null;
            this.btnKML.Size = new System.Drawing.Size(100, 28);
            this.btnKML.TabIndex = 9;
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
            this.btnGeoTag.Location = new System.Drawing.Point(8, 23);
            this.btnGeoTag.Margin = new System.Windows.Forms.Padding(5);
            this.btnGeoTag.Name = "btnGeoTag";
            this.btnGeoTag.PushStyle = true;
            this.btnGeoTag.RightImage = null;
            this.btnGeoTag.Size = new System.Drawing.Size(100, 28);
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
            this.groupBoxVideo.Controls.Add(this.sliderVideo);
            this.groupBoxVideo.Location = new System.Drawing.Point(290, 10);
            this.groupBoxVideo.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxVideo.Name = "groupBoxVideo";
            this.groupBoxVideo.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxVideo.Size = new System.Drawing.Size(299, 60);
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
            this.toolStripButtonStop,
            this.toolStripButtonSnapshot});
            this.toolStripVideo.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripVideo.Location = new System.Drawing.Point(4, 23);
            this.toolStripVideo.Name = "toolStripVideo";
            this.toolStripVideo.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripVideo.Size = new System.Drawing.Size(93, 23);
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
            // toolStripButtonSnapshot
            // 
            this.toolStripButtonSnapshot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSnapshot.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSnapshot.Image")));
            this.toolStripButtonSnapshot.ImageTransparentColor = System.Drawing.Color.Red;
            this.toolStripButtonSnapshot.Name = "toolStripButtonSnapshot";
            this.toolStripButtonSnapshot.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonSnapshot.Text = "toolStripButtonSnapshot";
            this.toolStripButtonSnapshot.Click += new System.EventHandler(this.toolStripButtonSnapshot_Click);
            // 
            // volumeSlider2
            // 
            this.volumeSlider2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeSlider2.Location = new System.Drawing.Point(197, 20);
            this.volumeSlider2.Margin = new System.Windows.Forms.Padding(5);
            this.volumeSlider2.MaximumSize = new System.Drawing.Size(133, 28);
            this.volumeSlider2.Name = "volumeSlider2";
            this.volumeSlider2.ShowVolumeText = true;
            this.volumeSlider2.Size = new System.Drawing.Size(93, 28);
            this.volumeSlider2.TabIndex = 6;
            this.volumeSlider2.Volume = ((uint)(100u));
            this.volumeSlider2.VolumeChanged += new ActivityPicturePlugin.Helper.VolumeSlider.VolumeChangedEventHandler(this.volumeSlider2_VolumeChanged);
            // 
            // sliderVideo
            // 
            this.sliderVideo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderVideo.BackColor = System.Drawing.Color.Transparent;
            this.sliderVideo.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.sliderVideo.LargeChange = ((uint)(10u));
            this.sliderVideo.Location = new System.Drawing.Point(101, 24);
            this.sliderVideo.Margin = new System.Windows.Forms.Padding(4);
            this.sliderVideo.Maximum = 1000;
            this.sliderVideo.MouseEffects = false;
            this.sliderVideo.Name = "sliderVideo";
            this.sliderVideo.Size = new System.Drawing.Size(88, 20);
            this.sliderVideo.SmallChange = ((uint)(1u));
            this.sliderVideo.TabIndex = 6;
            this.sliderVideo.ThumbRoundRectSize = new System.Drawing.Size(15, 15);
            this.sliderVideo.ThumbSize = 20;
            this.sliderVideo.Value = 0;
            this.sliderVideo.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sliderVideo_Scroll);
            // 
            // groupBoxImage
            // 
            this.groupBoxImage.Controls.Add(this.labelImageSize);
            this.groupBoxImage.Controls.Add(this.sliderImageSize);
            this.groupBoxImage.Location = new System.Drawing.Point(10, 10);
            this.groupBoxImage.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxImage.Name = "groupBoxImage";
            this.groupBoxImage.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxImage.Size = new System.Drawing.Size(280, 60);
            this.groupBoxImage.TabIndex = 7;
            this.groupBoxImage.TabStop = false;
            this.groupBoxImage.Text = "Image Options";
            // 
            // labelImageSize
            // 
            this.labelImageSize.AutoSize = true;
            this.labelImageSize.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelImageSize.Location = new System.Drawing.Point(8, 25);
            this.labelImageSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelImageSize.Name = "labelImageSize";
            this.labelImageSize.Size = new System.Drawing.Size(81, 17);
            this.labelImageSize.TabIndex = 2;
            this.labelImageSize.Text = "Image Size:";
            // 
            // sliderImageSize
            // 
            this.sliderImageSize.BackColor = System.Drawing.Color.Transparent;
            this.sliderImageSize.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.sliderImageSize.ContextMenuStrip = this.contextMenuSliderImageSize;
            this.sliderImageSize.LargeChange = ((uint)(10u));
            this.sliderImageSize.Location = new System.Drawing.Point(136, 24);
            this.sliderImageSize.Margin = new System.Windows.Forms.Padding(4);
            this.sliderImageSize.Minimum = 1;
            this.sliderImageSize.MouseEffects = false;
            this.sliderImageSize.Name = "sliderImageSize";
            this.sliderImageSize.Size = new System.Drawing.Size(135, 20);
            this.sliderImageSize.SmallChange = ((uint)(1u));
            this.sliderImageSize.TabIndex = 6;
            this.sliderImageSize.ThumbRoundRectSize = new System.Drawing.Size(15, 15);
            this.sliderImageSize.ThumbSize = 20;
            this.sliderImageSize.Value = 20;
            this.sliderImageSize.ValueChanged += new System.EventHandler(this.sliderImageSize_ValueChanged);
            // 
            // contextMenuSliderImageSize
            // 
            this.contextMenuSliderImageSize.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuFitToWindow});
            this.contextMenuSliderImageSize.Name = "contextMenuSliderImageSize";
            this.contextMenuSliderImageSize.Size = new System.Drawing.Size(204, 26);
            // 
            // toolStripMenuFitToWindow
            // 
            this.toolStripMenuFitToWindow.Name = "toolStripMenuFitToWindow";
            this.toolStripMenuFitToWindow.Size = new System.Drawing.Size(203, 22);
            this.toolStripMenuFitToWindow.Text = "Fit Images To View";
            this.toolStripMenuFitToWindow.CheckStateChanged += new System.EventHandler(this.toolStripMenuFitToWindow_CheckStateChanged);
            this.toolStripMenuFitToWindow.Click += new System.EventHandler(this.toolStripMenuFitToWindow_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.BackColor = System.Drawing.SystemColors.Window;
            this.progressBar1.Location = new System.Drawing.Point(4, 4);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.MarqueeAnimationSpeed = 5;
            this.progressBar1.Maximum = 40;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(584, 20);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 10;
            this.progressBar1.Visible = false;
            // 
            // waypointDataGridViewTextBoxColumn
            // 
            this.waypointDataGridViewTextBoxColumn.Name = "waypointDataGridViewTextBoxColumn";
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
            this.contextMenuStripView.Size = new System.Drawing.Size(97, 70);
            // 
            // pictureAlbumToolStripMenuItem
            // 
            this.pictureAlbumToolStripMenuItem.Name = "pictureAlbumToolStripMenuItem";
            this.pictureAlbumToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.pictureAlbumToolStripMenuItem.Text = "Album";
            this.pictureAlbumToolStripMenuItem.Click += new System.EventHandler(this.pictureAlbumToolStripMenuItem_Click);
            // 
            // pictureListToolStripMenuItem
            // 
            this.pictureListToolStripMenuItem.Name = "pictureListToolStripMenuItem";
            this.pictureListToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.pictureListToolStripMenuItem.Text = "List";
            this.pictureListToolStripMenuItem.Click += new System.EventHandler(this.pictureListToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
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
            this.timerVideo.Interval = 1000;
            this.timerVideo.Tick += new System.EventHandler(this.timerVideo_Tick);
            // 
            // contextMenuListGE
            // 
            this.contextMenuListGE.Name = "contextMenuListGE";
            this.contextMenuListGE.Size = new System.Drawing.Size(61, 4);
            this.contextMenuListGE.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuListGE_ItemClicked);
            // 
            // contextMenuPictureAlbum
            // 
            this.contextMenuPictureAlbum.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuOpenFolder,
            this.toolStripMenuResetSnapshot,
            this.toolStripMenuRemove});
            this.contextMenuPictureAlbum.Name = "contextMenuStrip1";
            this.contextMenuPictureAlbum.Size = new System.Drawing.Size(227, 70);
            // 
            // toolStripMenuOpenFolder
            // 
            this.toolStripMenuOpenFolder.Name = "toolStripMenuOpenFolder";
            this.toolStripMenuOpenFolder.Size = new System.Drawing.Size(226, 22);
            this.toolStripMenuOpenFolder.Text = "Open Containing Folder";
            this.toolStripMenuOpenFolder.Click += new System.EventHandler(this.toolStripMenuOpenFolder_Click);
            // 
            // toolStripMenuResetSnapshot
            // 
            this.toolStripMenuResetSnapshot.Name = "toolStripMenuResetSnapshot";
            this.toolStripMenuResetSnapshot.Size = new System.Drawing.Size(226, 22);
            this.toolStripMenuResetSnapshot.Text = "Reset Snapshot";
            this.toolStripMenuResetSnapshot.Click += new System.EventHandler(this.toolStripMenuResetSnapshot_Click);
            // 
            // toolStripMenuRemove
            // 
            this.toolStripMenuRemove.Name = "toolStripMenuRemove";
            this.toolStripMenuRemove.Size = new System.Drawing.Size(226, 22);
            this.toolStripMenuRemove.Text = "Remove";
            this.toolStripMenuRemove.Click += new System.EventHandler(this.toolStripMenuRemove_Click_1);
            // 
            // ActivityPicturePageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelViews);
            this.Controls.Add(this.actionBannerViews);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ActivityPicturePageControl";
            this.Size = new System.Drawing.Size(600, 400);
            this.Load += new System.EventHandler(this.ActivityPicturePageControl_Load);
            this.VisibleChanged += new System.EventHandler(this.ActivityPicturePageControl_VisibleChanged);
            this.panelViews.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImages)).EndInit();
            this.contextMenuListImages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceImageList)).EndInit();
            this.panelPictureAlbumView.ResumeLayout(false);
            this.panelPictureAlbumView.PerformLayout();
            this.groupBoxListOptions.ResumeLayout(false);
            this.groupBoxListOptions.PerformLayout();
            this.toolstripListOptions.ResumeLayout(false);
            this.toolstripListOptions.PerformLayout();
            this.groupBoxVideo.ResumeLayout(false);
            this.groupBoxVideo.PerformLayout();
            this.toolStripVideo.ResumeLayout(false);
            this.toolStripVideo.PerformLayout();
            this.groupBoxImage.ResumeLayout(false);
            this.groupBoxImage.PerformLayout();
            this.contextMenuSliderImageSize.ResumeLayout(false);
            this.contextMenuStripView.ResumeLayout(false);
            this.contextMenuPictureAlbum.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ZoneFiveSoftware.Common.Visuals.ActionBanner actionBannerViews;
        private ZoneFiveSoftware.Common.Visuals.Panel panelViews;
        // Removed panel from PictureAlbum to prevent flickering
        // Added panelPictureAlbumView so we still get scrollbars
        private PanelEx panelPictureAlbumView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripView;
        private System.Windows.Forms.ToolStripMenuItem pictureAlbumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pictureListToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSourceImageList;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogImport;
        private ActivityPicturePlugin.Helper.PictureAlbum pictureAlbumView;
        private System.Windows.Forms.OpenFileDialog openFileDialogImport;
        private System.Windows.Forms.GroupBox groupBoxListOptions;
        private System.Windows.Forms.Timer timerVideo;
        private System.Windows.Forms.DataGridViewTextBoxColumn waypointDataGridViewTextBoxColumn;
        private ImportControl importControl1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private ZoneFiveSoftware.Common.Visuals.Button btnGeoTag;
        private ZoneFiveSoftware.Common.Visuals.Button btnKML;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private ZoneFiveSoftware.Common.Visuals.Button btnTimeOffset;
        private System.Windows.Forms.GroupBox groupBoxImage;
        private System.Windows.Forms.Label labelImageSize;
        private System.Windows.Forms.GroupBox groupBoxVideo;
        private ActivityPicturePlugin.Helper.VolumeSlider volumeSlider2;
        private System.Windows.Forms.ToolStrip toolStripVideo;
        private System.Windows.Forms.ToolStripButton toolStripButtonPlay;
        private System.Windows.Forms.ToolStripButton toolStripButtonPause;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridView dataGridViewImages;
        private System.Windows.Forms.ContextMenuStrip contextMenuListImages;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuTypeImage;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuExifGPS;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuAltitude;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuComment;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuDateTime;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuTitle;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuCamera;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuPhotoSource;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuReferenceID;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuNone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataGridViewImageColumn cThumbnail;
        private System.Windows.Forms.DataGridViewImageColumn cTypeImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn cExifGPS;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAltitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDateTimeOriginal;
        private System.Windows.Forms.DataGridViewTextBoxColumn cComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPhotoTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCamera;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPhotoSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn cReferenceID;
        private System.Windows.Forms.ProgressBar progressBar1;
        // Replaced basic TrackBar with ColorSlider to prevent flickering
        private MB.Controls.ColorSlider sliderImageSize;
        private MB.Controls.ColorSlider sliderVideo;
        private System.Windows.Forms.ToolStripButton toolStripButtonSnapshot;
        private System.Windows.Forms.ContextMenuStrip contextMenuSliderImageSize;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuFitToWindow;
        private System.Windows.Forms.ToolStrip toolstripListOptions;
        private System.Windows.Forms.ToolStripButton toolStripButtonGE;
        private ZoneFiveSoftware.Common.Visuals.Button btnGEList;
        private System.Windows.Forms.ContextMenuStrip contextMenuListGE;
        private System.Windows.Forms.ContextMenuStrip contextMenuPictureAlbum;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuResetSnapshot;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuRemove;
    }
}