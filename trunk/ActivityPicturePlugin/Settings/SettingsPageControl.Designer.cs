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
        protected override void Dispose(bool disposing)
            {
            if (disposing && (components != null))
                {
                components.Dispose();
                }
            base.Dispose(disposing);
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
                this.lblQualityValue = new System.Windows.Forms.Label();
                this.lblSizeValue = new System.Windows.Forms.Label();
                this.lblImageQuality = new System.Windows.Forms.Label();
                this.lblImageSize = new System.Windows.Forms.Label();
                this.trackBarQuality = new System.Windows.Forms.TrackBar();
                this.trackBarSize = new System.Windows.Forms.TrackBar();
                this.groupBoxImport.SuspendLayout();
                this.groupBox2.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.trackBarQuality)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).BeginInit();
                this.SuspendLayout();
                // 
                // groupBoxImport
                // 
                this.groupBoxImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.groupBoxImport.Controls.Add(this.importControl1);
                this.groupBoxImport.Location = new System.Drawing.Point(3, 98);
                this.groupBoxImport.Name = "groupBoxImport";
                this.groupBoxImport.Size = new System.Drawing.Size(581, 356);
                this.groupBoxImport.TabIndex = 6;
                this.groupBoxImport.TabStop = false;
                this.groupBoxImport.Text = "Import";
                // 
                // importControl1
                // 
                this.importControl1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.importControl1.Location = new System.Drawing.Point(3, 16);
                this.importControl1.Name = "importControl1";
                this.importControl1.ShowAllActivities = true;
                this.importControl1.Size = new System.Drawing.Size(575, 337);
                this.importControl1.TabIndex = 5;
                // 
                // groupBox2
                // 
                this.groupBox2.Controls.Add(this.lblQualityValue);
                this.groupBox2.Controls.Add(this.lblSizeValue);
                this.groupBox2.Controls.Add(this.lblImageQuality);
                this.groupBox2.Controls.Add(this.lblImageSize);
                this.groupBox2.Controls.Add(this.trackBarQuality);
                this.groupBox2.Controls.Add(this.trackBarSize);
                this.groupBox2.Location = new System.Drawing.Point(6, 3);
                this.groupBox2.Name = "groupBox2";
                this.groupBox2.Size = new System.Drawing.Size(575, 89);
                this.groupBox2.TabIndex = 7;
                this.groupBox2.TabStop = false;
                this.groupBox2.Text = "Google Earth";
                // 
                // lblQualityValue
                // 
                this.lblQualityValue.AutoSize = true;
                this.lblQualityValue.Location = new System.Drawing.Point(163, 65);
                this.lblQualityValue.Name = "lblQualityValue";
                this.lblQualityValue.Size = new System.Drawing.Size(85, 13);
                this.lblQualityValue.TabIndex = 5;
                this.lblQualityValue.Text = "labelSampleText";
                this.lblQualityValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                // 
                // lblSizeValue
                // 
                this.lblSizeValue.AutoSize = true;
                this.lblSizeValue.Location = new System.Drawing.Point(25, 65);
                this.lblSizeValue.Name = "lblSizeValue";
                this.lblSizeValue.Size = new System.Drawing.Size(85, 13);
                this.lblSizeValue.TabIndex = 4;
                this.lblSizeValue.Text = "labelSampleText";
                this.lblSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                // 
                // lblImageQuality
                // 
                this.lblImageQuality.AutoSize = true;
                this.lblImageQuality.Location = new System.Drawing.Point(141, 17);
                this.lblImageQuality.Name = "lblImageQuality";
                this.lblImageQuality.Size = new System.Drawing.Size(71, 13);
                this.lblImageQuality.TabIndex = 3;
                this.lblImageQuality.Text = "Image Quality";
                // 
                // lblImageSize
                // 
                this.lblImageSize.AutoSize = true;
                this.lblImageSize.Location = new System.Drawing.Point(6, 17);
                this.lblImageSize.Name = "lblImageSize";
                this.lblImageSize.Size = new System.Drawing.Size(59, 13);
                this.lblImageSize.TabIndex = 2;
                this.lblImageSize.Text = "Image Size";
                // 
                // trackBarQuality
                // 
                this.trackBarQuality.AutoSize = false;
                this.trackBarQuality.LargeChange = 1;
                this.trackBarQuality.Location = new System.Drawing.Point(144, 33);
                this.trackBarQuality.Minimum = 1;
                this.trackBarQuality.Name = "trackBarQuality";
                this.trackBarQuality.Size = new System.Drawing.Size(104, 30);
                this.trackBarQuality.TabIndex = 1;
                this.trackBarQuality.TickStyle = System.Windows.Forms.TickStyle.None;
                this.trackBarQuality.Value = 8;
                this.trackBarQuality.Scroll += new System.EventHandler(this.trackBar2_Scroll);
                // 
                // trackBarSize
                // 
                this.trackBarSize.AutoSize = false;
                this.trackBarSize.LargeChange = 1;
                this.trackBarSize.Location = new System.Drawing.Point(6, 33);
                this.trackBarSize.Minimum = 1;
                this.trackBarSize.Name = "trackBarSize";
                this.trackBarSize.Size = new System.Drawing.Size(104, 30);
                this.trackBarSize.TabIndex = 0;
                this.trackBarSize.TickStyle = System.Windows.Forms.TickStyle.None;
                this.trackBarSize.Value = 8;
                this.trackBarSize.Scroll += new System.EventHandler(this.trackBar1_Scroll);
                // 
                // SettingsPageControl
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.groupBox2);
                this.Controls.Add(this.groupBoxImport);
                this.Name = "SettingsPageControl";
                this.Size = new System.Drawing.Size(587, 457);
                this.Load += new System.EventHandler(this.SettingsPageControl_Load);
                this.Enter += new System.EventHandler(this.SettingsPageControl_Enter);
                this.groupBoxImport.ResumeLayout(false);
                this.groupBox2.ResumeLayout(false);
                this.groupBox2.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.trackBarQuality)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).EndInit();
                this.ResumeLayout(false);

            }

        #endregion

        private ActivityPicturePlugin.UI.ImportControl importControl1;
        private System.Windows.Forms.GroupBox groupBoxImport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TrackBar trackBarSize;
        private System.Windows.Forms.Label lblQualityValue;
        private System.Windows.Forms.Label lblSizeValue;
        private System.Windows.Forms.Label lblImageQuality;
        private System.Windows.Forms.Label lblImageSize;
        private System.Windows.Forms.TrackBar trackBarQuality;
        }
    }
