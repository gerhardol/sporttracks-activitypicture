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
    partial class TimeOffset
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
            {
                this.nudHour = new System.Windows.Forms.NumericUpDown();
                this.nudMinute = new System.Windows.Forms.NumericUpDown();
                this.nudSecond = new System.Windows.Forms.NumericUpDown();
                this.label1 = new System.Windows.Forms.Label();
                this.label2 = new System.Windows.Forms.Label();
                this.label3 = new System.Windows.Forms.Label();
                this.btnOK = new ZoneFiveSoftware.Common.Visuals.Button();
                this.btnCancel = new ZoneFiveSoftware.Common.Visuals.Button();
                ((System.ComponentModel.ISupportInitialize)(this.nudHour)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.nudMinute)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.nudSecond)).BeginInit();
                this.SuspendLayout();
                // 
                // nudHour
                // 
                this.nudHour.Location = new System.Drawing.Point(12, 29);
                this.nudHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
                this.nudHour.Minimum = new decimal(new int[] {
            23,
            0,
            0,
            -2147483648});
                this.nudHour.Name = "nudHour";
                this.nudHour.Size = new System.Drawing.Size(42, 20);
                this.nudHour.TabIndex = 0;
                // 
                // nudMinute
                // 
                this.nudMinute.Location = new System.Drawing.Point(60, 29);
                this.nudMinute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
                this.nudMinute.Minimum = new decimal(new int[] {
            59,
            0,
            0,
            -2147483648});
                this.nudMinute.Name = "nudMinute";
                this.nudMinute.Size = new System.Drawing.Size(42, 20);
                this.nudMinute.TabIndex = 1;
                // 
                // nudSecond
                // 
                this.nudSecond.Location = new System.Drawing.Point(108, 29);
                this.nudSecond.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
                this.nudSecond.Minimum = new decimal(new int[] {
            59,
            0,
            0,
            -2147483648});
                this.nudSecond.Name = "nudSecond";
                this.nudSecond.Size = new System.Drawing.Size(42, 20);
                this.nudSecond.TabIndex = 2;
                // 
                // label1
                // 
                this.label1.AutoSize = true;
                this.label1.Location = new System.Drawing.Point(26, 13);
                this.label1.Name = "label1";
                this.label1.Size = new System.Drawing.Size(13, 13);
                this.label1.TabIndex = 3;
                this.label1.Text = "h";
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Location = new System.Drawing.Point(69, 14);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(23, 13);
                this.label2.TabIndex = 4;
                this.label2.Text = "min";
                // 
                // label3
                // 
                this.label3.AutoSize = true;
                this.label3.Location = new System.Drawing.Point(122, 13);
                this.label3.Name = "label3";
                this.label3.Size = new System.Drawing.Size(12, 13);
                this.label3.TabIndex = 5;
                this.label3.Text = "s";
                // 
                // btnOK
                // 
                this.btnOK.BackColor = System.Drawing.Color.Transparent;
                this.btnOK.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
                this.btnOK.CenterImage = null;
                this.btnOK.DialogResult = System.Windows.Forms.DialogResult.None;
                this.btnOK.HyperlinkStyle = false;
                this.btnOK.ImageMargin = 2;
                this.btnOK.LeftImage = null;
                this.btnOK.Location = new System.Drawing.Point(177, 66);
                this.btnOK.Name = "btnOK";
                this.btnOK.PushStyle = true;
                this.btnOK.RightImage = null;
                this.btnOK.Size = new System.Drawing.Size(75, 23);
                this.btnOK.TabIndex = 6;
                this.btnOK.Text = "OK";
                this.btnOK.TextAlign = System.Drawing.StringAlignment.Center;
                this.btnOK.TextLeftMargin = 2;
                this.btnOK.TextRightMargin = 2;
                this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
                // 
                // btnCancel
                // 
                this.btnCancel.BackColor = System.Drawing.Color.Transparent;
                this.btnCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
                this.btnCancel.CenterImage = null;
                this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.None;
                this.btnCancel.HyperlinkStyle = false;
                this.btnCancel.ImageMargin = 2;
                this.btnCancel.LeftImage = null;
                this.btnCancel.Location = new System.Drawing.Point(96, 66);
                this.btnCancel.Name = "btnCancel";
                this.btnCancel.PushStyle = true;
                this.btnCancel.RightImage = null;
                this.btnCancel.Size = new System.Drawing.Size(75, 23);
                this.btnCancel.TabIndex = 7;
                this.btnCancel.Text = "Cancel";
                this.btnCancel.TextAlign = System.Drawing.StringAlignment.Center;
                this.btnCancel.TextLeftMargin = 2;
                this.btnCancel.TextRightMargin = 2;
                this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
                // 
                // TimeOffset
                // 
                this.AcceptButton = this.btnOK;
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(264, 101);
                this.ControlBox = false;
                this.Controls.Add(this.btnCancel);
                this.Controls.Add(this.btnOK);
                this.Controls.Add(this.label3);
                this.Controls.Add(this.label2);
                this.Controls.Add(this.label1);
                this.Controls.Add(this.nudSecond);
                this.Controls.Add(this.nudMinute);
                this.Controls.Add(this.nudHour);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "TimeOffset";
                this.ShowIcon = false;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                this.Text = "Time Offset";
                this.TopMost = true;
                ((System.ComponentModel.ISupportInitialize)(this.nudHour)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.nudMinute)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.nudSecond)).EndInit();
                this.ResumeLayout(false);
                this.PerformLayout();

            }

        #endregion

        private System.Windows.Forms.NumericUpDown nudHour;
        private System.Windows.Forms.NumericUpDown nudMinute;
        public System.Windows.Forms.NumericUpDown nudSecond;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private ZoneFiveSoftware.Common.Visuals.Button btnOK;
        private ZoneFiveSoftware.Common.Visuals.Button btnCancel;
        }
    }