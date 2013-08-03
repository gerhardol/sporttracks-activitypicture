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
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
#if !ST_2_1
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Visuals.Util;
#endif

namespace ActivityPicturePlugin.UI.Activities
{
    class ActivityPicturePage : IDisposable,

#if ST_2_1
 IActivityDetailPage
#else
     IDetailPage
#endif
    {
#if !ST_2_1
        public ActivityPicturePage(IDailyActivityView view)
        {
            this.m_view = view;
            this.m_view.SelectionProvider.SelectedItemsChanged += new EventHandler(OnViewSelectedItemsChanged);
        }

        private void OnViewSelectedItemsChanged(object sender, EventArgs e)
        {
            m_activities = CollectionUtils.GetAllContainedItemsOfType<IActivity>(m_view.SelectionProvider.SelectedItems);
            if ((control != null))
            {
                control.Activities = m_activities;
            }
            RefreshPage();
        }
        public System.Guid Id { get { return GUIDs.Activity; } }
#endif

        #region IActivityDetailPage Members

#if ST2_1
		public IActivity Activity {
            set
            {
                if (null == value) { m_activities = new List<IActivity>(); }
                else { m_activities = new List<IActivity> { value }; }
                if ((control != null))
                {
                    m_control.Activities = m_activities;
                }
            }
		}
#endif

        public bool MenuEnabled
        {
            get { return menuEnabled; }
            set { menuEnabled = value; OnPropertyChanged( "MenuEnabled" ); }
        }

        public IList<string> MenuPath
        {
            get { return menuPath; }
            set { menuPath = value; OnPropertyChanged( "MenuPath" ); }
        }

        public bool MenuVisible
        {
            get { return menuVisible; }
            set { menuVisible = value; OnPropertyChanged( "MenuVisible" ); }
        }

        public bool PageMaximized
        {
            get { return pageMaximized; }
            set { pageMaximized = value; OnPropertyChanged( "PageMaximized" ); }
        }

        #endregion

        #region IDialogPage Members

        public Control CreatePageControl()
        {
            if ( control == null )
            {
#if !ST_2_1
                control = new ActivityPicturePageControl(this, m_view);
#else
                control = new ActivityPicturePageControl();
#endif
#if ST_2_1
                control.Activities = this.m_activities;
#endif
                control.CleanupThumbnails();
            }
            return control;
        }

        public bool HidePage()
        {
            if ( null != this.control )
            {
                this.control.HidePage();
            }
            return true;
        }

        public string PageName
        {
            get { return Title; }
        }

        public void ShowPage( string bookmark )
        {
            if ( control != null )
            {
                //return;
                control.ShowPage( bookmark );
            }
        }

        public IPageStatus Status
        {
            get { return null; }
        }

        public void ThemeChanged( ITheme visualTheme )
        {
            if ( control != null )
            {
                control.ThemeChanged( visualTheme );
            }
        }

        public string Title
        {
            get { return Resources.Resources.ActivityPicturePage_Title; }
        }

        public void RefreshPage()
        {
            if ( control != null )
            {
                //control.RefreshPage();	//Note:  If you must call RefreshPage, check _showPage first.
                control.Refresh();
            }
        }

        public void UICultureChanged( CultureInfo culture )
        {
            if ( control != null )
            {
                control.ShowProgressBar();
                control.UICultureChanged( culture );
                control.HideProgressBar();
            }
        }

        #endregion

        #region INotifyPropertyChanged Members
#pragma warning disable 67
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Private methods
        private void OnPropertyChanged( string propertyName )
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }
        #endregion

        #region Private members
#if !ST_2_1
        private IDailyActivityView m_view = null;
#endif
        private IList<IActivity> m_activities = new List<IActivity>();
        private ActivityPicturePageControl control = null;
        private IList<string> menuPath = null;
        private bool menuEnabled = true;
        private bool menuVisible = true;
        private bool pageMaximized = false;
        #endregion

        public void Dispose()
        {
            if ( this.control != null )
            {
                this.control.Dispose();
                this.control = null;
            }
        }
    }
}