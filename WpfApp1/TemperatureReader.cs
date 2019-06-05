/*
    Wiz610io/DS18B20 IoT temperature probe
    Copyright (C) 2019  Bob Dempsey

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;

namespace WpfApp1
{
    class TemperatureReader
    {
        private Panel panel;

        public TemperatureReader(Panel panel)
        {
            this.panel = panel;
        }

        public void StartReader()
        {
            var thdObj = new TemperatureReaderThreadClass(panel);
            var thd = new Thread(new ThreadStart(thdObj.Run))
            {
                IsBackground = true,
            };
            thd.SetApartmentState(ApartmentState.STA);
            thd.Start();
        }

    }
}
