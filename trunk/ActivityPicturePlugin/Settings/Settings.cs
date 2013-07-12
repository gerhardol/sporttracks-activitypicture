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
        }
    }
}