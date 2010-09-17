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
using ZoneFiveSoftware.Common.Data.Fitness;
#if !ST_2_1
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Visuals.Util;
#endif

using System.Windows.Forms;
using ActivityPicturePlugin.Helper;

namespace ActivityPicturePlugin.UI.Activities
{
    class ExtendActivityExportActions :
#if ST_2_1
    IExtendActivityExportActions
#else
    IExtendDailyActivityViewActions, IExtendActivityReportsViewActions
#endif
    {
#if ST_2_1
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
#else
        #region IExtendDailyActivityViewActions Members
        public IList<IAction> GetActions(IDailyActivityView view,
                                                 ExtendViewActions.Location location)
        {
            if (location == ExtendViewActions.Location.ExportMenu)
            {
                return new IAction[] { new TestExportAction(view) };
            }
            else return new IAction[0];
        }
        public IList<IAction> GetActions(IActivityReportsView view,
                                         ExtendViewActions.Location location)
        {
            if (location == ExtendViewActions.Location.ExportMenu)
            {
                return new IAction[] { new TestExportAction(view) };
            }
            else return new IAction[0];
        }
        #endregion
#endif
    }

    class TestExportAction : IAction
    {
#if !ST_2_1
        public TestExportAction(IDailyActivityView view)
        {
            this.dailyView = view;
        }
        public TestExportAction(IActivityReportsView view)
        {
            this.reportView = view;
        }
#else
        public TestExportAction(IActivity act)
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
        public TestExportAction(IList<IActivity> acts)
            {
            if (acts.Count > 0)
                {
                this.enabled = true;
                this.title = Resources.Resources.ResourceManager.GetString("GoogleEarthExport_Title");
                activities = acts;
                }
            }
#endif
        #region IAction Members
        private bool enabled = false;
        private string title = "";
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

        public IList<string> MenuPath
        {
            get
            {
                return new List<string>();
            }
        }
        public void Refresh()
        {
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
        public bool Visible
        {
            get
            {
                if (activities.Count == 0) return false;
                return true;
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

#pragma warning disable 67
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion
#if !ST_2_1
        private IDailyActivityView dailyView = null;
        private IActivityReportsView reportView = null;
#endif
        private IList<IActivity> _activities = null;
        private IList<IActivity> activities
        {
            get
            {
#if !ST_2_1
                //activities are set either directly or by selection,
                //not by more than one view
                if (_activities == null)
                {
                    if (dailyView != null)
                    {
                        return CollectionUtils.GetAllContainedItemsOfType<IActivity>(dailyView.SelectionProvider.SelectedItems);
                    }
                    else if (reportView != null)
                    {
                        return CollectionUtils.GetAllContainedItemsOfType<IActivity>(reportView.SelectionProvider.SelectedItems);
                    }
                    else
                    {
                        return new List<IActivity>();
                    }
                }
#endif
                return _activities;
            }
            set
            {
                _activities = value;
            }
        }
    }
}

