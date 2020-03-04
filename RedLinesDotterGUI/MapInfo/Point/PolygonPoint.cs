using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedLinesDotterGUI.MapInfo
{
    internal struct PolygonPoint
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public int PolygonNumber { get; private set; }

        public PolygonPoint(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
            PolygonNumber = 0;
        }

        public PolygonPoint(double X, double Y, int PolygonNumber)
        {
            this.X = X;
            this.Y = Y;
            this.PolygonNumber = PolygonNumber;
        }
    }
}
