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
using System.Collections;
using ActivityPicturePlugin.Helper;

namespace ActivityPicturePlugin.UI.Settings
{
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
}
