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
using System.IO;
using System.Text;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Data.Fitness;
using System.Drawing;
using System.Windows.Forms;
using ActivityPicturePlugin.Properties;
using ZoneFiveSoftware.Common.Data.Measurement;
using DexterLib;

namespace ActivityPicturePlugin.Helper
{
    
    public class ImageData : IDisposable, IComparable
    {
        public ImageData(ImageDataSerializable IDSer, IActivity activity)
        {
            try
            {
                this.activity = activity;
                System.Diagnostics.Debug.Assert(IDSer.Type != DataTypes.Nothing, "No type set for " + IDSer.PhotoSource );
                this.IDser = IDSer;
                bool bNewThumb = this.SetThumbnail();

                if ( this.Thumbnail != null )
                {
                    this.ew = new ExifWorks( this.ThumbnailPath );

                    // Get the best time from photosource and assign it to thumbnail.
                    // Only assign best time if a new thumbnail was created.  Otherwise, we might
                    // overwrite whatever timeoffsets that may have already been performed on the 
                    // thumbnail.
                    if ( bNewThumb )
                    {
                        FileInfo file = new FileInfo( this.PhotoSource );
                        if ( file.Exists )
                        {
                            // Get photosource best time.
                            ActivityInfo info = ActivityInfoCache.Instance.GetInfo( this.activity );
                            DateTime exif = DateTime.MinValue;
                            ExifWorks eWorks = new ExifWorks(this.PhotoSource);
                            DateTime dtBest = Functions.GetBestTime( file,
                                eWorks.DateTimeOriginal.ToUniversalTime(),
                                this.activity.StartTime,
                                info.EndTime );

                            // Convert to Local Time
                            dtBest = dtBest.ToLocalTime();

                            // Save best time to thumbnail if it's different
                            if ( this.DateTimeOriginal != dtBest )
                            {
                                string strDate = dtBest.ToString( Functions.NeutralDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture );
                                if ( !string.IsNullOrEmpty( strDate ) )
                                {
                                    this.EW.SetPropertyString( (int)( ExifWorks.TagNames.ExifDTOrig ), strDate );

                                    //Save Exif data to the thumbnail
                                    this.EW.GetBitmap().Save( this.ThumbnailPath );
                                    this.EW.Dispose();
                                    this.ew = new ExifWorks( this.ThumbnailPath );
                                }
                            }
                        }
                    }
                }
                else
                {
                    this.ew = new ExifWorks();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
        }

        #region Private Members
        public enum DataTypes
        {
            Nothing,
            Image,
            Video
        }

        private IActivity activity;
        //private DataTypes type;
        //private string photosource;
        //private string referenceID;
        private ImageDataSerializable IDser;
        private ExifWorks ew;
        private Image thumbnailImage;
        private Single? ratio=null;
        private string m_thumbnailPath;
        private IGPSPoint gpsPoint;
        private bool selected;

#if ST_2_1
        private static string ImageFilesFolder = System.IO.Path.GetFullPath( ActivityPicturePlugin.Plugin.GetApplication().SystemPreferences.WebFilesFolder + "\\Images\\" );
#else
        private static string ImageFilesFolder = ActivityPicturePlugin.Plugin.GetApplication().Configuration.CommonWebFilesFolder + System.IO.Path.DirectorySeparatorChar + GUIDs.PluginMain.ToString() + System.IO.Path.DirectorySeparatorChar;
        private static string ImageFilesFolderST2 = System.IO.Path.GetFullPath(ActivityPicturePlugin.Plugin.GetApplication().Configuration.CommonWebFilesFolder + "\\..\\..\\2.0\\Web Files\\Images\\");
#endif
        #endregion

        #region Public Members
        #endregion

        #region Public Properties
        public static void CreateImageFilesFolder()
        {
            //Create directory if it does not already exist!
            if (!System.IO.Directory.Exists(ImageData.ImageFilesFolder))
            {
                System.IO.Directory.CreateDirectory(ImageData.ImageFilesFolder);
            }
        }

        //Compare static only, not Exif
        public bool Equals( ImageData pd1 )
        {
            if (//this.Altitude.Equals(pd1.Altitude) &&
                //this.Comments.Equals(pd1.Comments) &&
                //this.DateTimeOriginal.Equals(pd1.DateTimeOriginal) &&
                //this.EquipmentModel.Equals(pd1.EquipmentModel) &&
                this.EW.Equals( pd1.EW ) &&
                //this.ExifGPS.Equals(pd1.ExifGPS) &&
                //this.KMLGPS.Equals(pd1.KMLGPS) &&
                this.PhotoSource.Equals( pd1.PhotoSource ) &&
                //this.PhotoSourceFileName.Equals(pd1.PhotoSourceFileName) &&
                this.Ratio.Equals( pd1.Ratio ) &&
                this.ReferenceID.Equals( pd1.ReferenceID ) &&
                //this.ReferenceIDPath.Equals(pd1.ReferenceIDPath) &&
                //this.Title.Equals(pd1.Title) &&
                //this.TypeImage.Equals(pd1.TypeImage) &&
                this.Type.Equals( pd1.Type ) )
            {
                return true;
            }
            return false;
        }

        public Image TypeImage
        {
            get
            {
                if ( this.Type == DataTypes.Image )
                {
                    return Resources.btnimage;
                }
                else if ( this.Type == DataTypes.Video )
                {
                    return Resources.btnvideo;
                }
                return null;
            }
        }

        public DataTypes Type
        {
            get { return this.IDser.Type; }
            //set { type = value; }
        }

        public String PhotoSource
        {
            get { return this.IDser.PhotoSource; }
            //set { photosource = value; }
        }

        public string ReferenceID
        {
            get { return this.IDser.ReferenceID; }
            //set { referenceID = value; }
        }

        private ExifWorks EW
        {
            get { return ew; }
            //set { ew = value; }
        }

        public Image Thumbnail
        {
            get { return this.thumbnailImage; }
            //set { this.thumbnailImage = value; }
        }

        public DateTime DateTimeOriginal
        {
            get
            {
                return this.EW.DateTimeOriginal;
            }
        }

        public void SetPhotoSource(string s)
        {
            this.IDser.PhotoSource = s;
        }

        public void SetDateTimeOriginal(DateTime dt)
        {
            this.EW.SetPropertyString((int)(ExifWorks.TagNames.ExifDTOrig), dt.ToString(Functions.NeutralDateTimeFormat));
            SavePhotoSourceProperty(ExifWorks.TagNames.ExifDTOrig);
        }

        public Bitmap ExifBitmap()
        {
            return this.EW.GetBitmap();
        }

        public string Title
        {
            get
            {
                try
                {
                    if ( this.EW.FileExplorerTitle != null )
                    {
                        return CleanInput( this.EW.FileExplorerTitle );
                    }
                    else return "";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                    return "";
                }
            }
            set
            {
                try
                {
                    this.EW.FileExplorerTitle = value;
                    SavePhotoSourceProperty( ExifWorks.TagNames.FileExplorerTitle );
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                    throw;
                }
            }
        }

        public string Comments
        {
            get
            {
                try
                {
                    if ( this.EW.FileExplorerComments != null )
                        return CleanInput( this.EW.FileExplorerComments );
                    else return "";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                    return "";
                }

            }
            set
            {
                try
                {
                    this.EW.FileExplorerComments = value;
                    SavePhotoSourceProperty( ExifWorks.TagNames.FileExplorerComments );
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                    throw;
                }
            }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public Single Ratio
        {
            get
            {
                if (ratio == null)
                {
                    System.Drawing.Bitmap b = this.EW.GetBitmap();
                    if (b == null)
                    {
                        this.ratio = 1;
                    }
                    else
                    {
                        this.ratio = (Single)(this.EW.GetBitmap().Width) / (Single)(this.EW.GetBitmap().Height);
                    }

                } 
                return (Single)ratio;
            }
            //set { ratio = value; }
        }

        static String CleanInput( string strIn )
        {
            // Replace invalid characters with empty strings.
            return System.Text.RegularExpressions.Regex.Replace( strIn, @"[^ -ÿ]", "" );
        }

        public string EquipmentModel
        {
            get
            {
                try
                {
                    if ( this.EW.EquipmentModel != null )
                        return CleanInput( this.EW.EquipmentModel );
                    else return "";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                    return "";
                }
            }
        }

        //Used in Image List
        public string ExifGps
        {
            get
            {
                return GPS.GpsString(this.GpsPoint);
            }
        }

        public bool HasExifGps = false;
        //GPS info, Original time could come from the original Exif (transfered to the thumbnail)
        //Original time are stored in exif for the thumbnail, but GPS may be calculated.
        //As we do not want to keep track of how GPS is calculated when editing time on the image or
        //the activity GPS, keep GPS "dynamic" but cached
        //(GPS should be invalidated if activity is changed)
        public IGPSPoint GpsPoint
        {
            get 
            {
                if (this.gpsPoint == null)
                {
                    float lat = (float)ew.GPSLatitude;
                    float lon = (float)ew.GPSLongitude;
                    float alt = (float)ew.GPSAltitude;
                    if (lat != 0 || lon != 0)
                    {
                        HasExifGps = true;
                    }
                    if (!HasExifGps && this.activity != null && this.activity.GPSRoute != null)
                    {
                        DateTime time = this.EW.DateTimeOriginal.ToUniversalTime();
                        ZoneFiveSoftware.Common.Data.ITimeValueEntry<IGPSPoint> g = this.activity.GPSRoute.GetInterpolatedValue( time );
                        if ( g != null )
                        {
                            gpsPoint = g.Value;
                        }
                    }

                    if ( gpsPoint == null )
                        gpsPoint = new GPSPoint( lat, lon, alt );
                }

                return gpsPoint;
            }
        }

        public void FlushGpsPoint()
        {
            this.gpsPoint = null;
            this.ew.GPSLatitude = 0;
            this.ew.GPSLongitude = 0;
            this.ew.GPSAltitude = 0;
        }

        public bool HasGps()
        {
            IGPSPoint g = this.GpsPoint;
            return !((g.LatitudeDegrees == 0) && (g.LongitudeDegrees == 0));
        }

        public string KMLGPS
        {
            get
            {
                string kml = "";

                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo( "en-US" );
                kml = this.GpsPoint.LatitudeDegrees.ToString( "0.00000000", ci ) + "," + 
                    this.GpsPoint.LongitudeDegrees.ToString( "0.00000000", ci ) + "," + 
                this.GpsPoint.ElevationMeters.ToString( "0.00", ci );
                return kml;
            }
        }

        internal static string thumbnailPath(string referenceID)
        {
            string path = ImageFilesFolder + referenceID + ".jpg";
#if !ST_2_1
            //If thumbnails created in ST2 (or earlier versions of the plugin..), keep them
            //TODO: They should be migrated eventually instead
            if (!System.IO.File.Exists(path))
            {
                string ThumbnailPathST2 = ImageFilesFolderST2 + referenceID + ".jpg";
                if (System.IO.File.Exists(ThumbnailPathST2))
                {
                    return ThumbnailPathST2;
                }
            }
#endif
            return path;
        }

        public string ThumbnailPath
        {
            get
            {
                if ((m_thumbnailPath == null) && (this.ReferenceID!=null))
                {
                    //Cached, as (ST3) call make fileIO
                    m_thumbnailPath = thumbnailPath(this.ReferenceID);
                }
                return m_thumbnailPath;
            }
        }

        internal void DeleteThumbnail()
        {
            try
            {
                if (System.IO.File.Exists(this.ThumbnailPath))
                {
                    System.IO.File.Delete(this.ThumbnailPath);
                }
            }
            catch (Exception /*ex*/)
            {
                //TODO: Fails in list view and import. At least map must be updated
                //System.Diagnostics.Debug.Assert(false, ex.Message);
            }
        }

        public string GetBestImage()
        {
            string path = null;
            //try to open Photosource first
            try
            {
                if (System.IO.File.Exists(this.PhotoSource))
                {
                    path = this.PhotoSource;
                }
                // if not found, try next to open image from ...\Web Files\Images folder
                else
                {
                    if (System.IO.File.Exists(this.ThumbnailPath))
                    {
                        path = this.ThumbnailPath;
                    }
                    // if no location is not found, nothing will happen
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                throw;
            }
            return path;
        }

        public string Altitude
        {
            get
            {
                if (!HasGps())
                {
                    return "";
                }
                return GpsRunningPlugin.Util.UnitUtil.Elevation.ToString(this.GpsPoint.ElevationMeters);
            }
        }

        public string PhotoSourceFileName
        {
            get
            {
                int i = this.PhotoSource.LastIndexOf( @"\" );
                if (i > 0) return this.PhotoSource.Substring(i + 1);
                else return "";
            }
        }
        #endregion

        #region Private Methods
        private void SavePhotoSourceProperty( ExifWorks.TagNames prop )
        {
            try
            {
                if ((System.IO.File.Exists(this.PhotoSource)) && (Functions.IsExifFileExt(new System.IO.FileInfo(this.PhotoSource))))
                // Save Exif data to the original source
                {
                    using ( ExifWorks EWPhotoSource = new ExifWorks( this.PhotoSource ) )
                    {
                        switch ( prop )
                        {
                            case ExifWorks.TagNames.ExifDTOrig:
                                EWPhotoSource.DateTimeOriginal = ew.DateTimeOriginal;
                                break;
                            case ExifWorks.TagNames.FileExplorerTitle:
                                EWPhotoSource.FileExplorerTitle = ew.FileExplorerTitle;
                                break;
                            case ExifWorks.TagNames.FileExplorerComments:
                                EWPhotoSource.FileExplorerComments = ew.FileExplorerComments;
                                break;
                            case ExifWorks.TagNames.GpsAltitude:
                                EWPhotoSource.GPSAltitude = ew.GPSAltitude;
                                break;
                            case ExifWorks.TagNames.GpsLongitude:
                                EWPhotoSource.GPSLongitude = ew.GPSLongitude;
                                break;
                            case ExifWorks.TagNames.GpsLatitude:
                                EWPhotoSource.GPSLatitude = ew.GPSLatitude;
                                break;
                        }
                        EWPhotoSource.GetBitmap().Save( this.PhotoSource );
                    }
                }

                //Save Exif data to the webfiles image
                this.EW.GetBitmap().Save( this.ThumbnailPath );
                this.EW.Dispose();
                this.ew = new ExifWorks( this.ThumbnailPath );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }

        }
        #endregion

        #region Public Methods

        // Returns true if new thumbnail was created
        private bool SetVideoThumbnail()
        {
            bool bRet = false;
            try
            {
                string defpath = this.ThumbnailPath;

                //Check if image on the WebFiles folder exists
                if ( System.IO.File.Exists( defpath ) )
                {
                    using (FileStream fs = new FileStream(defpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (Image img = Image.FromStream(fs, true, false))
                    {
                        //The thumbnail is being created
                        this.thumbnailImage = Functions.getThumbnailWithBorder(50, img);
                    }
                }
                //File has not yet been created
                else
                {
                    //Check if image at specified PhotoSource location exists
                    if ( System.IO.File.Exists( this.PhotoSource ) )
                    {
                        // Create new image in the default folder
                        using (Bitmap bmp = (Bitmap)(Resources.video).Clone())
                        {
                            Functions.SaveThumbnailImage(bmp, defpath, 10);
                            this.thumbnailImage = Functions.getThumbnailWithBorder(50, bmp);
                        }
                        bRet = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                throw;
            }

            return bRet;
        }

        public void ResetVideoThumbnail()
        {
            string defpath = this.ThumbnailPath;

            //Check if image on the WebFiles folder exists
            if ( System.IO.File.Exists( defpath ) )
            {
                // Create new image in the default folder
                using ( Bitmap bmp = (Bitmap)( Resources.video ).Clone() )
                {
                    Functions.SaveThumbnailImage( bmp, defpath, 10 );
                    this.thumbnailImage = Functions.getThumbnailWithBorder(50, bmp);
                }
            }
        }

        //Replaces the current thumbnail with the video image at iFrame
        //If iFrame is -1, default video image is used
        public bool ReplaceVideoThumbnail( int iFrame, Size size, double dblTimePerFrame )
        {
            try
            {
                Bitmap bmpOrig = null;
                string defpath = this.ThumbnailPath;

                //Check if image at specified PhotoSource location exists
                if ( System.IO.File.Exists( this.PhotoSource ) )
                {
                    // Create new image in the default folder
                    if ( iFrame == -1 ) bmpOrig = (Bitmap)( Resources.video ).Clone();
                    else
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo( this.PhotoSource );
                        // DexterLib
                        // If Interop.DexterLib.dll is missing, exception is thrown (caught)
                        //bmpOrig = GetDexterAviBmp( this.PhotoSource, iFrame, size, dblTimePerFrame );
                        //if ( ( bmpOrig == null ) && ( String.Compare( fi.Extension.ToLower(), ".avi" ) == 0 ) )
                            //bmpOrig = GetAviBmp( this.PhotoSource, iFrame );

                        if ( String.Compare( fi.Extension.ToLower(), ".avi" ) == 0 )
                            bmpOrig = GetAviBmp( this.PhotoSource, iFrame );
                        if ( bmpOrig == null )
                            bmpOrig = GetDexterAviBmp( this.PhotoSource, iFrame, size, dblTimePerFrame );
                        
                    }

                    if ( bmpOrig != null )
                    {
                        // Create new image in the default folder
                        Size newsize = new Size();
                        int UpperPixelLimit = 500;
                        Double ratio = (double)( bmpOrig.Width ) / (double)( bmpOrig.Height );
                        if ( ratio > 1 )
                        {
                            newsize.Width = UpperPixelLimit;
                            newsize.Height = (int)( UpperPixelLimit / ratio );
                        }
                        else
                        {
                            newsize.Height = UpperPixelLimit;
                            newsize.Width = (int)( UpperPixelLimit * ratio );
                        }
                        using ( Bitmap bmp = new Bitmap( bmpOrig, newsize ) )
                        {
                            Functions.SaveThumbnailImage( bmp, defpath, 10 );
                            this.thumbnailImage = Functions.getThumbnailWithBorder(50, bmp);
                        }
                        bmpOrig.Dispose();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
            return false;
        }

        private Bitmap GetDexterAviBmp( string VideoFile, int iFrame, Size frameSize, double dblTimePerFrame )
        {
            Bitmap bitmap = null;

            try
            {
                MediaDetClass md = new MediaDetClass();
                md.Filename = VideoFile;
                md.CurrentStream = 0;

                double time = iFrame * dblTimePerFrame;
                if ( time > md.StreamLength ) time = md.StreamLength;

                string sTempFile = System.IO.Path.GetTempFileName();
                md.WriteBitmapBits( time, frameSize.Width, frameSize.Height, sTempFile );

                // Creating the bitmap from a stream so we can delete the temporary file
                System.IO.FileInfo fi = new System.IO.FileInfo( sTempFile );

                using ( System.IO.FileStream fs = fi.OpenRead() )
                {
                    bitmap = (Bitmap)Bitmap.FromStream( fs );
                }

                // Stamp the thumbnail with "Video" so it's easier to distinguish
                // between movies and pics in Album view.
                using ( Graphics g = Graphics.FromImage( bitmap ) )
                using ( Font f = new Font( PictureAlbum.DefaultFont.FontFamily, 15 ) )
                {
                    g.DrawString( Resources.groupBoxVideo_Text, f, Brushes.White, new PointF( 0, 0 ) );
                }

                System.IO.File.Delete( sTempFile ); //cleanup the temporary file
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
            return bitmap;
        }

        // Gets the bitmap of the specified frame
        private Bitmap GetAviBmp( string VideoFile, int iFrame )
        {
            AviFile.AviManager aviManager = null;
            AviFile.VideoStream stream = null;
            Bitmap bmp = null;
            try
            {
                aviManager = new AviFile.AviManager( VideoFile, true, true );
                if ( aviManager != null )
                {
                    stream = aviManager.GetVideoStream();
                    stream.GetFrameOpen();
                    if ( iFrame <= 0 ) iFrame = 1;
                    if ( iFrame > stream.CountFrames ) iFrame = stream.CountFrames;
                    bmp = stream.GetBitmap( iFrame );

                    // Stamp the thumbnail with "Video" so it's easier to distinguish
                    // between movies and pics in Album view.
                    using ( Graphics g = Graphics.FromImage( bmp ) )
                    using ( Font f = new Font( PictureAlbum.DefaultFont.FontFamily, 15 ) )
                    {
                        g.DrawString( Resources.groupBoxVideo_Text, f, Brushes.White, new PointF( 0, 0 ) );
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                // Access denied errors could be thrown, etc.
            }
            finally
            {
                if ( stream != null )
                {
                    stream.GetFrameClose();
                    stream = null;
                }

                if ( aviManager != null )
                {
                    aviManager.Close();
                    aviManager = null;
                }
            }

            return bmp;
        }

        internal bool SetThumbnail()
        {
            bool bRet = false;
            if (this.Type == ImageData.DataTypes.Image)
            {
                bRet = this.SetImageThumbnail();
            }
            else if (this.Type == ImageData.DataTypes.Video)
            {
                bRet = this.SetVideoThumbnail();
            }
            return bRet;
        }

        // Returns true if new thumbnail was created
        private bool SetImageThumbnail()
        {
            bool bRet = false;
            try
            {
                string defpath = this.ThumbnailPath;

                //Check if image on the WebFiles folder exists
                if ( System.IO.File.Exists( defpath ) )
                {
                    using (FileStream fs = new FileStream(defpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (Image img = Image.FromStream(fs, true, false)) 
                    {
                        //Create Image
                        this.thumbnailImage = Functions.getThumbnailWithBorder(50, img);
                    }
                }
                //Thumbnail file has not yet been created
                else
                {
                    //Check if image at specified PhotoSource location exists
                    if ( System.IO.File.Exists( this.PhotoSource ) )
                    {
                        using (FileStream fs = new FileStream(this.PhotoSource, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (Image bmpOrig = Image.FromStream(fs, true, false))
                        {
                            const int UpperPixelLimit = 500;
                            Size size = new Size();
                            Double ratio = (double)(bmpOrig.Width) / (double)(bmpOrig.Height);
                            if ( ratio > 1 )
                            {
                                size.Width = UpperPixelLimit;
                                size.Height = (int)( UpperPixelLimit / ratio );
                            }
                            else
                            {
                                size.Height = UpperPixelLimit;
                                size.Width = (int)( UpperPixelLimit * ratio );
                            }
                            using (Bitmap bmp = new Bitmap( bmpOrig, size ) )
                            {
                                //copying the metadata of the original file into the new image
                                foreach ( System.Drawing.Imaging.PropertyItem pItem in bmpOrig.PropertyItems )
                                {
                                    try
                                    {
                                        //Mono TODO: NotImplemented
                                        bmp.SetPropertyItem( pItem );
                                    }
                                    catch { }
                                }

                                // Create new image in the default folder
                                Functions.SaveThumbnailImage(bmp, defpath, 10);
                                this.thumbnailImage = Functions.getThumbnailWithBorder(50, bmp);
                            }
                        }
                        bRet = true;
                    }
                    // Thumbnail cannot be created, both target locations are invalid
                    else
                    {
                        //TODO: implement a way to work when images are not found!
                        // Works when no images are found.
                        //MessageBox.Show("both paths not found");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }

            return bRet;
        }

        internal ImageDataSerializable GetSerialzable(IList<ImageDataSerializable> images)
        {
            ImageDataSerializable res = null;
            foreach (ImageDataSerializable ids in images)
            {
                if (this.ReferenceID == ids.ReferenceID)
                {
                    res = ids;
                    break;
                }
            }
            System.Diagnostics.Debug.Assert(res != null, "ImageDataSerializable not found" + this.PhotoSource);
            return res;
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        protected virtual void Dispose( bool disposing )
        {
            if (this.EW != null)
            {
                this.EW.Dispose();
            }
            if ((disposing) && (this.thumbnailImage != null))
            {
                this.thumbnailImage.Dispose();
                this.thumbnailImage = null;
            }
        }
        #endregion

        int IComparable.CompareTo(object y)
        {
            int retval = 0;

            try
            {
                // If x is not null...
                if (y == null) return 1;// ...and y is null, x is greater.
                if (!(y is ImageData)) return 1;
                ImageData y2 = y as ImageData;

                // ...and y is not null, compare the dates
                switch ((PictureAlbum.ImageSortMode)ActivityPicturePlugin.Source.Settings.SortMode)
                {
                    case PictureAlbum.ImageSortMode.byAltitudeAscending:
                        retval = this.EW.GPSAltitude.CompareTo(y2.EW.GPSAltitude);
                        break;
                    case PictureAlbum.ImageSortMode.byAltitudeDescending:
                        retval = y2.EW.GPSAltitude.CompareTo(this.EW.GPSAltitude);
                        break;
                    case PictureAlbum.ImageSortMode.byCameraModelAscending:
                        retval = this.EquipmentModel.CompareTo(y2.EquipmentModel);
                        break;
                    case PictureAlbum.ImageSortMode.byCameraModelDescending:
                        retval = y2.EquipmentModel.CompareTo(this.EquipmentModel);
                        break;
                    case PictureAlbum.ImageSortMode.byCommentAscending:
                        retval = this.Comments.CompareTo(y2.Comments);
                        break;
                    case PictureAlbum.ImageSortMode.byCommentDescending:
                        retval = y2.Comments.CompareTo(this.Comments);
                        break;
                    case PictureAlbum.ImageSortMode.byDateTimeAscending:
                        retval = this.EW.DateTimeOriginal.CompareTo(y2.EW.DateTimeOriginal);
                        break;
                    case PictureAlbum.ImageSortMode.byDateTimeDescending:
                        retval = y2.EW.DateTimeOriginal.CompareTo(this.EW.DateTimeOriginal);
                        break;
                    case PictureAlbum.ImageSortMode.byExifGPSAscending:
                        retval = GPS.Compare(this.GpsPoint, y2.GpsPoint);
                        break;
                    case PictureAlbum.ImageSortMode.byExifGPSDescending:
                        retval = GPS.Compare(y2.GpsPoint, this.GpsPoint);
                        break;
                    case PictureAlbum.ImageSortMode.byPhotoSourceAscending:
                        retval = this.PhotoSource.CompareTo(y2.PhotoSource);
                        break;
                    case PictureAlbum.ImageSortMode.byPhotoSourceDescending:
                        retval = y2.PhotoSource.CompareTo(this.PhotoSource);
                        break;
                    case PictureAlbum.ImageSortMode.byTitleAscending:
                        retval = this.Title.CompareTo(y2.Title);
                        break;
                    case PictureAlbum.ImageSortMode.byTitleDescending:
                        retval = y2.Title.CompareTo(this.Title);
                        break;
                    case PictureAlbum.ImageSortMode.byTypeAscending:
                        retval = this.Type.CompareTo(y2.Type);
                        break;
                    case PictureAlbum.ImageSortMode.byTypeDescending:
                        retval = y2.Type.CompareTo(this.Type);
                        break;
                    case PictureAlbum.ImageSortMode.none:
                        break;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }

            return retval;
        }
    }

    //serializable extract of the ImageData class. Only this information of each image will be saved with SetExtensionData
    [Serializable()]
    public class ImageDataSerializable
    {
        public static ImageDataSerializable FromFile(System.IO.FileInfo file)
        {
            ImageDataSerializable ids = null;
            ImageData.DataTypes dt = Functions.GetMediaType(file.FullName);

            if (dt != ImageData.DataTypes.Nothing)
            {
                ids = new ImageDataSerializable();
                ids.photosource = file.FullName;
                ids.referenceID = Guid.NewGuid().ToString();
                ids.type = dt;
            }
            return ids;
        }

        private ImageData.DataTypes type;
        private string photosource;
        private string referenceID;

        public string ReferenceID
        {
            get { return referenceID; }
            set { referenceID = value; }
        }

        public string PhotoSource
        {
            get { return photosource; }
            set { photosource = value; }
        }

        public ImageData.DataTypes Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}