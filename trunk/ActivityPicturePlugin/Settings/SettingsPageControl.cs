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
using ActivityPicturePlugin.UI.Activities;

namespace ActivityPicturePlugin.Settings
{
    public partial class SettingsPageControl : UserControl, ZoneFiveSoftware.Common.Visuals.ISettingsPage
    {
        public SettingsPageControl()
        {
            InitializeComponent();


            //localization
            this.groupBoxImport.Text = Resources.Resources.ResourceManager.GetString("btnManImp_Text");
            this.lblImageQuality.Text = Resources.Resources.ResourceManager.GetString("SettingsPageControl_lblQuality_Text");
            this.lblImageSize.Text = Resources.Resources.ResourceManager.GetString("labelImageSize_Text");

            trackBarQuality.Value = ActivityPicturePageControl.PluginSettingsData.data.Quality;
            trackBarSize.Value = ActivityPicturePageControl.PluginSettingsData.data.Size;
            lblSizeValue.Text = trackBarSize.Value * baseNum + " x " + trackBarSize.Value * baseNum * 3 / 4;
            lblQualityValue.Text = trackBarQuality.Value * 10 + " %";
        }
        //ActivityPicturePlugin.UI.Activities.PluginSettings ps;
        int baseNum = 50;
        #region ISettingsPage Members
        public Guid Id
        {
            get { return new Guid(); }
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
            get { return "Activity Picture Plugin"; }
        }
        public void ShowPage(string bookmark)
        {
            importControl1.LoadNodes();
            this.Show();
        }
        public ZoneFiveSoftware.Common.Visuals.IPageStatus Status
        {
            get { return null; }
        }
        public void ThemeChanged(ZoneFiveSoftware.Common.Visuals.ITheme visualTheme)
        {
            this.BackColor = visualTheme.Window;
        }
        public string Title
        {
            get { return "Picture Plugin"; }
        }
        public void UICultureChanged(System.Globalization.CultureInfo culture)
        {
            //localization
            this.groupBoxImport.Text = Resources.Resources.ResourceManager.GetString("btnManImp_Text");
            this.lblImageQuality.Text = Resources.Resources.ResourceManager.GetString("SettingsPageControl_lblQuality_Text");
            this.lblImageSize.Text = Resources.Resources.ResourceManager.GetString("labelImageSize_Text");
        }

        #endregion
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void SettingsPageControl_Load(object sender, EventArgs e)
        {
            //UpdateControls();
        }

        private void SettingsPageControl_Enter(object sender, EventArgs e)
        {
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lblSizeValue.Text = trackBarSize.Value * baseNum + " x " + trackBarSize.Value * baseNum * 3 / 4;
            ActivityPicturePageControl.PluginSettingsData.data.Size = trackBarSize.Value;
            ActivityPicturePageControl.PluginSettingsData.WriteSettings();
        }
      

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            lblQualityValue.Text = trackBarQuality.Value * 10 + " %";
            ActivityPicturePageControl.PluginSettingsData.data.Quality = trackBarQuality.Value;
            ActivityPicturePageControl.PluginSettingsData.WriteSettings();
        }
    }

}
