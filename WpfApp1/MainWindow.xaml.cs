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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using Newtonsoft.Json;
using System.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void LayoutGraphLines()
        {
            var maxx = canGraph.Width;
            var maxy = canGraph.Height;
            var borderLine = new Line()
            {
                X1 = 0,
                Y1 = 0,
                X2 = 0,
                Y2 = maxy - 1,
                Stroke = Brushes.DarkRed,
                StrokeThickness = 0.5,
            };
            canGraph.Children.Add(borderLine);
            borderLine = new Line()
            {
                X1 = 0,
                Y1 = maxy - 1,
                X2 = maxx - 1,
                Y2 = maxy - 1,
                Stroke = Brushes.DarkRed,
                StrokeThickness = 0.5,
            };
            canGraph.Children.Add(borderLine);
            borderLine = new Line()
            {
                X1 = maxx - 1,
                Y1 = maxy - 1,
                X2 = maxx - 1,
                Y2 = 0,
                Stroke = Brushes.DarkRed,
                StrokeThickness = 0.5,
            };
            canGraph.Children.Add(borderLine);
            borderLine = new Line()
            {
                X1 = maxx - 1,
                Y1 = 0,
                X2 = 0,
                Y2 = 0,
                Stroke = Brushes.DarkRed,
                StrokeThickness = 0.5,
            };
            canGraph.Children.Add(borderLine);
            for (var x = 60; x < maxx; x += 60)
            {
                var hashLine = new Line()
                {
                    X1 = x,
                    Y1 = canGraph.Height - 0 - 1,
                    X2 = x,
                    Y2 = canGraph.Height - maxy - 1,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.25,
                };
                canGraph.Children.Add(hashLine);
            }
            for (var y = 60; y < maxy; y += 60)
            {
                var hashLine = new Line()
                {
                    X1 = 0,
                    Y1 = canGraph.Height - y - 1,
                    X2 = maxx - 1,
                    Y2 = canGraph.Height - y - 1,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.25,
                };
                canGraph.Children.Add(hashLine);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            LayoutGraphLines();
            var reader = new TemperatureReader(canGraph);
            reader.StartReader();
        }
    }
}
