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
using ActivityPicturePlugin.Properties;
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
            this.m_layer = PicturesLayer.Instance((IView)view);
            this.pictureAlbumView.m_layer = this.m_layer;
            this.importControl1.m_layer = this.m_layer;
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

            ImageData.CreateImageFilesFolder();

            //read settings from logbook
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
        private List<ImageData> SelectedImages = new List<ImageData>();
        private bool PreventRowsRemoved = false;//To prevent recursive calls
        #endregion

        #region Public members
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
        public IList<IActivity> Activities
        {
            set
            {
                this.importControl1.Activities = value;
                //if activity has changed
                //if ( ( !object.ReferenceEquals( _Activity, value ) ) )
                {
                    //Update activity - only one activity right now
                    if (value == null || value.Count == 0)
                    {
                        _Activity = null;
                    }
                    else
                    {
                        _Activity = value[0];
                    }
                    if ( !m_Active )
                    {
                        ResetPage();
                    }
                    else
                    {
                        this.pictureAlbumView.ActivityChanging();
                        this.pictureAlbumView.ClearImageList();
                        this.dataGridViewImages.Visible = false;
                        //ResetPage();
                        if ( _Activity != null )
                        {
                            ReloadData();
                            UpdateView();
                        }
                        else
                        {
                            DefaultView();
                            ResetPage();
                        }
                    }
                }
            }
            //get
            //{
            //    return _Activity;
            //}
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
            toolStripMenuTypeImage.Checked = ActivityPicturePlugin.Source.Settings.CTypeImage;
            toolStripMenuExifGPS.Checked = ActivityPicturePlugin.Source.Settings.CExifGPS;
            toolStripMenuAltitude.Checked = ActivityPicturePlugin.Source.Settings.CAltitude;
            toolStripMenuComment.Checked = ActivityPicturePlugin.Source.Settings.CComment;
            toolStripMenuDateTime.Checked = ActivityPicturePlugin.Source.Settings.CDateTimeOriginal;
            toolStripMenuTitle.Checked = ActivityPicturePlugin.Source.Settings.CPhotoTitle;
            toolStripMenuCamera.Checked = ActivityPicturePlugin.Source.Settings.CCamera;
            toolStripMenuPhotoSource.Checked = ActivityPicturePlugin.Source.Settings.CPhotoSource;
            toolStripMenuReferenceID.Checked = ActivityPicturePlugin.Source.Settings.CReferenceID;

            toolStripMenuFitToWindow.Checked = ActivityPicturePlugin.Source.Settings.MaxImageSize == (int)PictureAlbum.MaxImageSize.FitToWindow;

            //TODO: Add a field to notify users which files have exif data SAVED?
            this.dataGridViewImages.Columns["cTypeImage"].Visible = ActivityPicturePlugin.Source.Settings.CTypeImage;
            this.dataGridViewImages.Columns["cExifGPS"].Visible = ActivityPicturePlugin.Source.Settings.CExifGPS;
            this.dataGridViewImages.Columns["cAltitude"].Visible = ActivityPicturePlugin.Source.Settings.CAltitude;
            this.dataGridViewImages.Columns["cComment"].Visible = ActivityPicturePlugin.Source.Settings.CComment;
            this.dataGridViewImages.Columns["cThumbnail"].Visible = ActivityPicturePlugin.Source.Settings.CThumbnail;
            this.dataGridViewImages.Columns["cDateTimeOriginal"].Visible = ActivityPicturePlugin.Source.Settings.CDateTimeOriginal;
            this.dataGridViewImages.Columns["cPhotoTitle"].Visible = ActivityPicturePlugin.Source.Settings.CPhotoTitle;
            this.dataGridViewImages.Columns["cCamera"].Visible = ActivityPicturePlugin.Source.Settings.CCamera;
            this.dataGridViewImages.Columns["cPhotoSource"].Visible = ActivityPicturePlugin.Source.Settings.CPhotoSource;
            this.dataGridViewImages.Columns["cReferenceID"].Visible = ActivityPicturePlugin.Source.Settings.CReferenceID;

            volumeSlider2.Volume = ActivityPicturePlugin.Source.Settings.VolumeValue;
        }

        public void ResetPage()
        {
            this.timerVideo.Stop();
            this.pictureAlbumView.StopVideo();
            this.pictureAlbumView.ActivityChanging();
            this.pictureAlbumView.ClearImageList();
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
                this.pictureAlbumView.ClearImageList();
                this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );
                ReloadData();
                UpdateView();
                this.dataGridViewImages.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );

                this.Invalidate();
                this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
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

            // Page may have been Reset (ResetPage) before it was shown.
            // Set _Activity again, this time with m_Active = true.
            if ( _Activity != null ) Activities = new List<IActivity> { _Activity };

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
            this.dataGridViewImages.BackgroundColor = visualTheme.Window;
            this.dataGridViewImages.GridColor = visualTheme.Border;
            this.dataGridViewImages.DefaultCellStyle.BackColor = visualTheme.Window;
            this.dataGridViewImages.RowHeadersDefaultCellStyle.BackColor = visualTheme.SubHeader;
            this.dataGridViewImages.ColumnHeadersDefaultCellStyle.BackColor = visualTheme.SubHeader;
            this.dataGridViewImages.ColumnHeadersDefaultCellStyle.ForeColor = visualTheme.SubHeaderText;

            this.dataGridViewImages.RowsDefaultCellStyle.SelectionBackColor = visualTheme.Selected;
            this.dataGridViewImages.RowsDefaultCellStyle.SelectionForeColor = visualTheme.SelectedText;

            this.dataGridViewImages.RowsDefaultCellStyle.BackColor = visualTheme.Window;
            this.dataGridViewImages.AlternatingRowsDefaultCellStyle.BackColor = GridAltRowColor(visualTheme.Window);

            this.progressBar1.BackColor = this.BackColor;
            this.progressBar1.ForeColor = visualTheme.Selected;
        }

        public void UICultureChanged( System.Globalization.CultureInfo culture )
        {
            if ( this.Mode == ShowMode.Album )
                this.actionBannerViews.Text = Resources.pictureAlbumToolStripMenuItem_Text;
            else if ( this.Mode == ShowMode.List )
                this.actionBannerViews.Text = CommonResources.Text.LabelList;
            else if ( this.Mode == ShowMode.Import )
                this.actionBannerViews.Text = CommonResources.Text.ActionImport;
            this.cAltitude.HeaderText = CommonResources.Text.LabelElevation;
            this.cComment.HeaderText = Resources.commentDataGridViewTextBoxColumn_HeaderText;
            this.cDateTimeOriginal.HeaderText = CommonResources.Text.LabelDate;
            this.cCamera.HeaderText = Resources.equipmentModelDataGridViewTextBoxColumn_HeaderText;
            // Not sure how important it is to have this capitalized, nor am I
            // sure if this should be done for all resources.
            this.cExifGPS.HeaderText = Functions.CapitalizeAllWords( CommonResources.Text.LabelGPSLocation );
            this.cPhotoSource.HeaderText = Resources.photoSourceDataGridViewTextBoxColumn_HeaderText;
            this.cReferenceID.HeaderText = Resources.referenceIDDataGridViewTextBoxColumn_HeaderText;
            this.cThumbnail.HeaderText = Resources.thumbnailDataGridViewImageColumn_HeaderText;
            this.cPhotoTitle.HeaderText = Resources.titleDataGridViewTextBoxColumn_HeaderText;
            this.pictureAlbumToolStripMenuItem.Text = Resources.pictureAlbumToolStripMenuItem_Text;
            this.pictureListToolStripMenuItem.Text = CommonResources.Text.LabelList;
            this.importToolStripMenuItem.Text = CommonResources.Text.ActionImport;
            this.waypointDataGridViewTextBoxColumn.HeaderText = Resources.waypointDataGridViewTextBoxColumn_HeaderText;
            this.groupBoxVideo.Text = Resources.groupBoxVideo_Text;
            this.groupBoxImage.Text = CommonResources.Text.LabelPhoto;
            this.groupBoxListOptions.Text = Resources.groupBoxListOptions_Text;
            this.toolStripButtonPause.ToolTipText = Resources.toolStripButtonPause_ToolTipText;
            this.toolStripButtonPlay.ToolTipText = Resources.toolStripButtonPlay_ToolTipText;
            this.toolStripButtonStop.ToolTipText = Resources.toolStripButtonStop_ToolTipText;
            this.toolStripButtonSnapshot.ToolTipText = Resources.toolStripButtonSnapshot_ToolTipText;

            this.toolTip1.SetToolTip( this.btnGeoTag, Resources.tooltip_OnlySelectedImages );
            this.toolTip1.SetToolTip( this.btnKML, Resources.tooltip_OnlySelectedImages );
            this.toolTip1.SetToolTip( this.btnTimeOffset, Resources.tooltip_OnlySelectedImages );

            this.cTypeImage.HeaderText = Resources.TypeImage_HeaderText;

            using ( Graphics g = this.CreateGraphics() )
            {
                this.btnGeoTag.Text = Resources.btnGeoTag_Text;
                this.btnGeoTag.Width = (int)g.MeasureString( this.btnGeoTag.Text, this.btnGeoTag.Font ).Width + 10; ;
                this.btnKML.Text = Resources.btnKML_Text;
                this.btnKML.Width = (int)g.MeasureString( this.btnKML.Text, this.btnKML.Font ).Width + 10;
                this.btnTimeOffset.Text = Resources.btnTimeOffset_Text;
                this.btnTimeOffset.Width = (int)g.MeasureString( this.btnTimeOffset.Text, this.btnTimeOffset.Font ).Width + 10;
                this.btnTimeOffset.Left = ( this.btnGeoTag.Right + 10 );
                this.btnKML.Left = ( this.btnTimeOffset.Right + 10 );
                this.labelImageSize.Text = Resources.labelImageSize_Text;
                this.labelImageSize.Width = (int)g.MeasureString( this.labelImageSize.Text, this.labelImageSize.Font ).Width + 10;
            }

            this.sliderImageSize.Location = new Point( this.labelImageSize.Width + this.labelImageSize.Left + 20, this.sliderImageSize.Top );
            this.groupBoxImage.Width = this.sliderImageSize.Width + this.sliderImageSize.Left + 10;
            this.groupBoxVideo.Left = this.groupBoxImage.Left + this.groupBoxImage.Width + 10;
            this.groupBoxVideo.Width = this.panelViews.Width - this.groupBoxVideo.Left - 10;

            this.toolStripMenuFitToWindow.Text = Resources.FitImagesToView_Text;

            this.toolStripMenuCopy.Text = CommonResources.Text.ActionCopy;
            this.toolStripMenuNone.Text = Resources.HideAllColumns_Text;
            this.toolStripMenuAll.Text = Resources.ShowAllColumns_Text;
            this.toolStripMenuTypeImage.Text = Resources.TypeImage_HeaderText;
            this.toolStripMenuExifGPS.Text = CommonResources.Text.LabelGPSLocation;
            this.toolStripMenuAltitude.Text = CommonResources.Text.LabelElevation;
            this.toolStripMenuComment.Text = Resources.commentDataGridViewTextBoxColumn_HeaderText;
            this.toolStripMenuTitle.Text = Resources.titleDataGridViewTextBoxColumn_HeaderText;
            //this.toolStripMenuThumbnail.Text = Resources.thumbnailDataGridViewImageColumn_HeaderText;
            this.toolStripMenuDateTime.Text = CommonResources.Text.LabelDate;
            this.toolStripMenuCamera.Text = Resources.equipmentModelDataGridViewTextBoxColumn_HeaderText;
            this.toolStripMenuPhotoSource.Text = Resources.photoSourceDataGridViewTextBoxColumn_HeaderText;
            this.toolStripMenuReferenceID.Text = Resources.referenceIDDataGridViewTextBoxColumn_HeaderText;

            this.toolStripMenuResetSnapshot.Text = Resources.ResetSnapshot_Text;
            this.toolStripMenuOpenFolder.Text = Resources.OpenContainingFolder_Text;
            this.toolStripMenuRemove.Text = CommonResources.Text.ActionRemove;

            this.importControl1.UpdateUICulture( culture );
        }

        // Cleans up the thumbnails that were created but not saved during the last session
        public void CleanupThumbnails()
        {
            string s = ActivityPicturePlugin.Source.Settings.NewThumbnailsCreated;
            //Set to null immediately, to avoid that cleanup is started from ReportView too
            ActivityPicturePlugin.Source.Settings.NewThumbnailsCreated = "";
            if (s != "")
            {
                Thread thread = new Thread( new ParameterizedThreadStart( RunCleanup ) );
                thread.Start( s );
            }
        }

        private void DefaultView()
        {
            if ( _Activity != null )
            {
                if ( this.PluginExtensionData.Images.Count == 0 )
                {
                    this.contextMenuStripView.Enabled = false;
                    this.Mode = ShowMode.Import;
                }
                else
                {
                    this.contextMenuStripView.Enabled = true; 
                    this.Mode = (ShowMode)ActivityPicturePlugin.Source.Settings.ActivityMode;
                }
            }
            else
            {
                this.Visible = false;
                this.contextMenuStripView.Enabled = false;
                this.Mode = ShowMode.Import;
            }
        }

        private void RunCleanup( object sNewThumbs )
        {
            string s = sNewThumbs as string;
            string[] sFiles = s.Split( '\t' );
            List<string> thumbNails = Functions.GetThumbnailPathsAllActivities();

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
                    if (!bFound && System.IO.File.Exists(fi.FullName))
                    {
                        fi.Delete();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                }
            }
        }

        private void UpdateView()
        {
            try
            {
                DefaultView();
                //this.contextMenuStripView.Enabled = true;
                this.dataGridViewImages.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );

                //Load controls depending on selected view
                if ( this.Mode == ShowMode.Album )
                {
                    this.actionBannerViews.Text = Resources.pictureAlbumToolStripMenuItem_Text;
                    this.groupBoxVideo.Enabled = true;
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
                    this.importControl1.LoadActivityNodes();

                    // LoadActivityNodes() is a long task which calls DoEvents.
                    // We need to check to see if anything has occurred to
                    // prevent us from continuing.
                    if ( this.Mode != ShowMode.Import )
                        throw new ActivityPicturePageControlException( ActivityPicturePageControlException.Error_ShowModeChanged );
                    this.importControl1.Visible = true;
                }

                this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this.dataGridViewImages_CellValueChanged );
            }
            catch ( ActivityPicturePageControlException ex )
            {
                System.Diagnostics.Debug.Print( ex.Message );
            }
            catch ( ImportControl.ImportControlException ex )
            {
                System.Diagnostics.Debug.Print( ex.Message );
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.Assert( false, ex.Message );
            }
        }

        [Serializable()]
        public class ActivityPicturePageControlException : Exception
        {
            public static readonly string Error_ShowModeChanged = Properties.Resources.ViewChanged_Text;

            public ActivityPicturePageControlException() : base() { }
            public ActivityPicturePageControlException( string message ) : base( message ) { }
            public ActivityPicturePageControlException( string message, Exception inner ) : base( message, inner ) { }

            // A constructor is needed for serialization when an
            // exception propagates from a remoting server to the client.
            protected ActivityPicturePageControlException( System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context ) { }
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
                    this.Visible = _showPage;

                    //Read data and add new controls
                    this.PluginExtensionData = Helper.Functions.ReadExtensionData( _Activity );
                    if ( ( this.PluginExtensionData.Images.Count != 0 ) ||
                        this.pictureAlbumView.ImageList == null ||
                        ( this.PluginExtensionData.Images.Count != this.pictureAlbumView.ImageList.Count ) )
                    {
                        this.pictureAlbumView.ImageList = this.PluginExtensionData.LoadImageData( this.PluginExtensionData.Images, _Activity );
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
                    this.m_layer.HidePage(); //defer updates
                    this.m_layer.PictureSize = this.sliderImageSize.Value;
                    this.m_layer.Pictures = this.pictureAlbumView.ImageList;
                    this.m_layer.ShowPage("");//Refresh
#endif
                }
                else
                {
                    this.Visible = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                if (tsmiRemove != null)
                    tsmiRemove.Dispose();
                tsmiRemove = null;
            }
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

                //Could be done with format updater too...
                //Exif GPS
                if (this.dataGridViewImages.Columns[this.cExifGPS.DisplayIndex].Visible)
                {
                    for (int i = 0; i < this.dataGridViewImages.Rows.Count; i++)
                    {
                        if (!this.pictureAlbumView.ImageList[this.dataGridViewImages.Rows[i].Index].HasExifGps)
                        {
                            Font f2 = this.dataGridViewImages.Rows[i].Cells[this.cExifGPS.DisplayIndex].Style.Font;
                            if (f2 == null)
                            {
                                f2 = System.Windows.Forms.DataGridView.DefaultFont;
                            }
                            this.dataGridViewImages.Rows[i].Cells[this.cExifGPS.DisplayIndex].Style.Font =
                                new System.Drawing.Font(f2, System.Drawing.FontStyle.Italic);
                        }
                        else
                        {
                            this.dataGridViewImages.Rows[i].Cells[this.cExifGPS.DisplayIndex].Style.Font = null;
                        }
                    }

                }

                //Original
                if (this.dataGridViewImages.Columns[this.cPhotoSource.DisplayIndex].Visible)
                {
                    for (int i = 0; i < this.dataGridViewImages.Rows.Count; i++)
                    {
                        String filePath = this.pictureAlbumView.ImageList[this.dataGridViewImages.Rows[i].Index].PhotoSource;
                        if (!(new System.IO.FileInfo(filePath)).Exists)
                        {
                            this.dataGridViewImages.Rows[i].Cells[this.cPhotoSource.DisplayIndex].Style.BackColor = Color.Red;
                        }
                    }
                }

                this.dataGridViewImages.Invalidate();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
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
            PictureAlbum.ImageSortMode ism = (PictureAlbum.ImageSortMode)ActivityPicturePlugin.Source.Settings.SortMode;
            DataGridViewColumn col = GetSortColumn( ism );
            if (this.pictureAlbumView.ImageList != null)
            {
                this.pictureAlbumView.ImageList.Sort();
            }
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

        #endregion

        #region Event handler methods
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

                        // Save the referenceid of the selected row
                        List<string> listSelected = GetDataGridViewImagesSelectedRefIDs();
                        ReloadData();
                        SetDataGridViewSelectedRows( listSelected );
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
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
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
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
                    foreach (ImageData im in this.SelectedImages)
                    {
                        im.DeleteThumbnail();
                    }
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

                if ( ( col.ValueType == typeof( string ) ) ||
                    ( col.ValueType == typeof( DateTime ) ) )
                {
                    ActivityPicturePlugin.Source.Settings.SortMode = (int)GetSortMode( col );

                    // Get a list of selected rows
                    DataGridViewSelectedRowCollection selRows = dataGridViewImages.SelectedRows;
                    string[] srefs = new string[selRows.Count];
                    for ( int i = 0; i < selRows.Count; i++ )
                        srefs[i] = selRows[i].Cells["cReferenceID"].Value.ToString();

                    if ( ActivityPicturePlugin.Source.Settings.SortMode != (int)PictureAlbum.ImageSortMode.none )
                    {
                        this.pictureAlbumView.ImageList.Sort();
                        UpdateDataGridView();
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


            catch ( Exception ex )
            {
                System.Diagnostics.Debug.Assert( false, ex.Message );
            }
        }

        private void pictureListToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if ( this.Mode == ShowMode.Import )
            {
                this.pictureAlbumView.ClearImageList();
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
                this.pictureAlbumView.ClearImageList();
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
                this.ThemeChanged( m_theme );
                InitializeDataGridView();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
        }

        private void dataGridViewImages_SelectionChanged( object sender, EventArgs e )
        {
            try
            {
                SelectedImages.Clear();
                DataGridViewSelectedRowCollection SelRows = ( (DataGridView)( sender ) ).SelectedRows;
                for ( int i = 0; i < SelRows.Count; i++ )
                {
                    ImageData im = this.pictureAlbumView.ImageList[SelRows[i].Index];
                    SelectedImages.Add(im);
               }
                //TODO: Or use zoom button
#if !ST_2_1
                m_layer.SelectedPictures = SelectedImages;
#endif
                SelRows = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
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
            if ( this.Mode == ShowMode.Album && sender == sliderImageSize )
            {
                this.pictureAlbumView.Zoom = this.sliderImageSize.Value;

                ActivityPicturePlugin.Source.Settings.ImageZoom = this.sliderImageSize.Value;
                this.PluginExtensionData.ImageZoom = this.sliderImageSize.Value;
                Helper.Functions.WriteExtensionData( _Activity, this.PluginExtensionData );

                this.pictureAlbumView.Invalidate();
#if !ST_2_1
                //Race condition?
                if (this.m_layer != null)
                {
                    this.m_layer.PictureSize = this.sliderImageSize.Value;
                    this.m_layer.Refresh();
                }
#endif
            }
        }

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

                this.pictureAlbumView.ClearImageList();

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
                if ( ( id.Type == ImageData.DataTypes.Image ) || ( id.Type == ImageData.DataTypes.Video ) )
                {
                    //This writes the exif data if exists, otherwise estimates from activity
                    // TODO: Currently there's no way to change GPS data once it's been saved.
                    //       Changing DateTimeOriginal is not sufficient. ie ModifyTimeStamp, TimeOffset.
                    if ( id.HasGps() )
                    {
                        // Flush the Gps point.  We're overriding previous detection mechanisms.
                        id.FlushGpsPoint(); // May not be the best way to go about it.
                        if ( Functions.IsExifFileExt( new FileInfo( id.PhotoSource ) ) )
                            if ( System.IO.File.Exists( id.PhotoSource ) ) Functions.GeoTagFromGps( id.PhotoSource, id.GpsPoint );
                        if ( System.IO.File.Exists( id.ThumbnailPath ) ) Functions.GeoTagFromGps( id.ThumbnailPath, id.GpsPoint );
                    }
                }
            }

            // Save the referenceid of the selected row
            List<string> listSelected = GetDataGridViewImagesSelectedRefIDs();
            
            this.pictureAlbumView.ClearImageList();
            ReloadData();

            //UpdateView();     //Needed?

            SetDataGridViewSelectedRows( listSelected );

            this.UseWaitCursor = false;
            Cursor.Position = Cursor.Position;  // Trigger cursor update: UseWaitCursor = false
            dataGridViewImages.Enabled = true;
            btnGeoTag.Enabled = true;
            btnKML.Enabled = true;
            btnTimeOffset.Enabled = true;
        }

        private List<string> GetDataGridViewImagesSelectedRefIDs()
        {
            List<string> listSelected = new List<string>();
            foreach ( DataGridViewRow row in this.dataGridViewImages.SelectedRows )
                listSelected.Add( row.Cells["cReferenceID"].Value as string );
            return listSelected;
        }

        private void SetDataGridViewSelectedRows( List<string> RefIds )
        {
            // Deselect the automatically selected row
            if ( this.dataGridViewImages.SelectedRows.Count == 1 )
                this.dataGridViewImages.SelectedRows[0].Selected = false;

            // Reselect the previously selected rows
            int nFirstIndex = int.MaxValue; // Index of first selected item
            foreach ( DataGridViewRow row in this.dataGridViewImages.Rows )
            {
                foreach ( string sRefId in RefIds )
                {
                    if ( sRefId == row.Cells["cReferenceID"].Value as string )
                    {
                        row.Selected = true;
                        nFirstIndex = row.Index < nFirstIndex ? row.Index : nFirstIndex;
                        RefIds.Remove( sRefId );
                        break;
                    }
                }
            }

            // Scroll to the first selected item.
            if ( nFirstIndex != int.MaxValue )
                this.dataGridViewImages.FirstDisplayedScrollingRowIndex = nFirstIndex;

        }

        private void btnKML_Click( object sender, EventArgs e )
        {
            this.saveFileDialog.FileName = "";
            this.saveFileDialog.DefaultExt = "kmz";
            this.saveFileDialog.AddExtension = true;
            this.saveFileDialog.CheckPathExists = true;
            this.saveFileDialog.Filter = "Google Earth compressed (*.kmz)|*.kmz|Google Earth KML (*.kml)|*.kml";
            this.saveFileDialog.InitialDirectory = ActivityPicturePlugin.Source.Settings.LastGeDirectory;
            DialogResult dres = this.saveFileDialog.ShowDialog();
            if ( dres == DialogResult.OK & this.saveFileDialog.FileName != "" )
            {
                ActivityPicturePlugin.Source.Settings.LastGeDirectory = (new System.IO.FileInfo(this.saveFileDialog.FileName)).DirectoryName;
                Functions.PerformExportToGoogleEarth( GetSelectedImageData(), this._Activity, this.saveFileDialog.FileName );
                if ( ActivityPicturePlugin.Source.Settings.GEAutoOpen )
                    Functions.OpenExternal( this.saveFileDialog.FileName );

                if ( ActivityPicturePlugin.Source.Settings.GEStoreFileLocation )
                {
                    if ( !PluginExtensionData.GELinks.Contains( this.saveFileDialog.FileName ) )
                    {
                        PluginExtensionData.GELinks.Add( this.saveFileDialog.FileName );
                        Helper.Functions.WriteExtensionData( _Activity, this.PluginExtensionData );
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

                List<string> listSelected = GetDataGridViewImagesSelectedRefIDs();
                ReloadData();
                SetDataGridViewSelectedRows( listSelected );
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
            toolStripMenuDateTime.Checked = false;
            toolStripMenuTitle.Checked = false;
            toolStripMenuCamera.Checked = false;
            toolStripMenuPhotoSource.Checked = false;
            toolStripMenuReferenceID.Checked = false;
        }

        private void toolStripMenuExifGps_Click(object sender, EventArgs e)
        {
            //List<ImageData> iList = GetSelectedImageData();
            //foreach (ImageData id in iList)
            //{
            //    if (id.HasExifGps)
            //    {
            //    }
            //}
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
                    ss = ss.Replace(System.Environment.NewLine, " " );

                    s.Append( ss + "\t" );
                }
            }
            s.Append(System.Environment.NewLine);
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
                            ss = ss.Replace(System.Environment.NewLine, " " );

                            s.Append( ss + "\t" );
                        }
                    }
                    s.Append(System.Environment.NewLine);
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
            contextMenuListGE.Show( btnGEList, new Point( 0, btnGEList.Height + 5 ) );
        }

        private void contextMenuListGE_ItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            if ( e.ClickedItem.Tag != null )
            {
                if ( !Helper.Functions.OpenExternal( e.ClickedItem.Tag.ToString() ) )
                {
                    if ( MessageBox.Show( Resources.FileNotFound_Text + ".\r\n" + Resources.RemoveFromList_Text, Resources.FileNotFound_Text,
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
                Helper.Functions.WriteExtensionData( _Activity, this.PluginExtensionData );
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
                this.pictureAlbumView.ClearImageList();
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
                if ( MessageBox.Show( Resources.ConfirmDeleteLong_Text, Resources.ConfirmDeleteShort_Text,
                     MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation ) == DialogResult.Yes )
                {
                    RemoveImageFromActivity(this.pictureAlbumView.ImageList[ixSelected]);
                }
            }
        }

        private void RemoveImageFromActivity(ImageData im)
        {
            //Delete selected images
            PluginData data = Helper.Functions.ReadExtensionData(_Activity);

            ImageDataSerializable ids = im.GetSerialzable(data.Images);
            if (ids != null)
            {
                data.Images.Remove(ids);
            }

            Functions.WriteExtensionData(_Activity, data);

            ReloadData();
            UpdateView();

            //This must be done after view is reloaded (image locked)
            im.DeleteThumbnail();
        }

        private void importControl1_ActivityImagesChanged( object sender, ImportControl.ActivityImagesChangedEventArgs e )
        {
            if ( ( e.Items != null ) && ( e.Items.Count > 0 ) )
                this.contextMenuStripView.Enabled = true;
            else
                this.contextMenuStripView.Enabled = false;
        }

        private void panelViews_Resize( object sender, EventArgs e )
        {
            // Not doing anything here but we may need to.
        }

        private void dataGridViewImages_KeyDown( object sender, KeyEventArgs e )
        {
            switch ( e.KeyCode )
            {
                case Keys.Delete:
                    if ( MessageBox.Show( Resources.ConfirmDeleteLong_Text, Resources.ConfirmDeleteShort_Text,
                         MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation ) == DialogResult.Yes )
                    {
                        foreach ( DataGridViewRow row in dataGridViewImages.SelectedRows )
                            dataGridViewImages.Rows.Remove( row );
                    }
                    break;
            }
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