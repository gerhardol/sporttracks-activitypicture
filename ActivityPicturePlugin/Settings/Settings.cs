/*

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
using System.Xml;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using ZoneFiveSoftware.Common.Data.Measurement;

namespace ActivityPicturePlugin.Source
{
    class Settings
    {
        static Settings()
        {
            defaults();
        }

        private static int folderView = 0;
        public static int FolderView
        {
            get { return folderView; }
            set
            {
                folderView = value;
            }
        }
        private static int activityView = 0;
        public static int ActivityView
        {
            get { return activityView; }
            set
            {
                activityView = value;
            }
        }
        private static int activityMode = 2;
        public static int ActivityMode
        {
            get { return activityMode; }
            set
            {
                activityMode = value;
            }
        }
        private static int imageZoom = 20;
        public static int ImageZoom
        {
            get { return imageZoom; }
            set
            {
                imageZoom = value;
            }
        }
        private static bool cTypeImage = true;
        public static bool CTypeImage
        {
            get { return cTypeImage; }
            set
            {
                cTypeImage = value;
            }
        }
        private static bool cExifGPS = true;
        public static bool CExifGPS
        {
            get { return cExifGPS; }
            set
            {
                cExifGPS = value;
            }
        }
        private static bool cAltitude = true;
        public static bool CAltitude
        {
            get { return cAltitude; }
            set
            {
                cAltitude = value;
            }
        }
        private static bool cComment = true;
        public static bool CComment
        {
            get { return cComment; }
            set
            {
                cComment = value;
            }
        }
        private static bool cThumbnail = true;
        public static bool CThumbnail
        {
            get { return cThumbnail; }
            set
            {
                cThumbnail = value;
            }
        }
        private static bool cDateTimeOriginal = true;
        public static bool CDateTimeOriginal
        {
            get { return cDateTimeOriginal; }
            set
            {
                cDateTimeOriginal = value;
            }
        }
        private static bool cPhotoTitle = true;
        public static bool CPhotoTitle
        {
            get { return cPhotoTitle; }
            set
            {
                cPhotoTitle = value;
            }
        }
        private static bool cCamera = true;
        public static bool CCamera
        {
            get { return cCamera; }
            set
            {
                cCamera = value;
            }
        }
        private static bool cPhotoSource = true;
        public static bool CPhotoSource
        {
            get { return cPhotoSource; }
            set
            {
                cPhotoSource = value;
            }
        }
        private static bool cReferenceID = true;
        public static bool CReferenceID
        {
            get { return cReferenceID; }
            set
            {
                cReferenceID = value;
            }
        }
        private static uint volumeValue = 80;
        public static uint VolumeValue
        {
            get { return volumeValue; }
            set
            {
                volumeValue = value;
            }
        }
        private static int maxImageSize = 0;
        public static int MaxImageSize
        {
            get { return maxImageSize; }
            set { maxImageSize = value; }
        }
        private static string newThumbnailsCreated = "";
        public static string NewThumbnailsCreated
        {
            get { return newThumbnailsCreated; }
            set { newThumbnailsCreated = value; }
        }
        private static int geSize = 8;
        public static int GESize
        {
            get { return geSize; }
            set { geSize = value; }
        }
        private static int geQuality = 8;
        public static int GEQuality
        {
            get { return geQuality; }
            set { geQuality = value; }
        }
        private static int sortMode = (int)Helper.PictureAlbum.ImageSortMode.byDateTimeAscending;
        public static int SortMode
        {
            get { return sortMode; }
            set { sortMode = value; }
        }
        private static bool geAutoOpen = false;
        public static bool GEAutoOpen
        {
            get { return geAutoOpen; }
            set { geAutoOpen = value; }
        }
        private static bool geStoreFileLocations = true;
        public static bool GEStoreFileLocation
        {
            get { return geStoreFileLocations; }
            set { geStoreFileLocations = value; }
        }
        private static string _LastGeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string LastGeDirectory
        {
            get { return _LastGeDirectory; }
            set { _LastGeDirectory = value; }
        }
        private static string _LastImportDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        public static string LastImportDirectory
        {
            get { return _LastImportDirectory; }
            set { _LastImportDirectory = value; }
        }

        // ImportControl Settings
        private static int importSplitter1Offset = 224;
        public static int ImportSplitter1Offset
        {
            get { return importSplitter1Offset; }
            set { importSplitter1Offset = value; }
        }
        private static int importSplitter2Offset = 202;
        public static int ImportSplitter2Offset
        {
            get { return importSplitter2Offset; }
            set { importSplitter2Offset = value; }
        }
        private static int importSplitter3Offset = 202;
        public static int ImportSplitter3Offset
        {
            get { return importSplitter3Offset; }
            set { importSplitter3Offset = value; }
        }
        private static int settingsSplitter1Offset = 224;
        public static int SettingsSplitter1Offset
        {
            get { return settingsSplitter1Offset; }
            set { settingsSplitter1Offset = value; }
        }
        private static int settingsSplitter2Offset = 202;
        public static int SettingsSplitter2Offset
        {
            get { return settingsSplitter2Offset; }
            set { settingsSplitter2Offset = value; }
        }
        private static int settingsSplitter3Offset = 202;
        public static int SettingsSplitter3Offset
        {
            get { return settingsSplitter3Offset; }
            set { settingsSplitter3Offset = value; }
        }
        private static int listDriveColThumbnailWidth = 150;
        public static int ListDriveColThumbnailWidth
        {
            get { return listDriveColThumbnailWidth; }
            set { listDriveColThumbnailWidth = value; }
        }
        private static int listDriveColDateTimeWidth = 100;
        public static int ListDriveColDateTimeWidth
        {
            get { return listDriveColDateTimeWidth; }
            set { listDriveColDateTimeWidth = value; }
        }
        private static int listDriveColGPSWidth = 120;
        public static int ListDriveColGPSWidth
        {
            get { return listDriveColGPSWidth; }
            set { listDriveColGPSWidth = value; }
        }
        private static int listDriveColTitleWidth = 100;
        public static int ListDriveColTitleWidth
        {
            get { return listDriveColTitleWidth; }
            set { listDriveColTitleWidth = value; }
        }
        private static int listDriveColCommentWidth = 100;
        public static int ListDriveColCommentWidth
        {
            get { return listDriveColCommentWidth; }
            set { listDriveColCommentWidth = value; }
        }
        private static int listActColThumbnailWidth = 150;
        public static int ListActColThumbnailWidth
        {
            get { return listActColThumbnailWidth; }
            set { listActColThumbnailWidth = value; }
        }
        private static int listActColDateTimeWidth = 100;
        public static int ListActColDateTimeWidth
        {
            get { return listActColDateTimeWidth; }
            set { listActColDateTimeWidth = value; }
        }
        private static int listActColGPSWidth = 120;
        public static int ListActColGPSWidth
        {
            get { return listActColGPSWidth; }
            set { listActColGPSWidth = value; }
        }
        private static int listActColTitleWidth = 100;
        public static int ListActColTitleWidth
        {
            get { return listActColTitleWidth; }
            set { listActColTitleWidth = value; }
        }
        private static int listActColCommentWidth = 100;
        public static int ListActColCommentWidth
        {
            get { return listActColCommentWidth; }
            set { listActColCommentWidth = value; }
        }

        public static void defaults()
        {
            folderView = 0;
            activityView = 0;
            activityMode = 2;
            imageZoom = 20;
            cTypeImage = true;
            cExifGPS = true;
            cAltitude = true;
            cComment = true;
            cThumbnail = true;
            cDateTimeOriginal = true;
            cPhotoTitle = true;
            cCamera = true;
            cPhotoSource = true;
            cReferenceID = true;
            volumeValue = 80;
            maxImageSize = 0;
            newThumbnailsCreated = "";
            geSize = 8;
            geQuality = 8;
            sortMode = (int)Helper.PictureAlbum.ImageSortMode.byDateTimeAscending;
            geAutoOpen = false;
            geStoreFileLocations = true;

            //ImportControl Settings
            importSplitter1Offset = 224;
            importSplitter2Offset = 202;
            importSplitter3Offset = 202;
            settingsSplitter1Offset = 224;
            settingsSplitter2Offset = 202;
            settingsSplitter3Offset = 202;
            listDriveColThumbnailWidth = 150;
            listDriveColDateTimeWidth = 100;
            listDriveColGPSWidth = 120;
            listDriveColTitleWidth = 100;
            listDriveColCommentWidth = 100;
            listActColThumbnailWidth = 150;
            listActColDateTimeWidth = 100;
            listActColGPSWidth = 120;
            listActColTitleWidth = 100;
            listActColCommentWidth = 100;

        }

        public static void ReadOptions( XmlDocument xmlDoc, XmlNamespaceManager nsmgr, XmlElement pluginNode )
        {
            String attr;

            attr = pluginNode.GetAttribute( xmlTags.settingsVersion );
            if ( attr.Length > 0 ) { settingsVersion = (Int16)XmlConvert.ToInt16( attr ); }

            attr = pluginNode.GetAttribute( xmlTags.folderView );
            if ( attr.Length > 0 ) { folderView = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.activityView );
            if ( attr.Length > 0 ) { activityView = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.activityMode );
            if ( attr.Length > 0 ) { activityMode = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.imageZoom );
            if ( attr.Length > 0 ) { imageZoom = XmlConvert.ToInt32( attr ); }

            attr = pluginNode.GetAttribute( xmlTags.cTypeImage );
            if ( attr.Length > 0 ) { cTypeImage = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.cExifGPS );
            if ( attr.Length > 0 ) { cExifGPS = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.cAltitude );
            if ( attr.Length > 0 ) { cAltitude = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.cComment );
            if ( attr.Length > 0 ) { cComment = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.cThumbnail );
            if ( attr.Length > 0 ) { cThumbnail = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.cDateTimeOriginal );
            if ( attr.Length > 0 ) { cDateTimeOriginal = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.cPhotoTitle );
            if ( attr.Length > 0 ) { cPhotoTitle = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.cCamera );
            if ( attr.Length > 0 ) { cCamera = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.cPhotoSource );
            if ( attr.Length > 0 ) { cPhotoSource = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.cReferenceID );
            if ( attr.Length > 0 ) { cReferenceID = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.volumeValue );
            if ( attr.Length > 0 ) { volumeValue = XmlConvert.ToUInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.maxImageSize );
            if ( attr.Length > 0 ) { maxImageSize = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.newThumbnailsCreated );
            if ( attr.Length > 0 ) { newThumbnailsCreated = attr; }
            attr = pluginNode.GetAttribute( xmlTags.geSize );
            if ( attr.Length > 0 ) { geSize = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.geQuality );
            if ( attr.Length > 0 ) { geQuality = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.sortMode );
            if ( attr.Length > 0 ) { sortMode = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.geAutoOpen );
            if ( attr.Length > 0 ) { geAutoOpen = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.geStoreFileLocations );
            if ( attr.Length > 0 ) { geStoreFileLocations = XmlConvert.ToBoolean( attr ); }
            attr = pluginNode.GetAttribute(xmlTags.LastGeDirectory);
            if (attr.Length > 0) { LastGeDirectory = attr; }
            attr = pluginNode.GetAttribute(xmlTags.LastImportDirectory);
            if (attr.Length > 0) { LastImportDirectory = attr; }

            //ImportControl Settings
            attr = pluginNode.GetAttribute( xmlTags.importSplitter1Offset );
            if ( attr.Length > 0 ) { importSplitter1Offset = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.importSplitter2Offset);
            if ( attr.Length > 0 ) { importSplitter2Offset = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.importSplitter3Offset);
            if ( attr.Length > 0 ) { importSplitter3Offset = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.settingsSplitter1Offset );
            if ( attr.Length > 0 ) { settingsSplitter1Offset = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.settingsSplitter2Offset );
            if ( attr.Length > 0 ) { settingsSplitter2Offset = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.settingsSplitter3Offset );
            if ( attr.Length > 0 ) { settingsSplitter3Offset = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.listDriveColThumbnailWidth );
            if ( attr.Length > 0 ) { listDriveColThumbnailWidth = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.listDriveColDateTimeWidth);
            if ( attr.Length > 0 ) { listDriveColDateTimeWidth = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.listDriveColGPSWidth );
            if ( attr.Length > 0 ) { listDriveColGPSWidth = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.listDriveColTitleWidth);
            if ( attr.Length > 0 ) { listDriveColTitleWidth = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.listDriveColCommentWidth );
            if ( attr.Length > 0 ) { listDriveColCommentWidth = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.listActColThumbnailWidth );
            if ( attr.Length > 0 ) { listActColThumbnailWidth = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.listActColDateTimeWidth );
            if ( attr.Length > 0 ) { listActColDateTimeWidth = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.listActColGPSWidth );
            if ( attr.Length > 0 ) { listActColGPSWidth = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.listActColTitleWidth );
            if ( attr.Length > 0 ) { listActColTitleWidth = XmlConvert.ToInt32( attr ); }
            attr = pluginNode.GetAttribute( xmlTags.listActColCommentWidth );
            if ( attr.Length > 0 ) { listActColCommentWidth = XmlConvert.ToInt32( attr ); }
        }

        public static void WriteOptions( XmlDocument xmlDoc, XmlElement pluginNode )
        {
            pluginNode.SetAttribute( xmlTags.settingsVersion, XmlConvert.ToString( settingsVersionCurrent ) );

            pluginNode.SetAttribute( xmlTags.folderView, XmlConvert.ToString( folderView ) );
            pluginNode.SetAttribute( xmlTags.activityView, XmlConvert.ToString( activityView ) );
            pluginNode.SetAttribute( xmlTags.activityMode, XmlConvert.ToString( activityMode ) );
            pluginNode.SetAttribute( xmlTags.imageZoom, XmlConvert.ToString( imageZoom ) );

            pluginNode.SetAttribute( xmlTags.cTypeImage, XmlConvert.ToString( cTypeImage ) );
            pluginNode.SetAttribute( xmlTags.cExifGPS, XmlConvert.ToString( cExifGPS ) );
            pluginNode.SetAttribute( xmlTags.cAltitude, XmlConvert.ToString( cAltitude ) );
            pluginNode.SetAttribute( xmlTags.cComment, XmlConvert.ToString( cComment ) );
            pluginNode.SetAttribute( xmlTags.cThumbnail, XmlConvert.ToString( cThumbnail ) );
            pluginNode.SetAttribute( xmlTags.cDateTimeOriginal, XmlConvert.ToString( cDateTimeOriginal ) );
            pluginNode.SetAttribute( xmlTags.cPhotoTitle, XmlConvert.ToString( cPhotoTitle ) );
            pluginNode.SetAttribute( xmlTags.cCamera, XmlConvert.ToString( cCamera ) );
            pluginNode.SetAttribute( xmlTags.cPhotoSource, XmlConvert.ToString( cPhotoSource ) );
            pluginNode.SetAttribute( xmlTags.cReferenceID, XmlConvert.ToString( cReferenceID ) );
            pluginNode.SetAttribute( xmlTags.volumeValue, XmlConvert.ToString( volumeValue ) );
            pluginNode.SetAttribute( xmlTags.maxImageSize, XmlConvert.ToString( maxImageSize ) );
            pluginNode.SetAttribute( xmlTags.newThumbnailsCreated, newThumbnailsCreated );
            pluginNode.SetAttribute( xmlTags.geSize, XmlConvert.ToString( geSize ) );
            pluginNode.SetAttribute( xmlTags.geQuality, XmlConvert.ToString( geQuality ) );
            pluginNode.SetAttribute( xmlTags.sortMode, XmlConvert.ToString( sortMode ) );
            pluginNode.SetAttribute( xmlTags.geAutoOpen, XmlConvert.ToString( geAutoOpen ) );
            pluginNode.SetAttribute(xmlTags.geStoreFileLocations, XmlConvert.ToString(geStoreFileLocations));
            pluginNode.SetAttribute(xmlTags.LastGeDirectory, LastGeDirectory);
            pluginNode.SetAttribute(xmlTags.LastImportDirectory, LastImportDirectory);

            //ImportControl Settings
            pluginNode.SetAttribute( xmlTags.importSplitter1Offset, XmlConvert.ToString( importSplitter1Offset ) );
            pluginNode.SetAttribute( xmlTags.importSplitter2Offset, XmlConvert.ToString( importSplitter2Offset ) );
            pluginNode.SetAttribute( xmlTags.importSplitter3Offset, XmlConvert.ToString( importSplitter3Offset ) );
            pluginNode.SetAttribute( xmlTags.settingsSplitter1Offset, XmlConvert.ToString( settingsSplitter1Offset ) );
            pluginNode.SetAttribute( xmlTags.settingsSplitter2Offset, XmlConvert.ToString( settingsSplitter2Offset ) );
            pluginNode.SetAttribute( xmlTags.settingsSplitter3Offset, XmlConvert.ToString( settingsSplitter3Offset ) );
            pluginNode.SetAttribute( xmlTags.listDriveColThumbnailWidth, XmlConvert.ToString( listDriveColThumbnailWidth ) );
            pluginNode.SetAttribute( xmlTags.listDriveColDateTimeWidth, XmlConvert.ToString( listDriveColDateTimeWidth ) );
            pluginNode.SetAttribute( xmlTags.listDriveColGPSWidth, XmlConvert.ToString( listDriveColGPSWidth ) );
            pluginNode.SetAttribute( xmlTags.listDriveColTitleWidth, XmlConvert.ToString( listDriveColTitleWidth ) );
            pluginNode.SetAttribute( xmlTags.listDriveColCommentWidth, XmlConvert.ToString( listDriveColCommentWidth ) );
            pluginNode.SetAttribute( xmlTags.listActColThumbnailWidth, XmlConvert.ToString( listActColThumbnailWidth ) );
            pluginNode.SetAttribute( xmlTags.listActColDateTimeWidth, XmlConvert.ToString( listActColDateTimeWidth ) );
            pluginNode.SetAttribute( xmlTags.listActColGPSWidth, XmlConvert.ToString( listActColGPSWidth ) );
            pluginNode.SetAttribute( xmlTags.listActColTitleWidth, XmlConvert.ToString( listActColTitleWidth ) );
            pluginNode.SetAttribute( xmlTags.listActColCommentWidth, XmlConvert.ToString( listActColCommentWidth ) );            

        }

        private static int settingsVersion = 0; //default when not existing
        private const int settingsVersionCurrent = 1;

        private class xmlTags
        {
            public const string settingsVersion = "settingsVersion";
            public const string folderView = "folderView";
            public const string activityView = "activityView";
            public const string activityMode = "activityMode";
            public const string imageZoom = "imageZoom";
            public const string cTypeImage = "cTypeImage";
            public const string cExifGPS = "cExifGPS";
            public const string cAltitude = "cAltitude";
            public const string cComment = "cComment";
            public const string cThumbnail = "cThumbnail";
            public const string cDateTimeOriginal = "cDateTimeOriginal";
            public const string cPhotoTitle = "cPhotoTitle";
            public const string cCamera = "cCamera";
            public const string cPhotoSource = "cPhotoSource";
            public const string cReferenceID = "cReferenceID";
            public const string volumeValue = "volumeValue";
            public const string maxImageSize = "maxImageSize";
            public const string newThumbnailsCreated = "newThumbnailsCreated";
            public const string geSize = "geSize";
            public const string geQuality = "geQuality";
            public const string sortMode = "sortMode";
            public const string geAutoOpen = "geAutoOpen";
            public const string geStoreFileLocations = "geStoreFileLocations";
            public const string LastGeDirectory = "LastGeDirectory";
            public const string LastImportDirectory = "LastImportDirectory";

            public const string importSplitter1Offset = "importSplitter1Offset";
            public const string importSplitter2Offset = "importSplitter2Offset";
            public const string importSplitter3Offset = "importSplitter3Offset";
            public const string settingsSplitter1Offset = "settingsSplitter1Offset";
            public const string settingsSplitter2Offset = "settingsSplitter2Offset";
            public const string settingsSplitter3Offset = "settingsSplitter3Offset";
            public const string listDriveColThumbnailWidth = "listDriveColThumbnailWidth";
            public const string listDriveColDateTimeWidth = "listDriveColDateTimeWidth";
            public const string listDriveColGPSWidth = "listDriveColGPSWidth";
            public const string listDriveColTitleWidth = "listDriveColTitleWidth";
            public const string listDriveColCommentWidth = "listDriveColCommentWidth";

            public const string listActColThumbnailWidth = "listActColThumbnailWidth";
            public const string listActColDateTimeWidth = "listActColDateTimeWidth";
            public const string listActColGPSWidth = "listActColGPSWidth";
            public const string listActColTitleWidth = "listActColTitleWidth";
            public const string listActColCommentWidth = "listActColCommentWidth";

        }
    }
}