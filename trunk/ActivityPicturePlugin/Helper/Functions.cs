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
using System.Drawing;
using QuartzTypeLib;
using ActivityPicturePlugin.UI.Activities;
using ActivityPicturePlugin.Properties;
using ActivityPicturePlugin.Settings;
using ZoneFiveSoftware.Common.Data.Fitness;
using System.Xml;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using com.drew.metadata.exif;
using System.Runtime.InteropServices;
using System.Globalization;

namespace ActivityPicturePlugin.Helper
{
    static class Functions
    {
        [DllImport( "shlwapi.dll" )]
        private static extern bool PathCompactPathEx( [Out] StringBuilder pszOut, string szPath, int cchMax, int dwFlags );

        internal static string TruncatePath( string path, int length )
        {
            StringBuilder sb = new StringBuilder();
            PathCompactPathEx( sb, path, length, 0 );
            return sb.ToString();
        }

        internal static readonly string NeutralDateTimeFormat = "yyyy:MM:dd HH:mm:ss"; //textual sortable
        private static readonly string[] ExifExt = { ".jpg", ".jpeg", ".tif", ".tiff" };
        private static readonly string[] ImageExt = { ".jpg", ".jpeg", ".png", ".tif", ".tiff", ".gif", ".bmp" };
        private static readonly string[] VideoExt = { ".avi", ".wmv", ".mpg", ".mpeg", ".mov", ".mp4", ".rm" };

        internal static bool IsExifFileExt( FileInfo file )
        {
            foreach ( string iExt in ExifExt )
            {
                string ext = file.Extension.ToLower();
                if ( ext == iExt )
                {
                    return true;
                }
            }
            return false;
        }

        internal static CultureInfo NeutralToSpecificCulture( string sCultName )
        {
            CultureInfo culture = null;
            if ( CultureInfo.GetCultureInfo( sCultName ).IsNeutralCulture )
                culture = CultureInfo.CreateSpecificCulture( sCultName );
            else
                culture = CultureInfo.GetCultureInfo( sCultName );
            return culture;
        }

        //Return exif original date time
        internal static DateTime GetExifOriginalTime(FileInfo file)
        {
            ExifDirectory ex = null;
            if (Functions.IsExifFileExt(file))
            {
                ex = SimpleRun.ShowOneFileExifDirectory(file.FullName);
            }
            return GetExifOriginalTime(file, ex);
        }

        internal static DateTime GetExifOriginalTime(FileInfo file, ExifDirectory ex)
        {
            string fileTime = null;
            if (ex != null)
            {
                // unspecified timezone
                // ignores current timezone
                // format yyyy:MM:dd HH:mm:ss
                // this doesn't return in UTC.  Is there a way to distinguish which
                // time zone it is?

                //TODO:
                // This may also return an invalid (non-null) date/time.  Do we check here or should the caller?
                fileTime = ex.GetDescription(ExifDirectory.TAG_DATETIME_ORIGINAL);
            }
            if (!string.IsNullOrEmpty(fileTime))
            {
                IFormatProvider culture = CultureInfo.InvariantCulture;
                DateTime dt;

                // fileTime may not be a valid date time.
                // The call above to GetFileTimeString may return in UTC or an unknown timezone
                // If it's unknown it's probably best to assume it's in LocalTime
                // Therefore... maybe it's better if GetFileTimeString returned in Local time
                if (DateTime.TryParseExact(fileTime, NeutralDateTimeFormat, culture, DateTimeStyles.None, out dt))
                {
                    // Ok, now we're returning in LocalTime
                    dt = DateTime.SpecifyKind(dt, DateTimeKind.Local);
                    return dt;
                }
            }

            return DateTime.MinValue;
        }

        public static DateTime GetBestTime(FileInfo fi, DateTime exif, DateTime actStart, DateTime actEnd)
        {
            DateTime time = exif;

            //DateTime actStart = DateTime.MinValue;
            //                        DateTime actEnd = DateTime.MaxValue;
            //            if (activity != null && activity.GPSRoute != null)
            //                    {
            //                        actStart = activity.GPSRoute.StartTime;
            //                        actEnd = activity.GPSRoute.StartTime.AddSeconds(activity.GPSRoute.TotalElapsedSeconds);
            //            }
            if (time < actStart || time > actEnd)
            {
                time = fi.CreationTimeUtc;
                if (time < actStart || time > actEnd)
                {
                    //Try modification, could be better
                    time = fi.LastWriteTimeUtc;
                }
            }
            else
            {
                //MinDate cannot be adjusted to UTC
                time = time.ToUniversalTime();
            }

            // Strip ticks less than a second
            // Makes date comparisons possible
            time = new DateTime( ( (long)( time.Ticks / 10000000 ) ) * 10000000 );

            return time;
        }

        //TODO: Merge these methods. However, the use slightly differs too...
        // Returns in format "yyyy:MM:dd HH:mm:ss", local time
        //internal static string GetFileTimeString(FileInfo file)
        //{
        //    if (string.IsNullOrEmpty(strx))
        //    {
        //        // LocalTime since SimpleRun returns an unspecified timezone
        //        // and it's probably best to assume it's in Local time.
        //        // We should be consistent.
        //        strx = file.CreationTime.ToString( NeutralDateTimeFormat, CultureInfo.InvariantCulture );
        //    }
        //    return strx;
        //}

        public static string DateTimeString(DateTime time)
        {
            //DateTime dt = new DateTime(1950, 1, 1);
            DateTime dt = DateTime.MinValue;
            if (dt < time)
            {
                string strShortDate = "";
                string strShortTime = "";
                System.Globalization.CultureInfo specificCulture = Functions.NeutralToSpecificCulture(System.Globalization.CultureInfo.CurrentUICulture.Name);
                if (specificCulture != null)
                {
                    strShortDate = time.ToString(specificCulture.DateTimeFormat.ShortDatePattern, specificCulture);
                    strShortTime = time.ToString(specificCulture.DateTimeFormat.ShortTimePattern, specificCulture);
                }
                else
                {
                    strShortDate = time.ToString(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                    strShortTime = time.ToString(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);
                }
                return (
                    strShortDate
                    + Environment.NewLine
                    + strShortTime);

            }
            else return "";
        }

        internal static bool IsNormalFile(FileSystemInfo file)
        {
            return ( (file.Attributes & 
                (FileAttributes.Hidden | FileAttributes.Encrypted | FileAttributes.System )) == 0 );
        }

        internal static bool ValidVideoFile( string s )
        {
            try
            {
                FilgraphManagerClass FilGrMan = new FilgraphManagerClass();
                FilGrMan.RenderFile( s );
                FilGrMan = null;
                return true;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                return false;
            }

        }

        internal static void PerformExportToGoogleEarth( List<ImageData> images, ZoneFiveSoftware.Common.Data.Fitness.IActivity act, string SavePath )
        {
            try
            {
                images.Sort( CompareByDate );
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ( "    " );

                string strLongDate = "";
                CultureInfo specificCulture = Functions.NeutralToSpecificCulture( CultureInfo.CurrentUICulture.Name );
                if ( specificCulture != null )
                    strLongDate = act.StartTime.ToLocalTime().ToString( specificCulture.DateTimeFormat.LongDatePattern, specificCulture );
                else
                    strLongDate = act.StartTime.ToLocalTime().ToLongDateString();

                string KMZname = strLongDate + " "
                            + Resources.ImportControl_in + " " + act.Location;

                //string sysFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern;
                //string KMZname = act.StartTime.ToLocalTime().ToString( sysFormat ) + " "
                            //+ Resources.Resources.ImportControl_in + " " + act.Location;
                //string KMZname = act.StartTime.ToLocalTime().ToString( "dd. MMMM,yyyy" ) + " "
                //+ Resources.ImportControl_in + " " + act.Location;
                string KMZstyle = "Photo";

                //using (XmlWriter writer = XmlWriter.Create(Console.Out, settings))
                string docFile = "";
                FileInfo kmzFile = new FileInfo( SavePath );

                String picDir = kmzFile.Directory.ToString() + "\\" + act.ReferenceId;
                if ( kmzFile.Extension == ".kmz" )
                {
                    if ( !System.IO.Directory.Exists( picDir ) ) System.IO.Directory.CreateDirectory( picDir );
                    docFile = kmzFile.Directory.ToString() + "\\doc.kml";
                }
                else
                {
                    docFile = SavePath;
                }

                //writing KML file
                using ( XmlWriter writer = XmlWriter.Create( docFile, settings ) )
                {
                    writer.WriteStartElement( "kml", "http://earth.google.com/kml/2.2" );
                    writer.WriteStartElement( "Document" );

                    //writer.WriteElementString( "name", "SportTracks Images exported with Activity Picture Plugin" );
                    writer.WriteElementString( "name", Resources.SportTracksImagesExportedWith_Text + " " + Resources.ActivityPicturePlugin_Text );
                    writer.WriteElementString( "open", "1" );

                    if ( kmzFile.Extension == ".kml" )
                    {
                        writer.WriteStartElement( "Style" );
                        writer.WriteAttributeString( "id", KMZstyle );
                        writer.WriteElementString( "geomScale", "0.75" );
                        writer.WriteStartElement( "LabelStyle" );
                        writer.WriteElementString( "scale", "0" );
                        writer.WriteEndElement(); //LabelStyle
                        writer.WriteStartElement( "IconStyle" );
                        writer.WriteElementString( "color", "ffffffff" );
                        writer.WriteStartElement( "Icon" );
                        writer.WriteElementString( "href", "root://icons/palette-4.png" );
                        writer.WriteElementString( "x", "192" );
                        writer.WriteElementString( "y", "96" );
                        writer.WriteElementString( "w", "32" );
                        writer.WriteElementString( "h", "32" );
                        writer.WriteEndElement(); //Icon
                        writer.WriteEndElement(); //IconStyle
                        writer.WriteEndElement(); //Style
                    }
                    else
                    {
                        foreach ( ImageData id in images )
                        {
                            // TODO: Need to also check if KMLGPS is valid or change implementation
                            if (id.HasGps())
                            {
                                //stylemaps
                                KMZstyle = id.ReferenceID;
                                WriteStyleMaps( act, KMZstyle, writer );
                            }
                        }
                    }

                    writer.WriteStartElement( "Folder" );
                    writer.WriteElementString( "name", KMZname );
                    writer.WriteElementString( "open", "1" );

                    WriteTrackXML( act, writer );

                    foreach ( ImageData id in images )
                    {
                        // TODO: Need to also check if KMLGPS is valid or change implementation
                        if (id.HasGps())
                        {
                            string KMZfilesource; //source of image (which will be embedded in case of kmz)
                            string KMZLink;
                            if ( kmzFile.Extension == ".kmz" )
                            {
                                CreateKMZImages( picDir, id );
                                KMZfilesource = act.ReferenceId + "/" + id.ReferenceID + ".jpg";
                                KMZLink = ">";
                                KMZstyle = id.ReferenceID;

                            }
                            else
                            {
                                KMZfilesource = id.ThumbnailPath;
                                KMZstyle = "Photo";
                                KMZLink = " href='file://" + id.PhotoSource + "'>";
                            }

                            writer.WriteStartElement( "Placemark" );
                            writer.WriteElementString( "name", id.PhotoSourceFileName );
                            writer.WriteElementString( "Snippet", "" );
                            writer.WriteStartElement( "description" );
                            //int width = (int)( Math.Min( 500, id.Ratio * 500 ) );
                            //int height = (int)( (Single)( width ) / id.Ratio );

                            int width, height;
                            double ratio = id.Ratio;
                            if ( ratio > 1 )
                            {
                                width = ActivityPicturePlugin.Source.Settings.GESize * 50;
                                height = (int)Math.Ceiling( width / ratio );
                            }
                            else
                            {
                                height = ActivityPicturePlugin.Source.Settings.GESize * 50;
                                width = (int)Math.Ceiling( height * ratio );
                            }

                            string KMZpicdescription =
                                //"<P><FONT face=Verdana>"+ KMZname + "</FONT></P>" + 
                                "<P><FONT face=Verdana size=2>"
                            + id.PhotoSourceFileName
                            + "</FONT></P><P><A"
                            + KMZLink
                            + "<IMG height="
                            + height
                            + " alt=" +
                            id.PhotoSourceFileName
                            + " hspace=0 src='"
                            + KMZfilesource
                            + "' width="
                            + width
                            + "></A></P><P><FONT face=Verdana size=2>"
                            + Functions.DateTimeString(id.DateTimeOriginal)
                            + "</FONT></P><P><FONT face=Verdana size=2>"
                            + GPS.GpsString(id.GpsPoint).Replace( Environment.NewLine, ", " )
                            + "</FONT></P><P><FONT face=Verdana size=1>"
                            + String.Format( Resources.CreatedWithXForSportTracks_Text, Resources.ActivityPicturePlugin_Text )
                            + "</FONT></P>";
                            writer.WriteCData( KMZpicdescription );
                            writer.WriteEndElement();//description

                            writer.WriteElementString( "styleUrl", "#" + KMZstyle );
                            writer.WriteStartElement( "Point" );
                            writer.WriteElementString( "altitudeMode", "absolute" );
                            writer.WriteElementString( "extrude", "1" );
                            writer.WriteElementString( "coordinates", id.KMLGPS );
                            writer.WriteEndElement(); //Point
                            writer.WriteEndElement(); //Placemark
                        }
                    }

                    writer.WriteEndElement(); //Folder
                    writer.WriteEndElement(); //Document
                    writer.WriteEndElement(); //kml

                    // Write the XML to file and close the writer.
                    writer.Flush();
                    //writer.Close();
                }


                if ( kmzFile.Extension == ".kmz" )
                {
                    using ( FileStream f = File.Create( SavePath ) )
                    {
                        ZipOutputStream s = new ZipOutputStream( f );

                        s.SetLevel( 6 );
                        s.IsStreamOwner = true;
                        FileInfo fi;

                        string[] filenames = Directory.GetFiles( picDir );

                        foreach ( string file in filenames )
                        {
                            ZipEntry entry = new ZipEntry( act.ReferenceId + "/" + Path.GetFileName( file ) );
                            fi = new FileInfo( file );
                            entry.DateTime = DateTime.Now;
                            entry.Size = fi.Length;
                            s.PutNextEntry( entry );
                            using ( FileStream fs = File.OpenRead( file ) )
                            {
                                byte[] buffer = new byte[fs.Length];
                                fs.Read( buffer, 0, buffer.Length );
                                s.Write( buffer, 0, buffer.Length );
                            }

                        }
                        //add doc.kml
                        ZipEntry doc = new ZipEntry( Path.GetFileName( docFile ) );
                        fi = new FileInfo( docFile );
                        doc.DateTime = DateTime.Now;
                        doc.Size = fi.Length;
                        s.PutNextEntry( doc );
                        using ( FileStream fs = File.OpenRead( docFile ) )
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read( buffer, 0, buffer.Length );
                            s.Write( buffer, 0, buffer.Length );
                        }
                        s.Finish();
                        //s.Close();
                    }
                    Directory.Delete( picDir, true );
                    File.Delete( docFile );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                // File IO can throw a variety of exceptions (Diskspace, No permission to create/delete files/folders, etc)
            }
        }

        private static void WriteTrackXML( ZoneFiveSoftware.Common.Data.Fitness.IActivity act, XmlWriter writer )
        {
            //write track path of route
            writer.WriteStartElement( "Placemark" );
            writer.WriteElementString( "name", "Route" );
            writer.WriteElementString( "visibility", "1" );
            writer.WriteStartElement( "Style" );
            writer.WriteStartElement( "IconStyle" );
            writer.WriteStartElement( "Icon" );
            writer.WriteElementString( "href", "http://www.zonefivesoftware.com/SportTracks/Images/SportTracksIcon48.png" );
            writer.WriteElementString( "w", "48" );
            writer.WriteElementString( "h", "48" );
            writer.WriteEndElement(); //Icon
            writer.WriteEndElement(); //IconStyle
            writer.WriteElementString( "geomScale", "6" );
            writer.WriteElementString( "geomColor", "660000ff" );
            writer.WriteEndElement(); //Style
            writer.WriteStartElement( "LineString" );
            string strCoord = "";
            foreach ( ZoneFiveSoftware.Common.Data.TimeValueEntry<ZoneFiveSoftware.Common.Data.GPS.IGPSPoint> gps in act.GPSRoute )
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo( "en-US" );
                strCoord += gps.Value.LongitudeDegrees.ToString( "0.00000000", ci ) + "," + gps.Value.LatitudeDegrees.ToString( "0.00000000", ci ) + ",30" + Environment.NewLine;
            }
            writer.WriteElementString( "coordinates", strCoord );
            writer.WriteEndElement(); //LineString
            writer.WriteEndElement(); //Placemark
        }

        internal static void PerformMultipleExportToGoogleEarth( IList<ZoneFiveSoftware.Common.Data.Fitness.IActivity> acts, string SavePath )
        {
            try
            {
                bool ImageFound = false; //if no images found, doc file will not be created

                string KMZstyle = "Photo";
                FileInfo kmzFile = new FileInfo( SavePath );

                string docFile = SavePath;
                if ( kmzFile.Extension == ".kmz" ) docFile = kmzFile.Directory.ToString() + "\\doc.kml";

                //string sysFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern;

                //start writing xml file
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ( "    " );
                using ( XmlWriter writer = XmlWriter.Create( docFile, settings ) )
                {
                    writer.WriteStartElement( "kml", "http://earth.google.com/kml/2.2" );
                    writer.WriteStartElement( "Document" );

                    //writer.WriteElementString( "name", "SportTracks Images exported with Activity Picture Plugin" );
                    writer.WriteElementString( "name", Resources.SportTracksImagesExportedWith_Text + " " + Resources.ActivityPicturePlugin_Text );
                    writer.WriteElementString( "open", "1" );

                    if ( kmzFile.Extension == ".kml" )
                    {
                        writer.WriteStartElement( "Style" );
                        writer.WriteAttributeString( "id", KMZstyle );
                        writer.WriteElementString( "geomScale", "0.75" );
                        writer.WriteStartElement( "LabelStyle" );
                        writer.WriteElementString( "scale", "0" );
                        writer.WriteEndElement(); //LabelStyle
                        writer.WriteStartElement( "IconStyle" );
                        writer.WriteElementString( "color", "ffffffff" );
                        writer.WriteStartElement( "Icon" );
                        writer.WriteElementString( "href", "root://icons/palette-4.png" );
                        writer.WriteElementString( "x", "192" );
                        writer.WriteElementString( "y", "96" );
                        writer.WriteElementString( "w", "32" );
                        writer.WriteElementString( "h", "32" );
                        writer.WriteEndElement(); //Icon
                        writer.WriteEndElement(); //IconStyle
                        writer.WriteEndElement(); //Style
                    }

                    foreach ( IActivity act in acts )
                    {
                        //Get image data
                        List<ImageData> images = new List<ImageData>();
                        images.Sort( CompareByDate );
                        PluginData pd = ReadExtensionData( act );
                        if ( pd.Images.Count != 0 )
                        {
                            images = pd.LoadImageData( pd.Images, act );
                            ImageFound = true;
                        }
                        else continue; //if no images, continue to next activity

                        //Activity contains images
                        //string KMZname = act.StartTime.ToLocalTime().ToString( "dd. MMMM,yyyy" ) + " "
                        //+ Resources.ImportControl_in + " " + act.Location;

                        string strLongDate = "";
                        CultureInfo specificCulture = Functions.NeutralToSpecificCulture( CultureInfo.CurrentUICulture.Name );
                        if ( specificCulture != null )
                            strLongDate = act.StartTime.ToLocalTime().ToString( specificCulture.DateTimeFormat.LongDatePattern, specificCulture );
                        else
                            strLongDate = act.StartTime.ToLocalTime().ToLongDateString();

                        string KMZname = strLongDate + " "
                                    + Resources.ImportControl_in + " " + act.Location;

                        //string KMZname = act.StartTime.ToLocalTime().ToString( sysFormat ) + " "
                                    //+ Resources.Resources.ImportControl_in + " " + act.Location;

                        String picDir = kmzFile.Directory.ToString() + "\\" + act.ReferenceId;
                        if ( kmzFile.Extension == ".kmz" )
                        {
                            if ( !System.IO.Directory.Exists( picDir ) ) System.IO.Directory.CreateDirectory( picDir );
                        }

                        //writing KML file for activity
                        if ( kmzFile.Extension == ".kmz" )
                        {
                            foreach ( ImageData id in images )
                            {
                                // TODO: Need to also check if KMLGPS is valid or change implementation
                                if (id.HasGps())
                                {
                                    //stylemaps
                                    KMZstyle = id.ReferenceID;
                                    WriteStyleMaps( act, KMZstyle, writer );
                                }
                            }
                        }

                        writer.WriteStartElement( "Folder" );
                        writer.WriteElementString( "name", KMZname );
                        writer.WriteElementString( "open", "1" );

                        WriteTrackXML( act, writer );

                        foreach ( ImageData id in images )
                        {
                            // TODO: Need to also check if KMLGPS is valid or change implementation
                            if (id.HasGps())
                            {
                                string KMZfilesource; //source of image (which will be embedded in case of kmz)
                                string KMZLink;
                                if ( kmzFile.Extension == ".kmz" )
                                {
                                    CreateKMZImages( picDir, id );
                                    KMZfilesource = act.ReferenceId + "/" + id.ReferenceID + ".jpg";
                                    KMZLink = ">";
                                    KMZstyle = id.ReferenceID;

                                }
                                else
                                {
                                    KMZfilesource = id.ThumbnailPath;
                                    KMZstyle = "Photo";
                                    KMZLink = " href='file://" + id.PhotoSource + "'>";
                                }

                                writer.WriteStartElement( "Placemark" );
                                writer.WriteElementString( "name", id.PhotoSourceFileName );
                                writer.WriteElementString( "Snippet", "" );
                                writer.WriteStartElement( "description" );
                                //int width = (int)( Math.Min( 500, id.Ratio * 500 ) );
                                //int height = (int)( (Single)( width ) / id.Ratio );

                                int width, height;
                                double ratio = id.Ratio;
                                if ( ratio > 1 )
                                {
                                    width = ActivityPicturePlugin.Source.Settings.GESize * 50;
                                    height = (int)Math.Ceiling( width / ratio );
                                }
                                else
                                {
                                    height = ActivityPicturePlugin.Source.Settings.GESize * 50;
                                    width = (int)Math.Ceiling( height * ratio );
                                }

                                string KMZpicdescription = "<P><FONT face=Verdana>"
                                + KMZname
                                + "</FONT></P><P><FONT face=Verdana size=2>"
                                + id.PhotoSourceFileName
                                + "</FONT></P><P><A"
                                + KMZLink
                                + "<IMG height="
                                + height
                                + " alt=" +
                                id.PhotoSourceFileName
                                + " hspace=0 src='"
                                + KMZfilesource
                                + "' width="
                                + width
                                + "></A></P><P><FONT face=Verdana size=2>"
                                + Functions.DateTimeString(id.DateTimeOriginal)
                                + "</FONT></P><P><FONT face=Verdana size=2>"
                                + GPS.GpsString(id.GpsPoint).Replace( Environment.NewLine, ", " )
                                + "</FONT></P><P><FONT face=Verdana size=1>"
                                + String.Format( Resources.CreatedWithXForSportTracks_Text, Resources.ActivityPicturePlugin_Text )
                                + "</FONT></P>";
                                writer.WriteCData( KMZpicdescription );
                                writer.WriteEndElement();//description

                                writer.WriteElementString( "styleUrl", "#" + KMZstyle );
                                writer.WriteStartElement( "Point" );
                                writer.WriteElementString( "altitudeMode", "absolute" );
                                writer.WriteElementString( "extrude", "1" );
                                writer.WriteElementString( "coordinates", id.KMLGPS );
                                writer.WriteEndElement(); //Point
                                writer.WriteEndElement(); //Placemark
                            }
                        }
                        writer.WriteEndElement(); //Folder
                    }

                    writer.WriteEndElement(); //Document
                    writer.WriteEndElement(); //kml

                    // Write the XML to file and close the writer.
                    writer.Flush();
                    //writer.Close();
                }


                if ( !ImageFound ) //no image at all found
                {
                    File.Delete( Path.GetFileName( docFile ) );
                    return;
                }


                // create zip file
                if ( kmzFile.Extension == ".kmz" )
                {
                    using ( FileStream f = File.Create( SavePath ) )
                    {
                        ZipOutputStream s = new ZipOutputStream( f );
                        s.SetLevel( 6 );
                        s.IsStreamOwner = true;
                        FileInfo fi;

                        //create zip files for images
                        foreach ( IActivity act in acts )
                        {
                            if ( Directory.Exists( kmzFile.Directory.ToString() + "\\" + act.ReferenceId ) ) //exists only if act. contains images
                            {
                                string[] filenames = Directory.GetFiles( kmzFile.Directory.ToString() + "\\" + act.ReferenceId );
                                foreach ( string file in filenames )
                                {
                                    ZipEntry entry = new ZipEntry( act.ReferenceId + "/" + Path.GetFileName( file ) );
                                    fi = new FileInfo( file );
                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fi.Length;
                                    s.PutNextEntry( entry );
                                    using ( FileStream fs = File.OpenRead( file ) )
                                    {
                                        byte[] buffer = new byte[fs.Length];
                                        fs.Read( buffer, 0, buffer.Length );
                                        s.Write( buffer, 0, buffer.Length );
                                    }
                                }
                                Directory.Delete( kmzFile.Directory.ToString() + "\\" + act.ReferenceId, true );
                            }
                        }

                        //add doc.kml
                        ZipEntry doc = new ZipEntry( Path.GetFileName( docFile ) );
                        fi = new FileInfo( docFile );
                        doc.DateTime = DateTime.Now;
                        doc.Size = fi.Length;
                        s.PutNextEntry( doc );
                        using ( FileStream fs = File.OpenRead( docFile ) )
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read( buffer, 0, buffer.Length );
                            s.Write( buffer, 0, buffer.Length );
                        }
                        s.Finish();
                        //s.Close();
                    }
                    File.Delete( Path.GetFileName( docFile ) );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                // File IO can throw a variety of exceptions (Diskspace, No permission to create/delete files/folders, etc)
            }
        }

        private static void CreateKMZImages( String picDir, ImageData id )
        {
            //create small thumbnail
            using ( Bitmap bmp = CreateSmallThumbnail( id, 108 ) )
            {
                Functions.SaveThumbnailImage( bmp, picDir + "\\" + id.ReferenceID + "_small.jpg", ActivityPicturePlugin.Source.Settings.GEQuality );	// ActivityPicturePageControl.PluginSettingsData.data.Quality );
            }

            if ( !File.Exists( picDir + "\\" + id.ReferenceID + ".jpg" ) )
            //copy image from webfiles folder to the image folder of the zip archive (if not already exist)
            {
                using ( Bitmap bmp = CreateSmallThumbnail( id, (int)( ActivityPicturePlugin.Source.Settings.GESize * 50 * 0.75 ) ) )
                {
                    Functions.SaveThumbnailImage( bmp, picDir + "\\" + id.ReferenceID + ".jpg", ActivityPicturePlugin.Source.Settings.GEQuality );	// ActivityPicturePageControl.PluginSettingsData.data.Quality );
                    //System.IO.File.Copy(id.ReferenceIDPath, picDir + "\\" + id.ReferenceID + ".jpg");
                }
            }
        }

        private static Bitmap CreateSmallThumbnail( ImageData id, int minSize )
        {
            Bitmap bmp = null;
            Bitmap bmpNew = null;
            //if (minSize > 150)
            //{
            //    if (File.Exists(id.PhotoSource)) bmp = new Bitmap(id.PhotoSource);
            //}

            try
            {
                //minsize<=150 or original image not found
                if ( bmp == null )
                    bmp = new Bitmap( id.ThumbnailPath );

                int Swidth, Sheight;
                double ratio = (double)( bmp.Width ) / (double)( bmp.Height );
                if ( ratio > 1 )
                {
                    Swidth = (int)( minSize * ratio );
                    Sheight = minSize;
                }
                else
                {
                    Swidth = minSize;
                    Sheight = (int)( minSize / ratio );
                }
                Size size = new Size( Swidth, Sheight );
                bmpNew = new Bitmap( bmp, size );

                //copying the metadata of the original file into the new image
                foreach ( System.Drawing.Imaging.PropertyItem pItem in bmp.PropertyItems )
                {
                    try
                    {
                        //Mono TODO: NotImplemented
                        bmpNew.SetPropertyItem( pItem );
                    }
                    catch { }
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

            //quality too bad
            //bmp = (Bitmap)(bmp.GetThumbnailImage(Swidth, Sheight, null, new IntPtr()));
            return bmpNew;
        }

        private static void WriteStyleMaps( ZoneFiveSoftware.Common.Data.Fitness.IActivity act, string KMZstyle, XmlWriter writer )
        {
            writer.WriteStartElement( "StyleMap" );
            writer.WriteAttributeString( "id", KMZstyle );
            writer.WriteStartElement( "Pair" );
            writer.WriteElementString( "key", "normal" );
            writer.WriteElementString( "styleUrl", "#" + KMZstyle + "_norm" );
            writer.WriteEndElement(); //Pair
            writer.WriteStartElement( "Pair" );
            writer.WriteElementString( "key", "highlight" );
            writer.WriteElementString( "styleUrl", "#" + KMZstyle + "_high" );
            writer.WriteEndElement(); //Pair
            writer.WriteEndElement(); //StyleMap

            writer.WriteStartElement( "Style" );
            writer.WriteAttributeString( "id", KMZstyle + "_norm" );
            writer.WriteStartElement( "IconStyle" );
            writer.WriteStartElement( "Icon" );
            writer.WriteElementString( "href", act.ReferenceId + "/" + KMZstyle + "_small.jpg" );
            writer.WriteEndElement(); //Icon
            writer.WriteEndElement(); //IconStyle
            writer.WriteStartElement( "LabelStyle" );
            writer.WriteElementString( "scale", "0" );
            writer.WriteEndElement(); //LabelStyle
            writer.WriteStartElement( "BalloonStyle" );
            writer.WriteElementString( "text", "$[description]" );
            writer.WriteEndElement(); //BalloonStyle
            writer.WriteEndElement(); //Style


            writer.WriteStartElement( "Style" );
            writer.WriteAttributeString( "id", KMZstyle + "_high" );
            writer.WriteStartElement( "IconStyle" );
            writer.WriteElementString( "scale", "2" );
            writer.WriteStartElement( "Icon" );
            writer.WriteElementString( "href", act.ReferenceId + "/" + KMZstyle + "_small.jpg" );
            writer.WriteEndElement(); //Icon
            writer.WriteEndElement(); //IconStyle
            writer.WriteStartElement( "BalloonStyle" );
            writer.WriteElementString( "text", "$[description]" );
            writer.WriteEndElement(); //BalloonStyle
            writer.WriteEndElement(); //Style
        }

        internal static byte[] GetGPSByteValue( double value )
        {
            byte[] val = new byte[24];
            Int32 d1 = (Int32)( value ); //degree
            Int32 d1den = 1;
            Int32 d2 = (Int32)( ( value - d1 ) * 60 ); //minutes
            Int32 d2den = 1;
            Int32 d3 = (Int32)( ( value - d1 - (double)( d2 ) / 60 ) * 60 * 60 * 100 ); //seconds with 2 digits after comma
            Int32 d3den = 100;
            BitConverter.GetBytes( d1 ).CopyTo( val, 0 );
            BitConverter.GetBytes( d1den ).CopyTo( val, 4 );
            BitConverter.GetBytes( d2 ).CopyTo( val, 8 );
            BitConverter.GetBytes( d2den ).CopyTo( val, 12 );
            BitConverter.GetBytes( d3 ).CopyTo( val, 16 );
            BitConverter.GetBytes( d3den ).CopyTo( val, 20 );
            return val;
        }

        internal static double GetGPSDoubleValue( byte[] val )
        {
            Int32 d1 = BitConverter.ToInt32( val, 0 );
            Int32 d1den = BitConverter.ToInt32( val, 4 );
            Int32 d2 = BitConverter.ToInt32( val, 8 );
            Int32 d2den = BitConverter.ToInt32( val, 12 );
            Int32 d3 = BitConverter.ToInt32( val, 16 );
            Int32 d3den = BitConverter.ToInt32( val, 20 );
            double d = (double)( d1 ) / (double)( d1den ) + (double)( d2 ) / (double)( d2den ) / 60 + (double)( d3 ) / (double)( d3den ) / 60 / 60;
            return d;
        }

        //public static bool ValidImageFile(string p)
        //    {
        //    try
        //        {
        //        Image img = new Bitmap(p);
        //        img.Dispose();
        //        return true;
        //        }
        //    catch (Exception ex)
        //        {
        //        System.Diagnostics.Debug.Assert(false, ex.Message);
        //        return false;
        //        }
        //    }

        public static bool OpenExternal( string sFile )
        {
            bool ret = true;
            try
            {
                if ( ( new FileInfo( sFile ).Exists ) || ( new DirectoryInfo( sFile ).Exists ) )
                    System.Diagnostics.Process.Start( sFile );
                else
                    ret = false;
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.Assert( false, ex.Message );
                ret = false;
            }
            return ret;
        }

        public static bool OpenExternal( ImageData im )
        {
            bool ret = true;
            if ( im.Type == ImageData.DataTypes.Image )
            {
                try
                {
                    string sPath = im.GetBestImage();
                    if ( sPath != null ) System.Diagnostics.Process.Start( sPath );
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                    ret = false;
                }
            }
            else if ( im.Type == ImageData.DataTypes.Video )
            {
                try
                {
                    //Functions.OpenVideoInExternalWindow( im.PhotoSource );
                    System.Diagnostics.Process.Start( im.PhotoSource );

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                    ret = false;
                }
            }
            return ret;
        }

        public static void OpenExternal(string path, ImageData.DataTypes dt)
        {
            if (dt == ImageData.DataTypes.Image)
            {
                Functions.OpenExternal(path);
            }
            else if (dt == ImageData.DataTypes.Video)
            {
                Functions.OpenVideoInExternalWindow(path);
            }
        }

        //private static void OpenImageWithWindowsViewer( string ImageLocation )
        //{
        //    try
        //    {
        //        //show picture with windows
        //        string sys = System.Environment.GetFolderPath( Environment.SpecialFolder.System );
        //        System.Diagnostics.ProcessStartInfo f = new System.Diagnostics.ProcessStartInfo
        //        ( sys + "\\rundll32.exe",
        //        sys + "\\shimgvw.dll,ImageView_Fullscreen " +
        //        ImageLocation );
        //        System.Diagnostics.Process.Start( f );
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.Assert(false, ex.Message);
        //    }
        //}

        private static void OpenVideoInExternalWindow( string p )
        {
            try
            {
                if ( p != null ) System.Diagnostics.Process.Start( p );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }

            /*try
            {
                FilgraphManager graphManager = new FilgraphManager();
                // QueryInterface for the IMediaControl interface:
                IMediaControl mc = (IMediaControl)graphManager;
                // Call some methods on a COM interface 
                // Pass in file to RenderFile method on COM object. 
                mc.RenderFile(p);
                // Show file. 
                mc.Run();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
            Console.WriteLine("Press Enter to continue.");*/
        }

        public static void WriteExtensionData( ZoneFiveSoftware.Common.Data.Fitness.IActivity act, PluginData pd )
        {
            try
            {
                PluginData pd0 = ReadExtensionData( act );

                if ( !pd.Equals( pd0 ) )
                {
                    //store data of images in the serializable wrapper class
                    if ( ( pd.Images.Count == 0 ) && ( pd.GELinks.Count == 0 ) )
                    {
                        act.SetExtensionData( ActivityPicturePlugin.GUIDs.PluginMain, null );
                        act.SetExtensionText( ActivityPicturePlugin.GUIDs.PluginMain, "" );
                    }
                    else
                    {
                        using ( System.IO.MemoryStream mem = new System.IO.MemoryStream() )
                        {
                            System.Xml.Serialization.XmlSerializer xmlSer = new System.Xml.Serialization.XmlSerializer( typeof( PluginData ) );
                            xmlSer.Serialize( mem, pd );
                            act.SetExtensionData( ActivityPicturePlugin.GUIDs.PluginMain, mem.ToArray() );
                            act.SetExtensionText( ActivityPicturePlugin.GUIDs.PluginMain, "Picture Plugin" );
                        }
                    }

                    ActivityPicturePlugin.Plugin.GetApplication().Logbook.Modified = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                throw;
            }
        }

        public static PluginData ReadExtensionData( ZoneFiveSoftware.Common.Data.Fitness.IActivity act )
        {
            try
            {
                if ( act == null ) return new PluginData();

                PluginData pd;
                byte[] b = act.GetExtensionData( ActivityPicturePlugin.GUIDs.PluginMain );
                if ( !( b.Length == 0 ) )
                {
                    System.Xml.Serialization.XmlSerializer xmlSer = new System.Xml.Serialization.XmlSerializer( typeof( PluginData ) );
                    using ( System.IO.MemoryStream mem = new System.IO.MemoryStream() )
                    {
                        mem.Write( b, 0, b.Length );
                        mem.Position = 0;
                        pd = (PluginData)xmlSer.Deserialize( mem );
                    }
                    xmlSer = null;
                    b = null;
                    foreach (ImageDataSerializable ids in pd.Images)
                    {
                        //Old plugin bugs(?) left type unset?
                        if (ids.Type == ImageData.DataTypes.Nothing)
                        {
                            ids.Type = Functions.GetMediaType(ids.PhotoSource);
                        }
                    }
                    return pd;
                }
                else
                {
                    return new PluginData();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                return new PluginData();
           }

        }

        internal static void SaveThumbnailImage( Bitmap bmp, string defpath, Int64 quality )
        {
            try
            {
                System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                System.Drawing.Imaging.ImageCodecInfo codec = null;
                for ( int i = 0; i < codecs.Length; i++ )
                {
                    if ( codecs[i].MimeType.Equals( "image/jpeg" ) )
                    {
                        codec = codecs[i];
                        break;
                    }
                }
                if ( codec != null )
                {
                    //save with the changed settings (quality, color depth)
                    System.Drawing.Imaging.Encoder encoderInstance = System.Drawing.Imaging.Encoder.Quality;
                    System.Drawing.Imaging.EncoderParameters encoderParametersInstance;
                    using ( encoderParametersInstance = new System.Drawing.Imaging.EncoderParameters( 2 ) )
                    {
                        //100% quality
                        if ( quality < 1 | quality > 10 ) quality = 10; //check for wrong input;
                        System.Drawing.Imaging.EncoderParameter encoderParameterInstance = new System.Drawing.Imaging.EncoderParameter( encoderInstance, (long)( quality * 10 ) );
                        encoderParametersInstance.Param[0] = encoderParameterInstance;
                        encoderInstance = System.Drawing.Imaging.Encoder.ColorDepth;
                        //24bit color depth
                        encoderParameterInstance = new System.Drawing.Imaging.EncoderParameter( encoderInstance, 24L );
                        encoderParametersInstance.Param[1] = encoderParameterInstance;
                        bmp.Save( defpath, codec, encoderParametersInstance );
                    }
                }
                else
                //save with the default settings
                {
                    bmp.Save( defpath, System.Drawing.Imaging.ImageFormat.Jpeg );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }

        }

        internal static void GeoTagFromGps( string filepath, ZoneFiveSoftware.Common.Data.GPS.IGPSPoint gps )
        {
            try
            {
                using ( ExifWorks EW = new ExifWorks( filepath ) )
                {
                    EW.GPSLatitude = gps.LatitudeDegrees;
                    EW.GPSLongitude = gps.LongitudeDegrees;
                    EW.GPSAltitude = gps.ElevationMeters;
                    // Save Image with new Exif data

                    EW.GetBitmap().Save( filepath );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }

        }

        //TODO: Avoid read the embedded thumbnail here?
        internal static Image getThumbnailWithBorder(int width, Image img)
        {
            Image thumb = null;
            Image tmp = null;
            try
            {
                thumb = new Bitmap( width, width );
                tmp = null;
                //If the original image is small than the Thumbnail size, just draw in the center
                if ( img.Width < width && img.Height < width )
                {
                    using ( Graphics g = Graphics.FromImage( thumb ) )
                    {
                        int xoffset = (int)( ( width - img.Width ) / 2 );
                        int yoffset = (int)( ( width - img.Height ) / 2 );
                        g.DrawImage( img, xoffset, yoffset, img.Width, img.Height );
                    }
                }
                else //Otherwise we have to get the thumbnail for drawing
                {
                    Image.GetThumbnailImageAbort myCallback = new
                        Image.GetThumbnailImageAbort( ThumbnailCallback );
                    if ( img.Width == img.Height )
                    {
                        if ( thumb != null )
                            thumb.Dispose();
                        thumb = null;
                        thumb = img.GetThumbnailImage(
                                 width, width,
                                 myCallback, IntPtr.Zero );
                    }
                    else
                    {
                        int k = 0;
                        int xoffset = 0;
                        int yoffset = 0;
                        if ( img.Width < img.Height )
                        {
                            k = (int)( width * img.Width / img.Height );
                            tmp = img.GetThumbnailImage( k, width, myCallback, IntPtr.Zero );
                            xoffset = (int)( ( width - k ) / 2 );
                        }
                        if ( img.Width > img.Height )
                        {
                            k = (int)( width * img.Height / img.Width );
                            tmp = img.GetThumbnailImage( width, k, myCallback, IntPtr.Zero );
                            yoffset = (int)( ( width - k ) / 2 );
                        }
                        using ( Graphics g = Graphics.FromImage( thumb ) )
                        {
                            g.DrawImage( tmp, xoffset, yoffset, tmp.Width, tmp.Height );
                        }
                    }
                }
                using ( Graphics g = Graphics.FromImage( thumb ) )
                {
                    g.DrawRectangle( Pens.Black, 0, 0, thumb.Width - 1, thumb.Height - 1 );
                }
                if (tmp != null)
                {
                    tmp.Dispose();
                    tmp = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                if (thumb != null)
                {
                    thumb.Dispose();
                    thumb = null;
                }

                if (tmp != null)
                {
                    tmp.Dispose();
                    tmp = null;
                }

                return null;
            }
            return thumb;
        }

        internal static bool ThumbnailCallback()
        {
            return true;
        }

        internal static List<string> GetThumbnailPathsAllActivities()
        {
            List<string> s = new List<string>();

            IEnumerable<IActivity> activities = new List<IActivity>();
            activities = ActivityPicturePlugin.Plugin.GetApplication().Logbook.Activities;

            foreach (IActivity activity in activities)
            {
                PluginData data = Helper.Functions.ReadExtensionData(activity);
                foreach(ImageDataSerializable ids in data.Images)
                {
                    s.Add(ImageData.thumbnailPath(ids.ReferenceID));
                }
            }
            return s;
        }

        internal static ImageData.DataTypes GetMediaType(string p)
        {
            if ( !string.IsNullOrEmpty( p ) )
            {
                System.IO.FileInfo fi = new FileInfo( p );
                //if ( p.Length > 4 ) p = p.Substring( p.Length - 4 );
                foreach ( string str in ImageExt )
                {
                    //if ( str == p.ToLower() ) return ImageData.DataTypes.Image;
                    if ( str == fi.Extension.ToLower() ) return ImageData.DataTypes.Image;
                }
                foreach ( string str in VideoExt )
                {
                    //if ( str == p.ToLower() ) return ImageData.DataTypes.Video;
                    if ( str == fi.Extension.ToLower() ) return ImageData.DataTypes.Video;
                }
            }
            return ImageData.DataTypes.Nothing;
        }

        internal static int CompareByDate( ImageData x, ImageData y )
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
                        retval = x.DateTimeOriginal.CompareTo(y.DateTimeOriginal);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }

            return retval;
        }

        internal static string UppercaseFirst( string s )
        {
            // Check for empty string.
            if ( string.IsNullOrEmpty( s ) )
                return string.Empty;

            // Return char and concat substring.
            if ( s.Length > 1 )
                return char.ToUpper( s[0] ) + s.Substring( 1 );
            else return s.ToUpper();
        }

        internal static string CapitalizeAllWords( string s )
        {
            // Check for empty string.
            if ( string.IsNullOrEmpty( s ) )
                return string.Empty;

            string ret = "";
            string[] split = s.Split( ' ' );
            foreach ( string word in split )
            {
                // Capitalize char and concat substring.
                if ( word.Length > 1 )
                    ret += char.ToUpper( word[0] ) + word.Substring( 1 );
                else ret += word.ToUpper();
                ret += " ";
            }
            // remove the trailing space.
            if ( ret.Length > 0 ) ret = ret.Substring( 0, ret.Length - 1 );
            return ret;
        }

    }
}