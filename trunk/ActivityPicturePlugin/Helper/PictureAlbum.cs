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
using System.Drawing;
using System.Windows.Forms;
using QuartzTypeLib;

using System.IO;

namespace ActivityPicturePlugin.Helper
{
    public partial class PictureAlbum : UserControl
    {
        public PictureAlbum()
        {
            InitializeComponent();
            EnableDoubleBuffering();
            this.ActivityChanged += new ActivityChangedEventHandler( PictureAlbum_ActivityChanged );
            this.VideoChanged += new CurrentVideoIndexChangedEventHandler( PictureAlbum_VideoChanged );
            this.SelectedChanged += new SelectedChangedEventHandler( PictureAlbum_SelectedChanged );
            this.ImageRectangles[0] = new Rectangle();
            this.albumToolTipTimer.Tick += new System.EventHandler( ToolTipTimer_Tick );
            this.albumToolTipTimer.Interval = 1000; // Increased delay so it's less annoying
        }

        #region Overrides
        [System.Security.Permissions.SecurityPermission( System.Security.Permissions.SecurityAction.LinkDemand,
            Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode )]
        protected override void WndProc( ref Message m )
        {
            try
            {
                switch ( m.Msg )
                {
                    case WM_VSCROLL:
                        //return;
                        break;
                    case WM_GRAPHNOTIFY:
                        int lEventCode;
                        int lParam1, lParam2;
                        while ( true )
                        {
                            if ( MediaEventEx == null ) break;
                            MediaEventEx.GetEvent( out lEventCode,
                                out lParam1,
                                out lParam2,
                                0 );

                            MediaEventEx.FreeEventParams( lEventCode, lParam1, lParam2 );
                            if ( ( lEventCode == EC_COMPLETE ) || //video is at end position
                                ( lEventCode == EC_USERABORT ) ||
                                ( lEventCode == EC_ERRORABORT ) )
                            {
                                FilGrMan.Stop();
                                FilGrMan.CurrentPosition = 0;
                                CurrentStatus = MediaStatus.Stopped;
                                UpdateVideoToolBar( this, new EventArgs() );
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch ( System.Runtime.InteropServices.COMException ex )
            {
                if ( (uint)ex.ErrorCode == 0x80004004 ) //Operation Aborted (E_ABORT)
                {
                    //TODO ???  Not sure why these are occurring nor what to do about them.
                    //Seems to run just fine, though.
                }
                else
                    System.Diagnostics.Debug.Assert( false, ex.Message );
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.Assert( false, ex.Message );
                //break;
            }
            base.WndProc( ref m );
        }


        public class ZoomEventArgs : System.EventArgs
        {
            public ZoomEventArgs()
            {
            }
            public ZoomEventArgs( int inc )
            {
                _increment = inc;
            }
            private int _increment = 0;
            public int Increment
            {
                get { return _increment; }
                set { _increment = value; }
            }
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            this.PaintAlbumView( false, e );
            base.OnPaint( e );
        }
        #endregion

        #region EventHandlers
        public delegate void SelectedChangedEventHandler( System.Object sender, SelectedChangedEventArgs e );
        public delegate void ActivityChangedEventHandler( System.Object sender, System.EventArgs e );
        public delegate void ZoomChangeEventHandler( System.Object sender, ZoomEventArgs e );
        public delegate void UpdateVideoToolBarEventHandler( System.Object sender, System.EventArgs e );
        public delegate void ShowVideoOptionsEventHandler( System.Object sender, System.EventArgs e );
        public delegate void CurrentVideoIndexChangedEventHandler( System.Object sender, System.EventArgs e );
        #endregion

        #region Events
        public event SelectedChangedEventHandler SelectedChanged;
        public event ActivityChangedEventHandler ActivityChanged;
        public event ZoomChangeEventHandler ZoomChange;
        public event UpdateVideoToolBarEventHandler UpdateVideoToolBar;
        public event ShowVideoOptionsEventHandler ShowVideoOptions;
        public event CurrentVideoIndexChangedEventHandler VideoChanged;
        public class SelectedChangedEventArgs : EventArgs
        {
            public SelectedChangedEventArgs( int ix, Rectangle r )
            {
                selectedIndex = ix;
                rect = r;
            }

            private int selectedIndex = -1;
            public int SelectedIndex
            {
                get { return selectedIndex; }
                private set { selectedIndex = value; }
            }

            private Rectangle rect = new Rectangle( -1, -1, -1, -1 );
            public Rectangle Rect
            {
                get { return rect; }
                private set { rect = value; }
            }
        }

        #endregion

        #region public members
        public List<ImageData> ImageList
        {
            get { return imagelist; }
            set { imagelist = value; }
        }

        private int selectedIndex = -1;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                Rectangle r;
                if ( ( value >= 0 ) && ( value < ImageRectangles.Length ) )
                    r = ImageRectangles[value];
                else
                    r = new Rectangle( -1, -1, -1, -1 );
                this.SelectedChanged( this, new SelectedChangedEventArgs( value, r ) );
            }
        }

        public int Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

        public bool NoThumbNails
        {
            get { return nothumbnails; }
            set { nothumbnails = value; }
        }

        // Determines whether the images are allowed to be zoomed larger
        // than the display.  Note: Active videos cannot be larger.
        private MaxImageSize m_MaxImageSize = MaxImageSize.NoLimit;
        public MaxImageSize MaximumImageSize
        {
            get { return m_MaxImageSize; }
            set { m_MaxImageSize = value; }
        }

        //public ImageSortMode SortMode
        //{
        //    get { return sortmode; }
        //    set { sortmode = value; }
        //}
        public enum MaxImageSize
        {
            NoLimit = 0,
            FitToWindow
        }
        public enum ImageSortMode
        {
            none,
            byDateTimeAscending,
            byDateTimeDescending,
            byTypeAscending,
            byTypeDescending,
            byExifGPSAscending,
            byExifGPSDescending,
            byAltitudeAscending,
            byAltitudeDescending,
            byCommentAscending,
            byCommentDescending,
            byTitleAscending,
            byTitleDescending,
            byCameraModelAscending,
            byCameraModelDescending,
            byPhotoSourceAscending,
            byPhotoSourceDescending
        }

        #endregion

        #region private members
        private bool nothumbnails;
        private int zoom = 20;
        private List<ImageData> imagelist;// = new List<ImageData>();
        private Rectangle[] ImageRectangles = new Rectangle[1];
        //private ImageSortMode sortmode = ImageSortMode.none;
        #endregion

        #region private methods

        private Rectangle GetImagePositions()
        {
            try
            {
                int NewHeight = (int)( 30 + 5 * Zoom );
                int NewWidth = 120;
                int left = 0, top = 0, row = 0;
                int nImagesInRow = 0;

                if ( ( this.ImageList != null ) && ( this.ImageList.Count > 0 ) )
                {
                    ImageRectangles = new Rectangle[this.ImageList.Count];
                    for ( int i = 0; i <= this.ImageList.Count - 1; i++ )
                    {
                        nImagesInRow++;
                        NewHeight = (int)( 30 + 5 * Zoom );
                        NewWidth = (int)( ImageList[i].Ratio * NewHeight );
                        if ( ( this.Width < NewWidth ) && ( m_MaxImageSize == MaxImageSize.FitToWindow ) )
                        {
                            //Maximum zoom exceeded. Calculate maximum and re-call GetImagePositions
                            double zoomchange = ( ( ( this.Width / ImageList[i].Ratio ) - 30 ) / 5 ) - Zoom;
                            Zoom += (int)Math.Floor( zoomchange );
                            ZoomChange( this, new ZoomEventArgs( 0 ) );	// Signal a zoom change has occurred.
                            return GetImagePositions();
                        }

                        //calculate new Position
                        if ( ( ( left + NewWidth ) > this.Width ) && ( nImagesInRow > 1 ) )  //(nImagesInRow>1) ensures at least one image in row
                        {
                            //start new row
                            left = 0;
                            row += 1;
                            top += NewHeight;
                            nImagesInRow = 1;	// this image begins a new row
                        }

                        if ( ( i != currentVideoIndex ) || ( NewWidth < this.Width - 1 ) )
                            ImageRectangles[i] = new Rectangle( left, top, NewWidth, NewHeight );
                        else
                        {
                            int vidHeight = (int)( ( this.Width - 1 ) * (double)NewHeight / NewWidth );
                            // The video cannot be wider than this
                            ImageRectangles[i] = new Rectangle( left, top, this.Width - 1, vidHeight );

                            // Even though the top of the next row is calculated in the next iteration
                            // we decrement the current top the difference between the NewHeight and
                            // this current vidHeight.
                            top -= ( NewHeight - vidHeight );
                        }

                        //left for the next image
                        left += NewWidth;
                    }
                    return new Rectangle( 0, 0, this.Width, top + NewHeight );
                }
                else return new Rectangle( 0, 0, 0, 0 );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                return new Rectangle(0, 0, 0, 0);
            }
        }

        private void DrawImages( bool CompleteRedraw, PaintEventArgs e )
        {
            Image img = null;
            try
            {
                //int ZoomLevel = this.parentctl.GetZoom();
                //int NewHeight = (int)(30 + 5 * ZoomLevel);
                //int NewWidth = 120; //reference width
                //int left = 0, top = 0, row = 0;

                //List<ImageData> imgList = this.ImageList;//.parentctl.Images;
                if ( this.ImageList.Count > 0 )
                {
                    //ImageRectangles = new Rectangle[imgList.Count];
                    Graphics g = e.Graphics;
                    int ixSelected = -1;
                    for ( int i = 0; i < this.ImageList.Count; i++ )
                    {
                        if ( ( this.ImageList[i].ThumbnailPath != null ) )
                        {
                            if ( NoThumbNails ) img = new Bitmap( this.ImageList[i].PhotoSource );
                            else img = this.ImageList[i].EW.GetBitmap();

                            if ( img == null )
                            {
                                //Image is missing.  Load the 'delete' image
                                img = (Image)ZoneFiveSoftware.Common.Visuals.CommonResources.Images.Delete16.Clone();
                            }
                            //    NewWidth = (int)((double)(img.Width) / (double)(img.Height) * NewHeight);
                        }

                        //draw the images
                        using ( Pen p1 = new Pen( Brushes.Black, 2 ) )
                        {
                            g.DrawImage( img, ImageRectangles[i] );
                            g.DrawRectangle( p1, ImageRectangles[i] );
                            if ( this.ImageList[i].Selected ) ixSelected = i;
                        }

                        img.Dispose();
                        img = null;
                    }

                    // test for solution without invalidating
                    using ( Region r = new Region() )
                    {
                        for ( int i = 0; i < ImageRectangles.Length; i++ )
                        {
                            r.Xor( ImageRectangles[i] );
                        }

                        //Rectangle rect = new Rectangle(new Point(0, 0), this.panel1.Size);
                        ////r.Complement(rect);
                        using ( SolidBrush sb = new SolidBrush( this.BackColor ) )
                        {
                            g.FillRegion( sb, r );
                        }
                    }

                    if ( ( this.FilGrMan != null ) & ( currentVideoIndex != -1 ) )
                    {
                        FilGrMan.SetWindowPosition( ImageRectangles[currentVideoIndex].Left,
                            ImageRectangles[currentVideoIndex].Top,
                            ImageRectangles[currentVideoIndex].Width,
                            ImageRectangles[currentVideoIndex].Height );
                    }

                    //draw yellow border around selected image
                    if ( ixSelected != -1 )
                    {
                        //using ( Pen p2 = new Pen( Brushes.Blue, 2 ) )
                        using ( Pen p2 = new Pen( Brushes.Yellow, 2 ) )
                        {
                            g.DrawRectangle( p2, ImageRectangles[ixSelected] );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                // throw;
            }
            finally
            {
                if ( img != null )
                    img.Dispose();
                img = null;
            }
        }

        private int GetIndexOfCurrentImage( Point p )
        {
            try
            {
                if ( ImageRectangles != null && this.ImageList.Count != 0 )
                {
                    for ( int i = 0; i < ImageRectangles.Length; i++ )
                    {
                        if ( ImageRectangles[i] != null )
                        {
                            using ( Region r = new Region( ImageRectangles[i] ) )
                            {
                                if ( r.IsVisible( p ) )
                                    return i;
                            }
                        }
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                return -1;
            }

        }

        private void EnableDoubleBuffering()
        {
            // Set the value of the double-buffering style bits to true.
            this.SetStyle( ControlStyles.DoubleBuffer |
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint,
               true );
            this.SetStyle( ControlStyles.ResizeRedraw, true );
            this.UpdateStyles();
        }

        private void PaintAlbumView( bool CompleteRedraw, PaintEventArgs e )
        {
            Rectangle rect = this.GetImagePositions();
            if ( rect.Height == 0 ) rect.Height = 1;	// No images to display.  This line results in control being resized.
            if ( this.Height != rect.Height ) this.Height = rect.Height;
            this.DrawImages( CompleteRedraw, e );
        }
        #endregion

        #region public methods
        public void PaintAlbumView( bool CompleteRedraw )
        {
            using ( PaintEventArgs e = new PaintEventArgs( this.CreateGraphics(), this.DisplayRectangle ) )
            {
                PaintAlbumView( CompleteRedraw, e );
            }

        }

        public void ActivityChanging()
        {
            this.ActivityChanged( this, new EventArgs() );
        }

        public void ThemeChanged( ZoneFiveSoftware.Common.Visuals.ITheme visualTheme )
        {
            this.BackColor = visualTheme.Control;
            this.ForeColor = visualTheme.ControlText;
        }
        #endregion

        #region Event handler methods

        //ToolTip support - hints from omb
        // private member variables of the Control - initialization omitted
        Timer albumToolTipTimer = new Timer();
        //bool albumTooltipDisabled = false; // is set to true, whenever a tooltip would be annoying, e.g. while a context menu is shown
        int IndexOfLastHoveredImage = -1;

        private string GenerateToolTipText()
        {
            string tooltip = "";
            int i = IndexOfLastHoveredImage;
            if ( i >= 0 )
            {
                tooltip = this.ImageList[i].PhotoSource;
                DateTime dt = DateTime.MinValue;
                if ( dt < this.ImageList[i].EW.DateTimeOriginal ) tooltip += Environment.NewLine + this.ImageList[i].DateTimeOriginal;
                if ( this.ImageList[i].EW.GPSLatitude != 0 ) tooltip += Environment.NewLine + this.ImageList[i].ExifGPS.Replace( Environment.NewLine, ", " );
                if ( !String.IsNullOrEmpty( this.ImageList[i].Title ) ) tooltip += Environment.NewLine + this.ImageList[i].Title;
            }
            return tooltip;
        }

        private void ToolTipTimer_Tick( object sender, EventArgs e )
        {
            albumToolTipTimer.Stop();

            if ( IndexOfLastHoveredImage >= 0 &&
                    ( this.ImageList[IndexOfLastHoveredImage].Type == ImageData.DataTypes.Image ||
                    this.ImageList[IndexOfLastHoveredImage].Type == ImageData.DataTypes.Video ) )
            {
                string tooltip = GenerateToolTipText();
                this.toolTip1.SetToolTip( this, tooltip );
                //this.toolTip1.Show( tooltip, this );
            }
            else
                this.toolTip1.Hide( this );
        }

        private void PictureAlbum_MouseEnter( object sender, EventArgs e )
        {
            //Activate tooltip
            toolTip1.Active = true;
            this.Focus();
        }

        private void PictureAlbum_MouseLeave( object sender, EventArgs e )
        {
            //Deactivate tooltip
            albumToolTipTimer.Stop();
            IndexOfLastHoveredImage = -1;
            toolTip1.Active = false;
        }

        private void PictureAlbum_MouseMove( object sender, MouseEventArgs e )
        {
            int i = GetIndexOfCurrentImage( e.Location );
            // Activate tip if mouse is over a valid section of the control (ie. an image).
            toolTip1.Active = ( i >= 0 );

            if ( i == IndexOfLastHoveredImage )
                return; // Return if we've already processed a tip for this index
            else
            {
                // It's a new index so hide the old tip and reactivate the timer
                // so that a new tip will popup after a time.
                if ( this.toolTip1.Active )
                    toolTip1.Hide( this );
                IndexOfLastHoveredImage = i;
                albumToolTipTimer.Stop();
                albumToolTipTimer.Start();
            }

        }

        private void toolTip1_Popup( object sender, PopupEventArgs e )
        {
            // Zero length string used here as a flag of a message that should be cancelled. 
            // ie. Invalid IndexOfLastHoveredImage.
            string s = "";

            if ( IndexOfLastHoveredImage >= 0 ) s = GenerateToolTipText();
            if ( toolTip1.GetToolTip( this ) != s )
            {
                // It seems that when the tip first attempts to popup, it shows with its previous message.
                // If this message doesn't match what the tip should be for this image, cancel the operation.
                if ( string.IsNullOrEmpty( s ) )
                {
                    // Attempting to set a zero length string.  This seems to cause the tooltip to disable
                    // temporarily causing odd behaviour.  Instead, hide it.
                    toolTip1.Hide( this );
                    e.Cancel = true;        // Do not set the zero length tooltip.
                }
                else
                    e.Cancel = true;    // Do not show the previous tooltip message
            }
            else
            {
                // noop: Nothing to do.  Seems good.
            }

        }

        private void PictureAlbum_MouseDoubleClick( object sender, MouseEventArgs e )
        {
            if ( e.Button == System.Windows.Forms.MouseButtons.Left )
            {
                int i = this.GetIndexOfCurrentImage( e.Location );
                if ( i >= 0 )
                {
                    //Note: The click does not "get through"toolTip1 Video
                    Helper.Functions.OpenExternal( this.ImageList[i] );
                }
            }
        }

        private void PictureAlbum_MouseClick( object sender, MouseEventArgs e )
        {
            try
            {
                if ( ( e.Button == System.Windows.Forms.MouseButtons.Left ) || ( e.Button == System.Windows.Forms.MouseButtons.Right ) )
                {
                    CurrentVideoIndex = -1;
                    int i = this.GetIndexOfCurrentImage( e.Location );
                    if ( i >= 0 )
                    {
                        // Select it if it isn't already
                        if ( !this.ImageList[i].Selected )
                            SelectedIndex = i;
                    }
                    else
                    {
                        // Deselect all images
                        SelectedIndex = -1;
                    }
                }

                if ( e.Button == System.Windows.Forms.MouseButtons.Left )
                {
                    if ( selectedIndex != -1 )
                    {
                        if ( this.ImageList[selectedIndex].Type == ImageData.DataTypes.Video )
                            this.OpenVideo( this.ImageList[selectedIndex].PhotoSource, selectedIndex );
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
        }

        #region PictureAlbum: Keyboard Scrolling
        private void PictureAlbumMoveSelectionUp()
        {
            List<ImageData> id = this.ImageList;
            if ( ( id == null ) || ( id.Count < 0 ) ) return;

            int ixSelected = -1;
            int ixNew = -1;
            Rectangle rSelected = new Rectangle( -1, -1, -1, -1 );
            int yNewRow = Int32.MinValue;

            int i = SelectedIndex;
            if ( ( i <= -1 ) || ( i >= id.Count ) ) i = 0;
            for ( i = id.Count - 1; i >= 0; i-- )
            {
                if ( ixSelected == -1 )
                {
                    if ( id[i].Selected )
                    {
                        ixSelected = i;
                        rSelected = ImageRectangles[i];
                    }
                    else continue;  // No point carrying on until we find the selected item. NEXT.
                }

                // We're at the first item.  Break out.
                if ( i == 0 )
                {
                    if ( ixNew == -1 )
                    {
                        ixNew = i;
                        break;
                    }
                }

                // We're at the highest row.  Select the first item.
                if ( rSelected.Top == 0 )
                {
                    ixNew = 0;
                    break;
                }

                if ( ImageRectangles[i].Y < rSelected.Y )
                {
                    if ( yNewRow <= ImageRectangles[i].Y ) yNewRow = ImageRectangles[i].Y;  // We found a higher row
                    else break;	// We found a row too high.  Break out.

                    if ( rSelected.X == 0 )
                    {
                        // The currently selected item is the left-most item in the row
                        // We want a left-align to the window match with an item in the next row if we can
                        if ( ImageRectangles[i].X == 0 )
                        {
                            ixNew = i;
                            break;
                        }
                    }
                    else if ( ( ixSelected + 1 == id.Count ) || ( ImageRectangles[ixSelected + 1].Y > rSelected.Y ) )
                    {
                        // The currently selected item is the right-most item in the row
                        // We want a right-align match with an item in the previous row if we can
                        if ( ImageRectangles[i - 1].Y <= yNewRow )
                        {
                            ixNew = i;
                            break;
                        }
                    }
                    else
                    {
                        // The currently selected item is not the right-most item in the row
                        // We want a left-align to the item match with an item in the previous row
                        if ( ImageRectangles[i].Width >= rSelected.Width )
                        {
                            if ( ( rSelected.Left >= ImageRectangles[i].Left ) && ( rSelected.Right <= ImageRectangles[i].Right ) )
                                ixNew = i;
                        }
                        else if ( rSelected.Width >= ImageRectangles[i].Width )
                        {
                            if ( ( ImageRectangles[i].Right <= rSelected.Right ) && ( ImageRectangles[i].Left >= rSelected.Left ) )
                                ixNew = i;
                        }

                        if ( rSelected.Left > ImageRectangles[i].Right )
                        {
                            //We either went too far or this is the last image on the previous row
                            if ( ixNew == -1 ) ixNew = i;
                            break;
                        }

                        if ( rSelected.Right < ImageRectangles[i].Left )
                            continue;

                        int x = 0, r = 0;
                        if ( rSelected.Left >= ImageRectangles[i].Left )
                        {
                            x = rSelected.Left;
                            if ( rSelected.Right < ImageRectangles[i].Right ) r = rSelected.Right;
                            else r = ImageRectangles[i].Right;
                        }
                        else
                        {
                            x = ImageRectangles[i].Left;
                            r = rSelected.Right;
                        }

                        if ( rSelected.Width >= ImageRectangles[i].Width )
                        {
                            // At least half of the image overlaps
                            // Prevents from 'unexpectedly' choosing an image that only overlaps
                            // by say... 1 pixel or so.
                            if ( ( ( (double)r - x ) / ImageRectangles[i].Width ) >= 0.500 )
                                ixNew = i;
                        }
                        else
                        {
                            // At least half of the image overlaps
                            // Prevents from 'unexpectedly' choosing an image that only overlaps
                            // by say... 1 pixel or so.
                            if ( ( ( (double)r - x ) / rSelected.Width ) >= 0.500 )
                            {
                                ixNew = i;
                                break;
                            }
                        }

                        if ( rSelected.Left > ImageRectangles[i].Right )
                        {
                            //We either went too far or this is the last image on the previous row
                            if ( ixNew == -1 ) ixNew = i;
                            break;
                        }
                    }
                }
            }

            if ( ( ixSelected != -1 ) && ( ixNew != -1 ) )
            {
                id[ixSelected].Selected = false;
                id[ixNew].Selected = true;
            }

            selectedIndex = ixNew;
            SelectedIndex = ixNew;
            //this.SelectedChanged( this, new SelectedChangedEventArgs( ixNew, this.ImageRectangles[ixNew] ) );
            this.Invalidate();

        }
        private void PictureAlbumMoveSelectionDown()
        {
            List<ImageData> id = this.ImageList;
            if ( ( id == null ) || ( id.Count < 0 ) ) return;

            int ixSelected = -1;
            int ixNew = -1;
            Rectangle rSelected = new Rectangle( -1, -1, -1, -1 );
            int yNewRow = Int32.MaxValue;

            int i = SelectedIndex;
            if ( ( i <= -1 ) || ( i >= id.Count ) ) i = 0;
            for ( i = 0; i < id.Count; i++ )
            {
                if ( ixSelected == -1 )
                {
                    if ( id[i].Selected )
                    {
                        ixSelected = i;
                        rSelected = ImageRectangles[i];
                    }
                    else continue;  // No point carrying on until we find the selected item. NEXT.
                }

                // We're at the last item.  Break out.
                if ( i + 1 == id.Count )
                {
                    if ( yNewRow >= ImageRectangles[i].Y ) ixNew = i;
                    break;
                }

                // We're at the lowest row.  Select the last item.
                if ( rSelected.Top == ImageRectangles[id.Count - 1].Top )
                {
                    ixNew = id.Count - 1;
                    break;
                }

                if ( ImageRectangles[i].Y > rSelected.Y )
                {
                    if ( yNewRow >= ImageRectangles[i].Y ) yNewRow = ImageRectangles[i].Y;  // We found a lower row
                    else break;	// We found a row too low.  Break out.

                    if ( rSelected.X == 0 )
                    {
                        // The currently selected item is the left-most item in the row
                        // We want a left-align to the window match with an item in the next row if we can
                        if ( ImageRectangles[i].X == 0 )
                        {
                            ixNew = i;
                            break;
                        }
                    }
                    else if ( ImageRectangles[ixSelected + 1].Y > rSelected.Y )
                    {
                        // The currently selected item is the right-most item in the row
                        // We want a right-align match with an item in the next row if we can
                        if ( ImageRectangles[i].X <= rSelected.X + rSelected.Width )
                        {
                            ixNew = i;
                            if ( ImageRectangles[i].X + ImageRectangles[i].Width > rSelected.X + rSelected.Width )
                                break;
                        }
                        else break;
                    }
                    else
                    {
                        // The currently selected item is not the right-most item in the row
                        // We want a left-align to the item match with an item in the next row
                        if ( ImageRectangles[i].Width >= rSelected.Width )
                        {
                            if ( ( ( rSelected.Left >= ImageRectangles[i].Left ) && ( rSelected.Right <= ImageRectangles[i].Right ) ) ||
                                ( ( ImageRectangles[i].Left + ImageRectangles[i].Width / 2 > rSelected.Left ) && ( ImageRectangles[i].Left < rSelected.Left ) ) ||
                                ( ImageRectangles[i].Left > rSelected.Left ) )
                            {
                                ixNew = i;
                                break;
                            }
                        }
                        else if ( rSelected.Width >= ImageRectangles[i].Width )
                        {
                            // At least half of the image overlaps
                            // Prevents from 'unexpectedly' choosing an image that only overlaps
                            // by say... 1 pixel or so.
                            if ( ( ImageRectangles[i].Left >= rSelected.Left ) ||
                                ( ( ImageRectangles[i].Right - rSelected.Left ) * 2 > ImageRectangles[i].Width ) )
                            {
                                ixNew = i;
                                break;
                            }
                        }
                        else
                        {
                            if ( ixNew == -1 ) ixNew = i;
                            break;
                        }
                    }
                }
            }

            if ( ( ixSelected != -1 ) && ( ixNew != -1 ) )
            {
                id[ixSelected].Selected = false;
                id[ixNew].Selected = true;
            }
            selectedIndex = ixNew;
            SelectedIndex = ixNew;
            //this.SelectedChanged( this, new SelectedChangedEventArgs( ixNew, this.ImageRectangles[ixNew] ) );
            this.Invalidate();

        }
        private void PictureAlbumMoveSelectionRight()
        {
            // So much simpler than moving up and down, haha.
            List<ImageData> id = this.ImageList;
            if ( ( id == null ) || ( id.Count < 0 ) ) return;

            int i = SelectedIndex;
            if ( ( i != -1 ) && ( i < id.Count - 1 ) )
            {
                id[i].Selected = false;
                id[i + 1].Selected = true;
                selectedIndex = i + 1;
                SelectedIndex = i + 1;
            }
        }
        private void PictureAlbumMoveSelectionLeft()
        {
            // So much simpler than moving up and down, haha.
            List<ImageData> id = this.ImageList;
            if ( ( id == null ) || ( id.Count < 0 ) ) return;

            int i = SelectedIndex;
            if ( ( i > 0 ) && ( id.Count > 1 ) )
            {
                id[i].Selected = false;
                id[i - 1].Selected = true;
                selectedIndex = i - 1;
                SelectedIndex = i - 1;
            }

        }
        private void PictureAlbumMoveSelectionHome()
        {
            // Select the first image
            List<ImageData> id = this.ImageList;
            if ( ( id == null ) || ( id.Count < 0 ) ) return;

            int i = SelectedIndex;
            if ( ( i != -1 ) && ( i < id.Count ) )
                id[i].Selected = false;

            id[0].Selected = true;
            selectedIndex = 0;
            SelectedIndex = 0;


        }
        private void PictureAlbumMoveSelectionEnd()
        {
            // Select the last image
            List<ImageData> id = this.ImageList;
            if ( ( id == null ) || ( id.Count < 0 ) ) return;

            int i = SelectedIndex;
            if ( ( i != -1 ) && ( i < id.Count ) )
                id[i].Selected = false;
            id[id.Count - 1].Selected = true;
            selectedIndex = id.Count - 1;
            SelectedIndex = id.Count - 1;
        }
        #endregion

        private void PictureAlbum_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
        {
            try
            {
                if ( e.Control )
                {
                    if ( e.KeyCode == Keys.Up )
                    {
                        if ( this.Zoom < 99 ) this.ZoomChange( this, new ZoomEventArgs( 2 ) );
                    }
                    else if ( e.KeyCode == Keys.Down )
                    {
                        if ( this.Zoom > 1 ) this.ZoomChange( this, new ZoomEventArgs( -2 ) );
                    }
                }
                else
                {
                    if ( !e.Control )
                    {
                        if ( e.KeyCode == Keys.Right )
                            PictureAlbumMoveSelectionRight();
                        else if ( e.KeyCode == Keys.Left )
                            PictureAlbumMoveSelectionLeft();
                        else if ( e.KeyCode == Keys.Down )
                            PictureAlbumMoveSelectionDown();
                        else if ( e.KeyCode == Keys.Up )
                            PictureAlbumMoveSelectionUp();
                        else if ( e.KeyCode == Keys.Enter )
                        {
                            CurrentVideoIndex = -1;
                            int i = SelectedIndex;
                            if ( i >= 0 )
                            {
                                if ( this.ImageList[i].Type == ImageData.DataTypes.Video )
                                {
                                    if ( !this.OpenVideo( this.ImageList[i].PhotoSource, i ) )
                                        Helper.Functions.OpenExternal( this.ImageList[i] );
                                }
                                else
                                    Helper.Functions.OpenExternal( this.ImageList[i] );
                            }
                        }
                        else if ( e.KeyCode == Keys.Home )
                            PictureAlbumMoveSelectionHome();
                        else if ( e.KeyCode == Keys.End )
                            PictureAlbumMoveSelectionEnd();
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                //throw;
            }
        }

        private void PictureAlbum_ActivityChanged( object sender, EventArgs e )
        {
            CurrentVideoIndex = -1;
            SelectedIndex = -1;
        }

        #endregion

        #region Video

        #region Public

        internal enum MediaStatus { None, Stopped, Paused, Running };
        internal MediaStatus CurrentStatus = MediaStatus.None;

        internal void SetVideoPosition( int pos )
        {
            if ( FilGrMan != null )
                FilGrMan.CurrentPosition = FilGrMan.Duration * (double)( pos ) / 1000;
        }

        //returns the position in % of the overall length
        internal double GetVideoPosition()
        {
            try
            {
                if ( this.FilGrMan != null )
                {
                    double pos = this.FilGrMan.CurrentPosition / this.FilGrMan.Duration;
                    if ( ( pos >= 0 ) & ( pos <= 1 ) ) return pos;
                }
                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                return 0;
            }
        }

        // Get the current frame number
        // Used for taking snapshots
        internal int GetCurrentVideoFrame()
        {
            int iFrame = 0;
            try
            {
                if ( FilGrMan != null )
                {
                    if ( FilGrMan.AvgTimePerFrame != 0 )
                        iFrame = (int)( FilGrMan.CurrentPosition / FilGrMan.AvgTimePerFrame );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                return 0;
            }
            return iFrame;
        }

        // Get frame rate of the video
        // Used for taking snapshots
        internal double GetCurrentVideoTimePerFrame()
        {
            double dblTimePerFrame = 0;
            try
            {
                if (FilGrMan != null)
                {
                    dblTimePerFrame = FilGrMan.AvgTimePerFrame;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
            return dblTimePerFrame;
        }

        // Get the current frame number
        // Used for taking snapshots
        internal Size GetVideoSize()
        {
            Size size = new System.Drawing.Size( -1, -1 );
            try
            {
                if ( FilGrMan != null )
                {
                    size.Height = FilGrMan.SourceHeight;
                    size.Width = FilGrMan.SourceWidth;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
            return size;
        }

        // Returns true if video is an avi.
        // Currently only avis support taking snapshots
        public bool IsAvi()
        {
            if ( ( CurrentVideoIndex != -1 ) && ( this.ImageList != null ) )
            {
                System.IO.FileInfo fi = new FileInfo( this.ImageList[CurrentVideoIndex].PhotoSource );
                if ( String.Compare( fi.Extension.ToLower(), ".avi" ) == 0 )
                    return true;
            }
            return false;
        }

        internal void PauseVideo()
        {
            if ( ( FilGrMan != null ) && ( currentVideoIndex != -1 ) )
            {
                FilGrMan.Pause();
                CurrentStatus = MediaStatus.Paused;
            }
        }

        internal void StopVideo()
        {
            if ( ( FilGrMan != null ) && ( currentVideoIndex != -1 ) )
            {
                FilGrMan.Stop();
                FilGrMan.CurrentPosition = 0;
                CurrentStatus = MediaStatus.Stopped;
            }
        }

        internal void PlayVideo()
        {
            if ( currentVideoIndex != -1 )
            {
                if ( FilGrMan != null )
                {
                    FilGrMan.Run();
                    CurrentStatus = MediaStatus.Running;
                }
                else if ( this.ImageList != null )
                    Helper.Functions.OpenExternal( this.ImageList[currentVideoIndex] );
            }
        }
        #endregion

        #region Private
        private const int WM_APP = 0x8000;
        private const int WM_VSCROLL = 277;
        private const int WM_GRAPHNOTIFY = WM_APP + 1;
        private const int EC_COMPLETE = 0x01;
        private const int EC_USERABORT = 0x02;
        private const int EC_ERRORABORT = 0x03;
        private const int WS_CHILD = 0x40000000;
        private const int WS_CLIPCHILDREN = 0x2000000;
        private const int WS_EX_TOOLWINDOW = 0x00000080;    // Tool windows don't show up in taskbar, even when they're orphaned.
        private FilgraphManagerClass FilGrMan = null;
        private IMediaEventEx MediaEventEx = null;

        private int currentVideoIndex = -1;
        public int CurrentVideoIndex
        {
            get { return this.currentVideoIndex; }
            private set
            {
                bool bChanged = this.currentVideoIndex != value;
                this.currentVideoIndex = value;
                if ( bChanged ) this.VideoChanged( this, new EventArgs() );
            }
        }

        // Note:  Issues with video playback may be corrected by installing the
        // appropriate codec.  Just because the clip plays in VLC or Windows Media Player
        // it doesn't mean a codec is available for our uses.
        // Try installing K-Lite_Codec_Pack_995_Full or something similar.
        private bool OpenVideo( string file, int imageNumber )
        {
            bool bRet = true;
            try
            {
                //int ZoomLevel = this.parentctl.GetZoom();
                int height = (int)( 30 + 3 * Zoom );

                CurrentVideoIndex = imageNumber;
                FilGrMan = new FilgraphManagerClass();
                FilGrMan.RenderFile( file );

                int width = (int)( (double)( FilGrMan.DestinationWidth ) / (double)( FilGrMan.DestinationHeight ) * height );

                try
                {
                    FilGrMan.Owner = (int)this.Handle;
                    FilGrMan.WindowStyle = WS_CHILD | WS_CLIPCHILDREN;
                    FilGrMan.WindowStyleEx = WS_EX_TOOLWINDOW;	//Prevents ActiveMovie window from showing in taskbar during Cleanup()

                    //The video cannot be wider than this.
                    Rectangle r = ImageRectangles[imageNumber];
                    if ( r.Width > this.Width - 01 )
                    {
                        r.Width = this.Width - 01;
                        //Maintain aspect ratio.
                        r.Height = (int)( r.Width * (double)ImageRectangles[imageNumber].Height / ImageRectangles[imageNumber].Width );
                    }

                    FilGrMan.SetWindowPosition( ImageRectangles[imageNumber].Left,
                        r.Top, r.Width, r.Height );

                    /*FilGrMan.SetWindowPosition( ImageRectangles[imageNumber].Left,
                        ImageRectangles[imageNumber].Top,
                        ImageRectangles[imageNumber].Width,
                        ImageRectangles[imageNumber].Height );*/
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                    bRet = false;
                    FilGrMan.Visible = 0;
                    FilGrMan.Owner = 0;
                }

                MediaEventEx = FilGrMan as IMediaEventEx;
                MediaEventEx.SetNotifyWindow( (int)this.Handle, WM_GRAPHNOTIFY, 0 );

                FilGrMan.Run();

                CurrentStatus = MediaStatus.Running;
                ShowVideoOptions( this, new EventArgs() );
                UpdateVideoToolBar( this, new EventArgs() );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
                bRet = false;
                if ( FilGrMan != null ) FilGrMan = null;
                if ( MediaEventEx != null ) MediaEventEx = null;
                // throw;
            }

            return bRet;
        }

        private void CleanUp()
        {
            if ( FilGrMan != null ) FilGrMan.Stop();

            CurrentStatus = MediaStatus.Stopped;
            if ( FilGrMan != null )
            {
                //Setting FilGrMan.Ower=0 can cause clip to be opened in new Active Movie window.
                //Setting width and height to 0 prevents it from being shown.
                //WS_EX_TOOLWINDOW prevents Active Movie from showing in taskbar.
                FilGrMan.Width = 0;
                FilGrMan.Height = 0;
                FilGrMan.Caption = "";
                if ( FilGrMan.Visible != 0 ) FilGrMan.Visible = 0;
                if ( FilGrMan.Owner != 0 ) FilGrMan.Owner = 0;
                FilGrMan = null;
            }

            if ( MediaEventEx != null )
            {
                MediaEventEx.SetNotifyWindow( 0, 0, 0 );
                MediaEventEx = null;
            }
        }

        #endregion

        #region Event handler methods
        private void PictureAlbum_VideoChanged( object sender, EventArgs e )
        {
            this.CleanUp();
            if ( this.currentVideoIndex == -1 ) CurrentStatus = MediaStatus.None;
            else this.CurrentStatus = MediaStatus.Stopped;
        }

        void PictureAlbum_SelectedChanged( object sender, SelectedChangedEventArgs e )
        {
            try
            {
                if ( e.SelectedIndex == SelectedIndex )
                {
                    this.Invalidate();
                    return;
                }

                if ( this.ImageList != null )
                {
                    if ( ( e.SelectedIndex == -1 ) || ( e.SelectedIndex >= this.ImageList.Count ) )
                    {
                        selectedIndex = -1;
                        for ( int j = 0; j < this.ImageList.Count; j++ )
                            this.ImageList[j].Selected = false;
                    }
                    else
                    {
                        if ( ( SelectedIndex == -1 ) || ( SelectedIndex >= this.ImageList.Count ) || ( !this.ImageList[SelectedIndex].Selected ) )
                        {
                            // We should scan through all the images to make sure they're all deseleted.
                            for ( int j = 0; j < this.ImageList.Count; j++ )
                                this.ImageList[j].Selected = false;
                        }
                        else
                            this.ImageList[SelectedIndex].Selected = false;

                        //for ( int j = 0; j < this.ImageList.Count; j++ )
                        //this.ImageList[j].Selected = false;
                        selectedIndex = e.SelectedIndex;
                        this.ImageList[e.SelectedIndex].Selected = true;
                    }
                }
                else
                {
                    selectedIndex = -1;
                }

                this.Invalidate();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }

        }

        #endregion

        #endregion
    }
}

//public void SaveFirstFrame( string VideoFile, string BitmapFile )
//{
//    AviManager aviManager = new AviManager( VideoFile, true );
//    VideoStream stream = aviManager.GetVideoStream();
//    stream.GetFrameOpen();
//    stream.ExportBitmap( 1, BitmapFile );
//    Bitmap bmp = stream.GetBitmap( 1 );
//    stream.GetFrameClose();
//    aviManager.Close();
//    Functions.SaveThumbnailImage( bmp, BitmapFile + ".jpg", 10 );
//}

