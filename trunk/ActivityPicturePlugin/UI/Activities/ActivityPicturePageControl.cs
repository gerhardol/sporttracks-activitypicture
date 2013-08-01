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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Resources;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ActivityPicturePlugin.Helper;
using ActivityPicturePlugin.Settings;
#if !ST_2_1
using ActivityPicturePlugin.UI.MapLayers;
#endif

//todo:
//2. When doubleclicking the pictures in GE, the pictures are gray. Minatures are fine, so are pictures extracted from the .kmz 
//4. Can this be the default in List view too? I can sort the pictures, but if I do something with them, the default order is actived again. 
//Remove last three list columns, use tooltip instead
//Copy list text, including ref path?
//Migration of paths
//Migration of thumbnails 2->3
//Click on pictures in Route: open imageviewer?

namespace ActivityPicturePlugin.UI.Activities
{
    public partial class ActivityPicturePageControl : UserControl, IDisposable
    {
#if !ST_2_1
        public ActivityPicturePageControl(IDetailPage detailPage, IDailyActivityView view)
           : this()
        {
            //m_DetailPage = detailPage;
            m_view = view;
            //if (m_DetailPage != null)
            //{
            //    expandButton.Visible = true;
            //}
            m_layer = PicturesLayer.Instance((IView)view);
        }
#endif

        public ActivityPicturePageControl()
        {
            this.Visible = false;
            InitializeComponent();

            // Setting Dock to Fill through Designer causes it to reformat
            // and marks it as 'changed' everytime you open it.
            // Workaround until I figure out what's going on.
            this.importControl1.Dock = DockStyle.Fill;

            LoadSettings();

            //Create directory if it does not already exist!
            if ( !Directory.Exists( ImageFilesFolder ) ) Directory.CreateDirectory( ImageFilesFolder );

            //read settings from logfile
            ActivityPicturePageControl.PluginSettingsData.ReadSettings();
        }

        #region Private members
        private IActivity _Activity; //Current activity
        private enum ShowMode : short
        {
            Album = 1,
            List = 2,
            Import = 3,
        }
        private ShowMode Mode = ShowMode.List;
        private PluginData PluginExtensionData = new PluginData();
        private bool _showPage = false;
        private List<string> SelectedReferenceIDs = new List<string>();
        private bool PreventRowsRemoved = false;//To prevent recursive calls
        #endregion

        #region Public members
        //TODO: GetFullPath required due to relative paths
        public static string ImageFilesFolder = System.IO.Path.GetFullPath( ActivityPicturePlugin.Plugin.GetApplication().
#if ST_2_1
            //TODO:
            SystemPreferences.WebFilesFolder + "\\Images\\" );
#else
            //TODO: Still ST2
Configuration.CommonWebFilesFolder + "\\..\\..\\2.0\\Web Files\\Images\\");
       // + GUIDs.PluginMain.ToString() + Path.DirectorySeparatorChar);
#endif

        public static PluginSettings PluginSettingsData = new PluginSettings();
#if !ST_2_1
        //private IDetailPage m_DetailPage = null;
        private IDailyActivityView m_view = null;
        private PicturesLayer m_layer = null;
#endif
        #endregion

        #region Public properties
        bool m_Active = false;
        ZoneFiveSoftware.Common.Visuals.ITheme m_theme;
        public IActivity Activity
        {
            set
            {
                //if activity has changed
                if ( ( !object.ReferenceEquals( _Activity, value ) ) )
                {
                    //Update activity
                    _Activity = value;
                    if ( !m_Active ) ResetPage();
                    else
                    {
                        this.pictureAlbumView.ActivityChanging();
                        Functions.ClearImageList( this.pictureAlbumView );
                        this.dataGridViewImages.Visible = false;
                        //ResetPage();
                        if ( _Activity != null )
                        {
                            ReloadData();
                            UpdateView();
                        }
                        else
                            ResetPage();
                    }
                }
            }
            get
            {
                return _Activity;
            }
        }

        public void ShowProgressBar()
        {
            this.progressBar1.MarqueeAnimationSpeed = 1;
            this.progressBar1.Visible = true;
        }
        public void HideProgressBar()
        {
            this.progressBar1.MarqueeAnimationSpeed = 0;
            this.progressBar1.Visible = false;
        }
        #endregion

        #region Helper Methods
        public void LoadSettings()
        {
            Mode = (ShowMode)ActivityPicturePlugin.Source.Settings.ActivityMode; //Activity Picture Mode (Album, List, Import)
            //sliderImageSize.Value = ActivityPicturePlugin.Source.Settings.ImageZoom;

            //pictureAlbumView.MaximumImageSize = (PictureAlbum.MaxImageSize)ActivityPicturePlugin.Source.Settings.MaxImageSize;
            toolStripMenuTypeImage.Checked = ActivityPicturePlugin.Source.Settings.CTypeImage;
            toolStripMenuExifGPS.Checked = ActivityPicturePlugin.Source.Settings.CExifGPS;
            toolStripMenuAltitude.Checked = ActivityPicturePlugin.Source.Settings.CAltitude;
            toolStripMenuComment.Checked = ActivityPicturePlugin.Source.Settings.CComment;
            //toolStripMenuThumbnail.Checked = ActivityPicturePlugin.Source.Settings.CThumbnail;
            toolStripMenuDateTime.Checked = ActivityPicturePlugin.Source.Settings.CDateTimeOriginal;
            toolStripMenuTitle.Checked = ActivityPicturePlugin.Source.Settings.CPhotoTitle;
            toolStripMenuCamera.Checked = ActivityPicturePlugin.Source.Settings.CCamera;
            toolStripMenuPhotoSource.Checked = ActivityPicturePlugin.Source.Settings.CPhotoSource;
            toolStripMenuReferenceID.Checked = ActivityPicturePlugin.Source.Settings.CReferenceID;

            toolStripMenuFitToWindow.Checked = ActivityPicturePlugin.Source.Settings.MaxImageSize == (int)PictureAlbum.MaxImageSize.FitToWindow;

            dataGridViewImages.Columns["cTypeImage"].Visible = ActivityPicturePlugin.Source.Settings.CTypeImage;
            dataGridViewImages.Columns["cExifGPS"].Visible = ActivityPicturePlugin.Source.Settings.CExifGPS;
            dataGridViewImages.Columns["cAltitude"].Visible = ActivityPicturePlugin.Source.Settings.CAltitude;
            dataGridViewImages.Columns["cComment"].Visible = ActivityPicturePlugin.Source.Settings.CComment;
            dataGridViewImages.Columns["cThumbnail"].Visible = ActivityPicturePlugin.Source.Settings.CThumbnail;
            dataGridViewImages.Columns["cDateTimeOriginal"].Visible = ActivityPicturePlugin.Source.Settings.CDateTimeOriginal;
            dataGridViewImages.Columns["cPhotoTitle"].Visible = ActivityPicturePlugin.Source.Settings.CPhotoTitle;
            dataGridViewImages.Columns["cCamera"].Visible = ActivityPicturePlugin.Source.Settings.CCamera;
            dataGridViewImages.Columns["cPhotoSource"].Visible = ActivityPicturePlugin.Source.Settings.CPhotoSource;
            dataGridViewImages.Columns["cReferenceID"].Visible = ActivityPicturePlugin.Source.Settings.CReferenceID;

            volumeSlider2.Volume = ActivityPicturePlugin.Source.Settings.VolumeValue;
            //volumeSlider2.Volume = 10u;

        }

        public void ResetPage()
        {
            this.timerVideo.Stop();
            this.pictureAlbumView.StopVideo();
            this.pictureAlbumView.ActivityChanging();
            Functions.ClearImageList( this.pictureAlbumView );
            this.contextMenuStripView.Enabled = false;
            this.groupBoxImage.Visible = false;
            this.groupBoxVideo.Visible = false;
            this.groupBoxListOptions.Visible = false;
            this.pictureAlbumView.Visible = false;
            this.panelPictureAlbumView.Visible = false;
            this.importControl1.Visible = false;
            this.dataGridViewImages.Visible = false;
            UpdateDataGridView();
        }

        public void RefreshPage()
        {
            try
            {
                this.dataGridViewImages.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );
                Functions.ClearImageList( this.pictureAlbumView );
                this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );
                ReloadData();
                UpdateView();
                this.dataGridViewImages.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );

                this.Invalidate();
                this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );
            }
            catch ( Exception )
            {
                //throw;
            }
        }

        public bool HidePage()
        {
            _showPage = false;
            m_Active = false;
            this.ResetPage();
#if !ST_2_1
            m_layer.HidePage();
#endif
            return false;
        }

        public void ShowPage( string bookmark )
        {
            _showPage = true;
            this.Visible = true;
            m_Active = true;

#if !ST_2_1
            m_layer.ShowPage("");
#endif
        }

        public void ThemeChanged( ZoneFiveSoftware.Common.Visuals.ITheme visualTheme )
        {
            m_theme = visualTheme;

            //change theme colors
            this.Invalidate();

            this.BackColor = visualTheme.Control;
            this.actionBannerViews.ThemeChanged( visualTheme );

            this.panelViews.ThemeChanged( visualTheme );
            this.panelViews.HeadingBackColor = visualTheme.Control;
            //this.panelViews.BackColor = visualTheme.Control;
            //this.panelViews.ForeColor = visualTheme.ControlText;


            this.importControl1.ThemeChanged( visualTheme );
            // I do not know why the ImportControl is so hard to resize.  
            // If a background color other than transparent is used, 
            // it overlaps the lower and right borders of its parent.
            // Might be a bug in the SplitterControl panels.  Don't
            // Feel like creating a new one.  Flickers a bit too.
            this.importControl1.BackColor = Color.Transparent;

            this.pictureAlbumView.ThemeChanged( visualTheme );
            this.groupBoxListOptions.BackColor = visualTheme.Control;
            this.groupBoxListOptions.ForeColor = visualTheme.ControlText;

            this.groupBoxVideo.BackColor = visualTheme.Control;
            this.groupBoxVideo.ForeColor = visualTheme.ControlText;
            this.toolStripVideo.BackColor = visualTheme.Control;
            this.toolstripListOptions.BackColor = visualTheme.Control;

            this.sliderImageSize.BarInnerColor = visualTheme.Selected;
            this.sliderImageSize.BarOuterColor = visualTheme.MainHeader;
            this.sliderImageSize.BarPenColor = visualTheme.SubHeader;
            this.sliderImageSize.ElapsedInnerColor = visualTheme.Window;
            this.sliderImageSize.ElapsedOuterColor = visualTheme.MainHeader;

            this.sliderVideo.BarInnerColor = visualTheme.Selected;
            this.sliderVideo.BarOuterColor = visualTheme.MainHeader;
            this.sliderVideo.BarPenColor = visualTheme.SubHeader;
            this.sliderVideo.ElapsedInnerColor = visualTheme.Window;
            this.sliderVideo.ElapsedOuterColor = visualTheme.MainHeader;

            this.groupBoxImage.BackColor = visualTheme.Control;
            this.groupBoxImage.ForeColor = visualTheme.ControlText;
            this.sliderImageSize.BackColor = visualTheme.Control;
            this.sliderImageSize.ForeColor = visualTheme.MainHeader;

            this.volumeSlider2.ThemeChanged( visualTheme );

            this.dataGridViewImages.ForeColor = visualTheme.ControlText;
            //this.dataGridViewImages.BackgroundColor = visualTheme.Control;
            this.dataGridViewImages.BackgroundColor = visualTheme.Window;
            this.dataGridViewImages.GridColor = visualTheme.Border;
            this.dataGridViewImages.DefaultCellStyle.BackColor = visualTheme.Window;
            this.dataGridViewImages.RowHeadersDefaultCellStyle.BackColor = visualTheme.SubHeader;
            this.dataGridViewImages.ColumnHeadersDefaultCellStyle.BackColor = visualTheme.SubHeader;
            this.dataGridViewImages.ColumnHeadersDefaultCellStyle.ForeColor = visualTheme.SubHeaderText;

            dataGridViewImages.RowsDefaultCellStyle.SelectionBackColor = visualTheme.Selected;
            dataGridViewImages.RowsDefaultCellStyle.SelectionForeColor = visualTheme.SelectedText;

            dataGridViewImages.RowsDefaultCellStyle.BackColor = visualTheme.Window;
            dataGridViewImages.AlternatingRowsDefaultCellStyle.BackColor = GridAltRowColor( visualTheme.Window );

            this.progressBar1.BackColor = this.BackColor;
            this.progressBar1.ForeColor = visualTheme.Selected;
        }

        public void UICultureChanged( System.Globalization.CultureInfo culture )
        {
            //change number formats
            RefreshPage();

            if ( this.Mode == ShowMode.Album )
                this.actionBannerViews.Text = Resources.Resources.pictureAlbumToolStripMenuItem_Text;
            else if ( this.Mode == ShowMode.List )
                this.actionBannerViews.Text = CommonResources.Text.LabelList;
            else if ( this.Mode == ShowMode.Import )
                this.actionBannerViews.Text = CommonResources.Text.ActionImport;
            this.cAltitude.HeaderText = CommonResources.Text.LabelElevation;
            this.cComment.HeaderText = Resources.Resources.commentDataGridViewTextBoxColumn_HeaderText;
            this.cDateTimeOriginal.HeaderText = CommonResources.Text.LabelDate;
            this.cCamera.HeaderText = Resources.Resources.equipmentModelDataGridViewTextBoxColumn_HeaderText;
            // Not sure how important it is to have this capitalized, nor am I
            // sure if this should be done for all resources.
            this.cExifGPS.HeaderText = Functions.CapitalizeAllWords( CommonResources.Text.LabelGPSLocation );
            this.cPhotoSource.HeaderText = Resources.Resources.photoSourceDataGridViewTextBoxColumn_HeaderText;
            this.cReferenceID.HeaderText = Resources.Resources.referenceIDDataGridViewTextBoxColumn_HeaderText;
            this.cThumbnail.HeaderText = Resources.Resources.thumbnailDataGridViewImageColumn_HeaderText;
            this.cPhotoTitle.HeaderText = Resources.Resources.titleDataGridViewTextBoxColumn_HeaderText;
            this.pictureAlbumToolStripMenuItem.Text = Resources.Resources.pictureAlbumToolStripMenuItem_Text;
            this.pictureListToolStripMenuItem.Text = CommonResources.Text.LabelList;
            this.importToolStripMenuItem.Text = CommonResources.Text.ActionImport;
            this.waypointDataGridViewTextBoxColumn.HeaderText = Resources.Resources.waypointDataGridViewTextBoxColumn_HeaderText;
            this.groupBoxVideo.Text = Resources.Resources.groupBoxVideo_Text;
            this.groupBoxImage.Text = CommonResources.Text.LabelPhoto;
            this.groupBoxListOptions.Text = Resources.Resources.goupBoxListOptions_Text;
            this.toolStripButtonPause.ToolTipText = Resources.Resources.toolStripButtonPause_ToolTipText;
            this.toolStripButtonPlay.ToolTipText = Resources.Resources.toolStripButtonPlay_ToolTipText;
            this.toolStripButtonStop.ToolTipText = Resources.Resources.toolStripButtonStop_ToolTipText;
            this.toolStripButtonSnapshot.ToolTipText = Resources.Resources.toolStripButtonSnapshot_ToolTipText;

            this.toolTip1.SetToolTip( this.btnGeoTag, Resources.Resources.tooltip_OnlySelectedImages );
            this.toolTip1.SetToolTip( this.btnKML, Resources.Resources.tooltip_OnlySelectedImages );
            this.toolTip1.SetToolTip( this.btnTimeOffset, Resources.Resources.tooltip_OnlySelectedImages );

            this.cTypeImage.HeaderText = Resources.Resources.TypeImage_HeaderText;

            using ( Graphics g = this.CreateGraphics() )
            {
                this.btnGeoTag.Text = Resources.Resources.btnGeoTag_Text;
                this.btnGeoTag.Width = (int)g.MeasureString( this.btnGeoTag.Text, this.btnGeoTag.Font ).Width + 10; ;
                this.btnKML.Text = Resources.Resources.btnKML_Text;
                this.btnKML.Width = (int)g.MeasureString( this.btnKML.Text, this.btnKML.Font ).Width + 10;
                this.btnTimeOffset.Text = Resources.Resources.btnTimeOffset_Text;
                this.btnTimeOffset.Width = (int)g.MeasureString( this.btnTimeOffset.Text, this.btnTimeOffset.Font ).Width + 10;
                this.btnTimeOffset.Left = ( this.btnGeoTag.Right + 10 );
                this.btnKML.Left = ( this.btnTimeOffset.Right + 10 );
                this.labelImageSize.Text = Resources.Resources.labelImageSize_Text;
                this.labelImageSize.Width = (int)g.MeasureString( this.labelImageSize.Text, this.labelImageSize.Font ).Width + 10;
            }

            this.sliderImageSize.Location = new Point( this.labelImageSize.Width + this.labelImageSize.Left + 20, this.sliderImageSize.Top );
            this.groupBoxImage.Width = this.sliderImageSize.Width + this.sliderImageSize.Left + 10;
            this.groupBoxVideo.Left = this.groupBoxImage.Left + this.groupBoxImage.Width + 10;
            //this.groupBoxVideo.Width = this.Width - this.groupBoxVideo.Left - 10;
            this.groupBoxVideo.Width = this.panelViews.Width - this.groupBoxVideo.Left - 10;

            this.toolStripMenuFitToWindow.Text = Resources.Resources.FitImagesToView_Text;

            //this.toolStripMenuCopy.Text = Resources.Resources.CopyToClipboard_Text;
            this.toolStripMenuCopy.Text = CommonResources.Text.ActionCopy;
            this.toolStripMenuNone.Text = Resources.Resources.HideAllColumns_Text;
            this.toolStripMenuAll.Text = Resources.Resources.ShowAllColumns_Text;
            this.toolStripMenuTypeImage.Text = Resources.Resources.TypeImage_HeaderText;
            this.toolStripMenuExifGPS.Text = CommonResources.Text.LabelGPSLocation;
            this.toolStripMenuAltitude.Text = CommonResources.Text.LabelElevation;
            this.toolStripMenuComment.Text = Resources.Resources.commentDataGridViewTextBoxColumn_HeaderText;
            //this.toolStripMenuThumbnail.Text = Resources.Resources.thumbnailDataGridViewImageColumn_HeaderText;
            this.toolStripMenuDateTime.Text = CommonResources.Text.LabelDate;
            this.toolStripMenuCamera.Text = Resources.Resources.equipmentModelDataGridViewTextBoxColumn_HeaderText;
            this.toolStripMenuPhotoSource.Text = Resources.Resources.photoSourceDataGridViewTextBoxColumn_HeaderText;
            this.toolStripMenuReferenceID.Text = Resources.Resources.referenceIDDataGridViewTextBoxColumn_HeaderText;

            this.toolStripMenuResetSnapshot.Text = Resources.Resources.ResetSnapshot_Text;
            this.toolStripMenuOpenFolder.Text = Resources.Resources.OpenContainingFolder_Text;
            this.toolStripMenuRemove.Text = CommonResources.Text.ActionRemove;

            this.importControl1.UpdateUICulture( culture );
        }

        // Cleans up the thumbnails that were created but not saved during the last session
        public void CleanupThumbnails()
        {
            string s = ActivityPicturePlugin.Source.Settings.NewThumbnailsCreated;
            if ( s != "" )
            {
                Thread thread = new Thread( new ParameterizedThreadStart( RunCleanup ) );
                thread.Start( s );
            }
            ActivityPicturePlugin.Source.Settings.NewThumbnailsCreated = "";
        }

        private void RunCleanup( object sNewThumbs )
        {
            string s = sNewThumbs as string;
            string[] sFiles = s.Split( '\t' );
            List<string> thumbNails = GetThumbnailPathsAllActivities();

            foreach ( string sFile in sFiles )
            {
                if ( sFile == "" ) continue;
                try
                {
                    FileInfo fi = new FileInfo( sFile );
                    bool bFound = false;
                    if ( sFile != "" )
                    {
                        foreach ( string sThumbnail in thumbNails )
                        {
                            if ( String.Compare( sThumbnail, sFile, true ) == 0 )
                            {
                                bFound = true;
                                break;
                            }
                        }
                    }
                    if ( !bFound )
                        fi.Delete();
                }
                catch ( Exception )
                {
                }
            }
        }

        private void UpdateView()
        {
            try
            {
                this.contextMenuStripView.Enabled = true;
                this.dataGridViewImages.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );

                //Load controls depending on selected view
                if ( this.Mode == ShowMode.Album )
                {
                    this.actionBannerViews.Text = Resources.Resources.pictureAlbumToolStripMenuItem_Text;
                    this.groupBoxVideo.Enabled = true;	// ( this.pictureAlbumView.CurrentStatus != PictureAlbum.MediaStatus.None );
                    this.groupBoxImage.Visible = true;
                    this.groupBoxImage.Enabled = true;
                    this.groupBoxVideo.Visible = true;
                    this.groupBoxListOptions.Visible = false;
                    this.UpdateToolBar();
                    this.dataGridViewImages.Visible = false;
                    this.pictureAlbumView.Visible = true;
                    this.importControl1.Visible = false;
                    this.panelPictureAlbumView.Visible = true;
                    if ( ( this.pictureAlbumView.ImageList == null ) || ( this.pictureAlbumView.ImageList.Count == 0 ) )
                    {
                        this.groupBoxImage.Enabled = false;
                        this.groupBoxVideo.Enabled = false;
                    }
                    this.pictureAlbumView.Invalidate();
                }
                else if ( this.Mode == ShowMode.List )
                {
                    this.pictureAlbumView.PauseVideo();
                    this.actionBannerViews.Text = CommonResources.Text.LabelList;
                    this.groupBoxImage.Visible = false;
                    this.groupBoxVideo.Enabled = false;
                    this.groupBoxVideo.Visible = false;
                    this.groupBoxListOptions.Visible = true;
                    this.dataGridViewImages.Visible = true;
                    this.pictureAlbumView.Visible = false;
                    this.panelPictureAlbumView.Visible = false;
                    this.importControl1.Visible = false;
                    UpdateDataGridView();
                    if ( this.dataGridViewImages.Rows.Count > 0 )
                    {
                        this.btnGeoTag.Enabled = true;
                        this.btnTimeOffset.Enabled = true;
                    }
                    else
                    {
                        this.btnGeoTag.Enabled = false;
                        this.btnTimeOffset.Enabled = false;
                    }
                }
                else if ( this.Mode == ShowMode.Import )
                {
                    this.pictureAlbumView.PauseVideo();
                    this.actionBannerViews.Text = CommonResources.Text.ActionImport;
                    this.dataGridViewImages.Visible = false;
                    this.pictureAlbumView.Visible = false;
                    this.panelPictureAlbumView.Visible = false;
                    this.groupBoxImage.Visible = false;
                    this.groupBoxVideo.Enabled = false;
                    this.groupBoxVideo.Visible = false;
                    this.groupBoxListOptions.Visible = false;
                    this.importControl1.LoadNodes();
                    this.importControl1.Visible = true;
                }

                this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );
            }
            catch ( Exception )
            {
                //throw;
            }
        }

        public void UpdateToolBar()
        {
            switch ( this.pictureAlbumView.CurrentStatus )
            {
                case Helper.PictureAlbum.MediaStatus.None:
                    toolStripButtonPlay.Enabled = false;
                    toolStripButtonPause.Enabled = false;
                    toolStripButtonStop.Enabled = false;
                    toolStripButtonSnapshot.Enabled = false;
                    sliderVideo.Enabled = false;
                    volumeSlider2.Enabled = false;
                    break;

                case Helper.PictureAlbum.MediaStatus.Paused:
                    toolStripButtonPlay.Enabled = true;
                    toolStripButtonPause.Enabled = false;
                    toolStripButtonStop.Enabled = true;
                    toolStripButtonSnapshot.Enabled = true; //pictureAlbumView.IsAvi();
                    sliderVideo.Enabled = true;
                    volumeSlider2.Enabled = true;
                    break;

                case Helper.PictureAlbum.MediaStatus.Running:
                    toolStripButtonPlay.Enabled = false;
                    toolStripButtonPause.Enabled = true;
                    toolStripButtonStop.Enabled = true;
                    toolStripButtonSnapshot.Enabled = true;
                    toolStripButtonSnapshot.Enabled = true; //pictureAlbumView.IsAvi();
                    sliderVideo.Enabled = true;
                    volumeSlider2.Enabled = true;
                    timerVideo.Start();
                    break;

                case Helper.PictureAlbum.MediaStatus.Stopped:
                    toolStripButtonPlay.Enabled = true;
                    toolStripButtonPause.Enabled = false;
                    toolStripButtonStop.Enabled = false;
                    timerVideo.Stop();
                    toolStripButtonSnapshot.Enabled = false;
                    sliderVideo.Enabled = false;
                    volumeSlider2.Enabled = true;
                    break;
            }
        }

        private void ReloadData()
        {
            System.Windows.Forms.ToolStripMenuItem tsmiRemove = null;
            try
            {
                if ( _Activity != null )
                {
                    this.dataGridViewImages.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );
                    //this.Visible = true;
                    this.Visible = _showPage;

                    //Read data and add new controls
                    this.PluginExtensionData = Helper.Functions.ReadExtensionData( _Activity );
                    //if ( this.PluginExtensionData.Images.Count != 0 )
                    if ( ( this.PluginExtensionData.Images.Count != 0 ) || ( this.PluginExtensionData.Images.Count != this.pictureAlbumView.ImageList.Count ) )
                    {
                        this.pictureAlbumView.ImageList = this.PluginExtensionData.LoadImageData( this.PluginExtensionData.Images );
                        SortListView();
                    }
                    sliderImageSize.Value = this.PluginExtensionData.ImageZoom;

                    for ( int i = 0; i < contextMenuListGE.Items.Count; i++ ) //ToolStripMenuItem mi in contextMenuListGE.Items )
                        contextMenuListGE.Items[i].Dispose();
                    contextMenuListGE.Items.Clear();

                    if ( this.PluginExtensionData.GELinks.Count > 0 )
                    {
                        this.btnGEList.Visible = true;

                        foreach ( string sFile in this.PluginExtensionData.GELinks )
                        {
                            System.IO.FileInfo fi = new FileInfo( sFile );
                            ToolStripMenuItem tsi = (ToolStripMenuItem)this.contextMenuListGE.Items.Add( fi.Name );
                            tsi.Tag = fi.FullName;
                            tsi.ToolTipText = fi.FullName;
                            tsmiRemove = new ToolStripMenuItem( CommonResources.Text.ActionRemove, null, new System.EventHandler( this.toolStripMenuRemove_Click ) );
                            tsmiRemove.Tag = fi.FullName;
                            tsmiRemove.Name = "Remove";
                            tsi.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] { tsmiRemove } );
                        }
                    }
                    else
                    {
                        this.btnGEList.Visible = false;
                    }


                    this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );
#if !ST_2_1
                    m_layer.HidePage(); //defer updates
                    this.m_layer.PictureSize = this.sliderImageSize.Value;
                    m_layer.Pictures = this.pictureAlbumView.ImageList;
                    m_layer.ShowPage("");//Refresh
#endif
                }
                else
                {
                    this.Visible = false;
                }
            }
            catch ( Exception )
            {
                if ( tsmiRemove != null )
                    tsmiRemove.Dispose();
                tsmiRemove = null;
                //throw;
            }
        }

        private List<string> GetThumbnailPathsAllActivities()
        {
            List<string> s = new List<string>();

            IEnumerable<IActivity> activities = new List<IActivity>();
            activities = ActivityPicturePlugin.Plugin.GetApplication().Logbook.Activities;

            foreach ( IActivity activity in activities )
            {
                PluginData data = Helper.Functions.ReadExtensionData( activity );
                if ( data.Images.Count > 0 )
                {
                    for ( int j = 0; j < data.Images.Count; j++ )
                        s.Add( Functions.thumbnailPath( data.Images[j].ReferenceID ) );
                }
            }
            return s;
        }

        private static Color GridAltRowColor( Color defaultRowColor )
        {
            int R = (int)( defaultRowColor.R / 1.012 );
            int G = (int)( defaultRowColor.G / 1.02 );
            int B = (int)( defaultRowColor.B / 1.025 );
            return Color.FromArgb( R, G, B );
        }

        private void InitializeDataGridView()
        {
            this.cThumbnail.DisplayIndex = 0;
            this.cTypeImage.DisplayIndex = 1;
            this.cExifGPS.DisplayIndex = 2;
            this.cAltitude.DisplayIndex = 3;
            this.cDateTimeOriginal.DisplayIndex = 4;
            this.cPhotoTitle.DisplayIndex = 5;
            this.cComment.DisplayIndex = 6;
            this.cCamera.DisplayIndex = 7;
            this.cPhotoSource.DisplayIndex = 8;
            this.cReferenceID.DisplayIndex = 9;
            this.waypointDataGridViewTextBoxColumn.DisplayIndex = 10;

            SortListView();
        }

        private void UpdateDataGridView()
        {
            try
            {
                PreventRowsRemoved = true;
                this.bindingSourceImageList.DataSource = this.pictureAlbumView.ImageList;
                this.bindingSourceImageList.ResetBindings( false );
                PreventRowsRemoved = false;
                SetSortGlyph();

                this.dataGridViewImages.Invalidate();

            }
            catch ( Exception )
            {
                //throw;
            }
        }

        private void SetSortGlyph()
        {
            PictureAlbum.ImageSortMode ism = (PictureAlbum.ImageSortMode)ActivityPicturePlugin.Source.Settings.SortMode;
            DataGridViewColumn col = GetSortColumn( ism );
            col.HeaderCell.SortGlyphDirection = GetSortDirection( ism );
        }

        private void SortListView()
        {
            //DataGridViewColumn col = GetSortColumn( ActivityPicturePageControl.PluginSettingsData.data.SortMode );
            PictureAlbum.ImageSortMode ism = (PictureAlbum.ImageSortMode)ActivityPicturePlugin.Source.Settings.SortMode;
            DataGridViewColumn col = GetSortColumn( ism );
            this.pictureAlbumView.ImageList.Sort( CompareImageData );
            UpdateDataGridView();
        }

        private DataGridViewColumn GetSortColumn( PictureAlbum.ImageSortMode imageSortMode )
        {
            switch ( imageSortMode )
            {
                case PictureAlbum.ImageSortMode.byAltitudeAscending:
                case PictureAlbum.ImageSortMode.byAltitudeDescending:
                    return this.cAltitude;
                case PictureAlbum.ImageSortMode.byCameraModelAscending:
                case PictureAlbum.ImageSortMode.byCameraModelDescending:
                    return this.cCamera;
                case PictureAlbum.ImageSortMode.byCommentAscending:
                case PictureAlbum.ImageSortMode.byCommentDescending:
                    return this.cComment;
                case PictureAlbum.ImageSortMode.byDateTimeAscending:
                case PictureAlbum.ImageSortMode.byDateTimeDescending:
                    return this.cDateTimeOriginal;
                case PictureAlbum.ImageSortMode.byExifGPSAscending:
                case PictureAlbum.ImageSortMode.byExifGPSDescending:
                    return this.cExifGPS;
                case PictureAlbum.ImageSortMode.byPhotoSourceAscending:
                case PictureAlbum.ImageSortMode.byPhotoSourceDescending:
                    return this.cPhotoSource;
                case PictureAlbum.ImageSortMode.byTitleAscending:
                case PictureAlbum.ImageSortMode.byTitleDescending:
                    return this.cPhotoTitle;
                case PictureAlbum.ImageSortMode.byTypeAscending:
                case PictureAlbum.ImageSortMode.byTypeDescending:
                    return this.cTypeImage;
                case PictureAlbum.ImageSortMode.none:
                default:
                    return null;
            }
        }

        private SortOrder GetSortDirection( PictureAlbum.ImageSortMode imageSortMode )
        {
            SortOrder so = SortOrder.None;

            switch ( imageSortMode )
            {
                case PictureAlbum.ImageSortMode.byAltitudeAscending:
                case PictureAlbum.ImageSortMode.byCameraModelAscending:
                case PictureAlbum.ImageSortMode.byCommentAscending:
                case PictureAlbum.ImageSortMode.byDateTimeAscending:
                case PictureAlbum.ImageSortMode.byExifGPSAscending:
                case PictureAlbum.ImageSortMode.byPhotoSourceAscending:
                case PictureAlbum.ImageSortMode.byTitleAscending:
                case PictureAlbum.ImageSortMode.byTypeAscending:
                    so = SortOrder.Ascending;
                    break;
                case PictureAlbum.ImageSortMode.byAltitudeDescending:
                case PictureAlbum.ImageSortMode.byCameraModelDescending:
                case PictureAlbum.ImageSortMode.byCommentDescending:
                case PictureAlbum.ImageSortMode.byDateTimeDescending:
                case PictureAlbum.ImageSortMode.byExifGPSDescending:
                case PictureAlbum.ImageSortMode.byPhotoSourceDescending:
                case PictureAlbum.ImageSortMode.byTitleDescending:
                case PictureAlbum.ImageSortMode.byTypeDescending:
                    so = SortOrder.Descending;
                    break;
                default:
                    so = SortOrder.None;
                    break;
            }

            return so;

        }

        private PictureAlbum.ImageSortMode GetSortMode( DataGridViewColumn col )
        {
            PictureAlbum.ImageSortMode sortmode = (PictureAlbum.ImageSortMode)ActivityPicturePlugin.Source.Settings.SortMode;

            if ( col == this.cDateTimeOriginal )
            {
                if ( sortmode == PictureAlbum.ImageSortMode.byDateTimeAscending )
                    return PictureAlbum.ImageSortMode.byDateTimeDescending;
                else
                    return PictureAlbum.ImageSortMode.byDateTimeAscending;
            }
            else if ( col == this.cExifGPS )
            {
                if ( sortmode == PictureAlbum.ImageSortMode.byExifGPSAscending )
                    return PictureAlbum.ImageSortMode.byExifGPSDescending;
                else
                    return PictureAlbum.ImageSortMode.byExifGPSAscending;
            }
            else if ( col == this.cAltitude )
            {
                if ( sortmode == PictureAlbum.ImageSortMode.byAltitudeAscending )
                    return PictureAlbum.ImageSortMode.byAltitudeDescending;
                else
                    return PictureAlbum.ImageSortMode.byAltitudeAscending;
            }
            else if ( col == this.cCamera )
            {
                if ( sortmode == PictureAlbum.ImageSortMode.byCameraModelAscending )
                    return PictureAlbum.ImageSortMode.byCameraModelDescending;
                else
                    return PictureAlbum.ImageSortMode.byCameraModelAscending;
            }
            else if ( col == this.cPhotoTitle )
            {
                if ( sortmode == PictureAlbum.ImageSortMode.byTitleAscending )
                    return PictureAlbum.ImageSortMode.byTitleDescending;
                else
                    return PictureAlbum.ImageSortMode.byTitleAscending;
            }
            else if ( col == this.cPhotoSource )
            {
                if ( sortmode == PictureAlbum.ImageSortMode.byPhotoSourceAscending )
                    return PictureAlbum.ImageSortMode.byPhotoSourceDescending;
                else
                    return PictureAlbum.ImageSortMode.byPhotoSourceAscending;
            }
            else if ( col == this.cComment )
            {
                if ( sortmode == PictureAlbum.ImageSortMode.byCommentAscending )
                    return PictureAlbum.ImageSortMode.byCommentDescending;
                else
                    return PictureAlbum.ImageSortMode.byCommentAscending;
            }
            else if ( col == this.cThumbnail )
                return PictureAlbum.ImageSortMode.none;
            else if ( col == this.cTypeImage )
            {
                if ( sortmode == PictureAlbum.ImageSortMode.byTypeAscending )
                    return PictureAlbum.ImageSortMode.byTypeDescending;
                else
                    return PictureAlbum.ImageSortMode.byTypeAscending;
            }
            else
                return PictureAlbum.ImageSortMode.none;
        }

        private List<ImageData> GetSelectedImageData()
        {
            List<ImageData> il = new List<ImageData>();
            DataGridViewSelectedRowCollection SelRows = this.dataGridViewImages.SelectedRows;
            for ( int i = 0; i < SelRows.Count; i++ )
            {
                il.Add( this.pictureAlbumView.ImageList[SelRows[i].Index] );
            }
            return il;
        }

        internal int CompareImageData( ImageData x, ImageData y )
        {
            int retval = 0;

            try
            {
                if ( x == null )
                {
                    if ( y == null ) return 0; // If x is null and y is null, they're equal. 
                    else return -1; // If x is null and y is not null, y is greater. 
                }
                else
                {
                    // If x is not null...
                    if ( y == null ) return 1;// ...and y is null, x is greater.
                    else
                    {
                        // ...and y is not null, compare the dates
                        switch ( (PictureAlbum.ImageSortMode)ActivityPicturePlugin.Source.Settings.SortMode )
                        //switch ( ActivityPicturePageControl.PluginSettingsData.data.SortMode )
                        {
                            case PictureAlbum.ImageSortMode.byAltitudeAscending:
                                retval = x.EW.GPSAltitude.CompareTo( y.EW.GPSAltitude );
                                break;
                            case PictureAlbum.ImageSortMode.byAltitudeDescending:
                                retval = y.EW.GPSAltitude.CompareTo( x.EW.GPSAltitude );
                                break;
                            case PictureAlbum.ImageSortMode.byCameraModelAscending:
                                retval = x.EquipmentModel.CompareTo( y.EquipmentModel );
                                break;
                            case PictureAlbum.ImageSortMode.byCameraModelDescending:
                                retval = y.EquipmentModel.CompareTo( x.EquipmentModel );
                                break;
                            case PictureAlbum.ImageSortMode.byCommentAscending:
                                retval = x.Comments.CompareTo( y.Comments );
                                break;
                            case PictureAlbum.ImageSortMode.byCommentDescending:
                                retval = y.Comments.CompareTo( x.Comments );
                                break;
                            case PictureAlbum.ImageSortMode.byDateTimeAscending:
                                retval = x.EW.DateTimeOriginal.CompareTo( y.EW.DateTimeOriginal );
                                break;
                            case PictureAlbum.ImageSortMode.byDateTimeDescending:
                                retval = y.EW.DateTimeOriginal.CompareTo( x.EW.DateTimeOriginal );
                                break;
                            case PictureAlbum.ImageSortMode.byExifGPSAscending:
                                retval = x.ExifGPS.CompareTo( y.ExifGPS );
                                break;
                            case PictureAlbum.ImageSortMode.byExifGPSDescending:
                                retval = y.ExifGPS.CompareTo( x.ExifGPS );
                                break;
                            case PictureAlbum.ImageSortMode.byPhotoSourceAscending:
                                retval = x.PhotoSource.CompareTo( y.PhotoSource );
                                break;
                            case PictureAlbum.ImageSortMode.byPhotoSourceDescending:
                                retval = y.PhotoSource.CompareTo( x.PhotoSource );
                                break;
                            case PictureAlbum.ImageSortMode.byTitleAscending:
                                retval = x.Title.CompareTo( y.Title );
                                break;
                            case PictureAlbum.ImageSortMode.byTitleDescending:
                                retval = y.Title.CompareTo( x.Title );
                                break;
                            case PictureAlbum.ImageSortMode.byTypeAscending:
                                retval = x.Type.CompareTo( y.Type );
                                break;
                            case PictureAlbum.ImageSortMode.byTypeDescending:
                                retval = y.Type.CompareTo( x.Type );
                                break;
                            case PictureAlbum.ImageSortMode.none:
                                break;
                        }
                    }
                }
            }
            catch ( Exception )
            {
                //throw;
            }

            return retval;

        }

        #endregion

        #region Event handler methods
        void dataGridViewImages_DataError( object sender, DataGridViewDataErrorEventArgs e )
        {
        }

        void dataGridViewImages_CellDoubleClick( object sender, DataGridViewCellEventArgs e )
        {
            try
            {
                if ( this.dataGridViewImages.Columns[e.ColumnIndex].Name == this.cPhotoSource.Name
                 || this.dataGridViewImages.Columns[e.ColumnIndex].Name == this.cThumbnail.Name
                 || this.dataGridViewImages.Columns[e.ColumnIndex].Name == this.cTypeImage.Name )
                {
                    //open the image/video in external window
                    Helper.Functions.OpenExternal( this.pictureAlbumView.ImageList[e.RowIndex] );
                }
                else if ( this.dataGridViewImages.Columns[e.ColumnIndex].Name == this.cDateTimeOriginal.Name )
                {
                    //set the time stamp
                    using ( ModifyTimeStamp frm = new ModifyTimeStamp( this.pictureAlbumView.ImageList[e.RowIndex] ) )
                    {
                        frm.ThemeChanged( m_theme );
                        frm.ShowDialog();
                    }

                }
            }
            catch ( Exception )
            {
                //throw;
            }
        }

        void dataGridViewImages_CellValueChanged( object sender, DataGridViewCellEventArgs e )
        {
            if ( _showPage )
            {
                try
                {
                    if ( this._Activity != null )
                    {
                        this.PluginExtensionData.GetImageDataSerializable( this.pictureAlbumView.ImageList );
                        Functions.WriteExtensionData( _Activity, this.PluginExtensionData );
                    }
                }
                catch ( Exception )
                {
                    //throw;
                }
            }
        }

        void dataGridViewImages_RowsRemoved( object sender, DataGridViewRowsRemovedEventArgs e )
        {
            if ( !PreventRowsRemoved )
            {
                if ( this._Activity != null )
                {
                    // remove thumbnail image in web folder
                    Functions.DeleteThumbnails( this.SelectedReferenceIDs );
                    this.PluginExtensionData.GetImageDataSerializable( this.pictureAlbumView.ImageList );
                    Functions.WriteExtensionData( _Activity, this.PluginExtensionData );
                }
            }
        }

        private void dataGridViewImages_ColumnHeaderMouseClick( object sender, DataGridViewCellMouseEventArgs e )
        {
            try
            {
                PictureAlbum.ImageSortMode oldSortMode = (PictureAlbum.ImageSortMode)ActivityPicturePlugin.Source.Settings.SortMode;
                DataGridViewColumn oldcol = GetSortColumn( oldSortMode );
                DataGridViewColumn col = this.dataGridViewImages.Columns[e.ColumnIndex];

                if ( col.ValueType == typeof( string ) )
                {
                    ActivityPicturePlugin.Source.Settings.SortMode = (int)GetSortMode( col );

                    // Get a list of selected rows
                    DataGridViewSelectedRowCollection selRows = dataGridViewImages.SelectedRows;
                    string[] srefs = new string[selRows.Count];
                    for ( int i = 0; i < selRows.Count; i++ )
                        srefs[i] = selRows[i].Cells["cReferenceID"].Value.ToString();

                    if ( ActivityPicturePlugin.Source.Settings.SortMode != (int)PictureAlbum.ImageSortMode.none )
                    {
                        this.pictureAlbumView.ImageList.Sort( CompareImageData );
                        UpdateDataGridView();
                        //ActivityPicturePageControl.PluginSettingsData.WriteSettings();
                    }
                    else
                    {
                        ActivityPicturePlugin.Source.Settings.SortMode = (int)oldSortMode;
                    }

                    //Reselect the previously selected rows
                    int nFirstSelectedRow = Int32.MaxValue;
                    for ( int i = 0; i < dataGridViewImages.Rows.Count; i++ )
                    {
                        dataGridViewImages.Rows[i].Selected = false;
                        for ( int j = 0; j < srefs.Length; j++ )
                        {
                            if ( String.Compare( srefs[j],
                                dataGridViewImages.Rows[i].Cells["cReferenceID"].Value.ToString(),
                                true ) == 0 )
                            {
                                dataGridViewImages.Rows[i].Selected = true;
                                if ( i < nFirstSelectedRow ) nFirstSelectedRow = i;
                                break;
                            }
                        }
                    }

                    if ( nFirstSelectedRow != Int32.MaxValue )
                        dataGridViewImages.FirstDisplayedScrollingRowIndex = nFirstSelectedRow;

                    SetSortGlyph();
                }
            }


            catch ( Exception )
            {
                // throw;
            }
        }

        private void pictureListToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if ( this.Mode == ShowMode.Import )
            {
                Functions.ClearImageList( this.pictureAlbumView );
                ReloadData();
            }
            this.Mode = ShowMode.List;
            ActivityPicturePlugin.Source.Settings.ActivityMode = (Int32)Mode;
            UpdateView();
        }

        private void actionBanner1_MenuClicked( object sender, EventArgs e )
        {
            this.contextMenuStripView.Show( this.panelViews, this.panelViews.Width - this.contextMenuStripView.Width - 1, 0 );
        }

        private void pictureAlbumToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if ( this.Mode == ShowMode.Import )
            {
                Functions.ClearImageList( this.pictureAlbumView );
                ReloadData();
            }
            this.Mode = ShowMode.Album;
            ActivityPicturePlugin.Source.Settings.ActivityMode = (Int32)Mode;
            UpdateView();
        }

        private void ActivityPicturePageControl_Load( object sender, EventArgs e )
        {
            try
            {
                //this.pictureAlbumView.ParentCtl = this;
                this.ThemeChanged( m_theme );
                InitializeDataGridView();
                //RefreshPage();

                //ReloadData();
                //UpdateView();
            }
            catch ( Exception )
            {

                //throw;
            }
        }

        private void dataGridViewImages_SelectionChanged( object sender, EventArgs e )
        {
            try
            {
                SelectedReferenceIDs.Clear();
                DataGridViewSelectedRowCollection SelRows = ( (DataGridView)( sender ) ).SelectedRows;
                IList<ImageData> selImages = new List<ImageData>();
                for ( int i = 0; i < SelRows.Count; i++ )
                {
                    ImageData im = this.pictureAlbumView.ImageList[SelRows[i].Index];
                    SelectedReferenceIDs.Add( im.ReferenceID );
                    selImages.Add( im );
                }
                //TODO: Or use zoom button
#if !ST_2_1
                m_layer.SelectedPictures = selImages;
#endif
                SelRows = null;
            }
            catch ( Exception )
            {
                //throw;
            }

        }

        private void dataGridViewImages_BindingContextChanged( object sender, EventArgs e )
        {
            SetSortGlyph();
        }

        private void sliderVideo_Scroll( object sender, ScrollEventArgs e )
        {
            this.pictureAlbumView.SetVideoPosition( sliderVideo.Value );
        }

        private void sliderImageSize_ValueChanged( object sender, EventArgs e )
        {
            if ( this.Mode == ShowMode.Album )
            {
                MB.Controls.ColorSlider tb = (MB.Controls.ColorSlider)( sender );
                this.pictureAlbumView.Zoom = tb.Value;

                ActivityPicturePlugin.Source.Settings.ImageZoom = tb.Value;
                this.PluginExtensionData.ImageZoom = tb.Value;
                Helper.Functions.WriteExtensionData( _Activity, this.PluginExtensionData );

                this.pictureAlbumView.Invalidate();
#if !ST_2_1
                this.m_layer.PictureSize = tb.Value;
                this.m_layer.Refresh();
#endif
            }
        }

        //private void btnImpDir_Click(object sender, EventArgs e)
        //    {
        //    try
        //        {
        //        DialogResult dlg = this.folderBrowserDialogImport.ShowDialog();

        //        if ((folderBrowserDialogImport.SelectedPath != null) & (dlg != DialogResult.Cancel))
        //            {
        //            ImportImages(System.IO.Directory.GetFiles(folderBrowserDialogImport.SelectedPath), true);

        //            //sort by exif date
        //            this.pictureAlbumView.ImageList.Sort(CompareByDate);

        //            //save image locations
        //            this.PluginExtensionData.GetImageDataSerializable(this.pictureAlbumView.ImageList);
        //            Functions.WriteExtensionData(_Activity, this.PluginExtensionData);

        //            if (this.Mode == ShowMode.Album)
        //                {
        //                //this.pictureAlbumView.Invalidate();
        //                this.pictureAlbumView.PaintAlbumView(true);
        //                }
        //            else if (this.Mode == ShowMode.List)
        //                {
        //                UpdateDataGridView();
        //                }
        //            }
        //        }
        //    catch (Exception)
        //        {

        //        //throw;
        //        }
        //    }

        //private void btnImpSel_Click(object sender, EventArgs e)
        //    {
        //    try
        //        {
        //        this.openFileDialogImport.FileName = "";
        //        this.openFileDialogImport.Multiselect = true;
        //        this.openFileDialogImport.Filter = "(*.bmp;*.jpg;*.gif;*.png)|*.bmp;*.jpg;*.gif;*.png|(*.avi;*.wmv)|*.avi;*.wmv|(*.*)|*.*";

        //        //this.openFileDialog1.FilterIndex = 
        //        this.openFileDialogImport.ShowDialog();
        //        ImportImages(this.openFileDialogImport.FileNames, false);
        //        //save image locations

        //        this.PluginExtensionData.GetImageDataSerializable(this.pictureAlbumView.ImageList);
        //        Functions.WriteExtensionData(_Activity, this.PluginExtensionData);

        //        if (this.Mode == ShowMode.List)
        //            {
        //            UpdateDataGridView();
        //            }
        //        else if (this.Mode == ShowMode.Album)
        //            {
        //            this.pictureAlbumView.Invalidate();
        //            }
        //        }
        //    catch (Exception)
        //        {

        //        throw;
        //        }
        //    }

        private void toolStripButtonPlay_Click( object sender, EventArgs e )
        {
            this.pictureAlbumView.PlayVideo();
            UpdateToolBar();
        }

        private void toolStripButtonPause_Click( object sender, EventArgs e )
        {
            this.pictureAlbumView.PauseVideo();
            UpdateToolBar();
        }

        private void toolStripButtonStop_Click( object sender, EventArgs e )
        {
            this.pictureAlbumView.StopVideo();
            this.sliderVideo.Value = 0;
            UpdateToolBar();
        }

        private void toolStripButtonSnapshot_Click( object sender, EventArgs e )
        {
            List<ImageData> ids = pictureAlbumView.ImageList;
            if ( pictureAlbumView.CurrentVideoIndex != -1 )
            {
                // Disable button until operation completes
                toolStripButtonSnapshot.Enabled = false;
                Application.DoEvents();

                ImageData id = ids[pictureAlbumView.CurrentVideoIndex];
                int iFrame = pictureAlbumView.GetCurrentVideoFrame();
                Size sizeFrame = pictureAlbumView.GetVideoSize();
                if ( sizeFrame.Width == -1 ) sizeFrame.Width = 500;
                if ( sizeFrame.Height == -1 ) sizeFrame.Height = 375;
                double dblTimePerFrame = pictureAlbumView.GetCurrentVideoTimePerFrame();
                id.ReplaceVideoThumbnail( iFrame, sizeFrame, dblTimePerFrame );

                Functions.ClearImageList( this.pictureAlbumView );

                toolStripButtonSnapshot.Enabled = true; //Re-enable button
                ReloadData();
                UpdateView();
            }
            else
                toolStripButtonSnapshot.Enabled = false;
        }

        private void timerVideo_Tick( object sender, EventArgs e )
        {
            this.sliderVideo.Value = (int)( this.sliderVideo.Maximum * this.pictureAlbumView.GetVideoPosition() );
        }

        private void importToolStripMenuItem_Click( object sender, EventArgs e )
        {
            this.Mode = ShowMode.Import;
            ActivityPicturePlugin.Source.Settings.ActivityMode = (Int32)Mode;
            UpdateView();
        }

        private void pictureAlbumView_ZoomChange( object sender, ActivityPicturePlugin.Helper.PictureAlbum.ZoomEventArgs e )
        {
            int increment = e.Increment;
            if ( increment != 0 ) this.sliderImageSize.Value += increment;
            else this.sliderImageSize.Value = this.pictureAlbumView.Zoom;

            ActivityPicturePlugin.Source.Settings.ImageZoom = this.sliderImageSize.Value;
        }

        private void pictureAlbumView_UpdateVideoToolBar( object sender, EventArgs e )
        {
            UpdateToolBar();
        }

        private void pictureAlbumView_ShowVideoOptions( object sender, EventArgs e )
        {
            this.groupBoxVideo.Enabled = true;
        }

        private void pictureAlbumView_Load( object sender, EventArgs e )
        {
            PictureAlbum pa = (PictureAlbum)( sender );
            pa.Zoom = this.sliderImageSize.Value;
        }

        private void btnGeoTag_Click( object sender, EventArgs e )
        {
            this.UseWaitCursor = true;
            dataGridViewImages.Enabled = false;
            btnGeoTag.Enabled = false;
            btnKML.Enabled = false;
            btnTimeOffset.Enabled = false;
            Application.DoEvents();

            List<ImageData> iList = GetSelectedImageData();
            foreach ( ImageData id in iList )
            {
                if ( id.Type == ImageData.DataTypes.Image )
                {
                    if ( System.IO.File.Exists( id.PhotoSource ) ) Functions.GeoTagWithActivity( id.PhotoSource, this._Activity );
                    if ( System.IO.File.Exists( id.ThumbnailPath ) ) Functions.GeoTagWithActivity( id.ThumbnailPath, this._Activity );
                }
            }
            Functions.ClearImageList( this.pictureAlbumView );
            ReloadData();
            UpdateView();

            this.UseWaitCursor = false;
            dataGridViewImages.Enabled = true;
            btnGeoTag.Enabled = true;
            btnKML.Enabled = true;
            btnTimeOffset.Enabled = true;
        }

        private void btnKML_Click( object sender, EventArgs e )
        {
            this.saveFileDialog.FileName = "";
            this.saveFileDialog.DefaultExt = "kmz";
            this.saveFileDialog.AddExtension = true;
            this.saveFileDialog.CheckPathExists = true;
            this.saveFileDialog.Filter = "Google Earth compressed (*.kmz)|*.kmz|Google Earth KML (*.kml)|*.kml";
            this.saveFileDialog.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments );
            DialogResult dres = this.saveFileDialog.ShowDialog();
            if ( dres == DialogResult.OK & this.saveFileDialog.FileName != "" )
            {
                Functions.PerformExportToGoogleEarth( GetSelectedImageData(), this._Activity, this.saveFileDialog.FileName );
                if ( ActivityPicturePlugin.Source.Settings.GEAutoOpen )
                    Functions.OpenExternal( this.saveFileDialog.FileName );

                if ( ActivityPicturePlugin.Source.Settings.GEStoreFileLocation )
                {
                    if ( !PluginExtensionData.GELinks.Contains( this.saveFileDialog.FileName ) )
                    {
                        PluginExtensionData.GELinks.Add( this.saveFileDialog.FileName );
                        Helper.Functions.WriteExtensionData( Activity, this.PluginExtensionData );
                        this.btnGEList.Visible = true;
                        ReloadData();
                    }
                }
            }

        }

        private void btnTimeOffset_Click( object sender, EventArgs e )
        {
            using ( TimeOffset frm = new TimeOffset( GetSelectedImageData() ) )
            {
                frm.ThemeChanged( m_theme );
                frm.ShowDialog();
            }
        }

        private void ActivityPicturePageControl_VisibleChanged( object sender, EventArgs e )
        {
            if ( _Activity == null ) this.Visible = false;
        }

        private void volumeSlider2_VolumeChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.VolumeValue = volumeSlider2.Volume;
        }

        #region toolStripMenuTypeImage Event Handlers
        private void toolStripMenuTypeImage_Click( object sender, EventArgs e )
        {
            toolStripMenuTypeImage.Checked = !toolStripMenuTypeImage.Checked;
        }
        private void toolStripMenuTypeImage_CheckStateChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.CTypeImage = toolStripMenuTypeImage.Checked;
            dataGridViewImages.Columns["cTypeImage"].Visible = toolStripMenuTypeImage.Checked;

        }
        private void toolStripMenuExifGPS_Click( object sender, EventArgs e )
        {
            toolStripMenuExifGPS.Checked = !toolStripMenuExifGPS.Checked;
        }
        private void toolStripMenuExifGPS_CheckStateChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.CExifGPS = toolStripMenuExifGPS.Checked;
            dataGridViewImages.Columns["cExifGPS"].Visible = toolStripMenuExifGPS.Checked;

        }
        private void toolStripMenuAltitude_Click( object sender, EventArgs e )
        {
            toolStripMenuAltitude.Checked = !toolStripMenuAltitude.Checked;
        }
        private void toolStripMenuAltitude_CheckStateChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.CAltitude = toolStripMenuAltitude.Checked;
            dataGridViewImages.Columns["cAltitude"].Visible = toolStripMenuAltitude.Checked;

        }
        private void toolStripMenuComment_Click( object sender, EventArgs e )
        {
            toolStripMenuComment.Checked = !toolStripMenuComment.Checked;
        }
        private void toolStripMenuComment_CheckStateChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.CComment = toolStripMenuComment.Checked;
            dataGridViewImages.Columns["cComment"].Visible = toolStripMenuComment.Checked;

        }
        /*private void toolStripMenuThumbnail_Click( object sender, EventArgs e )
        {
            toolStripMenuThumbnail.Checked = !toolStripMenuThumbnail.Checked;
        }
        private void toolStripMenuThumbnail_CheckStateChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.CThumbnail = toolStripMenuThumbnail.Checked;
            dataGridViewImages.Columns["cThumbnail"].Visible = toolStripMenuThumbnail.Checked;

        }*/
        private void toolStripMenuDateTime_Click( object sender, EventArgs e )
        {
            toolStripMenuDateTime.Checked = !toolStripMenuDateTime.Checked;
        }
        private void toolStripMenuDateTime_CheckStateChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.CDateTimeOriginal = toolStripMenuDateTime.Checked;
            dataGridViewImages.Columns["cDateTimeOriginal"].Visible = toolStripMenuDateTime.Checked;

        }
        private void toolStripMenuTitle_Click( object sender, EventArgs e )
        {
            toolStripMenuTitle.Checked = !toolStripMenuTitle.Checked;
        }
        private void toolStripMenuTitle_CheckStateChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.CPhotoTitle = toolStripMenuTitle.Checked;
            dataGridViewImages.Columns["cPhotoTitle"].Visible = toolStripMenuTitle.Checked;

        }
        private void toolStripMenuCamera_Click( object sender, EventArgs e )
        {
            toolStripMenuCamera.Checked = !toolStripMenuCamera.Checked;
        }
        private void toolStripMenuCamera_CheckStateChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.CCamera = toolStripMenuCamera.Checked;
            dataGridViewImages.Columns["cCamera"].Visible = toolStripMenuCamera.Checked;

        }
        private void toolStripMenuPhotoSource_Click( object sender, EventArgs e )
        {
            toolStripMenuPhotoSource.Checked = !toolStripMenuPhotoSource.Checked;
        }
        private void toolStripMenuPhotoSource_CheckStateChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.CPhotoSource = toolStripMenuPhotoSource.Checked;
            dataGridViewImages.Columns["cPhotoSource"].Visible = toolStripMenuPhotoSource.Checked;

        }
        private void toolStripMenuReferenceID_Click( object sender, EventArgs e )
        {
            toolStripMenuReferenceID.Checked = !toolStripMenuReferenceID.Checked;
        }
        private void toolStripMenuReferenceID_CheckStateChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.CReferenceID = toolStripMenuReferenceID.Checked;
            dataGridViewImages.Columns["cReferenceID"].Visible = toolStripMenuReferenceID.Checked;
        }
        private void toolStripMenuAll_Click( object sender, EventArgs e )
        {
            toolStripMenuTypeImage.Checked = true;
            toolStripMenuExifGPS.Checked = true;
            toolStripMenuAltitude.Checked = true;
            toolStripMenuComment.Checked = true;
            //toolStripMenuThumbnail.Checked = true;
            toolStripMenuDateTime.Checked = true;
            toolStripMenuTitle.Checked = true;
            toolStripMenuCamera.Checked = true;
            toolStripMenuPhotoSource.Checked = true;
            toolStripMenuReferenceID.Checked = true;
        }
        private void toolStripMenuNone_Click( object sender, EventArgs e )
        {
            toolStripMenuTypeImage.Checked = false;
            toolStripMenuExifGPS.Checked = false;
            toolStripMenuAltitude.Checked = false;
            toolStripMenuComment.Checked = false;
            //toolStripMenuThumbnail.Checked = false;
            toolStripMenuDateTime.Checked = false;
            toolStripMenuTitle.Checked = false;
            toolStripMenuCamera.Checked = false;
            toolStripMenuPhotoSource.Checked = false;
            toolStripMenuReferenceID.Checked = false;
        }
        private void toolStripMenuCopy_Click( object sender, EventArgs e )
        {
            StringBuilder s = new StringBuilder();
            foreach ( DataGridViewColumn column in dataGridViewImages.Columns )
            {
                if ( ( column.Visible ) && ( column.ValueType == typeof( string ) ) )
                {
                    string ss = column.HeaderText;
                    ss = ss.Replace( "\t", " " );
                    ss = ss.Replace( "\r\n", " " );

                    s.Append( ss + "\t" );
                }
            }
            s.Append( "\r\n" );
            int rowIndex = 0;
            foreach ( DataGridViewRow row in dataGridViewImages.Rows )
            {
                if ( row.Selected )
                {
                    foreach ( DataGridViewCell cell in row.Cells )
                    {
                        if ( ( cell.Visible ) && ( cell.ValueType == typeof( string ) ) )
                        {
                            string ss = cell.Value + "";
                            ss = ss.Replace( "\t", " " );
                            ss = ss.Replace( "\r\n", " " );

                            s.Append( ss + "\t" );
                        }
                    }
                    s.Append( "\r\n" );
                }
                rowIndex++;
            }
            Clipboard.SetText( s.ToString() );
        }

        #endregion

        private void toolStripMenuFitToWindow_Click( object sender, EventArgs e )
        {
            toolStripMenuFitToWindow.Checked = !toolStripMenuFitToWindow.Checked;
        }
        private void toolStripMenuFitToWindow_CheckStateChanged( object sender, EventArgs e )
        {
            if ( toolStripMenuFitToWindow.Checked )
            {
                ActivityPicturePlugin.Source.Settings.MaxImageSize = (int)PictureAlbum.MaxImageSize.FitToWindow;
                pictureAlbumView.MaximumImageSize = PictureAlbum.MaxImageSize.FitToWindow;
            }
            else
            {
                ActivityPicturePlugin.Source.Settings.MaxImageSize = (int)PictureAlbum.MaxImageSize.NoLimit;
                pictureAlbumView.MaximumImageSize = PictureAlbum.MaxImageSize.NoLimit;
            }
            this.sliderImageSize.Value = this.sliderImageSize.Value;
        }

        private void pictureAlbumView_VideoChanged( object sender, EventArgs e )
        {
            UpdateToolBar();
        }

        private void pictureAlbumView_SelectedChanged( object sender, PictureAlbum.SelectedChangedEventArgs e )
        {
            if ( e.SelectedIndex != -1 )
            {
                // Scroll into view if it is completely or partially hidden
                Rectangle r = panelPictureAlbumView.DisplayRectangle;
                if ( ( e.Rect.Y + e.Rect.Height + r.Y ) > panelPictureAlbumView.Height )
                {
                    Point p = new Point( e.Rect.X, e.Rect.Y + e.Rect.Height - panelPictureAlbumView.Height );
                    panelPictureAlbumView.AutoScrollPosition = p;
                }
                else if ( ( e.Rect.Y + e.Rect.Height + r.Y ) < e.Rect.Height )
                {
                    Point p = new Point( e.Rect.X, e.Rect.Y );
                    panelPictureAlbumView.AutoScrollPosition = p;
                }
            }
        }

        private void panelPictureAlbumView_Click( object sender, EventArgs e )
        {
            this.pictureAlbumView.SelectedIndex = -1;
        }

        private void btnGEList_Click( object sender, EventArgs e )
        {
            //contextMenuListGE.Show( btnGEList, new Point( -( contextMenuListGE.Width + 5 ), 0 ) );
            contextMenuListGE.Show( btnGEList, new Point( 0, btnGEList.Height + 5 ) );
        }

        private void contextMenuListGE_ItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            if ( e.ClickedItem.Tag != null )
            {
                if ( !Helper.Functions.OpenExternal( e.ClickedItem.Tag.ToString() ) )
                {
                    if ( MessageBox.Show( Resources.Resources.FileNotFound_Text + ".\r\n" + Resources.Resources.RemoveFromList_Text, Resources.Resources.FileNotFound_Text,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation ) == DialogResult.Yes )
                    {
                        //Delete item from list
                        toolStripMenuRemove_Click( ( (ToolStripMenuItem)e.ClickedItem ).DropDown.Items["Remove"], new EventArgs() );
                    }
                }
            }
            contextMenuListGE.Close( ToolStripDropDownCloseReason.ItemClicked );
        }

        private void toolStripMenuRemove_Click( object sender, EventArgs e )
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if ( tsmi.Tag != null )
            {
                PluginExtensionData.GELinks.Remove( tsmi.Tag.ToString() );
                Helper.Functions.WriteExtensionData( Activity, this.PluginExtensionData );
                ReloadData();
            }
        }

        private void pictureAlbumView_MouseClick( object sender, MouseEventArgs e )
        {
            if ( e.Button == System.Windows.Forms.MouseButtons.Right )
            {
                int i = this.pictureAlbumView.SelectedIndex;
                if ( i >= 0 )
                {
                    if ( this.pictureAlbumView.ImageList[i].Type == ImageData.DataTypes.Video )
                        contextMenuPictureAlbum.Items["toolStripMenuResetSnapshot"].Visible = true;
                    else
                        contextMenuPictureAlbum.Items["toolStripMenuResetSnapshot"].Visible = false;

                    contextMenuPictureAlbum.Show( pictureAlbumView.PointToScreen( e.Location ) );
                }
            }
        }

        private void toolStripMenuResetSnapshot_Click( object sender, EventArgs e )
        {
            List<ImageData> ids = this.pictureAlbumView.ImageList;
            ImageData id = ids[this.pictureAlbumView.SelectedIndex];
            if ( id.Type == ImageData.DataTypes.Video )
            {
                id.ResetVideoThumbnail();
                Functions.ClearImageList( this.pictureAlbumView );
                ReloadData();
                UpdateView();
            }
        }

        private void toolStripMenuOpenFolder_Click( object sender, EventArgs e )
        {
            try
            {
                int ixImage = this.pictureAlbumView.SelectedIndex;
                if ( ixImage != -1 )
                {
                    string sFile = this.pictureAlbumView.ImageList[ixImage].PhotoSource;
                    System.IO.FileInfo fi = new FileInfo( sFile );

                    Functions.OpenExternal( fi.DirectoryName );
                }
            }
            catch ( System.IO.IOException )
            {
            }

        }

        private void toolStripMenuRemove_Click_1( object sender, EventArgs e )
        {
            int ixSelected = this.pictureAlbumView.SelectedIndex;
            if ( ixSelected != -1 )
            {
                if ( MessageBox.Show( Resources.Resources.ConfirmDeleteLong_Text, Resources.Resources.ConfirmDeleteShort_Text,
                     MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation ) == DialogResult.Yes )
                {
                    string sRefID = this.pictureAlbumView.ImageList[ixSelected].ReferenceID;
                    RemoveImageFromActivity( sRefID );
                }
            }
        }

        private void RemoveImageFromActivity( string referenceID )
        {
            //Delete selected images
            PluginData data = Helper.Functions.ReadExtensionData( Activity );

            foreach ( ImageDataSerializable ids in data.Images )
            {
                if ( ids.ReferenceID == referenceID )
                {
                    data.Images.Remove( ids );
                    break;
                }
            }

            Functions.WriteExtensionData( Activity, data );
            List<string> referenceIDs = new List<string>();
            referenceIDs.Add( referenceID );
            Functions.DeleteThumbnails( referenceIDs );

            ReloadData();
            UpdateView();

        }


        #endregion

    }

    public class PanelEx : ZoneFiveSoftware.Common.Visuals.Panel
    {
        public PanelEx() { }

        protected override Point ScrollToControl( Control activeControl )
        {
            // Returning the current location prevents the panel from        
            // scrolling to the active control when the panel loses and regains focus        
            return this.DisplayRectangle.Location;
        }
    }

    //public class IRouteWaypoint
    ////Dummy until available in API
    //{
    //    public enum MarkerType
    //    {
    //        Start,
    //        End,
    //        ElapsedTime,
    //        FixedDateTime,
    //        Distance,
    //        GPSLocation
    //    }
    //    public MarkerType Type { get { return MarkerType.FixedDateTime; } set { } }
    //    public TimeSpan ElapsedTime { get { return System.TimeSpan.MinValue; } set { } }
    //    public DateTime FixedDateTime { get { return System.DateTime.Now; } set { } }
    //    public double DistanceMeters { get { return 0; } set { } }
    //    public IGPSLocation GPSLocation { get { return null; } set { } }
    //    public string Notes { get { return null; } set { } }
    //    public string Description { get { return null; } set { } }
    //    public Image MarkerImage { get { return null; } set { } }
    //    public Image Photo { get { return null; } set { } }
    //    public new string ToString
    //    {
    //        get
    //        {
    //            return "Not yet implemented in API";
    //        }
    //    }
    //}
}