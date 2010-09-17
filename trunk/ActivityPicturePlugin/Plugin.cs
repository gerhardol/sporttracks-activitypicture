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
                ActivityPicturePlugin.Plugin.version = GetType().Assembly.GetName().Version.ToString(3);
                application = value; 
            }
        }

        public Guid Id
        {
            get
            {
                return GUID;
            }
        }

        public string Name
        {
            get { return "Activity Picture Plugin"; }
        }

        public void ReadOptions(XmlDocument xmlDoc, XmlNamespaceManager nsmgr, XmlElement pluginNode)
        {
            //throw new Exception("The method or operation is not implemented.");
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
            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion
        /// <summary> 
        /// The unique identifier of the plugin
        /// </summary> 
        /// <remarks></remarks> 
        /// <history> 
        /// [doml] 05.09.2007 Created 
        /// </history> 
        public static Guid GUID = new Guid("{1a0840b9-1d83-4845-ada9-b0c0a6959f40}");
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
