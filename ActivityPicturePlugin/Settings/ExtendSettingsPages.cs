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

namespace ActivityPicturePlugin.Settings
{
    class ExtendSettingsPages : IExtendSettingsPages, IDisposable
    {
        SettingsPageControl m_Control = null;
        IList<ISettingsPage> _settingsPage = null;

        #region IExtendSettingsPages Members

        public IList<ISettingsPage> SettingsPages
        {
            get
            {
                if ( m_Control == null )
                {
                    m_Control = new SettingsPageControl();
                    _settingsPage = null;
                }
                if ( _settingsPage == null ) _settingsPage = new ISettingsPage[] { m_Control };
                return _settingsPage;

                //return new ISettingsPage[] {
                //    new SettingsPageControl()};
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }
        protected virtual void Dispose( bool disposing )
        {
            if ( ( disposing ) && ( m_Control != null ) )
            {
                m_Control.Dispose();
                m_Control = null;
            }
        }
        #endregion

    }
}