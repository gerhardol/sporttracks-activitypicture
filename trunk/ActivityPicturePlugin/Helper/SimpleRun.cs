/* Adapted from the 2.30 version of MetaDataExtractor */
/* Some example functions from old SampleRun used in the plugin */
/* Note: MetaDataExtractor project adapted to output library */

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
namespace ActivityPicturePlugin.Helper
{
    /// <summary>
    /// This class is a simple example of how to use the classes inside this project.
    /// </summary>
    public sealed class SimpleRun
        {
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
            GpsDirectory lcGPSDirectory = (GpsDirectory)lcMetadata.GetDirectory("com.drew.metadata.exif.GpsDirectory");
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
            
            ExifDirectory lcExifDirectory = (ExifDirectory)lcMetadata.GetDirectory("com.drew.metadata.exif.ExifDirectory");
            return lcExifDirectory;

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
                //TODO: fall back on standard file time
                Console.Error.WriteLine(e.Message);
                return "";
                }

            // Now try to print them
            //StringBuilder lcBuff = new StringBuilder(1024);
            //lcBuff.Append("---> ").Append(aFileName).Append(" <---").AppendLine();
            // We want anly IPCT directory

            ExifDirectory lcExifDirectory = (ExifDirectory)lcMetadata.GetDirectory("com.drew.metadata.exif.ExifDirectory");
            if (lcExifDirectory == null)
                {
                //lcBuff.Append("No Iptc for this image.!").AppendLine();
                return "";// lcBuff.ToString();
                }

            // We look for potentiel error
            if (lcExifDirectory.HasError)
            {
                Console.Error.WriteLine("Some errors were found, activate trace using /d:TRACE option with the compiler");
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
        }
    }