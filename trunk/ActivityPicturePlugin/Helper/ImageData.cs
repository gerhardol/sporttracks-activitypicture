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
using System.Text;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Data.GPS;
using System.Drawing;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Data.Measurement;
using DexterLib;

namespace ActivityPicturePlugin.Helper
{

    public class ImageData : IDisposable
    {
        public ImageData()
        {
        }

        public ImageData( ImageDataSerializable IDSer )
        {
            try
            {
                if ( IDSer.Type == DataTypes.Nothing )
                {
                    IDSer.Type = Functions.GetMediaType( IDSer.PhotoSource );
                }
                this.type = IDSer.Type;
                this.photosource = IDSer.PhotoSource;
                this.referenceID = IDSer.ReferenceID;
                this.SetThumbnail();
                if (this.Thumbnail != null)
                {
                    this.EW = new ExifWorks(this.ThumbnailPath);
                }
                else
                {
                    this.EW = new ExifWorks();
                }
                System.Drawing.Bitmap b = this.EW.GetBitmap();
                if (b == null)
                {
                    this.Ratio = 1;
                }
                else
                {
                    this.Ratio = (Single)(this.EW.GetBitmap().Width) / (Single)(this.EW.GetBitmap().Height);
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

        private DataTypes type;
        private bool selected;
        private string photosource;
        private string referenceID;
        private ExifWorks ew;
        private Image thumbnail;
        private Single ratio;
        private string m_thumbnailPath;

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
                //this.Waypoint.Equals(pd1.Waypoint) &&
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
                    return Resources.Resources.btnimage;
                }
                else if ( this.Type == DataTypes.Video )
                {
                    return Resources.Resources.btnvideo;
                }
                return null;

            }
        }

        public DataTypes Type
        {
            get { return type; }
            set { type = value; }
        }

        public String PhotoSource
        {
            get { return photosource; }
            set { photosource = value; }
        }

        public string ReferenceID
        {
            get { return referenceID; }
            set { referenceID = value; }
        }

        public ExifWorks EW
        {
            get { return ew; }
            set { ew = value; }
        }

        public Image Thumbnail
        {
            get { return this.thumbnail; }
            set { this.thumbnail = value; }
        }

        public void OffsetDateTimeOriginal( int year, int month, int day, int hour, int min, int sec )
        {
            DateTime dt = EW.DateTimeOriginal;
            dt = dt.AddYears( year );
            dt = dt.AddMonths( month );
            dt = dt.AddDays( day );
            dt = dt.AddHours( hour );
            dt = dt.AddMinutes( min );
            dt = dt.AddSeconds( sec );
            EW.SetPropertyString( (int)( ExifWorks.TagNames.ExifDTOrig ), dt.ToString( Functions.NeutralDateTimeFormat ) );
            SavePhotoSourceProperty( ExifWorks.TagNames.ExifDTOrig );
        }

        public string DateTimeOriginal
        {
            get
            {
                DateTime dt = new DateTime( 1950, 1, 1 );
                if ( dt < EW.DateTimeOriginal ) return (
                      EW.DateTimeOriginal.ToString( System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern )
                      + Environment.NewLine
                      + EW.DateTimeOriginal.ToString( System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern ) );
                else return "";
            }
        }

        public string Title
        {
            get
            {
                try
                {
                    if ( this.EW.FileExplorerTitle != null )
                    {
                        //return System.Text.RegularExpressions.Regex.Replace(this.EW.FileExplorerTitle, @"[\W]", "");
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
            get { return ratio; }
            set { ratio = value; }
        }

        String CleanInput( string strIn )
        {
            // Replace invalid characters with empty strings.
            return System.Text.RegularExpressions.Regex.Replace( strIn, @"[^ -�]", "" );
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

        public IGPSLocation GpsLocation
        {
            get { return new GPSLocation( (float)ew.GPSLatitude, (float)ew.GPSLongitude ); }
        }

        public IGPSPoint GpsPoint
        {
            get { return new GPSPoint( (float)ew.GPSLatitude, (float)ew.GPSLongitude, (float)ew.GPSAltitude ); }
        }

        public string ExifGPS
        {
            get
            {
                try
                {
                    if ( ( ew.GPSLatitude == 0 ) & ( ew.GPSLongitude == 0 ) ) return "";
                    else
                    {
                        string GPSString = "";
                        double degLat, minLat, secLat, degLon, minLon, secLon;
                        switch ( Plugin.GetApplication().SystemPreferences.GPSLocationUnits )
                        {
                            case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.MinutesSeconds:

                                degLat = Math.Abs( Math.Truncate( ew.GPSLatitude ) );
                                minLat = Math.Truncate( ( Math.Abs( ew.GPSLatitude ) - degLat ) * 60 );
                                secLat = ( ( ( Math.Abs( ew.GPSLatitude ) - degLat ) * 60 ) - minLat ) * 60;

                                degLon = Math.Abs( Math.Truncate( ew.GPSLongitude ) );
                                minLon = Math.Truncate( ( Math.Abs( ew.GPSLongitude ) - degLon ) * 60 );
                                secLon = ( ( ( Math.Abs( ew.GPSLongitude ) - degLon ) * 60 ) - minLon ) * 60;

                                GPSString = degLat.ToString() + "� " + minLat.ToString() + "' " + secLat.ToString( "00" )
                                    + (char)34 + " " + ew.GPSLatitudeReference + Environment.NewLine +
                                    degLon.ToString() + "� " + minLon.ToString() + "' " + secLon.ToString( "00" )
                                + (char)34 + " " + ew.GPSLongitudeReference;
                                break;
                            case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.Decimal3:
                                GPSString = ew.GPSLatitude.ToString( "0.000" ) + Environment.NewLine +
                                   ew.GPSLongitude.ToString( "0.000" );
                                break;
                            case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.Decimal4:
                                GPSString = ew.GPSLatitude.ToString( "0.0000" ) + Environment.NewLine +
                                   ew.GPSLongitude.ToString( "0.0000" );
                                break;
                            case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.Minutes:
                                degLat = Math.Truncate( Math.Abs( ew.GPSLatitude ) );
                                minLat = Math.Truncate( ( Math.Abs( ew.GPSLatitude ) - degLat ) * 60 );
                                degLon = Math.Abs( Math.Truncate( ew.GPSLongitude ) );
                                minLon = Math.Truncate( ( Math.Abs( ew.GPSLongitude ) - degLon ) * 60 );
                                GPSString = degLat.ToString() + "� " + minLat.ToString() + "' " + ew.GPSLatitudeReference
                                    + Environment.NewLine + degLon.ToString() + "� " +
                                    minLon.ToString() + "' " + ew.GPSLongitudeReference;
                                break;
                            default:
                                GPSString = ew.GPSLatitude.ToString( "0.0000" ) + Environment.NewLine +
                                    ew.GPSLongitude.ToString( "0.0000" );
                                break;
                        }

                        return GPSString;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                    //throw;
                    return "";
                }

            }
        }

        public string KMLGPS
        {
            get
            {
                string kml = "";

                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo( "en-US" );
                kml = ew.GPSLongitude.ToString( "0.00000000", ci ) + "," + ew.GPSLatitude.ToString( "0.00000000", ci ) + "," + ew.GPSAltitude.ToString( "0.00", ci );
                return kml;
            }
        }

        private static string thumbnailPath(string referenceID)
        {
            string ThumbnailPath = ImageFilesFolder + referenceID + ".jpg";
#if !ST_2_1
            //If thumbnails created in ST2 (or earlier versions of the plugin..), keep them
            //TODO: They should be migrated eventually instead
            if (!System.IO.File.Exists(ThumbnailPath))
            {
                string ThumbnailPathST2 = ImageFilesFolderST2 + referenceID + ".jpg";
                if (System.IO.File.Exists(ThumbnailPathST2))
                {
                    return ThumbnailPathST2;
                }
            }
#endif
            return ThumbnailPath;
        }

        public string ThumbnailPath
        {
            get
            {
                if (m_thumbnailPath == null)
                {
                    //Cached, as (ST3) call make fileIO
                    m_thumbnailPath = thumbnailPath(this.referenceID);
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
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
                    // if both locations are not found, nothing will happen
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
                try
                {
                    // A valid image was not found... No valid Exif data exists
                    if ( this.thumbnail == null ) return "";   

                    Length.Units units = Plugin.GetApplication().SystemPreferences.ElevationUnits;
                    string strFormat = String.Format( "N{0}u", Length.DefaultDecimalPrecision( units ) );
                    double Alt = Length.Convert( ew.GPSAltitude, Length.Units.Meter, units );
                    string AltStr = Length.ToString( Alt, units, strFormat );

                    return AltStr;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                    return "";
                }
            }
        }

        public string PhotoSourceFileName
        {
            get
            {
                int i = photosource.LastIndexOf( @"\" );
                if ( i > 0 ) return photosource.Substring( i + 1 );
                else return "";
            }
        }
        #endregion

        #region Private Methods
        public void SetDateTimeOriginal( DateTime dt )
        {
            this.EW.SetPropertyString( (int)( ExifWorks.TagNames.ExifDTOrig ), dt.ToString( Functions.NeutralDateTimeFormat ) );
            SavePhotoSourceProperty( ExifWorks.TagNames.ExifDTOrig );
        }

        private void SavePhotoSourceProperty( ExifWorks.TagNames prop )
        {
            try
            {
                if ( ( System.IO.File.Exists( this.photosource ) ) & ( this.Type == DataTypes.Image ) )
                // Save Exif data to the original source
                {
                    using ( ExifWorks EWPhotoSource = new ExifWorks( this.photosource ) )
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
                        EWPhotoSource.GetBitmap().Save( this.photosource );
                    }
                    //EWPhotoSource.Dispose();
                }

                //Save Exif data to the webfiles image
                this.EW.GetBitmap().Save( this.ThumbnailPath );
                this.EW.Dispose();
                this.EW = new ExifWorks( this.ThumbnailPath );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                //throw;
            }

        }
        #endregion

        #region Public Methods
        public void SetVideoThumbnail()
        {
            Bitmap bmp = null;
            try
            {
                string defpath = this.ThumbnailPath;

                //Check if image on the WebFiles folder exists
                if ( System.IO.File.Exists( defpath ) )
                {
                    bmp = new Bitmap( defpath );
                    //The thumbnail is being created
                    //int width = (int)((double)(bmp.Width) / (double)(bmp.Height) * 50);
                    //this.Thumbnail = bmp.GetThumbnailImage(width, 50, null, new IntPtr());
                    this.Thumbnail = Functions.getThumbnailWithBorder( 50, bmp );
                    bmp.Dispose();
                    bmp = null;
                }
                //File has not yet been created
                else
                {
                    //Check if image at specified PhotoSource location exists
                    if ( System.IO.File.Exists( this.PhotoSource ) )
                    {
                        // Create new image in the default folder
                        bmp = (Bitmap)( Resources.Resources.video ).Clone();
                        Functions.SaveThumbnailImage( bmp, defpath, 10 );
                        //int width = (int)((double)(bmp.Width) / (double)(bmp.Height) * 50);
                        //this.Thumbnail = bmp.GetThumbnailImage(width, 50, null, new IntPtr());
                        this.Thumbnail = Functions.getThumbnailWithBorder( 50, bmp );
                        bmp.Dispose();
                        bmp = null;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                throw;
            }
            finally
            {
                if ( bmp != null )
                    bmp.Dispose();
                bmp = null;
            }
        }

        public void ResetVideoThumbnail()
        {
            string defpath = this.ThumbnailPath;

            //Check if image on the WebFiles folder exists
            if ( System.IO.File.Exists( defpath ) )
            {
                // Create new image in the default folder
                using ( Bitmap bmp = (Bitmap)( Resources.Resources.video ).Clone() )
                {
                    Functions.SaveThumbnailImage( bmp, defpath, 10 );
                    this.Thumbnail = Functions.getThumbnailWithBorder( 50, bmp );
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
                    if ( iFrame == -1 ) bmpOrig = (Bitmap)( Resources.Resources.video ).Clone();
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
                            this.Thumbnail = Functions.getThumbnailWithBorder( 50, bmp );
                            //bmp.Dispose();
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

        internal Bitmap GetDexterAviBmp( string VideoFile, int iFrame, Size frameSize, double dblTimePerFrame )
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
                    g.DrawString( Resources.Resources.groupBoxVideo_Text, f, Brushes.White, new PointF( 0, 0 ) );
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
                        g.DrawString( Resources.Resources.groupBoxVideo_Text, f, Brushes.White, new PointF( 0, 0 ) );
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

        internal void SetThumbnail()
        {
            if (this.Type == ImageData.DataTypes.Image)
            {
                this.SetImageThumbnail();
            }
            else if (this.Type == ImageData.DataTypes.Video)
            {
                SetVideoThumbnail();
            }
        }

        internal void SetImageThumbnail()
        {
            try
            {
                Bitmap bmp;
                string defpath = "";
                defpath = this.ThumbnailPath;

                //Check if image on the WebFiles folder exists
                if ( System.IO.File.Exists( defpath ) )
                {
                    using ( bmp = new Bitmap( defpath ) )
                    {
                        //The thumbnail is being created
                        //int width = (int)((double)(bmp.Width) / (double)(bmp.Height) * 50);
                        //this.Thumbnail = bmp.GetThumbnailImage(width, 50, null, new IntPtr());
                        this.Thumbnail = Functions.getThumbnailWithBorder( 50, bmp );
                    }
                    //bmp.Dispose();
                }
                //File has not yet been created
                else
                {
                    //Check if image at specified PhotoSource location exists
                    if ( System.IO.File.Exists( this.PhotoSource ) )
                    {
                        // Create new image in the default folder
                        Size size = new Size();
                        using ( Bitmap bmpOrig = new Bitmap( this.PhotoSource ) )
                        {
                            int UpperPixelLimit = 500;
                            Double ratio = (double)( bmpOrig.Width ) / (double)( bmpOrig.Height );
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
                            using ( bmp = new Bitmap( bmpOrig, size ) )
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

                                Functions.SaveThumbnailImage( bmp, defpath, 10 );

                                ////is replaced due to smaller file size with jpg + the ability to store more metadata
                                //bmp.Save(defpath,System.Drawing.Imaging.ImageFormat.Png);

                                this.Thumbnail = Functions.getThumbnailWithBorder( 50, bmp );
                            }
                        }
                    }
                    // Thumbnail cannot be created, both target locations are invalid
                    else
                    {
                        bmp = null;
                        //TODO: implement a way to work when images are not found!

                        // Works when no images are found.

                        //MessageBox.Show("both paths not found");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                // throw;
            }
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
            //if ( this.Thumbnail != null ) this.Thumbnail.Dispose();
            // this.Waypoint.Dispose():
        }
        protected virtual void Dispose( bool disposing )
        {
            if ( ( disposing ) && ( this.Thumbnail != null ) )
            {
                this.Thumbnail.Dispose();
                this.Thumbnail = null;
            }
        }
        #endregion
    }

    //serializable extract of the ImageData class. Only this information of each image will be saved with SetExtensionData
    [Serializable()]
    public class ImageDataSerializable
    {
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