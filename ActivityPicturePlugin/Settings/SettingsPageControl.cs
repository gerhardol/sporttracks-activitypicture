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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ActivityPicturePlugin.Properties;
using ActivityPicturePlugin.UI.Activities;
using ZoneFiveSoftware.Common.Visuals;

namespace ActivityPicturePlugin.Settings
{
    public partial class SettingsPageControl : UserControl, ZoneFiveSoftware.Common.Visuals.ISettingsPage
    {
        public SettingsPageControl()
        {
            InitializeComponent();
            importControl1.InitComponent();

            trackBarQuality.Value = ActivityPicturePlugin.Source.Settings.GEQuality;
            trackBarSize.Value = ActivityPicturePlugin.Source.Settings.GESize;
            cbOpenGE.Checked = ActivityPicturePlugin.Source.Settings.GEAutoOpen;
            cbStoreGEFileLocations.Checked = ActivityPicturePlugin.Source.Settings.GEStoreFileLocation;

            lblSizeValue.Text = trackBarSize.Value * baseNum + " x " + trackBarSize.Value * baseNum * 3 / 4;
            lblQualityValue.Text = trackBarQuality.Value * 10 + " %";
        }

        int baseNum = 50;
        #region ISettingsPage Members
        public Guid Id
        {
            get { return GUIDs.Settings; }
        }
        public IList<ZoneFiveSoftware.Common.Visuals.ISettingsPage> SubPages
        {
            get { return null; }
        }
        #endregion
        #region IDialogPage Members
        public Control CreatePageControl()
        {
            return this;
        }
        public bool HidePage()
        {
            this.Hide();
            return true;
        }
        public string PageName
        {
            get
            {
                return Resources.ActivityPicturePlugin_Text;
            }
        }
        public void ShowPage( string bookmark )
        {
            importControl1.LoadActivityNodes();
            this.Show();
            this.groupBoxImport_Resize( this, new EventArgs() );
        }
        public ZoneFiveSoftware.Common.Visuals.IPageStatus Status
        {
            get { return null; }
        }
        public void ThemeChanged( ZoneFiveSoftware.Common.Visuals.ITheme visualTheme )
        {
            this.BackColor = visualTheme.Control;
            this.importControl1.ThemeChanged( visualTheme );
            this.groupBoxImport.BackColor = visualTheme.Control;
            this.groupBoxImport.ForeColor = visualTheme.ControlText;
            this.groupBox2.BackColor = visualTheme.Control;
            this.groupBox2.ForeColor = visualTheme.ControlText;

            lblImageQuality.ForeColor = visualTheme.ControlText;
            lblImageSize.ForeColor = visualTheme.ControlText;
            lblQualityValue.ForeColor = visualTheme.ControlText;
            lblSizeValue.ForeColor = visualTheme.ControlText;
            cbOpenGE.ForeColor = visualTheme.ControlText;

            this.trackBarSize.BarInnerColor = visualTheme.SubHeader;
            this.trackBarSize.BarOuterColor = visualTheme.MainHeader;
            this.trackBarSize.BarPenColor = visualTheme.SubHeader;
            this.trackBarSize.ElapsedInnerColor = visualTheme.Window;
            this.trackBarSize.ElapsedOuterColor = visualTheme.MainHeader;

            this.trackBarQuality.BarInnerColor = visualTheme.SubHeader;
            this.trackBarQuality.BarOuterColor = visualTheme.MainHeader;
            this.trackBarQuality.BarPenColor = visualTheme.SubHeader;
            this.trackBarQuality.ElapsedInnerColor = visualTheme.Window;
            this.trackBarQuality.ElapsedOuterColor = visualTheme.MainHeader;

        }
        public string Title
        {
            get { return "Picture Plugin"; }
        }
        public void UICultureChanged( System.Globalization.CultureInfo culture )
        {
            //localization
            this.groupBoxImport.Text = CommonResources.Text.ActionImport;
            this.lblImageQuality.Text = Resources.SettingsPageControl_lblQuality_Text;
            this.lblImageSize.Text = Resources.labelImageSize_Text;
            this.cbOpenGE.Text = Resources.OpenInGoogleEarthWhenCreated_Text;
            this.cbStoreGEFileLocations.Text = Resources.StoreGEFileLocations_Text;
            this.importControl1.UpdateUICulture( culture );
        }

        #endregion
        #region INotifyPropertyChanged Members

#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Eventhandlers
        private void trackBarSize_ValueChanged( object sender, EventArgs e )
        {
            lblSizeValue.Text = trackBarSize.Value * baseNum + " x " + trackBarSize.Value * baseNum * 3 / 4;
            ActivityPicturePlugin.Source.Settings.GESize = trackBarSize.Value;
        }

        private void trackBarQuality_ValueChanged( object sender, EventArgs e )
        {
            lblQualityValue.Text = trackBarQuality.Value * 10 + " %";
            ActivityPicturePlugin.Source.Settings.GEQuality = trackBarQuality.Value;
        }

        private void cbOpenGE_CheckedChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.GEAutoOpen = cbOpenGE.Checked;
        }

        private void cbStoreGEFileLocations_CheckedChanged( object sender, EventArgs e )
        {
            ActivityPicturePlugin.Source.Settings.GEStoreFileLocation = cbStoreGEFileLocations.Checked;
        }

        private void groupBoxImport_Resize( object sender, EventArgs e )
        {
            importControl1.Height = groupBoxImport.Height - ( importControl1.Top + 10 );
            importControl1.Width = groupBoxImport.Width - ( importControl1.Left * 2 );
        }
        #endregion

    }

}