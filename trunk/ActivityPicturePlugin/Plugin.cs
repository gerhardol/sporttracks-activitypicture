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
using System.Xml;

using ZoneFiveSoftware.Common.Visuals.Fitness;

namespace ActivityPicturePlugin
{
    public class Plugin : IPlugin
    {
        #region IPlugin Members

        public IApplication Application
        {
            set {
                ActivityPicturePlugin.Plugin.version = GetType().Assembly.GetName().Version.ToString(4);
                application = value; 
            }
        }

        public Guid Id
        {
            get
            {
                return GUIDs.PluginMain;
            }
        }

        public string Name
        {
            get { return "Activity Picture Plugin"; }
        }

        public void ReadOptions(XmlDocument xmlDoc, XmlNamespaceManager nsmgr, XmlElement pluginNode)
        {
        }

        public string Version
        {
            get
            {
                return ActivityPicturePlugin.Plugin.version;
            }
        }

        public void WriteOptions(XmlDocument xmlDoc, XmlElement pluginNode)
        {
        }

        #endregion
        public static String version;
        
        public static IApplication GetIApplication()
        {
            return application;
        }

        #region Private members
        private static IApplication application;
        #endregion
    }
}
