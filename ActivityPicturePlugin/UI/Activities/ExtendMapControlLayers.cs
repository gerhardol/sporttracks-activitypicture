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
using System.Text;

using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
#if ST_2_1
using ZoneFiveSoftware.Common.Visuals.Fitness.GPS;
#else
using ZoneFiveSoftware.Common.Visuals.Mapping;
#endif

#if ST_2_1
//TODO: MapMarkers are not implemented in neither ST2/ST3
namespace ActivityPicturePlugin.UI.Activities
{
    class ExtendMapControlLayers : IExtendMapControlLayers
    {
        #region IExtendMapControlLayers Members

        public IList<IMapControlLayer> MapLayers(IMapControl mapControl)
        {
                return new IMapControlLayer[]
                {
                    //new MyMapControlLayer()
                };
        }

        #endregion
    }

    class MyMapControlLayer : IMapControlLayer
    {
        #region IMapControlLayer Members

        public void Draw(IMapDrawContext drawContext)
        {
            //throw new NotImplementedException();
        }

        public ICollection<IMapControlObject> HitTest(System.Drawing.Rectangle rectClient, IMapDrawContext drawContext)
        {
            return null;
            //throw new NotImplementedException();
        }

        public IMapControlObject HitTest(System.Drawing.Point ptClient, bool bSelect, IMapDrawContext drawContext, out System.Windows.Forms.Cursor cursor)
        {
            cursor = System.Windows.Forms.Cursor.Current;
            return null;
            //throw new NotImplementedException();
        }

        public Guid Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { return "test"; }
        }

        public string Path
        {
            get { return "test"; }
        }

        #endregion
    }



}
#endif