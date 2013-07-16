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
using System.Runtime.InteropServices;
using ZoneFiveSoftware.Common.Visuals;

namespace ActivityPicturePlugin.Helper
{
    public partial class VolumeSlider : UserControl
    {
        [DllImport( "winmm.dll" )]
        public static extern int waveOutGetVolume( IntPtr hwo, out uint dwVolume );

        [DllImport( "winmm.dll" )]
        public static extern int waveOutSetVolume( IntPtr hwo, uint dwVolume );

        public VolumeSlider()
        {
            InitializeComponent();
            this.VolumeChanged += new VolumeChangedEventHandler( VolumeSlider_VolumeChanged );
            // At this point, CurrVol gets assigned the volume
            uint CurrVol = 0;
            try
            {
                //Mono TODO:
                waveOutGetVolume( IntPtr.Zero, out CurrVol );
            }
            catch { }
            ushort CalcVol = (ushort)( CurrVol & 0x0000ffff );
            // Get the volume on a scale of 1 to 100
            Volume = (uint)( CalcVol / ( ushort.MaxValue / 100 ) );
        }

        public delegate void VolumeChangedEventHandler( System.Object sender, System.EventArgs e );
        public event VolumeChangedEventHandler VolumeChanged;
        private Color barForeColor = Color.Blue;
        private Color barBackColor = Color.Black;
        private Color barTextColor = Color.Black;
        private int horMargin = 5;
        private int verMargin = 5;
        private uint volume;
        public uint Volume
        {
            get { return volume; }
            set
            {
                volume = Math.Max( 0, Math.Min( value, 100 ) );
                VolumeChanged( this, new EventArgs() );
            }
        }

        private bool m_showVolumeText = true;
        public bool ShowVolumeText
        {
            get { return m_showVolumeText; }
            set { m_showVolumeText = value; }
        }

        private ITheme m_theme;
        public void ThemeChanged( ITheme visualTheme )
        {
            m_theme = visualTheme;
            //barBackColor = visualTheme.Control;
            //barForeColor = visualTheme.ControlText;
            //this.BackColor = visualTheme.Control;
            //this.ForeColor = visualTheme.ControlText;

            barBackColor = System.Drawing.SystemColors.GrayText;
            barForeColor = visualTheme.MainHeader;
            barTextColor = visualTheme.ControlText;
            this.BackColor = visualTheme.Control;
            this.ForeColor = visualTheme.Window;
        }

        private void VolumeSlider_Load( object sender, EventArgs e )
        {
            //this.volume = 80;
        }

        private void VolumeSlider_VolumeChanged( System.Object sender, System.EventArgs e )
        {
            // Calculate the volume that's being set
            int NewVolume = ( ( ushort.MaxValue / 100 ) * (int)( Volume ) );
            // Set the same volume for both the left and the right channels
            uint NewVolumeAllChannels = ( ( (uint)NewVolume & 0x0000ffff ) | ( (uint)NewVolume << 16 ) );
            // Set the volume
            try
            {
                //Mono TODO:
                waveOutSetVolume( IntPtr.Zero, NewVolumeAllChannels );
            }
            catch { }

            this.Invalidate();
        }

        private void VolumeSlider_MouseMove( object sender, MouseEventArgs e )
        {
            if ( Capture & e.Button == MouseButtons.Left )
            {
                double eX = (double)( e.X ) - horMargin;
                double max = (double)this.Width - 2 * horMargin;
                if ( eX < 0 ) eX = 0;
                if ( eX > max ) eX = max;

                volume = (uint)( 100 * ( eX / max ) );
                VolumeChanged( this, new EventArgs() );
            }
        }

        private void VolumeSlider_MouseUp( object sender, MouseEventArgs e )
        {
            Capture = false;
            this.Invalidate();
        }

        private void VolumeSlider_MouseDown( object sender, MouseEventArgs e )
        {
            if ( e.Button == MouseButtons.Left )
            {
                Capture = true;
                VolumeSlider_MouseMove( sender, e );
            }
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            base.OnPaint( e );

            //double imax = 10000;
            //Pen p = new Pen(BarBackColor, 1);
            //for (int i = 0; i <= imax; i++)
            //{
            //    int x = (int)(horMargin + (double)(i) / imax * (this.Width - 2 * horMargin));
            //    int y1 = (int)(verMargin + (1 - (double)(i) / imax) * (this.Height - 2 * verMargin));
            //    int y2 = this.Height - (int)(verMargin);
            //    if ((double)(i) * 100 / imax >= volume)
            //        p.Color = barBackColor;
            //    else
            //        p.Color = barForeColor;
            //    e.Graphics.DrawLine(p, x, y1, x, y2);
            //}
            /*int x = (int)( horMargin + (double)( volume ) / 100 * ( this.Width - 2 * horMargin ) );
            Point p1 = new Point( horMargin, this.Height - verMargin );
            Point p2 = new Point( x, this.Height - verMargin );
            Point p3 = new Point( x, (int)( verMargin + ( 1 - (double)( volume ) / 100 ) * ( this.Height - 2 * verMargin ) ) );
            Point p4 = new Point( this.Width - horMargin, verMargin );
            Point p5 = new Point( this.Width - horMargin, this.Height - verMargin );
            Point[] vol = { p1, p2, p3 };
            Point[] full = { p2, p3, p4, p5 };*/

            int offsetNoVolume = 2;	//slightly widens the low end of the volume graphic.
            int x = (int)( horMargin + (double)( volume ) / 100 * ( this.Width - 2 * horMargin ) );
            double slope = (double)( this.Height - offsetNoVolume - 2 * verMargin ) / ( this.Width - 2 * horMargin );
            Point p1 = new Point( horMargin, this.Height - verMargin );
            Point p2 = new Point( x, this.Height - verMargin );
            Point p3 = new Point( x, ( this.Height - offsetNoVolume - verMargin ) - (int)( x * slope ) );
            Point p4 = new Point( this.Width - horMargin, verMargin );
            Point p5 = new Point( this.Width - horMargin, this.Height - verMargin );
            Point p6 = new Point( horMargin, this.Height - verMargin - offsetNoVolume );
            Point[] vol = { p1, p2, p3, p6 };
            Point[] full = { p2, p3, p4, p5 };

            string text = volume + "%";

            using ( Brush bvol = new SolidBrush( this.barForeColor ),
                    bfull = new SolidBrush( this.barBackColor ),
                    btext = new SolidBrush( this.barTextColor ) )
            {
                if ( this.Enabled )
                {
                    e.Graphics.FillPolygon( bvol, vol );
                    e.Graphics.FillPolygon( bfull, full );
                    if ( m_showVolumeText && Capture )
                        e.Graphics.DrawString( text, this.Font, btext, new PointF( 0, 0 ) );
                }
                else
                {
                    Brush disabled = SystemBrushes.GrayText;
                    e.Graphics.FillPolygon( disabled, vol );
                    e.Graphics.FillPolygon( disabled, full );
                }
            }
        }

    }
}
