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


namespace ActivityPicturePlugin.UI.Activities
{
    class ExtendActivityDetailPages : IExtendActivityDetailPages, IDisposable
    {
        #region IExtendActivityDetailPages Members

#if ST_2_1

        ActivityPicturePage m_Control = null;
        IList<IActivityDetailPage> _activityDetailPage = null;

        public IList<IActivityDetailPage> ActivityDetailPages
        {
            get
            {
                if ( m_Control == null )
                {
                    m_Control = new ActivityPicturePage();
                    _activityDetailPage = null;
                }
                if ( _activityDetailPage == null ) _activityDetailPage = new IActivityDetailPage[] { m_Control };
                return _activityDetailPage;
                //return new IActivityDetailPage[] { new ActivityPicturePage() };
            }
        }

        protected virtual void Dispose( bool disposing )
        {
            if ( ( disposing ) && ( m_Control != null ) )
            {
                m_Control.Dispose();
                m_Control = null;
            }
        }

#else
        public IList<IDetailPage> GetDetailPages(IDailyActivityView view, ExtendViewDetailPages.Location location)
        {
            return new IDetailPage[] { new ActivityPicturePage(view) };
        }
#endif

        public void Dispose()
        {
#if ST_2_1
            Dispose( true );
#endif
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
