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

namespace ActivityPicturePlugin.Settings
{
    partial class SettingsPageControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxImport = new System.Windows.Forms.GroupBox();
            this.importControl1 = new ActivityPicturePlugin.UI.ImportControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbOpenGE = new System.Windows.Forms.CheckBox();
            this.cbStoreGEFileLocations = new System.Windows.Forms.CheckBox();
            this.lblQualityValue = new System.Windows.Forms.Label();
            this.lblSizeValue = new System.Windows.Forms.Label();
            this.lblImageQuality = new System.Windows.Forms.Label();
            this.lblImageSize = new System.Windows.Forms.Label();
            this.trackBarQuality = new MB.Controls.ColorSlider();
            this.trackBarSize = new MB.Controls.ColorSlider();
            this.groupBoxImport.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxImport
            // 
            this.groupBoxImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxImport.Controls.Add(this.importControl1);
            this.groupBoxImport.Location = new System.Drawing.Point(4, 130);
            this.groupBoxImport.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxImport.Name = "groupBoxImport";
            this.groupBoxImport.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxImport.Size = new System.Drawing.Size(775, 429);
            this.groupBoxImport.TabIndex = 6;
            this.groupBoxImport.TabStop = false;
            this.groupBoxImport.Text = "Import";
            this.groupBoxImport.Resize += new System.EventHandler(this.groupBoxImport_Resize);
            // 
            // importControl1
            // 
            this.importControl1.Location = new System.Drawing.Point(4, 19);
            this.importControl1.Margin = new System.Windows.Forms.Padding(5);
            this.importControl1.Name = "importControl1";
            this.importControl1.Padding = new System.Windows.Forms.Padding(4);
            this.importControl1.ShowAllActivities = true;
            this.importControl1.Size = new System.Drawing.Size(765, 400);
            this.importControl1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbOpenGE);
            this.groupBox2.Controls.Add(this.cbStoreGEFileLocations);
            this.groupBox2.Controls.Add(this.lblQualityValue);
            this.groupBox2.Controls.Add(this.lblSizeValue);
            this.groupBox2.Controls.Add(this.lblImageQuality);
            this.groupBox2.Controls.Add(this.lblImageSize);
            this.groupBox2.Controls.Add(this.trackBarQuality);
            this.groupBox2.Controls.Add(this.trackBarSize);
            this.groupBox2.Location = new System.Drawing.Point(8, 4);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(771, 110);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Google Earth";
            // 
            // cbOpenGE
            // 
            this.cbOpenGE.AutoSize = true;
            this.cbOpenGE.Location = new System.Drawing.Point(385, 21);
            this.cbOpenGE.Name = "cbOpenGE";
            this.cbOpenGE.Size = new System.Drawing.Size(257, 21);
            this.cbOpenGE.TabIndex = 2;
            this.cbOpenGE.Text = "Open in Google Earth when created";
            this.cbOpenGE.UseVisualStyleBackColor = true;
            this.cbOpenGE.CheckedChanged += new System.EventHandler(this.cbOpenGE_CheckedChanged);
            // 
            // cbStoreGEFileLocations
            // 
            this.cbStoreGEFileLocations.AutoSize = true;
            this.cbStoreGEFileLocations.Location = new System.Drawing.Point(385, 47);
            this.cbStoreGEFileLocations.Name = "cbStoreGEFileLocations";
            this.cbStoreGEFileLocations.Size = new System.Drawing.Size(200, 21);
            this.cbStoreGEFileLocations.TabIndex = 8;
            this.cbStoreGEFileLocations.Text = "Store kmz/kml file locations";
            this.cbStoreGEFileLocations.UseVisualStyleBackColor = true;
            this.cbStoreGEFileLocations.CheckedChanged += new System.EventHandler(this.cbStoreGEFileLocations_CheckedChanged);
            // 
            // lblQualityValue
            // 
            this.lblQualityValue.AutoSize = true;
            this.lblQualityValue.Location = new System.Drawing.Point(217, 80);
            this.lblQualityValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQualityValue.Name = "lblQualityValue";
            this.lblQualityValue.Size = new System.Drawing.Size(112, 17);
            this.lblQualityValue.TabIndex = 5;
            this.lblQualityValue.Text = "labelSampleText";
            this.lblQualityValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSizeValue
            // 
            this.lblSizeValue.AutoSize = true;
            this.lblSizeValue.Location = new System.Drawing.Point(33, 80);
            this.lblSizeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSizeValue.Name = "lblSizeValue";
            this.lblSizeValue.Size = new System.Drawing.Size(112, 17);
            this.lblSizeValue.TabIndex = 4;
            this.lblSizeValue.Text = "labelSampleText";
            this.lblSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblImageQuality
            // 
            this.lblImageQuality.AutoSize = true;
            this.lblImageQuality.Location = new System.Drawing.Point(192, 21);
            this.lblImageQuality.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImageQuality.Name = "lblImageQuality";
            this.lblImageQuality.Size = new System.Drawing.Size(94, 17);
            this.lblImageQuality.TabIndex = 3;
            this.lblImageQuality.Text = "Image Quality";
            // 
            // lblImageSize
            // 
            this.lblImageSize.AutoSize = true;
            this.lblImageSize.Location = new System.Drawing.Point(8, 21);
            this.lblImageSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImageSize.Name = "lblImageSize";
            this.lblImageSize.Size = new System.Drawing.Size(77, 17);
            this.lblImageSize.TabIndex = 2;
            this.lblImageSize.Text = "Image Size";
            // 
            // trackBarQuality
            // 
            this.trackBarQuality.BackColor = System.Drawing.Color.Transparent;
            this.trackBarQuality.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.trackBarQuality.LargeChange = ((uint)(1u));
            this.trackBarQuality.Location = new System.Drawing.Point(192, 41);
            this.trackBarQuality.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarQuality.Maximum = 10;
            this.trackBarQuality.Minimum = 1;
            this.trackBarQuality.MouseEffects = false;
            this.trackBarQuality.Name = "trackBarQuality";
            this.trackBarQuality.Size = new System.Drawing.Size(139, 37);
            this.trackBarQuality.SmallChange = ((uint)(1u));
            this.trackBarQuality.TabIndex = 1;
            this.trackBarQuality.ThumbRoundRectSize = new System.Drawing.Size(15, 15);
            this.trackBarQuality.Value = 8;
            this.trackBarQuality.ValueChanged += new System.EventHandler(this.trackBarQuality_ValueChanged);
            // 
            // trackBarSize
            // 
            this.trackBarSize.BackColor = System.Drawing.Color.Transparent;
            this.trackBarSize.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.trackBarSize.LargeChange = ((uint)(1u));
            this.trackBarSize.Location = new System.Drawing.Point(8, 41);
            this.trackBarSize.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarSize.Maximum = 10;
            this.trackBarSize.Minimum = 1;
            this.trackBarSize.MouseEffects = false;
            this.trackBarSize.Name = "trackBarSize";
            this.trackBarSize.Size = new System.Drawing.Size(139, 37);
            this.trackBarSize.SmallChange = ((uint)(1u));
            this.trackBarSize.TabIndex = 0;
            this.trackBarSize.ThumbRoundRectSize = new System.Drawing.Size(15, 15);
            this.trackBarSize.Value = 8;
            this.trackBarSize.ValueChanged += new System.EventHandler(this.trackBarSize_ValueChanged);
            // 
            // SettingsPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxImport);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SettingsPageControl";
            this.Size = new System.Drawing.Size(783, 562);
            this.groupBoxImport.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ActivityPicturePlugin.UI.ImportControl importControl1;
        private System.Windows.Forms.GroupBox groupBoxImport;
        private System.Windows.Forms.GroupBox groupBox2;
        private MB.Controls.ColorSlider trackBarSize;
        private System.Windows.Forms.Label lblQualityValue;
        private System.Windows.Forms.Label lblSizeValue;
        private System.Windows.Forms.Label lblImageQuality;
        private System.Windows.Forms.Label lblImageSize;
        private MB.Controls.ColorSlider trackBarQuality;
        private System.Windows.Forms.CheckBox cbOpenGE;
        private System.Windows.Forms.CheckBox cbStoreGEFileLocations;

    }
}