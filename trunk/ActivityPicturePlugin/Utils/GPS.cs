/*
Copyright (C) 2009 Brendan Doherty

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

//Used in both Trails and Matrix plugin

using System.Collections.Generic;
using ZoneFiveSoftware.Common.Data.GPS;
using System;

namespace ActivityPicturePlugin
{
    public class GPS
    {
        public static IGPSPoint LocationToPoint(IGPSLocation location)
        {
            return new GPSPoint(location.LatitudeDegrees, location.LongitudeDegrees, 0);
        }

        public static IGPSLocation PointToLocation(IGPSPoint point)
        {
            return new GPSLocation(point.LatitudeDegrees, point.LongitudeDegrees);
        }

#if !ST_2_1
        public static IGPSBounds GetBounds(IList<IList<IGPSPoint>> trks)
        {
            GPSBounds area = null;
            foreach (IList<IGPSPoint> trk in trks)
            {
                GPSBounds area2 = GPSBounds.FromGPSPoints(trk);
                if (area2 != null)
                {
                    if (area == null)
                    {
                        area = area2;
                    }
                    else
                    {
                        area = (GPSBounds)area.Union(area2);
                    }
                }
            }
            return area;
        }
#endif
        public static string GpsString(IGPSLocation loc)
        {
            if (loc.LatitudeDegrees != 0 || loc.LongitudeDegrees != 0)
            {
                return GPSLocation.ToString(loc, GPS.GpsFormat());
            }
            return "";
        }

        public static string GpsFormat()
        {
            string GPSString = "";
            switch (Plugin.GetApplication().SystemPreferences.GPSLocationUnits)
            {
                case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.MinutesSeconds:
                    GPSString = "m0";
                    break;
                case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.Decimal3:
                    GPSString = "+3";
                    break;
                case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.Decimal4:
                    GPSString = "+4";
                    break;
                case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.Minutes:
                    GPSString = "m0"; //TODO: incomplete, must fix format string
                    break;
                default:
                    GPSString = ""; //Same as "+4" 
                    break;
            }

            return GPSString;
        }
        public static int Compare(IGPSPoint x, IGPSPoint y)
        {
            if (x.Equals(y))
            {
                return 0;
            }
            if (x.LatitudeDegrees > y.LatitudeDegrees ||
                x.LatitudeDegrees == y.LatitudeDegrees &&
                (x.LongitudeDegrees > y.LongitudeDegrees ||
                x.LongitudeDegrees == y.LongitudeDegrees &&
                x.ElevationMeters > y.ElevationMeters))
            {
                return 1;
            }
            return -1;
        }
 
        //old implementation, to be 
        //double precision, to not (seemingly) loose exif precision in presentations
        public class GpsLoc : IComparable
        {
            public GpsLoc(double lat, double lon)
            {
                this.lat = lat;
                this.lon = lon;
            }
            public double lat;
            public double lon;


            public override string ToString()
            {
                try
                {
                    if ((this.lat == 0) && (this.lon == 0)) return "";
                    else
                    {
                        string GPSString = "";
                        double degLat, minLat, secLat, degLon, minLon, secLon;
                        //TODO: Localization?
                        string latReference = this.lat > 0 ? "N" : "S";
                        string lonReference = this.lon > 0 ? "E" : "W";
                        switch (Plugin.GetApplication().SystemPreferences.GPSLocationUnits)
                        {
                            case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.MinutesSeconds:

                                degLat = Math.Abs(Math.Truncate(this.lat));
                                minLat = Math.Truncate((Math.Abs(this.lat) - degLat) * 60);
                                secLat = (((Math.Abs(this.lat) - degLat) * 60) - minLat) * 60;

                                degLon = Math.Abs(Math.Truncate(this.lon));
                                minLon = Math.Truncate((Math.Abs(this.lon) - degLon) * 60);
                                secLon = (((Math.Abs(this.lon) - degLon) * 60) - minLon) * 60;

                                GPSString = degLat.ToString() + "° " + minLat.ToString() + "' " + secLat.ToString("00")
                                    + (char)34 + " " + latReference + Environment.NewLine +
                                    degLon.ToString() + "° " + minLon.ToString() + "' " + secLon.ToString("00")
                                + (char)34 + " " + lonReference;
                                break;
                            case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.Decimal3:
                                GPSString = this.lat.ToString("0.000") + Environment.NewLine +
                                   this.lon.ToString("0.000");
                                break;
                            case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.Decimal4:
                                GPSString = this.lat.ToString("0.0000") + Environment.NewLine +
                                   this.lon.ToString("0.0000");
                                break;
                            case ZoneFiveSoftware.Common.Data.GPS.GPSLocation.Units.Minutes:
                                degLat = Math.Truncate(Math.Abs(this.lat));
                                minLat = Math.Truncate((Math.Abs(this.lat) - degLat) * 60);
                                degLon = Math.Abs(Math.Truncate(this.lon));
                                minLon = Math.Truncate((Math.Abs(this.lon) - degLon) * 60);
                                GPSString = degLat.ToString() + "° " + minLat.ToString() + "' " + latReference
                                    + Environment.NewLine + degLon.ToString() + "° " +
                                    minLon.ToString() + "' " + lonReference;
                                break;
                            default:
                                GPSString = this.lat.ToString("0.0000") + Environment.NewLine +
                                    this.lon.ToString("0.0000");
                                break;
                        }

                        return GPSString;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Assert(false, ex.Message);
                    //throw;
                    return "";
                }
            }

            public int CompareTo(object obj)
            {
                if (!(obj is GpsLoc))
                {
                    return 1;
                }
                GpsLoc oth = obj as GpsLoc;
                if (this.lat > oth.lat) { return 1; }
                if (this.lat < oth.lat) { return -1; }
                if (this.lon > oth.lon) { return 1; }
                if (this.lon < oth.lon) { return -1; }
                return 0;
            }
        }
    }
}