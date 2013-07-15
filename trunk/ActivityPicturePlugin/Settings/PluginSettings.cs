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

namespace ActivityPicturePlugin.Settings
{
    // I don't think this class is currently being used
    //data that will be serialized and saved with SetExtensionData
    [Serializable()]
    public class PluginSettings
    {
        public class SettingsData
        {
        }

        #region private
        private ZoneFiveSoftware.Common.Data.Fitness.ILogbook log
        {
            get { return ActivityPicturePlugin.Plugin.GetApplication().Logbook; }
        }
        #endregion

        #region public
        public SettingsData data = new SettingsData();

        public SettingsData dataRead = new SettingsData(); //data last read from logbook

        public void ReadSettings()
        {
            try
            {
                byte[] b = log.GetExtensionData( ActivityPicturePlugin.GUIDs.PluginMain );
                if ( !( b.Length == 0 ) )
                {
                    System.Xml.Serialization.XmlSerializer xmlSer = new System.Xml.Serialization.XmlSerializer( typeof( SettingsData ) );
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    mem.Write( b, 0, b.Length );
                    mem.Position = 0;
                    data = dataRead = (SettingsData)xmlSer.Deserialize( mem );
                    mem.Dispose();
                    xmlSer = null;
                    b = null;
                }
                else
                {
                }
            }
            catch ( Exception )
            {
                //throw;
            }
        }

        public void WriteSettings()
        {
            if ( !data.Equals( dataRead ) )
            {
                try
                {
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    System.Xml.Serialization.XmlSerializer xmlSer = new System.Xml.Serialization.XmlSerializer( typeof( SettingsData ) );
                    xmlSer.Serialize( mem, data );
                    log.SetExtensionData( ActivityPicturePlugin.GUIDs.PluginMain, mem.ToArray() );
                    log.SetExtensionText( ActivityPicturePlugin.GUIDs.PluginMain, "Picture Plugin" );
                    mem.Close();
                    log.Modified = true;
                }
                catch ( Exception )
                {
                    throw;
                }
            }
        }
        #endregion
    }
}