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

namespace ActivityPicturePlugin.UI.Activities
{
    partial class ModifyTimeStamp
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new ZoneFiveSoftware.Common.Visuals.Button();
            this.btnCancel = new ZoneFiveSoftware.Common.Visuals.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BorderColor = System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 100 ) ) ) ), ( (int)( ( (byte)( 40 ) ) ) ), ( (int)( ( (byte)( 50 ) ) ) ), ( (int)( ( (byte)( 120 ) ) ) ) );
            this.btnOK.CenterImage = null;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnOK.HyperlinkStyle = false;
            this.btnOK.ImageMargin = 2;
            this.btnOK.LeftImage = null;
            this.btnOK.Location = new System.Drawing.Point( 128, 81 );
            this.btnOK.Margin = new System.Windows.Forms.Padding( 5 );
            this.btnOK.Name = "btnOK";
            this.btnOK.PushStyle = true;
            this.btnOK.RightImage = null;
            this.btnOK.Size = new System.Drawing.Size( 100, 28 );
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnOK.TextLeftMargin = 2;
            this.btnOK.TextRightMargin = 2;
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderColor = System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 100 ) ) ) ), ( (int)( ( (byte)( 40 ) ) ) ), ( (int)( ( (byte)( 50 ) ) ) ), ( (int)( ( (byte)( 120 ) ) ) ) );
            this.btnCancel.CenterImage = null;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnCancel.HyperlinkStyle = false;
            this.btnCancel.ImageMargin = 2;
            this.btnCancel.LeftImage = null;
            this.btnCancel.Location = new System.Drawing.Point( 236, 81 );
            this.btnCancel.Margin = new System.Windows.Forms.Padding( 5 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PushStyle = true;
            this.btnCancel.RightImage = null;
            this.btnCancel.Size = new System.Drawing.Size( 100, 28 );
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnCancel.TextLeftMargin = 2;
            this.btnCancel.TextRightMargin = 2;
            this.btnCancel.Click += new System.EventHandler( this.btnCancel_Click );
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd.MM.yyyy, h:m:ss  ";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point( 16, 32 );
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding( 4 );
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size( 183, 22 );
            this.dateTimePicker1.TabIndex = 0;
            // 
            // ModifyTimeStamp
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 8F, 16F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 348, 120 );
            this.ControlBox = false;
            this.Controls.Add( this.dateTimePicker1 );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding( 4 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModifyTimeStamp";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Time Stamp";
            this.TopMost = true;
            this.ResumeLayout( false );

        }

        #endregion

        private ZoneFiveSoftware.Common.Visuals.Button btnOK;
        private ZoneFiveSoftware.Common.Visuals.Button btnCancel;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}