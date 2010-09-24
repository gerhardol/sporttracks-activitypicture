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
using QuartzTypeLib;
using ZoneFiveSoftware.Common.Data.GPS;

namespace ActivityPicturePlugin.Helper
{
    public partial class PictureAlbum : UserControl
    {
        public PictureAlbum()
        {
            InitializeComponent();
            EnableDoubleBuffering();
            this.ActivityChanged += new ActivityChangedEventHandler(PictureAlbum_ActivityChanged);
            this.ImageRectangles[0] = new Rectangle();
            this.albumToolTipTimer.Tick += new System.EventHandler(ToolTipTimer_Tick);
            this.albumToolTipTimer.Interval = 200;
        }
        #region Overrides
        protected override void WndProc(ref Message m)
        {
            try
            {
                switch (m.Msg)
                {
                    case WM_VSCROLL:
                        //return;
                        break;
                    case WM_GRAPHNOTIFY:
                        int lEventCode;
                        int lParam1, lParam2;
                        while (true)
                        {
                            MediaEventEx.GetEvent(out lEventCode,
                                out lParam1,
                                out lParam2,
                                0);
                            MediaEventEx.FreeEventParams(lEventCode, lParam1, lParam2);
                            if (lEventCode == EC_COMPLETE) //video is a end position
                            {
                                FilGrMan.Stop();
                                FilGrMan.CurrentPosition = 0;
                                CurrentStatus = MediaStatus.Stopped;
                                UpdateVideoToolBar(this, new EventArgs());
                            }
                        }
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                //break;
            }
            base.WndProc(ref m);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            this.PaintAlbumView(false);
            PaintEventArgs pe = new PaintEventArgs(this.CreateGraphics(), this.DisplayRectangle);
            base.OnPaint(pe);
        }
        #endregion
        #region EventHandlers
        public delegate void ActivityChangedEventHandler(System.Object sender, System.EventArgs e);
        public delegate void ZoomChangeEventHandler(System.Object sender, int increment);
        public delegate void UpdateVideoToolBarEventHandler(System.Object sender, System.EventArgs e);
        public delegate void ShowVideoOptionsEventHandler(System.Object sender, System.EventArgs e);
        #endregion
        #region Events
        public event ActivityChangedEventHandler ActivityChanged;
        public event ZoomChangeEventHandler ZoomChange;
        public event UpdateVideoToolBarEventHandler UpdateVideoToolBar;
        public event ShowVideoOptionsEventHandler ShowVideoOptions;
        #endregion
        #region public members
        public List<ImageData> ImageList
        {
            get { return imagelist; }
            set { imagelist = value; }
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
        //public ImageSortMode SortMode
        //{
        //    get { return sortmode; }
        //    set { sortmode = value; }
        //}
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
                int NewHeight = (int)(30 + 5 * Zoom);
                int NewWidth = 120;
                int offset = 0;
                int left = 0, top = 0, row = 0;

                if (this.ImageList.Count > 0)
                {
                    ImageRectangles = new Rectangle[this.ImageList.Count];
                    for (int i = 0; i <= this.ImageList.Count - 1; i++)
                    {
                        NewWidth = (int)(ImageList[i].Ratio * NewHeight);
                        if (this.panel1.Width < NewWidth)
                        {
                            ZoomChange(this, -1);
                            return new Rectangle(0, 0, 0, 0);
                        }

                        //calculate new Position
                        if ((left + NewWidth) > this.panel1.Width)
                        {
                            //start new row
                            left = 0;
                            row += 1;
                        }

                        top = offset + row * NewHeight;
                        ImageRectangles[i] = new Rectangle(left, top, NewWidth, NewHeight);

                        //left for the next image
                        left += NewWidth;
                    }
                    return new Rectangle(0, 0, this.panel1.Width, top + NewHeight);
                }
                else return new Rectangle(0, 0, 0, 0);
            }
            catch (Exception)
            {
                return new Rectangle(0, 0, 0, 0);
            }


        }
        private void DrawImages(bool CompleteRedraw)
        {
            try
            {
                //int ZoomLevel = this.parentctl.GetZoom();
                //int NewHeight = (int)(30 + 5 * ZoomLevel);
                //int NewWidth = 120; //reference width
                //int offset = 0;
                //int left = 0, top = 0, row = 0;
                Image img = null;
                //List<ImageData> imgList = this.ImageList;//.parentctl.Images;
                if (this.ImageList.Count > 0)
                {
                    //ImageRectangles = new Rectangle[imgList.Count];
                    Graphics g = this.panel1.CreateGraphics();
                    for (int i = 0; i <= this.ImageList.Count - 1; i++)
                    {
                        if ((this.ImageList[i].ReferenceIDPath != null))
                        {
                            if (NoThumbNails) img = new Bitmap(this.ImageList[i].PhotoSource);
                            else img = this.ImageList[i].EW.GetBitmap();
                            //    NewWidth = (int)((double)(img.Width) / (double)(img.Height) * NewHeight);
                        }

                        //draw the images
                        Pen p = new Pen(Brushes.Black, 2);
                        g.DrawImage(img, ImageRectangles[i]);
                        if (this.ImageList[i].Selected) g.DrawRectangle(new Pen(Brushes.Blue, 2), ImageRectangles[i]);
                        else g.DrawRectangle(p, ImageRectangles[i]);
                    }

                    // test for solution without invalidating
                    Region r = new Region();
                    for (int i = 0; i < ImageRectangles.Length; i++)
                    {
                        r.Xor(ImageRectangles[i]);
                    }
                    //Rectangle rect = new Rectangle(new Point(0, 0), this.panel1.Size);
                    ////r.Complement(rect);
                    g.FillRegion(Brushes.White, r);

                    if ((this.FilGrMan != null) & (currentVideoIndex != -1))
                    {
                        FilGrMan.SetWindowPosition(ImageRectangles[currentVideoIndex].Left,
                            ImageRectangles[currentVideoIndex].Top,
                            ImageRectangles[currentVideoIndex].Width,
                            ImageRectangles[currentVideoIndex].Height);
                    }
                }
            }
            catch (Exception)
            {
                // throw;
            }
        }
        private int GetIndexOfCurrentImage(Point p)
        {
            try
            {
                if (ImageRectangles != null && this.ImageList.Count != 0)
                {
                    for (int i = 0; i < ImageRectangles.Length; i++)
                    {
                        if (ImageRectangles[i] != null)
                        {
                            if (new Region(ImageRectangles[i]).IsVisible(p))
                            {
                                    return i;
                            }
                        }
                    }
                }
                return -1;
            }
            catch (Exception)
            {
                return -1;
            }

        }
        private void EnableDoubleBuffering()
        {
            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.DoubleBuffer |
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint,
               true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }
        #endregion
        #region public methods
        public void PaintAlbumView(bool CompleteRedraw)
        {
            Rectangle rect = this.GetImagePositions();
            if (rect.Height > 0)
            {
                if (this.panel1.Height != rect.Height) this.panel1.Height = rect.Height;
                this.DrawImages(CompleteRedraw);
            }
        }
        public void ActivityChanging()
        {
            this.ActivityChanged(this, new EventArgs());
        }
        public void ThemeChanged(ZoneFiveSoftware.Common.Visuals.ITheme visualTheme)
        {
            this.BackColor = visualTheme.Control;
            this.ForeColor = visualTheme.ControlText;
        }
        #endregion
        #region Event handler methods
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //PaintAlbumView(true);
        }

        //ToolTip support - hints from omb

        // private member variables of the Control - initialization omitted
        Timer albumToolTipTimer = new Timer();
        bool albumTooltipDisabled = false; // is set to true, whenever a tooltip would be annoying, e.g. while a context menu is shown
        int IndexOfLastImage = -1;

        private void ToolTipTimer_Tick(object sender, EventArgs e)
        {
            albumToolTipTimer.Stop();
            if (IndexOfLastImage >= 0 &&
                    this.ImageList[IndexOfLastImage].Type == ImageData.DataTypes.Image)
            {
                int i = IndexOfLastImage;
                string tooltip = "";
                tooltip = this.ImageList[i].PhotoSource;
                DateTime dt = new DateTime(1950, 1, 1);
                if (dt < this.ImageList[i].EW.DateTimeOriginal) tooltip += Environment.NewLine + this.ImageList[i].DateTimeOriginal;
                if (this.ImageList[i].EW.GPSLatitude != 0) tooltip += Environment.NewLine + this.ImageList[i].ExifGPS;
                if (this.ImageList[i].Title != "") tooltip += Environment.NewLine + this.ImageList[i].Title;
                this.toolTip1.SetToolTip(this.panel1, tooltip);
            }
            else
            {
                toolTip1.Hide(panel1);
            }
        }
        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            this.panel1.Focus();
        }
        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            albumToolTipTimer.Stop();
            toolTip1.Hide(panel1);
            IndexOfLastImage = -1;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            int i = GetIndexOfCurrentImage(e.Location);
            if (i == IndexOfLastImage)
                return;
            else
                toolTip1.Hide(panel1);

            IndexOfLastImage = i;
            albumToolTipTimer.Stop();
            if (i>=0)
                albumToolTipTimer.Start();
        }
        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int i = this.GetIndexOfCurrentImage(e.Location);
            if (i >= 0)
            {
                string s = this.ImageList[i].PhotoSource;
                if (this.ImageList[i].Type == ImageData.DataTypes.Image)
                {
                    Functions.OpenImage(s, this.ImageList[i].ReferenceID);
                }
                else if (this.ImageList[i].Type == ImageData.DataTypes.Video)
                {
                    //Note: The click does not "get through"
                    Functions.OpenVideoInExternalWindow(s);
                }
            }
        }
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int i = this.GetIndexOfCurrentImage(e.Location);
            if (i >= 0)
            {
                string s = this.ImageList[i].PhotoSource;
                if (this.ImageList[i].Type == ImageData.DataTypes.Video)
                {
                    this.OpenVideo(this.ImageList[i].PhotoSource, i);
                }
            }
        }
        private void panel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                //TODO: update size after update, not working now
                if (e.KeyCode == Keys.Up)
                {
                    if (this.Zoom < 99) this.ZoomChange(this, +2);
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.Zoom > 1) this.ZoomChange(this, -2);
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }
        private void PictureAlbum_Load(object sender, EventArgs e)
        {
            this.panel1.Width = this.Width - 6;
            this.panel1.Height = this.Height - 6;
            this.panel1.Left = 3;
            this.panel1.Top = 3;
        }
        private void PictureAlbum_ActivityChanged(object sender, EventArgs e)
        {
            if (FilGrMan != null)
            {
                CleanUp();
            }

        }
        #endregion
        #region Video
        #region Public
        internal enum MediaStatus { None, Stopped, Paused, Running };
        internal MediaStatus CurrentStatus = MediaStatus.None;
        internal void SetVideoPosition(int pos)
        {
            FilGrMan.CurrentPosition = FilGrMan.Duration * (double)(pos) / 1000;
        }
        internal double GetVideoPosition()
        //returns the position in % of the overall length
        {
            try
            {
                if (this.FilGrMan != null)
                {
                    double pos = this.FilGrMan.CurrentPosition / this.FilGrMan.Duration;
                    if ((pos >= 0) & (pos <= 1)) return pos;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        internal void PauseVideo()
        {
            FilGrMan.Pause();
            CurrentStatus = MediaStatus.Paused;
        }
        internal void StopVideo()
        {
            FilGrMan.Stop();
            FilGrMan.CurrentPosition = 0;
            CurrentStatus = MediaStatus.Stopped;
        }
        internal void PlayVideo()
        {
            FilGrMan.Run();
            CurrentStatus = MediaStatus.Running;
        }
        #endregion
        #region Private
        private const int WM_APP = 0x8000;
        private const int WM_VSCROLL = 277;
        private const int WM_GRAPHNOTIFY = WM_APP + 1;
        private const int EC_COMPLETE = 0x01;
        private const int WS_CHILD = 0x40000000;
        private const int WS_CLIPCHILDREN = 0x2000000;
        private FilgraphManagerClass FilGrMan = null;
        private IMediaEventEx MediaEventEx = null;
        private int currentVideoIndex = -1;
        private void OpenVideo(string file, int imageNumber)
        {
            try
            {
                //int ZoomLevel = this.parentctl.GetZoom();
                int height = (int)(30 + 3 * Zoom);


                CleanUp();
                FilGrMan = new FilgraphManagerClass();
                FilGrMan.RenderFile(file);

                int width = (int)((double)(FilGrMan.DestinationWidth) / (double)(FilGrMan.DestinationHeight) * height);


                try
                {
                    FilGrMan.Owner = (int)panel1.Handle;
                    FilGrMan.WindowStyle = WS_CHILD | WS_CLIPCHILDREN;
                    FilGrMan.SetWindowPosition(ImageRectangles[imageNumber].Left,
                        ImageRectangles[imageNumber].Top,
                        ImageRectangles[imageNumber].Width,
                        ImageRectangles[imageNumber].Height);
                }
                catch (Exception)
                {
                    FilGrMan.Visible = 0;
                    FilGrMan.Owner = 0;
                }

                MediaEventEx = FilGrMan as IMediaEventEx;
                MediaEventEx.SetNotifyWindow((int)this.Handle, WM_GRAPHNOTIFY, 0);

                FilGrMan.Run();

                currentVideoIndex = imageNumber;
                CurrentStatus = MediaStatus.Running;
                //UpdateStatusBar();
                ShowVideoOptions(this, new EventArgs());
                UpdateVideoToolBar(this, new EventArgs());
            }
            catch (Exception)
            {
                // throw;
            }
        }
        private void CleanUp()
        {
            if (FilGrMan != null) FilGrMan.Stop();

            CurrentStatus = MediaStatus.Stopped;
            currentVideoIndex = -1;
            if (MediaEventEx != null) MediaEventEx.SetNotifyWindow(0, 0, 0);

            if (FilGrMan != null)
            {
                if (FilGrMan.Visible != 0) FilGrMan.Visible = 0;
                if (FilGrMan.Owner != 0) FilGrMan.Owner = 0;
                FilGrMan = null;
            }
            if (MediaEventEx != null) MediaEventEx = null;
        }
        #endregion
        #endregion
    }
}

//public void SaveFirstFrame(string VideoFile, string BitmapFile)
//{
//    AviManager aviManager = new AviManager(VideoFile, true);
//    VideoStream stream = aviManager.GetVideoStream();
//    stream.GetFrameOpen();
//    stream.ExportBitmap(1, BitmapFile);
//    Bitmap bmp = stream.GetBitmap(1);
//    stream.GetFrameClose();
//    aviManager.Close();
//    SaveThumbnailImage(bmp, BitmapFile + ".jpg");
//}