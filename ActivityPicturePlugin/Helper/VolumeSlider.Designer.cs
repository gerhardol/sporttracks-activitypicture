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

namespace ActivityPicturePlugin.Helper
{
    partial class VolumeSlider
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
            this.SuspendLayout();
            // 
            // VolumeSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 8F, 16F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding( 4 );
            this.Name = "VolumeSlider";
            this.Size = new System.Drawing.Size( 200, 42 );
            this.Load += new System.EventHandler( this.VolumeSlider_Load );
            this.MouseDown += new System.Windows.Forms.MouseEventHandler( this.VolumeSlider_MouseDown );
            this.MouseMove += new System.Windows.Forms.MouseEventHandler( this.VolumeSlider_MouseMove );
            this.MouseUp += new System.Windows.Forms.MouseEventHandler( this.VolumeSlider_MouseUp );
            this.ResumeLayout( false );

        }
        #endregion
    }
}