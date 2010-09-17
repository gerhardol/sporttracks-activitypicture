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
using ZoneFiveSoftware.Common.Visuals.Fitness;
using System.Windows.Forms;
using ActivityPicturePlugin.Helper;
namespace ActivityPicturePlugin.UI.Activities
    {
    class ExtendActivityExportActions : IExtendActivityExportActions
        {
        #region IExtendActivityExportActions Members

        public IList<IAction> GetActions(IList<ZoneFiveSoftware.Common.Data.Fitness.IActivity> activities)
            {
            return new IAction[] { new TestExportAction(activities) };
            }

        public IList<IAction> GetActions(ZoneFiveSoftware.Common.Data.Fitness.IActivity activity)
            {
            return new IAction[] { new TestExportAction(activity) };
            }

        #endregion
        }
    class TestExportAction : IAction
        {
        public TestExportAction(ZoneFiveSoftware.Common.Data.Fitness.IActivity act)
            {
            this.title = Resources.Resources.ResourceManager.GetString("GoogleEarthExport_Title");
            if (act != null)
                {
                if (Helper.Functions.ReadExtensionData(act).Images.Count != 0)
                    {
                    this.enabled = true;
                    activities.Add(act);
                    }
                }
            }
        public TestExportAction(IList<ZoneFiveSoftware.Common.Data.Fitness.IActivity> acts)
            {
            if (acts.Count > 0)
                {
                this.enabled = true;
                this.title = Resources.Resources.ResourceManager.GetString("GoogleEarthExport_Title");
                activities = acts;
                }
            }
        #region IAction Members
        private bool enabled = false;
        private string title = "";
        private IList<ZoneFiveSoftware.Common.Data.Fitness.IActivity> activities = new List<ZoneFiveSoftware.Common.Data.Fitness.IActivity>();
        public bool Enabled
            {
            get
                {
                return enabled;
                }
            }

        public bool HasMenuArrow
            {
            get
                {
                return true;
                }
            }

        public System.Drawing.Image Image
            {
            get { return Resources.Resources.GE; }
            }

        public void Refresh()
            {
            //throw new Exception("The method or operation is not implemented.");
            }

        public void Run(System.Drawing.Rectangle rectButton)
            {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "";
            sfd.DefaultExt = "kmz";
            sfd.AddExtension = true;
            sfd.CheckPathExists = true;
            sfd.Filter = "Google Earth compressed (*.kmz)|*.kmz|Google Earth KML (*.kml)|*.kml";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult dres = sfd.ShowDialog();
            if (dres == DialogResult.OK & sfd.FileName != "")
                {
                Functions.PerformMultipleExportToGoogleEarth(activities, sfd.FileName);
                }
            }

        public string Title
            {
            get { return title; }
            }

        #endregion

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion
        }
    }

