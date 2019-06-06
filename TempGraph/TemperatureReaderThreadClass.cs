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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Threading;
using System.Net;
using Newtonsoft.Json;

namespace TempGraph
{
    class TemperatureReaderThreadClass
    {
        private bool Debug;

        private const int YScale = 4;

        private const int YOffset = 70;

        private Panel panel;

        private PointCollection pointCollection;

        private Point prevPoint;

        private Polyline polyLine;

        public TemperatureReaderThreadClass(Panel panel)
        {
            Debug = false;
            this.panel = panel;
            pointCollection = new PointCollection();
            polyLine = null;
        }

        public class TemperatureClass
        {
            public float Temperature { get; set; }

            public override string ToString()
            {
                return $"Temperature = {Temperature}";
            }
        }

        private int NextTemp()
        {
            if (Debug)
            {
                return 72;
            }
            else
            {
                var probeIP = Properties.Settings.Default.ProbeIpAddr;
                var probePort = Properties.Settings.Default.ProbeIpPort;
                try
                {
                    using (var webClient = new WebClient())
                    {
                        var result = webClient.DownloadString($"http://{probeIP}:{probePort}/");
                        var temp = JsonConvert.DeserializeObject<TemperatureClass>(result);
                        return (int)(temp.Temperature + 0.5);
                    }
                }
                catch (Exception)
                {
                    return 72;
                }
            }
        }

        private void InsertFirstTemp()
        {
            prevPoint = new Point()
            {
                X = 0,
                Y = panel.Height - 72 * YScale + YOffset,
            };
            pointCollection.Add(prevPoint);
            polyLine = new Polyline()
            {
                Points = pointCollection,
                Stroke = Brushes.DarkBlue,
                StrokeThickness = 1,
            };
            panel.Children.Add(polyLine);

        }

        private void ScrollGraph()
        {
            var newPointCollection = new PointCollection();
            foreach (var point in pointCollection)
            {
                if (point.X >= 60)
                {
                    var newPoint = new Point()
                    {
                        X = point.X - 60,
                        Y = point.Y,
                    };
                    newPointCollection.Add(newPoint);
                }
            }
            panel.Children.Remove(polyLine);
            pointCollection = newPointCollection;
            polyLine = new Polyline()
            {
                Points = pointCollection,
                Stroke = Brushes.DarkBlue,
                StrokeThickness = 1,
            };
            panel.Children.Add(polyLine);
        }

        private void InsertNextTemp()
        {
            var nextPoint = new Point()
            {
                X = prevPoint.X + 1,
                Y = panel.Height - NextTemp() * YScale + YOffset,
            };
            if (nextPoint.X >= panel.Width)
            {
                ScrollGraph();
                nextPoint.X = panel.Width - 60;
            }
            pointCollection.Add(nextPoint);
            prevPoint = nextPoint;

        }

        public void Run()
        {
            panel.Dispatcher.Invoke(InsertFirstTemp);

            for (; ; )
            {
                panel.Dispatcher.Invoke(InsertNextTemp);

                if (Debug)
                {
                    Thread.Sleep(25);
                }
                else
                {
                    Thread.Sleep(10 * 1000);
                }
            }

        }

    }
}
