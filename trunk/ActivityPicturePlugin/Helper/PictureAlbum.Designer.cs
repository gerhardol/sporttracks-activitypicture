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
    partial class PictureAlbum
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
        [System.Security.Permissions.PermissionSet( System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust" )] 
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 1000;
            // 
            // PictureAlbum
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size( 50, 50 );
            this.AutoSize = true;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding( 0 );
            this.Name = "PictureAlbum";
            this.Size = new System.Drawing.Size( 344, 294 );
            this.MouseClick += new System.Windows.Forms.MouseEventHandler( this.PictureAlbum_MouseClick );
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.PictureAlbum_MouseDoubleClick );
            this.MouseEnter += new System.EventHandler( this.PictureAlbum_MouseEnter );
            this.MouseLeave += new System.EventHandler( this.PictureAlbum_MouseLeave );
            this.MouseMove += new System.Windows.Forms.MouseEventHandler( this.PictureAlbum_MouseMove );
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.PictureAlbum_PreviewKeyDown );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;

    }
}
