using System.Collections.Generic;
using System.Windows;

namespace RedLinesDotterGUI.Views
{
    public enum FigureType { Polygon, Polyline }
    public sealed class Figure
    {
        public Dictionary<Point, int> Dots { get; private set; }
        public FigureType FigureType { get; private set; }

        public Figure(FigureType type)
        {
            Dots = new Dictionary<Point, int>();
            FigureType = type;
        }

        public void Add(Point point, int indexOfPolygon)
        {
            if (!Dots.ContainsKey(point))
                Dots.Add(point, indexOfPolygon);
        }
    }
}
