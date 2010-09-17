using System;
using System.Text;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using com.drew.metadata;
using com.drew.metadata.iptc;
using com.drew.imaging.jpg;
using com.drew.metadata.exif;

/// <summary>
/// The C# class was made by Ferret Renaud: 
/// <a href="mailto:renaud91@free.fr">renaud91@free.fr</a>
/// If you find a bug in the C# code, feel free to mail me.
/// </summary>
namespace com
    {
    /// <summary>
    /// This class is a simple example of how to use the classes inside this project.
    /// </summary>
    public sealed class SimpleRun
        {
        /// <summary>
        /// Shows all metadata and all tag for one file.
        /// </summary>
        /// <param name="aFileName">the image file name (ex: c:/temp/a.jpg)</param>
        /// <returns>The information about the image as a string</returns>
        public static String ShowOneFileAllMetaDataAllTag(string aFileName)
            {
            Metadata lcMetadata = null;
            try
                {
                FileInfo lcImgFile = new FileInfo(aFileName);
                // Loading all meta data
                lcMetadata = JpegMetadataReader.ReadMetadata(lcImgFile);
                }
            catch (JpegProcessingException e)
                {
                Console.Error.WriteLine(e.Message);
                return "Error";
                }

            // Now try to print them
            StringBuilder lcBuff = new StringBuilder(1024);
            lcBuff.Append("---> ").Append(aFileName).Append(" <---").AppendLine();
            // We want all directory, so we iterate on each
            IEnumerator<AbstractDirectory> lcDirectoryEnum = lcMetadata.GetDirectoryIterator();
            while (lcDirectoryEnum.MoveNext())
                {
                // We get the current directory
                AbstractDirectory lcDirectory = lcDirectoryEnum.Current;
                // We look for potentiel error
                IEnumerator<string> lcErrorsEnum = lcDirectory.GetErrors();
                while (lcErrorsEnum.MoveNext())
                    {
                    Console.Error.WriteLine("Error Found: " + lcErrorsEnum.Current);
                    }
                lcBuff.Append("---+ ").Append(lcDirectory.GetName()).AppendLine();
                // Then we want all tags, so we iterate on the current directory
                IEnumerator<Tag> lcTagsIterator = lcDirectory.GetTagIterator();
                while (lcTagsIterator.MoveNext())
                    {
                    Tag lcTag = lcTagsIterator.Current;
                    string lcTagDescription = null;
                    try
                        {
                        lcTagDescription = lcTag.GetDescription();
                        }
                    catch (MetadataException e)
                        {
                        Console.Error.WriteLine(e.Message);
                        }
                    string lcTagName = lcTag.GetTagName();
                    lcBuff.Append(lcTagName).Append('=').Append(lcTagDescription).AppendLine();

                    lcTag = null;
                    lcTagDescription = null;
                    lcTagName = null;
                    }
                lcDirectory = null;
                lcTagsIterator = null;
                }
            lcDirectoryEnum = null;
            lcMetadata = null;

            return lcBuff.ToString();
            }

        /// <summary>
        /// Shows only IPTC directory and all of its tag for one file.
        /// </summary>
        /// <param name="aFileName">the image file name (ex: c:/temp/a.jpg)</param>
        /// <returns>The information about IPTC for this image as a string</returns>
        public static String ShowOneFileOnlyIptcAllTag(string aFileName)
            {
            Metadata lcMetadata = null;
            try
                {
                FileInfo lcImgFile = new FileInfo(aFileName);
                // Loading all meta data
                lcMetadata = JpegMetadataReader.ReadMetadata(lcImgFile);
                }
            catch (JpegProcessingException e)
                {
                Console.Error.WriteLine(e.Message);
                return "Error";
                }

            // Now try to print them
            StringBuilder lcBuff = new StringBuilder(1024);
            lcBuff.Append("---> ").Append(aFileName).Append(" <---").AppendLine();
            // We want anly IPCT directory
            IptcDirectory lcIptDirectory = (IptcDirectory)lcMetadata.GetDirectory(Type.GetType("com.drew.metadata.iptc.IptcDirectory"));
            if (lcIptDirectory == null)
                {
                lcBuff.Append("No Iptc for this image.!").AppendLine();
                return lcBuff.ToString();
                }

            // We look for potentiel error
            IEnumerator<string> lcErrorsEnum = lcIptDirectory.GetErrors();
            while (lcErrorsEnum.MoveNext())
                {
                Console.Error.WriteLine("Error Found: " + lcErrorsEnum.Current);
                }

            // Then we want all tags, so we iterate on the Iptc directory
            IEnumerator<Tag> lcTagsIterator = lcIptDirectory.GetTagIterator();
            while (lcTagsIterator.MoveNext())
                {
                Tag lcTag = lcTagsIterator.Current;
                string lcTagDescription = null;
                try
                    {
                    lcTagDescription = lcTag.GetDescription();
                    }
                catch (MetadataException e)
                    {
                    Console.Error.WriteLine(e.Message);
                    }
                string lcTagName = lcTag.GetTagName();
                lcBuff.Append(lcTagName).Append('=').Append(lcTagDescription).AppendLine();

                lcTag = null;
                lcTagDescription = null;
                lcTagName = null;
                }

            return lcBuff.ToString();
            }

        /// <summary>
        /// Shows only IPTC directory and only the TAG_HEADLINE value for one file.
        /// </summary>
        /// <param name="aFileName">the image file name (ex: c:/temp/a.jpg)</param>
        /// <returns>The information about IPTC for this image but only the TAG_HEADLINE tag as a string</returns>
        public static string ShowOneFileOnlyIptcOnlyTagTAG_HEADLINE(string aFileName)
            {
            Metadata lcMetadata = null;
            try
                {
                FileInfo lcImgFile = new FileInfo(aFileName);
                // Loading all meta data
                lcMetadata = JpegMetadataReader.ReadMetadata(lcImgFile);
                }
            catch (JpegProcessingException e)
                {
                Console.Error.WriteLine(e.Message);
                return "Error";
                }

            // Now try to print them
            StringBuilder lcBuff = new StringBuilder(1024);
            lcBuff.Append("---> ").Append(aFileName).Append(" <---").AppendLine();
            // We want anly IPCT directory
            IptcDirectory lcIptDirectory = (IptcDirectory)lcMetadata.GetDirectory(Type.GetType("com.drew.metadata.iptc.IptcDirectory"));
            if (lcIptDirectory == null)
                {
                lcBuff.Append("No Iptc for this image.!").AppendLine();
                return lcBuff.ToString();
                }

            // We look for potentiel error
            IEnumerator<string> lcErrorsEnum = lcIptDirectory.GetErrors();
            while (lcErrorsEnum.MoveNext())
                {
                Console.Error.WriteLine("Error Found: " + lcErrorsEnum.Current);
                }

            // Then we want only the TAG_HEADLINE tag
            if (!lcIptDirectory.ContainsTag(IptcDirectory.TAG_HEADLINE))
                {
                lcBuff.Append("No TAG_HEADLINE for this image.!").AppendLine();
                return lcBuff.ToString();
                }
            string lcTagDescription = null;
            try
                {
                lcTagDescription = lcIptDirectory.GetDescription(IptcDirectory.TAG_HEADLINE);
                }
            catch (MetadataException e)
                {
                Console.Error.WriteLine(e.Message);
                }
            string lcTagName = lcIptDirectory.GetTagName(IptcDirectory.TAG_HEADLINE);
            lcBuff.Append(lcTagName).Append('=').Append(lcTagDescription).AppendLine();

            return lcBuff.ToString();
            }
        public static GpsDirectory ShowOneFileGPSDirectory(string aFileName)
            {
            Metadata lcMetadata = null;
            try
                {
                FileInfo lcImgFile = new FileInfo(aFileName);
                // Loading all meta data
                lcMetadata = JpegMetadataReader.ReadMetadata(lcImgFile);
                }
            catch (JpegProcessingException e)
                {
                Console.Error.WriteLine(e.Message);
                return null;
                }
            GpsDirectory lcGPSDirectory = (GpsDirectory)lcMetadata.GetDirectory(Type.GetType("com.drew.metadata.exif.GpsDirectory"));
            //ExifDirectory lcExifDirectory = (ExifDirectory)lcMetadata.GetDirectory(Type.GetType("com.drew.metadata.exif.ExifDirectory"));
            return lcGPSDirectory;
            
            }
        public static ExifDirectory ShowOneFileExifDirectory(string aFileName)
            {
            Metadata lcMetadata = null;
            try
                {
                FileInfo lcImgFile = new FileInfo(aFileName);
                // Loading all meta data
                lcMetadata = JpegMetadataReader.ReadMetadata(lcImgFile);
                }
            catch (JpegProcessingException e)
                {
                Console.Error.WriteLine(e.Message);
                return null;
                }
            
            ExifDirectory lcExifDirectory = (ExifDirectory)lcMetadata.GetDirectory(Type.GetType("com.drew.metadata.exif.ExifDirectory"));
            return lcExifDirectory;

            }
        public static byte[] ShowOneFileThumbnailData(string aFileName)
            {
            Metadata lcMetadata = null;
            try
                {
                FileInfo lcImgFile = new FileInfo(aFileName);
                // Loading all meta data
                lcMetadata = JpegMetadataReader.ReadMetadata(lcImgFile);
                }
            catch (JpegProcessingException e)
                {
                Console.Error.WriteLine(e.Message);
                return null;
                }


            // Now try to print them
            //StringBuilder lcBuff = new StringBuilder(1024);
            //lcBuff.Append("---> ").Append(aFileName).Append(" <---").AppendLine();
            // We want anly IPCT directory

            ExifDirectory lcExifDirectory = (ExifDirectory)lcMetadata.GetDirectory(Type.GetType("com.drew.metadata.exif.ExifDirectory"));
            if (lcExifDirectory == null)
                {
                //lcBuff.Append("No Iptc for this image.!").AppendLine();
                return null;// lcBuff.ToString();
                }

            // We look for potentiel error
            IEnumerator<string> lcErrorsEnum = lcExifDirectory.GetErrors();
            while (lcErrorsEnum.MoveNext())
                {
                Console.Error.WriteLine("Error Found: " + lcErrorsEnum.Current);
                }

            if (!lcExifDirectory.ContainsTag(ExifDirectory.TAG_THUMBNAIL_DATA))
                {
                return null;// lcBuff.ToString();
                }
            try
                {
                //string height = lcExifDirectory.GetDescription(ExifDirectory.TAG_THUMBNAIL_IMAGE_HEIGHT);
                return lcExifDirectory.GetThumbnailData();
                }
            catch (MetadataException e)
                {
                Console.Error.WriteLine(e.Message);
                return null;
                }
            }

        public static string ShowOneFileOnlyTagOriginalDateTime(string aFileName)
            {
            Metadata lcMetadata = null;
            try
                {
                FileInfo lcImgFile = new FileInfo(aFileName);
                // Loading all meta data
                lcMetadata = JpegMetadataReader.ReadMetadata(lcImgFile);
                }
            catch (JpegProcessingException e)
                {
                Console.Error.WriteLine(e.Message);
                return "Error";
                }

            // Now try to print them
            //StringBuilder lcBuff = new StringBuilder(1024);
            //lcBuff.Append("---> ").Append(aFileName).Append(" <---").AppendLine();
            // We want anly IPCT directory

            ExifDirectory lcExifDirectory = (ExifDirectory)lcMetadata.GetDirectory(Type.GetType("com.drew.metadata.exif.ExifDirectory"));
            if (lcExifDirectory == null)
                {
                //lcBuff.Append("No Iptc for this image.!").AppendLine();
                return "";// lcBuff.ToString();
                }

            // We look for potentiel error
            IEnumerator<string> lcErrorsEnum = lcExifDirectory.GetErrors();
            while (lcErrorsEnum.MoveNext())
                {
                Console.Error.WriteLine("Error Found: " + lcErrorsEnum.Current);
                }

            // Then we want only the TAG_HEADLINE tag
            if (!lcExifDirectory.ContainsTag(ExifDirectory.TAG_DATETIME_ORIGINAL))
                {
                //lcBuff.Append("No TAG_HEADLINE for this image.!").AppendLine();
                return "";// lcBuff.ToString();
                }
            // string lcTagDescription = null;
            try
                {
                //lcTagDescription = lcExifDirectory.GetDescription(ExifDirectory.TAG_DATETIME_ORIGINAL);
                return lcExifDirectory.GetDescription(ExifDirectory.TAG_DATETIME_ORIGINAL);
                }
            catch (MetadataException e)
                {
                Console.Error.WriteLine(e.Message);
                return "";
                }
            //string lcTagName = lcExifDirectory.GetTagName(ExifDirectory.TAG_DATETIME_ORIGINAL);
            //lcBuff.Append(lcTagName).Append('=').Append(lcTagDescription).AppendLine();

            //return lcBuff.ToString();
            }
        public static string ShowOneFileOnlyTagGPSAltitude(string aFileName)
            {
            Metadata lcMetadata = null;
            try
                {
                FileInfo lcImgFile = new FileInfo(aFileName);
                // Loading all meta data
                lcMetadata = JpegMetadataReader.ReadMetadata(lcImgFile);
                }
            catch (JpegProcessingException e)
                {
                Console.Error.WriteLine(e.Message);
                return "Error";
                }
            GpsDirectory lcGPSDirectory = (GpsDirectory)lcMetadata.GetDirectory(Type.GetType("com.drew.metadata.exif.GpsDirectory"));
            //ExifDirectory lcExifDirectory = (ExifDirectory)lcMetadata.GetDirectory(Type.GetType("com.drew.metadata.exif.ExifDirectory"));
            if (lcGPSDirectory == null)
                {
                return "";
                }
            // We look for potentiel error
            IEnumerator<string> lcErrorsEnum = lcGPSDirectory.GetErrors();
            while (lcErrorsEnum.MoveNext())
                {
                Console.Error.WriteLine("Error Found: " + lcErrorsEnum.Current);
                }
            if (!lcGPSDirectory.ContainsTag(GpsDirectory.TAG_GPS_ALTITUDE))
                {
                return "";
                }
            try
                {
                return lcGPSDirectory.GetDescription(GpsDirectory.TAG_GPS_ALTITUDE);
                }
            catch (MetadataException e)
                {
                Console.Error.WriteLine(e.Message);
                return "";
                }
            }
        public static string ShowOneFileOnlyTagGPSLongitude(string aFileName)
            {
            Metadata lcMetadata = null;
            try
                {
                FileInfo lcImgFile = new FileInfo(aFileName);
                // Loading all meta data
                lcMetadata = JpegMetadataReader.ReadMetadata(lcImgFile);
                }
            catch (JpegProcessingException e)
                {
                Console.Error.WriteLine(e.Message);
                return "Error";
                }
            GpsDirectory lcGPSDirectory = (GpsDirectory)lcMetadata.GetDirectory(Type.GetType("com.drew.metadata.exif.GpsDirectory"));
            //ExifDirectory lcExifDirectory = (ExifDirectory)lcMetadata.GetDirectory(Type.GetType("com.drew.metadata.exif.ExifDirectory"));
            if (lcGPSDirectory == null)
                {
                return "";
                }
            // We look for potentiel error
            IEnumerator<string> lcErrorsEnum = lcGPSDirectory.GetErrors();
            while (lcErrorsEnum.MoveNext())
                {
                Console.Error.WriteLine("Error Found: " + lcErrorsEnum.Current);
                }
            if (!lcGPSDirectory.ContainsTag(GpsDirectory.TAG_GPS_LONGITUDE))
                {
                return "";
                }
            try
                {
                return lcGPSDirectory.GetDescription(GpsDirectory.TAG_GPS_LONGITUDE);
                }
            catch (MetadataException e)
                {
                Console.Error.WriteLine(e.Message);
                return "";
                }
            }
        /*
        [STAThread]
        public static void Main(string[] someArgs)
        {
            string lcFileName = "c:/temp/a.jpg";
            Console.WriteLine(ShowOneFileAllMetaDataAllTag(lcFileName));
            Console.ReadLine();
            Console.WriteLine(ShowOneFileOnlyIptcAllTag(lcFileName));
            Console.ReadLine();
            Console.WriteLine(ShowOneFileOnlyIptcOnlyTagTAG_HEADLINE(lcFileName));
            Console.ReadLine();
        }
         */
        }
    }