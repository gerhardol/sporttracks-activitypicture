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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ActivityPicturePlugin.Helper;
using ActivityPicturePlugin.Properties;
using ZoneFiveSoftware.Common.Visuals;

namespace ActivityPicturePlugin.UI.Activities
{
    public partial class ModifyTimeStamp : Form
    {
        ImageData imgD;
        public ModifyTimeStamp(ImageData id)
        {
            InitializeComponent();

            //localization
            this.Text = Resources.ModifyTimeStamp_Text;
            this.btnOK.Text = CommonResources.Text.ActionOk;
            this.btnCancel.Text = CommonResources.Text.ActionCancel;



            imgD = id;
            DateTime dt = id.DateTimeOriginal();
            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;

            string datePattern = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + ", " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;

            this.dateTimePicker1.CustomFormat = datePattern;
            try
            {
                this.dateTimePicker1.Value = id.DateTimeOriginal();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);

                // throw;
            }
        }

        public void ThemeChanged(ITheme visualTheme)
        {
            this.BackColor = visualTheme.Control;
            this.ForeColor = visualTheme.ControlText;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            ApplyTimeStamp();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ApplyTimeStamp()
        {
            try
            {
                imgD.SetDateTimeOriginal(dateTimePicker1.Value);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                //throw;
            }
        }
    }
}