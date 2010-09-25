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
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Visuals.Mapping;
using System.Collections.Generic;
using Microsoft.Win32;
using ActivityPicturePlugin.Helper;

namespace ActivityPicturePlugin.UI.MapLayers
{
    class ExtendRouteControlLayerProviders : IExtendRouteControlLayerProviders
    {
        public IList<IRouteControlLayerProvider> RouteControlLayerProviders
        {
            get
            {
                return new IRouteControlLayerProvider[] { PicturesProvider.Instance };
            }
        }
    }
    class PicturesProvider : IRouteControlLayerProvider
    {
        private static PicturesProvider m_instance = null;
        public static PicturesProvider Instance
        {
            get
            {
                if (PicturesProvider.m_instance == null)
                {
                    PicturesProvider.m_instance = new PicturesProvider();
                }
                return PicturesProvider.m_instance;
            }
        }
        private IRouteControlLayer m_layer = null;
        public IRouteControlLayer RouteControlLayer
        {
            get { return m_layer; }
        }

        public IRouteControlLayer CreateControlLayer(IRouteControl control)
        {
            if (m_layer == null)
            {
                m_layer = new PicturesLayer(this,control);
            }
            return m_layer;
        }

        public Guid Id
        {
            get { return GUIDs.PicturesControlLayerProvider; }
        }

        public string Name
        {
            get { return Resources.Resources.PicturesControlLayerProvider; }
        }
    }

    class PicturesLayer : RouteControlLayerBase, IRouteControlLayer
    {
        public PicturesLayer(IRouteControlLayerProvider provider, IRouteControl control)
            : base(provider, control, 1)
        {
            //PluginMain.GetApplication().SystemPreferences.PropertyChanged += new PropertyChangedEventHandler(SystemPreferences_PropertyChanged);
            //listener = new RouteItemsDataChangeListener(control);
            //listener.PropertyChanged += new PropertyChangedEventHandler(OnRouteItemsPropertyChanged);
        }

        public IList<ImageData> Pictures
        {
            get
            {
                return m_Pictures;
            }
            set
            {
                bool changed = false;
                if (!value.Equals(m_Pictures)) { changed = true; }
                m_Pictures = value;
                if (changed) { RefreshOverlays(true); }
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
                if (value.Count > 0 && _showPage)
                {
                    float north = -180;
                    float south = +180;
                    float east = -90;
                    float west = 90;
                    bool isValid = false;
                    foreach (ImageData g in value)
                    {
                        if (g.GpsLocation.LatitudeDegrees != 0 &&
                            g.GpsLocation.LongitudeDegrees != 0)
                        {
                            north = Math.Max(north, g.GpsLocation.LatitudeDegrees);
                            south = Math.Min(south, g.GpsLocation.LatitudeDegrees);
                            east = Math.Max(east, g.GpsLocation.LongitudeDegrees);
                            west = Math.Min(west, g.GpsLocation.LongitudeDegrees);
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
                        GPSBounds area = new GPSBounds(new GPSLocation(north, west), new GPSLocation(south, east));
                        this.MapControl.SetLocation(area.Center,
                          this.MapControl.ComputeZoomToFit(area));
                    }
                }
                    m_SelectedPictures = value;
            }
        }

        public void Refresh()
        {
            //Should not be necessary in ST3, updated when needed
            RefreshOverlays(); 
        }
        public bool ShowPage
        {
            get { return _showPage; }
            set
            {
                bool changed = (value != _showPage);
                _showPage = value;
                if (changed)
                {
                    RefreshOverlays(true);
                }
            }
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
            if (RouteControl.Visible && routeSettingsChanged)
            {
                ClearOverlays();
                routeSettingsChanged = false;
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

            if (!_showPage) return;

            IGPSBounds windowBounds = MapControlBounds;

            IList<ImageData> visibleLocations = new List<ImageData>();
            foreach (ImageData point in m_Pictures)
            {
                if (windowBounds.Contains(point.GpsLocation))
                {
                    visibleLocations.Add(point);
                }
            }
            if (0 == visibleLocations.Count) return;

            IDictionary<IGPSPoint, IMapOverlay> newPointOverlays = new Dictionary<IGPSPoint, IMapOverlay>();
            IList<IMapOverlay> addedOverlays = new List<IMapOverlay>();

            foreach (ImageData location in visibleLocations)
            {
                if ((!m_scalingChanged) && pointOverlays.ContainsKey(location.GpsPoint))
                {
                    //No need to refresh this point
                    newPointOverlays.Add(location.GpsPoint, pointOverlays[location.GpsPoint]);
                    //((MapMarker)pointOverlays[location.GpsPoint]).DoubleClick -= new MouseEventHandler(pointOverlay_DoubleClick);
                    pointOverlays.Remove(location.GpsPoint);
                }
                else
                {
                    
                        Size iconSize = new Size(15, 15);
                        string fileURL = "file://"+location.PhotoSource;
                        m_icon = new MapIcon(fileURL, iconSize);

                        MapMarker pointOverlay = new MapMarker(location.GpsPoint, m_icon, false);
                        //pointOverlay.DoubleClick += new MouseEventHandler(pointOverlay_DoubleClick);
                        newPointOverlays.Add(location.GpsPoint, pointOverlay);
                    addedOverlays.Add(pointOverlay);
                    m_scalingChanged = false;
                }
            }

            ClearOverlays();
            MapControl.AddOverlays(addedOverlays);
            pointOverlays = newPointOverlays;
        }

        //MapMarkers must be converted to ImageData (or ImageData implement MapMarker)
        //void pointOverlay_DoubleClick(object sender, MouseEventArgs e)
        //{
        //    if (sender is MapMarker)
        //    {
        //        MapMarker im = sender as MapMarker;
        //        Helper.Functions.OpenExternal(im);
        //    }
        //    //throw new NotImplementedException();
        //}

        private void ClearOverlays()
        {
            //((MapMarker)pointOverlays[location.GpsPoint]).DoubleClick -= new MouseEventHandler(pointOverlay_DoubleClick);
            MapControl.RemoveOverlays(pointOverlays.Values);
            pointOverlays.Clear();
        }

        private bool m_scalingChanged = false;
        MapIcon m_icon = null;
        private bool routeSettingsChanged = false;
        private IDictionary<IGPSPoint, IMapOverlay> pointOverlays = new Dictionary<IGPSPoint, IMapOverlay>();

        //private RouteItemsDataChangeListener listener;

        private IList<ImageData> m_Pictures = new List<ImageData>();
        private IList<ImageData> m_SelectedPictures = new List<ImageData>();
        private static bool _showPage;
    }
}
#endif