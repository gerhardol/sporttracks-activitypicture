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
//using QuartzTypeLib;

//todo:
//2. When doubleclicking the pictures in GE, the pictures are gray. Minatures are fine, so are pictures extracted from the .kmz 
//4. Can this be the default in List view too? I can sort the pictures, but if I do something with them, the default order is actived again. 

namespace ActivityPicturePlugin.UI.Activities
{
    public partial class ActivityPicturePageControl : UserControl
    {
        public ActivityPicturePageControl()
        {
            InitializeComponent();

            //Create directory if it does not already exist!
            if (!Directory.Exists(ImageFilesFolder)) Directory.CreateDirectory(ImageFilesFolder);

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

        private List<string> SelectedReferenceIDs = new List<string>();
        private bool PreventRowsRemoved = false;//To prevent recursive calls
        private bool PreventCellValueChanged = false;
        #endregion

        #region Public members
        //TODO: GetFullPath required due to relative paths
        public static string ImageFilesFolder = System.IO.Path.GetFullPath(ActivityPicturePlugin.Plugin.GetIApplication().
#if ST_2_1
            //TODO:
            SystemPreferences.WebFilesFolder+"\\Images\\";
#else
Configuration.CommonWebFilesFolder + "\\..\\..\\2.0\\Web Files\\Images\\");
       // + GUIDs.PluginMain.ToString() + Path.DirectorySeparatorChar);
#endif

        public static PluginSettings PluginSettingsData = new PluginSettings();
        //public List<ImageData> Images = new System.Collections.Generic.List<ImageData>();
        #endregion

        #region Public properties
        public IActivity Activity
        {
            set
            {
                //if activity has changed
                if ((!object.ReferenceEquals(_Activity, value)))
                {
                    //Update activity
                    _Activity = value;
                    this.pictureAlbumView.ActivityChanging();
                    //this.dataGridViewImages.Visible = false;
                    Functions.ClearImageList(this.pictureAlbumView);
                    ReloadData();
                    UpdateView();
                }
            }
            get
            {
                return _Activity;
            }
        }
        #endregion

        #region Helper Methods


        public void RefreshPage()
        {
            try
            {
                this.dataGridViewImages.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellValueChanged);
                Functions.ClearImageList(this.pictureAlbumView);
                this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellValueChanged);
                ReloadData();
                UpdateView();
                this.dataGridViewImages.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellValueChanged);
                if (this.Mode == ShowMode.Album)
                {
                    this.actionBannerViews.Text = Resources.Resources.ResourceManager.GetString("pictureAlbumToolStripMenuItem_Text");
                }
                else if (this.Mode == ShowMode.List)
                {
                    this.actionBannerViews.Text = Resources.Resources.ResourceManager.GetString("pictureListToolStripMenuItem_Text");
                }
                else if (this.Mode == ShowMode.Import)
                {
                    this.actionBannerViews.Text = Resources.Resources.ResourceManager.GetString("btnManImp_Text");
                }
                this.Altitude.HeaderText = Resources.Resources.ResourceManager.GetString("Altitude_HeaderText");
                this.commentDataGridViewTextBoxColumn.HeaderText = Resources.Resources.ResourceManager.GetString("commentDataGridViewTextBoxColumn_HeaderText");
                this.dateTimeOriginalDataGridViewTextBoxColumn.HeaderText = Resources.Resources.ResourceManager.GetString("dateTimeOriginalDataGridViewTextBoxColumn_HeaderText");
                this.equipmentModelDataGridViewTextBoxColumn.HeaderText = Resources.Resources.ResourceManager.GetString("equipmentModelDataGridViewTextBoxColumn_HeaderText");
                this.ExifGPS.HeaderText = Resources.Resources.ResourceManager.GetString("ExifGPS_HeaderText");
                this.photoSourceDataGridViewTextBoxColumn.HeaderText = Resources.Resources.ResourceManager.GetString("photoSourceDataGridViewTextBoxColumn_HeaderText");
                this.referenceIDDataGridViewTextBoxColumn.HeaderText = Resources.Resources.ResourceManager.GetString("referenceIDDataGridViewTextBoxColumn_HeaderText");
                this.thumbnailDataGridViewImageColumn.HeaderText = Resources.Resources.ResourceManager.GetString("thumbnailDataGridViewImageColumn_HeaderText");
                this.titleDataGridViewTextBoxColumn.HeaderText = Resources.Resources.ResourceManager.GetString("titleDataGridViewTextBoxColumn_HeaderText");
                this.pictureAlbumToolStripMenuItem.Text = Resources.Resources.ResourceManager.GetString("pictureAlbumToolStripMenuItem_Text");
                this.pictureListToolStripMenuItem.Text = Resources.Resources.ResourceManager.GetString("pictureListToolStripMenuItem_Text");
                this.importToolStripMenuItem.Text = Resources.Resources.ResourceManager.GetString("btnManImp_Text");
                this.waypointDataGridViewTextBoxColumn.HeaderText = Resources.Resources.ResourceManager.GetString("waypointDataGridViewTextBoxColumn_HeaderText");
                this.groupBoxVideo.Text = Resources.Resources.ResourceManager.GetString("groupBoxVideo_Text");
                this.groupBoxImage.Text = Resources.Resources.ResourceManager.GetString("groupBoxImage_Text");
                this.groupBoxListOptions.Text = Resources.Resources.ResourceManager.GetString("goupBoxListOptions_Text");
                this.toolStripButtonPause.ToolTipText = Resources.Resources.ResourceManager.GetString("toolStripButtonPause_ToolTipText");
                this.toolStripButtonPlay.ToolTipText = Resources.Resources.ResourceManager.GetString("toolStripButtonPlay_ToolTipText");
                this.toolStripButtonStop.ToolTipText = Resources.Resources.ResourceManager.GetString("toolStripButtonStop_ToolTipText");

                this.toolTip1.SetToolTip(this.btnGeoTag, Resources.Resources.ResourceManager.GetString("tooltip_OnlySelectedImages"));
                this.toolTip1.SetToolTip(this.btnKML, Resources.Resources.ResourceManager.GetString("tooltip_OnlySelectedImages"));
                this.toolTip1.SetToolTip(this.btnTimeOffset, Resources.Resources.ResourceManager.GetString("tooltip_OnlySelectedImages"));

                this.TypeImage.HeaderText = Resources.Resources.ResourceManager.GetString("TypeImage_HeaderText");
                this.btnGeoTag.Text = Resources.Resources.ResourceManager.GetString("btnGeoTag_Text");
                this.btnKML.Text = Resources.Resources.ResourceManager.GetString("btnKML_Text");
                this.btnTimeOffset.Text = Resources.Resources.ResourceManager.GetString("btnTimeOffset_Text");
                this.labelImageSize.Text = Resources.Resources.ResourceManager.GetString("labelImageSize_Text");

                this.Invalidate();
                this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellValueChanged);
            }
            catch (Exception)
            {
                //throw;
            }
        }
        ZoneFiveSoftware.Common.Visuals.ITheme m_theme;
        public void ThemeChanged(ZoneFiveSoftware.Common.Visuals.ITheme visualTheme)
        {
            m_theme = visualTheme;
            //change theme colors
            //labelImageSize.ForeColor = visualTheme.ControlText;
            //this.trackBar1.BackColor = visualTheme.Window;

            this.Invalidate();
 
            this.BackColor = visualTheme.Control;
            this.actionBannerViews.ThemeChanged(visualTheme);
            //this.actionBannerViews.BackColor = Theme.SubHeader;
            this.panelViews.ThemeChanged(visualTheme);
            this.panelViews.HeadingBackColor = visualTheme.Control;
            //this.panelViews.Border = ControlBorder.Style.SmallRoundShadow;
            //this.panelViews.BorderColor = Theme.Border;
            //this.panelViews.BorderShadowColor = System.Drawing.Color.Transparent;

            this.importControl1.ThemeChanged(visualTheme);
            this.pictureAlbumView.ThemeChanged(visualTheme);
            this.groupBoxListOptions.BackColor = visualTheme.Control;
            this.groupBoxListOptions.ForeColor = visualTheme.ControlText;

            this.groupBoxVideo.BackColor = visualTheme.Control;
            this.groupBoxVideo.ForeColor = visualTheme.ControlText;
            this.trackBarVideo.BackColor = visualTheme.Control;
            this.toolStripVideo.BackColor = visualTheme.Control;
            this.volumeSlider2.ThemeChanged(visualTheme);

            this.groupBoxImage.BackColor = visualTheme.Control;
            this.groupBoxImage.ForeColor = visualTheme.ControlText;
            this.trackBarImageSize.BackColor = visualTheme.Control;

            this.dataGridViewImages.ForeColor = visualTheme.ControlText;
            this.dataGridViewImages.BackgroundColor = visualTheme.Control;
            this.dataGridViewImages.GridColor = visualTheme.Border;
            this.dataGridViewImages.DefaultCellStyle.BackColor = visualTheme.Window;
            this.dataGridViewImages.RowHeadersDefaultCellStyle.BackColor = visualTheme.SubHeader;
            this.dataGridViewImages.ColumnHeadersDefaultCellStyle.BackColor = visualTheme.SubHeader;
            this.dataGridViewImages.ColumnHeadersDefaultCellStyle.ForeColor = visualTheme.SubHeaderText;
        }
        public void UICultureChanged(System.Globalization.CultureInfo culture)
        {
            //change number formats
            RefreshPage();
            this.importControl1.UpdateUICulture(culture);
        }
        private void UpdateView()
        {
            try
            {
                this.dataGridViewImages.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellValueChanged);
                //Load controls depending on selected view
                if (this.Mode == ShowMode.Album)
                {                  
                    this.actionBannerViews.Text = Resources.Resources.ResourceManager.GetString("pictureAlbumToolStripMenuItem_Text"); ;
                    this.groupBoxImage.Visible = true;
                    this.groupBoxVideo.Visible = true;
                    this.groupBoxListOptions.Visible = false;
                    this.dataGridViewImages.Visible = false;
                    this.pictureAlbumView.Visible = true;
                    this.importControl1.Visible = false;
                    this.pictureAlbumView.Invalidate();
                }
                else if (this.Mode == ShowMode.List)
                {
                    this.actionBannerViews.Text = Resources.Resources.ResourceManager.GetString("pictureListToolStripMenuItem_Text"); ;
                    this.groupBoxImage.Visible = false;
                    this.groupBoxVideo.Visible = false;
                    this.groupBoxListOptions.Visible = true;
                    this.dataGridViewImages.Visible = true;
                    this.pictureAlbumView.Visible = false;
                    this.importControl1.Visible = false;
                    UpdateDataGridView();
                }
                else if (this.Mode == ShowMode.Import)
                {
                    this.actionBannerViews.Text = Resources.Resources.ResourceManager.GetString("btnManImp_Text");
                    this.dataGridViewImages.Visible = false;
                    this.pictureAlbumView.Visible = false;
                    this.groupBoxImage.Visible = false;
                    this.groupBoxVideo.Visible = false;
                    this.groupBoxListOptions.Visible = false;
                    this.importControl1.LoadNodes();
                    this.importControl1.Visible = true;
                }

                this.groupBoxVideo.Enabled = false;
                this.pictureAlbumView.StopVideo();
                this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellValueChanged);
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void ReloadData()
        {
            try
            {
                if (_Activity != null)
                {
                    this.dataGridViewImages.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellValueChanged);
                    this.Enabled = true;

                    //Read data and add new controls
                    this.PluginExtensionData = Helper.Functions.ReadExtensionData(_Activity);
                    if (this.PluginExtensionData.Images.Count != 0)
                    {
                        this.pictureAlbumView.ImageList = this.PluginExtensionData.LoadImageData(this.PluginExtensionData.Images);
                        SortListView();
                    }
                    this.dataGridViewImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImages_CellValueChanged);
                }
                else
                {
                    this.Enabled = false;
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void UpdateDataGridView()
        {
            try
            {
                PreventRowsRemoved = true;
                this.bindingSourceImageList.DataSource = this.pictureAlbumView.ImageList;
                this.bindingSourceImageList.ResetBindings(false);
                PreventRowsRemoved = false;
                this.dataGridViewImages.Invalidate();
            }
            catch (Exception)
            {
                //throw;
            }
        }

        #endregion

        #region Event handler methods
        void dataGridViewImages_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }
        void dataGridViewImages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataGridViewImages.Columns[e.ColumnIndex].Name == this.photoSourceDataGridViewTextBoxColumn.Name
                 || this.dataGridViewImages.Columns[e.ColumnIndex].Name == this.thumbnailDataGridViewImageColumn.Name
                 || this.dataGridViewImages.Columns[e.ColumnIndex].Name == this.TypeImage.Name)
                {
                    //open the image/video in external window
                    if (this.pictureAlbumView.ImageList[e.RowIndex].Type == ImageData.DataTypes.Image)
                    {
                        Helper.Functions.OpenImage(this.pictureAlbumView.ImageList[e.RowIndex].PhotoSource, this.pictureAlbumView.ImageList[e.RowIndex].ReferenceIDPath);
                    }
                    else if (this.pictureAlbumView.ImageList[e.RowIndex].Type == ImageData.DataTypes.Video)
                    {
                        Functions.OpenVideoInExternalWindow(this.pictureAlbumView.ImageList[e.RowIndex].PhotoSource);
                    }
                }
                else if (this.dataGridViewImages.Columns[e.ColumnIndex].Name == this.dateTimeOriginalDataGridViewTextBoxColumn.Name)
                {
                    //set the time stamp
                    ModifyTimeStamp frm = new ModifyTimeStamp(this.pictureAlbumView.ImageList[e.RowIndex]);
                    frm.ThemeChanged(m_theme);
                    frm.ShowDialog();
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        void dataGridViewImages_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!PreventCellValueChanged)
            {
                 PreventCellValueChanged = true;
                 try
                 {
                    if (this._Activity != null)
                    {
                        this.PluginExtensionData.GetImageDataSerializable(this.pictureAlbumView.ImageList);
                        Functions.WriteExtensionData(_Activity, this.PluginExtensionData);
                    }
                 }
                 catch (Exception)
                 {
                //throw;
                 }
                 PreventCellValueChanged = false;
            }
        }
        void dataGridViewImages_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (!PreventRowsRemoved)
            {
                if (this._Activity != null)
                {
                    // remove thumbnail image in web folder
                    Functions.DeleteThumbnails(this.SelectedReferenceIDs);
                    this.PluginExtensionData.GetImageDataSerializable(this.pictureAlbumView.ImageList);
                    Functions.WriteExtensionData(_Activity, this.PluginExtensionData);
                }
            }
        }
        private void dataGridViewImages_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //not possible for bound data
            //dataGridViewImages.Sort(this.dateTimeOriginalDataGridViewTextBoxColumn, ListSortDirection.Ascending);
            try
            {
                PictureAlbum.ImageSortMode oldSortMode = ActivityPicturePageControl.PluginSettingsData.data.SortMode;
                DataGridViewColumn oldcol = GetSortColumn(oldSortMode);


                DataGridViewColumn col = this.dataGridViewImages.Columns[e.ColumnIndex];
                ActivityPicturePageControl.PluginSettingsData.data.SortMode = GetSortMode(col);

                if (ActivityPicturePageControl.PluginSettingsData.data.SortMode != PictureAlbum.ImageSortMode.none)
                {
                    if (oldcol != null) oldcol.HeaderCell.Style.BackColor = this.dataGridViewImages.ColumnHeadersDefaultCellStyle.BackColor;
                    col.HeaderCell.Style.BackColor = m_theme.MainHeader;
                    this.pictureAlbumView.ImageList.Sort(CompareImageData);
                    UpdateDataGridView();
                    ActivityPicturePageControl.PluginSettingsData.WriteSettings();
                }
                else
                {
                    ActivityPicturePageControl.PluginSettingsData.data.SortMode = oldSortMode;
                }
            }


            catch (Exception)
            {
                // throw;
            }
        }

        private DataGridViewColumn GetSortColumn(PictureAlbum.ImageSortMode imageSortMode)
        {
            switch (imageSortMode)
            {
                case PictureAlbum.ImageSortMode.byAltitudeAscending:
                case PictureAlbum.ImageSortMode.byAltitudeDescending:
                    return this.Altitude;
                case PictureAlbum.ImageSortMode.byCameraModelAscending:
                case PictureAlbum.ImageSortMode.byCameraModelDescending:
                    return this.equipmentModelDataGridViewTextBoxColumn;
                case PictureAlbum.ImageSortMode.byCommentAscending:
                case PictureAlbum.ImageSortMode.byCommentDescending:
                    return this.commentDataGridViewTextBoxColumn;
                case PictureAlbum.ImageSortMode.byDateTimeAscending:
                case PictureAlbum.ImageSortMode.byDateTimeDescending:
                    return this.dateTimeOriginalDataGridViewTextBoxColumn;
                case PictureAlbum.ImageSortMode.byExifGPSAscending:
                case PictureAlbum.ImageSortMode.byExifGPSDescending:
                    return this.ExifGPS;
                case PictureAlbum.ImageSortMode.byPhotoSourceAscending:
                case PictureAlbum.ImageSortMode.byPhotoSourceDescending:
                    return this.photoSourceDataGridViewTextBoxColumn;
                case PictureAlbum.ImageSortMode.byTitleAscending:
                case PictureAlbum.ImageSortMode.byTitleDescending:
                    return this.titleDataGridViewTextBoxColumn;
                case PictureAlbum.ImageSortMode.byTypeAscending:
                case PictureAlbum.ImageSortMode.byTypeDescending:
                    return this.TypeImage;
                case PictureAlbum.ImageSortMode.none:
                default:
                    return null;
            }
        }

        private PictureAlbum.ImageSortMode GetSortMode(DataGridViewColumn col)
        {
            if (col == this.dateTimeOriginalDataGridViewTextBoxColumn)
            {
                if (ActivityPicturePageControl.PluginSettingsData.data.SortMode == PictureAlbum.ImageSortMode.byDateTimeAscending)
                {
                    return PictureAlbum.ImageSortMode.byDateTimeDescending;
                }
                else
                {
                    return PictureAlbum.ImageSortMode.byDateTimeAscending;
                }
            }
            else if (col == this.ExifGPS)
            {
                if (ActivityPicturePageControl.PluginSettingsData.data.SortMode == PictureAlbum.ImageSortMode.byExifGPSAscending)
                {
                    return PictureAlbum.ImageSortMode.byExifGPSDescending;
                }
                else
                {
                    return PictureAlbum.ImageSortMode.byExifGPSAscending;
                }
            }
            else if (col == this.Altitude)
            {
                if (ActivityPicturePageControl.PluginSettingsData.data.SortMode == PictureAlbum.ImageSortMode.byAltitudeAscending)
                {
                    return PictureAlbum.ImageSortMode.byAltitudeDescending;
                }
                else
                {
                    return PictureAlbum.ImageSortMode.byAltitudeAscending;
                }
            }
            else if (col == this.equipmentModelDataGridViewTextBoxColumn)
            {
                if (ActivityPicturePageControl.PluginSettingsData.data.SortMode == PictureAlbum.ImageSortMode.byCameraModelAscending)
                {
                    return PictureAlbum.ImageSortMode.byCameraModelDescending;
                }
                else
                {
                    return PictureAlbum.ImageSortMode.byCameraModelAscending;
                }
            }
            else if (col == this.titleDataGridViewTextBoxColumn)
            {
                if (ActivityPicturePageControl.PluginSettingsData.data.SortMode == PictureAlbum.ImageSortMode.byTitleAscending)
                {
                    return PictureAlbum.ImageSortMode.byTitleDescending;
                }
                else
                {
                    return PictureAlbum.ImageSortMode.byTitleAscending;
                }
            }
            else if (col == this.photoSourceDataGridViewTextBoxColumn)
            {
                if (ActivityPicturePageControl.PluginSettingsData.data.SortMode == PictureAlbum.ImageSortMode.byPhotoSourceAscending)
                {
                    return PictureAlbum.ImageSortMode.byPhotoSourceDescending;
                }
                else
                {
                    return PictureAlbum.ImageSortMode.byPhotoSourceAscending;
                }
            }
            else if (col == this.commentDataGridViewTextBoxColumn)
            {
                if (ActivityPicturePageControl.PluginSettingsData.data.SortMode == PictureAlbum.ImageSortMode.byCommentAscending)
                {
                    return PictureAlbum.ImageSortMode.byCommentDescending;
                }
                else
                {
                    return PictureAlbum.ImageSortMode.byCommentAscending;
                }
            }
            else if (col == this.thumbnailDataGridViewImageColumn)
            {
                return PictureAlbum.ImageSortMode.none;
            }
            else if (col == this.TypeImage)
            {
                if (ActivityPicturePageControl.PluginSettingsData.data.SortMode == PictureAlbum.ImageSortMode.byTypeAscending)
                {
                    return PictureAlbum.ImageSortMode.byTypeDescending;
                }
                else
                {
                    return PictureAlbum.ImageSortMode.byTypeAscending;
                }
            }
            else
            {
                return PictureAlbum.ImageSortMode.none;
            }
        }


        internal int CompareImageData(ImageData x, ImageData y)
        {
            try
            {
                if (x == null)
                {
                    if (y == null) return 0; // If x is null and y is null, they're equal. 
                    else return -1; // If x is null and y is not null, y is greater. 
                }
                else
                {
                    // If x is not null...
                    if (y == null) return 1;// ...and y is null, x is greater.
                    else
                    {
                        // ...and y is not null, compare the dates
                        int retval = 0;
                        switch (ActivityPicturePageControl.PluginSettingsData.data.SortMode)
                        {
                            case PictureAlbum.ImageSortMode.byAltitudeAscending:
                                retval = x.EW.GPSAltitude.CompareTo(y.EW.GPSAltitude);
                                break;
                            case PictureAlbum.ImageSortMode.byAltitudeDescending:
                                retval = y.EW.GPSAltitude.CompareTo(x.EW.GPSAltitude);
                                break;
                            case PictureAlbum.ImageSortMode.byCameraModelAscending:
                                retval = x.EquipmentModel.CompareTo(y.EquipmentModel);
                                break;
                            case PictureAlbum.ImageSortMode.byCameraModelDescending:
                                retval = y.EquipmentModel.CompareTo(x.EquipmentModel);
                                break;
                            case PictureAlbum.ImageSortMode.byCommentAscending:
                                retval = x.Comments.CompareTo(y.Comments);
                                break;
                            case PictureAlbum.ImageSortMode.byCommentDescending:
                                retval = y.Comments.CompareTo(x.Comments);
                                break;
                            case PictureAlbum.ImageSortMode.byDateTimeAscending:
                                retval = x.DateTimeOriginal.CompareTo(y.DateTimeOriginal);
                                break;
                            case PictureAlbum.ImageSortMode.byDateTimeDescending:
                                retval = y.DateTimeOriginal.CompareTo(x.DateTimeOriginal);
                                break;
                            case PictureAlbum.ImageSortMode.byExifGPSAscending:
                                retval = x.ExifGPS.CompareTo(y.ExifGPS);
                                break;
                            case PictureAlbum.ImageSortMode.byExifGPSDescending:
                                retval = y.ExifGPS.CompareTo(x.ExifGPS);
                                break;
                            case PictureAlbum.ImageSortMode.byPhotoSourceAscending:
                                retval = x.PhotoSource.CompareTo(y.PhotoSource);
                                break;
                            case PictureAlbum.ImageSortMode.byPhotoSourceDescending:
                                retval = y.PhotoSource.CompareTo(x.PhotoSource);
                                break;
                            case PictureAlbum.ImageSortMode.byTitleAscending:
                                retval = x.Title.CompareTo(y.Title);
                                break;
                            case PictureAlbum.ImageSortMode.byTitleDescending:
                                retval = y.Title.CompareTo(x.Title);
                                break;
                            case PictureAlbum.ImageSortMode.byTypeAscending:
                                retval = x.Type.CompareTo(y.Type);
                                break;
                            case PictureAlbum.ImageSortMode.byTypeDescending:
                                retval = y.Type.CompareTo(x.Type);
                                break;
                            case PictureAlbum.ImageSortMode.none:
                                break;
                        }

                        if (retval != 0) return retval;// If they are not equal, the later date is greater.
                        else return 0;// If the dates are equal, 0 is returned
                    }
                }
            }
            catch (Exception)
            {
                return 0;
                //throw;
            }

        }


        private void dataGridViewImages_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //this.CurrentReferenceID = this.this.pictureAlbumView.ImageList[e.RowIndex].ReferenceID;
        }




        private void panel1_Resize(object sender, EventArgs e)
        {
            if (this.Mode == ShowMode.Album)
            {
                //this.pictureAlbumView.Invalidate();
                this.pictureAlbumView.PaintAlbumView(false);
            }
        }

        private void pictureListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Mode == ShowMode.Import) ReloadData();
            this.Mode = ShowMode.List;
            UpdateView();
        }
        private void actionBanner1_MenuClicked(object sender, EventArgs e)
        {
            this.contextMenuStripView.Show(this.panelViews, this.panelViews.Width - this.contextMenuStripView.Width - 1, 0);
        }
        private void pictureAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Mode == ShowMode.Import) ReloadData();
            this.Mode = ShowMode.Album;
            UpdateView();
        }
        private void ActivityPicturePageControl_Load(object sender, EventArgs e)
        {
            try
            {
                //this.pictureAlbumView.ParentCtl = this;
                this.ThemeChanged(m_theme);
                InitializeDataGridView();
                RefreshPage();
                //ReloadData();
                //UpdateView();
            }
            catch (Exception)
            {

                //throw;
            }

        }

        private void InitializeDataGridView()
        {
            this.thumbnailDataGridViewImageColumn.DisplayIndex = 0;
            this.TypeImage.DisplayIndex = 1;
            this.ExifGPS.DisplayIndex = 2;
            this.Altitude.DisplayIndex = 3;
            this.dateTimeOriginalDataGridViewTextBoxColumn.DisplayIndex = 4;
            this.titleDataGridViewTextBoxColumn.DisplayIndex = 5;
            this.commentDataGridViewTextBoxColumn.DisplayIndex = 6;
            this.equipmentModelDataGridViewTextBoxColumn.DisplayIndex = 7;
            this.photoSourceDataGridViewTextBoxColumn.DisplayIndex = 8;
            this.referenceIDDataGridViewTextBoxColumn.DisplayIndex = 9;
            this.waypointDataGridViewTextBoxColumn.DisplayIndex = 10;


            SortListView();
        }

        private void SortListView()
        {
            DataGridViewColumn col = GetSortColumn(ActivityPicturePageControl.PluginSettingsData.data.SortMode);
            if (col != null) col.HeaderCell.Style.BackColor = m_theme.MainHeader;
            this.pictureAlbumView.ImageList.Sort(CompareImageData);
            UpdateDataGridView();
        }


        private void dataGridViewImages_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedReferenceIDs.Clear();
                DataGridViewSelectedRowCollection SelRows = ((DataGridView)(sender)).SelectedRows;
                for (int i = 0; i < SelRows.Count; i++)
                {
                    SelectedReferenceIDs.Add(this.pictureAlbumView.ImageList[SelRows[i].Index].ReferenceID);
                }
                SelRows = null;
            }
            catch (Exception)
            {
                //throw;
            }

        }
        private List<ImageData> GetSelectedImageData()
        {
            List<ImageData> il = new List<ImageData>();
            DataGridViewSelectedRowCollection SelRows = this.dataGridViewImages.SelectedRows;
            for (int i = 0; i < SelRows.Count; i++)
            {
                il.Add(this.pictureAlbumView.ImageList[SelRows[i].Index]);
            }
            return il;
        }
        #endregion

        private void trackBarImageSize_ValueChanged(object sender, EventArgs e)
        {
            if (this.Mode == ShowMode.Album)
            {
                //this.pictureAlbumView.Invalidate();
                TrackBar tb = (TrackBar)(sender);
                this.pictureAlbumView.Zoom = tb.Value;
                this.pictureAlbumView.PaintAlbumView(false);
            }
        }


        private void pictureAlbum1_MouseClick(object sender, MouseEventArgs e)
        {

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

        public void UpdateToolBar()
        {
            switch (this.pictureAlbumView.CurrentStatus)
            {
                case Helper.PictureAlbum.MediaStatus.None:
                    toolStripButtonPlay.Enabled = false;
                    toolStripButtonPause.Enabled = false;
                    toolStripButtonStop.Enabled = false;
                    break;

                case Helper.PictureAlbum.MediaStatus.Paused:
                    toolStripButtonPlay.Enabled = true;
                    toolStripButtonPause.Enabled = false;
                    toolStripButtonStop.Enabled = true;
                    break;

                case Helper.PictureAlbum.MediaStatus.Running:
                    toolStripButtonPlay.Enabled = false;
                    toolStripButtonPause.Enabled = true;
                    toolStripButtonStop.Enabled = true;
                    break;

                case Helper.PictureAlbum.MediaStatus.Stopped:
                    toolStripButtonPlay.Enabled = true;
                    toolStripButtonPause.Enabled = false;
                    toolStripButtonStop.Enabled = false;
                    break;
            }
        }

        private void toolStripButtonPlay_Click(object sender, EventArgs e)
        {
            this.pictureAlbumView.PlayVideo();
            UpdateToolBar();
        }
        private void toolStripButtonPause_Click(object sender, EventArgs e)
        {
            this.pictureAlbumView.PauseVideo();
            UpdateToolBar();
        }
        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            this.pictureAlbumView.StopVideo();
            UpdateToolBar();
        }

        private void pictureAlbum1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void timerVideo_Tick(object sender, EventArgs e)
        {
            this.trackBarVideo.Value = (int)(this.trackBarVideo.Maximum * this.pictureAlbumView.GetVideoPosition());
        }

        private void trackBarVideo_Scroll(object sender, EventArgs e)
        {
            this.pictureAlbumView.SetVideoPosition(this.trackBarVideo.Value);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Mode = ShowMode.Import;
            UpdateView();
        }

        private void pictureAlbumView_ZoomChange(object sender, int increment)
        {
            this.trackBarImageSize.Value += increment;
        }

        private void pictureAlbumView_UpdateVideoToolBar(object sender, EventArgs e)
        {
            UpdateToolBar();
        }

        private void pictureAlbumView_ShowVideoOptions(object sender, EventArgs e)
        {
            this.groupBoxVideo.Enabled = true;
        }

        private void pictureAlbumView_Load(object sender, EventArgs e)
        {
            PictureAlbum pa = (PictureAlbum)(sender);
            pa.Zoom = this.trackBarImageSize.Value;
        }

        private void importControl1_Load(object sender, EventArgs e)
        {

        }

        private void importControl1_Load_1(object sender, EventArgs e)
        {

        }

        private void btnGeoTag_Click(object sender, EventArgs e)
        {
            List<ImageData> iList = GetSelectedImageData();
            foreach (ImageData id in iList)
            {
                if (id.Type == ImageData.DataTypes.Image)
                {
                    if (System.IO.File.Exists(id.PhotoSource)) Functions.GeoTagWithActivity(id.PhotoSource, this._Activity);
                    if (System.IO.File.Exists(id.ReferenceIDPath)) Functions.GeoTagWithActivity(id.ReferenceIDPath, this._Activity);
                }
            }
            Functions.ClearImageList(this.pictureAlbumView);
            ReloadData();
            UpdateView();
        }

        private void btnKML_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.FileName = "";
            this.saveFileDialog.DefaultExt = "kmz";
            this.saveFileDialog.AddExtension = true;
            this.saveFileDialog.CheckPathExists = true;
            this.saveFileDialog.Filter = "Google Earth compressed (*.kmz)|*.kmz|Google Earth KML (*.kml)|*.kml";
            this.saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult dres = this.saveFileDialog.ShowDialog();
            if (dres == DialogResult.OK & this.saveFileDialog.FileName != "")
            {
                Functions.PerformExportToGoogleEarth(GetSelectedImageData(), this._Activity, this.saveFileDialog.FileName);
            }

        }

        private void btnTimeOffset_Click(object sender, EventArgs e)
        {
            TimeOffset frm = new TimeOffset(GetSelectedImageData());
            frm.ThemeChanged(m_theme);
            frm.ShowDialog();
        }

        private void pictureAlbumView_ActivityChanged(object sender, EventArgs e)
        {
            try
            {
                //PictureAlbum pa = (PictureAlbum)(sender);
                this.GetSortColumn(ActivityPicturePageControl.PluginSettingsData.data.SortMode).HeaderCell.Style.BackColor = this.dataGridViewImages.ColumnHeadersDefaultCellStyle.BackColor;
            }
            catch (Exception)
            {
                // throw;
            }
        }
    }

    //data that will be serialized and saved with SetExtensionData
    [Serializable()]
    public class PluginData
    {
        #region private members
        private List<ImageDataSerializable> images = new List<ImageDataSerializable>();
        #endregion

        #region public properties
        public bool Equals(PluginData pd1)
        {
            //Version not checked
            if (this.NumberOfImages.Equals(pd1.NumberOfImages))
            {
                //The lists may not be in the same order
                IDictionary<string, ImageDataSerializable> pd1Map =
                            new Dictionary<string, ImageDataSerializable>(this.NumberOfImages);

                for (int i = 0; i < this.NumberOfImages; i++)
                {
                    pd1Map.Add(pd1.images[i].ReferenceID, pd1.images[i]);
                }
                for (int i = 0; i < this.NumberOfImages; i++)
                {
                    if (pd1Map.ContainsKey(this.images[i].ReferenceID))
                    {
                        ImageDataSerializable i2 = pd1Map[images[i].ReferenceID];
                        if (!(this.images[i].PhotoSource.Equals(i2.PhotoSource) &&
                            this.images[i].Type.Equals(i2.Type)))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        public string Version
        {
            get { return ActivityPicturePlugin.Plugin.version; }
        }
        public int NumberOfImages
        {
            get
            {
                return this.Images.Count;
            }
        }
        public List<ImageDataSerializable> Images
        {
            get { return images; }
            set { images = value; }
        }
        #endregion

        #region public methods

        public void GetImageDataSerializable(List<ImageData> idorig)
        {
            List<ImageDataSerializable> idListSer = new List<ImageDataSerializable>();
            ImageDataSerializable ids;
            foreach (ImageData id in idorig)
            {
                ids = new ImageDataSerializable();
                ids.PhotoSource = id.PhotoSource;
                ids.ReferenceID = id.ReferenceID;
                ids.Type = id.Type;
                idListSer.Add(ids);
            }
            this.Images = idListSer;
        }

        /// <summary> 
        /// Method for updating ImageData after reading in the logbook
        /// </summary> 
        /// <value></value> 
        /// <remarks></remarks> 
        /// <history> 
        /// [doml] 04.09.2007 Created 
        /// </history> 
        public List<ImageData> LoadImageData(List<ImageDataSerializable> IDSer)
        {
            List<ImageData> IDList = new System.Collections.Generic.List<ImageData>();
            ImageData ID;
            for (int i = 0; i < this.NumberOfImages; i++)
            {
                ID = new ImageData(IDSer[i]);
                IDList.Add(ID);
            }
            return IDList;
        }

        //public byte[] imageToByteArray(System.Drawing.Image imageIn)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //    return ms.ToArray();
        //}
        //public Image byteArrayToImage(byte[] byteArrayIn)
        //{
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    Image returnImage = Image.FromStream(ms);
        //    return returnImage;
        //}
        #endregion
    }


    [Serializable()]
    public class PluginSettings
    {
        public class SettingsData
        {
            public bool Equals(SettingsData sd1)
            {
                if (this.quality == sd1.quality &&
                    this.size == sd1.size &&
                    this.sortmode == sd1.sortmode)
                {
                    return true;
                }
                return false;
            }
            #region private methods
            private PictureAlbum.ImageSortMode sortmode = PictureAlbum.ImageSortMode.byDateTimeAscending;
            private int size = 8;
            private int quality = 8;
            public PictureAlbum.ImageSortMode SortMode 
            {
                get { return sortmode; }
                set { sortmode = value; }
            }
            public int Quality
            {
                get
                {
                    return quality;
                }
                set
                {
                    quality = value;
                }
            }
            public int Size
            {
                get
                {
                    return size;
                }
                set
                {
                    size = value;
                }
            }
        }
        public SettingsData data = new SettingsData();
        public SettingsData dataRead = new SettingsData(); //data last read from logbook
        private ZoneFiveSoftware.Common.Data.Fitness.ILogbook log
        {
            get
            {
                return ActivityPicturePlugin.Plugin.GetIApplication().Logbook;
            }
        }


        public void ReadSettings()
        {
            try
            {
                byte[] b = log.GetExtensionData(ActivityPicturePlugin.GUIDs.PluginMain);
                if (!(b.Length == 0))
                {
                    System.Xml.Serialization.XmlSerializer xmlSer = new System.Xml.Serialization.XmlSerializer(typeof(SettingsData));
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    mem.Write(b, 0, b.Length);
                    mem.Position = 0;
                    data = dataRead = (SettingsData)xmlSer.Deserialize(mem);
                    mem.Dispose();
                    xmlSer = null;
                    b = null;

                    //this.Quality = p.Quality;
                    //this.Size = p.Size;               
                }
                else
                {

                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        public void WriteSettings()
        {
            if (!data.Equals(dataRead))
            {
                try
                {
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    System.Xml.Serialization.XmlSerializer xmlSer = new System.Xml.Serialization.XmlSerializer(typeof(SettingsData));
                    xmlSer.Serialize(mem, data);
                    log.SetExtensionData(ActivityPicturePlugin.GUIDs.PluginMain, mem.ToArray());
                    log.SetExtensionText(ActivityPicturePlugin.GUIDs.PluginMain, "Picture Plugin");
                    mem.Close();
                    log.Modified = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
            #endregion
    }

    public class IRouteWaypoint
    //Dummy until available in API
    {
        public enum MarkerType
        {
            Start,
            End,
            ElapsedTime,
            FixedDateTime,
            Distance,
            GPSLocation
        }
        public MarkerType Type { get { return MarkerType.FixedDateTime; } set { } }
        public TimeSpan ElapsedTime { get { return System.TimeSpan.MinValue; } set { } }
        public DateTime FixedDateTime { get { return System.DateTime.Now; } set { } }
        public double DistanceMeters { get { return 0; } set { } }
        public IGPSLocation GPSLocation { get { return null; } set { } }
        public string Notes { get { return null; } set { } }
        public string Description { get { return null; } set { } }
        public Image MarkerImage { get { return null; } set { } }
        public Image Photo { get { return null; } set { } }
        public new string ToString
        {
            get
            {
                return "Not yet implemented in API";
            }
        }
    }
}
