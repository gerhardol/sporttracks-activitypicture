/*
Copyright (C) 2010 Gerhard Olsson

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

//ST_3_0: Display Pictures, not included for ST_2_1 
//Adopted from Trails plugin

#if !ST_2_1
using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Visuals.Mapping;
using System.Collections.Generic;
using Microsoft.Win32;
using ActivityPicturePlugin.Helper;

namespace ActivityPicturePlugin.UI.MapLayers
{
    class PicturesLayer : RouteControlLayerBase, IRouteControlLayer
    {
        class ImageMapMarker : MapMarker
        {
            public ImageMapMarker(ImageData image, float size, int zindex)
                : base(new GPSPoint(image.GpsPoint.LatitudeDegrees, image.GpsPoint.LongitudeDegrees, image.GpsPoint.ElevationMeters), 
                new MapIcon("file://" + image.ThumbnailPath, new Size((int)size, (int)(size/image.Ratio))),
                true, zindex)
            {
                this.image = image;
            }
            public ImageData image;
        }

        private DateTime m_creationTime = DateTime.Now;
        public PicturesLayer(IRouteControlLayerProvider provider, IRouteControl control)
            : base(provider, control, 1, false)
        {
            Guid currentView = Plugin.GetApplication().ActiveView.Id;
            if (m_layers.ContainsKey(currentView))
            {
                m_layers[currentView].m_extraMapLayer = this;
            }
            else
            {
                m_layers[currentView] = this;
            }
            this.clickWaitTimer.Tick += new System.EventHandler(clickWaitTimer_Tick);
            this.clickWaitTimer.Interval = System.Windows.Forms.SystemInformation.DoubleClickTime;
        }

        //Note: There is an assumption of the relation between view and route control/layer
        //See the following: http://www.zonefivesoftware.com/sporttracks/forums/viewtopic.php?t=9465
        public static PicturesLayer Instance(IView view)
        {
            PicturesLayer result = null;
            if (view != null && m_layers != null && m_layers.ContainsKey(view.Id))
            {
                result = m_layers[view.Id];
            }
            else if (m_layers.Count > 0)
            {
                foreach (PicturesLayer l in m_layers.Values)
                {
                    //Just any layer - the first should be the best
                    result = l;
                    break;
                }
            }
            return result;
        }

        public IList<ImageData> Pictures
        {
            //get
            //{
            //    return m_Pictures;
            //}
            set
            {
                foreach (ImageMapMarker imm in this.m_Pictures)
                {
                    imm.Click -= pointOverlay_Click;
                    imm.DoubleClick -= pointOverlay_DoubleClick;
                }
                this.m_Pictures = new List<ImageMapMarker>();
                foreach (ImageData im in value)
                {
                    if (im.ThumbnailPath != null)
                    {
                        ImageMapMarker imm2 = new ImageMapMarker(im, this.m_pictureSize, 0);
                        this.m_Pictures.Add(imm2);
                        imm2.Click += pointOverlay_Click;
                        imm2.DoubleClick += pointOverlay_DoubleClick;
                    }
                }
                RefreshOverlays(true);
            }
        }
        public IList<ImageData> SelectedPictures
        {
            //get
            //{
            //    return m_SelectedPictures;
            //}
            set
            {
                //Set selected area to include selected points, including radius and some more
                if (value.Count > 0 && m_showPage)
                {
                    float north = -180;
                    float south = +180;
                    float east = -90;
                    float west = 90;
                    bool isValid = false;
                    foreach (ImageData g in value)
                    {
                        if (g.GpsPoint.LatitudeDegrees != 0 &&
                            g.GpsPoint.LongitudeDegrees != 0)
                        {
                            north = Math.Max(north, g.GpsPoint.LatitudeDegrees);
                            south = Math.Min(south, g.GpsPoint.LatitudeDegrees);
                            east = Math.Max(east, g.GpsPoint.LongitudeDegrees);
                            west = Math.Min(west, g.GpsPoint.LongitudeDegrees);
                            isValid = true;
                        }
                    }
                    if (isValid)
                    {
                        //Adopted from Trails
                        //Get approx degrees for the radius offset
                        //The magic numbers are size of a degree at the equator
                        //latitude increases about 1% at the poles
                        //longitude is up to 40% longer than linear extension - compensate 20%
                        const int m_highlightRadius = 100;
                        float lat = 2 * m_highlightRadius / 110574 * 1.005F;
                        float lng = 2 * m_highlightRadius / 111320 * Math.Abs(south) / 90 * 1.2F;
                        north += lat;
                        south -= lat;
                        east += lng;
                        west -= lng;
                        IGPSBounds area = new GPSBounds(new GPSLocation(north, west), new GPSLocation(south, east));
                        this.SetLocation(area);

                        for (int i = 0; i < this.m_Pictures.Count; i++)
                        {
                            ImageMapMarker imp = this.m_Pictures[i];
                            int zindex = imp.ZIndex;
                            bool found = false;
                            foreach (ImageData g in value)
                            {
                                if (g == imp.image)
                                {
                                    found = true;
                                }
                            }
                            if (found)
                            {
                                zindex = 1;
                            }
                            else if (zindex > 0)
                            {
                                //Do not change "demoted" pictures
                                zindex = 0;
                            }
                            imp.Click -= pointOverlay_Click;
                            imp.DoubleClick -= pointOverlay_DoubleClick;
                            imp = new ImageMapMarker(imp.image, this.m_pictureSize, zindex);
                            this.m_Pictures[i] = imp;
                            imp.Click += pointOverlay_Click;
                            imp.DoubleClick += pointOverlay_DoubleClick;
                        }
                        this.RefreshOverlays(true);
                    }
                }
                //m_SelectedPictures = value;
            }
        }

        void pointOverlay_Click(object sender, MouseEventArgs e)
        {
            if (sender is ImageMapMarker)
            {
                clickWaitTimer.Start();
                ImageMapMarker im = sender as ImageMapMarker;
                lastClickedImage = im;
            }
        }

        private void clickWaitTimer_Tick(object sender, EventArgs e)
        {
            clickWaitTimer.Stop();
            if (lastClickedImage != null)
            {
                IList<ImageMapMarker> pictures = new List<ImageMapMarker>();
                IList<ImageMapMarker> visible = new List<ImageMapMarker>();
                bool visible0z = false;

                foreach (ImageMapMarker imm in this.m_Pictures)
                {
                    if (this.MapControl.MapBounds.Contains(imm.Location))
                    {
                        visible.Add(imm);
                        if (imm.ZIndex == 0 && lastClickedImage != imm)
                        {
                            visible0z = true;
                        }
                    }
                }

                foreach (ImageMapMarker imm in this.m_Pictures)
                {
                    int zindex = imm.ZIndex;
                    if (imm == lastClickedImage)
                    {
                        //send to back
                        zindex = 1 - visible.Count;
                    }
                    else if (visible.Contains(imm))
                    {
                        if (zindex < -1 ||
                            //Let all 0 prio be visible before promoting
                            zindex == -1 && !visible0z)
                        {
                            zindex++;
                        }
                    }

                    imm.Click -= pointOverlay_Click;
                    imm.DoubleClick -= pointOverlay_DoubleClick;
                    ImageMapMarker imm2 = new ImageMapMarker(imm.image, this.m_pictureSize, zindex);
                    pictures.Add(imm2);
                    imm2.Click += pointOverlay_Click;
                    imm2.DoubleClick += pointOverlay_DoubleClick;
                }
                this.m_Pictures = pictures;
                this.RefreshOverlays();
            }
        }

        void pointOverlay_DoubleClick(object sender, MouseEventArgs e)
        {
            // Stop the timer from ticking.
            clickWaitTimer.Stop();
            if (sender is ImageMapMarker)
            {
                //ImageMapMarker im = sender as ImageMapMarker;
                if (lastClickedImage != null)
                {
                    Helper.Functions.OpenExternal(lastClickedImage.image);
                    //TODO: rollback active image
                }
                lastClickedImage = null;
            }
        }

        public new void SetLocation(IGPSBounds area)
        {
            if (m_showPage)
            {
                base.SetLocation(area);
                if (m_extraMapLayer != null)
                {
                    m_extraMapLayer.SetLocation(area);
                }
            }
        }

        //Zoom to "relevant" contents (normally done when activities are updated)
        //public void DoZoom()
        //{
        //    this.DoZoom(this.RelevantArea());
        //}

        public new void DoZoom(IGPSBounds area)
        {
            //Note no m_showPage here, can be done when creating tracks
            base.DoZoom(area);
            if (m_extraMapLayer != null)
            {
                m_extraMapLayer.DoZoom(area);
            }
        }

        public float PictureSize
        {
            set
            {
                if ((int)m_pictureSize != (int)value)
                {
                    m_scalingChanged = true;
                }
                //Keep scaling here for now
                m_pictureSize = 3 * value;
            }
        }
        public void Refresh()
        {
            //Should not be necessary in ST3, updated when needed
            RefreshOverlays();
        }
        public bool HidePage()
        {
            m_showPage = false;
            RefreshOverlays(true);
            return true;
        }
        public void ShowPage(string bookmark)
        {
            m_showPage = true;
            RefreshOverlays(true);
        }

        /*************************************************************/
        protected override void OnMapControlZoomChanged(object sender, EventArgs e)
        {
            m_scalingChanged = true;
            RefreshOverlays();
        }

        protected override void OnMapControlCenterMoveEnd(object sender, EventArgs e)
        {
            RefreshOverlays();
        }

        protected override void OnRouteControlResize(object sender, EventArgs e)
        {
            RefreshOverlays();
        }

        protected override void OnRouteControlVisibleChanged(object sender, EventArgs e)
        {
            if (RouteControl.Visible && m_routeSettingsChanged)
            {
                ClearOverlays();
                m_routeSettingsChanged = false;
                RefreshOverlays();
            }
        }

        protected override void OnRouteControlMapControlChanged(object sender, EventArgs e)
        {
            RefreshOverlays();
        }

        protected override void OnRouteControlItemsChanged(object sender, EventArgs e)
        {
            RefreshOverlays();
        }

        //private void SystemPreferences_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "RouteSettings.ShowGPSPoints" ||
        //        e.PropertyName == "RouteSettings.MarkerShape" ||
        //        e.PropertyName == "RouteSettings.MarkerSize" ||
        //        e.PropertyName == "RouteSettings.MarkerColor")
        //    {
        //        if (RouteControl.Visible)
        //        {
        //            RefreshOverlays();
        //        }
        //        else
        //        {
        //            routeSettingsChanged = true;
        //        }
        //    }
        //}

        //private void OnRouteItemsPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == Activity.PropertyName.GPSRoute ||
        //        e.PropertyName == Route.PropertyName.GPSRoute ||
        //        e.PropertyName == Activity.PropertyName.Category )
        //    {
        //        ClearCachedLocations();
        //        RefreshOverlays();
        //    }
        //}

        private void RefreshOverlays()
        {
            RefreshOverlays(false);
        }

        private void RefreshOverlays(bool clear)
        {
            if (clear || MapControlChanged)
            {
                ClearOverlays();
                ResetMapControl();
            }

            if (!m_showPage) return;

            IGPSBounds windowBounds = MapControlBounds;

            IList<ImageMapMarker> visibleLocations = new List<ImageMapMarker>();
            foreach (ImageMapMarker point in m_Pictures)
            {
                //Use the image gpspoint, the MapMarker point should be modified to stack
                if (windowBounds.Contains(point.image.GpsPoint))
                {
                    visibleLocations.Add(point);
                }
            }
            if (0 == visibleLocations.Count) return;

            if (this.m_scalingChanged)
            {
                this.m_scalingChanged = false;
                IList<ImageMapMarker> pictures = new List<ImageMapMarker>();
                foreach (ImageMapMarker imm in this.m_Pictures)
                {
                    imm.Click -= pointOverlay_Click;
                    imm.DoubleClick -= pointOverlay_DoubleClick;
                    ImageMapMarker imm2 = new ImageMapMarker(imm.image, this.m_pictureSize, imm.ZIndex);
                    pictures.Add(imm2);
                    imm2.Click += pointOverlay_Click;
                    imm2.DoubleClick += pointOverlay_DoubleClick;
                }
                this.m_Pictures = pictures;
            }

            IDictionary<IGPSPoint, IMapOverlay> newPointOverlays = new Dictionary<IGPSPoint, IMapOverlay>();
            IList<IMapOverlay> addedOverlays = new List<IMapOverlay>();

            foreach (ImageMapMarker imm in visibleLocations)
            {
                //Use image location (MapMarker should be adjusted)
                if (m_pointOverlays.ContainsKey(imm.image.GpsPoint))
                {
                    //No need to refresh this point
                    newPointOverlays.Add(imm.Location, m_pointOverlays[imm.Location]);
                }
                else
                {
                    //Add with MapMarker location
                    newPointOverlays.Add(imm.Location, imm);
                    addedOverlays.Add(imm);
                }
            }

            ClearOverlays();
            MapControl.AddOverlays(addedOverlays);
            m_pointOverlays = newPointOverlays;
            if (m_extraMapLayer != null)
            {
                try
                {
                    //Remove overlays are not working properly, the Map is not very usable
                    m_extraMapLayer.MapControl.AddOverlays(addedOverlays);
                }
                catch (Exception ex)
                {
                    //Exceptions ignored, statement added to remove warnings
                    System.Diagnostics.Debug.Assert(true, ex.Message);
                }
                m_extraMapLayer.m_pointOverlays = newPointOverlays;
            }
        }

        private void ClearOverlays()
        {
            MapControl.RemoveOverlays(m_pointOverlays.Values);
            m_pointOverlays.Clear();
            if (m_extraMapLayer != null)
            {
                m_extraMapLayer.ClearOverlays();
            }
        }

        private bool m_scalingChanged = false;
        //MapIcon m_icon = null;
        private bool m_routeSettingsChanged = false;
        private IDictionary<IGPSPoint, IMapOverlay> m_pointOverlays = new Dictionary<IGPSPoint, IMapOverlay>();

        //private RouteItemsDataChangeListener listener;

        private IList<ImageMapMarker> m_Pictures = new List<ImageMapMarker>();
        //private IList<ImageData> m_SelectedPictures = new List<PointMapMarker>();
        private float m_pictureSize;
        private static bool m_showPage;
        private static IDictionary<Guid, PicturesLayer> m_layers = new Dictionary<Guid, PicturesLayer>();
        private PicturesLayer m_extraMapLayer = null;
        private Timer clickWaitTimer = new Timer();
        private ImageMapMarker lastClickedImage = null; //instead of timer to supress click
    }
}
#endif