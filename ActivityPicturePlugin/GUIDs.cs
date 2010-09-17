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

namespace ActivityPicturePlugin
{
	class GUIDs {
        public static readonly Guid PluginMain = new Guid("1a0840b9-1d83-4845-ada9-b0c0a6959f40");
//#if ST_2_1
//        public static readonly Guid MapControlLayer = new Guid("2aa470c3-0a3c-42c7-bbee-a43073072ee7");
//#else
//        public static readonly Guid TrailPointsControlLayerProvider = new Guid("2aa470c3-0a3c-42c7-bbee-a43073072ee7");
//#endif
        public static readonly Guid Settings = new Guid("c5d04e80-6108-11df-a08a-0800200c9a66");
        public static readonly Guid Activity = new Guid("0af379d0-5ebe-11df-a08a-0800200c9a66");
    }
}